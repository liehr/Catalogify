
using Microsoft.EntityFrameworkCore;
using DBFactory = Microsoft.EntityFrameworkCore.IDbContextFactory<Catalogify.Data.ApplicationDbContext>;
namespace Catalogify.Components.Pages.Item.View;


public class Data
{
    internal static async Task<Item?> GetItemAsync(Guid id, Guid inventoryId, DBFactory dbContextFactory)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        
        var item = await dbContext.Items
            .Where(e => e.Id == id && e.InventoryId == inventoryId)
            .FirstOrDefaultAsync();
        
        return item is null ? null : FromEntity(item);
    }
    
    private static Item FromEntity(Catalogify.Data.Entities.Item entity)
    {
        return new Item
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            Price = entity.Price,
            Quantity = entity.Quantity,
            InventoryId = entity.InventoryId,
            Inventory = entity.Inventory,
            Categories = entity.Categories
        };
    }
    
    public static Catalogify.Data.Entities.Item ToEntity(Item model)
    {
        return new Catalogify.Data.Entities.Item
        {
            Id = model.Id,
            Name = model.Name,
            Description = model.Description,
            Price = model.Price,
            Quantity = model.Quantity,
            InventoryId = model.InventoryId,
            Inventory = model.Inventory,
            Categories = model.Categories
        };
    }
    
    internal static async Task SaveItemAsync(Item model, DBFactory dbContextFactory)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        
        var entity = ToEntity(model);
        
        if (entity.Id == Guid.Empty)
        {
            entity.Id = Guid.NewGuid();
            dbContext.Items.Add(entity);
        }
        else
        {
            dbContext.Items.Update(entity);
        }
        
        await dbContext.SaveChangesAsync();
    }
}