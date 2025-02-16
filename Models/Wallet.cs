using System;

public class Wallet
{
    public int WalletID { get; set; }
    public int UserID { get; set; }
    public decimal Balance { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Modified { get; set; }
    public DateTime? Deleted { get; set; }
}