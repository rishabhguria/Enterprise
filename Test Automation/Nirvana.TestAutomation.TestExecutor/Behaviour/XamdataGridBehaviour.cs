using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interactivity;
using Nirvana.TestAutomation.Utilities;
using System.Collections;
using Infragistics.Controls.Grids;
using Infragistics.Windows.DataPresenter;
using System.Windows.Forms;
using Nirvana.TestAutomation.Factory;
using Nirvana.TestAutomation.Interfaces;
using System.Data;
using Infragistics.Windows.Editors;

namespace Nirvana.TestAutomation.TestExecutor
{
    public class XamdataGridBehaviour : Behavior<XamDataGrid>
    {
        /// <summary>
        /// RowCount stores the number of rows in xamDataGrid
        /// </summary>
        public static int _rowCount = -1;

        /// <summary>
        /// Global Class Object
        /// </summary>
        public static XamdataGridBehaviour _xamDataGridBehaviourobject = new XamdataGridBehaviour();

        protected override void OnAttached()
        {
            AssociatedObject.CellUpdated += AssociatedObject_CellUpdated;
        }

        private void AssociatedObject_CellUpdated(object sender, Infragistics.Windows.DataPresenter.Events.CellUpdatedEventArgs e)
        {
            try
            {
                var obj = e.Cell.Field.Label;
                if (obj.ToString() == "Workbook")
                {
                    DataRecord cellRow = e.Cell.Record as DataRecord;
                    cellRow.Cells["Modules"].EditorType = typeof(XamComboEditor);
                   
                    Style cellStyle = new Style();
                    XamdataGridBehaviour gridBehaviourObject = new XamdataGridBehaviour();
                    ObservableCollection<string> moduleList = GetAllWorkSheets(e.Cell.Value.ToString(), _xamDataGridBehaviourobject.TestDataFolder);
                    cellStyle.TargetType = typeof(XamComboEditor);
                    cellStyle.Setters.Add(new Setter(XamComboEditor.ItemsSourceProperty, moduleList));
                    cellRow.Cells["Modules"].EditorStyle = cellStyle;
                }
                //else if (obj.ToString() == "Modules")
                //{
                    
                //    XamdataGridBehaviour gridBehaviourObject = new XamdataGridBehaviour();
                //    DataRecord cellRow = e.Cell.Record as DataRecord;
                //    cellRow.Cells["TestCases"].EditorType = typeof(XamComboEditor);
                //    //Style cellStyle = new Style();
                //    //string obj1 = cellRow.Cells["Workbook"].Value.ToString();
                    
                //    ObservableCollection<string> testCaseIDList = GetAllTestCases(e.Cell.Value.ToString(), _xamDataGridBehaviourobject.TestDataFolder, cellRow.Cells["Workbook"].Value.ToString());
                //    //cellStyle.TargetType = typeof(XamComboEditor);
                //    //cellStyle.Setters.Add(new Setter(XamComboEditor.ItemsSourceProperty, testCaseIDList));
                //    //cellRow.Cells["TestCases"].EditorStyle = cellStyle;
                //}
            }
            catch
            {
                throw;
            }
        }

        public static DependencyProperty AddRowProperty = DependencyProperty.Register("AddRowFlag", typeof(bool), typeof(XamdataGridBehaviour), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, AddNewRow));
        public bool AddRowFlag
        {
            get { return (bool)GetValue(AddRowProperty); }
            set { SetValue(AddRowProperty, value); }
        }

        public static DependencyProperty WorkBookListProperty = DependencyProperty.Register("WorkBookList", typeof(ObservableCollection<string>), typeof(XamdataGridBehaviour), new FrameworkPropertyMetadata());
        public ObservableCollection<string> WorkBookList
        {
            get { return (ObservableCollection<string>)GetValue(WorkBookListProperty); }
            set { SetValue(WorkBookListProperty, value); }
        }

        public static DependencyProperty TestDataFolderProperty = DependencyProperty.Register("TestDataFolder", typeof(string), typeof(XamdataGridBehaviour), new FrameworkPropertyMetadata(SetTestCasesList));
        public string TestDataFolder
        {
            get { return (string)GetValue(TestDataFolderProperty); }
            set { SetValue(TestDataFolderProperty, value); }
        }

        public static DependencyProperty TestCasesDictionaryProperty = DependencyProperty.Register("TestCasesDictionary", typeof(Dictionary<string, Dictionary<string, List<string>>>), typeof(XamdataGridBehaviour), new FrameworkPropertyMetadata());
        public Dictionary<string, Dictionary<string, List<string>>> TestCasesDictionary
        {
            get { return (Dictionary<string, Dictionary<string, List<string>>>)GetValue(TestCasesDictionaryProperty); }
            set { SetValue(TestCasesDictionaryProperty, value); }
        }


        public static DependencyProperty CreateDictionaryProperty = DependencyProperty.Register("CreateDictionaryFlag", typeof(bool), typeof(XamdataGridBehaviour), new FrameworkPropertyMetadata(CreateDictionary));
        public bool CreateDictionaryFlag
        {
            get { return (bool)GetValue(CreateDictionaryProperty); }
            set { SetValue(CreateDictionaryProperty, value); }
        }

        public static DependencyProperty DeleteRowProperty = DependencyProperty.Register("DeleteRowFlag", typeof(bool), typeof(XamdataGridBehaviour), new FrameworkPropertyMetadata(DeleteRow));
        public bool DeleteRowFlag
        {
            get { return (bool)GetValue(DeleteRowProperty); }
            set { SetValue(DeleteRowProperty, value); }
        }

        private static void DeleteRow(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                if (!(bool)e.NewValue) return;
                XamdataGridBehaviour._rowCount--;
                XamdataGridBehaviour xamGridbehaviourObject = (XamdataGridBehaviour)d;
                xamGridbehaviourObject.DeleteRowFlag = false;
            }
            catch
            {
                throw;
            }
        }

        private static void CreateDictionary(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                if (!(bool)e.NewValue) return;
                XamdataGridBehaviour xamDataGridBehaviourobj = (XamdataGridBehaviour)d;
                XamDataGrid xamGridObj = xamDataGridBehaviourobj.AssociatedObject;
                Dictionary<string, Dictionary<string, List<string>>> dictionary = new Dictionary<string, Dictionary<string, List<string>>>();
                string duplicateModules = string.Empty;
                foreach (Record row in xamGridObj.Records)
                {
                    DataRecord rowcell = row as DataRecord;
                    string workBook = rowcell.Cells["Workbook"].Value.ToString();
                    string module = rowcell.Cells["Modules"].Value.ToString();
                    string[] testCaseIDList = rowcell.Cells["TestCases"].Value.ToString().Split(',');
                    List<string> testIDList = new List<string>();                    

                    foreach (string testID in testCaseIDList)
                    {
                        testIDList.Add(testID);
                    }

                    if (rowcell.Cells["SelectMethod"].Value.ToString() == "Exclude")
                    {
                        testIDList = xamDataGridBehaviourobj.GetList(workBook, module, testIDList);
                    }

                    if (dictionary.ContainsKey(workBook))
                    {
                        if (dictionary[workBook].ContainsKey(module))
                        {
                            duplicateModules = duplicateModules + " WorkBook: " + workBook + ", Module: " + module + "\n";
                        }
                        else
                        {
                            dictionary[workBook].Add(module, testIDList);
                        }
                    }
                    else
                    {
                        Dictionary<string, List<string>> moduleDictionary = new Dictionary<string, List<string>>();
                        moduleDictionary.Add(module, testIDList);
                        dictionary.Add(workBook, moduleDictionary);
                    }
                }
                if (string.IsNullOrEmpty(duplicateModules)) 
					xamDataGridBehaviourobj.TestCasesDictionary = dictionary;
                else
                {
                    System.Windows.MessageBox.Show(duplicateModules);
                }
                xamDataGridBehaviourobj.CreateDictionaryFlag = false;
            }
            catch
            {
                throw;
            }
        }

        public List<string> GetList(string workBook, string workSheet, List<string> testIDList)
        {
            try
            {
                ITestDataProvider provider = TestDataProvider.GetProvider(ProviderType.OpenXml);
                DataSet testCases = provider.GetTestData(_xamDataGridBehaviourobject.TestDataFolder + "\\" + workBook, 5, 2);

                if (testCases.Tables.Contains(workSheet))
                {
                    foreach (DataRow row in testCases.Tables[workSheet].Rows)
                    {
                        String testcaseid = row[ExcelStructureConstants.COL_TESTCASEID].ToString();
                        if (String.IsNullOrWhiteSpace(testcaseid))
                            continue;
                        if (!testIDList.Contains(testcaseid))
                        {
                            testIDList.Add(testcaseid);
                        }
                        else
                        {
                            testIDList.Remove(testcaseid);
                        }
                    }
                }
                return testIDList;
            }
            catch
            {
                throw;
            }
        }

        public ObservableCollection<string> GetAllWorkSheets(string workBook, string folderPath)
        {
            try
            {
                var a = TestDataFolder;
                ObservableCollection<string> workSheets = new ObservableCollection<string>();
                ITestDataProvider provider = TestDataProvider.GetProvider(ProviderType.OpenXml);
                DataSet testCases = provider.GetTestData(folderPath + "\\" + workBook, 5, 2);
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

        private ObservableCollection<string> GetAllTestCases(string workSheet, string folderPath, string TestFileNameWithExtention)
        {
            try
            {
                ObservableCollection<string> tCases = new ObservableCollection<string>();
                ITestDataProvider provider = TestDataProvider.GetProvider(ProviderType.OpenXml);
                DataSet testCases = provider.GetTestData(folderPath + "\\" + TestFileNameWithExtention, 5, 2);

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

        private static void SetTestCasesList(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                _xamDataGridBehaviourobject = (XamdataGridBehaviour)d;
            }
            catch
            {
                throw;
            }
        }

        private static void AddNewRow(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                if (!(bool)e.NewValue) return;

                XamdataGridBehaviour._rowCount++;
                XamdataGridBehaviour xamDataGridBehaviourobj = (XamdataGridBehaviour)d;
                ObservableCollection<string> workBookList = xamDataGridBehaviourobj.WorkBookList;
                DataRecord newRow = xamDataGridBehaviourobj.AssociatedObject.Records[XamdataGridBehaviour._rowCount] as DataRecord;

                newRow.Cells["Workbook"].EditorType = typeof(XamComboEditor);
                Style newRowStyle = new Style();
                newRowStyle.TargetType = typeof(XamComboEditor);
                newRowStyle.Setters.Add(new Setter(XamComboEditor.ItemsSourceProperty, workBookList));
                newRow.Cells["Workbook"].EditorStyle = newRowStyle;


                newRow.Cells["SelectMethod"].EditorType = typeof(XamComboEditor);
                Style newSelectMethodStyle = new Style();
                newSelectMethodStyle.TargetType = typeof(XamComboEditor);
                newSelectMethodStyle.Setters.Add(new Setter(XamComboEditor.ItemsSourceProperty, new ObservableCollection<string> { "Include", "Exclude" }));
                newRow.Cells["SelectMethod"].EditorStyle = newSelectMethodStyle;

                xamDataGridBehaviourobj.AddRowFlag = false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        
    }
}