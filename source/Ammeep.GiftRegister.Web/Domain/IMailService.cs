using System.Net.Mail;
using Ammeep.GiftRegister.Web.Domain.Logging;
using Ammeep.GiftRegister.Web.Domain.Model;
using Ammeep.GiftRegister.Web.Models;

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
            GiftPurchaseConfirmationEmail emailData = new GiftPurchaseConfirmationEmail(guest,guestPurchase,gift);

            try
            {
                var smtpClient = new SmtpClient();
                var mailMessage = new MailMessage(emailData.FromAddress, emailData.ToAddress, emailData.Subject, emailData.GenerateEmailBody());
                mailMessage.IsBodyHtml = true;
                smtpClient.Send(mailMessage);
            }catch(SmtpException exception)
            {
                _logger.LogError("Could not send email",exception);
            }
        }
    }
}