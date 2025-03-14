using System.Net;
using System.Net.Mail;

namespace Wolt.SMTP;

public class SMTPService
{
    public static void SendEmail(string toAddress, string subject, string body, string attachmentPath = null)
    {
        string senderEmail = "k.maminaishvili@gmail.com";
        string appPassword = "ykws duyg kznm rhiu";


        MailMessage mail = new MailMessage();
        mail.From = new MailAddress(senderEmail);
        mail.To.Add(toAddress);
        mail.Subject = subject;
        mail.Body = body;
        mail.IsBodyHtml = true;
        
        if (!string.IsNullOrEmpty(attachmentPath) && File.Exists(attachmentPath))
        {
            Attachment attachment = new Attachment(attachmentPath);
            mail.Attachments.Add(attachment);
        }

        var smtpClient = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            EnableSsl = true,
            Credentials = new NetworkCredential(senderEmail, appPassword)
        };

        smtpClient.Send(mail);

    }
}