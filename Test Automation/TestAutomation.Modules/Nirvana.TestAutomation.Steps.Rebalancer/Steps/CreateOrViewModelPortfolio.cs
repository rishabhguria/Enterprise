using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Nirvana.TestAutomation.BussinessObjects;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using System.IO;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.Steps.Rebalancer;
using Nirvana.TestAutomation.UIAutomation;

namespace Nirvana.TestAutomation.Steps.Rebalancer
{
    class CreateOrViewModelPortfolio : RebalancerUIMap, IUIAutomationTestStep
    {
        public TestResult RunUIAutomationTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenRebalancer();
                Wait(4000);
                ModelPortfolio.Click(MouseButtons.Left);
                KeyboardUtilities.MaximizeWindow(ref RebalanceTab);
                ApplicationArguments.mapdictionary = DataUtilities.CreateDictionaryFromDataTable(ApplicationArguments.IUIAutomationMappingTables["RebalancerWindow"]);
                UIAutomationHelper uiAutomationHelper = new UIAutomationHelper();

                if (testData.Tables[sheetIndexToName[0]].Rows.Count > 0)
                {
                    DataRow dr = testData.Tables[sheetIndexToName[0]].Rows[0];
                    AddPortfolio(dr, uiAutomationHelper);
                }
            }
            catch (Exception ex)
            {
                _result.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return _result;
        }


        private void AddPortfolio(DataRow dr, UIAutomationHelper uiAutomationHelper)
        {
            try
            {
                try
                {
                    uiAutomationHelper.FindAndClickElementIfVisible(ApplicationArguments.mapdictionary["btnNewMPView"].AutomationUniqueValue);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    if (New.IsVisible)
                    {
                        New.Click(MouseButtons.Left);
                    }
                }

                if (!dr[TestDataConstants.PORTFOLIONAME].ToString().Equals(string.Empty))
                {
                    TextBox2.Click(MouseButtons.Left);
                    Keyboard.SendKeys(dr[TestDataConstants.PORTFOLIONAME].ToString());
                    Wait(2000);
                }
                if (!dr[TestDataConstants.PORTFOLIOTYPE].ToString().Equals(string.Empty))
                {
                    TextBoxPresenter4.DoubleClick(MouseButtons.Left);
                    DataUtilities.clearTextData(30);
                    Keyboard.SendKeys(dr[TestDataConstants.PORTFOLIOTYPE].ToString());
                    Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                    Wait(2000);
                }
                if (!dr[TestDataConstants.POSITIONTYPE].ToString().Equals(string.Empty))
                {
                    TextBoxPresenter6.DoubleClick(MouseButtons.Left);
                    DataUtilities.clearTextData(30);
                    Keyboard.SendKeys(dr[TestDataConstants.POSITIONTYPE].ToString());
                    Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                    Wait(2000);
                }
                if (!dr[TestDataConstants.MODELTYPE].ToString().Equals(string.Empty))
                {
                    TextBoxPresenter5.DoubleClick(MouseButtons.Left);
                    DataUtilities.clearTextData(30);
                    Keyboard.SendKeys(dr[TestDataConstants.MODELTYPE].ToString());
                    Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                    Wait(2000);
                }
                if (!dr[TestDataConstants.ACCOUNTNAME].ToString().Equals(string.Empty))
                {
                    TextBoxPresenter7.DoubleClick(MouseButtons.Left);
                    DataUtilities.clearTextData(30);
                    Keyboard.SendKeys(dr[TestDataConstants.ACCOUNTNAME].ToString());
                    Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                    Wait(2000);
                }
                if (!dr[TestDataConstants.MASTERFUNDNAME].ToString().Equals(string.Empty))
                {
                    TextBoxPresenter8.DoubleClick(MouseButtons.Left);
                    DataUtilities.clearTextData(30);
                    Keyboard.SendKeys(dr[TestDataConstants.MASTERFUNDNAME].ToString());
                    Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                    Wait(2000);
                }
                if (!dr[TestDataConstants.CUSTOMGROUP].ToString().Equals(string.Empty))
                {
                    TextBoxPresenter9.Click(MouseButtons.Left);
                    DataUtilities.clearTextData(30);
                    Keyboard.SendKeys(dr[TestDataConstants.CUSTOMGROUP].ToString());
                    Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                    Wait(2000);
                }
                if (!dr[TestDataConstants.CUSTOMGROUP].ToString().Equals(string.Empty))
                {
                    TextBoxPresenter9.Click(MouseButtons.Left);
                    DataUtilities.clearTextData(30);
                    Keyboard.SendKeys(dr[TestDataConstants.CUSTOMGROUP].ToString());
                    Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                    Wait(2000);
                }


                if (dr.Table.Columns.Contains(TestDataConstants.Use_Tolerance) && !string.IsNullOrEmpty(dr[TestDataConstants.Use_Tolerance].ToString()))
                {
                    TextBoxPresenter11.DoubleClick(MouseButtons.Left);
                    DataUtilities.clearTextData();
                    Keyboard.SendKeys(dr[TestDataConstants.Use_Tolerance].ToString());
                    Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                    Wait(2000);
                }

                if (dr.Table.Columns.Contains(TestDataConstants.In_Percentage_In_BPS) && !string.IsNullOrEmpty(dr[TestDataConstants.In_Percentage_In_BPS].ToString()))
                {
                    TextBoxPresenter12.DoubleClick(MouseButtons.Left);
                    DataUtilities.clearTextData(30);
                    Keyboard.SendKeys(dr[TestDataConstants.In_Percentage_In_BPS].ToString());
                    Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                    Wait(2000);
                }

                if (!dr[TestDataConstants.IMPORTMODELPORTFOLIOPATH].ToString().Equals(string.Empty))
                {
                    ImportModelPortfolio1.Click(MouseButtons.Left);
                    Wait(2000);
                    TextBoxFilename4.Click(MouseButtons.Left);
                    Wait(2000);
                    Keyboard.SendKeys(dr[TestDataConstants.IMPORTMODELPORTFOLIOPATH].ToString());
                    ButtonOpen1.Click(MouseButtons.Left);
                    if (!dr.Table.Columns.Contains("CheckPopUp"))
                    {
                        if (NirvanaAlert1.IsVisible)
                        {
                            ButtonOK.Click(MouseButtons.Left);
                        }
                    }
                    else if (string.IsNullOrEmpty(dr["CheckPopUp"].ToString()))
                    {
                        if (NirvanaAlert1.IsVisible)
                        {
                            ButtonOK.Click(MouseButtons.Left);
                        }
                    }
                    Wait(3000);
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);

                }

                if (dr.Table.Columns.Contains("BringTo") && !string.IsNullOrEmpty(dr["BringTo"].ToString()))
                {

                    TextBoxPresenter13.DoubleClick(MouseButtons.Left);
                    DataUtilities.clearTextData();
                    Keyboard.SendKeys(dr["BringTo"].ToString());
                    Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                }
                if (dr.Table.Columns.Contains("CheckPopUp") && dr["CheckPopUp"].ToString().ToUpper().Equals("TRUE"))
                {
                    ButtonOK.Click(MouseButtons.Left);
                }
                else
                {
                    Save3.Click(MouseButtons.Left);
                    if (NirvanaAlert1.IsVisible)
                    {
                        ButtonOK.Click(MouseButtons.Left);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

    }
}
