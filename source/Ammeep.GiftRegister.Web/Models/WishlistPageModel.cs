using System.Collections.Generic;

namespace Ammeep.GiftRegister.Web.Models
{
    public class WishlistPageModel
    {
        public string WishlistTitle { get; set; }
        public IEnumerable<GiftWish> GiftWishes { get; set; }
    }
}