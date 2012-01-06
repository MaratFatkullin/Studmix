using AI_.Studmix.ApplicationServices.Services.OrderService;
using AI_.Studmix.ApplicationServices.Services.OrderService.Requests;
using AI_.Studmix.ApplicationServices.Tests.Mocks;
using AI_.Studmix.Domain.Entities;
using AI_.Studmix.Domain.Services.Abstractions;
using AI_.Studmix.Domain.Tests;
using FluentAssertions;
using Moq;
using Xunit.Extensions;

namespace AI_.Studmix.ApplicationServices.Tests.Services
{
    public class OrderServiceTestFixture : TestFixtureBase
    {
        protected Mock<IFinanceService> FinanceService = new Mock<IFinanceService>();
        protected UnitOfWorkMock UnitOfWork = new UnitOfWorkMock();

        public OrderService CreateSut()
        {
            return new OrderService(UnitOfWork, FinanceService.Object);
        }
    }


    public class OrderServiceTests : OrderServiceTestFixture
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ViewOrder_Simple_ExpectedBehavior(bool canUserBuyPackage)
        {
            // Arrange
            var user = CreateUser();
            user.IncomeMoney(33);
            UnitOfWork.GetRepository<User>().Insert(user);

            var package = CreateContentPackage();
            package.Price = 30;
            UnitOfWork.GetRepository<ContentPackage>().Insert(package);
            UnitOfWork.Save();
            FinanceService.Setup(s => s.UserCanBuyPackage(user, package))
                .Returns(canUserBuyPackage);

            var service = CreateSut();
            var request = new ViewOrderRequest {PackageID = package.ID, UserName = user.UserName};

            // Act
            var responses = service.ViewOrder(request);

            // Assert
            responses.UserBalance.Should().Be(33);
            responses.OrderPrice.Should().Be(30);
            responses.IsUserCanBuyPackage.Should().Be(canUserBuyPackage);
        }
    }
}