using System;
using System.Collections.Generic;

namespace SnackUpAPI.Db_Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? Created { get; set; }

    public DateTime? Modified { get; set; }

    public DateTime? Deleted { get; set; }
}
