using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium;
using System.Threading;
using System.Data;
using TestAutomationFX.Core;
using TestAutomationFX.Core.UIAutomationSupport;
using TestAutomationFX.UI;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace Nirvana.TestAutomation.Interfaces
{
    public interface IAutomationProvider
    {
        void LeftClick(string path = "",MsaaObject msaa = null);  //void LeftClick(string path ,UIAutomationElement obj =null );
        void RightClick(string path = "", MsaaObject msaa = null);
        void KeyboardInput(string path, string input);
        string ReadValue(string path);

        // DataTable fillDataTablewpfgrid(targetelement, moduleStepWiseGridStorrer);
    }


   
    public  class TafxProvider : IAutomationProvider
    {

        
        
        public void Click(MsaaObject msaa, MouseButtons button)
        {
            try
            {
                Rectangle bounds = msaa.Bounds;
                MouseController.MoveTo(bounds.Left + bounds.Width / 2, bounds.Top + bounds.Height / 2, TestAutomationFX.Core.UI.MousePath.Direct);
                MouseController.Click(button);
            }
            catch (Exception ex)
            {
                Log.Error("Clicking on button is failed :" + ex.Message);
                
            }
        }
        public void LeftClick(string path="",MsaaObject msaa=null)
        {
            try
            {
                Rectangle bounds = msaa.Bounds;
                MouseController.MoveTo(bounds.Left + bounds.Width / 2, bounds.Top + bounds.Height / 2, TestAutomationFX.Core.UI.MousePath.Direct);
                MouseController.Click();
            }
            catch (Exception ex)
            {
                Log.Error("Clicking on button is failed :" + ex.Message);

            }
        }
        public void RightClick(string path = "", MsaaObject msaa = null)
        {
            try
            {
                Rectangle bounds = msaa.Bounds;
                MouseController.MoveTo(bounds.Left + bounds.Width / 2, bounds.Top + bounds.Height / 2, TestAutomationFX.Core.UI.MousePath.Direct);
                MouseController.Click();
            }
            catch (Exception ex)
            {
                Log.Error("Clicking on button is failed :" + ex.Message);

            }
        }

        public void KeyboardInput(string path, string input)
        {

        }

        public string ReadValue(string path)
        {

            return null;
        }
    }

    public class WinAppDriverProvider : IAutomationProvider
    {
        private WindowsDriver<WindowsElement> driver;

        private WindowsDriver<WindowsElement> driver2;
        //needs modification
        private bool shouldCreateNewInstance = false;
        private string appPath = string.Empty;
        public WinAppDriverProvider(bool createNewInstance = false,string AppPath="" )
        {
            appPath = AppPath;
            shouldCreateNewInstance = createNewInstance;
            InitializeDriver();
        }

        private void InitializeDriver()
        {
            if (shouldCreateNewInstance || driver == null)
            {
                bool retry = true;
                while(retry==true)
                {
                try
                {
                    AppiumOptions appiumOptions = new AppiumOptions();
                    appiumOptions.AddAdditionalCapability("app", appPath);

                    driver = new WindowsDriver<WindowsElement>(new Uri("http://127.0.0.1:4723"), appiumOptions);
                    retry = false;

                }
                catch (Exception ex)
                {
                    QuitDriver();
                    Console.WriteLine(ex.Message);
                    continue;
                }
               }
                
                
            }
            else  if (shouldCreateNewInstance && driver !=null)
            {
                AppiumOptions appiumOptions = new AppiumOptions();
                appiumOptions.AddAdditionalCapability("app", "YourAppPath");

                driver2 = new WindowsDriver<WindowsElement>(new Uri("http://127.0.0.1:4723"), appiumOptions);
            }

        }
       /* public WinAppDriverProvider()
        {
            AppiumOptions appiumOptions = new AppiumOptions();
            appiumOptions.AddAdditionalCapability("app", "YourAppPath");

            driver = new WindowsDriver<WindowsElement>(new Uri("http://127.0.0.1:4723"), appiumOptions);
        }*/
        public bool ShouldCreateNewInstance
        {
            get { return shouldCreateNewInstance; }
            set { shouldCreateNewInstance = value; }
        }

        public WindowsDriver<WindowsElement> Driver
        {
            get { return driver; }
        }

        public void LeftClick(string path= "",MsaaObject msaa=null)
        {

            WindowsElement element = driver.FindElementByXPath(path);
            element.Click();
        }
        

        public void SwitchWindow(string windowName = "")
        {
            Thread.Sleep(6000);

            if (string.IsNullOrEmpty(windowName))
            {
                Console.WriteLine(driver.Title);

                var pranaMainWindowHandle = driver.CurrentWindowHandle;

                foreach (var handle in driver.WindowHandles)
                {
                    if (handle != pranaMainWindowHandle)
                    {
                        driver.SwitchTo().Window(handle);
                        break;
                    }
                }
            }
            else
            {
                Console.WriteLine(driver.Title);

              
                var windowHandles = driver.WindowHandles;

                foreach (var handle in windowHandles)
                {
                    driver.SwitchTo().Window(handle);

                 
                    if (driver.Title == windowName)
                    {
                        return; 
                    }
                }
            }
        }
        public void RightClick(string path = "", MsaaObject msaa = null)
        {
            WindowsElement element = driver.FindElementByXPath(path);
            OpenQA.Selenium.Interactions.Actions action = new OpenQA.Selenium.Interactions.Actions(driver);
            action.MoveToElement(element).ContextClick().Perform();
        }
        
         public void KeyboardInput(string path, string input)
        {
            try
            {
                Console.WriteLine(path);
                string trimmedpath = pathModifier(path);
                bool ischecked =trimmedpath.Equals("//Edit[@AutomationId=\'txtLoginID']");

               // driver.FindElementByXPath("//Edit[@AutomationId=\'txtLoginID']").SendKeys(input);
               // xpath = path.Replace("\"", "").Replace("\\", "\");
                //driver.FindElementByXPath(trimmedpath).SendKeys(input);
                WindowsElement element = driver.FindElementByXPath(trimmedpath);
                element.Click();
                element.SendKeys(input);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
               
                WindowsElement element = driver.FindElementByXPath(path);
                element.SendKeys(input);
            }
        }
         public List<WindowsElement> FindElementsByXPath(string xpath)
         {
             try
             {
                
                 IReadOnlyCollection<WindowsElement> elements = driver.FindElementsByXPath(xpath);
                 List<WindowsElement> elementsList = new List<WindowsElement>(elements);
                 return elementsList;
             }
             catch (Exception ex)
             {
                 Console.WriteLine(ex.Message+ "while getting elements by xPath");
                 throw; 
             }
         }

         public void SelectRow(int rowIndex, string xpathDataItems)
         {
             var elementFinder = driver.FindElementsByXPath(xpathDataItems);

             if (rowIndex >= 0 && rowIndex < elementFinder.Count)
             {
                 var rowElement = elementFinder[rowIndex];               
                 rowElement.Click();
             }
             else
             {
                 throw new ArgumentOutOfRangeException("rowIndex", "Row index is out of range.");
             }
         }

         public void FindElementbyIDAndTypeDataonPTT(DataTable pttDt, int selectedRowindex, Dictionary<string, int> columnToIndexMapping, DataRow dr, string xpathDataItems, string xpathEditor, Dictionary<string, List<int>> ColumnindexDictionary)
         {
             var elementFinder = driver.FindElementsByXPath(xpathDataItems);

             foreach (DataColumn col in pttDt.Columns)
             {
                 if (dr.Table.Columns.Contains(col.ColumnName))
                 {
                     if (!string.IsNullOrEmpty(dr[col.ColumnName].ToString()))
                     {
                         string columnName = col.ColumnName;

                         int rowindexer = 0;
                         if (pttDt.Columns.Contains("Round Lots"))
                             rowindexer = 1;

                         int elementIndex = ColumnindexDictionary[col.ColumnName][(selectedRowindex * 2) + rowindexer];
                         var element = elementFinder[elementIndex];

                         element.Click();
                         element.Click();

                         var childElement = element.FindElement(By.XPath(xpathEditor));
                         while (childElement.Text.Length > 0)
                         {
                             childElement.Click();
                             childElement.SendKeys(OpenQA.Selenium.Keys.Control + "a");
                             childElement.SendKeys(OpenQA.Selenium.Keys.Backspace);
                         }

                         childElement.SendKeys(dr[col.ColumnName].ToString());
                     }
                 }
             }
         }
        public void KeyboardInputWithVerification(string path, string input)
        {
            try
            {
                int maxRetries = 3;
                bool inputSentSuccessfully = false;
                for (int tryCount = 0; tryCount <= maxRetries; tryCount++)
                {
                    try
                    {
                        Console.WriteLine(path);

                        WindowsElement element = driver.FindElementByXPath(path);
                        element.Click();
                        element.SendKeys(input);
                        string actualValue = element.Text.ToString();
                        if (actualValue.Equals(input))
                        {
                            inputSentSuccessfully = true;
                            break;
                        }



                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Attempt"+tryCount +" :" +ex.Message);
 
                    }
                    Thread.Sleep(1000);
                }
                  

                //string trimmedpath = pathModifier(path);
                //bool ischecked =trimmedpath.Equals("//Edit[@AutomationId=\'txtLoginID']");

               // driver.FindElementByXPath("//Edit[@AutomationId=\'txtLoginID']").SendKeys(input);
               // xpath = path.Replace("\"", "").Replace("\\", "\");
                //driver.FindElementByXPath(trimmedpath).SendKeys(input);
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
               
                WindowsElement element = driver.FindElementByXPath(path);
                element.SendKeys(input);
            }
        }

        public string ReadValue(string path)
        {

            WindowsElement element = driver.FindElementByXPath(path);
            return element.Text;
        }

        public void MaximizeWindow()
        {
            driver.Manage().Window.Maximize();
        }

        public void MinimizeWindow()
        {
            driver.Manage().Window.Minimize();
        }

        public void CloseWindow()
        {
            driver.Close();
        }

        public void QuitDriver()
        {
            driver.Quit();
        }

        public string pathModifier(string path )
        {
            StringBuilder path2 = new StringBuilder();

         for (int i = 0; i < path.Length; i++)
         {
            if (i + 1 < path.Length && path[i] == '\\' && path[i + 1] == '\\')
            {
                path2.Append('\\');
                i++; // Skip the second backslash
            }
            else
            {
                path2.Append(path[i]);
            }
          }

         Console.WriteLine(path2.ToString());
         return path2.ToString();
        }
        public void RightClickAndSelectFromContextMenu(string elementXPath, string menuItemName ,string patternWithValue ,string controlType)
        {
            WindowsElement element = driver.FindElementByXPath(elementXPath);

            Actions action = new Actions(driver);
            action.ContextClick(element).Perform();
           // patternWithValue = "//*[@IsLegacyIAccessiblePatternAvailable='True']";
            WindowsElement contextMenu = null;
            var descendants = element.FindElementsByXPath(patternWithValue);
            foreach (var descendant in descendants)
            {
                //controlType = ("LocalizedControlType") == "menu");
                if (descendant.GetAttribute("LocalizedControlType") == "menu")
                {
                    contextMenu = (WindowsElement)descendant;
                    break;
                }
            }

            if (contextMenu != null)
            {
                var menuItem = contextMenu.FindElementByName(menuItemName);

                if (menuItem != null)
                {
                    menuItem.Click();
                }
                else
                {
                    throw new NoSuchElementException("Menu item '{menuItemName}' not found in the context menu.");
                }
            }
            else
            {
                throw new NoSuchElementException("Context menu not found.");
            }
        }



    }

    public static class MsaaObjectExtensions
    {
        public static void LeftClick(this MsaaObject msaa, MouseButtons button)
        {
            try
            {
                Rectangle bounds = msaa.Bounds;
                MouseController.MoveTo(bounds.Left + bounds.Width / 2, bounds.Top + bounds.Height / 2, TestAutomationFX.Core.UI.MousePath.Direct);
                MouseController.Click(button);
            }
            catch (Exception ex)
            {
                Log.Error("Clicking on button is failed: " + ex.Message);
            }
        }

        public static void LeftClick(this MsaaObject msaa)
        {
            try
            {
                Rectangle bounds = msaa.Bounds;
                MouseController.MoveTo(bounds.Left + bounds.Width / 2, bounds.Top + bounds.Height / 2, TestAutomationFX.Core.UI.MousePath.Direct);
                MouseController.Click();
            }
            catch (Exception ex)
            {
                Log.Error("Clicking on button is failed: " + ex.Message);
            }
        }
    }
}
