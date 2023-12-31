using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace TicketLand_project.Common
{
    public class MailHelper
    {
        public void SendMail(string MailTo, string title, string content)
        {

            var fromAddress = new MailAddress("alice02052003@gmail.com", "Ticket Land");
            var toAddress = new MailAddress(MailTo.Trim());
            string subject = title.Trim();
            string body = content.Trim();

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(fromAddress.Address, "nmrpswppafufevzu"),
                Timeout = 20000
            };
            var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                IsBodyHtml = true,
                Body = body
            };
            smtp.Send(message);
            // Thong bao
        }
    }
}