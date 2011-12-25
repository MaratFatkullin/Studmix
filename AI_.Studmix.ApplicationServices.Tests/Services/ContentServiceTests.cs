using System.Collections.Generic;
using System.IO;
using System.Linq;
using AI_.Studmix.ApplicationServices.DataTransferObjects;
using AI_.Studmix.ApplicationServices.FileRepository;
using AI_.Studmix.ApplicationServices.Services.ContentService;
using AI_.Studmix.ApplicationServices.Services.ContentService.Requests;
using AI_.Studmix.ApplicationServices.Tests.Mocks;
using AI_.Studmix.Domain.Entities;
using AI_.Studmix.Domain.Tests;
using FluentAssertions;
using Moq;
using Xunit;

namespace AI_.Studmix.ApplicationServices.Tests.Services
{
    public class ContentServiceTestFixture : TestFixtureBase
    {
        protected Mock<IFileRepository> FileRepository;
        protected UnitOfWorkMock UnitOfWork;

        public ContentServiceTestFixture()
        {
            UnitOfWork = new UnitOfWorkMock();
            FileRepository = new Mock<IFileRepository>();
        }

        public ContentService CreateSut()
        {
            return new ContentService(UnitOfWork, FileRepository.Object);
        }

        public Stream CreateStream()
        {
            var mock = new Mock<Stream>();
            return mock.Object;
        }
    }


    public class ContentServiceTests : ContentServiceTestFixture
    {
        [Fact]
        public void GetProperties_Simple_PropertiesReturned()
        {
            // Arrange
            var repository = UnitOfWork.GetRepository<Property>();
            repository.Insert(CreateProperty("prop2", 2));
            repository.Insert(CreateProperty("prop1", 1));
            UnitOfWork.Save();

            var service = CreateSut();

            // Act
            var response = service.GetProperties();

            // Assert
            response.Properties.First().Name.Should().Be("prop1");
            response.Properties.Last().Name.Should().Be("prop2");
            response.Properties.Count().Should().Be(2);
        }

        [Fact]
        public void Store_Simple_StoredPackageHasCorrectAttributes()
        {
            // Arrange
            var user = CreateUser();
            UnitOfWork.GetRepository<User>().Insert(user);
            UnitOfWork.Save();

            var request = new StoreRequest
                          {
                              Caption = "caption",
                              Description = "description",
                              OwnerUserName = user.UserName,
                              Price = 100,
                          };

            var service = CreateSut();

            // Act
            service.Store(request);

            // Assert
            var package = UnitOfWork.GetRepository<ContentPackage>().Get().Single();

            package.Description.Should().Be("description");
            package.Caption.Should().Be("caption");
            package.Owner.Should().Be(user);
            package.Price.Should().Be(100);
        }

        [Fact]
        public void Store_Simple_ContentFilesBounedToPackage()
        {
            // Arrange
            var user = CreateUser();
            UnitOfWork.GetRepository<User>().Insert(user);
            UnitOfWork.Save();

            var request = new StoreRequest
                          {
                              OwnerUserName = user.UserName,
                              ContentFiles =
                                  new List<StoreRequest.File>
                                  {new StoreRequest.File("file1", CreateStream())},
                              PreviewContentFiles =
                                  new List<StoreRequest.File>
                                  {new StoreRequest.File("file2", CreateStream())},
                          };

            var service = CreateSut();

            // Act
            service.Store(request);

            // Assert
            var package = UnitOfWork.GetRepository<ContentPackage>().Get().Single();
            package.Files.Should().Contain(f => f.Name == "file1" && !f.IsPreview);
            package.Files.Should().Contain(f => f.Name == "file2" && f.IsPreview);
        }

        [Fact]
        public void Store_Simple_FilesStoredToFileSystem()
        {
            // Arrange
            var user = CreateUser();
            UnitOfWork.GetRepository<User>().Insert(user);

            var property = CreateProperty();
            UnitOfWork.GetRepository<Property>().Insert(property);

            var propertyState = property.GetState("state1");
            UnitOfWork.Save();

            var stream1 = CreateStream();
            var stream2 = CreateStream();
            var request = new StoreRequest
                          {
                              OwnerUserName = user.UserName,
                              States = new List<PropertyStateDto> {new PropertyStateDto(1, "state1")},
                              ContentFiles =
                                  new List<StoreRequest.File> {new StoreRequest.File("filename1", stream1)},
                              PreviewContentFiles =
                                  new List<StoreRequest.File> {new StoreRequest.File("filename2", stream2)}
                          };

            var service = CreateSut();

            // Act
            service.Store(request);

            // Assert
            FileRepository.Verify(r => r.Store(
                It.Is<ContentFile>(
                    f => f.Name == "filename1" && f.ContentPackage.PropertyStates.Single() == propertyState),
                stream1));

            FileRepository.Verify(r => r.Store(
                It.Is<ContentFile>(
                    f => f.Name == "filename2" && f.ContentPackage.PropertyStates.Single() == propertyState),
                stream2));
        }

        [Fact]
        public void Store_PropertyStatesNotExists_PackageHasCreatedPropertyStates()
        {
            // Arrange
            var user = CreateUser();
            UnitOfWork.GetRepository<User>().Insert(user);

            var property1 = CreateProperty();
            var property2 = CreateProperty();
            var propertyRepository = UnitOfWork.GetRepository<Property>();
            propertyRepository.Insert(property1);
            propertyRepository.Insert(property2);
            UnitOfWork.Save();

            var request = new StoreRequest
                          {
                              OwnerUserName = user.UserName,
                              States = new List<PropertyStateDto>
                                       {
                                           new PropertyStateDto(1, "state1"),
                                           new PropertyStateDto(2, "state2")
                                       },
                          };

            var service = CreateSut();

            // Act
            service.Store(request);

            // Assert
            var package = UnitOfWork.GetRepository<ContentPackage>().Get().Single();
            package.PropertyStates.Count.Should().Be(2);

            var state1 = package.PropertyStates.First();
            state1.Property.Should().Be(property1);
            state1.Value.Should().Be("state1");

            var state2 = package.PropertyStates.Last();
            state2.Property.Should().Be(property2);
            state2.Value.Should().Be("state2");
        }

        [Fact]
        public void Store_PropertyStateExists_PackageHasExistingPropertyStates()
        {
            // Arrange
            var user = CreateUser();
            UnitOfWork.GetRepository<User>().Insert(user);

            var property = CreateProperty();
            UnitOfWork.GetRepository<Property>().Insert(property);

            var existingPropertyState = property.GetState("state1");
            UnitOfWork.Save();

            var request = new StoreRequest
                          {
                              OwnerUserName = user.UserName,
                              States =
                                  new List<PropertyStateDto>
                                  {new PropertyStateDto(1, existingPropertyState.Value)},
                          };

            var service = CreateSut();

            // Act
            service.Store(request);

            // Assert
            var package = UnitOfWork.GetRepository<ContentPackage>().Get().Single();
            package.PropertyStates.Single().Should().Be(existingPropertyState);
        }

        [Fact]
        public void GetPackageByID_PackageExists_PackageReturned()
        {
            // Arrange
            var package = CreateContentPackage();
            package.AddFile("filename", false);
            UnitOfWork.GetRepository<ContentPackage>().Insert(package);
            UnitOfWork.Save();

            var service = CreateSut();

            // Act
            var response = service.GetPackageByID(new GetPackageByIDRequest(package.ID));

            // Assert
            response.ContentPackage.Caption.Should().Be(package.Caption);
            response.ContentPackage.ShouldHave().Properties(p => p.Caption,
                                                            p => p.ID,
                                                            p => p.CreateDate,
                                                            p => p.Description,
                                                            p => p.Price).EqualTo(package);
            response.ContentPackage.PropertyStates.Should().HaveCount(package.PropertyStates.Count);
            response.ContentPackage.Files.Should().HaveCount(package.Files.Count);
        }

        [Fact]
        public void GetPackageByID_PackageNotExists_NullReturned()
        {
            // Arrange
            var service = CreateSut();

            // Act
            var response = service.GetPackageByID(new GetPackageByIDRequest(1));

            // Assert
            response.ContentPackage.Should().BeNull();
        }
    }
}