using Nirvana.TestAutomation.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nirvana.TestAutomation.TestDataProvider;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using System.Configuration;

namespace Nirvana.TestAutomation.Steps.Allocation
{
    class TradeAttributeBulkChangeGroups : AllocationUIMap, ITestStep
    {
         /// <summary>
       /// Runs the test.
       /// </summary>
       /// <param name="testData">The test data.</param>
       /// <returns></returns>
       public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
           try
           {
               OpenTradeAtributeBulkChange();
               InputChanges(testData, sheetIndexToName);
               AllocationGrids.Click(MouseButtons.Left);
           }
           catch (Exception ex)
           {
               SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "TradeAttributeBulkChangeGroups");
               _res.IsPassed = false;
               bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
               if (rethrow)
                   throw;
           }
           finally 
           {
               MinimizeAllocation();
           }
           return _res;
       }

       /// <summary>
       /// Opens the Trade Attribute bulk change.
       /// </summary>
       private void OpenTradeAtributeBulkChange()
       {
           try
           {
               //  Shortcut to open allocation module(CTRL + SHIFT + A)
               Keyboard.SendKeys(ConfigurationManager.AppSettings["OPEN_ALLOCATION"]);
               ExtentionMethods.WaitForVisible(ref Allocation, 15); 
               //Wait(15000);
               //Trade.Click(MouseButtons.Left);
               //Allocation2.Click(MouseButtons.Left);
               TradeAttributebulkchange.Click(MouseButtons.Left);
           }
           catch (Exception)
           {
               throw;
           }
       }

       /// <summary>
       /// Inputs the Trade Attribute Bulk Changes on Groups.
       /// </summary>
       private void InputChanges(DataSet testData, Dictionary<int, string> sheetIndexToName)
       {
           try
           {
               DataRow dr = testData.Tables[sheetIndexToName[0]].Rows[0];

             
                   GroupLevel.Click(MouseButtons.Left);
               
                   
               if (!dr[TestDataConstants.COL_TRADE_ATTRIBUTE1].ToString().Equals(String.Empty))
               {
                   if (!TradeAttribute1.IsChecked)
                       TradeAttribute1.Click(MouseButtons.Left);
                   TextBoxPresenter1.Click(MouseButtons.Left);
                   ExtentionMethods.CheckCellValueConditions(dr[TestDataConstants.COL_TRADE_ATTRIBUTE1].ToString(), string.Empty, true);
                   Keyboard.SendKeys(KeyboardConstants.TABKEY);
               }

               if (!dr[TestDataConstants.COL_TRADE_ATTRIBUTE2].ToString().Equals(String.Empty))
               {
                   if (!TradeAttribute2.IsChecked)
                       TradeAttribute2.Click(MouseButtons.Left);
                   TextBoxPresenter2.Click(MouseButtons.Left);
                   ExtentionMethods.CheckCellValueConditions(dr[TestDataConstants.COL_TRADE_ATTRIBUTE2].ToString(), string.Empty, true);
                   Keyboard.SendKeys(KeyboardConstants.TABKEY);
               }

               if (!dr[TestDataConstants.COL_TRADE_ATTRIBUTE3].ToString().Equals(String.Empty))
               {
                   if (!TradeAttribute3.IsChecked)
                       TradeAttribute3.Click(MouseButtons.Left);
                   TextBoxPresenter3.Click(MouseButtons.Left);
                   ExtentionMethods.CheckCellValueConditions(dr[TestDataConstants.COL_TRADE_ATTRIBUTE3].ToString(), string.Empty, true);
                   Keyboard.SendKeys(KeyboardConstants.TABKEY);
               }

               if (!dr[TestDataConstants.COL_TRADE_ATTRIBUTE4].ToString().Equals(String.Empty))
               {
                   if (!TradeAttribute4.IsChecked)
                       TradeAttribute4.Click(MouseButtons.Left);
                   TextBoxPresenter4.Click(MouseButtons.Left);
                   ExtentionMethods.CheckCellValueConditions(dr[TestDataConstants.COL_TRADE_ATTRIBUTE4].ToString(), string.Empty, true);
                   Keyboard.SendKeys(KeyboardConstants.TABKEY);
               }

               if (!dr[TestDataConstants.COL_TRADE_ATTRIBUTE5].ToString().Equals(String.Empty))
               {
                   if (!TradeAttribute5.IsChecked)
                       TradeAttribute5.Click(MouseButtons.Left);
                   TextBoxPresenter5.Click(MouseButtons.Left);
                   ExtentionMethods.CheckCellValueConditions(dr[TestDataConstants.COL_TRADE_ATTRIBUTE5].ToString(), string.Empty, true);
                   Keyboard.SendKeys(KeyboardConstants.TABKEY);
               }

               if (!dr[TestDataConstants.COL_TRADE_ATTRIBUTE6].ToString().Equals(String.Empty))
               {
                   if (!TradeAttribute6.IsChecked)
                       TradeAttribute6.Click(MouseButtons.Left);
                   TextBoxPresenter6.Click(MouseButtons.Left);
                   ExtentionMethods.CheckCellValueConditions(dr[TestDataConstants.COL_TRADE_ATTRIBUTE6].ToString(), string.Empty, true);
                   Keyboard.SendKeys(KeyboardConstants.TABKEY);
               }

               Update.Click(MouseButtons.Left);
            

           }
           catch (Exception)
           {
               throw;
           }
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
    

