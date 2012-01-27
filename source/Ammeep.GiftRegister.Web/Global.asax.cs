using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Ammeep.GiftRegister.Web.Domain;
using Ammeep.GiftRegister.Web.Domain.Authentication;
using Ammeep.GiftRegister.Web.Domain.Logging;

namespace Ammeep.GiftRegister.Web
{
    public class GiftmeApplication : System.Web.HttpApplication
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthenticationService _authenticationService;
        private readonly ILoggingService _loggingService;

        public GiftmeApplication()
        {
            _configuration = DependencyResolver.Current.GetService<IConfiguration>();
            _authenticationService = DependencyResolver.Current.GetService<IAuthenticationService>();
            _loggingService = DependencyResolver.Current.GetService<ILoggingService>();
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("RegistryPage",
                            "Registry/RegistryPage/{page}/{categoryId}",
                            new
                                {
                                    controller = "Registry",
                                    action = "RegistryPage",
                                    page = 0,
                                    categoryId = 0
                                }
                );

            routes.MapRoute("ManageRegistryPage",
                           "Manage/ManageRegistryPage/{page}/{categoryId}",
                           new
                           {
                               controller = "Manage",
                               action = "ManageRegistryPage",
                               page = 0,
                               categoryId = 0
                           }
               );

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new {controller = "Registry", action = "Index", id = UrlParameter.Optional} // Parameter defaults
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

        protected void Application_Error()
        {
            Exception exception = Server.GetLastError();
            _loggingService.LogFatal("Epic Fail", exception);
        }
    }
}