using System.Web.Mvc;
using System.Web.Routing;
using log4net.Config;

namespace Ammeep.GiftRegister.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
           // filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("RegistryItemScroll",
                "{controller}/NRegistry/{pageSize}/{pageNumber}/{categoryId}",
                new { controller = "Gift", action = "NRegistry", pageSize = 5, pageNumber = 0, categoryId = 0 });
            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Gift", action = "Registry", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            XmlConfigurator.Configure();
            AreaRegistration.RegisterAllAreas();
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}