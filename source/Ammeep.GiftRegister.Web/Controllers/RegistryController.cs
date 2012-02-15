using System;
using System.Web.Mvc;
using Ammeep.GiftRegister.Web.Domain;
using Ammeep.GiftRegister.Web.Domain.Logging;
using Ammeep.GiftRegister.Web.Models;

namespace Ammeep.GiftRegister.Web.Controllers
{
    public class RegistryController : Controller
    {
        private readonly IRegistryManager _registryManager;
        private readonly IConfiguration _config;
        private readonly ILoggingService _loggingService;

        public RegistryController(IRegistryManager registryManager, IConfiguration configuration,ILoggingService loggingService )
        {
            _registryManager = registryManager;
            _config = configuration;
            _loggingService = loggingService;
        }

        public ActionResult Index(int pageNumber,int pageSize, int categoryId)
        {
            var gifts = _registryManager.GetRegistry(pageSize, pageNumber, categoryId);
            var categories = _registryManager.GetCategories();
            RegistryItemsPage itemsPage = new RegistryItemsPage(gifts, categories, pageSize, categoryId);
            return View(itemsPage);
        }

        [HttpPost]
        public ActionResult GetThis(GetThisModel getThisModel)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    _registryManager.ReserveGift(getThisModel.Name, getThisModel.Email, getThisModel.GiftId, getThisModel.Quantity);
                    return PartialView("ThankYou");

                }catch(Exception exception)
                {
                    _loggingService.LogError(string.Format("The guest {0} could not reserve gift {1}. Email: {2}",getThisModel.Name,getThisModel.GiftId,getThisModel.Email),exception);
                    return PartialView("OppsError");
                }

            }
            return PartialView("OppsError");
        }

        public ActionResult ConfirmReservation(Guid confirmationId)
        {
            ReservationConfirmationPage model =_registryManager.ConfirmReservation(confirmationId);
            if(model.IsConfirmed)
            {
                return View(model);
            }
            return View("CouldNotConfirmReservation");

        }
    }
}
