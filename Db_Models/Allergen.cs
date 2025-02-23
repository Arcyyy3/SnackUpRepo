﻿using System;
using System.Collections.Generic;

namespace SnackUpAPI.Db_Models;

public partial class Allergen
{
    public int AllergenId { get; set; }

    public string AllergenName { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? Created { get; set; }

    public DateTime? Modified { get; set; }

    public DateTime? Deleted { get; set; }

    public virtual ICollection<ProductAllergen> ProductAllergens { get; set; } = new List<ProductAllergen>();
}
