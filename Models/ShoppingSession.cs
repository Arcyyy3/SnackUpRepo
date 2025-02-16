using System;

namespace SnackUpAPI.Models
{
    public class ShoppingSession
    {
        public int SessionID { get; set; } // Chiave primaria
        public int? UserID { get; set; } // Collegamento all'utente (nullable per utenti anonimi)
        public string Status { get; set; } = "Active"; // Stato della sessione (Active, Completed, Cancelled)
        public decimal TotalAmount { get; set; } = 0.00m; // Totale della sessione
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Data di creazione
        public DateTime? UpdatedAt { get; set; } // Data di aggiornamento
    }
}
