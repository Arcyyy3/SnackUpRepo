using System;
using System.Collections.Generic;

namespace SnackUpAPI.Db_Models;

public partial class Inventory
{
    public int ProductId { get; set; }

    public int QuantityAvailable { get; set; }

    public int? QuantityReserved { get; set; }

    public int? ReorderLevel { get; set; }

    public DateTime LastUpdated { get; set; }

    public DateTime Created { get; set; }

    public DateTime? Modified { get; set; }

    public DateTime? Deleted { get; set; }

    public virtual Product Product { get; set; } = null!;
}
