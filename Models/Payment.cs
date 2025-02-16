public class Payment
{
    public int PaymentID { get; set; }
    public int? OrderID { get; set; }
    public int? SubscriptionID { get; set; }
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; }
    public DateTime PaymentDate { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Modified { get; set; }
    public DateTime? Deleted { get; set; }
    public string Status { get; set; } = "Pending"; // Default value
}
