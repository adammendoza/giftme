using System;

namespace Ammeep.GiftRegister.Web.Domain.Model
{
    public class GiftPruchase
    {
        public GiftPruchase(){}

        public GiftPruchase(int giftId,int quantity)
        {
            GiftId = giftId;
            Quantity = quantity;
            Confirmed = false;
            CreatedOn = DateTime.Now;
            ConfirmationId = Guid.NewGuid();
            IsActive = true;
        }

        public Guid ConfirmationId { get; set; }
        public int GiftPurchaseId { get; set; }
        public int GiftId { get; set; }
        public int Quantity { get; set; }
        public bool Confirmed { get; set; }
        public DateTime ConfirmedOn { get; set; }
        public DateTime CreatedOn { get; set; }
        public int GuestId { get; set; }
        public bool IsActive { get; set; }
        
    }
}