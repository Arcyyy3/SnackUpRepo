using System;
using System.ComponentModel.DataAnnotations;

namespace SnackUpAPI.Models
{
    public class SupportRequest
    {
        public int SupportRequestID { get; set; }
        public int UserID { get; set; }
        public int? OrderID { get; set; }
        [Required]
        [MaxLength(255)]
        public string Subject { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [MaxLength(50)]
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime LastUpdatedDate { get; set; } = DateTime.Now;
        public DateTime? Created { get; set; } = DateTime.Now;
        public DateTime? Modified { get; set; }
        public DateTime? Deleted { get; set; }
    }
}
