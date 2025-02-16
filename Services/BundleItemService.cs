using System;
using System.Collections.Generic;
using SnackUpAPI.Models;
using SnackUpAPI.Data;

namespace SnackUpAPI.Services
{
    public class BundleItemService
    {
        private readonly IDatabaseService _databaseService;

        public BundleItemService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public IEnumerable<BundleItem> GetAllBundleItems()
        {
            return _databaseService.Query<BundleItem>(
                "SELECT * FROM BundleItems WHERE Deleted IS NULL"
            );
        }

        public IEnumerable<BundleItem> GetItemsByBundleID(int bundleID)
        {
            return _databaseService.Query<BundleItem>(
                "SELECT * FROM BundleItems WHERE BundleID = @BundleID AND Deleted IS NULL",
                new { BundleID = bundleID }
            );
        }

        public BundleItem GetBundleItemById(int bundleItemID)
        {
            return _databaseService.QuerySingle<BundleItem>(
                "SELECT * FROM BundleItems WHERE BundleItemID = @BundleItemID AND Deleted IS NULL",
                new { BundleItemID = bundleItemID }
            );
        }

        public void AddBundleItem(BundleItem bundleItem)
        {
            _databaseService.Execute(
                @"INSERT INTO BundleItems (BundleID, ProductID, Quantity, Created, Modified, Deleted) 
                  VALUES (@BundleID, @ProductID, @Quantity, @Created, NULL, NULL)",
                new
                {
                    bundleItem.BundleID,
                    bundleItem.ProductID,
                    bundleItem.Quantity,
                    Created = DateTime.UtcNow
                }
            );
        }

        public void UpdateBundleItem(BundleItem bundleItem)
        {
            _databaseService.Execute(
                @"UPDATE BundleItems 
                  SET BundleID = @BundleID, ProductID = @ProductID, Quantity = @Quantity, 
                      Modified = @Modified 
                  WHERE BundleItemID = @BundleItemID AND Deleted IS NULL",
                new
                {
                    bundleItem.BundleID,
                    bundleItem.ProductID,
                    bundleItem.Quantity,
                    Modified = DateTime.UtcNow,
                    bundleItem.BundleItemID
                }
            );
        }

        public void DeleteBundleItem(int bundleItemID)
        {
            _databaseService.Execute(
                @"UPDATE BundleItems 
                  SET Deleted = @Deleted 
                  WHERE BundleItemID = @BundleItemID",
                new
                {
                    Deleted = DateTime.UtcNow,
                    BundleItemID = bundleItemID
                }
            );
        }
    }
}
