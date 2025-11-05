using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Nirvana.TestAutomation.Steps.DailyValuation;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities.Constants;
using Nirvana.TestAutomation.BussinessObjects;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Configuration;

namespace Nirvana.TestAutomation.Steps.DailyValuation
{
    public class UpdateMultipleDailyCash : DailyCashUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenDailyValuation();
                ClearOldCash();
                var gridMssaObject = GrdPivotDisplay.MsaaObject;
                BtnAdd.Click(MouseButtons.Left);

                //Dictionary that stores mapping of column names with it's index
                Dictionary<string, int> colToIndexMappingDictionary = new Dictionary<string, int>();

                int Children = 0;

                //if( (gridMssaObject.CachedChildren[0].CachedChildren[1].Accessible).Equals( true))
                // Children = gridMssaObject.CachedChildren[0].CachedChildren[1].ChildCount;
                // else
                Children = gridMssaObject.CachedChildren[0].CachedChildren[1].ChildCount;


                for (int i = 0; i < Children; i++)
                {
                    //All colums i.e=> account,local currency, cash value
                    colToIndexMappingDictionary.Add(gridMssaObject.CachedChildren[0].CachedChildren[1].CachedChildren[i].Name, i);
                }

                //List that stores column names from excel sheet whose value needs to be inserted in grid
                List<string> columnlist = new List<string>();

                foreach (DataColumn columnName in testData.Tables[sheetIndexToName[0]].Columns)
                {
                    // jo exceeel sshet ke columns h
                    columnlist.Add(columnName.ColumnName);
                }
                int recordsCounter = 0;
                int RecordsCount = 1;

                while (RecordsCount != testData.Tables[sheetIndexToName[0]].Rows.Count)
                {
                    BtnAdd.Click(MouseButtons.Left);
                    RecordsCount++;
                }
                gridMssaObject = GrdPivotDisplay.MsaaObject;
                foreach (DataRow dr in testData.Tables[sheetIndexToName[0]].Rows)
                {

                    // gridMssaObject.CachedChildren[0].CachedChildren[recordsCounter + 1].Click();
                    foreach (string col in columnlist)
                    {
                        gridMssaObject.CachedChildren[0].CachedChildren[recordsCounter + 1].CachedChildren[colToIndexMappingDictionary[col]].Click();
                        gridMssaObject.CachedChildren[0].CachedChildren[recordsCounter + 1].CachedChildren[colToIndexMappingDictionary[col]].Click();
                        Keyboard.SendKeys(dr[col].ToString());
                    }
                    recordsCounter++;

                }
                GrdPivotDisplay.Click(MouseButtons.Left);
                BtnSave.Click(MouseButtons.Left);
                GrdPivotDisplay.Click(MouseButtons.Left);
                return _result;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
                _result.IsPassed = false;
                return _result;
            }
            finally
            {
                KeyboardUtilities.CloseWindow(ref MarkPriceAndForexConversion_UltraFormManager_Dock_Area_Top);
            }
        }



        private void ClearOldCash()
        {
            try
            {
                var gridMssaObject = GrdPivotDisplay.MsaaObject;
                int childs = gridMssaObject.CachedChildren[0].ChildCount;
                for (int i = 1; i < childs; i++)
                {
                    gridMssaObject.CachedChildren[0].CachedChildren[i].CachedChildren[1].Click();
                    gridMssaObject.CachedChildren[0].CachedChildren[i].CachedChildren[1].Click();
                    BtnRemove.Click(MouseButtons.Left);

                }
                BtnSave.Click(MouseButtons.Left);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Opens the daily valuation.
        /// </summary>
        private void OpenDailyValuation()
        {
            try
            {
                Keyboard.SendKeys(ConfigurationManager.AppSettings["OPEN_DAILY_VAL"]);
                ExtentionMethods.WaitForVisible(ref MarkPriceAndForexConversion, 15);
               // Wait(5000);
                //DataManagement.Click(MouseButtons.Left);
                //DailyValuation.Click(MouseButtons.Left);
                DailyCash.Click(MouseButtons.Left);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
