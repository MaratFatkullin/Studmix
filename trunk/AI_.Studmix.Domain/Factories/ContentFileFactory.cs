using System;
using AI_.Studmix.Domain.Entities;

namespace AI_.Studmix.Domain.Factories
{
    public class ContentFileFactory
    {
        public ContentFile CreateContentFile(string filename, bool isPreview)
        {
            return new ContentFile
                   {
                       Name = filename,
                       IsPreview = isPreview,
                       GlobalID = Guid.NewGuid()
                   };
        }
    }
}