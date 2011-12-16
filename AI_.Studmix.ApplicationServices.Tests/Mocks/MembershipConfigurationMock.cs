using AI_.Studmix.ApplicationServices.Services.MembershipService;

namespace AI_.Studmix.ApplicationServices.Tests.Mocks
{
    public class MembershipConfigurationMock : IMembershipConfiguration
    {
        #region IMembershipConfiguration Members

        public bool RequiresUniqueEmail { get; set; }

        public int MinRequiredPasswordLength { get; set; }

        public int MinRequiredNonAlphanumericCharacters { get; set; }

        public int NewPasswordLength { get; set; }

        public bool RequiresQuestionAndAnswer { get; set; }

        public bool RequiresEmail { get; set; }

        #endregion

        public MembershipConfigurationMock()
        {
            NewPasswordLength = 10;
            MinRequiredPasswordLength = 6;
            MinRequiredNonAlphanumericCharacters = 6;
        }
    }
}