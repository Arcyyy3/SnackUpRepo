// Models/ProductPageData.cs
namespace SnackUpAPI.Models
{
    public class ProductPageData
    {
        public int ProductID { get; set; }
        public string ProducerName { get; set; }
        public string ProductName { get; set; }
        public int QuantityAvailable { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public string Description { get; set; }
        public string Details { get; set; }
        public string Raccomandation { get; set; }
        public string PhotoLinkProdotto { get; set; }    // Categorie separate da virgola
        public string PhotoLinkProduttore { get; set; }    // Categorie separate da virgola
        public string Categories { get; set; }    // Categorie separate da virgola
        public string Allergens { get; set; }     // Allergeni separati da virgola
        public decimal? DiscountedPrice { get; set; }
    }
}
