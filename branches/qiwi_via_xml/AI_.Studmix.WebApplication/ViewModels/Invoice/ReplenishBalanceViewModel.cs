using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;

namespace AI_.Studmix.WebApplication.ViewModels.Invoice
{
    public class ReplenishBalanceViewModel
    {
        [Required]
        [Display(Name = "Сумма")]
        [Min(0.01)]
        public decimal Amount { get; set; }
    }
}