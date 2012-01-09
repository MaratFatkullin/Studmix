using System;
using System.Collections.Generic;
using AI_.Studmix.Domain.Entities;
using AI_.Studmix.Domain.Services.Abstractions;

namespace AI_.Studmix.Domain.Repository
{
    public interface IPaymentSystemInvoiceRepository
    {
        void StoreInvoice(Invoice invoice);
        InvoiceStatus GetInvoiceStatus(Invoice invoice);
        IDictionary<Guid, InvoiceStatus> GetInvoiceStatuses(IEnumerable<Invoice> invoices);
    }
}