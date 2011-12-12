using System;
using AI_.Studmix.Domain.Entities;
using AI_.Studmix.Domain.Services;
using FluentAssertions;
using Xunit;
using System.Linq;

namespace AI_.Studmix.Domain.Tests.Services
{
    public class FinanceServiceTestFixture : TestFixtureBase
    {
        protected FinanceService FinanceService;
        protected Order Order;
        protected ContentPackage Package;
        protected User User;
        protected User Owner;

        public FinanceServiceTestFixture()
        {
            User = CreateUser();
            Owner = CreateUser();
            Package = CreateContentPackage(Owner);

            FinanceService = new FinanceService();
        }
    }


    public class FinanceServiceTest : FinanceServiceTestFixture
    {
        [Fact]
        public void UserCanBuyPackage_UserHasEnoughMoney_TrueReturned()
        {
            // Arrange
            User.IncomeMoney(100);

            // Act
            var userCanOrderPackage = FinanceService.UserCanBuyPackage(User, Package);

            // Assert
            userCanOrderPackage.Should().BeTrue();
        }

        [Fact]
        public void MakeOrder_UserHasEnoughMoney_OrderMade()
        {
            // Arrange
            User.IncomeMoney(110);

            // Act
            FinanceService.MakeOrder(User,Package);

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

            // Act, Assert
            FinanceService.Invoking(s => s.MakeOrder(User, Package)).ShouldThrow<InvalidOperationException>();
        }
    }
}