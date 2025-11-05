using Nirvana.TestAutomation.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nirvana.TestAutomation.TestDataProvider;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.Allocation
{
    public class BookOrUpdateSwap : AllocationUIMap , ITestStep
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
                AllocationGrids1.Click(MouseButtons.Left);
                Keyboard.SendKeys(KeyboardConstants.ALT_SPACE_X);
                //Due to not select the trade in allocation grid so the Swap UI is not active that's why wait
                Wait(1000);
                SwapControl.Click(MouseButtons.Left);
                InputSwapChanges(testData, sheetIndexToName);
                SaveAllocation();
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "BookOrUpdateSwap");
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
        /// Input swap change data.
        /// </summary>
        private void InputSwapChanges(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            try
            { 
                DataRow dr = testData.Tables[sheetIndexToName[0]].Rows[0];
               
               // For click the other columns of swap grid except comment which belong to XamMaskedEditor
                //foreach (DataColumn dc in testData.Tables[sheetIndexToName[0]].Columns)
                
                // called clearTextData function to clear old UI data before entering data sheet value 
                    if (!dr[TestDataConstants.COL_NOTIONAL_VALUE].ToString().Equals(String.Empty))
                    {
                        XamMaskedEditor.Click(MouseButtons.Left);
                        DataUtilities.clearTextData();
                        ExtentionMethods.UpdateCellValueConditions(dr[TestDataConstants.COL_NOTIONAL_VALUE].ToString(), "", XamMaskedEditor);
                    }
                    if (!dr[TestDataConstants.COL_INTEREST_RATE].ToString().Equals(String.Empty))
                    {
                        XamMaskedEditor5.Click(MouseButtons.Left);
                        DataUtilities.clearTextData();
                        ExtentionMethods.UpdateCellValueConditions(dr[TestDataConstants.COL_INTEREST_RATE].ToString(), "", XamMaskedEditor5);
                    }
                    if (!dr[TestDataConstants.COL_SPREAD_BP].ToString().Equals(String.Empty))
                    {
                        XamMaskedEditor6.Click(MouseButtons.Left);
                        DataUtilities.clearTextData();
                        ExtentionMethods.UpdateCellValueConditions(dr[TestDataConstants.COL_INTEREST_RATE].ToString(), "", XamMaskedEditor6);
                    }
                    if (!dr[TestDataConstants.COL_FIRST_RESET_DATE].ToString().Equals(String.Empty))
                    {

                        String resetdate = string.Empty;
                        XamMaskedEditor7.Click(MouseButtons.Left);
                        DataUtilities.clearTextData();
                        try
                        {
                            string tempDate = DataUtilities.DateHandler(testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_FIRST_RESET_DATE].ToString());
                            resetdate = String.Format(ExcelStructureConstants.COMMON_DATE_FORMAT, DateTime.Parse(tempDate));
                            ExtentionMethods.CheckCellValueConditions(resetdate, string.Empty, true);
                        }
                        catch 
                        {
                            resetdate = String.Format(ExcelStructureConstants.COMMON_DATE_FORMAT, DateTime.Parse(testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_FIRST_RESET_DATE].ToString()));
                            ExtentionMethods.CheckCellValueConditions(resetdate, string.Empty, true);
                        }

                    }
                    if (!dr[TestDataConstants.COL_DAYCOUNT].ToString().Equals(String.Empty))
                    {
                        XamMaskedEditor8.Click(MouseButtons.Left);
                        DataUtilities.clearTextData();
                        ExtentionMethods.UpdateCellValueConditions(dr[TestDataConstants.COL_DAYCOUNT].ToString(), "", XamMaskedEditor8);
                    }
                    if (!dr[TestDataConstants.COL_ORIGINAL_TRADE_DATE].ToString().Equals(String.Empty))
                    {
                        String originaldate=string.Empty;
                        XamMaskedEditor9.Click(MouseButtons.Left);
                        DataUtilities.clearTextData();
                        try
                        {
                            string tempDate = DataUtilities.DateHandler(testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_ORIGINAL_TRADE_DATE].ToString());
                            originaldate = String.Format(ExcelStructureConstants.COMMON_DATE_FORMAT, DateTime.Parse(tempDate));
                            ExtentionMethods.CheckCellValueConditions(originaldate, string.Empty, true);
                        }
                        catch
                        {
                            originaldate = String.Format(ExcelStructureConstants.COMMON_DATE_FORMAT, DateTime.Parse(testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_ORIGINAL_TRADE_DATE].ToString()));
                            ExtentionMethods.CheckCellValueConditions(originaldate, string.Empty, true);
                        }


                    }
                    if (!dr[TestDataConstants.COL_ORIGINAL_COST_BASIC].ToString().Equals(String.Empty))
                    {
                        XamMaskedEditor10.Click(MouseButtons.Left);
                        DataUtilities.clearTextData();
                        ExtentionMethods.UpdateCellValueConditions(dr[TestDataConstants.COL_ORIGINAL_COST_BASIC].ToString(), "", XamMaskedEditor10);
                    }
                //}

                if (!dr[TestDataConstants.COL_SWAP_DESCRIPTION].ToString().Equals(String.Empty))
                {
                    TextBox2.Click(MouseButtons.Left);
                    DataUtilities.clearTextData();  // calling of method after clicking on button
                    Keyboard.SendKeys(dr[TestDataConstants.COL_SWAP_DESCRIPTION].ToString());
                }
                if (dr.Table.Columns.Contains("CommissionRuleRecalculate"))
                {
                    string value = string.Empty;
                    if (!dr[TestDataConstants.COL_CommissionRuleRecalculate].ToString().Equals(String.Empty))
                    {
                        value = dr[TestDataConstants.COL_CommissionRuleRecalculate].ToString();
                        if (value.ToUpper().Equals("YES"))
                        {
                            if (BookasSwap.IsVisible)
                                BookasSwap.Click(MouseButtons.Left);
                            if (NirvanaAllocation.IsVisible)
                                ButtonYes.Click(MouseButtons.Left);
                            if (SwapUpdate.IsVisible)
                                SwapUpdate.Click(MouseButtons.Left);
                            if (NirvanaAllocation.IsVisible)
                                ButtonYes.Click(MouseButtons.Left);
                        }
                        else if (value.ToUpper().Equals("NO"))
                        {
                            if (BookasSwap.IsVisible)
                                BookasSwap.Click(MouseButtons.Left);
                            if (NirvanaAllocation.IsVisible)
                                ButtonNo.Click(MouseButtons.Left);
                            if (SwapUpdate.IsVisible)
                                SwapUpdate.Click(MouseButtons.Left);
                            if (NirvanaAllocation.IsVisible)
                                ButtonNo.Click(MouseButtons.Left);
                        }
                    }
                    else
                    {
                        if (BookasSwap.IsVisible)
                            BookasSwap.Click(MouseButtons.Left);
                        if (NirvanaAllocation.IsVisible)
                            ButtonYes.Click(MouseButtons.Left);
                        if (SwapUpdate.IsVisible)
                            SwapUpdate.Click(MouseButtons.Left);
                        if (NirvanaAllocation.IsVisible)
                            ButtonYes.Click(MouseButtons.Left);
                    }
                }
                else
                {
                    if (BookasSwap.IsVisible)
                        BookasSwap.Click(MouseButtons.Left);
                    if (NirvanaAllocation.IsVisible)
                        ButtonYes.Click(MouseButtons.Left);
                    if (SwapUpdate.IsVisible)
                        SwapUpdate.Click(MouseButtons.Left);
                    if (NirvanaAllocation.IsVisible)
                        ButtonYes.Click(MouseButtons.Left);
                    // Wait(500);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Clicks the xam masked editor.
        /// </summary>
        /// <param name="id">The identifier.</param>
        private void ClickXamMaskedEditor(int id)
        {
            try
            {
                UIAutomationElement editorObj = new UIAutomationElement();
                editorObj.AutomationName = "";
                editorObj.ClassName = "XamMaskedEditor";
                editorObj.Comment = null;
                editorObj.Index = id + 8;
                editorObj.ItemType = "";
                editorObj.MatchedIndex = id;
                editorObj.Name = "XamMaskedEditor";
                editorObj.Parent = this.ScrollViewer5;
                editorObj.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Unknown;
                editorObj.UseCoordinatesOnClick = false;
                editorObj.Click(MouseButtons.Left);
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
