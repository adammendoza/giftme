namespace Ammeep.GiftRegister.Web.Domain
{
    public interface IUserManager
    {
        LoginResult LoginUser(string userName, string password);
    }

    public class UserManager : IUserManager
    {
        public LoginResult LoginUser(string userName, string password)
        {
            return new LoginResult(true);
        }
    }
}