using System.Security.Claims;
using BlazorBootstrap;
using Catalogify.Data.Transfer;
using Catalogify.Services.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using IAuthenticationService = Catalogify.Services.Interface.IAuthenticationService;


namespace Catalogify.Components.Pages.Auth;

public class LoginController(IAuthenticationService authenticationService, ToastService toastService) : Controller
{
    [HttpPost("/authentication/login")]
    public async Task<IActionResult> LoginAsync([FromForm] string email, [FromForm] string password, [FromForm] bool rememberMe) 
    {
        var user = await authenticationService.LoginAsync(new LoginUser
        {
            Email = email,
            Password = password
        });

        if (user is null)
        {
            return Redirect("/" + "?loginFailed");
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Email),
            new(ClaimTypes.Role, user.Role),
            new("Id", user.Id.ToString())
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties
        {
            IsPersistent = rememberMe,
            ExpiresUtc = DateTimeOffset.UtcNow.AddMonths(6)
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);

        return Redirect("/");
    }
    
    [HttpGet("/authentication/logout")]
    public async Task<IActionResult> CookieLogoutAsync()
    {
        await HttpContext.SignOutAsync();
        
        return Redirect("/");
    }
}