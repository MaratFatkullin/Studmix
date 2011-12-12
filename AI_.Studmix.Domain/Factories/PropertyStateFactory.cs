using System.Collections.ObjectModel;
using AI_.Studmix.Domain.Entities;

namespace AI_.Studmix.Domain.Factories
{
    public class PropertyStateFactory
    {
        public PropertyState CreatePropertyState(Property property,
                                                 string value,
                                                 int index,
                                                 bool boundStateToProperty = false)
        {
            var propertyState = new PropertyState(property, value, index);

            if (boundStateToProperty)
            {
                property.States.Add(propertyState);
            }

            return propertyState;
        }
    }
}