using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Utility
{
    public interface IEmailHandler
    {
        void SendEmail(string fromEmail, string[] toEmail, string emailSubject, string emailContent, bool isHtmlFormat,
            string displayName, Dictionary<string, string> listAttachmentFilename = null, string[] ccEmail = null);

        void SendEmailSsl(string fromEmail, string[] toEmail, string emailSubject, string emailContent, byte[] linkImage,bool isHtmlFormat,
            string displayName, Dictionary<string, string> listAttachmentFilename = null, string[] ccEmail = null);
    }

    public class EmailHandler : IEmailHandler
    {
        private readonly SmtpClient _smtpClient;
        public EmailHandler()
        {
            var appSettingReader = new AppSettingsReader();
            _smtpClient = new SmtpClient
            {

                Host = (string)appSettingReader.GetValue("Host", typeof(String)),
                EnableSsl = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,                
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential((string)appSettingReader.GetValue("EmailFrom", typeof(String)), (string)appSettingReader.GetValue("Password", typeof(String)))
            };
        }

        public void SendEmail(string fromEmail, string[] toEmail, string emailSubject, string emailContent, bool isHtmlFormat,
                                string displayName, Dictionary<string, string> listAttachmentFilename = null, string[] ccEmail = null)
        {
            var message = new MailMessage { From = new MailAddress(fromEmail, displayName) };
            if (toEmail.Length > 0)
            {
                foreach (var item in toEmail.Where(item => !string.IsNullOrEmpty(item)))
                {
                    message.To.Add(new MailAddress(item));
                }
            }
            if (ccEmail != null && ccEmail.Length > 0)
            {
                foreach (var item in toEmail.Where(item => !string.IsNullOrEmpty(item)))
                {
                    message.CC.Add(new MailAddress(item));
                }
            }
            message.Subject = emailSubject;
            message.Body = emailContent;
            message.IsBodyHtml = isHtmlFormat;
            message.BodyEncoding = Encoding.UTF8;
            message.SubjectEncoding = Encoding.UTF8;
            if (listAttachmentFilename != null && listAttachmentFilename.Count > 0)
            {
                foreach (var attachmentFilename in listAttachmentFilename)
                {
                    var attachment = new Attachment(attachmentFilename.Value, MediaTypeNames.Application.Octet);
                    var disposition = attachment.ContentDisposition;
                    disposition.CreationDate = File.GetCreationTime(attachmentFilename.Value);
                    disposition.ModificationDate = File.GetLastWriteTime(attachmentFilename.Value);
                    disposition.ReadDate = File.GetLastAccessTime(attachmentFilename.Value);
                    disposition.FileName = attachmentFilename.Key;
                    disposition.Size = new FileInfo(attachmentFilename.Value).Length;
                    disposition.DispositionType = DispositionTypeNames.Attachment;
                    message.Attachments.Add(attachment);
                }
            }
            
            _smtpClient.Send(message);
        }

        public void SendEmailSsl(string fromEmail, string[] toEmail, string emailSubject, string emailContent, byte[] linkImage, bool isHtmlFormat,
                                string displayName, Dictionary<string, string> listAttachmentFilename = null, string[] ccEmail = null)
        {
            var appSettingReader = new AppSettingsReader();
            MailMessage mail = new MailMessage { From = new MailAddress(fromEmail, displayName) };
            if (toEmail.Length > 0)
            {
                foreach (var item in toEmail.Where(item => !string.IsNullOrEmpty(item)))
                {
                    mail.To.Add(new MailAddress(item));
                }
            }

            //
            //var path = AppDomain.CurrentDomain.BaseDirectory;

            var htmlView = AlternateView.CreateAlternateViewFromString(
              emailContent,
               null, "text/html");

            //http
            //string image = linkImage;
            //var webClient = new WebClient();
            //byte[] imageBytes = webClient.DownloadData(image);
            MemoryStream ms = new MemoryStream(linkImage);
            //
            LinkedResource logo = new LinkedResource(ms, MediaTypeNames.Image.Jpeg); //new LinkedResource(path + "Content\\img\\logo.png");
            logo.ContentId = "companylogo";

            htmlView.LinkedResources.Add(logo);

            mail.AlternateViews.Add(htmlView);
            //
            mail.Subject = emailSubject;
            //mail.Body = emailContent;
            //mail.IsBodyHtml = true;

            Task.Factory.StartNew(() =>
            {
                using (var smtp = new SmtpClient())
                {
                    smtp.Host = (string)appSettingReader.GetValue("Host", typeof(String));
                    smtp.Port = Convert.ToInt32((string)appSettingReader.GetValue("Port", typeof(String)));
                    smtp.Credentials = new NetworkCredential((string)appSettingReader.GetValue("EmailFrom", typeof(String)), (string)appSettingReader.GetValue("Password", typeof(String)));
                    smtp.EnableSsl = true;

                    smtp.Send(mail);
                }
            });
        }
    }
}