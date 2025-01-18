using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnackUpClient.Models
{

    public class OrdineHistory
    {
        public string Giorno { get; set; }
        public string Orario { get; set; } // Indica la ricreazione
        public List<Prodotto> Prodotti { get; set; }
        public decimal PrezzoTotale => Prodotti.Sum(p => p.Prezzo * p.Quantità);
        public string GetReadableRecreation()
        {
            return Orario switch
            {
                "First" => "Prima Ricreazione",
                "Second" => "Seconda Ricreazione",
                _ => "Altro"
            };
        }
    }
}
