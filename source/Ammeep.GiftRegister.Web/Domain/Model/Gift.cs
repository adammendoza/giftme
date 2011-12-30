using System.ComponentModel.DataAnnotations;

namespace Ammeep.GiftRegister.Web.Domain.Model
{
    public class Gift
    {
        public string ImageLocation { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public string Website { get; set; }
        public string SuggestedStores { get; set; }

        [UIHint("YesNo")]
        public bool IsSpecificItemRequired { get; set; }
        public int QuantityRequired { get; set; }

        [UIHint("Cost")]
        public decimal RetailPrice { get; set; }
        public bool IsPurchased { get; set; }
    }
}