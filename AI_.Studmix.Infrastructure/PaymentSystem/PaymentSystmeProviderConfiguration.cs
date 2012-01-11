using System;
using System.Configuration;

namespace AI_.Studmix.Infrastructure.PaymentSystem
{
    public class PaymentSystmeProviderConfiguration : IPaymentSystmeProviderConfiguration
    {
        public int Login { get; private set; }
        public string Password { get; private set; }
        public string Url { get; private set; }

        public PaymentSystmeProviderConfiguration()
        {
            Login = Convert.ToInt32(ConfigurationManager.AppSettings["QiwiLogin"]);
            Password = ConfigurationManager.AppSettings["QiwiPassword"];
            Url = ConfigurationManager.AppSettings["QiwiUrl"];
        }
    }
}