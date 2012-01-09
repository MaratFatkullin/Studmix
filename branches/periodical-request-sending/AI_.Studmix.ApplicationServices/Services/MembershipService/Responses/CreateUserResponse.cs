using System.Web.Security;

namespace AI_.Studmix.ApplicationServices.Services.MembershipService.Responses
{
    public class CreateUserResponse
    {
        public MembershipCreateStatus Status { get; set; }

        public string UserName { get; set; }

        public CreateUserResponse(MembershipCreateStatus status)
        {
            Status = status;
        }

        public CreateUserResponse(MembershipCreateStatus status, string userName)
        {
            Status = status;
            UserName = userName;
        }
    }
}