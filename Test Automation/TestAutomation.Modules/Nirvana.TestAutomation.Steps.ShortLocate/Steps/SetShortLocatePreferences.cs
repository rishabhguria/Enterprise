using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using System.Linq;

namespace Nirvana.TestAutomation.Steps.ShortLocate
{
    public class SetShortLocatePreferences : ShortLocatePreferencesUIMap, ITestStep
    {
        /// <summary>
        /// Runs the test.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <returns></returns>
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenShortLocatePreferences();
                DataRow row = testData.Tables[sheetIndexToName[0]].Rows[0];
                SetShortLocatePreference(row);
                BtnSave.Click(MouseButtons.Left);
            }
            catch (Exception ex)
            {
                _result.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {
                BtnClose.Click(MouseButtons.Left);
            }
            return _result;

        }
        /// <summary>
        /// Enter ShortLocate Preferences
        /// </summary>
        /// <param name="row"></param>
        private void SetShortLocatePreference(DataRow dr)
        {
            try
            {
                if (dr[TestDataConstants.COL_Alert].ToString() != string.Empty)
                {
                    CmbAlert.Click(MouseButtons.Left);
                    ClearText(CmbAlert);
                    Keyboard.SendKeys(dr[TestDataConstants.COL_Alert].ToString());
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                }
                if (dr[TestDataConstants.COL_Fees].ToString() != string.Empty)
                {
                    CmbYTD.Click(MouseButtons.Left);
                    ClearText(CmbYTD);
                    Keyboard.SendKeys(dr[TestDataConstants.COL_Fees].ToString());
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                }
                if (dr[TestDataConstants.COL_RebateFees].ToString() != string.Empty)
                {
                    CmbRebateFees.Click(MouseButtons.Left);
                    ClearText(CmbRebateFees);
                    Keyboard.SendKeys(dr[TestDataConstants.COL_RebateFees].ToString());
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                }
                if (dr[TestDataConstants.COL_BROKER].ToString() != string.Empty)
                {
                    CmbDefaultBorrowerBroker.Click(MouseButtons.Left);
                    ClearText(CmbDefaultBorrowerBroker);
                    Keyboard.SendKeys(dr[TestDataConstants.COL_BROKER].ToString());
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                }
                MsaaObject obj = GrdDecimalPlaces.MsaaObject.CachedChildren.First(x => x.Name.Equals("BindingList`1"));
                if (dr[TestDataConstants.COL_LastPX_Decimal].ToString() != string.Empty)
                {
                    obj.CachedChildren[1].CachedChildren[1].Click(MouseButtons.Left);
                    Keyboard.SendKeys(dr[TestDataConstants.COL_LastPX_Decimal].ToString());
                }
                if (dr[TestDataConstants.COL_RebateFees_Decimal].ToString() != string.Empty)
                {
                    obj.CachedChildren[2].CachedChildren[1].Click(MouseButtons.Left);
                    Keyboard.SendKeys(dr[TestDataConstants.COL_RebateFees_Decimal].ToString());
                }
                if (dr[TestDataConstants.COL_TotalAmount_Decimal].ToString() != string.Empty)
                {
                    obj.CachedChildren[3].CachedChildren[1].Click(MouseButtons.Left);
                    Keyboard.SendKeys(dr[TestDataConstants.COL_TotalAmount_Decimal].ToString());
                }
            }
            catch (Exception) { throw; }
        }
    }
}
