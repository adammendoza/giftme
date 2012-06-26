using System.Web.Mvc;
using Ammeep.GiftRegister.Web.Domain;
using Ammeep.GiftRegister.Web.Domain.Model;

namespace Ammeep.GiftRegister.Web.Controllers
{
    public class FeedbackController :Controller
    {
        private readonly IFeedbackManager _feedbackManager;

        public FeedbackController(IFeedbackManager feedbackManager)
        {
            _feedbackManager = feedbackManager;
        }

        public ActionResult Index()
        {
            return View(new UserFeedback());
        }

        [HttpPost]
        public ActionResult Index(UserFeedback userFeedback)
        {
            if(ModelState.IsValid)
            {
                _feedbackManager.SubmitFeedback(userFeedback);
                return RedirectToAction("Thanks");
            }
            return View(userFeedback);
        }

        public ActionResult Thanks()
        {
            return View();
        }
    }

}