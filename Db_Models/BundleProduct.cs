using System;
using System.Collections.Generic;

namespace SnackUpAPI.Db_Models;

public partial class BundleProduct
{
    public int BundleId { get; set; }

    public string BundleName { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public string Moment { get; set; } = null!;

    public DateTime Created { get; set; }

    public DateTime? Modified { get; set; }

    public DateTime? Deleted { get; set; }

    public virtual ICollection<BundleItem> BundleItems { get; set; } = new List<BundleItem>();
}
