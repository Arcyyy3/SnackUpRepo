using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SnackUpAPI.Models
{
    public class InventoryHistory
    {
        [Key]
        public int HistoryID { get; set; } // Chiave primaria autonumerata

        [Required]
        [ForeignKey("Product")]
        public int ProductID { get; set; } // Legame con Products

        [Required]
        public int ChangeAmount { get; set; } // Quantità aggiunta o rimossa

        [Required]
        [MaxLength(255)]
        public string ChangeReason { get; set; } // Motivo del cambiamento

        [Required]
        [MaxLength(100)]
        public string ChangedBy { get; set; } // Utente che ha effettuato il cambiamento

        public DateTime Created { get; set; } = DateTime.UtcNow; // Data di creazione
        public DateTime? Modified { get; set; } // Data di modifica
        public DateTime? Deleted { get; set; } // Data di eliminazione (soft delete)
        public DateTime ChangeDate { get; set; } = DateTime.UtcNow; // Data del cambiamento

        // Navigational Property
        public Product Product { get; set; }
    }
}
