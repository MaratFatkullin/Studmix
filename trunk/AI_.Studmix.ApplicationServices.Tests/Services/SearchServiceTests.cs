using System.Collections.Generic;
using System.Collections.ObjectModel;
using AI_.Studmix.ApplicationServices.Services;
using AI_.Studmix.ApplicationServices.Services.DataTransferObjects.SearchService.Requests;
using AI_.Studmix.ApplicationServices.Tests.Mocks;
using AI_.Studmix.Domain.Entities;
using AI_.Studmix.Domain.Tests;
using Xunit;
using Xunit.Extensions;
using FluentAssertions;

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

            UnitOfWork.GetRepository<PropertyState>().Insert(state1);
            UnitOfWork.GetRepository<PropertyState>().Insert(state2);
            UnitOfWork.GetRepository<PropertyState>().Insert(state3);

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
            var request = new FindPackagesWithSamePropertyStatesRequest(states);

            var searchService = CreateService();

            // Act
            var response = searchService.FindPackagesWithSamePropertyStates(request);

            // Assert
            response.Packages.Should().HaveCount(count);
        }
    }
}