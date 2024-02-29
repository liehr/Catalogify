using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Routing;

namespace Catalogify.Components.Layout;

public partial class NavMenu
{
    [Inject]
    private AuthenticationStateProvider? AuthenticationStateProvider { get; set; }
    
    private Sidebar? _sidebar;
    private IEnumerable<NavItem>? _navItems;
    private string? _currentUrl;

    private async Task<SidebarDataProviderResult> SidebarDataProviderAsync(SidebarDataProviderRequest request)
    {
        if (_navItems is null)
            _navItems = await GetNavItemsAsync();

        return request.ApplyTo(_navItems);
    }

    private async Task<IEnumerable<NavItem>> GetNavItemsAsync()
    {
        var authState = await AuthenticationStateProvider!.GetAuthenticationStateAsync();
        //Global Nav Items
        var items = new List<NavItem>
        {
            new() {Href = "/", IconName = IconName.House, Sequence = 0,Text = "Home", Match = NavLinkMatch.All},
            new() {Href = "Inventory/List", IconName = IconName.Journals, Sequence = 1, Text = "Inventory"},
            new() {Href = "Account/Profile", IconName = IconName.Person, Sequence = 900, Text = $"{authState.User.Identity?.Name}"},
            new() {Href = "authentication/logout", IconName = IconName.BoxArrowInLeft, Sequence = 999, Text = "Logout"},
        };

        return items;
    }

    protected override void OnInitialized()
    {
        _currentUrl = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
        NavigationManager.LocationChanged += OnLocationChanged;
    }

    private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
    {
        _currentUrl = NavigationManager.ToBaseRelativePath(e.Location);
        StateHasChanged();
    }

    public void Dispose()
    {
        NavigationManager.LocationChanged -= OnLocationChanged;
    }
}