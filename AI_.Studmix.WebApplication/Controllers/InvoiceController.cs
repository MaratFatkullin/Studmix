using System.Web.Mvc;
using AI_.Studmix.ApplicationServices.Services.InvoiceService;
using AI_.Studmix.ApplicationServices.Services.InvoiceService.Requests;
using AI_.Studmix.WebApplication.ViewModels.Invoice;
using AI_.Studmix.WebApplication.ViewModels.Shared;

namespace AI_.Studmix.WebApplication.Controllers
{
    [Authorize]
    public class InvoiceController : ControllerBase
    {
        public IInvoiceService InvoiceService { get; set; }

        public InvoiceController(IInvoiceService invoiceService)
        {
            InvoiceService = invoiceService;
        }

        [HttpGet]
        public ActionResult ReplenishBalance()
        {
            return View(new ReplenishBalanceViewModel());
        }

        [HttpPost]
        public ActionResult ReplenishBalance(ReplenishBalanceViewModel viewModel)
        {
            var request = new ReplenishBalanceRequest {Amount = viewModel.Amount,UserName = User.Identity.Name};
            InvoiceService.ReplenishBalance(request);

            return InformationView("Счет на пополнение баланса выставлен",
                                   "На Ваш Qiwi-аккаунт выставлен счет к оплате.\n" +
                                   "После оплаты счета Ваш балнс на STUDMIX будет пополнен на указаную сумму.",
                                   new ActionLinkInfo("Account", "ViewAccount", "Вернуться в личный кабинет"));
        }

        public ViewResult QiwiInstructions()
        {
            return View();
        }
    }
}