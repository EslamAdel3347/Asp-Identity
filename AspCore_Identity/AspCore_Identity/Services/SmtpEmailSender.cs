using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace AspCore_Identity.Services
{
    public class SmtpEmailSender : IEmailSender
    {
        public readonly IOptions<SmtpOptions> _options;

        public SmtpEmailSender(IOptions<SmtpOptions> options)
        {
            _options = options;
        }
        public async Task SendEmailAddress(string FromAddress, string ToAddress, string Subject, string Message)
        {
            var MailMessage = new MailMessage(FromAddress, ToAddress, Subject, Message);

            using (var Cilent = new SmtpClient(_options.Value.Host, _options.Value.port)
            {
                Credentials = new NetworkCredential(_options.Value.Username, _options.Value.Password),
                EnableSsl=true
                
            })
            {
                await Cilent.SendMailAsync(MailMessage);
            }


        }
    }
}
