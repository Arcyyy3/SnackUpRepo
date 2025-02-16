using System;
using System.ComponentModel.DataAnnotations;

namespace SnackUpAPI.Models
{
    public class BundleProduct
    {
        public int BundleID { get; set; } // ID del bundle

        [Required]
        [MaxLength(255)]
        public string BundleName { get; set; } // Nome del bundle

        public string Description { get; set; } // Descrizione del bundle

        [Required]
        public decimal Price { get; set; } // Prezzo del bundle

        [Required]
        [MaxLength(50)]
        public string Moment { get; set; } // Momento (First/Second)

        public DateTime? Created { get; set; } = DateTime.Now;
        public DateTime? Modified { get; set; }
        public DateTime? Deleted { get; set; }
    }
}
