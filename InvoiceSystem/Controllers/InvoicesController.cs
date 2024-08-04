using InvoiceSystemAPI.Models;
using Microsoft.AspNetCore.Mvc;
//using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InvoiceSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicesController : ControllerBase
    {
        private static List<Invoice> Invoices = new List<Invoice>();

        // POST /api/invoices
        [HttpPost]
        public IActionResult CreateInvoice([FromBody] Invoice invoice)
        {
            invoice.Id = Guid.NewGuid().ToString();
            invoice.Status = "pending";
            Invoices.Add(invoice);
            //return CreatedAtAction(nameof(GetInvoiceById), new { id = invoice.Id }, new { id = invoice.Id });
            return CreatedAtAction(nameof(GetInvoiceById), new { id = invoice.Id }, new Dictionary<string, string> { { "id", invoice.Id } });

        }

        // GET /api/invoices
        [HttpGet]
        public IActionResult GetInvoices()
        {
            return Ok(Invoices);
        }

        // GET /api/invoices/{id}
        [HttpGet("{id}")]
        public IActionResult GetInvoiceById(string id)
        {
            var invoice = Invoices.FirstOrDefault(i => i.Id == id);
            if (invoice == null)
            {
                return NotFound();
            }
            return Ok(invoice);
        }

        // POST /api/invoices/{id}/payments
        [HttpPost("{id}/payments")]
        public IActionResult PayInvoice(string id, [FromBody] Payment payment)
        {
            var invoice = Invoices.FirstOrDefault(i => i.Id == id);
            if (invoice == null)
            {
                return NotFound();
            }

            invoice.PaidAmount += payment.Amount;
            if (invoice.PaidAmount >= invoice.Amount)
            {
                invoice.Status = "paid";
            }

            return Ok(invoice);
        }

        // POST /api/invoices/process-overdue
        [HttpPost("process-overdue")]
        public IActionResult ProcessOverdueInvoices([FromBody] ProcessOverdueRequest request)
        {
            var overdueInvoices = Invoices.Where(i => i.Status == "pending" && i.DueDate < DateTime.Now.AddDays(-request.OverdueDays)).ToList();
            //Assert.NotEmpty(overdueInvoices); // Ensure invoices are found

            foreach (var invoice in overdueInvoices)
            {
                if (invoice.PaidAmount > 0)
                {
                    // Partially paid invoice
                    decimal remainingAmount = invoice.Amount - invoice.PaidAmount;
                    invoice.Status = "paid";

                    // Create a new invoice for the remaining amount + late fee
                    var newInvoice = new Invoice
                    {
                        Id = Guid.NewGuid().ToString(),
                        Amount = remainingAmount + request.LateFee,
                        PaidAmount = 0,
                        DueDate = DateTime.Now.AddDays(request.OverdueDays),
                        Status = "pending"
                    };
                    Invoices.Add(newInvoice);
                }
                else
                {
                    // Unpaid invoice
                    invoice.Status = "void";

                    // Create a new invoice for the original amount + late fee
                    var newInvoice = new Invoice
                    {
                        Id = Guid.NewGuid().ToString(),
                        Amount = invoice.Amount + request.LateFee,
                        PaidAmount = 0,
                        DueDate = DateTime.Now.AddDays(request.OverdueDays),
                        Status = "pending"
                    };
                    Invoices.Add(newInvoice);
                }
            }

            return Ok();
        }

    }
}

