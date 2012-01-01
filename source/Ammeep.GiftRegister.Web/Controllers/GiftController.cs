using System.Collections.Generic;
using System.Web.Mvc;
using Ammeep.GiftRegister.Web.Domain;
using Ammeep.GiftRegister.Web.Domain.Logging;
using Ammeep.GiftRegister.Web.Domain.Model;
using Ammeep.GiftRegister.Web.Models;

namespace Ammeep.GiftRegister.Web.Controllers
{
    public class GiftController : Controller
    {
        private readonly IRegistryManager _registryManager;
        private readonly ILoggingService _loggingService;

        public GiftController(IRegistryManager registryManager, ILoggingService loggingService)
        {
            _registryManager = registryManager;
            _loggingService = loggingService;
        }

        public ActionResult Registry()
        {          
            _loggingService.LogDebug("Getting all registry items");
            RegistryPage page = new RegistryPage();
            page.Gifts = _registryManager.GetRegistry();
            page.Categories = _registryManager.GetCategories();
            return View(page);
        }

        public ActionResult Manage()
        {
            IEnumerable<Gift> gifts = _registryManager.GetRegistry();
            ManagePageModel pageModel = new ManagePageModel();
            pageModel.Gifts = gifts;
            return View(pageModel);
        }

        public ActionResult Edit(int giftId)
        {
            Gift giftToEdit = _registryManager.GetGift(giftId);
            EditGiftPage editGiftPage = new EditGiftPage();
            editGiftPage.Gift = giftToEdit;
            return View(editGiftPage);
        }

        [HttpPost]
        public ActionResult Edit(Gift gift)
        {
            if(ModelState.IsValid)
            {
                _registryManager.UpdateGift(gift);
                return RedirectToAction("Manage");
            }
            EditGiftPage editGiftPage = new EditGiftPage();
            editGiftPage.Gift = gift;
            return View(editGiftPage);
        }

        public ActionResult Delete(int giftId)
        {
            _registryManager.DeleteGift(giftId);
            return RedirectToAction("Manage");
        }

    }
}
