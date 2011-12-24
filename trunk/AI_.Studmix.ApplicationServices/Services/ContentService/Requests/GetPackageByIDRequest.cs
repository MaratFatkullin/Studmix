namespace AI_.Studmix.ApplicationServices.Services.ContentService.Requests
{
    public class GetPackageByIDRequest
    {
        public int ID { get; set; }

        public GetPackageByIDRequest(int id)
        {
            ID = id;
        }
    }
}