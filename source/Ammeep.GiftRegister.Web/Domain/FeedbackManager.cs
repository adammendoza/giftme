using System;
using Ammeep.GiftRegister.Web.Domain.Logging;
using Ammeep.GiftRegister.Web.Domain.Model;

namespace Ammeep.GiftRegister.Web.Domain
{
    class FeedbackManager : IFeedbackManager
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly ILoggingService _loggingService;
        private readonly ICurrentUser _currentUser;

        public FeedbackManager(IFeedbackRepository feedbackRepository, ILoggingService loggingService,ICurrentUser currentUser)
        {
            _feedbackRepository = feedbackRepository;
            _loggingService = loggingService;
            _currentUser = currentUser;
        }

        public void SubmitFeedback(UserFeedback userFeedback)
        {
            _loggingService.LogInformation("Inserting user feedback");
            Feedback feedback = new Feedback
                                    {
                                        Name = userFeedback.Name,
                                        InterpretationComments = userFeedback.InterpretationComments,
                                        WasItClear = userFeedback.WasItClear,
                                        NotClearComments = userFeedback.NotClearComments,
                                        PricingModelComments = userFeedback.PricingModelComments,
                                        AgreesWithPricingModels = userFeedback.AgreesWithPricingModels,
                                        WantsToSeeReservedGifts = userFeedback.WantsToSeeReservedGifts,
                                        SeeReservedGiftsComments = userFeedback.SeeReservedGiftsComments,
                                        FinalComments = userFeedback.FinalComments,
                                        CreateDate = DateTime.Now,
                                        CreatedBy = _currentUser.AccountId,
                                        LastUpdatedBy = _currentUser.AccountId,
                                        LastUpdatedDate = DateTime.Now,

                                    };
            _feedbackRepository.InsertFeeback(feedback);
        }
    }
}