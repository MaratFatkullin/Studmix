using System.Collections.Generic;

namespace AI_.Studmix.ApplicationServices.Services.SearchService.Responses
{
    public class GetBoundedStatesResponse
    {
        public IEnumerable<string> States { get; set; }

        public GetBoundedStatesResponse()
        {
            States = new List<string>();
        }

        public GetBoundedStatesResponse(IEnumerable<string> states)
        {
            States = states;
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