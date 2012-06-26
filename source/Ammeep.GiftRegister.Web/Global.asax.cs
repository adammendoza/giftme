using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Ammeep.GiftRegister.Web.Domain;
using Ammeep.GiftRegister.Web.Domain.Authentication;

namespace Ammeep.GiftRegister.Web
{
    public class GiftmeApplication : HttpApplication
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthenticationService _authenticationService;

        public GiftmeApplication()
        {
            _configuration = DependencyResolver.Current.GetService<IConfiguration>();
            _authenticationService = DependencyResolver.Current.GetService<IAuthenticationService>();
     
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("ConfirmReservation",
                         "Registry/ConfirmReservation/{confirmationId}",
                         new
                         {
                             controller = "Registry",
                             action = "ConfirmReservation",
                             confirmationId = 0
                         }
             );

            routes.MapRoute("Manage",
                          "Manage/{action}/{pageNumber}/{pageSize}/{categoryId}", // URL with parameters
                           new
                           {
                               controller = "Manage",
                               action = "Index",
                               pageNumber = 0,
                               pageSize = 10,
                               categoryId = 0
                           }
               );

         
            routes.MapRoute(
                "Feedback", // Route name
                "Feedback/{action}", // URL with parameters
                new {controller = "Feedback", action = "Index"}
                // Parameter defaults
                );

            routes.MapRoute(
             "Registery", // Route name
             "{controller}/{action}/{pageNumber}/{pageSize}/{categoryId}", // URL with parameters
             new { controller = "Registry", action = "Index", pageNumber = 0, pageSize = 10, categoryId = 0 } // Parameter defaults
             );


        }

        protected void Application_Start()
        {        
            AreaRegistration.RegisterAllAreas();
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

        /// <summary>
        /// Fires when the application is processing an authenticated request.
        /// </summary>
        protected void Application_AuthenticateRequest()
        {
            HttpCookie authenticationCookie = HttpContext.Current.Request.Cookies[_configuration.AuthenticatedUserCookieName];

           _authenticationService.AuthenticateRequest(authenticationCookie);
        }


    }
}