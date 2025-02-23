using System;
using System.Collections.Generic;

namespace SnackUpAPI.Db_Models;

public partial class CartItem
{
    public int CartItemId { get; set; }

    public int SessionId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public decimal Total { get; set; }

    public DateTime? DeliveryDate { get; set; }

    public string? Recreation { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual ShoppingSession Session { get; set; } = null!;
}
