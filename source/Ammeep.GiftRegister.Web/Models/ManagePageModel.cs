using System.Collections.Generic;
using Ammeep.GiftRegister.Web.Domain.Model;

namespace Ammeep.GiftRegister.Web.Models
{
    public class ManagePageModel
    {
        public IEnumerable<Gift> Gifts { get; set; }
    }
}