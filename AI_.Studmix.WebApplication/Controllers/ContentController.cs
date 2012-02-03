using System.Collections.Generic;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using AI_.Studmix.ApplicationServices.DataTransferObjects;
using AI_.Studmix.ApplicationServices.Services.ContentService;
using AI_.Studmix.ApplicationServices.Services.ContentService.Requests;
using AI_.Studmix.ApplicationServices.Services.MembershipService;
using AI_.Studmix.ApplicationServices.Services.MembershipService.Requests;
using AI_.Studmix.ApplicationServices.Services.SearchService;
using AI_.Studmix.ApplicationServices.Services.SearchService.Requests;
using AI_.Studmix.WebApplication.Infrastructure;
using AI_.Studmix.WebApplication.ViewModels.Content;
using AI_.Studmix.WebApplication.ViewModels.Shared;
using MvcContrib.Pagination;

namespace AI_.Studmix.WebApplication.Controllers
{
    [Authorize]
    public class ContentController : ControllerBase
    {
        protected IContentService ContentService { get; set; }
        protected ISearchService SearchService { get; set; }
        protected IMembershipService MembershipService { get; set; }

        public ContentController(IContentService contentService,
                                 ISearchService searchService,
                                 IMembershipService membershipService)
        {
            ContentService = contentService;
            SearchService = searchService;
            MembershipService = membershipService;
        }

        [HttpGet]
        public ViewResult Upload()
        {
            var response = ContentService.GetProperties();

            var viewModel = new UploadViewModel
                            {
                                Properties = response.Properties
                            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Upload(UploadViewModel viewModel)
        {
            var request = new StoreRequest
                          {
                              Caption = viewModel.Caption,
                              Price = viewModel.Price,
                              Description = viewModel.Description,
                              OwnerUserName = User.Identity.Name,
                              States =
                                  viewModel.States.Select(s => new PropertyStateDto(s.Key, s.Value)).ToList(),
                              ContentFiles = viewModel.ContentFiles
                                  .Where(f => f != null)
                                  .Select(f => new FileStreamDto(f.FileName, f.InputStream)),
                              PreviewContentFiles = viewModel.PreviewContentFiles
                                  .Where(f => f != null)
                                  .Select(f => new FileStreamDto(f.FileName, f.InputStream))
                          };

            ContentService.Store(request);

            return InformationView("Загрузка завершена",
                                   "Контент успешно загружен. Благодарим за использование нашего ресурса.",
                                   new ActionLinkInfo("Content", "Upload", "Вернуться"));
        }

        [HttpPost]
        public JsonResult UpdateStates(Dictionary<int, string> states, int id)
        {
            var request = new GetBoundedStatesRequest {States = states, PropertyID = id};
            var response = SearchService.GetBoundedStates(request);
            return Json(response.States);
        }

        [HttpGet]
        public ViewResult Details(int id)
        {
            var response = ContentService.GetPackageByID(new GetPackageByIDRequest(id, User.Identity.Name));
            var getPropertiesResponse = ContentService.GetProperties();
            var viewModel = new DetailsViewModel
                            {
                                Package = response.ContentPackage,
                                IsFullAccessGranted = response.IsFullAccessGranted,
                                Properties = getPropertiesResponse.Properties
                            };
            return View(viewModel);
        }

        public ActionResult Preview(int id)
        {
            var response = ContentService.DownloadFile(new DownloadRequest(id, User.Identity.Name));
            var webImage = new WebImage(response.File.Stream);
            var resizedImage = webImage.Resize(100, 100, true, false);

            return File(resizedImage.GetBytes(), resizedImage.ImageFormat);
        }

        [HttpGet]
        public ViewResult Search()
        {
            var response = MembershipService.GetUser(new GetUserRequest(User.Identity.Name));
            var viewModel = ViewModelMapper.Map<SearchViewModel>(response);
            return View(viewModel);
        }

        [HttpPost]
        public ViewResult Search(SearchViewModel viewModel)
        {
            var getUserResponse = MembershipService.GetUser(new GetUserRequest(User.Identity.Name));
            viewModel = ViewModelMapper.Map<SearchViewModel>(getUserResponse);
            var request = new FindPackagesByPropertyStatesRequest(viewModel.States);
            var response = SearchService.FindPackagesByPropertyStates(request);

            if (viewModel.PageNumber == null)
                viewModel.PageNumber = 1;
            viewModel.PackagesPagination = response.Packages.AsPagination((int) viewModel.PageNumber, 20);

            return View(viewModel);
        }

        public ActionResult Download(int id)
        {
            var response = ContentService.DownloadFile(new DownloadRequest(id, User.Identity.Name));
            if (response.IsAccessGranted)
            {
                return new FileStreamResult(response.File.Stream, "image/jpeg");
            }
            else
            {
                return ErrorView("Ошибка доступа", "Доступ к скачиванию файла закрыт.");
            }
        }
    }
}