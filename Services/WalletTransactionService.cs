using System;
using System.Collections.Generic;
using SnackUpAPI.Models;
using SnackUpAPI.Data;

namespace SnackUpAPI.Services
{
    public class WalletTransactionService
    {
        private readonly IDatabaseService _databaseService;

        public WalletTransactionService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        // Ottieni tutte le transazioni
        public IEnumerable<WalletTransaction> GetAllTransactions()
        {
            return _databaseService.Query<WalletTransaction>(
                "SELECT * FROM WalletTransactions WHERE Deleted IS NULL"
            );
        }

        // Ottieni una transazione per ID
        public WalletTransaction GetTransactionById(int transactionId)
        {
            return _databaseService.QuerySingle<WalletTransaction>(
                "SELECT * FROM WalletTransactions WHERE WalletTransactionID = @TransactionID AND Deleted IS NULL",
                new { TransactionID = transactionId }
            );
        }

        // Aggiungi una nuova transazione
        public void AddTransaction(WalletTransaction transaction)
        {
            _databaseService.Execute(
                @"INSERT INTO WalletTransactions (WalletID, Amount, TransactionType, Created) 
                  VALUES (@WalletID, @Amount, @TransactionType, @Created)",
                new
                {
                    transaction.WalletID,
                    transaction.Amount,
                    transaction.TransactionType,
                    Created = DateTime.UtcNow
                }
            );
        }

        // Cancella logicamente una transazione
        public void DeleteTransaction(int transactionId)
        {
            _databaseService.Execute(
                @"UPDATE WalletTransactions SET Deleted = @Deleted WHERE WalletTransactionID = @TransactionID",
                new
                {
                    Deleted = DateTime.UtcNow,
                    TransactionID = transactionId
                }
            );
        }

        // Ottieni tutte le transazioni per un WalletID specifico
        public IEnumerable<WalletTransaction> GetTransactionsByWalletId(int walletId)
        {
            return _databaseService.Query<WalletTransaction>(
                "SELECT * FROM WalletTransactions WHERE WalletID = @WalletID AND Deleted IS NULL",
                new { WalletID = walletId }
            );
        }
    }
}
