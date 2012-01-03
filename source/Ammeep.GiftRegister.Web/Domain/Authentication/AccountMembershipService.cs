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
            User user = _userRepository.GetUserByUserName(userName);
            return true;// PasswordHash.ValidatePassword(password, user.PasswordHash);
        }

        public MembershipCreateStatus CreateUser(string userName,string firstName,string lastName, string password, string email)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");
            if (String.IsNullOrEmpty(password)) throw new ArgumentException("Value cannot be null or empty.", "password");
            if (String.IsNullOrEmpty(email)) throw new ArgumentException("Value cannot be null or empty.", "email");

            bool usernameUnique = _userRepository.IsUsernameUnique(userName);
            if (usernameUnique)
            {
                //PasswordHash hash = new PasswordHash(password);

                //User newUser = new User();
                //newUser.UserName = userName;
                //newUser.Email = email;
                //newUser.FirstName = firstName;
                //newUser.LastName = lastName;
                //newUser.PasswordHash = hash.Hash;
                //newUser.PasswordSalt = hash.Salt;
                //_userRepository.InsertUser(newUser);
                return MembershipCreateStatus.Success;
            }
            return MembershipCreateStatus.DuplicateUserName;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _userRepository.GetAllUsers();
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