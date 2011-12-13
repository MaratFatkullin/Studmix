using System.Collections.Generic;

namespace AI_.Studmix.Domain.Entities
{
    public class PropertyState : Entity
    {
        public Property Property { get; protected set; }

        public string Value { get; protected set; }

        public int Index { get; protected set; }

        public PropertyState(Property property, string value, int index)
        {
            Property = property;
            Value = value;
            Index = index;
        }
    }
}