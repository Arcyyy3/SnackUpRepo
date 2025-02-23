using System;
using System.Collections.Generic;

namespace SnackUpAPI.Db_Models;

public partial class WalletTransaction
{
    public int TransactionId { get; set; }

    public int WalletId { get; set; }

    public decimal Amount { get; set; }

    public string TransactionType { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime TransactionDate { get; set; }

    public DateTime Created { get; set; }

    public DateTime? Modified { get; set; }

    public DateTime? Deleted { get; set; }

    public virtual Wallet Wallet { get; set; } = null!;
}
