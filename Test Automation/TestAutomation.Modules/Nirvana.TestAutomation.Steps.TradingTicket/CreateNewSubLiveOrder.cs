using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Nirvana.TestAutomation.Steps.TradingTicket
{
    public class CreateNewSubLiveOrder : TradingTicketUIMap, ITestStep
    {

        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                ExtentionMethods.WaitForVisible(ref TradingTicket1, 20);
                if (TradingTicket1.IsVisible)
                {
                    if (testData != null)
                    {
                        foreach (DataRow dr in testData.Tables[0].Rows)
                        {
                            InputDetails(dr, BtnSend);
                            BtnSend.Click(MouseButtons.Left);
                            
                            if (TradingTicket1.IsValid)
                            {
                                if (TradingTicket2.IsVisible)
                                {
                                    ButtonYes1.Click(MouseButtons.Left);
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
                            if (PromptWindow_Fill_Panel.IsVisible)
                            {
                                BtnPlace.Click(MouseButtons.Left);
                            }
                            if (uiWindow1.IsVisible)
                            {
                                ButtonOK2.Click();
                            }
                            if (UltraButtonYes1.IsVisible)// handling for AUEC broker mapping popup.
                            {
                                UltraButtonYes1.Click(MouseButtons.Left);
                            }
                            Wait(1000);
                            if (dr.Table.Columns.Contains(TestDataConstants.COL_ALLOW_TRADE))
                            {
                                if (dr[TestDataConstants.COL_ALLOW_TRADE].ToString() != String.Empty)
                                {
                                    CheckCompliance(dr);
                                }
                            }
                        }
                        SaveData(testData);
                    }
                }
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "CreateNewSubLiveOrder");
                _result.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return _result;
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

        // Save the multiple trades as it was overwriting the data previously
        public void SaveData(DataSet LiveData)
        {
            try
            {
                //Code Change : 07/19/2018 
                //Author : Sumit Chand
                //Comment : SimulatorData folder if doesn't exist must be created and LiveTrades.xml file should be copied
                LiveData.Tables[0].TableName = "CreateNewSubLiveOrder";
                if (!Directory.Exists("SimulatorData"))
                    Directory.CreateDirectory("SimulatorData");
                if (File.Exists(@"SimulatorData/LiveTrades.xml"))
                {
                    try
                    {
                        XmlDocument xml1 = new XmlDocument();
                        XmlDocument xml2 = new XmlDocument();
                        xml1.Load(@"SimulatorData/LiveTrades.xml");

                        if (xml1.SelectSingleNode("NewDataSet/CreateNewSubLiveOrder") != null || xml1.SelectSingleNode("NewDataSet/CreateLiveOrder") != null || xml1.SelectSingleNode("NewDataSet/CreateReplaceLiveOrder") != null ||  xml1.SelectSingleNode("NewDataSet/SendOrderUsingMTT") != null)
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
