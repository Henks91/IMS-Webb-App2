﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PäronWebbApp.Models
{
    public class Transaction
    {

        public int Id { get; set; }
        [Display(Name = "Antal")]
        public int Quantity { get; set; }
        [Display(Name = "Datum")]
        public DateTimeOffset TransactionDate { get; set; }

        public string ProductId { get; set; }
        public Product Product { get; set; }

        public int WarehouseId { get; set; }
        [Display(Name = "Stad")]
        public Warehouse Warehouse { get; set; }


    }
}
