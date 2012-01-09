using AI_.Studmix.ApplicationServices.Services.InvoiceService.Requests;
using AI_.Studmix.ApplicationServices.Services.InvoiceService.Responses;

namespace AI_.Studmix.ApplicationServices.Services.InvoiceService
{
    public interface IInvoiceService
    {
        ReplenishBalanceResponse ReplenishBalance(ReplenishBalanceRequest request);
    }
}