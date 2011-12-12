using AI_.Studmix.ApplicationServices.Services.DataTransferObjects.ContentService.Requests;
using AI_.Studmix.ApplicationServices.Services.DataTransferObjects.ContentService.Responses;

namespace AI_.Studmix.ApplicationServices.Services.Abstractions
{
    public interface IContentService
    {
        GetPropertiesResponse GetProperties();

        void Store(StoreRequest request);
    }
}