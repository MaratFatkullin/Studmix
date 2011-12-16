using AI_.Studmix.ApplicationServices.Services.SearchService.Requests;
using AI_.Studmix.ApplicationServices.Services.SearchService.Responses;

namespace AI_.Studmix.ApplicationServices.Services.SearchService
{
    public interface ISearchService
    {
        FindPackagesByPropertyStatesResponse FindPackagesByPropertyStates(
            FindPackagesByPropertyStatesRequest request);

        GetBoundedStatesResponse GetBoundedStates(GetBoundedStatesRequest request);
    }
}