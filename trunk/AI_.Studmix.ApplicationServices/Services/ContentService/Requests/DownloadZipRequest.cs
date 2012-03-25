namespace AI_.Studmix.ApplicationServices.Services.ContentService.Requests
{
    public class DownloadZipRequest
    {
        public int PackageID { get; set; }

        public DownloadZipRequest(int id)
        {
            PackageID = id;
        }
    }
}