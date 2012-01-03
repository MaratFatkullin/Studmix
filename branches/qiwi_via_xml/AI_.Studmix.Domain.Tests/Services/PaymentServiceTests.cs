using AI_.Studmix.Domain.Entities;
using AI_.Studmix.Domain.Repository;
using AI_.Studmix.Domain.Services;
using AI_.Studmix.Domain.Services.Abstractions;
using FluentAssertions;
using Moq;
using Xunit;
using Xunit.Extensions;

namespace AI_.Studmix.Domain.Tests.Services
{
    public class PaymentServiceTestFixture : TestFixtureBase
    {
        protected Mock<IPaymentSystemInvoiceRepository> PaymentSystemInvoiceRepository =
            new Mock<IPaymentSystemInvoiceRepository>();

        public PaymentService CreateSut()
        {
            return new PaymentService(PaymentSystemInvoiceRepository.Object);
        }
    }


    public class PaymentServiceTests : PaymentServiceTestFixture
    {
        [Fact]
        public void GetActualBalance_InvoicePaid_UserBalanceRecalculated()
        {
            // Arrange
            var user = CreateUser();
            var invoice = CreateInvoice(user, 100);
            user.Invoices.Add(invoice);

            PaymentSystemInvoiceRepository.Setup(r => r.GetInvoiceStatus(invoice))
                .Returns(InvoiceStatus.Paid);

            var service = CreateSut();

            // Act
            var balance = service.GetActualBalance(user);

            // Assert
            balance.Should().Be(100);
        }

        [Theory]
        [InlineData(InvoiceStatus.Invoiced)]
        [InlineData(InvoiceStatus.Canceled)]
        public void GetActualBalance_InvoiceNotPaid_UserBalanceNotChanged(InvoiceStatus status)
        {
            // Arrange
            var user = CreateUser();
            var invoice = CreateInvoice(user, 100);
            user.Invoices.Add(invoice);

            PaymentSystemInvoiceRepository.Setup(r => r.GetInvoiceStatus(invoice))
                .Returns(status);

            var service = CreateSut();

            // Act
            var balance = service.GetActualBalance(user);

            // Assert
            balance.Should().Be(0);
        }

        [Fact]
        public void SendInvoiceToUser_Simple_IvoiceSended()
        {
            // Arrange
            var service = CreateSut();
            var user = CreateUser();

            // Act
            service.SendInvoiceToUser(user, 100, "comment");

            // Assert
            PaymentSystemInvoiceRepository.Verify(
                r =>
                r.StoreInvoice(It.Is<Invoice>(i => i.User == user && i.Amount == 100 && i.Comment == "comment")));
        }
    }
}