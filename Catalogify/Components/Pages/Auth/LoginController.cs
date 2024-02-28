using System.Security.Claims;
using Catalogify.Data.Transfer;
using Catalogify.Services.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using IAuthenticationService = Catalogify.Services.Interface.IAuthenticationService;


namespace Catalogify.Components.Pages.Auth;

public class LoginController(IAuthenticationService authenticationService) : Controller
{
    [HttpPost("/authentication/login")]
    public async Task<IActionResult> CookieLoginAsync([FromForm] string email, [FromForm] string password)
    {
        var data = await authenticationService.LoginAsync(new LoginUser { Email = email, Password = password });

        var claims = new List<Claim>();

        claims.Add(new Claim(ClaimTypes.Name, $"{data.Firstname} {data.Lastname}"));
        claims.Add(new Claim("Id", data.Id.ToString()));

        if (!string.IsNullOrWhiteSpace(data.Role))
            claims.Add(new Claim(ClaimTypes.Role, data.Role));

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(principal);

        return Redirect("/");
    }
    
    [HttpGet("/authentication/logout")]
    public async Task<IActionResult> CookieLogoutAsync()
    {
        await HttpContext.SignOutAsync();
        return Redirect("/");
    }
}