using biz.main.tecnicah.Models.Email;
using biz.main.tecnicah.Services.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace dal.main.tecnicah.Services.Email
{
    public class EmailService : IEmailService
    {
        public string SendEmail(EmailModel email)
        {
            var response = "";
            try
            {
                SmtpClient smtpClient = new SmtpClient("Smtp.Gmail.com", 587);
                // smtpClient.TargetName = "STARTTLS/smtp.office365.com";
                smtpClient.EnableSsl = true;
                smtpClient.Timeout = 10000;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential("info@4mypatient.org", "hwbfteakcdipyzsy");
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("info@4mypatient.org", "4MyPatient");
                mailMessage.To.Add(email.To);
                mailMessage.Subject = email.Subject;
                mailMessage.SubjectEncoding = System.Text.Encoding.UTF8;
                mailMessage.IsBodyHtml = email.IsBodyHtml;
                mailMessage.Body = email.Body;
                mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
                smtpClient.Send(mailMessage);

                response = "Correo enviado";
            }
            catch (Exception ex)
            {
                response = ex.ToString();
            }

            return response;
        }

        public string SendEmailAttach(EmailModelAttach email)
        {
            var response = "";
            try
            {
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Host = "Smtp.Gmail.com";
                smtpClient.Port = 587;
                smtpClient.EnableSsl = true;
                smtpClient.Timeout = 10000;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential("lucero.vargas@zumit.tech", "Temporal9808");

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("lucero.vargas@zumit.tech", "Hemofilia");
                mailMessage.To.Add(email.To);
                mailMessage.Subject = email.Subject;
                mailMessage.IsBodyHtml = email.IsBodyHtml;
                mailMessage.Body = email.Body;

                foreach (var data in email.File)
                {
                    //string path = HttpContext.Current.Server.MapPath();
                    string random = Path.GetFullPath(data.attach);
                    Attachment attachment;
                    attachment = new Attachment(random);
                    mailMessage.Attachments.Add(attachment);
                }

                smtpClient.Send(mailMessage);

                response = "Correo enviado";
            }
            catch (Exception ex)
            {
                response = ex.ToString();
            }

            return response;
        }
    }
}
