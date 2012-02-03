using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AI_.Studmix.ApplicationServices.DataTransferObjects;
using AI_.Studmix.ApplicationServices.Services.ContentService;
using AI_.Studmix.ApplicationServices.Services.ContentService.Requests;
using AI_.Studmix.ApplicationServices.Services.ContentService.Responses;
using AI_.Studmix.ApplicationServices.Services.MembershipService;
using AI_.Studmix.ApplicationServices.Services.MembershipService.Requests;
using AI_.Studmix.ApplicationServices.Services.MembershipService.Responses;
using AI_.Studmix.ApplicationServices.Services.SearchService;
using AI_.Studmix.ApplicationServices.Services.SearchService.Requests;
using AI_.Studmix.ApplicationServices.Services.SearchService.Responses;
using AI_.Studmix.WebApplication.Controllers;
using AI_.Studmix.WebApplication.ViewModels.Content;
using FluentAssertions;
using Moq;
using Xunit;
using Xunit.Extensions;

namespace AI_.Studmix.WebApplication.Tests.Controllers
{
    public class ContentControllerTestFixture : TestFixtureBase
    {
        protected Mock<IContentService> ContentService = new Mock<IContentService>();
        protected Mock<ISearchService> SearchService = new Mock<ISearchService>();
        protected Mock<IMembershipService> MembershipService = new Mock<IMembershipService>();

        public ContentController CreateSut()
        {
            return new ContentController(ContentService.Object, SearchService.Object, MembershipService.Object);
        }
    }


    public class ContentControllerTests : ContentControllerTestFixture
    {
        [Fact]
        public void UpdateStates_Simple_StatesReturned()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>();
            SearchService.Setup(s => s.GetBoundedStates(
                It.Is<GetBoundedStatesRequest>(r => r.States == dictionary)))
                .Returns(new GetBoundedStatesResponse(new List<string> {"val1", "val2"}));

            var controller = CreateSut();

            // Act
            var result = controller.UpdateStates(dictionary, 2);

            // Assert
            var states = (IEnumerable<string>) result.Data;
            states.Should().Contain("val1");
            states.Should().Contain("val2");
        }

        [Fact]
        public void Upload_Simple_PackageUploaded()
        {
            // Arrange
            ContentService.Setup(s => s.GetProperties())
                .Returns(new GetPropertiesResponse(new List<PropertyDto> {new PropertyDto(1, "st1")}));
            var controller = CreateSut();

            // Act
            var result = controller.Upload();

            // Assert
            var model = (UploadViewModel) result.Model;
            model.Properties.Single().Name.Should().Be("st1");
            result.ViewName.Should().BeEmpty();
        }

        [Fact]
        public void UploadPost_Simple_PackageStored()
        {
            // Arrange
            var stream1 = CreateStream();
            var stream2 = CreateStream();

            var viewModel = new UploadViewModel
                            {
                                Caption = "caption",
                                Description = "description",
                                Price = 20,
                                ContentFiles = new List<HttpPostedFileBase>
                                               {
                                                   CreatePostedFile(stream1, "file1")
                                               },
                                PreviewContentFiles = new List<HttpPostedFileBase>
                                                      {
                                                          CreatePostedFile(stream2, "file2")
                                                      },
                                States = new Dictionary<int, string> {{2, "state"}}
                            };

            var controller = CreateSut();
            controller.ControllerContext = CreateContext("username");

            // Act
            var result = controller.Upload(viewModel);

            // Assert
            ContentService.Verify(s => s.Store(
                It.Is<StoreRequest>(r => r.Caption == "caption"
                                         && r.Description == "description"
                                         && r.Price == 20
                                         && r.OwnerUserName == "username"
                                         && r.ContentFiles.Single().FileName == "file1"
                                         && r.ContentFiles.Single().Stream == stream1
                                         && r.PreviewContentFiles.Single().FileName == "file2"
                                         && r.PreviewContentFiles.Single().Stream == stream2
                                         && r.States.Single().Key == 2
                                         && r.States.Single().Value == "state"
                    )));
        }

        [Fact]
        public void Search_Simple_PropertiesPassed()
        {
            // Arrange
            var properties = new List<PropertyDto>();
            MembershipService.Setup(s => s.GetUser(It.Is<GetUserRequest>(r=>r.UserName=="user")))
                .Returns(new GetUserResponse {Properties = properties});

            var controller = CreateSut();
            controller.ControllerContext = CreateContext("user");

            // Act
            var result = controller.Search();

            // Assert
            var viewModel = (SearchViewModel) result.Model;
            viewModel.Properties.Should().Equal(properties);
        }

        [Fact]
        public void Search_Simple_PackagesPassed()
        {
            // Arrange
            var properties = new List<PropertyDto>();
            ContentService.Setup(s => s.GetProperties())
                .Returns(new GetPropertiesResponse(properties));

            var states = new Dictionary<int, string>();
            var contentPackageInfos = new List<ContentPackageDto>();
            SearchService.Setup(s => s.FindPackagesByPropertyStates(
                It.Is<FindPackagesByPropertyStatesRequest>(r => r.PropertyStates == states)))
                .Returns(new FindPackagesByPropertyStatesResponse {Packages = contentPackageInfos});

            var viewModel = new SearchViewModel {States = states};

            var controller = CreateSut();

            // Act
            var result = controller.Search(viewModel);

            // Assert
            var resultViewModel = (SearchViewModel) result.Model;
            resultViewModel.PackagesPagination.Should().Equal(contentPackageInfos);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Details_Simple_DetailsProvided(bool accessGranted)
        {
            // Arrange
            var properties = new List<PropertyDto>();
            var package = new ContentPackageDto();

            ContentService.Setup(s => s.GetProperties())
                .Returns(new GetPropertiesResponse(properties));

            ContentService.Setup(s => s.GetPackageByID(
                It.Is<GetPackageByIDRequest>(r => r.ID == 3
                                                  && r.UserName == "username")))
                .Returns(new GetPackageByIDResponse(package, accessGranted));

            var controller = CreateSut();
            controller.ControllerContext = CreateContext("username");

            // Act
            var result = controller.Details(3);

            // Assert
            var viewModel = (DetailsViewModel) result.Model;
            viewModel.Package.Should().Be(package);
            viewModel.Properties.Should().Equal(properties);
            viewModel.IsFullAccessGranted.Should().Be(accessGranted);
        }

        [Fact]
        public void Download_AccessGranted_FileStreamProvided()
        {
            // Arrange
            var stream = CreateStream();
            ContentService.Setup(
                s => s.DownloadFile(It.Is<DownloadRequest>(r => r.FileID == 5 && r.UserName == "username")))
                .Returns(new DownloadResponse(new FileStreamDto("filename", stream)));

            var controller = CreateSut();
            controller.ControllerContext = CreateContext("username");

            // Act
            var result = controller.Download(5);

            // Assert
            var fileStreamResult = (FileStreamResult) result;
            fileStreamResult.FileStream.Should().Be(stream);
        }

        [Fact]
        public void Download_AccessDenied_ErrorMessageShown()
        {
            // Arrange
            ContentService.Setup(s => s.DownloadFile(It.IsAny<DownloadRequest>()))
                .Returns(new DownloadResponse());
            var controller = CreateSut();
            controller.ControllerContext = CreateContext("username");

            // Act
            var result = controller.Download(5);

            // Assert
            var viewResult = (ViewResult) result;
            viewResult.ViewName.Should().Be("ApplicationError");
        }
    }
}