using System.Collections.Generic;
using Ammeep.GiftRegister.Web.Domain.Model;

namespace Ammeep.GiftRegister.Web.Models
{
    public class AllAccountsPage
    {
        public IEnumerable<Account> AdminUsers { get; set; }

        public IEnumerable<Account> Guests { get; set; }
    }

    public class ManageUserPage
    {
        public Account Account { get; set; }
        public bool CanEdit { get; set; }
        public bool CanView { get; set; }
    }
}