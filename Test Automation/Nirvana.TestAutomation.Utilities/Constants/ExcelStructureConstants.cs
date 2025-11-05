using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nirvana.TestAutomation.Utilities
{
    public static class ExcelStructureConstants
    {
        /// <summary>
        /// The un allocated trades export file name
        /// </summary>
        public const string UnAllocatedTradesExportFileName = @"UnAllocateExpo.xlsx";

        /// <summary>
        /// The allocated trades export file name
        /// </summary>
        public const string AllocatedTradesExportFileName = @"AllocateExpo.xlsx";

        /// <summary>
        /// The checkallocationfilename
        /// </summary>
        public const string CHECKALLOCATIONFILENAME = @"AllocatedTrades";
        /// <summary>
        /// The PSTExportfilenname
        /// </summary>
        public const string PSTExportName = @"PSTData.xlsx";

        /// <summary>
        /// The dateformat
        /// </summary>
        public const string DATEFORMAT = "yyyyMMdd";

        /// <summary>
        /// 
        /// </summary>
        public const string MONTH_YEAR_DATE_FORMAT = "{0:MM/yyyy}";

        /// <summary>
        /// The allocation_ date_ format
        /// </summary>
        public const string COMMON_DATE_FORMAT = "{0:MM/dd/yyyy}";

        /// <summary>
        /// The date_ format without leading zeroes
        /// </summary>
        public const string DATE_FORMAT = "{0:M/d/yyyy}";

        /// <summary>
        /// The tt date format
        /// </summary>
        public const string TT_DATE_FORMAT = "{0:MM/dd/yyyy}";

        /// <summary>
        /// The inpu t_ sheet
        /// </summary>
        public static string COL_INPUT_SHEET = "Input Sheet ";

        /// <summary>
        /// The col_ modules
        /// </summary>
        public static string COL_MODULES = "Modules";

        /// <summary>
        /// The col_ steps
        /// </summary>
        public static string COL_STEPS = "Steps";

        /// <summary>
        /// The col_ namespace
        /// </summary>
        public static string COL_NAMESPACE = "Namespace Names";

        /// <summary>
        /// The col_ testcaseid
        /// </summary>
        public static string COL_TESTCASEID = "TestCaseID";
        
        /// <summary>
        /// The col category
        /// </summary>
        public static string COL_CATEGORY = "Category";

        /// <summary>
        /// The col_ module
        /// </summary>
        public static string COL_MODULE = "Module";

        /// <summary>
        /// The col_ step
        /// </summary>
        public static string COL_STEP = "Step";

        /// <summary>
        /// The col_ description
        /// </summary>
        public static string COL_DESCRIPTION = "Description";

        /// <summary>
        /// The file_ steps_ mapping
        /// </summary>
        public static string FILE_STEPS_MAPPING = @"\Module Step Settings.xlsx";

        /// <summary>
        /// The blan k_ constant
        /// </summary>
        public static string BLANK_CONST = "$#$";

        /// <summary>
        /// The table test cases
        /// </summary>
        public static string TABLE_TEST_CASES = "TTBlotter";

        /// <summary>
        /// The total input sheets
        /// </summary>
        public static int Total_input_sheets = 4;

        /// <summary>
        /// The fixedpreference filename
        /// </summary>
        public const string FIXEDPREFERENCE_FILENAME = "\\FixedPref.xlsx";

        /// <summary>
        /// The col get all visible data from the grid
        /// </summary>
        public static string COL_GET_ALL_VISIBLE_DATA_FROM_THE_GRID = "GetAllVisibleDataFromTheGrid";

        /// <summary>
        /// The closing moulde name.
        /// </summary>
        public const string CLOSING = "Closing";

        /// <summary>
        /// The Closing Cleanup step name.
        /// </summary>
        public const string CLOSING_CLEAN_UP = "ClosingCleanUp";
        /// <summary>
        /// The Allocation module name.
        /// </summary>
        public const string ALLOCATION = "Allocation";

        /// <summary>
        /// The Cleanup step name of Allocation.
        /// </summary>
        public const string ALLOCATION_CLEAN_UP = "CleanUp";

        /// <summary>
        /// The Blotter export file name
        /// </summary>
        public const string BlotterExportFileName = @"BlotterExport.xls";

        /// <summary>
        /// The Blotter Execution Report  file name
        /// </summary>
        public const string BlotterExecutionReportFileName = @"BlotterExecutionReportExport.xls";

        /// <summary>
        /// Simulator Constant req  for setting manual response as on default start up.
        /// </summary>
        public const string CAMERON_SIMULATOR = "Simulator";

        /// <summary>
        /// Simulator Constant req  for setting manual response as on default start up.
        /// </summary>
        public const string SET_MANUAL_RESP = "SetManualResponse";

        /// <summary>
        /// The Rebalancer grid export file name
        /// </summary>
        public const string REBALANCERGRIDDATA = @"RebalancerGridData.xlsx";

        /// <summary>
        /// The col_ testcaseid
        /// </summary>
        public static string COL_TESTCASEWeight = "CasesMappedCount";

        /// <summary>
        /// The Trade List grid export file name
        /// </summary>
        public const string TRADELISTGRIDDATA = @"TradeListGridData.xlsx";

        /// <summary>
        /// The allocated trades export file name
        /// </summary>
        public const string ShortLocateDownloadedFile = @"ShortLocate";

        /// <summary>
        /// The allocated trades export file name
        /// </summary>
        public const string RebalancerCompExportFile = @"RebalancerCompExpo.csv";

        
        /// <summary>
        /// The allocated trades export file name
        /// </summary>
        public const string BasketCompExportFileName = @"BasketCompExpo.csv";

        /// <summary>
        /// The Pre-Trade Compliance export file name
        /// </summary>

        public const string VerifyCompExportFileName = @"VerifyCompExpo.xls";

        /// <summary>
        /// Simulator Clear req  for setting Clear Ui as on default start up.
        /// </summary>
        public const string ClearUI = @"ClearUI";

        /// <summary>
        /// The Audit Trail trades export file name
        /// </summary>
        public const string AUDITTRAILGRIDDATA = @"AuditTrailGridData.xls";
        /// <summary>
        /// The inpu t_ sheet_1
        /// </summary>
        public static string COL_INPUT_SHEET1 = "Input Sheet 1";
    }
}