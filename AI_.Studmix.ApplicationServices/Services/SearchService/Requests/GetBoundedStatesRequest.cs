using System.Collections.Generic;

namespace AI_.Studmix.ApplicationServices.Services.DataTransferObjects.SearchService.Requests
{
    public class GetBoundedStatesRequest
    {
        public IDictionary<int, string> States { get; set; }

        public GetBoundedStatesRequest()
        {
            States = new Dictionary<int, string>();
        }
    }
}