using System.Collections.Generic;

namespace AI_.Studmix.Domain.Entities
{
    public class PropertyState : ValueObject
    {
        public Property Property { get; protected set; }

        public string Value { get; protected set; }

        public int Index { get; protected set; }

        public ICollection<ContentPackage> Packages { get; set; }

        public PropertyState(Property property, string value, int index)
        {
            Property = property;
            Value = value;
            Index = index;
        }

        public PropertyState()
        {
        }
    }
}