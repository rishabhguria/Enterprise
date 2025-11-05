using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.UIAutomation;
using TestAutomationFX.Core;
using System.Configuration;
using Nirvana.TestAutomation.Utilities;

namespace Nirvana.TestAutomation.Steps.Unknown
{
    class OpenColumnChooser : IUIAutomationTestStep
    {
        public TestResult RunUIAutomationTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {

            TestResult _res = new TestResult();
            try
            {
                if (testData == null || testData.Tables[0].Rows.Count < 1)
                {
                    string error = "Invalid TestData for OpenColumnChooser";
                    throw new Exception(error);
                }
                GridDataProvider uiAutomationHelper = new GridDataProvider();
                ApplicationArguments.mapdictionary = DataUtilities.CreateDictionaryFromDataTable(ApplicationArguments.IUIAutomationMappingTables["ColumnChooserDialog"]);
                if(testData.Tables[0].Columns.Contains("GridAutomationId"))
                {
                    foreach (DataRow dr in testData.Tables[0].Rows)
                    {
                        if (!string.IsNullOrEmpty(dr["GridAutomationId"].ToString()))
                        {
                            uiAutomationHelper.OpenColumnChooser(dr["GridAutomationId"].ToString(), ApplicationArguments.mapdictionary["ModuleWindow"].AutomationUniqueValue);
                        }
                    }
                }

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
