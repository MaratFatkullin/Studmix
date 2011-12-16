using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
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
            var request = new GetBoundedStatesRequest();
            request.States = states;
            var response = SearchService.GetBoundedStates(request);
            return Json(response.States.Where(s => s.PropertyID == id).Select(s => s.Value));
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
                              States = viewModel.States.Select(s => new StateInfo(s.Key, s.Value)).ToList(),
                              ContentFiles = viewModel.ContentFiles
                                  .Select(f => new ContentFileInfo(f.FileName, f.InputStream)),
                              PreviewContentFiles = viewModel.PreviewContentFiles
                                  .Select(f => new ContentFileInfo(f.FileName, f.InputStream))
                          };

            ContentService.Store(request);

            return RedirectToAction("Index", "Home");
        }
    }
}