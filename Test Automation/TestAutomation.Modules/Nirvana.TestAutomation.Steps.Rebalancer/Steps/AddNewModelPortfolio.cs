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

namespace Nirvana.TestAutomation.Steps.Rebalancer
{
    class AddNewModelPortfolio : RebalancerUIMap, ITestStep
    {
        /// <summary>
        /// Run Test
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        /// <returns></returns>
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenRebalancer();
                Wait(4000);
                ModelPortfolio.Click(MouseButtons.Left);
                KeyboardUtilities.MaximizeWindow(ref RebalanceTab);
                if (testData.Tables[sheetIndexToName[0]].Rows.Count > 0)
                {
                    DataRow dr = testData.Tables[sheetIndexToName[0]].Rows[0];
                    AddPortfolio(dr);                  
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


        private void AddPortfolio(DataRow dr)
        {
            try
            {
                if (New.IsVisible)
                {
                    New.Click(MouseButtons.Left);
                    Wait(2000);
                }
                if (!dr[TestDataConstants.PORTFOLIONAME].ToString().Equals(string.Empty))
                {
                    TextBox2.Click(MouseButtons.Left);
                    DataUtilities.clearTextData(30);
                    Keyboard.SendKeys(dr[TestDataConstants.PORTFOLIONAME].ToString());
                }
                Keyboard.SendKeys(KeyboardConstants.TABKEY);
                if (!dr[TestDataConstants.PORTFOLIOTYPE].ToString().Equals(string.Empty))
                {
                    TextBoxPresenter4.Click(MouseButtons.Left);
                    DataUtilities.clearTextData(30);
                    Keyboard.SendKeys(dr[TestDataConstants.PORTFOLIOTYPE].ToString());
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                }                
                if (!dr[TestDataConstants.POSITIONTYPE].ToString().Equals(string.Empty))
                {
                        DataUtilities.clearTextData(30);
                        Keyboard.SendKeys(dr[TestDataConstants.POSITIONTYPE].ToString());
                        Keyboard.SendKeys(KeyboardConstants.TABKEY);  
                }                
                if (!dr[TestDataConstants.MODELTYPE].ToString().Equals(string.Empty))
                {      
                        DataUtilities.clearTextData(30);
                        Keyboard.SendKeys(dr[TestDataConstants.MODELTYPE].ToString());
                        Keyboard.SendKeys(KeyboardConstants.TABKEY); 
                }
                if (!dr[TestDataConstants.ACCOUNTNAME].ToString().Equals(string.Empty))
                {                   
                        
                        DataUtilities.clearTextData(30);
                        Keyboard.SendKeys(dr[TestDataConstants.ACCOUNTNAME].ToString());
                        Keyboard.SendKeys(KeyboardConstants.TABKEY);                    
                }
                if (!dr[TestDataConstants.MASTERFUNDNAME].ToString().Equals(string.Empty))
                {                    
                        DataUtilities.clearTextData(30);
                        Keyboard.SendKeys(dr[TestDataConstants.MASTERFUNDNAME].ToString());
                        Keyboard.SendKeys(KeyboardConstants.TABKEY);                    
                }
                if (!dr[TestDataConstants.CUSTOMGROUP].ToString().Equals(string.Empty))
                {                    
                        DataUtilities.clearTextData(30);
                        Keyboard.SendKeys(dr[TestDataConstants.CUSTOMGROUP].ToString());
                        Keyboard.SendKeys(KeyboardConstants.TABKEY);                    
                }
                if (!dr[TestDataConstants.CUSTOMGROUP].ToString().Equals(string.Empty))
                {                   
                       
                        DataUtilities.clearTextData(30);
                        Keyboard.SendKeys(dr[TestDataConstants.CUSTOMGROUP].ToString());
                        Keyboard.SendKeys(KeyboardConstants.TABKEY);                    
                }
                if (!dr[TestDataConstants.IMPORTMODELPORTFOLIOPATH].ToString().Equals(string.Empty))
                {
                        Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                        //DataUtilities.clearTextData();
                        Keyboard.SendKeys(dr[TestDataConstants.IMPORTMODELPORTFOLIOPATH].ToString());
                        ButtonOpen1.Click(MouseButtons.Left);
                        if (NirvanaAlert1.IsVisible)
                        {
                            ButtonOK.Click(MouseButtons.Left);
                        }
                        Wait(3000);
                        Keyboard.SendKeys(KeyboardConstants.TABKEY);
                    
                }
                Save3.Click(MouseButtons.Left);
                if (NirvanaAlert1.IsVisible)
                {
                    ButtonOK.Click(MouseButtons.Left);
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