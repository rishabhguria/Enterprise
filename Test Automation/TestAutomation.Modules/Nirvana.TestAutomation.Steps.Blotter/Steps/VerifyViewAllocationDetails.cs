using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Interfaces;
using System.Collections.Generic;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.Blotter
{
    public partial class VerifyViewAllocationDetails : BlotterUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                OpenBlotter();
                WorkingSubsTab.Click(MouseButtons.Left);
                if (testData != null)
                {
                    DgBlotter.MsaaObject.CachedChildren[0].CachedChildren[1].Click(MouseButtons.Right);
                    bool isClicked = false;
                    try
                    {
                        isClicked = pickFromMenuItem(PopupMenuContext, TestDataConstants.Allocate);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    if (isClicked == false)
                    {
                        WorkingSubBlotterGrid2.Click(MouseButtons.Right);
                        ViewAllocationDetails1.Click(MouseButtons.Left);
                    }
                }
                List<String> errors = VerifyGridData(testData.Tables[0]);
                if (errors.Count > 0)
                {
                    _res.ErrorMessage = String.Join("\n\r", errors);
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
                //Close1.Click(MouseButtons.Left);
                CloseBlotter();
            }
            return _res;
        }

        private List<String> VerifyGridData(DataTable dTable)
        {
            List<String> errors = new List<string>();
            try
            {
                ViewAllColumnsOnGrid(dTable);
                DataTable dtBlotter = DataUtilities.RemoveTrailingZeroes(CSVHelper.CSVAsDataTable(this.GridViewAllocation1.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString()));
                dtBlotter = DataUtilities.RemoveCommas(dtBlotter);
                List<String> columns = new List<string>();
                /*columns = (from DataColumn x in dTable.Columns
                select x.ColumnName).ToList();*/
                //columns.Add("Symbol");
                if (dTable.Rows.Count!=0 && dtBlotter.Rows.Count!=0)
                {
                    errors = Recon.RunRecon(dtBlotter, dTable, columns, 0.01);
                }  
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return errors;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
