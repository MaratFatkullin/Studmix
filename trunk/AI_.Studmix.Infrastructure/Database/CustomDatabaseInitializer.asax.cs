using System.Collections.ObjectModel;
using System.Data.Entity;
using AI_.Studmix.Domain.Entities;
using AI_.Studmix.Domain.Factories;

namespace AI_.Studmix.Infrastructure.Database
{
    public class CustomDatabaseInitializer : DropCreateDatabaseAlways<DataContext>
    {
        protected override void Seed(DataContext context)
        {
            base.Seed(context);

            var adminRole = new Role
                            {
                                RoleName = "admin"
                            };
            var userRole = new Role
                           {
                               RoleName = "user"
                           };

            context.Set<Role>().Add(adminRole);
            context.Set<Role>().Add(userRole);


            var userFactory = new UserFactory();
            var marat = userFactory.CreateUser("marat", "123", "", "", userRole);
            var admin = userFactory.CreateUser("admin", "123", "", "", adminRole);

            marat.IncomeMoney(1000);
            admin.IncomeMoney(1000);

            context.Set<User>().Add(admin);
            context.Set<User>().Add(marat);

            var countryProp = new Property
                              {
                                  Name = "������",
                                  Order = 1
                              };
            var cityProp = new Property
                           {
                               Name = "�����",
                               Order = 2
                           };
            var instituteType = new Property
                                {
                                    Name = "���",
                                    Order = 3
                                };
            var instituteName = new Property
                                {
                                    Name = "������������ �����������",
                                    Order = 4
                                };
            var studingForm = new Property
                              {
                                  Name = "����� ��������",
                                  Order = 5
                              };
            var faculty = new Property
                          {
                              Name = "���������",
                              Order = 6
                          };
            var course = new Property
                         {
                             Name = "����",
                             Order = 7
                         };
            var group = new Property
                        {
                            Name = "������",
                            Order = 8
                        };
            var disciple = new Property
                           {
                               Name = "����������",
                               Order = 9
                           };
            var book = new Property
                       {
                           Name = "�������\\��� ���.",
                           Order = 10
                       };
            var variant = new Property
                          {
                              Name = "�������",
                              Order = 11
                          };
            var data = new Property
                       {
                           Name = "������\\��.",
                           Order = 12
                       };

            context.Set<Property>().Add(countryProp);
            context.Set<Property>().Add(cityProp);
            context.Set<Property>().Add(instituteType);
            context.Set<Property>().Add(instituteName);
            context.Set<Property>().Add(studingForm);
            context.Set<Property>().Add(faculty);
            context.Set<Property>().Add(course);
            context.Set<Property>().Add(group);
            context.Set<Property>().Add(disciple);
            context.Set<Property>().Add(book);
            context.Set<Property>().Add(variant);
            context.Set<Property>().Add(data);

            var propertyStateFactory = new PropertyStateFactory();
            var russia = propertyStateFactory.CreatePropertyState(countryProp, "������", 1);
            var czech = propertyStateFactory.CreatePropertyState(countryProp, "������� ����������", 2);
            var french = propertyStateFactory.CreatePropertyState(countryProp, "�������", 3);

            context.Set<PropertyState>().Add(russia);
            context.Set<PropertyState>().Add(czech);
            context.Set<PropertyState>().Add(french);

            var moscow = propertyStateFactory.CreatePropertyState(cityProp, "������", 1);
            var kazan = propertyStateFactory.CreatePropertyState(cityProp, "������", 2);
            var prague = propertyStateFactory.CreatePropertyState(cityProp, "�����", 3);
            var paris = propertyStateFactory.CreatePropertyState(cityProp, "�����", 4);
            var marsel = propertyStateFactory.CreatePropertyState(cityProp, "�������", 5);

            context.Set<PropertyState>().Add(moscow);
            context.Set<PropertyState>().Add(kazan);
            context.Set<PropertyState>().Add(prague);
            context.Set<PropertyState>().Add(paris);
            context.Set<PropertyState>().Add(marsel);

            var contentPackage1 = new ContentPackage {Price = 70, Owner = marat};
            contentPackage1.PropertyStates = new Collection<PropertyState> {russia, moscow};

            var contentPackage2 = new ContentPackage {Owner = marat};
            contentPackage2.PropertyStates = new Collection<PropertyState> {russia, kazan};

            var contentPackage3 = new ContentPackage {Owner = admin};
            contentPackage3.PropertyStates = new Collection<PropertyState> {czech, prague};

            context.Set<ContentPackage>().Add(contentPackage1);
            context.Set<ContentPackage>().Add(contentPackage2);
            context.Set<ContentPackage>().Add(contentPackage3);

            context.SaveChanges();
        }
    }
}