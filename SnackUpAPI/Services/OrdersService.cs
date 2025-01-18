using System;
using System.Collections.Generic;
using System.Diagnostics;
using SnackUpAPI.Models;

namespace SnackUpAPI.Services
{
    public class OrderService
    {
        private readonly DatabaseService _databaseService;

        public OrderService(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public IEnumerable<Order> GetAllOrders()
        {
            return _databaseService.Query<Order>(
                "SELECT * FROM Orders WHERE Deleted IS NULL"
            );
        }


        /*
        public List<OrderCollection> GetOrdersBySchoolClassAndMoment(int schoolClassID, string moment)
        {
            var rawData = _databaseService.Query<dynamic>(
                @"SELECT 
            o.OrderID,
            o.OrderDate,
            o.Status,
            o.TotalPrice,
            u.Name AS StudentName,
            u.Surname AS StudentSurname,
            p.ProductID,
            p.Name AS ProductName,
            od.Quantity,
            od.UnitPrice,
            od.TotalPrice AS ProductTotalPrice
          FROM 
            Orders o
          INNER JOIN 
            Users u ON o.UserID = u.UserID
          INNER JOIN 
            OrderDetails od ON o.OrderID = od.OrderID
          INNER JOIN 
            Products p ON od.ProductID = p.ProductID
          WHERE 
            o.SchoolClassID = @SchoolClassID
            AND o.Recreation = @Moment
            AND o.Deleted IS NULL;",
                new { SchoolClassID = schoolClassID, Moment = moment }
            );

            return rawData
                .GroupBy(r => new { r.OrderID, r.OrderDate, r.Status, r.TotalPrice, r.StudentName, r.StudentSurname })
                .Select(g => new OrderCollection
                {
                    OrderID = g.Key.OrderID,
                    OrderDate = g.Key.OrderDate,
                    Status = g.Key.Status,
                    TotalPrice = g.Key.TotalPrice,
                    StudentName = g.Key.StudentName,
                    StudentSurname = g.Key.StudentSurname,
                    Items = g.Select(i => new OrderDetailCollection
                    {
                        ProductID = i.ProductID,
                        ProductName = i.ProductName,
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice,
                        TotalPrice = i.ProductTotalPrice
                    }).ToList()
           
                }).ToList();
        }
        */
        public List<OrderCollection> GetOrdersBySchoolClassAndMoment(int schoolClassID, string moment)
        {
            try
            {
                var orders = _databaseService.Query<OrderCollection>(
                    @"SELECT 
                        o.OrderID,
                        o.OrderDate,
                        o.Status,
                        o.TotalPrice,
                        u.Name AS StudentName,
                        u.Surname AS StudentSurname
                      FROM Orders o
                      INNER JOIN Users u ON o.UserID = u.UserID
                      WHERE o.SchoolClassID = @SchoolClassID 
                        AND o.Recreation = @Moment
                        AND o.Deleted IS NULL",
                    new { SchoolClassID = schoolClassID, Moment = moment }
                ).ToList();

                foreach (var order in orders)
                {
                    var orderItems = _databaseService.Query<OrderDetailCollection>(
                        @"SELECT 
                            od.ProductID,
                            p.Name AS ProductName,
                            od.Quantity,
                            od.UnitPrice,
                            od.TotalPrice
                          FROM OrderDetails od
                          INNER JOIN Products p ON od.ProductID = p.ProductID
                          WHERE od.OrderID = @OrderID 
                            AND od.Deleted IS NULL",
                        new { OrderID = order.OrderID }
                    ).ToList();

                    order.Items = orderItems;
                }

                return orders;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore durante il recupero degli ordini per la classe {schoolClassID} e il momento {moment}: {ex.Message}");
                throw;
            }
        }
    }
    public List<OrderCollection> GetOrdersBySchoolClassAndMoment(int schoolClassID, string moment)
        {
            var orders = _databaseService.Query<OrderCollection>(
                @"SELECT 
                o.OrderID,
                o.OrderDate,
                o.Status,
                o.TotalPrice,
                u.Name AS StudentName,
                u.Surname AS StudentSurname
              FROM 
                Orders o
              INNER JOIN 
                Users u ON o.UserID = u.UserID
              WHERE 
                o.SchoolClassID = @SchoolClassID 
            AND o.Recreation = @Moment
                AND o.Deleted IS NULL",
                new { SchoolClassID = schoolClassID, Moment = moment }
            ).ToList();

            foreach (var order in orders)
            {
                var orderItems = _databaseService.Query<OrderItemCollection>(
                    @"SELECT 
                    od.ProductID,
                    p.Name AS ProductName,
                    od.Quantity,
                    od.UnitPrice,
                    od.TotalPrice
                  FROM 
                    OrderDetails od
                  INNER JOIN 
                    Products p ON od.ProductID = p.ProductID
                  WHERE 
                    od.OrderID = @OrderID 
                    AND od.Deleted IS NULL",
                    new { OrderID = order.OrderID }
                ).ToList();

                order.Items = orderItems;
            }

            return orders;
        }
      
        public int GetSchoolClassIDByUserID(int userID)
        {
            return _databaseService.QuerySingle<int>(
                @"SELECT 
                SchoolClassID
              FROM 
                Users
              WHERE 
                UserID = @UserID
                AND Deleted IS NULL;",
                new { UserID = userID }
            );
        }

        public void AddOrder(Order order)
        {
            _databaseService.Execute(
                @"INSERT INTO Orders (UserID, ProducerID, SchoolClassID, OrderDate, recreation,Status, TotalPrice, CancellationReason, Created, Modified, Deleted) 
                  VALUES (@UserID, @ProducerID, @SchoolClassID, @OrderDate,@recreation, @Status, @TotalPrice, @CancellationReason, @Created, NULL, NULL)",
                new
                {
                    order.UserID,
                    order.ProducerID,
                    order.SchoolClassID, // Aggiunto SchoolClassID
                    order.OrderDate,
                    order.recreation,
                    order.Status,
                    order.TotalPrice,
                    order.CancellationReason,
                    Created = DateTime.UtcNow // Imposta Created come la data e ora correnti
                }
            );
        }
        public Order GetOrderById(int id)
{
    try
    {
        var order = _databaseService.QuerySingle<Order>(
            "SELECT * FROM Orders WHERE OrderID = @OrderID AND Deleted IS NULL",
            new { OrderID = id }
        );

        if (order == null)
            throw new KeyNotFoundException($"Order with ID {id} not found.");

        return order;
    }
    catch (Exception ex)
    {
        Debug.WriteLine($"Error retrieving order by ID {id}: {ex.Message}");
        throw;
    }
}


        public static string GetCurrentMoment()
        {
            var now = DateTime.Now.TimeOfDay;

            if (now >= new TimeSpan(10, 0, 0) && now <= new TimeSpan(10, 20, 0))
                return "FirstBreak";
            if (now >= new TimeSpan(12, 0, 0) && now <= new TimeSpan(12, 20, 0))
                return "SecondBreak";
            if (now >= new TimeSpan(13, 5, 0) && now <= new TimeSpan(13, 40, 0))
                return "Lunch";

            return "OutsideHours";
        }

        public void UpdateOrder(Order order)
        {
            _databaseService.Execute(
                @"UPDATE Orders 
                  SET UserID = @UserID, ProducerID = @ProducerID, SchoolClassID = @SchoolClassID, OrderDate = @OrderDate, 
                      Status = @Status, TotalPrice = @TotalPrice, CancellationReason = @CancellationReason, 
                      Modified = @Modified 
                  WHERE OrderID = @OrderID AND Deleted IS NULL",
                new
                {
                    order.UserID,
                    order.ProducerID,
                    order.SchoolClassID, // Aggiunto SchoolClassID
                    order.OrderDate,
                    order.Status,
                    order.TotalPrice,
                    order.CancellationReason,
                    Modified = DateTime.UtcNow, // Imposta Modified come la data e ora correnti
                    order.OrderID
                }
            );
        }

        public void DeleteOrder(int id)
        {
            _databaseService.Execute(
                @"UPDATE Orders 
                  SET Deleted = @Deleted 
                  WHERE OrderID = @OrderID",
                new
                {
                    Deleted = DateTime.UtcNow, // Imposta Deleted come la data e ora correnti
                    OrderID = id
                }
            );
        }
        public IEnumerable<dynamic> GetGroupedOrdersByUserId(int userId)
        {
            var query = @"
        SELECT 
            o.OrderID,
            o.UserID,
            o.Created,
            od.DeliveryDate,
            od.Recreation,
            od.ProductID,
            p.Name AS ProductName,
            od.Quantity,
            od.UnitPrice,
            p.PhotoLink
        FROM Orders o
        INNER JOIN OrderDetails od ON o.OrderID = od.OrderID
        INNER JOIN Products p ON od.ProductID = p.ProductID
        WHERE o.UserID = @UserID AND o.Deleted IS NULL
        ORDER BY od.DeliveryDate, od.Recreation";

            var rawResults = _databaseService.Query<OrderRawResult>(query, new { UserID = userId });

            return rawResults
                .GroupBy(r => new { r.DeliveryDate, r.Recreation })
                .Select(group => new
                {
                    Date = group.Key.DeliveryDate,
                    Recreation = group.Key.Recreation,
                    Orders = group
                        .GroupBy(o => o.OrderID)
                        .Select(orderGroup => new
                        {
                            OrderID = orderGroup.Key,
                            Products = orderGroup.Select(o => new
                            {
                                o.ProductID,
                                o.ProductName,
                                o.Quantity,
                                o.UnitPrice,
                                o.PhotoLink
                            }).ToList(),
                            TotalPrice = orderGroup.Sum(o => o.Quantity * o.UnitPrice)
                        }).ToList()
                }).ToList();
        }

        public class OrderCollection
        {
            public int OrderID { get; set; }
            public DateTime OrderDate { get; set; }
            public string Status { get; set; }
            public decimal TotalPrice { get; set; }
            public string StudentName { get; set; }
            public string StudentSurname { get; set; }
            public List<OrderItemCollection> Items { get; set; }
        }
    public class OrderDetailCollection
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
    public class OrderItemCollection
        {
            public int ProductID { get; set; }
            public string ProductName { get; set; }
            public int Quantity { get; set; }
            public decimal UnitPrice { get; set; }
            public decimal TotalPrice { get; set; }
        }
        public class OrderRawResult
        {
            public int OrderID { get; set; }
            public int UserID { get; set; }
            public DateTime OrderDate { get; set; }
            public DateTime DeliveryDate { get; set; } // Aggiunta della proprietà DeliveryDate
            public string Recreation { get; set; }
            public int ProductID { get; set; }
            public string ProductName { get; set; }
            public int Quantity { get; set; }
            public decimal UnitPrice { get; set; }
            public string PhotoLink { get; set; }
        }


    }
}
