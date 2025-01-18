using System;
using System.Collections.Generic;
using SnackUpAPI.Models;

namespace SnackUpAPI.Services
{
    public class ProductAllergenService
    {
        private readonly DatabaseService _databaseService;

        public ProductAllergenService(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public IEnumerable<ProductAllergen> GetAllergensByProductId(int productId)
        {
            return _databaseService.Query<ProductAllergen>(
                @"SELECT * FROM ProductAllergens 
                  WHERE ProductID = @ProductID AND Deleted IS NULL",
                new { ProductID = productId }
            );
        }

        public void AddProductAllergen(int productId, int allergenId)
        {
            _databaseService.Execute(
                @"INSERT INTO ProductAllergens (ProductID, AllergenID, Created) 
                  VALUES (@ProductID, @AllergenID, @Created)",
                new
                {
                    ProductID = productId,
                    AllergenID = allergenId,
                    Created = DateTime.UtcNow
                }
            );
        }

        public void RemoveProductAllergen(int productId, int allergenId)
        {
            _databaseService.Execute(
                @"DELETE FROM ProductAllergens 
                  WHERE ProductID = @ProductID AND AllergenID = @AllergenID",
                new
                {
                    ProductID = productId,
                    AllergenID = allergenId
                }
            );
        }
    }
}
