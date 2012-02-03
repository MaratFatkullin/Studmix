using System.Collections.Generic;
using AI_.Studmix.ApplicationServices.DataTransferObjects;

namespace AI_.Studmix.WebApplication.ViewModels.Account
{
    public class ViewAccountViewModel
    {
        public UserDto User { get; set; }

        public IEnumerable<PropertyDto> Properties { get; set; }

        /// <summary>
        ///   Словарь пары ID свойства - состояние.
        /// </summary>
        public Dictionary<int, string> States { get; set; }

        public ViewAccountViewModel()
        {
            States = new Dictionary<int, string>();
        }
    }
}