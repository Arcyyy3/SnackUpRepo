using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SnackUpAPI.Models
{
    public class CategoryProduct
    {
        [Key, Column(Order = 0)]
        public int ProductID { get; set; }

        [Key, Column(Order = 1)]
        public int CategoryID { get; set; }

        public DateTime? Created { get; set; } = DateTime.UtcNow;

        public DateTime? Modified { get; set; }

        public DateTime? Deleted { get; set; }

        // Relazioni (Navigational Properties)
        public Product Product { get; set; }
        public Category Category { get; set; }
    }
}
