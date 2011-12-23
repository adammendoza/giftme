using System.Web.Mvc;
using Ammeep.GiftRegister.Web.Models;

namespace Ammeep.GiftRegister.Web.Controllers
{
    public class AdminController :Controller
    {
        public ActionResult AdminLogin()
        {
            return View(new AdminLoginModel());
        }
    }
}