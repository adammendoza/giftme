using System.Collections.Generic;
using System.Web.Security;
using Ammeep.GiftRegister.Web.Domain.Authentication;
using Ammeep.GiftRegister.Web.Domain.Logging;
using Ammeep.GiftRegister.Web.Domain.Model;
using Ammeep.GiftRegister.Web.Domain.Validation;

namespace Ammeep.GiftRegister.Web.Domain
{
    public interface IUserManager
    {
        Result SignIn(string userName, string password, bool rememberMe);
        void SignOut();
        Result RegisterHostUser(string userName, string name, string password, string email);
        IEnumerable<Account> GetAdminUsers();
        IEnumerable<Account> GetGuestList();
        Account GetAccount(int accountId);
    }

    public class UserManager : IUserManager
    {

        private readonly IFormsAuthenticationService _formsAuthenticationService;
        private readonly IUserRepository _userRepository;
        private readonly ILoggingService _loggingService;

        public UserManager(IFormsAuthenticationService formsAuthenticationService,IUserRepository userRepository, ILoggingService loggingService)
        {
            _formsAuthenticationService = formsAuthenticationService;
            _userRepository = userRepository;
            _loggingService = loggingService;
        }

        public Result SignIn(string userName, string password, bool rememberMe)
        {
            Result result = new Result();
            bool passwordCorrect = ValidateAccount(userName, password);
            if (passwordCorrect)
            {
                _formsAuthenticationService.SignIn(userName, rememberMe);
                result.Successful = true;
            }else
            {
                result.Errors.Add("", "The user name or password provided is incorrect.");
            }
            return result;
        }

        private bool ValidateAccount(string userName, string password)
        {
            Account adminAccount = _userRepository.GetAdminUserByUsername(userName);
            if(adminAccount != null)
            {
                return adminAccount.ValidatePassword(password);
            }
            return false;
        }

        public void SignOut()
        {
            _formsAuthenticationService.SignOut();
        }

        public Result RegisterHostUser(string userName, string name, string password, string email)
        {
            Result result = new Result();
            Account hostAccount = new Account(AccountType.Host,userName, password, name, email);
            bool usernameUnique = _userRepository.IsUsernameUnique(hostAccount.Username);
            MembershipCreateStatus createStatus;
            if (usernameUnique)
            {
                _userRepository.InsertAdminUser(hostAccount);
                createStatus = MembershipCreateStatus.Success;
            }
            else
            {
                createStatus = MembershipCreateStatus.DuplicateUserName;
            }
            if (createStatus == MembershipCreateStatus.Success)
            {
                _formsAuthenticationService.SignIn(userName, false);
                result.Successful = true;
            }
            else
            {
                result.Errors.Add("", AccountValidation.ErrorCodeToString(createStatus));
            }
            return result;
        }

        public IEnumerable<Account> GetAdminUsers()
        {
           return _userRepository.GetAllAdminUsers();
        }

        public IEnumerable<Account> GetGuestList()
        {
            return _userRepository.GetAllGuestUsers();
        }
    
        public Account GetAccount(int accountId)
        {
            return _userRepository.GetAccountById(accountId);
        }

        
    }
}