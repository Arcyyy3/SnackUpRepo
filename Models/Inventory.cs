using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SnackUpAPI.Models
{
    public class Inventory
    {
        [Key]
        [ForeignKey("Product")]
        public int ProductID { get; set; } // Legame con Products

        [Required]
        public int QuantityAvailable { get; set; } // Scorte disponibili

        public int QuantityReserved { get; set; } = 0; // Quantità riservata

        public int ReorderLevel { get; set; } = 10; // Soglia scorte basse

        [Required]
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow; // Ultima modifica

        public DateTime Created { get; set; } = DateTime.UtcNow; // Data di creazione
        public DateTime? Modified { get; set; } // Data di modifica
        public DateTime? Deleted { get; set; } // Data di eliminazione (soft delete)

        // Navigational Property
        public Product Product { get; set; }
    }
}
