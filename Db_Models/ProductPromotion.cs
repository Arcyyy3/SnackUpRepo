using System;
using System.Collections.Generic;

namespace SnackUpAPI.Db_Models;

public partial class ProductPromotion
{
    public int ProductPromotionId { get; set; }

    public int ProductId { get; set; }

    public int PromotionId { get; set; }

    public DateTime Created { get; set; }

    public DateTime? Modified { get; set; }

    public DateTime? Deleted { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual Promotion Promotion { get; set; } = null!;
}
