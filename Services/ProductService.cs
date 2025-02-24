using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.JSInterop;
using SnackUpAPI.Models;
using Microsoft.Data.SqlClient; // Per Dapper con Microsoft.Data.SqlClient
using System.Data.SqlClient; // Per Dapper con System.Data.SqlClient
using SnackUpAPI.Data;

namespace SnackUpAPI.Services
{
    public class ProductService
    {
        private readonly IDatabaseService _databaseService;

        public ProductService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _databaseService.Query<Product>(
                "SELECT * FROM Products WHERE Deleted IS NULL"
            );
        }
        public IEnumerable<object> GetProductsGroupedByCategory()
        {
            var query = @"
   	  SELECT 
      cs.CategoryName AS CategoryName,
      p.PhotoLinkProdotto AS PhotoLinkProdotto,
      p.ProductName AS ProductName
  FROM Products p
  INNER JOIN CategoryProducts cp ON p.ProductID = cp.ProductID
  INNER JOIN Categories cs ON cp.CategoryID = cs.CategoryID
  WHERE cp.CategoryID IN (1, 2, 3, 4, 5, 6, 7,10) and cs.Deleted IS  NULL and cp.Deleted is null
  ORDER BY cs.CategoryID";

            var results = _databaseService.Query<dynamic>(query);

            // Raggruppa i prodotti per categoria
            var groupedResult = results
                .GroupBy(
                    r => r.CategoryName,
                    r => new { PhotoLinkProdotto = r.PhotoLinkProdotto, ProductName = r.ProductName })
                .Select(group => new
                {
                    CategoryName = group.Key,
                    Products = group.ToList()
                });

            return groupedResult;
        }


        public Product GetProductById(int id)
        {
            return _databaseService.QuerySingle<Product>(
                "SELECT * FROM Products WHERE ProductID = @ProductID AND Deleted IS NULL",
                new { ProductID = id }
            );
        }

        public IEnumerable<BundleCatalog> GetBundleCatalog() // RESTITUISCE UNA LISTA
        {
            return _databaseService.Query<BundleCatalog>(
                @"SELECT 
            P.ProductName,
            P.Description,
            P.PhotoLinkProdotto,
            P.Details,
            P.Price
          FROM Products AS P
          WHERE P.BundleID IS NOT NULL"
            );
        }


        public List<string> GetProductByProducerID(int producerID)
        {
            return _databaseService.Query<string>(
                "SELECT DISTINCT ProductName FROM Products WHERE ProducerID = @ProducerID AND Deleted IS NULL",
                new { ProducerID = producerID }
            ).ToList();
        }


        public ProductPromotionDTO GetProductPromotionsByUserID(int userID)
        {
            // 1. Verifica se l'utente esiste ed è un producer → recupera ProducerID
            //    Presupponiamo che nella tabella Users ci sia un campo ProducerID e/o un Role = 'Producer'.
            var producerID = _databaseService.QuerySingleOrDefault<int?>(
                @"SELECT ProducerID 
          FROM ProducerUsers
          WHERE UserID = @UserID
            AND Deleted IS NULL",
                new { UserID = userID }
            );

            // 2. Se non c'è un ProducerID, l'utente non è un producer o non esiste → restituisci oggetto vuoto o null
            if (!producerID.HasValue)
            {
                // Puoi anche restituire `null` o lanciare un'eccezione, ma
                // qui preferiamo restituire un DTO vuoto.
                return new ProductPromotionDTO();
            }

            // 3. Recupera i prodotti, come facevi prima
            var result = new ProductPromotionDTO();

            // Prodotti BUNDLE (BundleID NON NULL)
            result.BundleProducts = _databaseService.Query<string>(
                @"SELECT DISTINCT ProductName 
          FROM Products 
          WHERE ProducerID = @ProducerID 
            AND BundleID IS NOT NULL 
            AND Deleted IS NULL",
                new { ProducerID = producerID.Value }
            ).ToList();

            // Prodotti REGULAR (BundleID NULL)
            result.RegularProducts = _databaseService.Query<string>(
                @"SELECT DISTINCT ProductName 
          FROM Products 
          WHERE ProducerID = @ProducerID 
            AND BundleID IS NULL 
            AND Deleted IS NULL",
                new { ProducerID = producerID.Value }
            ).ToList();

            return result;
        }


        public int GetProductIDByName(string productName)
        {
            try
            {
                var producerId = _databaseService.QuerySingleOrDefault<int>(
                    "SELECT DISTINCT ProductID FROM Products WHERE ProductName = @ProductName AND Deleted IS NULL",
                    new { ProductName = productName }
                );

                if (producerId == 0) // Supponendo che 0 non sia un ID valido
                {
                    throw new Exception($"Nessun produttore trovato per il nome del prodotto '{productName}'.");
                }

                return producerId;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore durante il recupero dell'ID del produttore per '{productName}': {ex.Message}");
                throw; // Propaga l'errore per un'ulteriore gestione
            }
        }

        public string GetImagePathByName(string productName)
        {
            var product = _databaseService.QuerySingle<Product>(
                "SELECT PhotoLinkProdotto FROM Products WHERE ProductName = @ProductName AND Deleted IS NULL",
                new { ProductName = productName }
            );
            Console.WriteLine($"LINK FOTO: {product.PhotoLinkProdotto}");
            return product.PhotoLinkProdotto;
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
                @"INSERT INTO Products (BundleID, ProductName, Description, Details, Raccomandation, Price, ProducerID, PhotoLinkProdotto, Created, Modified, Deleted) 
                  VALUES (@BundleID, @ProductName, @Description, @Details, @Raccomandation, @Price, @ProducerID, @PhotoLinkProdotto, @Created, NULL, NULL)",
                new
                {
                    product.BundleID,
                    product.ProductName,
                    product.Description,
                    product.Details,
                    product.Raccomandation,
                    product.Price,
                    product.ProducerID,
                    product.PhotoLinkProdotto,
                    Created = DateTime.UtcNow
                }
            );
        }
        public IEnumerable<object> GetProductsWithStockByCategory(string category)
        {
            var query = @"
        SELECT 
            p.ProductID,
            p.ProductName,
            p.Description,
            p.PhotoLinkProdotto,
            p.Price,
            c.CategoryName AS CategoryName,
           (I.QuantityAvailable - I.QuantityReserved) AS QuantityAvailable,
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
          AND c.CategoryName = @Category AND cp.Deleted is null AND C.Deleted is null";

            return _databaseService.Query<object>(query, new { Category = category });
        }


        public IEnumerable<object> GetPreferredProducts(int userId)
        {
            var query = @"  WITH ProductPurchaseSummary AS (
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
SELECT TOP 5 
    p.ProductID,
    p.ProductName,
    p.Description,
    p.PhotoLinkProdotto,
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
    AND EXISTS (
        SELECT 1 
        FROM CategoryProducts cp 
        WHERE cp.ProductID = p.ProductID AND cp.Deleted IS NULL
    )
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
        p.ProductName,
        p.Description,
        p.PhotoLinkProdotto,
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
        public IEnumerable<object> GetMostPurchasedBundles()
        {
            var query = @"
WITH BundlePurchaseSummary AS (
    SELECT 
        bi.BundleID,
        SUM(od.Quantity) AS TotalQuantity
    FROM 
        Orders o
        INNER JOIN OrderDetails od ON o.OrderID = od.OrderID
		inner join Products BP on od.ProductID = BP.ProductID
        INNER JOIN BundleItems bi ON od.ProductID = bi.ProductID -- Collegamento tra i prodotti e i bundle
    WHERE 
        o.Deleted IS NULL
    GROUP BY 
        bi.BundleID
)
SELECT 
    TOP 5 
    b.ProductName,
    b.Description,
    b.PhotoLinkProdotto,
    b.Price,
    ps.TotalQuantity
FROM 
    BundlePurchaseSummary ps
    INNER JOIN Products b ON ps.BundleID = b.BundleID
WHERE 
    b.Deleted IS NULL
ORDER BY 
    ps.TotalQuantity DESC;";

            return _databaseService.Query<object>(query);
        }

        public ProductDetailsDto GetProductDetailsByName(string productName)
        {
            return _databaseService.QuerySingle<ProductDetailsDto>(
                "SELECT ProductID, Price FROM Products WHERE ProductName = @ProductName AND Deleted IS NULL",
                new { ProductName = productName }
            );

        }


        public void UpdateProduct(Product product)
        {
            _databaseService.Execute(
                @"UPDATE Products 
                  SET BundleID = @BundleID, ProductName = @ProductName, Description = @Description, 
                      Price = @Price, ProducerID = @ProducerID, PhotoLinkProdotto = @PhotoLinkProdotto, 
                      Modified = @Modified 
                  WHERE ProductID = @ProductID AND Deleted IS NULL",
                new
                {
                    product.BundleID,
                    product.ProductName,
                    product.Description,
                    product.Price,
                    product.ProducerID,
                    product.PhotoLinkProdotto,
                    Modified = DateTime.UtcNow,
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
                    Deleted = DateTime.UtcNow,
                    ProductID = id
                }
            );
        }


        public ProductPageData GetProductPageData(int ProductID)
        {
            string query = @"
	SELECT 
    p.ProductID,
    pr.ProducerName AS ProducerName,
    p.ProductName AS ProductName,
    ISNULL(i.QuantityAvailable, 0) AS QuantityAvailable,
    p.Price AS OriginalPrice,
    prom.DiscountPercentage,
    p.Description,
    p.Details,
    p.Raccomandation,
    p.PhotoLinkProdotto,
    pr.PhotoLinkProduttore,
    (
        SELECT STRING_AGG(c.CategoryName, ', ') 
        FROM CategoryProducts cp
        INNER JOIN Categories c ON cp.CategoryID = c.CategoryID
        WHERE cp.ProductID = p.ProductID AND cp.Deleted IS NULL AND c.Deleted IS NULL
    ) AS Categories,
    (
        SELECT STRING_AGG(a.AllergenName, ', ') 
        FROM ProductAllergens pa
        INNER JOIN Allergens a ON pa.AllergenID = a.AllergenID
        WHERE pa.ProductID = p.ProductID AND a.Deleted IS NULL AND pa.Deleted IS NULL
    ) AS Allergens,
    -- Calcolo del prezzo scontato
    CASE 
        WHEN prom.DiscountPercentage IS NOT NULL THEN 
            ROUND(p.Price * (1 - prom.DiscountPercentage / 100.0), 2)
        ELSE 
            p.Price
    END AS DiscountedPrice
FROM 
    Products p
INNER JOIN 
    Producers pr ON p.ProducerID = pr.ProducerID
LEFT JOIN 
    Inventory i ON p.ProductID = i.ProductID
LEFT JOIN 
    (
        SELECT 
            pp.ProductID,
            MAX(prom.DiscountPercentage) AS DiscountPercentage -- Prendi la promozione con lo sconto maggiore
        FROM 
            ProductPromotions pp
        INNER JOIN 
            Promotions prom ON pp.PromotionID = prom.PromotionID 
        WHERE 
            prom.Deleted IS NULL
            AND prom.StartDate <= GETDATE() 
            AND (prom.EndDate IS NULL OR prom.EndDate >= GETDATE())
        GROUP BY 
            pp.ProductID
    ) prom ON p.ProductID = prom.ProductID
WHERE 
    p.ProductID = @ProductID -- Sostituisci con il ProductID desiderato
    AND p.Deleted IS NULL;
";

            var productPageData = _databaseService.QuerySingleOrDefault<ProductPageData>(query, new { ProductID = ProductID });

            if (productPageData == null)
            {
                throw new Exception($"Prodotto con ID {ProductID} non trovato.");
            }

            return productPageData;
        }
        public ProductDetailsDTO GetProductDetails(string photoLinkProdotto)
        {
            // 1️⃣ Recupera il ProductID dal PhotoLinkProdotto
            int? productId = _databaseService.QuerySingleOrDefault<int?>(
                @"SELECT ProductID FROM Products WHERE PhotoLinkProdotto = @PhotoLinkProdotto AND Deleted IS NULL",
                new { PhotoLinkProdotto = photoLinkProdotto }
            );

            if (!productId.HasValue) return null; // Se non trova il prodotto, restituisce null

            // 2️⃣ Recupera le informazioni di base del prodotto
            var product = _databaseService.QuerySingle<ProductDetailsDTO>(
                @"SELECT 
            p.ProductID, 
            p.ProductName, 
            p.Description, 
            p.Details AS Ingredients, 
            p.Price, 
            p.PhotoLinkProdotto
          FROM Products p
          WHERE p.ProductID = @ProductID AND p.Deleted IS NULL",
                new { ProductID = productId }
            );

            if (product == null) return null;

            // 4️⃣ Recupera le categorie del prodotto
            product.Categories = _databaseService.Query<string>(
                @"SELECT c.CategoryName 
          FROM CategoryProducts cp
          JOIN Categories c ON cp.CategoryID = c.CategoryID
          WHERE cp.ProductID = @ProductID AND c.Deleted IS NULL AND cp.Deleted IS NULL",
                new { ProductID = productId }
            ).ToList();

            // 5️⃣ Recupera gli allergeni associati
            product.Allergens = _databaseService.Query<string>(
                @"SELECT a.AllergenName 
          FROM ProductAllergens pa
          JOIN Allergens a ON pa.AllergenID = a.AllergenID
          WHERE pa.ProductID = @ProductID AND a.Deleted IS NULL AND pa.Deleted IS NULL",
                new { ProductID = productId }
            ).ToList();

            // 6️⃣ Recupera lo stock disponibile e la quantità giornaliera disponibile
            var inventory = _databaseService.QuerySingleOrDefault<InventoryData>(
                @"SELECT 
            i.QuantityAvailable, 
            (i.QuantityAvailable - i.QuantityReserved) AS DailyAvailable
          FROM Inventory i
          WHERE i.ProductID = @ProductID AND i.Deleted IS NULL",
                new { ProductID = productId }
            );

            if (inventory != null)
            {
                product.Stock = inventory.QuantityAvailable;
                product.DailyAvailable = inventory.DailyAvailable;
            }
            else
            {
                product.Stock = 0;
                product.DailyAvailable = 0;
            }

            return product;
        }
        //UPDATE PER SUPPLIER
        public void UpdateProductName(int productID, string newName)
        {
            // Aggiorna il ProductName nella tabella Products
            _databaseService.Execute(
                @"UPDATE Products
          SET ProductName = @NewName,
              Modified = GETUTCDATE()
          WHERE ProductID = @ProductID
            AND Deleted IS NULL",
                new { NewName = newName, ProductID = productID }
            );
        }
        public void UpdateProductPrice(int productID, decimal newPrice)
        {
            _databaseService.Execute(
                @"UPDATE Products
          SET Price = @NewPrice,
              Modified = GETUTCDATE()
          WHERE ProductID = @ProductID
            AND Deleted IS NULL",
                new { NewPrice = newPrice, ProductID = productID }
            );
        }
        public void UpdateProductDetailsAndAssociations(
       int productID,
       List<string> newCategories,
       List<string> newAllergens,
       string newDescription,
       string newDetails)
        {
            // Step A: Aggiorna descrizione e dettagli del prodotto
            _databaseService.Execute(
                @"UPDATE Products
          SET Description = @Description,
              Details = @Details,
              Modified = GETUTCDATE()
          WHERE ProductID = @ProductID
            AND Deleted IS NULL",
                new { Description = newDescription, Details = newDetails, ProductID = productID }
            );

            // ---------------------------
            // Gestione delle Categorie
            // ---------------------------
            // Recupera le associazioni correnti attive per il prodotto
            var currentCategories = _databaseService.Query<CategoryAssociation>(
                @"SELECT cp.CategoryID, c.CategoryName
          FROM CategoryProducts cp
          INNER JOIN Categories c ON cp.CategoryID = c.CategoryID
          WHERE cp.ProductID = @ProductID
            AND cp.Deleted IS NULL
            AND c.Deleted IS NULL",
                new { ProductID = productID }
            );

            // Creiamo un set dei nomi di categoria attualmente associati (case insensitive)
            var currentCategoryNames = currentCategories
                                        .Select(x => x.CategoryName)
                                        .ToHashSet(StringComparer.OrdinalIgnoreCase);

            // Soft-delete delle associazioni che non sono più presenti nella nuova lista
            foreach (var assoc in currentCategories)
            {
                if (newCategories == null || !newCategories.Contains(assoc.CategoryName, StringComparer.OrdinalIgnoreCase))
                {
                    _databaseService.Execute(
                        @"UPDATE CategoryProducts
                  SET Deleted = GETUTCDATE()
                  WHERE ProductID = @ProductID
                    AND CategoryID = @CategoryID
                    AND Deleted IS NULL",
                        new { ProductID = productID, CategoryID = assoc.CategoryID }
                    );
                }
            }

            // Aggiunge le nuove associazioni mancanti
            if (newCategories != null)
            {
                foreach (var categoryName in newCategories)
                {
                    if (!currentCategoryNames.Contains(categoryName))
                    {
                        // Recupera l’ID della categoria (o la crea se non esiste)
                        var categoryID = _databaseService.QuerySingleOrDefault<int?>(
                            @"SELECT CategoryID FROM Categories
                      WHERE CategoryName = @CategoryName
                        AND Deleted IS NULL",
                            new { CategoryName = categoryName }
                        );

                        if (!categoryID.HasValue)
                        {
                            categoryID = _databaseService.QuerySingle<int>(
                                @"INSERT INTO Categories(CategoryName, Description, Created) 
                          VALUES(@CategoryName, @CategoryName, GETUTCDATE());
                          SELECT CAST(SCOPE_IDENTITY() AS int);",
                                new { CategoryName = categoryName }
                            );
                        }

                        // Inserisce la nuova associazione in CategoryProducts
                        _databaseService.Execute(
                            @"INSERT INTO CategoryProducts(ProductID, CategoryID, Created)
                      VALUES(@ProductID, @CategoryID, GETUTCDATE())",
                            new { ProductID = productID, CategoryID = categoryID.Value }
                        );
                    }
                }
            }

            // ---------------------------
            // Gestione degli Allergeni
            // ---------------------------
            var currentAllergens = _databaseService.Query<AllergenAssociation>(
                @"SELECT pa.AllergenID, a.AllergenName
      FROM ProductAllergens pa
      INNER JOIN Allergens a ON pa.AllergenID = a.AllergenID
      WHERE pa.ProductID = @ProductID
        AND pa.Deleted IS NULL
        AND a.Deleted IS NULL",
                new { ProductID = productID }
            );

            // Creiamo un set dei nomi di allergene attualmente associati (active)
            var currentAllergenNames = currentAllergens
                                        .Select(x => x.AllergenName)
                                        .ToHashSet(StringComparer.OrdinalIgnoreCase);

            // Soft-delete delle associazioni di allergeni non presenti nella nuova lista
            foreach (var assoc in currentAllergens)
            {
                if (newAllergens == null || !newAllergens.Contains(assoc.AllergenName, StringComparer.OrdinalIgnoreCase))
                {
                    _databaseService.Execute(
                        @"UPDATE ProductAllergens
              SET Deleted = GETUTCDATE()
              WHERE ProductID = @ProductID
                AND AllergenID = @AllergenID
                AND Deleted IS NULL",
                        new { ProductID = productID, AllergenID = assoc.AllergenID }
                    );
                }
            }

            // Aggiunge le nuove associazioni mancanti
            if (newAllergens != null)
            {
                foreach (var allergenName in newAllergens)
                {
                    // Se il nome non è tra quelli attivi
                    if (!currentAllergenNames.Contains(allergenName))
                    {
                        // Recupera l’ID dell’allergene (o lo crea se non esiste)
                        var allergenID = _databaseService.QuerySingleOrDefault<int?>(
                            @"SELECT AllergenID FROM Allergens
                  WHERE AllergenName = @AllergenName
                    AND Deleted IS NULL",
                            new { AllergenName = allergenName }
                        );
                        if (!allergenID.HasValue)
                        {
                            allergenID = _databaseService.QuerySingle<int>(
                                @"INSERT INTO Allergens(AllergenName, Description, Created) 
                      VALUES(@AllergenName, @AllergenName, GETUTCDATE());
                      SELECT CAST(SCOPE_IDENTITY() AS int);",
                                new { AllergenName = allergenName }
                            );
                        }

                        // Verifica se esiste già un'associazione per questo prodotto e allergene,
                        // anche se soft-deleted
                        var existingAssociationID = _databaseService.QuerySingleOrDefault<int?>(
                            @"SELECT ProductAllergenID
                  FROM ProductAllergens
                  WHERE ProductID = @ProductID
                    AND AllergenID = @AllergenID",
                            new { ProductID = productID, AllergenID = allergenID.Value }
                        );
                        if (existingAssociationID.HasValue)
                        {
                            // Riattiva l'associazione soft-deleted
                            _databaseService.Execute(
                                @"UPDATE ProductAllergens
                      SET Deleted = NULL, Modified = GETUTCDATE()
                      WHERE ProductAllergenID = @ProductAllergenID",
                                new { ProductAllergenID = existingAssociationID.Value }
                            );
                        }
                        else
                        {
                            // Inserisce la nuova associazione
                            _databaseService.Execute(
                                @"INSERT INTO ProductAllergens(ProductID, AllergenID, Created)
                      VALUES(@ProductID, @AllergenID, GETUTCDATE())",
                                new { ProductID = productID, AllergenID = allergenID.Value }
                            );
                        }
                    }
                }
            }
        }



            public void UpdateProductStock(int productID, int newQuantityAvailable)
        {
            // Aggiorna la tabella Inventory
            // Se non esiste un record per questo prodotto, lo puoi creare
            var existingInventory = _databaseService.QuerySingleOrDefault<int?>(
                "SELECT ProductID FROM Inventory WHERE ProductID = @ProductID AND Deleted IS NULL",
                new { ProductID = productID }
            );

            if (existingInventory == null)
            {
                // Creiamo la riga in Inventory
                _databaseService.Execute(
                    @"INSERT INTO Inventory(ProductID, QuantityAvailable, Created)
              VALUES(@ProductID, @QuantityAvailable, GETUTCDATE())",
                    new { ProductID = productID, QuantityAvailable = newQuantityAvailable }
                );
            }
            else
            {
                // Aggiorniamo la riga
                _databaseService.Execute(
                    @"UPDATE Inventory
              SET QuantityAvailable = @QuantityAvailable,
                  Modified = GETUTCDATE()
              WHERE ProductID = @ProductID
                AND Deleted IS NULL",
                    new { QuantityAvailable = newQuantityAvailable, ProductID = productID }
                );
            }
        }




        public class ProductDetailsDto
        {
            public int ProductID { get; set; }
            public double Price { get; set; }
        }
        public class BundleCatalog
        {
            public string ProductName { get; set; }
            public string Description { get; set; }
            public string PhotoLinkProdotto { get; set; }
            public string Details { get; set; }
            public decimal Price { get; set; }
        }
    public class ProductDetailsDTO
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string Ingredients { get; set; }
        public decimal Price { get; set; }
        public string PhotoLinkProdotto { get; set; }
        public List<string> Categories { get; set; } = new List<string>();
        public List<string> Allergens { get; set; } = new List<string>();
        public int Stock { get; set; }
        public int DailyAvailable { get; set; }
    }
    public class InventoryData
    {
        public int QuantityAvailable { get; set; }
        public int DailyAvailable { get; set; }
    }
        public class ProductPromotionDTO
        {
            public List<string> BundleProducts { get; set; } = new List<string>();
            public List<string> RegularProducts { get; set; } = new List<string>();
        }
        // Classi di supporto per mappare le associazioni correnti
        private class CategoryAssociation
        {
            public int CategoryID { get; set; }
            public string CategoryName { get; set; }
        }

        private class AllergenAssociation
        {
            public int AllergenID { get; set; }
            public string AllergenName { get; set; }
        }
    }
}