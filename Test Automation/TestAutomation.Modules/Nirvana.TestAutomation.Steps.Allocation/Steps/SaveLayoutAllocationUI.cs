using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
namespace Nirvana.TestAutomation.Steps.Allocation
{
    class SaveLayoutAllocationUI : AllocationUIMap, ITestStep
    {
        /// <summary>
        /// Begins the test execution
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        /// <returns></returns>
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                OpenAllocation();
                Button1.Click(MouseButtons.Left);
                foreach (DataRow dr in testData.Tables[0].Rows)
                {
                    SavePinLayout(dr["Tab Name"].ToString(), dr["IsPinned"].ToString());
                }
                GridUnallocated.Click();
                MouseController.RightClick();
                SaveLayout1.Click(MouseButtons.Left);
                if (NirvanaAllocation.IsVisible)
                    ButtonOK.Click(MouseButtons.Left);
               // Wait(1000);

            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "SaveLayoutAllocationUI");
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

        /// <summary>
        /// method to pin or unpin tab items
        /// </summary>
        /// <param name="tabName"></param>
        /// <param name="isPinned"></param>
        private void SavePinLayout(string tabName, string isPinned)
        {
            try
            {
                switch (tabName)
                {
                    case "Filters":
                        PinUnpinTab(Filters, UnpinBtn1, UnpinBtn2, isPinned);
                        break;
                    case "Swap Control":
                        PinUnpinTab(SwapControl, UnpinBtn3, UnpinBtn4, isPinned);
                        break;
                    case "Edit Trade":
                        PinUnpinTab(EditTrade, UnpinBtn5, UnpinBtn6, isPinned);
                        break;
                    case "Review Existing Positions":
                        PinUnpinTab(ReviewExistingPositions, UnpinBtn7, UnpinBtn8, isPinned);
                        break;
                    case "Allocate/Reallocate":
                        PinUnpinTab(AllocateUnallocatePinTab, UnpinBtn9, UnpinBtn10, isPinned);
                        break;
                    default: break;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }
        /// <summary>
        /// common method used to pin and unpin the elements
        /// </summary>
        /// <param name="sideTabElement"></param>
        /// <param name="pinButton"></param>
        /// <param name="unpinButton"></param>
        /// <param name="isPinned"></param>
        private void PinUnpinTab(UIAutomationElement sideTabElement, UIAutomationElement pinButton, UIAutomationElement unpinButton, string isPinned)
        {
            try
            {
                sideTabElement.WaitForObject();
                if (isPinned == "Yes")
                {
                    if (sideTabElement.IsValid == true)
                    {
                        sideTabElement.Click(MouseButtons.Left);
                        pinButton.Click(MouseButtons.Left);
                    }
                }
                else
                {
                    if (sideTabElement.IsValid == false)
                    {
                        unpinButton.Click(MouseButtons.Left);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
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
