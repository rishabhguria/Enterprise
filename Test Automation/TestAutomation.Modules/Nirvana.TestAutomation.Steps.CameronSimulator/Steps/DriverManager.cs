using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Nirvana.TestAutomation.Steps.Simulator
{

    public static class DriverManager
    {
        private static IWebDriver _driver;
        private static WebDriverWait _wait;

        private static string ChromeDriverPath = @"C:\Users\shivam.tanwar\Downloads\chromedriver-win64 (2)\chromedriver-win64";
        private static string RemoteDebuggingPort = "9222";
        private static string DebuggerAddress = "localhost:9222";

        public static IWebDriver Driver
        {
            get { return _driver; }
        }

        public static WebDriverWait Wait
        {
            get { return _wait; }
        }

        public static void Initialize()
        {
            if (_driver != null)
                return;

            try
            {
                ChromeOptions attachOptions = new ChromeOptions();
                attachOptions.DebuggerAddress = DebuggerAddress;

                _driver = new ChromeDriver(ChromeDriverPath, attachOptions); // Try attach
            }
            catch (WebDriverException)
            {
                // Attach failed – start new instance with remote debugging
                ChromeOptions launchOptions = new ChromeOptions();
                launchOptions.AddArgument("--remote-debugging-port=" + RemoteDebuggingPort);

                ChromeDriverService service = ChromeDriverService.CreateDefaultService(ChromeDriverPath);
                _driver = new ChromeDriver(service, launchOptions);
            }

            _driver.Manage().Window.Size = new System.Drawing.Size(1920, 1080);

            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(35));
        }

        public static bool IsInitialized()
        {
            return _driver != null;
        }

        public static void Quit()
        {
            try
            {
                if (_driver != null)
                {
                    _driver.Quit();
                }
            }
            catch
            {
                // suppress
            }
            finally
            {
                _driver = null;
                _wait = null;
            }
        }
    }
}

