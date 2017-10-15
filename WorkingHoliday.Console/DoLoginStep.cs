using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using WorkingHoliday.Console.Properties;

namespace WorkingHoliday.Console
{
    partial class Program
    {
        public class DoLoginStep : Step
        {
            public override IStep NextStep => new StatusStep();
            public override IStep OnErrorStep => this;

            public override void InnerExecute()
            {
                var _wait = new WebDriverWait(DriverInstance.Driver, TimeSpan.FromSeconds(5));

                _wait.Until(d => d.FindElement(By.Name("username")));
                _wait.Until(d => d.FindElement(By.Name("password")));

                var query = DriverInstance.Driver.FindElement(By.Name("username"));
                query.Clear();
                query.SendKeys(Settings.Default.ImmigrationUsername);

                var query2 = DriverInstance.Driver.FindElement(By.Name("password"));
                query2.Clear();
                query2.SendKeys(Settings.Default.ImmigrationPassword);

                query2.Submit();
            }
        }
    }
}
