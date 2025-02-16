using System;
using System.Collections.Generic;
using SnackUpAPI.Models;
using SnackUpAPI.Data;

namespace SnackUpAPI.Services
{
    public class ShoppingSessionService
    {
        private readonly IDatabaseService _databaseService;

        public ShoppingSessionService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public IEnumerable<ShoppingSession> GetAllSessions()
        {
            return _databaseService.Query<ShoppingSession>(
                "SELECT * FROM ShoppingSessions WHERE Status = 'Active'"
            );
        }
        public ShoppingSession GetActiveSessionForUser(int userId)
        {
            return _databaseService.QuerySingleOrDefault<ShoppingSession>(
                "SELECT * FROM ShoppingSessions WHERE UserID = @UserID AND Status = 'Active'",
                new { UserID = userId }
            );
        }

        public ShoppingSession GetOrCreateActiveSessionForUser(int userId)
        {
            var session = GetActiveSessionForUser(userId);
            if (session == null)
            {
                var newSessionId = CreateSession(userId);
                return new ShoppingSession
                {
                    SessionID = newSessionId,
                    UserID = userId,
                    TotalAmount = 0.0m,
                    Status = "Active"
                };
            }

            return session;
        }


        public ShoppingSession GetSessionById(int sessionId)
        {
            return _databaseService.QuerySingle<ShoppingSession>(
                "SELECT * FROM ShoppingSessions WHERE SessionID = @SessionID",
                new { SessionID = sessionId }
            );
        }

        public int CreateSession(int? userId)
        {
            return _databaseService.ExecuteScalar<int>(
                @"INSERT INTO ShoppingSessions (UserID, TotalAmount, Status, CreatedAt)
                  OUTPUT INSERTED.SessionID
                  VALUES (@UserID, 0.00, 'Active', GETDATE())",
                new { UserID = userId }
            );
        }

        public void UpdateSessionTotal(int sessionId)
        {
            _databaseService.Execute(
                @"UPDATE ShoppingSessions
                  SET TotalAmount = (SELECT SUM(Total) FROM CartItems WHERE SessionID = @SessionID),
                      UpdatedAt = GETDATE()
                  WHERE SessionID = @SessionID",
                new { SessionID = sessionId }
            );
        }

        public void DeleteSession(int sessionId)
        {
            _databaseService.Execute(
                @"UPDATE ShoppingSessions 
                  SET Status = 'Cancelled', UpdatedAt = GETDATE()
                  WHERE SessionID = @SessionID",
                new { SessionID = sessionId }
            );
        }
        public void CompleteSession(int userId)
        {
            _databaseService.Execute(
                @"UPDATE ShoppingSessions 
          SET Status = 'Complete', UpdatedAt = GETDATE()
          WHERE UserID = @UserID AND Status = 'Active'",
                new { UserID = userId }
            );
        }

    }
}
