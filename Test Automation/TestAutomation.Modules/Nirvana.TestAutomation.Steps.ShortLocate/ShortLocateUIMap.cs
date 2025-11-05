using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using System.IO;
using System.Reflection;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Configuration;

namespace Nirvana.TestAutomation.Steps.ShortLocate
{
    [UITestFixture]
    public partial class ShortLocateUIMap : UIMap
    {
        public ShortLocateUIMap()
        {
            InitializeComponent();
        }

        public ShortLocateUIMap(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
        public void OpenShortLocateUI()
        {
            try
            {
                //PranaMain.WaitForVisible();
                if (!PranaMain.IsVisible)
                {
                    ExtentionMethods.WaitForVisible(ref PranaMain, 40);
                }
                //Shortcut to open pricing input module(CTRL + ALT + L)
                Keyboard.SendKeys(ConfigurationManager.AppSettings["OPEN_SL"]);
                Wait(5000);
                //Trade.Click(MouseButtons.Left);
                //ShortLocate1.Click(MouseButtons.Left);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }
        public string modifyCSV()
        {
            string final = string.Empty;
            try
            {
                string csvit = GrdShortLocate1.InvokeMethod(ExcelStructureConstants.COL_GET_ALL_VISIBLE_DATA_FROM_THE_GRID, null).ToString();
                string[] splitstr = { "Source#~#\n" };
                string[] csvit1 = csvit.Split(splitstr, System.StringSplitOptions.RemoveEmptyEntries);
                string cs1 = csvit1[0];
                string cs2 = csvit1[1];
                cs1 = (cs1.Replace(" \n ", " \n"));
                cs1 = (cs1.Replace(" \n", "\n"));
                cs1 = (cs1.Replace("\n", " "))+ splitstr[0];
                final = cs1 + cs2;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return final;
        }
        public void MinimizeShortLocate()
        {
            try
            {
                KeyboardUtilities.MinimizeWindow(ref ShortLocate_UltraFormManager_Dock_Area_Top1);
               // Wait(100);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
        }

        private void ShortLocate_AttachFailing(object sender, AttachFailingEventArgs e)
        {
            try
            {
                if (e.CurrentRetryCount > 2)
                    ShortLocate.AttachFailing -= ShortLocate_AttachFailing;
                else
                {
                    uiWindow1.MatchedIndex = 1;
                    ShortLocate.MatchedIndex = 1;
                    //Wait(1000);
                    uiWindow1.MatchedIndex = 0;
                    ShortLocate.MatchedIndex = 0;
                   // Wait(1000);
                    e.Action = AttachFailingAction.Retry;
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
