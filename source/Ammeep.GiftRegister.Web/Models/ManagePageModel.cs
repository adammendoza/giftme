using System.Collections.Generic;

namespace Ammeep.GiftRegister.Web.Models
{
    public class ManagePageModel
    {
        public IEnumerable<Gift> Gifts { get; set; }
    }
}