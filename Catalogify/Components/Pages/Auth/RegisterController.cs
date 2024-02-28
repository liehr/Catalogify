using Catalogify.Data.Transfer;
using Microsoft.AspNetCore.Mvc;
using IAuthenticationService = Catalogify.Services.Interface.IAuthenticationService;

namespace Catalogify.Components.Pages.Auth;

public class RegisterController(IAuthenticationService authenticationService) : Controller
{
    [HttpPost("/authentication/register")]
    public async Task<IActionResult> RegisterAsync([FromForm] string registeremail, [FromForm] string registerpassword)
    {
        var data = await authenticationService.GetUserByEmailAsync(registeremail);
        
        if (string.IsNullOrWhiteSpace(registeremail) || string.IsNullOrWhiteSpace(registerpassword) || data is not null)
        {
            return Redirect("/" + "?registerFailed");
        }
        
        await authenticationService.RegisterAsync(new CreateUser
        {
            Email = registeremail,
            Password = registerpassword
        });

        return Redirect("/" + "?registerSuccess");
    }
}