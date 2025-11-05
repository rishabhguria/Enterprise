using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.UIAutomation;
using TestAutomationFX.Core;
using System.Configuration;
namespace Nirvana.TestAutomation.Steps.MultiTradingTicket
{
    class SelectAllMTT : IUIAutomationTestStep
    {
        public TestResult RunUIAutomationTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {

            TestResult _res = new TestResult();
            try
            {
                UIAutomationHelper uiAutomationHelper = new UIAutomationHelper();
                if (testData.Tables[0] != null && testData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in testData.Tables[0].Rows)
                    {
                        if (!string.IsNullOrEmpty(row["SelectAllMTT"].ToString()))
                        {
                            UIAutomationHelper.DetectAndSwitchWindow("MultiTradingTicket");

                            ApplicationArguments.mapdictionary = DataUtilities.CreateDictionaryFromDataTable(ApplicationArguments.IUIAutomationMappingTables["MultiTradingTicket"]);

                            uiAutomationHelper.FindAndClickElementPartiallyLeft(ApplicationArguments.mapdictionary["SelectAllMTT"].AutomationUniqueValue);
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