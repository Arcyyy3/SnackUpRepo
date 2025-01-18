    using System.ComponentModel.DataAnnotations;
namespace SnackUpAPI.Models
{
    public class Subscription
    {
        public int SubscriptionID { get; set; }  // Identificativo univoco dell'abbonamento
        public int UserID { get; set; }  // Collegamento all'utente che ha sottoscritto l'abbonamento
        public int ProducerID { get; set; }  // Collegamento al fornitore associato
        public DateTime StartDate { get; set; }  // Data di inizio
        public DateTime EndDate { get; set; }  // Data di fine
        public string SubscriptionType { get; set; }  // Tipo di abbonamento (es. "Mensile", "Settimanale")
        public decimal TotalPrice { get; set; }  // Prezzo totale dell'abbonamento
        public DateTime? Created { get; set; } = DateTime.Now;
        public DateTime? Modified { get; set; } = DateTime.Now;
        public DateTime? Deleted { get; set; } = null;
    }
}
