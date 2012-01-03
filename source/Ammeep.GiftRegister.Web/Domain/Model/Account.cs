using System;
using System.Security.Cryptography;
using System.Text;

namespace Ammeep.GiftRegister.Web.Domain.Model
{
  
    public abstract class Account
    {
        protected Account(AccountType type,string username):this(type,username,null){}

        protected Account(AccountType type, string username, string password)
        {
            Type = type;
            Username = username;
            RequiresPassword = type != AccountType.Guest;
            PasswordSalt = CreateNewPasswordSalt();
            PasswordHash = HashPassword(password);
        }

       
        public AccountType Type { get; set; }
        public string Username { get; set; }
        public int AccountId { get; set; }
        public bool RequiresPassword { get; set; }
        public string PasswordSalt { get; set; }
        public string PasswordHash { get; set; }

        public bool ValidatePassword(string password)
        {
            if (string.IsNullOrEmpty(PasswordHash) || PasswordHash.Length < 128)
                throw new ArgumentException("There is an invalid password hash to validate against.");
            string passHash = HashPassword(password);
            return string.Compare(PasswordHash, passHash) == 0;
        }

        private string CreateNewPasswordSalt()
        {
            using (RNGCryptoServiceProvider random = new RNGCryptoServiceProvider())
            {
                byte[] salt = new byte[32];
                random.GetBytes(salt);
                return BytesToHex(salt);
            }
        }

        private string HashPassword(string password)
        {
            using (SHA256Managed hash = new SHA256Managed())
            {
                byte[] utf8 = UTF8Encoding.UTF8.GetBytes(PasswordSalt + password);
                return BytesToHex(hash.ComputeHash(utf8));
            }
        }

        private static string BytesToHex(byte[] toConvert)
        {
            StringBuilder s = new StringBuilder(toConvert.Length * 2);
            foreach (byte b in toConvert)
            {
                s.Append(b.ToString("x2"));
            }
            return s.ToString();
        }
    }

  
    public enum AccountType
    {
        Guest =0,
        Host =1,
        Admin =2
    }

    public class AdminAccount :Account
    {
        public AdminAccount() : base(AccountType.Host, null){}

        public AdminAccount(string name, string email, string username, string password) : base(AccountType.Host,username,password)
        {
            Name = name;
            Email = email;
        }

        public string Name { get; set; }
        public string Email { get; set; }       
    }

    public class GuestUser 
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}