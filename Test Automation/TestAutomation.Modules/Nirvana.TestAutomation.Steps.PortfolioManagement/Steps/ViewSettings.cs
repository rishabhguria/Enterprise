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
    class ViewSettings : PortfolioManagementUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {

                OpenConsolidationView();
                Main.Click(MouseButtons.Left);
                Main.Click(MouseButtons.Right);
                Wait(1000);
                String colList = string.Empty;
                if (!PopupMenuDropDown.IsVisible)
                {
                    Main.Click(MouseButtons.Right);
                }
                DataUtilities.pickFromMenuItem(PopupMenuDropDown, "Appearance");
                Wait(500);
                ViewsSettings.Click();

                
                if (!String.IsNullOrEmpty(testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_CopyFrom].ToString()))
                {
                    ComboCopyFrom.Click();
                    Keyboard.SendKeys(testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_CopyFrom].ToString());
                    Wait(1500);
                    Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                }
                

                
                if (!String.IsNullOrEmpty(testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_COPY_TO].ToString()))
                {
                    colList = testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_COPY_TO].ToString();
                    ButtonDropDown.Click(MouseButtons.Left);
                    Wait(1500);
                    foreach (string colName in colList.Split(','))
                    {
                        CopyTo(colName);
                    }
                    ButtonDropDown.Click(MouseButtons.Left);
                   
                }
                

                
                if (!String.IsNullOrEmpty(testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_DefaultView].ToString()))
                {
                    ComboDefaultView.DoubleClick(MouseButtons.Left);
                    Keyboard.SendKeys(testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_DefaultView].ToString());
                    Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                    Wait(1500);
                }
                

                
                if (!String.IsNullOrEmpty(testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_IncludeGrouping].ToString()))
                {
                    if (CheckboxIncludeGrouping.IsChecked.ToString().ToUpper() != testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_IncludeGrouping].ToString().ToUpper())
                    {
                        CheckboxIncludeGrouping.Click();
                        Wait(1500);
                    }
                }
                

                
                if (!String.IsNullOrEmpty(testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_IncludeFilters].ToString()))
                {
                    if (CheckboxIncludeFilers.IsChecked.ToString().ToUpper() != testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_IncludeFilters].ToString().ToUpper())
                    {
                        CheckboxIncludeFilers.Click();
                        Wait(1500);
                    }
                }
                

                BtnSave.DoubleClick(MouseButtons.Left);
                Wait(5000);
                if (Information.IsVisible) {
                    ButtonOK1.Click();
                }
                if (testData.Tables[sheetIndexToName[0]].Columns.Contains(TestDataConstants.COL_ViewSuccessfullPopUp))
                {
                    if (!String.IsNullOrEmpty(testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_ViewSuccessfullPopUp].ToString()))
                    {
                        if (testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_ViewSuccessfullPopUp].ToString().ToUpper().Equals("FALSE") && LabelStatus.IsVisible)
                        {
                            throw new Exception("View(s) successfully updated.Please restart PM is visible but It's column has FALSE value");
                        }
                        else if (testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_ViewSuccessfullPopUp].ToString().ToUpper().Equals("TRUE") && !LabelStatus.IsVisible)
                        {
                            throw new Exception("View(s) successfully updated.Please restart PM is not visible but It's column has TRUE value");
                        }
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
                KeyboardUtilities.CloseWindow(ref Appearance_UltraFormManager_Dock_Area_Top);
                Wait(500);
                KeyboardUtilities.MinimizeWindow(ref PM_UltraFormManager_Dock_Area_Top);
            }
            return _result;
        }

        private void CopyTo(string str)
        {
            try
            {
                UIAutomationElement accountComboItem = new UIAutomationElement();
                accountComboItem.AutomationName = str;
                accountComboItem.Comment = null;
                accountComboItem.ItemType = "";
                accountComboItem.MatchedIndex = 0;
                accountComboItem.Name = str;
                accountComboItem.Parent = this.CheckedMultipleItems;
                accountComboItem.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Unknown;
                accountComboItem.UseCoordinatesOnClick = true;
                accountComboItem.Click(MouseButtons.Left);

            }
            catch (Exception)
            {
                throw new Exception("Custom view with name " + str + " doesn't exist");
            }

        }
    }
}
