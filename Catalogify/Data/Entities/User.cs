namespace Catalogify.Data.Entities;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public string Firstname { get; set; } = string.Empty;
    
    public string Lastname { get; set; } = string.Empty;
    public string Salt { get; set; } = string.Empty;

    public int Credits { get; set; } = 1000;
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime ModifiedAt { get; set; }

    public string Role { get; set; } = "User";
    
    public List<Inventory>? Inventories { get; set; }
}