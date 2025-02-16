using System;
using System.Collections.Generic;
using SnackUpAPI.Models;
using SnackUpAPI.Data;

namespace SnackUpAPI.Services
{
    public class CategoryProductService
    {
        private readonly IDatabaseService _databaseService;

        public CategoryProductService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public IEnumerable<CategoryProductClass1> GetProductsByCategoryId(int categoryId)
        {
            return _databaseService.Query<CategoryProductClass1>(
                @"
					  SELECT p.* ,
					  CASE 
        WHEN prom.DiscountPercentage IS NOT NULL AND prom.DiscountPercentage > 0 THEN 
            ROUND(p.Price * (1 - prom.DiscountPercentage / 100.0), 2)
        ELSE 
            p.Price
    END AS DiscountedPrice
                  FROM Products p
                  INNER JOIN CategoryProducts cp ON p.ProductID = cp.ProductID
				  LEFT JOIN 
    ProductPromotions pp ON p.ProductID = pp.ProductID AND pp.Deleted IS NULL
				  LEFT JOIN 
    Promotions prom ON pp.PromotionID = prom.PromotionID AND prom.Deleted IS NULL

                  WHERE cp.CategoryID = @CategoryID AND p.Deleted IS NULL AND cp.Deleted IS NULL AND (
        (prom.StartDate <= GETDATE() AND (prom.EndDate IS NULL OR prom.EndDate >= GETDATE()))
        OR prom.PromotionID IS NULL
    );",
                new { CategoryID = categoryId }
            );
        }
        public IEnumerable<ProductDrink> GetDrink()
        {
            return _databaseService.Query<ProductDrink>(
                @"
					
				  SELECT p.ProductName ,
                         p.PhotoLinkProdotto,
				  CASE 
    WHEN prom.DiscountPercentage IS NOT NULL AND prom.DiscountPercentage > 0 THEN 
        ROUND(p.Price * (1 - (prom.DiscountPercentage) / 100.0), 2)
    ELSE 
         (p.Price)

END AS DiscountedPrice,
                         p.ProductID
              FROM Products p
              INNER JOIN CategoryProducts cp ON p.ProductID = cp.ProductID
			  LEFT JOIN 
ProductPromotions pp ON p.ProductID = pp.ProductID AND pp.Deleted IS NULL
			  LEFT JOIN 
Promotions prom ON pp.PromotionID = prom.PromotionID AND prom.Deleted IS NULL

              WHERE cp.CategoryID = 6 AND p.Deleted IS NULL AND cp.Deleted IS NULL AND (
    (prom.StartDate <= GETDATE() AND (prom.EndDate IS NULL OR prom.EndDate >= GETDATE()))
    OR prom.PromotionID IS NULL
);"

            );
        }
        public IEnumerable<Product> GetProductsByCategoryIdPreview(int categoryId)
        {
            return _databaseService.Query<Product>(
                @"SELECT p.PhotoLink, p.ProductName,p.Description,p.IsLowStock
                    
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
    public class CategoryProductClass1
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Details { get; set; }
        public string Raccomandation { get; set; }
        public decimal Price { get; set; }
        public string? PhotoLinkProdotto { get; set; }
        public int? ProducerID { get; set; }
        public DateTime? Created { get; set; } = DateTime.Now;
        public DateTime? Modified { get; set; }
        public DateTime? Deleted { get; set; }
        public decimal DiscountedPrice { get; set; }
    }
    public class ProductDrink
    {
        public string? Name { get; set; }
        public string PhotoLinkProdotto { get; set; }
        public string DiscountedPrice { get; set; }
        public int? ProductID { get; private set; }
    }

}
