using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using Infragistics.Win.UltraWinGrid;

using Nirvana.Admin.PositionManagement.Classes;
using Nirvana.Admin.PositionManagement.BusinessObjects;

namespace Nirvana.Admin.PositionManagement.Controls
{
    public partial class CtrlSetupTradeRecon : UserControl
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

        BindingSource _formBindingSource = new BindingSource();
        DataSourceReconColumnsInfo _dataSourceReconColumnsInfo = new DataSourceReconColumnsInfo();

        internal event EventHandler CancelClicked;

        public CtrlSetupTradeRecon()
        {
            InitializeComponent();
            
        }

        #region Initialize the control
        private bool _isInitialized = false;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is initialized.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is initialized; otherwise, <c>false</c>.
        /// </value>
        public bool IsInitialized
        {   
            get { return _isInitialized; }
            set { _isInitialized = value; }
        }


        /// <summary>
        /// Initialize the control.
        /// </summary>
        public void InitControl()
        {
            if (!_isInitialized)
            {
                ctrlSourceName1.InitControl();
                SetupBinding();
                _isInitialized = true;
            }
        }
       
        #endregion

        private void SetupBinding()
        {
            //_formBindingSource.DataSource = typeof(DataSourceReconColumnsInfo);
            _formBindingSource.DataSource = _dataSourceReconColumnsInfo; // newInfo;

            BindGridComboBoxes();

            grdAppReconCols.DataMember = "AppReconciliedColumnList";
            grdAppReconCols.DataSource = _formBindingSource;
            
        }

        private void BindGridComboBoxes()
        {
            cmbEntryType.DisplayMember = "DisplayText";
            cmbEntryType.ValueMember = "Value";
            cmbEntryType.DataSource = EnumHelper.ConvertEnumForBinding(typeof(EntryType));
            Utils.UltraDropDownFilter(cmbEntryType, "DisplayText");

            cmbAcceptData.DisplayMember = "DisplayText";
            cmbAcceptData.ValueMember = "Value";
            cmbAcceptData.DataSource = EnumHelper.ConvertEnumForBinding(typeof(AcceptDataFrom));
            Utils.UltraDropDownFilter(cmbAcceptData, "DisplayText");

            List<EnumerationValue> deviationSignList = CreateDeviationSignList();
            cmbDeviationSign.DisplayMember = "DisplayText";
            cmbDeviationSign.ValueMember = "Value";
            cmbDeviationSign.DataSource = deviationSignList;
            Utils.UltraDropDownFilter(cmbDeviationSign, "DisplayText");
        }

        private static BindingList<AppReconciliedColumn> RetrieveAppReconColumns(int dataSourceId)
        {
            BindingList<AppReconciliedColumn> list = null;
            ///All
            if (dataSourceId == 0)
            {
                list = RetrieveDataSourceIdAll(dataSourceId);
            }
            if (dataSourceId == 1)
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

            if (dataSourceId == 32)
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

            if (dataSourceId == 35)
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

        private static BindingList<AppReconciliedColumn> RetrieveDataSourceIdAll(int dataSourceId)
        {
            BindingList<AppReconciliedColumn> list = new BindingList<AppReconciliedColumn>();
            SortableSearchableList<DataSourceNameID> fullList = DataSourceNameIDList.GetInstance().Retrieve;
            foreach (DataSourceNameID dataSourceNameID in fullList)
            {
                if (!dataSourceNameID.FullName.Equals(Constants.C_COMBO_SELECT) || !dataSourceNameID.FullName.Equals(Constants.C_COMBO_ALL))
                {
                    if (dataSourceId == 1)
                    {
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

                    if (dataSourceId == 32)
                    {
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

                    if (dataSourceId == 35)
                    {
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

                }
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

            deviationSignList.Add(new EnumerationValue(NOTAPPLICABLE, NOTAPPLICABLE));
            deviationSignList.Add(new EnumerationValue(PLUS, PLUS));
            deviationSignList.Add(new EnumerationValue(MINUS, MINUS));
            deviationSignList.Add(new EnumerationValue(PLUS_MINUS, PLUS_MINUS));

            return deviationSignList;
        }

        private void grdAppReconCols_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            UltraGridBand band = e.Layout.Bands[0];
            e.Layout.Override.WrapHeaderText = Infragistics.Win.DefaultableBoolean.True;

            grdAppReconCols.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
            grdAppReconCols.DisplayLayout.ScrollBounds = ScrollBounds.ScrollToLastItem;
            grdAppReconCols.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;
            grdAppReconCols.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            grdAppReconCols.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            grdAppReconCols.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            grdAppReconCols.DisplayLayout.Override.AllowColSwapping = AllowColSwapping.NotAllowed;
            grdAppReconCols.DisplayLayout.Override.HeaderAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grdAppReconCols.DisplayLayout.Bands[0].Override.AllowColSwapping = AllowColSwapping.NotAllowed;

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
            colAcceptData.Header.VisiblePosition = 7;
            colAcceptData.Width = 100;

            //UltraGridColumn colIsSourceDataAccepted = band.Columns[COL_IsSourceDataAccepted];
            //colIsSourceDataAccepted.Header.Caption = "Accept Source Data";
            //colIsSourceDataAccepted.Header.VisiblePosition = 7;

            //UltraGridColumn colIsApplicationDataAccepted = band.Columns[COL_IsApplicationDataAccepted];
            //colIsApplicationDataAccepted.Header.Caption = "Accept Application Data";
            //colIsApplicationDataAccepted.Header.VisiblePosition = 8;

            //grdAppReconCols.CreationFilter = headerSourceDataCheckBox;

        }

        

        void ctrlSourceName1_SelectionChanged(object sender, EventArgs e)
        {

            DataSourceNameID changedDataSourceNameID = ((DataSourceEventArgs)e).DataSourceNameID;
            if (_formBindingSource.List.Count > 0)
            {
                _dataSourceReconColumnsInfo = _formBindingSource.List[0] as DataSourceReconColumnsInfo;
            }

            _dataSourceReconColumnsInfo.DataSourceNameIDValue = changedDataSourceNameID;
            _dataSourceReconColumnsInfo.AppReconciliedColumnList = RetrieveAppReconColumns(changedDataSourceNameID.ID);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (CancelClicked != null)
            {
                CancelClicked(this, e);
            }            
        }

    }
}
