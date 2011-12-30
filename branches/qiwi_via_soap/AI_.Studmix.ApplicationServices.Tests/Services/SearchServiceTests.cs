using System.Collections.Generic;
using System.Collections.ObjectModel;
using AI_.Studmix.ApplicationServices.Services.SearchService;
using AI_.Studmix.ApplicationServices.Services.SearchService.Requests;
using AI_.Studmix.ApplicationServices.Tests.Mocks;
using AI_.Studmix.Domain.Entities;
using AI_.Studmix.Domain.Tests;
using FluentAssertions;
using Xunit.Extensions;

namespace AI_.Studmix.ApplicationServices.Tests.Services
{
    public class SearchServiceTestFixture : TestFixtureBase
    {
        protected UnitOfWorkMock UnitOfWork;

        public SearchServiceTestFixture()
        {
            UnitOfWork = new UnitOfWorkMock();
        }

        public SearchService CreateService()
        {
            return new SearchService(UnitOfWork);
        }

        protected void CreateAndStoreTwoInitalizedPackages()
        {
            var property1 = CreateProperty(order: 1);
            var property2 = CreateProperty(order: 2);

            var state1 = property1.GetState("state1");
            var state2 = property1.GetState("state2");
            var state3 = property2.GetState("state3");

            var package1 = CreateContentPackage(new Collection<PropertyState> {state1, state3});
            var package2 = CreateContentPackage(new Collection<PropertyState> {state2, state3});

            //
            //storing to repositories
            UnitOfWork.GetRepository<Property>().Insert(property1);
            UnitOfWork.GetRepository<Property>().Insert(property2);

            UnitOfWork.GetRepository<ContentPackage>().Insert(package1);
            UnitOfWork.GetRepository<ContentPackage>().Insert(package2);

            UnitOfWork.Save();
        }
    }


    public class SearchServiceTests : SearchServiceTestFixture
    {
        [Theory]
        [InlineData(new object[] {"state1", "state3", 1})]
        [InlineData(new object[] {"state2", "state3", 1})]
        [InlineData(new object[] {"", "state3", 2})]
        public void FindPackageWithSamePropertyStates_Simple_PackageFounded(string firstPropertyState,
                                                                            string secondPropertyState,
                                                                            int count)
        {
            // Arrange
            CreateAndStoreTwoInitalizedPackages();

            var states = new Dictionary<int, string> {{1, firstPropertyState}, {2, secondPropertyState}};
            var request = new FindPackagesByPropertyStatesRequest(states);

            var searchService = CreateService();

            // Act
            var response = searchService.FindPackagesByPropertyStates(request);

            // Assert
            response.Packages.Should().HaveCount(count);
        }


        [Theory]
        [InlineData(1, 1, "11")]
        [InlineData(2, 2, "21")]
        [InlineData(2, 2, "22")]
        [InlineData(3, 1, "32")]
        public void GetBoundedStates_Simple_BounedeStatesReturned(int propertyId,
                                                                  int boundedStatesCount,
                                                                  string expectedState)
        {
            // Arrange
            var property1 = CreateProperty("property1", 1);
            var property2 = CreateProperty("property2", 2);
            var property3 = CreateProperty("property3", 3);

            UnitOfWork.GetRepository<Property>().Insert(property1);
            UnitOfWork.GetRepository<Property>().Insert(property2);
            UnitOfWork.GetRepository<Property>().Insert(property3);

            var state11 = property1.GetState("11");
            var state12 = property1.GetState("12");
            var state21 = property2.GetState("21");
            var state22 = property2.GetState("22");
            var state31 = property3.GetState("31");
            var state32 = property3.GetState("32");

            var contentPackage1 =
                CreateContentPackage(new Collection<PropertyState> {state11, state21, state32});
            var contentPackage2 =
                CreateContentPackage(new Collection<PropertyState> {state11, state22, state31});
            UnitOfWork.GetRepository<ContentPackage>().Insert(contentPackage1);
            UnitOfWork.GetRepository<ContentPackage>().Insert(contentPackage2);
            UnitOfWork.Save();

            var request = new GetBoundedStatesRequest();
            request.States[2] = "21";
            request.PropertyID = propertyId;

            var searchService = CreateService();

            // Act
            var response = searchService.GetBoundedStates(request);

            // Assert
            response.States.Should().HaveCount(boundedStatesCount);
            response.States.Should().Contain(expectedState);
        }
    }
}