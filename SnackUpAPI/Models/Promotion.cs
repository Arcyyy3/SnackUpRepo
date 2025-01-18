using System;
using System.ComponentModel.DataAnnotations;


namespace SnackUpAPI.Models
{
    public class Promotion
    {
        public int PromotionID { get; set; }
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal DiscountPercentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? Created { get; set; } = DateTime.Now;
        public DateTime? Modified { get; set; }
        public DateTime? Deleted { get; set; }
    }
}
    