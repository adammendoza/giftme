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
        IEnumerable<AdminAccount> GetAdminUsers();
        IEnumerable<GuestAccount> GetGuestList();
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
            AdminAccount adminAccount = _userRepository.GetAdminUserByUsername(userName);
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
            AdminAccount hostAccount = new AdminAccount(name, email,userName,password);
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

        public IEnumerable<AdminAccount> GetAdminUsers()
        {
           return _userRepository.GetAllAdminUsers();
        }

        public IEnumerable<GuestAccount> GetGuestList()
        {
            return _userRepository.GetAllGuestUsers();
        }
    }
}