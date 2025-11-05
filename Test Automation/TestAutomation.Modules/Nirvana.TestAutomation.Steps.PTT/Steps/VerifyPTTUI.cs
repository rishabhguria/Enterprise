using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using System.Configuration;
using Nirvana.TestAutomation.Factory;
using Nirvana.TestAutomation.UIAutomation;

namespace Nirvana.TestAutomation.Steps.PTT
{
    class VerifyPTTUI : IUIAutomationTestStep
    {
        public TestResult RunUIAutomationTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {

            TestResult _res = new TestResult();
            try
            {
                if (testData.Tables[0] == null || testData.Tables[0].Rows.Count <= 0)
                {
                    throw new Exception("VerifyPTTUI failed as DataSet is empty.");
                }
                DataTable excelData = testData.Tables[0].Copy();
                ApplicationArguments.mapdictionary = DataUtilities.CreateDictionaryFromDataTable(ApplicationArguments.IUIAutomationMappingTables["Window"]);
                UIAutomationHelper uiAutomationHelper = new UIAutomationHelper();

                UIAutomationHelper.DetectAndSwitchWindow(ApplicationArguments.mapdictionary["ModuleWindow"].AutomationUniqueValue);
                Keyboard.SendKeys(ConfigurationManager.AppSettings["OPEN_PTT"]);
                UIAutomationHelper.DetectAndSwitchWindow(ApplicationArguments.mapdictionary["ModuleWindow"].AutomationUniqueValue);
               
                GridDataProvider gridDataProvider = new GridDataProvider();
                DataTable dtable = gridDataProvider.GetWPFGridData(ApplicationArguments.mapdictionary["ModuleWindow"].AutomationUniqueValue);
                //need to edit ptt grid here for future tasks
                dtable = DataUtilities.RemoveCommas(dtable);
                dtable = DataUtilities.RemovePercent(dtable);
                dtable = DataUtilities.RemoveTrailingZeroes(dtable);

                excelData = DataUtilities.RemoveCommas(dtable);
                excelData = DataUtilities.RemovePercent(dtable);
                excelData = DataUtilities.RemoveTrailingZeroes(dtable);

                List<string> errors = new List<string>();
                List<string> columns = new List<string>();

                errors = Recon.RunRecon(dtable, excelData, columns, 0.01);
                if (errors.Count > 0)
                    _res.ErrorMessage = String.Join("\n\r", errors);

            }
            catch (Exception ex)
            {
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return _res;
        }
    }
}


