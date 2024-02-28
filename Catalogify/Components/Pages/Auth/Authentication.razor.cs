using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;

namespace Catalogify.Components.Pages.Auth;

public partial class Authentication
{
    private bool LoginFailed { get; set; }
    
    private bool RegisterFailed { get; set; }

    [Inject]
    private NavigationManager? NavigationManager { get; set; }
    
    protected override void OnInitialized()
    {
        var uri = NavigationManager!.ToAbsoluteUri(NavigationManager.Uri);
        LoginFailed = QueryHelpers.ParseQuery(uri.Query).TryGetValue("loginFailed", out var _);
        RegisterFailed = QueryHelpers.ParseQuery(uri.Query).TryGetValue("registerFailed", out var _);
        
        base.OnInitialized();
    }
}