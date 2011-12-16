using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AI_.Studmix.Domain.Entities;
using AI_.Studmix.Domain.Factories;

namespace AI_.Studmix.Domain.Tests
{
    public class TestFixtureBase
    {
        protected User CreateUser(Role role)
        {
            var factory = new UserFactory();

            return factory.CreateUser("username", "password", "email", "phoneNumber", role);
        }

        protected User CreateUser()
        {
            var factory = new UserFactory();
            return factory.CreateUser("username", "password", "email", "phoneNumber");
        }

        protected Property CreateProperty(string propertyName = "property", int order = 1, int id = 0)
        {
            var factory = new PropertyFactory();
            var property = factory.CreateProperty(propertyName, order);
            property.ID = id;
            return property;
        }

        protected ContentPackage CreateContentPackage(ICollection<PropertyState> states)
        {
            var factory = new ContentPackageFactory();
            return factory.CreateContentPackage("caption",
                                                "description",
                                                null,
                                                100,
                                                states);
        }

        protected ContentPackage CreateContentPackage(User owner = null)
        {
            var factory = new ContentPackageFactory();
            return factory.CreateContentPackage("caption",
                                                "description",
                                                owner,
                                                100,
                                                new Collection<PropertyState>());
        }

        protected ContentFile CreateContentFile(string filename = "filename",
                                                bool isPreview = false,
                                                Guid globalId = default(Guid))
        {
            var contentFileFactory = new ContentFileFactory();
            var contentFile = contentFileFactory.CreateContentFile(filename, isPreview);
            contentFile.GlobalID = globalId;
            return contentFile;
        }
    }
}