using System;
using System.ComponentModel.DataAnnotations;


namespace SnackUpAPI.Models
{
    public class OrderDetail
    {
        public int DetailID { get; set; }
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string Recreation { get; set; }
        public string ProductCode { get; set; }
        public DateTime? Created { get; set; } = DateTime.Now;
        public DateTime? Modified { get; set; }
        public DateTime? Deleted { get; set; }
    }
} 
