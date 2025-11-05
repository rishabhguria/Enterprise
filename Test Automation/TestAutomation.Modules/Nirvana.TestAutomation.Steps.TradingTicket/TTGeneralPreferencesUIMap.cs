using System;
using System.ComponentModel;
using System.Windows.Forms;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Configuration;


namespace Nirvana.TestAutomation.Steps.TradingTicket
{
    [UITestFixture]
    public partial class TTGeneralPreferencesUIMap : UIMap
    {
        public TTGeneralPreferencesUIMap()
        {
            InitializeComponent();
        }

        public TTGeneralPreferencesUIMap(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        public void OpenGeneralPreferences()
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
                //Wait(5000);
                ExtentionMethods.WaitForVisible(ref PreferencesMain, 10);
                if (PreferencesMain.IsVisible)
                {
                    //Tools.Click(MouseButtons.Left);
                    //Preferences.Click(MouseButtons.Left);
                    Trading.Click(MouseButtons.Left);
                    GeneralPreferences.Click(MouseButtons.Left);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }
        public void OpenTTCompliancePreference()
        {
            try
            {
                //PranaMain.WaitForVisible();
                if (!PranaMain.IsVisible)
                {
                    ExtentionMethods.WaitForVisible(ref PranaMain, 40);
                }
                Keyboard.SendKeys(ConfigurationManager.AppSettings["OPEN_PREF"]);
                ExtentionMethods.WaitForVisible(ref PreferencesMain, 10);
                if (PreferencesMain.IsVisible)
                {
                    Trading.Click(MouseButtons.Left);
                    Compliance.Click(MouseButtons.Left);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
 
            }
        }

    }
}
