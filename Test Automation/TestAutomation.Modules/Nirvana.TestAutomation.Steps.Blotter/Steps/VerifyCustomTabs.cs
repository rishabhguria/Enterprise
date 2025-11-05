using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Interfaces;
using System.Collections.Generic;
using Nirvana.TestAutomation.Steps.Blotter;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;


namespace Nirvana.TestAutomation.Steps.Blotter
{
    class VerifyCustomTabs : BlotterUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {

            TestResult _res = new TestResult();
            try
            {
                OpenBlotter();
                MaximizeBlotter();
                Wait(1000);
                bool flag = true;

                if (testData.Tables[0].Columns.Contains(TestDataConstants.COL_ACTION))
                {
                    if (!string.IsNullOrEmpty(testData.Tables[0].Rows[0][TestDataConstants.COL_ACTION].ToString()))
                    {
                        DgBlotter4.Click(MouseButtons.Right);
                        if (testData.Tables[0].Rows[0][TestDataConstants.COL_ACTION].ToString().ToUpper().Equals("RENAME"))
                        {

                            bool check = pickFromMenuItem(PopupMenuContext, "Rename Tab");
                            if (TextBox11.IsEnabled)
                            {

                                TextBox11.Click(MouseButtons.Left);
                                TextBox11.Properties["Text"] = testData.Tables[0].Rows[0][TestDataConstants.Col_CustomTabName].ToString();

                            }
                            BtnOK1.Click(MouseButtons.Left);
                                
                            if (testData.Tables[0].Rows[0][TestDataConstants.ApprovalForAction].ToString().ToUpper().Equals("TRUE"))
                            {
                                if (NirvanaBlotter.IsVisible)
                                {
                                    ButtonYes.Click(MouseButtons.Left);
                                }

                            }
                            else if (testData.Tables[0].Rows[0][TestDataConstants.ApprovalForAction].ToString().ToUpper().Equals("FALSE"))
                            {
                                if (NirvanaBlotter.IsVisible)
                                {
                                    ButtonNo.Click(MouseButtons.Left);
                                }
                            }


                        }
                        else if (testData.Tables[0].Rows[0][TestDataConstants.COL_ACTION].ToString().ToUpper().Equals("REMOVE"))
                        {
                            bool check = pickFromMenuItem(PopupMenuContext, "Remove Tab");
                            if (testData.Tables[0].Rows[0][TestDataConstants.ApprovalForAction].ToString().ToUpper().Equals("TRUE"))
                            {
                                if (NirvanaBlotter.IsVisible)
                                {
                                    ButtonYes.Click(MouseButtons.Left);
                                }
                            }
                            else if (testData.Tables[0].Rows[0][TestDataConstants.ApprovalForAction].ToString().ToUpper().Equals("FALSE"))
                            {
                                if (NirvanaBlotter.IsVisible)
                                {
                                    ButtonNo.Click(MouseButtons.Left);
                                }
                            }

                        }
                    }
                }
                
                UIAutomationElement accountComboItem = new UIAutomationElement();
                accountComboItem.AutomationName = testData.Tables[0].Rows[0][TestDataConstants.TAB_NAME].ToString();
                accountComboItem.Comment = null;
                accountComboItem.ItemType = "";
                accountComboItem.MatchedIndex = 0;
                accountComboItem.Name = testData.Tables[0].Rows[0][TestDataConstants.TAB_NAME].ToString();
                accountComboItem.Parent = this.BlotterTabControl1;
                accountComboItem.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Unknown;
                accountComboItem.UseCoordinatesOnClick = true;
                try
                {
                    accountComboItem.Click(MouseButtons.Left);
                }
                catch (Exception ex)
                {
                    flag = false;
                }
                if (flag.ToString().ToUpper() != testData.Tables[0].Rows[0][TestDataConstants.COL_Tab_Valid].ToString().ToUpper())
                {
                    throw new Exception("Tab visiblity is " + testData.Tables[0].Rows[0][TestDataConstants.COL_Tab_Valid].ToString() + " but Tab with name " + testData.Tables[0].Rows[0][TestDataConstants.TAB_NAME].ToString() + " visiblity is " + accountComboItem.IsVisible);
                }

                if (!string.IsNullOrEmpty(testData.Tables[0].Rows[0][TestDataConstants.COL_LOWERSTRIPMESSAGE].ToString()))
                {
                    if (StatusStrip11.IsVisible.ToString().ToUpper() != testData.Tables[0].Rows[0][TestDataConstants.COL_LOWERSTRIPMESSAGE].ToString().ToUpper())
                    {
                        throw new Exception("Status strip visiblity is " + StatusStrip11.IsVisible + " but Excel has " + testData.Tables[0].Rows[0][TestDataConstants.COL_LOWERSTRIPMESSAGE].ToString());
                    }
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
                MinimizeBlotter();
            }
            return _res;
        }
    }
}
