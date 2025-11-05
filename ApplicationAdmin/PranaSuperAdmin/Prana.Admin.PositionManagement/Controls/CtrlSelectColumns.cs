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
    public partial class CtrlSelectColumns : UserControl
    {
        #region Grid Column Names

        const string COL_SourceColumnName = "SourceColumnName";
        const string COL_Description = "Description";
        const string COL_Type = "Type";
        const string COL_SampleValue = "SampleValue";
        const string COL_Notes = "Notes";

        #endregion Grid Column Names

        BindingSource _formBindingSource = new BindingSource();
        private SelectColumns _setupColumns = new SelectColumns();

        public CtrlSelectColumns()
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
                _isInitialized = true;
            }
        }

        #endregion

        private void SetupBinding(DataSourceNameID dataSourceNameID)
        {
            _formBindingSource.DataSource = RetrieveSetupColumns(dataSourceNameID); // newInfo;

            BindGridComboBoxes();

            //create a binding object
            Binding sourceNameBinding = new System.Windows.Forms.Binding("Text", _formBindingSource, "DataSourceNameID");
            //add new binding
            lblDataSourceName.DataBindings.Add(sourceNameBinding);

            grdColumns.DataMember = "SelectColumnItems";
            grdColumns.DataSource = _formBindingSource;

        }

        private void BindGridComboBoxes()
        {
            cmbSourceColumnType.DisplayMember = "DisplayText";
            cmbSourceColumnType.ValueMember = "Value";
            cmbSourceColumnType.DataSource = EnumHelper.ConvertEnumForBinding(typeof(SelectColumnsType));
            Utils.UltraDropDownFilter(cmbSourceColumnType, "DisplayText");
        }

        private SelectColumns RetrieveSetupColumns(DataSourceNameID dataSourceNameID)
        {
            _setupColumns.DataSourceNameID = dataSourceNameID;
            _setupColumns.SelectColumnItems = SelectColumnsItemList.Retrieve(dataSourceNameID);

            return _setupColumns;
        }


        private void grdColumns_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            UltraGridBand band = e.Layout.Bands[0];
            e.Layout.Override.WrapHeaderText = Infragistics.Win.DefaultableBoolean.True;

            grdColumns.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
            grdColumns.DisplayLayout.ScrollBounds = ScrollBounds.ScrollToLastItem;
            grdColumns.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;
            grdColumns.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            grdColumns.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            grdColumns.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            grdColumns.DisplayLayout.Override.AllowColSwapping = AllowColSwapping.NotAllowed;
            grdColumns.DisplayLayout.Override.HeaderAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grdColumns.DisplayLayout.Bands[0].Override.AllowColSwapping = AllowColSwapping.NotAllowed;

            ///TODO : Remove this hardcoding of column names, We might have some attribute to be put on the name 
            ///of the property.

            UltraGridColumn colSourceColumnName = band.Columns[COL_SourceColumnName];
            colSourceColumnName.Header.Caption = "Source Column Name";
            colSourceColumnName.Header.VisiblePosition = 1;

            UltraGridColumn colDescription = band.Columns[COL_Description];
            colDescription.Header.Caption = "Description";
            colDescription.Header.VisiblePosition = 2;

            UltraGridColumn colType = band.Columns[COL_Type];
            colType.Header.Caption = "Type";
            colType.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;
            colType.ButtonDisplayStyle = ButtonDisplayStyle.Always;
            colType.ValueList = cmbSourceColumnType;
            colType.Header.VisiblePosition = 3;

            UltraGridColumn colSampleValue = band.Columns[COL_SampleValue];
            colSampleValue.Header.Caption = "Sample Value";
            colSampleValue.Header.VisiblePosition = 4;

            UltraGridColumn colNotes = band.Columns[COL_Notes];
            colNotes.Header.Caption = "Notes";
            colNotes.Header.VisiblePosition = 5;
        }
    }
}
