using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace Nirvana.TestAutomation.Utilities
{
    public static class SwitchWindow
    {

        public static bool SwitchToWindow(WebDriver driver, string windowname, bool isExact = false, string xpath = "")
        {
            int maxTry = 3;

            for (int tryCount = 1; tryCount <= maxTry; tryCount++)
            {
                try
                {

                    if (windowname == "Login")
                    {
                        Stopwatch LoginWatch = new Stopwatch();
                        LoginWatch.Start();
                        while (LoginWatch.ElapsedMilliseconds <= 60000)
                        {
                            if (SwitchWindowMethod(driver, windowname, isExact))
                                return true;
                        }
                        if (!SwitchWindowMethod(driver, "Dock", isExact))
                            return true;
                        SamsaraHelperClass.CaptureMyScreen("LoginIssue", ApplicationArguments.TestCaseToBeRun);
                        return false;
                    }
                    else
                    {
                        Stopwatch otherWatch = new Stopwatch();
                        otherWatch.Start();
                        while (otherWatch.ElapsedMilliseconds <= 15000)
                        {
                            if (SwitchWindowMethod(driver, windowname, isExact, xpath))
                                return true;
                        }
                        SamsaraHelperClass.CaptureMyScreen("WindowNotSwitched", ApplicationArguments.TestCaseToBeRun);
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    if (tryCount == maxTry)
                    {
                        SamsaraHelperClass.CaptureMyScreen("Exception_" + ex.GetType().Name, ApplicationArguments.TestCaseToBeRun, "Window not found " + windowname);
                        return false;
                    }
                }
            }

            SamsaraHelperClass.CaptureMyScreen("WindowNotSwitched", ApplicationArguments.TestCaseToBeRun, "Window not found " + windowname);
            return false;
        }

        private static bool SwitchWindowMethod(WebDriver driver, string windowname, bool isExact, string xpath = "")
        {
            if (xpath == "")
            {
                xpath = SamsaraHelperClass.SamsaraXpath("CloseModule", "Blotter");
            }
            bool flag = true;
            string str = null;
            for (int i = 0; i < driver.WindowHandles.Count; i++)
            {

                try
                {
                    driver.SwitchTo().Window(driver.WindowHandles[i]);
                    //Console.WriteLine(driver.Title);
                    string a = driver.Title;
                    if (isExact)
                    {
                        if (driver.Title.ToString().ToLower() == windowname.ToLower())
                        {
                            Console.WriteLine("Window Switched to " + driver.Title);
                            Thread.Sleep(1000);
                            return true;
                        }
                    }
                    else
                    {
                        if (driver.Title.Contains(windowname))
                        {
                            try
                            {
                                Actions actions = new Actions(driver);
                                IWebElement element = driver.FindElement(By.XPath(xpath));
                                str = driver.WindowHandles[i];
                                continue;
                            }
                            catch (Exception)
                            {
                                flag = false;
                            }

                            if (!flag)
                            {
                                Thread.Sleep(1000);
                                Console.WriteLine("Window Switched to " + driver.Title);
                                return true;
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    // some windows may get closed during Runtime startup
                    // so may get this exception depending on timing
                    Console.WriteLine("Ignoring NoSuchWindowException " + driver.WindowHandles[i] + e);
                }
            }
            Task.Delay(1000);
            return false;

        }

        public static bool SwitchToChildWindow(WebDriver driver, string parentWindowName, string DashBoardXpath)
        {
         
        string originalWindow = driver.CurrentWindowHandle;
        IList<string> windowHandles = driver.WindowHandles;

        foreach (string handle in windowHandles)
        {
            driver.SwitchTo().Window(handle);
            if (driver.Title.Contains(parentWindowName))
            {
                Console.WriteLine("Switched to window with title:"+driver.Title);
                
                try
                {
                    var element = driver.FindElements(By.XPath(DashBoardXpath));
                    if (element.Count > 0)
                    {
                        Console.WriteLine("Element found in the window.");
                        return true;
                    }
                }
                catch (WebDriverTimeoutException)
                {
                    Console.WriteLine("Element not found in this window. Continuing search...");
                }
            }
        }
        driver.SwitchTo().Window(originalWindow);
        Console.WriteLine("Element not found in any window. Switched back to the original window.");
        return false;
        }


        public static bool SwitchToParentWindow(WebDriver driver, string parentWindowName, string Xpath)
        {
            bool flag = true;
            string str = null;
            int j = 0;
            for (int i = 0; i < driver.WindowHandles.Count; i++)
            {

                try
                {
                    driver.SwitchTo().Window(driver.WindowHandles[i]);

                    if (driver.Title.Contains(parentWindowName))
                    {
                        try
                        {
                            Actions actions = new Actions(driver);
                            IWebElement element = driver.FindElement(By.XPath(Xpath));
                            str = driver.WindowHandles[i];
                            flag = false;
                        }
                        catch (Exception)
                        {
                            j = i;
                            flag = true;
                            continue;
                        }

                        if (!flag)
                        {
                            Thread.Sleep(1000);
                            Console.WriteLine("Window Switched to " + driver.Title);
                            return true;
                        }
                    }
                    
                }
                catch (Exception e)
                {
                    // some windows may get closed during Runtime startup
                    // so may get this exception depending on timing
                    Console.WriteLine("Ignoring NoSuchWindowException " + driver.WindowHandles[i] + e);
                }
            }
            Task.Delay(1000);
            return false;

        
        }
    }
}