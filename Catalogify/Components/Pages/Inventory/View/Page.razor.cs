using BlazorBootstrap;
using Catalogify.Data.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using DBFactory = Microsoft.EntityFrameworkCore.IDbContextFactory<Catalogify.Data.ApplicationDbContext>;

namespace Catalogify.Components.Pages.Inventory.View;

public partial class Page
{
    private string _searchTerm = string.Empty;

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
    
    private List<Catalogify.Data.Entities.Item>? FilteredItems { get; set; }

    private string SearchTerm
    {
        get => _searchTerm;
        set
        {
            _searchTerm = value;
            
            FilteredItems = string.IsNullOrWhiteSpace(value) ? Inventory?.Items :
                // Otherwise, filter the items based on the search term
                Inventory?.Items?.Where(e => e.Name.Contains(value, StringComparison.OrdinalIgnoreCase)).ToList();
        }
    }

    protected override async Task OnInitializedAsync()
    {
        
        if (AuthenticationStateProvider is null || DbFactory is null || NavigationManager is null)
        {
            ToastService?.Notify(new ToastMessage
            {
                Title = "Error",
                Type = ToastType.Danger,
                Message = "Your inventories could not be loaded. Something on our end went wrong.",
                HelpText = "Try again later!",
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
                Message = "You do not have access to this inventory. It may have been deleted or you may not have permission to view it. An Email has been sent to the owner.",
                HelpText = "Not Authorized!",
                IconName = IconName.ExclamationTriangleFill,
                AutoHide = true
            });
            
            NavigationManager?.NavigateTo("/Inventory/List");
            return;
        }
        
        FilteredItems = Inventory.Items;
    }

    private async Task OnCreateItem()
    {
        var item = new Catalogify.Data.Entities.Item
        {
            Name = "New Item",
            Description = "New Item Description",
            InventoryId = Id,
            Inventory = Data.ToEntity(Inventory!),
            Quantity = 1,
            Price = 1.0m
        };
        
        if (Inventory!.Items is null)
        {
            Inventory.Items = new List<Catalogify.Data.Entities.Item>();
        }
        
        Inventory.Items.Add(item);
        
        await Data.SaveInventoryAsync(Inventory, DbFactory!);
        
        StateHasChanged();
    }
}