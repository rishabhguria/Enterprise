using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using Nirvana.Admin.PositionManagement.BusinessObjects;
using Nirvana.Admin.PositionManagement.Classes;

using Infragistics.Win.UltraWinGrid;


namespace Nirvana.Admin.PositionManagement.Controls
{
    public partial class CtrlMapAUEC : UserControl
    {
        #region Grid Column Names

        const string COL_SourceItemName = "SourceItemName";
        const string COL_SourceItemFullName = "SourceItemFullName";
        const string COL_ApplicationItemName = "ApplicationItemName";
        const string COL_ApplicationItemFullName = "ApplicationItemFullName";
        const string COL_Lock = "Lock";

        #endregion Grid Column Names

        BindingSource _formBindingSource = new BindingSource();
        private MapColumns _mapAUEC = new MapColumns();

        public CtrlMapAUEC()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            // Close the Container Form
            this.FindForm().Close();
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
        public void InitControl(DataSourceNameID dataSourceNameID)
        {
            if (!_isInitialized)
            {

                SetupBinding(dataSourceNameID);
                //_isInitialized = true;
            }
            else
            {
                SetupBinding(dataSourceNameID);
            }

            ReStructureGrid();
            //Reset datasource bindings
            _formBindingSource.ResetBindings(false);   
        }

        #endregion

        private void SetupBinding(DataSourceNameID dataSourceNameID)
        {
            //grdAUECMapping.DataBindings.Clear();
            _formBindingSource.DataSource = RetrieveMapAUEC(dataSourceNameID); // newInfo;

            BindGridComboBoxes();

            //lblDataSourceName.DataBindings.Add(_formBindingSource,

            //create a binding object
            Binding sourceNameBinding = new System.Windows.Forms.Binding("Text", _formBindingSource, "DataSourceNameID");
            //add new binding
            lblDataSourceName.DataBindings.Clear();
            lblDataSourceName.DataBindings.Add(sourceNameBinding);

            grdAUECMapping.DataMember = "MappingItemList";
            grdAUECMapping.DataSource = _formBindingSource;

            //_formBindingSource.ResetBindings(false);
           // grdAUECMapping.Refresh();

        }

        private void BindGridComboBoxes()
        {
            cmbSourceColumn.DataBindings.Clear();
            cmbSourceColumn.DisplayMember = "DisplayText";
            cmbSourceColumn.ValueMember = "Value";
            cmbSourceColumn.DataSource = CreateSourceColumns();
            Utils.UltraDropDownFilter(cmbSourceColumn, "DisplayText");
            //cmbSourceColumn.Refresh();

            cmbApplicationColumn.DataBindings.Clear();
            cmbApplicationColumn.DisplayMember = "DisplayText";
            cmbApplicationColumn.ValueMember = "Value";
            cmbApplicationColumn.DataSource = CreateApplicationColumns();
            Utils.UltraDropDownFilter(cmbApplicationColumn, "DisplayText");
            //cmbApplicationColumn.Refresh();
        }

        private MapColumns RetrieveMapAUEC(DataSourceNameID dataSourceNameID)
        {
            _mapAUEC.DataSourceNameID = dataSourceNameID;
            string selectedTabKey = tabMapAUEC.SelectedTab != null?tabMapAUEC.SelectedTab.Key : "Asset";

            switch (selectedTabKey)
            {
                case "Asset":
                    _mapAUEC.MappingItemList = MappingItemList.RetrieveAssetMappings(dataSourceNameID);
                    tabMapAUEC.Tabs["Asset"].Selected = true;
                    break;
                case "Underlying":
                    _mapAUEC.MappingItemList = MappingItemList.RetrieveUnderlyingMappings(dataSourceNameID);
                    tabMapAUEC.Tabs["Underlying"].Selected = true;
                    break;
                case "Exchange":
                    _mapAUEC.MappingItemList = MappingItemList.RetrieveExchangeMappings(dataSourceNameID);
                    tabMapAUEC.Tabs["Exchange"].Selected = true;
                    break;
                case "Currency":
                    _mapAUEC.MappingItemList = MappingItemList.RetrieveCurrencyMappings(dataSourceNameID);
                    tabMapAUEC.Tabs["Currency"].Selected = true;
                    break;
            }
            

            return _mapAUEC;
        }


        private List<EnumerationValue> CreateSourceColumns()
        {
            List<EnumerationValue> sourceColumns = new List<EnumerationValue>();

            string selectedTabKey = tabMapAUEC.SelectedTab != null ? tabMapAUEC.SelectedTab.Key : "Asset";

            switch (selectedTabKey)
            {
                case "Asset":
                    sourceColumns.Add(new EnumerationValue("Equ", 0));
                    sourceColumns.Add(new EnumerationValue("Fut", 1));
                    sourceColumns.Add(new EnumerationValue("Opt", 2));
                    break;
                case "Underlying":
                    sourceColumns.Add(new EnumerationValue("US Equitiy Options", 0));
                    sourceColumns.Add(new EnumerationValue("US Fut", 1));
                    sourceColumns.Add(new EnumerationValue("UK Equitiy Opt", 2));
                    break;
                case "Exchange":
                    sourceColumns.Add(new EnumerationValue("tse", 0));
                    sourceColumns.Add(new EnumerationValue("lse", 1));
                    sourceColumns.Add(new EnumerationValue("nyse", 2));
                    break;
                case "Currency":
                    sourceColumns.Add(new EnumerationValue("Us", 0));
                    sourceColumns.Add(new EnumerationValue("Yn", 1));
                    sourceColumns.Add(new EnumerationValue("Gb", 2));
                    break;
            }
            return sourceColumns;
        }

        private List<EnumerationValue> CreateApplicationColumns()
        {
            List<EnumerationValue> applicationColumns = new List<EnumerationValue>();
            string selectedTabKey = tabMapAUEC.SelectedTab != null?tabMapAUEC.SelectedTab.Key : "Asset";

            switch (selectedTabKey)
            {
                case "Asset":
                    applicationColumns.Add(new EnumerationValue("Equities", 0));
                    applicationColumns.Add(new EnumerationValue("Futures", 1));
                    applicationColumns.Add(new EnumerationValue("Options", 2));
                    break;
                case "Underlying":
                    applicationColumns.Add(new EnumerationValue("US Options", 0));
                    applicationColumns.Add(new EnumerationValue("US Futures", 1));
                    applicationColumns.Add(new EnumerationValue("UK Options", 2));
                    break;
                case "Exchange":
                    applicationColumns.Add(new EnumerationValue("TSE", 0));
                    applicationColumns.Add(new EnumerationValue("LSE", 1));
                    applicationColumns.Add(new EnumerationValue("NYSE", 2));
                    break;
                case "Currency":
                    applicationColumns.Add(new EnumerationValue("USD", 0));
                    applicationColumns.Add(new EnumerationValue("Yen", 1));
                    applicationColumns.Add(new EnumerationValue("GBP", 2));
                    break;
            }
            

            return applicationColumns;
        }

        private void grdAUECMapping_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
          ReStructureGrid();
        }

        private void ReStructureGrid()
        {
            UltraGridBand band = grdAUECMapping.DisplayLayout.Bands[0];
            grdAUECMapping.DisplayLayout.Override.WrapHeaderText = Infragistics.Win.DefaultableBoolean.True;
            grdAUECMapping.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
            grdAUECMapping.DisplayLayout.ScrollBounds = ScrollBounds.ScrollToLastItem;
            grdAUECMapping.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;
            grdAUECMapping.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            grdAUECMapping.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            grdAUECMapping.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            grdAUECMapping.DisplayLayout.Override.AllowColSwapping = AllowColSwapping.NotAllowed;
            grdAUECMapping.DisplayLayout.Override.HeaderAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grdAUECMapping.DisplayLayout.Bands[0].Override.AllowColSwapping = AllowColSwapping.NotAllowed;


            UltraGridColumn colSourceItemFullName = band.Columns[COL_SourceItemFullName];
            colSourceItemFullName.Hidden = true;

            UltraGridColumn colApplicationItemFullName = band.Columns[COL_ApplicationItemFullName];
            colApplicationItemFullName.Hidden = true;

            UltraGridColumn colLock = band.Columns[COL_Lock];
            colLock.Hidden = true;

            UltraGridColumn colSourceItemName = band.Columns[COL_SourceItemName];
            colSourceItemName.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;
            colSourceItemName.ButtonDisplayStyle = ButtonDisplayStyle.Always;
            colSourceItemName.ValueList = cmbSourceColumn;
            colSourceItemName.Header.VisiblePosition = 1;

            UltraGridColumn colApplicationItemName = band.Columns[COL_ApplicationItemName];
            colApplicationItemName.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;
            colApplicationItemName.ButtonDisplayStyle = ButtonDisplayStyle.Always;
            colApplicationItemName.ValueList = cmbApplicationColumn;
            colApplicationItemName.Header.VisiblePosition = 2;

            ///TODO : Remove this hardcoding of column names, We might have some attribute to be put on the name 
            ///of the property.
            ///
            string selectedTabKey = tabMapAUEC.SelectedTab != null ? tabMapAUEC.SelectedTab.Key : "Asset";

            switch (selectedTabKey)
            {
                case "Asset":
                    colSourceItemName.Header.Caption = "Source Asset Class";
                    colApplicationItemName.Header.Caption = "Application Asset Class";
                    break;
                case "Underlying":
                    colSourceItemName.Header.Caption = "Source Underlying";
                    colApplicationItemName.Header.Caption = "Application Underlying";
                    break;
                case "Exchange":
                    colSourceItemName.Header.Caption = "Source Exchange";
                    colSourceItemName.Header.VisiblePosition = 3;

                    colApplicationItemName.Header.Caption = "Application Exchange";

                    colApplicationItemFullName.Header.Caption = "Exchange Full Name";
                    colApplicationItemFullName.Hidden = false;
                    colApplicationItemFullName.Header.VisiblePosition = 2;
                    break;
                case "Currency":
                    colSourceItemName.Header.Caption = "Source Currency";
                    colSourceItemName.Header.VisiblePosition = 3;

                    colApplicationItemName.Header.Caption = "Application Currency";

                    colApplicationItemFullName.Header.Caption = "Currency Full Name";
                    colApplicationItemFullName.Hidden = false;
                    colApplicationItemFullName.Header.VisiblePosition = 2;
                    break;
            }
        }

        private void tabMapAUEC_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {

            InitControl(_mapAUEC.DataSourceNameID);
        }

    }
}
