using System;
using System.ComponentModel.DataAnnotations;


namespace SnackUpAPI.Models
{
    public class Review
    {
        public int ReviewID { get; set; }  // Identificativo univoco della recensione
        public int ProductID { get; set; }  // Collegamento al prodotto recensito
        public int UserID { get; set; }  // Collegamento all'utente che ha lasciato la recensione
        public int Rating { get; set; }  // Valutazione (1-5 stelle)
        public string Comment { get; set; }  // Commento alla recensione
        public DateTime ReviewDate { get; set; }  // Data della recensione
        public DateTime? Created { get; set; } = DateTime.Now;
        public DateTime? Modified { get; set; } = DateTime.Now;
        public DateTime? Deleted { get; set; } = null;
    }
}
