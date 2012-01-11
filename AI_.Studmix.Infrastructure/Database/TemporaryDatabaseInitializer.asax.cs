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

            var countryProp = propertyFactory.CreateProperty("Страна", 1);
            context.Set<Property>().Add(countryProp);
            var cityProp = propertyFactory.CreateProperty("Город", 2);
            context.Set<Property>().Add(cityProp);
            context.Set<Property>().Add(propertyFactory.CreateProperty("Вид", 3));
            context.Set<Property>().Add(propertyFactory.CreateProperty("Наименование учереждения", 4));
            context.Set<Property>().Add(propertyFactory.CreateProperty("Форма обучения", 5));
            context.Set<Property>().Add(propertyFactory.CreateProperty("Факультет", 6));
            context.Set<Property>().Add(propertyFactory.CreateProperty("Курс", 7));
            context.Set<Property>().Add(propertyFactory.CreateProperty("Группа", 8));
            context.Set<Property>().Add(propertyFactory.CreateProperty("Дисциплина", 9));
            context.Set<Property>().Add(propertyFactory.CreateProperty("Учебник\\год вып.", 10));
            context.Set<Property>().Add(propertyFactory.CreateProperty("Вариант", 11));
            context.Set<Property>().Add(propertyFactory.CreateProperty("Данные\\кр.", 12));

            var russia = countryProp.GetState("Россия");
            var czech = countryProp.GetState("Чешская республика");
            var franze = countryProp.GetState("Франция");

            var moscow = cityProp.GetState("Москва");
            var kazan = cityProp.GetState("Казань");
            var prague = cityProp.GetState("Прага");
            var paris = cityProp.GetState("Париж");
            var marsel = cityProp.GetState("Марсель");

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