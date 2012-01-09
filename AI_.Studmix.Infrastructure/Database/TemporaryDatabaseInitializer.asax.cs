using System.Collections.ObjectModel;
using System.Data.Entity;
using AI_.Studmix.Domain.Entities;
using AI_.Studmix.Domain.Factories;

namespace AI_.Studmix.Infrastructure.Database
{
    public class TemporaryDatabaseInitializer : DropCreateDatabaseAlways<DataContext>
    {
        protected override void Seed(DataContext context)
        {
            base.Seed(context);

            #region Users and roles

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

            #endregion

            #region Properties and states

            var propertyFactory = new PropertyFactory();

            var countryProp = propertyFactory.CreateProperty("������", 1);
            context.Set<Property>().Add(countryProp);
            var cityProp = propertyFactory.CreateProperty("�����", 2);
            context.Set<Property>().Add(cityProp);
            context.Set<Property>().Add(propertyFactory.CreateProperty("���", 3));
            context.Set<Property>().Add(propertyFactory.CreateProperty("������������ �����������", 4));
            context.Set<Property>().Add(propertyFactory.CreateProperty("����� ��������", 5));
            context.Set<Property>().Add(propertyFactory.CreateProperty("���������", 6));
            context.Set<Property>().Add(propertyFactory.CreateProperty("����", 7));
            context.Set<Property>().Add(propertyFactory.CreateProperty("������", 8));
            context.Set<Property>().Add(propertyFactory.CreateProperty("����������", 9));
            context.Set<Property>().Add(propertyFactory.CreateProperty("�������\\��� ���.", 10));
            context.Set<Property>().Add(propertyFactory.CreateProperty("�������", 11));
            context.Set<Property>().Add(propertyFactory.CreateProperty("������\\��.", 12));

            var russia = countryProp.GetState("������");
            var czech = countryProp.GetState("������� ����������");
            var franze = countryProp.GetState("�������");

            var moscow = cityProp.GetState("������");
            var kazan = cityProp.GetState("������");
            var prague = cityProp.GetState("�����");
            var paris = cityProp.GetState("�����");
            var marsel = cityProp.GetState("�������");

            #endregion

            #region Packages

            var packageFactory = new ContentPackageFactory();
            var contentPackage1 = packageFactory.CreateContentPackage(
                "",
                "",
                marat,
                70,
                new Collection<PropertyState> {russia, moscow});

            var contentPackage2 = packageFactory.CreateContentPackage(
                "",
                "",
                marat,
                100,
                new Collection<PropertyState> {russia, kazan});

            var contentPackage3 = packageFactory.CreateContentPackage(
                "",
                "",
                admin,
                100,
                new Collection<PropertyState> {czech, prague});

            context.Set<ContentPackage>().Add(contentPackage1);
            context.Set<ContentPackage>().Add(contentPackage2);
            context.Set<ContentPackage>().Add(contentPackage3);

            #endregion

            context.SaveChanges();
        }
    }
}