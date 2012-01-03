namespace Ammeep.GiftRegister.Web.Domain.Model
{
    public class User
    {
        public int UserId { get; set; }
       
     
     
    }

    public class Account
    {
        public AccountType Type { get; set; }
        public int AccountId { get; set; }
        public bool RequiresPassword { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
    }

    public enum AccountType
    {
        Guest =0,
        Host =1,
        Admin =2
    }

    public class AdminUser
    {
        public Account AcountDetails { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
    }

    public class GuestUser
    {
        public Account AcountDetails { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}