using System.Collections.Generic;
using AI_.Studmix.ApplicationServices.DataTransferObjects;

namespace AI_.Studmix.WebApplication.ViewModels.Admin
{
    public class UsersViewModel
    {
        public IEnumerable<UserDto> Users { get; set; }

        public int PageSize { get; set; }
    }
}