namespace AI_.Studmix.ApplicationServices.Services.MembershipService
{
    public interface IMembershipConfiguration
    {
        bool RequiresUniqueEmail { get; }
        int MinRequiredPasswordLength { get; }
        int MinRequiredNonAlphanumericCharacters { get; }
        int NewPasswordLength { get; }
        bool RequiresQuestionAndAnswer { get; }
        bool RequiresEmail { get; }
    }
}