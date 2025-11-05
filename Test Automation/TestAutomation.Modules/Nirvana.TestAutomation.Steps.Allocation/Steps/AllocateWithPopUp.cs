using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestAutomationFX.Core;

namespace Nirvana.TestAutomation.Steps.Allocation
{
    class AllocateWithPopUp : AllocationUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                OpenAllocation();
                Keyboard.SendKeys(KeyboardConstants.ALT_SPACE_X);
                AllocateUnallocatePinTab.Click(MouseButtons.Left);
                btnAllocate.DoubleClick(MouseButtons.Left);
                foreach (DataRow dr in testData.Tables[0].Rows)
                {
                    if (NirvanaAllocation.IsVisible)
                    {
                        if (dr[TestDataConstants.COL_Recalculate_Comm_PopUp].ToString().Equals("Yes"))
                        {
                            ButtonYes.Click(MouseButtons.Left);
                        }
                        else
                        {
                            ButtonNo.Click(MouseButtons.Left);
                        }
                    }
                }
                if (SavewDivideStatus.Bounds.X >= 0 && SavewDivideStatus.Bounds.Y >= 0)
                {
                    SavewDivideStatus.Click(MouseButtons.Left);
                    SavewDivideStatus.Click(MouseButtons.Left);
                }
                if (SavewDivideoStatus.Bounds.X >= 0 && SavewDivideoStatus.Bounds.Y >= 0)
                {
                    SavewDivideoStatus.Click(MouseButtons.Left);
                    SavewDivideoStatus.Click(MouseButtons.Left);
                }
               // Wait(1000);
            }

            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "AllocateWithPopUp");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {
                MinimizeAllocation();
            }
            return _res;
        }
    }
}
