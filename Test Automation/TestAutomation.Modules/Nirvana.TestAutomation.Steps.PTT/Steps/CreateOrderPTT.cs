using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Xml;
using TestAutomationFX.Core;
using System.Collections;

namespace Nirvana.TestAutomation.Steps.PTT
{
    class CreateOrderPTT : PTTUIMap, ITestStep
    {
        public TestResult RunTest(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                Boolean NirvanaCompliance = false;
                OpenPTT();
                InputParametersPTT(testData, sheetIndexToName);
                Calculate.Click(MouseButtons.Left);
                Calculate.Click(MouseButtons.Left);
                //Wait(2000);
                CreateOrder.Click(MouseButtons.Left);
                Wait(1000);
                if (ComplianceAlertPopUp.IsVisible)
                {
                    AlertPopupGridCompliance.Click();


                    DataTable dtCompliancePopUp = DataUtilities.RemoveTrailingZeroes(CSVHelper.CSVAsDataTable(this.AlertPopupGridCompliance.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString()));

                    // ExportData();
                    if (AlertPopupGridCompliance.IsEnabled)
                    {

                        HandleSoftNotes(testData.Tables[0], dtCompliancePopUp);

                    }
                    DataRow dr;
                    dr = testData.Tables[0].Rows[0];

                    if (testData.Tables[0].Columns.Contains("DoNotAllowTrade"))
                    {
                        if (ResponseButton.IsVisible && dr[TestDataConstants.COL_TRADENOTALLOW].ToString() == String.Empty)
                        {
                            ResponseButton.Click(MouseButtons.Left);
                        }
                        if (dr[TestDataConstants.COL_TRADENOTALLOW].ToString() != String.Empty)
                        {
                            CancelButton.Click(MouseButtons.Left);
                        }

                    }
                    if (ResponseButton.IsVisible)
                    {
                        ResponseButton.Click(MouseButtons.Left);
                    }

                    if (dr.Table.Columns.Contains(TestDataConstants.COL_ALERT_TYPE))
                    {
                        foreach (DataRow drow in testData.Tables[0].Rows)
                        {
                            if (drow[TestDataConstants.COL_ALERT_TYPE].ToString() == "Requires Approval")
                            {
                                NirvanaCompliance = true;
                                Wait(5000);
                            }
                        }
                        if (NirvanaCompliance == true)
                        {
                            Keyboard.SendKeys(KeyboardConstants.TABKEY);
                            Keyboard.SendKeys(KeyboardConstants.SPACE);

                        }
                    }

                }
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "CreateOrderPTT");
                _result.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }

            //finally
            //{
            //    PercentTradingTool.Click(MouseButtons.Left);
            //    PercentTradingTool.Click(MouseButtons.Right);
            //    KeyboardUtilities.CloseWindow(ref PercentTradingTool);

            //}

            return _result;
        }

        public void HandleSoftNotes(DataTable dTable, DataTable dtCompliancePopUp)
        {
            try
            {
                IDictionary<int, string> Usn = new Dictionary<int, string>();
                int count = 0, softindexes = 0, softcounts = 0;
                DataRow drq = dTable.Rows[0];


                if (dTable.Columns.Contains("SoftNotesCount"))
                {

                    string softcount = drq[TestDataConstants.COL_SOFTNOTESCOUNT].ToString();

                    if (String.IsNullOrEmpty(softcount) == false)
                    {
                        softcounts = Int32.Parse(softcount);
                    }

                    foreach (DataRow dtrow1 in dtCompliancePopUp.Rows)
                    {
                        if (dtrow1[TestDataConstants.COL_ALERT_TYPE].ToString() == "Soft with Notes")
                        {
                            Usn.Add(softindexes, "Yes");
                        }
                        softindexes++;
                    }

                    while (softcounts > 0)
                    {
                        count++;
                        softcounts--;
                    }

                    if (count > 0)
                    {
                        int k = 0;
                        if (ComplianceAlertPopUp.IsVisible || ComplianceAlertPopUp.IsEnabled)
                        {
                            AlertPopupGridCompliance.Click();


                            // DataTable dtCompliancePopUp = DataUtilities.RemoveTrailingZeroes(CSVHelper.CSVAsDataTable(this.AlertPopupGridCompliance.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString()));

                            foreach (DataRow dtrow in dtCompliancePopUp.Rows)
                            {


                                if (dtrow[TestDataConstants.COL_ALERT_TYPE].ToString() == "Soft with Notes" && Usn[k] == "Yes")
                                {
                                    if (k == 0)
                                    {
                                        UserNotes.Click();
                                        UserNotes.Click();
                                    }
                                    else if (k == 1)// code will only work till 2 rows if having user with Soft with Notes
                                    {
                                        UserNotes1.Click();
                                        UserNotes1.Click();

                                    }
                                    /* else
                                     {
                                         UserNotes5.Click();//USE THIS IF needs to add softwith notes 
                                         UserNotes5.Click();
 
                                     }*/

                                    Keyboard.SendKeys(TestDataConstants.COL_SOFTNOTESHANDLE);
                                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                                    k++;
                                }

                                //user opt not to write anything on soft  notes in usernotes section
                                else
                                    k++;
                            }
                        }

                    }
                }
            }
            catch (Exception) { throw; }
        }
    }
}
