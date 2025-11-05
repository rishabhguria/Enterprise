using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.Factory;
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
using System.IO;
using System.Configuration;

namespace Nirvana.TestAutomation.Steps.Recon
{
    [UITestFixture]
    public partial class ReconUIMap : UIMap
    {
        private string _fromDate = string.Empty;
        public string FromDate
        {
            get { return _fromDate; }
            set { _fromDate = value; }
        }

        private string _toDate = string.Empty;
        public string ToDate
        {
            get { return _toDate; }
            set { _toDate = value; }
        }

        public ReconUIMap()
        {
            InitializeComponent();
        }

        public ReconUIMap(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        /// <summary>
        /// Opens the Reconciliation (Recon) uner tools menu.
        /// </summary>
        protected void OpenRecon()
        {
            try
            {
                //PranaMain.WaitForVisible();
                if (!PranaMain.IsVisible)
                {
                    ExtentionMethods.WaitForVisible(ref PranaMain, 40);
                }
                //Shortcut to open recon module(CTRL + ALT + E)
                Keyboard.SendKeys(ConfigurationManager.AppSettings["OPEN_RECON"]);
                Wait(5000);      
                //Tools.Click(MouseButtons.Left);
                //Reconciliation.Click(MouseButtons.Left);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Closes the Reconciliation (Recon) uner tools menu.
        /// </summary>
        protected void CloseRecon()
        {
            try
            {
                KeyboardUtilities.CloseWindow(ref DataCompareForm_UltraFormManager_Dock_Area_Top);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
 
        }

        /// <summary>
        /// Minimizes the Reconciliation (Recon) uner tools menu.
        /// </summary>
        protected void MinimizeRecon()
        {
            try
            {
                KeyboardUtilities.MinimizeWindow(ref DataCompareForm_UltraFormManager_Dock_Area_Top);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Maximizes the Reconciliation (Recon) uner tools menu.
        /// </summary>
        protected void MaximizeRecon()
        {
            try
            {
                KeyboardUtilities.MaximizeWindow(ref DataCompareForm_UltraFormManager_Dock_Area_Top);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
 
        }

        public DataTable ExceptionReportData(string exceptionFileName)
        {
           
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) +@"\"+"ReconReports"+ @"\" + VerifyExceptionReport.exceptionFilePath + @"\" + exceptionFileName;
            
            ITestDataProvider provider = TestDataProvider.GetProvider(ProviderType.Xls);
            DataSet exceptionDataSet = provider.GetTestData(path, 1, 1);
            DataTable dtExceptionReport = exceptionDataSet.Tables[0];
            return dtExceptionReport;
        }

        public DataTable ExportData(String uiName)
        {
            try
            {
                    ExportToExcel.Click(MouseButtons.Left);
                    Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                    Wait(2000);
                    if (uiName == "ApplicationMatchedData")
                    {
                    Keyboard.SendKeys(KeyboardConstants.RIGHT_ARROWKEY);
                    Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                    Wait(5000);
                    }
                    else if (uiName == "PBMatchedData")
                    {
                        Keyboard.SendKeys(KeyboardConstants.RIGHT_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                    }
                    Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                    if (uiName == "ApplicationUnMatchedData")
                    {
                        Keyboard.SendKeys(KeyboardConstants.RIGHT_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                    } 
                    else if (uiName == "PBUnMatchedData")
                    {
                        Keyboard.SendKeys(KeyboardConstants.RIGHT_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                    }
                    TextBoxFilename.Click(MouseButtons.Left);
                    string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\";
                    KeyboardUtilities.PressKey(2, KeyboardConstants.BACKSPACEKEY);
                   // Keyboard.SendKeys(path + ExcelStructureConstants.ApplicationDataOnRecon);
                    ButtonSave.Click(MouseButtons.Left);
                    if (ConfirmSaveAs1.IsVisible)
                    {
                        ButtonYes.Click(MouseButtons.Left);
                    }
                    ITestDataProvider provider = TestDataProvider.GetProvider(ProviderType.Xls);
                   //Test it at commit time DataSet applicationDataSet = provider.GetTestData(path + @"\" + ExcelStructureConstants.ApplicationDataOnRecon);
                    DataSet applicationDataSet = provider.GetTestData(path + @"\" + ExcelStructureConstants.AllocatedTradesExportFileName);

                    DataTable dtApplicationData = applicationDataSet.Tables[0];
                    return dtApplicationData;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
                return null;
            }

        }

    }
}
