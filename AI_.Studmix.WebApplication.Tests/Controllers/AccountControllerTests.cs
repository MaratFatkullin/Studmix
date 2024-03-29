﻿using System.Web.Mvc;
using System.Web.Security;
using AI_.Studmix.ApplicationServices.Services.MembershipService;
using AI_.Studmix.ApplicationServices.Services.MembershipService.Requests;
using AI_.Studmix.ApplicationServices.Services.MembershipService.Responses;
using AI_.Studmix.WebApplication.Controllers;
using AI_.Studmix.WebApplication.Infrastructure.Authentication;
using AI_.Studmix.WebApplication.ViewModels.Account;
using FluentAssertions;
using Moq;
using Xunit;
using Xunit.Extensions;

namespace AI_.Studmix.WebApplication.Tests.Controllers
{
    public class AccountControllerTestFixture : TestFixtureBase
    {
        protected Mock<IAuthenticationProvider> AuthenticationProvider = new Mock<IAuthenticationProvider>();
        protected Mock<IMembershipService> MembershipService = new Mock<IMembershipService>();

        public AccountController CreateSut()
        {
            return new AccountController(MembershipService.Object, AuthenticationProvider.Object);
        }

        //public IPrincipal CreatePrincipal(string username)
        //{
        //    var principal = new Mock<IPrincipal>();
        //    var identity = new Mock<IIdentity>();
        //    principal.SetupGet(p => p.Identity).Returns(identity.Object);
        //    return principal.Object;
        //}
    }


    public class AccountControllerTests : AccountControllerTestFixture
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void LogOn_UserValid_UserAutenticated(bool rememberUser)
        {
            // Arrange
            MembershipService.Setup(s => s.ValidateUser(
                It.Is<ValidateUserRequest>(r => r.UserName == "username" && r.Password == "password")))
                .Returns(new ValidateUserResponce(true));

            var logOnModel = new LogOnViewModel
                             {
                                 UserName = "username",
                                 Password = "password",
                                 RememberMe = rememberUser
                             };

            var controller = CreateSut();

            // Act
            var result = controller.LogOn(logOnModel, "/successUrl");

            // Assert
            AuthenticationProvider.Verify(p => p.LogOn("username", rememberUser));
            ((RedirectResult) result).Url.Should().Be("/successUrl");
        }

        [Fact]
        public void LogOn_UserNotValid_UserNotAutenticated()
        {
            // Arrange
            MembershipService.Setup(s => s.ValidateUser(It.IsAny<ValidateUserRequest>()))
                .Returns(new ValidateUserResponce(false));

            var controller = CreateSut();

            // Act
            var result = controller.LogOn(new LogOnViewModel(), "");

            // Assert
            ((ViewResult) result).ViewName.Should().BeEmpty();
            controller.ModelState.IsValid.Should().BeFalse();
        }

        [Fact]
        public void LogOff_Simple_UserLogedOff()
        {
            // Arrange
            var controller = CreateSut();

            // Act
            var result = controller.LogOff();

            // Assert
            AuthenticationProvider.Verify(p => p.LogOut());

            var redirectToRouteResult = (RedirectToRouteResult) result;
            redirectToRouteResult.RouteValues["controller"].Should().Be("Home");
            redirectToRouteResult.RouteValues["action"].Should().Be("Index");
        }


        [Fact]
        public void Register_UserDataValid_UserRegistered()
        {
            // Arrange
            MembershipService.Setup(s => s.CreateUser(It.Is<CreateUserRequest>(r =>
                                                                               r.UserName == "username"
                                                                               && r.Password == "password"
                                                                               && r.Email == "email"
                                                                               && r.PhoneNumber == "5552200"
                                                                               && r.IsApproved)))
                .Returns(new CreateUserResponse(MembershipCreateStatus.Success, "username"));

            var viewModel = new RegisterViewModel
                            {
                                UserName = "username",
                                Password = "password",
                                Email = "email",
                                PhoneNumber = "5552200"
                            };

            var controller = CreateSut();

            // Act
            var result = controller.Register(viewModel);

            // Assert
            var redirectToRouteResult = (RedirectToRouteResult) result;
            redirectToRouteResult.RouteValues["controller"].Should().Be("Home");
            redirectToRouteResult.RouteValues["action"].Should().Be("Index");

            AuthenticationProvider.Verify(p => p.LogOn("username", false));
        }

        [Fact]
        public void Register_UserDataNotValid_UserRegistered()
        {
            // Arrange
            MembershipService.Setup(s => s.CreateUser(It.IsAny<CreateUserRequest>()))
                .Returns(new CreateUserResponse(MembershipCreateStatus.UserRejected));

            var viewModel = new RegisterViewModel();

            var controller = CreateSut();

            // Act
            var result = controller.Register(viewModel);

            // Assert
            var viewResult = (ViewResult) result;
            viewResult.ViewName.Should().BeEmpty();
            controller.ModelState.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Register_Simple_MinRequiredPasswordLengthPassed()
        {
            // Arrange
            MembershipService.SetupGet(s => s.Configuration.MinRequiredPasswordLength)
                .Returns(3);

            var controller = CreateSut();

            // Act
            var result = controller.Register();

            // Assert
            var viewModel = (RegisterViewModel)result.Model;
            viewModel.MinRequiredPasswordLength.Should().Be(3);
        }

        [Fact]
        public void ChangePassword_PasswordChanged_RedirectedToSeccessView()
        {
            // Arrange
            MembershipService.Setup(s => s.ChangePassword(
                It.Is<ChangePasswordRequest>(r => r.NewPassword == "new"
                                                  && r.OldPassword == "old"
                                                  && r.UserName == "username")))
                .Returns(new ChangePasswordResponse(true));

            var viewModel = new ChangePasswordViewModel
                            {
                                OldPassword = "old",
                                NewPassword = "new",
                                ConfirmPassword = "new"
                            };

            var controller = CreateSut();
            controller.ControllerContext = CreateContext("username");

            // Act
            var result = controller.ChangePassword(viewModel);

            // Assert
            var redirectToRouteResult = ((RedirectToRouteResult) result);
            redirectToRouteResult.RouteValues["action"].Should().Be("ChangePasswordSuccess");
        }


        [Fact]
        public void ChangePassword_PasswordNotChanged_ErrorsShown()
        {
            // Arrange
            MembershipService.Setup(s => s.ChangePassword(It.IsAny<ChangePasswordRequest>()))
                .Returns(new ChangePasswordResponse(true));

            var viewModel = new ChangePasswordViewModel();

            var controller = CreateSut();
            controller.ControllerContext = CreateContext("username");

            // Act
            var result = controller.ChangePassword(viewModel);

            // Assert
            var redirectToRouteResult = ((RedirectToRouteResult) result);
            redirectToRouteResult.RouteValues["action"].Should().Be("ChangePasswordSuccess");
        }
    }
}