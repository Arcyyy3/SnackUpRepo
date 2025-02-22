using System;
using System.Collections.Generic;
using SnackUpAPI.Models;
using SnackUpAPI.Data;

namespace SnackUpAPI.Services
{
    public class PromotionService
    {
        private readonly IDatabaseService _databaseService;

        public PromotionService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public IEnumerable<Promotion> GetAllPromotions()
        {
            return _databaseService.Query<Promotion>(
                "SELECT * FROM Promotions WHERE Deleted IS NULL"
            );
        }

   
        public ActivePromotionsDTO GetActivePromotions()
        {
            var result = new ActivePromotionsDTO();

            // Recupera le promozioni attive per i prodotti BUNDLE
            result.BundlePromotions = _databaseService.Query<PromotionActive>(
                @"SELECT 
          PR.ProductName,
          P.DiscountPercentage,
          P.StartDate,
          P.EndDate
          FROM Promotions P
          INNER JOIN ProductPromotions PP ON P.PromotionID = PP.PromotionID
          INNER JOIN Products PR ON PP.ProductID = PR.ProductID
          WHERE PR.BundleID IS NOT NULL
          AND P.StartDate <= GETDATE() 
          AND P.EndDate >= GETDATE() 
          AND P.Deleted IS NULL"
            ).ToList();

            // Recupera le promozioni attive per i prodotti REGOLARI
            result.RegularPromotions = _databaseService.Query<PromotionActive>(
                @"SELECT 
          PR.ProductName,
          P.DiscountPercentage,
          P.StartDate,
          P.EndDate
          FROM Promotions P
          INNER JOIN ProductPromotions PP ON P.PromotionID = PP.PromotionID
          INNER JOIN Products PR ON PP.ProductID = PR.ProductID
          WHERE PR.BundleID IS NULL
          AND P.StartDate <= GETDATE() 
          AND P.EndDate >= GETDATE() 
          AND P.Deleted IS NULL"
            ).ToList();

            return result;
        }
        public ActivePromotionsDTOMenu GetActivePromotionsMenu()
        {
            var result = new ActivePromotionsDTOMenu();

            // Recupera le promozioni attive per i prodotti BUNDLE
            result.BundlePromotionsMenu = _databaseService.Query<PromotionActiveMenu>(
                @"SELECT 
          PR.ProductName,
          P.DiscountPercentage,
          PR.Price,
          PR.PhotoLinkProdotto,
          PR.Description,
          PR.Details 
          FROM Promotions P
          INNER JOIN ProductPromotions PP ON P.PromotionID = PP.PromotionID
          INNER JOIN Products PR ON PP.ProductID = PR.ProductID
          WHERE PR.BundleID IS NOT NULL
          AND P.StartDate <= GETDATE() 
          AND P.EndDate >= GETDATE() 
          AND P.Deleted IS NULL"
            ).ToList();

            // Recupera le promozioni attive per i prodotti REGOLARI
            result.RegularPromotionsMenu = _databaseService.Query<PromotionActiveMenu>(
                @"SELECT 
          PR.ProductName,
          P.DiscountPercentage,
          PR.Price,
          PR.PhotoLinkProdotto,
          PR.Description,
          PR.Details 
          FROM Promotions P
          INNER JOIN ProductPromotions PP ON P.PromotionID = PP.PromotionID
          INNER JOIN Products PR ON PP.ProductID = PR.ProductID
          WHERE PR.BundleID IS NULL
          AND P.StartDate <= GETDATE() 
          AND P.EndDate >= GETDATE() 
          AND P.Deleted IS NULL"
            ).ToList();

            return result;
        }
        public void AddPromotionForProductsByName(PromotionForProductRequest request)
        {
            // Verifica che la lista dei prodotti non sia vuota
            if (request.ProductNames == null || !request.ProductNames.Any())
            {
                throw new ArgumentException("La lista dei prodotti non può essere vuota");
            }

            // 1. Recupera il ProductID per ogni prodotto nella lista
            List<int> productIds = new List<int>();
            foreach (var productName in request.ProductNames)
            {
                int? productId = _databaseService.QuerySingleOrDefault<int?>(
                    @"SELECT ProductID
              FROM Products
              WHERE ProductName = @ProductName
                AND Deleted IS NULL",
                    new { ProductName = productName }
                );

                if (!productId.HasValue)
                {
                    throw new KeyNotFoundException($"Nessun prodotto trovato con nome '{productName}'");
                }

                productIds.Add(productId.Value);
            }

            // 2. Per ogni prodotto, verifica se esiste già una promozione sovrapposta
            foreach (var productId in productIds)
            {
                bool overlappingExists = _databaseService.Query<int>(
                    @"SELECT 1
              FROM Promotions p
              JOIN ProductPromotions pp ON p.PromotionID = pp.PromotionID
              WHERE pp.ProductID = @ProductID
                AND p.Deleted IS NULL
                AND p.StartDate <= @NewEndDate
                AND p.EndDate >= @NewStartDate",
                    new
                    {
                        ProductID = productId,
                        NewStartDate = request.StartDate.ToUniversalTime(),
                        NewEndDate = request.EndDate.ToUniversalTime()
                    }
                ).Any();

                if (overlappingExists)
                {
                    throw new InvalidOperationException(
                        $"Esiste già una promozione attiva in questo intervallo di date per il prodotto con ID {productId}."
                    );
                }
            }

            // 3. Inserisci la nuova promozione (una sola volta)
            int newPromotionID = _databaseService.QuerySingle<int>(
                @"INSERT INTO Promotions 
            (PromotionName, Description, DiscountPercentage, StartDate, EndDate, Created, Modified, Deleted) 
          VALUES
            (@PromotionName, @Description, @DiscountPercentage, @StartDate, @EndDate, @Created, NULL, NULL);
          SELECT SCOPE_IDENTITY();",
                new
                {
                    request.PromotionName,
                    request.Description,
                    request.DiscountPercentage,
                    StartDate = request.StartDate.ToUniversalTime(),
                    EndDate = request.EndDate.ToUniversalTime(),
                    Created = DateTime.UtcNow
                }
            );

            // 4. Inserisci la relazione in ProductPromotions per ogni prodotto
            foreach (var productId in productIds)
            {
                _databaseService.Execute(
                    @"INSERT INTO ProductPromotions (ProductID, PromotionID, Created)
              VALUES (@ProductID, @PromotionID, GETDATE())",
                    new
                    {
                        ProductID = productId,
                        PromotionID = newPromotionID
                    }
                );
            }
        }


        public void UpdatePromotion(Promotion promotion)
        {
          

            _databaseService.Execute(
                @"UPDATE Promotions 
                  SET PromotionName = @PromotionName, Description = @Description, DiscountPercentage = @DiscountPercentage, 
                      StartDate = @StartDate, EndDate = @EndDate, Modified = @Modified 
                  WHERE PromotionID = @PromotionID AND Deleted IS NULL",
                new
                {
                    promotion.PromotionName,
                    promotion.Description,
                    promotion.DiscountPercentage,
                    StartDate = promotion.StartDate.ToUniversalTime(),
                    EndDate = promotion.EndDate.ToUniversalTime(),
                    Modified = DateTime.UtcNow,
                    promotion.PromotionID
                }
            );
        }

        public void DeletePromotion(int promotionId)
        {
            _databaseService.Execute(
                @"UPDATE Promotions 
                  SET Deleted = @Deleted 
                  WHERE PromotionID = @PromotionID",
                new
                {
                    Deleted = DateTime.UtcNow,
                    PromotionID = promotionId
                }
            );
        }
        public class PromotionActive
        {
            public string ProductName { get; set; }
            public int DiscountPercentage { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
        }
        public class ActivePromotionsDTO
        {
            public List<PromotionActive> BundlePromotions { get; set; } = new List<PromotionActive>();
            public List<PromotionActive> RegularPromotions { get; set; } = new List<PromotionActive>();
        }
        public class PromotionActiveMenu
        {
            public string ProductName { get; set; }
            public int DiscountPercentage { get; set; }
            public double Price { get; set; }
            public string PhotoLinkProdotto { get; set; }
            public string Description { get; set; }
            public string Details { get; set; }

        }
        public class ActivePromotionsDTOMenu
        {
            public List<PromotionActiveMenu> BundlePromotionsMenu { get; set; } = new List<PromotionActiveMenu>();
            public List<PromotionActiveMenu> RegularPromotionsMenu { get; set; } = new List<PromotionActiveMenu>();
        }
    }
}
