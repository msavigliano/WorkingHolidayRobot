using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace WorkingHoliday.Console
{
    partial class Program
    {
        public class StatusStep : Step
        {
            public override IStep NextStep => new ExistingApplicationsStep();
            public override IStep OnErrorStep => new RedirectToExistingApplicationsStep();
            public override void InnerExecute()
            {
                var wait = new WebDriverWait(DriverInstance.Driver, TimeSpan.FromSeconds(5));

                wait.Until(d => d.FindElement(By.LinkText("Argentina/NZ Working Holiday Scheme")));
                var link = DriverInstance.Driver.FindElement(By.LinkText("Argentina/NZ Working Holiday Scheme"));
                link.Click();
            }
        }
    }
}
