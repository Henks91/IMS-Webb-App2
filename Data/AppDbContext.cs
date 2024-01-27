using Microsoft.EntityFrameworkCore;
using PäronWebbApp.Models;

namespace PäronWebbApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Product> Products { get; set; } = default!;
        public DbSet<Warehouse> Warehouses { get; set; } = default!;
        public DbSet<Transaction> Transactions { get; set; } = default!;
        public DbSet<InventoryBalance> inventoryBalances { get; set; } = default!;



    }
}
