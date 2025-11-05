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

namespace Nirvana.TestAutomation.Steps.Blotter
{
    class VerifyExpandedBR : BlotterExecutionReportUIMap, IUIAutomationTestStep
    {
        public TestResult RunUIAutomationTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {

            TestResult _res = new TestResult();
            try
            {
                ApplicationArguments.mapdictionary = DataUtilities.CreateDictionaryFromDataTable(ApplicationArguments.IUIAutomationMappingTables["CommonMappings"]);
                DataSet ds = UIAutomationHelper.ExpandGridElements();
                List<string> errors = new List<string>();
                List<string> columns = new List<string>();
                try
                {
                    DataUtilities.RemoveEmptyRows(ds);
                    errors = Recon.RunRecon(ds.Tables[0], testData.Tables[0], columns, 0.01);
                    if (errors.Count > 0)
                        _res.ErrorMessage = String.Join("\n\r", errors);
                }
                catch (Exception ex)
                {
                    bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                    if (rethrow)
                        throw;
                }
            }
            catch (Exception ex)
            {
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {
                MinimizeBlotterExecutionReport();
            }
            return _res;
        }
    }
}
