using System;

namespace SnackUpClient.Models
{
    public class ShoppingSession
    {
        public int SessionID { get; set; } // ID della sessione
        public int? UserID { get; set; } // ID dell'utente (può essere null per sessioni anonime)
        public decimal TotalAmount { get; set; } // Totale della sessione
        public string Status { get; set; } // Stato della sessione (es. Active, Completed, Cancelled)
        public DateTime CreatedAt { get; set; } // Data di creazione
        public DateTime? UpdatedAt { get; set; } // Data di aggiornamento
    }
}
