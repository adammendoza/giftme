using System.Web.Mvc;
using Ammeep.GiftRegister.Web.Domain;
using Ammeep.GiftRegister.Web.Models;

namespace Ammeep.GiftRegister.Web.Controllers
{
    public class AdminController :Controller
    {
        private readonly IUserManager _userManager;

        public AdminController(IUserManager userManager)
        {
            _userManager = userManager;
        }


        public ActionResult AdminLogin()
        {
            return View(new AdminLoginModel());
        }

        [HttpPost]
        public ActionResult AdminLogin(AdminLoginModel model)
        {
            if(ModelState.IsValid)
            {
                LoginResult loginUserResult = _userManager.LoginUser(model.UserName, model.Password);
                if (loginUserResult.SuccessfulLogin)
                {
                    return RedirectToAction("Manage");
                }
                ModelState.AddModelError("AdminLogin",loginUserResult.Errors);
            }
            return View(model);
        }

        public ActionResult Manage()
        {
            return View();
        }
    }
}