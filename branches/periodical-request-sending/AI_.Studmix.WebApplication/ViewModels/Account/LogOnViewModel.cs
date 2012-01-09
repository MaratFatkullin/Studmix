using System.ComponentModel.DataAnnotations;

namespace AI_.Studmix.WebApplication.ViewModels.Account
{
    public class LogOnViewModel
    {
        [Required]
        [Display(Name = "�����")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "������")]
        public string Password { get; set; }

        [Display(Name = "��������� ����")]
        public bool RememberMe { get; set; }
    }
}