using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MimeKit;
using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DentistApp.Data.Services
{
    public class EmailService
    {

        //email send with MailKit and Googleservices
        public void EmailConfirm(string confirmationLink, string userName, string userEmail)
        {
            using (SmtpClient client = new SmtpClient())
            {

                MimeMessage message = new MimeMessage();
                message.From.Add(new MailboxAddress("Dentist app", "testApp@asd.com"));
                message.To.Add(new MailboxAddress(userName, userEmail));
                message.Subject = $"Confirm Email for {userName}";
                message.Body = new BodyBuilder() { HtmlBody = $"<h5 style=\"color:black;\">Confirm User Registration,click the link:</h5><a href=\"{confirmationLink}\">Click here!</a>" }.ToMessageBody();

                client.Connect("smtp.gmail.com", 465, true);
                client.Authenticate("test.serv.g@gmail.com", "SuperAdmin966@");
                client.Send(message);

                client.Disconnect(true);
            }

        }


        public void EmailNotification(string userName, string userEmail, DateTime available)
        {
            using (SmtpClient client = new SmtpClient())
            {
                MimeMessage message = new MimeMessage();
                message.From.Add(new MailboxAddress("Dentist app", "testApp@asd.com"));
                message.To.Add(new MailboxAddress(userName, userEmail));
                message.Subject = $"Booking Notifycation for {userName}";
                message.Body = new BodyBuilder() { HtmlBody = $"<h6 style=\"color:black;\">Hello {userName} you are succesfull booked on : {available}</h6>" }.ToMessageBody();

                client.Connect("smtp.gmail.com", 465, true);
                client.Authenticate("test.serv.g@gmail.com", "SuperAdmin966@");
                client.Send(message);

                client.Disconnect(true);
            }
        }


    }
}
