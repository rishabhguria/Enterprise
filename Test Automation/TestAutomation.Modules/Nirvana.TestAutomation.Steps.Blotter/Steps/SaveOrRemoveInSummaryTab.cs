using System.Data;
using System.Linq;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.Blotter
{
    public class SaveOrRemoveInSummaryTab : BlotterUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();

            try
            {
                OpenBlotter();
                //Wait(6000);
                MaximizeBlotter();
                Summary.Click(MouseButtons.Left);
                DgBlotter7.Click(MouseButtons.Left);
             
                if (testData != null)
                {
                    DataRow dr = testData.Tables[0].Rows[0];
                    if (String.IsNullOrEmpty(dr[TestDataConstants.REMOVEFILTER].ToString()).Equals(false) && dr[TestDataConstants.REMOVEFILTER].ToString().ToUpper() == "YES")
                    {
                        DgBlotter7.Click(MouseButtons.Right);
                        RemoveFilter.Click(MouseButtons.Left);
                        Wait(3000);
                    }
                    if (String.IsNullOrEmpty(dr[TestDataConstants.SAVELAYOUT].ToString()).Equals(false) && dr[TestDataConstants.SAVELAYOUT].ToString().ToUpper() == "YES")
                    {
                        DgBlotter7.Click(MouseButtons.Right);
                        SaveLayout.Click(MouseButtons.Left);
                        Wait(3000);
                    }
                }
                OrdersTab.Click(MouseButtons.Left);
                Summary.Click(MouseButtons.Left);
                DgBlotter7.Click(MouseButtons.Left);
               
            }

            catch (Exception ex)
            {
                _result.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {

                CloseBlotter();
            }
            return _result;
        }



        /// <summary>
        /// Disposes resources
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
