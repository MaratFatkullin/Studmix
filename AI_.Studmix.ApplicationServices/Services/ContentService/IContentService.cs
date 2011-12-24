using AI_.Studmix.ApplicationServices.Services.ContentService.Requests;
using AI_.Studmix.ApplicationServices.Services.ContentService.Responses;

namespace AI_.Studmix.ApplicationServices.Services.ContentService
{
    public interface IContentService
    {
        GetPropertiesResponse GetProperties();
        void Store(StoreRequest request);
        GetPackageByIDResponse GetPackageByID(GetPackageByIDRequest request);
    }
}