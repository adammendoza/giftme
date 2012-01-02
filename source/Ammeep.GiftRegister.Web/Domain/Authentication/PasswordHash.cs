using System;
using System.Security.Cryptography;
using System.Text;

namespace Ammeep.GiftRegister.Web.Domain.Authentication
{
    public class PasswordHash
    {
        public PasswordHash(string password)
        {
            Salt = GetRandomSalt();
            string hash = Sha256Hex(Salt + password);
            Hash = Salt + hash;
        }

        public string Hash { get; private set; }

        public string Salt { get; private set; }

        public static bool ValidatePassword(string password, string correctHash)
        {
            if (correctHash.Length < 128)
                throw new ArgumentException("correctHash must be 128 hex characters!");
            string salt = correctHash.Substring(0, 64);
            string validHash = correctHash.Substring(64, 64);
            string passHash = Sha256Hex(salt + password);
            return string.Compare(validHash, passHash) == 0;
        }

        private static string Sha256Hex(string toHash)
        {
            SHA256Managed hash = new SHA256Managed();
            byte[] utf8 = UTF8Encoding.UTF8.GetBytes(toHash);
            return BytesToHex(hash.ComputeHash(utf8));
        }

        private string GetRandomSalt()
        {
            RNGCryptoServiceProvider random = new RNGCryptoServiceProvider();
            byte[] salt = new byte[32]; //256 bits
            random.GetBytes(salt);
            return BytesToHex(salt);
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
}