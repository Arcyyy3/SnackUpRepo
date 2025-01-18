using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnackUpClient.Models
{
    public class Ordine
    {
        public string Giorno { get; set; }
        public string Orario { get; set; }
        public List<Prodotto> Prodotti { get; set; }
        public decimal PrezzoTotale => Prodotti.Sum(p => p.Prezzo * p.Quantità);

        // Aggiungi la proprietà Recreation
        public string Recreation { get; set; }

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
