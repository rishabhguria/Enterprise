using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nirvana.TestAutomation.TestDataProvider;
using Nirvana.TestAutomation.Interfaces;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Support.UI;
using Nirvana.TestAutomation.Utilities;
using OfficeOpenXml;
using System.IO;

namespace Nirvana.TestAutomation.Factory
{
    public class AutomationProviderFactory
    {
        private static AutomationProviderFactory _instance;
        private static readonly object _lock = new object();
        //i will implement singleton logic if approved

        public IAutomationProvider CreateAutomationProvider(string providerKey)
        {
            switch (providerKey)
            {
                case "TAFX":
                    return new TafxProvider();
                case "WinAppDriver":
                    return new WinAppDriverProvider();
                default:
                    throw new ArgumentException("Invalid provider key.");
            }
        }
    }

}
