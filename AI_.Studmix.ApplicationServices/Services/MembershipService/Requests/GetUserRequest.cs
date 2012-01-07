namespace AI_.Studmix.ApplicationServices.Services.MembershipService.Requests
{
    public class GetUserRequest
    {
        public string UserName { get; set; }

        public GetUserRequest(string userName)
        {
            UserName = userName;
        }
    }
}