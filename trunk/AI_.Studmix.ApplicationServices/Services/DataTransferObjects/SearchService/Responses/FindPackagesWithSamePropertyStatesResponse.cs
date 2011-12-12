using System.Collections.Generic;

namespace AI_.Studmix.ApplicationServices.Services.DataTransferObjects.SearchService.Responses
{
    public class FindPackagesWithSamePropertyStatesResponse
    {
        public IDictionary<int, string> Packages { get; set; }
    }
}