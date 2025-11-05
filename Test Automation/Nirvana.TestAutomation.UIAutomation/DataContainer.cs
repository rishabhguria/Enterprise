using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nirvana.TestAutomation.UIAutomation
{
    public static class DataContainer
    {
        public static string appplicationPath = "E:\\DistributedAutomation\\TestAutomationDev\\Release\\Client Release\\Prana.exe";

        public static string automationId = "PranaMain"; 
        public static string login_id = "txtLoginID_EmbeddableTextBox"; 
        public static string password = "txtPassword_EmbeddableTextBox";
        public static string loginButton = "btnLogin";

        public static string pttClass = "ViewableRecordCollection";

        public static string pttidforclick = "//Edit[@ClassName=\'XamMaskedEditor']";


        public static string xpath_login_id = "//Edit[@AutomationId='txtLoginID_EmbeddableTextBox']";

        public static string xpath_login2 = "//Edit[@AutomationId=\'txtLoginID']";


        public static string xpath_passwrd = "//Edit[@AutomationId=\'txtPassword']";


        public static string xpath_loginbtn = "//Button[@AutomationId=\'btnLogin']";

        public static string xpath_pttGrid = "//DataGrid[@ClassName=\'RecordListGroup\']";

        public static string xpath_PranaMain = "//Window[@AutomationId=\'PranaMain']";
        public static string openPTT = "//Button[@AutomationId=\'[Toolbar : subToolBar Tools] Tool : % Trading Tool - Index : 3 ']";

        public static string AutomationId_PranaMain = "_PranaMain_UltraFormManager_Dock_Area_Top";

        public static string xPath_PTT_Symbol = "//Edit[@AutomationId=\'TextBoxPresenter\']";


        public static string xPathPTTAccounts = "//Custom[@ClassName=\'Cell\']/Custom[@ClassName=\'XamComboEditor\']";

        public static string xDataItems = "//DataItem[@ClassName=\'Record\']";

        public static string Open_PTT = "//Button[@AutomationId=\'[Toolbar : subToolBar Tools] Tool : % Trading Tool - Index : 3 ']";

        public static string Open_RB = "//Button[@AutomationId=\'[Toolbar : subToolBar Tools] Tool : Rebalancer - Index : 4 ']";

        public static string xPath_rb_AccountGroups = "//Custom[@Name=\"CmbAccountsAndGroups\"][@AutomationId=\"CmbAccountsAndGroups\"]/Edit[@AutomationId=\"TextBoxPresenter\"]";

        public static string xPath_rb_FetchBtn = "//Custom[@AutomationId=\"RebalancerView\"]/Pane[@ClassName=\"ScrollViewer\"]/Button[@ClassName=\"Button\"][@Name=\"Fetch\"]";

        public static string xPath_rb_AddCash = "//Edit[@AutomationId=\"CashFowEditor\"]";

        public static string xPath_rb_RebalanceBtn = "//Group[@ClassName=\"GroupBox\"][@Name=\"Rebalance\"]/Button[@ClassName=\"Button\"]";

        public static string xpath_ptt_dataitems = "//DataGrid[@ClassName=\'RecordListGroup\']/DataItem[@ClassName=\'Record\']/Custom[@ClassName=\'Cell\']";

        public static string xameditor = "//Edit[@ClassName =\'XamMaskedEditor\']";

        public static string ptt_Trade = "//Button[@Name=\"Trade\"][@AutomationId=\"btnTrade\"]/Text[@ClassName=\"TextBlock\"][@Name=\"Trade\"]";

        public static string ptt_Calculate = "//Button[@Name=\"Calculate\"][@AutomationId=\"btnCalculate\"]/Text[@ClassName=\"TextBlock\"][@Name=\"Calculate\"]";

        public static string ptt_CreateOrder = "//Button[@Name=\"Create Order \"][@AutomationId=\"btnStage\"]/Text[@ClassName=\"TextBlock\"][@Name=\"Create Order \"]";

        public static string ptt_Export = "//Button[@Name=\"Export\"][@AutomationId=\"btnExport\"]/Text[@ClassName=\"TextBlock\"][@Name=\"Export\"]";


        public static string ptt_Preferences = "//Button[@Name=\"Preferences\"][@AutomationId=\"btnPreferences\"]/Text[@ClassName=\"TextBlock\"][@Name=\"Preferences\"]";


        public static DataTable ptt_GridDT = null;
        public static DataTable ptt_GridDT2 = null;
        public static DataSet CalculatePTT = null;
        
        public static Dictionary<KeyValuePair<string, string>, Dictionary<string, List<string>>> dictmap = new Dictionary<KeyValuePair<string, string>, Dictionary<string, List<string>>>();
        public static Dictionary<KeyValuePair<string, string>, Dictionary<string, string>> dictmapalldata = new Dictionary<KeyValuePair<string, string>, Dictionary<string, string>>();
        public static Dictionary<string, Dictionary<string, string>> _correctioncolumn = new Dictionary<string, Dictionary<string, string>>();

        public static Dictionary<string, List<string>> HeadersxListViewClassNameMappings = new Dictionary<string, List<string>>();
        public static Dictionary<int, string> tempsheetIndexToName = new Dictionary<int, string>();

        public const string Username_Id1 = "Support1";

        public const string Password_Id = "Nirvana@1";

        public const string ptt_customName = "CalculatedValues";
         public const string rb_customName = "RebalacerDataGrid";
         public static CustomizedDataKeeperClasses.ModuleStepWiseGridStorrer rebalanceGridStorrer;
        /*CustomizedDataKeeperClasses.ModuleStepWiseGridStorrer rebalanceGridStorrer = new CustomizedDataKeeperClasses.ModuleStepWiseGridStorrer
        {
            StepName = "CheckRebalancegrid",
            gridType = "WPF",
            duplicateValue = "", // any duplicate column, such as ""
            duplicateValueReplacer = new List<string> { "Lock/Unlock", "Default" },
            gridElementName =  rb_customName
        };
        */
       // DataTable rebalanceGridData = GetWPFData("RebalacerDataGrid", rebalanceGridStorrer);
        public static CustomizedDataKeeperClasses.ModuleStepWiseGridStorrer calculatedValuesGridStorrer;

        public static Dictionary<string, Dictionary<string, List<WinAPPMappings>>> WinAppMapper = new Dictionary<string, Dictionary<string, List<WinAPPMappings>>>();
        

       // DataTable calculatedValuesGridData = GetWPFData("CalculatedValuesGrid", calculatedValuesGridStorrer);

   
    }
} 
