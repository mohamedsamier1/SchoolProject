using MailKit.Net.Smtp;
using MimeKit;
using SchoolProject.Data.Helpers;
using SchoolProject.Service.Abstracts;

namespace SchoolProject.Service.Implementations
{
    public class EmailService : IEmailService
    {
        #region Fields
        private readonly EmailSettings _emailsettings;
        #endregion
        #region Constructor
        public EmailService(EmailSettings emailsettings)
        {
            _emailsettings = emailsettings;
        }
        #endregion
        #region Handle func
        public async Task<string> SendEmail(string email, string Message, string? reason)
        {
            try
            {
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(_emailsettings.Host, _emailsettings.Port, true);
                    client.Authenticate(_emailsettings.FromEmail, _emailsettings.Password);
                    var bodybuilder = new BodyBuilder
                    {
                        HtmlBody = $"{Message}",
                        TextBody = "Wellcome"
                    };
                    var message = new MimeMessage
                    {
                        Body = bodybuilder.ToMessageBody()
                    };
                    message.From.Add(new MailboxAddress("Future Team", _emailsettings.FromEmail));
                    message.To.Add(new MailboxAddress("Testing", email));
                    message.Subject = reason == null ? "No Submitted" : reason;
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
                return "Success";
            }
            catch (Exception)
            {

                return "Failed";
            }

        }
        #endregion

    }
}
