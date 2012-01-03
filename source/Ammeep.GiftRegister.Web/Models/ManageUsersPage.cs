using System.Collections.Generic;
using Ammeep.GiftRegister.Web.Domain.Model;

namespace Ammeep.GiftRegister.Web.Models
{
    public class ManageUsersPage
    {
        public IEnumerable<AdminAccount> Users { get; set; }
    }
}