using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PäronWebbApp.Models
{
    public class Product
    {
        
        [Required]
        public string ProductId { get; set; }
        [Required]
        [Display(Name = "Produkt")]
        public string ProductName { get; set; }
        [Required]
        [Display(Name = "Pris")]
        public decimal Price { get; set; }

        [Display(Name = "Skapad")]
        public DateTimeOffset Created {  get; set; }

        public Product(string productId, string productName, decimal price, DateTimeOffset created)
        {
            ProductId = productId;
            ProductName = productName;
            Price = price;
            Created = created;
        }

    }
}
