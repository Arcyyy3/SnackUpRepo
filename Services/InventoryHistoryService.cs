using System;
using System.Collections.Generic;
using SnackUpAPI.Models;
using SnackUpAPI.Data;

namespace SnackUpAPI.Services
{
    public class InventoryHistoryService
    {
        private readonly IDatabaseService _databaseService;

        public InventoryHistoryService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public IEnumerable<InventoryHistory> GetAllHistory()
        {
            return _databaseService.Query<InventoryHistory>(
                "SELECT * FROM InventoryHistory WHERE Deleted IS NULL"
            );
        }

        public IEnumerable<InventoryHistory> GetHistoryByProductID(int productID)
        {
            return _databaseService.Query<InventoryHistory>(
                "SELECT * FROM InventoryHistory WHERE ProductID = @ProductID AND Deleted IS NULL",
                new { ProductID = productID }
            );
        }

        public InventoryHistory GetHistoryByID(int historyID)
        {
            return _databaseService.QuerySingle<InventoryHistory>(
                "SELECT * FROM InventoryHistory WHERE HistoryID = @HistoryID AND Deleted IS NULL",
                new { HistoryID = historyID }
            );
        }

        public void AddHistory(InventoryHistory history)
        {
            _databaseService.Execute(
                @"INSERT INTO InventoryHistory
                  (ProductID, ChangeAmount, ChangeReason, ChangedBy, Created, ChangeDate)
                  VALUES (@ProductID, @ChangeAmount, @ChangeReason, @ChangedBy, @Created, @ChangeDate)",
                new
                {
                    history.ProductID,
                    history.ChangeAmount,
                    history.ChangeReason,
                    history.ChangedBy,
                    Created = DateTime.UtcNow,
                    ChangeDate = DateTime.UtcNow
                }
            );
        }

        public void SoftDeleteHistory(int historyID)
        {
            _databaseService.Execute(
                @"UPDATE InventoryHistory
                  SET Deleted = @Deleted
                  WHERE HistoryID = @HistoryID",
                new
                {
                    Deleted = DateTime.UtcNow,
                    HistoryID = historyID
                }
            );
        }
    }
}
