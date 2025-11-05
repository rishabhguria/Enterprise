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
    public partial class CtrlMapFunds : UserControl
    {
        #region Grid Column Names
        
        const string COL_SourceItemName = "SourceItemName";
        const string COL_SourceItemFullName = "SourceItemFullName";
        const string COL_ApplicationItemName = "ApplicationItemName";
        const string COL_ApplicationItemFullName = "ApplicationItemFullName";
        const string COL_Lock = "Lock";
        
        #endregion Grid Column Names

        BindingSource _formBindingSource = new BindingSource();
        private MapFunds _mapFunds = new MapFunds();
	
        public CtrlMapFunds()
        {
            InitializeComponent();
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
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
                _isInitialized = true;
            }
        }

        #endregion

        private void SetupBinding(DataSourceNameID dataSourceNameID)
        {
            _formBindingSource.DataSource = RetrieveMapFunds(dataSourceNameID); // newInfo;

            BindGridComboBoxes();

            //lblDataSourceName.DataBindings.Add(_formBindingSource,

            //create a binding object
            Binding sourceNameBinding = new System.Windows.Forms.Binding("Text", _formBindingSource, "DataSourceNameID");
            //add new binding
            lblDataSourceName.DataBindings.Add(sourceNameBinding);

            grdMapFunds.DataMember = "MappingItemList";
            grdMapFunds.DataSource = _formBindingSource;

        }

        private void BindGridComboBoxes()
        {
            cmbSourceColumn.DisplayMember = "DisplayText";
            cmbSourceColumn.ValueMember = "Value";
            cmbSourceColumn.DataSource = CreateSourceColumns();
            Utils.UltraDropDownFilter(cmbSourceColumn, "DisplayText");

            cmbApplicationColumn.DisplayMember = "DisplayText";
            cmbApplicationColumn.ValueMember = "Value";
            cmbApplicationColumn.DataSource = CreateApplicationColumns();
            Utils.UltraDropDownFilter(cmbApplicationColumn, "DisplayText");
        }

        private MapFunds RetrieveMapFunds(DataSourceNameID dataSourceNameID)
        {
            _mapFunds.DataSourceNameID = dataSourceNameID;
            _mapFunds.MappingItemList = MappingItemList.RetrieveFundMappings(dataSourceNameID);

            return _mapFunds;
        }


        private List<EnumerationValue> CreateSourceColumns()
        {
            List<EnumerationValue> sourceColumns = new List<EnumerationValue>();

            sourceColumns.Add(new EnumerationValue("Fund 1", 0));
            sourceColumns.Add(new EnumerationValue("Fund 2", 1));
            sourceColumns.Add(new EnumerationValue("Fund 3", 2));

            return sourceColumns;
        }

        private List<EnumerationValue> CreateApplicationColumns()
        {
            List<EnumerationValue> applicationColumns = new List<EnumerationValue>();

            applicationColumns.Add(new EnumerationValue("Fund X", 0));
            applicationColumns.Add(new EnumerationValue("Fund Y", 1));
            applicationColumns.Add(new EnumerationValue("Fund Z", 2));

            return applicationColumns;
        }

        private void grdMapColumns_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            UltraGridBand band = e.Layout.Bands[0];
            e.Layout.Override.WrapHeaderText = Infragistics.Win.DefaultableBoolean.True;

            grdMapFunds.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
            grdMapFunds.DisplayLayout.ScrollBounds = ScrollBounds.ScrollToLastItem;
            grdMapFunds.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;
            grdMapFunds.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            grdMapFunds.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            grdMapFunds.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            grdMapFunds.DisplayLayout.Override.AllowColSwapping = AllowColSwapping.NotAllowed;
            grdMapFunds.DisplayLayout.Override.HeaderAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grdMapFunds.DisplayLayout.Bands[0].Override.AllowColSwapping = AllowColSwapping.NotAllowed;

            ///TODO : Remove this hardcoding of column names, We might have some attribute to be put on the name 
            ///of the property.

            UltraGridColumn colSourceItemFullName = band.Columns[COL_SourceItemFullName];
            colSourceItemFullName.Hidden = true;

            UltraGridColumn colApplicationItemFullName = band.Columns[COL_ApplicationItemFullName];
            colApplicationItemFullName.Hidden = true;

            UltraGridColumn colSourceItemName = band.Columns[COL_SourceItemName];
            colSourceItemName.Header.Caption = "Source Funds";
            colSourceItemName.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;
            colSourceItemName.ButtonDisplayStyle = ButtonDisplayStyle.Always;
            colSourceItemName.ValueList = cmbSourceColumn;
            colSourceItemName.Header.VisiblePosition = 1;

            UltraGridColumn colApplicationItemName = band.Columns[COL_ApplicationItemName];
            colApplicationItemName.Header.Caption = "Applicatin Funds";
            colApplicationItemName.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;
            colApplicationItemName.ButtonDisplayStyle = ButtonDisplayStyle.Always;
            colApplicationItemName.ValueList = cmbApplicationColumn;
            colApplicationItemName.Header.VisiblePosition = 2;

            UltraGridColumn colLock = band.Columns[COL_Lock];
            colLock.Hidden = true;
        }
    }
}
