using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Nirvana.TestAutomation.BussinessObjects;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using System.IO;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.Steps.PTT;
using Nirvana.TestAutomation.Interfaces.Enums;
using System.Configuration;
using System.Reflection;
using System.Globalization;
using System.ComponentModel;
using System.Diagnostics;
using Nirvana.TestAutomation.Factory;
using Nirvana.TestAutomation.Utilities;

namespace Nirvana.TestAutomation.Steps.PTT
{
    public class VerifyPTTCompliance: PTTUIMap, ITestStep 
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenPTT();
               // Wait(2000);
                CheckCompliance.Click(MouseButtons.Left);
                //Wait(4000);
                List<String> errors = VerifyCompliance(testData.Tables[0]);
                if (errors.Count > 0)
                    _result.ErrorMessage = String.Join("\n\r", errors);
                else
                    ResponseButton.Click(MouseButtons.Left);
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "VerifyPTTCompliance");
                _result.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }

            return _result;
        }
        public List<String> VerifyCompliance(DataTable dTable)
        {
            try
            {
                Wait(2000);
                ComplianceAlertPopUp.BringToFront();
                AlertPopupGridCompliance.Click();
                DataTable dtCompliancePopUp = DataUtilities.RemoveTrailingZeroes(CSVHelper.CSVAsDataTable(this.AlertPopupGridCompliance.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString()));
                List<String> columns = new List<string>();
                columns.Add("Rule Name");
                DataTable Verifydata = new DataTable();
                Verifydata = dTable.Copy();
                Verifydata = DataUtilities.RemoveColumnsAndRows("Description of Rule", Verifydata);
                List<String> errors = Recon.RunRecon(dtCompliancePopUp, Verifydata, columns, 0.01);
                return errors;
            }
            catch (Exception) { throw; }
        }
    }
}
