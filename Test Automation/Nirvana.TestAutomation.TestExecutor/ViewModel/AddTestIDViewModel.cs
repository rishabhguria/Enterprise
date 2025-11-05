using GalaSoft.MvvmLight.CommandWpf;
using Infragistics.Windows.DataPresenter;
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
using System.Windows;
using System.Windows.Forms;

namespace Nirvana.TestAutomation.TestExecutor
{
    public class AddTestIDViewModel : BindableBase
    {
        #region Events

        internal event EventHandler OnFormCloseButtonEvent;

        #endregion
        /// <summary>
        /// TestDataFolder Property Stores Path of Workbook
        /// </summary>
        private string _testDataFolder;
        public string TestDataFolder
        {
            get { return _testDataFolder; }
            set { _testDataFolder = value; }
        }
        /// <summary>
        /// GridColumns Class Object
        /// </summary>
        GridColumns _gridRow = new GridColumns();

        public RelayCommand<object> SelectID { get; set; }
        public RelayCommand FormClosingButton { get; set; }

        /// <summary>
        /// XamComboEditor Selected Items Property
        /// </summary>
        private ObservableCollection<object> _selectedItems=new ObservableCollection<object>();
        public ObservableCollection<object> ItemsSelected
        {
            get { return _selectedItems; }
            set { _selectedItems = value; OnPropertyChanged("SelectedItem"); }
        }

        /// <summary>
        /// AddTestIDViewModel Class Constructor
        /// </summary>
        public AddTestIDViewModel()
        {
            LoadTestIDS();
            FormClosingButton = new RelayCommand(() => FormClose());
            SelectID = new RelayCommand<object>((parameter) => SelectTestIDS(parameter));
        }

        /// <summary>
        /// ExecutionDetails Class Object
        /// </summary>
        private ExecutionDetails _executionDetails;
        public ExecutionDetails ExecutionDetailsObject
        {
            get { return _executionDetails; }
            set { _executionDetails = value; }
        }


        public AddTestIDViewModel(GridColumns rowObject, string folderPath,ExecutionDetails executionDetails)
        {
            TestDataFolder = folderPath;
            _gridRow = rowObject;
            LoadTestIDS();
            ExecutionDetailsObject = executionDetails;
            FormClosingButton = new RelayCommand(() => FormClose());
            SelectID = new RelayCommand<object>((parameter) => SelectTestIDS(parameter));
        }

        private object FormClose()
        {
            try
            {
                if (OnFormCloseButtonEvent != null)
                    OnFormCloseButtonEvent(this, EventArgs.Empty);
            }
            catch
            {
                throw;
            }
            return null;
        }

        /// <summary>
        /// This Function Closes AddTestIDViewModelUI window and set testIds Column value to selected items of xamcomboeditor
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private object SelectTestIDS(object parameter)
        {
            try
            {
                Window SelectTestCase = parameter as Window;
                if (SelectTestCase != null)
                {
                    string commaSeperatedTestIDS = "";
                    foreach (object value in ItemsSelected)
                    {
                        commaSeperatedTestIDS = commaSeperatedTestIDS + (value.ToString() + ",");
                    }

                    ExecutionDetailsObject.Cl[ExecutionDetailsObject.Cl.IndexOf(_gridRow)].TestCases = commaSeperatedTestIDS.Substring(0,commaSeperatedTestIDS.Length-1);
                    SelectTestCase.Close();
                }
            }
            catch
            {
                throw;
            }
            return null;
        }

        /// <summary>
        /// TestCases Property
        /// </summary>
        private List<String> _testCases;
        public List<string> TestCases
        {
            get { return _testCases; }
            set { SetProperty(ref _testCases, value); }
        }

        /// <summary>
        /// Load DataSource of XamComboeditor
        /// </summary>
        /// <returns></returns>
        public object LoadTestIDS()
        {
            try
            {
                TestCases=GetTestIDS(_gridRow.Workbook,_gridRow.Modules,TestDataFolder); 
                return null;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get Test Ids From Workbook
        /// </summary>
        /// <param name="workBook"></param>
        /// <param name="workSheet"></param>
        /// <param name="TestDataFolder"></param>
        /// <returns></returns>
        public List<string> GetTestIDS(string workBook, string workSheet, string TestDataFolder)
        {
            try
            {
                List<string> tCases = new List<string>();
                ITestDataProvider provider = TestDataProvider.GetProvider(ProviderType.OpenXml);
                DataSet testCases = provider.GetTestData(TestDataFolder + "\\" + workBook, 5, 2);
                if (testCases.Tables.Contains(workSheet))
                {
                    foreach (DataRow row in testCases.Tables[workSheet].Rows)
                    {
                        String testcaseid = row[ExcelStructureConstants.COL_TESTCASEID].ToString();
                        if (String.IsNullOrWhiteSpace(testcaseid))
                            continue;
                        if (!tCases.Contains(testcaseid))
                        {
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
    }
}
