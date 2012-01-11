using AI_.Studmix.Domain.Entities;
using AI_.Studmix.Domain.Services.Abstractions;

namespace AI_.Studmix.Domain.Factories
{
    public class InvoiceFactory
    {
        public Invoice CreateInvoice(User user, decimal amount, string comment)
        {
            var invoice = new Invoice
                          {
                              User = user,
                              Amount = amount,
                              Comment = comment,
                              Status = (int) InvoiceStatus.Invoiced
                          };
            user.Invoices.Add(invoice);
            return invoice;
        }
    }
}