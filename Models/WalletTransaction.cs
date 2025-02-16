// Modello per WalletTransaction
using System;

public class WalletTransaction
{
    public int TransactionID { get; set; }
    public int WalletID { get; set; }
    public decimal Amount { get; set; }
    public string TransactionType { get; set; }
    public string Description { get; set; }
    public DateTime TransactionDate { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Modified { get; set; }
    public DateTime? Deleted { get; set; }
}