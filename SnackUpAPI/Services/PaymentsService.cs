using System;
using System.Collections.Generic;
using System.Linq;
using SnackUpAPI.Models;

namespace SnackUpAPI.Services
{
    public class PaymentService
    {
        private readonly DatabaseService _databaseService;

        public PaymentService(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        // Ottieni tutti i pagamenti
        public IEnumerable<Payment> GetAllPayments()
        {
            return _databaseService.Query<Payment>(
                "SELECT * FROM Payments WHERE Deleted IS NULL"
            );
        }

        // Ottieni un pagamento specifico
        public Payment GetPaymentById(int paymentId)
        {
            return _databaseService.QuerySingle<Payment>(
                "SELECT * FROM Payments WHERE PaymentID = @PaymentID AND Deleted IS NULL",
                new { PaymentID = paymentId }
            );
        }

        // Crea un nuovo pagamento
        public void CreatePayment(Payment payment)
        {
            _databaseService.Execute(
                @"INSERT INTO Payments (OrderID, SubscriptionID, Amount, PaymentMethod, PaymentDate, Status, Created) 
                  VALUES (@OrderID, @SubscriptionID, @Amount, @PaymentMethod, @PaymentDate, @Status, @Created)",
                new
                {
                    payment.OrderID,
                    payment.SubscriptionID,
                    payment.Amount,
                    payment.PaymentMethod,
                    payment.PaymentDate,
                    payment.Status, // Stato iniziale
                    Created = DateTime.UtcNow
                }
            );
        }

        // Aggiorna lo stato di un pagamento
        public void UpdatePaymentStatus(int paymentId, string status)
        {
            _databaseService.Execute(
                @"UPDATE Payments 
                  SET Status = @Status, Modified = @Modified 
                  WHERE PaymentID = @PaymentID",
                new { Status = status, Modified = DateTime.UtcNow, PaymentID = paymentId }
            );
        }

        // Elimina un pagamento (soft delete)
        public void DeletePayment(int paymentId)
        {
            _databaseService.Execute(
                @"UPDATE Payments 
                  SET Deleted = @Deleted 
                  WHERE PaymentID = @PaymentID",
                new { Deleted = DateTime.UtcNow, PaymentID = paymentId }
            );
        }
    }
}
