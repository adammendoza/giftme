using System;
using System.Security.Cryptography;
using System.Text;

namespace Ammeep.GiftRegister.Web.Domain.Model
{
    public class Account
    {
        public Account(){}
        public Account(AccountType type, string username, string password, string name, string email)
        {
            AccountType = type;
            Username = username;
            Name = name;
            Email = email;
            RequiresPassword = type != AccountType.Guest;
            PasswordSalt = CreateNewPasswordSalt();
            PasswordHash = HashPassword(password);
        }
    
        public AccountType AccountType { get; set; }
        public string Username { get; set; }
        public int AccountId { get; set; }
        public bool RequiresPassword { get; set; }
        public string PasswordSalt { get; set; }
        public string PasswordHash { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }     

        public bool ValidatePassword(string password)
        {
            if (string.IsNullOrEmpty(PasswordHash))
            {
                throw new ArgumentException("There is an invalid password hash to validate against.");
            }
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

        public bool HasPermissionToView(ICurrentUser currentUser)
        {
            switch (AccountType)
            {
                case AccountType.Guest:
                    return currentUser.AccountType == AccountType.Admin;
                case AccountType.Host:
                    return currentUser.AccountType == AccountType.Admin ||
                          (currentUser.AccountType == AccountType.Host && currentUser.AccountId == AccountId);
                case AccountType.Admin:
                    return currentUser.AccountType == AccountType.Admin;
                default:
                    return false;
            }
        }

        public bool HasPermissionToEdit(ICurrentUser currentUser)
        {
            switch (AccountType)
            {
                case AccountType.Guest:
                    return currentUser.AccountType == AccountType.Admin;
                case AccountType.Host:
                    return currentUser.AccountType == AccountType.Admin ||
                          (currentUser.AccountType == AccountType.Host && currentUser.AccountId == AccountId);
                case AccountType.Admin:
                    return currentUser.AccountType == AccountType.Admin;
                default:
                    return false;
            }
        }
    }
 
    public enum AccountType
    {
        Guest = 0,
        Host =  1,
        Admin = 2
    }

}