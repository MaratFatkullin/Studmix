using System.Collections.Generic;
using System.IO;
using System.Linq;
using AI_.Studmix.ApplicationServices.DataTransferObjects;
using AI_.Studmix.ApplicationServices.FileRepository;
using AI_.Studmix.ApplicationServices.Services.ContentService;
using AI_.Studmix.ApplicationServices.Services.ContentService.Requests;
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
    public class ContentServiceTestFixture : TestFixtureBase
    {
        protected Mock<IFileRepository> FileRepository;
        protected Mock<IPermissionService> PermissionService;
        protected UnitOfWorkMock UnitOfWork;

        public ContentServiceTestFixture()
        {
            UnitOfWork = new UnitOfWorkMock();
            FileRepository = new Mock<IFileRepository>();
            PermissionService = new Mock<IPermissionService>();
        }

        public ContentService CreateSut()
        {
            return new ContentService(UnitOfWork, FileRepository.Object, PermissionService.Object);
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
                                  new List<FileStreamDto>
                                  {new FileStreamDto("file1", CreateStream())},
                              PreviewContentFiles =
                                  new List<FileStreamDto>
                                  {new FileStreamDto("file2", CreateStream())},
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
                                  new List<FileStreamDto> {new FileStreamDto("filename1", stream1)},
                              PreviewContentFiles =
                                  new List<FileStreamDto> {new FileStreamDto("filename2", stream2)}
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

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetPackageByID_PackageExists_PackageReturned(bool accessGranted)
        {
            // Arrange
            var user = CreateUser();
            var package = CreateContentPackage();
            package.AddFile("filename", false);
            UnitOfWork.GetRepository<ContentPackage>().Insert(package);
            UnitOfWork.GetRepository<User>().Insert(user);
            UnitOfWork.Save();

            PermissionService.Setup(s => s.UserHasPermissions(user, package))
                .Returns(accessGranted);

            var service = CreateSut();

            // Act
            var response = service.GetPackageByID(new GetPackageByIDRequest(package.ID,user.UserName));

            // Assert
            response.ContentPackage.Caption.Should().Be(package.Caption);
            response.ContentPackage.ShouldHave().Properties(p => p.Caption,
                                                            p => p.ID,
                                                            p => p.CreateDate,
                                                            p => p.Description,
                                                            p => p.Price).EqualTo(package);
            response.ContentPackage.PropertyStates.Should().HaveCount(package.PropertyStates.Count);
            response.ContentPackage.Files.Should().HaveCount(package.Files.Count);
            response.IsFullAccessGranted.Should().Be(accessGranted);
        }

        [Fact]
        public void GetPackageByID_PackageNotExists_NullReturned()
        {
            // Arrange
            var service = CreateSut();

            // Act
            var response = service.GetPackageByID(new GetPackageByIDRequest(1,"username"));

            // Assert
            response.ContentPackage.Should().BeNull();
        }

        [Fact]
        public void DownloadFile_UserHasPermissions_FileStreamProvided()
        {
            // Arrange
            var stream = CreateStream();
            var user = CreateUser();
            var package = CreateContentPackage(user);
            var file = package.AddFile("filename", false);
            UnitOfWork.GetRepository<User>().Insert(user);
            UnitOfWork.GetRepository<ContentPackage>().Insert(package);
            UnitOfWork.GetRepository<ContentFile>().Insert(file);
            UnitOfWork.Save();

            PermissionService.Setup(s => s.UserHasPermissions(user, package))
                .Returns(true);

            FileRepository.Setup(r => r.GetFileStream(file))
                .Returns(stream);

            var service = CreateSut();
            var request = new DownloadRequest(1, user.UserName);

            // Act
            var response = service.DownloadFile(request);

            // Assert
            response.File.Stream.Should().Be(stream);
            response.IsAccessGranted.Should().BeTrue();
        }

        [Fact]
        public void DownloadFile_UserHasNoPermissions_FileStreamNotProvided()
        {
            // Arrange
            var user = CreateUser();
            var package = CreateContentPackage(user);
            var file = package.AddFile("filename", false);
            UnitOfWork.GetRepository<User>().Insert(user);
            UnitOfWork.GetRepository<ContentPackage>().Insert(package);
            UnitOfWork.GetRepository<ContentFile>().Insert(file);
            UnitOfWork.Save();

            PermissionService.Setup(s => s.UserHasPermissions(user, package))
                .Returns(false);

            FileRepository.Setup(r => r.GetFileStream(file))
                .Returns(CreateStream());

            var service = CreateSut();
            var request = new DownloadRequest(1, user.UserName);

            // Act
            var response = service.DownloadFile(request);

            // Assert
            response.File.Should().BeNull();
            response.IsAccessGranted.Should().BeFalse();
        }
    }
}