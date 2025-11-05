using System;
using System.ComponentModel;
using System.Windows.Forms;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Configuration;
namespace Nirvana.TestAutomation.Steps.Closing
{
    [UITestFixture]
    public partial class ClosingPreferencesUIMap : UIMap
    {
        public ClosingPreferencesUIMap()
        {
            InitializeComponent();
        }

        public ClosingPreferencesUIMap(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        public void OpenClosingPreferences()
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
                ExtentionMethods.WaitForVisible(ref PreferencesMain, 15);
                //Tools.Click(MouseButtons.Left);
                //Preferences.Click(MouseButtons.Left);
                ClosePositions.Click(MouseButtons.Left);
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
