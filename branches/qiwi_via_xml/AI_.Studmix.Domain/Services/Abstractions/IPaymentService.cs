using AI_.Studmix.Domain.Entities;

namespace AI_.Studmix.Domain.Services.Abstractions
{
    public interface IPaymentService
    {
        decimal GetActualBalance(User user);
        void SendInvoiceToUser(User user, decimal amount, string comment);
    }
}