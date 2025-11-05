using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.Factory;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using System.Configuration;
using System.IO;
using System.Reflection;

namespace Nirvana.TestAutomation.Steps.PTT
{
    [UITestFixture]
    public partial class PTTUIMap : UIMap
    {
        public PTTUIMap()
        {
            InitializeComponent();
        }

        public PTTUIMap(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
        
        /// <summary>
        /// Open Percent trading tool. 
        /// </summary>
        public void OpenPTT()
        {
            try
            {
                //Shortcut to open PTT (CTRL + SHIFT + %)
                if (!PranaMain.IsVisible)
                {
                    ExtentionMethods.WaitForVisible(ref PranaMain, 60);
                }
                Keyboard.SendKeys(ConfigurationManager.AppSettings["OPEN_PTT"]);
               // Wait(5000);
                ExtentionMethods.WaitForVisible(ref  PercentTradingTool1,15);
                //Trade.Click(MouseButtons.Left);
                //PercentTradingTool4.Click(MouseButtons.Left);
                PercentTradingTool2.Click(MouseButtons.Left);
                //Nirvana.Click(MouseButtons.Right);

                //Maximize.Click(MouseButtons.Left);
                KeyboardUtilities.MaximizeWindow(ref Nirvana);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }

        }

        /// <summary>
        /// Open General Preferences from Tools
        /// </summary>
        protected void OpenGeneralPreferences()
        {
            try
            {

                //PranaMain.WaitForVisible();
                if (!PranaMain.IsVisible)
                {
                    ExtentionMethods.WaitForVisible(ref PranaMain, 40);
                }//Shortcut to open Preferences under Tools (CTRL + ALT + F)
                Keyboard.SendKeys(ConfigurationManager.AppSettings["OPEN_PREF"]);
               // Wait(5000);
                ExtentionMethods.WaitForVisible(ref PreferencesMain, 15);
                //Tools.Click(MouseButtons.Left);
                //Preferences.Click(MouseButtons.Left);
                Trading.Click(MouseButtons.Left);
                GeneralPreferences.Click(MouseButtons.Left);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// It will copy the TTGeneralPref.xml in backup folder
        /// </summary>
        protected void CopyTTGeneralPref()
        {
            try
            {
                string DefaultSymbologySourceOldFile = ConfigurationManager.AppSettings["DefaultSymbologySourceOldFile"];
                string DefaultSymbologySourceNewFile = ConfigurationManager.AppSettings["DefaultSymbologySourceNewFile"];
                File.Copy(DefaultSymbologySourceOldFile, DefaultSymbologySourceNewFile, true);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        protected void RevertTTGenPref()
        {
            try
            {
                string DefaultSymbologySourceOldFile = ConfigurationManager.AppSettings["DefaultSymbologySourceOldFile"];
                string DefaultSymbologySourceNewFile = ConfigurationManager.AppSettings["DefaultSymbologySourceNewFile"]; ;
                Console.WriteLine("Reverting Pref.");
                File.Delete(DefaultSymbologySourceOldFile);
                File.Move(DefaultSymbologySourceNewFile, DefaultSymbologySourceOldFile);
                Console.WriteLine("Pref Reverted Successfully");
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }
        /// <summary>
        /// Open Percent trading tool without extra click on grid so that editing can be done easily
        /// </summary>
        public void OpenPTTool()
        {
            try
            {
                Trade.Click(MouseButtons.Left);
                PercentTradingTool4.Click(MouseButtons.Left);
                PercentTradingTool.Click(MouseButtons.Right);
                KeyboardUtilities.PressKey(5, KeyboardConstants.DOWN_ARROWKEY);
                Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Inputs the parameters PST.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <exception cref="System.Exception"></exception>
        public void InputParametersPTT(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            try
            {               
                foreach (DataRow dataRow in testData.Tables[sheetIndexToName[0]].Rows)
                {
                    if (dataRow[TestDataConstants.COL_TICKER].ToString() != string.Empty)
                    {
                        PART_SymbolTextBox.Click(MouseButtons.Left);    // Ticker
                        Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                        PART_SymbolTextBox.ClearText();
                        Keyboard.SendKeys(dataRow[TestDataConstants.COL_TICKER].ToString());
                        Wait(3000);
                        Keyboard.SendKeys(KeyboardConstants.TABKEY);
                        Wait(5000);

                    }
                    
                    if (dataRow[TestDataConstants.COL_ADD_SET].ToString() != string.Empty)// Add/set
                    {
                        ToggleButton5.Click(MouseButtons.Left);
                        var item = dataRow[TestDataConstants.COL_ADD_SET].ToString();
                        SelectAccounts(item);
                        Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                    }
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                    ToggleButton2.Click(MouseButtons.Left);
                    if (dataRow[TestDataConstants.COL_TYPE].ToString() != string.Empty)
                    {
                        string item = dataRow[TestDataConstants.COL_TYPE].ToString();// Type
                        SelectAccounts(item);
                    }

                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                   
                    if (dataRow[TestDataConstants.COL_TARGET].ToString() != string.Empty)   //target
                    {                        
                        ExtentionMethods.CheckCellValueConditions(dataRow[TestDataConstants.COL_TARGET].ToString(), KeyboardConstants.ENTERKEY, false);
                    }
                    Wait(1000);
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                
                    if (dataRow[TestDataConstants.COL_PRICE].ToString() != string.Empty) //Price
                    {
                        ExtentionMethods.CheckCellValueConditions(dataRow[TestDataConstants.COL_PRICE].ToString(), KeyboardConstants.ENTERKEY, false);
                    }

                    if (dataRow[TestDataConstants.COL_COMBINED_ACCOUNT_TOTAL].ToString() != string.Empty)//Combined account total
                    {
                        ToggleButton1.Click(MouseButtons.Left);
                        string item = dataRow[TestDataConstants.COL_COMBINED_ACCOUNT_TOTAL].ToString();
                        SelectAccounts(item);
                    }
                    if (dataRow[TestDataConstants.COL_MASTERFUND_fUND].ToString() != string.Empty)
                    {
                        ToggleButton3.Click(MouseButtons.Left);
                        string item = dataRow[TestDataConstants.COL_MASTERFUND_fUND].ToString();// MasterFund account. 
                        SelectAccounts(item);
                    }

                    if (dataRow[TestDataConstants.COL_ACCOUNTS].ToString() != string.Empty) // account
                    {
                        string[] accounts = dataRow[TestDataConstants.COL_ACCOUNTS].ToString().Split(',');
                        ToggleButton4.Click(MouseButtons.Left);
                        Wait(3000);
                        foreach (var acc in accounts)
                        {
                            SelectAccounts(acc);
                        }
                        ToggleButton4.Click(MouseButtons.Left);
                    }

                    if (dataRow.Table.Columns.Contains(TestDataConstants.COL_ROUND_LOTS) && dataRow[TestDataConstants.COL_ROUND_LOTS].ToString() != string.Empty)
                    {
                        if (dataRow[TestDataConstants.COL_ROUND_LOTS].ToString() != string.Empty)
                        {
                            if (!XamComboEditor7.IsEnabled)
                            {
                                ToggleButton8.Click(MouseButtons.Left);
                                string item = dataRow[TestDataConstants.COL_ROUND_LOTS].ToString(); // Round Lot. 
                                SelectAccounts(item);
                            }
                            else {
                                ToggleButton7.Click(MouseButtons.Left);
                                string item = dataRow[TestDataConstants.COL_ROUND_LOTS].ToString(); // Round Lot. 
                                SelectAccounts(item);
                            }
                            
                        }
                    }

                    if (dataRow.Table.Columns.Contains(TestDataConstants.Cusdtodian_Broker))
                    {
                        if (!String.IsNullOrEmpty(dataRow[TestDataConstants.Cusdtodian_Broker].ToString()))
                        {
                            ToggleButton6.Click(MouseButtons.Left);
                            string item = dataRow[TestDataConstants.Cusdtodian_Broker].ToString(); // Cusdtodian Broker 
                            SelectAccounts(item);
                        }
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

        /// <summary>
        /// Selects the accounts.
        /// </summary>
        /// <param name="acc">The acc.</param>
        private void SelectAccounts(string acc)
        {
            try
            {
                UIAutomationElement accountComboItem = new UIAutomationElement();
                accountComboItem.AutomationName = acc;
                accountComboItem.ClassName = "ComboEditorItemControl";
                accountComboItem.Comment = null;
                accountComboItem.ItemType = "";
                accountComboItem.MatchedIndex = 0;
                accountComboItem.Name = acc;
                accountComboItem.Parent = this.Popup;
                accountComboItem.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Unknown;
                accountComboItem.UseCoordinatesOnClick = true;
                accountComboItem.Click(MouseButtons.Left);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Clears the cell data.
        /// </summary>
        private void ClearCellData()
        {
            try
            {
                KeyboardUtilities.PressKey(10, KeyboardConstants.BACKSPACEKEY);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }
        

         /// <summary>
        /// /// Exports the Basket Compliance data.
        /// </summary>
        /// <returns></returns>

        public void ExportData()
        {
           try
            {
               string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + TestDataConstants.CAP_AUTOMATION_FOLDER + @"\";
                if (!Directory.Exists(path))
                {
                   Directory.CreateDirectory(path);
                }

                if (ExportButton.IsVisible)
                {
                    ExportButton.Click(MouseButtons.Left);

                    if (SaveAs2.IsVisible)
                    {
                        Clipboard.SetText(path + ExcelStructureConstants.BasketCompExportFileName);
                        Keyboard.SendKeys("[CTRL+V]");
                        ButtonSave2.Click(MouseButtons.Left);
                        Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                        Wait(5000);
                        if (ConfirmSaveAs2.IsVisible)
                        {
                            ButtonYes2.Click(MouseButtons.Left);
                            Wait(2000);
                        }
                        if (AlertsDataExport1.IsVisible)
                            ButtonOK1.Click(MouseButtons.Left);
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
        /// Exports the PST data.
        /// </summary>
        /// <returns></returns>
        public DataTable ExportPSTData()
        {
            try
            {
                Export.DoubleClick(MouseButtons.Left);
                KeyboardUtilities.PressKey(2, KeyboardConstants.BACKSPACEKEY);
                TextBoxFilename.Click(MouseButtons.Left);
                string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\";
                KeyboardUtilities.PressKey(2, KeyboardConstants.BACKSPACEKEY);
                Keyboard.SendKeys(path + ExcelStructureConstants.PSTExportName);
                ButtonSave.Click(MouseButtons.Left);
                ConfirmSaveAs1.WaitForObject();
                if (ConfirmSaveAs1.IsValid)
                {
                    ButtonYes1.Click(MouseButtons.Left);
                }
                ButtonNo.Click(MouseButtons.Left);

                ITestDataProvider provider = TestDataProvider.GetProvider(ProviderType.OpenXml);
                DataSet pstDataSet = provider.GetTestData(path + @"\" + ExcelStructureConstants.PSTExportName);
                DataTable dtCalculatedData = pstDataSet.Tables[TestDataConstants.COL_CALCULATEDVALUES];
                return dtCalculatedData;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
                return null;
            }
        }
        /// <summary>
        /// for PTT MINIMIZE BUTTON
        /// </summary>
        public void MinimizePTT()
        {
            try
            {
                Nirvana.Click(MouseButtons.Left);
                KeyboardUtilities.MinimizeWindow(ref Nirvana);
                //Wait(100);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
        }
        public void CloseWindowPTT()
        {
            try
            {
                Nirvana.Click(MouseButtons.Left);
                KeyboardUtilities.CloseWindow(ref Nirvana);
                Wait(100);
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
