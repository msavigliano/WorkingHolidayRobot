using System;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;
using SendGrid;
using SendGrid.Helpers.Mail;
using WorkingHoliday.Console.Properties;

namespace RorkingHoliday.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            IWebDriver driver;
            var options = new ChromeOptions();
            
            driver = new ChromeDriver(options);
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(20);
            driver.Navigate().GoToUrl("https://onlineservices.immigration.govt.nz/?STATUS");


            var _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));

            _wait.Until(d => d.FindElement(By.Name("username")));
            _wait.Until(d => d.FindElement(By.Name("password")));


            IWebElement query = driver.FindElement(By.Name("username"));
            query.SendKeys(Settings.Default.ImmigrationUsername);            

            IWebElement query2 = driver.FindElement(By.Name("password"));
            query2.SendKeys(Settings.Default.ImmigrationPassword);

            query2.Submit();

            
            _wait.Until(d => d.FindElement(By.LinkText("Argentina/NZ Working Holiday Scheme")));
            IWebElement link = driver.FindElement(By.LinkText("Argentina/NZ Working Holiday Scheme"));
            link.Click();

            while (!DoPay(driver, _wait))
            {
                SendErrorEmail();
                System.Threading.Thread.Sleep(60 * 1000); // Sleep one minute.

                driver.ExecuteJavaScript("alert('Fallo')");                
            }

            SendSuccessEmail();

            //run your javascript and alert code
            driver.ExecuteJavaScript("alert('Listo para pagar!!!')");
            driver.SwitchTo().Alert();

            driver.Quit();
        }

        private static void SendErrorEmail()
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(Settings.Default.SuccessEmailFrom, "WORKING HOLIDAY ROBOT");
            var subject = "FALLO";
            //var to = new EmailAddress("elisaulloa1986@gmail.com", "Elisa");
            var to = new EmailAddress(Settings.Default.SuccessEmailTo);
            var plainTextContent = "FALLO";
            var htmlContent = plainTextContent;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            client.SendEmailAsync(msg);
        }

        private static void SendSuccessEmail()
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(Settings.Default.SuccessEmailFrom, "WORKING HOLIDAY ROBOT");
            var subject = "DAME BOLA! SE ABRIO UN CUPO DE LA WORKING HOLIDAY!!!!";
            //var to = new EmailAddress("elisaulloa1986@gmail.com", "Elisa");
            var to = new EmailAddress(Settings.Default.SuccessEmailTo);
            var plainTextContent = "Tenes un chrome abierto por ahi listo para pagar!!!!!";
            var htmlContent = plainTextContent;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            client.SendEmailAsync(msg);
        }

        private static bool DoPay(IWebDriver driver, IWait<IWebDriver> _wait)
        {
            _wait.Until(d => d.FindElement(By.LinkText("Pay")));
            IWebElement payLink = driver.FindElement(By.LinkText("Pay"));
            payLink.Click();

            _wait.Until(d => d.FindElement(By.ClassName("super-link")));
            var payGatewayLink = driver.FindElement(By.ClassName("super-link")).FindElement(By.TagName("a"));
            payGatewayLink.Click();

            _wait.Until(d => d.FindElement(By.XPath("//input[contains(@name, 'payerNameTextBox')]")));
            IWebElement payerNameInput = driver.FindElement(By.XPath("//input[contains(@name, 'payerNameTextBox')]"));
            payerNameInput.SendKeys(Settings.Default.PayerName);

            _wait.Until(d => d.FindElement(By.XPath("//input[contains(@name, 'okButton')]")));
            IWebElement okPayButton = driver.FindElement(By.XPath("//input[contains(@name, 'okButton')]"));
            okPayButton.Click();

            if (driver.FindElements(By.XPath("//span[contains(text(), 'Scheme unavailable')]")).Any())
            {
                IWebElement cancelButton = driver.FindElement(By.XPath("//input[contains(@name, 'cancelButton')]"));
                cancelButton.Click();

                return false;
            }

            return true;
        }
    }
}
