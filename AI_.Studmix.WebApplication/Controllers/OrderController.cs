using System.Web.Mvc;
using AI_.Studmix.ApplicationServices.Services.OrderService;
using AI_.Studmix.ApplicationServices.Services.OrderService.Requests;
using AI_.Studmix.WebApplication.ViewModels.Finance;
using AI_.Studmix.WebApplication.ViewModels.Shared;

namespace AI_.Studmix.WebApplication.Controllers
{
    [Authorize]
    public class OrderController : ControllerBase
    {
        protected IOrderService OrderService { get; set; }

        public OrderController(IOrderService orderService)
        {
            OrderService = orderService;
        }

        [HttpGet]
        public ViewResult ViewOrder(int id)
        {
            var request = new ViewOrderRequest {PackageID = id, UserName = User.Identity.Name};
            var responses = OrderService.ViewOrder(request);

            if (!responses.IsUserCanBuyPackage)
                ModelState.AddModelError("", "Для совершения заказа недостаточно средств на счету.");

            var viewModel = new ViewOrderViewModel();
            viewModel.ContentPackageID = id;
            viewModel.OrderPrice = responses.OrderPrice;
            viewModel.UserBalance = responses.UserBalance;
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult MakeOrder(ViewOrderViewModel viewModel)
        {
            var request = new MakeOrderRequest
                          {
                              ContentPackageID = viewModel.ContentPackageID,
                              UserName = User.Identity.Name
                          };
            OrderService.MakeOrder(request);

            return InformationView(
                "Покупка успешно произведена.",
                "Благодарим за использование ресурса. Приобретенный материал доступен через поиск или в личном кабинете.",
                new ActionLinkInfo(Url.Action("Details", "Content", new {id = viewModel.ContentPackageID}),"Вернуться к просмотру"),
                new ActionLinkInfo(Url.Action("ViewAccount", "Account"),"Перейти к личному кабинету"));
        }
    }
}