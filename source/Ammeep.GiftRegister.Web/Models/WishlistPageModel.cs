using System.Collections.Generic;

namespace Ammeep.GiftRegister.Web.Models
{
    public class WishlistPageModel
    {
        public string WishlistTitle { get; set; }
        public IEnumerable<GiftWish> GiftWishes { get; set; }
    }

    public class GiftRow
    {
        public bool IsFirst { get; set; }
        public GiftWish Item { get; set; }
    }
}