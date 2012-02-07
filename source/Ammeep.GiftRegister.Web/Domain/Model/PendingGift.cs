namespace Ammeep.GiftRegister.Web.Domain.Model
{
    public class PendingGift
    {
        public Gift Gift{ get; set; }
        public Guest Guest{ get; set; }
        public GiftPruchase GiftPruchase { get; set; }
    }

    public class ReservedGift
    {
        public Gift Gift { get; set; }
        public Guest Guest { get; set; }
        public GiftPruchase GiftPruchase { get; set; }
    }
}