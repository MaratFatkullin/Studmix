using AI_.Studmix.ApplicationServices.DataTransferObjects;

namespace AI_.Studmix.ApplicationServices.Services.ContentService.Responses
{
    public class DownloadResponse
    {
        public bool IsAccessGranted { get; set; }
        public FileStreamDto File { get; set; }

        public DownloadResponse(FileStreamDto file)
        {
            File = file;
            IsAccessGranted = true;
        }

        public DownloadResponse()
        {
            IsAccessGranted = false;
        }
    }
}