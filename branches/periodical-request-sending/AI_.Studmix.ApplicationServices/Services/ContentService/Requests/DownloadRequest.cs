namespace AI_.Studmix.ApplicationServices.Services.ContentService.Requests
{
    public class DownloadRequest
    {
        public int FileID { get; set; }

        public string UserName { get; set; }

        public DownloadRequest(int id, string userName)
        {
            FileID = id;
            UserName = userName;
        }
    }
}