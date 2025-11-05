using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using Infragistics.Win.UltraWinGrid;

using Nirvana.Admin.PositionManagement.Classes;
using Nirvana.Admin.PositionManagement.Forms;
using Nirvana.Admin.PositionManagement.BusinessObjects;

namespace Nirvana.Admin.PositionManagement.Controls
{
    public partial class CtrlCashRecon : UserControl
    {
        #region Grid Column Names

        const string COL_IsSelected = "IsSelected";
        const string COL_DataSourceNameID = "DataSourceNameID";
        const string COL_Currency = "Currency";
        const string COL_Fund = "Fund";
        const string COL_EstimatedClosingBalance = "EstimatedClosingBalance";
        const string COL_BalanceBF = "BalanceBF";
        const string COL_Difference = "Difference";
        const string COL_ManualEntry = "ManualEntry";
        const string COL_Status = "Status";
        const string COL_DetailsButton = "DetailsButton";
        const string COL_ManualEntryButton = "ManualEntryButton";

        #endregion Grid Column Names

        BindingSource _formBindingSource = new BindingSource();
        CashRecon _cashRecon = new CashRecon();
        CheckBoxOnHeader_CreationFilter headerCheckBox = new CheckBoxOnHeader_CreationFilter();

        public CtrlCashRecon()
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
                ctrlSourceName1.IsSelectItemRequired = false;
                ctrlSourceName1.IsAllDataSourceAvailable = true;
                ctrlSourceName1.InitControl();

                SetupBinding();

                _isInitialized = true;
            }
        }

        private void SetupBinding()
        {
            _formBindingSource.DataSource = _cashRecon;

            cmbDate.DataBindings.Add("Value", _formBindingSource, "Date");

            BindGridComboBox();
            //TODO : Need to do it
            //ctrlSourceName1.Source = _formBindingSource;

            grdCashRecon.DataMember = "CashReconItemList";
            grdCashRecon.DataSource = _formBindingSource;
        }

        #endregion

        private void BindGridComboBox()
        {
            cmbReconStatus.DisplayMember = "DisplayText";
            cmbReconStatus.ValueMember = "Value";
            cmbReconStatus.DataSource = EnumHelper.ConvertEnumForBinding(typeof(ReconStatus));
            Utils.UltraDropDownFilter(cmbReconStatus, "DisplayText");
        }

        private void ctrlSourceName1_SelectionChanged(object sender, EventArgs e)
        {
            _cashRecon.DataSourceNameID = ((DataSourceEventArgs)e).DataSourceNameID;
        }

        private void btnRunReconciliation_Click(object sender, EventArgs e)
        {
            OnCashRecon();
        }

        /// <summary>
        /// Called when cash recon button is clicked.
        /// </summary>
        private void OnCashRecon()
        {
            if (!_cashRecon.DataSourceNameID.FullName.Equals(Constants.C_COMBO_SELECT))
            {
                _cashRecon.CashReconItemList = RetrieveCashReconItemList(_cashRecon.Date, _cashRecon.DataSourceNameID);
                ///TODO : Instead of reset bindings, Implement onproperty change in business objects through "BusinessObjectBase" class
                _formBindingSource.ResetBindings(false);
            }
        }

        private void cmbDate_ValueChanged(object sender, EventArgs e)
        {
            //if (_formBindingSource.List.Count > 0)
            //{
            //    _dataSourceReconSummaryInfo = _formBindingSource.List[0] as DataSourceReconSummaryInfo;
            //}

            _cashRecon.Date = Convert.ToDateTime(cmbDate.Value);

            TimeSpan ts = new TimeSpan(1, 0, 0, 0, 0);
            DateTime previousdate = DateTime.Today.Subtract(ts);

            if (_cashRecon.Date.Equals(previousdate))
            {
                if (!_cashRecon.DataSourceNameID.FullName.Equals(Constants.C_COMBO_SELECT))
                {
                    _cashRecon.CashReconItemList = RetrieveCashReconItemList(previousdate, _cashRecon.DataSourceNameID);
                    ///TODO : Instead of reset bindings, Implement onproperty change in business objects through "BusinessObjectBase" class
                    _formBindingSource.ResetBindings(false);
                }
            }
        }

        #region Retrieve Data
        private CashReconItemList RetrieveCashReconItemList(DateTime cashReconDate, DataSourceNameID dataSourceNameID)
        {
            TimeSpan ts = new TimeSpan(1, 0, 0, 0, 0);
            DateTime previousdate = DateTime.Today.Subtract(ts);
            CashReconItemList cashReconItemList = new CashReconItemList();

            if (cashReconDate.Equals(previousdate))
            {
                if (dataSourceNameID.ID.Equals(1))
                {
                    CashReconItem _cashReconItem = new CashReconItem();
                    _cashReconItem.DataSourceNameID = dataSourceNameID;
                    _cashReconItem.Currency.Name = "USD";
                    _cashReconItem.Fund.Name = "Long";
                    _cashReconItem.EstimatedClosingBalance = 150000;
                    _cashReconItem.BalanceBF = 145000;
                    _cashReconItem.Difference = _cashReconItem.EstimatedClosingBalance - _cashReconItem.BalanceBF;
                    _cashReconItem.Status = ReconStatus.Open;
                    cashReconItemList.Add(_cashReconItem);
                }
            }

            if (cashReconDate.Equals(DateTime.Today))
            {
                if (dataSourceNameID.ID.Equals(1))
                {
                    CashReconItem _cashReconItem = new CashReconItem();
                    _cashReconItem.DataSourceNameID = dataSourceNameID;
                    _cashReconItem.Currency.Name = "USD";
                    _cashReconItem.Fund.Name = "Long";
                    _cashReconItem.EstimatedClosingBalance = 150000;
                    _cashReconItem.BalanceBF = 145000;
                    _cashReconItem.Difference = _cashReconItem.EstimatedClosingBalance - _cashReconItem.BalanceBF;
                    _cashReconItem.Status = ReconStatus.Open;
                    cashReconItemList.Add(_cashReconItem);

                    _cashReconItem = new CashReconItem();
                    _cashReconItem.DataSourceNameID = dataSourceNameID;
                    _cashReconItem.Currency.Name = "YEN";
                    _cashReconItem.Fund.Name = "Short";
                    _cashReconItem.EstimatedClosingBalance = 100000;
                    _cashReconItem.BalanceBF = 100000;
                    _cashReconItem.Difference = _cashReconItem.EstimatedClosingBalance - _cashReconItem.BalanceBF;
                    _cashReconItem.Status = ReconStatus.Closed;
                    cashReconItemList.Add(_cashReconItem);
                }

                if (dataSourceNameID.ID.Equals(32))
                {
                    CashReconItem _cashReconItem = new CashReconItem();
                    _cashReconItem.DataSourceNameID = dataSourceNameID;
                    _cashReconItem.Currency.Name = "USD";
                    _cashReconItem.Fund.Name = "Short";
                    _cashReconItem.EstimatedClosingBalance = 50000;
                    _cashReconItem.BalanceBF = 49900;
                    _cashReconItem.Difference = _cashReconItem.EstimatedClosingBalance - _cashReconItem.BalanceBF;
                    _cashReconItem.Status = ReconStatus.Open;
                    cashReconItemList.Add(_cashReconItem);
                }

                if (dataSourceNameID.ID == 35)
                {
                    CashReconItem _cashReconItem = new CashReconItem();
                    _cashReconItem.DataSourceNameID = dataSourceNameID;
                    _cashReconItem.Currency.Name = "USD";
                    _cashReconItem.Fund.Name = "Short";
                    _cashReconItem.EstimatedClosingBalance = 50000;
                    _cashReconItem.BalanceBF = 49900;
                    _cashReconItem.Difference = _cashReconItem.EstimatedClosingBalance - _cashReconItem.BalanceBF;
                    _cashReconItem.Status = ReconStatus.Open;
                    cashReconItemList.Add(_cashReconItem);
                }

                ///All
                if (dataSourceNameID.ID == 0)
                {
                    SortableSearchableList<DataSourceNameID> fullList = DataSourceNameIDList.GetInstance().Retrieve;
                    foreach (DataSourceNameID dataSource in fullList)
                    {
                        if (!dataSource.FullName.Equals(Constants.C_COMBO_SELECT) && !dataSource.FullName.Equals(Constants.C_COMBO_ALL))
                        {
                            if (dataSource.ID == 1)
                            {
                                CashReconItem _cashReconItem = new CashReconItem();
                                _cashReconItem.DataSourceNameID = dataSource;
                                _cashReconItem.Currency.Name = "USD";
                                _cashReconItem.Fund.Name = "Long";
                                _cashReconItem.EstimatedClosingBalance = 150000;
                                _cashReconItem.BalanceBF = 145000;
                                _cashReconItem.Difference = _cashReconItem.EstimatedClosingBalance - _cashReconItem.BalanceBF;
                                _cashReconItem.Status = ReconStatus.Open;
                                cashReconItemList.Add(_cashReconItem);

                                _cashReconItem = new CashReconItem();
                                _cashReconItem.DataSourceNameID = dataSource;
                                _cashReconItem.Currency.Name = "YEN";
                                _cashReconItem.Fund.Name = "Short";
                                _cashReconItem.EstimatedClosingBalance = 100000;
                                _cashReconItem.BalanceBF = 100000;
                                _cashReconItem.Difference = _cashReconItem.EstimatedClosingBalance - _cashReconItem.BalanceBF;
                                _cashReconItem.Status = ReconStatus.Closed;
                                cashReconItemList.Add(_cashReconItem);
                            }
                            if (dataSource.ID == 32)
                            {
                                CashReconItem _cashReconItem = new CashReconItem();
                                _cashReconItem.DataSourceNameID = dataSource;
                                _cashReconItem.Currency.Name = "USD";
                                _cashReconItem.Fund.Name = "Short";
                                _cashReconItem.EstimatedClosingBalance = 50000;
                                _cashReconItem.BalanceBF = 49900;
                                _cashReconItem.Difference = _cashReconItem.EstimatedClosingBalance - _cashReconItem.BalanceBF;
                                _cashReconItem.Status = ReconStatus.Open;
                                cashReconItemList.Add(_cashReconItem);
                            }
                            if (dataSource.ID == 35)
                            {
                                CashReconItem _cashReconItem = new CashReconItem();
                                _cashReconItem.DataSourceNameID = dataSource;
                                _cashReconItem.Currency.Name = "USD";
                                _cashReconItem.Fund.Name = "Short";
                                _cashReconItem.EstimatedClosingBalance = 50000;
                                _cashReconItem.BalanceBF = 49900;
                                _cashReconItem.Difference = _cashReconItem.EstimatedClosingBalance - _cashReconItem.BalanceBF;
                                _cashReconItem.Status = ReconStatus.Open;
                                cashReconItemList.Add(_cashReconItem);
                            }
                        }
                    }
                }

            }

            return cashReconItemList;

        } 
        #endregion

        private void grdCashRecon_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            UltraGridBand band = e.Layout.Bands[0];
            e.Layout.Override.WrapHeaderText = Infragistics.Win.DefaultableBoolean.True;
            grdCashRecon.CreationFilter = headerCheckBox;
            grdCashRecon.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
            grdCashRecon.DisplayLayout.ScrollBounds = ScrollBounds.ScrollToLastItem;
            grdCashRecon.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;
            grdCashRecon.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            grdCashRecon.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            grdCashRecon.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            grdCashRecon.DisplayLayout.Override.AllowColSwapping = AllowColSwapping.NotAllowed;
            grdCashRecon.DisplayLayout.Override.HeaderAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grdCashRecon.DisplayLayout.Bands[0].Override.AllowColSwapping = AllowColSwapping.NotAllowed;

            ///TODO : Remove this hardcoding of column names, We might have some attribute to be put on the name 
            ///of the property.

            UltraGridColumn colIsSelected = band.Columns[COL_IsSelected];
            colIsSelected.Header.Caption = "";
            colIsSelected.Header.VisiblePosition = 1;

            UltraGridColumn colDataSourceNameID = band.Columns[COL_DataSourceNameID];
            colDataSourceNameID.Header.Caption = "Data Source";
            colDataSourceNameID.Header.VisiblePosition = 2;

            UltraGridColumn colCurrency = band.Columns[COL_Currency];
            colCurrency.Header.Caption = "Data Source";
            colCurrency.Header.VisiblePosition = 3;

            UltraGridColumn colFund = band.Columns[COL_Fund];
            colFund.Header.Caption = "Currency";
            colFund.Header.VisiblePosition = 4;

            UltraGridColumn colEstimatedClosingBalance = band.Columns[COL_EstimatedClosingBalance];
            colEstimatedClosingBalance.Header.Caption = "Estimated Closing Balance (Application data)";
            colEstimatedClosingBalance.Header.VisiblePosition = 5;

            UltraGridColumn colBalanceBF = band.Columns[COL_BalanceBF];
            colBalanceBF.Header.Caption = "Balance B/F (Source data)";
            colBalanceBF.Header.VisiblePosition = 6;

            UltraGridColumn colDifference = band.Columns[COL_Difference];
            colDifference.Header.Caption = "Difference";
            colDifference.Header.VisiblePosition = 7;

            UltraGridColumn colManualEntry = band.Columns[COL_ManualEntry];
            colManualEntry.Header.Caption = "Manual Entry";
            colManualEntry.Header.VisiblePosition = 8;

            UltraGridColumn colStatus = band.Columns[COL_Status];
            colStatus.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;
            colStatus.ButtonDisplayStyle = ButtonDisplayStyle.Always;
            colStatus.ValueList = cmbReconStatus;
            colStatus.Header.Caption = "Status";
            colStatus.Header.VisiblePosition = 9;

            UltraGridColumn colDetailsButton = band.Columns.Add(COL_DetailsButton);
            colDetailsButton.Header.Caption = "Detail Screen";
            colDetailsButton.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
            colDetailsButton.ButtonDisplayStyle = ButtonDisplayStyle.Always;
            colDetailsButton.Header.VisiblePosition = 10;

            UltraGridColumn colManualEntryButton = band.Columns.Add(COL_ManualEntryButton);
            colManualEntryButton.Header.Caption = "Manual Entry Screen";
            colManualEntryButton.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
            colManualEntryButton.ButtonDisplayStyle = ButtonDisplayStyle.Always;
            colManualEntryButton.Header.VisiblePosition = 11;
        }

        /// <summary>
        /// Handles the ClickCellButton event of the grdCashRecon control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Infragistics.Win.UltraWinGrid.CellEventArgs"/> instance containing the event data.</param>
        private void grdCashRecon_ClickCellButton(object sender, CellEventArgs e)
        {
            if (e.Cell.Column.Key.Equals(COL_ManualEntryButton))
            {
                Forms.CashReconManualEntry cashTransactionDetailRecon = new Forms.CashReconManualEntry();
                //ManualEntryPassword manualEntryPassword = new ManualEntryPassword(Constants.CASHMANUALENTRYFORM);
                cashTransactionDetailRecon.ShowDialog();

            }
            if (e.Cell.Column.Key.Equals(COL_DetailsButton))
            {
                DataSourceNameID selectedDataSourceNameID = grdCashRecon.ActiveRow.Cells[COL_DataSourceNameID].Value as DataSourceNameID;
                if (selectedDataSourceNameID != null)
                {
                    Forms.CashReconDetails frmCashReconDetails = new Forms.CashReconDetails(selectedDataSourceNameID);
                    frmCashReconDetails.ShowDialog();
                }
                
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.FindForm().Close();
        }

     
    }
}
