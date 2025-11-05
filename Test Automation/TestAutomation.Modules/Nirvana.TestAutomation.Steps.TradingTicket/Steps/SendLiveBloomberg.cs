using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Windows.Forms;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using System.Xml.Linq;
using System.Xml;

namespace Nirvana.TestAutomation.Steps.TradingTicket
{
    public class SendLiveBloomberg : TradingTicketUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                CopyTTGeneralPref();
                OpenGeneralPreferences();
                CmbSymbology.Properties[TestDataConstants.TEXT_PROPERTY] = "Bloomberg Symbol";
                BtnSave.Click(MouseButtons.Left);
                Wait(5000);
                BtnClose.Click(MouseButtons.Left);
                OpenManualTradingTicket();
                //UltraComboEditorSymbology.Properties[TestDataConstants.TEXT_PROPERTY] = "Bloomberg Symbol";
                //Wait(2000);
                if (testData != null)
                {
                    foreach (DataRow dr in testData.Tables[0].Rows)
                    {
                        InputDetails(dr, BtnDoneAway);
                        Wait(3000);
                        BtnSend.Click(MouseButtons.Left);
                        Wait(1000);

                        if (TradingTicket1.IsValid)
                        {
                            if (uiWindow1.IsVisible)
                            {
                                ButtonOK2.Click(MouseButtons.Left);
                            }
                        }
                        if (TradingTicket1.IsValid)
                        {
                            if (PromptWindow_Fill_Panel.IsVisible)
                            {
                                if (dr.Table.Columns.Contains(TestDataConstants.Col_Res_Allowed_List_PopUp))
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
                            }

                        }
                        if (TradingTicket1.IsValid)
                        {
                            if (TradingRulesViolatedPopUp.IsVisible)
                            {
                                UltraButtonYes.Click(MouseButtons.Left);
                            }
                        }
                    }
                    SaveData(testData);
                }

                string DefaultSymbologySourceNewFile = ConfigurationManager.AppSettings["DefaultSymbologySourceNewFile"];
                if (File.Exists(DefaultSymbologySourceNewFile))
                {
                    RevertTTGenPref();
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
                CloseTradingTicket();
                string DefaultSymbologySourceNewFile = ConfigurationManager.AppSettings["DefaultSymbologySourceNewFile"];
                if (File.Exists(DefaultSymbologySourceNewFile))
                {
                    RevertTTGenPref();
                }
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