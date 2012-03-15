using System;
using System.Collections.Generic;
using System.Threading;
using AI_.Studmix.Domain.Entities;
using AI_.Studmix.Domain.Repository;
using AI_.Studmix.Domain.Services.Abstractions;
using System.Linq;

namespace AI_.Studmix.ApplicationServices
{
    public class InvoiceStatusDispatcher
    {
        protected IUnitOfWork UnitOfWork { get; set; }
        protected IPaymentSystemInvoiceRepository Repository { get; set; }
        protected ILogger Logger { get; set; }

        public InvoiceStatusDispatcher(IUnitOfWork unitOfWork,
                                       IPaymentSystemInvoiceRepository repository, ILogger logger)
        {
            UnitOfWork = unitOfWork;
            Repository = repository;
            Logger = logger;
        }

        public void Start()
        {
            IDictionary<Guid, InvoiceStatus> statuses;
            var repository = UnitOfWork.GetRepository<Invoice>();
            while (true)
            {
                try
                {
                    var expectedInvoices = repository.Get(i => i.Status == (int)InvoiceStatus.Invoiced);
                    if (expectedInvoices.Count > 0)
                    {
                        statuses = Repository.GetInvoiceStatuses(expectedInvoices);
                        foreach (var status in statuses)
                        {
                            if (status.Value == InvoiceStatus.Paid)
                            {
                                var invoice = repository.Get(i => i.TransactionID == status.Key).Single();
                                invoice.MarkAsPaid();
                            }
                        }
                        UnitOfWork.Save();
                    }
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }
                Thread.Sleep((int) (new TimeSpan(0, 0, 10).TotalMilliseconds * 1));
            }
        }
    }
}