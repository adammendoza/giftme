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

        private readonly IAuthenticationService _authenticationService;
        private readonly IUserRepository _userRepository;
        private readonly ILoggingService _loggingService;

        public UserManager(IAuthenticationService authenticationService,IUserRepository userRepository, ILoggingService loggingService)
        {
            _authenticationService = authenticationService;
            _userRepository = userRepository;
            _loggingService = loggingService;
        }

        public Result SignIn(string userName, string password, bool rememberMe)
        {
            _loggingService.LogInformation(string.Format("User {0} attempting to sign in",userName));
            Result result = new Result();

            Account adminAccount = _userRepository.GetAdminUserByUsername(userName);
            bool passwordCorrect = false;
            if (adminAccount != null)
            {
                passwordCorrect = adminAccount.ValidatePassword(password);
                if (!passwordCorrect)
                {
                    _loggingService.LogError(string.Format("User {0} attempted to sign in with an incorrect password.", userName));
                }
            }

            if (passwordCorrect)
            {
                _authenticationService.SignIn(adminAccount);
                _loggingService.LogInformation(string.Format("User {0} successfully signed in", userName));
                result.Successful = true;
            }else
            {
                _loggingService.LogWarning(string.Format("User {0} could not sign in. The user name or password provided is incorrect.", userName));
                result.Errors.Add("", "The user name or password provided is incorrect.");
            }
            return result;
        }

       

        public void SignOut()
        {
            _loggingService.LogInformation("User signing out");
            _authenticationService.SignOut();
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
                _authenticationService.SignIn(hostAccount);
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