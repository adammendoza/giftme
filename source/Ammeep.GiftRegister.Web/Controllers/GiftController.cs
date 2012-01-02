using System.Linq;
using System.Web.Mvc;
using Ammeep.GiftRegister.Web.Domain;
using Ammeep.GiftRegister.Web.Models;

namespace Ammeep.GiftRegister.Web.Controllers
{
    public class GiftController : Controller
    {
        private readonly IRegistryManager _registryManager;
        private readonly IConfiguration _config;

        public GiftController(IRegistryManager registryManager, IConfiguration configuration)
        {
            _registryManager = registryManager;
            _config = configuration;
        }

        public ActionResult Registry()
        {
            var registryPageSize = _config.RegistryPageSize;
            var gifts = _registryManager.GetRegistry(registryPageSize, 0, 0);
            var categories = _registryManager.GetCategories();
            RegistryItemsPage itemsPage = new RegistryItemsPage(gifts,categories,registryPageSize);
            return View(itemsPage);
        }

        public PartialViewResult NextItems(int pageSize, int pageNumber, int categoryId)
        {
            var nextItems = _registryManager.GetRegistry(pageSize, pageNumber, categoryId);
            return PartialView("RegistryItems", nextItems.Select(gift => new GiftRow {Item = gift}));
        }
    }
}
