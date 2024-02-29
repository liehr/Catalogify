using Catalogify.Data.Entities;

namespace Catalogify.Components.Pages.Inventory.View;

public class Inventory
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
    
    public int ItemCount { get; set; }
    
    public Guid OwnerId { get; set; }

    public List<Catalogify.Data.Entities.Inventory>? SubInventories { get; set; }
    
    public List<Item>? Items { get; set; }
}