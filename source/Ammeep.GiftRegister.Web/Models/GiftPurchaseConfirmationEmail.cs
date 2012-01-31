using System.Globalization;
using System.IO;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using Ammeep.GiftRegister.Web.Domain.Model;
using RazorEngine;

namespace Ammeep.GiftRegister.Web.Models
{
    public class GiftPurchaseConfirmationEmail
    {
        private readonly Guest _guest;
        private readonly GiftPruchase _guestPurchase;
        private readonly Gift _gift;

        public GiftPurchaseConfirmationEmail(Guest guest, GiftPruchase guestPurchase, Gift gift)
        {
            _guest = guest;
            _guestPurchase = guestPurchase;
            _gift = gift;
        }

        public string FromAddress
        {
            get { return "Amy.Palamountain@gmail.com"; }
        }

        public string ToAddress
        {
            get { return _guest.Email; }
        }

        public string Subject
        {
            get { return "Please confirm your intent to purchase."; }
        }

        public string RecipientsName
        {
            get { return _guest.Name; }
        }

        public string GiftLocation
        {
            get { return _gift.ImageLocation; }
        }

        public string GiftName
        {
            get { return _gift.Name; }
        }

        public string GiftDescription
        {
            get { return _gift.Description; }
        }

        public string GiftSuggestedStore
        {
            get { return _gift.SuggestedStores; }
        }

        public string SpecificGiftRequried
        {
            get { return _gift.SpecificItemRequried ? "Yes" : "No"; }
        }

        public int NominatedQuntity
        {
            get { return _guestPurchase.Quantity; }
        }

        public string GiftPrice
        {
            get { return string.Format("{0:C}", _gift.RetailPrice.ToString(CultureInfo.InvariantCulture)); }
        }

        public string Website
        {
            get { return _gift.Website; }
        }

        public string GenerateEmailBody()
        {
            string pathToTemplate = HttpContext.Current.Server.MapPath(@"\Views\Email\ConfirmationEmail.cshtml");
            string template = File.ReadAllText(pathToTemplate);
            string emailBody = Razor.Parse(template, this);
            return emailBody;
        }

      public string GenerateConfirmUrl()
      {
          HttpContext httpContext = HttpContext.Current;         
          UrlHelper url = new UrlHelper(httpContext.Request.RequestContext);
          return string.Format("{0}://{1}{2}",
                                                httpContext.Request.Url.Scheme,
                                                httpContext.Request.Url.Authority,
                                                url.Action("ConfirmReservation", "Registry",new {confirmationId = _guestPurchase.ConfirmationId}));
        
      }
       
    }
}