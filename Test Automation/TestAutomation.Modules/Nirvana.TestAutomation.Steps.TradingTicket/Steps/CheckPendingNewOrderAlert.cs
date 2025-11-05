using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml;
using System.Windows.Automation;


namespace Nirvana.TestAutomation.Steps.TradingTicket
{
    public class CheckPendingNewOrderAlert : TradingTicketUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                foreach (DataRow dr in testData.Tables[0].Rows)
                {
                    string pendingNewOrderAlertTime = dr["PendingNewOrderAlertTime"].ToString();
                    int waitTime = Int32.Parse(pendingNewOrderAlertTime)*(2000);
                    Wait(waitTime);
                }
                if (!ClickOkOnPendingNewAlert())
                {
                    throw new InvalidOperationException("Timeout Waiting for Pending Order Alert Pop up.");
                }
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "CheckPendingNewOrderAlert");
                _result.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return _result;
           
        }

        /// <summary>
        /// Clicks the ok on pending new alert.
        /// </summary>
        /// <returns></returns>
        private bool ClickOkOnPendingNewAlert()
        {
            try
            {
                AutomationElement aePrana = AutomationElement.RootElement.FindFirst(TreeScope.Children,
                        new PropertyCondition(AutomationElement.NameProperty, "Nirvana"));

                AutomationElement aeAlertBox = aePrana.FindFirst(TreeScope.Children,
                  new PropertyCondition(AutomationElement.AutomationIdProperty, "CustomMessageBox"));

                AutomationElement aeOKButton = null;
                aeOKButton = aeAlertBox.FindFirst(TreeScope.Descendants,
                  new PropertyCondition(AutomationElement.AutomationIdProperty, "ultraOkButton"));
                var invokePattern = aeOKButton.GetCurrentPattern(InvokePattern.Pattern) as InvokePattern;
                invokePattern.Invoke();
                return true;
            }
            catch (Exception ex)
            {
                try
                {
                    if (CustomMessageBox.IsVisible)
                    {
                        UltraOkButton.Click(MouseButtons.Left);
                        return true;
                    }
                    else if (CustomMessageBox1.IsVisible)
                    {
                        UltraOkButton1.Click(MouseButtons.Left);
                        return true;
                    }
                }
                catch (Exception e)
                {
                    bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                    if (rethrow)
                        throw;
                }
            }
            return false;
        }
    }
}
