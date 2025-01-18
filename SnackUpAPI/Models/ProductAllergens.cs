using System;

namespace SnackUpAPI.Models
{
    public class ProductAllergen
    {
        public int ProductID { get; set; }
        public int AllergenID { get; set; }

        public DateTime? Created { get; set; } = DateTime.UtcNow;
        public DateTime? Modified { get; set; }
        public DateTime? Deleted { get; set; }
    }
}
