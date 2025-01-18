using System;
using System.ComponentModel.DataAnnotations;


namespace SnackUpAPI.Models
{
    public class Product
    {
        public int ProductID { get; set; }
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Details { get; set; }
        public string Raccomandation { get; set; }
        public decimal Price { get; set; }
        public int? ProducerID { get; set; }
        public string? PhotoLink { get; set; }
        public DateTime? Created { get; set; } = DateTime.Now;
        public DateTime? Modified { get; set; }
        public DateTime? Deleted { get; set; }
    }
}
