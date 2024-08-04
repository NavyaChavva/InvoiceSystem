using System;

namespace InvoiceSystemAPI.Models
{
    public class Invoice
    {
        public string Id { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public decimal PaidAmount { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; } = "pending"; //pending, paid, void
    }
}
