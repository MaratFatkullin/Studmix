using System;
using AI_.Studmix.Domain.Entities;
using AI_.Studmix.Domain.Repository;
using AI_.Studmix.Domain.Services;
using AI_.Studmix.Domain.Services.Abstractions;
using FluentAssertions;
using Moq;
using Xunit;
using System.Linq;
using Xunit.Extensions;

namespace AI_.Studmix.Domain.Tests.Services
{
    public class FinanceServiceTestFixture : TestFixtureBase
    {
        protected Order Order;
        protected ContentPackage Package;
        protected User User;
        protected User Owner;

        protected Mock<IPaymentSystemInvoiceRepository> PaymentSystemInvoiceRepository =
           new Mock<IPaymentSystemInvoiceRepository>();

        public FinanceServiceTestFixture()
        {
            User = CreateUser();
            Owner = CreateUser();
            Package = CreateContentPackage(Owner);
        }

        public FinanceService CreateSut()
        {
            return new FinanceService(PaymentSystemInvoiceRepository.Object);
        }
    }


    public class FinanceServiceTest : FinanceServiceTestFixture
    {
        [Fact]
        public void UserCanBuyPackage_UserHasEnoughMoney_TrueReturned()
        {
            // Arrange
            User.IncomeMoney(100);
            var service = CreateSut();

            // Act
            var userCanOrderPackage = service.UserCanBuyPackage(User, Package);

            // Assert
            userCanOrderPackage.Should().BeTrue();
        }

        [Fact]
        public void MakeOrder_UserHasEnoughMoney_OrderMade()
        {
            // Arrange
            User.IncomeMoney(110);
            var service = CreateSut();

            // Act
            service.MakeOrder(User, Package);

            // Assert
            var order = User.Orders.Single();
            order.User.Should().Be(User);
            order.ContentPackage.Should().Be(Package);

            Owner.Balance.Should().Be(100);
            User.Balance.Should().Be(10);
        }


        [Fact]
        public void MakeOrder_UserHasNotEnoughMoney_ExceptionThrown()
        {
            // Arrange
            var service = CreateSut();

            // Act, Assert
            service.Invoking(s => s.MakeOrder(User, Package)).ShouldThrow<InvalidOperationException>();
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