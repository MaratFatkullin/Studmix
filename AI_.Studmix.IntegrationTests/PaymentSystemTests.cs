using System.Collections.Generic;
using System.Linq;
using AI_.Studmix.Domain.Entities;
using AI_.Studmix.Domain.Services.Abstractions;
using AI_.Studmix.Infrastructure.PaymentSystem;
using FluentAssertions;
using Xunit;

namespace AI_.Studmix.IntegrationTests
{
    public class PaymentSystemTestFixture
    {
        public QiwiInvoiceRepository CreateSut()
        {
            return new QiwiInvoiceRepository(new PaymentSystmeProviderConfiguration());
        }
    }


    public class PaymentSystemTests : IUseFixture<PaymentSystemTestFixture>
    {
        protected PaymentSystemTestFixture Fixture { get; set; }

        #region IUseFixture<PaymentSystemTestFixture> Members

        public void SetFixture(PaymentSystemTestFixture data)
        {
            Fixture = data;
        }

        #endregion

        [Fact]
        public void GetBillStatus_ServiceRespondWithActualStatusCode()
        {
            // Arrange
            var bill1 = new Invoice
                        {
                            ToAccount = "9872854409",
                            Amount = 100,
                            Comment = "___Test operation___"
                        };
            var bill2 = new Invoice
                        {
                            ToAccount = "9872854409",
                            Amount = 100,
                            Comment = "___Test operation___"
                        };
            var paymentSystem = Fixture.CreateSut();
            paymentSystem.StoreInvoice(bill1);
            paymentSystem.StoreInvoice(bill2);
            var invoiceStatuses = paymentSystem.GetInvoiceStatuses(new List<Invoice> {bill1, bill2});
            invoiceStatuses.First().Value.Should().Be(InvoiceStatus.Invoiced);
            invoiceStatuses.Last().Value.Should().Be(InvoiceStatus.Invoiced);
        }
    }
}