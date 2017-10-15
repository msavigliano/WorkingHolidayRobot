using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using WorkingHoliday.Console.Properties;

namespace WorkingHoliday.Console
{
    partial class Program
    {
        public class InputPayerNameStep : Step
        {
            public override IStep NextStep => new PaymentStep();
            public override IStep OnErrorStep => new ExistingApplicationsStep();
            public override void InnerExecute()
            {
                var wait = new WebDriverWait(DriverInstance.Driver, TimeSpan.FromSeconds(5));

                wait.Until(d => d.FindElement(By.XPath("//input[contains(@name, 'payerNameTextBox')]")));
                var payerNameInput = DriverInstance.Driver.FindElement(By.XPath("//input[contains(@name, 'payerNameTextBox')]"));
                payerNameInput.SendKeys(Settings.Default.PayerName);

                wait.Until(d => d.FindElement(By.XPath("//input[contains(@name, 'okButton')]")));
                var okPayButton = DriverInstance.Driver.FindElement(By.XPath("//input[contains(@name, 'okButton')]"));
                okPayButton.Click();
            }
        }
    }
}
