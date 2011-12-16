using AI_.Studmix.ApplicationServices.Services.Abstractions;

namespace AI_.Studmix.ApplicationServices.Services
{
    public class MembershipConfiguration : IMembershipConfiguration
    {
        #region IMembershipConfiguration Members

        public bool RequiresUniqueEmail
        {
            get { return false; }
        }

        public int MinRequiredPasswordLength
        {
            get { return 6; }
        }

        public int MinRequiredNonAlphanumericCharacters
        {
            get { return 4; }
        }

        public int NewPasswordLength
        {
            get { return 12; }
        }

        public bool RequiresQuestionAndAnswer
        {
            get { return false; }
        }

        public bool RequiresEmail
        {
            get { return true; }
        }

        #endregion
    }
}