using System.Data.Entity;
using AI_.Studmix.Domain.Entities;
using AI_.Studmix.Domain.Factories;

namespace AI_.Studmix.Infrastructure.Database
{
    public class ProductionDatabaseInitializer : CreateDatabaseIfNotExists<DataContext>
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
            var admin = userFactory.CreateUser("admin", "god_object", "", "", adminRole);

            admin.IncomeMoney(1000);

            context.Set<User>().Add(admin);

            #endregion

            #region Properties and states

            var propertyFactory = new PropertyFactory();

            var countryProp = propertyFactory.CreateProperty("Страна", 1, true);
            context.Set<Property>().Add(countryProp);
            var cityProp = propertyFactory.CreateProperty("Город", 2, true);
            context.Set<Property>().Add(cityProp);
            context.Set<Property>().Add(propertyFactory.CreateProperty("Вид", 3, true));
            context.Set<Property>().Add(propertyFactory.CreateProperty("Наименование учереждения", 4, true));
            context.Set<Property>().Add(propertyFactory.CreateProperty("Форма обучения", 5, true));
            context.Set<Property>().Add(propertyFactory.CreateProperty("Факультет", 6, true));
            context.Set<Property>().Add(propertyFactory.CreateProperty("Курс", 7, true));
            context.Set<Property>().Add(propertyFactory.CreateProperty("Группа", 8, true));
            context.Set<Property>().Add(propertyFactory.CreateProperty("Дисциплина", 9, false));
            context.Set<Property>().Add(propertyFactory.CreateProperty("Учебник\\год вып.", 10, false));
            context.Set<Property>().Add(propertyFactory.CreateProperty("Вариант", 11, false));
            context.Set<Property>().Add(propertyFactory.CreateProperty("Данные\\кр.", 12, false));

            #endregion

            context.SaveChanges();
        }
    }
}