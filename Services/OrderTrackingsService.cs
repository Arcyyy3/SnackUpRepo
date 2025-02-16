using System;
using System.Collections.Generic;
using SnackUpAPI.Models;
using SnackUpAPI.Data;

namespace SnackUpAPI.Services
{
    public class OrderTrackingService
    {
        private readonly IDatabaseService _databaseService;

        public OrderTrackingService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public IEnumerable<OrderTracking> GetAllOrderTrackings()
        {
            return _databaseService.Query<OrderTracking>(
                "SELECT * FROM OrderTrackings WHERE Deleted IS NULL"
            );
        }

        public OrderTracking GetTrackingByOrderId(int orderId)
        {
            return _databaseService.QuerySingle<OrderTracking>(
                "SELECT * FROM OrderTrackings WHERE OrderID = @OrderID AND Deleted IS NULL",
                new { OrderID = orderId }
            );
        }

        public void AddOrderTracking(OrderTracking orderTracking)
        {
            _databaseService.Execute(
                @"INSERT INTO OrderTrackings (OrderID, Status, LastUpdated, Created, Modified, Deleted) 
                  VALUES (@OrderID, @Status, @LastUpdated, @Created, NULL, NULL)",
                new
                {
                    orderTracking.OrderID,
                    orderTracking.Status,
                    orderTracking.LastUpdated,
                    Created = DateTime.UtcNow // Imposta Created come la data e ora correnti
                }
            );
        }

        public void UpdateOrderTracking(OrderTracking orderTracking)
        {
            _databaseService.Execute(
                @"UPDATE OrderTrackings 
                  SET Status = @Status, LastUpdated = @LastUpdated, Modified = @Modified 
                  WHERE TrackingID = @TrackingID AND Deleted IS NULL",
                new
                {
                    orderTracking.Status,
                    orderTracking.LastUpdated,
                    Modified = DateTime.UtcNow, // Imposta Modified come la data e ora correnti
                    orderTracking.TrackingID
                }
            );
        }

        public void DeleteOrderTracking(int trackingId)
        {
            _databaseService.Execute(
                @"UPDATE OrderTrackings 
                  SET Deleted = @Deleted 
                  WHERE TrackingID = @TrackingID",
                new
                {
                    Deleted = DateTime.UtcNow, // Imposta Deleted come la data e ora correnti
                    TrackingID = trackingId
                }
            );
        }
    }
}
