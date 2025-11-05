using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Steps.Simulator;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nirvana.TestAutomation.Utilities;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.Utilities.Constants;
using System.Diagnostics;
using System.Xml.Linq;
using System.Xml;
using System.IO;

namespace Nirvana.TestAutomation.Steps.Simulator
{
    public class SaveTradeData : CameronSimulator, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                if (!string.IsNullOrEmpty(testData.Tables[0].Rows[0]["ClearXMLData"].ToString())) 
                {
                    ClearSimulatorData();
                }
                testData.Tables[0].Columns.Remove("ClearXMLData");
                SaveData(testData);

            }
            catch (Exception ex)
            {
                _res.ErrorMessage = ex.Message;
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return _res;
        }
        public void SaveData(DataSet LiveData)
        {
            try
            {
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

                        if (xml1.SelectSingleNode("NewDataSet/CreateNewSubLiveOrder") != null || xml1.SelectSingleNode("NewDataSet/CreateLiveOrder") != null || xml1.SelectSingleNode("NewDataSet/CreateReplaceLiveOrder") != null || xml1.SelectSingleNode("NewDataSet/SendOrderUsingMTT") != null)
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
