using System;

namespace SnackUpAPI.Models
{
    public class AddToCartRequest
    {
        public int SessionID { get; set; } // ID della sessione attiva
        public int ProductID { get; set; } // ID del prodotto
        public int Quantity { get; set; }  // Quantità del prodotto
        public decimal Price { get; set; } // Prezzo unitario del prodotto
    }
}
