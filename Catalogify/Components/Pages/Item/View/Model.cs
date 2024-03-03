using Catalogify.Data.Entities;

namespace Catalogify.Components.Pages.Item.View;

public class Item
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
    
    public decimal? Price { get; set; }
    
    public int Quantity { get; set; }
    
    public Guid InventoryId { get; set; }
    
    public required Catalogify.Data.Entities.Inventory Inventory { get; set; }
    
    public List<Category>? Categories { get; set; }
}