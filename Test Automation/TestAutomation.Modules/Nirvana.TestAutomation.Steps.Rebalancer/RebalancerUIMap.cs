using System;
using System.ComponentModel;
using System.Windows.Forms;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using System.Data;
using Nirvana.TestAutomation.Utilities;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.Interfaces;
using System.Collections.Generic;
using System.Diagnostics;
using Nirvana.TestAutomation.BussinessObjects;
using System.IO;
using Nirvana.TestAutomation.Steps.Rebalancer;
using System.Reflection;
using System.Configuration;
using System.Runtime.InteropServices;
using UIAutomationClient;

namespace Nirvana.TestAutomation.Steps.Rebalancer
{
    [UITestFixture]
    public partial class RebalancerUIMap : UIMap
    {

        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, UIntPtr dwExtraInfo);

        public const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        public const uint MOUSEEVENTF_LEFTUP = 0x0004;
        public RebalancerUIMap()
        {
            InitializeComponent();
        }
        private static CUIAutomation automation = new CUIAutomation();
        public RebalancerUIMap(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        /// <summary>
        /// Open Rebalancer
        /// </summary>
        [UITest()]
        protected void OpenRebalancer()
        {
            try
            {
                if (!PranaMain.IsVisible)
                {
                    ExtentionMethods.WaitForVisible(ref PranaMain, 60);
                }
                //Shortcut to open rebalancer input module(CTRL + SHIFT + R)
                Keyboard.SendKeys(ConfigurationManager.AppSettings["OPEN_REBALANCER"]);
                //Wait(5000);
                ExtentionMethods.WaitForVisible(ref RebalanceTab, 60);
                KeyboardUtilities.MaximizeWindow(ref RebalanceTab);
                //Trade.Click(MouseButtons.Left);
                //Rebalancer.Click(MouseButtons.Left);
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public static void GetData(IUIAutomationElement gridElement, DataTable dt)
        {
            var rawChildren = gridElement.FindAll(TreeScope.TreeScope_Subtree, automation.CreateTrueCondition());
            for (int i = 0; i < rawChildren.Length; i++)
            {
                var child = rawChildren.GetElement(i);

                if (child.CurrentControlType == UIA_ControlTypeIds.UIA_DataItemControlTypeId)
                {
                    var dataItemChildren = child.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());
                    DataRow dr = dt.NewRow();
                    bool flag = true;
                    for (int j = 1; j < dataItemChildren.Length; j++)
                    {
                        var dataItemChild = dataItemChildren.GetElement(j);
                        if (j == 1 && (dataItemChild.CurrentName.Length > 10 || string.IsNullOrEmpty(dataItemChild.CurrentName.ToString())))
                        {
                            flag = false;
                            continue;
                        }
                        if (!flag)
                        {
                            dr[j - 2] = dataItemChild.CurrentName;
                        }
                        else
                        {
                            dr[j - 1] = dataItemChild.CurrentName;

                        }
                    }
                    dt.Rows.Add(dr);

                }
            }

        }


        public void EditRow(DataTable testCaseDataTable, DataTable UiDataTable, string GridID)
        {
            IUIAutomationElement appWindow = automation.GetRootElement().FindFirst(
                TreeScope.TreeScope_Children,
                automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Nirvana"));
            IUIAutomationElement gridElement = appWindow.FindFirst(
                    TreeScope.TreeScope_Descendants,
                    automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, GridID));
            for (int i = 0; i < testCaseDataTable.Rows.Count; i++) {
                DataRow dr = testCaseDataTable.Rows[i];
                int index = -1;
                if (!string.IsNullOrEmpty(dr["Action"].ToString())) {
                    if (dr["Action"].ToString().ToUpper().Equals("SELECT")) 
                    {
                        dr["Action"] = string.Empty;
                        DataRow dtRow = DataUtilities.GetMatchingDataRow(DataUtilities.RemoveTrailingZeroes(UiDataTable), dr);
                        index = UiDataTable.Rows.IndexOf(dtRow);
                        if (index < 0)
                        {
                            List<String> errors = Recon.RunRecon(UiDataTable, dr.Table, new List<string>(), 0.01);
                            throw new Exception("Row not found \nRecon Error: " + String.Join("\n\r", errors));
                        }
                        IUIAutomationElement childElement = gridElement.FindFirst(
                                TreeScope.TreeScope_Children,
                                automation.CreateTrueCondition()
                            );
                        IUIAutomationElementArray rawChildren = childElement.FindAll(
                            TreeScope.TreeScope_Children,
                            automation.CreateTrueCondition());
                        object selectObject = rawChildren.GetElement(index + 1).GetCurrentPattern(UIA_PatternIds.UIA_SelectionItemPatternId);
                        if (selectObject != null)
                        {
                            IUIAutomationSelectionItemPattern selectObjectPattern = (IUIAutomationSelectionItemPattern)selectObject;
                            selectObjectPattern.Select();
                        }

                        IUIAutomationElementArray elementsToEdit = rawChildren.GetElement(index + 1).FindAll(
                            TreeScope.TreeScope_Children,
                            automation.CreateTrueCondition()
                        );
                        bool target = false;
                        bool price = false;
                        bool fx = false;
                        bool AccountOrGroup = false;

                        for (int j = 2; j < elementsToEdit.Length; j++) {
                            IUIAutomationElement ele = elementsToEdit.GetElement(j);
                            if (!target &&(!string.IsNullOrEmpty(testCaseDataTable.Rows[i + 1]["Target"].ToString()) && ele.CurrentName.Equals(dr["Target"].ToString()))) {
                                target = true;
                                selectObject = ele.GetCurrentPattern(UIA_PatternIds.UIA_SelectionItemPatternId);
                                if (selectObject != null)
                                {
                                    IUIAutomationSelectionItemPattern selectObjectPattern = (IUIAutomationSelectionItemPattern)selectObject;
                                    selectObjectPattern.Select();
                                }
                                click(ele);
                                DataUtilities.clearTextData();
                                Keyboard.SendKeys(testCaseDataTable.Rows[i + 1]["Target"].ToString());
                                testCaseDataTable.Rows[i]["Target"] = testCaseDataTable.Rows[i + 1]["Target"].ToString();
                            }

                            if (!price &&(!string.IsNullOrEmpty(testCaseDataTable.Rows[i + 1]["Price"].ToString()) && ele.CurrentName.Equals(dr["Price"].ToString())))
                            {
                                price = true;
                                selectObject = ele.GetCurrentPattern(UIA_PatternIds.UIA_SelectionItemPatternId);
                                if (selectObject != null)
                                {
                                    IUIAutomationSelectionItemPattern selectObjectPattern = (IUIAutomationSelectionItemPattern)selectObject;
                                    selectObjectPattern.Select();
                                }
                                click(ele);
                                DataUtilities.clearTextData();
                                Keyboard.SendKeys(testCaseDataTable.Rows[i + 1]["Price"].ToString());
                                testCaseDataTable.Rows[i]["Price"] = testCaseDataTable.Rows[i + 1]["Price"].ToString();

                            }

                            if (!fx && (!string.IsNullOrEmpty(testCaseDataTable.Rows[i + 1]["Fx"].ToString()) && ele.CurrentName.Equals(dr["Fx"].ToString())))
                            {
                                fx = true;
                                selectObject = ele.GetCurrentPattern(UIA_PatternIds.UIA_SelectionItemPatternId);
                                if (selectObject != null)
                                {
                                    IUIAutomationSelectionItemPattern selectObjectPattern = (IUIAutomationSelectionItemPattern)selectObject;
                                    selectObjectPattern.Select();
                                }
                                click(ele);
                                DataUtilities.clearTextData();
                                Keyboard.SendKeys(testCaseDataTable.Rows[i + 1]["Fx"].ToString());
                                testCaseDataTable.Rows[i]["Fx"] = testCaseDataTable.Rows[i + 1]["Fx"].ToString();
                            }

                            if (!AccountOrGroup && (!string.IsNullOrEmpty(testCaseDataTable.Rows[i + 1]["AccountOrGroup"].ToString()) && ele.CurrentName.Equals(dr["AccountOrGroup"].ToString())))
                            {
                                AccountOrGroup = true;
                                selectObject = ele.GetCurrentPattern(UIA_PatternIds.UIA_SelectionItemPatternId);
                                if (selectObject != null)
                                {
                                    IUIAutomationSelectionItemPattern selectObjectPattern = (IUIAutomationSelectionItemPattern)selectObject;
                                    selectObjectPattern.Select();
                                }
                                click(ele);
                                Keyboard.SendKeys(testCaseDataTable.Rows[i + 1]["AccountOrGroup"].ToString());
                                Wait(1000);
                                Keyboard.SendKeys(KeyboardConstants.TABKEY);
                                testCaseDataTable.Rows[i]["AccountOrGroup"] = testCaseDataTable.Rows[i + 1]["AccountOrGroup"].ToString();
                            }
                            Wait(1000);
                        }
                        UiDataTable.Clear();
                        GetData(gridElement, UiDataTable);
                    }
                }
                
            }
            for (int i = testCaseDataTable.Rows.Count - 1; i >= 0; i--)
            {
                if (testCaseDataTable.Rows[i]["Action"].ToString().ToUpper().Equals("EDIT"))
                {
                    testCaseDataTable.Rows[i].Delete();
                }
            }
        }

        public static void click(IUIAutomationElement Element)
        {
            double left = Element.CurrentBoundingRectangle.left;
            double top = Element.CurrentBoundingRectangle.top;
            double right = Element.CurrentBoundingRectangle.right;
            double bottom = Element.CurrentBoundingRectangle.bottom;

            int centerX = (int)((left + right) / 2);
            int centerY = (int)((top + bottom) / 2);

            SetCursorPos(centerX, centerY);
            Wait(1000);
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, UIntPtr.Zero);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, UIntPtr.Zero);
        }
     
        /// <summary>
        /// Export Rebalancer Grid Data
        /// </summary>
        /// <returns></returns>
       [UITest()]
        public DataTable ExportRebalancerGridData()
       {
           try
           {
               //RecordListControl.Click(MouseButtons.Right);
               PranaRebalancerRebalancerNewModelsRebalancerModel5.Click();
               Cell1.Click(MouseButtons.Right);
               //ExportToExcel.Click(MouseButtons.Left);
               DataUtilities.pickFromMenuItem(Window, "Export To Excel", null, "RB");

               string path = ApplicationArguments.ApplicationStartUpPath + @"\TestAutomation.Steps" + TestDataConstants.CAP_AUTOMATION_FOLDER + @"\";
               string directory = (ApplicationArguments.ApplicationStartUpPath + @"\TestAutomation.Steps" + TestDataConstants.CAP_AUTOMATION_FOLDER);

               if (!Directory.Exists(directory))
               {
                   Directory.CreateDirectory(directory);
               }

               Clipboard.SetText(path + ExcelStructureConstants.REBALANCERGRIDDATA);
               KeyboardUtilities.PressKey(2, KeyboardConstants.BACKSPACEKEY);
               txtbxFileName.Click(MouseButtons.Left);

               KeyboardUtilities.PressKey(2, KeyboardConstants.BACKSPACEKEY);
               Keyboard.SendKeys("[CTRL+V]");

               ButtonSave.Click(MouseButtons.Left);


               if (ConfirmSaveAs1.IsVisible)
               {
                   ButtonYes.Click(MouseButtons.Left);
               }


               if (NirvanaAlert.IsVisible)
                   ButtonNo.Click(MouseButtons.Left);


               // Load Export data into datatable
               ITestDataProvider provider = Factory.TestDataProvider.GetProvider(ProviderType.OpenXml);
               DataSet testCases = provider.GetTestData(path + @"\" + ExcelStructureConstants.REBALANCERGRIDDATA, 1, 2);
               DataTable dtExportedData = testCases.Tables[0];
               return dtExportedData;
           }
           catch (Exception)
           { throw; }
       }

       public DataTable ExportTradeListGrid()
       {
           try
           {
               Wait(3000);
               True8.Click(MouseButtons.Right);
               Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
               Keyboard.SendKeys(KeyboardConstants.ENTERKEY);

               string path = ApplicationArguments.ApplicationStartUpPath + @"\TestAutomation.Steps" + TestDataConstants.CAP_AUTOMATION_FOLDER + @"\";
               string directory = (ApplicationArguments.ApplicationStartUpPath + @"\TestAutomation.Steps" + TestDataConstants.CAP_AUTOMATION_FOLDER);

               if (!Directory.Exists(directory))
               {
                   Directory.CreateDirectory(directory);
               }

               Clipboard.SetText(path + ExcelStructureConstants.TRADELISTGRIDDATA);
               KeyboardUtilities.PressKey(2, KeyboardConstants.BACKSPACEKEY);
               TextBoxFilename1.Click(MouseButtons.Left);

               KeyboardUtilities.PressKey(2, KeyboardConstants.BACKSPACEKEY);
               Keyboard.SendKeys("[CTRL+V]");

               ButtonSave1.Click(MouseButtons.Left);
               Wait(3000);
               Keyboard.SendKeys(KeyboardConstants.LEFT_ARROWKEY);
               Keyboard.SendKeys(KeyboardConstants.ENTERKEY);

               // Commented because of customized popup
               //if (ConfirmSaveAs3.IsVisible)
               //{
               //    ButtonYes.Click(MouseButtons.Left);
               //}

               Wait(2000);
               Keyboard.SendKeys(KeyboardConstants.LEFT_ARROWKEY);
               Keyboard.SendKeys(KeyboardConstants.ENTERKEY);

               // Commented beacause of customized pop up
               //if (NirvanaAlert6.IsVisible)
               //{
               //    ButtonNo.Click(MouseButtons.Left);
               //}


               // Load Export data into datatable
               ITestDataProvider provider = Factory.TestDataProvider.GetProvider(ProviderType.OpenXml);
               DataSet testCases = provider.GetTestData(path + @"\" + ExcelStructureConstants.TRADELISTGRIDDATA, 1, 1);
               DataTable dtExportedData = testCases.Tables[0];
               return dtExportedData;
           }
           catch (Exception) { throw; }
       }

         /// <summary>
        /// Maximize Rebalancer
        /// </summary>
        /// <param name="RebalanceTab"></param>
       internal void MaximizeRebalancer(UIAutomationElement RebalanceTab)
       {
           try
           {
               RebalanceTab.Click(MouseButtons.Right);
               Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
               Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
               Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
               Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
               Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
               Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
           }
           catch (Exception) { throw; }
       }


        /// <summary>
        /// Minimizes the Rebalancer
        /// </summary>
       public void MinimizeRebalancer()
       {
           try
           {
               RebalanceTab.Click(MouseButtons.Left);
               KeyboardUtilities.MinimizeWindow(ref RebalanceTab);
               Wait(100);
           }
           catch (Exception ex)
           {
               bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
               if (rethrow)
                   throw;
           }
       }

       public void CloseTradeList()
       {
           try
           {
               TradeList1.Click(MouseButtons.Right);
               Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
               Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
               Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
               Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
               Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
               Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
               Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
           }
           catch (Exception) { throw; }
       }

       /// <summary>
       /// Method used to lock the securities in RebalancerDataGrid with the help of row index 
       /// </summary>
       /// <param name="testData">Excel Sheet Data</param>
       /// <param name="sheetIndexToName">Step name</param>
       /// <param name="checkFlag"></param>
       public void LockSecurities(DataSet testData, Dictionary<int, string> sheetIndexToName, bool checkFlag = false)
       {
           try
           {
               DataTable dtExportedTrades = new DataTable();
               RebalacerDataGrid = GetLatestGridObject(RecordListControl);
               dtExportedTrades = ExportRebalancerGridData();
               if (dtExportedTrades != null)
                   LockDivideUnlock1(testData, sheetIndexToName, RecordListControl, dtExportedTrades, checkFlag);
           }
           catch (Exception ex)
           {
               bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
               if (rethrow)
                   throw;
           }
       }

        /// <summary>
        /// Method used to Verify the Rows from testdata and lock/unlock them with the help of the row index
        /// </summary>
        /// <param name="testData">Excel Sheet Data</param>
        /// <param name="sheetIndexToName">Step name in Regression sheet</param>
        /// <param name="RebalancerDataGrid">Rebalancer Data Grid</param>
        /// <param name="dtTrades">Datatable in which exported data saved</param>
        /// <param name="checkFlag"></param>
       public void LockDivideUnlock1(DataSet testData, Dictionary<int, string> sheetIndexToName, UIAutomationElement RebalancerDataGrid, DataTable dtTrades, bool checkFlag = false)
       {
           string _errorMessage = string.Empty;
           try
           {
               Cell1.Click(MouseButtons.Left);
               Wait(1000);
               SendKeys.SendWait("{TAB}");
               List<int> tradesIndexList = new List<int>();
               DataRow[] matchedRows = DataUtilities.GetMatchingMultipleDataRows(dtTrades, testData.Tables[sheetIndexToName[0]], _errorMessage, checkFlag);
               foreach (DataRow dr in matchedRows)
               {
                   tradesIndexList.Add(dtTrades.Rows.IndexOf(dr));
               }
               tradesIndexList.Sort();
               int oldindex = 0;
               KeyboardUtilities.PressKey(3, KeyboardConstants.HOMEKEY);

               foreach (var tradeno in tradesIndexList)
               {
                   KeyboardUtilities.PressDownKeyWithWait(tradeno - oldindex);
                   Keyboard.SendKeys(KeyboardConstants.F1KEY);
                   oldindex = tradeno;
               }
               Wait(1000);
           }
           catch (Exception ex)
           {

               bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
               if (rethrow)
                   throw;
           }
       }
       
        /// <summary>
        /// Used to get the latest grid object from the Rebalancer UI
        /// </summary>
        /// <param name="gridObject">passes the UIAutomationElement</param>
        /// <returns></returns>
       public static UIAutomationElement GetLatestGridObject(UIAutomationElement gridObject)
       {
           try
           {
               UIAutomationElement TempRecords = new UIAutomationElement();
               TempRecords.AutomationName = TestDataConstants.CONST_RECORDS;
               TempRecords.ClassName = TestDataConstants.CONST_VIEWABLERECORDCOLLECTION;
               TempRecords.Comment = null;
               TempRecords.ItemType = "";
               TempRecords.MatchedIndex = 0;
               TempRecords.Name = TestDataConstants.CONST_RECORDS;
               TempRecords.ObjectImage = null;
               TempRecords.Parent = gridObject;
               TempRecords.UIObjectType = UIObjectTypes.Unknown;
               TempRecords.UseCoordinatesOnClick = true;
               return TempRecords;
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
       /// Clear button on rebalancer
       /// </summary>
       public void ClearCalculations()
       {
           try
           {
               ClearCalculation1.Click(MouseButtons.Left);
               ButtonYes2.Click(MouseButtons.Left);

               Wait(4000);
           }
           catch (Exception) { throw; }
       }
       [UITest()]
       public void RightClickScroll()
       {
           try
           {
               IncreaseBtn.Click(MouseButtons.Right);
               PageDown1.Click(47, 7);
           }
           catch (Exception) { throw; }
       }

       //[UITest()]
       //public void Test1()
       //{
       //    NirvanaAlert5.Click(MouseButtons.Left);
       //    AllowNegativeCash4.Click(MouseButtons.Left);
       //}

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
               Wait(5000);
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
       public void CloseRebalancerui()
       {
           try
           {
               RebalanceTab.Click(MouseButtons.Left);
               KeyboardUtilities.CloseWindow(ref RebalanceTab);
               Wait(100);
           }
           catch (Exception ex)
           {
               bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
               if (rethrow)
                   throw;
           }
       }

       protected List<String> RunReconOnTables(DataTable uiDATA, DataTable excelData)
       {
           List<string> errors = new List<string>();
           List<string> columns = new List<string>();
           try
           {
              
               try
               {
                   uiDATA = RemoveCommasPercentTrailingZeroes(uiDATA);
                   excelData = RemoveCommasPercentTrailingZeroes(excelData);
                   if (uiDATA != null && excelData != null)
                   {

                       errors = Recon.RunRecon(uiDATA, excelData, columns, 0.01);
                   }
                   else
                       throw new Exception("Either uiDATA is null or excelData is null");
               }
               catch (Exception ex)
               {
                   bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                   if (rethrow)
                       throw;
               }
               return errors;

           }
           catch (Exception ex)
           {
               bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
               if (rethrow)
                   throw;
           }

           return errors;
       }

       private DataTable RemoveCommasPercentTrailingZeroes(DataTable dt)
       {

               try
               {
                   if (dt != null && dt.Rows.Count > 0)
                   {
                       dt = DataUtilities.RemoveCommas(dt);
                       dt = DataUtilities.RemovePercent(dt);
                       dt = DataUtilities.RemoveTrailingZeroes(dt);
                      
                   }

                   return dt;
               }
               catch (Exception ex)
               {
                   bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                   if (rethrow)
                       throw;
               }
               return null;
       }
    }
}
