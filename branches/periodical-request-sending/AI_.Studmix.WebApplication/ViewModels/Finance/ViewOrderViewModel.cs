using System.ComponentModel;

namespace AI_.Studmix.WebApplication.ViewModels.Finance
{
    public class ViewOrderViewModel
    {
        [DisplayName("Баланс пользователя")]
        public decimal UserBalance { get; set; }

        [DisplayName("Общая стоимость заказа")]
        public decimal OrderPrice { get; set; }

        public int ContentPackageID { get; set; }
    }
}