using AI_.Studmix.Domain.Entities;

namespace AI_.Studmix.Domain.Services.Abstractions
{
    public interface IFinanceService
    {
        bool UserCanBuyPackage(User user, ContentPackage package);
        Order MakeOrder(User user, ContentPackage package);
        void SendInvoiceToUser(User user, decimal amount, string comment);
    }
}