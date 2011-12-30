using System.Collections.Generic;
using AI_.Studmix.ApplicationServices.DataTransferObjects;

namespace AI_.Studmix.ApplicationServices.Services.SearchService.Responses
{
    public class FindPackagesByPropertyStatesResponse
    {
        public IEnumerable<ContentPackageDto> Packages { get; set; }
    }
}