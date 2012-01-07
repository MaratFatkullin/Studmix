using System;
using System.Linq;
using AI_.Studmix.Domain.Entities;
using AI_.Studmix.Domain.Factories;
using AI_.Studmix.Domain.Repository;
using AI_.Studmix.Domain.Services.Abstractions;

namespace AI_.Studmix.Domain.Services
{
    public class FinanceService : IFinanceService
    {
        protected IPaymentSystemInvoiceRepository Repository { get; set; }

        #region IFinanceService Members

        public FinanceService(IPaymentSystemInvoiceRepository repository)
        {
            Repository = repository;
        }

        public bool UserCanBuyPackage(User user, ContentPackage package)
        {
            return GetActualBalance(user) >= package.Price;
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

        public decimal GetActualBalance(User user)
        {
            var unpaidInvoices = user.Invoices.Where(inv => inv.Status == InvoiceStatus.Invoiced);
            foreach (var invoice in unpaidInvoices)
            {
                var status = Repository.GetInvoiceStatus(invoice);
                invoice.Status = status;
                if (status == InvoiceStatus.Paid)
                {
                    user.IncomeMoney(invoice.Amount);
                }
            }
            return user.Balance;
        }

        public void SendInvoiceToUser(User user, decimal amount, string comment)
        {
            var factory = new InvoiceFactory();
            var invoice = factory.CreateInvoice(user, amount, comment);
            Repository.StoreInvoice(invoice);
        }

        #endregion
    }
}