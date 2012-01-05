using System.ComponentModel.DataAnnotations;

namespace Ammeep.GiftRegister.Web.Domain.Model
{
    public class Gift
    {
        [Display(Name = "Image Url")]
        [UIHint("Image")]
        [Required]
        public string ImageLocation { get; set; }

        [Display(Name = "Item Name")]
        [UIHint("ItemName")]
        [Required]
        public string Name { get; set; }

        [UIHint("Description")]
        [Required]
        public string Description { get; set; }

        [UIHint("Website")]
        [Required]
        public string Website { get; set; }

        [Display(Name = "Suggested Store(s)")]
        [UIHint("Stores")]
        [Required]
        public string SuggestedStores { get; set; }

        [UIHint("YesNo")]
        [Display(Name = "Flexible")]
        public bool IsSpecificItemRequired { get; set; }

        [Display(Name = "Quantity")]
        [UIHint("Quantity")]
        [Required]
        public int QuantityRequired { get; set; }

        [UIHint("Cost")]
        [Display(Name = "RRP")]
        [Required]
        public decimal RetailPrice { get; set; }
        public bool IsPurchased { get; set; }

        [UIHint("Category")]
        [Required]
        public string Category { get; set; }
        public int GiftId { get; set; }
    }
}