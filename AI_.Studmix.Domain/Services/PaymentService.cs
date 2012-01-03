using System.Linq;
using AI_.Studmix.Domain.Entities;
using AI_.Studmix.Domain.Factories;
using AI_.Studmix.Domain.Repository;
using AI_.Studmix.Domain.Services.Abstractions;

namespace AI_.Studmix.Domain.Services
{
    public class PaymentService : IPaymentService
    {
        protected IPaymentSystemInvoiceRepository Repository { get; set; }

        public PaymentService(IPaymentSystemInvoiceRepository repository)
        {
            Repository = repository;
        }

        #region IPaymentService Members

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
            var invoice = factory.CreateInvoice(user,amount,comment);
            Repository.StoreInvoice(invoice);
        }

        #endregion
    }
}