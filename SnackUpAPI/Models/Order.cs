using System;
using System.ComponentModel.DataAnnotations;

namespace SnackUpAPI.Models
{
    public class Order
    {
        public int OrderID { get; set; }
        public int UserID { get; set; }
        public int ProducerID { get; set; }
        public int SchoolClassID { get; set; } // Nuovo campo aggiunto
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public string recreation { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; }
        public decimal TotalPrice { get; set; }
        public string? CancellationReason { get; set; }
        public DateTime? Created { get; set; } = DateTime.Now;
        public DateTime? Modified { get; set; }
        public DateTime? Deleted { get; set; }
    }
}
