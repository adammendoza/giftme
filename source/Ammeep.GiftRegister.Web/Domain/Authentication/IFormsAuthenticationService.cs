namespace Ammeep.GiftRegister.Web.Domain.Authentication
{
    public interface IFormsAuthenticationService
    {
        void SignIn(string userName, bool createPersistentCookie);
        void SignOut();
    }
}