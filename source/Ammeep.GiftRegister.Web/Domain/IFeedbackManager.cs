using Ammeep.GiftRegister.Web.Domain.Model;

namespace Ammeep.GiftRegister.Web.Domain
{
    public interface IFeedbackManager
    {
        void SubmitFeedback(UserFeedback userFeedback);
    }
}