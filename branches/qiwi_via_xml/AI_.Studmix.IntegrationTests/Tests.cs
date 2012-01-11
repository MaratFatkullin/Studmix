using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using AI_.Studmix.Domain.Entities;
using AI_.Studmix.Infrastructure.Database;
using AI_.Studmix.Infrastructure.Repository;
using FluentAssertions;
using Xunit;

namespace AI_.Studmix.IntegrationTests
{
    public class Fixture
    {
        public void InitDatabase()
        {
            Database.SetInitializer(new TemporaryDatabaseInitializer());
        }
    }


    public class Tests : IUseFixture<Fixture>
    {
        #region IUseFixture<Fixture> Members

        public void SetFixture(Fixture fixture)
        {
            fixture.InitDatabase();
        }

        #endregion

        [Fact]
        public void MethodName_StateUnderTest_ExpectedBehavior()
        {
            var unitOfWork = new EntityFrameworkUnitOfWork<DataContext>();

            var package =
                unitOfWork.GetRepository<ContentPackage>().Get(p => p.Owner.UserName == "marat").First();

            var user = unitOfWork.GetRepository<User>().Get(u => u.UserName == "marat").Single();

            user.Should().Be(package.Owner);
        }

        [Fact]
        public void MethodName_StateUnderTest_ExpectedBehavior1()
        {
            var unitOfWork = new EntityFrameworkUnitOfWork<DataContext>();
            var contentPackages = unitOfWork.GetRepository<ContentPackage>().Get();
            foreach (var package in contentPackages)
            {
                Trace.WriteLine(package.CreateDate);
            }

            contentPackages.Should().Match(r => r.Any(s => true), "");
        }
    }
}