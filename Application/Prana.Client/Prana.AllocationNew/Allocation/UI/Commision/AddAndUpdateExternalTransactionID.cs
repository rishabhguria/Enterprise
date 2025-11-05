using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;
using Prana.WCFConnectionMgr;
using Prana.Interfaces;
using Prana.Utilities.UIUtilities;

namespace Prana.AllocationNew
{
    public partial class AddAndUpdateExternalTransactionID : Form
    {
        private static AddAndUpdateExternalTransactionID _addAndUpdateExternalTransactionIDForm = null;

        static object _locker = new object();

        BackgroundWorker _bgFetchData = new BackgroundWorker();

        private AddAndUpdateExternalTransactionID()
        {
            InitializeComponent();
            if (CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
            {
                SetButtonsColor();
            }
            _bgFetchData.DoWork += new DoWorkEventHandler(_bgFetchData_DoWork);
            _bgFetchData.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_bgFetchData_RunWorkerCompleted);
            _bgFetchData.WorkerSupportsCancellation = true;
        }

        private void SetButtonsColor()
        {
            try
            {
                btnOk.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
                btnOk.ForeColor = System.Drawing.Color.White;
                btnOk.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnOk.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnOk.UseAppStyling = false;
                btnOk.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnCancel.BackColor = System.Drawing.Color.FromArgb(140, 5, 5);
                btnCancel.ForeColor = System.Drawing.Color.White;
                btnCancel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnCancel.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnCancel.UseAppStyling = false;
                btnCancel.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        #region Grid columns and captions

        const string COL_TaxlotID = "TaxlotID";
        const string COL_Symbol = "Symbol";
        const string COL_Side = "Side";
        const string COL_Quantity = "Quantity";
        const string COL_StrategyID = "StrategyID";
        const string COL_Strategy = "Strategy";
        const string COL_ExternalTransactionID = "ExternalTransactionID";


        const string CAP_TaxlotId = "Taxlot ID";
        const string CAP_Symbol = "Symbol";
        const string CAP_Side = "Side";
        const string CAP_Quantity = "Quantity";
        const string CAP_Strategy = "Strategy";
        const string CAP_ExternalTransactionID = "External Transaction ID";

        #endregion Grid columns and captions

        ProxyBase<IAllocationServices> _allocationServices = null;
        public ProxyBase<IAllocationServices> AllocationServices
        {
            set
            {
                _allocationServices = value;
            }
        }

        public static AddAndUpdateExternalTransactionID GetInstance()
        {
            if (_addAndUpdateExternalTransactionIDForm == null)
            {
                lock (_locker)
                {
                    if (_addAndUpdateExternalTransactionIDForm == null)
                    {
                        _addAndUpdateExternalTransactionIDForm = new AddAndUpdateExternalTransactionID();
                    }
                }
            }
            return _addAndUpdateExternalTransactionIDForm;
        }       

        private string _externalTransactionIDs;

        public string ExternalTransactionIDs
        {
            get { return _externalTransactionIDs; }
            set { _externalTransactionIDs = value; }
        }

        public void BindDataSource(DataTable dt)
        {
            try
            {
                if (dt != null)
                {
                    grdExternalTransactionID.DataSource = dt;

                    //if (grdExternalTransactionID.DataSource != null && grdExternalTransactionID.Rows.Count > 0)
                    //{
                    //    grdExternalTransactionID.ActiveRow = grdExternalTransactionID.Rows[0];
                    //    grdExternalTransactionID.ActiveCell = grdExternalTransactionID.Rows[0].Cells[COL_ExternalTransactionID];
                    //    //grdExternalTransactionID.ActiveCell.Activate();
                    //    //grdExternalTransactionID.Rows[0].Selected = true;
                    //    //grdExternalTransactionID.Rows[0].Cells[COL_ExternalTransactionID].Selected = true;
                    //}
                }
                else
                { toolStripStatusLabel1.Text = string.Empty; }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        #region Grid Initialization and other events

        private void grdExternalTransactionID_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
        {
            try
            {
                SetOpenTradesGridRowAppearance(e);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SetOpenTradesGridRowAppearance(InitializeRowEventArgs e)
        {
            try
            {
                e.Row.Appearance.ForeColor = Color.Yellow;
                grdExternalTransactionID.DisplayLayout.Bands[0].Override.CellClickAction = CellClickAction.EditAndSelectText;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void grdExternalTransactionID_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                SetGridColumns(grdExternalTransactionID);

                e.Layout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

                grdExternalTransactionID.DisplayLayout.Override.RowAppearance.BackColor = Color.FromArgb(64, 64, 64);
                grdExternalTransactionID.DisplayLayout.Override.RowAppearance.BackColor2 = Color.Transparent;
                grdExternalTransactionID.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Black;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SetGridColumns(UltraGrid grid)
        {
            try
            {
                UltraGridBand gridBand = grid.DisplayLayout.Bands[0];
                ColumnsCollection gridColumns = gridBand.Columns;
                foreach (UltraGridColumn column in gridColumns)
                {
                    column.Hidden = true;
                    column.CellActivation = Activation.NoEdit;
                }
                UltraGridColumn colTaxlotID = gridBand.Columns[COL_TaxlotID];
                colTaxlotID.Hidden = false;
                colTaxlotID.Header.Caption = CAP_TaxlotId;
                colTaxlotID.Header.VisiblePosition = 0;
                colTaxlotID.CellActivation = Activation.NoEdit;
                colTaxlotID.Width = 140;

                UltraGridColumn colSymbol = gridBand.Columns[COL_Symbol];
                colSymbol.Hidden = false;
                colSymbol.Header.VisiblePosition = 1;
                colSymbol.Header.Caption = CAP_Symbol;
                colSymbol.CellActivation = Activation.NoEdit;

                UltraGridColumn colSide = gridBand.Columns[COL_Side];
                colSide.Hidden = false;
                colSide.Header.VisiblePosition = 2;
                colSide.Header.Caption = CAP_Side;

                UltraGridColumn colQuantity = gridBand.Columns[COL_Quantity];
                colQuantity.Hidden = false;
                colQuantity.Header.VisiblePosition = 3;
                colQuantity.Header.Caption = CAP_Quantity;
                colQuantity.Format = ApplicationConstants.FORMAT_QTY;
                colQuantity.CellActivation = Activation.NoEdit;

                UltraGridColumn colStrategy = gridBand.Columns[COL_Strategy];
                colStrategy.Hidden = false;
                colStrategy.Header.Caption = CAP_Strategy;
                colStrategy.Header.VisiblePosition = 4;
                colStrategy.Width = 132;
                colStrategy.CellActivation = Activation.NoEdit;

                UltraGridColumn colExternalTransactionID = gridBand.Columns[COL_ExternalTransactionID];
                colExternalTransactionID.Hidden = false;
                colExternalTransactionID.Header.Caption = CAP_ExternalTransactionID;
                colExternalTransactionID.Header.VisiblePosition = 5;
                colExternalTransactionID.CellActivation = Activation.AllowEdit;
                colExternalTransactionID.DefaultCellValue = string.Empty;
                colExternalTransactionID.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                colExternalTransactionID.MaxLength = 100;

                UltraGridColumn colStrategyID = gridBand.Columns[COL_StrategyID];
                colStrategyID.Hidden = true;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        #endregion Grid Initialization and other events

        #region Button click events
        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                string externalTranID = string.Empty;
                grdExternalTransactionID.UpdateData();
                DataTable dTable = grdExternalTransactionID.DataSource as DataTable;
                if (dTable != null && dTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dTable.Rows)
                    {
                        if (!string.IsNullOrEmpty(row[COL_ExternalTransactionID].ToString()))
                            externalTranID = externalTranID + row[COL_StrategyID] + ":" + row[COL_ExternalTransactionID] + ",";
                    }
                }
                if (externalTranID.Length > 0)
                {
                    externalTranID = externalTranID.Substring(0, externalTranID.Length - 1);
                }
                _externalTransactionIDs = externalTranID;

                this.DialogResult = DialogResult.OK;

                this.Close();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                _externalTransactionIDs = "Cancelled";
                this.Close();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        #endregion Button click events

        private void AddAndUpdateExternalTransactionID_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                _addAndUpdateExternalTransactionIDForm = null;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void grdExternalTransactionID_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                switch (e.KeyCode)
                {
                    case Keys.Down:
                        grdExternalTransactionID.PerformAction(UltraGridAction.BelowCell);
                        grdExternalTransactionID.PerformAction(UltraGridAction.EnterEditMode);
                        e.Handled = true;
                        break;

                    case Keys.Up:
                        grdExternalTransactionID.PerformAction(UltraGridAction.AboveCell);
                        grdExternalTransactionID.PerformAction(UltraGridAction.EnterEditMode);
                        e.Handled = true;
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        #region BackGroundWorker to Fetch Data Events

        public void SetUp(string taxlotID)
        {
            try
            {
                //if (UIValidation.GetInstance().validate(this))
                //{
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)(delegate()
                    {
                        SetUp(taxlotID);

                    }));
                }
                else
                {
                    if (!string.IsNullOrEmpty(taxlotID))
                    {
                        grpMain.Enabled = false;
                        toolStripStatusLabel1.Text = "Fetching data...";
                        //pass taxlotID as arguments to backgroundworker to fetch data.
                        object[] arguments = new object[2];
                        arguments[0] = taxlotID;
                        if (!_bgFetchData.IsBusy)
                        {
                            //wingrid performance improve
                            //http://help.infragistics.com/Help/NetAdvantage/WinForms/2012.1/CLR2.0/html/WinGrid_Formatting_and_Appearance_based_Performance_Improvement.html

                            //this.grdExternalTransactionID.Enabled = false;
                            //this.grdExternalTransactionID.BeginUpdate();
                            //this.grdExternalTransactionID.SuspendRowSynchronization();

                            _bgFetchData.RunWorkerAsync(arguments);
                        }
                    }
                }
            }
            //}
            catch (Exception ex)
            {
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDTHROW);

                toolStripStatusLabel1.Text = string.Empty;
                grpMain.Enabled = true;
                if (rethrow)
                {
                    throw;
                }
            }
        }

        void _bgFetchData_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (!_bgFetchData.CancellationPending)//checks for cancel request
                {
                    object[] arguments = e.Argument as object[];

                    string taxlotID = arguments[0] as string;

                    DataTable dTable = _allocationServices.InnerChannel.GetTaxlotDetailsToUpdateExternalTransactionID(taxlotID);

                    object[] result = new object[2];
                    result[0] = dTable;
                    e.Result = result;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        void _bgFetchData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled)//it doesn't matter if the BG worker ends normally, or gets cancelled,
                {              //both cases RunWorkerCompleted is invoked, so we need to check what has happened
                    MessageBox.Show("Operation has been cancelled!", "Fetch Data Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (e.Result != null)
                {
                    object[] arguments = e.Result as object[];
                    DataTable dTable = arguments[0] as DataTable;

                    BindDataSource(dTable);

                    toolStripStatusLabel1.Text = "Data fetched successfully";  
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
                grpMain.Enabled = true;

                //this.grdExternalTransactionID.Enabled = true;
                //this.grdExternalTransactionID.ResumeRowSynchronization();
                //this.grdExternalTransactionID.EndUpdate();
            }
        }


        #endregion
    }
}