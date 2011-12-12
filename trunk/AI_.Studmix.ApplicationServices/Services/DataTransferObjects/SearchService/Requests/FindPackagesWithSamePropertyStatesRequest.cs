using System.Collections.Generic;
using AI_.Studmix.Domain.Entities;

namespace AI_.Studmix.ApplicationServices.Services.DataTransferObjects.SearchService.Requests
{
    public class FindPackagesWithSamePropertyStatesRequest
    {
        public IDictionary<int, string> PropertyStates { get; set; }

        public FindPackagesWithSamePropertyStatesRequest(IDictionary<int, string> propertyStates)
        {
            PropertyStates = propertyStates;
        }
    }
}