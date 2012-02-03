namespace AI_.Studmix.ApplicationServices.Services.MembershipService.Requests
{
    public class GetUserRequest
    {
        public string UserName { get; set; }

        public bool NeedAllProperties { get; set; }

        public GetUserRequest(string userName, bool needAllProperties = true)
        {
            UserName = userName;
            NeedAllProperties = needAllProperties;
        }
    }
}