using Catalogify.Data.Transfer;
using Microsoft.AspNetCore.Mvc;
using IAuthenticationService = Catalogify.Services.Interface.IAuthenticationService;

namespace Catalogify.Components.Pages.Auth;

public class RegisterController(IAuthenticationService authenticationService) : Controller
{
    [HttpPost("/authentication/register")]
    public async Task<IActionResult> RegisterAsync([FromForm] string email, [FromForm] string password)
    {
        var user = await authenticationService.RegisterAsync(new CreateUser
        {
            Email = email,
            Password = password
        });

        return Redirect("/");
    }
}