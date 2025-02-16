using System.Collections.Generic;
using SnackUpAPI.Models;
using SnackUpAPI.Data;

namespace SnackUpAPI.Services
{
    public class SubscriptionService
    {
        private readonly IDatabaseService _databaseService;

        public SubscriptionService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public IEnumerable<Subscription> GetAllSubscriptions()
        {
            return _databaseService.Query<Subscription>(
                "SELECT * FROM Subscriptions WHERE Deleted IS NULL"
            );
        }

        public IEnumerable<Subscription> GetSubscriptionsByUserId(int userId)
        {
            return _databaseService.Query<Subscription>(
                "SELECT * FROM Subscriptions WHERE UserID = @UserID AND Deleted IS NULL",
                new { UserID = userId }
            );
        }

        public void AddSubscription(Subscription subscription)
        {
            _databaseService.Execute(
                @"INSERT INTO Subscriptions 
                  (UserID, ProducerID, StartDate, EndDate, SubscriptionType, TotalPrice, Created, Modified, Deleted) 
                  VALUES (@UserID, @ProducerID, @StartDate, @EndDate, @SubscriptionType, @TotalPrice, @Created, NULL, NULL)",
                new
                {
                    subscription.UserID,
                    subscription.ProducerID,
                    subscription.StartDate,
                    subscription.EndDate,
                    subscription.SubscriptionType,
                    subscription.TotalPrice,
                    Created = DateTime.UtcNow // Imposta Created come la data e ora correnti
                }
            );
        }

        public void UpdateSubscription(Subscription subscription)
        {
            _databaseService.Execute(
                @"UPDATE Subscriptions 
                  SET UserID = @UserID, ProducerID = @ProducerID, StartDate = @StartDate, 
                      EndDate = @EndDate, SubscriptionType = @SubscriptionType, TotalPrice = @TotalPrice, 
                      Modified = @Modified 
                  WHERE SubscriptionID = @SubscriptionID AND Deleted IS NULL",
                new
                {
                    subscription.UserID,
                    subscription.ProducerID,
                    subscription.StartDate,
                    subscription.EndDate,
                    subscription.SubscriptionType,
                    subscription.TotalPrice,
                    Modified = DateTime.UtcNow, // Imposta Modified come la data e ora correnti
                    subscription.SubscriptionID
                }
            );
        }

        public void DeleteSubscription(int subscriptionId)
        {
            _databaseService.Execute(
                @"UPDATE Subscriptions 
                  SET Deleted = @Deleted 
                  WHERE SubscriptionID = @SubscriptionID",
                new
                {
                    Deleted = DateTime.UtcNow, // Imposta Deleted come la data e ora correnti
                    SubscriptionID = subscriptionId
                }
            );
        }
    }
}
