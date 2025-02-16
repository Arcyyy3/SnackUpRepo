using System;

public class PromotionForProductRequest
{
    public string PromotionName { get; set; }
    public string Description { get; set; }
    public decimal DiscountPercentage { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public List<string> ProductNames { get; set; } // Nome del prodotto invece dell'ID
}
