using System.Web.Mvc;
using AI_.Studmix.ApplicationServices.Services.MembershipService;
using AI_.Studmix.ApplicationServices.Services.MembershipService.Requests;
using AI_.Studmix.WebApplication.ViewModels.Admin;
using AI_.Studmix.WebApplication.ViewModels.Shared;

namespace AI_.Studmix.WebApplication.Controllers
{
    [Authorize(Users = "admin")]
    public class AdminController : ControllerBase
    {
        private const int PAGE_SIZE = 20;
        protected IMembershipService MembershipService { get; set; }

        public AdminController(IMembershipService membershipService)
        {
            MembershipService = membershipService;
        }

        [HttpGet]
        public ViewResult Users(int id = 1 /*pageIndex*/)
        {
            var request = new GetUserListRequest(PAGE_SIZE, id);
            var response = MembershipService.GetUserList(request);

            var viewModel = new UsersViewModel {PageSize = PAGE_SIZE, Users = response.Users};
            return View(viewModel);
        }

        [HttpGet]
        public ViewResult UserDetails(int id /*userID*/)
        {
            var request = new GetUserRequest(id);
            var response = MembershipService.GetUser(request);
            var viewModel = new UserDetailsViewModel {User = response.User};
            return View(viewModel);
        }

        public ViewResult UpdateUser(UserDetailsViewModel viewModel)
        {
            var request = new UpdateUserRequest {User = viewModel.User};
            MembershipService.UpdateUser(request);

            return InformationView("Обновление завершено",
                                   "Изменение учетной записи прошло успешно.",
                                   new ActionLinkInfo("admin", "Users", "К списку пользователей"));
        }
    }
}