using System;
using SendGrid;
using SendGrid.Helpers.Mail;
using WorkingHoliday.Console.Properties;

namespace WorkingHoliday.Console
{
    partial class Program
    {
        private static SendGridClient _emailClient = new SendGridClient(Environment.GetEnvironmentVariable("WORKINGHOLIDAYEMAILKEY", EnvironmentVariableTarget.Machine));

        static void Main(string[] args)
        {
            SendServiceStartedEmail();

            var initialStep = new RedirectToLoginStep();
            initialStep.Execute();
                                                    
            SendServiceStoppedEmail();

            System.Console.WriteLine("Press enter to finish the robot.");
            System.Console.ReadLine();
        }

        private static async void SendServiceStartedEmail()
        {
            var from = new EmailAddress(Settings.Default.SuccessEmailFrom, "WORKING HOLIDAY ROBOT");
            var subject = "WORKING HOLIDAY ROBOT STARTED!";
            var to = new EmailAddress(Settings.Default.SuccessEmailTo);
            var plainTextContent = $"Iniciado a las {DateTime.Now}";
            var htmlContent = plainTextContent;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            await _emailClient.SendEmailAsync(msg);

        }

        private static async void SendServiceStoppedEmail()
        {
            var from = new EmailAddress(Settings.Default.SuccessEmailFrom, "WORKING HOLIDAY ROBOT");
            var subject = "WORKING HOLIDAY ROBOT STOPPED!";
            var to = new EmailAddress(Settings.Default.SuccessEmailTo);
            var plainTextContent = $"Finalizado a las {DateTime.Now}";
            var htmlContent = plainTextContent;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            await _emailClient.SendEmailAsync(msg);

        }
    }
}
