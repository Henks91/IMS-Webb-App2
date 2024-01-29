using System.ComponentModel.DataAnnotations;

namespace PäronWebbApp.Models
{
    public class InventoryBalance
    {
        

        public int Id { get; set; }
        public string ProductId { get; set; }

        [Display(Name = "Produkt")]
        public Product? Product { get; set; }

        public int WarehouseId { get; set; }

        [Display(Name = "Stad")]
        public Warehouse? Warehouse { get; set; }

        [Display(Name = "Lagersaldo")]
        public int TotalAmount { get; set; }
    }
}
