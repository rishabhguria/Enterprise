using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Infragistics.Win.UltraWinGrid;

using Nirvana.Admin.PositionManagement;
using Nirvana.Admin.PositionManagement.BusinessObjects;
using Nirvana.Admin.PositionManagement.Controls;
using Nirvana.Admin.PositionManagement.Classes;

namespace Nirvana.Admin.PositionManagement.Forms
{
    public partial class GridTestForm : Form
    {

        #region Grid Column Names
        const string COL_AppReconciliedColumnName = "AppReconciliedColumnName";
        const string COL_Description = "Description";
        const string COL_IsIncludedAsCash = "IsIncludedAsCash";
        const string COL_Type = "Type";
        const string COL_AcceptableDeviationSign = "AcceptableDeviationSign";
        const string COL_AcceptableDeviation = "AcceptableDeviation";
        const string COL_AcceptData = "AcceptData";

        //const string COL_IsSourceDataAccepted = "IsSourceDataAccepted";
        //const string COL_IsApplicationDataAccepted = "IsApplicationDataAccepted";
        #endregion Grid Column Names

        //CheckBoxOnHeader_AlternateCreationFilter headerSourceDataCheckBox = new CheckBoxOnHeader_AlternateCreationFilter();
        UltraGridBand _band =  null;
        DataSourceReconColumnsInfo dataSourceReconColumnsInfo = new DataSourceReconColumnsInfo();

        public GridTestForm()
        {
            InitializeComponent();
            
            SetupBinding();
            _band = grdTest.DisplayLayout.Bands[0];
            ctrlSourceName1.InitControl();

            //headerSourceDataCheckBox._CLICKED += new CheckBoxOnHeader_AlternateCreationFilter.HeaderCheckBoxClickedHandler(headerSourceDataCheckBox__CLICKED);
            //headerSourceDataCheckBox.ColumnHeader = _band.Columns[COL_IsSourceDataAccepted].Header;
            //headerSourceDataCheckBox.AlternateColumnHeader = _band.Columns[COL_IsApplicationDataAccepted].Header;

            ctrlSourceName1.SelectionChanged += new EventHandler(ctrlSourceName1_SelectionChanged);
            //newInfo.PropertyChanged += new PropertyChangedEventHandler(newInfo_PropertyChanged);

            //_formBindingSource.DataSourceChanged += new EventHandler(_formBindingSource_DataSourceChanged);
            
        }

        private void SetupBinding()
        {
            _formBindingSource.DataSource = typeof(DataSourceReconColumnsInfo);
            //grdTest.DataSource = typeof(BindingList<AppReconciliedColumn>);

            BindForm();
            BindGridComboBoxes();
            //BindDataGrid();
        }

        private void BindForm()
        {
            //DataSourceReconColumnsInfo newInfo = new DataSourceReconColumnsInfo();

            //newInfo.DataSourceNameIDValue = new DataSourceNameID(43, "DummyRecord", "DR");
            //newInfo.AppReconciliedColumnList = RetrieveAppReconColumns(43);

            _formBindingSource.DataSource = dataSourceReconColumnsInfo; // newInfo;

            //ctrlSourceName1.DataSource = newInfo; // _formBindingSource;
            //ctrlSourceName1.DataMember = "DataSourceNameIDValue";
            //ctrlSourceName1.InitControl();

            grdTest.DataMember = "AppReconciliedColumnList";
            grdTest.DataSource = _formBindingSource;

        }

        private void BindGridComboBoxes()
        {
            cmbEntryType.DisplayMember = "DisplayText";
            cmbEntryType.ValueMember = "Value";
            cmbEntryType.DataSource = ConvertEnumForBinding(typeof(EntryType));
            Utils.UltraDropDownFilter(cmbEntryType, "DisplayText");

            cmbAcceptData.DisplayMember = "DisplayText";
            cmbAcceptData.ValueMember = "Value";
            cmbAcceptData.DataSource = ConvertEnumForBinding(typeof(AcceptDataFrom));
            Utils.UltraDropDownFilter(cmbAcceptData, "DisplayText");

            List<EnumerationValue> deviationSignList = CreateDeviationSignList();
            cmbDeviationSign.DisplayMember = "DisplayText";
            cmbDeviationSign.ValueMember = "Value";
            cmbDeviationSign.DataSource = deviationSignList;
            Utils.UltraDropDownFilter(cmbDeviationSign, "DisplayText");


        }

        private void BindDataGrid()
        {
            //_grdBindingSource = new BindingSource();

            //BindingList<AppReconciliedColumn> list = RetrieveDataList();

            //_grdBindingSource.DataSource = list;
            //grdTest.DataSource = _grdBindingSource;
        }

        void newInfo_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("DataSourceNameIDValue"))
            {
                PropertyDescriptorCollection propertyDescriptorCollection = TypeDescriptor.GetProperties(_formBindingSource.Current);
            }
        }


        void ctrlSourceName1_SelectionChanged(object sender, EventArgs e)
        {
            
            DataSourceNameID changedDataSourceNameID = ((DataSourceEventArgs)e).DataSourceNameID;
            if (_formBindingSource.List.Count > 0)
            {
                dataSourceReconColumnsInfo = _formBindingSource.List[0] as DataSourceReconColumnsInfo;
            }

            dataSourceReconColumnsInfo.DataSourceNameIDValue = changedDataSourceNameID;
            dataSourceReconColumnsInfo.AppReconciliedColumnList = RetrieveAppReconColumns(changedDataSourceNameID.ID);

            //_formBindingSource.ResetBindings(false);
            //grdTest.DataMember = "AppReconciliedColumnList" ;
            //grdTest.DataSource = dataSourceReconColumnsInfo;
            //grdTest.Invalidate();
        }


        void _formBindingSource_DataSourceChanged(object sender, EventArgs e)
        {
            MessageBox.Show("data source changed");
        }

        //BindingSource _grdBindingSource = null;
        BindingSource _formBindingSource = new BindingSource();
        

       

        private static BindingList<AppReconciliedColumn> RetrieveAppReconColumns(int dataSourceId)
        {
            BindingList<AppReconciliedColumn> list = null;

            if (dataSourceId == 43)
            {
                list = new BindingList<AppReconciliedColumn>();
                AppReconciliedColumn a = new AppReconciliedColumn();
                a.AcceptableDeviationSign = NOTAPPLICABLE;
                a.AcceptableDeviation = 0;
                a.AppReconciliedColumnName = "Data Source";
                a.Description = "Book of records";
                a.IsIncludedAsCash = false;
                a.Type = EntryType.Required;
                a.AcceptData = AcceptDataFrom.Source;
                //a.IsSourceDataAccepted = false;
                //a.IsApplicationDataAccepted = false;

                AppReconciliedColumn a1 = new AppReconciliedColumn();
                a1.AcceptableDeviationSign = NOTAPPLICABLE;
                a1.AcceptableDeviation = 0;
                a1.AppReconciliedColumnName = "Funds";
                a1.Description = "Company Funds";
                a1.IsIncludedAsCash = false;
                a1.Type = EntryType.Required;
                a1.AcceptData = AcceptDataFrom.Source;
                //a1.IsSourceDataAccepted = false;
                //a1.IsApplicationDataAccepted = false;

                AppReconciliedColumn a2 = new AppReconciliedColumn();
                a2.AcceptableDeviationSign = NOTAPPLICABLE;
                a2.AcceptableDeviation = 0;
                a2.AppReconciliedColumnName = "Settlement Date";
                a2.Description = "Settlement Date";
                a2.IsIncludedAsCash = false;
                a2.Type = EntryType.Optional;
                a2.AcceptData = AcceptDataFrom.Application;
                //a2.IsSourceDataAccepted = false;
                //a2.IsApplicationDataAccepted = false;

                AppReconciliedColumn b = new AppReconciliedColumn();
                b.AcceptableDeviationSign = PLUS_MINUS;
                b.AcceptableDeviation = 2;
                b.AppReconciliedColumnName = "Position";
                b.Description = "Position of security";
                b.IsIncludedAsCash = false;
                b.Type = EntryType.Required;
                b.AcceptData = AcceptDataFrom.Source;
                //b.IsSourceDataAccepted = false;
                //b.IsApplicationDataAccepted = true;

                list.Add(a);
                list.Add(a1);
                list.Add(a2);
                list.Add(b);
            }

            if (dataSourceId == 30)
            {
                list = new BindingList<AppReconciliedColumn>();
                AppReconciliedColumn a2 = new AppReconciliedColumn();
                a2.AcceptableDeviationSign = NOTAPPLICABLE;
                a2.AcceptableDeviation = 0;
                a2.AppReconciliedColumnName = "Settlement Date";
                a2.Description = "Settlement Date";
                a2.IsIncludedAsCash = false;
                a2.Type = EntryType.Optional;
                a2.AcceptData = AcceptDataFrom.Source;
                //a2.IsSourceDataAccepted = false;
                //a2.IsApplicationDataAccepted = false;

                AppReconciliedColumn b = new AppReconciliedColumn();
                b.AcceptableDeviationSign = PLUS_MINUS;
                b.AcceptableDeviation = 2;
                b.AppReconciliedColumnName = "Position";
                b.Description = "Position of security";
                b.IsIncludedAsCash = false;
                b.Type = EntryType.Required;
                b.AcceptData = AcceptDataFrom.Source;
                //b.IsSourceDataAccepted = false;
                //b.IsApplicationDataAccepted = true;
                
                list.Add(a2);
                list.Add(b);
            }

            if (dataSourceId == 37)
            {
                list = new BindingList<AppReconciliedColumn>();
                AppReconciliedColumn a2 = new AppReconciliedColumn();
                a2.AcceptableDeviationSign = NOTAPPLICABLE;
                a2.AcceptableDeviation = 0;
                a2.AppReconciliedColumnName = "Settlement Date";
                a2.Description = "Settlement Date";
                a2.IsIncludedAsCash = false;
                a2.Type = EntryType.Optional;
                a2.AcceptData = AcceptDataFrom.Application;
                //a2.IsSourceDataAccepted = false;
                //a2.IsApplicationDataAccepted = false;

                list.Add(a2);

            }

            return list;
        }

        

        const string NOTAPPLICABLE = "N/A";
        const string PLUS = "+";
        const string MINUS = "-";
        const string PLUS_MINUS = "+/-";

        private List<EnumerationValue> CreateDeviationSignList()
        {
            List<EnumerationValue> deviationSignList = new List<EnumerationValue>();

            deviationSignList.Add(new EnumerationValue(NOTAPPLICABLE,NOTAPPLICABLE));
            deviationSignList.Add(new EnumerationValue(PLUS, PLUS));
            deviationSignList.Add(new EnumerationValue(MINUS, MINUS));
            deviationSignList.Add(new EnumerationValue(PLUS_MINUS,PLUS_MINUS));

            return deviationSignList;
        }

        /// <summary>
        /// On the header check box clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void headerSourceDataCheckBox__CLICKED(object sender, CheckBoxOnHeader_AlternateCreationFilter.HeaderCheckBoxEventArgs e)
        {
            
            //CheckBoxOnHeader_AlternateCreationFilter.HeaderCheckBoxEventArgs newEventArg = null;

            //switch (e.Header.Column.Key)
            //{
            //    case COL_IsSourceDataAccepted:
            //        if (e.CurrentCheckState == CheckState.Checked)
            //        {
            //            Infragistics.Win.UltraWinGrid.ColumnHeader colHeader = band.Columns[COL_IsApplicationDataAccepted].Header;
            //            newEventArg = new CheckBoxOnHeader_AlternateCreationFilter.HeaderCheckBoxEventArgs(colHeader, CheckState.Unchecked, grdTest.Rows);
            //        }
            //        break;

            //    case COL_IsApplicationDataAccepted:
            //        if (e.CurrentCheckState == CheckState.Checked)
            //        {
            //            Infragistics.Win.UltraWinGrid.ColumnHeader colHeader = band.Columns[COL_IsSourceDataAccepted].Header;
            //            newEventArg = new CheckBoxOnHeader_AlternateCreationFilter.HeaderCheckBoxEventArgs(colHeader, CheckState.Unchecked, grdTest.Rows);
                        
            //        }
            //        break;
            //}



            //headerSourceDataCheckBox.ON_CLICKED(newEventArg);
            
            //if (e.CurrentCheckState == CheckState.Checked)
            //{
            //    _selcetedBasketIDS.Clear();

            //    // _selectedOrders = new OrderCollection();
            //    foreach (DataRow row in basketCollection.Rows)
            //    {
            //        _selcetedBasketIDS.Add(row[1].ToString());
            //    }
            //    // ApplyAllRowsColor(_selectedRowBackColor, _selectedRowForeColor);
            //}
            //else if (e.CurrentCheckState == CheckState.Unchecked)
            //{
            //    _selcetedBasketIDS.Clear();
            //    // ApplyAllRowsColor(_notTradedBackColor, _notTradedForeColor);
            //}
            //grdUnallocated.Refresh();
        }

        /// <summary>
        /// For Individual check box selection in row.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdUnallocated_MouseUp(object sender, MouseEventArgs e)
        {
            //#region Getting checkBox Cell

            //if (e.Button.ToString() == "Right")
            //    return;
            //UIElement objUIElement;
            //UltraGridCell objUltraGridCell;
            //objUIElement = grdUnallocated.DisplayLayout.UIElement.ElementFromPoint(new Point(e.X, e.Y));
            //if (objUIElement == null)
            //    return;
            //objUltraGridCell = (UltraGridCell)objUIElement.GetContext(typeof(UltraGridCell));
            //if (objUltraGridCell == null)
            //    return;

            //if ((objUltraGridCell.Column.Key != "checkBox"))
            //{
            //    return;

            //}
            //#endregion
            //string selectedbasketID = objUltraGridCell.Row.Cells["TradedBasketID"].Value.ToString();
            //if (_selcetedBasketIDS.Contains(selectedbasketID))
            //{
            //    _selcetedBasketIDS.Remove(selectedbasketID);
            //    ApplyRowColor(grdUnallocated, objUltraGridCell.Row.Index, _notTradedBackColor, _notTradedForeColor);
            //    objUltraGridCell.Value = false;
            //}

            //else
            //{
            //    ApplyRowColor(grdUnallocated, objUltraGridCell.Row.Index, _selectedRowBackColor, _selectedRowForeColor);
            //    _selcetedBasketIDS.Add(selectedbasketID);
            //    objUltraGridCell.Value = true;
            //}
            //  grdUnallocated.Refresh();
        }

        private void grdTest_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            UltraGridBand band = e.Layout.Bands[0];
            e.Layout.Override.WrapHeaderText = Infragistics.Win.DefaultableBoolean.True;
            
            grdTest.DisplayLayout.Override.HeaderAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            
            ///TODO : Remove this hardcoding of column names, We might have some attribute to be put on the name 
            ///of the property.

            UltraGridColumn colAppReconciliedColumnName = band.Columns[COL_AppReconciliedColumnName];
            colAppReconciliedColumnName.Header.Caption = "Available Columns";
            colAppReconciliedColumnName.Header.VisiblePosition = 1;

            UltraGridColumn colDescription = band.Columns[COL_Description];
            colDescription.Header.VisiblePosition = 2;

            UltraGridColumn colIsIncludedAsCash = band.Columns[COL_IsIncludedAsCash];
            colIsIncludedAsCash.Header.Caption = "Include as Cash";
            colIsIncludedAsCash.Header.VisiblePosition = 3;

            UltraGridColumn colType = band.Columns[COL_Type];
            colType.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;
            colType.ButtonDisplayStyle = ButtonDisplayStyle.Always;
            colType.ValueList = cmbEntryType;
            colType.Header.VisiblePosition = 4;
            colType.Width = 100;
            
            UltraGridColumn colAcceptableDeviationSign = band.Columns[COL_AcceptableDeviationSign];
            colAcceptableDeviationSign.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;
            colAcceptableDeviationSign.ButtonDisplayStyle = ButtonDisplayStyle.Always;
            colAcceptableDeviationSign.ValueList = cmbDeviationSign;
            colAcceptableDeviationSign.Header.Caption = "Deviation Sign";
            colAcceptableDeviationSign.Header.VisiblePosition = 5;
            colAcceptableDeviationSign.Width = 100;

            UltraGridColumn colAcceptableDeviation = band.Columns[COL_AcceptableDeviation];
            colAcceptableDeviation.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.IntegerNonNegativeWithSpin;
            colAcceptableDeviation.ButtonDisplayStyle = ButtonDisplayStyle.Always;
            colAcceptableDeviation.MaskInput = "nnn\\ %"; ///TODO : Need to change it to decimal value later on
            colAcceptableDeviation.MaxValue = 100;
            colAcceptableDeviation.Header.Caption = "Acceptable Deviation %";
            colAcceptableDeviation.Header.VisiblePosition = 6;

            UltraGridColumn colAcceptData = band.Columns[COL_AcceptData];
            colAcceptData.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;
            colAcceptData.ButtonDisplayStyle = ButtonDisplayStyle.Always;
            colAcceptData.ValueList = cmbAcceptData;
            colAcceptData.Header.Caption = "Accept Data From";
            colAcceptData.Header.VisiblePosition = 5;
            colAcceptData.Width = 100;


            //UltraGridColumn colIsSourceDataAccepted = band.Columns[COL_IsSourceDataAccepted];
            //colIsSourceDataAccepted.Header.Caption = "Accept Source Data";
            //colIsSourceDataAccepted.Header.VisiblePosition = 7;

            //UltraGridColumn colIsApplicationDataAccepted = band.Columns[COL_IsApplicationDataAccepted];
            //colIsApplicationDataAccepted.Header.Caption = "Accept Application Data";
            //colIsApplicationDataAccepted.Header.VisiblePosition = 8;

            //grdTest.CreationFilter = headerSourceDataCheckBox;

        }

        private List<EnumerationValue> ConvertEnumForBinding(Type enumType) // System.Enum enumeration)
        {
            List<EnumerationValue> results = new List<EnumerationValue>();

            // Use reflection to see what values the enum provides
            string[] members = Enum.GetNames(enumType); //.GetMembers();
            foreach (string member in members)
            {
                string name = member;
                object value = Enum.Parse(enumType, name); 
                results.Add(new EnumerationValue(name, value));
            }

            return results;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            string s = string.Empty;
        }

    }

    

}