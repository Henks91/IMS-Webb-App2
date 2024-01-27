using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PäronWebbApp.Models
{
    public class Warehouse
    {
        
        public int WarehouseId { get; set; }
        [Required]
        [Display(Name = "Stad")]
        public string City { get; set; }

        public Warehouse(string city)
        {
            
            City = city;
        }
    }
}
