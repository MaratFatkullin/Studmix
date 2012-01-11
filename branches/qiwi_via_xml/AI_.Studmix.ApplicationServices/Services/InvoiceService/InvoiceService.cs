using System.Linq;
using AI_.Studmix.ApplicationServices.Services.InvoiceService.Requests;
using AI_.Studmix.ApplicationServices.Services.InvoiceService.Responses;
using AI_.Studmix.ApplicationServices.Specifications;
using AI_.Studmix.Domain.Entities;
using AI_.Studmix.Domain.Repository;
using AI_.Studmix.Domain.Services.Abstractions;

namespace AI_.Studmix.ApplicationServices.Services.InvoiceService
{
    public class InvoiceService : IInvoiceService
    {
        public IUnitOfWork UnitOfWork { get; set; }
        public IFinanceService FinanceService { get; set; }

        public InvoiceService(IUnitOfWork unitOfWork, IFinanceService financeService)
        {
            UnitOfWork = unitOfWork;
            FinanceService = financeService;
        }

        public ReplenishBalanceResponse ReplenishBalance(ReplenishBalanceRequest request)
        {
            var user = UnitOfWork.GetRepository<User>().Get(new GetUserByUserName(request.UserName)).Single();
            FinanceService.SendInvoiceToUser(user, request.Amount, "Studmix.ru. Пополнение баланса.");
            UnitOfWork.Save();
            return new ReplenishBalanceResponse();
        }
    }
}