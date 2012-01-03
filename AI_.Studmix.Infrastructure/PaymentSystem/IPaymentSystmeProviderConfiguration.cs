namespace AI_.Studmix.Infrastructure.PaymentSystem
{
    public interface IPaymentSystmeProviderConfiguration
    {
        int Login { get; }
        string Password { get; }
        string Url { get; }
    }
}