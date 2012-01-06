using Ammeep.GiftRegister.Web.Domain;
using Ammeep.GiftRegister.Web.Domain.Authentication;
using Ammeep.GiftRegister.Web.Domain.Logging;
using Ammeep.GiftRegister.Web.Domain.Model;

[assembly: WebActivator.PreApplicationStartMethod(typeof(Ammeep.GiftRegister.Web.App_Start.NinjectActivation), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(Ammeep.GiftRegister.Web.App_Start.NinjectActivation), "Stop")]

namespace Ammeep.GiftRegister.Web.App_Start
{
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Web.Mvc;

    public static class NinjectActivation 
    {
        private static readonly Bootstrapper Bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestModule));
            DynamicModuleUtility.RegisterModule(typeof(HttpApplicationInitializationModule));
            Bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            Bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            RegisterServices(kernel);
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IConfiguration>().To<Configuration>();
            kernel.Bind<ILoggingService>().To<LoggingService>();
            kernel.Bind<IAuthenticationService>().To<AuthenticationService>();
            kernel.Bind<ICurrentUser>().To<CurrentUser>();
            kernel.Bind<IGiftRepository>().To<GiftRepository>();
            kernel.Bind<IUserRepository>().To<UserRepository>();
            kernel.Bind<IUserManager>().To<UserManager>();
            kernel.Bind<IRegistryManager>().To<RegistryManager>();
        }        
    }
}
