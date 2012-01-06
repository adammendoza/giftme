using System.Web;
using Ammeep.GiftRegister.Web.Domain.Model;

namespace Ammeep.GiftRegister.Web.Domain.Authentication
{
    public interface IAuthenticationService
    {
        void SignIn(Account account);
        void SignOut();
        void AuthenticateRequest(HttpCookie authenticationCookie);
    }
}