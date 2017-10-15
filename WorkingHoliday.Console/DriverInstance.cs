using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace WorkingHoliday.Console
{
    partial class Program
    {
        public class DriverInstance
        {
            private static IWebDriver _driver;

            public static IWebDriver Driver {
                get
                {
                    if (_driver == null)
                        InitializeDriver();

                    return _driver;
                }
            }

            private static void InitializeDriver()
            {
                var options = new ChromeOptions();

                _driver = new ChromeDriver(options);
                _driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(20);
            }

            protected DriverInstance()
            {
            }
        }
    }
}
