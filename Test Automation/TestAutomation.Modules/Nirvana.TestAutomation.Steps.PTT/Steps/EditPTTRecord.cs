using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Factory;
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
    class EditPTTRecord : PTTUIMap, ITestStep 
    {
        public TestResult RunTest(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenPTTool();
                int index = GetColumnIndex();
                KeyboardUtilities.PressKey(index, KeyboardConstants.TABKEY);
                DataRow dtRow = testData.Tables[0].Rows[0];
                foreach  (DataColumn dtColumn in testData.Tables[0].Columns)
                {
                    Keyboard.SendKeys(dtRow[dtColumn.ColumnName].ToString());
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                }
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
        /// This method retreives the column index of desired column from which we want to start editing
        /// </summary>
        /// <returns></returns>
        private int GetColumnIndex()
        {
            int index=-1;
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\";
                ITestDataProvider provider = TestDataProvider.GetProvider(ProviderType.OpenXml);
                DataSet pstDataSet = provider.GetTestData(path + @"\" + ExcelStructureConstants.PSTExportName);
                DataTable dtCalculatedData = pstDataSet.Tables[TestDataConstants.COL_CALCULATEDVALUES];
                index = dtCalculatedData.Columns.IndexOf("% Change");
           }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return index;
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
            catch(Exception)
            {
                    throw;
            }
        }

    }
}
