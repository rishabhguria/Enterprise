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
    class AddTabBlotter : BlotterUIMap, ITestStep
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
                        string tab = dr[TestDataConstants.BUTTON_NAME].ToString();
                        if (tab == "Add Tab(Working)")
                        {
                            AddTabWorking.Click();
                        }
                        if (tab == "Add Tab(Order)")
                        {
                            AddTabOrder.Click();
                        }

                       // InputBox2.WaitForVisible();

                             InputBox1.Click();
                        
                            if (TextBox11.IsEnabled)
                            {

                                    TextBox11.Click(MouseButtons.Left);
                                    TextBox11.Properties["Text"] = dr[TestDataConstants.TAB_NAME].ToString();
                                
                            }
                            BtnOK1.Click(MouseButtons.Left);

                            if (Error.IsVisible) {
                                ButtonOK6.Click(MouseButtons.Left);
                                KeyboardUtilities.CloseWindow(ref InputBox_UltraFormManager_Dock_Area_Top);
                            }
                    }
                }
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "AddTabBlotter");
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
