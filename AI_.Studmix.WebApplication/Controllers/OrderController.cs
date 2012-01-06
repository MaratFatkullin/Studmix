using System;
using System.Web.Mvc;
using AI_.Studmix.ApplicationServices.Services.OrderService;
using AI_.Studmix.ApplicationServices.Services.OrderService.Requests;
using AI_.Studmix.WebApplication.ViewModels.Finance;

namespace AI_.Studmix.WebApplication.Controllers
{
    public class OrderController : Controller
    {
        protected IOrderService OrderService { get; set; }

        public OrderController(IOrderService orderService)
        {
            OrderService = orderService;
        }

        public ViewResult ViewOrder(int id)
        {
            var request = new ViewOrderRequest {PackageID = id,UserName = User.Identity.Name};
            var responses = OrderService.ViewOrder(request);

            if(!responses.IsUserCanBuyPackage)
                ModelState.AddModelError("","Для совершения заказа недостаточно средств на счету.");

            var viewModel = new ViewOrderViewModel();
            viewModel.ContentPackageID = id;
            viewModel.OrderPrice = responses.OrderPrice;
            viewModel.UserBalance = responses.UserBalance;
            return View(viewModel);
        }

        public ActionResult MakeOrder()
        {
            throw new NotImplementedException();
        }
    }
}