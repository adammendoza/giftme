using System.Web.Mvc;
using Ammeep.GiftRegister.Web.Domain;
using Ammeep.GiftRegister.Web.Models;
using ActionResult = System.Web.Mvc.ActionResult;

namespace Ammeep.GiftRegister.Web.Controllers
{
    public class ManageRegistryController :Controller
    {
        private readonly IUserManager _userManager;

        public ManageRegistryController(IUserManager userManager)
        {
            _userManager = userManager;
        }

        public ActionResult Index()
        {
            return View();
        }
      
    }
}