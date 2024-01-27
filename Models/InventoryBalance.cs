using System.ComponentModel.DataAnnotations;

namespace PäronWebbApp.Models
{
    public class InventoryBalance
    {
        public int Id { get; set; }
        [Display(Name = "Lagersaldo")]
        public int TotalAmount { get; set; }
        public string ProductId { get; set; }
        public Product Product { get; set; }

        public int WarehouseId { get; set; }

        public Warehouse Warehouse { get; set; }
    }
}
