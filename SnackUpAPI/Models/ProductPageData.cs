using System.Collections.Generic;

namespace SnackUpAPI.Models
{
    public class ProductPageData
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string Details { get; set; }
        public string Raccomandation { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal? DiscountedPrice { get; set; } // Aggiunta
        public string StoreName { get; set; }
        public string StoreImage { get; set; }
        public string ProductImage { get; set; }
        public int? RemainingItems { get; set; }
        public List<string> Categories { get; set; } // Aggiunta
        public List<string> Allergens { get; set; } // Aggiunta
    }
}
