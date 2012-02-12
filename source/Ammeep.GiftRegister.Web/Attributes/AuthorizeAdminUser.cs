using System;
using System.Web.Mvc;
using Ammeep.GiftRegister.Web.Domain;
using Ammeep.GiftRegister.Web.Domain.Model;

namespace Ammeep.GiftRegister.Web.Attributes
{
    public class AuthorizeAdminUserFilter : FilterAttribute, IAuthorizationFilter
    {
        private readonly ICurrentUser _currentUser;

        public AuthorizeAdminUserFilter(ICurrentUser currentUser)
        {
            _currentUser = currentUser;
        }

     

        public void OnAuthorization(AuthorizationContext filterContext)
        {
           bool isAdmin = _currentUser.AccountType == AccountType.Admin;
           if(!isAdmin)
           {
               filterContext.Result = new ViewResult
               {
                   ViewName = "~/Views/Shared/Unauthorized.cshtml"
               };

           }
        }
    }

    public class AuthorizeAdminUserAttribute : Attribute
    {
    }
}