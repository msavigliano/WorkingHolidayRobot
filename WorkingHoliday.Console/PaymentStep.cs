using System;
using System.Linq;
using OpenQA.Selenium;

namespace WorkingHoliday.Console
{
    partial class Program
    {
        public class PaymentStep : Step
        {
            public override IStep NextStep => new ExistingApplicationsStep();
            public override IStep OnErrorStep => new ExistingApplicationsStep();

            public override void InnerExecute()
            {
                if (DriverInstance.Driver.FindElements(By.XPath("//span[contains(text(), 'Scheme unavailable')]")).Any())
                {
                    IWebElement cancelButton = DriverInstance.Driver.FindElement(By.XPath("//input[contains(@name, 'cancelButton')]"));
                    cancelButton.Click();

                    System.Threading.Thread.Sleep(30 * 1000); // Sleep one minute.
                    throw new InvalidOperationException();
                }
            }
        }
    }
}
