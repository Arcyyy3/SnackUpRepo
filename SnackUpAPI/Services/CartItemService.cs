using System;
using System.Collections.Generic;
using SnackUpAPI.Models;

namespace SnackUpAPI.Services
{
    public class CartItemService
    {
        private readonly DatabaseService _databaseService;

        public CartItemService(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public IEnumerable<CartItem> GetItemsBySessionId(int sessionId)
        {
            return _databaseService.Query<CartItem>(
                "SELECT * FROM CartItems WHERE SessionID = @SessionID",
                new { SessionID = sessionId }
            );
        }
        public IEnumerable<CartItem> GetItemsBySessionIdAAA(int sessionId)
        {
            return _databaseService.Query<CartItem>(
                "SELECT * FROM CartItems WHERE SessionID = @SessionID",
                new { SessionID = sessionId }
            );
        }

        public double GetTotalPrice(int userID)
        {
            return _databaseService.QuerySingle<double>(
                "DECLARE @TotalSum DECIMAL(10, 2); SELECT @TotalSum = SUM(CI.Total) FROM CartItems CI INNER JOIN ShoppingSessions SS ON CI.SessionID = SS.SessionID WHERE SS.UserID = 9 AND CI.DeletedAt IS NULL; UPDATE ShoppingSessions SET TotalAmount = @TotalSum WHERE UserID = 9; SELECT @TotalSum AS TotalSum;",
                new { UserID = userID }
            );
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
                      INSERT (SessionID, ProductID, Quantity, Price, Total, CreatedAt)
                      VALUES (@SessionID, @ProductID, @Quantity, @Price, @Price * @Quantity, GETDATE());",
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
        "SELECT ProductID FROM Products WHERE Name = @ProductName",
        new { ProductName = cartName }
    );
            if (productId == null)
            {
                throw new Exception($"Il prodotto con nome '{cartName}' non esiste.");
            }
            int? sessionID = _databaseService.QuerySingleOrDefault<int?>(
                "SELECT SessionID FROM ShoppingSessions WHERE UserID = @UserID",
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




    }
}
