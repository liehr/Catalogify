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
    
    private List<Item>? FilteredItems { get; set; }

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
            return;
        }
        
        FilteredItems = Inventory.Items;
    }

    private async Task OnCreateItem()
    {
        if (DbFactory is null || NavigationManager is null)
        {
            ToastService?.Notify(new ToastMessage
            {
                Title = "Error",
                Type = ToastType.Danger,
                Message = "The item could not be created.",
                HelpText = "Please contact the administrator.",
                IconName = IconName.ExclamationTriangleFill,
                AutoHide = true
            });
            
            return;
        }
        
        var item = new Item
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
            Inventory.Items = new List<Item>();
        }
        
        Inventory.Items.Add(item);
        
        await Data.SaveInventoryAsync(Inventory, DbFactory);
        
        StateHasChanged();
    }
}