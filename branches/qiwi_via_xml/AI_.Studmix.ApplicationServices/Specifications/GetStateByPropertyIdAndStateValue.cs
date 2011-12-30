using AI_.Studmix.Domain.Entities;
using AI_.Studmix.Domain.Repository;

namespace AI_.Studmix.ApplicationServices.Specifications
{
    public class GetStateByPropertyIdAndStateValue : Specification<PropertyState>
    {
        public GetStateByPropertyIdAndStateValue(int propertyId, string stateValue)
        {
            Filter = p => (p.Property.ID == propertyId && p.Value == stateValue);
        }
    }
}