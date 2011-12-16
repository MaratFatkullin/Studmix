using System.Collections.Generic;

namespace AI_.Studmix.ApplicationServices.Services.SearchService.Responses
{
    public class GetBoundedStatesResponse
    {
        public IList<PropertyStateInfo> States { get; set; }

        public GetBoundedStatesResponse()
        {
            States = new List<PropertyStateInfo>();
        }
    }


    public class PropertyStateInfo
    {
        public int PropertyID { get; set; }

        public string Value { get; set; }

        public PropertyStateInfo(int propertyID, string value)
        {
            PropertyID = propertyID;
            Value = value;
        }
    }
}