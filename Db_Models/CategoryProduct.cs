using System;
using System.Collections.Generic;

namespace SnackUpAPI.Db_Models;

public partial class CategoryProduct
{
    public int ProductId { get; set; }

    public int CategoryId { get; set; }

    public DateTime? Created { get; set; }

    public DateTime? Modified { get; set; }

    public DateTime? Deleted { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
