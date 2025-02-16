public class UpdateCartQuantityRequest
{
    public int SessionID { get; set; }
    public int ProductID { get; set; }
    public int Quantity { get; set; } // Quantità finale desiderata
}
