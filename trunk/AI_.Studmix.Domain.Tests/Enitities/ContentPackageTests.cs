using AI_.Studmix.Domain.Entities;
using Xunit;
using FluentAssertions;

namespace AI_.Studmix.Domain.Tests.Enitities
{
    public class ContentPackageTestFixture :TestFixtureBase
    {
    }


    public class ContentPackageTests : ContentPackageTestFixture
    {
        [Fact]
        public void AddPropertyState_Simple_StateAdded()
        {
            // Arrange
            var package = CreateContentPackage();
            var propertyState = CreatePropertyState(CreateProperty(), "value", 1);

            // Act
            package.AddPropertyState(propertyState);

            // Assert
            package.PropertyStates.Should().Contain(propertyState);
        }
    }
}