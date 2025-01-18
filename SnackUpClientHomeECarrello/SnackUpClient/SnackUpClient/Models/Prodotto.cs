using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnackUpClient.Models
{

    public class Prodotto
    {
        public string Nome { get; set; }
        public int Quantità { get; set; }
        public decimal Prezzo { get; set; }
        public string QuantitàFormattata => $"x{Quantità}";
        public string Img {  get; set; }
    }

}
