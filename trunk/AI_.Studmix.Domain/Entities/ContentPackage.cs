using System.Collections.Generic;
using AI_.Studmix.Domain.Factories;

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

        public ContentFile AddFile(string filename, bool isPreview)
        {
            var factory = new ContentFileFactory();
            var contentFile = factory.CreateContentFile(filename, isPreview);
            Files.Add(contentFile);
            contentFile.ContentPackage = this;

            return contentFile;
        }
    }
}