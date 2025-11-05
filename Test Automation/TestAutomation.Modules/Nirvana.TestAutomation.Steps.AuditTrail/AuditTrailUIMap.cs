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


namespace Nirvana.TestAutomation.Steps.AuditTrail
{
    [UITestFixture]
    public partial class AuditTrailUIMap : UIMap
    {
        public AuditTrailUIMap()
        {
            InitializeComponent();
        }

        public AuditTrailUIMap(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        public void OpenAuditTrail()
        {
            try
            {
                if (!PranaMain.IsVisible)
                {
                    ExtentionMethods.WaitForVisible(ref PranaMain, 40);
                }
                //  Shortcut to open allocation module(CTRL + SHIFT + A)
                Keyboard.SendKeys(ConfigurationManager.AppSettings["OPEN_AUDIT_TRAIL"]);
                //Wait(15000);
                //GrdTradeAudit.WaitForVisible();
                //DataUtilities.waitForGridDataToGetVisible(GrdTradeAudit, 10, 0);    
                
                GrdTradeAudit.Click(MouseButtons.Left);
                KeyboardUtilities.MaximizeWindow(ref TradeAuditUI_UltraFormManager_Dock_Area_Top);
                //Keyboard.SendKeys(KeyboardConstants.ALT_SPACE_R);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }
        public DataTable ExportAuditTrialGrid()
        {
            BtGetData.Click(MouseButtons.Left);
            ExtentionMethods.WaitForEnabled(ref BtExport, 10);
            BtExport.Click(MouseButtons.Left);

            string path = ApplicationArguments.ApplicationStartUpPath + @"\TestAutomation.Steps" + TestDataConstants.CAP_AUTOMATION_FOLDER + @"\";
            string directory = (ApplicationArguments.ApplicationStartUpPath + @"\TestAutomation.Steps" + TestDataConstants.CAP_AUTOMATION_FOLDER);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            //Clipboard.SetText(path + ExcelStructureConstants.AUDITTRAILGRIDDATA);
            
            //KeyboardUtilities.PressKey(2, KeyboardConstants.BACKSPACEKEY);
            TextBoxFilename.Click(MouseButtons.Left);
            KeyboardUtilities.PressKey(2, KeyboardConstants.BACKSPACEKEY);
            Keyboard.SendKeys(path + ExcelStructureConstants.AUDITTRAILGRIDDATA);

            //KeyboardUtilities.PressKey(2, KeyboardConstants.BACKSPACEKEY);
            //Keyboard.SendKeys("[CTRL+V]");

            ButtonSave.Click(MouseButtons.Left);
            if (ExportAuditTrailtoanExcelFile.IsVisible)
            {
                if (ButtonYes.IsVisible)
                {
                    ButtonYes.Click(MouseButtons.Left);
                }
            }
            Wait(3000);
            // Load Export data into datatable
            ITestDataProvider provider = Factory.TestDataProvider.GetProvider(ProviderType.Xls);
            DataSet testCases = provider.GetTestData(path + @"\" + ExcelStructureConstants.AUDITTRAILGRIDDATA, 1, 1);
            DataTable dtExportedData = testCases.Tables[0];
            return dtExportedData;

        }
        protected void CloseAuditTrail()
        {
            try
            {
                // Wait(3000);
                TradeAuditUI_UltraFormManager_Dock_Area_Top.BringToFront();
                TradeAuditUI_UltraFormManager_Dock_Area_Top.Click(MouseButtons.Left);
                KeyboardUtilities.CloseWindow(ref TradeAuditUI_UltraFormManager_Dock_Area_Top);
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
