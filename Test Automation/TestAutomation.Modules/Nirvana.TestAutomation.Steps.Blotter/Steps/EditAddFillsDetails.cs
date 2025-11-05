using System.Data;
using System.Linq;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;


namespace Nirvana.TestAutomation.Steps.Blotter
{
    public class EditAddFillsDetails : BlotterUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                PranaApplication.BringToFrontOnAttach = false;

               // Wait(6000);
                if (testData != null)
                {
                    foreach (DataRow dr in testData.Tables[0].Rows)
                    {
                        InputEnter(dr);
                    }
                }
               
                if (NirvanaAlert.IsVisible|| NirvanaAlert.IsValid)
                {
                    ButtonOK3.Click(MouseButtons.Left);
 
                }
                BtnSave.Click(MouseButtons.Left);
               // Wait(2000);
                if (Warning3.IsVisible || Warning3.IsValid)
                {
                    ButtonOK5.Click(MouseButtons.Left);
                    AddFills_UltraFormManager_Dock_Area_Top1.Click(MouseButtons.Left);
                    KeyboardUtilities.CloseWindow(ref AddFills_UltraFormManager_Dock_Area_Top1);
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
                CloseBlotter();
            }
            return _res;
        }
        private void InputEnter(DataRow dr)
        {
            try
            {

                Dictionary<String, int> colToIndex = new Dictionary<String, int>();
                var fillmsaobject = blotterFillGrid.MsaaObject;
                if (colToIndex.Count == 0)
                {
                    colToIndex = createDictionary(fillmsaobject);
                }
                if (dr.Table.Columns.Contains(TestDataConstants.COL_New) && dr[TestDataConstants.COL_New].ToString() == "False")
                {
                    fillmsaobject.FindDescendantByName("OrderBindingList", 3000).FindDescendantByName("OrderBindingList row 1", 3000).FindDescendantByName("Fill", 3000).Click(MouseButtons.Left);
                    Keyboard.SendKeys("[HOME]");
                    MouseController.DoubleClick();
                    Keyboard.SendKeys(KeyboardConstants.BACKSPACEKEY);
                    Keyboard.SendKeys(dr[TestDataConstants.COL_FILL_QUANTITY].ToString());
                    Wait(1000);
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                
                    Keyboard.SendKeys(dr[TestDataConstants.COL_LAST_FILL_PRICE_LOCAL].ToString());
                }
                else
                {
                    fillmsaobject.FindDescendantByName("OrderBindingList", 3000).FindDescendantByName("Template Add Row", 3000).FindDescendantByName("Fill", 3000).Click(MouseButtons.Left);
                    Keyboard.SendKeys("[HOME]");
                    MouseController.DoubleClick();
                    Keyboard.SendKeys(KeyboardConstants.BACKSPACEKEY);
                    Keyboard.SendKeys(dr[TestDataConstants.COL_FILL_QUANTITY].ToString());
                    Wait(1000);
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                    Keyboard.SendKeys(dr[TestDataConstants.COL_LAST_FILL_PRICE_LOCAL].ToString());
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static Dictionary<String, int> createDictionary(MsaaObject fillmsaobject)
        {
            Dictionary<String, int> colToIndex = new Dictionary<String, int>();
            try
            {
                var colHeaders = fillmsaobject.FindDescendantByName("OrderBindingList", 3000).FindDescendantByName("Column Headers", 3000);

                for (int i = 0; i < colHeaders.ChildCount; i++)
                {
                    colToIndex.Add(colHeaders.CachedChildren[i].Name, i);
                }
            }
            catch (Exception)
            {
                
                throw;
            }

            return colToIndex;
        }

        /// <summary>
        /// Disposes resources
        /// </summary>
        /// <param name="disposing"></param>1
        protected override void Dispose(bool disposing)
        {
            base.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
