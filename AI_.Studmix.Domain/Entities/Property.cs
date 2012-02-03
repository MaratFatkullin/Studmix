using System.Collections.Generic;
using System.Linq;

namespace AI_.Studmix.Domain.Entities
{
    public class Property : AggregationRoot
    {
        public string Name { get; set; }

        public int Order { get; set; }

        public bool IsUserProperty { get; set; }

        public virtual ICollection<PropertyState> States { get; set; }

        public PropertyState GetState(string value)
        {
            var existingPropertyStates = States.Where(state => state.Value == value).FirstOrDefault();
            if (existingPropertyStates != null)
                return existingPropertyStates;

            int index = States.Count == 0 ? 1 : States.Max(x => x.Index) + 1;
            var propertyState = new PropertyState(this, value, index);
            States.Add(propertyState);

            return propertyState;
        }
    }
}