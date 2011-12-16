namespace AI_.Studmix.ApplicationServices.Services.DataTransferObjects.MembershipService.Requests
{
    public class ValidateUserRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public ValidateUserRequest(string username, string password)
        {
            UserName = username;
            Password = password;
        }
    }
}