using System;
using System.Collections.Generic;
using SnackUpAPI.Models;
using SnackUpAPI.Data;

namespace SnackUpAPI.Services
{
    public class BundleProductService
    {
        private readonly IDatabaseService _databaseService;

        public BundleProductService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public IEnumerable<BundleProduct> GetAllBundles()
        {
            return _databaseService.Query<BundleProduct>(
                "SELECT * FROM BundleProducts WHERE Deleted IS NULL"
            );
        }

        public BundleProduct GetBundleById(int bundleID)
        {
            return _databaseService.QuerySingle<BundleProduct>(
                "SELECT * FROM BundleProducts WHERE BundleID = @BundleID AND Deleted IS NULL",
                new { BundleID = bundleID }
            );
        }

        public void AddBundle(BundleProduct bundle)
        {
            _databaseService.Execute(
                @"INSERT INTO BundleProducts (BundleName, Description, Price, Moment, Created, Modified, Deleted) 
                  VALUES (@BundleName, @Description, @Price, @Moment, @Created, NULL, NULL)",
                new
                {
                    bundle.BundleName,
                    bundle.Description,
                    bundle.Price,
                    bundle.Moment,
                    Created = DateTime.UtcNow
                }
            );
        }

        public void UpdateBundle(BundleProduct bundle)
        {
            _databaseService.Execute(
                @"UPDATE BundleProducts 
                  SET BundleName = @BundleName, Description = @Description, 
                      Price = @Price, Moment = @Moment, Modified = @Modified 
                  WHERE BundleID = @BundleID AND Deleted IS NULL",
                new
                {
                    bundle.BundleName,
                    bundle.Description,
                    bundle.Price,
                    bundle.Moment,
                    Modified = DateTime.UtcNow,
                    bundle.BundleID
                }
            );
        }

        public void DeleteBundle(int bundleID)
        {
            _databaseService.Execute(
                @"UPDATE BundleProducts 
                  SET Deleted = @Deleted 
                  WHERE BundleID = @BundleID",
                new
                {
                    Deleted = DateTime.UtcNow,
                    BundleID = bundleID
                }
            );
        }
    }
}
