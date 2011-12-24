using AI_.Studmix.ApplicationServices.DataTransferObjects;

namespace AI_.Studmix.ApplicationServices.Services.ContentService.Responses
{
    public class GetPackageByIDResponse
    {
        public GetPackageByIDResponse(ContentPackageDto contentPackageDto)
        {
            ContentPackage = contentPackageDto;
        }

        public ContentPackageDto ContentPackage { get; set; }
    }
}