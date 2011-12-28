using System.Collections.Generic;
using System.Web.Mvc;
using Ammeep.GiftRegister.Web.Domain;
using Ammeep.GiftRegister.Web.Models;

namespace Ammeep.GiftRegister.Web.Controllers
{
    public class GiftController : Controller
    {
        private readonly IGiftManager _giftManager;

        public GiftController(IGiftManager giftManager)
        {
            _giftManager = giftManager;
        }

        public ActionResult Registry()
        {
            IEnumerable<Gift> gifts = _giftManager.GetRegistry();
            WishlistPageModel pageModel = new WishlistPageModel();
            pageModel.WishlistTitle = "The mightly list";
            pageModel.GiftWishes = gifts;
            return View(pageModel);
        }

        public ActionResult Manage()
        {
            IEnumerable<Gift> gifts = _giftManager.GetRegistry();
            ManagePageModel pageModel = new ManagePageModel();
            pageModel.Gifts = gifts;
            return View(pageModel);
        }

        public ActionResult Edit(int giftId)
        {
            Gift giftToEdit = _giftManager.GetGift(giftId);
            EditGiftPage editGiftPage = new EditGiftPage();
            editGiftPage.Gift = giftToEdit;
            return View(editGiftPage);
        }

        [HttpPost]
        public ActionResult Edit(Gift gift)
        {
            if(ModelState.IsValid)
            {
                _giftManager.UpdateGift(gift);
                return RedirectToAction("Manage");
            }
            EditGiftPage editGiftPage = new EditGiftPage();
            editGiftPage.Gift = gift;
            return View(editGiftPage);
        }

        public ActionResult Delete(int giftId)
        {
            _giftManager.DeleteGift(giftId);
            return RedirectToAction("Manage");
        }

    }
}
