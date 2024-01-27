using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PäronWebbApp.Models
{
    public class Warehouse
    {
        

        public int WarehouseId { get; set; }
        [Required]
        public string City { get; set; }

        public Warehouse(int warehouseId, string city)
        {
            WarehouseId = warehouseId;
            City = city;
        }
    }
}
