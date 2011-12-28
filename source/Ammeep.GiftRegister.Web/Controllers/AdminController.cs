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


        public ActionResult Login()
        {
            return View(new AdminLoginModel());
        }

        [HttpPost]
        public ActionResult Login(AdminLoginModel model)
        {
            if(ModelState.IsValid)
            {
                LoginResult loginUserResult = _userManager.LoginUser(model.UserName, model.Password);
                if (loginUserResult.SuccessfulLogin)
                {
                    return RedirectToAction("Manage","Gift");
                }
                ModelState.AddModelError("Login",loginUserResult.Errors);
            }
            return View(model);
        }

      
    }
}