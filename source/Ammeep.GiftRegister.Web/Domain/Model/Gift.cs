using System;
using System.ComponentModel.DataAnnotations;

namespace Ammeep.GiftRegister.Web.Domain.Model
{
    public class Gift
    {
        [Display(Name = "Image Url:")]
        [UIHint("Image")]
        [Required]
        public string ImageLocation { get; set; }

        [Display(Name = "Item Name:")]
        [UIHint("ItemName")]
        [Required]
        public string Name { get; set; }

        [UIHint("Description")]
        [Required]
        [Display(Name = "Description:")]
        public string Description { get; set; }

        [UIHint("Website")]
        [Required]
        [Display(Name = "Product website:")]
        public string Website { get; set; }

        [Display(Name = "Suggested store(s):")]
        [UIHint("Stores")]
        [Required]
        public string SuggestedStores { get; set; }

        [UIHint("YesNo")]
        [Display(Name = "Specific item desired:")]
        public bool SpecificItemRequried { get; set; }

        [Display(Name = "Quantity we'd like:")]
        [UIHint("Quantity")]
        [Required]
        public int QuantityRequired { get; set; }

        [UIHint("Cost")]
        [Display(Name = "RRP:")]
        [Required]
        public decimal RetailPrice { get; set; }

        [UIHint("Category")]
        [Required]
        public string Category { get; set; }
        public int GiftId { get; set; }

        public DateTime LastUpdatedDate { get; set; }

        public int LastUpdatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public int CreatedBy { get; set; }

        [Display(Name = "Quantity we'd like:")]
        [UIHint("Quantity")]
        public int QuantityRemaining { get; set; }

        public bool IsActive { get; set; }
        public bool Reserved { get; set; }
        public bool PendingReservation { get; set; }

        public void ConfirmReservation(int quantityReserved)
        {
            QuantityRemaining -= quantityReserved;
            Reserved = QuantityRemaining <= 0;
            PendingReservation = false;
        }

        public void UpdateQuantityRequired(int newQuantityRequired)
        {
            if(QuantityRequired > newQuantityRequired)
            {
                QuantityRemaining -= newQuantityRequired;
            }
            else
            {
                QuantityRemaining += newQuantityRequired;
            }
            Reserved = QuantityRemaining <= 0;
        }
    }
}