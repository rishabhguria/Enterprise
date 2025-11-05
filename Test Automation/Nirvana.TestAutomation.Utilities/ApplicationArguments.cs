using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Nirvana.TestAutomation.Utilities
{
    public class StepWiseController
    {
        public List<string> StepsToRunOnEnterprise { get; set; }
        public List<string> StepsToRunOnSamsara { get; set; }

        public StepWiseController()
        {
            StepsToRunOnEnterprise = new List<string>();
            StepsToRunOnSamsara = new List<string>();
        }
    }


    public static class ApplicationArguments
    {
        /// <summary>
        /// The application startup path
        /// </summary>
        private static String _applicationStartUpPath =Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        /// <summary>
        /// Gets or sets the application start up path.
        /// </summary>
        /// <value>
        /// The application start up path.
        /// </value>
        public static String ApplicationStartUpPath
        {
            get { return _applicationStartUpPath; }
            set { _applicationStartUpPath = value; }
        }

        /// <summary>
        /// The test data folder path
        /// </summary>
        public static String _testDataFolderPath = ApplicationStartUpPath + "\\TestCaseData";

        /// <summary>
        /// Gets or sets the test data folder path.
        /// </summary>
        /// <value>
        /// The test data folder path.
        /// </value>
        public static String TestDataFolderPath
        {
            get { return _testDataFolderPath; }
            set { _testDataFolderPath = value; }
        }
      
        /// <summary>
        /// The set maximum timer for testcase run
        /// </summary>
        private static int _setMaxTimerForTestcaseRun= 20;

        /// <summary>
        /// Gets the set maximum timer for testcase run.
        /// </summary>
        /// <value>
        /// The set maximum timer for testcase run.
        /// </value>
        public static int SetMaxTimerForTestcaseRun
        {
            get { return _setMaxTimerForTestcaseRun; }
        }

        /// <summary>
        /// The Retry Count Value 
        /// </summary>
        private static int _retryCount = 0;
        /// <summary>
        /// Gets or sets the Count After That retry Begin
        /// </summary>
        /// <value>
        /// The Retry Of Fail Test Cases With DataBase cleanUp and Release.
        /// </value>
        public static int RetryCount
        {
            get { return _retryCount; }
            set { _retryCount = value; }
        }

        /// <summary>
        /// Size Of Actual Or Total Test Cases
        /// </summary>
      private static int _retrySize = 0;
        /// <summary>
        /// Total No. Of Test Cases Executed 
        /// </summary>
        /// <value>
        /// gets Or sets the No. Of Test Cases
        /// </value>
        public static int RetrySize
        {
            get { return _retrySize; }
            set { _retrySize = value; }
        }

        /// <summary>
        /// The compression option
        /// 0 is for Taxlots and 1 is for Account_Symbol
        /// </summary>
        private static int _compressionOption = 0;
        /// <summary>
        /// Gets or sets the compression.
        /// </summary>
        /// <value>
        /// The compression.
        /// </value>
        public static int Compression
        {
            get { return _compressionOption; }
            set { _compressionOption = value; }
        }
        /// <summary>
        /// The steps mapping file path
        /// </summary>
        private static String _stepMappingFilePath = ApplicationStartUpPath + "\\TestAutomation.Steps";

        /// <summary>
        /// Gets or sets the step mapping file path.
        /// </summary>
        /// <value>
        /// The step mapping file path.
        /// </value>
        public static String StepMappingFilePath
        {
            get { return _stepMappingFilePath; }
            set { _stepMappingFilePath = value; }
        }

        /// <summary>
        /// The test case file
        /// </summary>
        private static Dictionary<String, Dictionary<String, List<String>>> _testCasesWorkBooks = new Dictionary<string, Dictionary<string, List<string>>>();

        /// <summary>
        /// Gets or sets the name of the test case file.
        /// </summary>
        /// <value>
        /// The name of the test case file.
        /// </value>
        public static Dictionary<String, Dictionary<String, List<String>>> TestCasesDictionary
        {
            get { return _testCasesWorkBooks; }
            set { _testCasesWorkBooks = value; }
        }

        /// <summary>
        /// The test case weight dictionary
        /// </summary>
        public  static Dictionary<string, int> _testCaseWeightDict = new Dictionary<string, int>();

        /// <summary>
        /// Gets or sets the test case weight dictionary.
        /// </summary>
        public static Dictionary<string, int> TestCaseWeightDict
        {
            get { return _testCaseWeightDict; }
            set { _testCaseWeightDict = value; }
        }

        /// <summary>
        /// The drive folder identifier
        /// </summary>
        private static String _driveFolderId = "0BxNxzd2k8eYULThrakVWR1daUk0";

        /// <summary>
        /// Gets or sets the drive folder identifier.
        /// </summary>
        /// <value>
        /// The drive folder identifier.
        /// </value>
        public static String DriveFolderId
        {
            get { return _driveFolderId; }
            set { _driveFolderId = value; }
        }

        /// <summary>
        /// The report folder identifier
        /// </summary>
        private static String _reportFolderId = "";

        /// <summary>
        /// Gets or sets the report folder identifier.
        /// </summary>
        /// <value>
        /// The report folder identifier.
        /// </value>
        public static String ReportFolderId
        {
            get { return _reportFolderId; }
            set { _reportFolderId = value; }
        }

        /// <summary>
        /// The steps mapping file identifier
        /// </summary>
        private static String _stepsMappingFileId = "1QYFtJ2mHXx4fdexe3L-2aE-25_jBsFKtUdXu6cTHfCI";

        /// <summary>
        /// Gets or sets the steps mapping file identifier.
        /// </summary>
        /// <value>
        /// The steps mapping file identifier.
        /// </value>
        public static String StepsMappingFileId
        {
            get { return _stepsMappingFileId; }
            set { _stepsMappingFileId = value; }
        }

        /// <summary>
        /// The upload Slave Test Report
        /// </summary>
        private static Boolean _uploadSlaveTestReport = false;

        /// <summary>
        /// Gets or sets a value indicating whether [upload slave data sheet].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [upload slave data Sheet] otherwise, <c>false</c>.
        /// </value>
        public static Boolean UploadSlaveTestReport
        {
            get { return _uploadSlaveTestReport; }
            set { _uploadSlaveTestReport = value; }
        }

        /// <summary>
        /// The Master
        /// </summary>
        private static Boolean _onMasterConfig = false;

        /// <summary>
        /// Gets or sets a value indicating whether [Master Active Or Not].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [Master Active]; otherwise, <c>false</c>.
        /// </value>
        public static Boolean OnMasterConfig
        {
            get { return _onMasterConfig; }
            set { _onMasterConfig = value; }
        }
        /// <summary>
        /// The download data
        /// </summary>
        private static Boolean _downloadData = false;

        /// <summary>
        /// Gets or sets a value indicating whether [download data].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [download data]; otherwise, <c>false</c>.
        /// </value>
        public static Boolean DownloadData
        {
            get { return _downloadData; }
            set { _downloadData = value; }
        }

        /// <summary>
        /// The run description
        /// </summary>
        private static String _runDescription = "Regression";

        /// <summary>
        /// Gets or sets  The run description.
        /// </summary>
        /// <value>
        /// The email subject.
        /// </value>
        public static String RunDescription
        {
            get { return _runDescription; }
            set { _runDescription = value; }
        }

        /// <summary>
        /// The test case to be run
        /// </summary>
        private static String _testCaseToBeRun = string.Empty;

        /// <summary>
        /// Gets or sets the test case to be run.
        /// </summary>
        /// <value>
        /// The test case to be run.
        /// </value>
        public static String TestCaseToBeRun
        {
            get { return _testCaseToBeRun; }
            set { _testCaseToBeRun = value; }
        }

        /// <summary>
        /// The workbook
        /// </summary>
        private static String _workbook = string.Empty;

        /// <summary>
        /// Gets or sets the workbook.
        /// </summary>
        /// <value>
        /// The workbook
        /// </value>
        public static String Workbook
        {
            get { return _workbook; }
            set { _workbook = value; }
        }
        // <summary>
        /// The sheet name
        /// </summary>
        private static String _sheetName = string.Empty;

        /// <summary>
        /// Gets or sets the sheet Name
        /// </summary>
        /// <value>
        /// The Sheet Name
        /// </value>
        public static String SheetName
        {
            get { return _sheetName; }
            set { _sheetName = value; }
        }
        /// <summary>
        /// The log path
        /// </summary>
        private static String _logFolder = "TestLogs";

        /// <summary>
        /// Gets or sets the log path.
        /// </summary>
        /// <value>
        /// The log path.
        /// </value>
        public static String LogFolder
        {
            get { return _logFolder; }
            set { _logFolder = value; }
        }

        /// <summary>
        /// The skip login
        /// </summary>
        private static Boolean _skipLogin = true;

        /// <summary>
        /// Gets or sets a value indicating whether [skip login].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [skip login]; otherwise, <c>false</c>.
        /// </value>
        public static Boolean SkipLogin
        {
            get { return _skipLogin; }
            set { _skipLogin = value; }
        }

        /// <summary>
        /// The skip start up
        /// </summary>
        private static Boolean _skipStartUp = true;

        /// <summary>
        /// Gets or sets a value indicating whether [skip start up].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [skip start up]; otherwise, <c>false</c>.
        /// </value>
        public static Boolean SkipStartUp
        {
            get { return _skipStartUp; }
            set { _skipStartUp = value; }
        }

        /// <summary>
        /// The skip Simulator Start Up
        /// </summary>
        private static Boolean _skipSimulatorStartUp = true;

        /// <summary>
        /// Gets or sets a value indicating whether [skip Simulator Start Up].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [skip Simulator Start Up]; otherwise, <c>false</c>.
        /// </value>
        public static Boolean SkipSimulatorStartUp
        {
            get { return _skipSimulatorStartUp; }
            set { _skipSimulatorStartUp = value; }
        }
        /// <summary>
        /// /////////////////////
        /// </summary>
        private static Boolean _skipCompliance;
        public static Boolean SkipCompliance
        {
            get { return _skipCompliance; }
            set { _skipCompliance = value; }
        }
     

        private static Boolean _skipBasketCompliance;
        public static Boolean SkipBasketCompliance
        {
            get { return _skipBasketCompliance; }
            set { _skipBasketCompliance = value; }
        }

        /// <summary>
        /// The exit code
        /// </summary>
        private static int _exitCode;

        /// <summary>
        /// Gets or sets the exit code.
        /// </summary>
        /// <value>
        /// The exit code.
        /// </value>
        public static int ExitCode
        {
            get { return _exitCode; }
            set { _exitCode = value; }
        }
        /// <summary>
        /// link of change sheet 
        /// </summary>
        private static string _changeSheet = "14ZdWZu4MzxF7o5bSV8Pa_vz_fRnvQFLGow3TGJFLyxc";

        /// <summary>
        /// Gets or sets the change sheet.
        /// </summary>
        /// <value>
        /// The change sheet.
        /// </value>
        public static string ChangeSheet
        {
            get { return _changeSheet; }
            set { _changeSheet = value; }
        }

        /// <summary>
        /// link of test case file 
        /// </summary>
        private static string _testCasesFile = "1EeiCGm403aJ7vsn9-Uc2ZTIwJoIRTOELOR0wp9NIYj0";

        /// <summary>
        /// Gets or sets the test cases file.
        /// </summary>
        /// <value>
        /// The test cases file.
        /// </value>
        public static string TestCasesFile
        {
            get { return _testCasesFile; }
            set { _testCasesFile = value; }
        }

        private static string _clientDB = "QANirvanaClient";

        /// <summary>
        /// Gets or sets the Client DB.
        /// </summary>
        /// <value>
        /// The test cases file.
        /// </value>
        public static string ClientDB
        {
            get { return _clientDB; }
            set { _clientDB = value; }
        }


        public static String ChromeDriverExePath = @"E:\DistributedAutomation\";


        private static Boolean _sendEmailNotifcations = false;
        /// <summary>
        /// The report file
        /// </summary>
        private static string _reportFile = string.Empty;

        /// <summary>
        /// Gets or sets the report file.
        /// </summary>
        /// <value>
        /// The report file.
        /// </value>
        public static string ReportFile
        {
            get { return _reportFile; }
            set { _reportFile = value; }
        }
        /// Gets or sets a value indicating whether [send email notifcations].
        /// </summary>
        /// <value>
        /// <c>true</c> if [send email notifcations]; otherwise, <c>false</c>.
        /// </value>
        public static Boolean SendEmailNotifcations
        {
            get { return _sendEmailNotifcations; }
            set { _sendEmailNotifcations = value; }
        }

        private static string _senderemail = "buildserver@nirvanasolutions.com";
        /// <summary>
        /// Gets or sets the sender email.
        /// </summary>
        /// <value>
        /// The sender email.
        /// </value>
        public static string SenderEmail
        {
            get { return _senderemail; }
            set { _senderemail = value; }
        }
       

        private static string _receiveremail = "disha.sharma@nirvanasolutions.com";
        /// <summary>
        /// Gets or sets the receiver email.
        /// </summary>
        /// <value>
        /// The receiver email.
        /// </value>
        public static string ReceiverEmail
        {
            get { return _receiveremail; }
            set { _receiveremail = value; }
        }

        private static string _ccemail = string.Empty;
        /// <summary>
        /// Gets or sets the cc email.
        /// </summary>
        /// <value>
        /// The cc email.
        /// </value>
        public static string CcEmail
        {
            get { return _ccemail; }
            set { _ccemail = value; }
        }

        private static String _jbossCompliancePath = string.Empty;
        public static String JbossCompliancePath
        {
            get { return _jbossCompliancePath; }
            set { _jbossCompliancePath = value; }
        }

        private static String _ruleEngineCompliancePath = string.Empty;
        public static String RuleEngineCompliancePath
        {
            get { return _ruleEngineCompliancePath; }
            set { _ruleEngineCompliancePath = value; }
        }

        private static String _esperCompliancePath = string.Empty;
        public static String EsperCompliancePath
        {
            get { return _esperCompliancePath; }
            set { _esperCompliancePath = value; }
        }

        private static String _basketCompliancePath = string.Empty;
        public static String BasketCompliancePath
        {
            get { return _basketCompliancePath; }
            set { _basketCompliancePath = value; }
        }

        private static String _calculationServicePath = string.Empty;
        public static String CalculationServicePath
        {
            get { return _calculationServicePath; }
            set { _calculationServicePath = value; }
        }


       
        /// <summary>
        /// The pricing release path
        /// </summary>
        private static String _pricingReleasePath = string.Empty;

        /// <summary>
        /// Gets or sets the pricing release path.
        /// </summary>
        /// <value>
        /// The pricing release path.
        /// </value>
        public static String PricingReleasePath
        {
            get { return _pricingReleasePath; }
            set { _pricingReleasePath = value; }
        }

        /// <summary>
        /// The server release path
        /// </summary>
        private static String _serverReleasePath = string.Empty;

        /// <summary>
        /// Gets or sets the server release path.
        /// </summary>
        /// <value>
        /// The server release path.
        /// </value>
        public static String ServerReleasePath
        {
            get { return _serverReleasePath; }
            set { _serverReleasePath = value; }
        }

        /// <summary>
        /// The expnl release path
        /// </summary>
        private static String _expnlReleasePath = string.Empty;

        /// <summary>
        /// Gets or sets the expnl release path.
        /// </summary>
        /// <value>
        /// The expnl release path.
        /// </value>
        public static String ExpnlReleasePath
        {
            get { return _expnlReleasePath; }
            set { _expnlReleasePath = value; }
        }

        /// <summary>
        /// The expn UI release path
        /// </summary>
        private static String _expnlReleaseUIPath = string.Empty;

        /// <summary>
        /// Gets or sets the expnl release path.
        /// </summary>
        /// <value>
        /// The expnl release path.
        /// </value>
        public static String ExpnlReleaseUIPath
        {
            get { return _expnlReleaseUIPath; }
            set { _expnlReleaseUIPath = value; }
        }

        // <summary>
        /// The Server UI release path
        /// </summary>
        private static String _serverReleaseUIPath = string.Empty;
        /// <summary>
        /// Gets or sets the server release path.
        /// </summary>
        /// <value>
        /// The server release path.
        /// </value>
        public static String ServerReleaseUIPath
        {
            get { return _serverReleaseUIPath; }
            set { _serverReleaseUIPath = value; }
        }

        /// <summary>
        /// The client release path
        /// </summary>
        private static String _clientReleasePath = string.Empty;

        /// <summary>
        /// Gets or sets the client release path.
        /// </summary>
        /// <value>
        /// The client release path.
        /// </value>
        public static String ClientReleasePath
        {
            get { return _clientReleasePath; }
            set { _clientReleasePath = value; }
        }

        /// <summary>
        /// The _admin release path
        /// </summary>
        private static String _adminReleasePath = string.Empty;

        /// <summary>
        /// Gets or sets the admin release path.
        /// </summary>
        /// <value>
        /// The admin release path.
        /// </value>
        public static String AdminReleasePath
        {
            get { return _adminReleasePath; }
            set { _adminReleasePath = value; }
        }

        /// <summary>
        /// The cameron path
        /// </summary>
        private static String _cameronSimulatorPath = string.Empty;

        /// <summary>
        /// Gets or sets the _cameron release path.
        /// </summary>
        /// <value>
        /// The _cameron release path.
        /// </value>
        public static String CameronSimulatorPath
        {
            get { return _cameronSimulatorPath; }
            set { _cameronSimulatorPath = value; }
        }

     
        /**********************************************CodeChange : 08/01/2018******************************************/
        //Code Change : 07/23/2018 
        //Author : Sumit Chand
        //Comment : Setting the Java env variable from the local system

        /// <summary>
        /// Path for JavaApplication imagepath 
        /// </summary>
        private static String _javaApplicationImagePath = String.IsNullOrEmpty(Environment.GetEnvironmentVariable("JAVA_HOME")) ? string.Empty : Environment.GetEnvironmentVariable("JAVA_HOME");   
        /// <summary>
        /// Gets or sets a path value for Java Application ImagePath.
        /// </summary>

        public static string JavaApplicationImagePath
        {
            get { return _javaApplicationImagePath; }
            set { _javaApplicationImagePath = value; }
        }


        /// <summary>
        /// The _release user name
        /// </summary>
        private static String _releaseUserName = "Support1";

        /// <summary>
        /// Gets or sets the name of the release user.
        /// </summary>
        /// <value>
        /// The name of the release user.
        /// </value>
        public static String ReleaseUserName
        {
            get { return _releaseUserName; }
            set { _releaseUserName = value; }
        }

        /// <summary>
        /// The _release password
        /// </summary>
        private static String _releasePassword = "Nirvana@1";

        /// <summary>
        /// Gets or sets the release password.
        /// </summary>
        /// <value>
        /// The release password.
        /// </value>
        public static String ReleasePassword
        {
            get { return _releasePassword; }
            set { _releasePassword = value; }
        }

        private static String _testCaseCategory = string.Empty;
        /// <summary>
        /// Gets or sets the test case category.
        /// </summary>
        /// <value>
        /// The test case category.
        /// </value>
        public static String TestCaseCategory
        {
            get { return _testCaseCategory; }
            set { _testCaseCategory = value; }
        }

        /// <summary>
        /// Run clean up after test case.
        /// </summary>
        private static Boolean _runCleanUpAfterTestCase = false;
        
        /// <summary>
        /// Gets or sets the clean up step 
        /// </summary>
        public static Boolean RunCleanUpAfterTestCase
        {
            get { return _runCleanUpAfterTestCase; }
            set { _runCleanUpAfterTestCase = value; }
        }        

        /// <summary>
        /// Run clean up after test case.
        /// </summary>
        private static Boolean _runCleanUp = false;

        /// <summary>
        /// Gets or sets the clean up step 
        /// </summary>
        public static Boolean RunCleanUp
        {
            get { return _runCleanUp; }
            set { _runCleanUp = value; }
        }


        /// <summary>
        /// The data base
        /// </summary>
        private static string _dbInstanceName = "localhost\\mssqlserver22";
        /// <summary>
        /// Gets or sets the data base.
        /// </summary>
        /// <value>
        /// The data base.
        /// </value>
        public static string DBInstanceName
        {
            get { return _dbInstanceName; }
            set { _dbInstanceName = value; }
        }


        /// <summary>
        /// Java Home Path
        /// </summary>
        private static string _javaHome_Path;
        public static string JavaHomePath
        {
            get
            {
                _javaHome_Path = Environment.GetEnvironmentVariable("JAVA_HOME");
                return _javaHome_Path;

            }
        }

        /// <summary>
        /// Path where main sheet is present
        /// </summary>
        private static String _masterSheetPath = string.Empty;

        /// <summary>
        /// Gets or sets the path of main sheet .
        /// </summary>
        /// <value>
        /// Main sheet path.
        /// </value>

        public static String MasterSheetPath
        {
            get { return _masterSheetPath; }
            set { _masterSheetPath = value; }
        }

        /// <summary>
        /// Toggle For Copy Log File To Master MAchine
        /// </summary>
        private static String _copyLogToMaster = string.Empty;

        /// <summary>
        /// Gets or sets the argument to copy log file
        /// </summary>
        /// <value>
        /// Boolean True to copy Log to Master
        /// </value>
        public static String CopyLogToMaster
        {
            get { return _copyLogToMaster; }
            set { _copyLogToMaster = value; }
        }

        /// <summary>
        /// The skip DropCopy Start Up
        /// </summary>
        private static Boolean _skipDropCopyStartUp = true;

        /// <summary>
        /// Gets or sets a value indicating whether [skip DropCopy Start Up].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [skip Simulator Start Up]; otherwise, <c>false</c>.
        /// </value>
        public static Boolean SkipDropCopyStartUp
        {
            get { return _skipDropCopyStartUp; }
            set { _skipDropCopyStartUp = value; }
        }

        /// <summary>
        /// The dropcopy path
        /// </summary>
        private static String _dropCopyPath = string.Empty;

        /// <summary>
        /// Gets or sets the dropcopy path.
        /// </summary>
        /// <value>
        /// The _dropcopy path.
        /// </value>

        public static String DropCopyPath
        {
            get { return _dropCopyPath; }
            set { _dropCopyPath = value; }
        }

        // <summary>
        /// Exclude slaves file path
        /// </summary>
        private static string _excludeSlavesFilePath;

        public static string ExcludeSlavesFilePath
        {
            get { return _excludeSlavesFilePath; }
            set { _excludeSlavesFilePath = value; }
        }


        // <summary>
        /// Exit code dictionary
        /// </summary>
        private static Dictionary<int, string> _exitCodeDictionary = new Dictionary<int, string>() { { 1, "Login or Application Start Up Failed" }, { 2, "UpdateDailyCash" }, {3, "Machine Specific issue"} };

        public static Dictionary<int, string> ExitCodeDictionary
        {
            get { return _exitCodeDictionary; }
            set { _exitCodeDictionary = value; }
        }


        private static String _masterPranaPrefPath = string.Empty;

        /// <summary>
        /// Gets or sets the master machine Prana Preferences path 
        /// </summary>
        /// <value>
        /// Master Machine Prana Preferences Path
        /// </value>
        public static String MasterPranaPrefPath
        {
            get { return _masterPranaPrefPath; }
            set { _masterPranaPrefPath = value;}
        }

        private static String _currentPranaPrefPath = string.Empty;


        /// <summary>
        /// Gets or sets the current machine Prana Preferences path on which case is running 
        /// </summary>
        /// <value>
        /// Current Machine Prana Preferences Path
        /// </value>
        public static String CurrentPranaPrefPath
        {
            get { return _currentPranaPrefPath; }
            set { _currentPranaPrefPath = value; }
        }


        private static String _masterSheetBackupPath = string.Empty;

        /// <summary>
        /// Gets or sets the path for bakcup .
        /// </summary>
        /// <value>
        /// Backup path.
        /// </value>

        public static String MasterSheetBackupPath
        {
            get { return _masterSheetBackupPath; }
            set { _masterSheetBackupPath = value; }
        }


        private static String _member = string.Empty;

        public static String MemberPath
        {
            get { return _member; }
            set { _member = value; }
        }
        private static String _deleteunneccesarySheets = string.Empty;
        public static String DeleteUnneccessarySheets
        {
            get { return _deleteunneccesarySheets; }
            set { _deleteunneccesarySheets = value; }
        }


        private static String _dataBasePath = string.Empty;

        /// <summary>
        /// Gets or sets the DataBase Path
        /// </summary>
        /// <value>
        /// DataBase Path
        /// </value>
        public static String DataBasePath
        {
            get { return _dataBasePath; }
            set { _dataBasePath = value; }
        }

        private static string _masterDB = "DistSecurityMasterDev";

        /// <summary>
        /// Gets or sets the Client DB.
        /// </summary>
        /// <value>
        /// The test cases file.
        /// </value>
        public static string MasterDB
        {
            get { return _masterDB; }
            set { _masterDB = value; }
        }


        private static String _tradeserviceUIPath = "";

        /// <summary>
        /// Gets or sets the client release path.
        /// </summary>
        /// <value>
        /// The client release path.
        /// </value>
        public static String TradeServiceUIPath
        {
            get { return _tradeserviceUIPath; }
            set { _tradeserviceUIPath = value; }
        }

        public static string SamsaraReleaseUserName = "Support6";
        public static string ProductDependency = "Enterprise";

        //To create A Dictionary for Module Step Settings file mappings

        public static Dictionary<string, string> _ModuleStepMapping = new Dictionary<string, string>();

        public static string runType = "Enterprise";
        private static string _KakfaPath = @"E:\Kafka\";
        public static string KafkaPath { get { return _KakfaPath; } set { _KakfaPath = value; } }

        public static Dictionary<string, StepWiseController> StepProductTypeControlHandler = new Dictionary<string, StepWiseController>();
        public static List<string> RestartActionList = new List<string>();
        public static bool isVerificationSuceeded = false;

        private static String _automationProviderKey = "TAFX";

        public static String AutomationProviderKey
        {
            get { return _automationProviderKey; }
            set { _automationProviderKey = value; }
        
        }
        public static List<string> PopUpStepsList = new List<string>();
        

        public static string WinAppMappingsFilePath = string.Empty;

        public static DataTable ptt_GridDT = new DataTable();


        public static Dictionary<KeyValuePair<string, string>, Dictionary<string, List<string>>> dictmap = new Dictionary<KeyValuePair<string, string>, Dictionary<string, List<string>>>();
        public static Dictionary<KeyValuePair<string, string>, Dictionary<string, string>> dictmapalldata = new Dictionary<KeyValuePair<string, string>, Dictionary<string, string>>();
        public static Dictionary<string, Dictionary<string, string>> _correctioncolumn = new Dictionary<string, Dictionary<string, string>>();

        public static Dictionary<string, List<string>> HeadersxListViewClassNameMappings = new Dictionary<string, List<string>>();
        public static bool winappStarted = false;

        private static String _jarPath = "E:\\SimulatorActions\\sikulixide-2.0.5.jar";

        public static String JarPath
        {
            get { return _jarPath; }
            set { _jarPath = value; }
        }
        private static String _scriptFilePath = "E:\\SimulatorActions\\SimulatorActionVerifier.sikuli\\SimulatorActionVerifier.py";

        public static String ScriptFilePath
        {
            get { return _scriptFilePath; }
            set { _scriptFilePath = value; }
        }
        public static string columnMappingFile = "";
        public static List<string> GroupedDataOnStepsList = new List<string>();

        private static String _currentlyActiveUser = "Default";

        public static String CurrentlyActiveUser
        {
            get { return _currentlyActiveUser; }
            set { _currentlyActiveUser = value; }
        }

        private static String _requiredActiveUser = "Support1";

        public static String RequiredActiveUser
        {
            get { return _requiredActiveUser; }
            set { _requiredActiveUser = value; }
        }
        
        public static String EnterpriseActiveUser = "Support1";
        public static String SamsaraActiveUser = "Default";
        public static List<string> PrefRestartList = new List<string>();

        // Type of Release
        public static String releaseType = "Dev";
        public static bool isFallbackEnabled = false;

        public static Dictionary<string, DataTable> IUIAutomationDataTables = null;
        public static Dictionary<string, DataTable> IUIAutomationMappingTables = null;

        public static  Dictionary<string, AutomationUniqueControlType> mapdictionary = null;

        public static String PortfolioDBBackUpsMaster = "";

        public static String PortfolioDBBackUpsSlave = "";

        public static String isPreRequisiteType = "";

        public static TestCaseTracker testcaseTracker = new TestCaseTracker();

        public static String UIAutomationCommonActionResult = "";
       
        public static String ActiveStep = "";
        public static String NirvanaUIAutomationModuleName = "UIAutomation";
        public static String UIAutomationClass = "UIAutomationRunData";
        public static String UIAutomationHelperClass = "UIAutomationHelper";
        public static bool UIAutomationRunDataStep= false;

        public static List<string> SkipFixSteps = new List<string>();
        public static int GlobalStepCounter = 0;

        public static String ActiveInputStep = "";
        public static bool isTestCaseFixingRequired = false;
        public static List<string> GlobalErrorList = new List<string>();
        public static DataSet GlobalTestCaseDataSet = new DataSet();
        public static Dictionary<string, TestCaseFixingMode> TestCaseFixingDic = null;
        public static Dictionary<string, DataTable> ITestCaseFixingTables = null;
        public static String ActiveApproach = "";
        public static Dictionary<string, TestCaseFixingRow> TestCaseFixingRowDictionary = new Dictionary<string, TestCaseFixingRow>();
  

        public static void SetApproach(string testCaseId, string approach)
        {
            if (!TestCaseFixingDic.ContainsKey(testCaseId))
            {
                TestCaseFixingDic[testCaseId] = new TestCaseFixingMode();
            }
            TestCaseFixingDic[testCaseId].Approach = approach ?? string.Empty;
        }

        public static void AddStepDetails(string testCaseId, string step, string details)
        {
            if (!TestCaseFixingDic.ContainsKey(testCaseId))
            {
                TestCaseFixingDic[testCaseId] = new TestCaseFixingMode();
            }
            if (!TestCaseFixingDic[testCaseId].StepxDetails.ContainsKey(step))
            {
                TestCaseFixingDic[testCaseId].StepxDetails[step] = new List<string>();
            }
            TestCaseFixingDic[testCaseId].StepxDetails[step].Add(details ?? string.Empty);
        }

        public static void AddStepUpdatedColumns(string testCaseId, string step, string updatedColumns)
        {
            if (!TestCaseFixingDic.ContainsKey(testCaseId))
            {
                TestCaseFixingDic[testCaseId] = new TestCaseFixingMode();
            }
            if (!TestCaseFixingDic[testCaseId].StepxUpdatedColumns.ContainsKey(step))
            {
                TestCaseFixingDic[testCaseId].StepxUpdatedColumns[step] = new List<string>();
            }
            TestCaseFixingDic[testCaseId].StepxUpdatedColumns[step].Add(updatedColumns ?? string.Empty);
        }
        public static bool RemoveTestCase(string testCaseId)
        {
            if (string.IsNullOrWhiteSpace(testCaseId))
                return false;

            return TestCaseFixingDic.Remove(testCaseId);
        }

    }
    public class TestCaseFixingRow
    {
        public string TestCaseID { get; set; }
        public string Step { get; set; }
        public string ExcelRow { get; set; }
        public List<string> UpdatedColumns { get; set; }
        public List<string> OlderValues { get; set; }
        public List<string> NewerValues { get; set; }
        public List<string> Details { get; set; }
        public string Status { get; set; }
        public string Approach { get; set; }

        public TestCaseFixingRow()
        {
            UpdatedColumns = new List<string>();
            OlderValues = new List<string>();
            NewerValues = new List<string>();
            Details = new List<string>();
        }
        public void Reset()
        {
            TestCaseID = string.Empty;
            Step = string.Empty;
            ExcelRow = string.Empty;
            Status = string.Empty;
            Approach = string.Empty;

            UpdatedColumns.Clear();
            OlderValues.Clear();
            NewerValues.Clear();
            Details.Clear();
        }
    }


    public class TestCaseFixingMode
    {
        public string Approach { get; set; }
        public Dictionary<string, List<string>> StepxDetails { get; set; }
        public Dictionary<string, List<string>> StepxUpdatedColumns { get; set; }
        public TestCaseFixingMode() 
        {
            Approach = string.Empty;
            StepxDetails = new Dictionary<string, List<string>>();
            StepxUpdatedColumns = new Dictionary<string, List<string>>();
        }
        public Dictionary<string, List<string>> StepxExcelRow { get; set; }

    }

    public class AutomationUniqueControlType
    {
        public string AutomationUniqueValue { get; set; }
        public string UniquePropertyType { get; set; }
        public string ControlType { get; set; }

        public AutomationUniqueControlType()
        {
            UniquePropertyType = string.Empty;
            ControlType = string.Empty;
        }
    }

    public class TestCaseTracker
    {
        public string TestCaseID { get; set; }
        public string Result { get; set; }
        public string Log { get; set; }
        public string PreRequisiteType{ get; set; }
        public string portfolioDBRestoreFail { get; set; }

        public TestCaseTracker()
        {
            TestCaseID = string.Empty;
            Result = string.Empty;
        }
        public TestCaseTracker(string testCaseID, string result,string log)
        {
            TestCaseID = testCaseID;
            Result = result;
            Log = log;
            
        }
        public void TestCaseTrackerClear()
        {
            TestCaseID = string.Empty;
            Result = string.Empty;
            Log = string.Empty;
            PreRequisiteType = string.Empty;
            portfolioDBRestoreFail = string.Empty;
        }
    }


         
}
