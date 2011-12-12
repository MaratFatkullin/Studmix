using AI_.Studmix.Domain.Entities;

namespace AI_.Studmix.Domain.Services.Abstractions
{
    public interface IFinanceService
    {
        bool IsOrderAvailable(Order order);
        bool UserCanBuyPackage(User user, ContentPackage package);
        Order MakeOrder(User user, ContentPackage package);
    }
}