﻿using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using DBFactory = Microsoft.EntityFrameworkCore.IDbContextFactory<Catalogify.Data.ApplicationDbContext>;

namespace Catalogify.Components.Pages.Inventory.List;

public partial class Page
{
    [Inject]
    private AuthenticationStateProvider? AuthenticationStateProvider { get; set; }
    
    [Inject]
    private DBFactory? DbFactory { get; set; }
    
    [Inject]
    private ToastService? ToastService { get; set; }
    
    private List<Inventory>? _inventories;

    private string? _userName;
    
    protected override async Task OnInitializedAsync()
    {
        await GetInventories();
    }
    
    private async Task GetInventories()
    {
        if (AuthenticationStateProvider is null || DbFactory is null)
        {
            ToastService?.Notify(new ToastMessage
            {
                Title = "Fehler",
                Type = ToastType.Danger,
                Message = "Die Inventare konnten nicht geladen werden.",
                HelpText = "Kontaktieren Sie den Administrator.",
                IconName = IconName.ExclamationTriangleFill,
                AutoHide = true
            });
            return;
        }
        
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var idClaim = authState.User.Claims.FirstOrDefault(e => e.Type == "Id");
        _userName = authState.User.Identity?.Name;
        _inventories = await Data.GetInventoriesAsync(Guid.Parse(idClaim!.Value), DbFactory);
    }
}