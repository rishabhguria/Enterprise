using System;
using System.ComponentModel;
using System.Data;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using System.Diagnostics;
using System.Windows.Forms;
using System.Xml;
using Nirvana.TestAutomation.Utilities;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using System.Collections.Generic;
using System.IO;
using Nirvana.TestAutomation.Utilities.Constants;
using System.Windows.Automation;
using Nirvana.TestAutomation.Interfaces.Enums;

namespace Nirvana.TestAutomation.Steps.Simulator
{
    [UITestFixture]
    public partial class CameronSimulator : UIMap
    {
        public static void Main(String[] args)
        {
            
        }
        /// <summary>
        /// Initializer
        /// </summary>
        public CameronSimulator()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Components Container
        /// </summary>
        /// <param name="container"></param>
        public CameronSimulator(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }
        /// <summary>
        /// Opening Live TT After Opening 
        /// </summary>      
        public void OpenLiveTT()
        {
            try
            {
                StartFixApplication.Start();
                Config_TT.Click();
                if (aePrana != null)
                {
                    aePrana.SetFocus();
                }
                Wait(10000);
                AccessBridgeHelper.Inititalize();
                Wait(2000);
                AccessBridgeHelper.SendMessage(CameronConstants.buttonCommand, CameronConstants.corButton);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }


        internal AutomationElement aePrana = null;
        public void BringSimToFront()
        {
            try
            {
                AutomationElement aeDesktop = AutomationElement.RootElement;
                if (aeDesktop != null)
                {
                    int numWaits1 = 0;
                    do
                    {
                        aePrana = aeDesktop.FindFirst(TreeScope.Children,
                            new PropertyCondition(AutomationElement.NameProperty, "MS"));
                        ++numWaits1;
                        Wait(200);
                    }
                    while (aePrana == null && numWaits1 < 50);

                    if (aePrana != null)
                    {
                        aePrana.SetFocus();
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

        /// <summary>
        /// Clearing Existing Trade Fro UI 
        /// </summary>
        public void ClearUI()
        {
            try
            {

                Console.WriteLine("Clear UI simulator");
                ClearSimulatorData();
                
                if (aePrana != null)
                {
                    aePrana.SetFocus();
                }
                AccessBridgeHelper.SendMessage(CameronConstants.menuButtonCommand, CameronConstants.clearButton);

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
        }
        /// <summary>
        /// If Directory does not exist then create the directory as well as xml file for live trades.
        /// </summary>
        public void checkSellSide()
        {
            try
            {
                if (SellSideLog.IsEnabled)
                {
                    KeyboardUtilities.CloseWindow(ref TitleBar);
                }
            }
            catch (Exception) { throw; }
        }
        public void ClearSimulatorData()
        {
            try
            {
                string filePath = SimulatorConstants.Simulator_Data;
                string directoryPath = Path.GetDirectoryName(filePath);

                // Ensure the directory exists
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                // Delete the file if it exists
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                // Create a new XML file with the root element
                using (XmlWriter xml = XmlWriter.Create(filePath))
                {
                    xml.WriteStartDocument(true);
                    xml.WriteStartElement("Root");
                }

            

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
        }
        /// <summary>
        /// Closing Simulator UI
        /// </summary>
        public void Close()
        {
            try
            {
                if (aePrana != null)
                {
                    aePrana.SetFocus();
                }
                foreach (var pr in Process.GetProcessesByName("java"))
                {
                    pr.Kill();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
        }
        /// <summary>
        /// To select trades need to press tab key instead of down key directly.
        /// </summary>
        /// <param name="Index"></param>
        public void SelectTrade(int Index)
        {
            try
            {
              //  SikuliAction.Click(SimulatorConstants.Default_Trade);
                for (int i = 0; i < 8; i++)
                    Keyboard.SendKeys("[TAB]");
                for (int counter = 1; counter <= Index; counter++)
                {
                    Keyboard.SendKeys(SimulatorConstants.Next_Trade);
                    Wait(500);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Getting Index For Selecting Trade
        /// </summary>
        /// <param name="Select"></param>
        /// <returns></returns>
        public string GetTradeIndex(DataTable Select)
        {
            try
            {
                if (Directory.Exists("SimulatorData"))
                {
                    DataSet LiveData = new DataSet();
                    LiveData.ReadXml(SimulatorConstants.Simulator_Data);
                    IDictionary<int, string> LiveDataNameIndexMapping = new Dictionary<int, string>();
                    if (LiveData.Tables.Contains("CreateLiveOrder"))
                    {
                        LiveDataNameIndexMapping.Add(LiveData.Tables.IndexOf("CreateLiveOrder"), "CreateLiveOrder");
                    }
                    if (LiveData.Tables.Contains("CreateReplaceLiveOrder"))
                    {
                        LiveDataNameIndexMapping.Add(LiveData.Tables.IndexOf("CreateReplaceLiveOrder"), "CreateReplaceLiveOrder");
                    }
                    if (LiveData.Tables.Contains("CreateNewSubLiveOrder"))
                    {
                        LiveDataNameIndexMapping.Add(LiveData.Tables.IndexOf("CreateNewSubLiveOrder"), "CreateNewSubLiveOrder");
                    }
                     // Add condition for Send Order Using MTT step
                    if (LiveData.Tables.Contains("SendOrderUsingMTT"))
                    {
                        LiveDataNameIndexMapping.Add(LiveData.Tables.IndexOf("SendOrderUsingMTT"), "SendOrderUsingMTT");
                    }
                    bool foundflag = false;
                    int foundrowindex = 0;
                    for (int i = 0; i < LiveData.Tables.Count; i++)
                    {
                        String foundindex = FindIndexRow(Select, LiveData.Tables[i]);
                            if(foundindex == null)
                            {
                                foundrowindex = foundrowindex + LiveData.Tables[0].Rows.Count;
                            }
                            else if(foundindex != null)
                            {
                                 foundrowindex = foundrowindex + Convert.ToInt16(foundindex);
                                foundflag = true;
                                break;
                            }
 
                    }
                    //
                    if(foundflag == false)
                    {
                         throw new Exception("Trades Not Found On Gird : Verify Test Data");
                    }
                    
                    if(foundflag == true )
                   return foundrowindex.ToString();
                                   
                  
                }
                 
            }
            catch (Exception ex)
            {
                 bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return string.Empty;
        }



        public string FindIndexRow(DataTable Select,DataTable LiveDataTable)
        {
         // List<string,int> =
            try
            {
                DataTable dt = LiveDataTable; //.Tables["CreateLiveOrder"];
                if (dt == null)
                    return null;

                /*   dt = LiveData.Tables["CreateReplaceLiveOrder"];
               if(dt == null)
                   dt = LiveData.Tables["CreateNewSubLiveOrder"];
             //  DataTable dt = null;*/

                if (dt.Columns.Contains("Order Side"))
                {
                    dt.Columns["Order Side"].ColumnName = "Side";
                }
                if (!dt.Columns.Contains("AvgPrice"))
                    dt.Columns.Add("AvgPrice", typeof(string));

                foreach (DataRow dr in dt.Rows)
                {
                    foreach (DataRow SelRow in Select.Rows)
                    {
                        DataRow RowToSelect = DataUtilities.GetMatchingDataRow(dt, SelRow, false);

                        {
                            if (RowToSelect != null)
                            {
                                return dt.Rows.IndexOf(RowToSelect).ToString();
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

            return null;
        }
        /// <summary>
        /// Verify the Grid data of Simulator
        /// </summary>
        /// <param name="Select"></param>
        /// <returns></returns>
        public String VerifyData(DataTable Verify)
        {
            List<String> _errors = new List<String>();
            try
            {
                if (Directory.Exists("SimulatorData"))
                {
                    DataSet LiveData = new DataSet();
                    LiveData.ReadXml(SimulatorConstants.Simulator_Data);
                    DataTable dt = LiveData.Tables["CreateLiveOrder"];
                    dt.Columns["Order Side"].ColumnName = "Side";
                    if (!dt.Columns.Contains("AvgPrice"))
                        dt.Columns.Add("AvgPrice", typeof(string));
                    DataTable dtGrid = DataUtilities.RemoveTrailingZeroes(dt);
                    dtGrid = DataUtilities.RemoveCommas(dtGrid);
                    List<String> columns = new List<String>();
                    _errors = Recon.RunRecon(dtGrid, Verify, columns, 0.01, false, false, ReconType.RoundingMatch, 2, MidpointRounding.AwayFromZero);
                    if (_errors.Count == 0)
                    {
                        return "Data verified";
                    }
                    else
                    {
                        throw new Exception("Trades not found in Simulator data");
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return string.Empty;
        }
    }
}
