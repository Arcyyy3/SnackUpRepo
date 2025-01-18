using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnackUpClient.Models
{
    public class Order
    {
        public int OrderID { get; set; }
        public int UserID { get; set; }
        public int ProducerID { get; set; }
        public int SchoolClassID { get; set; }
        public DateTime OrderDate { get; set; }
        public string Recreation { get; set; } // "First" o "Second"
        public string Status { get; set; } // "Pending", "Active", ecc.
        public decimal TotalPrice { get; set; }
        public List<Prodotto> Products { get; set; }
        public string CancellationReason { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }
        public DateTime? Deleted { get; set; }

        // Metodo per tradurre Recreation in formato leggibile
        public string GetReadableRecreation()
        {
            return Recreation switch
            {
                "First" => "Prima Ricreazione",
                "Second" => "Seconda Ricreazione",
                _ => "Altro"
            };
        }
    }
}
