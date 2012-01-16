using System.Collections.Generic;
using AI_.Studmix.ApplicationServices.DataTransferObjects;
using AI_.Studmix.Domain.Entities;

namespace AI_.Studmix.ApplicationServices.Services.MembershipService.Responses
{
    public class GetUserListResponse
    {
        public GetUserListResponse(IEnumerable<UserDto> users, int totalUsers)
        {
            Users = users;
            TotalUsers = totalUsers;
        }

        public IEnumerable<UserDto> Users { get; set; }

        public int TotalUsers { get; set; }
    }
}