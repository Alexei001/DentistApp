using DentistApp.Data.Services;
using MailKit.Net.Smtp;
using MimeKit;
using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DentistApp.Data
{
    public class SimpleJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.JobDetail.JobDataMap;

            var clientTrigger = (ClientTrigger)dataMap.Get("client");

            for (int i = 0; i < clientTrigger.Clients.Count; i++)
            {
                var diffdate = clientTrigger.Clients[i].Available.Subtract(DateTime.Now).TotalHours;
                if (diffdate < 24.00)
                {
                    using (SmtpClient client = new SmtpClient())
                    {
                        MimeMessage message = new MimeMessage();
                        message.From.Add(new MailboxAddress("Dentist app", "testApp@asd.com"));
                        message.To.Add(new MailboxAddress(clientTrigger.Clients[i].Name, clientTrigger.Clients[i].Email));
                        message.Subject = $"Notification Email for ";
                        message.Body = new BodyBuilder()
                        {
                            HtmlBody =
                            $"<h4 style=\"color:green;\">Client Notification!! :</h4>" +
                            $"<p>Dear Client {clientTrigger.Clients[i].Name}, " +
                            $"you have a rezervation on: {clientTrigger.Clients[i].Available} </p>"
                        }.ToMessageBody();

                        client.Connect("smtp.gmail.com", 465, true);
                        client.Authenticate("test.serv.g@gmail.com", "SuperAdmin966@");
                        client.Send(message);

                        client.Disconnect(true);
                    }
                    var message1 = $"Message Send to: {clientTrigger.Clients[i].Name}";
                    Debug.WriteLine(message1);
                }
                else
                {
                    var message1 = $"Message not send to: {clientTrigger.Clients[i].Name}";
                    Debug.WriteLine(message1);
                }
            }




        }


    }
}




