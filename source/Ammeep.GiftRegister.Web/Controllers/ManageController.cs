using System.Web.Mvc;
using Ammeep.GiftRegister.Web.Domain;

namespace Ammeep.GiftRegister.Web.Controllers
{
    [Authorize]
    public class ManageController :Controller
    {
        private readonly IUserManager _userManager;

        public ManageController(IUserManager userManager)
        {
            _userManager = userManager;
        }

        public ActionResult Index()
        {
            return View();
        }
      
    }
}