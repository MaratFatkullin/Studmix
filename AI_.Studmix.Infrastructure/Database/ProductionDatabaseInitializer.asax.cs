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

            var countryProp = propertyFactory.CreateProperty("������", 1, true);
            context.Set<Property>().Add(countryProp);
            var cityProp = propertyFactory.CreateProperty("�����", 2, true);
            context.Set<Property>().Add(cityProp);
            context.Set<Property>().Add(propertyFactory.CreateProperty("���", 3, true));
            context.Set<Property>().Add(propertyFactory.CreateProperty("������������ �����������", 4, true));
            context.Set<Property>().Add(propertyFactory.CreateProperty("����� ��������", 5, true));
            context.Set<Property>().Add(propertyFactory.CreateProperty("���������", 6, true));
            context.Set<Property>().Add(propertyFactory.CreateProperty("����", 7, true));
            context.Set<Property>().Add(propertyFactory.CreateProperty("������", 8, true));
            context.Set<Property>().Add(propertyFactory.CreateProperty("����������", 9, false));
            context.Set<Property>().Add(propertyFactory.CreateProperty("�������\\��� ���.", 10, false));
            context.Set<Property>().Add(propertyFactory.CreateProperty("�������", 11, false));
            context.Set<Property>().Add(propertyFactory.CreateProperty("������\\��.", 12, false));

            #endregion

            context.SaveChanges();
        }
    }
}