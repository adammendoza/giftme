using System;
using System.Collections.Generic;
using System.Web.Security;
using Ammeep.GiftRegister.Web.Domain.Model;

namespace Ammeep.GiftRegister.Web.Domain.Authentication
{
    public class AccountMembershipService : IMembershipService
    {
        private readonly IUserRepository _userRepository;

        public AccountMembershipService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public bool ValidateUser(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");
            if (string.IsNullOrEmpty(password)) throw new ArgumentException("Value cannot be null or empty.", "password");
            Account account = _userRepository.GetUserByUserName(userName);
            return true;// PasswordHash.ValidatePassword(password, Account.PasswordHash);
        }

        public MembershipCreateStatus CreateAdminUser(EventHostAccount eventHostAccount)
        {
          
            bool usernameUnique = _userRepository.IsUsernameUnique(eventHostAccount.Username);
            if (usernameUnique)
            {
                _userRepository.InsertAdminUser(eventHostAccount);
                return MembershipCreateStatus.Success;
            }
            return MembershipCreateStatus.DuplicateUserName;
        }

        public IEnumerable<EventHostAccount> GetAllUsers()
        {
            return _userRepository.GetAllEventHostUsers();
        }

        public bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            //if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");
            //if (String.IsNullOrEmpty(oldPassword)) throw new ArgumentException("Value cannot be null or empty.", "oldPassword");
            //if (String.IsNullOrEmpty(newPassword)) throw new ArgumentException("Value cannot be null or empty.", "newPassword");

            //// The underlying ChangePassword() will throw an exception rather
            //// than return false in certain failure scenarios.
            //try
            //{
            //    MembershipUser currentUser = _provider.GetUser(userName, true /* userIsOnline */);
            //    return currentUser.ChangePassword(oldPassword, newPassword);
            //}
            //catch (ArgumentException)
            //{
            //    return false;
            //}
            //catch (MembershipPasswordException)
            //{
            //    return false;
            //}
            return false;
        }
    }
}