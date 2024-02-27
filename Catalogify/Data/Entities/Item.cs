namespace Catalogify.Data.Entities;

public class Item
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
    
    public decimal? Price { get; set; }
    
    public int Quantity { get; set; }
    
    public Guid InventoryId { get; set; }
    
    public required Inventory Inventory { get; set; }
    
    public List<Category>? Categories { get; set; }
}