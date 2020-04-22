using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net.Mail;

namespace Loyalty_Email_API.Controllers
{
    
    [ApiController]
    [Route("api/EmailController")]
    public class EmailController : ControllerBase
    {
        [HttpPost()]
        public IActionResult SendMail(Email email)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            var configuration = builder.Build();

            string ServidorSMTP = string.Empty;
            int PuertoServidorSMTP = 0;
            string RemitenteMail = string.Empty;
            string DisplayNameRemitente = string.Empty;
            string PassMailRemitente = string.Empty;
            bool Ssl = false;

            ServidorSMTP = configuration["ServidorSMTP"];
            PuertoServidorSMTP = int.Parse(configuration["PuertoServidorSMTP"]);
            RemitenteMail = configuration["RemitenteMail"];
            DisplayNameRemitente = configuration["DisplayNameRemitente"];
            PassMailRemitente = configuration["PassMailRemitente"];
            Ssl = bool.Parse(configuration["Ssl"]);

            SmtpClient smtpClient = new SmtpClient(ServidorSMTP, PuertoServidorSMTP)
            {
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(RemitenteMail, PassMailRemitente),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = Ssl
            };

            MailMessage mail = new MailMessage();
            mail.IsBodyHtml = email.IsBodyHtml;
            mail.Body = email.CuerpoMail;
            mail.Subject = email.AsuntoMail;
            mail.From = new MailAddress(RemitenteMail, DisplayNameRemitente);
            mail.To.Add(new MailAddress(email.Destinatario));

            try
            {
                smtpClient.Send(mail);

                Logger.LoggerMessage("Email API: 200 - Email has been send");
                return StatusCode(200, "Email has been send");
            }
            catch (SmtpFailedRecipientsException ex)
            {
                for (int i = 0; i < ex.InnerExceptions.Length; i++)
                {
                    SmtpStatusCode status = ex.InnerExceptions[i].StatusCode;
                    if (status == SmtpStatusCode.MailboxBusy ||
                        status == SmtpStatusCode.MailboxUnavailable)
                    {
                        
                        System.Threading.Thread.Sleep(5000);
                        smtpClient.Send(mail);
                        return StatusCode(200, "Email has been send");
                    }
                    else
                    {
                        Logger.LoggerMessage("Email API: 520 - " + status + " - " + ex.Message.ToString());
                        return StatusCode(520, status + " - " + ex.Message.ToString());
                    }
                }
                Logger.LoggerMessage("Email API: 520" + ex.Message.ToString());
                return StatusCode(520, ex.Message.ToString());
            }
            catch (SmtpException ex)
            {
                Logger.LoggerMessage("Email API: 520 - " + ex.Message.ToString());
                return StatusCode(520, ex.Message.ToString());
            }
            catch (Exception ex)
            {
                Logger.LoggerMessage("Email API: 520 - " + ex.Message.ToString());
                return StatusCode(520, ex.Message.ToString());
            }
        }
    }
}
