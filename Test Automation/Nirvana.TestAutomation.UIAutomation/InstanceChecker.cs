using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Support.UI;
using System.IO;

namespace Nirvana.TestAutomation.UIAutomation
{
    public  static class InstanceChecker
    {
        public static bool TryAttachToRunningInstance(out WindowsDriver<WindowsElement> driver)
        {
            driver = null;

            if (IsApplicationRunning("Prana.exe"))
            {
                AppiumOptions appiumOptionsExisting = new AppiumOptions();
                appiumOptionsExisting.AddAdditionalCapability(MobileCapabilityType.App, "Prana.exe");

                driver = new WindowsDriver<WindowsElement>(new Uri("http://127.0.0.1:4723"), appiumOptionsExisting);
                return true;
            }

            return false;
        }

        static bool IsApplicationRunning(string processName)
        {
            Process[] processes = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(processName));
            return processes.Length > 0;
        }

    }
}
