using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Ammeep.GiftRegister.Web.Models;

namespace Ammeep.GiftRegister.Web.Controllers
{
    public class WishlistController : Controller
    {

        public ActionResult Index()
        {
            WishlistPageModel pageModel = new WishlistPageModel();
            pageModel.WishlistTitle = "The mightly list";
            GiftWish wish1 = new GiftWish();
            wish1.ImageLocation = new Uri("http://baconmockup.com/170/165");
            wish1.ItemName = "Item Name";
            wish1.Description = "Shoulder hamburger frankfurter, biltong tail shankle drumstick prosciutto short ribs pastrami. Boudin kielbasa shank cow. Andouille turducken filet mignon, pancetta capicola beef ribs pork meatloaf. Shoulder corned beef ball tip jerky pig. Short ribs pork loin sirloin pig, tail meatloaf turducken swine. Flank tail cow chicken filet mignon, capicola andouille biltong pastrami frankfurter. Meatball jerky shankle, jowl pork chop prosciutto tongue andouille turducken tail rump.";
            wish1.QuantityRequired = 2;
            wish1.RetailPrice = 44.87m;
            wish1.Website = new Uri("http://www.google.com");
            wish1.SuggestedStores = "Bed, Bath n' Table, Briscoes";
            wish1.IsSpecificItemRequired = true;
            pageModel.GiftWishes = new List<GiftWish> {wish1, wish1, wish1};
            return View(pageModel);
        }

    }
}
