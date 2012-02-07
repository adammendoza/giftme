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
            RegistryItemsPage itemsPage = new RegistryItemsPage(gifts, categories, registryPageSize,0);
            return View(itemsPage);
        }

        public ActionResult ManageRegistryPage(int page, int categoryId)
        {
            var registryPageSize = _configuration.RegistryPageSize;
            var gifts = _registryManager.GetRegistry(registryPageSize, page, categoryId);
            var categories = _registryManager.GetCategories();
            RegistryItemsPage itemsPage = new RegistryItemsPage(gifts, categories, registryPageSize, categoryId);
            return View("Index", itemsPage);
        }



        public ActionResult Edit(int id)
        {
            Gift giftToEdit = _registryManager.GetGift(id);
            var categories = _registryManager.GetCategories();
            EditableGiftPage editableGiftPage = new EditableGiftPage(giftToEdit,categories); 
            return View(editableGiftPage);
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
            EditableGiftPage editableGiftPage = new EditableGiftPage(gift,categories);
            editableGiftPage.Gift = gift;
            return View(editableGiftPage);
        }

        public PartialViewResult GetGiftPreview(Gift gift)
        {
            return PartialView("RegistryItemPreview", new GiftRow { IsFirst = true, Item = gift });
        }

        public ActionResult Delete(int id)
        {
            _registryManager.DeactivateGift(id);
            return RedirectToAction("Index");
        }

        public ActionResult Add()
        {
            var categories = _registryManager.GetCategories();
            EditableGiftPage addGiftPage = new EditableGiftPage(new Gift(), categories);
            return View(addGiftPage);
        }

        [HttpPost]
        public ActionResult Add(Gift gift)
        {
            if (ModelState.IsValid)
            {
                _registryManager.AddNewGift(gift);
                return RedirectToAction("Index");
            }
            var categories = _registryManager.GetCategories();
            EditableGiftPage addGiftPage = new EditableGiftPage(gift, categories);
            addGiftPage.Gift = gift;
            return View(addGiftPage);
        }

        public ActionResult GiftStatues()
        {
            GiftStatusesPage model = _registryManager.GetAllGifts();
            return View(model);
        }

        public ActionResult Reactivate(int giftId)
        {
            _registryManager.ReactivateGift(giftId);
            return RedirectToAction("GiftStatues");
        }

        public ActionResult ResendConfirmationEmail()
        {
            return RedirectToAction("GiftStatues");
        }

        public ActionResult RemovePendingStatus(int giftId,int giftPurchaseId)
        {
            _registryManager.RemovePendingStatus(giftId);
            _registryManager.RemoveGiftPurhcase(giftPurchaseId);
            return RedirectToAction("GiftStatues");
        }
    }
}