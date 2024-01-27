using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PäronWebbApp.Models
{
    public class Transaction
    {

        public int Id { get; set; }
        
        public int Quantity { get; set; }
        public DateTimeOffset TransactionDate { get; set; }

        public string ProductId { get; set; }
        public Product Product { get; set; }

        public int WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }


    }
}
