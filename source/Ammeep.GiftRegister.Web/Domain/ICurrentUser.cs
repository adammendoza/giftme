using System.Security.Principal;
using System.Web;
using Ammeep.GiftRegister.Web.Domain.Model;

namespace Ammeep.GiftRegister.Web.Domain
{
    public interface ICurrentUser
    {
       
        AccountType AccountType { get;  }
        int AccountId { get; }
        string UserName { get; }
    }

    public class CurrentUser : ICurrentUser
    {

        public CurrentUser()
        {
            IIdentity identity = HttpContext.Current.User.Identity;
            if(identity is SignedInUser)
            {
                SignedInUser user = (SignedInUser)identity;
                AccountType = user.AccountType;
                AccountId = user.AccountId;
                UserName = user.UserName;
            }        
        }


        public AccountType AccountType { get; private set; }
        public int AccountId { get; private set; }
        public string UserName { get; private set; }
    }

    public class SignedInUser : IIdentity
    {

        public AccountType AccountType { get; private set; }
        public int AccountId { get; private set; }
        public string UserName { get; private set; }
        public bool IsAuthenticated { get; private set; }


        public SignedInUser(AccountType accountType, int userId, string userName, bool isAuthenticated)
        {
            AccountType = accountType;
            AccountId = userId;
            UserName = userName;
            IsAuthenticated = isAuthenticated;
        }

        public string Name
        {
            get { return UserName; }
        }

        public string AuthenticationType
        {
            get { return "Forms"; }
        }
    }

}