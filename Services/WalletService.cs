using System;
using System.Collections.Generic;
using SnackUpAPI.Models;
using SnackUpAPI.Data;

namespace SnackUpAPI.Services
{
    public class WalletService
    {
        private readonly IDatabaseService _databaseService;

        public WalletService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }
        public Wallet GetWalletByUserId(int userId)
        {
            return _databaseService.QuerySingleOrDefault<Wallet>(
                "SELECT * FROM Wallets WHERE UserID = @UserID AND Deleted IS NULL",
                new { UserID = userId }
            );
        }


        public IEnumerable<WalletTransaction> GetTransactionsByWalletId(int walletId)
        {
            return _databaseService.Query<WalletTransaction>(
                "SELECT * FROM WalletTransactions WHERE WalletID = @WalletID AND Deleted IS NULL ORDER BY TransactionDate DESC",
                new { WalletID = walletId }
            );
        }

        public void AddFunds(int walletId, decimal amount, string description)
        {
            var wallet = GetWalletByWalletId(walletId);
            if (wallet == null)
                throw new Exception("Wallet not found.");

            _databaseService.Execute(
                "UPDATE Wallets SET Balance = Balance + @Amount, Modified = @Modified WHERE WalletID = @WalletID",
                new { Amount = amount, Modified = DateTime.UtcNow, WalletID = walletId }
            );

            _databaseService.Execute(
                @"INSERT INTO WalletTransactions (WalletID, Amount, TransactionType, Description, TransactionDate, Created) 
              VALUES (@WalletID, @Amount, @TransactionType, @Description, @TransactionDate, @Created)",
                new
                {
                    WalletID = walletId,
                    Amount = amount,
                    TransactionType = "Deposit",
                    Description = description,
                    TransactionDate = DateTime.UtcNow,
                    Created = DateTime.UtcNow
                }
            );
        }

        public void DeductFunds(int walletId, decimal amount, string description)
        {
            var wallet = GetWalletByWalletId(walletId);
            if (wallet == null)
                throw new Exception("Wallet not found.");

            if (wallet.Balance < amount)
                throw new Exception("Insufficient funds.");

            _databaseService.Execute(
                "UPDATE Wallets SET Balance = Balance - @Amount, Modified = @Modified WHERE WalletID = @WalletID",
                new { Amount = amount, Modified = DateTime.UtcNow, WalletID = walletId }
            );

            _databaseService.Execute(
                @"INSERT INTO WalletTransactions (WalletID, Amount, TransactionType, Description, TransactionDate, Created) 
              VALUES (@WalletID, @Amount, @TransactionType, @Description, @TransactionDate, @Created)",
                new
                {
                    WalletID = walletId,
                    Amount = -amount,
                    TransactionType = "Withdrawal",
                    Description = description,
                    TransactionDate = DateTime.UtcNow,
                    Created = DateTime.UtcNow
                }
            );
        }

        private Wallet GetWalletByWalletId(int walletId)
        {
            return _databaseService.QuerySingle<Wallet>(
                "SELECT * FROM Wallets WHERE WalletID = @WalletID AND Deleted IS NULL",
                new { WalletID = walletId }
            );
        }
        public WalletWithName GetWalletByUserIdWithName(int userID)
        {
            return _databaseService.QuerySingle<WalletWithName>(
                "SELECT W.Balance, U.UserName  FROM Wallets as W inner join Users as U on W.UserID = U.UserID WHERE W.UserID = @UserID AND W.Deleted IS NULL",
                new {UserID = userID }
            );
        }
        public void CreateWallet(int userId)
        {
            // Controlla se il wallet esiste già
            var existingWallet = GetWalletByUserId(userId);
            if (existingWallet != null)
            {
                throw new Exception("Wallet already exists for the specified user.");
            }

            // Crea il nuovo wallet con saldo iniziale di 0
            _databaseService.Execute(
                @"INSERT INTO Wallets (UserID, Balance, Created, Modified, Deleted) 
          VALUES (@UserID, 0, @Created, NULL, NULL)",
                new
                {
                    UserID = userId,
                    Created = DateTime.UtcNow
                }
            );
        }
        public class WalletWithName
        {
            public double Balance { get; set; }
            public string UserName { get; set; }
        }

    }
}