using System;
using AI_.Studmix.Domain.Entities;
using AI_.Studmix.Domain.Services.Abstractions;

namespace AI_.Studmix.Domain.Services
{
    public class FinanceService : IFinanceService
    {
        #region IFinanceService Members

        public bool UserCanBuyPackage(User user, ContentPackage package)
        {
            return user.Balance >= package.Price;
        }

        public Order MakeOrder(User user, ContentPackage package)
        {
            if (!UserCanBuyPackage(user, package))
            {
                throw new InvalidOperationException("User cannot buy this content.");
            }
            var order = new Order(user, package);
            user.Orders.Add(order);
            user.OutcomeMoney(package.Price);
            package.Owner.IncomeMoney(package.Price);
            return order;
        }

        #endregion
    }
}