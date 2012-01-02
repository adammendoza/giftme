using System.Web.Mvc;
using Ammeep.GiftRegister.Web.Domain;
using Ammeep.GiftRegister.Web.Domain.Model;
using Ammeep.GiftRegister.Web.Models;

namespace Ammeep.GiftRegister.Web.Controllers
{
    [Authorize]
    public class ManageController :Controller
    {
        private readonly IRegistryManager _registryManager;
        private readonly IConfiguration _configuration;

        public ManageController(IRegistryManager registryManager, IConfiguration configuration)
        {
            _registryManager = registryManager;
            _configuration = configuration;
        }

        public ActionResult Index()
        {
            var registryPageSize = _configuration.RegistryPageSize;
            var gifts = _registryManager.GetRegistry(registryPageSize, 0, 0);
            var categories = _registryManager.GetCategories();
            RegistryItemsPage itemsPage = new RegistryItemsPage(gifts, categories, registryPageSize);
            return View(itemsPage);
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
            if (ModelState.IsValid)
            {
                _registryManager.UpdateGift(gift);
                return RedirectToAction("Index");
            }
            EditGiftPage editGiftPage = new EditGiftPage();
            editGiftPage.Gift = gift;
            return View(editGiftPage);
        }

        public ActionResult Delete(int giftId)
        {
            _registryManager.DeleteGift(giftId);
            return RedirectToAction("Index");
        }
      
    }
}