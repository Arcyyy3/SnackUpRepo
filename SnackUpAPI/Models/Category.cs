using System;
using System.ComponentModel.DataAnnotations;

namespace SnackUpAPI.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public DateTime? Created { get; set; } = DateTime.UtcNow;

        public DateTime? Modified { get; set; }

        public DateTime? Deleted { get; set; }
    }
}
