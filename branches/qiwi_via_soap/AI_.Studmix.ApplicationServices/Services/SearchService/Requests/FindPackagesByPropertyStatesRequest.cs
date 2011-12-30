using System.Collections.Generic;

namespace AI_.Studmix.ApplicationServices.Services.SearchService.Requests
{
    public class FindPackagesByPropertyStatesRequest
    {
        public IDictionary<int, string> PropertyStates { get; set; }

        public FindPackagesByPropertyStatesRequest(IDictionary<int, string> propertyStates)
        {
            PropertyStates = propertyStates;
        }
    }
}