using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Utilities.Constants;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;


namespace Nirvana.TestAutomation.Steps.PTT
{
    [UITestFixture]
    public partial class PTTPreferencesUIMap : UIMap
    {
        public PTTPreferencesUIMap()
        {
            InitializeComponent();
        }

        public PTTPreferencesUIMap(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }

        protected void OpenPTTPreference()
        {
            try
            {
                //PranaMain.WaitForVisible();
                if (!PranaMain.IsVisible)
                {
                    ExtentionMethods.WaitForVisible(ref PranaMain, 60);
                }
                //Shortcut to open Preferences under Tools (CTRL + ALT + F)
                Keyboard.SendKeys(ConfigurationManager.AppSettings["OPEN_PREF"]);
                //Wait(5000);
                ExtentionMethods.WaitForVisible(ref PreferencesMain, 15);
                //Tools.Click(MouseButtons.Left);
                //Preferences.Click(MouseButtons.Left);
                //Wait(3000);
                PreferencesModules.Click(MouseButtons.Left);
                PercentTradingTool.Click(MouseButtons.Left);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        public void ClosePreferencePTT()
        {
            try
            {
                KeyboardUtilities.CloseWindow(ref PreferencesMain_UltraFormManager_Dock_Area_Top);    
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }

        }

    }
}
