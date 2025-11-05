using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Interfaces;
using System.Collections.Generic;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.AuditTrail
{
    class VerifyAuditTrail : AuditTrailUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {

                List<String> errors = InputEnter(testData.Tables[0]);
                if (errors.Count > 0)
                    _res.ErrorMessage = String.Join("\n\r", errors);

            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "VerifyAuditTrail");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {
                CloseAuditTrail();
            }
            return _res;
        }
        private List<String> InputEnter(DataTable dTable)
        {
            try
            {
                DataTable dataTable = ExportAuditTrialGrid();
                List<string> errors = new List<string>();
                List<string> columns = new List<string>();
                try
                {
                    errors = Recon.RunRecon(dataTable, dTable, columns, 0.01);
                }
                catch (Exception ex)
                {
                    bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                    if (rethrow)
                        throw;
                }
                return errors;

            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
