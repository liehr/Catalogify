using BlazorBootstrap;
using Microsoft.AspNetCore.Components.Routing;

namespace Catalogify.Components.Layout;

public partial class NavMenu
{
    private Sidebar? _sidebar;
    private IEnumerable<NavItem>? _navItems;
    private string? _currentUrl;

    private async Task<SidebarDataProviderResult> SidebarDataProviderAsync(SidebarDataProviderRequest request)
    {
        _navItems ??= await GetNavItemsAsync();

        return request.ApplyTo(_navItems);
    }

    private static Task<IEnumerable<NavItem>> GetNavItemsAsync()
    {
        var items = new List<NavItem>
        {
            new() {Href = "/", IconName = IconName.House, Sequence = 0,Text = "Home", Match = NavLinkMatch.All},
            new() {Href = "/Counter", IconName = IconName.Plus, Sequence = 1, Text = "Counter", Match = NavLinkMatch.Prefix},
            new() {Href = "/FetchData", IconName = IconName.Table, Sequence = 2, Text = "Fetch data", Match = NavLinkMatch.Prefix},
        };

        return Task.FromResult<IEnumerable<NavItem>>(items);
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