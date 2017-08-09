using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Mail;
using System.Web;
using Peer.PublicCsharpModule.CSharpLogToFile;
using System.Threading;
namespace Peer.PublicCsharpModule.PCSharpemail
{
    public class SendEmail
    {


        public static bool GoToSendEmail(string email, string name, string subject, string message,string tel, string smtpServer, string smtpServerPort,
            string smtpUserName, string smtpPassword, string boolEnableSsl)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(smtpUserName, "消息服务中心");
                    mail.ReplyToList.Add(new MailAddress(smtpUserName, "消息服务中心"));

                    mail.To.Add(smtpUserName);
                    mail.Subject = "邮件主题：" +  subject;

                    mail.Body = "<div style=\"font: 11px verdana, arial\">";
                    mail.Body += System.Web.HttpUtility.HtmlEncode(message).Replace("\n", "<br />") + "<br /><br />";
                    mail.Body += "<hr /><br />";
                    mail.Body += "<div style=\"font-size:10px;line-height:16px\">";
                    mail.Body += "<strong>" + "联系人名字" + ":</strong> " + System.Web.HttpUtility.HtmlEncode(name) + "<br />";
                    mail.Body += "<strong>" + "联系人邮箱" + ":</strong> " + System.Web.HttpUtility.HtmlEncode(email) + "<br />";
                    mail.Body += "<strong>" + "联系人电话" + ":</strong> " + System.Web.HttpUtility.HtmlEncode(tel) + "<br />";

                    if (SendMailMessage(mail,smtpServer,smtpServerPort,smtpUserName,smtpPassword,boolEnableSsl).Length > 0)
                    {
                        return false;
                    };
                }

                return true;
            }
            catch (Exception ex)
            {
                CSharpLogToFile.LogManager.WriteLog(LogFile.Error, ex.Message.ToString());

                return false;
            }
        }










       
        /// <summary>
        /// Sends a MailMessage object using the SMTP settings.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="smtpServer">SMTP server</param>
        /// <param name="smtpServerPort">SMTP server port</param>
        /// <param name="smtpUserName">User name</param>
        /// <param name="smtpPassword">Password</param>
        /// <param name="enableSsl">Enable SSL</param>
        /// <returns>
        /// Error message, if any.
        /// </returns>
        public static string SendMailMessage(MailMessage message, string smtpServer , string smtpServerPort,
            string smtpUserName , string smtpPassword , string boolEnableSsl)
        {
            StringBuilder errorMsg = new StringBuilder();
            bool boolSssl = false;
            int intPort = 25;

            if (message == null)
                throw new ArgumentNullException("message");

            try
            {
               
                if (!string.IsNullOrEmpty(smtpServerPort))
                    intPort = int.Parse(smtpServerPort);

                if (!string.IsNullOrEmpty(boolEnableSsl))
                    bool.TryParse(boolEnableSsl, out boolSssl);

                message.IsBodyHtml = true;
                message.BodyEncoding = Encoding.UTF8;
                var smtp = new SmtpClient(smtpServer);

                // don't send credentials if a server doesn't require it,
                // linux smtp servers don't like that 
                if (!string.IsNullOrEmpty(smtpUserName))
                {
                    smtp.Credentials = new NetworkCredential(smtpUserName, smtpPassword);
                }

                smtp.Port = intPort;
                smtp.EnableSsl = boolSssl;
                smtp.Send(message);
                OnEmailSent(message);
            }
            catch (Exception ex)
            {
                OnEmailFailed(message);

                errorMsg.Append("SendMailMessage函数发送邮件失败: ");
                Exception current = ex;

                while (current != null)
                {
                    if (errorMsg.Length > 0) { errorMsg.Append(" "); }
                    errorMsg.Append(current.Message);
                    current = current.InnerException;
                }

                LogManager.WriteLog(LogFile.Error, errorMsg.ToString());
            }
            finally
            {
                // Remove the pointer to the message object so the GC can close the thread.
                message.Dispose();
            }

            return errorMsg.ToString();
        }
        /// <summary>
        ///     Occurs after an e-mail has been sent. The sender is the MailMessage object.
        /// </summary>
        public static event EventHandler<EventArgs> EmailFailed;

        /// <summary>
        ///     Occurs after an e-mail has been sent. The sender is the MailMessage object.
        /// </summary>
        public static event EventHandler<EventArgs> EmailSent;
        /// <summary>
        /// The on email sent.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        private static void OnEmailSent(MailMessage message)
        {
            if (EmailSent != null)
            {
                EmailSent(message, EventArgs.Empty);
            }
        }
        /// <summary>
        /// The on email failed.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        private static void OnEmailFailed(MailMessage message)
        {
            if (EmailFailed != null)
            {
                EmailFailed(message, EventArgs.Empty);
            }
        }
        /// <summary>
        /// Sends the mail message asynchronously in another thread.
        /// </summary>
        /// <param name="message">
        /// The message to send.
        /// </param>
        public static void SendMailMessageAsync(MailMessage message, string smtpServer , string smtpServerPort,
            string smtpUserName , string smtpPassword , string enableSsl)
        {
            // Before entering a BG thread, retrieve the current instance blog settings.
            ThreadPool.QueueUserWorkItem(delegate
            {
                SendMailMessage(message, smtpServer, smtpServerPort, smtpUserName, smtpPassword, enableSsl);
            });
        }

    }
}
