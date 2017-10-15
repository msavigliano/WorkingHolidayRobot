using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace WorkingHoliday.Console
{
    partial class Program
    {
        public class ExistingApplicationsStep : Step
        {
            public override IStep NextStep => new ProceedToSecurePaymentStep();
            public override IStep OnErrorStep => this;
            public override void InnerExecute()
            {
                var wait = new WebDriverWait(DriverInstance.Driver, TimeSpan.FromSeconds(5));

                wait.Until(d => d.FindElement(By.LinkText("Pay")));
                var link = DriverInstance.Driver.FindElement(By.LinkText("Pay"));
                link.Click();
            }
        }
    }
}
