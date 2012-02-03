using System.Collections.Generic;
using AI_.Studmix.ApplicationServices.DataTransferObjects;
using AI_.Studmix.ApplicationServices.Services.MembershipService;
using AI_.Studmix.ApplicationServices.Services.MembershipService.Requests;
using AI_.Studmix.ApplicationServices.Services.MembershipService.Responses;
using AI_.Studmix.WebApplication.Controllers;
using AI_.Studmix.WebApplication.ViewModels.Admin;
using FluentAssertions;
using Moq;
using Xunit;

namespace AI_.Studmix.WebApplication.Tests.Controllers
{
    public class AdminControllerTestFixture
    {
        protected Mock<IMembershipService> MembershipService = new Mock<IMembershipService>();

        public AdminController CreateSut()
        {
            return new AdminController(MembershipService.Object);
        }
    }


    public class AdminControllerTests : AdminControllerTestFixture
    {
        [Fact]
        public void Users_Simple_UserListProvided()
        {
            // Arrange
            var users = new List<UserDto>();

            MembershipService.Setup(s => s.GetUserList(
                It.Is<GetUserListRequest>(r => r.PageIndex == 2)))
                .Returns(new GetUserListResponse(users, 60));

            var controller = CreateSut();

            // Act
            var result = controller.Users(2);

            // Assert
            var viewModel = (UsersViewModel) result.Model;
            viewModel.UsersPagination.Should().Equal(users);
        }

        [Fact]
        public void UserDetails_Simple_UserDetailsShown()
        {
            // Arrange
            var user = new UserDto();
            MembershipService.Setup(s => s.GetUser(It.Is<GetUserRequest>(r => r.UserName == "username")))
                .Returns(new GetUserResponse {User = user});

            var controller = CreateSut();

            // Act
            var result = controller.UserDetails("username");

            // Assert
            var viewModel = (UserDetailsViewModel) result.Model;
            viewModel.User.Should().Be(user);
        }

        [Fact]
        public void UpdateUser_Simple_UserPropertiesUpdated()
        {
            // Arrange
            var user = new UserDto();
            var viewModel = new UserDetailsViewModel {User = user};

            var controller = CreateSut();

            // Act
            var viewResult = controller.UpdateUser(viewModel);

            // Assert
            MembershipService.Verify(s => s.UpdateUser(It.Is<UpdateUserRequest>(r => r.User == user)));
            viewResult.ViewName.Should().Be("Information");
        }
    }
}