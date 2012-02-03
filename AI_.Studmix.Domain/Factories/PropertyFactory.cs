using System.Collections.ObjectModel;
using AI_.Studmix.Domain.Entities;

namespace AI_.Studmix.Domain.Factories
{
    public class PropertyFactory
    {
        public Property CreateProperty(string name, int order, bool isUserProperty)
        {
            var property = new Property
                           {
                               Name = name,
                               Order = order,
                               IsUserProperty = isUserProperty,
                               States = new Collection<PropertyState>(),
                           };
            return property;
        }
    }
}