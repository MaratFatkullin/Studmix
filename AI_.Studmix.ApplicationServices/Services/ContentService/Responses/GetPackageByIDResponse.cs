using AI_.Studmix.ApplicationServices.DataTransferObjects;

namespace AI_.Studmix.ApplicationServices.Services.ContentService.Responses
{
    public class GetPackageByIDResponse
    {
        public ContentPackageDto ContentPackage { get; set; }
        public bool IsFullAccessGranted { get; set; }

        public GetPackageByIDResponse(ContentPackageDto contentPackageDto, bool isFullAccessGranted)
        {
            ContentPackage = contentPackageDto;
            IsFullAccessGranted = isFullAccessGranted;
        }
    }
}