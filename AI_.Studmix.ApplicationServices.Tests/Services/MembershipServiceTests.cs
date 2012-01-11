using System.Linq;
using System.Web.Security;
using AI_.Studmix.ApplicationServices.DataTransferObjects;
using AI_.Studmix.ApplicationServices.Services.MembershipService;
using AI_.Studmix.ApplicationServices.Services.MembershipService.Requests;
using AI_.Studmix.ApplicationServices.Tests.Mocks;
using AI_.Studmix.Domain.Entities;
using AI_.Studmix.Domain.Services.Abstractions;
using AI_.Studmix.Domain.Tests;
using FluentAssertions;
using Moq;
using Xunit;
using Xunit.Extensions;

namespace AI_.Studmix.ApplicationServices.Tests.Services
{
    public class MembershipServiceTestFixture : TestFixtureBase
    {
        protected MembershipConfigurationMock MembershipConfiguration;
        protected UnitOfWorkMock UnitOfWork;

        public MembershipServiceTestFixture()
        {
            UnitOfWork = new UnitOfWorkMock();
            MembershipConfiguration = new MembershipConfigurationMock();
        }

        protected MembershipService CreateSut()
        {
            return new MembershipService(UnitOfWork, MembershipConfiguration);
        }

        protected static CreateUserRequest CreateUserRequest(string username = "username",
                                                             string password = "password",
                                                             string email = "email",
                                                             string phoneNumber = "phoneNumber",
                                                             string passwordQuestion = "passwordQuestion",
                                                             string passwordAnswer = "passwordAnswer")
        {
            return new CreateUserRequest(username,
                                         password,
                                         email,
                                         phoneNumber,
                                         passwordQuestion,
                                         passwordAnswer,
                                         true);
        }
    }


    public class MembershipServiceTests : MembershipServiceTestFixture
    {
        [Fact]
        public void CreateUser_Simple_UserCreated()
        {
            // Arrange
            var request = CreateUserRequest();
            var membershipService = CreateSut();

            // Act
            var response = membershipService.CreateUser(request);

            // Assert
            response.Status.Should().Be(MembershipCreateStatus.Success);
            response.UserName.Should().Be(request.UserName);

            var user = UnitOfWork.GetRepository<User>().Get().Single();
            user.ShouldHave().SharedProperties().EqualTo(request);
        }

        [Fact]
        public void CreateUser_UserWithSameUserName_UserNotCreated()
        {
            // Arrange
            var request = CreateUserRequest();
            var membershipService = CreateSut();
            membershipService.CreateUser(request);

            // Act
            var response = membershipService.CreateUser(request);

            // Assert
            response.Status.Should().Be(MembershipCreateStatus.DuplicateUserName);
            var users = UnitOfWork.GetRepository<User>().Get();
            users.Count.Should().Be(1);
        }


        [Fact]
        public void CreateUser_UniqueEmailConstraintViolated_UserNotCreated()
        {
            // Arrange
            MembershipConfiguration.RequiresUniqueEmail = true;
            var request = CreateUserRequest("username1");
            var membershipService = CreateSut();
            membershipService.CreateUser(request);
            request.UserName = "username2";

            // Act
            var response = membershipService.CreateUser(request);

            // Assert
            response.Status.Should().Be(MembershipCreateStatus.DuplicateEmail);

            var users = UnitOfWork.GetRepository<User>().Get();
            users.Count.Should().Be(1);
        }


        [Fact]
        public void CreateUser_MinRequiredPasswordLengthConstraintViolated_UserNotCreated()
        {
            // Arrange
            MembershipConfiguration.MinRequiredPasswordLength = 4;
            var request = CreateUserRequest(password: "123");
            var membershipService = CreateSut();

            // Act
            var response = membershipService.CreateUser(request);

            // Assert
            response.Status.Should().Be(MembershipCreateStatus.InvalidPassword);

            var users = UnitOfWork.GetRepository<User>().Get();
            users.Should().BeEmpty();
        }

        [Fact]
        public void CreateUser_PasswordContainsWhiteSpace_UserNotCreated()
        {
            // Arrange
            var request = CreateUserRequest(password: "pass word");
            var membershipService = CreateSut();

            // Act
            var response = membershipService.CreateUser(request);

            // Assert
            response.Status.Should().Be(MembershipCreateStatus.InvalidPassword);

            var users = UnitOfWork.GetRepository<User>().Get();
            users.Should().BeEmpty();
        }

        [Fact]
        public void CreateUser_RequiresEmailConstraintViolated_UserNotCreated()
        {
            // Arrange
            MembershipConfiguration.RequiresEmail = true;
            var request = CreateUserRequest(email: string.Empty);
            var membershipService = CreateSut();

            // Act
            var response = membershipService.CreateUser(request);

            // Assert
            response.Status.Should().Be(MembershipCreateStatus.InvalidEmail);

            var users = UnitOfWork.GetRepository<User>().Get();
            users.Should().BeEmpty();
        }

        [Fact]
        public void CreateUser_RequiresQuestionConstraintViolated_UserNotCreated()
        {
            // Arrange
            MembershipConfiguration.RequiresQuestionAndAnswer = true;
            var request = CreateUserRequest(passwordQuestion: string.Empty);
            var membershipService = CreateSut();

            // Act
            var response = membershipService.CreateUser(request);

            // Assert
            response.Status.Should().Be(MembershipCreateStatus.InvalidQuestion);

            var users = UnitOfWork.GetRepository<User>().Get();
            users.Should().BeEmpty();
        }

        [Fact]
        public void CreateUser_RequiresAnswerConstraintViolated_UserNotCreated()
        {
            // Arrange
            MembershipConfiguration.RequiresQuestionAndAnswer = true;
            var request = CreateUserRequest(passwordAnswer: string.Empty);
            var membershipService = CreateSut();

            // Act
            var response = membershipService.CreateUser(request);

            // Assert
            response.Status.Should().Be(MembershipCreateStatus.InvalidAnswer);

            var users = UnitOfWork.GetRepository<User>().Get();
            users.Should().BeEmpty();
        }

        [Fact]
        public void ValidateUser_Simple_UserValid()
        {
            // Arrange
            var user = CreateUser();
            UnitOfWork.GetRepository<User>().Insert(user);
            UnitOfWork.Save();

            var membershipService = CreateSut();
            var request = new ValidateUserRequest(user.UserName, user.Password);

            // Act
            var responce = membershipService.ValidateUser(request);

            // Assert
            responce.IsValid.Should().BeTrue();
        }


        [Fact]
        public void ValidateUser_UserNotExists_UserNotValid()
        {
            // Arrange
            var user = CreateUser();

            var membershipService = CreateSut();
            var request = new ValidateUserRequest(user.UserName, user.Password);

            // Act
            var responce = membershipService.ValidateUser(request);

            // Assert
            responce.IsValid.Should().BeFalse();
        }


        [Fact]
        public void ValidateUser_UserNotApproved_UserNotValid()
        {
            // Arrange
            var user = CreateUser();
            user.IsApproved = false;
            UnitOfWork.GetRepository<User>().Insert(user);
            UnitOfWork.Save();
            var membershipService = CreateSut();
            var request = new ValidateUserRequest(user.UserName, user.Password);

            // Act
            var responce = membershipService.ValidateUser(request);

            // Assert
            responce.IsValid.Should().BeFalse();
        }

        [Fact]
        public void ValidateUser_UserLocked_UserNotValid()
        {
            // Arrange
            var user = CreateUser();
            user.IsLocked = true;
            UnitOfWork.GetRepository<User>().Insert(user);
            UnitOfWork.Save();
            var membershipService = CreateSut();
            var request = new ValidateUserRequest(user.UserName, user.Password);

            // Act
            var responce = membershipService.ValidateUser(request);

            // Assert
            responce.IsValid.Should().BeFalse();
        }

        [Fact]
        public void ValidateUser_IncorrectPassword_UserNotValid()
        {
            // Arrange
            var user = CreateUser();
            UnitOfWork.GetRepository<User>().Insert(user);
            UnitOfWork.Save();
            var membershipService = CreateSut();
            var request = new ValidateUserRequest(user.UserName, "incorrectPassword");

            // Act
            var responce = membershipService.ValidateUser(request);

            // Assert
            responce.IsValid.Should().BeFalse();
        }

        [Fact]
        public void ChangePassword_RequestDataCorrect_PasswordChanged()
        {
            // Arrange
            var user = CreateUser();
            UnitOfWork.GetRepository<User>().Insert(user);
            UnitOfWork.Save();

            var request = new ChangePasswordRequest(user.UserName, user.Password, "newPassword");
            var service = CreateSut();

            // Act
            var response = service.ChangePassword(request);

            // Assert
            response.ChangePasswordSucceeded.Should().BeTrue();
            user.Password.Should().Be("newPassword");
        }

        [Fact]
        public void ChangePassword_OldPasswordIncorrect_PasswordNotChanged()
        {
            // Arrange
            var user = CreateUser();
            user.Password = "old";
            UnitOfWork.GetRepository<User>().Insert(user);
            UnitOfWork.Save();

            var request = new ChangePasswordRequest(user.UserName, "wrongOldPassword", "newPassword");
            var service = CreateSut();

            // Act
            var response = service.ChangePassword(request);

            // Assert
            response.ChangePasswordSucceeded.Should().BeFalse();
            user.Password.Should().Be("old");
        }

        [Fact]
        public void ChangePassword_UserNameIncorrect_PasswordNotChanged()
        {
            // Arrange
            var request = new ChangePasswordRequest("incorrectUserName", "OldPassword", "newPassword");
            var service = CreateSut();

            // Act
            var response = service.ChangePassword(request);

            // Assert
            response.ChangePasswordSucceeded.Should().BeFalse();
        }

        [Theory]
        [InlineData(2,1,2)]
        [InlineData(1,2,1)]
        [InlineData(3,1,3)]
        [InlineData(2,2,1)]
        public void GetUserList_Simple_UsersProvided(int pageSize,int pageIndex,int actualCount)
        {
            // Arrange
            var repository = UnitOfWork.GetRepository<User>();
            repository.Insert(CreateUser());
            repository.Insert(CreateUser());
            repository.Insert(CreateUser());
            UnitOfWork.Save();

            var service = CreateSut();
            var request = new GetUserListRequest(pageSize,pageIndex);

            // Act
            var response = service.GetUserList(request);

            // Assert
            response.Users.Should().HaveCount(actualCount);
        }

        [Fact]
        public void GetUser_Simple_UserProvided()
        {
            // Arrange
            var user = CreateUser();
            UnitOfWork.GetRepository<User>().Insert(user);
            UnitOfWork.Save();

            var service = CreateSut();
            var request = new GetUserRequest(user.ID);

            // Act
            var response = service.GetUser(request);

            // Assert
            response.User.ShouldHave().SharedProperties().EqualTo(user);
        }

        [Fact]
        public void UpdateUser_Simple_UserUpdated()
        {
            // Arrange
            var user = CreateUser();
            user.IncomeMoney(33);
            UnitOfWork.GetRepository<User>().Insert(user);
            UnitOfWork.Save();

            var request = new UpdateUserRequest();
            var userDto = new UserDto
                          {
                              ID = user.ID,
                              Balance = 42,
                          };
            request.User = userDto;

            var service = CreateSut();

            // Act
            service.UpdateUser(request);

            // Assert
            user.Balance.Should().Be(userDto.Balance);
        }
    }
}