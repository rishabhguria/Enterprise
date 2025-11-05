using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml;
using TestAutomationFX.Core;
using System.Reflection;
using System.Globalization;
using System.Configuration;


namespace Nirvana.TestAutomation.Steps.TradingTicket
{
    class VerifyComplianceOrder : TradingTicketUIMap, ITestStep
    {
        private static string Trade = string.Empty;
        
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                Boolean NirvanaCompliance = false;
                string PopUpVal = "FALSE";
                Boolean flag = false;
                DataTable Verifydata = new DataTable();
                Verifydata = testData.Tables[0].Copy();
                Verifydata.Columns.Remove("Write UserNote");
                Verifydata.Columns.Remove("AllowTrade");
                Verifydata.Columns.Remove("ClickonMultiple");
                if (Verifydata.Columns.Contains("PendingApprovalPopUp")) {
                    Verifydata.Columns.Remove("PendingApprovalPopUp");
                }
                if (Verifydata.Columns.Contains("VerifyExportState") && !string.IsNullOrEmpty(Verifydata.Rows[0]["VerifyExportState"].ToString())) 
                {
                    DataTable dt = SqlUtilities.GetDataFromQuery("SELECT * FROM T_CompanyMarketDataProvider where CompanyId>0", "Client");                    
                    bool ExportState = true;
                    foreach (DataRow dr in dt.Rows) {
                        if (dr["IsMarketDataBlocked"].ToString().ToUpper().Equals("TRUE"))
                        {
                            ExportState = false;
                        }
                    }
                    if (ComplianceAlertPopupUC2.IsVisible) 
                    {
                        if (UltraPanelBottom.IsVisible) {
                            if (ExportButton.IsEnabled.Equals(ExportState))
                            {
                                Console.WriteLine("Export button state verified");
                            }
                            else {
                                throw new Exception("Export button availablity is " + ExportButton.IsEnabled.ToString() + " but state is " + ExportState);
                            }
                        }
                        else if (UltraPanelBottom1.IsVisible) {
                            if (ExportButton1.IsEnabled.Equals(ExportState))
                            {
                                Console.WriteLine("Export button state verified");
                            }
                            else
                            {
                                throw new Exception("Export button availablity is " + ExportButton.IsEnabled.ToString() + " but Excel has " + ExportState);
                            }
                        }
                        Wait(3000);
                        ResponseButton.DoubleClick(MouseButtons.Left);
                    }
                    else if (ComplianceAlertPopupUC1.IsVisible) 
                    {
                        if (UltraPanelBottom2.IsVisible)
                        {
                            if (ExportButton3.IsEnabled.Equals(ExportState))
                            {
                                Console.WriteLine("Export button state verified");
                            }
                            else
                            {
                                throw new Exception("Export button availablity is " + ExportButton.IsEnabled.ToString() + " but state is " + ExportState);
                            }
                        }
                        Wait(3000);
                        ResponseButton3.DoubleClick(MouseButtons.Left);
                    }
                    return _res;
                }
                else if (Verifydata.Columns.Contains("VerifyExportState")) {
                    Verifydata.Columns.Remove("VerifyExportState");                    
                }
                Verifydata = DataUtilities.RemoveColumnsAndRows("Description of Rule", Verifydata);
                List<String> errors = VerifyCompliancePopUp(Verifydata);
                // DataTable WithoutColumn = testData.Tables[0];// to remove unwanted columns during verification
                //List<String> errors = VerifyCompliancePopUp(testData.Tables[0]);
                if (errors.Count > 0)
                {
                    _res.ErrorMessage = String.Join("\n\r", errors);
                }
                else
                {
                    //
                    if (!string.IsNullOrEmpty(Trade)) {
                        testData.Tables[0].Rows[0]["Trade"] = Trade;
                    }
                    UpdateSoftNotes(testData.Tables[0]);
                    ClickonMultiple(testData.Tables[0].Rows[0]);
                    CheckComplianceOrder(testData.Tables[0].Rows[0], Verifydata.Rows.Count);// in case of hard alert type..need to provide all hard rule names along with data in excel


                    if (testData.Tables[0].Columns.Contains(TestDataConstants.PendingApprovalPopUp))
                    {
                        flag = true;
                    }

                    ///Handling for Nirvana Compliance(can check only one row)but incase in future Requires Approval may get low priority and occur at lower row 
                    foreach (DataRow dr in testData.Tables[0].Rows)
                    {
                        if (dr[TestDataConstants.COL_ALERT_TYPE].ToString().ToUpper() == "REQUIRES APPROVAL")
                        {
                            NirvanaCompliance = true;
                        }
                        if (flag)
                        {
                            if (dr[TestDataConstants.PendingApprovalPopUp].ToString().ToUpper() == "TRUE")
                            {
                                PopUpVal = "TRUE";
                            }
                        }

                    }
                    if (NirvanaCompliance == true)
                    {
                        Wait(TestDataConstants.COMPLIANCE_PROCESS_WAIT);
                        if (flag)
                        {
                            if (PopUpVal == "TRUE")
                            {
                                ButtonYes2.Click();
                            }
                            else
                            {
                                ButtonNo.Click();
                            }
                        }
                        else
                        {
                            Keyboard.SendKeys(KeyboardConstants.TABKEY);
                            Keyboard.SendKeys(KeyboardConstants.SPACE);
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "VerifyComplianceOrder");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {
                string tradee = testData.Tables[0].Rows[0][TestDataConstants.COL_TRADEORDER].ToString();
                if (testData.Tables[0].Columns.Contains("VerifyExportState") && !string.IsNullOrEmpty(testData.Tables[0].Rows[0]["VerifyExportState"].ToString())) {
                    Console.WriteLine("Export button verification case");
                } else if (tradee != "Replace") {
                    if (TradingTicket1.IsEnabled)
                    {
                        CloseTradingTicket();
                    }
                }
            }
            return _res;
        }

        public List<String> VerifyCompliancePopUp(DataTable dTable)
        {
            DataTable dtCompliancePopUp = new DataTable();
            Wait(10000);
            if (string.IsNullOrEmpty(dTable.Rows[0][TestDataConstants.COL_TRADEORDER].ToString()))
            {
                
                if (ComplianceAlertPopupUC1.IsVisible || ComplianceAlertPopupUC1.IsEnabled)
                {
                    dTable.Rows[0][TestDataConstants.COL_TRADEORDER] = "CreateOrder";
                    Trade = "CreateOrder";
                }else if (ComplianceAlertPopupUC2.IsVisible || ComplianceAlertPopupUC2.IsEnabled)
                {
                    dTable.Rows[0][TestDataConstants.COL_TRADEORDER] = "Replace";
                    Trade = "Replace";
                }else if (ComplianceAlertPopUp.IsVisible || ComplianceAlertPopUp.IsEnabled)
                {
                    dTable.Rows[0][TestDataConstants.COL_TRADEORDER] = "DoneAway";
                    Trade = "DoneAway";                
                } 
            
            }
            if (dTable.Rows[0][TestDataConstants.COL_TRADEORDER].ToString().Equals("Replace") || dTable.Rows[0][TestDataConstants.COL_TRADEORDER].ToString().Equals("TradeNewSub"))
            {

                dtCompliancePopUp = ReturnExportedData(dTable);

            }
            if (dTable.Rows[0][TestDataConstants.COL_TRADEORDER].ToString().Equals("DoneAway") || dTable.Rows[0][TestDataConstants.COL_TRADEORDER].ToString().Equals("Send"))
            {
                if (AlertPopupGridCompliance.IsEnabled)
                {
                    int index = AlertPopupGridCompliance.MatchedIndex;
                }
                AlertPopupGridCompliance.MatchedIndex = 0;
                AlertPopupGridCompliance.Click();

                dtCompliancePopUp = DataUtilities.RemoveTrailingZeroes(CSVHelper.CSVAsDataTable(this.AlertPopupGridCompliance.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString()));

            }
            if (dTable.Rows[0][TestDataConstants.COL_TRADEORDER].ToString().Equals("CreateOrder"))
            {
                AlertPopupGridCompliance1.Click();
                dtCompliancePopUp = DataUtilities.RemoveTrailingZeroes(CSVHelper.CSVAsDataTable(this.AlertPopupGridCompliance1.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString()));
            }
            dTable.Columns.Remove("Trade");
            List<String> columns = new List<string>();
            columns.Add("Rule Name");

            try
            {
                string StepName = "VerifyComplianceOrder";
                DataSet columMapDs = DataUtilities.GetTestCaseTestData(ApplicationArguments.columnMappingFile, 1, 1, columns);
                Nirvana.TestAutomation.Utilities.SamsaraCustomizableVerificationHandler.LinkExcelData(ref dtCompliancePopUp);
                SamsaraCustomizableVerificationHandler.CustomizableVerificationHandler(ref StepName, columMapDs.Tables["VerificationHandlerOnEnterprise"], ref dTable);
            }
            catch (Exception)
            { }

            List<String> errors = Recon.RunRecon(dtCompliancePopUp, dTable, columns, 0.01);
            return errors;
        }
        
        public void ClickonMultiple(DataRow dr)
        {

            try
            {
                if (!String.IsNullOrWhiteSpace(dr[TestDataConstants.COL_CLICKONMUL].ToString()))
                {
                    if (dr[TestDataConstants.COL_TRADEORDER].ToString().Equals("Replace") || dr[TestDataConstants.COL_TRADEORDER].ToString().Equals("DoneAway") || dr[TestDataConstants.COL_TRADEORDER].ToString().Equals("Send"))
                    {
                        if (ComplianceAlertPopupUC2.IsVisible || ComplianceAlertPopupUC2.IsEnabled)
                        {

                            AlertPopupGridCompliance.Click();


                            string clickonmultiple = dr[TestDataConstants.COL_ALLOW_TRADE].ToString();

                            if (clickonmultiple.Equals("Yes", StringComparison.CurrentCultureIgnoreCase))
                            {
                                Threshold1.Click();
                                if (ThresholdandActualResult2.IsEnabled)
                                {
                                    ThresholdandActualResult2.Click();
                                    // DockTop.Click(MouseButtons.Right);
                                    KeyboardUtilities.CloseWindow(ref DockTop2);
                                    Wait(TestDataConstants.COMPLIANCE_PROCESS_WAIT);
                                }


                            }
                        }
                    }
                    if (dr[TestDataConstants.COL_TRADEORDER].ToString().Equals("CreateOrder"))
                    {

                        if (AlertPopupGridCompliance1.IsEnabled)
                        {
                            AlertPopupGridCompliance1.Click();
                        }

                        string clickonmultiple = dr[TestDataConstants.COL_ALLOW_TRADE].ToString();

                        if (clickonmultiple.Equals("Yes", StringComparison.CurrentCultureIgnoreCase))
                        {
                            Threshold.Click();
                            if (ThresholdandActualResult.IsEnabled)
                            {
                                ThresholdandActualResult.Click();
                                // DockTop.Click(MouseButtons.Right);
                                KeyboardUtilities.CloseWindow(ref DockTop);
                                Wait(TestDataConstants.COMPLIANCE_PROCESS_WAIT);
                            }


                        }

                    }
                }

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }



        }

        public void UpdateSoftNotes(DataTable dTable)
        {

            IDictionary<int, string> Usn = new Dictionary<int, string>();
            int i = 0, count = 0;
            foreach (DataRow dt in dTable.Rows)
            {
                if (dt[TestDataConstants.COL_ALERT_TYPE].ToString() == "Soft with Notes" && dt[TestDataConstants.USERNOTES_YESORNO].ToString() == "Yes")
                {
                    Usn.Add(i, dt[TestDataConstants.USERNOTES_YESORNO].ToString());
                    count++;
                }
                i++;
            }
            if (count > 0)
            {
                int k = 0;
                if (dTable.Rows[0][TestDataConstants.COL_TRADEORDER].ToString().Equals("Replace") || dTable.Rows[0][TestDataConstants.COL_TRADEORDER].ToString().Equals("DoneAway") || dTable.Rows[0][TestDataConstants.COL_TRADEORDER].ToString().Equals("Send"))
                {
                    if (ComplianceAlertPopupUC2.IsVisible || ComplianceAlertPopupUC2.IsEnabled)
                    {
                        AlertPopupGridCompliance.Click();


                        DataTable dtCompliancePopUp = DataUtilities.RemoveTrailingZeroes(CSVHelper.CSVAsDataTable(this.AlertPopupGridCompliance.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString()));

                        foreach (DataRow dtrow in dtCompliancePopUp.Rows)
                        {


                            if (dtrow[TestDataConstants.COL_ALERT_TYPE].ToString() == "Soft with Notes" && Usn[k] == "Yes")
                            {
                                if (k == 0)
                                {
                                    UserNotes1.Click();
                                    UserNotes1.Click();
                                }
                                else if (k == 1)// code will only work till 2 rows if having user with Soft with Notes
                                {
                                    UserNotes2.Click();
                                    UserNotes2.Click();

                                }
                                else
                                {
                                    UserNotes5.Click();
                                    UserNotes5.Click();

                                }
                                Keyboard.SendKeys(TestDataConstants.USERNOTES);
                                k++;
                            }

                            //user opt not to write anything on soft  notes in usernotes section
                            k++;
                        }
                    }


                }

                if (dTable.Rows[0][TestDataConstants.COL_TRADEORDER].ToString().Equals("CreateOrder"))
                {
                    if (ComplianceAlertPopupUC1.IsVisible || ComplianceAlertPopupUC1.IsEnabled)
                    {
                        AlertPopupGridCompliance1.Click();
                        DataTable dtCompliancePopUp = DataUtilities.RemoveTrailingZeroes(CSVHelper.CSVAsDataTable(this.AlertPopupGridCompliance1.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString()));

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
                                    UserNotes3.Click();
                                    UserNotes3.Click();

                                }
                                else
                                {
                                    UserNotes4.Click();
                                    UserNotes4.Click();
                                }
                                Keyboard.SendKeys(TestDataConstants.USERNOTES);
                                k++;
                            }

                            //user opt not to write anything on soft  notes in usernotes section
                            k++;
                        }
                    }

                }
            }
        }
        
        public void CheckComplianceOrder(DataRow dr, int rowcount)
        {
            try
            {
                string allowTrade = string.Empty;
                string alertt = string.Empty;

                if (dr[TestDataConstants.COL_TRADEORDER].ToString().Equals("Replace") || dr[TestDataConstants.COL_TRADEORDER].ToString().Equals("DoneAway") || dr[TestDataConstants.COL_TRADEORDER].ToString().Equals("Send") || dr[TestDataConstants.COL_TRADEORDER].ToString().Equals("TradeNewSub"))
                {
                    if (ComplianceAlertPopupUC2.IsVisible || ComplianceAlertPopupUC2.IsEnabled)
                    {
                        if (!String.IsNullOrWhiteSpace(dr[TestDataConstants.COL_ALLOW_TRADE].ToString()))
                            allowTrade = dr[TestDataConstants.COL_ALLOW_TRADE].ToString();

                        bool ishard = false;

                        if (dr[TestDataConstants.COL_ALERT_TYPE].ToString().ToUpper() == "HARD" && dr[TestDataConstants.COL_TRADEORDER].ToString().ToUpper() == "REPLACE")
                        {
                            ishard = true;
                        }

                        if (allowTrade.ToUpper().Equals("NO", StringComparison.CurrentCultureIgnoreCase) || allowTrade.Equals(String.Empty, StringComparison.CurrentCultureIgnoreCase) || allowTrade.ToUpper().Equals("FALSE", StringComparison.CurrentCultureIgnoreCase) || allowTrade.ToUpper().Equals("CANCEL", StringComparison.CurrentCultureIgnoreCase))
                        {
                            if (CancelButton1.IsVisible || CancelButton1.IsEnabled)
                            {
                                CancelButton1.DoubleClick(MouseButtons.Left);
                            }
                            else
                            {
                                CancelButton.DoubleClick(MouseButtons.Left);
                            }
                            Wait(TestDataConstants.COMPLIANCE_PROCESS_WAIT);
                        }
                        else 
                        {
                            if (ishard == true)
                            {
                                // AlertPopupGridCompliance.Click();

                                /*while (rowcount != 0)
                                {
                                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                                    rowcount--;
                                }*/
                                Keyboard.SendKeys(KeyboardConstants.LEFT_ARROWKEY);
                                Keyboard.SendKeys(KeyboardConstants.SPACE);

                            }

                            else if (ishard == false && (ResponseButton.IsVisible || ResponseButton.IsEnabled))
                            {
                                ResponseButton.DoubleClick(MouseButtons.Left);
                            }
                            else
                            {
                                ResponseButton1.DoubleClick(MouseButtons.Left);
                            }
                            Wait(TestDataConstants.COMPLIANCE_PROCESS_WAIT);
                        }
                    }
                }

                if (dr[TestDataConstants.COL_TRADEORDER].ToString().Equals("CreateOrder"))
                {
                    if (ComplianceAlertPopupUC1.IsVisible || ComplianceAlertPopupUC1.IsEnabled)
                    {
                        if (!String.IsNullOrWhiteSpace(dr[TestDataConstants.COL_ALLOW_TRADE].ToString()))
                            allowTrade = dr[TestDataConstants.COL_ALLOW_TRADE].ToString();
                         if (allowTrade.ToUpper().Equals("NO", StringComparison.CurrentCultureIgnoreCase) || allowTrade.Equals(String.Empty, StringComparison.CurrentCultureIgnoreCase) || allowTrade.ToUpper().Equals("FALSE", StringComparison.CurrentCultureIgnoreCase) || allowTrade.ToUpper().Equals("CANCEL", StringComparison.CurrentCultureIgnoreCase))
                        {
                            CancelButton3.DoubleClick(MouseButtons.Left);
                            Wait(TestDataConstants.COMPLIANCE_PROCESS_WAIT);
                        }else
                        {
                            ResponseButton3.DoubleClick(MouseButtons.Left);
                            Wait(TestDataConstants.COMPLIANCE_PROCESS_WAIT);
                        }
                    }
                }



            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
        }

        public DataTable ReturnExportedData(DataTable dt)
        {
            try
            {
                int trycount = 0, tryexport = 0;
                string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + TestDataConstants.CAP_AUTOMATION_FOLDER + @"\";
                // Wait(5000);
                DataTable dtExportedTrades = null;
                bool ishard = false;
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr[TestDataConstants.COL_ALERT_TYPE].ToString().ToUpper() == "HARD")
                    {
                        ishard = true;
                        break;
                    }
                }


                tryexport = Doexportandreturncount(trycount, path, ishard, dt.Rows.Count);
                trycount = tryexport;
                DataSet testCases;
                Wait(6000);
                bool checkiffileexist = fileexist(path);
                // to check if export done correctly or not

                if (tryexport < 2 && (checkiffileexist == false))
                {
                    tryexport = Doexportandreturncount(trycount, path, ishard, dt.Rows.Count);
                    checkiffileexist = fileexist(path);
                    Console.WriteLine("exported file found");
                }

                if (checkiffileexist == true)
                {
                    Console.WriteLine("exported file found");
                    ITestDataProvider provider = Factory.TestDataProvider.GetProvider(ProviderType.Xls);
                    testCases = provider.GetTestData(path + @"\" + ExcelStructureConstants.VerifyCompExportFileName);
                    dtExportedTrades = testCases.Tables[0];
                    //     dtExportedTrades = testCases.Tables["AlertsList"];
                    return dtExportedTrades;
                }


                return dtExportedTrades;
            }

            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
                return null;
            }


        }
        
        public bool fileexist(string path)
        {
            if (File.Exists(path + ExcelStructureConstants.VerifyCompExportFileName))
                return true;
            else
                return false;
        }

        public int Doexportandreturncount(int trycount, string path, bool ishard, int rowcount)
        {
            try
            {
                // Wait(5000);
                if (ishard == true)
                {
                    if (trycount < 1)
                    {
                        while (rowcount + 1 != 0)
                        {
                            Keyboard.SendKeys(KeyboardConstants.TABKEY);
                            rowcount--;
                        }
                        Keyboard.SendKeys(KeyboardConstants.SPACE);
                    }
                }
                else if (trycount < 1)
                {
                    ExportButton1.Click(MouseButtons.Left);
                }

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                //handling for if file exist..delete it first
                bool present = (File.Exists(path + ExcelStructureConstants.VerifyCompExportFileName));

                if (File.Exists(path + ExcelStructureConstants.VerifyCompExportFileName))
                {
                    if (File.Exists(path + "bak" + ExcelStructureConstants.VerifyCompExportFileName))
                    {
                        File.Delete(path + "bak" + ExcelStructureConstants.VerifyCompExportFileName);// del old bak file

                    }
                    File.Move(path + ExcelStructureConstants.VerifyCompExportFileName, path + "bak" + ExcelStructureConstants.VerifyCompExportFileName);

                }

                Clipboard.SetText(path + ExcelStructureConstants.VerifyCompExportFileName);
                Keyboard.SendKeys("[CTRL+V]");

                Keyboard.SendKeys(KeyboardConstants.ENTERKEY);//save
                // ButtonSave1.Click(MouseButtons.Left);
                Wait(4000);

                Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                return trycount + 1;
            }


            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;

            }
            return (trycount + 1);

        }

    }
}
