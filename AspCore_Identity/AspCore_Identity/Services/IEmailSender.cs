using System.Threading.Tasks;

namespace AspCore_Identity.Services
{
    public interface IEmailSender
    {
        Task SendEmailAddress(string FromAddress, string ToAddress, string Subject, string Message);
    }
}
