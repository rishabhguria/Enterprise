using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Factory;
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
using Nirvana.TestAutomation.UIAutomation;
using System.Configuration;

namespace Nirvana.TestAutomation.Steps.Rebalancer
{
    class VerifyTradeListSummary : RebalancerUIMap, IUIAutomationTestStep
    {
        public TestResult RunUIAutomationTest(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                try
                {
                    GridDataProvider gridDataProvider = new GridDataProvider();
                    ApplicationArguments.mapdictionary = DataUtilities.CreateDictionaryFromDataTable(ApplicationArguments.IUIAutomationMappingTables["TradeBuySellListSummaryView"]);
                    DataTable uiData = gridDataProvider.GetWPFGridData(ApplicationArguments.mapdictionary["TradeListSummaryModuleWindow"].AutomationUniqueValue, ApplicationArguments.mapdictionary["TradeListSummaryGridName"].AutomationUniqueValue);
                    
                    List<string> errors = RunReconOnTables(uiData, testData.Tables[0]);
                    if (errors.Count > 0)
                        _result.ErrorMessage = String.Join("\n\r", errors);
                }
                catch (Exception ex)
                {
                    _result.IsPassed = false;
                    bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                    if (rethrow)
                        throw;
                }



            }
            catch (Exception ex)
            {
                _result.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return _result;
        }
    }
}
