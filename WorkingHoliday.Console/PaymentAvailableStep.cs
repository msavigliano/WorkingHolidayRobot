using OpenQA.Selenium.Support.Extensions;
using SendGrid.Helpers.Mail;
using WorkingHoliday.Console.Properties;

namespace WorkingHoliday.Console
{
    partial class Program
    {
        public class PaymentAvailableStep : Step
        {
            public override IStep NextStep => null;
            public override IStep OnErrorStep => null;

            public override void InnerExecute()
            {
                //run your javascript and alert code
                DriverInstance.Driver.ExecuteJavaScript("alert('Listo para pagar!!!')");
                DriverInstance.Driver.SwitchTo().Alert();

                SendSuccessEmail();
            }

            private static async void SendSuccessEmail()
            {
                var from = new EmailAddress(Settings.Default.SuccessEmailFrom, "WORKING HOLIDAY ROBOT");
                var subject = "DAME BOLA! SE ABRIO UN CUPO DE LA WORKING HOLIDAY!!!!";
                var to = new EmailAddress(Settings.Default.SuccessEmailTo);
                var plainTextContent = "Tenes un chrome abierto por ahi listo para pagar!!!!!";
                var htmlContent = plainTextContent;
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                await _emailClient.SendEmailAsync(msg);
            }
        }
    }
}
