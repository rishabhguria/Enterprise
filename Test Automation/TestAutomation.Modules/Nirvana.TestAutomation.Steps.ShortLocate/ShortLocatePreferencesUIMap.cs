using System;
using System.ComponentModel;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Windows.Forms;
using System.Configuration;

namespace Nirvana.TestAutomation.Steps.ShortLocate
{
    [UITestFixture]
    public partial class ShortLocatePreferencesUIMap : UIMap
    {
        public ShortLocatePreferencesUIMap()
        {
            InitializeComponent();
        }

        public ShortLocatePreferencesUIMap(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        [UITest()]
        public void OpenShortLocatePreferences()
        {
            try
            {
                //PranaMain.WaitForVisible();
                if (!PranaMain.IsVisible)
                {
                    ExtentionMethods.WaitForVisible(ref PranaMain, 40);
                }
                //Shortcut to open Preferences under Tools (CTRL + ALT + F)
                Keyboard.SendKeys(ConfigurationManager.AppSettings["OPEN_PREF"]);
                Wait(5000);
                //Tools.Click();
                //Preferences.Click();
                ShortLocate.Click();
            }
            catch (Exception) { throw; }
        }
        public void MinimizeShortLocatePreferences()
        {
            try
            {
                KeyboardUtilities.MinimizeWindow(ref PreferencesMain);
                Wait(100);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
        }
        /// <summary>
        /// Clears the text.
        /// </summary>
        /// <param name="cmb">The CMB.</param>
        public void ClearText(UIWindow cmb)
        {
            try
            {
                while (cmb.Text.Length > 0)
                {
                    cmb.Click(MouseButtons.Left);
                    Keyboard.SendKeys("[END][SHIFT+HOME][BACKSPACE]");

                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
