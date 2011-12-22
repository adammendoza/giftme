using System;
using System.ComponentModel.DataAnnotations;

namespace Ammeep.GiftRegister.Web.Models
{
    public class GiftWish
    {
        public Uri ImageLocation { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public Uri Website { get; set; }
        public string SuggestedStores { get; set; }

        [UIHint("YesNo")]
        public bool IsSpecificItemRequired { get; set; }
        public int QuantityRequired { get; set; }

        [UIHint("Cost")]
        public decimal RetailPrice { get; set; }
        public bool IsPurchased { get; set; }


    }
}