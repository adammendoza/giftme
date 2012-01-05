using System.ComponentModel.DataAnnotations;

namespace Ammeep.GiftRegister.Web.Domain.Model
{
    public class Gift
    {
        [Display(Name = "Image Url")]
        [UIHint("Image")]
        public string ImageLocation { get; set; }

        [Display(Name = "Item Name")]
        [UIHint("ItemName")]
        public string Name { get; set; }

        [UIHint("Description")]
        public string Description { get; set; }

        [UIHint("Website")]
        public string Website { get; set; }

        [Display(Name = "Suggested Store(s)")]
        [UIHint("Stores")]
        public string SuggestedStores { get; set; }

        [UIHint("YesNo")]
        [Display(Name = "Flexible")]
        public bool IsSpecificItemRequired { get; set; }

        [Display(Name = "Quantity")]
        [UIHint("Quantity")]
        public int QuantityRequired { get; set; }

        [UIHint("Cost")]
        [Display(Name = "RRP")]
        public decimal RetailPrice { get; set; }
        public bool IsPurchased { get; set; }

        [UIHint("Category")]
        public string Category { get; set; }
        public int GiftId { get; set; }
    }
}