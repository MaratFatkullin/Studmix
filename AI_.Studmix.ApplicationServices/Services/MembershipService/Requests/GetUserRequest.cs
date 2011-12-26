namespace AI_.Studmix.ApplicationServices.Services.MembershipService.Requests
{
    public class GetUserRequest
    {
        public int UserID { get; set; }

        public GetUserRequest(int userID)
        {
            UserID = userID;
        }
    }
}