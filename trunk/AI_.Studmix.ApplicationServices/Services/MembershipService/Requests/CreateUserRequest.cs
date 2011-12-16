namespace AI_.Studmix.ApplicationServices.Services.MembershipService.Requests
{
    public class CreateUserRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PasswordQuestion { get; set; }
        public string PasswordAnswer { get; set; }
        public bool IsApproved { get; set; }

        public CreateUserRequest(string username,
                                 string password,
                                 string email,
                                 string phoneNumber,
                                 string passwordQuestion,
                                 string passwordAnswer,
                                 bool isApproved)
        {
            UserName = username;
            Password = password;
            Email = email;
            PhoneNumber = phoneNumber;
            PasswordQuestion = passwordQuestion;
            PasswordAnswer = passwordAnswer;
            IsApproved = isApproved;
        }
    }
}