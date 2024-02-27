namespace Catalogify.Data.Entities;

public class Category
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
    
    public List<Item>? Items { get; set; }
    
    public Guid OwnerId { get; set; }
    
    public User? Owner { get; set; }
}