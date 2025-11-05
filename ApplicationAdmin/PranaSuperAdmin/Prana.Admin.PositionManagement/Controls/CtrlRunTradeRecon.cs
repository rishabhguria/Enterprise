using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using Infragistics.Win.UltraWinGrid;

using Nirvana.Admin.PositionManagement.Forms;
using Nirvana.Admin.PositionManagement.Classes;
using Nirvana.Admin.PositionManagement.BusinessObjects;

namespace Nirvana.Admin.PositionManagement.Controls
{
    public partial class CtrlRunTradeRecon : UserControl
    {
        #region Grid Column Names
        const string COL_IsSelectedforViewing = "IsSelectedforViewing";
        const string COL_DataSourceName = "DataSourceName";
        const string COL_NoOfDataSourceRecords = "NoOfDataSourceRecords";
        const string COL_NoOfApplicationRecords = "NoOfApplicationRecords";
        const string COL_NoOfReconRecords = "NoOfReconRecords";
        const string COL_NoOfMatchedRecords = "NoOfMatchedRecords";
        const string COL_NoOfMismatchedRecords = "NoOfMismatchedRecords";
        const string COL_ReconStatus = "ReconStatus";

        #endregion Grid Column Names

        BindingSource _formBindingSource = new BindingSource();
        DataSourceReconSummaryInfo _dataSourceReconSummaryInfo = new DataSourceReconSummaryInfo();
        internal event EventHandler CancelClicked;

        //internal event EventHandler ViewExceptionReport;

        public CtrlRunTradeRecon()
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

        #endregion

        private void SetupBinding()
        {
            //_formBindingSource.DataSource = typeof(DataSourceReconColumnsInfo);
            _formBindingSource.DataSource = _dataSourceReconSummaryInfo; // newInfo;

            BindGridComboBoxes();

            //cmbReconDate.DataBindings.Add("Value", _formBindingSource, "ReconDate");

            grdReconSummary.DataMember = "TradeReconSummaryList";
            grdReconSummary.DataSource = _formBindingSource;
        }

        /// <summary>
        /// On the change of any information in the bindingsource
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _formBindingSource_BindingComplete(object sender, BindingCompleteEventArgs e)
        {
           
        }

        private void BindGridComboBoxes()
        {
            cmbReconStatus.DisplayMember = "DisplayText";
            cmbReconStatus.ValueMember = "Value";
            cmbReconStatus.DataSource = EnumHelper.ConvertEnumForBinding(typeof(ReconStatus));
            Utils.UltraDropDownFilter(cmbReconStatus, "DisplayText");
        }


        private void btnView_Click(object sender, EventArgs e)
        {
            OnViewClick();
        }

        /// <summary>
        /// Called when [View click].
        /// </summary>
        private void OnViewClick()
        {
            if (_formBindingSource.List.Count > 0)
            {
                _dataSourceReconSummaryInfo = _formBindingSource.List[0] as DataSourceReconSummaryInfo;
            }

            if (_dataSourceReconSummaryInfo == null || _dataSourceReconSummaryInfo.TradeReconSummaryList == null || _dataSourceReconSummaryInfo.TradeReconSummaryList.Count == 0)
            {
                MessageBox.Show("Reconciliation Summary Data Unavailable.", "Summary Unavailable", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            foreach (TradeReconSummary summary in _dataSourceReconSummaryInfo.TradeReconSummaryList)
            {
                if (summary.IsSelectedforViewing)
                {
                    ViewExceptionReport(summary.DataSourceName);
                    break;
                    //if (ViewExceptionReport != null)
                    //{
                    //    ViewExceptionReport(this, eventArgs);
                    //}
                }
            }
        }

        /// <summary>
        /// Open the ExceptionReport form 
        /// </summary>
        private static void ViewExceptionReport(DataSourceNameID dataSourceNameID)
        {
            ExceptionReport exceptionReport = new ExceptionReport(dataSourceNameID);
            exceptionReport.ShowDialog();
        }

        
        private void grdReconSummary_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            UltraGridBand band = e.Layout.Bands[0];
            e.Layout.Override.WrapHeaderText = Infragistics.Win.DefaultableBoolean.True;

            grdReconSummary.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
            grdReconSummary.DisplayLayout.ScrollBounds = ScrollBounds.ScrollToLastItem;
            grdReconSummary.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;
            grdReconSummary.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            grdReconSummary.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            grdReconSummary.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            grdReconSummary.DisplayLayout.Override.AllowColSwapping = AllowColSwapping.NotAllowed;
            grdReconSummary.DisplayLayout.Override.HeaderAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grdReconSummary.DisplayLayout.Bands[0].Override.AllowColSwapping = AllowColSwapping.NotAllowed;

            ///TODO : Remove this hardcoding of column names, We might have some attribute to be put on the name 
            ///of the property.

            UltraGridColumn colIsSelectedforViewing = band.Columns[COL_IsSelectedforViewing];
            colIsSelectedforViewing.Header.Caption = "";
            colIsSelectedforViewing.Header.VisiblePosition = 1;

            UltraGridColumn colDataSourceName = band.Columns[COL_DataSourceName];
            colDataSourceName.Header.Caption = "Data Source";
            colDataSourceName.Header.VisiblePosition = 2;

            UltraGridColumn colNoOfDataSourceRecords = band.Columns[COL_NoOfDataSourceRecords];
            colNoOfDataSourceRecords.Header.Caption = "Data Source Records";
            colNoOfDataSourceRecords.Header.VisiblePosition = 3;

            UltraGridColumn colNoOfApplicationRecords = band.Columns[COL_NoOfApplicationRecords];
            colNoOfApplicationRecords.Header.Caption = "Application Records";
            colNoOfApplicationRecords.Header.VisiblePosition = 4;

            UltraGridColumn colNoOfReconRecords = band.Columns[COL_NoOfReconRecords];
            colNoOfReconRecords.Header.Caption = "Reconciled Records";
            colNoOfReconRecords.Header.VisiblePosition = 5;

            UltraGridColumn colNoOfMatchedRecords = band.Columns[COL_NoOfMatchedRecords];
            colNoOfMatchedRecords.Header.Caption = "Matched Records";
            colNoOfMatchedRecords.Header.VisiblePosition = 6;

            UltraGridColumn colNoOfMismatchedRecords = band.Columns[COL_NoOfMismatchedRecords];
            colNoOfMismatchedRecords.Header.Caption = "Mis-Matched Records";
            colNoOfMismatchedRecords.Header.VisiblePosition = 7;

            UltraGridColumn colReconStatus = band.Columns[COL_ReconStatus];
            colReconStatus.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;
            colReconStatus.ButtonDisplayStyle = ButtonDisplayStyle.Always;
            colReconStatus.ValueList = cmbReconStatus;
            colReconStatus.Header.Caption = "Status";
            colReconStatus.Header.VisiblePosition = 8;

        }

        /// <summary>
        /// TODO : Need to be replaced by some bindingsource change event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ctrlSourceName1_SelectionChanged(object sender, EventArgs e)
        {
            DataSourceNameID changedDataSourceNameID = ((DataSourceEventArgs)e).DataSourceNameID;

            if (_formBindingSource.List.Count > 0)
            {
                _dataSourceReconSummaryInfo = _formBindingSource.List[0] as DataSourceReconSummaryInfo;
            }

            _dataSourceReconSummaryInfo.DataSourceNameIDValue = changedDataSourceNameID;
            _dataSourceReconSummaryInfo.ReconDate = _dataSourceReconSummaryInfo.ReconDate;
            _dataSourceReconSummaryInfo.TradeReconSummaryList = RetrieveAppReconColumns(changedDataSourceNameID,Convert.ToDateTime(cmbReconDate.Value));
        }

        private BindingList<TradeReconSummary> RetrieveAppReconColumns(DataSourceNameID dataSource,DateTime date)
        {
            BindingList<TradeReconSummary> list = null;

            if (date == DateTime.Today)
            {
                if (dataSource.ID == 1)
                {
                    list = RetrieveDataSourceId1(dataSource);
                }

                if (dataSource.ID == 32)
                {
                    list = RetrieveDataSourceId32(dataSource);
                }

                if (dataSource.ID == 35)
                {
                    list = RetrieveDataSourceId35(dataSource);
                }

                ///All
                if (dataSource.ID == 0)
                {
                    list = RetrieveDataSourceIdAll();
                }
            }
           
            return list;
        }

        private BindingList<TradeReconSummary> RetrieveDataSourceIdAll()
        {
            BindingList<TradeReconSummary> list = new BindingList<TradeReconSummary>();
            SortableSearchableList<DataSourceNameID> fullList = DataSourceNameIDList.GetInstance().Retrieve;
            foreach (DataSourceNameID dataSourceNameID in fullList)
            {
                if (!dataSourceNameID.FullName.Equals(Constants.C_COMBO_SELECT) || !dataSourceNameID.FullName.Equals(Constants.C_COMBO_ALL))
                {
                    if (dataSourceNameID.ID == 1)
                    {
                        TradeReconSummary tradeReconSummary = new TradeReconSummary();
                        tradeReconSummary.DataSourceName = dataSourceNameID;    
                        tradeReconSummary.NoOfDataSourceRecords = 1000;
                        tradeReconSummary.NoOfApplicationRecords = 1000;
                        tradeReconSummary.NoOfReconRecords = 1000;
                        tradeReconSummary.NoOfMatchedRecords = 990;
                        tradeReconSummary.NoOfMismatchedRecords = 100;
                        tradeReconSummary.ReconStatus = ReconStatus.Open;

                        list.Add(tradeReconSummary);
                    }
                    if (dataSourceNameID.ID == 32)
                    {
                        TradeReconSummary tradeReconSummary = new TradeReconSummary();
                        tradeReconSummary.DataSourceName = dataSourceNameID;
                        tradeReconSummary.NoOfDataSourceRecords = 500;
                        tradeReconSummary.NoOfApplicationRecords = 500;
                        tradeReconSummary.NoOfReconRecords = 500;
                        tradeReconSummary.NoOfMatchedRecords = 480;
                        tradeReconSummary.NoOfMismatchedRecords = 20;
                        tradeReconSummary.ReconStatus = ReconStatus.Open;

                        list.Add(tradeReconSummary);
                    }
                    if (dataSourceNameID.ID == 35)
                    {
                        TradeReconSummary tradeReconSummary = new TradeReconSummary();
                        tradeReconSummary.DataSourceName = dataSourceNameID;
                        tradeReconSummary.NoOfDataSourceRecords = 100;
                        tradeReconSummary.NoOfApplicationRecords = 100;
                        tradeReconSummary.NoOfReconRecords = 100;
                        tradeReconSummary.NoOfMatchedRecords = 99;
                        tradeReconSummary.NoOfMismatchedRecords = 1;
                        tradeReconSummary.ReconStatus = ReconStatus.Open;

                        list.Add(tradeReconSummary);
                    }  

                }
            }

            return list;
        }

        private BindingList<TradeReconSummary> RetrieveDataSourceId1(DataSourceNameID dataSource)
        {
            TradeReconSummary tradeReconSummary = new TradeReconSummary();
            tradeReconSummary.DataSourceName = dataSource;
            tradeReconSummary.NoOfDataSourceRecords = 1000;
            tradeReconSummary.NoOfApplicationRecords = 1000;
            tradeReconSummary.NoOfReconRecords = 1000;
            tradeReconSummary.NoOfMatchedRecords = 990;
            tradeReconSummary.NoOfMismatchedRecords = 100;
            tradeReconSummary.ReconStatus = ReconStatus.Open;

            BindingList<TradeReconSummary> list = new BindingList<TradeReconSummary>();
            list.Add(tradeReconSummary);
            return list;
        }

        private BindingList<TradeReconSummary> RetrieveDataSourceId32(DataSourceNameID dataSource)
        {
            TradeReconSummary tradeReconSummary = new TradeReconSummary();
            tradeReconSummary.DataSourceName = dataSource;
            tradeReconSummary.NoOfDataSourceRecords = 500;
            tradeReconSummary.NoOfApplicationRecords = 500;
            tradeReconSummary.NoOfReconRecords = 500;
            tradeReconSummary.NoOfMatchedRecords = 480;
            tradeReconSummary.NoOfMismatchedRecords = 20;
            tradeReconSummary.ReconStatus = ReconStatus.Open;

            BindingList<TradeReconSummary> list = new BindingList<TradeReconSummary>();
            list.Add(tradeReconSummary);
            return list;
        } 

        private BindingList<TradeReconSummary> RetrieveDataSourceId35(DataSourceNameID dataSource)
        {
            TradeReconSummary tradeReconSummary = new TradeReconSummary();
            tradeReconSummary.DataSourceName = dataSource;
            tradeReconSummary.NoOfDataSourceRecords = 100;
            tradeReconSummary.NoOfApplicationRecords = 100;
            tradeReconSummary.NoOfReconRecords = 100;
            tradeReconSummary.NoOfMatchedRecords = 99;
            tradeReconSummary.NoOfMismatchedRecords = 1;
            tradeReconSummary.ReconStatus = ReconStatus.Open;

            BindingList<TradeReconSummary> list = new BindingList<TradeReconSummary>();
            list.Add(tradeReconSummary);
            return list;
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (CancelClicked != null)
            {
                CancelClicked(this, e);
            }
        }

        /// <summary>
        /// TODO : ReconDate is not binding 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbReconDate_ValueChanged(object sender, EventArgs e)
        {
            if (_formBindingSource.List.Count > 0)
            {
                _dataSourceReconSummaryInfo = _formBindingSource.List[0] as DataSourceReconSummaryInfo;
                _dataSourceReconSummaryInfo.TradeReconSummaryList = RetrieveAppReconColumns(_dataSourceReconSummaryInfo.DataSourceNameIDValue, Convert.ToDateTime(cmbReconDate.Value)); //_dataSourceReconSummaryInfo.ReconDate);    
            }
        }

    }
}
