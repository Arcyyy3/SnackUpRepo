using System;
using System.Collections.Generic;

namespace SnackUpAPI.Db_Models;

public partial class ProductAllergen
{
    public int ProductId { get; set; }

    public int AllergenId { get; set; }

    public DateTime? Created { get; set; }

    public DateTime? Modified { get; set; }

    public DateTime? Deleted { get; set; }

    public virtual Allergen Allergen { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
