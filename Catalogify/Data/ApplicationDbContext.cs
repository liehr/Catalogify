using Catalogify.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Catalogify.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    
    public DbSet<Item> Items { get; set; }
    
    public DbSet<Inventory> Inventories { get; set; }
    
    public DbSet<Category> Categories { get; set; }
}