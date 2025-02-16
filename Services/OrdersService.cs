using System;
using System.Collections.Generic;
using System.Diagnostics;
using SnackUpAPI.Models;
using System.Text.Json;
using SnackUpAPI.Data;

namespace SnackUpAPI.Services
{
    public class OrderService
    {
        private readonly IDatabaseService _databaseService;

        public OrderService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public IEnumerable<Order> GetAllOrders()
        {
            return _databaseService.Query<Order>(
                "SELECT * FROM Orders WHERE Deleted IS NULL"
            );
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
        public int AddOrder(Order order)
        {
            return _databaseService.QuerySingle<int>(
                @"INSERT INTO Orders (UserID, SchoolClassID, Status, TotalPrice, Created, Modified, Deleted) 
          VALUES (@UserID, @SchoolClassID, @Status, @TotalPrice, @Created, NULL, NULL);
          SELECT CAST(SCOPE_IDENTITY() AS INT);",
                new
                {
                    order.UserID,
                    order.SchoolClassID,
                    order.Status,
                    order.TotalPrice,
                    Created = DateTime.UtcNow
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
                  SET UserID = @UserID, SchoolClassID = @SchoolClassID, 
                      Status = @Status, TotalPrice = @TotalPrice,
                      Modified = @Modified 
                  WHERE OrderID = @OrderID AND Deleted IS NULL",
                new
                {
                    order.UserID,
                    order.SchoolClassID, // Aggiunto SchoolClassID
                    order.Status,
                    order.TotalPrice,
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
      p.ProductName AS ProductName,
      od.Quantity,
      od.UnitPrice,
      p.PhotoLinkProdotto
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
                                o.PhotoLinkProdotto
                            }).ToList(),
                            TotalPrice = orderGroup.Sum(o => o.Quantity * o.UnitPrice)
                        }).ToList()
                }).ToList();
        }
        public IEnumerable<dynamic> GetOrdersByUserIdAndCurrentRecreation(int userId)
        {
            var query = @"DECLARE @CurrentDate DATE = CAST(GETDATE() AS DATE);
DECLARE @CurrentTime TIME = CAST(GETDATE() AS TIME);

-- Controlla se l'utente dato è un MasterStudent
IF EXISTS (
    SELECT 1 
    FROM Users 
    WHERE UserID = @UserID AND Role = 'MasterStudent'
)
BEGIN
    SELECT 
        o.OrderID,
        o.Created AS OrderDate,
        o.Status,
        o.TotalPrice AS OrderTotalPrice,
        u.UserName AS StudentName,
        u.Surname AS StudentSurname,
        od.ProductID,
        p.ProductName AS ProductName,
        od.Quantity,
        od.UnitPrice,
        (od.Quantity * od.UnitPrice) AS ProductTotalPrice,
        CASE 
            WHEN @CurrentTime BETWEEN '09:00:00' AND '10:20:00' THEN 'First'
            WHEN @CurrentTime BETWEEN '11:00:00' AND '12:20:00' THEN 'Second'
            ELSE 'Other'
        END AS Recreation
    FROM Orders o
    INNER JOIN Users u ON o.UserID = u.UserID
    INNER JOIN OrderDetails od ON o.OrderID = od.OrderID
    INNER JOIN Products p ON od.ProductID = p.ProductID
    WHERE u.SchoolClassID = (SELECT SchoolClassID FROM Users WHERE UserID = @UserID)
      AND od.DeliveryDate = @CurrentDate
      AND (
          (@CurrentTime BETWEEN '09:00:00' AND '10:20:00' AND 'First' = 'First') OR
          (@CurrentTime BETWEEN '11:00:00' AND '12:20:00' AND 'Second' = 'Second')
      )
    ORDER BY o.OrderID, od.ProductID;
END
ELSE
BEGIN
    -- Restituisce un set vuoto se l'utente non è un MasterStudent
    SELECT NULL AS OrderID, NULL AS OrderDate, NULL AS Status, 
           NULL AS OrderTotalPrice, NULL AS StudentName, NULL AS StudentSurname,
           NULL AS ProductID, NULL AS ProductName, NULL AS Quantity, 
           NULL AS UnitPrice, NULL AS ProductTotalPrice, NULL AS Recreation
    WHERE 1 = 0;
END;
    ";

            // Esegui la query
            var rawResults = _databaseService.Query<OrderRawResultOrder>(query, new { UserID = userId });
            Debug.WriteLine($"RISULTATI::::{rawResults}");
            // Verifica risultati grezzi
            if (rawResults == null || !rawResults.Any())
            {
                Console.WriteLine("No raw results returned from query.");
                return Enumerable.Empty<dynamic>();
            }

            // Log dei risultati grezzi
            Console.WriteLine($"Raw Results: {JsonSerializer.Serialize(rawResults)}");

            // Raggruppamento dei risultati
            return rawResults
                .GroupBy(r => r.OrderID)
                .Select(orderGroup => new
                {
                    OrderID = orderGroup.Key,
                    OrderDate = orderGroup.First().OrderDate,
                    Status = orderGroup.First().Status,
                    TotalPrice = orderGroup.First().OrderTotalPrice,
                    StudentName = orderGroup.First().StudentName,
                    StudentSurname = orderGroup.First().StudentSurname,
                    Items = orderGroup
                        .Where(item => item.ProductID != null) // Escludi eventuali risultati vuoti
                        .Select(item => new
                        {
                            item.ProductID,
                            item.ProductName,
                            item.Quantity,
                            item.UnitPrice,
                            TotalPrice = item.ProductTotalPrice
                        }).ToList()
                }).ToList();
        }


        public class OrderCollection
        {
            public int OrderID { get; set; }
            public DateTime OrderDate { get; set; }
            public string Status { get; set; }
            public decimal OrderTotalPrice { get; set; }
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
        public decimal ProductTotalPrice { get; set; }
        public string Recreation { get; set; } // Aggiunto per rappresentare la ricreazione

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
            public string PhotoLinkProdotto { get; set; }
        }

        public class OrderRawResultOrder
        {
            public int OrderID { get; set; }
            public DateTime OrderDate { get; set; }
            public string Status { get; set; }
            public decimal OrderTotalPrice { get; set; }
            public string StudentName { get; set; }
            public string StudentSurname { get; set; }
            public int ProductID { get; set; }
            public string ProductName { get; set; }
            public int Quantity { get; set; }
            public decimal UnitPrice { get; set; }
            public decimal ProductTotalPrice { get; set; } // Cambia il nome per evitare conflitti
            public string Recreation { get; set; } // Aggiunto per rappresentare la ricreazione
        }

    }
}
