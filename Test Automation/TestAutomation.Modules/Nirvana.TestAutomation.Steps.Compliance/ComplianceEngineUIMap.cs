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

namespace Nirvana.TestAutomation.Steps.Compliance
{
    [UITestFixture]
    public partial class ComplianceEngineUIMap : UIMap
    {
        public ComplianceEngineUIMap()
        {
            InitializeComponent();
        }

        public ComplianceEngineUIMap(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        /// <summary>
        /// For Closing the ComplianceEngine window
        /// </summary>
        public void CloseComplianceEngine()
        {
            try
            {
                ComplianceEngine_UltraFormManager_Dock_Area_Top.Click(MouseButtons.Left);
                KeyboardUtilities.CloseWindow(ref  ComplianceEngine_UltraFormManager_Dock_Area_Top);
                Wait(500);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
       
        /// <summary>
        /// For minimizing ComplianceEngine windows
        /// </summary>
        public void MinimizeComplianceEngine()
        {
            try
            {
                ComplianceEngine_UltraFormManager_Dock_Area_Top.Click(MouseButtons.Left);
                KeyboardUtilities.MinimizeWindow(ref ComplianceEngine_UltraFormManager_Dock_Area_Top);
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
        /// Open ComplianceEngine UI
        /// </summary>
        public void OpenComplianceEngine()
        {
            try
            {
                //  Shortcut to open ComplianceEngine module(CTRL + SHIFT + C)
                Keyboard.SendKeys(ConfigurationManager.AppSettings["OPEN_COM_ENGINE"]);
                Wait(15000);

                if (!ComplianceEngine.IsVisible)
                {
                    ExtentionMethods.WaitForVisible(ref ComplianceEngine, 10);
                }
                ComplianceEngine_UltraFormManager_Dock_Area_Top.Click(MouseButtons.Left);

                //KeyboardUtilities.MaximizeWindow(ref );
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }
        public void FileReplace(string DefaultInterceptorFile, string TempInterceptorFile, string NewInterceptorFile)
        {
            File.Copy(NewInterceptorFile, DefaultInterceptorFile,true);

            
        }
        public void CreateTempFileAndCopyFromOriginal(string DefaultInterceptorFile, string TempInterceptorFile, string NewInterceptorFile)
        {
           /* if (!File.Exists(TempInterceptorFile))
            {
                File.Create(TempInterceptorFile);//create a temporary file 
            }*/
            File.Copy(DefaultInterceptorFile, TempInterceptorFile,true);
        }
    }
}
