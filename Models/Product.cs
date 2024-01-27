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
        public string ProductName { get; set; }
        [Required]
        public decimal Price { get; set; }

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
