using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AI_.Studmix.ApplicationServices.DataTransferObjects;
using AI_.Studmix.ApplicationServices.Services.ContentService;
using AI_.Studmix.ApplicationServices.Services.ContentService.Requests;
using AI_.Studmix.ApplicationServices.Services.SearchService;
using AI_.Studmix.ApplicationServices.Services.SearchService.Requests;
using AI_.Studmix.WebApplication.ViewModels.Content;

namespace AI_.Studmix.WebApplication.Controllers
{
    [Authorize]
    public class ContentController : Controller
    {
        protected IContentService ContentService { get; set; }
        protected ISearchService SearchService { get; set; }

        public ContentController(IContentService contentService, ISearchService searchService)
        {
            ContentService = contentService;
            SearchService = searchService;
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
        public JsonResult UpdateStates(Dictionary<int, string> states, int id)
        {
            var request = new GetBoundedStatesRequest {States = states, PropertyID = id};
            var response = SearchService.GetBoundedStates(request);
            return Json(response.States);
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
                                  .Select(f => new StoreRequest.File(f.FileName, f.InputStream)),
                              PreviewContentFiles = viewModel.PreviewContentFiles
                                  .Where(f => f != null)
                                  .Select(f => new StoreRequest.File(f.FileName, f.InputStream))
                          };

            ContentService.Store(request);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ViewResult Details(int id)
        {
            var response = ContentService.GetPackageByID(new GetPackageByIDRequest(id));
            var getPropertiesResponse = ContentService.GetProperties();
            var viewModel = new DetailsViewModel
                            {
                                Package = response.ContentPackage,
                                IsFullAccessGranted = false,
                                Properties = getPropertiesResponse.Properties
                            };
            return View(viewModel);
        }

        [HttpGet]
        public ViewResult Search()
        {
            var viewModel = new SearchViewModel();
            var response = ContentService.GetProperties();
            viewModel.Properties = response.Properties;

            return View(viewModel);
        }

        [HttpPost]
        public ViewResult Search(SearchViewModel viewModel)
        {
            var getPropertiesResponse = ContentService.GetProperties();
            viewModel.Properties = getPropertiesResponse.Properties;

            var request = new FindPackagesByPropertyStatesRequest(viewModel.States);
            var response = SearchService.FindPackagesByPropertyStates(request);
            viewModel.Packages = response.Packages;

            return View(viewModel);
        }

        public ActionResult Download(int id)
        {
            throw new NotImplementedException();
        }
    }
}