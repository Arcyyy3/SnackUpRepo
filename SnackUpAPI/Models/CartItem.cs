using System;

namespace SnackUpAPI.Models
{
    public class CartItem
    {
        public int CartItemID { get; set; } // Chiave primaria
        public int SessionID { get; set; } // Collegamento alla sessione
        public int ProductID { get; set; } // Collegamento al prodotto
        public int Quantity { get; set; } = 1; // Quantità del prodotto
        public decimal Price { get; set; } // Prezzo unitario del prodotto
        public decimal Total { get; set; } // Totale per questo elemento (Price * Quantity)
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Data di creazione
        public DateTime? UpdatedAt { get; set; } // Data di aggiornamento
    }
}
