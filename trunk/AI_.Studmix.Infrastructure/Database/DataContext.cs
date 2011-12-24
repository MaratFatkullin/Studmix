using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using AI_.Studmix.Domain.Entities;

namespace AI_.Studmix.Infrastructure.Database
{
    public class DataContext : DbContext
    {
        public DataContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }

        public DataContext()
            : base("DataContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<IncludeMetadataConvention>();

            modelBuilder.Entity<User>();
            modelBuilder.Entity<Role>();
            modelBuilder.Entity<ContentPackage>();
            modelBuilder.Entity<ContentFile>();
            modelBuilder.Entity<Order>();
            modelBuilder.Entity<Property>();
            modelBuilder.Entity<PropertyState>();

            //modelBuilder.Entity<ContentPackage>()
            //    .HasMany<PropertyState>(package => package.PropertyStates)
            //    .WithMany(state => state.ContentPackages);
            //.Map(mapping => mapping.ToTable("ContentPackagePropertyStates"));

            modelBuilder.Entity<PropertyState>()
                .HasRequired<Property>(state => state.Property)
                .WithMany(property => property.States);

            modelBuilder.Entity<PropertyState>()
                .HasMany<ContentPackage>(ps=>ps.Packages)
                .WithMany(p=>p.PropertyStates);
        }
    }
}