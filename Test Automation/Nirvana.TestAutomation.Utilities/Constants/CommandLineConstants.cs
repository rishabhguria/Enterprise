using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nirvana.TestAutomation.Utilities
{
    public static class CommandLineConstants
    {
        /// <summary>
        /// The const_ pricing_release_path
        /// </summary>
        public const string CONST_PRICING_RELEASE_PATH = "-pricingReleasePath";
        /// <summary>
        /// The CONST_JBOSS_COMPLIANCE_PATH
        /// </summary>
        public const string CONST_JBOSS_COMPLIANCE_PATH = "-jbossCompliancePath";
        /// <summary>
        /// The CONST_RULEENGINE_COMPLIANCE_PATH
        /// </summary>
        public const string CONST_RULEENGINE_COMPLIANCE_PATH = "-ruleEngineCompliancePath";
        /// <summary>
        /// CONST_ESPER_COMPLIANCE_PATH
        /// </summary>
        public const string CONST_ESPER_COMPLIANCE_PATH = "-esperCompliancePath";

        /// <summary>
        /// CONST_BASKET_COMPLIANCE_PATH
        /// </summary>
        public const string CONST_BASKET_COMPLIANCE_PATH = "-basketCompliancePath";

        /// <summary>
        /// The const_server_release_path
        /// </summary>
        public const string CONST_SERVER_RELEASE_PATH = "-serverReleasePath";

        /// <summary>
        /// The const_expnl_release_path
        /// </summary>
        public const string CONST_EXPNL_RELEASE_PATH = "-expnlReleasePath";

        /// The constant email subject
        /// </summary>
        public const string CONST_RUN_DESCRIPTION = "-runDescription";
        
		/// <summary>
        /// The const_client_release_path
        /// </summary>
        public const string CONST_CLIENT_RELEASE_PATH = "-clientReleasePath";

        /// <summary>
        /// The const_admin_release_path
        /// </summary>
        public const string CONST_ADMIN_RELEASE_PATH = "-adminReleasePath";

        /// <summary>
        /// The const_string_format_with_space
        /// </summary>
        public const string CONST_STRING_FORMAT_WITH_SPACE = @"""{0}"" ";

        /// <summary>
        /// The const_ run test
        /// </summary>
        public const string CONST_RUNTEST = "-runTest";

        /// The constant Upload Slave Test Report subject
        /// </summary>
        public const string CONST_UPLOAD_SLAVE_TEST_REPORT = "-uploadslavetestreport";

        /// The constant on master configuration 
        /// </summary>
        public const string CONST_ON_MASTER_CONFIG = "-onmasterconfig";

        /// The Constant For Skip Cameron Simulator start Up
        /// </Summary>
        public const string CONST_SKIP_SIMULATOR_START_UP = "-skipsimulatorstartup";

        /// The Constant For Cameron Simulator Path
        /// </Summary>
        public const string CONST_CAMERON_SIMULATOR_PATH = "-cameronsimulatorpath";

        /// <summary>
        /// The const_ log_ path
        /// </summary>
        public const string CONST_LOG_FOLDER = "-logFolder";

        /// <summary>
        /// The const_ Regression Sheet Link
        /// </summary>
        public const string CONST_REGRESSION_SHEET_LINK = "-regressionSheetLink";

        /// <summary>
        /// The const_ Client DB
        /// </summary>
        public const string CONST_CLIENT_DB = "-clientDB";
        

        /// <summary>
        /// The const_test_ file
        /// </summary>
        public const string CONST_TEST_CASES = "-testCases";

        /// <summary>
        /// const_exclude_test_cases
        /// </summary>
        public const string CONST_EXCLUDE_TEST_CASES = "-excludeTestCase";

        /// <summary>
        /// The const_test_ folder
        /// </summary>
        public const string CONST_TEST_FOLDER = "-testFolder";

        /// <summary>
        /// The const_ skip_login
        /// </summary>
        public const string CONST_SKIP_LOGIN = "-skiplogin";

        /// <summary>
        /// The CONST_SKIP_COMPLIANCE
        /// </summary>
        public const string CONST_SKIP_COMPLIANCE = "-skipCompliance";

        /// <summary>
        /// The const run clean up
        /// </summary>
        public const string RUN_CLEAN_UP = "-runCleanUp";

        /// <summary>
        /// The const_ skip_startup
        /// </summary>
        public const string CONST_SKIP_STARTUP = "-skipstartup";

        /// <summary>
        /// The const_test_cases
        /// </summary>
        public const string CONST_TEST_CASE_TO_BE_RUN = "-testCaseToBeRun";

        /// <summary>
        /// The const_download_data
        /// </summary>
        public const string CONST_DOWNLOAD_DATA = "-downloaddata";

        /// <summary>
        /// The const_drive_folder_id
        /// </summary>
        public const string CONST_DRIVE_FOLDER_ID = "-drivefolderid";

        /// <summary>
        /// The constant report folder identifier
        /// </summary>
        public const string CONST_REPORT_FOLDER_ID = "-reportfolderid";

        /// <summary>
        /// The const_test_case_category
        /// </summary>
        public const string CONST_TEST_CASE_CATEGORY = "-testcasecategory";

        /// <summary>
        /// The const_compression_category
        /// </summary>
        public const string CONST_COMPRESSION = "-compression";

        /// <summary>
        /// The constant application startup path
        /// </summary>
        public const string CONST_APPLICATION_STARTUP_PATH = "-applicationstartuppath";

        /// <summary>
        /// The constant cc email
        /// </summary>
        public const string CONST_CC_EMAIL = "-ccemail";

        /// <summary>
        /// The constant send email notification
        /// </summary>
        public const string CONST_SEND_EMAIL_NOTIFICATION = "-sendemailnotification";

        /// <summary>
        /// The constant receiver email
        /// </summary>
        public const string CONST_RECEIVER_EMAIL = "-receiverEmail";

        /// <summary>
        /// The constant sender email
        /// </summary>
        public const string CONST_SENDER_EMAIL = "-senderEmail";

        /// <summary>
        /// The constant select all
        /// </summary>
        public const string CONST_SELECT_ALL = "Select All";

        /// <summary>
        /// The constant unselect all
        /// </summary>
        public const string CONST_UNSELECT_ALL = "Unselect All";

        /// <summary>
        /// The constant run the clean up after test case.
        /// </summary>
        public const string CONST_RUN_CLEANUP_AFTER_TESTCASE = "-runCleanUpAfterTestCase";

        /// <summary>
        /// The constant database version
        /// </summary>
        public const string CONST_DB_INSTANCE_NAME = "-dBInstanceName";

        /// <summary>
        /// The constant master sheet path
        /// </summary>
        public const string CONST_MASTER_SHEET_PATH = "-masterSheetPath";

        /// <summary>
        /// To allow log file to be copied to master machine
        /// </summary>
        public const string CONST_ALLOW_COPY_LOG_FILE_TO_MASTER = "-AllowCopyLogFileToMaster";

        /// <summary>
        /// The constant to skip dropcopy startup
        /// </summary>
        public const string CONST_SKIP_DROPCOPY_STARTUP = "-skipdropcopystartup";

        /// <summary>
        /// The constant for dropcopy path
        /// </summary>
        public const string CONST_DROPCOPY_PATH = "-dropcopypath";

        /// <summary>
        /// The constant master sheet path
        /// </summary>
        public const string CONST_MASTER_SHEET_BK_PATH = "-masterSheetBackupPath";

        public const string CONST_MASTER_PRANA_PREF_PATH = "-masterPranaPrefPath";

        public const string CONST_CURRENT_PRANA_PREF_PATH = "-currentPranaPrefPath";

        public const string CONST_ALLOW_DELETE_UNNECESSARY_FILES = "-AllowDeleteUnnecessaryFiles";
        public const string ProductDependency = "-ProductDependency";
        public const string RunType = "-RunType";

        public const string AUTOMATIONPROVIDER_KEY = "-automationProviderKey";
        public const string WINAPPMAPPINGSFILE = "-WinAPPMappingsFile";
        public const string INTERNALTRETRYSIZE = "-InternalRetrySize";

        public const string automationUserName= "-AutomationUser";
        public const string checkActiveUserNameBeforeCaseRun = "-CheckActiveUser";

    }
}
