using System;
using System.Collections.Generic;
using SnackUpAPI.Models;
using SnackUpAPI.Data;

namespace SnackUpAPI.Services
{
    public class CartItemService
    {
        private readonly IDatabaseService _databaseService;

        public CartItemService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public IEnumerable<CartItem> GetItemsBySessionId(int sessionId)
        {
            var query = @"
    WITH ActivePromotions AS (
        SELECT 
            pp.ProductID,
            MAX(prom.DiscountPercentage) AS DiscountPercentage
        FROM ProductPromotions pp
        INNER JOIN Promotions prom ON pp.PromotionID = prom.PromotionID
        WHERE 
            prom.Deleted IS NULL
            AND prom.StartDate <= GETDATE()
            AND (prom.EndDate IS NULL OR prom.EndDate >= GETDATE())
        GROUP BY pp.ProductID
    )
    SELECT 
        c.CartItemID,
        c.SessionID,
        c.ProductID,
        c.Quantity,
        c.DeliveryDate,
        c.Recreation,
        c.CreatedAt,
        c.UpdatedAt,
        c.DeletedAt,
        -- Sostituisce il prezzo originale con il prezzo scontato se presente
        ROUND(c.Price * (1 - COALESCE(ap.DiscountPercentage, 0) / 100.0), 2) AS Price,
        -- Mantiene il calcolo del totale coerente con il nuovo prezzo
        ROUND(c.Quantity * (c.Price * (1 - COALESCE(ap.DiscountPercentage, 0) / 100.0)), 2) AS Total
    FROM CartItems c
    LEFT JOIN ActivePromotions ap ON c.ProductID = ap.ProductID
    WHERE c.SessionID = @SessionID AND c.DeletedAt IS NULL";

            return _databaseService.Query<CartItem>(query, new { SessionID = sessionId });
        }

        public IEnumerable<CartItem> GetItemsBySessionIdAAA(int sessionId)
        {
            return _databaseService.Query<CartItem>(
                "SELECT * FROM CartItems WHERE SessionID = @SessionID",
                new { SessionID = sessionId }
            );
        }
        public IEnumerable<CartProduct> GetCartProduct(int userID)
        {
            return _databaseService.Query<CartProduct>(
                "SELECT distinct  P.ProductName AS ProductName,  I.QuantityAvailable AS RemainingQuantity,  CASE  WHEN AP.PromotionID IS NOT NULL  THEN P.Price * (1 - (AP.DiscountPercentage / 100.0))   ELSE P.Price END AS DiscountedPrice FROM ShoppingSessions AS SS INNER JOIN CartItems AS CI   ON SS.SessionID = CI.SessionID INNER JOIN Products AS P ON CI.ProductID = P.ProductID INNER JOIN Inventory AS I ON P.ProductID = I.ProductID LEFT JOIN ( SELECT   PP.ProductID, PR.PromotionID, PR.DiscountPercentage FROM ProductPromotions AS PP  INNER JOIN Promotions AS PR ON PP.PromotionID = PR.PromotionID WHERE GETDATE() BETWEEN PR.StartDate AND PR.EndDate AND PR.Deleted IS NULL ) AS AP ON P.ProductID = AP.ProductID WHERE SS.UserID = @UserID;",
                new { UserID = userID }
            );
        }
        public double GetTotalPrice(int userID)
        {
            return _databaseService.QuerySingle<double>(
                "DECLARE @TotalSum DECIMAL(10, 2); SELECT @TotalSum = SUM(CI.Total) FROM CartItems CI INNER JOIN ShoppingSessions SS ON CI.SessionID = SS.SessionID WHERE SS.UserID = 9 AND CI.DeletedAt IS NULL; UPDATE ShoppingSessions SET TotalAmount = @TotalSum WHERE UserID = @UserID; SELECT @TotalSum AS TotalSum;",
                new { UserID = userID }
            );
        }
        public void AddItemToCartFromPayload(string productName, int quantity, int userId, decimal price)
        {
            try
            {
                // Trova il ProductID dal nome del prodotto
                var productId = _databaseService.QuerySingleOrDefault<int?>(
                    "SELECT ProductID FROM Products WHERE ProductName = @ProductName AND Deleted IS NULL",
                    new { ProductName = productName }
                );

                if (productId == null)
                {
                    throw new Exception($"Il prodotto con nome '{productName}' non esiste.");
                }

                // Recupera l'ID della sessione per l'utente
                var sessionId = _databaseService.QuerySingleOrDefault<int?>(
                    "SELECT SessionID FROM ShoppingSessions WHERE UserID = @UserID AND Status = 'Active'",
                    new { UserID = userId }
                );

                if (sessionId == null)
                {
                    // Se non esiste una sessione attiva, creane una nuova
                    sessionId = _databaseService.QuerySingle<int>(
                        @"INSERT INTO ShoppingSessions (UserID, Status, TotalAmount, CreatedAt)
                  OUTPUT INSERTED.SessionID
                  VALUES (@UserID, 'Active', 0, GETDATE())",
                        new { UserID = userId }
                    );
                }

                // Aggiungi o aggiorna l'elemento nel carrello
                _databaseService.Execute(
                    @"MERGE INTO CartItems AS target
              USING (SELECT @SessionID AS SessionID, @ProductID AS ProductID) AS source
              ON target.SessionID = source.SessionID AND target.ProductID = source.ProductID
              WHEN MATCHED THEN
                  UPDATE SET Quantity = Quantity + @Quantity,
                             Total = Total + (@Price * @Quantity),
                             UpdatedAt = GETDATE()
              WHEN NOT MATCHED THEN
                  INSERT (SessionID, ProductID, Quantity, Price, Total, CreatedAt)
                  VALUES (@SessionID, @ProductID, @Quantity, @Price, @Price * @Quantity, GETDATE());",
                    new { SessionID = sessionId, ProductID = productId, Quantity = quantity, Price = price }
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore durante l'aggiunta al carrello: {ex.Message}");
                throw;
            }
        }
        public void AddOrUpdateItem(int sessionId, int productId, int quantity, decimal price)
        {
            _databaseService.Execute(
                @"MERGE INTO CartItems AS target
          USING (SELECT @SessionID AS SessionID, @ProductID AS ProductID) AS source
          ON target.SessionID = source.SessionID AND target.ProductID = source.ProductID
          WHEN MATCHED THEN
              UPDATE SET Quantity = Quantity + @Quantity,
                         Total = Total + (@Price * @Quantity),
                         UpdatedAt = GETDATE()
          WHEN NOT MATCHED THEN
              INSERT (SessionID, ProductID, Quantity, Price, Total, DeliveryDate, Recreation, CreatedAt)
              VALUES (@SessionID, @ProductID, @Quantity, @Price, @Price * @Quantity, NULL, NULL, GETDATE());",
                new { SessionID = sessionId, ProductID = productId, Quantity = quantity, Price = price }
            );
        }

        public void UpdateItemQuantity(int sessionId, int productId, int quantity)
        {
            if (quantity <= 0)
            {
                // Se la quantità è 0 o inferiore, elimina l'elemento dal carrello
                _databaseService.Execute(
                    "DELETE FROM CartItems WHERE SessionID = @SessionID AND ProductID = @ProductID",
                    new { SessionID = sessionId, ProductID = productId }
                );
            }
            else
            {
                // Aggiorna la quantità e il totale
                _databaseService.Execute(
                    @"UPDATE CartItems
              SET Quantity = @Quantity,
                  Total = Price * @Quantity,
                  UpdatedAt = GETDATE()
              WHERE SessionID = @SessionID AND ProductID = @ProductID",
                    new { SessionID = sessionId, ProductID = productId, Quantity = quantity }
                );
            }
        }


        public void RemoveItem(int cartItemId)
        {
            _databaseService.Execute(
                "DELETE FROM CartItems WHERE CartItemID = @CartItemID",
                new { CartItemID = cartItemId }
            );
        }
       
        public void RemoveItemFromName(string cartName, int userID)
{
    Console.WriteLine($"Starting deletion for cartName: {cartName}, userID: {userID}");

    int? productId = _databaseService.QuerySingleOrDefault<int?>(
        "SELECT ProductID FROM Products WHERE ProductName = @ProductName",
        new { ProductName = cartName }
    );
            if (productId == null)
            {
                throw new Exception($"Il prodotto con nome '{cartName}' non esiste.");
            }
            int? sessionID = _databaseService.QuerySingleOrDefault<int?>(
                "SELECT SessionID FROM ShoppingSessions WHERE UserID = @UserID AND Status= 'Active' ",
                new { UserID = userID }
            );
            if (userID == null)
            {
                throw new Exception($"Il prodotto con USerID '{userID}' non esiste.");
            }

            Console.WriteLine($"Found ProductID: {productId} for cartName: {cartName}");

    _databaseService.Execute(
        "DELETE FROM CartItems WHERE ProductID = @ProductID AND SessionID = @SessionID",
        new { ProductID = productId.Value, SessionID = sessionID.Value }
    );

    Console.WriteLine($"Successfully deleted ProductID: {productId} for userID: {userID}");
}

        public int SessionIDByUserID(int userID)
        {
            // Esegue la query per ottenere l'ID della sessione
            int? sessionID = _databaseService.QuerySingleOrDefault<int?>(
                "SELECT SessionID FROM ShoppingSessions WHERE UserID = @UserID",
                new { UserID = userID }
            );

            // Verifica se la sessione esiste
            if (sessionID == null)
            {
                throw new Exception($"Non esiste una sessione per l'UserID '{userID}'.");
            }

            // Ritorna l'ID della sessione come valore non nullo
            return sessionID.Value;
        }
        public int GetCartItemCount(int userID)
        {
            return _databaseService.QuerySingleOrDefault<int>(
                @"SELECT COALESCE(SUM(Quantity), 0) 
          FROM CartItems 
          INNER JOIN ShoppingSessions ON CartItems.SessionID = ShoppingSessions.SessionID
          WHERE ShoppingSessions.UserID = @UserID AND ShoppingSessions.Status = 'Active'",
                new { UserID = userID }
            );
        }
        public void UpdateCartItemParameters(int sessionId, int productId, DateTime deliveryDate, string recreation)
        {
            _databaseService.Execute(
                @"UPDATE CartItems
          SET DeliveryDate = @DeliveryDate,
              Recreation = @Recreation,
              UpdatedAt = GETDATE()
          WHERE SessionID = @SessionID AND ProductID = @ProductID",
                new { SessionID = sessionId, ProductID = productId, DeliveryDate = deliveryDate, Recreation = recreation }
            );
        }

        public class CartProduct
        {
            public string ProductName { get; set; }
            public string RemainingQuantity { get; set; }
            public string DiscountedPrice { get; set; }
        }


    }
}
