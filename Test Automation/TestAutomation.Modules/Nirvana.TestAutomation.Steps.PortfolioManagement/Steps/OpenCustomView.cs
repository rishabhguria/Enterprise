using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using System;
using System.ComponentModel;
using System.Data;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using System.Collections.Generic;
using System.Windows.Forms;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.PortfolioManagement
{
    class OpenCustomView : PortfolioManagementUIMap, ITestStep
    {
        public TestResult RunTest(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenConsolidationView();
               // Wait(2000);

                if(!string.IsNullOrEmpty(testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_CUSTOM_VIEW_NAME].ToString())){
                    try { 
                        UIAutomationElement accountComboItem = new UIAutomationElement();
                        accountComboItem.AutomationName = testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_CUSTOM_VIEW_NAME].ToString();
                        accountComboItem.Comment = null;
                        accountComboItem.ItemType = "";
                        accountComboItem.MatchedIndex = 0;
                        accountComboItem.Name = testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_CUSTOM_VIEW_NAME].ToString();
                        accountComboItem.Parent = this.PMTabView;
                        accountComboItem.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Unknown;
                        accountComboItem.UseCoordinatesOnClick = true;
                        accountComboItem.Click(MouseButtons.Left);
                    }
                    catch (Exception)
                    {
                        throw new Exception("Custom view with name " + testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_CUSTOM_VIEW_NAME].ToString() + " doesn't exist");
                    }
                }

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
                KeyboardUtilities.MinimizeWindow(ref PM_UltraFormManager_Dock_Area_Top);
                
            }
            return _result;

        }
    }
}
