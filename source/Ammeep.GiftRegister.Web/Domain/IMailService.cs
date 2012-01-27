using System.Net.Mail;
using Ammeep.GiftRegister.Web.Domain.Logging;
using Ammeep.GiftRegister.Web.Domain.Model;

namespace Ammeep.GiftRegister.Web.Domain
{
    public interface IMailService
    {
        void SendPurchaseConfirmationEmail(Guest guest, GiftPruchase guestPurchase, Gift gift);
    }

    public class MailService : IMailService
    {
        private readonly ILoggingService _logger;

        public MailService(ILoggingService logger)
        {
            _logger = logger;
        }

        public void SendPurchaseConfirmationEmail(Guest guest, GiftPruchase guestPurchase, Gift gift)
        {
            try
            {
                var smtpClient = new SmtpClient();
                smtpClient.Send(new MailMessage("a.palamountain@gmail.com", "amy@palamounta.in", "test", "hello"));
            }catch(SmtpException exception)
            {
                _logger.LogError("Could not send email",exception);
            }
        }
    }
}