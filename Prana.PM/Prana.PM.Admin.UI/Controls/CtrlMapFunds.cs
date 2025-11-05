using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Prana.PM.BLL;
using Prana.PM.DAL;
//using Prana.PM.Common;
using Prana.BusinessObjects;
using Prana.Global;
using Prana.Utilities.UIUtilities;
using Prana.BusinessObjects.PositionManagement;
using Infragistics.Win.UltraWinGrid;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation;


namespace Prana.PM.Admin.UI.Controls
{
    public partial class CtrlMapFunds : UserControl
    {
        #region Grid Column Names
        
        const string COL_SourceItemName = "SourceItemName";
        const string COL_SourceItemID = "SourceItemID";
        const string COL_SourceItemFullName = "SourceItemFullName";
        const string COL_ApplicationItemName = "ApplicationItemName";
        const string COL_ApplicationItemFullName = "ApplicationItemFullName";
        const string COL_ApplicationItemId = "ApplicationItemId"; 
        const string COL_Lock = "Lock";
        
        #endregion Grid Column Names

        BindingSource _formBindingSource = new BindingSource();

        private MapFunds _mapFunds = new MapFunds();
        MappingItemList _fundsMappingList = new MappingItemList();

     //   private CompanyNameID _companyNameID = new CompanyNameID();

        private FundList _companyFundsList = new FundList();
        private FundList _dataSourcesFundsList = new FundList();
	
        public CtrlMapFunds()
        {
            InitializeComponent();
            _formBindingSource.DataSource = typeof(MapFunds);
            bindingSource1.DataSource = typeof(MappingItemList);
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
        /// <param name="companyNameID">The company name ID.</param>
        public void InitControl(CompanyNameID companyNameID)
        {
            if (!_isInitialized)
            {
                ctrlSourceName1.IsSelectItemRequired = true;
                ctrlSourceName1.IsAllDataSourceAvailable = false;
                ctrlSourceName1.InitControl();
                _mapFunds.CompanyNameID = companyNameID;
                SetupBinding(companyNameID);
                _isInitialized = true;
            }
        }

        #endregion

        /// <summary>
        /// Setups the binding.
        /// </summary>
        /// <param name="companyNameID">The company name ID.</param>
        private void SetupBinding(CompanyNameID companyNameID)
        {
            int companyID = companyNameID.ID;
            _formBindingSource.DataSource = _mapFunds;

                
            //_mapFunds.MappingItems
            BindGridComboBoxes();
            
            //RetrieveMapFunds(companyNameID); // newInfo;
            
            

            //lblDataSourceName.DataBindings.Add(_formBindingSource,

            //create a binding object
           // Binding sourceNameBinding = new System.Windows.Forms.Binding("Text", _formBindingSource, "companyNameID.FullName");
            //add new binding
           // lblDataSourceName.DataBindings.Add(sourceNameBinding);

            //Bind DataSource Combo Value.      
            if (!int.Equals(_mapFunds.DataSourceNameID.ID,0) )
            {
                Binding ctrlSourceNameBinding = new System.Windows.Forms.Binding("Value", _mapFunds, "DataSourceNameID.ID", true);
                ctrlSourceName1.AddDataBindingForCombo(ctrlSourceNameBinding);
            }

            grdMapFunds.DataMember = "MappingItems";
            grdMapFunds.DataSource = _formBindingSource;

            //errorProvider1.Clear();
            //errorProvider1.DataSource = _formBindingSource;
            //errorProvider1.DataMember = "MappingItems";

            //bindingSource1.DataSource = _formBindingSource;
            //dataGridView1.DataMember = "MappingItems";
            //dataGridView1.DataSource = bindingSource1;

            //_mapFunds.BeginEdit();
        }

        /// <summary>
        /// Binds the grid combo boxes.
        /// </summary>
        private void BindGridComboBoxes()
        {
            _dataSourcesFundsList = CompanyManager.GetDataSourceCompanyFunds(_mapFunds.CompanyNameID.ID, _mapFunds.DataSourceNameID.ID);
            cmbSourceColumn.DisplayMember = "FullName";
            cmbSourceColumn.ValueMember = "ID";
            cmbSourceColumn.DataSource = null;
            cmbSourceColumn.DataSource = _dataSourcesFundsList;
            
            Utils.UltraDropDownFilter(cmbSourceColumn, "FullName");

            cmbApplicationColumn.DisplayMember = "FullName";
            cmbApplicationColumn.ValueMember = "ID";
            //cmbApplicationColumn.DataSource = typeof(FundList);
            Utils.UltraDropDownFilter(cmbApplicationColumn, "FullName");

            if (!int.Equals(_mapFunds.DataSourceNameID.ID, 0))
            {
                _companyFundsList = CompanyManager.GetApplicationFundsForCompany(_mapFunds.CompanyNameID.ID, _mapFunds.DataSourceNameID.ID);                
            }           
        }


        /// <summary>
        /// Retrieves the map funds.
        /// </summary>
        /// <param name="companyID">The company ID.</param>
        /// <param name="dataSourceID">The data source ID.</param>
        /// <returns></returns>
        private MappingItemList RetrieveMapFunds(int companyID, int dataSourceID)
        {
            //_mapFunds.DataSourceNameID = dataSourceNameID;
          //  SortableSearchableList<MappingItem> mappingItemList = new SortableSearchableList<MappingItem>();
            MappingItemList mappingItems ;
            mappingItems = CompanyManager.GetApplicationFundsDataForCompanyWithDataSource(companyID, dataSourceID);
            //MappingItemList.RetrieveFundMappings(companyID, dataSourceID);

            if (mappingItems == null)
            {
                mappingItems = new MappingItemList();
            }
            //if (mappingItems.Count > 0)
            //{
            //    _mapFunds.BeginEdit();
            //}

            return mappingItems;
        }




        private int dataSourceID = 0;
        /// <summary>
        /// Handles the SelectionChanged event of the ctrlSourceName1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void ctrlSourceName1_SelectionChanged(object sender, EventArgs e)
        {

            DataSourceNameID changedDataSourceNameID = ((DataSourceEventArgs)e).DataSourceNameID;
            dataSourceID = changedDataSourceNameID.ID;
            //if (_formBindingSource.List.Count > 0)
            //{
            //    _dataSourceReconColumnsInfo = _formBindingSource.List[0] as DataSourceReconColumnsInfo;
            //}
            _mapFunds.DataSourceNameID = changedDataSourceNameID;

            if (!int.Equals(changedDataSourceNameID.ID, -1))
            {
                _companyFundsList = CompanyManager.GetApplicationFundsForCompany(_mapFunds.CompanyNameID.ID, changedDataSourceNameID.ID);
                cmbApplicationColumn.DataSource = null;
                cmbApplicationColumn.DataSource = _companyFundsList;
                _mapFunds.DataSourceNameID = changedDataSourceNameID;
                Utils.UltraDropDownFilter(cmbApplicationColumn, "FullName");
                _fundsMappingList = RetrieveMapFunds(_mapFunds.CompanyNameID.ID, changedDataSourceNameID.ID);

                if (_fundsMappingList.Count > 0)
                {
                    //_mapFunds.BeginEdit();
                }
                _mapFunds.MappingItems = _fundsMappingList;
                _mapFunds.BeginEdit();
            }
            else
            {
                _fundsMappingList.Clear(); 
            }
        }

        /// <summary>
        /// Handles the InitializeLayout event of the grdMapColumns control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs"/> instance containing the event data.</param>
        private void grdMapColumns_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            UltraGridBand band = e.Layout.Bands[0];
            e.Layout.Override.WrapHeaderText = Infragistics.Win.DefaultableBoolean.True;

            grdMapFunds.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
            grdMapFunds.DisplayLayout.ScrollBounds = ScrollBounds.ScrollToLastItem;
            grdMapFunds.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            grdMapFunds.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            grdMapFunds.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            grdMapFunds.DisplayLayout.Override.AllowColSwapping = AllowColSwapping.NotAllowed;
            grdMapFunds.DisplayLayout.Override.HeaderAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grdMapFunds.DisplayLayout.Bands[0].Override.AllowColSwapping = AllowColSwapping.NotAllowed;

            ///TODO : Remove this hardcoding of column names, We might have some attribute to be put on the name 
            ///of the property.

            UltraGridColumn colSourceItemID = band.Columns[COL_SourceItemID];
            colSourceItemID.Hidden = true;

            UltraGridColumn colSourceItemFullName = band.Columns[COL_SourceItemFullName];
            colSourceItemFullName.Hidden = true;

            UltraGridColumn colApplicationItemFullName = band.Columns[COL_ApplicationItemFullName];
            colApplicationItemFullName.Hidden = true;

            UltraGridColumn colSourceItemName = band.Columns[COL_SourceItemName];
            colSourceItemName.Header.Caption = "Source Funds";
           // colSourceItemName.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;
           // colSourceItemName.ButtonDisplayStyle = ButtonDisplayStyle.Always;
           // colSourceItemName.ValueList = cmbSourceColumn;
            grdMapFunds.DisplayLayout.Bands[0].Columns[COL_SourceItemName].Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
            colSourceItemFullName.SortIndicator = SortIndicator.Disabled;
            //grdMapFunds.DisplayLayout.Bands[0].Columns[COL_SourceItemName].CellActivation = Activation.NoEdit;
            colSourceItemName.Header.VisiblePosition = 1;

            UltraGridColumn colApplicationItemId = band.Columns[COL_ApplicationItemId];
            colApplicationItemId.Header.Caption = "Application Funds";
            colApplicationItemId.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
            colApplicationItemId.ButtonDisplayStyle = ButtonDisplayStyle.Always;
            colApplicationItemId.ValueList = cmbApplicationColumn;
            colApplicationItemId.SortIndicator = SortIndicator.Disabled;
            colApplicationItemId.Header.VisiblePosition = 2;

            UltraGridColumn colApplicationItemName = band.Columns[COL_ApplicationItemName];
            colApplicationItemName.Hidden = true;

          //  UltraGridColumn colSourceItemID = band.Columns[COL_SourceItemID];
          //  colSourceItemID.Hidden = true;

            UltraGridColumn colLock = band.Columns[COL_Lock];
            colLock.Hidden = true;
        }

        /// <summary>
        /// Handles the Click event of the btnSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                bool isValid = true;
                if (_mapFunds.MappingItems != null && _mapFunds.MappingItems.Count > 0)
                {
                    foreach (MappingItem mappingItem in _mapFunds.MappingItems)
                    {
                        if (mappingItem.IsValid == false)
                        {
                            isValid = false;
                            break;
                        }
                    }
                    if (isValid == true)
                    {
                        _mapFunds.ApplyEdit();
                        int result = CompanyManager.SaveFundMappings(_mapFunds.MappingItems, _mapFunds.CompanyNameID.ID, _mapFunds.DataSourceNameID.ID);


                        _fundsMappingList = RetrieveMapFunds(_mapFunds.CompanyNameID.ID, dataSourceID);
                        if (_fundsMappingList.Count > 0)
                        {
                            //_mapFunds.BeginEdit();
                        }
                        _mapFunds.MappingItems = _fundsMappingList;
                        _mapFunds.BeginEdit();
                        MessageBox.Show(this, "Funds mapping saved !", "Position Management", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show(this, "Please map some funds before saving !", "Position Management", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            
        }

        /// <summary>
        /// Handles the Click event of the btnAdd control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            MappingItem item = new MappingItem(string.Empty, string.Empty, string.Empty);
            item.ApplicationItemId = 0;
            item.ApplicationItemName = ApplicationConstants.C_COMBO_SELECT;
            item.SourceItemName = string.Empty;
            try
            {
                //Ask the database only for the first time.
                if (_fundsMappingList == null)
                {
                    _fundsMappingList = new MappingItemList();
                }
                _fundsMappingList.Add(item);
                _formBindingSource.ResetBindings(false);
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            //_mapFunds.CancelEdit();
        }

        private void grdMapFunds_Click(object sender, EventArgs e)
        {
            //_mapFunds.CancelEdit();
        }
    }
}
