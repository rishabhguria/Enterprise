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

namespace Nirvana.TestAutomation.Steps.TradingTicket
{
    class CreateComplianceOrder : TradingTicketUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                Boolean isLiveOrder = false;
                if (!testData.Tables[0].Rows[0][TestDataConstants.COL_Btn].ToString().Equals("Replace"))
                OpenManualTradingTicket();
                if (testData != null)
                {
                    foreach (DataRow dr in testData.Tables[0].Rows)
                    {
                        if (!testData.Tables[0].Rows[0][TestDataConstants.COL_Btn].ToString().Equals("Replace"))
                        {
                            InputDetails(dr, BtnDoneAway);
                        }
                        else
                            InputDetails(dr, BtnReplace);
                        Wait(3000);
                        if (dr[TestDataConstants.COL_Btn].ToString() != String.Empty)
                        {
                            if (dr[TestDataConstants.COL_Btn].ToString().Equals("DoneAway"))
                                BtnDoneAway.Click(MouseButtons.Left);
                            else if (dr[TestDataConstants.COL_Btn].ToString().Equals("CreateOrder"))
                                BtnCreateOrder.Click(MouseButtons.Left);
                            else if (dr[TestDataConstants.COL_Btn].ToString().Equals("Replace"))
                                BtnReplace.Click(MouseButtons.Left);
                            else if (dr[TestDataConstants.COL_Btn].ToString().Equals("Send"))
                            {
                                BtnSend.Click(MouseButtons.Left);
                                isLiveOrder = true;
                            }
                        }
                        Wait(2000);
                        if (TradingTicket1.IsValid)
                        {
                            if (TradingTicket2.IsVisible)
                            {
                                ButtonYes1.Click(MouseButtons.Left);
                            }
                            Wait(2000);
                            if (TradingTicket1.IsValid)
                            {
                                if (uiWindow1.IsVisible)
                                {
                                    ButtonOK2.Click(MouseButtons.Left);
                                }
                            }
                            if (dr.Table.Columns.Contains(TestDataConstants.Col_Shares_Outstanding_PopUp_Qty) && (dr.Table.Columns.Contains(TestDataConstants.Col_Shares_Outstanding_PopUp)))
                            {
                                if (PromptWindow_Fill_Panel.IsVisible)
                                {
                                    if (dr[TestDataConstants.Col_Shares_Outstanding_PopUp_Qty].ToString() != String.Empty && NmrcTargetQuantity1.IsEnabled)
                                    {
                                        NmrcTargetQuantity1.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.Col_Shares_Outstanding_PopUp_Qty].ToString();
                                        BtnPlace.Click(MouseButtons.Left);
                                    }
                                }
                            }
                            if (TradingTicket1.IsValid)
                            {
                                if (TradingRulesViolatedPopUp.IsVisible)
                                {
                                    if (dr.Table.Columns.Contains(TestDataConstants.Col_Shares_Outstanding_PopUp))
                                        if (dr[TestDataConstants.Col_Shares_Outstanding_PopUp].Equals("No"))
                                        {
                                            UltraButtonNo.Click(MouseButtons.Left);
                                        }
                                        else
                                        {
                                            UltraButtonYes.Click(MouseButtons.Left);
                                        }
                                    else
                                    {
                                        UltraButtonYes.Click(MouseButtons.Left);
                                    }
                                }
                            }
                            if (TradingTicket1.IsValid)
                            {
                                if (popUpWindow != null && popUpWindow.IsVisible)
                                {
                                    string error = popUpWindow.MsaaObject.CachedChildren[2].ToString();
                                    ButtonOK1.Click(MouseButtons.Left);
                                    if (!error.Contains("Trading Tool"))
                                    {
                                        _result.ErrorMessage = error.ToString();
                                    }
                                }
                                if (PromptWindow_Fill_Panel.IsVisible)
                                {
                                    if (dr[TestDataConstants.COL_EditOrder].ToString() != String.Empty && dr[TestDataConstants.COL_EditOrder].ToString().Equals("TRUE", StringComparison.InvariantCultureIgnoreCase))
                                    {
                                        BtnEdit.Click(MouseButtons.Left);
                                    }
                                    else if (dr.Table.Columns.Contains(TestDataConstants.Col_Res_Allowed_List_PopUp))
                                    {
                                        if (dr[TestDataConstants.Col_Res_Allowed_List_PopUp].ToString().Equals("No"))
                                        {
                                            BtnEdit.Click(MouseButtons.Left);
                                        }
                                        else
                                        {
                                            BtnPlace.Click(MouseButtons.Left);
                                        }
                                    }
                                    else
                                    {
                                        BtnPlace.Click(MouseButtons.Left);
                                    }
                                }
                                Wait(3000);
                            }
                        }
                    }
                    if (isLiveOrder)
                    SaveData(testData);
                }
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "CreateComplianceOrder");
                _result.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return _result;
        }

        public void SaveData(DataSet LiveData)
        {
            try
            {
                Console.Write("Entered save data method");
                //Code Change : 07/19/2018 
                //Author : Sumit Chand
                //Comment : SimulatorData folder if doesn't exist must be created and LiveTrades.xml file should be copied
                LiveData.Tables[0].TableName = "CreateLiveOrder";
                if (!Directory.Exists("SimulatorData"))
                    Directory.CreateDirectory("SimulatorData");
                if (File.Exists(@"SimulatorData/LiveTrades.xml"))
                {
                    try
                    {
                        XmlDocument xml1 = new XmlDocument();
                        XmlDocument xml2 = new XmlDocument();
                        xml1.Load(@"SimulatorData/LiveTrades.xml");

                        if (xml1.SelectSingleNode("NewDataSet/CreateLiveOrder") != null)
                        {
                            xml2.LoadXml(LiveData.GetXml());
                            foreach (XmlNode list in xml2.SelectSingleNode("NewDataSet").ChildNodes)
                                xml1.SelectSingleNode("NewDataSet").AppendChild(xml1.ImportNode(list, true));
                            xml1.Save(@"SimulatorData/LiveTrades.xml");
                        }
                        else
                        {
                            LiveData.WriteXml(@"SimulatorData/LiveTrades.xml");
                        }
                    }
                    catch { LiveData.WriteXml(@"SimulatorData/LiveTrades.xml"); }
                }
                else
                    LiveData.WriteXml(@"SimulatorData/LiveTrades.xml");
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
        }
    }
}
