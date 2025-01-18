using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.JSInterop;
using SnackUpAPI.Models;

namespace SnackUpAPI.Services
{
    public class ProductService
    {
        private readonly DatabaseService _databaseService;

        public ProductService(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _databaseService.Query<Product>(
                "SELECT * FROM Products WHERE Deleted IS NULL"
            );
        }

        public Product GetProductById(int id)
        {
            return _databaseService.QuerySingle<Product>(
                "SELECT * FROM Products WHERE ProductID = @ProductID AND Deleted IS NULL",
                new { ProductID = id }
            );
        }

        public List<string> GetProductByProducerID(int producerID)
        {
            return _databaseService.Query<string>(
                "SELECT DISTINCT Name FROM Products WHERE ProducerID = @ProducerID AND Deleted IS NULL",
                new { ProducerID = producerID }
            ).ToList();
        }
        public int GetProductIDByName(string name)
        {
            try
            {
                var producerId = _databaseService.QuerySingleOrDefault<int>(
                    "SELECT DISTINCT ProductID FROM Products WHERE Name = @Name AND Deleted IS NULL",
                    new { Name = name }
                );

                if (producerId == 0) // Supponendo che 0 non sia un ID valido
                {
                    throw new Exception($"Nessun produttore trovato per il nome del prodotto '{name}'.");
                }

                return producerId;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore durante il recupero dell'ID del produttore per '{name}': {ex.Message}");
                throw; // Propaga l'errore per un'ulteriore gestione
            }
        }

        public string GetImagePathByName(string name)
        {
            var product = _databaseService.QuerySingle<Product>(
                "SELECT PhotoLink FROM Products WHERE Name = @Name AND Deleted IS NULL",
                new { Name = name }
            );
            Console.WriteLine($"LINK FOTO: {product.PhotoLink}");
            return product.PhotoLink;
        }

        public IEnumerable<Product> GetProductsByFilter(int? producerId, string category)
        {
            var query = "SELECT * FROM Products WHERE Deleted IS NULL";
            var parameters = new Dictionary<string, object>();

            if (producerId.HasValue)
            {
                query += " AND ProducerID = @ProducerID";
                parameters.Add("ProducerID", producerId.Value);
            }

            if (!string.IsNullOrWhiteSpace(category))
            {
                query += " AND Category = @Category";
                parameters.Add("Category", category);
            }

            return _databaseService.Query<Product>(query, parameters);
        }

        public void AddProduct(Product product)
        {
            _databaseService.Execute(
                @"INSERT INTO Products (Name, Description,  Details,  Raccomandation, Price, ProducerID, PhotoLink, Created, Modified, Deleted) 
                  VALUES (@Name, @Description,  @Details,  @Raccomandation, @Price, @ProducerID, @PhotoLink, @Created, NULL, NULL)",
                new
                {
                    product.Name,
                    product.Description,
                    product.Details,
                    product.Raccomandation,
                    product.Price,
                    product.ProducerID,
                    product.PhotoLink,
                    Created = DateTime.UtcNow // Imposta Created come la data e ora correnti
                }
            );
        }
        public IEnumerable<object> GetProductsWithStockByCategory(string category)
        {
            var query = @"
        SELECT 
            p.ProductID,
            p.Name,
            p.Description,
            p.PhotoLink,
            p.Price,
            c.Name AS CategoryName,
            i.QuantityAvailable,
            i.ReorderLevel,
            CASE 
                WHEN i.QuantityAvailable <= i.ReorderLevel THEN 1
                ELSE 0
            END AS IsLowStock
        FROM Products p
        LEFT JOIN CategoryProducts cp ON p.ProductID = cp.ProductID
        LEFT JOIN Categories c ON cp.CategoryID = c.CategoryID
        LEFT JOIN Inventory i ON p.ProductID = i.ProductID
        WHERE p.Deleted IS NULL
          AND c.Name = @Category";

            return _databaseService.Query<object>(query, new { Category = category });
        }


        public IEnumerable<object> GetPreferredProducts(int userId)
        {
            var query = @"
    WITH ProductPurchaseSummary AS (
        SELECT 
            od.ProductID,
            SUM(od.Quantity) AS TotalQuantity
        FROM 
            Orders o
            INNER JOIN OrderDetails od ON o.OrderID = od.OrderID
        WHERE 
            o.UserID = @UserID
        GROUP BY 
            od.ProductID
    )
    SELECT 
        TOP 5 
        p.ProductID,
        p.Name,
        p.Description,
        p.PhotoLink,
        p.Price,
        i.QuantityAvailable,
        i.ReorderLevel,
        CASE 
            WHEN i.QuantityAvailable <= i.ReorderLevel THEN 1
            ELSE 0
        END AS IsLowStock,
        ps.TotalQuantity
    FROM 
        ProductPurchaseSummary ps
        INNER JOIN Products p ON ps.ProductID = p.ProductID
        LEFT JOIN Inventory i ON p.ProductID = i.ProductID
    WHERE 
        p.Deleted IS NULL
    ORDER BY 
        ps.TotalQuantity DESC;";


            return _databaseService.Query<object>(query, new { UserID = userId });
        }

        public IEnumerable<object> GetMostPurchasedProducts()
        {
            var query = @"
    WITH ProductPurchaseSummary AS (
        SELECT 
            od.ProductID,
            SUM(od.Quantity) AS TotalQuantity
        FROM 
            Orders o
            INNER JOIN OrderDetails od ON o.OrderID = od.OrderID
        WHERE 
            o.Deleted IS NULL
        GROUP BY 
            od.ProductID
    )
    SELECT 
        TOP 10 
        p.ProductID,
        p.Name,
        p.Description,
        p.PhotoLink,
        p.Price,
        i.QuantityAvailable,
        i.ReorderLevel,
        CASE 
            WHEN i.QuantityAvailable <= i.ReorderLevel THEN 1
            ELSE 0
        END AS IsLowStock,
        ps.TotalQuantity
    FROM 
        ProductPurchaseSummary ps
        INNER JOIN Products p ON ps.ProductID = p.ProductID
        LEFT JOIN Inventory i ON p.ProductID = i.ProductID
    WHERE 
        p.Deleted IS NULL
    ORDER BY 
        ps.TotalQuantity DESC;";

            return _databaseService.Query<object>(query);
        }

        public (int ProductID, decimal Price) GetProductDetailsByName(string productName)
        {
            var product = _databaseService.QuerySingle<Product>(
                "SELECT ProductID, Price FROM Products WHERE Name = @Name AND Deleted IS NULL",
                new { Name = productName }
            );

            if (product == null)
            {
                throw new Exception($"Prodotto con nome '{productName}' non trovato.");
            }

            return (product.ProductID, product.Price);
        }


        public void UpdateProduct(Product product)
        {
            _databaseService.Execute(
                @"UPDATE Products 
                  SET Name = @Name, Description = @Description, 
                      Price = @Price, ProducerID = @ProducerID, PhotoLink=@Photolink 
                      Modified = @Modified 
                  WHERE ProductID = @ProductID AND Deleted IS NULL",
                new
                {
                    product.Name,
                    product.Description,
                    product.Price,
                    product.ProducerID,
                    product.PhotoLink,
                    Modified = DateTime.UtcNow, // Imposta Modified come la data e ora correnti
                    product.ProductID
                }
            );
        }

        public void DeleteProduct(int id)
        {
            _databaseService.Execute(
                @"UPDATE Products 
                  SET Deleted = @Deleted 
                  WHERE ProductID = @ProductID",
                new
                {
                    Deleted = DateTime.UtcNow, // Imposta Deleted come la data e ora correnti
                    ProductID = id
                }
            );


        }

        public ProductPageData GetProductPageData(int productId)
        {
            // Query principale per ottenere i dettagli del prodotto
            var query = @"
        SELECT 
            p.ProductID,
            p.Name AS ProductName,
            p.Description,
            p.Details,
            p.Raccomandation,
            p.Price AS OriginalPrice,
            pr.Name AS StoreName,
            pr.PhotoLink AS StoreImage,
            p.PhotoLink AS ProductImage,
            i.QuantityAvailable AS RemainingItems
        FROM Products p
        INNER JOIN Producers pr ON p.ProducerID = pr.ProducerID
        LEFT JOIN Inventory i ON p.ProductID = i.ProductID
        WHERE p.ProductID = @ProductID AND p.Deleted IS NULL";

            var productData = _databaseService.QuerySingle<ProductPageData>(query, new { ProductID = productId });

            if (productData == null)
            {
                throw new Exception($"Prodotto con ID {productId} non trovato.");
            }

            // Query per le categorie
            var categoriesQuery = @"
        SELECT c.Name
        FROM Categories c
        INNER JOIN CategoryProducts cp ON c.CategoryID = cp.CategoryID
        WHERE cp.ProductID = @ProductID AND c.Deleted IS NULL";
            var categories = _databaseService.Query<string>(categoriesQuery, new { ProductID = productId });

            // Query per gli allergeni
            var allergensQuery = @"
        SELECT a.Name
        FROM Allergens a
        INNER JOIN ProductAllergens pa ON a.AllergenID = pa.AllergenID
        WHERE pa.ProductID = @ProductID AND a.Deleted IS NULL";
            var allergens = _databaseService.Query<string>(allergensQuery, new { ProductID = productId });

            // Query per ottenere la percentuale di sconto
            var promotionsQuery = @"
        SELECT p.DiscountPercentage
        FROM Promotions p
        INNER JOIN ProductPromotions pp ON p.PromotionID = pp.PromotionID
        WHERE pp.ProductID = @ProductID 
          AND p.StartDate <= GETDATE() 
          AND p.EndDate >= GETDATE() 
          AND p.Deleted IS NULL";
            var discount = _databaseService.QuerySingleOrDefault<decimal>(promotionsQuery, new { ProductID = productId });
            Console.WriteLine($"discont:{discount.ToString()}");
            // Calcolo del prezzo scontato
            productData.DiscountedPrice = discount > 0 ? productData.OriginalPrice * (1 - discount / 100) : (decimal?)null;

            // Aggiunta di categorie e allergeni ai dati del prodotto
            productData.Categories = categories.ToList();
            productData.Allergens = allergens.ToList();

            return productData;
        }

    }
}