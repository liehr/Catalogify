namespace Catalogify.Data.Transfer;

public class CreateUser
{
    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}