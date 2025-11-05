using Nirvana.TestAutomation.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using UIA;
using UIAutomationClient;
using WindowsInput;
using WindowsInput.Native;
using TestAutomationFX.Core;
using System.Diagnostics;
using System.Windows.Forms;
using System.Configuration;

namespace Nirvana.TestAutomation.UIAutomation
{
    public class GridDataProvider
    {
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        private static readonly IUIAutomation Automation = new CUIAutomation();
        private readonly IUIAutomation _automation;

        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, UIntPtr dwExtraInfo);

        public const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        public const uint MOUSEEVENTF_LEFTUP = 0x0004;

        private const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
        private const int MOUSEEVENTF_RIGHTUP = 0x0010;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern void mouse_event(uint dwFlags, int dx, int dy, uint dwData, int dwExtraInfo);


        public GridDataProvider()
        {
            try
            {
                _automation = new CUIAutomation();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error initializing UIAutomation: " + ex.Message);
            }
        }

        public static DataTable GetMTTGrid(string GridID)
        {
            DataTable dt = new DataTable();
            IUIAutomationElement appWindow = Automation.GetRootElement().FindFirst(
                TreeScope.TreeScope_Children,
                Automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Nirvana"));
            IUIAutomationElement gridElement = appWindow.FindFirst(
                TreeScope.TreeScope_Descendants,
                Automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, GridID));
            if (gridElement == null)
            {
                throw new Exception("MTT Grid not found!");
            }

            IUIAutomationElement Headers = gridElement.FindFirst(
                        TreeScope.TreeScope_Descendants,
                        Automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "Band Headers"));

            if (Headers != null)
            {
                IUIAutomationElementArray childElements = Headers.FindAll(
                    TreeScope.TreeScope_Children,
                    Automation.CreateTrueCondition()
                );

                for (int i = 0; i < childElements.Length; i++)
                {
                    IUIAutomationElement child = childElements.GetElement(i);
                    if (!string.IsNullOrEmpty(child.CurrentName.ToString()))
                    {
                        dt.Columns.Add(child.CurrentName);
                    }
                }
            }
            IUIAutomationElementArray childElement = gridElement.FindAll(
                TreeScope.TreeScope_Children,
                Automation.CreateTrueCondition());

            for (int i = 0; i < childElement.Length; i++)
            {
                IUIAutomationElement child = childElement.GetElement(i);
                if (child.CurrentAutomationId.ToString().Equals("Band Headers"))
                {
                    continue;
                }
                var rawChildren = child.FindAll(TreeScope.TreeScope_Subtree, Automation.CreateTrueCondition());
                DataRow dr = dt.NewRow();
                for (int j = 0; j < rawChildren.Length; j++)
                {
                    if (rawChildren.GetElement(j).CurrentControlType == UIA_ControlTypeIds.UIA_EditControlTypeId || rawChildren.GetElement(j).CurrentControlType == UIA_ControlTypeIds.UIA_ComboBoxControlTypeId)
                    {
                        string columnName = rawChildren.GetElement(j).CurrentName;
                        if (string.IsNullOrEmpty(columnName))
                            continue;
                        object valuePatternObj = rawChildren.GetElement(j).GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId);
                        if (valuePatternObj != null)
                        {
                            IUIAutomationValuePattern valuePattern = valuePatternObj as IUIAutomationValuePattern;
                            string value = valuePattern.CurrentValue;
                            dr[columnName] = value;
                        }
                    }
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
        public void EditGrid(string GridID, DataTable testCaseDataTable)
        {
            DataTable tempTestData = testCaseDataTable.Copy();
            tempTestData.Columns.Remove("Symbol Validity");
            tempTestData.Columns.Remove("CheckBox");

            DataTable UiDataTable = GetWPFGridData("RebalancerWindow", "ModelPortfolioGrid");
            if (UiDataTable.Columns.Contains("Tolerance BPS"))
            {
                UiDataTable.Columns["Tolerance BPS"].ColumnName = "Tolerance %";
            }
            IUIAutomationElement appWindow = Automation.GetRootElement().FindFirst(
                TreeScope.TreeScope_Children,
                Automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Nirvana"));
            IUIAutomationElement gridElement = appWindow.FindFirst(
                    TreeScope.TreeScope_Descendants,
                    Automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, GridID));
            for (int i = 0; i < tempTestData.Rows.Count; i++)
            {
                DataRow dr = tempTestData.Rows[i];
                int index = -1;
                if (!string.IsNullOrEmpty(dr["Action"].ToString()))
                {
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
                        dr["Action"] = "Select";
                        IUIAutomationElement childElement = gridElement.FindFirst(
                                TreeScope.TreeScope_Children,
                                Automation.CreateTrueCondition()
                            );
                        IUIAutomationElement child1Element = childElement.FindFirst(
                                TreeScope.TreeScope_Children,
                                Automation.CreateTrueCondition()
                            );
                        IUIAutomationElementArray rawChildren = child1Element.FindAll(
                            TreeScope.TreeScope_Children,
                            Automation.CreateTrueCondition());
                        object selectObject = rawChildren.GetElement(index + 1).GetCurrentPattern(UIA_PatternIds.UIA_SelectionItemPatternId);
                        if (selectObject != null)
                        {
                            IUIAutomationSelectionItemPattern selectObjectPattern = (IUIAutomationSelectionItemPattern)selectObject;
                            selectObjectPattern.Select();
                        }

                        IUIAutomationElementArray elementsToEdit = rawChildren.GetElement(index + 1).FindAll(
                            TreeScope.TreeScope_Children,
                            Automation.CreateTrueCondition()
                        );
                        bool Symbol = false;
                        bool Target = false;
                        bool Tolerance = false;
                        for (int j = 2; j < elementsToEdit.Length; j++)
                        {
                            IUIAutomationElement ele = elementsToEdit.GetElement(j);
                            if (!Symbol && (!string.IsNullOrEmpty(testCaseDataTable.Rows[i + 1]["Symbol"].ToString()) && ele.CurrentName.Equals(dr["Symbol"].ToString())))
                            {
                                Symbol = true;
                                selectObject = ele.GetCurrentPattern(UIA_PatternIds.UIA_SelectionItemPatternId);
                                if (selectObject != null)
                                {
                                    IUIAutomationSelectionItemPattern selectObjectPattern = (IUIAutomationSelectionItemPattern)selectObject;
                                    selectObjectPattern.Select();
                                }
                                click(ele);
                                DataUtilities.clearTextData();
                                Keyboard.SendKeys(testCaseDataTable.Rows[i + 1]["Symbol"].ToString());
                                testCaseDataTable.Rows[i]["Symbol"] = testCaseDataTable.Rows[i + 1]["Symbol"].ToString();
                                Keyboard.SendKeys(KeyboardConstants.TABKEY);
                            }

                            if (!Target && (!string.IsNullOrEmpty(testCaseDataTable.Rows[i + 1]["Target %"].ToString()) && ele.CurrentName.Equals(dr["Target %"].ToString())))
                            {
                                Target = true;
                                selectObject = ele.GetCurrentPattern(UIA_PatternIds.UIA_SelectionItemPatternId);
                                if (selectObject != null)
                                {
                                    IUIAutomationSelectionItemPattern selectObjectPattern = (IUIAutomationSelectionItemPattern)selectObject;
                                    selectObjectPattern.Select();
                                }
                                click(ele);
                                DataUtilities.clearTextData();
                                Keyboard.SendKeys(testCaseDataTable.Rows[i + 1]["Target %"].ToString());
                                testCaseDataTable.Rows[i]["Target %"] = testCaseDataTable.Rows[i + 1]["Target %"].ToString();
                                Keyboard.SendKeys(KeyboardConstants.TABKEY);

                            }

                            if (!Tolerance && (!string.IsNullOrEmpty(testCaseDataTable.Rows[i + 1]["Tolerance %"].ToString()) && ele.CurrentName.Equals(dr["Tolerance %"].ToString())))
                            {
                                Tolerance = true;
                                selectObject = ele.GetCurrentPattern(UIA_PatternIds.UIA_SelectionItemPatternId);
                                if (selectObject != null)
                                {
                                    IUIAutomationSelectionItemPattern selectObjectPattern = (IUIAutomationSelectionItemPattern)selectObject;
                                    selectObjectPattern.Select();
                                }
                                click(ele);
                                DataUtilities.clearTextData();
                                Keyboard.SendKeys(testCaseDataTable.Rows[i + 1]["Tolerance %"].ToString());
                                testCaseDataTable.Rows[i]["Tolerance %"] = testCaseDataTable.Rows[i + 1]["Tolerance %"].ToString();
                                Keyboard.SendKeys(KeyboardConstants.TABKEY);
                            }
                        }
                    }
                    else
                    {
                        IUIAutomationElement childElement = gridElement.FindFirst(
                                    TreeScope.TreeScope_Children,
                                    Automation.CreateTrueCondition()
                                );
                        IUIAutomationElement child1Element = childElement.FindFirst(
                                TreeScope.TreeScope_Children,
                                Automation.CreateTrueCondition()
                            );
                        IUIAutomationElementArray rawChildren = child1Element.FindAll(
                            TreeScope.TreeScope_Children,
                            Automation.CreateTrueCondition());
                        object selectObject = rawChildren.GetElement(i + 1).GetCurrentPattern(UIA_PatternIds.UIA_SelectionItemPatternId);
                        if (selectObject != null)
                        {
                            IUIAutomationSelectionItemPattern selectObjectPattern = (IUIAutomationSelectionItemPattern)selectObject;
                            selectObjectPattern.Select();
                        }
                        IUIAutomationElementArray elementsToEdit = rawChildren.GetElement(i + 1).FindAll(
                            TreeScope.TreeScope_Children,
                            Automation.CreateTrueCondition()
                        );
                        bool Symbol = false;
                        bool Target = false;
                        bool Tolerance = false;
                        if (!string.IsNullOrEmpty(testCaseDataTable.Rows[i]["Symbol"].ToString()))
                        {
                            IUIAutomationElement ele = elementsToEdit.GetElement(UiDataTable.Columns.IndexOf("Symbol") + 1);
                            selectObject = ele.GetCurrentPattern(UIA_PatternIds.UIA_SelectionItemPatternId);
                            if (selectObject != null)
                            {
                                IUIAutomationSelectionItemPattern selectObjectPattern = (IUIAutomationSelectionItemPattern)selectObject;
                                selectObjectPattern.Select();
                            }
                            click(ele);
                            DataUtilities.clearTextData();
                            Keyboard.SendKeys(testCaseDataTable.Rows[i]["Symbol"].ToString());
                            Keyboard.SendKeys(KeyboardConstants.TABKEY);
                        }
                        if (!string.IsNullOrEmpty(testCaseDataTable.Rows[i]["Target %"].ToString()))
                        {
                            IUIAutomationElement ele = elementsToEdit.GetElement(UiDataTable.Columns.IndexOf("Target %") + 1);
                            selectObject = ele.GetCurrentPattern(UIA_PatternIds.UIA_SelectionItemPatternId);
                            if (selectObject != null)
                            {
                                IUIAutomationSelectionItemPattern selectObjectPattern = (IUIAutomationSelectionItemPattern)selectObject;
                                selectObjectPattern.Select();
                            }
                            click(ele);
                            DataUtilities.clearTextData();
                            Keyboard.SendKeys(testCaseDataTable.Rows[i]["Target %"].ToString());
                            Keyboard.SendKeys(KeyboardConstants.TABKEY);
                        }
                        if (!string.IsNullOrEmpty(testCaseDataTable.Rows[i]["Tolerance %"].ToString()))
                        {
                            IUIAutomationElement ele = elementsToEdit.GetElement(UiDataTable.Columns.IndexOf("Tolerance %") + 1);
                            selectObject = ele.GetCurrentPattern(UIA_PatternIds.UIA_SelectionItemPatternId);
                            if (selectObject != null)
                            {
                                IUIAutomationSelectionItemPattern selectObjectPattern = (IUIAutomationSelectionItemPattern)selectObject;
                                selectObjectPattern.Select();
                            }
                            click(ele);
                            DataUtilities.clearTextData();
                            Keyboard.SendKeys(testCaseDataTable.Rows[i]["Tolerance %"].ToString());
                            Keyboard.SendKeys(KeyboardConstants.TABKEY);
                        }
                    }
                }

            }
            for (int i = testCaseDataTable.Rows.Count - 1; i >= 1; i--)
            {
                if (testCaseDataTable.Rows[i - 1]["Action"].ToString().ToUpper().Equals("SELECT"))
                {
                    testCaseDataTable.Rows[i].Delete();
                }
                else if (!string.IsNullOrEmpty(testCaseDataTable.Rows[i]["Action"].ToString()))
                {
                    testCaseDataTable.Rows[i].Delete();
                }
            }
        }
        public static void doubleClick(IUIAutomationElement Element)
        {
            double left = Element.CurrentBoundingRectangle.left;
            double top = Element.CurrentBoundingRectangle.top;
            double right = Element.CurrentBoundingRectangle.right;
            double bottom = Element.CurrentBoundingRectangle.bottom;

            int centerX = (int)((left + right) / 2);
            int centerY = (int)((top + bottom) / 2);

            SetCursorPos(centerX, centerY);
            System.Threading.Thread.Sleep(1000);
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 0, 0, 0, UIntPtr.Zero);
            Thread.Sleep(100);
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 0, 0, 0, UIntPtr.Zero);

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
            System.Threading.Thread.Sleep(1000);
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, UIntPtr.Zero);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, UIntPtr.Zero);
        }


        public static bool DetectAndSwitchWindow(string automationId)
        {
            try
            {
                IUIAutomation automation = new CUIAutomation();
                IUIAutomationElement rootElement = automation.GetRootElement();
                IUIAutomationCondition condition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, automationId);
                IUIAutomationElement targetElement = rootElement.FindFirst(TreeScope.TreeScope_Descendants, condition);

                if (targetElement != null)
                {
                    IntPtr hWnd = (IntPtr)targetElement.CurrentNativeWindowHandle;
                    SetForegroundWindow(hWnd);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occurred: " + ex.Message);
                return false;
            }
        }

        public DataTable GetWPFGridData(string moduleID)
        {
            DataTable dt = new DataTable();
            try
            {
                IUIAutomationElement selectedRow = null;

                ApplicationArguments.mapdictionary = DataUtilities.CreateDictionaryFromDataTable(ApplicationArguments.IUIAutomationMappingTables[moduleID]);
                foreach (var entry in ApplicationArguments.mapdictionary)
                {
                    Console.WriteLine("DataName: " + entry.Key +
                   ", AutomationUniqueValue: " + entry.Value.AutomationUniqueValue +
                   ", UniquePropertyType: " + entry.Value.UniquePropertyType +
                   ", ControlType: " + entry.Value.ControlType);

                }

                IUIAutomation automation = new CUIAutomation();
                IUIAutomationElement rootElement = automation.GetRootElement();

                IUIAutomationElement windowElement = null;
                try
                {
                    windowElement = UIAutomationHelper.FindElementByUniquePropertyType(automation, rootElement, ApplicationArguments.mapdictionary["ModuleWindow"].UniquePropertyType, ApplicationArguments.mapdictionary["ModuleWindow"].AutomationUniqueValue);
                }
                catch (Exception ex)
                {
                    throw new Exception(" Window not visible");

                }

                if (windowElement != null)
                {
                    DetectAndSwitchWindow(ApplicationArguments.mapdictionary["ModuleWindow"].AutomationUniqueValue);

                    IUIAutomationCondition grdCondition = automation.CreatePropertyCondition(
                                    UIA_PropertyIds.UIA_AutomationIdPropertyId, ApplicationArguments.mapdictionary["GridName"].AutomationUniqueValue);
                    IUIAutomationElement gridElementMain = windowElement.FindFirst(TreeScope.TreeScope_Descendants, grdCondition);


                    if (gridElementMain == null)
                    {
                        Console.WriteLine("Grid element with AutomationId " + ApplicationArguments.mapdictionary["PendingApprovalGrid"].AutomationUniqueValue + "not found.");
                        throw new Exception("Grid element with AutomationId " + ApplicationArguments.mapdictionary["PendingApprovalGrid"].AutomationUniqueValue + "not found.");
                    }
                    else
                    {

                        Console.WriteLine(gridElementMain.CurrentName);
                        Console.WriteLine(gridElementMain.CurrentAutomationId);
                        IUIAutomationCondition gridControlTypeCondition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_DataGridControlTypeId);
                        IUIAutomationCondition nameCondition = automation.CreatePropertyCondition(
           UIA_PropertyIds.UIA_NamePropertyId,
           ApplicationArguments.mapdictionary["FirstDataGridName"].AutomationUniqueValue
       );

                        IUIAutomationCondition combinedCondition = automation.CreateAndCondition(gridControlTypeCondition, nameCondition);

                        IUIAutomationElement recordElement = gridElementMain.FindFirst(TreeScope.TreeScope_Descendants, gridControlTypeCondition);
                        IUIAutomationElement gridElement = gridElementMain.FindFirst(TreeScope.TreeScope_Descendants, combinedCondition);

                        if (gridElement == null)
                        {
                            IUIAutomationCondition listControlCondition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_ListControlTypeId);
                            gridElement = gridElementMain.FindFirst(TreeScope.TreeScope_Descendants, listControlCondition);
                        }

                        IUIAutomationElement customControlTypeID = null;


                        IUIAutomationElement buttonControlTypeID = null;

                        Console.WriteLine(recordElement.CurrentName);
                        Console.WriteLine(recordElement.CurrentLocalizedControlType);
                        Console.WriteLine(recordElement.CurrentAutomationId);

                        Console.WriteLine(gridElement.CurrentName);

                        Console.WriteLine(gridElement.CurrentAutomationId);


                        ////scroll
                        ///
                        IUIAutomationScrollPattern scrollPattern = null;
                        bool isScrollPatternAvailable = false;

                        try
                        {
                            scrollPattern = (IUIAutomationScrollPattern)recordElement.GetCurrentPattern(UIA_PatternIds.UIA_ScrollPatternId);
                            isScrollPatternAvailable = (scrollPattern != null);
                            if (!isScrollPatternAvailable)
                            {
                                scrollPattern = (IUIAutomationScrollPattern)gridElement.GetCurrentPattern(UIA_PatternIds.UIA_ScrollPatternId);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("ScrollPattern is not available or an error occurred: " + ex.Message);
                        }



                        IUIAutomationElementArray gridChildren = gridElement.FindAll(TreeScope.TreeScope_Children, _automation.CreateTrueCondition());
                        for (int i = 0; i < gridChildren.Length; i++)
                        {
                            DataRow dr = dt.NewRow();
                            IUIAutomationElement child = gridChildren.GetElement(i);
                            try
                            {
                                if (child.CurrentControlType == UIA_ControlTypeIds.UIA_HeaderControlTypeId)
                                {
                                    IUIAutomationElementArray columnHeaders = child.FindAll(TreeScope.TreeScope_Children, _automation.CreateTrueCondition());
                                    for (int j = 0; j < columnHeaders.Length; j++)
                                    {
                                        Console.WriteLine(columnHeaders.GetElement(j).CurrentName);
                                        if (columnHeaders.GetElement(j).CurrentControlType == UIA_ControlTypeIds.UIA_HeaderItemControlTypeId)
                                        {
                                            IUIAutomationElement column = columnHeaders.GetElement(j);
                                            if (string.Equals(column.CurrentName, "true", StringComparison.OrdinalIgnoreCase) || string.Equals(column.CurrentName, "false", StringComparison.OrdinalIgnoreCase))
                                            {
                                                if (!dt.Columns.Contains("CheckBox"))
                                                    dt.Columns.Add("CheckBox", typeof(string));
                                            }
                                            else
                                            {
                                                if (!dt.Columns.Contains(column.CurrentName))
                                                    dt.Columns.Add(column.CurrentName, typeof(string));
                                            }
                                        }
                                        else if (columnHeaders.GetElement(j).CurrentControlType == UIA_ControlTypeIds.UIA_ButtonControlTypeId)
                                        {
                                            IUIAutomationElement column = columnHeaders.GetElement(j);
                                            if (!dt.Columns.Contains("Button"))
                                                dt.Columns.Add("Button", typeof(string));
                                        }
                                    }
                                }
                                else if (child.CurrentControlType == UIA_ControlTypeIds.UIA_DataItemControlTypeId)
                                {
                                    selectedRow = child;
                                    bool result = false;

                                    IUIAutomationElementArray rowChildren = selectedRow.FindAll(TreeScope.TreeScope_Children, _automation.CreateTrueCondition());
                                    for (int k = 0; k < rowChildren.Length; k++)
                                    {
                                        try
                                        {
                                            string tableColumnValue = null;
                                            string foundElementValue = ControlTypeHandler.getValueOfElement(rowChildren.GetElement(k), ref result);
                                            IUIAutomationElement cell = rowChildren.GetElement(k);
                                            object patternprovider;

                                            if (cell.GetCurrentPattern(UIA_PatternIds.UIA_TableItemPatternId) != null)
                                            {
                                                patternprovider = cell.GetCurrentPattern(UIA_PatternIds.UIA_TableItemPatternId);
                                                IUIAutomationTableItemPattern selectionpatternprovider = patternprovider as IUIAutomationTableItemPattern;
                                                tableColumnValue = selectionpatternprovider.GetCurrentColumnHeaderItems().GetElement(0).CurrentName;
                                                Console.WriteLine("COLUMN  NAME   :  " + tableColumnValue);
                                                Console.WriteLine("CELL VALUE   :   " + cell.CurrentName);
                                            }

                                            if (rowChildren.GetElement(k).CurrentControlType == UIA_ControlTypeIds.UIA_CustomControlTypeId)
                                            {
                                                IUIAutomationElement column = rowChildren.GetElement(k);

                                                if (string.Equals(column.CurrentName, "true", StringComparison.OrdinalIgnoreCase) || string.Equals(column.CurrentName, "false", StringComparison.OrdinalIgnoreCase))
                                                {
                                                    if (dt.Columns.Contains("CheckBox"))
                                                        dr["CheckBox"] = foundElementValue;
                                                }
                                                else
                                                {
                                                    if (dt.Columns.Contains(tableColumnValue))
                                                        dr[tableColumnValue] = foundElementValue;
                                                }
                                            }
                                            else if (rowChildren.GetElement(k).CurrentControlType == UIA_ControlTypeIds.UIA_HeaderControlTypeId)
                                            {
                                                IUIAutomationElement column = rowChildren.GetElement(k);

                                                if (dt.Columns.Contains("Header"))
                                                    dr["Header"] = foundElementValue;

                                            }
                                            else if (rowChildren.GetElement(k).CurrentControlType == UIA_ControlTypeIds.UIA_ButtonControlTypeId)
                                            {
                                                IUIAutomationElement column = rowChildren.GetElement(k);
                                                if (dt.Columns.Contains("Button"))
                                                    dr["Button"] = foundElementValue;
                                            }


                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.ToString());
                                        }


                                    }
                                }



                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Error processing grid child elements: " + ex.Message);
                            }

                            bool hasNonEmptyValue = false;

                            foreach (var item in dr.ItemArray)
                            {
                                if (item != null && !string.IsNullOrEmpty(item.ToString()))
                                {
                                    hasNonEmptyValue = true;
                                    break;
                                }
                            }

                            if (hasNonEmptyValue)
                            {
                                dt.Rows.Add(dr);
                            }



                        }
                    }
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine("Exception occurred: " + ex.Message);
                return null;
            }
            return dt;
        }
        public DataTable GetWPFGridData(string moduleID, string gridID)
        {
            DataTable dt = new DataTable();
            try
            {
                IUIAutomationElement selectedRow = null;

                ApplicationArguments.mapdictionary = DataUtilities.CreateDictionaryFromDataTable(ApplicationArguments.IUIAutomationMappingTables[moduleID]);
                foreach (var entry in ApplicationArguments.mapdictionary)
                {
                    Console.WriteLine("DataName: " + entry.Key +
                   ", AutomationUniqueValue: " + entry.Value.AutomationUniqueValue +
                   ", UniquePropertyType: " + entry.Value.UniquePropertyType +
                   ", ControlType: " + entry.Value.ControlType);

                }

                IUIAutomation automation = new CUIAutomation();
                IUIAutomationElement rootElement = automation.GetRootElement();

                IUIAutomationElement windowElement = null;
                try
                {
                    windowElement = UIAutomationHelper.FindElementByUniquePropertyType(automation, rootElement, ApplicationArguments.mapdictionary["ModuleWindow"].UniquePropertyType, ApplicationArguments.mapdictionary["ModuleWindow"].AutomationUniqueValue);
                }
                catch (Exception ex)
                {
                    throw new Exception(" Window not visible");

                }

                if (windowElement != null)
                {
                    DetectAndSwitchWindow(ApplicationArguments.mapdictionary["ModuleWindow"].AutomationUniqueValue);

                    IUIAutomationCondition grdCondition = automation.CreatePropertyCondition(
                                    UIA_PropertyIds.UIA_AutomationIdPropertyId, ApplicationArguments.mapdictionary[gridID + "GridName"].AutomationUniqueValue);
                    IUIAutomationElement gridElementMain = windowElement.FindFirst(TreeScope.TreeScope_Descendants, grdCondition);


                    if (gridElementMain == null)
                    {
                        Console.WriteLine("Grid element with AutomationId " + ApplicationArguments.mapdictionary[gridID + "GridName"].AutomationUniqueValue + "not found.");
                        throw new Exception("Grid element with AutomationId " + ApplicationArguments.mapdictionary[gridID + "GridName"].AutomationUniqueValue + "not found.");
                    }
                    else
                    {

                        Console.WriteLine(gridElementMain.CurrentName);
                        Console.WriteLine(gridElementMain.CurrentAutomationId);
                        IUIAutomationCondition gridControlTypeCondition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_DataGridControlTypeId);
                        IUIAutomationCondition nameCondition = automation.CreatePropertyCondition(
           UIA_PropertyIds.UIA_NamePropertyId,
           ApplicationArguments.mapdictionary[gridID + "FirstDataGridName"].AutomationUniqueValue
       );

                        IUIAutomationCondition combinedCondition = automation.CreateAndCondition(gridControlTypeCondition, nameCondition);

                        IUIAutomationElement recordElement = gridElementMain.FindFirst(TreeScope.TreeScope_Descendants, gridControlTypeCondition);
                        IUIAutomationElement gridElement = gridElementMain.FindFirst(TreeScope.TreeScope_Descendants, combinedCondition);

                        if (gridElement == null)
                        {
                            IUIAutomationCondition listControlCondition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_ListControlTypeId);
                            gridElement = gridElementMain.FindFirst(TreeScope.TreeScope_Descendants, listControlCondition);
                        }
                        if (gridElement == null)
                        {
                            IUIAutomationElementArray recordElementChildren = recordElement.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());
                            if (recordElementChildren != null)
                            {
                                gridElement = recordElement;
                            }
                        }


                        ////scroll
                        ///
                        IUIAutomationScrollPattern scrollPattern = null;
                        bool isScrollPatternAvailable = false;

                        try
                        {
                            scrollPattern = (IUIAutomationScrollPattern)recordElement.GetCurrentPattern(UIA_PatternIds.UIA_ScrollPatternId);
                            isScrollPatternAvailable = (scrollPattern != null);
                            if (!isScrollPatternAvailable)
                            {
                                scrollPattern = (IUIAutomationScrollPattern)gridElement.GetCurrentPattern(UIA_PatternIds.UIA_ScrollPatternId);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("ScrollPattern is not available or an error occurred: " + ex.Message);
                        }



                        IUIAutomationElementArray gridChildren = gridElement.FindAll(TreeScope.TreeScope_Children, _automation.CreateTrueCondition());
                        for (int i = 0; i < gridChildren.Length; i++)
                        {
                            DataRow dr = dt.NewRow();
                            IUIAutomationElement child = gridChildren.GetElement(i);
                            try
                            {
                                if (child.CurrentControlType == UIA_ControlTypeIds.UIA_HeaderControlTypeId)
                                {
                                    IUIAutomationElementArray columnHeaders = child.FindAll(TreeScope.TreeScope_Children, _automation.CreateTrueCondition());
                                    for (int j = 0; j < columnHeaders.Length; j++)
                                    {
                                        Console.WriteLine(columnHeaders.GetElement(j).CurrentName);
                                        if (columnHeaders.GetElement(j).CurrentControlType == UIA_ControlTypeIds.UIA_HeaderItemControlTypeId)
                                        {
                                            IUIAutomationElement column = columnHeaders.GetElement(j);
                                            if (string.Equals(column.CurrentName, "true", StringComparison.OrdinalIgnoreCase) || string.Equals(column.CurrentName, "false", StringComparison.OrdinalIgnoreCase))
                                            {
                                                if (!dt.Columns.Contains("CheckBox"))
                                                    dt.Columns.Add("CheckBox", typeof(string));
                                            }
                                            else
                                            {
                                                if (!dt.Columns.Contains(column.CurrentName))
                                                    dt.Columns.Add(column.CurrentName, typeof(string));
                                            }
                                        }
                                        else if (columnHeaders.GetElement(j).CurrentControlType == UIA_ControlTypeIds.UIA_ButtonControlTypeId)
                                        {
                                            IUIAutomationElement column = columnHeaders.GetElement(j);
                                            if (!dt.Columns.Contains("Button"))
                                                dt.Columns.Add("Button", typeof(string));
                                        }
                                    }
                                }
                                else if (child.CurrentControlType == UIA_ControlTypeIds.UIA_DataItemControlTypeId)
                                {
                                    selectedRow = child;
                                    bool result = false;

                                    IUIAutomationElementArray rowChildren = selectedRow.FindAll(TreeScope.TreeScope_Children, _automation.CreateTrueCondition());
                                    for (int k = 0; k < rowChildren.Length; k++)
                                    {
                                        try
                                        {
                                            string tableColumnValue = null;
                                            string foundElementValue = ControlTypeHandler.getValueOfElement(rowChildren.GetElement(k), ref result);
                                            IUIAutomationElement cell = rowChildren.GetElement(k);
                                            object patternprovider;

                                            if (cell.GetCurrentPattern(UIA_PatternIds.UIA_TableItemPatternId) != null)
                                            {
                                                patternprovider = cell.GetCurrentPattern(UIA_PatternIds.UIA_TableItemPatternId);
                                                IUIAutomationTableItemPattern selectionpatternprovider = patternprovider as IUIAutomationTableItemPattern;
                                                tableColumnValue = selectionpatternprovider.GetCurrentColumnHeaderItems().GetElement(0).CurrentName;
                                                Console.WriteLine("COLUMN  NAME   :  " + tableColumnValue);
                                                Console.WriteLine("CELL VALUE   :   " + cell.CurrentName);
                                            }

                                            if (rowChildren.GetElement(k).CurrentControlType == UIA_ControlTypeIds.UIA_CustomControlTypeId)
                                            {
                                                IUIAutomationElement column = rowChildren.GetElement(k);

                                                if (string.Equals(column.CurrentName, "true", StringComparison.OrdinalIgnoreCase) || string.Equals(column.CurrentName, "false", StringComparison.OrdinalIgnoreCase))
                                                {
                                                    if (dt.Columns.Contains("CheckBox"))
                                                        dr["CheckBox"] = foundElementValue;
                                                }
                                                else
                                                {
                                                    if (dt.Columns.Contains(tableColumnValue))
                                                        dr[tableColumnValue] = foundElementValue;
                                                }
                                            }
                                            else if (rowChildren.GetElement(k).CurrentControlType == UIA_ControlTypeIds.UIA_HeaderControlTypeId)
                                            {
                                                IUIAutomationElement column = rowChildren.GetElement(k);

                                                if (dt.Columns.Contains("Header"))
                                                    dr["Header"] = foundElementValue;

                                            }
                                            else if (rowChildren.GetElement(k).CurrentControlType == UIA_ControlTypeIds.UIA_ButtonControlTypeId)
                                            {
                                                IUIAutomationElement column = rowChildren.GetElement(k);
                                                if (dt.Columns.Contains("Button"))
                                                    dr["Button"] = foundElementValue;
                                            }


                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.ToString());
                                        }


                                    }
                                }



                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Error processing grid child elements: " + ex.Message);
                            }

                            bool hasNonEmptyValue = false;

                            foreach (var item in dr.ItemArray)
                            {
                                if (item != null && !string.IsNullOrEmpty(item.ToString()))
                                {
                                    hasNonEmptyValue = true;
                                    break;
                                }
                            }

                            if (hasNonEmptyValue)
                            {
                                dt.Rows.Add(dr);
                            }



                        }
                    }
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine("Exception occurred: " + ex.Message);
                return null;
            }
            return dt;
        }

        public void SelectAndEditWPFGrid(string moduleID, DataTable dt)
        {
            try
            {

                DataTable dtable = GetWPFGridData(moduleID);

                IUIAutomationElement selectedRow = null;

                foreach (DataRow dr in dt.Rows)
                {
                    try
                    {
                        string action = dr["Action"].ToString();

                        DataRow matchingRow = null;
                        dr["Action"] = "";

                        if (string.Equals(action, "Select", StringComparison.OrdinalIgnoreCase))
                        {
                            try
                            {
                                matchingRow = DataUtilities.GetMatchingDataRow(dtable, dr);
                                int index = dtable.Rows.IndexOf(matchingRow);
                                selectedRow = GetWPFGridRowElementByIndex(moduleID, dtable, index);


                            }
                            catch (Exception ex)
                            { Console.WriteLine(ex.Message); }
                        }
                        else if (string.Equals(action, "Edit", StringComparison.OrdinalIgnoreCase))
                        {
                            try
                            {
                                if (selectedRow == null)
                                {
                                    throw new Exception("Selected Row is null");
                                }
                                else
                                {
                                    EditWPFGrid(moduleID, dr, selectedRow);
                                }

                            }
                            catch (Exception ex)
                            { Console.WriteLine(ex.Message); }
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


        }
        public void EditWPFGrid(string moduleID, DataRow dr, IUIAutomationElement selectedRow)
        {
            try
            {
                IUIAutomationElementArray rowChildren = selectedRow.FindAll(TreeScope.TreeScope_Children, _automation.CreateTrueCondition());
                for (int k = 0; k < rowChildren.Length; k++)
                {
                    try
                    {
                        bool result = true;

                        string tableColumnValue = null;
                        string foundElementValue = ControlTypeHandler.getValueOfElement(rowChildren.GetElement(k), ref result);
                        IUIAutomationElement cell = rowChildren.GetElement(k);
                        object patternprovider;

                        if (cell.GetCurrentPattern(UIA_PatternIds.UIA_TableItemPatternId) != null)
                        {
                            patternprovider = cell.GetCurrentPattern(UIA_PatternIds.UIA_TableItemPatternId);
                            IUIAutomationTableItemPattern selectionpatternprovider = patternprovider as IUIAutomationTableItemPattern;
                            tableColumnValue = selectionpatternprovider.GetCurrentColumnHeaderItems().GetElement(0).CurrentName;
                            Console.WriteLine("COLUMN  NAME   :  " + tableColumnValue);
                            Console.WriteLine("CELL VALUE   :   " + cell.CurrentName);

                        }
                        if (tableColumnValue != null)
                        {
                            if (dr.Table.Columns.Contains(tableColumnValue) && !string.IsNullOrEmpty(dr[tableColumnValue].ToString()))
                            {
                                if (cell != null)
                                {
                                    SetTextInCell(cell, dr[tableColumnValue].ToString());

                                    int retries = 0;
                                    while (retries < 10)
                                    {
                                        bool result2 = true;

                                        string currentValue = ControlTypeHandler.getValueOfElement(cell, ref result2);
                                        if (string.Equals(currentValue, dr[tableColumnValue].ToString(), StringComparison.OrdinalIgnoreCase))
                                        {
                                            Console.WriteLine(String.Format("Value verified for column {0}: {1}", tableColumnValue, currentValue));

                                            break;
                                        }
                                        retries++;
                                    }
                                }
                            }

                        }



                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


        }


        public IUIAutomationElement GetWPFGridRowElementByIndex(string moduleID, DataTable dt, int index, string directgridID = null)
        {
            IUIAutomationElement selectedRow = null;
            try
            {
                int foundRowIndex = -1;
                try
                {
                    ApplicationArguments.mapdictionary = DataUtilities.CreateDictionaryFromDataTable(ApplicationArguments.IUIAutomationMappingTables[moduleID]);
                    foreach (var entry in ApplicationArguments.mapdictionary)
                    {
                        Console.WriteLine("DataName: " + entry.Key +
                   ", AutomationUniqueValue: " + entry.Value.AutomationUniqueValue +
                   ", UniquePropertyType: " + entry.Value.UniquePropertyType +
                   ", ControlType: " + entry.Value.ControlType);

                    }

                    IUIAutomation automation = new CUIAutomation();
                    IUIAutomationElement rootElement = automation.GetRootElement();

                    IUIAutomationElement windowElement = null;
                    try
                    {
                        windowElement = UIAutomationHelper.FindElementByUniquePropertyType(automation, rootElement, ApplicationArguments.mapdictionary["ModuleWindow"].UniquePropertyType, ApplicationArguments.mapdictionary["ModuleWindow"].AutomationUniqueValue);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("complianceEngineElement Window not vsisible");

                    }

                    if (windowElement != null)
                    {
                        DetectAndSwitchWindow(ApplicationArguments.mapdictionary["ModuleWindow"].AutomationUniqueValue);

                        IUIAutomationCondition grdCondition = automation.CreatePropertyCondition(
                                        UIA_PropertyIds.UIA_AutomationIdPropertyId, ApplicationArguments.mapdictionary["GridName"].AutomationUniqueValue);
                        IUIAutomationElement gridElementMain = windowElement.FindFirst(TreeScope.TreeScope_Descendants, grdCondition);
                        if (gridElementMain == null && !string.IsNullOrEmpty(directgridID))
                        {
                            IUIAutomationCondition grdCondition2 = automation.CreatePropertyCondition(
                                                          UIA_PropertyIds.UIA_AutomationIdPropertyId, ApplicationArguments.mapdictionary[directgridID + "GridName"].AutomationUniqueValue);
                            gridElementMain = windowElement.FindFirst(TreeScope.TreeScope_Descendants, grdCondition2);
                        }
                        if (gridElementMain == null)
                        {
                            Console.WriteLine("Grid element with AutomationId not found.");
                            throw new Exception("Grid element with AutomationId not found.");
                        }
                        else
                        {

                            Console.WriteLine(gridElementMain.CurrentName);
                            Console.WriteLine(gridElementMain.CurrentAutomationId);

                            IUIAutomationCondition gridControlTypeCondition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_DataGridControlTypeId);
                            IUIAutomationCondition nameCondition = automation.CreatePropertyCondition(
               UIA_PropertyIds.UIA_NamePropertyId,
               string.IsNullOrEmpty(directgridID) ?
               ApplicationArguments.mapdictionary["FirstDataGridName"].AutomationUniqueValue : ApplicationArguments.mapdictionary[directgridID + "FirstDataGridName"].AutomationUniqueValue
           );

                            IUIAutomationCondition combinedCondition = automation.CreateAndCondition(gridControlTypeCondition, nameCondition);

                            IUIAutomationElement recordElement = gridElementMain.FindFirst(TreeScope.TreeScope_Descendants, gridControlTypeCondition);
                            IUIAutomationElement gridElement = gridElementMain.FindFirst(TreeScope.TreeScope_Descendants, combinedCondition);

                            if (gridElement == null)
                            {
                                IUIAutomationCondition listControlCondition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_ListControlTypeId);
                                gridElement = gridElementMain.FindFirst(TreeScope.TreeScope_Descendants, listControlCondition);
                            }

                            IUIAutomationElement customControlTypeID = null;


                            IUIAutomationElement buttonControlTypeID = null;

                            Console.WriteLine(recordElement.CurrentName);
                            Console.WriteLine(recordElement.CurrentLocalizedControlType);
                            Console.WriteLine(recordElement.CurrentAutomationId);

                            Console.WriteLine(gridElement.CurrentName);

                            Console.WriteLine(gridElement.CurrentAutomationId);
                            IUIAutomationScrollPattern scrollPattern = null;
                            bool isScrollPatternAvailable = false;

                            IUIAutomationElementArray gridChildren = gridElement.FindAll(TreeScope.TreeScope_Children, _automation.CreateTrueCondition());
                            for (int i = 0; i < gridChildren.Length; i++)
                            {
                                if (foundRowIndex > index)
                                {
                                    return null;
                                }

                                IUIAutomationElement child = gridChildren.GetElement(i);
                                try
                                {
                                    if (child.CurrentControlType == UIA_ControlTypeIds.UIA_HeaderControlTypeId)
                                    {
                                        foundRowIndex++;
                                    }
                                    else if (child.CurrentControlType == UIA_ControlTypeIds.UIA_DataItemControlTypeId)
                                    {
                                        bool result = false;
                                        selectedRow = child;

                                        if (foundRowIndex == index)
                                        {
                                            return selectedRow;
                                        }
                                        else
                                        {
                                            foundRowIndex++;
                                        }

                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Error processing grid child elements: " + ex.Message);
                                }
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in SelectAndEditWPFGrid method: " + ex.Message);
                return null;
            }

            return selectedRow;
        }







        private IUIAutomationElement FindElementByAutomationId(string automationId)
        {
            try
            {
                IUIAutomationElement rootElement = _automation.GetRootElement();
                IUIAutomationCondition condition = _automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, automationId);
                return rootElement.FindFirst(TreeScope.TreeScope_Descendants, condition);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error finding element by AutomationId: " + ex.Message);
                return null;
            }
        }

        private IUIAutomationElement FindGridElement(IUIAutomationElement parentElement)
        {
            try
            {
                IUIAutomationCondition condition = _automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_DataGridControlTypeId);
                return parentElement.FindFirst(TreeScope.TreeScope_Children, condition);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error finding grid element: " + ex.Message);
                return null;
            }
        }



        private void SetTextInCell(IUIAutomationElement cell, string text)
        {
            try
            {
                object valuePatternObj = cell.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId);

                IUIAutomationValuePattern valuePattern = valuePatternObj as IUIAutomationValuePattern;
                if (valuePattern != null)
                {
                    valuePattern.SetValue(text);
                }


                InputSimulator inputSimulator = new InputSimulator();
                inputSimulator.Keyboard.KeyPress(VirtualKeyCode.TAB);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error setting text in cell: " + ex.Message);
            }
        }

        public static IUIAutomationElement FindElementById(IUIAutomationElement parentElement, string automationId)
        {
            try
            {
                IUIAutomationCondition condition = Automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, automationId);
                return parentElement.FindFirst(TreeScope.TreeScope_Descendants, condition);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error finding element by ID: " + ex.Message);
                return null;
            }
        }


        public static void SetTextInElement(IUIAutomationElement element, string text)
        {
            try
            {
                if (element != null)
                {
                    IUIAutomationValuePattern valuePattern = element.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId) as IUIAutomationValuePattern;
                    if (valuePattern != null)
                    {
                        valuePattern.SetValue(text);
                    }
                    else
                    {
                        Console.WriteLine("Element does not support ValuePattern.");
                    }
                }
                else
                {
                    Console.WriteLine("Element is null, cannot set text.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error setting text in element: " + ex.Message);
            }
        }


        public static void InvokeElement(IUIAutomationElement element, int timeoutMilliseconds = 3000)
        {
            if (element == null)
            {
                Console.WriteLine("Element is null, cannot invoke.");
                return;
            }

            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(timeoutMilliseconds);
            var token = cancellationTokenSource.Token;

            Task invokeTask = Task.Run(() =>
            {
                try
                {
                    IUIAutomationInvokePattern invokePattern = (IUIAutomationInvokePattern)element.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId);
                    invokePattern.Invoke();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(String.Format("Error invoking element: {0}", ex.Message));

                }
            }, token);

            try
            {
                invokeTask.Wait(token);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Invocation timed out after " + timeoutMilliseconds + " milliseconds.");

            }
            catch (AggregateException ex)
            {
                Console.WriteLine(String.Format("Error invoking element: {0}", ex.InnerException != null ? ex.InnerException.Message : "No inner exception"));

            }
        }

        public DataTable GetWPFGridDataWithCheckBoxes(string moduleID, string gridID)
        {
            DataTable dt = new DataTable();
            try
            {
                ApplicationArguments.mapdictionary = UIAutomationHelper.CreateDictionaryFromDataTable(ApplicationArguments.IUIAutomationMappingTables[moduleID]);

                IUIAutomation automation = new CUIAutomation();
                IUIAutomationElement rootElement = automation.GetRootElement();

                IUIAutomationElement windowElement = UIAutomationHelper.FindElementByUniquePropertyType(
                    automation,
                    rootElement,
                    ApplicationArguments.mapdictionary[gridID + "ModuleWindow"].UniquePropertyType,
                    ApplicationArguments.mapdictionary[gridID + "ModuleWindow"].AutomationUniqueValue);

                if (windowElement == null)
                {
                    throw new Exception("Module window not found.");
                }

                DetectAndSwitchWindow(ApplicationArguments.mapdictionary[gridID + "ModuleWindow"].AutomationUniqueValue);

                IUIAutomationCondition gridCondition = automation.CreatePropertyCondition(
                    UIA_PropertyIds.UIA_AutomationIdPropertyId,
                    ApplicationArguments.mapdictionary[gridID + "GridName"].AutomationUniqueValue);

                IUIAutomationElement gridElementMain = windowElement.FindFirst(TreeScope.TreeScope_Descendants, gridCondition);
                if (gridElementMain == null)
                {
                    IUIAutomationElementArray descendants = windowElement.FindAll(TreeScope.TreeScope_Descendants, automation.CreateTrueCondition());
                    Console.WriteLine("Logging all descendants under the window element:");



                    throw new Exception("Grid element not found.");
                }

                IUIAutomationCondition gridControlTypeCondition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_DataGridControlTypeId);
                IUIAutomationCondition nameCondition = automation.CreatePropertyCondition(
   UIA_PropertyIds.UIA_NamePropertyId,
   ApplicationArguments.mapdictionary[gridID + "FirstDataGridName"].AutomationUniqueValue
);
                IUIAutomationCondition combinedCondition = automation.CreateAndCondition(gridControlTypeCondition, nameCondition);

                IUIAutomationElement recordElement = gridElementMain.FindFirst(TreeScope.TreeScope_Descendants, gridControlTypeCondition);
                IUIAutomationElement gridElement = gridElementMain.FindFirst(TreeScope.TreeScope_Descendants, combinedCondition);

                if (gridElement == null)
                {
                    IUIAutomationCondition listControlCondition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_ListControlTypeId);
                    gridElement = gridElementMain.FindFirst(TreeScope.TreeScope_Descendants, listControlCondition);
                }
                if (gridElement == null || ApplicationArguments.mapdictionary.ContainsKey(gridID))
                {
                    IUIAutomationElementArray recordElementChildren = recordElement.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());
                    if (recordElementChildren != null)
                    {
                        gridElement = recordElement;
                    }
                }

                IUIAutomationElementArray gridChildren = gridElement.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());
                for (int i = 0; i < gridChildren.Length; i++)
                {
                    IUIAutomationElement child = gridChildren.GetElement(i);
                    if (child.CurrentControlType == UIA_ControlTypeIds.UIA_HeaderControlTypeId)
                    {
                        IUIAutomationElementArray columnHeaders = child.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());
                        for (int j = 0; j < columnHeaders.Length; j++)
                        {
                            Console.WriteLine(columnHeaders.GetElement(j).CurrentName);
                            if (columnHeaders.GetElement(j).CurrentControlType == UIA_ControlTypeIds.UIA_HeaderItemControlTypeId)
                            {
                                IUIAutomationElement column = columnHeaders.GetElement(j);
                                if (string.Equals(column.CurrentName, "", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (column.CurrentBoundingRectangle.left > 0 &&
       column.CurrentBoundingRectangle.top > 0 &&
       column.CurrentBoundingRectangle.right > 0 &&
       column.CurrentBoundingRectangle.bottom > 0)
                                    {
                                        if (!dt.Columns.Contains("CheckBox"))
                                            dt.Columns.Add("CheckBox", typeof(string));
                                    }
                                }
                                else
                                {
                                    if (!dt.Columns.Contains(column.CurrentName))
                                        dt.Columns.Add(column.CurrentName, typeof(string));
                                }
                            }

                            else if (columnHeaders.GetElement(j).CurrentControlType == UIA_ControlTypeIds.UIA_ButtonControlTypeId)
                            {
                                IUIAutomationElement column = columnHeaders.GetElement(j);
                                if (!dt.Columns.Contains("Button"))
                                    dt.Columns.Add("Button", typeof(string));
                            }
                        }
                    }
                }



                Console.WriteLine(recordElement.CurrentName);
                Console.WriteLine(recordElement.CurrentLocalizedControlType);
                Console.WriteLine(recordElement.CurrentAutomationId);

                Console.WriteLine(gridElement.CurrentName);

                Console.WriteLine(gridElement.CurrentAutomationId);
                List<IUIAutomationElement> childElements = new List<IUIAutomationElement>();


                Parallel.For(0, gridChildren.Length, i =>
                {
                    IUIAutomationElement child = gridChildren.GetElement(i);

                    if (child.CurrentControlType == UIA_ControlTypeIds.UIA_DataItemControlTypeId)
                    {
                        try
                        {
                            // Create a local DataRow instance for this thread.
                            DataRow dr = dt.NewRow();
                            IUIAutomationElementArray rowChildren = child.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());

                            for (int k = 0; k < rowChildren.Length; k++)
                            {
                                try
                                {
                                    bool result = false;
                                    string foundElementValue = ControlTypeHandler.getValueOfElementAdvance(rowChildren.GetElement(k), ref result, "Name");
                                    IUIAutomationElement cell = rowChildren.GetElement(k);
                                    string tableColumnValue = null;

                                    var pattern = cell.GetCurrentPattern(UIA_PatternIds.UIA_TableItemPatternId);
                                    if (pattern != null)
                                    {
                                        var patternProvider = pattern as IUIAutomationTableItemPattern;
                                        if (patternProvider != null)
                                        {
                                            var headerItems = patternProvider.GetCurrentColumnHeaderItems();
                                            if (headerItems != null && headerItems.Length > 0)
                                            {
                                                tableColumnValue = headerItems.GetElement(0).CurrentName;
                                            }
                                        }
                                    }


                                    // Process data based on control type
                                    if (rowChildren.GetElement(k).CurrentControlType == UIA_ControlTypeIds.UIA_CustomControlTypeId)
                                    {
                                        if (!string.IsNullOrEmpty(tableColumnValue) && dt.Columns.Contains(tableColumnValue))
                                        {

                                            dr[tableColumnValue] = foundElementValue;
                                        }
                                        else
                                        {
                                            bool result4 = false;
                                            IUIAutomationElement checkboxSubchild = ControlTypeHandler.FindCheckBoxChildElement(rowChildren.GetElement(k));
                                            if (checkboxSubchild != null)
                                            {
                                                string valueObj = ControlTypeHandler.getValueOfElement(checkboxSubchild, ref result4);
                                                if (string.IsNullOrEmpty(valueObj))
                                                {
                                                    object propertyValue = checkboxSubchild.GetCurrentPropertyValue(UIA_PropertyIds.UIA_ValueValuePropertyId);
                                                    valueObj = propertyValue != null ? propertyValue.ToString() : null;
                                                }
                                                foundElementValue = !string.IsNullOrEmpty(valueObj) ? valueObj : "";
                                                if (string.IsNullOrEmpty(tableColumnValue))
                                                {
                                                    try
                                                    {
                                                        if (string.IsNullOrEmpty(dr["CheckBox"].ToString()))
                                                        {
                                                            dr["CheckBox"] = foundElementValue;
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Console.WriteLine(ex.Message);
                                                    }
                                                }

                                            }
                                            else if (ControlTypeHandler.ImageControlTypeChildExist(rowChildren.GetElement(k)))
                                            {
                                                try
                                                {
                                                    string valueObj = ControlTypeHandler.getValueOfElement(rowChildren.GetElement(k), ref result4);
                                                    if (string.IsNullOrEmpty(valueObj))
                                                    {
                                                        object propertyValue = rowChildren.GetElement(k).GetCurrentPropertyValue(UIA_PropertyIds.UIA_ValueValuePropertyId);
                                                        valueObj = propertyValue != null ? propertyValue.ToString() : null;
                                                    }
                                                    foundElementValue = !string.IsNullOrEmpty(valueObj) ? valueObj : "";

                                                    if (string.IsNullOrEmpty(dr["CheckBox"].ToString()))
                                                    {
                                                        dr["CheckBox"] = foundElementValue;
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    Console.WriteLine(ex.Message);
                                                }
                                            }
                                        }

                                    }
                                    else if (rowChildren.GetElement(k).CurrentControlType == UIA_ControlTypeIds.UIA_HeaderControlTypeId && dt.Columns.Contains("Header"))
                                    {
                                        dr["Header"] = foundElementValue;
                                    }
                                    else if (rowChildren.GetElement(k).CurrentControlType == UIA_ControlTypeIds.UIA_ButtonControlTypeId && dt.Columns.Contains("Button"))
                                    {
                                        dr["Button"] = foundElementValue;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Error processing cell: " + ex.Message);
                                }
                            }

                            // Add the DataRow to the DataTable safely
                            lock (dt)
                            {
                                bool hasNonEmptyValue = false;
                                foreach (var item in dr.ItemArray)
                                {
                                    if (item != null && !string.IsNullOrEmpty(item.ToString()))
                                    {
                                        hasNonEmptyValue = true;
                                        break;
                                    }
                                }

                                if (hasNonEmptyValue)
                                {
                                    dt.Rows.Add(dr);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error processing row: " + ex.Message);
                        }
                    }
                });

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error processing row: " + ex.Message);
                return null;
            }

            return dt;
        }
        public static DataSet GetWinformGridData(string parentAutomationID)
        {
            DataSet dataSet = new DataSet();
            string treeViewAutomationId = "ColScrollRegion: 0, RowScrollRegion: 0";
            try
            {
                IUIAutomation automation = new CUIAutomation();
                IUIAutomationElement rootElement = automation.GetRootElement();


                IUIAutomationCondition windowCondition = automation.CreatePropertyCondition(
                    UIA_PropertyIds.UIA_AutomationIdPropertyId, parentAutomationID);
                IUIAutomationElement mainWindowElement = rootElement.FindFirst(TreeScope.TreeScope_Descendants, windowCondition);

                if (mainWindowElement == null)
                {
                    Console.WriteLine("Main window element not found.");
                    return null;
                }

                List<string> gridAutomationIDs = ApplicationArguments.mapdictionary["GridAutomationID"].AutomationUniqueValue.ToString().Split(',').ToList();

                foreach (string gridAutomationID in gridAutomationIDs)
                {
                    try
                    {
                        IntPtr hWnd = (IntPtr)mainWindowElement.CurrentNativeWindowHandle;
                        SetForegroundWindow(hWnd);
                        DataTable dataTable = null;
                        IUIAutomationElement gridElement = null;

                        IUIAutomationCondition grdCondition = automation.CreatePropertyCondition(
                        UIA_PropertyIds.UIA_AutomationIdPropertyId, gridAutomationID);
                        gridElement = mainWindowElement.FindFirst(TreeScope.TreeScope_Descendants, grdCondition);


                        if (gridElement == null)
                        {
                            if (string.Equals(parentAutomationID, "WatchListMain", StringComparison.OrdinalIgnoreCase))
                            {
                                grdCondition = automation.CreatePropertyCondition(
                          UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_DataGridControlTypeId);
                                gridElement = mainWindowElement.FindFirst(TreeScope.TreeScope_Descendants, grdCondition);

                                if (gridElement == null)
                                {
                                    Console.WriteLine("Grid element with AutomationId " + gridAutomationID + " not found.");
                                    continue;
                                }
                            }
                            else
                            {
                                Console.WriteLine("Grid element with AutomationId " + gridAutomationID + " not found.");
                                continue;
                            }
                        }
                        IUIAutomationCondition treeViewCondition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, treeViewAutomationId);
                        IUIAutomationElement treeViewElement = gridElement.FindFirst(TreeScope.TreeScope_Descendants, treeViewCondition);
                        if (treeViewElement == null)
                        {
                            continue;
                        }
                        dataTable = CreateDataTable(gridElement, automation);
                        Stopwatch stopwatch = new Stopwatch();
                        stopwatch.Start();

                        PopulateDataRowsOptimized(dataTable, gridElement, automation, parentAutomationID, false);
                        stopwatch.Stop();
                        double elapsedSeconds = stopwatch.Elapsed.TotalSeconds;
                        dataTable.TableName = gridAutomationID;
                        dataSet.Tables.Add(dataTable);

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error processing grid with AutomationId :" + ex.Message);
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error processing row: " + ex.Message);
                return null;
            }
            return dataSet;

        }

        private static DataTable CreateDataTable(IUIAutomationElement gridElement, IUIAutomation automation)
        {
            try
            {
                DataTable dataTable = new DataTable();
                var headerElements = GetHeaderElements(gridElement, automation);
                foreach (var headerElement in headerElements)
                {
                    IUIAutomationCondition condition = automation.CreateTrueCondition(); // Match all sub-elements
                    IUIAutomationElementArray childArray = headerElement.FindAll(TreeScope.TreeScope_Children, condition);

                    for (int i = 0; i < childArray.Length; i++)
                    {
                        IUIAutomationElement childElement = childArray.GetElement(i);
                        string headerName = childElement != null && childElement.CurrentName != null
                            ? childElement.CurrentName
                            : "Unnamed Header";

                        if (!dataTable.Columns.Contains(headerName))
                        {
                            dataTable.Columns.Add(headerName);
                        }
                    }
                }

                return dataTable;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in CreateDataTable: " + ex.Message);
                return new DataTable();
            }
        }
        private static List<IUIAutomationElement> GetHeaderElements(IUIAutomationElement gridElement, IUIAutomation automation)
        {
            try
            {
                List<IUIAutomationElement> headerElements = new List<IUIAutomationElement>();
                IUIAutomationCondition headerCondition = automation.CreatePropertyCondition(
                    UIA_PropertyIds.UIA_ControlTypePropertyId,
                    UIA_ControlTypeIds.UIA_HeaderControlTypeId);

                IUIAutomationElementArray headerArray = gridElement.FindAll(TreeScope.TreeScope_Descendants, headerCondition);

                for (int i = 0; i < headerArray.Length; i++)
                {
                    headerElements.Add(headerArray.GetElement(i));
                }

                return headerElements;
            }
            catch (Exception ex)
            {
                return new List<IUIAutomationElement>(); // Return an empty list on error
            }
        }
        private static void PopulateDataRowsOptimized(DataTable dataTable, IUIAutomationElement gridElement, IUIAutomation automation, string parentAutomationID, bool allowParallel)
        {
            try
            {
                IUIAutomationCondition dataItemCondition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_DataItemControlTypeId);
                IUIAutomationElementArray dataItems = gridElement.FindAll(TreeScope.TreeScope_Descendants, dataItemCondition);
                if (allowParallel)
                {
                    Parallel.For(0, (int)dataItems.Length, delegate(int i)
                    {
                        try
                        {
                            IUIAutomationElement dataItem = dataItems.GetElement(i);
                            DataRow row = dataTable.NewRow();
                            IUIAutomationElementArray dataItemChildren = dataItem.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());

                            for (int j = 0; j < dataItemChildren.Length; j++)
                            {
                                IUIAutomationElement cellElement = dataItemChildren.GetElement(j);
                                string columnName = cellElement.CurrentName;
                                bool resulttemp = false;
                                string cellValue = ControlTypeHandler.getValueOfElement(cellElement, ref resulttemp);

                                if (dataTable.Columns.Contains(columnName))
                                {
                                    row[columnName] = cellValue;
                                }
                            }

                            lock (dataTable)
                            {
                                dataTable.Rows.Add(row);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error processing data item " + i + ": " + ex.Message);
                        }
                    });
                }
                else
                {
                    for (int i = 0; i < dataItems.Length; i++)
                    {
                        try
                        {
                            IUIAutomationElement dataItem = dataItems.GetElement(i);
                            DataRow row = dataTable.NewRow();
                            IUIAutomationElementArray dataItemChildren = dataItem.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());

                            for (int j = 0; j < dataItemChildren.Length; j++)
                            {
                                IUIAutomationElement cellElement = dataItemChildren.GetElement(j);
                                string columnName = cellElement.CurrentName;
                                bool resulttemp = false;
                                string cellValue = ControlTypeHandler.getValueOfElement(cellElement, ref resulttemp);

                                if (dataTable.Columns.Contains(columnName))
                                {
                                    row[columnName] = cellValue;
                                }
                            }

                            dataTable.Rows.Add(row);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error processing data item " + i + ": " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in PopulateDataRowsOptimized: " + ex.Message);
            }
        }

        public static DataTable CustomCashFlowNAVGrid(string moduleID, string gridID)
        {
            DataTable dt = null;
            int maxRetries = 3;
            int attempt = 0;

            while (attempt < maxRetries)
            {
                try
                {
                    dt = new DataTable();
                    ApplicationArguments.mapdictionary = DataUtilities.CreateDictionaryFromDataTable(ApplicationArguments.IUIAutomationMappingTables["CustomCashFlowWindow"]);
                    IUIAutomation automation = new CUIAutomation();
                    IUIAutomationElement rootElement = automation.GetRootElement();
                    IUIAutomationElement windowElement = UIAutomationHelper.FindElementByUniquePropertyType(
                        automation,
                        rootElement,
                        ApplicationArguments.mapdictionary[gridID + "ModuleWindow"].UniquePropertyType,
                        ApplicationArguments.mapdictionary[gridID + "ModuleWindow"].AutomationUniqueValue);

                    if (windowElement == null)
                    {
                        throw new Exception("Module window not found.");
                    }

                    DetectAndSwitchWindow(ApplicationArguments.mapdictionary[gridID + "ModuleWindow"].AutomationUniqueValue);

                    IUIAutomationCondition gridCondition = automation.CreatePropertyCondition(
                        UIA_PropertyIds.UIA_AutomationIdPropertyId,
                        ApplicationArguments.mapdictionary[gridID + "GridName"].AutomationUniqueValue);

                    IUIAutomationElement gridElementMain = windowElement.FindFirst(TreeScope.TreeScope_Descendants, gridCondition);
                    if (gridElementMain == null)
                    {
                        IUIAutomationElementArray descendants = windowElement.FindAll(TreeScope.TreeScope_Descendants, automation.CreateTrueCondition());
                        Console.WriteLine("Logging all descendants under the window element:");

                        for (int i = 0; i < descendants.Length; i++)
                        {
                            IUIAutomationElement element = descendants.GetElement(i);

                            string automationId = element.CurrentAutomationId;
                            string name = element.CurrentName;

                            Console.WriteLine("Element " + (i + 1).ToString() + ": AutomationId = " + automationId + ", Name = " + name);
                        }

                        throw new Exception("Grid element not found.");
                    }

                    IUIAutomationCondition gridControlTypeCondition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_DataGridControlTypeId);
                    IUIAutomationCondition nameCondition = automation.CreatePropertyCondition(
                        UIA_PropertyIds.UIA_NamePropertyId,
                        ApplicationArguments.mapdictionary[gridID + "FirstDataGridName"].AutomationUniqueValue
                    );
                    IUIAutomationCondition combinedCondition = automation.CreateAndCondition(gridControlTypeCondition, nameCondition);

                    IUIAutomationElement recordElement = gridElementMain.FindFirst(TreeScope.TreeScope_Descendants, gridControlTypeCondition);
                    IUIAutomationElement gridElement = gridElementMain.FindFirst(TreeScope.TreeScope_Descendants, combinedCondition);

                    if (gridElement == null)
                    {
                        IUIAutomationCondition listControlCondition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_ListControlTypeId);
                        gridElement = gridElementMain.FindFirst(TreeScope.TreeScope_Descendants, listControlCondition);
                    }
                    if (gridElement == null)
                    {
                        IUIAutomationElementArray recordElementChildren = recordElement.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());
                        if (recordElementChildren != null)
                        {
                            gridElement = recordElement;
                        }
                    }

                    IUIAutomationElementArray gridChildren = gridElement.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());
                    for (int i = 0; i < gridChildren.Length; i++)
                    {
                        IUIAutomationElement child = gridChildren.GetElement(i);
                        if (child.CurrentControlType == UIA_ControlTypeIds.UIA_HeaderControlTypeId)
                        {
                            IUIAutomationElementArray columnHeaders = child.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());
                            for (int j = 0; j < columnHeaders.Length; j++)
                            {
                                Console.WriteLine(columnHeaders.GetElement(j).CurrentName);
                                if (columnHeaders.GetElement(j).CurrentControlType == UIA_ControlTypeIds.UIA_HeaderItemControlTypeId)
                                {
                                    IUIAutomationElement column = columnHeaders.GetElement(j);
                                    if (string.Equals(column.CurrentName, "", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (column.CurrentBoundingRectangle.left > 0 &&
                                           column.CurrentBoundingRectangle.top > 0 &&
                                           column.CurrentBoundingRectangle.right > 0 &&
                                           column.CurrentBoundingRectangle.bottom > 0)
                                        {
                                            if (!dt.Columns.Contains("CheckBox"))
                                                dt.Columns.Add("CheckBox", typeof(string));
                                        }
                                    }
                                    else
                                    {
                                        if (!dt.Columns.Contains(column.CurrentName))
                                        {
                                            dt.Columns.Add(column.CurrentName, typeof(string));
                                            IntPtr hWnd = (IntPtr)columnHeaders.GetElement(j).CurrentNativeWindowHandle;
                                            SetForegroundWindow(hWnd);
                                            MouseOperations.ClickElement(columnHeaders.GetElement(j));
                                        }
                                    }
                                }

                                else if (columnHeaders.GetElement(j).CurrentControlType == UIA_ControlTypeIds.UIA_ButtonControlTypeId)
                                {
                                    IUIAutomationElement column = columnHeaders.GetElement(j);
                                    if (!dt.Columns.Contains("Button"))
                                        dt.Columns.Add("Button", typeof(string));
                                }
                            }
                        }
                    }

                    List<IUIAutomationElement> childElements = new List<IUIAutomationElement>();

                    for (int i = 0; i < gridChildren.Length; i++)
                    {
                        IUIAutomationElement child = gridChildren.GetElement(i);

                        if (child.CurrentControlType == UIA_ControlTypeIds.UIA_DataItemControlTypeId)
                        {
                            try
                            {
                                DataRow dr = dt.NewRow();
                                IUIAutomationElementArray rowChildren = child.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());

                                for (int k = 0; k < rowChildren.Length; k++)
                                {
                                    try
                                    {
                                        bool result = false;
                                        string foundElementValue = ControlTypeHandler.getValueOfElementAdvance(rowChildren.GetElement(k), ref result, "Name");
                                        IUIAutomationElement cell = rowChildren.GetElement(k);
                                        if (rowChildren.GetElement(k).CurrentControlType == UIA_ControlTypeIds.UIA_CustomControlTypeId)
                                        {
                                            try
                                            {
                                                dr[k - 1] = foundElementValue;
                                            }
                                            catch { }
                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine("Error processing cell: " + ex.Message);
                                    }

                                }

                                bool hasNonEmptyItem = false;
                                foreach (var item in dr.ItemArray)
                                {
                                    if (!string.IsNullOrEmpty(item.ToString()))
                                    {
                                        hasNonEmptyItem = true;
                                        break;
                                    }
                                }

                                if (hasNonEmptyItem)
                                {
                                    lock (dt)
                                    {
                                        dt.Rows.Add(dr);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Error processing row: " + ex.Message);
                            }
                        }
                    }

                    return dt;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception occurred: " + ex.Message);
                    if (attempt == maxRetries - 1)
                        return null;
                    attempt++;
                }
            }

            return dt;
        }
        public void CustomCashFlowNAVGridEdit(string moduleID, string gridID, int editRowindex, string valuetoFill)
        {
            int maxRetries = 3;
            int attempt = 0;

            try
            {
                ApplicationArguments.mapdictionary = DataUtilities.CreateDictionaryFromDataTable(ApplicationArguments.IUIAutomationMappingTables["CustomCashFlowWindow"]);
                IUIAutomation automation = new CUIAutomation();
                IUIAutomationElement rootElement = automation.GetRootElement();
                IUIAutomationElement windowElement = UIAutomationHelper.FindElementByUniquePropertyType(
                    automation,
                    rootElement,
                    ApplicationArguments.mapdictionary[gridID + "ModuleWindow"].UniquePropertyType,
                    ApplicationArguments.mapdictionary[gridID + "ModuleWindow"].AutomationUniqueValue);

                if (windowElement == null)
                {
                    throw new Exception("Module window not found.");
                }

                DetectAndSwitchWindow(ApplicationArguments.mapdictionary[gridID + "ModuleWindow"].AutomationUniqueValue);

                IUIAutomationCondition gridCondition = automation.CreatePropertyCondition(
                    UIA_PropertyIds.UIA_AutomationIdPropertyId,
                    ApplicationArguments.mapdictionary[gridID + "GridName"].AutomationUniqueValue);

                IUIAutomationElement gridElementMain = windowElement.FindFirst(TreeScope.TreeScope_Descendants, gridCondition);
                if (gridElementMain == null)
                {
                    throw new Exception("Grid element not found.");
                }

                IUIAutomationCondition gridControlTypeCondition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_DataGridControlTypeId);
                IUIAutomationCondition nameCondition = automation.CreatePropertyCondition(
                    UIA_PropertyIds.UIA_NamePropertyId,
                    ApplicationArguments.mapdictionary[gridID + "FirstDataGridName"].AutomationUniqueValue);
                IUIAutomationCondition combinedCondition = automation.CreateAndCondition(gridControlTypeCondition, nameCondition);

                IUIAutomationElement recordElement = gridElementMain.FindFirst(TreeScope.TreeScope_Descendants, gridControlTypeCondition);
                IUIAutomationElement gridElement = gridElementMain.FindFirst(TreeScope.TreeScope_Descendants, combinedCondition);

                if (gridElement == null)
                {
                    IUIAutomationCondition listControlCondition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_ListControlTypeId);
                    gridElement = gridElementMain.FindFirst(TreeScope.TreeScope_Descendants, listControlCondition);
                }

                if (gridElement == null)
                {
                    IUIAutomationElementArray recordElementChildren = recordElement.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());
                    if (recordElementChildren != null)
                    {
                        gridElement = recordElement;
                    }
                }

                IUIAutomationElementArray gridChildren = gridElement.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());
                IUIAutomationElement gridRowForEdit = gridChildren.GetElement(editRowindex + 1);
                IUIAutomationElementArray rowChildren = gridRowForEdit.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());

                if (rowChildren.Length > 0)
                {
                    try
                    {
                        bool result = false;
                        IUIAutomationElement lastChild = rowChildren.GetElement(rowChildren.Length - 1);
                        string foundElementValue = ControlTypeHandler.getValueOfElementAdvance(lastChild, ref result, "Name");

                        if (lastChild.CurrentControlType == UIA_ControlTypeIds.UIA_CustomControlTypeId)
                        {
                            try
                            {
                                while (attempt < maxRetries)
                                {
                                    MouseOperations.ClickElement(lastChild, "Left", true);
                                    InputSimulator inputSimulator = new InputSimulator();
                                    inputSimulator.Keyboard.TextEntry(valuetoFill);
                                    inputSimulator.Keyboard.KeyPress(VirtualKeyCode.TAB);
                                    foundElementValue = ControlTypeHandler.getValueOfElementAdvance(lastChild, ref result, "Name");

                                    if (!string.Equals(foundElementValue, valuetoFill, StringComparison.OrdinalIgnoreCase))
                                        SetTextInCell(lastChild, valuetoFill);

                                    foundElementValue = ControlTypeHandler.getValueOfElementAdvance(lastChild, ref result, "Name");

                                    if (string.Equals(foundElementValue, valuetoFill, StringComparison.OrdinalIgnoreCase))
                                    {
                                        return;
                                    }
                                    attempt++;
                                }

                                return; // Exit after max retries
                            }
                            catch
                            {
                                return;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error processing last child: " + ex.Message);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occurred: " + ex.Message);
                if (attempt == maxRetries - 1)
                    return;

                attempt++;
            }
        }

        public bool CheckEmptyGridData(DataTable excelData, DataTable uiData)
        {
            try
            {

                if (!excelData.Columns.Contains(TestDataConstants.TESTINGTYPE) || !excelData.Columns.Contains(TestDataConstants.COL_EMPTYGRIDDATA))
                    return false;

                foreach (DataRow row in excelData.Rows)
                {
                    var testType = row[TestDataConstants.TESTINGTYPE] != DBNull.Value ? row[TestDataConstants.TESTINGTYPE].ToString() : string.Empty;
                    var emptyGridData = row[TestDataConstants.COL_EMPTYGRIDDATA] != DBNull.Value ? row[TestDataConstants.COL_EMPTYGRIDDATA].ToString() : "FALSE";

                    if (testType.ToUpper().Equals("NEGATIVE", StringComparison.OrdinalIgnoreCase) &&
                        emptyGridData.ToUpper().Equals("TRUE", StringComparison.OrdinalIgnoreCase))
                    {
                        if (uiData.Rows.Count >= 1)
                            return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in CheckEmptyGridData: " + ex.Message);
                return false;
            }
        }

        public void OperationsonPM(DataTable dt, IUIAutomationElement targetElement)
        {

            IUIAutomation automation = new CUIAutomation();
            IUIAutomationElement rootElement = automation.GetRootElement();
            IUIAutomationElement windowElement = null;
            try
            {
                windowElement = UIAutomationHelper.FindElementByUniquePropertyType(automation, rootElement, ApplicationArguments.mapdictionary["ModuleWindow"].UniquePropertyType, ApplicationArguments.mapdictionary["ModuleWindow"].AutomationUniqueValue);
            }
            catch (Exception ex)
            {
                throw new Exception(" Window not visible : Exception -" + ex.Message);

            }
            try
            {
                if (dt == null || dt.Rows.Count == 0 || targetElement == null)
                    return;

                string expectedAutomationId = dt.Rows[0][TestDataConstants.COL_ROWINDEXWISE].ToString();

                try
                {
                    IUIAutomationCondition condition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_DataItemControlTypeId);
                    IUIAutomationElementArray children = targetElement.FindAll(TreeScope.TreeScope_Children, condition);

                    if (children == null)
                        return;

                    for (int i = 0; i < children.Length; i++)
                    {
                        IUIAutomationElement child = children.GetElement(i);
                        if (child != null)
                        {
                            string automationId = child.CurrentAutomationId;
                            string name = child.CurrentName;
                            if (automationId != null && automationId.Equals(expectedAutomationId, StringComparison.OrdinalIgnoreCase))
                            {
                                var rect = child.CurrentBoundingRectangle;
                                if ((rect.right - rect.left) > 0 && (rect.bottom - rect.top) > 0)
                                {
                                    int centerX = rect.left + 50;
                                    int centerY = (rect.top + rect.bottom) / 2;

                                    System.Windows.Forms.Cursor.Position = new System.Drawing.Point(centerX, centerY);
                                    Thread.Sleep(200);

                                    mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0);
                                    mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);

                                    Console.WriteLine("Right-click successfully performed.");
                                    System.Windows.Forms.Cursor.Position = new System.Drawing.Point(centerX, centerY);
                                    Thread.Sleep(100);

                                    mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0);
                                    mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);

                                    if ((rect.right - rect.left) > 0 && (rect.bottom - rect.top) > 0)
                                    {
                                        int centerX1 = rect.left + 50;
                                        int centerY1 = (rect.top + rect.bottom) / 2;

                                        for (int j = 0; j < 3; j++)
                                        {
                                            int offsetX = centerX1 + (j * 2);
                                            int offsetY = centerY1;

                                            System.Windows.Forms.Cursor.Position = new System.Drawing.Point(offsetX, offsetY);
                                            Thread.Sleep(3000);

                                            mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0);
                                            mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
                                            Thread.Sleep(2000);
                                        }

                                    }
                                }

                                IUIAutomationCondition nameCondition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "DropDown");
                                IUIAutomationCondition controlTypeCondition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_MenuControlTypeId);
                                IUIAutomationCondition dropdownCondition = automation.CreateAndCondition(nameCondition, controlTypeCondition);

                                IUIAutomationElement dropdownMenu = null;
                                int maxRetries = 2;
                                int retryCount = 0;

                                while (dropdownMenu == null && retryCount < maxRetries)
                                {
                                    dropdownMenu = windowElement.FindFirst(TreeScope.TreeScope_Descendants, dropdownCondition);

                                    if (dropdownMenu == null)
                                    {
                                        dropdownMenu = windowElement.FindFirst(TreeScope.TreeScope_Descendants, nameCondition);
                                        if (dropdownMenu == null)
                                        {
                                            dropdownMenu = windowElement.FindFirst(TreeScope.TreeScope_Descendants, controlTypeCondition);
                                        }

                                        if (dropdownMenu == null)
                                        {
                                            System.Threading.Thread.Sleep(2000);
                                        }

                                        retryCount++;
                                    }
                                }
                                if (dropdownMenu == null)
                                {
                                    Console.WriteLine("DropDown menu not found with regular method.");
                                    try
                                    {
                                        //adding thirdparty execution as a bruteforce method
                                        string actionElement = dt.Rows[0]["RightClickOperations"].ToString();
                                        string WindowAutomationID = ApplicationArguments.mapdictionary["ModuleWindow2"].AutomationUniqueValue;
                                        string ElementDetail = actionElement;

                                        string AutomationUniqueValue = ApplicationArguments.mapdictionary[actionElement].AutomationUniqueValue;
                                        string ControlTypeOfElementDetail = ApplicationArguments.mapdictionary[actionElement].UniquePropertyType;
                                        string ValueToPush = actionElement;
                                        string IUIAutomationMappingFile = ConfigurationManager.AppSettings["IUIAutomationMappingFile"].ToString();

                                        string args = string.Format("\"{0}\" \"{1}\" \"{2}\" \"{3}\" \"{4}\" \"{5}\"",
                                            WindowAutomationID,
                                            ElementDetail,
                                            ControlTypeOfElementDetail,
                                            ValueToPush,
                                            IUIAutomationMappingFile,
                                            AutomationUniqueValue);

                                        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
                                        Console.WriteLine("Third-party execution start time: " + stopwatch.ElapsedMilliseconds + " ms");
                                        int exitCode = ThirdPartyExecutionStatusReporter.RunHelperExe(args);
                                        stopwatch.Stop();
                                        Console.WriteLine("Third-party execution time: " + stopwatch.ElapsedMilliseconds + " ms");
                                        if (exitCode != 0)
                                        {
                                            Console.WriteLine("Third-party execution failed. Exit Code: " + exitCode);
                                            throw new Exception("DropDown Menu inaccessible");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Third-party execution succeeded.");
                                            return;//assuming only one menuitem is required to be clicked.
                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine("Error while dumping descendants: " + ex.Message);
                                    }
                                }
                                
                                IUIAutomationCondition menuItemCondition = automation.CreatePropertyCondition(
           UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_MenuItemControlTypeId);

                                IUIAutomationElementArray menuItems = dropdownMenu.FindAll(TreeScope.TreeScope_Children, menuItemCondition);

                                if (menuItems == null || menuItems.Length == 0)
                                {
                                    Console.WriteLine("No menu items found in DropDown.");
                                    return;
                                }
                                string action = string.Empty;
                                if (!string.IsNullOrEmpty(dt.Rows[0]["RightClickOperations"].ToString()))
                                {
                                    action = dt.Rows[0]["RightClickOperations"].ToString();
                                }
                                for (int z = 0; z < menuItems.Length; z++)
                                {
                                    IUIAutomationElement menuItem = menuItems.GetElement(z);
                                    Console.WriteLine("MenuItemList :" + menuItem.CurrentName);
                                    if (menuItem != null && string.Equals(menuItem.CurrentName, action, StringComparison.OrdinalIgnoreCase))
                                    {
                                        MouseOperations.ClickElement(menuItem);

                                        foreach (DataColumn column in dt.Columns)
                                        {
                                            if (column.ColumnName.Equals("RightClickOperations", StringComparison.OrdinalIgnoreCase))
                                                continue; 

                                            string value = dt.Rows[0][column].ToString();
                                            if (!string.IsNullOrEmpty(value) && value.All(char.IsLetter))
                                            {
                                                IUIAutomationElement submenuItem = null;
                                                IUIAutomationCondition submenuItemnameCondition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, value);
                                                submenuItem = windowElement.FindFirst(TreeScope.TreeScope_Descendants, submenuItemnameCondition);

                                                if (submenuItem != null)
                                                {
                                                    Console.WriteLine(submenuItem.CurrentName);
                                                    MouseOperations.ClickElement(submenuItem);

                                                }  
                                                
                                            }
                                        }

                                        return;
                                    }
                                }


                                break;
                            }
                        }
                    }



                }
                catch (Exception ex)
                {
                    throw new Exception("Exception occurred in OperationsonPM: " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception occurred in OperationsonPM: " + ex.Message);
            }
        }
        public DataTable GetWPFGridDataNew(string moduleID, string gridID, bool allowParallel = true)
        {
            DataTable dt = new DataTable();
            try
            {
                ApplicationArguments.mapdictionary = DataUtilities.CreateDictionaryFromDataTable(ApplicationArguments.IUIAutomationMappingTables[moduleID]);

                IUIAutomation automation = new CUIAutomation();
                IUIAutomationElement rootElement = automation.GetRootElement();

                IUIAutomationElement windowElement = UIAutomationHelper.FindElementByUniquePropertyType(
                    automation,
                    rootElement,
                    ApplicationArguments.mapdictionary[gridID + "ModuleWindow"].UniquePropertyType,
                    ApplicationArguments.mapdictionary[gridID + "ModuleWindow"].AutomationUniqueValue);

                if (windowElement == null)
                {
                    throw new Exception("Module window not found.");
                }

                DetectAndSwitchWindow(ApplicationArguments.mapdictionary[gridID + "ModuleWindow"].AutomationUniqueValue);

                IUIAutomationCondition gridCondition = automation.CreatePropertyCondition(
                    UIA_PropertyIds.UIA_AutomationIdPropertyId,
                    ApplicationArguments.mapdictionary[gridID + "GridName"].AutomationUniqueValue);

                IUIAutomationElement gridElementMain = windowElement.FindFirst(TreeScope.TreeScope_Descendants, gridCondition);
                if (gridElementMain == null)
                {
                    IUIAutomationElementArray descendants = windowElement.FindAll(TreeScope.TreeScope_Descendants, automation.CreateTrueCondition());

                    Console.WriteLine("Logging all descendants under the window element:");
                    for (int i = 0; i < descendants.Length; i++)
                    {
                        IUIAutomationElement element = descendants.GetElement(i);

                        Console.WriteLine("Element " + (i + 1) + ": AutomationId = " + element.CurrentAutomationId + ", Name = " + element.CurrentName);
                    }

                    throw new Exception("Grid element not found.");
                }

                IUIAutomationCondition gridControlTypeCondition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_DataGridControlTypeId);
                IUIAutomationCondition nameCondition = automation.CreatePropertyCondition(
                    UIA_PropertyIds.UIA_NamePropertyId,
                    ApplicationArguments.mapdictionary[gridID + "FirstDataGridName"].AutomationUniqueValue
                );
                IUIAutomationCondition combinedCondition = automation.CreateAndCondition(gridControlTypeCondition, nameCondition);

                IUIAutomationElement recordElement = gridElementMain.FindFirst(TreeScope.TreeScope_Descendants, gridControlTypeCondition);
                IUIAutomationElement gridElement = gridElementMain.FindFirst(TreeScope.TreeScope_Descendants, combinedCondition);

                if (gridElement == null)
                {
                    IUIAutomationCondition listControlCondition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_ListControlTypeId);
                    gridElement = gridElementMain.FindFirst(TreeScope.TreeScope_Descendants, listControlCondition);
                }
                if (gridElement == null)
                {
                    IUIAutomationElementArray recordElementChildren = recordElement.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());
                    if (recordElementChildren != null)
                    {
                        gridElement = recordElement;
                    }
                }

                IUIAutomationElementArray gridChildren = gridElement.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());
                for (int i = 0; i < gridChildren.Length; i++)
                {
                    IUIAutomationElement child = gridChildren.GetElement(i);
                    if (child.CurrentControlType == UIA_ControlTypeIds.UIA_HeaderControlTypeId)
                    {
                        IUIAutomationElementArray columnHeaders = child.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());
                        for (int j = 0; j < columnHeaders.Length; j++)
                        {
                            IUIAutomationElement column = columnHeaders.GetElement(j);
                            Console.WriteLine(column.CurrentName);

                            if (column.CurrentControlType == UIA_ControlTypeIds.UIA_HeaderItemControlTypeId)
                            {
                                if (string.IsNullOrEmpty(column.CurrentName))
                                {
                                    if (column.CurrentBoundingRectangle.left > 0 &&
                                        column.CurrentBoundingRectangle.top > 0 &&
                                        column.CurrentBoundingRectangle.right > 0 &&
                                        column.CurrentBoundingRectangle.bottom > 0)
                                    {
                                        if (!dt.Columns.Contains("CheckBox"))
                                        {
                                            dt.Columns.Add("CheckBox", typeof(string));
                                        }
                                    }
                                }
                                else
                                {
                                    if (!dt.Columns.Contains(column.CurrentName))
                                    {
                                        dt.Columns.Add(column.CurrentName, typeof(string));
                                    }
                                }
                            }
                            else if (column.CurrentControlType == UIA_ControlTypeIds.UIA_ButtonControlTypeId)
                            {
                                if (!dt.Columns.Contains("Button"))
                                {
                                    dt.Columns.Add("Button", typeof(string));
                                }
                            }
                        }
                    }
                }

                List<IUIAutomationElement> childElements = new List<IUIAutomationElement>();

                if (allowParallel)
                {
                    System.Threading.Tasks.Parallel.For(0, gridChildren.Length, delegate(int i)
                    {
                        IUIAutomationElement child = gridChildren.GetElement(i);
                        if (child.CurrentControlType == UIA_ControlTypeIds.UIA_DataItemControlTypeId)
                        {
                            try
                            {
                                DataRow dr = dt.NewRow();
                                IUIAutomationElementArray rowChildren = child.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());

                                for (int k = 0; k < rowChildren.Length; k++)
                                {
                                    try
                                    {
                                        bool result = false;
                                        string foundElementValue = ControlTypeHandler.getValueOfElementAdvance(rowChildren.GetElement(k), ref result, "Name");
                                        IUIAutomationElement cell = rowChildren.GetElement(k);
                                        string tableColumnValue = null;

                                        object tablePattern = cell.GetCurrentPattern(UIA_PatternIds.UIA_TableItemPatternId);
                                        if (tablePattern != null)
                                        {
                                            IUIAutomationTableItemPattern patternProvider = (IUIAutomationTableItemPattern)tablePattern;
                                            IUIAutomationElementArray headerItems = patternProvider.GetCurrentColumnHeaderItems();
                                            if (headerItems != null && headerItems.Length > 0)
                                            {
                                                tableColumnValue = headerItems.GetElement(0).CurrentName;
                                            }
                                        }

                                        if (rowChildren.GetElement(k).CurrentControlType == UIA_ControlTypeIds.UIA_CustomControlTypeId)
                                        {
                                            if (!string.IsNullOrEmpty(tableColumnValue) && dt.Columns.Contains(tableColumnValue))
                                            {
                                                dr[tableColumnValue] = foundElementValue;
                                            }
                                            else
                                            {
                                                bool result4 = false;
                                                IUIAutomationElement checkboxSubchild = ControlTypeHandler.FindCheckBoxChildElement(rowChildren.GetElement(k));
                                                if (checkboxSubchild != null)
                                                {
                                                    string valueObj = ControlTypeHandler.getValueOfElement(checkboxSubchild, ref result4);
                                                    if (string.IsNullOrEmpty(valueObj))
                                                    {
                                                        object propertyValue = checkboxSubchild.GetCurrentPropertyValue(UIA_PropertyIds.UIA_ValueValuePropertyId);
                                                        valueObj = propertyValue != null ? propertyValue.ToString() : null;
                                                    }
                                                    foundElementValue = !string.IsNullOrEmpty(valueObj) ? valueObj : "";

                                                    if (string.IsNullOrEmpty(tableColumnValue))
                                                    {
                                                        if (dt.Columns.Contains("CheckBox"))
                                                        {
                                                            if (string.IsNullOrEmpty(Convert.ToString(dr["CheckBox"])))
                                                            {
                                                                dr["CheckBox"] = foundElementValue;
                                                            }
                                                        }
                                                    }
                                                }
                                                else if (ControlTypeHandler.ImageControlTypeChildExist(rowChildren.GetElement(k)))
                                                {
                                                    bool result4b = false;
                                                    string valueObj = ControlTypeHandler.getValueOfElement(rowChildren.GetElement(k), ref result4b);
                                                    if (string.IsNullOrEmpty(valueObj))
                                                    {
                                                        object propertyValue = rowChildren.GetElement(k).GetCurrentPropertyValue(UIA_PropertyIds.UIA_ValueValuePropertyId);
                                                        valueObj = propertyValue != null ? propertyValue.ToString() : null;
                                                    }
                                                    foundElementValue = !string.IsNullOrEmpty(valueObj) ? valueObj : "";

                                                    if (dt.Columns.Contains("CheckBox"))
                                                    {
                                                        if (string.IsNullOrEmpty(Convert.ToString(dr["CheckBox"])))
                                                        {
                                                            dr["CheckBox"] = foundElementValue;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else if (rowChildren.GetElement(k).CurrentControlType == UIA_ControlTypeIds.UIA_HeaderControlTypeId && dt.Columns.Contains("Header"))
                                        {
                                            dr["Header"] = foundElementValue;
                                        }
                                        else if (rowChildren.GetElement(k).CurrentControlType == UIA_ControlTypeIds.UIA_ButtonControlTypeId && dt.Columns.Contains("Button"))
                                        {
                                            dr["Button"] = foundElementValue;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine("Error processing cell: " + ex.Message);
                                    }
                                }

                                lock (dt)
                                {
                                    bool hasData = false;
                                    foreach (object item in dr.ItemArray)
                                    {
                                        if (item != null && !string.IsNullOrEmpty(item.ToString()))
                                        {
                                            hasData = true;
                                            break;
                                        }
                                    }
                                    if (hasData)
                                    {
                                        dt.Rows.Add(dr);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Error processing row: " + ex.Message);
                            }
                        }
                    });
                }
                else
                {
                    for (int i = 0; i < gridChildren.Length; i++)
                    {
                        IUIAutomationElement child = gridChildren.GetElement(i);
                        if (child.CurrentControlType == UIA_ControlTypeIds.UIA_DataItemControlTypeId)
                        {
                            try
                            {
                                DataRow dr = dt.NewRow();
                                IUIAutomationElementArray rowChildren = child.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());

                                for (int k = 0; k < rowChildren.Length; k++)
                                {
                                    try
                                    {
                                        bool result = false;
                                        string foundElementValue = ControlTypeHandler.getValueOfElementAdvance(rowChildren.GetElement(k), ref result, "Name");
                                        IUIAutomationElement cell = rowChildren.GetElement(k);
                                        string tableColumnValue = null;

                                        object tablePattern = cell.GetCurrentPattern(UIA_PatternIds.UIA_TableItemPatternId);
                                        if (tablePattern != null)
                                        {
                                            IUIAutomationTableItemPattern patternProvider = (IUIAutomationTableItemPattern)tablePattern;
                                            IUIAutomationElementArray headerItems = patternProvider.GetCurrentColumnHeaderItems();
                                            if (headerItems != null && headerItems.Length > 0)
                                            {
                                                tableColumnValue = headerItems.GetElement(0).CurrentName;
                                            }
                                        }

                                        if (rowChildren.GetElement(k).CurrentControlType == UIA_ControlTypeIds.UIA_CustomControlTypeId)
                                        {
                                            if (!string.IsNullOrEmpty(tableColumnValue) && dt.Columns.Contains(tableColumnValue))
                                            {
                                                dr[tableColumnValue] = foundElementValue;
                                            }
                                            else
                                            {
                                                bool result4 = false;
                                                IUIAutomationElement checkboxSubchild = ControlTypeHandler.FindCheckBoxChildElement(rowChildren.GetElement(k));
                                                if (checkboxSubchild != null)
                                                {
                                                    string valueObj = ControlTypeHandler.getValueOfElement(checkboxSubchild, ref result4);
                                                    if (string.IsNullOrEmpty(valueObj))
                                                    {
                                                        object propertyValue = checkboxSubchild.GetCurrentPropertyValue(UIA_PropertyIds.UIA_ValueValuePropertyId);
                                                        valueObj = propertyValue != null ? propertyValue.ToString() : null;
                                                    }
                                                    foundElementValue = !string.IsNullOrEmpty(valueObj) ? valueObj : "";

                                                    if (string.IsNullOrEmpty(tableColumnValue))
                                                    {
                                                        if (dt.Columns.Contains("CheckBox"))
                                                        {
                                                            if (string.IsNullOrEmpty(Convert.ToString(dr["CheckBox"])))
                                                            {
                                                                dr["CheckBox"] = foundElementValue;
                                                            }
                                                        }
                                                    }
                                                }
                                                else if (ControlTypeHandler.ImageControlTypeChildExist(rowChildren.GetElement(k)))
                                                {
                                                    bool result4b = false;
                                                    string valueObj = ControlTypeHandler.getValueOfElement(rowChildren.GetElement(k), ref result4b);
                                                    if (string.IsNullOrEmpty(valueObj))
                                                    {
                                                        object propertyValue = rowChildren.GetElement(k).GetCurrentPropertyValue(UIA_PropertyIds.UIA_ValueValuePropertyId);
                                                        valueObj = propertyValue != null ? propertyValue.ToString() : null;
                                                    }
                                                    foundElementValue = !string.IsNullOrEmpty(valueObj) ? valueObj : "";

                                                    if (dt.Columns.Contains("CheckBox"))
                                                    {
                                                        if (string.IsNullOrEmpty(Convert.ToString(dr["CheckBox"])))
                                                        {
                                                            dr["CheckBox"] = foundElementValue;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else if (rowChildren.GetElement(k).CurrentControlType == UIA_ControlTypeIds.UIA_HeaderControlTypeId && dt.Columns.Contains("Header"))
                                        {
                                            dr["Header"] = foundElementValue;
                                        }
                                        else if (rowChildren.GetElement(k).CurrentControlType == UIA_ControlTypeIds.UIA_ButtonControlTypeId && dt.Columns.Contains("Button"))
                                        {
                                            dr["Button"] = foundElementValue;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine("Error processing cell: " + ex.Message);
                                    }
                                }

                                bool hasData = false;
                                foreach (object item in dr.ItemArray)
                                {
                                    if (item != null && !string.IsNullOrEmpty(item.ToString()))
                                    {
                                        hasData = true;
                                        break;
                                    }
                                }
                                if (hasData)
                                {
                                    dt.Rows.Add(dr);
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Error processing row: " + ex.Message);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return dt;
        }
        public static bool SelectWinFormGridData(DataTable ExcelData, DataTable uiData, string moduleID, string gridID, IUIAutomation automation, IUIAutomationElement windowElement)
        {
            string treeViewAutomationId = "ColScrollRegion: 0, RowScrollRegion: 0";
            bool success = true;
            UIAutomationHelper uiAutomationHelper = new UIAutomationHelper();
            try
            {
                windowElement.SetFocus();
                IUIAutomationCondition controlTypeCondition = automation.CreatePropertyCondition(
                      UIA_PropertyIds.UIA_AutomationIdPropertyId, gridID);

                IUIAutomationElement GridElement = windowElement.FindFirst(TreeScope.TreeScope_Descendants, controlTypeCondition);
                //access "ColScrollRegion: 0, RowScrollRegion: 0"

                if (GridElement != null)
                {
                    IUIAutomationCondition treeView = automation.CreatePropertyCondition(
                      UIA_PropertyIds.UIA_AutomationIdPropertyId, treeViewAutomationId);

                    IUIAutomationElement treeViewElement = GridElement.FindFirst(TreeScope.TreeScope_Descendants, treeView);

                    var dataItemCondition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_DataItemControlTypeId);

                    IUIAutomationElementArray dataItems = treeViewElement.FindAll(TreeScope.TreeScope_Descendants, dataItemCondition);
                    Console.WriteLine("Logging all descendants under the window element:");

                    for (int i = 0; i < dataItems.Length; i++)
                    {
                        IUIAutomationElement element = dataItems.GetElement(i);

                        string automationId = element.CurrentAutomationId;
                        string name = element.CurrentName;

                        Console.WriteLine("Element " + (i + 1) + " : AutomationId =" + automationId + ", Name =  " + name);
                    }
                    for (int i = 0; i < ExcelData.Rows.Count; i++)
                    {
                        DataRow excelRow = ExcelData.Rows[i];
                        string checkBoxValue = string.Empty;
                        if (ExcelData.Columns.Contains("CheckBox"))
                        {
                            checkBoxValue = Convert.ToString(excelRow["CheckBox"]);
                            excelRow["CheckBox"] = DBNull.Value;
                        }

                        DataRow dtRow = DataUtilities.GetMatchingDataRow(DataUtilities.RemoveTrailingZeroes(uiData), excelRow);
                        int index = uiData.Rows.IndexOf(dtRow);

                        if (index < 0)
                        {

                        }
                        else
                        {
                            IUIAutomationElement uiRowElement = dataItems.GetElement(index);

                            if (checkBoxValue == "ToggleState_On")
                            {
                                // Find checkbox control in the matched UI row
                                IUIAutomationCondition checkBoxCondition = automation.CreatePropertyCondition(
                                    UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_CheckBoxControlTypeId);

                                IUIAutomationElement checkBoxElement = uiRowElement.FindFirst(TreeScope.TreeScope_Descendants, checkBoxCondition);
                                object patternProvider = null;
                                string valuetouse = string.Empty;

                                if (checkBoxElement != null && checkBoxElement.GetCurrentPattern(UIA_PatternIds.UIA_TogglePatternId) != null)
                                {
                                    patternProvider = checkBoxElement.GetCurrentPattern(UIA_PatternIds.UIA_TogglePatternId);
                                    IUIAutomationTogglePattern togglePattern = patternProvider as IUIAutomationTogglePattern;

                                    if (togglePattern != null)
                                    {
                                        // Use full namespace to resolve ToggleState ambiguity
                                        UIAutomationClient.ToggleState toggleState = togglePattern.CurrentToggleState;
                                        string toggleStateString = toggleState.ToString();

                                        valuetouse = toggleStateString;
                                        Console.WriteLine("Toggle state: " + toggleStateString);

                                        if (toggleState != UIAutomationClient.ToggleState.ToggleState_On)
                                        {
                                            togglePattern.Toggle();  // Select the checkbox
                                            Console.WriteLine("Checkbox selected.");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Checkbox already selected.");
                                        }
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Checkbox not found in row: ");
                                }
                            }
                            else
                            {
                                // If not "ToggleState_On", click another subchild element
                                IUIAutomationElementArray children = uiRowElement.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());
                                for (int index2 = 0; index2 < children.Length; index2++)
                                {
                                    IUIAutomationElement child = children.GetElement(index2);

                                    if (child.CurrentControlType != UIA_ControlTypeIds.UIA_CheckBoxControlTypeId)
                                    {
                                        MouseOperations.ClickElement(child, "Left", true);
                                        Console.WriteLine("Clicked child element at index " + index + " (not a checkbox).");
                                        break;
                                    }
                                }
                            }
                        }

                    }

                    //traverse row of exceldata
                    //check if "CheckBox" column exist then extract its data and and set dr value null
                    //now fetch check whether that row exist in any UI row, if yes then save its index

                    //we need to check whether CheckBox value is "ToggleState_On"
                    //if yes then we need to traverse that index and find any child having checkbox control type
                    //and need to selectthatcheckboxi.e basically select the checkbox if not checked else do nothing
                    //if togglestate is something else..access indexed element and click any of its subchild.i.e 2 or 3rd

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return success;
        }

        private static IUIAutomationElement FindLeftMostVisibleChild(IUIAutomationElementArray elementArray)
        {
            if (elementArray == null || elementArray.Length == 0)
            {
                return null;
            }

            IUIAutomationElement leftMostElement = null;
            double minLeft = double.MaxValue;

            for (int i = 0; i < elementArray.Length; i++)
            {
                IUIAutomationElement element = elementArray.GetElement(i);

                try
                {
                    var rect = element.CurrentBoundingRectangle;
                    Console.WriteLine("Element " + i + " Coordinates: Left = " + rect.left + ", Top = " + rect.top + ", Right = " + rect.right + ", Bottom = " + rect.bottom);

                    if ((rect.right - rect.left > 0) && (rect.bottom - rect.top > 0))
                    {
                        if (rect.left < minLeft)
                        {
                            minLeft = rect.left;
                            leftMostElement = element;
                        }
                    }
                }
                catch { }
                {
                    continue;
                }
            }
            try
            {
                Console.WriteLine(" Found Element Automation Name :" + leftMostElement.CurrentName);
            }
            catch { }
           
            return leftMostElement;
        }

        public bool OpenColumnChooser(string gridID, string expectedModule)
        {
            try
            {
                IUIAutomation automation = new CUIAutomation();
                IUIAutomationElement rootElement = automation.GetRootElement();
                IUIAutomationElement gridwindowElement = null;
                int retryCount = 3;
                int delayMilliseconds = 2000;

                for (int attempt = 1; attempt <= retryCount; attempt++)
                {
                    try
                    {
                        gridwindowElement = UIAutomationHelper.FindElementByUniquePropertyType(
                            automation,
                            rootElement,
                            "",
                           gridID
                        );

                        if (gridwindowElement != null)
                        {
                            break;
                        }
                    }
                    catch
                    {
                        if (attempt == retryCount)
                        {
                            return false;
                        }
                        Thread.Sleep(delayMilliseconds);
                    }
                }

                if (gridwindowElement != null)
                {
                    int MAX_RETRIES = 3;
                    gridwindowElement.SetFocus();
                    string treeViewAutomationId = "ColScrollRegion: 0, RowScrollRegion: 0";

                    if (automation != null && gridwindowElement != null)
                    {
                        IUIAutomationCondition treeViewCondition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, treeViewAutomationId);
                        IUIAutomationElement TreeControlType = gridwindowElement.FindFirst(TreeScope.TreeScope_Descendants, treeViewCondition);

                        if (TreeControlType != null)
                        {
                            IUIAutomationCondition headerElementCondition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_HeaderControlTypeId);

                            IUIAutomationElement headerElement = TreeControlType.FindFirst(TreeScope.TreeScope_Children, headerElementCondition);

                            IUIAutomationElementArray dataheadersChilds = headerElement.FindAll(TreeScope.TreeScope_Descendants, automation.CreateTrueCondition());

                            IUIAutomationElement ExpectedWindow = null;
                            ExpectedWindow = UIAutomationHelper.FindElementByUniquePropertyType(
                                          automation,
                                          rootElement,
                                          "",
                                         expectedModule
                                      );
                            if (ExpectedWindow != null)
                            {
                                return true;
                            }

                            if (dataheadersChilds != null)
                            {
                                IUIAutomationElement firstChildElement = FindLeftMostVisibleChild(dataheadersChilds);
                                //IUIAutomationElement firstChildElement = dataheadersChilds.GetElement(0);
                                if (firstChildElement != null)
                                {
                                    var headerRect = headerElement.CurrentBoundingRectangle;
                                    Console.WriteLine("Header Coordinates: Left = " + headerRect.left + ", Top = " + headerRect.top + ", Right = " + headerRect.right + ", Bottom = " + headerRect.bottom);

                                    var firstChildRect = firstChildElement.CurrentBoundingRectangle;
                                    Console.WriteLine("First Child Coordinates: Left = " + firstChildRect.left + ", Top = " + firstChildRect.top + ", Right = " + firstChildRect.right + ", Bottom = " + firstChildRect.bottom);
                                    int x = firstChildRect.left - 10;
                                    int y = (int)(headerRect.top + (headerRect.bottom - headerRect.top) / 2);

                                    Console.WriteLine("Calculated Coordinates Behind First Child: X = " + x + ", Y = " + y);

                                    while (MAX_RETRIES > 0)
                                    {
                                        Cursor.Position = new System.Drawing.Point(x, y);
                                        Thread.Sleep(2000);

                                        mouse_event(MOUSEEVENTF_LEFTDOWN, x, y, 0, 0);

                                        mouse_event(MOUSEEVENTF_LEFTUP, x, y, 0, 0);
                                        Thread.Sleep(3000);
                                        ExpectedWindow = UIAutomationHelper.FindElementByUniquePropertyType(
                                            automation,
                                            rootElement,
                                            "",
                                           expectedModule
                                        );
                                        MAX_RETRIES--;
                                        if (ExpectedWindow != null)
                                        {
                                            break;
                                        }
                                        Console.WriteLine("Retrying...");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("First child element not found in the header.");
                                    return false;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;

        }
        public bool RightClickAndSelectMenuonActiveGrid(string gridID, DataTable excelData)
        {
            try
            {
                IUIAutomation automation = new CUIAutomation();
                IUIAutomationElement rootElement = automation.GetRootElement();
                IUIAutomationElement gridwindowElement = null;
                int retryCount = 3;
                int delayMilliseconds = 2000;

                for (int attempt = 1; attempt <= retryCount; attempt++)
                {
                    try
                    {
                        gridwindowElement = UIAutomationHelper.FindElementByUniquePropertyType(
                            automation,
                            rootElement,
                            "",
                           gridID
                        );

                        if (gridwindowElement != null)
                        {
                            break;
                        }
                    }
                    catch
                    {
                        if (attempt == retryCount)
                        {
                            return false;
                        }
                        Thread.Sleep(delayMilliseconds);
                    }
                }

                if (gridwindowElement != null)
                {

                    gridwindowElement.SetFocus();
                    string treeViewAutomationId = "ColScrollRegion: 0, RowScrollRegion: 0";

                    if (automation != null && gridwindowElement != null)
                    {
                        IUIAutomationCondition treeViewCondition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, treeViewAutomationId);
                        IUIAutomationElement TreeControlType = gridwindowElement.FindFirst(TreeScope.TreeScope_Descendants, treeViewCondition);


                        if (TreeControlType != null)
                        {
                            IUIAutomationCondition headerElementCondition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_HeaderControlTypeId);

                            IUIAutomationElement headerElement = TreeControlType.FindFirst(TreeScope.TreeScope_Children, headerElementCondition);

                            IUIAutomationElementArray dataheadersChilds = headerElement.FindAll(TreeScope.TreeScope_Descendants, automation.CreateTrueCondition());
                            IUIAutomationElement ExpectedWindow = null;
                            foreach (DataRow dr in excelData.Rows)
                            {
                                foreach (DataColumn col in excelData.Columns)
                                {
                                    int MAX_RETRIES = 3;
                                    var value = dr[col];
                                    if (value != null && !string.IsNullOrEmpty(value.ToString()))
                                    {
                                        Console.WriteLine(col.ColumnName + ": " + value);
                                        if (ApplicationArguments.mapdictionary.ContainsKey(col.ColumnName))
                                        {
                                            if (dataheadersChilds != null)
                                            {
                                                bool elementClicked = false;
                                                var uniqueValues = ApplicationArguments.mapdictionary[col.ColumnName].AutomationUniqueValue.Split(',');

                                                for (int i = 0; i < uniqueValues.Length; i++)
                                                {
                                                    string uniqueVal = uniqueValues[i];

                                                    ExpectedWindow = UIAutomationHelper.FindElementByUniquePropertyType(
                                                        automation,
                                                        rootElement,
                                                        ApplicationArguments.mapdictionary[col.ColumnName].UniquePropertyType,
                                                        uniqueVal);

                                                    if (ExpectedWindow != null)
                                                    {
                                                        var elementRect = ExpectedWindow.CurrentBoundingRectangle;
                                                        int xelementRect = elementRect.left + (elementRect.right - elementRect.left) / 2;
                                                        int yelementRect = elementRect.top + (elementRect.bottom - elementRect.top) / 2;

                                                        Cursor.Position = new System.Drawing.Point(xelementRect, yelementRect);
                                                        Thread.Sleep(5000); 

                                                        mouse_event(MOUSEEVENTF_LEFTDOWN, xelementRect, yelementRect, 0, 0);
                                                        mouse_event(MOUSEEVENTF_LEFTUP, xelementRect, yelementRect, 0, 0);
                                                        Thread.Sleep(4000);

                                                        Console.WriteLine("Element '" + uniqueVal + "' was already visible. Clicked without right-click.");
                                                        elementClicked = true;
                                                        break;
                                                    }
                                                }


                                                IUIAutomationElement firstChildElement = FindLeftMostVisibleChild(dataheadersChilds); 
                                                if (firstChildElement != null)
                                                {

                                                    try
                                                    {
                                                        object scrollItemPatternObj = firstChildElement.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId);
                                                        IUIAutomationScrollItemPattern scrollItemPattern = scrollItemPatternObj as IUIAutomationScrollItemPattern;

                                                        if (scrollItemPattern != null)
                                                        {
                                                            scrollItemPattern.ScrollIntoView();
                                                            Thread.Sleep(5000); 
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Console.WriteLine("ScrollIntoView failed: " + ex.Message);
                                                    }

                                                    var firstHeaderRect = firstChildElement.CurrentBoundingRectangle;
                                                    Console.WriteLine("First Header Coordinates: Left = " + firstHeaderRect.left + ", Top = " + firstHeaderRect.top + ", Right = " + firstHeaderRect.right + ", Bottom = " + firstHeaderRect.bottom);
                                                    int x = firstHeaderRect.left + (firstHeaderRect.right - firstHeaderRect.left) / 2;  // Center of the header
                                                    int y = firstHeaderRect.top + (firstHeaderRect.bottom - firstHeaderRect.top) / 2;  // Center of the header

                                                    Console.WriteLine("Calculated Coordinates for Right Click: X = " + x + ", Y = " + y);
                                                   
                                                    

                                                    while (MAX_RETRIES > 0 && !elementClicked)
                                                    {
                                                        Cursor.Position = new System.Drawing.Point(x, y);
                                                        Thread.Sleep(2000);
                                                        mouse_event(MOUSEEVENTF_RIGHTDOWN, x, y, 0, 0);
                                                        mouse_event(MOUSEEVENTF_RIGHTUP, x, y, 0, 0);

                                                        Thread.Sleep(3000);


                                                        foreach (var uniqueVal in uniqueValues)
                                                        {
                                                            ExpectedWindow = UIAutomationHelper.FindElementByUniquePropertyType(
                                                               automation,
                                                               rootElement,
                                                               ApplicationArguments.mapdictionary[col.ColumnName].UniquePropertyType,
                                                               uniqueVal);

                                                            if (ExpectedWindow != null)
                                                            {
                                                                var elementRect = ExpectedWindow.CurrentBoundingRectangle;
                                                                int xelementRect = elementRect.left + (elementRect.right - elementRect.left) / 2; // Center of the element horizontally
                                                                int yelementRect = elementRect.top + (elementRect.bottom - elementRect.top) / 2; // Center of the element vertically
                                                                Cursor.Position = new System.Drawing.Point(xelementRect, yelementRect);
                                                                Thread.Sleep(5000); 

                                                                mouse_event(MOUSEEVENTF_LEFTDOWN, xelementRect, yelementRect, 0, 0);
                                                                mouse_event(MOUSEEVENTF_LEFTUP, xelementRect, yelementRect, 0, 0);
                                                                Thread.Sleep(3000);
                                                                elementClicked = true;
                                                                MAX_RETRIES = 0;
                                                                break;
                                                            }

                                                        }
                                                        MAX_RETRIES--;

                                                        Console.WriteLine("Retrying...");
                                                    }
                                                }

                                                else
                                                {
                                                    Console.WriteLine("First child element not found in the header.");
                                                    return false;
                                                }
                                            }

                                        }
                                    }
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;

        }

        public static bool EditCT(string moduleID, string gridID, DataTable dt, string treeViewAutomationId)
        {
            bool success = true;

            try
            {
                IUIAutomation automation = new CUIAutomation();
                IUIAutomationElement rootElement = automation.GetRootElement();
                if (ApplicationArguments.IUIAutomationDataTables == null)
                {
                    ApplicationArguments.IUIAutomationDataTables = SamsaraHelperClass.ExcelToSamsaraDataTable(ConfigurationManager.AppSettings["IUIAutomationDataFile"]);
                }
                if (ApplicationArguments.IUIAutomationMappingTables == null)
                {
                    ApplicationArguments.IUIAutomationMappingTables = SamsaraHelperClass.ExcelToSamsaraDataTable(ConfigurationManager.AppSettings["IUIAutomationMappingFile"]);
                }
                ApplicationArguments.mapdictionary = DataUtilities.CreateDictionaryFromDataTable(ApplicationArguments.IUIAutomationMappingTables[moduleID]);
                
                IUIAutomationElement windowElement = null;
                int retryCount = 3;
                int delayMilliseconds = 2000;

                for (int attempt = 1; attempt <= retryCount; attempt++)
                {
                    try
                    {
                        windowElement = UIAutomationHelper.FindElementByUniquePropertyType(
                            automation,
                            rootElement,
                            ApplicationArguments.mapdictionary["ModuleWindow"].UniquePropertyType,
                            ApplicationArguments.mapdictionary["ModuleWindow"].AutomationUniqueValue
                        );

                        if (windowElement != null)
                        {
                            break;
                        }
                    }
                    catch
                    {
                        if (attempt == retryCount)
                        {
                            return false;
                        }
                        Thread.Sleep(delayMilliseconds);
                    }
                }

                if (windowElement != null)
                {
                    IUIAutomationCondition grdCondition = automation.CreatePropertyCondition(
                                   UIA_PropertyIds.UIA_AutomationIdPropertyId, ApplicationArguments.mapdictionary["GridID"].AutomationUniqueValue);
                    IUIAutomationElement gridElement = windowElement.FindFirst(TreeScope.TreeScope_Descendants, grdCondition);

                    IUIAutomationCondition treeViewCondition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, treeViewAutomationId);
                    IUIAutomationElement treeViewElement = gridElement.FindFirst(TreeScope.TreeScope_Descendants, treeViewCondition);
                    if (treeViewElement == null)
                    {
                        Console.WriteLine("TreeView element not found.");
                        return false;
                    }

                    IUIAutomationElement btnRemove = UIAutomationHelper.FindElementByUniquePropertyType(
                       automation,
                       rootElement,
                       ApplicationArguments.mapdictionary["btnRemove"].UniquePropertyType,
                       ApplicationArguments.mapdictionary["btnRemove"].AutomationUniqueValue);
                    IUIAutomationElement btnAdd = UIAutomationHelper.FindElementByUniquePropertyType(
                   automation,
                   rootElement,
                   ApplicationArguments.mapdictionary["btnAddToCloseTrade"].UniquePropertyType,
                   ApplicationArguments.mapdictionary["btnAddToCloseTrade"].AutomationUniqueValue);

                    if (btnRemove != null && btnAdd != null)
                    {

                        IUIAutomationElement btnSave = UIAutomationHelper.FindElementByUniquePropertyType(
                                         automation,
                                         rootElement,
                                         ApplicationArguments.mapdictionary["btnSave"].UniquePropertyType,
                                         ApplicationArguments.mapdictionary["btnSave"].AutomationUniqueValue);

                        if (btnSave != null)
                        {
                            var dataItemCondition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_DataItemControlTypeId);
                            IUIAutomationElementArray dataItems = treeViewElement.FindAll(TreeScope.TreeScope_Descendants, dataItemCondition);

                            if (dataItems != null)
                            {
                                // need to remove old data
                                for (int i = dataItems.Length - 1; i >= 0; i--)
                                {
                                    IUIAutomationElement dataItem = dataItems.GetElement(i);
                                    try
                                    {
                                        IUIAutomationElementArray dataItemChildren = dataItem.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());

                                        for (int j = dataItemChildren.Length - 1; j >= 0; j--)
                                        {
                                            IUIAutomationElement cellElement = dataItemChildren.GetElement(j);
                                            if (cellElement.CurrentName.Contains("Symbol"))
                                            {
                                                cellElement.SetFocus();
                                                MouseOperations.ClickElement(cellElement);
                                                Thread.Sleep(1000);
                                                SendKeys.SendWait("{TAB}");
                                                Thread.Sleep(3000);
                                                btnRemove.SetFocus();
                                                MouseOperations.ClickElement(btnRemove, "Left", true);
                                                Thread.Sleep(3000);
                                                Console.WriteLine("Switched to dialogue box window");// quality code will be updated later
                                                Console.WriteLine("Popup present, pressing Enter key");
                                                SendKeys.SendWait("{ENTER}");
                                                break;
                                            }

                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                    }
                                }


                            }

                            //now add new data
                            int count = dt.Rows.Count;

                            for (int counter = 0; counter < count; counter++)
                            {
                                MouseOperations.ClickElement(btnAdd);
                                Thread.Sleep(1000);
                            }

                            for (int row = 0; row < count; row++)
                            {
                                DataRow dr = dt.Rows[row];

                                dataItems = treeViewElement.FindAll(TreeScope.TreeScope_Descendants, dataItemCondition);

                                if (dataItems != null)
                                {
                                    IUIAutomationElement dataItem = dataItems.GetElement(row);
                                    try
                                    {
                                        IUIAutomationElementArray dataItemChildren = dataItem.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());

                                        for (int j = 0; j < dataItemChildren.Length; j++)
                                        {
                                            IUIAutomationElement childElement = dataItemChildren.GetElement(j);
                                            string elementName = childElement.CurrentName;
                                            string elementAutomationId = childElement.CurrentAutomationId;
                                            Console.WriteLine("Name: " + elementName + " AutomationId: " + elementAutomationId);

                                            DateTime cellDate;
                                            string formula = elementName;

                                            if (dr.Table.Columns.Contains(formula))
                                            {
                                                UIAutomationHelper uiAutomationHelper = new UIAutomationHelper();
                                                Console.WriteLine("Name: " + elementName + " Value to Fill: " + dr[formula].ToString());
                                                string valueToFill = dr[formula].ToString();
                                                double numericValue;
                                                try
                                                {
                                                    object scrollItemPatternObj = childElement.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId);
                                                    IUIAutomationScrollItemPattern scrollItemPattern = scrollItemPatternObj as IUIAutomationScrollItemPattern;

                                                    if (scrollItemPattern != null)
                                                    {
                                                        scrollItemPattern.ScrollIntoView();
                                                        Thread.Sleep(2000); // Allow scrolling to finish
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    Console.WriteLine("ScrollIntoView failed: " + ex.Message);
                                                }
                                                
                                                if (elementName.Contains("Date"))
                                                {
                                                    try
                                                    {
                                                        if (valueToFill.ToUpper().Contains("TODAY"))
                                                        {
                                                                     string tempDate = DataUtilities.DateHandler(valueToFill);
                                                                    string date = String.Format(ExcelStructureConstants.TT_DATE_FORMAT, DateTime.Parse(tempDate));
                                                           valueToFill =date;
                                                        }

                                                    }
                                                    catch(Exception ex) 
                                                    {
                                                     Console.WriteLine(ex.Message);
                                                    }
                                                  
                                                }

                                                childElement.SetFocus();

                                                MouseOperations.ClickElement(childElement);
                                                Thread.Sleep(1000);
                                                IUIAutomationElementArray miniitemItemChildren = childElement.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());

                                                for (int minichild = 0; minichild < miniitemItemChildren.Length; minichild++)
                                                {
                                                    try
                                                    {
                                                        MouseOperations.ClickElement(miniitemItemChildren.GetElement(minichild));
                                                    }
                                                    catch { }
                                                }

                                                 bool result = true;
                                                string foundElementValue = ControlTypeHandler.getValueOfElement(childElement, ref result);

                                                if (!string.Equals(foundElementValue, valueToFill,StringComparison.OrdinalIgnoreCase))
                                                {
                                                    if (double.TryParse(valueToFill, out numericValue))
                                                    {
                                                        try
                                                        {
                                                            IUIAutomationCondition childrenCondition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_EditControlTypeId);
                                                            IUIAutomationElement firstchildrenEdit = childElement.FindFirst(TreeScope.TreeScope_Descendants, childrenCondition);

                                                            uiAutomationHelper.SetTextBoxText(firstchildrenEdit, valueToFill);
                                                        }
                                                        catch
                                                        {
                                                            uiAutomationHelper.SetTextBoxText(childElement, valueToFill);
                                                        }

                                                    }
                                                    else
                                                    {

                                                        UIAutomationHelper.SetValue(childElement, valueToFill);
                                                    }


                                                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                                                }
                                            }

                                        }


                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                    }
                                }

                            }


                          //  MouseOperations.ClickElement(btnSave);

                        }
                    }


                }
                else
                    return false;

                return success;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

        }
    }
}
