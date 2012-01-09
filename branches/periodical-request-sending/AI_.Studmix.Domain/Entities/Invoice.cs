using System;
using AI_.Studmix.Domain.Services.Abstractions;

namespace AI_.Studmix.Domain.Entities
{
    public class Invoice : ValueObject
    {
        public string ToAccount { get; set; }

        public decimal Amount { get; set; }

        public Guid TransactionID { get; protected set; }

        public string Comment { get; set; }

        public InvoiceStatus Status { get; set; }

        public Invoice()
        {
            TransactionID = Guid.NewGuid();
        }

        public User User { get; set; }
    }
}