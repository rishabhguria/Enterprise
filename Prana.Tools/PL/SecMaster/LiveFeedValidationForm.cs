using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.ClientCommon;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.UI;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Prana.Tools.PL.SecMaster
{
    public partial class LiveFeedValidationForm : Form, IPluggableTools, ILaunchForm
    {

        public delegate void ConnectionInvokeDelegate(object sender, EventArgs e);
        ISecurityMasterServices _securityMaster = null;
        bool _isClearGrid = true;
        /// <summary>
        /// modified by: sachin mishra,30 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        public LiveFeedValidationForm()
        {
            try
            {
                InitializeComponent();
                BindSymbology();
                grdSymbol.DataSource = CreateEmptyDataTable();
                this.grdSymbol.DisplayLayout.Bands[0].Columns[1].CellActivation = Activation.NoEdit;
                this.grdSymbol.DisplayLayout.Bands[0].Columns[2].CellActivation = Activation.NoEdit;
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

        //private void UnHideColumns(string[] columns)
        //{
        //    try
        //    {
        //        foreach (string col in columns)
        //        {
        //            UltraGridColumn column = grdSymbol.DisplayLayout.Bands[0].Columns[col];
        //            column.Hidden = false; ;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        /// <summary>
        /// function to initialize LiveFeedValidation Form
        /// 
        ///modified by: sachin mishra 30 jan 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        /// <param name="securityMaster"></param>
        public void SetUp(string symbol = "", string symbology = "")
        {
            try
            {
                if (!string.IsNullOrEmpty(symbol) && !string.IsNullOrEmpty(symbology))
                {
                    DataTable dtSymbol = CreateEmptyDataTable();
                    DataRow dr = dtSymbol.NewRow();
                    dr["Symbol"] = symbol;
                    dtSymbol.Rows.Add(dr);
                    grdSymbol.DataSource = dtSymbol;
                    _isClearGrid = false;
                    cmbSymbology.Value = SecMasterHelper.GetSymbology(symbology);
                }
                else
                {
                    _isClearGrid = true;
                }
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
        /// Handle when server connected 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _securityMaster_Connected(object sender, EventArgs e)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        ConnectionInvokeDelegate connectionStatusDelegate = new ConnectionInvokeDelegate(_securityMaster_Connected);
                        this.BeginInvoke(connectionStatusDelegate, new object[] { sender, e });
                    }
                    else
                    {
                        ultraStatusBar1.Text = "Trade Server : Connected !";
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

        /// <summary>
        /// handle on server disconnected 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        void _securityMaster_Disconnected(object sender, EventArgs e)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        ConnectionInvokeDelegate connectionStatusDelegate = new ConnectionInvokeDelegate(_securityMaster_Disconnected);
                        this.BeginInvoke(connectionStatusDelegate, new object[] { sender, e });
                    }
                    else
                    {
                        ultraStatusBar1.Text = "Trade Server : Disconnected !!";
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

        /// <summary>
        /// To get sec master data response
        /// </summary>
        /// <param name="secMasterObj"></param>
        void _securityMaster_SecMstrDataResponse(object sender, EventArgs<SecMasterBaseObj> e)
        {
            try
            {
                SecMasterBaseObj secMasterObj = e.Value;
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        if (secMasterObj != null)
                        {
                            string symbol = string.Empty;
                            int symbology = int.Parse(cmbSymbology.Value.ToString());

                            switch ((ApplicationConstants.SymbologyCodes)symbology)
                            {
                                case ApplicationConstants.SymbologyCodes.TickerSymbol:
                                    symbol = secMasterObj.TickerSymbol;
                                    break;
                                case ApplicationConstants.SymbologyCodes.ReutersSymbol:
                                    symbol = secMasterObj.ReutersSymbol;
                                    break;
                                case ApplicationConstants.SymbologyCodes.ISINSymbol:
                                    symbol = secMasterObj.ISINSymbol;
                                    break;
                                case ApplicationConstants.SymbologyCodes.SEDOLSymbol:
                                    symbol = secMasterObj.SedolSymbol;
                                    break;
                                case ApplicationConstants.SymbologyCodes.CUSIPSymbol:
                                    symbol = secMasterObj.CusipSymbol;
                                    break;
                                case ApplicationConstants.SymbologyCodes.BloombergSymbol:
                                    symbol = secMasterObj.BloombergSymbol;
                                    break;
                                case ApplicationConstants.SymbologyCodes.OSIOptionSymbol:
                                    symbol = secMasterObj.OSIOptionSymbol;
                                    break;
                                case ApplicationConstants.SymbologyCodes.IDCOOptionSymbol:
                                    symbol = secMasterObj.IDCOOptionSymbol;
                                    break;
                                case ApplicationConstants.SymbologyCodes.OPRAOptionSymbol:
                                    symbol = secMasterObj.OpraSymbol;
                                    break;
                            }

                            foreach (UltraGridRow row in grdSymbol.Rows)
                            {

                                if (symbol.Equals(row.Cells["Symbol"].Value.ToString().Trim(), StringComparison.OrdinalIgnoreCase))
                                {
                                    if (secMasterObj.AUECID > 0)
                                    {
                                        row.Cells["Asset"].Value = secMasterObj.AssetCategory.ToString();
                                        row.Cells["Comments"].Value = "Security Validated";
                                        if (!string.IsNullOrEmpty(secMasterObj.ISINSymbol.ToString()))
                                        {
                                            row.Cells["ISIN"].Value = secMasterObj.ISINSymbol.ToString();
                                        }
                                        else
                                        {
                                            row.Cells["ISIN"].Value = "N/A";
                                        }
                                        if (!string.IsNullOrEmpty(secMasterObj.LongName.ToString()))
                                        {
                                            row.Cells["Description"].Value = secMasterObj.LongName.ToString();
                                        }
                                        else
                                        {
                                            row.Cells["Description"].Value = "N/A";
                                        }
                                        if (!string.IsNullOrEmpty(secMasterObj.SedolSymbol.ToString()))
                                        {
                                            row.Cells["Sedol"].Value = secMasterObj.SedolSymbol.ToString();
                                        }
                                        else
                                        {
                                            row.Cells["Sedol"].Value = "N/A";
                                        }
                                        if (!string.IsNullOrEmpty(secMasterObj.CusipSymbol.ToString()))
                                        {
                                            row.Cells["Cusip"].Value = secMasterObj.CusipSymbol.ToString();
                                        }
                                        else
                                        {
                                            row.Cells["Cusip"].Value = "N/A";
                                        }
                                        if (!string.IsNullOrEmpty(secMasterObj.CurrencyID.ToString()))
                                        {
                                            row.Cells["Currency"].Value = secMasterObj.CurrencyID.ToString();
                                        }
                                        else
                                        {
                                            row.Cells["Currency"].Value = "N/A";
                                        }
                                        if (!string.IsNullOrEmpty(secMasterObj.ExchangeID.ToString()))
                                        {
                                            row.Cells["TradedExchange"].Value = secMasterObj.ExchangeID.ToString();
                                        }
                                        else
                                        {
                                            row.Cells["TradedExchange"].Value = "N/A";
                                        }
                                    }
                                    else
                                    {
                                        row.Cells["Asset"].Value = "N/A";
                                        row.Cells["Comments"].Value = secMasterObj.Comments;
                                        if (!string.IsNullOrEmpty(secMasterObj.ISINSymbol.ToString()))
                                        {
                                            row.Cells["ISIN"].Value = secMasterObj.ISINSymbol.ToString();
                                        }
                                        else
                                        {
                                            row.Cells["ISIN"].Value = "N/A";
                                        }
                                        if (!string.IsNullOrEmpty(secMasterObj.SedolSymbol.ToString()))
                                        {
                                            row.Cells["Sedol"].Value = secMasterObj.SedolSymbol.ToString();
                                        }
                                        else
                                        {
                                            row.Cells["Sedol"].Value = "N/A";
                                        }
                                        if (!string.IsNullOrEmpty(secMasterObj.CusipSymbol.ToString()))
                                        {
                                            row.Cells["Cusip"].Value = secMasterObj.CusipSymbol.ToString();
                                        }
                                        else
                                        {
                                            row.Cells["Cusip"].Value = "N/A";
                                        }
                                        if (!string.IsNullOrEmpty(secMasterObj.CurrencyID.ToString()))
                                        {
                                            row.Cells["Currency"].Value = secMasterObj.CurrencyID.ToString();
                                        }
                                        else
                                        {
                                            row.Cells["Currency"].Value = "N/A";
                                        }
                                        if (!string.IsNullOrEmpty(secMasterObj.ExchangeID.ToString()))
                                        {
                                            row.Cells["TradedExchange"].Value = secMasterObj.ExchangeID.ToString();
                                        }
                                        else
                                        {
                                            row.Cells["TradedExchange"].Value = "N/A";
                                        }
                                    }
                                }
                            }
                        }
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

        /// <summary>
        /// Initialize the layout of the grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdSymbol_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                e.Layout.Override.ActiveRowAppearance.BackColor = Color.Black;
                e.Layout.Override.ActiveRowAppearance.BackColor2 = Color.Black;
                e.Layout.Override.ActiveRowAppearance.ForeColor = Color.White;

                e.Layout.Override.RowAppearance.BackColor = Color.Black;
                e.Layout.Override.RowAppearance.BackColor2 = Color.Black;
                e.Layout.Override.RowAppearance.ForeColor = Color.White;

                e.Layout.Override.ActiveCellAppearance.BackColor = Color.Black;
                e.Layout.Override.ActiveCellAppearance.BackColor2 = Color.Black;
                e.Layout.Override.ActiveCellAppearance.ForeColor = Color.White;

                e.Layout.Override.DefaultColWidth = 120;

                UltraGridBand grdBand = e.Layout.Bands[0];
                grdBand.Override.AllowRowFiltering = DefaultableBoolean.True;
                grdBand.Override.HeaderAppearance.TextHAlign = HAlign.Center;

                if (grdBand.Columns.Exists("Symbol"))
                {
                    UltraGridColumn colSymbol = grdBand.Columns["Symbol"];
                    colSymbol.Header.Caption = "Symbol";
                    colSymbol.CellActivation = Activation.AllowEdit;
                    colSymbol.Header.VisiblePosition = 1;
                }
                if (grdBand.Columns.Exists("Description"))
                {
                    UltraGridColumn colSymbol = grdBand.Columns["Description"];
                    colSymbol.Header.Caption = "Description";
                    colSymbol.CellActivation = Activation.AllowEdit;
                    colSymbol.Header.VisiblePosition = 2;
                }
                if (grdBand.Columns.Exists("Asset"))
                {
                    UltraGridColumn colAccount = grdBand.Columns["Asset"];
                    colAccount.Header.Caption = "Asset";
                    colAccount.CellActivation = Activation.NoEdit;
                    colAccount.Header.VisiblePosition = 3;
                }
                if (grdBand.Columns.Exists("Comments"))
                {
                    UltraGridColumn colComments = grdBand.Columns["Comments"];
                    colComments.Header.Caption = "Comments";
                    colComments.CellActivation = Activation.NoEdit;
                    colComments.Header.VisiblePosition = 4;
                }

                if (grdBand.Columns.Exists("ISIN"))
                {
                    UltraGridColumn colComments = grdBand.Columns["ISIN"];
                    colComments.Header.Caption = "ISIN";
                    colComments.CellActivation = Activation.NoEdit;
                    colComments.Header.VisiblePosition = 5;
                }
                if (grdBand.Columns.Exists("Sedol"))
                {
                    UltraGridColumn colComments = grdBand.Columns["Sedol"];
                    colComments.Header.Caption = "Sedol";
                    colComments.CellActivation = Activation.NoEdit;
                    colComments.Header.VisiblePosition = 6;
                }
                if (grdBand.Columns.Exists("Cusip"))
                {
                    UltraGridColumn colComments = grdBand.Columns["Cusip"];
                    colComments.Header.Caption = "Cusip";
                    colComments.CellActivation = Activation.NoEdit;
                    colComments.Header.VisiblePosition = 7;
                }
                if (grdBand.Columns.Exists("Currency"))
                {
                    UltraGridColumn colComments = grdBand.Columns["Currency"];
                    colComments.Header.Caption = "Currency";
                    colComments.CellActivation = Activation.NoEdit;
                    colComments.ValueList = SecMasterHelper.getInstance().Currencies.Clone();
                    colComments.Header.VisiblePosition = 8;
                }
                if (grdBand.Columns.Exists("TradedExchange"))
                {
                    UltraGridColumn colComments = grdBand.Columns["TradedExchange"];
                    colComments.Header.Caption = "Traded Exchange";
                    colComments.CellActivation = Activation.NoEdit;
                    colComments.ValueList = SecMasterHelper.getInstance().Exchanges.Clone();
                    colComments.Header.VisiblePosition = 9;
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

        //private void HideColumns(string[] columns)
        //{
        //    try
        //    {
        //        foreach (string col in columns)
        //        {
        //            UltraGridColumn column = grdSymbol.DisplayLayout.Bands[0].Columns[col];
        //            column.Hidden = true;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        /// <summary>
        /// Function to create empty datatable
        /// </summary>
        /// <returns></returns>
        private DataTable CreateEmptyDataTable()
        {
            DataTable dtSymbol = new DataTable();
            try
            {
                dtSymbol.Columns.Add("Symbol", typeof(string));
                dtSymbol.Columns.Add("Description", typeof(string));
                dtSymbol.Columns.Add("Asset", typeof(string));
                dtSymbol.Columns.Add("Comments", typeof(string));
                dtSymbol.Columns.Add("ISIN", typeof(string));
                dtSymbol.Columns.Add("Sedol", typeof(string));
                dtSymbol.Columns.Add("Cusip", typeof(string));
                dtSymbol.Columns.Add("Currency", typeof(string));
                dtSymbol.Columns.Add("TradedExchange", typeof(string));
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
            return dtSymbol;
        }

        /// <summary>
        /// Function to bind symbology
        /// 
        ///modified by: sachin mishra 30 jan 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        /// 
        private void BindSymbology()
        {
            try
            {
                List<EnumerationValue> listValuesToBind = new List<EnumerationValue>();
                List<EnumerationValue> listValues = EnumHelper.ConvertEnumForBindingWithAssignedValues(typeof(ApplicationConstants.SymbologyCodes));

                foreach (EnumerationValue value in listValues)
                {
                    if (value.Value.Equals((int)ApplicationConstants.SymbologyCodes.TickerSymbol)
                      || value.Value.Equals((int)ApplicationConstants.SymbologyCodes.BloombergSymbol))
                    {
                        listValuesToBind.Add(value);
                    }
                }

                cmbSymbology.DataSource = listValuesToBind;
                cmbSymbology.DisplayMember = "DisplayText";
                cmbSymbology.ValueMember = "Value";
                cmbSymbology.DataBind();
                cmbSymbology.DisplayLayout.Bands[0].Columns["Value"].Hidden = true;
                cmbSymbology.DisplayLayout.Bands[0].ColHeadersVisible = false;
                cmbSymbology.Value = TradingTktPrefs.TTGeneralPrefs.DefaultSymbology;

                int i = 0;
                //Narendra Kumar Jangir 2012/08/17 
                //show tooltip to show Secondary Sort Criteria
                foreach (EnumerationValue symbology in listValuesToBind)
                {
                    cmbSymbology.Rows[i].ToolTipText = symbology.DisplayText;
                    i++;
                }
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
        /// To validate all symbols in grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnValidateSecurity_Click(object sender, EventArgs e)
        {
            try
            {
                if (_securityMaster != null && _securityMaster.IsConnected)
                {
                    if (grdSymbol.Rows.Count > 0)
                    {
                        ultraStatusBar1.Text = "Validating symbols...";
                        SecMasterRequestObj reqObj = new SecMasterRequestObj();
                        ApplicationConstants.SymbologyCodes symbology = ApplicationConstants.SymbologyCodes.TickerSymbol;
                        Enum.TryParse(cmbSymbology.Value.ToString(), out symbology);
                        SecMasterConstants.SearchCriteria searchCriteria = (SecMasterConstants.SearchCriteria)Enum.Parse(typeof(SecMasterConstants.SearchCriteria), cmbSymbology.Value.ToString());

                        foreach (UltraGridRow row in grdSymbol.Rows)
                        {
                            if (!string.IsNullOrEmpty(row.Cells["Symbol"].Value.ToString()))
                            {
                                string input = row.Cells["Symbol"].Value.ToString().Trim().ToUpper();

                                if (searchCriteria == SecMasterConstants.SearchCriteria.BBGID)
                                {
                                    reqObj.AddData(input);
                                }
                                else
                                {
                                    reqObj.AddData(input, (ApplicationConstants.SymbologyCodes)symbology);
                                }
                            }
                        }
                        reqObj.IsSearchInLocalOnly = !CachedDataManager.GetInstance.IsMarketDataPermissionEnabled;
                        reqObj.HashCode = this.GetHashCode();
                        _securityMaster.SendRequest(reqObj);
                    }
                    ultraStatusBar1.Text = string.Empty;
                }
                else
                {
                    ultraStatusBar1.Text = "TradeService not connected";
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

        /// <summary>
        /// Function for change in symbology
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbSymbology_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (grdSymbol.Rows.Count > 0 && _isClearGrid)
                {
                    DialogResult dr = MessageBox.Show("Do you want to clear grid data?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (dr == DialogResult.Yes)
                    {
                        grdSymbol.DataSource = CreateEmptyDataTable();
                    }
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

        private void LiveFeedValidationForm_Load(object sender, EventArgs e)
        {
            try
            {
                SetButtonsColor();
                CustomThemeHelper.SetThemeProperties(sender as Form, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_SYMBOL_LOOKUP);
                if (CustomThemeHelper.ApplyTheme)
                {
                    this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                    this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, this.Text, CustomThemeHelper.UsedFont);
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
                btnValidateSecurity.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnValidateSecurity.ForeColor = System.Drawing.Color.White;
                btnValidateSecurity.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnValidateSecurity.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnValidateSecurity.UseAppStyling = false;
                btnValidateSecurity.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnCheckPrice.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnCheckPrice.ForeColor = System.Drawing.Color.White;
                btnCheckPrice.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnCheckPrice.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnCheckPrice.UseAppStyling = false;
                btnCheckPrice.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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

        public Form Reference()
        {
            return this;
        }


        //public event EventHandler ValidationFormClosed;

        public ISecurityMasterServices SecurityMaster
        {
            set { _securityMaster = value; }
        }


        public void SetUP()
        {

        }

        public event EventHandler PluggableToolsClosed;

        public IPostTradeServices PostTradeServices
        {
            set {; }
        }

        public IPricingAnalysis PricingAnalysis
        {
            set {; }
        }

        private void LiveFeedValidationForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {

                if (PluggableToolsClosed != null)
                {
                    PluggableToolsClosed(this, EventArgs.Empty);
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

        private void LiveFeedValidationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (_securityMaster != null)
                {
                    _securityMaster.SecMstrDataResponse -= new EventHandler<EventArgs<SecMasterBaseObj>>(_securityMaster_SecMstrDataResponse);
                    //new SecMasterDataHandler(_securityMaster_SecMstrDataResponse);
                    _securityMaster.Disconnected -= new EventHandler(_securityMaster_Disconnected);
                    _securityMaster.Connected -= new EventHandler(_securityMaster_Connected);

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

        private void btnCheckPrice_Click(object sender, EventArgs e)
        {
            try
            {
                if (LaunchForm != null)
                {


                    ListEventAargs args = new ListEventAargs();
                    args.listOfValues.Add(ApplicationConstants.CONST_PricingDataLookUp);
                    LaunchForm(this.FindForm(), args);

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

        public event EventHandler LaunchForm;

        private void grdSymbol_BeforeColumnChooserDisplayed(object sender, BeforeColumnChooserDisplayedEventArgs e)
        {

            try
            {
                e.Cancel = true;
                (this.FindForm()).AddCustomColumnChooser(this.grdSymbol);
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

        private void grdSymbol_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            (e.CustomRowFiltersDialog as Form).PaintDynamicForm();

        }
    }
}
