using System;
using System.Collections.Generic;
using Ammeep.GiftRegister.Web.Models;

namespace Ammeep.GiftRegister.Web.Domain
{
    public interface IGiftManager
    {
        IEnumerable<Gift> GetRegistry();
        Gift GetGift(int giftId);
        void UpdateGift(Gift gift);
        void DeleteGift(int giftId);
    }

    public class GiftManager : IGiftManager
    {
        public IEnumerable<Gift> GetRegistry()
        {
            Gift wish1 = new Gift();
            wish1.ImageLocation = new Uri("http://baconmockup.com/170/165");
            wish1.ItemName = "Item Name";
            wish1.Description = "Shoulder hamburger frankfurter, biltong tail shankle drumstick prosciutto short ribs pastrami. Boudin kielbasa shank cow. Andouille turducken filet mignon, pancetta capicola beef ribs pork meatloaf. Shoulder corned beef ball tip jerky pig. Short ribs pork loin sirloin pig, tail meatloaf turducken swine. Flank tail cow chicken filet mignon, capicola andouille biltong pastrami frankfurter. Meatball jerky shankle, jowl pork chop prosciutto tongue andouille turducken tail rump.";
            wish1.QuantityRequired = 2;
            wish1.RetailPrice = 44.87m;
            wish1.Website = new Uri("http://www.google.com");
            wish1.SuggestedStores = "Bed, Bath n' Table, Briscoes";
            wish1.IsSpecificItemRequired = true;
            return new List<Gift> { wish1, wish1, wish1 };
        }

        public Gift GetGift(int giftId)
        {
            Gift wish1 = new Gift();
            wish1.ImageLocation = new Uri("http://baconmockup.com/170/165");
            wish1.ItemName = "Item Name";
            wish1.Description = "Shoulder hamburger frankfurter, biltong tail shankle drumstick prosciutto short ribs pastrami. Boudin kielbasa shank cow. Andouille turducken filet mignon, pancetta capicola beef ribs pork meatloaf. Shoulder corned beef ball tip jerky pig. Short ribs pork loin sirloin pig, tail meatloaf turducken swine. Flank tail cow chicken filet mignon, capicola andouille biltong pastrami frankfurter. Meatball jerky shankle, jowl pork chop prosciutto tongue andouille turducken tail rump.";
            wish1.QuantityRequired = 2;
            wish1.RetailPrice = 44.87m;
            wish1.Website = new Uri("http://www.google.com");
            wish1.SuggestedStores = "Bed, Bath n' Table, Briscoes";
            wish1.IsSpecificItemRequired = true;
            return wish1;
        }

        public void UpdateGift(Gift gift)
        {
            return;
        }

        public void DeleteGift(int giftId)
        {
            return;
        }
    }
}