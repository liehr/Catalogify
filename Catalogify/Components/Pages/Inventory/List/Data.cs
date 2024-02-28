using Microsoft.EntityFrameworkCore;
using DBFactory = Microsoft.EntityFrameworkCore.IDbContextFactory<Catalogify.Data.ApplicationDbContext>;

namespace Catalogify.Components.Pages.Inventory.List;
public static class Data
{
    internal static async Task<List<Pages.Inventory.List.Inventory>> GetInventoriesAsync(Guid userid, DBFactory dbContextFactory)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        
        var inventories = await dbContext.Inventories
            .Where(e => e.OwnerId == userid)
            .Include(e => e.Items)
            .ToListAsync();
        
        return inventories.Select(FromEntity).ToList();
    }
    
    internal static Pages.Inventory.List.Inventory FromEntity(Catalogify.Data.Entities.Inventory entity)
    {
        return new Pages.Inventory.List.Inventory
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            ItemCount = entity.ItemCount,
            OwnerId = entity.OwnerId,
            User = entity.User,
            SubInventories = entity.SubInventories,
            Items = entity.Items
        };
    }
}