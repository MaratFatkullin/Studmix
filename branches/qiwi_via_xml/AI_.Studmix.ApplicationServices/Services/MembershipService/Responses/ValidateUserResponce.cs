namespace AI_.Studmix.ApplicationServices.Services.MembershipService.Responses
{
    public class ValidateUserResponce
    {
        public ValidateUserResponce(bool userIsValid)
        {
            IsValid = userIsValid;
        }

        public bool IsValid { get; set; }
    }
}