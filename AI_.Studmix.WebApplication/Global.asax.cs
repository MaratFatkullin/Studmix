using System.Collections.Generic;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AI_.Studmix.ApplicationServices.FileRepository;
using AI_.Studmix.ApplicationServices.Services.ContentService;
using AI_.Studmix.ApplicationServices.Services.MembershipService;
using AI_.Studmix.ApplicationServices.Services.OrderService;
using AI_.Studmix.ApplicationServices.Services.SearchService;
using AI_.Studmix.Domain.Repository;
using AI_.Studmix.Domain.Services;
using AI_.Studmix.Domain.Services.Abstractions;
using AI_.Studmix.Infrastructure.Database;
using AI_.Studmix.Infrastructure.FileSystem;
using AI_.Studmix.Infrastructure.PaymentSystem;
using AI_.Studmix.Infrastructure.Repository;
using AI_.Studmix.WebApplication.Infrastructure;
using AI_.Studmix.WebApplication.Infrastructure.Authentication;
using AI_.Studmix.WebApplication.Infrastructure.Filters;
using AI_.Studmix.WebApplication.Infrastructure.ModelBinders;
using Microsoft.Practices.Unity;

namespace AI_.Studmix.WebApplication
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new LogErrorAttribute("ErrorFilterPolicy"));
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default",
                // Route name
                "{controller}/{action}/{id}",
                // URL with parameters
                new {controller = "Home", action = "Index", id = UrlParameter.Optional} // Parameter defaults
                );
        }

        private static void InitializeDatabase()
        {
            Database.SetInitializer(new TemporaryDatabaseInitializer());
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            RegisterDependencyResolver();
            RegisterModelBinders();
            InitializeDatabase();
        }

        private void RegisterModelBinders()
        {
            ModelBinders.Binders.Add(typeof (Dictionary<int, string>), new DefaultDictionaryBinder());
        }

        private void RegisterDependencyResolver()
        {
            var container = new UnityContainer();
            ConfigureContainer(container);
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        private void ConfigureContainer(IUnityContainer container)
        {
            container.RegisterType<IControllerFactory, DefaultControllerFactory>();
            container.RegisterType<IControllerActivator, ControllerActivator>();
            container.RegisterType<IViewPageActivator, ViewPageActivator>();
            container.RegisterType<ModelMetadataProvider, DataAnnotationsModelMetadataProvider>();

            container.RegisterType<IUnitOfWork, EntityFrameworkUnitOfWork<DataContext>>(
                new PerResolveLifetimeManager());
            container.RegisterType<IAuthenticationProvider, AuthenticationProvider>();
            container.RegisterType<IPaymentSystemInvoiceRepository, QiwiInvoiceRepository>();
            container.RegisterType<IPaymentSystmeProviderConfiguration, PaymentSystmeProviderConfiguration>();

            container.RegisterType<IFinanceService, FinanceService>();
            container.RegisterType<IPermissionService, PermissionService>();
            container.RegisterType<IContentService, ContentService>();
            container.RegisterType<ISearchService, SearchService>();

            container.RegisterType<IMembershipService, MembershipService>();
            container.RegisterType<IMembershipConfiguration, MembershipConfiguration>();
            container.RegisterType<IFileRepository, FileRepository>();
            container.RegisterType<IFileSystemLocator, FileSystemLocator>();
            container.RegisterType<IFileSystemProvider, FileSystemProvider>();
            container.RegisterType<IOrderService, OrderService>();

        }
    }
}