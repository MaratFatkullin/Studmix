using System.Collections.Generic;
using AI_.Studmix.ApplicationServices.DataTransferObjects;

namespace AI_.Studmix.ApplicationServices.Services.MembershipService.Responses
{
    public class GetUserResponse
    {
        public UserDto User { get; set; }

        public IEnumerable<PropertyDto> Properties { get; set; }

        public IEnumerable<ContentPackageDto> BoughtPackages { get; set; }
    }
}