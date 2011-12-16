using System.Collections.Generic;

namespace AI_.Studmix.ApplicationServices.Services.ContentService.Responses
{
    public class GetPropertiesResponse
    {
        public Dictionary<int, string> Properties { get; set; }

        public GetPropertiesResponse(Dictionary<int, string> properties)
        {
            Properties = properties;
        }
    }
}