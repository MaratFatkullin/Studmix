using System;

namespace AI_.Studmix.Domain.Entities
{
    public class ContentFile : Entity
    {
        public virtual ContentPackage ContentPackage { get; set; }

        public string Name { get; set; }

        public bool IsPreview { get; set; }

        public Guid GlobalID { get; set; }
    }
}