using System;
using System.Collections.Generic;
using SnackUpAPI.Models;

namespace SnackUpAPI.Services
{
    public class CategoryProductService
    {
        private readonly DatabaseService _databaseService;

        public CategoryProductService(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public IEnumerable<Product> GetProductsByCategoryId(int categoryId)
        {
            return _databaseService.Query<Product>(
                @"SELECT p.* 
                  FROM Products p
                  INNER JOIN CategoryProducts cp ON p.ProductID = cp.ProductID
                  WHERE cp.CategoryID = @CategoryID AND p.Deleted IS NULL AND cp.Deleted IS NULL",
                new { CategoryID = categoryId }
            );
        }
         public IEnumerable<Product> GetProductsByCategoryIdPreview(int categoryId)
        {
            return _databaseService.Query<Product>(
                @"SELECT p.PhotoLink, p.Name,p.Description,p.IsLowStock
                    
                  FROM Products p
                  INNER JOIN CategoryProducts cp ON p.ProductID = cp.ProductID
                  WHERE cp.CategoryID = @CategoryID AND p.Deleted IS NULL AND cp.Deleted IS NULL",
                new { CategoryID = categoryId }
            );
        }
        public IEnumerable<Category> GetCategoriesByProductId(int productId)
        {
            return _databaseService.Query<Category>(
                @"SELECT c.* 
                  FROM Categories c
                  INNER JOIN CategoryProducts cp ON c.CategoryID = cp.CategoryID
                  WHERE cp.ProductID = @ProductID AND c.Deleted IS NULL AND cp.Deleted IS NULL",
                new { ProductID = productId }
            );
        }

        public void AddCategoryToProduct(int productId, int categoryId)
        {
            _databaseService.Execute(
                @"INSERT INTO CategoryProducts (ProductID, CategoryID, Created) 
                  VALUES (@ProductID, @CategoryID, @Created)",
                new
                {
                    ProductID = productId,
                    CategoryID = categoryId,
                    Created = DateTime.UtcNow
                }
            );
        }

        public void RemoveCategoryFromProduct(int productId, int categoryId)
        {
            _databaseService.Execute(
                @"UPDATE CategoryProducts 
                  SET Deleted = @Deleted 
                  WHERE ProductID = @ProductID AND CategoryID = @CategoryID",
                new
                {
                    Deleted = DateTime.UtcNow,
                    ProductID = productId,
                    CategoryID = categoryId
                }
            );
        }
    }
}
