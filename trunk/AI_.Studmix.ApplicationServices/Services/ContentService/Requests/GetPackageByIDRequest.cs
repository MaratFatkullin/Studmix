namespace AI_.Studmix.ApplicationServices.Services.ContentService.Requests
{
    public class GetPackageByIDRequest
    {
        public int ID { get; set; }

        public string UserName { get; set; }

        public GetPackageByIDRequest(int id, string username)
        {
            ID = id;
            UserName = username;
        }
    }
}