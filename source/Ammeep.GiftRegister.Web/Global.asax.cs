using System.Web.Mvc;
using System.Web.Routing;

namespace Ammeep.GiftRegister.Web
{
    public class GiftmeApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
           // filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Management", // Route name
                "Manage/{action}/{id}", // URL with parameters
                new {controller = "Manage", action = "Index", id = UrlParameter.Optional} // Parameter defaults
                );

            routes.MapRoute("RegistryNextItems",
                            "{controller}/NextItems/{pageSize}/{pageNumber}/{categoryId}",
                            new
                                {
                                    controller = "Gift",
                                    action = "NextItems",
                                    pageSize = 5,
                                    pageNumber = 0,
                                    categoryId = 0
                                }
                );
            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new {controller = "Gift", action = "Registry", id = UrlParameter.Optional} // Parameter defaults
                );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}