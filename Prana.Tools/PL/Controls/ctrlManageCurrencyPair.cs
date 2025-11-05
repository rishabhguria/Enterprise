using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes;
using Prana.CommonDatabaseAccess;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.PubSubService.Interfaces;
using Prana.Utilities.UI.UIUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Prana.Tools
{
    public partial class ctrlManageCurrencyPair : UserControl, ILiveFeedCallback, IPublishing
    {
        private Dictionary<int, string> _currencyDict = new Dictionary<int, string>();
        private CurrencyCollection _currencyCollection = new CurrencyCollection();
        DuplexProxyBase<IPricingService> _pricingServicesProxy = null;

        DataTable _dtGridDataSource = new DataTable();

        public ctrlManageCurrencyPair()
        {
            try
            {
                InitializeComponent();
                if (!CustomThemeHelper.IsDesignMode())
                {
                    this.InitializeControl();
                    CreatePricingServiceProxy();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void CreatePricingServiceProxy()
        {
            try
            {
                _pricingServicesProxy = new DuplexProxyBase<IPricingService>("PricingServiceEndpointAddress", this);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ctrlManageCurrencyPair_Load(object sender, EventArgs e)
        {
            try
            {
                if (!CustomThemeHelper.IsDesignMode() && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
                {
                    SetButtonsColor();
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SetButtonsColor()
        {
            try
            {
                ubtnSave.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                ubtnSave.ForeColor = System.Drawing.Color.White;
                ubtnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                ubtnSave.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                ubtnSave.UseAppStyling = false;
                ubtnSave.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                ubtnAdd.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                ubtnAdd.ForeColor = System.Drawing.Color.White;
                ubtnAdd.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                ubtnAdd.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                ubtnAdd.UseAppStyling = false;
                ubtnAdd.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }


        /// <summary>
        /// Initialize the control
        /// </summary>
        public void InitializeControl()
        {
            try
            {
                BindComboBoxes();
                _dtGridDataSource = WindsorContainerManager.GetAllStandardCurrencyPairs().Tables[0];
                _dtGridDataSource.AcceptChanges();
                if (_dtGridDataSource != null)
                {
                    grdCurrencyPair.DataSource = null;
                    grdCurrencyPair.DataSource = _dtGridDataSource;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }

        }


        /// <summary>
        /// Unique message method implementation
        /// </summary>
        /// <returns>The message (name of the control here)</returns>
        public string getReceiverUniqueName()
        {
            try
            {
                return "ctrlManageCurrencyPair";
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        #region ILiveFeedCallback Members

        public delegate void L1ObjHandler(SymbolData level1Data);
        public delegate void L1ListObjHandler(List<SymbolData> level1Data);

        public void SnapshotResponse(SymbolData data, [Optional, DefaultParameterValue(null)] SnapshotResponseData snapshotResponseData)
        {
        }

        public void OptionChainResponse(string symbol, List<OptionStaticData> data)
        {
        }

        public void LiveFeedConnected()
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        public void LiveFeedDisConnected()
        {
            //throw new Exception("The method or operation is not implemented.");
        }
        #endregion

        Infragistics.Win.UltraWinGrid.UltraDropDown cmbFromCurrency = new Infragistics.Win.UltraWinGrid.UltraDropDown();
        Infragistics.Win.UltraWinGrid.UltraDropDown cmbToCurrency = new Infragistics.Win.UltraWinGrid.UltraDropDown();

        private void BindComboBoxes()
        {
            try
            {
                _currencyCollection = WindsorContainerManager.GetCurrenciesWithSymbol();
                cmbFromCurrency.DataSource = null;
                cmbFromCurrency.DataSource = _currencyCollection;
                cmbFromCurrency.DisplayMember = "Symbol";
                cmbFromCurrency.ValueMember = "CurrencyID";
                Utils.UltraDropDownFilter(cmbFromCurrency, "Symbol");

                cmbToCurrency.DataSource = null;
                cmbToCurrency.DataSource = _currencyCollection;
                cmbToCurrency.DisplayMember = "Symbol";
                cmbToCurrency.ValueMember = "CurrencyID";
                Utils.UltraDropDownFilter(cmbToCurrency, "Symbol");

                foreach (Prana.BusinessObjects.Currency currency in _currencyCollection)
                {
                    if (!_currencyDict.ContainsKey(currency.CurrencyID))
                    {
                        _currencyDict.Add(currency.CurrencyID, currency.Symbol);
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void grdCurrencyPair_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                UltraGridColumn colFromCurrency = grdCurrencyPair.DisplayLayout.Bands[0].Columns["FromCurrencyID"];
                colFromCurrency.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                colFromCurrency.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                colFromCurrency.ValueList = cmbFromCurrency;
                colFromCurrency.Header.Caption = "From Currency";
                colFromCurrency.Width = 150;
                colFromCurrency.Header.VisiblePosition = 0;

                UltraGridColumn colToCurrency = grdCurrencyPair.DisplayLayout.Bands[0].Columns["ToCurrencyID"];
                colToCurrency.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                colToCurrency.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                colToCurrency.ValueList = cmbToCurrency;
                colToCurrency.Header.Caption = "To Currency";
                colToCurrency.Width = 150;
                colToCurrency.Header.VisiblePosition = 1;

                UltraGridColumn colSymbol = grdCurrencyPair.DisplayLayout.Bands[0].Columns["BloombergSymbol"];
                colSymbol.CellActivation = Activation.NoEdit;
                colSymbol.MaxLength = 100;
                colSymbol.CharacterCasing = CharacterCasing.Upper;
                colSymbol.Width = 150;
                colSymbol.Header.VisiblePosition = 2;
            }

            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void grdCurrencyPair_CellChange(object sender, CellEventArgs e)
        {
            try
            {
                UltraGridRow activeRow = grdCurrencyPair.ActiveRow;

                string fromCurrency = activeRow.Cells["FromCurrencyID"].Text;
                string toCurrency = activeRow.Cells["ToCurrencyID"].Text;
                activeRow.Cells["BloombergSymbol"].Value = string.Empty;
                bool result = ValidateRowForForex();
                if (!result)
                {
                    if ((!String.IsNullOrEmpty(fromCurrency) && !fromCurrency.Equals(ApplicationConstants.C_COMBO_SELECT)) && (!String.IsNullOrEmpty(toCurrency) && !toCurrency.Equals(ApplicationConstants.C_COMBO_SELECT)))
                    {
                        activeRow.Cells["BloombergSymbol"].Value = fromCurrency + toCurrency + " CURNCY";
                    }
                }
                else
                {
                    e.Cell.CancelUpdate();
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// To check valid currency pair
        /// </summary>
        /// <returns></returns>
        private bool ValidateRowForForex()
        {
            bool result = false;
            string fromCurrency = grdCurrencyPair.ActiveRow.Cells["FromCurrencyID"].Text;
            string toCurrency = grdCurrencyPair.ActiveRow.Cells["ToCurrencyID"].Text;

            int currentIndex = grdCurrencyPair.ActiveRow.Index;
            int checkIndex = 0;

            //If both the currency values are empty or null then do nothing in this case.
            if (String.IsNullOrEmpty(fromCurrency) && String.IsNullOrEmpty(toCurrency))
            {
                result = true;
                return result;
            }

            //Validation check for the same currency in "to and from" criteria.
            if (!String.IsNullOrEmpty(fromCurrency) && !String.IsNullOrEmpty(toCurrency))
            {
                if (toCurrency.Equals(fromCurrency))
                {
                    InformationMessageBox.Display("From Currency should be different from To Currency.", "Forex Conversion");
                    result = true;
                    return result;
                }
            }

            //If the same combination already exists.
            foreach (Infragistics.Win.UltraWinGrid.UltraGridRow dr in grdCurrencyPair.Rows)
            {
                string dfromCurrency = dr.Cells["FromCurrencyID"].Text;
                string dtoCurrency = dr.Cells["ToCurrencyID"].Text;

                checkIndex = dr.Index;
                if (((fromCurrency == dfromCurrency && toCurrency == dtoCurrency) || (fromCurrency == dtoCurrency && toCurrency == dfromCurrency)) && checkIndex != currentIndex)
                {
                    result = true;
                    InformationMessageBox.Display("Same currency pair or opposite currency pair already exists,please select different pair.", "Forex Conversion");
                    break;
                }
            }

            return result;
        }


        #region IPublishing Members

        public void Publish(MessageData e, string topicName)
        {
            try
            {

            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        #endregion

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if (_pricingServicesProxy != null)
                {
                    _pricingServicesProxy.Dispose();
                }
                if (_dtGridDataSource != null)
                {
                    _dtGridDataSource.Dispose();
                }
                if (cmbFromCurrency != null)
                {
                    cmbFromCurrency.Dispose();
                }
                if (cmbToCurrency != null)
                {
                    cmbToCurrency.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// event to add new currency pair
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ubtnAdd_Click(object sender, EventArgs e)
        {
            AddNewRow();
        }

        /// <summary>
        /// event to save currency pair
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ubtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                int rowsAffected = 0;
                string errorMsg = string.Empty;
                ultraStatusBarCurrencyPair.Text = "Saving...";
                DataTable dtDataToSave = new DataTable();
                dtDataToSave = _dtGridDataSource.Clone();

                errorMsg = GetDataToSave(errorMsg, dtDataToSave);
                if (string.IsNullOrWhiteSpace(errorMsg))
                {
                    if (dtDataToSave != null && dtDataToSave.Rows.Count > 0)
                    {
                        dtDataToSave.TableName = "StandardCurrencyPairs";
                        rowsAffected = _pricingServicesProxy.InnerChannel.SaveStandardCurrencyPair(dtDataToSave);
                    }
                    else
                    {
                        ultraStatusBarCurrencyPair.Text = "Nothing to save.";
                    }

                    if (rowsAffected > 0)
                        ultraStatusBarCurrencyPair.Text = "Data Saved.";
                }
                else
                {
                    MessageBox.Show(errorMsg, "Prana Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Get the datatable representing the changes
        /// </summary>
        /// <returns>Datatable of changes</returns>
        public string GetDataToSave(string errorMsg, DataTable dtDataToSave)
        {
            try
            {
                if (_dtGridDataSource.Rows.Count > 0)
                {
                    foreach (DataRow dr in _dtGridDataSource.Rows)
                    {
                        DataRowState state = dr.RowState;
                        if (state == DataRowState.Modified || state == DataRowState.Added)
                        {
                            if (String.IsNullOrEmpty(dr["FromCurrencyID"].ToString()) || dr["FromCurrencyID"].ToString() == "0")
                            {
                                errorMsg = "Please select from currency.";
                                break;
                            }

                            if (String.IsNullOrEmpty(dr["ToCurrencyID"].ToString()) || dr["ToCurrencyID"].ToString() == "0")
                            {
                                errorMsg = "Please select To currency.";
                                break;
                            }
                            //if (String.IsNullOrEmpty(dr["BloombergSymbol"].ToString()))
                            //{
                            //    errorMsg = "Symbol can not be empty.";
                            //    break;
                            //}

                            dtDataToSave.Rows.Add(dr.ItemArray);
                            dr.AcceptChanges();
                        }
                    }
                    return errorMsg;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        /// <summary>
        /// To add blank row for new currency pair
        /// </summary>
        public void AddNewRow()
        {
            try
            {
                _dtGridDataSource = (DataTable)grdCurrencyPair.DataSource;
                //double zeroValue = 0;
                DataRow dtRow = null;

                dtRow = _dtGridDataSource.NewRow();
                foreach (DataColumn dCol in _dtGridDataSource.Columns)
                {
                    if (dCol.ColumnName.ToString().Equals("BloombsergSymbol"))
                    {
                        dtRow[dCol] = string.Empty;
                    }
                    //else
                    //{
                    //    dtRow[dCol] = zeroValue;
                    //}
                }
                _dtGridDataSource.Rows.Add(dtRow);
                _dtGridDataSource.AcceptChanges();
                grdCurrencyPair.DataSource = null;
                grdCurrencyPair.DataSource = _dtGridDataSource;
                grdCurrencyPair.Update();
                RowsCollection rows = grdCurrencyPair.Rows;
                int i = rows.Count;
                grdCurrencyPair.ActiveCell = grdCurrencyPair.Rows[i - 1].Cells[0];
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        #region IServiceOnDemandStatus Members
        public System.Threading.Tasks.Task<bool> HealthCheck()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
