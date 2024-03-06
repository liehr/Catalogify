using Microsoft.EntityFrameworkCore;
using DBFactory = Microsoft.EntityFrameworkCore.IDbContextFactory<Catalogify.Data.ApplicationDbContext>;

namespace Catalogify.Components.Pages.Inventory.View;

public class Data
{
    internal static async Task<Inventory?> GetInventoryAsync(Guid id, Guid userId, DBFactory dbContextFactory)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        
        var inventory = await dbContext.Inventories
            .Where(e => e.Id == id && e.OwnerId == userId)
            .Include(e => e.Items)
            .FirstOrDefaultAsync();
        
        return inventory is null ? null : FromEntity(inventory);
    }
    
    private static Inventory FromEntity(Catalogify.Data.Entities.Inventory entity)
    {
        return new Inventory
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            ItemCount = entity.ItemCount,
            OwnerId = entity.OwnerId,
            SubInventories = entity.SubInventories,
            Items = entity.Items,
            Metadata = entity.Metadata,
            CreatedAt = entity.CreatedAt,
            LastModifiedAt = entity.LastModifiedAt,
            Categories = entity.Categories
        };
    }
    
    public static Catalogify.Data.Entities.Inventory ToEntity(Inventory model)
    {
        return new Catalogify.Data.Entities.Inventory
        {
            Id = model.Id,
            Name = model.Name,
            Description = model.Description,
            ItemCount = model.ItemCount,
            OwnerId = model.OwnerId,
            SubInventories = model.SubInventories,
            Items = model.Items,
            Metadata = model.Metadata,
            CreatedAt = model.CreatedAt,
            LastModifiedAt = model.LastModifiedAt,
            Categories = model.Categories
        };
    }
    
    internal static async Task SaveInventoryAsync(Inventory model, DBFactory dbContextFactory)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        
        var entity = ToEntity(model);
        
        if (entity.Id == Guid.Empty)
        {
            entity.Id = Guid.NewGuid();
            entity.CreatedAt = DateTime.UtcNow;
            entity.LastModifiedAt = DateTime.UtcNow;
            dbContext.Inventories.Add(entity);
        }
        else
        {
            entity.LastModifiedAt = DateTime.UtcNow;
            dbContext.Inventories.Update(entity);
        }
        
        await dbContext.SaveChangesAsync();
    }
}