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

        public ActionResult Edit(int id)
        {
            Gift giftToEdit = _registryManager.GetGift(id);
            var categories = _registryManager.GetCategories();
            EditGiftPage editGiftPage = new EditGiftPage(giftToEdit,categories); 
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
            var categories = _registryManager.GetCategories();
            EditGiftPage editGiftPage = new EditGiftPage(gift,categories);
            editGiftPage.Gift = gift;
            return View(editGiftPage);
        }

        public PartialViewResult GetGiftPreview(Gift gift)
        {
            return PartialView("RegistryItemPreview", new GiftRow { IsFirst = true, Item = gift });
        }

        public ActionResult Delete(int id)
        {
            _registryManager.DeleteGift(id);
            return RedirectToAction("Index");
        }
      
    }
}