namespace AI_.Studmix.ApplicationServices.Services.DataTransferObjects.MembershipService.Requests
{
    public class ChangePasswordRequest
    {
        public string NewPassword { get; set; }

        public string OldPassword { get; set; }

        public string UserName { get; set; }

        public ChangePasswordRequest(string userName, string oldPassword, string newPassword)
        {
            UserName = userName;
            OldPassword = oldPassword;
            NewPassword = newPassword;
        }
    }
}