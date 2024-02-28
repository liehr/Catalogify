using Catalogify.Data.Transfer;
using Microsoft.AspNetCore.Mvc;
using IAuthenticationService = Catalogify.Services.Interface.IAuthenticationService;

namespace Catalogify.Components.Pages.Auth;

public class RegisterController(IAuthenticationService authenticationService) : Controller
{
    [HttpPost("/authentication/register")]
    public async Task<IActionResult> RegisterAsync([FromForm] string registeremail, [FromForm] string registerpassword)
    {
        if (string.IsNullOrWhiteSpace(registeremail) || string.IsNullOrWhiteSpace(registerpassword))
        {
            return Redirect("/" + "?registerFailed");
        }
        
        var user = await authenticationService.RegisterAsync(new CreateUser
        {
            Email = registeremail,
            Password = registerpassword
        });

        return Redirect("/");
    }
}