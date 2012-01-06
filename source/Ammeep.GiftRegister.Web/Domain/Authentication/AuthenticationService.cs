using System;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using Ammeep.GiftRegister.Web.Domain.Model;

namespace Ammeep.GiftRegister.Web.Domain.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IConfiguration _configuration;

        public AuthenticationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SignIn(Account account)
        {
            if (account == null) throw new ArgumentException("Value cannot be null.", "account");

            DateTime ticketIssueTime = DateTime.Now;
            DateTime ticketExpiryTime = ticketIssueTime.AddMinutes(_configuration.AuthenticationExipryMinutes);
            string userData = string.Format("{0}|{1}", account.AccountId, account.AccountType);
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, account.Name, ticketIssueTime, ticketExpiryTime, false, userData);
            string encryptedTicket = FormsAuthentication.Encrypt(ticket);
            HttpCookie authCookie = new HttpCookie(_configuration.AuthenticatedUserCookieName, encryptedTicket);
            HttpContext.Current.Response.Cookies.Add(authCookie);
        }

        public void SignOut()
        {
            HttpCookie httpCookie = HttpContext.Current.Request.Cookies[_configuration.AuthenticatedUserCookieName];
            if (httpCookie != null)
            {
                httpCookie.Expires = DateTime.Now.Subtract(TimeSpan.FromDays(30));
                HttpContext.Current.Response.Cookies.Add(httpCookie);
                FormsAuthentication.SignOut();
            }
        }

        public void AuthenticateRequest(HttpCookie authenticationCookie)
        {
            if (authenticationCookie != null)
            {
                SetCustomPrincipal(authenticationCookie);
            }
            else
            {
                //ensure we have no principal running
                HttpContext.Current.Response.Cookies.Remove(_configuration.AuthenticatedUserCookieName);
                HttpContext.Current.User = null;
            }
        }

        private void SetCustomPrincipal(HttpCookie authenticationCookie)
        {
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authenticationCookie.Value);

            if (ticket != null && !ticket.Expired)
            {

                IPrincipal userPrincipal = CreateUserPrincipal(ticket);
                HttpContext.Current.User = userPrincipal;
            }
        }

        /// <summary>
        /// Creates the user principal from a forms authentication ticket.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns></returns>
        protected IPrincipal CreateUserPrincipal(FormsAuthenticationTicket ticket)
        {
            IPrincipal userPrincipal = null;
            if (ticket != null && !ticket.Expired)
            {
                string[] data = ticket.UserData.Split('|');
                int accountId = Convert.ToInt32(data[0]);
                string accountTypeString = data[1];
                AccountType accountType = (AccountType) Enum.Parse(typeof(AccountType), accountTypeString);
                IIdentity userIdentity = new SignedInUser(accountType,accountId, ticket.Name,true);
                userPrincipal = new UserPrincipal(userIdentity);
            }
            return userPrincipal;
        }
    }
}