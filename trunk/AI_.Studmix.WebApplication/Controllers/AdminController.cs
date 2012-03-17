using System.Web.Mvc;
using AI_.Studmix.ApplicationServices.DataTransferObjects;
using AI_.Studmix.ApplicationServices.Services.MembershipService;
using AI_.Studmix.ApplicationServices.Services.MembershipService.Requests;
using AI_.Studmix.WebApplication.ViewModels.Admin;
using AI_.Studmix.WebApplication.ViewModels.Shared;
using MvcContrib.Pagination;

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


            var viewModel = new UsersViewModel
                            {
                                UsersPagination = new CustomPagination<UserDto>(response.Users,
                                                                                id,
                                                                                PAGE_SIZE,
                                                                                2)
                            };
            return View(viewModel);
        }

        [HttpGet]
        public ViewResult UserDetails(string id /*UserName*/)
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
                                   new ActionLinkInfo(Url.Action("Users", "Admin"), "К списку пользователей"));
        }
    }
}