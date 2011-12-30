using System.Linq;
using AI_.Studmix.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace AI_.Studmix.Domain.Tests.Enitities
{
    public class PropertyTestsFixture : TestFixtureBase
    {
        protected Property Property;

        public PropertyTestsFixture()
        {
            Property = CreateProperty();
        }
    }


    public class PropertyTests : PropertyTestsFixture
    {
        [Fact]
        public void GetState_StateNotExists_StateAdded()
        {
            // Arrange

            // Act
            Property.GetState("value");

            // Assert
            var propertyState = Property.States.Single();
            propertyState.Value.Should().Be("value");
            propertyState.Index.Should().Be(1);
            propertyState.Property.Should().Be(Property);
        }

        [Fact]
        public void GetState_StateExists_ExistingStateReturned()
        {
            // Arrange
            var existingState = Property.GetState("value");

            // Act
            var propertyState = Property.GetState("value");

            //Assert
            propertyState.Should().Be(existingState);
        }

        [Fact]
        public void GetState_ThereIsAnotherStates_IndexHasUniqueValue()
        {
            // Arrange
            Property.GetState("anotherValue");

            // Act
            Property.GetState("value");

            // Assert
            var state = Property.States.Last();
            state.Index.Should().Be(2);
        }
    }
}