using InvoiceSystemAPI.Controllers;
using InvoiceSystemAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Xunit;

namespace InvoiceSystemAPI.Tests
{
    public class InvoicesControllerTests
    {
        private readonly InvoicesController _controller;

        public InvoicesControllerTests()
        {
            _controller = new InvoicesController();
            SeedInvoices(); // Adding some seed data for testing
        }

        private void SeedInvoices()
        {
            _controller.CreateInvoice(new Invoice { Amount = 100m, DueDate = DateTime.Now.AddDays(30) });
            _controller.CreateInvoice(new Invoice { Amount = 200m, DueDate = DateTime.Now.AddDays(30) });
        }

        [Fact]
        public void CreateInvoice_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var newInvoice = new Invoice { Amount = 199.99m, DueDate = DateTime.Now.AddDays(30) };

            // Act
            var result = _controller.CreateInvoice(newInvoice);

            // Assert
            var actionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<Dictionary<string, string>>(actionResult.Value);
            Assert.NotNull(returnValue["id"]);
        }


        [Fact]
        public void GetInvoices_ReturnsOkResult()
        {
            // Act
            var result = _controller.GetInvoices();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var invoices = Assert.IsType<List<Invoice>>(okResult.Value);
            Assert.NotEmpty(invoices);
        }


        [Fact]
        public void PayInvoice_ValidInvoiceId_ReturnsOkResult()
        {
            // Arrange
            var newInvoice = new Invoice { Amount = 100m, DueDate = DateTime.Now.AddDays(30), Status = "pending" };
            var createResult = _controller.CreateInvoice(newInvoice) as CreatedAtActionResult;
            var invoiceId = createResult?.RouteValues["id"].ToString();

            // Act
            var result = _controller.PayInvoice(invoiceId, new Payment { InvoiceId = invoiceId, Amount = 50m });

            // Assert
            Assert.IsType<OkObjectResult>(result); // Expecting the result to be Ok
            var invoice = (result as OkObjectResult)?.Value as Invoice;
            Assert.NotNull(invoice); // Ensure the invoice is not null
            Assert.Equal(50m, invoice.PaidAmount); // Ensure the amount is paid
        }

        [Fact]
        public void ProcessOverdueInvoices_ValidRequest_ProcessesInvoices()
        {
            // Arrange
            var overdueInvoice = new Invoice { Amount = 100m, DueDate = DateTime.Now.AddDays(-15), Status = "pending" };
            var createResult = _controller.CreateInvoice(overdueInvoice) as CreatedAtActionResult;
            var invoiceId = createResult?.RouteValues["id"].ToString();

            // Act
            var result = _controller.ProcessOverdueInvoices(new ProcessOverdueRequest { LateFee = 10.5m, OverdueDays = 10 });

            // Assert
            Assert.IsType<OkResult>(result);

            var processedInvoice = _controller.GetInvoiceById(invoiceId) as OkObjectResult;
            Assert.NotNull(processedInvoice);
            var invoice = Assert.IsType<Invoice>(processedInvoice.Value);
            Assert.Equal("void", invoice.Status);
        }


    }
}

