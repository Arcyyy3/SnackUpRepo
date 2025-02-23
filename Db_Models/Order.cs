using System;
using System.Collections.Generic;

namespace SnackUpAPI.Db_Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int UserId { get; set; }

    public int SchoolClassId { get; set; }

    public string Status { get; set; } = null!;

    public decimal? TotalPrice { get; set; }

    public DateTime? Created { get; set; }

    public DateTime? Modified { get; set; }

    public DateTime? Deleted { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<OrderTracking> OrderTrackings { get; set; } = new List<OrderTracking>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual SchoolClass SchoolClass { get; set; } = null!;

    public virtual ICollection<SupportRequest> SupportRequests { get; set; } = new List<SupportRequest>();

    public virtual User User { get; set; } = null!;
}
