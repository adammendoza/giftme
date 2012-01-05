using Ammeep.GiftRegister.Web.Domain.Model;

namespace Ammeep.GiftRegister.Web.Domain
{
    public interface ICurrentUser
    {
        AccountType AccountType { get;  }
        int AccountId { get; }
        string UserName { get; }
    }

    public class SignedInUser : ICurrentUser
    {
        public AccountType AccountType { get; set; }
        public int AccountId { get; set; }
        public string UserName { get; set; }

        public SignedInUser(AccountType accountType, int userId,string userName)
        {
            AccountType = accountType;
            AccountId = userId;
            UserName = userName;
        }
    }

    public class AnonymousUser:ICurrentUser
    {
        public AccountType AccountType
        {
            get { return AccountType.Guest;}
        }

        public int AccountId
        {
            get { return 0; }
        }

        public string UserName
        {
            get { return string.Empty; }
        }
    }
}