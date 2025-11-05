using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities.Constants;
using Nirvana.TestAutomation.BussinessObjects;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using System.Windows.Forms;

namespace Nirvana.TestAutomation.Steps.Blotter
{
    public class SaveOrRemoveInWorkingSubsTab : BlotterUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                OpenBlotter();
                WorkingSubsTab.Click(MouseButtons.Left);
                if (testData != null)
                {
                    DataRow dr = testData.Tables[0].Rows[0];
                    if (String.IsNullOrEmpty(dr[TestDataConstants.REMOVEFILTER].ToString()).Equals(false) && dr[TestDataConstants.REMOVEFILTER].ToString().ToUpper() == "YES")
                    {
                        DgBlotter1.Click(MouseButtons.Right);
                        RemoveFilter.Click(MouseButtons.Left);
                        Wait(3000);
                    }
                    if (String.IsNullOrEmpty(dr[TestDataConstants.SAVELAYOUT].ToString()).Equals(false) && dr[TestDataConstants.SAVELAYOUT].ToString().ToUpper() == "YES")
                    {
                        DgBlotter1.Click(MouseButtons.Right);
                        SaveLayout.Click(MouseButtons.Left);
                        Wait(3000);
                    }

                    OrdersTab.Click(MouseButtons.Left);
                    WorkingSubsTab.Click(MouseButtons.Left);
                    DgBlotter1.Click(MouseButtons.Left);

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
                OrdersTab.Click(MouseButtons.Left);
                CloseBlotter();
            }
            return _res;
        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
