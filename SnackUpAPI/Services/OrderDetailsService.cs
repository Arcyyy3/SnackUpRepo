using System;
using System.Collections.Generic;
using SnackUpAPI.Models;

namespace SnackUpAPI.Services
{
    public class OrderDetailService
    {
        private readonly DatabaseService _databaseService;

        public OrderDetailService(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public IEnumerable<OrderDetail> GetAllOrderDetails()
        {
            return _databaseService.Query<OrderDetail>(
                "SELECT * FROM OrderDetails WHERE Deleted IS NULL"
            );
        }

        public IEnumerable<OrderDetail> GetOrderDetailsByOrderId(int orderId)
        {
            return _databaseService.Query<OrderDetail>(
                "SELECT * FROM OrderDetails WHERE OrderID = @OrderID AND Deleted IS NULL",
                new { OrderID = orderId }
            );
        }

        public void AddOrderDetail(OrderDetail orderDetail)
        {
            _databaseService.Execute(
                @"INSERT INTO OrderDetails (OrderID, ProductID, Quantity, UnitPrice, Created, Modified, Deleted) 
                  VALUES (@OrderID, @ProductID, @Quantity, @UnitPrice, @Created, NULL, NULL)",
                new
                {
                    orderDetail.OrderID,
                    orderDetail.ProductID,
                    orderDetail.Quantity,
                    orderDetail.UnitPrice,
                    Created = DateTime.UtcNow // Imposta Created come la data e ora correnti
                }
            );
        }

        public void UpdateOrderDetail(OrderDetail orderDetail)
        {
            _databaseService.Execute(
                @"UPDATE OrderDetails 
                  SET OrderID = @OrderID, ProductID = @ProductID, Quantity = @Quantity, 
                      UnitPrice = @UnitPrice, Modified = @Modified 
                  WHERE OrderDetailID = @OrderDetailID AND Deleted IS NULL",
                new
                {
                    orderDetail.OrderID,
                    orderDetail.ProductID,
                    orderDetail.Quantity,
                    orderDetail.UnitPrice,
                    Modified = DateTime.UtcNow, // Imposta Modified come la data e ora correnti
                    orderDetail.OrderDetailID
                }
            );
        }

        public void DeleteOrderDetail(int orderDetailId)
        {
            _databaseService.Execute(
                @"UPDATE OrderDetails 
                  SET Deleted = @Deleted 
                  WHERE OrderDetailID = @OrderDetailID",
                new
                {
                    Deleted = DateTime.UtcNow, // Imposta Deleted come la data e ora correnti
                    OrderDetailID = orderDetailId
                }
            );
        }
    }
}
