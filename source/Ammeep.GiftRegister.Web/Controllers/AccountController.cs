﻿using System.Web.Mvc;
using Ammeep.GiftRegister.Web.Domain;
using Ammeep.GiftRegister.Web.Models;

namespace Ammeep.GiftRegister.Web.Controllers
{

    [HandleError]
    public class AccountController : Controller
    {
        private readonly IUserManager _userManager;
        private readonly IConfiguration _configuration;

        public AccountController(IUserManager userManager,IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
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
                    return RedirectToAction("Login", "ManageRegistry");
                }
                loginResult.AddModelErrors(ModelState);
            }
            return View(model);
        }


        public ActionResult LogOff()
        {
            _userManager.SignOut();
            return RedirectToAction("Login", "ManageRegistry");
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
                Result result = _userManager.RegisterUser(model.UserName,model.FirstName,model.LastName, model.Password, model.Email);
                if (result.Successful)
                {                  
                    return RedirectToAction("Index", "ManageRegistry");
                }
                result.AddModelErrors(ModelState);
            }

            ViewData["PasswordLength"] = _configuration.MinimumPasswordLength;
            return View(model);
        }

        //[Authorize]
        //public ActionResult ChangePassword()
        //{
        //    ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
        //    return View();
        //}

        //[Authorize]
        //[HttpPost]
        //public ActionResult ChangePassword(ChangePasswordModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (MembershipService.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword))
        //        {
        //            return RedirectToAction("ChangePasswordSuccess");
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
        //        }
        //    }

        //    // If we got this far, something failed, redisplay form
        //    ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
        //    return View(model);
        //}

        //// **************************************
        //// URL: /Account/ChangePasswordSuccess
        //// **************************************

        //public ActionResult ChangePasswordSuccess()
        //{
        //    return View();
        //}

    }
}
