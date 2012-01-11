using System;
using AI_.Studmix.Domain.Services.Abstractions;

namespace AI_.Studmix.Domain.Entities
{
    public class Invoice : ValueObject
    {
        public decimal Amount { get; set; }

        public Guid TransactionID { get; protected set; }

        public string Comment { get; set; }

        public int Status { get; set; }

        public Invoice()
        {
            TransactionID = Guid.NewGuid();
        }

        public virtual User User { get; set; }

        public void MarkAsPaid()
        {
            Status = (int) InvoiceStatus.Paid;
            User.IncomeMoney(Amount);
        }
    }
}