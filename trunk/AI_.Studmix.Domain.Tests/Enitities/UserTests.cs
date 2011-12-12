using System;
using AI_.Studmix.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace AI_.Studmix.Domain.Tests.Enitities
{
    public class UserTests : TestFixtureBase
    {
        [Fact]
        public void IsInRole_UserInRole_True()
        {
            // Arrange
            var user = CreateUser(new Role {RoleName = "role"});

            // Act
            var isInRole = user.IsInRole("role");

            // Assert
            isInRole.Should().BeTrue();
        }

        [Fact]
        public void IsInRole_UserNotInRole_False()
        {
            // Arrange
            var user = CreateUser();

            // Act
            var isInRole = user.IsInRole("role");

            // Assert
            isInRole.Should().BeFalse();
        }

        [Fact]
        public void IncomeMoney_Simple_BalanceIncreased()
        {
            // Arrange
            var user = CreateUser();

            // Act
            user.IncomeMoney(100);

            // Assert
            user.Balance.Should().Be(100);
        }

        [Fact]
        public void OutcomeMoney_UserHasEnoughMoney_BalanceReduced()
        {
            // Arrange
            var user = CreateUser();
            user.IncomeMoney(100);

            // Act
            user.OutcomeMoney(40);

            // Assert
            user.Balance.Should().Be(60);
        }

        [Fact]
        public void OutcomeMoney_UserHasNotEnoughMoney_ExceptionThrown()
        {
            // Arrange
            var user = CreateUser();
            user.IncomeMoney(100);

            // Act, Assert
            user.Invoking(u => u.OutcomeMoney(110)).ShouldThrow<InvalidOperationException>();
        }
    }
}