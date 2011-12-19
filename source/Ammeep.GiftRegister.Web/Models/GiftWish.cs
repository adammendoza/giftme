using System;
using System.Collections.Generic;

namespace Ammeep.GiftRegister.Web.Models
{
    public class GiftWish
    {
        public Uri ImageLocation { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public IEnumerable<Uri> SuggestedStoreLinks { get; set; }
        public bool IsSpecificItemRequired { get; set; }
        public int QuantityRequired { get; set; }
        public decimal RetailPrice { get; set; }
        public bool IsPurchased { get; set; }
    }
}