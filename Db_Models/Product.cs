using System;
using System.Collections.Generic;

namespace SnackUpAPI.Db_Models;

public partial class Product
{
    public int ProductId { get; set; }

    public int? BundleId { get; set; }

    public string ProductName { get; set; } = null!;

    public string? Description { get; set; }

    public string? Details { get; set; }

    public string? Raccomandation { get; set; }

    public decimal? Price { get; set; }

    public string? PhotoLinkProdotto { get; set; }

    public int ProducerId { get; set; }

    public DateTime Created { get; set; }

    public DateTime? Modified { get; set; }

    public DateTime? Deleted { get; set; }

    public virtual ICollection<BundleItem> BundleItems { get; set; } = new List<BundleItem>();

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual Inventory? Inventory { get; set; }

    public virtual ICollection<InventoryHistory> InventoryHistories { get; set; } = new List<InventoryHistory>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual Producer Producer { get; set; } = null!;

    public virtual ICollection<ProductAllergen> ProductAllergens { get; set; } = new List<ProductAllergen>();

    public virtual ICollection<ProductPromotion> ProductPromotions { get; set; } = new List<ProductPromotion>();
}
