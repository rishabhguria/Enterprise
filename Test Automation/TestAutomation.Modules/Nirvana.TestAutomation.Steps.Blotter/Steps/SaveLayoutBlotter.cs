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
    class SaveLayoutBlotter : BlotterUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();

            try
            {
                OpenBlotter();
                //Wait(6000);
                MaximizeBlotter();
                ChooseTab(testData.Tables[0].Rows[0][TestDataConstants.TAB_NAME].ToString());
                ColumnChooserBtn.Click(MouseButtons.Left);
                Filter.Click(MouseButtons.Left);
                Keyboard.SendKeys("[HOME]");
                Keyboard.SendKeys("[ENTER]");
                ColumnChoserTxtBox.Click(MouseButtons.Left);
                foreach (DataRow dr in testData.Tables[0].Rows)
                {
                    Clipboard.SetText(dr[TestDataConstants.COLUMN_NAME].ToString());
                    Keyboard.SendKeys("[CTRL+V]");
                    ListRowItem.Drag(ColumnHeaders);
                    ColumnChoserTxtBox.Click(MouseButtons.Left);
                    ColumnChoserTxtBox.Click(MouseButtons.Right);
                    SelectAll.Click(MouseButtons.Left);
                }
                DgBlotter1.Click(MouseButtons.Right);

                bool isClicked = false;
                try
                {
                    isClicked = pickFromMenuItem(PopupMenuContext, TestDataConstants.Save_Layout);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                if (isClicked == false)
                {
                    DgBlotter1.Click(MouseButtons.Right);
                    if (SaveLayout.IsVisible)
                    {
                        SaveLayout.Click(MouseButtons.Left);
                    }
                    else
                    {
                        Console.WriteLine("Menu Item {0} is not visible", SaveLayout.MsaaName);
                    }
                }
                
                //SaveLayout.Click(MouseButtons.Left);
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
               if(CloseDialogBtn.IsValid)
                CloseDialogBtn.Click(MouseButtons.Left);
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
