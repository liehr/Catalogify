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
        };
    }
}