using System;
using System.Collections.Generic;

namespace SnackUpAPI.Db_Models;

public partial class OrderTracking
{
    public int OrderTrackingId { get; set; }

    public int OrderId { get; set; }

    public string Status { get; set; } = null!;

    public DateTime LastUpdate { get; set; }

    public DateTime Created { get; set; }

    public DateTime? Modified { get; set; }

    public DateTime? Deleted { get; set; }

    public virtual Order Order { get; set; } = null!;
}
