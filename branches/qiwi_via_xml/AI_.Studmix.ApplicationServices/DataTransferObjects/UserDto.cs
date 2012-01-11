using System;

namespace AI_.Studmix.ApplicationServices.DataTransferObjects
{
    public class UserDto
    {
        public int ID { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string PasswordQuestion { get; set; }

        public string PasswordAnswer { get; set; }

        public bool IsApproved { get; set; }

        public bool IsLocked { get; set; }

        public DateTime? LastActivityDate { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime? LastLoginDate { get; set; }

        public DateTime? LastLockoutDate { get; set; }

        public DateTime? LastPasswordChangedDate { get; set; }

        public decimal Balance { get; set; }

        public string PhoneNumber { get; set; }
    }
}