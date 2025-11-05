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
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.Blotter
{
    public class AddTab : BlotterUIMap, ITestStep
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
                        if (testData.Tables[0].Columns.Contains("btnOK"))
                        {
                            InputBox1.Click();

                            if (TextBox11.IsEnabled)
                            {

                                TextBox11.Click(MouseButtons.Left);
                                TextBox11.Properties["Text"] = dr["Name"].ToString();

                            }
                            BtnOK1.Click(MouseButtons.Left);

                            if (Error.IsVisible)
                            {
                                ButtonOK6.Click(MouseButtons.Left);
                                KeyboardUtilities.CloseWindow(ref InputBox_UltraFormManager_Dock_Area_Top);
                            }
                        }
                        else
                        {
                            AddTab.WaitForVisible();
                            if (AddTab.IsValid)
                                AddTab.Click(MouseButtons.Left);
                            if (TextBox1.IsEnabled)
                            {
                                if (TextBox1.IsValid)
                                {
                                    TextBox1.Click(MouseButtons.Left);
                                    TextBox1.Properties["Text"] = dr[TestDataConstants.TAB_NAME].ToString();
                                }
                            }
                            BtnOK.Click(MouseButtons.Left);
                        }
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
            finally
            {
                MinimizeBlotter();
            }
            return _res;
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
