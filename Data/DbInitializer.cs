using PäronWebbApp.Models;

namespace PäronWebbApp.Data

{
    public class DbInitializer
    {
        public static void Seed(AppDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            //Lägger till produkter
            context.Products.Add(new Product("P001", "jTelefon", 8900, DateTime.UtcNow));
            context.Products.Add(new Product("P002", "jPlatta", 5700, DateTime.UtcNow));
            context.Products.Add(new Product("P003", "Päronklocka", 11000, DateTime.UtcNow));

            //Lägger till varulager
            context.Warehouses.Add(new Warehouse("Cupertino"));
            context.Warehouses.Add(new Warehouse("Norrköping"));
            context.Warehouses.Add(new Warehouse("Frankurt"));

            
            context.SaveChanges();

        }
    }
}
