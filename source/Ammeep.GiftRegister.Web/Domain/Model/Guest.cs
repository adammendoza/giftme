namespace Ammeep.GiftRegister.Web.Domain.Model
{
    public class Guest
    {
        public int GuestId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int GiftPurchaseId { get; set; }
    }
}