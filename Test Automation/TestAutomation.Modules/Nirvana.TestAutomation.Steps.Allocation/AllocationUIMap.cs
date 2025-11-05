using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using System.IO;
using System.Reflection;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Configuration;
using System.Runtime.InteropServices;
using UIAutomationClient;
using Nirvana.TestAutomation.UIAutomation;

namespace Nirvana.TestAutomation.Steps.Allocation
{
    [UITestFixture]
    public partial class AllocationUIMap : UIMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AllocationUIMap"/> class.
        /// </summary>
        protected AllocationUIMap()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AllocationUIMap"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public AllocationUIMap(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }

        /// <summary>
        /// For Saving the changes on grid
        /// </summary>
        private static CUIAutomation automation = new CUIAutomation();
        
        public void SaveAllocation()
        {
            try
            {
                if (SavewDivideStatus.Bounds.X >= 0 && SavewDivideStatus.Bounds.Y >= 0)
                    SavewDivideStatus.Click(MouseButtons.Left);
                if (SavewDivideoStatus.Bounds.X >= 0 && SavewDivideoStatus.Bounds.Y >= 0)
                    SavewDivideoStatus.Click(MouseButtons.Left);
               // Wait(1000);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// For Closing the allocation window
        /// </summary>
        public void CloseAllocation()
        {
            try
            {
                Historical.Click(MouseButtons.Left);
                KeyboardUtilities.CloseWindow(ref Allocation3);
                //Wait(500);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// For minimizing Allocation windows
        /// </summary>
        public void MinimizeAllocation()
        {
            try
            {
                Allocation3.Click(MouseButtons.Left);
                KeyboardUtilities.MinimizeWindow(ref Allocation3);
               // Wait(100);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Open Allocation UI
        /// </summary>
        public void OpenAllocation()
        {
            try
            {
                if (!PranaMain.IsVisible)
                {
                    ExtentionMethods.WaitForVisible(ref PranaMain, 40);
                }
                //  Shortcut to open allocation module(CTRL + SHIFT + A)
                Keyboard.SendKeys(ConfigurationManager.AppSettings["OPEN_ALLOCATION"]);
                //Wait(15000);
                ExtentionMethods.WaitForVisible(ref Allocation, 15);
                //Trade.Click(MouseButtons.Left);
                //Allocation2.Click(MouseButtons.Left);
                if (!AllocationGrids.IsVisible)
                {
                    Keyboard.SendKeys(ConfigurationManager.AppSettings["OPEN_ALLOCATION"]);
                    ExtentionMethods.WaitForVisibleUIElement(ref AllocationGrid, 20);
                }
                AllocationGrids.Click(MouseButtons.Left);
                AllocationGrids1.Click(MouseButtons.Left);
                Keyboard.SendKeys(KeyboardConstants.ALT_SPACE_R);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Allocates the trades.
        /// </summary>
        /// <param name="row">The row.</param>
        public static void AllocateTrades(DataRow row)
        {
            try
            {
                Keyboard.SendKeys(KeyboardConstants.UP_ARROWKEY);
                Keyboard.SendKeys(KeyboardConstants.CTRLA + KeyboardConstants.BACKSPACEKEY);
                if (row != null)
                    Keyboard.SendKeys(row[TestDataConstants.COL_ACCOUNT].ToString());
                Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                Keyboard.SendKeys(KeyboardConstants.RIGHT_ARROWKEY);
                if (row != null)
                    Keyboard.SendKeys(row[TestDataConstants.COL_QTYPERCENT].ToString() + KeyboardConstants.ENTERKEY);
                Keyboard.SendKeys(KeyboardConstants.LEFT_ARROWKEY);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Gets the latest grid object.
        /// </summary>
        /// <param name="gridObject">The grid object.</param>
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
        /// Exports the trades.
        /// </summary>
        /// <param name="GridRecords">The grid records.</param>
        /// <param name="TextBoxExportFileName">Name of the text box export file.</param>
        /// <param name="SaveEXportFileButton">The save e xport file button.</param>
        /// <param name="ConfirmExportSaveAs">The confirm export save as.</param>
        /// <param name="ButtonOverwriteFileYes">The button overwrite file yes.</param>
        /// <returns></returns>
        public DataTable ExportTrades(UIAutomationElement GridRecords, UIWindow TextBoxExportFileName, UIWindow SaveEXportFileButton, UIWindow ConfirmExportSaveAs, UIWindow ButtonOverwriteFileYes,bool isTaxlots=false, bool isAllocated=false)
        {
            try
            {
                GridRecords.Click(MouseButtons.Left);
                MouseController.RightClick();
                ExportData.Click(MouseButtons.Left);
                if (isAllocated)
                {
                    if (!isTaxlots && Groups.IsVisible)
                    {
                        Groups.Click(MouseButtons.Left);
                    }
                    else if (isTaxlots && Taxlots.IsVisible)
                    {
                        Taxlots.Click(MouseButtons.Left);
                    }
                }
                string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + TestDataConstants.CAP_AUTOMATION_FOLDER + @"\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                if (NirvanaAllocation.IsVisible)
                {
                    ButtonOK.Click(MouseButtons.Left);
                    CommonMethods.SaveScreenshotAndPreferences(DateTime.Now.ToString("MM-dd-yy-hh-mm-ss"));
                    DataTable dt = null;
                    return dt;
                }
                
            //    TextBoxExportFileName.Click(MouseButtons.Left);
             //   clearText(TextBoxExportFileName);
                Clipboard.SetText(path + ExcelStructureConstants.AllocatedTradesExportFileName);
                Keyboard.SendKeys("[CTRL+V]");
                SaveEXportFileButton.Click(MouseButtons.Left);
                if (ConfirmExportSaveAs.IsVisible)
                {
                    ButtonOverwriteFileYes.Click(MouseButtons.Left);
                }
                ButtonNo.Click(MouseButtons.Left);

                DataSet testCases;
                ITestDataProvider provider = Factory.TestDataProvider.GetProvider(ProviderType.OpenXml);
                if(isTaxlots)
                    testCases = provider.GetTestData(path + @"\"+ ExcelStructureConstants.AllocatedTradesExportFileName, 1, 2);
                else
                    testCases = provider.GetTestData(path + @"\" + ExcelStructureConstants.AllocatedTradesExportFileName);
                DataTable dtExportedTrades = testCases.Tables[0];
                return dtExportedTrades;
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
        /// Clears the text.
        /// </summary>
        /// <param name="TextBoxElement">The text box element.</param>
        public void clearText(UIAutomationElement TextBoxElement)
        {
            try
            {
                Stopwatch tmr = new Stopwatch();
                tmr.Start();
                while (TextBoxElement.Text.Length > 0 && tmr.ElapsedMilliseconds <= 15000)
                {
                    TextBoxElement.Click(MouseButtons.Left); 
                    Keyboard.SendKeys("[HOME]");
                    MouseController.DoubleClick();
                    Keyboard.SendKeys(KeyboardConstants.BACKSPACEKEY);
                }
                tmr.Stop();
            }
            catch (Exception ex)
            {

                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Clears the text.
        /// </summary>
        /// <param name="TextBoxElement">The text box element.</param>
        public void clearText(UIWindow TextBoxElement)
        {
            try
            {
                Stopwatch tmr = new Stopwatch();
                tmr.Start();
                while (TextBoxElement.Text.Length > 0 && tmr.ElapsedMilliseconds <= 15000)
                {
                    TextBoxElement.Click(MouseButtons.Left);
                    Keyboard.SendKeys("[HOME]");
                    MouseController.DoubleClick();  
                    Keyboard.SendKeys(KeyboardConstants.BACKSPACEKEY);
                }
                tmr.Stop();
            }
            catch (Exception ex)
            {

                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Clicks the on ComboBox item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="combo">The combo.</param>
        public void ClickOnComboBoxItem(string item, UIAutomationElement combo)
        {
            try
            {
                var cacheChildren = combo.AutomationElementWrapper.CachedChildren;
                Dictionary<string, int> nameToIndexMapping = new Dictionary<string, int>();

                for (int i = 1; i < cacheChildren.Count; i++)
                {
                    if (!nameToIndexMapping.ContainsKey(cacheChildren[i].CachedChildren[0].Name))
                        nameToIndexMapping.Add(cacheChildren[i].CachedChildren[0].Name, i);
                }
                if (nameToIndexMapping.Count > 0)
                    cacheChildren[nameToIndexMapping[item]].CachedChildren[0].WpfClick();
            }
            catch (Exception ex)
            {

                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Clickdropdownbuttons the specified parent automation element.
        /// </summary>
        /// <param name="parentAutomationElement">The parent automation element.</param>
        public void Clickdropdownbutton(UIAutomationElement parentAutomationElement)
        {
            try
            {
                UIControlPart dropdownButton = new UIControlPart();
                int x = parentAutomationElement.RelativeCenterPoint.X;
                dropdownButton.BoundsInParent = new System.Drawing.Rectangle(x * 2 - 18, 0, 18, 21);
                dropdownButton.Comment = null;
                dropdownButton.ControlPartProvider = null;
                dropdownButton.Name = "dropdownButton";
                dropdownButton.ObjectImage = null;
                dropdownButton.Parent = parentAutomationElement;
                dropdownButton.Path = null;
                dropdownButton.UIObjectType = TestAutomationFX.UI.UIObjectTypes.ControlPart;
                dropdownButton.UseCoordinatesOnClick = false;
                dropdownButton.Click(MouseButtons.Left);
            }
            catch (Exception ex)
            {

                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Clicks the text box.
        /// </summary>
        /// <param name="matchedindex">The matchedindex.</param>
        public void ClickTextBox(int matchedindex)
        {
            try
            {
                UIAutomationElement var = new UIAutomationElement();
                var.AutomationName = "";
                var.ClassName = "XamMaskedEditor";
                var.Comment = null;
                var.Index = matchedindex + 13;
                var.ItemType = "";
                var.MatchedIndex = matchedindex;
                var.Name = "XamMaskedEditor";
                var.Parent = this.GroupBox3;
                var.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Unknown;
                var.UseCoordinatesOnClick = false;
                var.Click(MouseButtons.Left);
            }
            catch (Exception ex)
            {

                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Clicks the CheckBox.
        /// </summary>
        /// <param name="name">The name.</param>
        public void ClickCheckBox(string name)
        {

            try
            {
                UIAutomationElement var = new UIAutomationElement();
                var.AutomationId = "Enable" + name;
                var.AutomationName = name;
                var.ClassName = "CheckBox";
                var.Comment = null;
                var.ItemType = "";
                var.MatchedIndex = 0;
                var.Name = name;
                var.Parent = GroupBox3;
                var.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Checkbox;
                var.UseCoordinatesOnClick = false;
                if (!var.IsChecked)
                    var.Click(MouseButtons.Left);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Creates the combo item.
        /// </summary>
        /// <param name="comboindex">The comboindex.</param>
        /// <returns></returns>
        public UIAutomationElement CreateComboItem(int comboindex)
        {
            UIAutomationElement var = new UIAutomationElement();
            try
            {
                var.AutomationName = "";
                var.ClassName = "ComboBox";
                var.Comment = null;
                if (comboindex < 6)
                {
                    var.Index = comboindex + 7;
                }
                else
                {
                    var.Index = comboindex + 19;
                }
                var.ItemType = "";
                var.MatchedIndex = comboindex;
                var.Name = "ComboBox";
                var.Parent = this.GroupBox3;
                var.UIObjectType = TestAutomationFX.UI.UIObjectTypes.ComboBox;
                var.UseCoordinatesOnClick = false;

            }
            catch (Exception  ex)
            {

                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return var;
        }

        /// <summary>
        /// Creates the toggle item.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <returns></returns>
        public UIControlPart CreateToggleItem(UIAutomationElement parent)
        {
            UIControlPart toggleBtn = new UIControlPart();
            try
            {
                toggleBtn.BoundsInParent = new System.Drawing.Rectangle(80, 2, 20, 18);
                toggleBtn.Comment = null;
                toggleBtn.ControlPartProvider = null;
                toggleBtn.Name = "ControlPartOfComboBox";
                toggleBtn.ObjectImage = null;
                toggleBtn.Parent = parent;
                toggleBtn.Path = null;
                toggleBtn.UIObjectType = TestAutomationFX.UI.UIObjectTypes.ControlPart;
                toggleBtn.UseCoordinatesOnClick = false;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }

            return toggleBtn;
        }

        /// <summary>
        /// Creates the dictionary.
        /// </summary>
        /// <param name="ComboElement">The combo element.</param>
        /// <returns></returns>
        public Dictionary<string, int> CreateDictionary(UIAutomationElement ComboElement)
        {
            try
            {
                int count = ComboElement.AutomationElementWrapper.CachedChildren.Count;
                Dictionary<String, int> NameToIndex = new Dictionary<String, int>();
                for (int i = 1; i < count; i++)
                {
                    int index = Convert.ToInt32(ComboElement.AutomationElementWrapper.CachedChildren[i].Index);
                    string name = ComboElement.AutomationElementWrapper.CachedChildren[i].CachedChildren[0].Name;
                    NameToIndex.Add(name, index);
                }
                return NameToIndex;
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
        /// Selects trades
        /// </summary>
        /// <param name="testData">contains trades to be selected</param>
        /// <param name="sheetIndexToName"></param>
        /// <param name="Records">where the data resides on UI</param>
        /// <param name="dtTrades">Master table</param>
        public void SelectTrades(DataSet testData, Dictionary<int, string> sheetIndexToName, UIAutomationElement Records, DataTable dtTrades,bool checkFlag=false)
        {
            string errorMessage = string.Empty;
            try
            {
                Records.AutomationElementWrapper.CachedChildren[1].WpfClickLeftBound(MouseButtons.Left, 80);
                List<int> tradesIndexList = new List<int>();
                if (testData.Tables[0].Columns.Contains("CheckBox") && !string.IsNullOrEmpty(testData.Tables[0].Rows[0]["CheckBox"].ToString()))
                {
                    int oldindex = 0;
                    foreach (DataRow dr in testData.Tables[sheetIndexToName[0]].Rows)
                    {
                        if (dr["CheckBox"].ToString().Equals("ToggleState_On"))
                        {
                            Records.AutomationElementWrapper.CachedChildren[1].WpfClickLeftBound(MouseButtons.Left, 80);
                            dr["CheckBox"] = string.Empty;
                            DataRow dtRow = DataUtilities.GetMatchingDataRow(DataUtilities.RemoveTrailingZeroes(dtTrades), dr);
                            tradesIndexList.Add(dtTrades.Rows.IndexOf(dtRow));
                            string getToggleState = getToggleStateofrow(testData.Tables[sheetIndexToName[0]].TableName, dtTrades.Rows.IndexOf(dtRow));
                            if (!getToggleState.Equals("ToggleState_On"))
                            {
                                foreach (var tradeno in tradesIndexList)
                                {
                                    KeyboardUtilities.PressDownKeyWithWait(tradeno);
                                    Keyboard.SendKeys(KeyboardConstants.F1KEY);
                                }
                                if (tradesIndexList.Count == 0)
                                {
                                    throw new Exception("Trades not found on the grid wrong data in the testcase");
                                }
                            }
                            
                        }
                        else {
                            dr["CheckBox"] = string.Empty;
                            DataRow dtRow = DataUtilities.GetMatchingDataRow(DataUtilities.RemoveTrailingZeroes(dtTrades), dr);
                            tradesIndexList.Add(dtTrades.Rows.IndexOf(dtRow));
                            foreach (var tradeno in tradesIndexList)
                            {
                                KeyboardUtilities.PressDownKeyWithWait(tradeno);
                                Keyboard.SendKeys(KeyboardConstants.F1KEY);
                                Wait(1000);
                            }
                            if (tradesIndexList.Count == 0)
                            {
                                throw new Exception("Trades not found on the grid wrong data in the testcase");
                            }
                        }
                        tradesIndexList.Clear();
                    }
                }
                else
                {

                    DataRow[] matchedRows = DataUtilities.GetMatchingMultipleDataRows(dtTrades, testData.Tables[sheetIndexToName[0]], errorMessage, checkFlag);
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
                    if (tradesIndexList.Count == 0)
                    {
                        throw new Exception("Trades not found on the grid wrong data in the testcase");
                    }
                }
                //Wait(1000);
            }
            catch (Exception ex)
            {

                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        private string getToggleStateofrow(string p, int idx)
        {
            string id = "";
            if (p.ToLower().Contains("unallocated"))
            {
                id = "GridUnallocated";
            }
            else 
            {
                id = "GridAllocated";
            }
            try
            {
                UIAutomationHelper ui = new UIAutomationHelper();
                ui.Maximize("AllocationClientWindow");
                IUIAutomationElement appWindow = automation.GetRootElement().FindFirst(
                    TreeScope.TreeScope_Children,
                    automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Nirvana"));
                IUIAutomationElement gridElement = appWindow.FindFirst(
                        TreeScope.TreeScope_Descendants,
                        automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, id));

                if (gridElement == null)
                {
                    throw new Exception("Grid not found!");
                }
                IUIAutomationCondition dataItemCondition = automation.CreatePropertyCondition(
                            UIA_PropertyIds.UIA_ControlTypePropertyId,
                            UIA_ControlTypeIds.UIA_DataItemControlTypeId);

                IUIAutomationElementArray dataItems = gridElement.FindAll(
                    TreeScope.TreeScope_Subtree, dataItemCondition);

                var elem = dataItems.GetElement(idx);
                IUIAutomationCondition checkBoxCondition = automation.CreatePropertyCondition(
                    UIA_PropertyIds.UIA_ControlTypePropertyId,
                    UIA_ControlTypeIds.UIA_CheckBoxControlTypeId);

                IUIAutomationElement checkBoxElement = elem.FindFirst(
                    TreeScope.TreeScope_Subtree, checkBoxCondition);

                if (checkBoxElement == null)
                {
                    Console.WriteLine("Checkbox not found inside DataItem.");
                    return null;
                }

                // Get the TogglePattern
                object patternObj = checkBoxElement.GetCurrentPattern(UIA_PatternIds.UIA_TogglePatternId);
                IUIAutomationTogglePattern togglePattern = patternObj as IUIAutomationTogglePattern;

                if (togglePattern != null)
                {
                    ToggleState state = togglePattern.CurrentToggleState;
                    Console.WriteLine("Checkbox Toggle State: " + state.ToString());
                    return state.ToString();
                }
                else
                {
                    Console.WriteLine("TogglePattern not supported on checkbox.");
                }

                
            }
            catch { }
            return "";
        }

        /// <summary>
        /// Selects unallocated trades
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        public void SelectUnallocated(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            try
            {
                DataTable dtExportedTrades = new DataTable();
                Records = GetLatestGridObject(GridUnallocated);
                dtExportedTrades = ExportTrades(Records, TextBoxFilename1, ButtonSave, ConfirmSaveAs, ButtonYesCnfrmSave, false, false);
                if (dtExportedTrades != null)
                    SelectTrades(testData, sheetIndexToName, Records, dtExportedTrades);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Selects the allocated trades
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        public void SelectAllocated(DataSet testData, Dictionary<int, string> sheetIndexToName,bool checkFlag=false)
        {
            try
            {
                DataTable dtExportedTrades = new DataTable();
                Records1 = GetLatestGridObject(GridAllocated);
                dtExportedTrades = ExportTrades(Records1, TextBoxFilename, ButtonSave2, ConfirmSaveAs4, ButtonYes1, false, true);
                if (dtExportedTrades != null)
                    SelectTrades(testData, sheetIndexToName, Records1, dtExportedTrades,checkFlag);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Enters the date in the given control using calendar
        /// </summary>
        /// <param name="control"></param>
        /// <param name="date"></param>
        public void EnterDateFromCalendar(UIAutomationElement control, DateTime date)
        {
            try
            {
                ClickCalendar(control);
                ClickToday(PART_Calendar);
                Keyboard.SendKeys(KeyboardConstants.HOMEKEY);
                Wait(500);
                if (DateTime.Now.Month - date.Month > 0)
                    KeyboardUtilities.PressDownKeyWithWait(Math.Abs(DateTime.Now.Month - date.Month));
                else
                    KeyboardUtilities.PressUpKeyWithWait(Math.Abs(DateTime.Now.Month - date.Month));
                Keyboard.SendKeys(KeyboardConstants.RIGHT_ARROWKEY);
                Keyboard.SendKeys(KeyboardConstants.RIGHT_ARROWKEY);
                Keyboard.SendKeys(KeyboardConstants.RIGHT_ARROWKEY);
                Wait(500);
                if (DateTime.Now.Day - date.Day > 0)
                    KeyboardUtilities.PressDownKeyWithWait(Math.Abs(DateTime.Now.Day - date.Day));
                else
                    KeyboardUtilities.PressUpKeyWithWait(Math.Abs(DateTime.Now.Day - date.Day));
                Keyboard.SendKeys(KeyboardConstants.RIGHT_ARROWKEY);
                Keyboard.SendKeys(KeyboardConstants.RIGHT_ARROWKEY);
                Keyboard.SendKeys(KeyboardConstants.RIGHT_ARROWKEY);
                Wait(500);
                if (DateTime.Now.Year - date.Year > 0)
                    KeyboardUtilities.PressDownKeyWithWait(Math.Abs(DateTime.Now.Year - date.Year));
                else
                    KeyboardUtilities.PressUpKeyWithWait(Math.Abs(DateTime.Now.Year - date.Year));
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// click on drop down button to reveal calendar
        /// </summary>
        /// <param name="parentAutomationElement"></param>
        public void ClickCalendar(UIAutomationElement parentAutomationElement)
        {
            try
            {
                UIControlPart dropdownButton = new UIControlPart();
                int x = parentAutomationElement.RelativeCenterPoint.X;
                dropdownButton.BoundsInParent = new System.Drawing.Rectangle(x * 2 - 14, 0, 14, 17);
                dropdownButton.Comment = null;
                dropdownButton.ControlPartProvider = null;
                dropdownButton.Name = "dropdownButton";
                dropdownButton.ObjectImage = null;
                dropdownButton.Parent = parentAutomationElement;
                dropdownButton.Path = null;
                dropdownButton.UIObjectType = TestAutomationFX.UI.UIObjectTypes.ControlPart;
                dropdownButton.UseCoordinatesOnClick = false;
                dropdownButton.Click(MouseButtons.Left);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// clicks on today of the calendar
        /// </summary>
        /// <param name="parentAutomationElement"></param>
        public void ClickToday(UIAutomationElement parentAutomationElement)
        {
            try
            {
                UIControlPart dropdownButton = new UIControlPart();
                int x = parentAutomationElement.RelativeCenterPoint.X;
                int y = parentAutomationElement.RelativeCenterPoint.Y;
                dropdownButton.BoundsInParent = new System.Drawing.Rectangle(40, y * 2 - 20, x * 2 - 80, 20);
                dropdownButton.Comment = null;
                dropdownButton.ControlPartProvider = null;
                dropdownButton.Name = "dropdownButton";
                dropdownButton.ObjectImage = null;
                dropdownButton.Parent = parentAutomationElement;
                dropdownButton.Path = null;
                dropdownButton.UIObjectType = TestAutomationFX.UI.UIObjectTypes.ControlPart;
                dropdownButton.UseCoordinatesOnClick = false;
                dropdownButton.Click(MouseButtons.Left);
                Wait(2000);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Enters values in different types of controls
        /// </summary>
        /// <param name="control"></param>
        /// <param name="value"></param>
        public void EnterValueInControl(UIAutomationElement control, string value)
        {
            try
            {
                DateTime date;
                if (control.ClassName.Equals("ComboBox"))
                {
                    Clickdropdownbutton(control);
                    Wait(500);
                    ClickOnComboBoxItem(value, control);
                    Wait(500);
                    if (NirvanaAllocation.IsVisible)
                    {
                        ButtonYes.Click(MouseButtons.Left);
                        Wait(3000);
                        EditTrade.Click(MouseButtons.Left);
                    }
                   
                }
                else if (control.ClassName.Equals("XamComboEditor"))
                {
                    control.Click(MouseButtons.Left);
                    Wait(500);
                    ExtentionMethods.UpdateCellValueConditions(value, "");
                    Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                    Wait(500);
                }
                else if (control.ClassName.Equals("XamMaskedEditor") && DateTime.TryParseExact(value,"MM/DD/YYYY",CultureInfo.InvariantCulture,DateTimeStyles.AssumeLocal,out date))
                {
                    EnterDateFromCalendar(control, date);
                }
                else
                {
                    control.Click(MouseButtons.Left);
                    ExtentionMethods.UpdateCellValueConditions(value, "", control);
                    if (Warning1.IsVisible)
                    {
                        ButtonOK1.Click(MouseButtons.Left);
                    }
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
        /// Enters the value in grid.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="value">The value.</param>
        /// <param name="isTradeAttribute">if set to <c>true</c> [is trade attribute].</param>
        public void EnterValueInGrid(int index, string value,bool isTradeAttribute)
         {
             try
             {
                 while (index-- > 0)
                 {
                     Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                     Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                     Keyboard.SendKeys(KeyboardConstants.TABKEY);
                 }
                 if (isTradeAttribute)
                 {
                     Keyboard.SendKeys(KeyboardConstants.TABKEY);
                     Keyboard.SendKeys(KeyboardConstants.CTRLA);
                 }
                 Keyboard.SendKeys(value);
                 Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                 Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
             }
             catch (Exception ex)
             {
                 bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                 if (rethrow)
                     throw;
             }
         }

        public void SamsaraTestDataHandler(string StepName, DataTable UIData, DataTable ExcelData, List<String> columns)
        {
            try
            {       
                    DataSet columMapDs = DataUtilities.GetTestCaseTestData(ApplicationArguments.columnMappingFile, 1, 1, columns);
                    Nirvana.TestAutomation.Utilities.SamsaraCustomizableVerificationHandler.LinkExcelData(ref ExcelData);
                    SamsaraCustomizableVerificationHandler.CustomizableVerificationHandler(ref StepName, columMapDs.Tables["VerificationHandlerOnEnterprise"], ref  UIData);
                    
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occur on SamsaraTestDataHandler :"+ex.Message );
            }
        }
    }
}

