using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace WorkingHoliday.Console
{
    partial class Program
    {
        public class ProceedToSecurePaymentStep : Step
        {
            public override IStep NextStep => new InputPayerNameStep();
            public override IStep OnErrorStep => new ExistingApplicationsStep();

            public override void InnerExecute()
            {
                var wait = new WebDriverWait(DriverInstance.Driver, TimeSpan.FromSeconds(5));

                wait.Until(d => d.FindElement(By.ClassName("super-link")));
                var payGatewayLink = DriverInstance.Driver.FindElement(By.ClassName("super-link")).FindElement(By.TagName("a"));
                payGatewayLink.Click();
            }
        }
    }
}
