using System;
using System.Collections.Generic;
using SnackUpAPI.Models;
using SnackUpAPI.Data;

namespace SnackUpAPI.Services
{
    public class ProductAllergenService
    {
        private readonly IDatabaseService _databaseService;

        public ProductAllergenService(IDatabaseService databaseService)
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
