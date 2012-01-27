using System;

namespace Ammeep.GiftRegister.Web.Domain.Model
{
    public class GiftPruchase
    {
        public int GiftPurchaseId { get; set; }
        public int GiftId { get; set; }
        public int Quantity { get; set; }
        public bool Confirmed { get; set; }
        public DateTime ConfirmedOn { get; set; }
        public DateTime CreatedOn { get; set; }
        public int GuestId { get; set; }
    }
}