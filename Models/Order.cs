using System;
using System.ComponentModel.DataAnnotations;

namespace SnackUpAPI.Models
{
    public class Order
    {
        public int OrderID { get; set; }
        public int UserID { get; set; }
        public int SchoolClassID { get; set; } // Nuovo campo aggiunto
        public string Status { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime? Created { get; set; } = DateTime.Now;
        public DateTime? Modified { get; set; }
        public DateTime? Deleted { get; set; }
    }
}
