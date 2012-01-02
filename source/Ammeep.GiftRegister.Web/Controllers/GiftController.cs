using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Ammeep.GiftRegister.Web.Domain;
using Ammeep.GiftRegister.Web.Domain.Model;
using Ammeep.GiftRegister.Web.Models;
using Newtonsoft.Json;

namespace Ammeep.GiftRegister.Web.Controllers
{
    public class GiftController : Controller
    {
        private readonly IRegistryManager _registryManager;
        private readonly IConfiguration _config;

        public GiftController(IRegistryManager registryManager, IConfiguration configuration)
        {
            _registryManager = registryManager;
            _config = configuration;
        }

        public ActionResult Registry()
        {
            var registryPageSize = _config.RegistryPageSize;
            var gifts = _registryManager.GetRegistry(registryPageSize, 0, 0);
            var categories = _registryManager.GetCategories();
            RegistryPage page = new RegistryPage(gifts,categories,registryPageSize);
            return View(page);
        }

        public PartialViewResult GetNextRegistryItems(int pageSize, int pageNumber, int categoryId)
        {
            var nextItems = _registryManager.GetRegistry(pageSize, pageNumber, categoryId);
            return PartialView("RegistryItems", nextItems.Select(gift=> new GiftRow{Item = gift}));
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
