using AI_.Studmix.ApplicationServices.DataTransferObjects;
using MvcContrib.Pagination;

namespace AI_.Studmix.WebApplication.ViewModels.Admin
{
    public class UsersViewModel
    {
        public IPagination<UserDto> UsersPagination { get; set; }
    }
}