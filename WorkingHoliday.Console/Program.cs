﻿using System;
using System.Linq;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;
using SendGrid;
using SendGrid.Helpers.Mail;
using WorkingHoliday.Console.Properties;

namespace WorkingHoliday.Console
{
    class Program
    {
        private static SendGridClient _emailClient = new SendGridClient(Environment.GetEnvironmentVariable("WORKINGHOLIDAYEMAILKEY", EnvironmentVariableTarget.Machine));

        static void Main(string[] args)
        {
            SendServiceStartedEmail();

            IWebDriver driver;
            var options = new ChromeOptions();

            driver = new ChromeDriver(options);
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(20);
            driver.Navigate().GoToUrl("https://onlineservices.immigration.govt.nz/?STATUS");

            var _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));

            _wait.Until(d => d.FindElement(By.Name("username")));
            _wait.Until(d => d.FindElement(By.Name("password")));


            IWebElement query = driver.FindElement(By.Name("username"));
            query.Clear();
            query.SendKeys(Settings.Default.ImmigrationUsername);

            IWebElement query2 = driver.FindElement(By.Name("password"));
            query2.Clear();
            query2.SendKeys(Settings.Default.ImmigrationPassword);

            query2.Submit();

            _wait.Until(d => d.FindElement(By.LinkText("Argentina/NZ Working Holiday Scheme")));
            IWebElement link = driver.FindElement(By.LinkText("Argentina/NZ Working Holiday Scheme"));
            link.Click();

            while (!DoPay(driver, _wait))
            {
                System.Threading.Thread.Sleep(60 * 1000); // Sleep one minute.
            }

            SendSuccessEmail();

            //run your javascript and alert code
            driver.ExecuteJavaScript("alert('Listo para pagar!!!')");
            driver.SwitchTo().Alert();

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
