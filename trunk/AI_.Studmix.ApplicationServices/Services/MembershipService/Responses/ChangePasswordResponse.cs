namespace AI_.Studmix.ApplicationServices.Services.DataTransferObjects.MembershipService.Responses
{
    public class ChangePasswordResponse
    {
        public bool ChangePasswordSucceeded { get; set; }

        public ChangePasswordResponse(bool changePasswordSucceeded)
        {
            ChangePasswordSucceeded = changePasswordSucceeded;
        }
    }
}