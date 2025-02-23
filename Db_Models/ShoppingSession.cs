using System;
using System.Collections.Generic;

namespace SnackUpAPI.Db_Models;

public partial class ShoppingSession
{
    public int SessionId { get; set; }

    public int? UserId { get; set; }

    public string Status { get; set; } = null!;

    public decimal TotalAmount { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual User? User { get; set; }
}
