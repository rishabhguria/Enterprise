using GalaSoft.MvvmLight.CommandWpf;
using Infragistics.Windows.DataPresenter;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Newtonsoft.Json;
using Nirvana.TestAutomation.Factory;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using Prana.MvvmDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Nirvana.TestAutomation.TestExecutor
{
    public class ExecutionDetails : BindableBase
    {
        #region Members

        /// <summary>
        /// The run description
        /// </summary>
        private string _runDescription;

        /// <summary>
        /// The Master
        /// </summary>
        private bool _onMasterConfig=false;
        /// <summary>
        /// The log folder
        /// </summary>
        private string _logFolder = "";

        /// <summary>
        /// The run clean up after test case
        /// </summary>
        private bool _runCleanUpAfterTestCase;

        /// <summary>
        /// The run clean up
        /// </summary>
        private bool _runCleanUp;

        /// <summary>
        /// The _skip login
        /// </summary>
        private bool _skipLogin;

        /// <summary>
        /// The _skip Simulator Start Up
        /// </summary>
        private bool _skipSimulatorStartUp=true;

        /// <summary>
        /// The _skipCompliance
        /// </summary>
        private bool _skipCompliance;

        /// <summary>
        /// The use SQL server05
        /// </summary>
        private String _dbInstanceName = @"localhost\mssqlserver22";

        /// <summary>
        /// The _download data
        /// </summary>
        private bool _downloadData;

        /// <summary>
        /// The _upload data sheet slaves
        /// </summary>
        private bool _uploadSlaveTestReport=false;
        /// <summary>
        /// The _drive folder identifier
        /// </summary>
        private String _driveFolderId;

        /// <summary>
        /// The _test data folder
        /// </summary>
        private String _testDataFolder;

        /// <summary>
        /// The _skip start up
        /// </summary>
        private bool _skipStartUp;

        /// <summary>
        /// The _test cases
        /// </summary>
        private List<String> _testCases;

        /// <summary>
        /// The _selected test cases
        /// </summary>
        private List<String> _selectedTestCases;

        /// <summary>
        /// The _workbooks
        /// </summary>
        private List<String> _workbooks;

        /// <summary>
        /// The _selected workbooks
        /// </summary>
        private List<String> _selectedWorkbooks;

        /// <summary>
        /// The _worksheets
        /// </summary>
        private List<String> _worksheets;

        /// <summary>
        /// The _selected worksheets
        /// </summary>
        private List<String> _selectedWorksheets;

        /// <summary>
        /// The _test file name
        /// </summary>
        private String _testFileName;

        /// <summary>
        /// _test cases dictionary of selected workbook-> worksheets-> TestCasesIds to be run
        /// </summary>
        private Dictionary<String, Dictionary<String, List<String>>> _testCasesWorkBooks;

        /// <summary>
        /// The _comma separated test case i ds
        /// </summary>
        private string _commaSeparatedTestCaseIDs;

        /// <summary>
        /// The _comma separated test case i ds
        /// </summary>
        private string _commaSeparatedWorkBooks;

        /// <summary>
        /// The _clientDB name
        /// </summary>
        private string _clientDB = "DistNirvanaClientDev";

        /// <summary>
        /// The _comma separated test case i ds
        /// </summary>
        private string _commaSeparatedWorkSheets;

        /// <summary>
        /// The _jbossCompliancePath 
        /// </summary>
        private string _jbossCompliancePath = string.Empty;

        /// <summary>
        /// The _ruleEngineCompliancePath 
        /// </summary>
        private string _ruleEngineCompliancePath = string.Empty;

        /// <summary>
        /// The _esperCompliancePath 
        /// </summary>
        private string _esperCompliancePath = string.Empty; 
        /// <summary>
        /// The pricing release path
        /// </summary>
        private String _pricingReleasePath = string.Empty;

        /// <summary>
        /// The server release path
        /// </summary>
        private String _serverReleasePath = string.Empty;
        /// <summary>
        /// The trade service ui path
        /// </summary>
        private String _serverserviceUIPath = string.Empty;


        /// <summary>
        /// The expnl release path
        /// </summary>
        private String _expnlReleasePath = string.Empty;

        /// <summary>
        /// The client release path
        /// </summary>
        private String _clientReleasePath = string.Empty;

        /// <summary>
        /// The _admin release path
        /// </summary>
        private String _adminReleasePath = string.Empty;

        /// <summary>
        /// Path for cameron simulator
        /// </summary>
        private String _cameronSimulatorPath = @"E:\DistributedAutomation\TestAutomationDev\Release\simulator\examples\win32\Nirvana\CameronSimulator";

        /// <summary>
        /// The ccemail
        /// </summary>
        private string _ccemail = string.Empty;

        /// <summary>
        /// The test case category
        /// </summary>
        private string _testCaseCategory = string.Empty;

        /// <summary>
        /// The compression
        /// </summary>
        private KeyValuePair<int, string> _compression;

        /// <summary>
        /// The send email notifcations
        /// </summary>
        private Boolean _sendEmailNotifcations;

        /// <summary>
        /// The senderemail
        /// </summary>
        private string _senderemail = string.Empty;

        /// <summary>
        /// The receiveremail
        /// </summary>
        private string _receiveremail = string.Empty;

        /// <summary>
        /// The report folder identifier
        /// </summary>
        private string _reportFolderId = string.Empty;
        #endregion Members

        #region Properties

        /// <summary>
        /// Gets or sets the comma separated test case i ds.
        /// </summary>
        /// <value>
        /// The comma separated test case i ds.
        /// </value>
        public String CommaSeparatedTestCaseIDs
        {
            get { return _commaSeparatedTestCaseIDs; }
            set { SetProperty(ref _commaSeparatedTestCaseIDs, value); }
        }
        /// <summary>
        /// Gets or sets the comma separated test case i ds.
        /// </summary>
        /// <value>
        /// The comma separated test case i ds.
        /// </value>
        public String CommaSeparatedWorkBooks
        {
            get { return _commaSeparatedWorkBooks; }
            set { SetProperty(ref _commaSeparatedWorkBooks, value); }
        }

        /// <summary>
        /// Gets or sets the comma separated test case i ds.
        /// </summary>
        /// <value>
        /// The comma separated test case i ds.
        /// </value>
        public String CommaSeparatedWorkSheets
        {
            get { return _commaSeparatedWorkSheets; }
            set { SetProperty(ref _commaSeparatedWorkSheets, value); }
        }

        /// <summary>
        /// Gets or sets the drive folder identifier.
        /// </summary>
        /// <value>
        /// The drive folder identifier.
        /// </value>
        public String DriveFolderId
        {
            get { return _driveFolderId; }
            set { SetProperty(ref _driveFolderId, value); }
        }

        /// <summary>
        /// Gets or sets the report folder identifier.
        /// </summary>
        /// <value>
        /// The report folder identifier.
        /// </value>
        public String ReportFolderId
        {
            get { return _reportFolderId; }
            set { SetProperty(ref _reportFolderId, value); }
        }

        /// <summary>
        /// Gets or sets the log folder.
        /// </summary>
        /// <value>
        /// The log folder.
        /// </value>
        public String LogFolder
        {
            get { return _logFolder; }
            set { SetProperty(ref _logFolder, value); }
        }

        /// <summary>
        /// Gets or sets the run test.
        /// </summary>
        /// <value>
        /// The run test.
        /// </value>
        public RelayCommand RunTest { get; set; }

        /// <summary>
        /// Gets or sets the export tocmd arguments.
        /// </summary>
        /// <value>
        /// The export tocmd arguments.
        /// </value>
        public RelayCommand ExportTocmdArg { get; set; }

        public RelayCommand TestAutomationUILoaded { get; set; }
        /// <summary>
        /// Gets or sets the browse file.
        /// </summary>
        /// <value>
        /// The browse file.
        /// </value>
        public RelayCommand BrowseFile { get; set; }

        /// <summary>
        /// Gets or sets the browse file.
        /// </summary>
        /// <value>
        /// The browse file.
        /// </value>
        public RelayCommand ReflectTestSheets { get; set; }

        /// <summary>
        /// Gets or sets the test case ids.
        /// </summary>
        public RelayCommand BindTestCases { get; set; }

        ///<summary>
        ///Gets or sets the row of ultragrid
        ///</summary>
        public RelayCommand<object> AddRow { get; set; }

        /// <summary>
        /// Gets or Sets the TestID
        /// </summary>
        public RelayCommand<object> AddTestID { get; set; }


        ///<summary>
        ///Delete the row of ultragrid
        ///</summary>
        public RelayCommand<object> DeleteRow { get; set; }

        /// <summary>
        /// Prepares the dictionary of selected test ids
        /// </summary>
        public RelayCommand PrepareDictionary { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [download data].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [download data]; otherwise, <c>false</c>.
        /// </value>
        public bool DownloadData
        {
            get { return _downloadData; }
            set { SetProperty(ref _downloadData, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [upload data slave sheet].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [upload slave data sheet]; otherwise, <c>false</c>.
        /// </value>
        public bool UploadSlaveTestReport
        {
            get { return _uploadSlaveTestReport; }
            set { SetProperty(ref _uploadSlaveTestReport, value); }
        }


        /// <summary>
        /// Gets or sets a value indicating whether [Master Active Or Not].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [Master]; otherwise, <c>false</c>.
        /// </value>
        public bool OnMasterConfig
        {
            get { return _onMasterConfig; }
            set { SetProperty(ref _onMasterConfig, value); }
        }
        
        /// <summary>
        /// Gets or sets a value indicating whether [skip login].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [skip login]; otherwise, <c>false</c>.
        /// </value>
        public bool SkipLogin
        {
            get { return _skipLogin; }
            set { SetProperty(ref _skipLogin, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [skip Simulator Start Up].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [skip Simultaor Start Up]; otherwise, <c>false</c>.
        /// </value>
        public bool SkipSimulatorStartUp
        {
            get { return _skipSimulatorStartUp; }
            set { SetProperty(ref _skipSimulatorStartUp, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [skip Compliance].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [skip Compliance]; otherwise, <c>false</c>.
        /// </value>
        public bool SkipCompliance
        {
            get { return _skipCompliance; }
            set { SetProperty(ref _skipCompliance, value); }
        }


        public String DBInstanceName
        {
            get { return _dbInstanceName; }
            set { SetProperty(ref _dbInstanceName, value); }
        }
        /// <summary>
        /// Gets or sets the compression.
        /// </summary>
        /// <value>
        /// The compression.
        /// </value>
        public KeyValuePair<int, string> Compression
        {
            get { return _compression; }
            set { SetProperty(ref _compression, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [skip start up].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [skip start up]; otherwise, <c>false</c>.
        /// </value>
        public bool SkipStartUp
        {
            get { return _skipStartUp; }
            set { SetProperty(ref _skipStartUp, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [run clean up after test case].
        /// </summary>
        /// <value>
        /// <c>true</c> if [run clean up after test case]; otherwise, <c>false</c>.
        /// </value>
        public bool RunCleanUpAfterTestCase
        {
            get { return _runCleanUpAfterTestCase; }
            set { SetProperty(ref _runCleanUpAfterTestCase, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [run clean up].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [run clean up]; otherwise, <c>false</c>.
        /// </value>
        public bool RunCleanUp
        {
            get { return _runCleanUp; }
            set { SetProperty(ref _runCleanUp, value); }
        }

        /// <summary>
        /// Gets or sets the test data folder.
        /// </summary>
        /// <value>
        /// The test data folder.
        /// </value>
        public String TestDataFolder
        {
            get { return _testDataFolder; }
            set { SetProperty(ref _testDataFolder, value); OnPropertyChanged("TestDataFolder"); }
        }

        /// <summary>
        /// Gets or sets the test cases.
        /// </summary>
        /// <value>
        /// The test cases.
        /// </value>
        public List<string> TestCases
        {
            get { return _testCases; }
            set { SetProperty(ref _testCases, value); }
        }

        /// <summary>
        /// Gets or sets the selected test cases
        /// </summary>
        public List<string> SelectedTestCases
        {
            get { return _selectedTestCases; }
            set { SetProperty(ref _selectedTestCases, value); }
        }
        /// <summary>
        /// Gets or sets workbooks.
        /// </summary>
        /// <value>
        /// The workbooks available.
        /// </value>
        public List<string> WorkBooks
        {
            get { return _workbooks; }
            set { SetProperty(ref _workbooks, value); }
        }

        /// <summary>
        /// The selected workbooks
        /// </summary>
        public List<string> SelectedWorkBooks
        {
            get { return _selectedWorkbooks; }
            set { SetProperty(ref _selectedWorkbooks, value); }
        }

        /// <summary>
        /// Gets or sets workbooks.
        /// </summary>
        /// <value>
        /// The workbooks available.
        /// </value>
        public List<string> WorkSheets
        {
            get { return _worksheets; }
            set { SetProperty(ref _worksheets, value); }
        }

        /// <summary>
        /// The selected worksheets
        /// </summary>
        public List<string> SelectedWorkSheets
        {
            get { return _selectedWorksheets; }
            set { SetProperty(ref _selectedWorksheets, value); }
        }

        /// <summary>
        /// Gets or sets the name of the test file.
        /// </summary>
        /// <value>
        /// The name of the test file.
        /// </value>
        public String TestFileNameWithExtention
        {
            get { return _testFileName; }
            set { SetProperty(ref _testFileName, value); }
        }

        /// <summary>
        /// The final test cases dictionary of selected workbook-> worksheets-> TestCasesIds to be run
        /// </summary>
        public Dictionary<String, Dictionary<String, List<String>>> TestCasesDictionary
        {
            get { return _testCasesWorkBooks; }
            set { SetProperty(ref _testCasesWorkBooks, value); }
        }

        /// <summary>
        /// The test case weight dictionary
        /// </summary>
        public Dictionary<string, int> _testCaseWeightDict = new Dictionary<string, int>();

        /// <summary>
        /// Gets or sets the test case weight dictionary.
        /// </summary>
        public Dictionary<string, int> TestCaseWeightDict
        {
            get { return _testCaseWeightDict; }
            set { _testCaseWeightDict = value; }
        }


        /// <summary>
        /// The delete row flag
        /// </summary>
        public bool _deleteRowFlag = false;
        /// <summary>
        /// Gets or sets a value indicating whether [delete row flag].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [delete row flag]; otherwise, <c>false</c>.
        /// </value>
        public bool DeleteRowFlag
        {
            get { return _deleteRowFlag; }
            set { _deleteRowFlag = value; OnPropertyChanged("DeleteRowFlag"); }
        }

        /// <summary>
        /// The add row flag
        /// </summary>
        public bool _addRowFlag = false;
        /// <summary>
        /// Gets or sets a value indicating whether [add row flag].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [add row flag]; otherwise, <c>false</c>.
        /// </value>
        public bool AddRowFlag
        {
            get { return _addRowFlag; }
            set { _addRowFlag = value; OnPropertyChanged("AddRowFlag"); }
        }

        /// <summary>
        /// The create dictionary flag
        /// </summary>
        public bool _createDictionaryFlag = false;
        /// <summary>
        /// Gets or sets a value indicating whether [create dictionary flag].
        /// </summary>
        /// <value>
        /// <c>true</c> if [create dictionary flag]; otherwise, <c>false</c>.
        /// </value>
        public bool CreateDictionaryFlag
        {
            get { return _createDictionaryFlag; }
            set { _createDictionaryFlag = value; OnPropertyChanged("CreateDictionaryFlag"); }
        }

        /// <summary>
        /// Gets or sets the Client DB Name
        /// </summary>
        public string ClientDB
        {
            get { return _clientDB; }
            set { SetProperty(ref _clientDB, value); }
        }

        /// <summary>
        /// The cl
        /// </summary>
        public ObservableCollection<GridColumns> _cl = new ObservableCollection<GridColumns>();
        /// <summary>
        /// Gets or sets the cl.
        /// </summary>
        /// <value>
        /// The cl.
        /// </value>
        public ObservableCollection<GridColumns> Cl
        {
            get { return _cl; }
            set { _cl = value; }
        }

        /// <summary>
        /// The work book list
        /// </summary>
        public ObservableCollection<string> _workBookList = new ObservableCollection<string>();
        /// <summary>
        /// Gets or sets the work book list.
        /// </summary>
        /// <value>
        /// The work book list.
        /// </value>
        public ObservableCollection<string> WorkBookList
        {
            get { return _workBookList; }
            set { _workBookList = value; OnPropertyChanged("WorkBookList"); }
        }


        /// <summary>
        /// The modules
        /// </summary>
        public ObservableCollection<string> _modules = new ObservableCollection<string>();
        /// <summary>
        /// Gets or sets the modules.
        /// </summary>
        /// <value>
        /// The modules.
        /// </value>
        public ObservableCollection<string> Modules
        {
            get { return _modules; }
            set { _modules = value; }
        }

        /// <summary>
        /// The select type
        /// </summary>
        public ObservableCollection<string> _selectType = new ObservableCollection<string>();
        /// <summary>
        /// Gets or sets the type of the select.
        /// </summary>
        /// <value>
        /// The type of the select.
        /// </value>
        public ObservableCollection<string> SelectType
        {
            get { return _selectType; }
            set { _selectType = value; }
        }

        /// <summary>
        /// The test caseids
        /// </summary>
        public ObservableCollection<string> _testCaseids = new ObservableCollection<string>();
        /// <summary>
        /// Gets or sets the test caseids.
        /// </summary>
        /// <value>
        /// The test caseids.
        /// </value>
        public ObservableCollection<string> TestCaseids
        {
            get { return _testCaseids; }
            set { _testCaseids = value; }
        }

        /// <summary>
        /// Gets or sets the pricing release path.
        /// </summary>
        /// <value>
        /// The pricing release path.
        /// </value>
        public String PricingReleasePath
        {
            get { return _pricingReleasePath; }
            set { SetProperty(ref _pricingReleasePath, value); }
        }

        /// <summary>
        /// Gets or sets the JbossCompliancePath
        /// </summary>
        /// <value>
        /// The Jboss Compliance Path
        /// </value>
        public String JbossCompliancePath
        {
            get { return _jbossCompliancePath; }
            set { SetProperty(ref _jbossCompliancePath, value); }
        }

        /// <summary>
        /// Gets or sets the RuleEngineCompliancePath
        /// </summary>
        /// <value>
        /// The Rule Engine Compliance Path
        /// </value>
        public String RuleEngineCompliancePath
        {
            get { return _ruleEngineCompliancePath; }
            set { SetProperty(ref _ruleEngineCompliancePath, value); }
        }

        /// <summary>
        /// Gets or sets the EsperCompliancePath
        /// </summary>
        /// <value>
        /// The Esper Compliance Path
        /// </value>
        public String EsperCompliancePath
        {
            get { return _esperCompliancePath; ; }
            set { SetProperty(ref _esperCompliancePath, value); }
        }

        



        /// <summary>
        /// Gets or sets the server release path.
        /// </summary>
        /// <value>
        /// The server release path.
        /// </value>
        public String ServerReleasePath
        {
            get { return _serverReleasePath; }
            set { SetProperty(ref _serverReleasePath, value); }
        }

        /// <summary>
        /// Gets or sets the expnl release path.
        /// </summary>
        /// <value>
        /// The expnl release path.
        /// </value>
        public String ExpnlReleasePath
        {
            get { return _expnlReleasePath; }
            set { SetProperty(ref _expnlReleasePath, value); }
        }

        /// <summary>
        /// The expnl compression level
        /// </summary>
        public ObservableDictionary<int, string> _expnlCompressionLevel = new ObservableDictionary<int, string>();

        /// <summary>
        /// Gets or sets the expnl compression level.
        /// </summary>
        /// <value>
        /// The expnl compression level.
        /// </value>
        public ObservableDictionary<int, string> ExpnlCompressionLevel
        {
            get { return _expnlCompressionLevel; }
            set { _expnlCompressionLevel = value; }
        }

        /// GridColumns Row Object
        /// </summary>
        private object _rowObject;
        public object RowObject
        {
            get { return _rowObject; }
            set
            {
                if (!(_rowObject != null && value != null && _rowObject.Equals(value)))
                {
                    _rowObject = value;
                    RaisePropertyChangedEvent("RowObject");
                }
            }
        }

        private static string _testCaseID = "";
        public static string TestCaseID
        {
            get { return _testCaseID; }
            set { _testCaseID = value; }
        }

        private static AddTestIDViewModel _addIDS = null;

        /// <summary>
        /// Gets or sets the client release path.
        /// </summary>
        /// <value>
        /// The client release path.
        /// </value>
        public String ClientReleasePath
        {
            get { return _clientReleasePath; }
            set { SetProperty(ref _clientReleasePath, value); }
        }

        /// <summary>
        /// Gets or sets the trade service UIpath.
        /// </summary>
        /// <value>
        /// The  trade service UIpath.
        /// </value>
        public String TradeServiceUIPath
        {
            get { return _serverserviceUIPath; }
            set { SetProperty(ref _serverserviceUIPath, value); }
        }
        /// <summary>
        /// Gets or sets the admin release path.
        /// </summary>
        /// <value>
        /// The admin release path.
        /// </value>
        public String AdminReleasePath
        {
            get { return _adminReleasePath; }
            set { SetProperty(ref _adminReleasePath, value); }
        }

        /// <summary>
        ///Gets or Sets Simulator Path 
        /// </summary>
        public String CameronSimulatorPath
        {
            get { return _cameronSimulatorPath; }
            set { SetProperty(ref _cameronSimulatorPath, value); }
        }

        public ObservableCollection<string> _category = new ObservableCollection<string>();

        public ObservableCollection<string> Category
        {
            get { return _category; }
            set { SetProperty(ref _category, value); }
        }
        /// <summary>
        /// Gets or sets the test case category.
        /// </summary>
        /// <value>
        /// The test case category.
        /// </value>
        public String TestCaseCategory
        {
            get { return _testCaseCategory; }
            set { SetProperty(ref _testCaseCategory, value); }
        }

        public String RunDescription
        {
            get { return _runDescription; }
            set { SetProperty(ref _runDescription, value); }
        }
        /// <summary>
        /// Gets or sets the cc email.
        /// </summary>
        /// <value>
        /// The cc email.
        /// </value>
        public string CcEmail
        {
            get { return _ccemail; }
            set { SetProperty(ref _ccemail, value); }
        }

        /// <summary>
        /// Gets or sets the sender email.
        /// </summary>
        /// <value>
        /// The sender email.
        /// </value>
        public string SenderEmail
        {
            get { return _senderemail; }
            set { SetProperty(ref _senderemail, value); }
        }

        /// <summary>
        /// Gets or sets the receiver email.
        /// </summary>
        /// <value>
        /// The receiver email.
        /// </value>
        public string ReceiverEmail
        {
            get { return _receiveremail; }
            set { SetProperty(ref _receiveremail, value); }
        }
        /// <summary>
        /// Gets or sets a value indicating whether [send email notifcations].
        /// </summary>
        /// <value>
        /// <c>true</c> if [send email notifcations]; otherwise, <c>false</c>.
        /// </value>
        public Boolean SendEmailNotifcations
        {
            get { return _sendEmailNotifcations; }
            set { SetProperty(ref _sendEmailNotifcations, value); }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutionDetails"/> class.
        /// </summary>
        public ExecutionDetails()
        {
            try
            {
                RunTest = new RelayCommand(() => RunTestEnvironment());
                BrowseFile = new RelayCommand(() => BrowseFileEnvironment());
                ReflectTestSheets = new RelayCommand(() => ReflectTestSheetsinSheetsTab());
                BindTestCases = new RelayCommand(() => BindTestCasesCombo());
                PrepareDictionary = new RelayCommand(() => PrepareTestCaseDictionary());
                AddRow = new RelayCommand<object>((parameter) => AddNewRow(parameter));
                DeleteRow = new RelayCommand<object>((parameter) => DeleteRows(parameter));
                ExportTocmdArg = new RelayCommand(() => ExportTocmdArgEnvironment());
                TestAutomationUILoaded = new RelayCommand(() => OnLoadTestAutomationUI());
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            AddTestID = new RelayCommand<object>((parameter) => AddTestCaseID(parameter));
        }

        /// <summary>
        /// Opens TestIDWindowUI
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private object AddTestCaseID(object parameter)
        {

            try
            {
                GridColumns gridObject = parameter as GridColumns;
                if (_addIDS == null)
                {
                    DialogService dialogService = DialogService.DialogServiceInstance;
                    if (parameter != null)
                        ShowAddTestIdUI(viewModel => dialogService.Show<TestIDWindow>(this, viewModel), parameter as GridColumns, TestDataFolder, this);

                }
                else
                {
                    MessageBox.Show("Add Test CaseID Form is Already Opened");
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return null;
        }

        /// <summary>
        /// Creates AddTestIDViewModel Object and send Folder Path as parameter to Constructor of AddTestIDViewModel Class
        /// </summary>
        /// <param name="showAddTestIdUI"></param>
        /// <param name="selectRow"></param>
        /// <param name="folderPath"></param>
        /// <param name="obj"></param>
        private static void ShowAddTestIdUI(Action<AddTestIDViewModel> showAddTestIdUI, GridColumns selectRow, string folderPath, ExecutionDetails obj)
        {
            try
            {
                _addIDS = new AddTestIDViewModel(selectRow, folderPath, obj);
                _addIDS.OnFormCloseButtonEvent += _addIDS_OnFormCloseButtonEvent;
                showAddTestIdUI(_addIDS);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// This function is called when TestIDWindow is Closed and sets AddTestIDViewModelUI to NULL
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void _addIDS_OnFormCloseButtonEvent(object sender, EventArgs e)
        {
            try
            {
                if (_addIDS != null)
                {
                    _addIDS = null;
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
        /// To prepare the dictionary of test cases ids to be run after selection
        /// </summary>
        /// <returns></returns>
        private object PrepareTestCaseDictionary()
        {
            try
            {
                SelectedWorkBooks = CommaSeparatedWorkBooks.Split(',').ToList();
                SelectedWorkSheets = CommaSeparatedWorkSheets.Split(',').ToList();
                SelectedTestCases = CommaSeparatedTestCaseIDs.Split(',').ToList();
                TestCasesDictionary = new Dictionary<string, Dictionary<string, List<string>>>();
                foreach (string workBook in SelectedWorkBooks)
                {
                    if (!TestCasesDictionary.ContainsKey(workBook))
                        TestCasesDictionary[workBook] = new Dictionary<string, List<string>>();

                    foreach (string workSheet in SelectedWorkSheets)
                    {
                        if (!TestCasesDictionary[workBook].ContainsKey(workSheet))
                        {
                            TestCasesDictionary[workBook][workSheet] = new List<string>();
                        }
                        TestCasesDictionary[workBook][workSheet] = SelectedTestCases;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return null;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// To browse the test cases files to be read at a specified folder location
        /// Changes from files to folder 
        /// </summary>
        /// <returns></returns>
        private object BrowseFileEnvironment()
        {
            try
            {
                FolderBrowserDialog folderDialog = new FolderBrowserDialog();
                DialogResult dialogRes = folderDialog.ShowDialog();
                if (dialogRes == DialogResult.OK && !string.IsNullOrWhiteSpace(folderDialog.SelectedPath))
                {
                    TestDataFolder = folderDialog.SelectedPath;
                    string[] files = Directory.GetFiles(TestDataFolder, "*.xlsx");
                    WorkBooks = new List<String>();
                    foreach (string TestFileNameWithPath in files)
                    {
                        TestFileNameWithExtention = Path.GetFileName(TestFileNameWithPath);
                        WorkBooks.Add(TestFileNameWithExtention);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return null;
        }

        /// <summary>
        /// Deletes Row From XamdataGrid of Test Executor UI
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private object DeleteRows(object parameter)
        {
            try
            {
                GridColumns gridColumnsObject = ((parameter as DataRecord).DataItem) as GridColumns;
                if (gridColumnsObject != null)
                    Cl.Remove(gridColumnsObject);

                OnPropertyChanged("Cl");
                DeleteRowFlag = true;
                return null;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return null;
        }

        /// <summary>
        /// Add Row to Xamdatagrid of TestExecutor UI
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private object AddNewRow(object parameter)
        {
            try
            {
                if (string.IsNullOrEmpty(TestDataFolder))
                {
                    MessageBox.Show("Please Browse Folder");
                    return null;
                }
                GridColumns obj = new GridColumns();
                obj.Workbook = "Select";
                obj.Modules = "Select";
                obj.SelectMethod = "Select";
                obj.TestCases = "Select";
                Cl.Add(obj);
                WorkBookList = new ObservableCollection<string>(WorkBooks);
                AddRowFlag = true;
                return null;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return null;
        }

        /// <summary>
        /// To reflect the selected workbook's test sheets only
        /// </summary>
        /// <returns></returns>
        private object ReflectTestSheetsinSheetsTab()
        {
            try
            {
                SelectedWorkBooks = CommaSeparatedWorkBooks.Split(',').ToList();
                WorkSheets = new List<string>();
                TestFileNameWithExtention = SelectedWorkBooks.ToString();
                foreach (string TestFileNameWithPath in SelectedWorkBooks)
                {
                    TestFileNameWithExtention = Path.GetFileName(TestFileNameWithPath);
                    ITestDataProvider provider = TestDataProvider.GetProvider(ProviderType.OpenXml);
                    DataSet testCases = provider.GetTestData(TestDataFolder + "\\" + TestFileNameWithExtention, 5, 2);
                    for (int tablesCount = 0; tablesCount < testCases.Tables.Count; tablesCount++)  //changes for multiple worksheets/Samee
                    {
                        if (!WorkSheets.Contains("Select All"))
                            WorkSheets.Add("Select All");
                        WorkSheets.Add(testCases.Tables[tablesCount].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return null;
        }

        /// <summary>
        /// Binds the test cases combo of selected worksheets only.
        /// </summary>
        public void BindTestCasesCombo()
        {
            try
            {
                TestCases = new List<String>();
                SelectedWorkSheets = CommaSeparatedWorkSheets.Split(',').ToList();
                ITestDataProvider provider = TestDataProvider.GetProvider(ProviderType.OpenXml);
                DataSet testCases = provider.GetTestData(TestDataFolder + "\\" + TestFileNameWithExtention, 5, 2);
                for (int tablesCount = 0; tablesCount < testCases.Tables.Count; tablesCount++)  //changes for multiple worksheets/Samee
                {
                    if (!TestCases.Contains("Select All"))
                        TestCases.Add("Select All");
                    if (SelectedWorkSheets.Contains(testCases.Tables[tablesCount].ToString()))
                    {
                        foreach (DataRow row in testCases.Tables[tablesCount].Rows)
                        {
                            String testcaseid = row[ExcelStructureConstants.COL_TESTCASEID].ToString();
                            if (String.IsNullOrWhiteSpace(testcaseid))
                                continue;
                            if (!TestCases.Contains(testcaseid))
                                TestCases.Add(testcaseid);
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
        /// Called when [load test automation UI].
        /// </summary>
        public void OnLoadTestAutomationUI()
        {
            try
            {
                //Expnl Compression level list
                ExpnlCompressionLevel.Add(0, "Taxlots");
                ExpnlCompressionLevel.Add(1, "Account Symbols");
                //Test Case Category list
                Category.Add("Acceptance");
                Category.Add("Regression");
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Create the arguments for run test cases
        /// </summary>
        public string CreateRunArguments()
        {
            StringBuilder arguments = new StringBuilder();
            string slaveIP = TestStatusLog.GetSytemIP().ToString();
            string slaveIPName = slaveIP.Substring(8).Replace('.', '_');
            try
            {   if(UploadSlaveTestReport)
                ApplicationArguments.RunDescription = String.Format(RunDescription + "(Distributed)" + '_' + slaveIPName + "-{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now);
                else
                ApplicationArguments.RunDescription = String.Format(RunDescription + '_' + slaveIPName + "-{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now);
                ApplicationArguments.TestCaseCategory = TestCaseCategory;
                // Release path set up
                ApplicationArguments.AdminReleasePath = AdminReleasePath;
                ApplicationArguments.ServerReleasePath = ServerReleasePath;
                ApplicationArguments.ClientReleasePath = ClientReleasePath;
                ApplicationArguments.PricingReleasePath = PricingReleasePath;
                ApplicationArguments.ExpnlReleasePath = ExpnlReleasePath;
                ApplicationArguments.CameronSimulatorPath = CameronSimulatorPath;
                ApplicationArguments.SkipLogin = SkipLogin;
                ApplicationArguments.JbossCompliancePath = JbossCompliancePath;
                ApplicationArguments.RuleEngineCompliancePath = RuleEngineCompliancePath;
                ApplicationArguments.EsperCompliancePath = EsperCompliancePath;
                ApplicationArguments.SkipStartUp = SkipStartUp;
                ApplicationArguments.SkipSimulatorStartUp = SkipSimulatorStartUp;
                ApplicationArguments.SkipCompliance = SkipCompliance;
                // For clean up
                ApplicationArguments.RunCleanUpAfterTestCase = RunCleanUpAfterTestCase;
                ApplicationArguments.RunCleanUp = RunCleanUp;
                // For sending mail
                ApplicationArguments.SendEmailNotifcations = SendEmailNotifcations;
                ApplicationArguments.SenderEmail = SenderEmail;
                ApplicationArguments.ReceiverEmail = ReceiverEmail;
                ApplicationArguments.CcEmail = CcEmail;
                ApplicationArguments.DriveFolderId = DriveFolderId;
                ApplicationArguments.ReportFolderId = ReportFolderId;

                ApplicationArguments.DBInstanceName = DBInstanceName;
                ApplicationArguments.DownloadData = DownloadData;
                ApplicationArguments.UploadSlaveTestReport = UploadSlaveTestReport;
                ApplicationArguments.OnMasterConfig = OnMasterConfig;
                ApplicationArguments.LogFolder = LogFolder;
                ApplicationArguments.ClientDB = ClientDB;
                ApplicationArguments.TestDataFolderPath = TestDataFolder;

                //if download data is set to true, then test data is downloaded to provided location
                if (DownloadData)
                {
                    DataFolderDownloader.DownloadFolder(DriveFolderId, ApplicationArguments.TestDataFolderPath, ApplicationArguments.ApplicationStartUpPath, true);
                    DataFolderDownloader.DownloadFolder(ApplicationArguments.StepsMappingFileId, ApplicationArguments.StepMappingFilePath, ApplicationArguments.ApplicationStartUpPath, false);
                    List<String> workbooks = GetAllExcelFiles();
                    for (int i = 0; i < workbooks.Count; i++)
                    {
                        TestCasesDictionary = DownloadedDictionary(("{'" + workbooks[i] + "':{}}").ToString().Replace("\"", string.Empty));
                        if (TestCaseCategory != "")
                        {
                            if (TestCasesDictionary.Count > 0)
                            {
                                ApplicationArguments.TestCasesDictionary = TestCasesDictionary;
                                string jsonSerialisedString = JsonConvert.SerializeObject(TestCasesDictionary);
                                jsonSerialisedString = jsonSerialisedString.Replace("\"", "'");
                                arguments.Append(CommandLineConstants.CONST_TEST_CASES + " ");
                                arguments.Append(String.Format(CommandLineConstants.CONST_STRING_FORMAT_WITH_SPACE, jsonSerialisedString));
                            }
                            if (TestCaseWeightDict.Count > 0)
                                ApplicationArguments.TestCaseWeightDict = TestCaseWeightDict;
                        }
                    }
                }
                if (!String.IsNullOrEmpty(TestDataFolder))
                {
                    arguments.Append(CommandLineConstants.CONST_TEST_FOLDER + " ");
                    arguments.Append(String.Format(CommandLineConstants.CONST_STRING_FORMAT_WITH_SPACE, TestDataFolder));
                    TestCasesDictionary = GetDictionary();
                    TestCaseWeightDict = GetModuleTestCaseWeights();
                    if (TestCaseCategory != "")
                    {
                        if (TestCasesDictionary.Count > 0)
                        {
                            ApplicationArguments.TestCasesDictionary = TestCasesDictionary;
                            string jsonSerialisedString = JsonConvert.SerializeObject(TestCasesDictionary);
                            jsonSerialisedString = jsonSerialisedString.Replace("\"", "'");
                            arguments.Append(CommandLineConstants.CONST_TEST_CASES + " ");
                            arguments.Append(String.Format(CommandLineConstants.CONST_STRING_FORMAT_WITH_SPACE, jsonSerialisedString));
                        }
                        if (TestCaseWeightDict.Count > 0)
                            ApplicationArguments.TestCaseWeightDict = TestCaseWeightDict;
                    }
                }

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return arguments.ToString();
        }

        private Dictionary<string, int> GetModuleTestCaseWeights()
        {
            Dictionary<string, int> testCasesWeights = new Dictionary<string, int>();
            try
            {
                foreach (string workBookName in TestCasesDictionary.Keys)
                {
                    ITestDataProvider provider = TestDataProvider.GetProvider(ProviderType.OpenXml);
                    DataSet testCases = provider.GetTestData(TestDataFolder + "\\" + workBookName, 5, 2);
                    foreach (string moduleName in TestCasesDictionary[workBookName].Keys.ToList())
                    {
                        if (testCases.Tables.Contains(moduleName))
                        {
                            foreach (DataRow row in testCases.Tables[moduleName].Rows)
                            {
                                int testcaseWeight = 1;

                                try { testcaseWeight = int.Parse(row[ExcelStructureConstants.COL_TESTCASEWeight].ToString()); }
                                catch (Exception) { testcaseWeight = 1; }

                                String testcaseid = row[ExcelStructureConstants.COL_TESTCASEID].ToString();

                                if (String.IsNullOrWhiteSpace(testcaseid))
                                    continue;
                                if (!testCasesWeights.ContainsKey(testcaseid))
                                    testCasesWeights.Add(testcaseid, testcaseWeight);
                            }
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
            return testCasesWeights;
        }

        /// <summary>
        /// Gets all excel files.
        /// </summary>
        /// <returns></returns>
        private List<String> GetAllExcelFiles()
        {
            try
            {
                string[] files = Directory.GetFiles(ApplicationArguments.TestDataFolderPath, "*.xlsx");
                WorkBooks = new List<String>();
                foreach (string TestFileNameWithPath in files)
                {
                    TestFileNameWithExtention = Path.GetFileName(TestFileNameWithPath);
                    if (!TestFileNameWithExtention.Contains("Test Data-Template"))
                        WorkBooks.Add(TestFileNameWithExtention);
                }
                return WorkBooks;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return null;
        }
        /// <summary>
        /// Downloaded the dictionary.
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, Dictionary<string, List<string>>> DownloadedDictionary(string args)
        {
            try
            {
                Dictionary<string, Dictionary<string, List<string>>> dictionary = new Dictionary<string, Dictionary<string, List<string>>>();
                dictionary = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, List<string>>>>(args);
                //if no worksheet present in dictionary then add all the worksheet in dictonary
                foreach (string workbook in dictionary.Keys)
                {
                    if (dictionary[workbook].Count == 0)
                    {
                        List<string> sheets;
                        sheets = GetAllWorkSheet(workbook);

                        foreach (string sheet in sheets)
                        {
                            dictionary[workbook].Add(sheet, new List<string>());
                        }
                    }

                    //check every worksheet present in dictionary 
                    //if present worksheet contains no test cases in dictionary then add all the test case of that worksheet in dictionary
                    foreach (string testCasesList in dictionary[workbook].Keys)
                    {
                        if (dictionary[workbook][testCasesList].Count == 0)
                        {
                            List<string> testCases = GetAllTestCases(workbook, testCasesList);
                            foreach (string testCase in testCases)
                            {
                                if (!dictionary[workbook][testCasesList].Contains(testCase))
                                    dictionary[workbook][testCasesList].Add(testCase);
                            }
                        }
                    }
                }
                return dictionary;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return null;
        }
        /// <summary>
        /// Get all the worksheet
        /// </summary>
        /// <param name="workBook"></param>
        /// <returns></returns>
        private static List<string> GetAllWorkSheet(string workBook)
        {
            try
            {
                List<string> workSheets = new List<string>();
                ITestDataProvider provider = TestDataProvider.GetProvider(ProviderType.OpenXml);
                DataSet testCases = provider.GetTestData(ApplicationArguments.TestDataFolderPath + "\\" + workBook, 5, 2);
                // String testcaseCategory = row[ExcelStructureConstants.COL_CATEGORY].ToString();
                for (int tablesCount = 0; tablesCount < testCases.Tables.Count; tablesCount++)
                {
                    if (!workSheets.Contains(testCases.Tables[tablesCount].TableName))
                        workSheets.Add(testCases.Tables[tablesCount].TableName);
                }
                return workSheets;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// get All the test cases
        /// </summary>
        /// <param name="workSheet"></param>
        /// <returns></returns>
        private static List<string> GetAllTestCases(string workBook, string workSheet)
        {
            try
            {
                List<string> tCases = new List<string>();
                ITestDataProvider provider = TestDataProvider.GetProvider(ProviderType.OpenXml);
                DataSet testCases = provider.GetTestData(ApplicationArguments.TestDataFolderPath + "\\" + workBook, 5, 2);

                if (testCases.Tables.Contains(workSheet))
                {
                    foreach (DataRow row in testCases.Tables[workSheet].Rows)
                    {
                        String testcaseid = row[ExcelStructureConstants.COL_TESTCASEID].ToString();
                        String testcaseCategory = row[ExcelStructureConstants.COL_CATEGORY].ToString();
                        if (String.IsNullOrWhiteSpace(testcaseid))
                            continue;
                        if (!tCases.Contains(testcaseid))
                        {
                            if (!tCases.Contains(testcaseCategory) && testcaseCategory.Equals(ApplicationArguments.TestCaseCategory))
                                tCases.Add(testcaseid);
                        }
                    }
                }
                return tCases;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Runs the test environment.
        /// </summary>
        /// <returns></returns>
        public object RunTestEnvironment()
        {
            try
            {
                string arguments = CreateRunArguments();
                TestExecutor.Run(false);
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }


        /// <summary>
        /// Exports the tocmd argument.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public object ExportTocmdArgEnvironment()
        {
            try
            {
                System.IO.File.WriteAllText(@"C:\cmd.txt", CreateRunArguments());
                System.Diagnostics.Process.Start(@"C:\cmd.txt");
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        /// <summary>
        /// returns dictionary of workbooks, worksheets(modules) and Test Cases
        /// </summary>
        /// <param name="details"></param>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        private Dictionary<string, Dictionary<string, List<string>>> GetDictionary()
        {
            try
            {
                CreateDictionaryFlag = true;
                return TestCasesDictionary;
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// returns list of all work sheets for given workbook
        /// </summary>
        /// <param name="workBook">workbook</param>
        /// <returns></returns>
        private List<string> GetAllWorkSheets(string workBook)
        {
            try
            {
                List<string> workSheets = new List<string>();
                ITestDataProvider provider = TestDataProvider.GetProvider(ProviderType.OpenXml);
                DataSet testCases = provider.GetTestData(TestDataFolder + "\\" + TestFileNameWithExtention, 5, 2);
                for (int tablesCount = 0; tablesCount < testCases.Tables.Count; tablesCount++)
                {
                    if (!workSheets.Contains(testCases.Tables[tablesCount].TableName))
                        workSheets.Add(testCases.Tables[tablesCount].TableName);
                }
                return workSheets;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// returns list of all test cases for given worksheet
        /// </summary>
        /// <param name="workSheet">worksheet</param>
        /// <returns></returns>
        private List<string> GetAllTestCases(string workSheet)
        {
            try
            {
                List<string> tCases = new List<string>();
                ITestDataProvider provider = TestDataProvider.GetProvider(ProviderType.OpenXml);
                DataSet testCases = provider.GetTestData(TestDataFolder + "\\" + TestFileNameWithExtention, 5, 2);

                if (testCases.Tables.Contains(workSheet))
                {
                    foreach (DataRow row in testCases.Tables[workSheet].Rows)
                    {
                        String testcaseid = row[ExcelStructureConstants.COL_TESTCASEID].ToString();
                        if (String.IsNullOrWhiteSpace(testcaseid))
                            continue;
                        if (!tCases.Contains(testcaseid))
                        {
                            CommaSeparatedTestCaseIDs += testcaseid + ",";
                            tCases.Add(testcaseid);
                        }
                    }
                }
                return tCases;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Methods
    }
}
