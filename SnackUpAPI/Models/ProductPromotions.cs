using System;
using System.ComponentModel.DataAnnotations;


namespace SnackUpAPI.Models
{
    public class ProductPromotion
    {
        public int ProductPromotionID { get; set; } // Chiave primaria
        public int ProductID { get; set; } // Collegamento al prodotto
        public int PromotionID { get; set; } // Collegamento alla promozione
        public DateTime? Created { get; set; } = DateTime.Now; // Data di creazione
        public DateTime? Modified { get; set; } = DateTime.Now; // Data di modifica
        public DateTime? Deleted { get; set; } = null; // Per cancellazione logica

        // Navigational Properties (opzionali, utili se stai usando un ORM come Entity Framework)
        public Product Product { get; set; }
        public Promotion Promotion { get; set; }
    }
}
