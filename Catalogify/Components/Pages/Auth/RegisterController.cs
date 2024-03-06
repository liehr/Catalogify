using System.Text.RegularExpressions;
using Catalogify.Data.Transfer;
using Microsoft.AspNetCore.Mvc;
using IAuthenticationService = Catalogify.Services.Interface.IAuthenticationService;

namespace Catalogify.Components.Pages.Auth;

public partial class RegisterController(IAuthenticationService authenticationService) : Controller
{
    [HttpPost("/authentication/register")]
    public async Task<IActionResult> RegisterAsync([FromForm] string registeremail, [FromForm] string registerpassword)
    {
        var data = await authenticationService.GetUserByEmailAsync(registeremail);
        
        // Password validation
        if (string.IsNullOrWhiteSpace(registeremail) || string.IsNullOrWhiteSpace(registerpassword) || data is not null)
        {
            return Redirect("/?registerFailed");
        }
        
        if (!MyRegex().IsMatch(registerpassword))
        {
            return Redirect("/?registerFailed");
        }
        
        await authenticationService.RegisterAsync(new CreateUser
        {
            Email = registeremail,
            Password = registerpassword
        });

        return Redirect("/" + "?registerSuccess");
    }

    [GeneratedRegex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,20}$")]
    private static partial Regex MyRegex();
}