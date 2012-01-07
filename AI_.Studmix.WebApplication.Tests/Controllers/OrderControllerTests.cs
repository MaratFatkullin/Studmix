using System.Web.Mvc;
using AI_.Studmix.ApplicationServices.Services.OrderService;
using AI_.Studmix.ApplicationServices.Services.OrderService.Requests;
using AI_.Studmix.ApplicationServices.Services.OrderService.Responses;
using AI_.Studmix.WebApplication.Controllers;
using AI_.Studmix.WebApplication.ViewModels.Finance;
using Moq;
using Xunit;
using FluentAssertions;

namespace AI_.Studmix.WebApplication.Tests.Controllers
{
    public class OrderControllerTestFixture : TestFixtureBase
    {
        protected Mock<IOrderService> OrderService = new Mock<IOrderService>();

        public OrderController CreateSut()
        {
            return new OrderController(OrderService.Object);
        }
    }


    public class OrderControllerTests : OrderControllerTestFixture
    {
        [Fact]
        public void ViewOrder_Simple_ViewModelDataProvided()
        {
            // Arrange
            OrderService.Setup(s => s.ViewOrder(It.Is<ViewOrderRequest>(r => r.PackageID == 5 && r.UserName == "username")))
                .Returns(new ViewOrderResponse
                         {
                             OrderPrice = 100,
                             UserBalance = 40,
                             IsUserCanBuyPackage = true
                         });
            var controller = CreateSut();
            controller.ControllerContext = CreateContext("username");

            // Act
            var result = controller.ViewOrder(5);

            // Assert
            var viewModel = (ViewOrderViewModel) result.Model;
            viewModel.ContentPackageID.Should().Be(5);
            viewModel.OrderPrice.Should().Be(100);
            viewModel.UserBalance.Should().Be(40);
        }

        [Fact]
        public void ViewOrder_UserCannotBuyPackage_ModelStateNotValid()
        {
            // Arrange
            OrderService.Setup(s => s.ViewOrder(It.IsAny<ViewOrderRequest>()))
                .Returns(new ViewOrderResponse {IsUserCanBuyPackage = false});
            var controller = CreateSut();
            controller.ControllerContext = CreateContext("username");

            // Act
            var result = controller.ViewOrder(1);

            // Assert
            controller.ModelState.IsValid.Should().BeFalse();
        }

        [Fact]
        public void ViewOrder_UserCanBuyPackage_ModelStateValid()
        {
            // Arrange
            OrderService.Setup(s => s.ViewOrder(It.IsAny<ViewOrderRequest>()))
                .Returns(new ViewOrderResponse {IsUserCanBuyPackage = true});
            var controller = CreateSut();
            controller.ControllerContext = CreateContext("username");

            // Act
            var result = controller.ViewOrder(1);

            // Assert
            controller.ModelState.IsValid.Should().BeTrue();
        }
    }
}