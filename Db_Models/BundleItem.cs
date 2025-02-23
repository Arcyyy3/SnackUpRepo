using System;
using System.Collections.Generic;

namespace SnackUpAPI.Db_Models;

public partial class BundleItem
{
    public int BundleItemId { get; set; }

    public int? BundleId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public DateTime Created { get; set; }

    public DateTime? Modified { get; set; }

    public DateTime? Deleted { get; set; }

    public virtual BundleProduct? Bundle { get; set; }

    public virtual Product Product { get; set; } = null!;
}
