using AI_.Studmix.ApplicationServices.DataTransferObjects;

namespace AI_.Studmix.ApplicationServices.Services.ContentService.Responses
{
    public class DownloadZipResponse
    {
        public FileStreamDto File { get; set; }

        public DownloadZipResponse(FileStreamDto file)
        {
            File = file;
        }
    }
}