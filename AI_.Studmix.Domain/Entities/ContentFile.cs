using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace AI_.Studmix.Domain.Entities
{
    public class ContentFile : Entity
    {
        public virtual ContentPackage ContentPackage { get; set; }

        public string Name { get; set; }

        public bool IsPreview { get; set; }

        public Guid GlobalID { get; set; }

        public bool IsImage
        {
            get
            {
                var extension = Path.GetExtension(Name);
                var list = new List<string> { "jpeg", "jpg", "bmp", "png", };
                return list.Any(ex => extension.EndsWith(ex, true, CultureInfo.InvariantCulture));
            }
        }
    }
}