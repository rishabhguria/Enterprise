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
using TestAutomationFX.Core;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;


namespace Nirvana.TestAutomation.Steps.Blotter
{
    public class SaveLayoutOrderGrid : BlotterUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                OpenBlotter();
                if (testData != null)
                {
                    foreach (DataRow dr in testData.Tables[0].Rows)
                    {
                        OrdersTab.Click(MouseButtons.Left);
                        InputEnterOrder(dr);
                        WorkingSubsTab.Click(MouseButtons.Left);
                        InputEnterWorkingSubs(dr);
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
        private void InputEnterOrder(DataRow dr)
        {
            try
            {
                List<string> columns = new List<string>();
                int itemIndex = 0;
                foreach (string item in dr.ItemArray)
                {
                    if(item != "")
                    if (item.Equals("true", StringComparison.InvariantCultureIgnoreCase) || item == "1")
                    {
                        columns.Add(dr.Table.Columns[itemIndex].ToString());
                    }
                    itemIndex++;
                }
                this.DgBlotter1.InvokeMethod("AddColumnsToGrid", columns);
                SaveAllLayout.Click(MouseButtons.Left);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void InputEnterWorkingSubs(DataRow dr)
        {
            try
            {
                List<string> columns = new List<string>();
                int itemIndex = 0;
                foreach (string item in dr.ItemArray)
                {
                    if (item != "")
                        if (item.Equals("true", StringComparison.InvariantCultureIgnoreCase) || item == "1")
                        {
                            columns.Add(dr.Table.Columns[itemIndex].ToString());
                        }
                    itemIndex++;
                }
                this.DgBlotter.InvokeMethod("AddColumnsToGrid", columns);
                SaveAllLayout.Click(MouseButtons.Left);
            }
            catch (Exception)
            {
                throw;
            }
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
