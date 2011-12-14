using AI_.Studmix.ApplicationServices.Services.DataTransferObjects.SearchService.Requests;
using AI_.Studmix.ApplicationServices.Services.DataTransferObjects.SearchService.Responses;

namespace AI_.Studmix.ApplicationServices.Services.Abstractions
{
    public interface ISearchService
    {
        FindPackagesByPropertyStatesResponse FindPackagesByPropertyStates(
            FindPackagesByPropertyStatesRequest request);

        GetBoundedStatesResponse GetBoundedStates(GetBoundedStatesRequest request);
    }
}