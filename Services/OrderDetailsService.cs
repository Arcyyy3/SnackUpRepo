using System;
using System.Collections.Generic;
using SnackUpAPI.Models;
using SnackUpAPI.Data;

namespace SnackUpAPI.Services
{
    public class OrderDetailService
    {
        private readonly IDatabaseService _databaseService;

        public OrderDetailService(IDatabaseService databaseService)
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
        public double GetTotalPrice(DateTime date)
        {
            const string query = @"
        DECLARE @CurrentDate DATE = @Date;
        SELECT 
            ISNULL(SUM(OD.UnitPrice * OD.Quantity), 0) AS TotalAmount -- Somma totale
        FROM 
            OrderDetails AS OD
        WHERE 
            OD.DeliveryDate = @CurrentDate";

            // Restituisce il risultato della query
            return _databaseService.QuerySingle<double>(query, new { Date = date });
        }
        public IEnumerable<ClassesDetailsOrder> GetDetailsProducer(DateTime date, string recreation)
        {
            const string query = @"
        DECLARE @CurrentDate DATE = @Date; 

        SELECT 
            SC.ClassYear,
            SC.ClassSection,
            C.CategoryName,
            P.ProductName,
            P.PhotoLinkProdotto,
            SUM(OD.Quantity) AS TotalQuantity -- Somma totale delle quantità
        FROM OrderDetails AS OD
        INNER JOIN Orders AS O ON OD.OrderID = O.OrderID
        INNER JOIN CategoryProducts CP ON OD.ProductID = CP.ProductID
        INNER JOIN Categories C ON CP.CategoryID = C.CategoryID
        INNER JOIN SchoolClasses SC ON O.SchoolClassID = SC.SchoolClassID
        INNER JOIN Products P ON OD.ProductID = P.ProductID
        WHERE 
            OD.DeliveryDate = @CurrentDate 
            AND C.CategoryID IN (1, 2, 3, 4, 5, 6, 7,10) AND OD.Recreation=@Recreation
        GROUP BY 
            SC.ClassYear, 
            SC.ClassSection, 
            C.CategoryName, 
            P.ProductName, 
            P.PhotoLinkProdotto
        ORDER BY 
            SC.ClassYear, SC.ClassSection, C.CategoryName, P.ProductName;
    ";

            var rawResults = _databaseService.Query<ProducerOrderPage>(query, new { Date = date, Recreation = recreation });

            // Organizza i dati per ClassYear e ClassSection
            var groupedResults = rawResults
                .GroupBy(r => new { r.ClassYear, r.ClassSection }) // Raggruppa per anno e sezione
                .Select(classGroup => new ClassesDetailsOrder
                {
                    ClassYear = classGroup.Key.ClassYear,
                    ClassSection = classGroup.Key.ClassSection,
                    Categories = classGroup
                        .GroupBy(r => r.CategoryName) // Raggruppa per categoria
                        .Select(categoryGroup => new CategoryDetailsOrder
                        {
                            CategoryName = categoryGroup.Key,
                            Products = categoryGroup
                                .Select(product => new ProductDetailsOrder
                                {
                                    ProductName = product.ProductName,
                                    PhotoLinkProdotto = product.PhotoLinkProdotto,
                                    TotalQuantity = product.TotalQuantity
                                })
                                .ToList()
                        })
                        .ToList()
                })
                .ToList();

            return groupedResults;
        }
        public IEnumerable<ClassesDetailsOrder> GetDetailsProducerAll(DateTime date)
        {
            const string query = @"
        DECLARE @CurrentDate DATE = @Date; 

        SELECT 
            SC.ClassYear,
            SC.ClassSection,
            C.CategoryName,
            P.ProductName,
            P.PhotoLinkProdotto,
            SUM(OD.Quantity) AS TotalQuantity -- Somma totale delle quantità
        FROM OrderDetails AS OD
        INNER JOIN Orders AS O ON OD.OrderID = O.OrderID
        INNER JOIN CategoryProducts CP ON OD.ProductID = CP.ProductID
        INNER JOIN Categories C ON CP.CategoryID = C.CategoryID
        INNER JOIN SchoolClasses SC ON O.SchoolClassID = SC.SchoolClassID
        INNER JOIN Products P ON OD.ProductID = P.ProductID
        WHERE 
            OD.DeliveryDate = @CurrentDate 
            AND C.CategoryID < 6
        GROUP BY 
            SC.ClassYear, 
            SC.ClassSection, 
            C.CategoryName, 
            P.ProductName, 
            P.PhotoLinkProdotto
        ORDER BY 
            SC.ClassYear, SC.ClassSection, C.CategoryName, P.ProductName;
    ";

            var rawResults = _databaseService.Query<ProducerOrderPage>(query, new { Date = date });

            // Organizza i dati per ClassYear e ClassSection
            var groupedResults = rawResults
                .GroupBy(r => new { r.ClassYear, r.ClassSection }) // Raggruppa per anno e sezione
                .Select(classGroup => new ClassesDetailsOrder
                {
                    ClassYear = classGroup.Key.ClassYear,
                    ClassSection = classGroup.Key.ClassSection,
                    Categories = classGroup
                        .GroupBy(r => r.CategoryName) // Raggruppa per categoria
                        .Select(categoryGroup => new CategoryDetailsOrder
                        {
                            CategoryName = categoryGroup.Key,
                            Products = categoryGroup
                                .Select(product => new ProductDetailsOrder
                                {
                                    ProductName = product.ProductName,
                                    PhotoLinkProdotto = product.PhotoLinkProdotto,
                                    TotalQuantity = product.TotalQuantity
                                })
                                .ToList()
                        })
                        .ToList()
                })
                .ToList();

            return groupedResults;
        }
        public IEnumerable<ProductCodeDetailsOrderPage> GetProductCodesByClassAndDate(DateTime date, int classYear, string classSection, string productName)
        {
            const string query = @"
            SELECT 
                OD.ProductCode,
                OD.Quantity
            FROM OrderDetails AS OD
            INNER JOIN Orders AS O ON OD.OrderID = O.OrderID
            INNER JOIN Products AS P ON OD.ProductID = P.ProductID
            INNER JOIN SchoolClasses AS SC ON O.SchoolClassID = SC.SchoolClassID
            WHERE 
                OD.DeliveryDate = @Date
                AND SC.ClassYear = @ClassYear
                AND SC.ClassSection = @ClassSection
                AND P.ProductName = @ProductName
            ORDER BY OD.ProductCode;";

            return _databaseService.Query<ProductCodeDetailsOrderPage>(query, new
            {
                Date = date,
                ClassYear = classYear,
                ClassSection = classSection,
                ProductName = productName
            });
        }

        public void AddOrderDetail(OrderDetail orderDetail)
        {
            _databaseService.Execute(
                @"INSERT INTO OrderDetails (OrderID, ProductID, Quantity, UnitPrice, DeliveryDate, Recreation, ProductCode, Created, Modified, Deleted) 
                  VALUES (@OrderID, @ProductID, @Quantity, @UnitPrice, @DeliveryDate, @Recreation, @ProductCode, @Created, NULL, NULL)",
                new
                {
                    orderDetail.OrderID,
                    orderDetail.ProductID,
                    orderDetail.Quantity,
                    orderDetail.UnitPrice,
                    orderDetail.DeliveryDate,
                    orderDetail.Recreation,
                    orderDetail.ProductCode,
                    Created = DateTime.UtcNow
                }
            );
        }

        public void UpdateOrderDetail(OrderDetail orderDetail)
        {
            _databaseService.Execute(
                @"UPDATE OrderDetails 
                  SET OrderID = @OrderID, ProductID = @ProductID, Quantity = @Quantity, 
                      UnitPrice = @UnitPrice, DeliveryDate = @DeliveryDate, Recreation = @Recreation, 
                      ProductCode = @ProductCode, Modified = @Modified 
                  WHERE DetailID = @DetailID AND Deleted IS NULL",
                new
                {
                    orderDetail.OrderID,
                    orderDetail.ProductID,
                    orderDetail.Quantity,
                    orderDetail.UnitPrice,
                    orderDetail.DeliveryDate,
                    orderDetail.Recreation,
                    orderDetail.ProductCode,
                    Modified = DateTime.UtcNow,
                    orderDetail.DetailID
                }
            );
        }

        public void DeleteOrderDetail(int orderDetailId)
        {
            _databaseService.Execute(
                @"UPDATE OrderDetails 
                  SET Deleted = @Deleted 
                  WHERE DetailID = @DetailID",
                new
                {
                    Deleted = DateTime.UtcNow,
                    DetailID = orderDetailId
                }
            );
        }
        public class ClassesDetailsOrder
        {
            public string ClassYear { get; set; }
            public string ClassSection { get; set; }
            public List<CategoryDetailsOrder> Categories { get; set; }
        }

        public class CategoryDetailsOrder
        {
            public string CategoryName { get; set; }
            public List<ProductDetailsOrder> Products { get; set; }
        }

        public class ProductDetailsOrder
        {
            public string ProductName { get; set; }
            public string PhotoLinkProdotto { get; set; }
            public int TotalQuantity { get; set; }
        }

        public class ProducerOrderPage
        {
            public string ClassYear { get; set; }
            public string ClassSection { get; set; }
            public string CategoryName { get; set; }
            public string ProductName { get; set; }
            public string PhotoLinkProdotto { get; set; }
            public int TotalQuantity { get; set; }
        }
        public class ProductCodeDetailsOrderPage
        {
            public string ProductCode { get; set; }
            public int Quantity { get; set; }
        }
    }
}

