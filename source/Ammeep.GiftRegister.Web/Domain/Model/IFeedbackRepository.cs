using Simple.Data;

namespace Ammeep.GiftRegister.Web.Domain.Model
{
    public interface IFeedbackRepository
    {
        void InsertFeeback(Feedback feedback);
    }

    class FeedbackRepository : IFeedbackRepository
    {
        private readonly IConfiguration _configuration;

        public FeedbackRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void InsertFeeback(Feedback feedback)
        {
            var connection = Database.OpenConnection(_configuration.GiftmeConnectionString);
            connection.Feedback.Insert(feedback);
        }
    }
}