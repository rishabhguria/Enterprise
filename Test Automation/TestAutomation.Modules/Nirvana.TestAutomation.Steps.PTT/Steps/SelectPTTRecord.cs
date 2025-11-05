using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Steps.PTT;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestAutomationFX.Core;
using TestAutomationFX.UI;

namespace Nirvana.TestAutomation.Steps.PTT
{
    class SelectPTTRecord : PTTUIMap, ITestStep
    {
        public TestResult RunTest(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                    OpenPTTool();
                    DataTable gridData = ExportPSTData();
                    CalculatedValue.Click(MouseButtons.Left);
                    KeyboardUtilities.PressKey(3, KeyboardConstants.HOMEKEY);
                    DataRow matchedRow = DataUtilities.GetMatchingDataRow(gridData, testData.Tables[0].Rows[0]);
                    int index = gridData.Rows.IndexOf(matchedRow);
                    KeyboardUtilities.PressDownKeyWithWait(index);
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
                PercentTradingTool.Click(MouseButtons.Left);
                PercentTradingTool.Click(MouseButtons.Right);
                Minimize.Click(MouseButtons.Left);
            }

            return _result;
        }


        /// <summary>
        /// Disposes resources
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            try
            {
                base.Dispose(true);
                GC.SuppressFinalize(this);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
