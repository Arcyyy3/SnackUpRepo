namespace SnackUpClient.Models
{
    public class FoodItem
    {
        public string Image { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsLowStock { get; set; }

        // Costruttore vuoto (necessario per il binding o framework che lo usa)
        public FoodItem() { }

        // Costruttore con parametri
        public FoodItem(string title, string image, string description, bool isLowStock)
        {
            Title = title;
            Image = image;
            Description = description;
            IsLowStock = isLowStock;
        }

        // Metodo per convertire FoodItem in FoodItemCart
        public FoodItemCart ToFoodItemCart(int productId, double price)
        {
            return new FoodItemCart
            {
                ProductID = productId,
                Image = this.Image,
                Title = this.Title,
                Description = this.Description,
                Price = price,
                IsLowStock = this.IsLowStock
            };
        }
        // Metodo per convertire FoodItem in ProductPageData
        public ProductPageData ToProductPageData(
            int productId,
            string details,
            string raccomandation,
            decimal originalPrice,
            decimal? discountedPrice,
            string storeName,
            string storeImage,
            int remainingItems,
            List<string> categories,
            List<string> allergens
            )
        {
            return new ProductPageData
            {
                ProductID = productId,
                ProductName = this.Title,
                Description = this.Description,
                Details = details,
                Raccomandation = raccomandation,
                OriginalPrice = originalPrice,
                DiscountedPrice = discountedPrice,
                StoreName = storeName,
                StoreImage = storeImage,
                ProductImage = this.Image,
                RemainingItems = remainingItems,
                Categories = categories,
                Allergens = allergens
            };
        }
    }
}
