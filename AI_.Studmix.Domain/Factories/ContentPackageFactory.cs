using System.Collections.Generic;
using System.Collections.ObjectModel;
using AI_.Studmix.Domain.Entities;

namespace AI_.Studmix.Domain.Factories
{
    public class ContentPackageFactory
    {
        public ContentPackage CreateContentPackage(string caption,
                                                   string description,
                                                   User owner,
                                                   int price,
                                                   ICollection<PropertyState> propertyStates,
                                                   ICollection<ContentFile> files)
        {
            var contentPackage = new ContentPackage
                                 {
                                     Caption = caption,
                                     Description = description,
                                     Owner = owner,
                                     Price = price,
                                     PropertyStates = new Collection<PropertyState>(),
                                     Files = new Collection<ContentFile>()
                                 };

            foreach (var contentFile in files)
            {
                contentPackage.Files.Add(contentFile);
                contentFile.ContentPackage = contentPackage;
            }

            foreach (var propertyState in propertyStates)
            {
                contentPackage.PropertyStates.Add(propertyState);
            }
            
            return contentPackage;
        }
    }
}