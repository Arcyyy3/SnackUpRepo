using System;
using System.Collections.Generic;
using SnackUpAPI.Models;

namespace SnackUpAPI.Services
{
    public class PromotionService
    {
        private readonly DatabaseService _databaseService;

        public PromotionService(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public IEnumerable<Promotion> GetAllPromotions()
        {
            return _databaseService.Query<Promotion>(
                "SELECT * FROM Promotions WHERE Deleted IS NULL"
            );
        }

        public IEnumerable<Promotion> GetActivePromotions()
        {
            return _databaseService.Query<Promotion>(
                "SELECT * FROM Promotions WHERE StartDate <= GETDATE() AND EndDate >= GETDATE() AND Deleted IS NULL"
            );
        }

        public bool IsPromotionOverlapping(DateTime startDate, DateTime endDate)
        {
            var result = _databaseService.QuerySingleOrDefault<int>(
                @"SELECT COUNT(1) 
                  FROM Promotions 
                  WHERE Deleted IS NULL 
                  AND StartDate <= @EndDate 
                  AND EndDate >= @StartDate",
                new { StartDate = startDate, EndDate = endDate }
            );

            return result > 0;
        }

        public void AddPromotion(Promotion promotion)
        {
            if (IsPromotionOverlapping(promotion.StartDate, promotion.EndDate))
            {
                throw new InvalidOperationException("Esiste già una promozione attiva in questo intervallo di date.");
            }

            _databaseService.Execute(
                @"INSERT INTO Promotions (Name, Description, DiscountPercentage, StartDate, EndDate, Created, Modified, Deleted) 
                  VALUES (@Name, @Description, @DiscountPercentage, @StartDate, @EndDate, @Created, NULL, NULL)",
                new
                {
                    promotion.Name,
                    promotion.Description,
                    promotion.DiscountPercentage,
                    StartDate = promotion.StartDate.ToUniversalTime(),
                    EndDate = promotion.EndDate.ToUniversalTime(),
                    Created = DateTime.UtcNow
                }
            );
        }

        public void UpdatePromotion(Promotion promotion)
        {
            if (IsPromotionOverlapping(promotion.StartDate, promotion.EndDate))
            {
                throw new InvalidOperationException("Esiste già una promozione attiva in questo intervallo di date.");
            }

            _databaseService.Execute(
                @"UPDATE Promotions 
                  SET Name = @Name, Description = @Description, DiscountPercentage = @DiscountPercentage, 
                      StartDate = @StartDate, EndDate = @EndDate, Modified = @Modified 
                  WHERE PromotionID = @PromotionID AND Deleted IS NULL",
                new
                {
                    promotion.Name,
                    promotion.Description,
                    promotion.DiscountPercentage,
                    StartDate = promotion.StartDate.ToUniversalTime(),
                    EndDate = promotion.EndDate.ToUniversalTime(),
                    Modified = DateTime.UtcNow,
                    promotion.PromotionID
                }
            );
        }

        public void DeletePromotion(int promotionId)
        {
            _databaseService.Execute(
                @"UPDATE Promotions 
                  SET Deleted = @Deleted 
                  WHERE PromotionID = @PromotionID",
                new
                {
                    Deleted = DateTime.UtcNow,
                    PromotionID = promotionId
                }
            );
        }
    }
}
