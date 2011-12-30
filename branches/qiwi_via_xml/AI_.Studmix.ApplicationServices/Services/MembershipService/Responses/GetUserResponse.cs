using AI_.Studmix.ApplicationServices.DataTransferObjects;

namespace AI_.Studmix.ApplicationServices.Services.MembershipService.Responses
{
    public class GetUserResponse
    {
        public UserDto User { get; set; }

        public GetUserResponse(UserDto user)
        {
            User = user;
        }
    }
}