using System.Net;
using System.Net.Mail;
using System.Runtime;
using System.Text;
using Microsoft.Extensions.Options;

namespace canbecheaperAPI.Utility
{
    public class MailService
    {
        private readonly MailSettings _settings;

        public MailService(IOptions<MailSettings> settings)
        {
            _settings = settings.Value;
        }

        public string Send(string email, int code)
        {
            string to = email;
            string from = _settings.Email;
            MailMessage message = new MailMessage(from, to);
            message.Subject = "Kod potwierdzający rejestrację";
            message.IsBodyHtml = true;
            message.Body = $@"
        <div style='font-family: Arial, sans-serif;'>
            <h2>Potwierdzenie rejestracji</h2>
            <p>Twój kod potwierdzający:</p>
            <h1 style='letter-spacing: 6px;'>{code}</h1>
            <p>Kod jest ważny przez <b>15 minut</b>.</p>
            <p>Jeśli to nie Ty – zignoruj tę wiadomość.</p>
        </div>";

            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.Credentials = new NetworkCredential(from, _settings.Password);
            client.EnableSsl = true;

            try
            {
                client.Send(message);
                return string.Empty;
            }
            catch (Exception ex)
            {
               return "Błąd wysyłki" + ex.ToString();
            }
        }
    }
}
