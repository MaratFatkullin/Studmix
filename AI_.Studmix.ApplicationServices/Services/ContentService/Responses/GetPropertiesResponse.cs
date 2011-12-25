using System.Collections.Generic;
using AI_.Studmix.ApplicationServices.DataTransferObjects;

namespace AI_.Studmix.ApplicationServices.Services.ContentService.Responses
{
    public class GetPropertiesResponse
    {
        public IEnumerable<PropertyDto> Properties { get; set; }

        public GetPropertiesResponse(IEnumerable<PropertyDto> properties)
        {
            Properties = properties;
        }
    }
}