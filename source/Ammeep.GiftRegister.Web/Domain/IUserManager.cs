using System.Collections.Generic;
using System.Web.Security;
using Ammeep.GiftRegister.Web.Domain.Authentication;
using Ammeep.GiftRegister.Web.Domain.Model;
using Ammeep.GiftRegister.Web.Domain.Validation;

namespace Ammeep.GiftRegister.Web.Domain
{
    public interface IUserManager
    {
        Result SignIn(string userName, string password, bool rememberMe);
        void SignOut();
        Result RegisterHostUser(string userName, string name, string password, string email);
        IEnumerable<AdminAccount> GetEventHostUsers();
    }

    public class UserManager : IUserManager
    {

        private readonly IFormsAuthenticationService _formsAuthenticationService;
        private readonly IMembershipService _membershipService;
        private readonly IUserRepository _userRepository;

        public UserManager(IFormsAuthenticationService formsAuthenticationService, IMembershipService membershipService, IUserRepository userRepository)
        {
            _formsAuthenticationService = formsAuthenticationService;
            _membershipService = membershipService;
            _userRepository = userRepository;
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
            MembershipCreateStatus createStatus = _membershipService.CreateAdminUser(hostAccount);

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

        public IEnumerable<AdminAccount> GetEventHostUsers()
        {
           return _membershipService.GetAllUsers();
        }
    }
}