using System.Collections.Generic;
using Ammeep.GiftRegister.Web.Domain.Model;

namespace Ammeep.GiftRegister.Web.Models
{
    public class ManageUsersPage
    {
        public IEnumerable<AdminAccount> AdminUsers { get; set; }

        public IEnumerable<GuestAccount> Guests { get; set; }
    }
}