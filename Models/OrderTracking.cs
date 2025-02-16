using System;
using System.ComponentModel.DataAnnotations;


namespace SnackUpAPI.Models
{
    public class OrderTracking
    {
        public int TrackingID { get; set; }
        public int OrderID { get; set; }
        [Required]
        [MaxLength(50)]
        public string Status { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.Now;
        public DateTime? Created { get; set; } = DateTime.Now;
        public DateTime? Modified { get; set; }
        public DateTime? Deleted { get; set; }
    }
}
