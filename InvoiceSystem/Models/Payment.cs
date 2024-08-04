namespace InvoiceSystemAPI.Models
{
    public class Payment
    {
        public string InvoiceId { get; set; } = string.Empty; // Default value

        public decimal Amount { get; set; }
    }
}