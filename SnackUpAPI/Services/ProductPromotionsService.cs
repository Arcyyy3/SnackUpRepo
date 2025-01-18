using SnackUpAPI.Models;
using System;
using System.Collections.Generic;

namespace SnackUpAPI.Services
{
    public class ProductPromotionService
    {
        private readonly DatabaseService _databaseService;

        public ProductPromotionService(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public IEnumerable<ProductPromotion> GetAllProductPromotions()
        {
            return _databaseService.Query<ProductPromotion>(
                "SELECT * FROM ProductPromotions WHERE Deleted IS NULL"
            );
        }

        public ProductPromotion GetProductPromotionById(int id)
        {
            return _databaseService.QuerySingleOrDefault<ProductPromotion>(
                "SELECT * FROM ProductPromotions WHERE ProductPromotionID = @Id AND Deleted IS NULL",
                new { Id = id }
            );
        }

        public void AddProductPromotion(ProductPromotion productPromotion)
        {
            _databaseService.Execute(
                @"INSERT INTO ProductPromotions (ProductID, PromotionID, Created, Modified, Deleted) 
                  VALUES (@ProductID, @PromotionID, @Created, @Modified, NULL)",
                new
                {
                    productPromotion.ProductID,
                    productPromotion.PromotionID,
                    Created = productPromotion.Created ?? DateTime.UtcNow,
                    Modified = productPromotion.Modified ?? DateTime.UtcNow
                }
            );
        }

        public void UpdateProductPromotion(ProductPromotion productPromotion)
        {
            _databaseService.Execute(
                @"UPDATE ProductPromotions 
                  SET ProductID = @ProductID, PromotionID = @PromotionID, Modified = @Modified
                  WHERE ProductPromotionID = @Id AND Deleted IS NULL",
                new
                {
                    productPromotion.ProductID,
                    productPromotion.PromotionID,
                    Modified = DateTime.UtcNow,
                    Id = productPromotion.ProductPromotionID
                }
            );
        }

        public void DeleteProductPromotion(int id)
        {
            _databaseService.Execute(
                @"UPDATE ProductPromotions 
                  SET Deleted = @Deleted 
                  WHERE ProductPromotionID = @Id",
                new
                {
                    Deleted = DateTime.UtcNow,
                    Id = id
                }
            );
        }
    }
}
