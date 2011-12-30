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
                                                   ICollection<PropertyState> propertyStates)
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

            foreach (var propertyState in propertyStates)
            {
                contentPackage.PropertyStates.Add(propertyState);
            }
            
            return contentPackage;
        }
    }
}