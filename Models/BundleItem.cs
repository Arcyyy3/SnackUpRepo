using System;
using System.ComponentModel.DataAnnotations;

namespace SnackUpAPI.Models
{
    public class BundleItem
    {
        public int BundleItemID { get; set; } // ID univoco dell'elemento del bundle

        [Required]
        public int BundleID { get; set; } // ID del bundle associato

        [Required]
        public int ProductID { get; set; } // ID del prodotto nel bundle

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; } = 1; // Quantità del prodotto nel bundle

        public DateTime? Created { get; set; } = DateTime.Now;
        public DateTime? Modified { get; set; }
        public DateTime? Deleted { get; set; }
    }
}
