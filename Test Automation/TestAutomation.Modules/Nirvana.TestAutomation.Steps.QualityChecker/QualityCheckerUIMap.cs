using System.Data;
using System.Linq;
using System.Windows.Forms;
using System;
using System.Collections.Generic;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using System.ComponentModel;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;


namespace Nirvana.TestAutomation.Steps.QualityChecker
{
    [UITestFixture]
    public partial class QualityCheckerUIMap : UIMap
    {
        public QualityCheckerUIMap()
        {
            InitializeComponent();
        }

        public QualityCheckerUIMap(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        /// <summary>
        /// Minimize the quality checker window
        /// </summary>
        /// <param name="disposing"></param>
        protected void MinimizeWindow()
        {
            try
            {
                ErrorDetectorTool_UltraFormManager_Dock_Area_Top.Click(MouseButtons.Right);
                Keyboard.SendKeys("[DOWN]");
                Keyboard.SendKeys("[DOWN]");
                Keyboard.SendKeys("[DOWN]");
                Keyboard.SendKeys("[DOWN]");
                Keyboard.SendKeys("[ENTER]");
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Restore window after minimizing
        /// </summary>
        /// <param name="disposing"></param>
        protected void RestoreWindow()
        {
            try
            {
                TitleBar.Click(MouseButtons.Left);
                Keyboard.SendKeys("[DOWN]");
                Keyboard.SendKeys("[ENTER]");
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Close the error window
        /// </summary>
        /// <param name="disposing"></param>
        protected void CloseErrorWindow()
        {
            try
            {
                ErrorViewer_UltraFormManager_Dock_Area_Top.Click(MouseButtons.Right);
                Keyboard.SendKeys("[UP]");
                Keyboard.SendKeys("[ENTER]");
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Close Quality Checker Window
        /// </summary>
        /// <param name="disposing"></param>
        protected void CloseQualityCheckerWindow()
        {
            try
            {
                ErrorDetectorTool_UltraFormManager_Dock_Area_Top.Click(MouseButtons.Right);
                Keyboard.SendKeys("[UP]");
                Keyboard.SendKeys("[ENTER]");
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
