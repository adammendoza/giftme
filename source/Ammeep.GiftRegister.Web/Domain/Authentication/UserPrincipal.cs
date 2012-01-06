using System.Security.Principal;

namespace Ammeep.GiftRegister.Web.Domain.Authentication
{
    public class UserPrincipal : IPrincipal
    {
        private readonly IIdentity _identity;

        public UserPrincipal(IIdentity identity)
        {
            _identity = identity;
        }

        /// <summary>
        /// Determines whether the current principal belongs to the specified role.
        /// As there are no user roles defined a user is always in role
        /// </summary>
        /// <param name="role">The name of the role for which to check membership.</param>
        /// <returns>
        /// true if the current principal is a member of the specified role; otherwise, false.
        /// </returns>
        public bool IsInRole(string role)
        {
            return true;
        }

        public IIdentity Identity
        {
            get { return _identity; }
        }
    }
}