using System.Web.Mvc;
using Ammeep.GiftRegister.Web.Domain;
using Ammeep.GiftRegister.Web.Models;

namespace Ammeep.GiftRegister.Web.Controllers
{
    public class RegistryController : Controller
    {
        private readonly IRegistryManager _registryManager;
        private readonly IConfiguration _config;

        public RegistryController(IRegistryManager registryManager, IConfiguration configuration)
        {
            _registryManager = registryManager;
            _config = configuration;
        }

        public ActionResult Index()
        {
            var registryPageSize = _config.RegistryPageSize;
            var gifts = _registryManager.GetRegistry(registryPageSize, 0, 0);
            var categories = _registryManager.GetCategories();
            RegistryItemsPage itemsPage = new RegistryItemsPage(gifts,categories,registryPageSize,0);
            return View(itemsPage);
        }

        public ActionResult RegistryPage(int page,int categoryId)
        {
            var registryPageSize = _config.RegistryPageSize;
            var gifts = _registryManager.GetRegistry(registryPageSize, page, categoryId);
            var categories = _registryManager.GetCategories();
            RegistryItemsPage itemsPage = new RegistryItemsPage(gifts, categories, registryPageSize,categoryId);
            return View("Index",itemsPage);
        }


    

        [HttpPost]
        public ActionResult GetThis(GetThisModel getThisModel)
        {
            if(ModelState.IsValid)
            {
                return PartialView("ThankYou");
                
            }
            return PartialView("Error");
        }
    }
}
