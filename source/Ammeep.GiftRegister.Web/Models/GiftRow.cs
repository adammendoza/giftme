using Ammeep.GiftRegister.Web.Domain.Model;

namespace Ammeep.GiftRegister.Web.Models
{
    public class GiftRow
    {
        public bool IsFirst { get; set; }
        public Gift Item { get; set; }
    }
}