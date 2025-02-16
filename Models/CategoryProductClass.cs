using System;
using System.ComponentModel.DataAnnotations;

namespace SnackUpAPI.Models
{
    public class CategoryProductClass
    {
        int ProductID { get; set; }
        string ProductName { get; set; }
        string Description { get; set; }
        string Details { get; set; }
        string Raccomandation { get; set; }
        decimal Price { get; set; }
        string? PhotoLinkProdotto { get; set; }
        int? ProducerID { get; set; }
        DateTime? Created { get; set; } = DateTime.Now;
        DateTime? Modified { get; set; }
        DateTime? Deleted { get; set; }
        decimal DiscountedPrice { get; set; }

    }
}