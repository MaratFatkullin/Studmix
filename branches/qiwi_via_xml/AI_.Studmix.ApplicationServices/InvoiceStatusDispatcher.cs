using System;
using System.Collections.Generic;
using System.Threading;
using AI_.Studmix.Domain.Entities;
using AI_.Studmix.Domain.Repository;
using AI_.Studmix.Domain.Services.Abstractions;

namespace AI_.Studmix.ApplicationServices
{
    public class InvoiceStatusDispatcher
    {
        public IUnitOfWork UnitOfWork { get; set; }
        public IPaymentSystemInvoiceRepository Repository { get; set; }

        public InvoiceStatusDispatcher(IUnitOfWork unitOfWork, IPaymentSystemInvoiceRepository repository)
        {
            UnitOfWork = unitOfWork;
            Repository = repository;
        }

        public void Start()
        {
            IDictionary<Guid,InvoiceStatus> statuses;
            var repository = UnitOfWork.GetRepository<Invoice>();
            while (true)
            {
                var expectedInvoices = repository.Get(i => i.Status == InvoiceStatus.Invoiced);
                if (expectedInvoices.Count>0)
                {
                     statuses = Repository.GetInvoiceStatuses(expectedInvoices);
                    foreach (var status in statuses)
                    {
                        if (status.Value != InvoiceStatus.Invoiced)
                        {
                            var invoice = repository.GetByID(status.Key);
                            invoice.Status = status.Value;
                            invoice.UpdateDate = DateTime.Now;
                        }
                    }
                    UnitOfWork.Save();
                    Thread.Sleep(60000 * 3);
                }
            }
        }
    }
}