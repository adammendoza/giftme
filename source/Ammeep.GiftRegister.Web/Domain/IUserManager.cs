using System.Web.Security;
using Ammeep.GiftRegister.Web.Domain.Authentication;
using Ammeep.GiftRegister.Web.Domain.Validation;

namespace Ammeep.GiftRegister.Web.Domain
{
    public interface IUserManager
    {
        Result SignIn(string userName, string password, bool rememberMe);
        void SignOut();
        Result RegisterUser(string userName, string firstName, string lastName, string password, string email);
    }

    public class UserManager : IUserManager
    {

        private readonly IFormsAuthenticationService _formsAuthenticationService;
        private readonly IMembershipService _membershipService;

        public UserManager(IFormsAuthenticationService formsAuthenticationService,IMembershipService membershipService)
        {
            _formsAuthenticationService = formsAuthenticationService;
            _membershipService = membershipService;
        }

        public Result SignIn(string userName, string password, bool rememberMe)
        {
            Result result = new Result();
            if (_membershipService.ValidateUser(userName, password))
            {
                _formsAuthenticationService.SignIn(userName, rememberMe);
                result.Successful = true;
            }else
            {
                result.Errors.Add("", "The user name or password provided is incorrect.");
            }
            return result;
        }

        public void SignOut()
        {
            _formsAuthenticationService.SignOut();
        }

        public Result RegisterUser(string userName,string firstName ,string lastName ,string password, string email)
        {
            Result result = new Result();
            MembershipCreateStatus createStatus = _membershipService.CreateUser(userName,firstName,lastName,password,email);

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
    }
}