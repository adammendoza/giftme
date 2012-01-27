using System.Web.Mvc;
using Ammeep.GiftRegister.Web.Domain.Logging;

namespace Ammeep.GiftRegister.Web.Attributes
{
    public class HandleAllTheThingsAttribute :HandleErrorAttribute
    {
        private readonly ILoggingService _loggingService;

        public HandleAllTheThingsAttribute(ILoggingService loggingService)
        {
            _loggingService = loggingService;
        }

        public override void OnException(ExceptionContext filterContext)
        {
            _loggingService.LogError("Epic Fail",filterContext.Exception);
            base.OnException(filterContext);
        }
    }
}