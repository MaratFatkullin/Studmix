using System.Collections.Generic;

namespace AI_.Studmix.Domain.Entities
{
    public class ContentPackage : AggregationRoot
    {
        public virtual ICollection<PropertyState> PropertyStates { get; set; }

        public virtual User Owner { get; set; }

        public virtual ICollection<ContentFile> Files { get; set; }

        public string Caption { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string Path { get; set; }

        public void AddPropertyState(PropertyState state)
        {
            PropertyStates.Add(state);
        }

        public void AddContentFile(ContentFile file)
        {
            Files.Add(file);
            file.ContentPackage = this;
        }
    }
}