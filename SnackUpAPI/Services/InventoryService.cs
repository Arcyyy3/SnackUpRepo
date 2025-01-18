using System;
using System.Collections.Generic;
using System.Linq;
using SnackUpAPI.Models;

namespace SnackUpAPI.Services
{
    public class InventoryService
    {
        private readonly DatabaseService _databaseService;

        public InventoryService(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public IEnumerable<Inventory> GetAllInventories()
        {
            return _databaseService.Query<Inventory>(
                "SELECT * FROM Inventory WHERE Deleted IS NULL"
            );
        }

        public Inventory GetInventoryByProductID(int productID)
        {
            return _databaseService.QuerySingle<Inventory>(
                "SELECT * FROM Inventory WHERE ProductID = @ProductID AND Deleted IS NULL",
                new { ProductID = productID }
            );
        }

        public void UpdateInventory(int productID, int quantityChange, string reason, string changedBy)
        {
            var inventory = GetInventoryByProductID(productID);

            if (inventory == null)
                throw new InvalidOperationException("Inventory not found.");

            inventory.QuantityAvailable += quantityChange;
            inventory.LastUpdated = DateTime.UtcNow;

            _databaseService.Execute(
                @"UPDATE Inventory
                  SET QuantityAvailable = @QuantityAvailable, LastUpdated = @LastUpdated
                  WHERE ProductID = @ProductID",
                new
                {
                    inventory.QuantityAvailable,
                    inventory.LastUpdated,
                    ProductID = productID
                }
            );

            AddInventoryHistory(productID, quantityChange, reason, changedBy);
        }

        public void AddInventoryHistory(int productID, int changeAmount, string changeReason, string changedBy)
        {
            _databaseService.Execute(
                @"INSERT INTO InventoryHistory
                  (ProductID, ChangeAmount, ChangeReason, ChangedBy, Created, ChangeDate)
                  VALUES (@ProductID, @ChangeAmount, @ChangeReason, @ChangedBy, @Created, @ChangeDate)",
                new
                {
                    ProductID = productID,
                    ChangeAmount = changeAmount,
                    ChangeReason = changeReason,
                    ChangedBy = changedBy,
                    Created = DateTime.UtcNow,
                    ChangeDate = DateTime.UtcNow
                }
            );
        }
    }
}
