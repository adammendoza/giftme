using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Ammeep.GiftRegister.Web.Attributes;
using Ammeep.GiftRegister.Web.Domain;
using Ammeep.GiftRegister.Web.Domain.Model;
using Ammeep.GiftRegister.Web.Models;

namespace Ammeep.GiftRegister.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserManager _userManager;
        private readonly IConfiguration _configuration;

        public AccountController(IUserManager userManager,IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        [AuthorizeAdminUser]
        public ActionResult Index()
        {
            IEnumerable<Account> adminUsers = _userManager.GetAdminUsers();
            IEnumerable<Account> guests = _userManager.GetGuestList();
            AllAccountsPage usersPage = new AllAccountsPage();
            usersPage.AdminUsers = adminUsers;
            usersPage.Guests = guests;
            return View(usersPage);
        }

      
        public ActionResult LogOn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                Result loginResult = _userManager.SignIn(model.UserName, model.Password,model.RememberMe);
                if (loginResult.Successful && !string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                if (loginResult.Successful)
                {
                    return RedirectToAction("Index", "Manage");
                }
                loginResult.AddModelErrors(ModelState);
            }
            return View(model);
        }


        public ActionResult LogOff()
        {
            _userManager.SignOut();
            return RedirectToAction("Index", "Registry");
        }


        public ActionResult Register()
        {
            ViewData["PasswordLength"] = _configuration.MinimumPasswordLength;
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                Result result = _userManager.RegisterHostUser(model.UserName, model.Name, model.Password, model.Email);
                if (result.Successful)
                {                  
                    return RedirectToAction("Index", "Manage");
                }
                result.AddModelErrors(ModelState);
            }

            ViewData["PasswordLength"] = _configuration.MinimumPasswordLength;
            return View(model);
        }
    }


}
