using System.Collections.Generic;
using SnackUpAPI.Models;

namespace SnackUpAPI.Services
{
    public class SupportRequestService
    {
        private readonly DatabaseService _databaseService;

        public SupportRequestService(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public IEnumerable<SupportRequest> GetAllSupportRequests()
        {
            return _databaseService.Query<SupportRequest>(
                "SELECT * FROM SupportRequests WHERE Deleted IS NULL"
            );
        }

        public IEnumerable<SupportRequest> GetSupportRequestsByUserId(int userId)
        {
            return _databaseService.Query<SupportRequest>(
                "SELECT * FROM SupportRequests WHERE UserID = @UserID AND Deleted IS NULL",
                new { UserID = userId }
            );
        }

        public void AddSupportRequest(SupportRequest supportRequest)
        {
            _databaseService.Execute(
                @"INSERT INTO SupportRequests 
                  (UserID, OrderID, Subject, Description, Status, Created, Modified, Deleted) 
                  VALUES (@UserID, @OrderID, @Subject, @Description, @Status, @Created, NULL, NULL)",
                new
                {
                    supportRequest.UserID,
                    supportRequest.OrderID,
                    supportRequest.Subject,
                    supportRequest.Description,
                    supportRequest.Status,
                    Created = DateTime.UtcNow // Imposta Created come la data e ora correnti
                }
            );
        }

        public void UpdateSupportRequest(SupportRequest supportRequest)
        {
            _databaseService.Execute(
                @"UPDATE SupportRequests 
                  SET UserID = @UserID, OrderID = @OrderID, Subject = @Subject, 
                      Description = @Description, Status = @Status, Modified = @Modified 
                  WHERE SupportRequestID = @SupportRequestID AND Deleted IS NULL",
                new
                {
                    supportRequest.UserID,
                    supportRequest.OrderID,
                    supportRequest.Subject,
                    supportRequest.Description,
                    supportRequest.Status,
                    Modified = DateTime.UtcNow, // Imposta Modified come la data e ora correnti
                    supportRequest.SupportRequestID
                }
            );
        }

        public void DeleteSupportRequest(int supportRequestId)
        {
            _databaseService.Execute(
                @"UPDATE SupportRequests 
                  SET Deleted = @Deleted 
                  WHERE SupportRequestID = @SupportRequestID",
                new
                {
                    Deleted = DateTime.UtcNow, // Imposta Deleted come la data e ora correnti
                    SupportRequestID = supportRequestId
                }
            );
        }
        public IEnumerable<SupportRequest> GetSupportRequestsByStatus(string status)
        {
            return _databaseService.Query<SupportRequest>(
                "SELECT * FROM SupportRequests WHERE Status = @Status AND Deleted IS NULL",
                new { Status = status }
            );
        }

    }
}
