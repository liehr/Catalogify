using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using DBFactory = Microsoft.EntityFrameworkCore.IDbContextFactory<Catalogify.Data.ApplicationDbContext>;

namespace Catalogify.Components.Pages.Inventory.View;

public partial class Page
{
    [Parameter]
    public Guid Id { get; set; }
    
    [Inject]
    private AuthenticationStateProvider? AuthenticationStateProvider { get; set; }
    
    [Inject]
    private DBFactory? DbFactory { get; set; }
    
    [Inject]
    private ToastService? ToastService { get; set; }
    
    [Inject]
    private NavigationManager? NavigationManager { get; set; }
    private Inventory? Inventory { get; set; }
    
    protected override async Task OnInitializedAsync()
    {
        
        if (AuthenticationStateProvider is null || DbFactory is null || NavigationManager is null)
        {
            ToastService?.Notify(new ToastMessage
            {
                Title = "Error",
                Type = ToastType.Danger,
                Message = "The inventory could not be loaded.",
                HelpText = "Please contact the administrator.",
                IconName = IconName.ExclamationTriangleFill,
                AutoHide = true
            });
            
            NavigationManager?.NavigateTo("/Inventory/List");
            return;
        }
        
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var idClaim = authState.User.Claims.FirstOrDefault(e => e.Type == "Id");
        
        Inventory = await Data.GetInventoryAsync(Id, Guid.Parse(idClaim!.Value), DbFactory);
        
        if (Inventory is null)
        {
            ToastService?.Notify(new ToastMessage
            {
                Title = "Error",
                Type = ToastType.Danger,
                Message = "The inventory could not be loaded.",
                HelpText = "You are not authorized to view this inventory.",
                IconName = IconName.ExclamationTriangleFill,
                AutoHide = true
            });
            
            NavigationManager?.NavigateTo("/Inventory/List");
        }
    }
}