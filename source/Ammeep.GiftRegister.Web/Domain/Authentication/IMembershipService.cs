using System.Collections.Generic;
using System.Web.Security;
using Ammeep.GiftRegister.Web.Domain.Model;

namespace Ammeep.GiftRegister.Web.Domain.Authentication
{
    public interface IMembershipService
    {
        bool ValidateUser(string userName, string password);
        bool ChangePassword(string userName, string oldPassword, string newPassword);
        MembershipCreateStatus CreateAdminUser(AdminAccount adminAccount);
        IEnumerable<AdminAccount> GetAllUsers();
    }
}