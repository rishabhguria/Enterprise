using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Factory;
using CommandType = Nirvana.TestAutomation.Interfaces.Enums.CommandType;

namespace Nirvana.TestAutomation.Steps.Allocation
{
    class CleanUpUI : AllocationUIMap, ITestStep
    {
        /// <summary>
        /// Runs the test.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <returns></returns>
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                OpenAllocation();
                if (testData != null)
                {
                    DataRow emptydatarow=null;
                    if (testData.Tables.Count==0)
                    {
                        Unallocate(emptydatarow);
                    }
                    else
                    {
                        foreach (DataRow dr in testData.Tables[0].Rows)
                        {
                            Unallocate(dr);
                        }
                     }
                    
                }
                if (NirvanaAllocation.IsVisible)
                {
                    ButtonNo.Click(MouseButtons.Left);
                }
                DeleteRecords();
                Save();
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "CleanUpUI");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return _res;
        }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        private void Save()
        {
            try
            {
                SavewDivideStatus.Click(MouseButtons.Left);
                //Wait(1000);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Ddeletes this instance.
        /// </summary>
        private void DeleteRecords()
        {
            try
            {
                AllocationGrids.Click(MouseButtons.Left);
                AllocationGrids1.Click(MouseButtons.Left);
                Records = GetLatestGridObject(this.GridUnallocated);
                TestAutomationFX.UI.UIControlPart CheckBoxupper = new TestAutomationFX.UI.UIControlPart();
                SelectAllCheckBox(CheckBoxupper, GridUnallocated);
                WaitOnItems(Records);
                if (Records.AutomationElementWrapper.Children.Count > 1)
                {
                    CheckBoxupper.Click(MouseButtons.Left);
                    CheckBoxupper.Click(MouseButtons.Right);
                    Delete.Click(MouseButtons.Left);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Unallocates this instance.
        /// </summary>
        private void Unallocate(DataRow dr)
        {
            try
            {
                Historical.Click(MouseButtons.Left);
                FromDate.Click(MouseButtons.Left);

                String fromdate = "1/1/1800";
                String todate = String.Format(ExcelStructureConstants.COMMON_DATE_FORMAT, DateTime.Today.ToString("MM/dd/yyyy"));
                if (dr != null)
                {
                    if (!string.IsNullOrWhiteSpace(dr[TestDataConstants.COL_FROM_DATE].ToString()))
                    {
                        fromdate = String.Format(ExcelStructureConstants.COMMON_DATE_FORMAT, DateTime.Parse(dr[TestDataConstants.COL_FROM_DATE].ToString()));
                    }
                    if (!string.IsNullOrWhiteSpace(dr[TestDataConstants.COL_TO_DATE].ToString()))
                    {
                        todate = String.Format(ExcelStructureConstants.COMMON_DATE_FORMAT, DateTime.Parse(dr[TestDataConstants.COL_TO_DATE].ToString()));

                    }

                }

                FromDate.Click(MouseButtons.Left);
                ExtentionMethods.CheckCellValueConditions(fromdate, string.Empty, true);
                Wait(2000);
                ToDate.Click(MouseButtons.Left);
                ExtentionMethods.CheckCellValueConditions(todate, string.Empty, true);

                GetData.Click(MouseButtons.Left);
                TestAutomationFX.UI.UIControlPart CheckBox = new TestAutomationFX.UI.UIControlPart();
                SelectAllCheckBox(CheckBox, GridAllocated);
                WaitOnItems(Records1);
                if (Records1.AutomationElementWrapper.Children.Count > 1)
                {
                    CheckBox.Click(MouseButtons.Left);
                    CheckBox.Click(MouseButtons.Right);
                    if (UnAllocate.IsEnabled)
                    {
                        UnAllocate.Click(MouseButtons.Left);
                    }
                   // Wait(1000);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void SelectAllCheckBox(TestAutomationFX.UI.UIControlPart CheckBox, UIAutomationElement parent)
        {
            CheckBox.BoundsInParent = new System.Drawing.Rectangle(30, 3, 20, 17);
            CheckBox.Comment = null;
            CheckBox.ControlPartProvider = null;
            CheckBox.Name = "ControlPartOfRecords";
            CheckBox.ObjectImage = null;
            CheckBox.Parent = parent;
            CheckBox.Path = null;
            CheckBox.UIObjectType = TestAutomationFX.UI.UIObjectTypes.ControlPart;
            CheckBox.UseCoordinatesOnClick = false;
        }

        private void WaitOnItems(UIAutomationElement uiAutomation)
        {
            try
            {
                uiAutomation.Click(MouseButtons.Left);
                Stopwatch tmr = new Stopwatch();
                tmr.Start();
                while (uiAutomation.AutomationElementWrapper.Children.Count <= 1)
                {
                    Wait(2000);
                    if (tmr.ElapsedMilliseconds >= 15000)
                    {
                        break;
                    }
                }
                tmr.Stop();
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

