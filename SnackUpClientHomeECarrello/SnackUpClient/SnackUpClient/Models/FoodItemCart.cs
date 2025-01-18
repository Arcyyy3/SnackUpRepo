using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnackUpClient.Models
{
    public class FoodItemCart
    {
        public int ProductID { get; set; } // ID prodotto
        public string Image { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public bool IsLowStock { get; set; }
    }
}
