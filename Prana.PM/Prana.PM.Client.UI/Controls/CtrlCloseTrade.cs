using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinGrid.ExcelExport;
using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.PositionManagement;
using Prana.ClientCommon;
using Prana.ClientCommon.BLL;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.Utilities.StringUtilities;
using Prana.Utilities.UI;
using Prana.Utilities.UI.MiscUtilities;
using Prana.Utilities.UI.UIUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Prana.PM.Client.UI.Controls
{
    public partial class CtrlCloseTrade : UserControl
    {
        public event EventHandler SaveLayout;
        public static MonthMarkPriceList _monthMarkPriceList;

        private bool c_blnGridLongLeftMousedown = false;
        private bool c_blnGridShortLeftMousedown = false;
        private string gridSelected = "";
        private bool _hasAccess = true;

        UltraGridBand _gridBandShortPositions = null;
        UltraGridBand _gridBandLongPositions = null;
        UltraGridBand _grdBandNetPositions = null;

        bool _isNetPositionsGridInitialized = false;
        bool _isShortPositionGridInitialized = false;
        bool _isLongPositionGridInitialized = false;

        const string GRIDSHORTPOSITIONNAME = "grdShort";
        const string GRIDLONGPOSITIONNAME = "grdLong";
        const string GRIDNETPOSITIONNAME = "grdNetPosition";
        const string SIDEBUY = "BUY";
        const string SIDEBUYTOCLOSE = "BUY TO CLOSE";
        const string SIDESELL = "SELL";
        const string SIDESELLSHORT = "SELL SHORT";
        const string SIDEBUYTOOPEN = "BUY TO OPEN";
        const string SIDESELLTOOPEN = "SELL TO SHORT";
        //Fill these from closing preferences
        string _currencyColumnFormat = "N4";
        string _quantityColumnFormat = "N8";
        //int _userID;
        //Prana.BusinessObjects.CompanyUser _user;
        // ForexConverter _forexConvertor = null;
        //Forms.CloseTradeMatchedTradeReport _frmCloseTradeMatchedTradeReport = null;

        ErrorTextBox myerror = null;

        ClosingPreferences preferences = null;
        private bool _isCancel = false;
        string _statusMessage = string.Empty;

        List<TradeAuditEntry> _tradeAuditCollection_Closing = new List<TradeAuditEntry>();

        #region Column Captions
        const string CAP_TransactionType = "Transaction Type";
        const string COL_TransactionType = "TransactionType";
        #endregion

        public List<string> DisplayableOpenTaxlotGridColumns
        {
            get
            {
                List<string> TaxlotGridColumns = new List<string>();
                TaxlotGridColumns.Add(ClosingConstants.COL_AllocationID);
                TaxlotGridColumns.Add(ClosingConstants.COL_TradeDate);
                TaxlotGridColumns.Add(ClosingConstants.COL_IsSwap);
                TaxlotGridColumns.Add(ClosingConstants.COL_Side);
                TaxlotGridColumns.Add(ClosingConstants.COL_Symbol);
                TaxlotGridColumns.Add(ClosingConstants.COL_SecurityFullName);
                TaxlotGridColumns.Add(ClosingConstants.COL_OpenQuantity);
                TaxlotGridColumns.Add(ClosingConstants.COL_AssetCategoryValue);
                return TaxlotGridColumns;
            }
        }

        public List<string> AllOpenTaxlotGridColumns
        {
            get
            {
                List<string> TaxlotGridColumns = new List<string>();
                TaxlotGridColumns.Add(ClosingConstants.COL_AllocationID);
                TaxlotGridColumns.Add(ClosingConstants.COL_TradeDate);
                TaxlotGridColumns.Add(ClosingConstants.COL_IsSwap);
                TaxlotGridColumns.Add(ClosingConstants.COL_Side);
                TaxlotGridColumns.Add(ClosingConstants.COL_Symbol);
                TaxlotGridColumns.Add(ClosingConstants.COL_SecurityFullName);
                TaxlotGridColumns.Add(ClosingConstants.COL_OpenQuantity);
                TaxlotGridColumns.Add(ClosingConstants.COL_AssetCategoryValue);
                TaxlotGridColumns.Add(ClosingConstants.COL_AveragePrice);
                TaxlotGridColumns.Add(ClosingConstants.COL_NetNotionalValue);
                TaxlotGridColumns.Add(ClosingConstants.COL_OpenCommission);
                TaxlotGridColumns.Add(ClosingConstants.COL_Underlying);
                TaxlotGridColumns.Add(ClosingConstants.COL_UnitCost);
                TaxlotGridColumns.Add(ClosingConstants.COL_PositionTagValue);
                TaxlotGridColumns.Add(ClosingConstants.COL_StrategyValue);
                TaxlotGridColumns.Add(ClosingConstants.COL_Account);
                TaxlotGridColumns.Add(ClosingConstants.COL_ProcessDate);
                TaxlotGridColumns.Add(ClosingConstants.COL_OriginalPurchaseDate);
                TaxlotGridColumns.Add(ClosingConstants.COL_LotId);
                TaxlotGridColumns.Add(ClosingConstants.COL_ExternalTransId);
                TaxlotGridColumns.Add(ClosingConstants.COL_AUECID);
                TaxlotGridColumns.Add(COL_TransactionType);
                TaxlotGridColumns.Add(OrderFields.PROPERTY_SETTLEMENTCURRENCYID);
                TaxlotGridColumns.Add(OrderFields.PROPERTY_UNITCOSTBASE);
                for (int i = 1; i <= 45; i++)
                {
                    TaxlotGridColumns.Add(OrderFields.PROPERTY_TRADEATTRIBUTE + i);
                }
                return TaxlotGridColumns;
            }
        }

        public List<string> DisplayableNetPositionGridColumns
        {
            get
            {
                List<string> NetPositionGridColumns = new List<string>();
                NetPositionGridColumns.Add(ClosingConstants.COL_ClosingTradeDate);
                NetPositionGridColumns.Add(ClosingConstants.COL_AccountValue);
                NetPositionGridColumns.Add(ClosingConstants.COL_Strategy);
                NetPositionGridColumns.Add(ClosingConstants.COL_PositionTag);
                NetPositionGridColumns.Add(ClosingConstants.COL_AssetCategoryValue);
                NetPositionGridColumns.Add(ClosingConstants.COL_Symbol);
                NetPositionGridColumns.Add(ClosingConstants.COL_ClosedQty);
                NetPositionGridColumns.Add(ClosingConstants.COL_OpenAveragePrice);
                NetPositionGridColumns.Add(ClosingConstants.COL_ClosedAveragePrice);
                NetPositionGridColumns.Add(ClosingConstants.COL_ClosingTotalCommissionandFees);
                NetPositionGridColumns.Add(ClosingConstants.COL_RealizedPNL);
                NetPositionGridColumns.Add(ClosingConstants.COL_Exchange);
                NetPositionGridColumns.Add(ClosingConstants.COL_PositionSide);
                NetPositionGridColumns.Add(ClosingConstants.COL_PositionCommission);
                NetPositionGridColumns.Add(ClosingConstants.COL_ClosingSide);
                NetPositionGridColumns.Add(ClosingConstants.COL_ClosingAlgo);
                return NetPositionGridColumns;
            }
        }

        public List<string> AllNetPositionGridColumns
        {
            get
            {
                List<string> NetPositionGridColumns = new List<string>();
                NetPositionGridColumns.Add(ClosingConstants.COL_ClosingTradeDate);
                NetPositionGridColumns.Add(ClosingConstants.COL_AccountValue);
                NetPositionGridColumns.Add(ClosingConstants.COL_Strategy);
                NetPositionGridColumns.Add(ClosingConstants.COL_PositionTag);
                NetPositionGridColumns.Add(ClosingConstants.COL_AssetCategoryValue);
                NetPositionGridColumns.Add(ClosingConstants.COL_Symbol);
                NetPositionGridColumns.Add(ClosingConstants.COL_ClosedQty);
                NetPositionGridColumns.Add(ClosingConstants.COL_OpenAveragePrice);
                NetPositionGridColumns.Add(ClosingConstants.COL_ClosedAveragePrice);
                NetPositionGridColumns.Add(ClosingConstants.COL_ClosingTotalCommissionandFees);
                NetPositionGridColumns.Add(ClosingConstants.COL_RealizedPNL);
                NetPositionGridColumns.Add(ClosingConstants.COL_Exchange);
                NetPositionGridColumns.Add(ClosingConstants.COL_PositionSide);
                NetPositionGridColumns.Add(ClosingConstants.COL_PositionCommission);
                NetPositionGridColumns.Add(ClosingConstants.COL_ClosingSide);
                NetPositionGridColumns.Add(ClosingConstants.COL_ID);
                NetPositionGridColumns.Add(ClosingConstants.COL_Currency);
                NetPositionGridColumns.Add(ClosingConstants.COL_Underlying);
                NetPositionGridColumns.Add(ClosingConstants.COL_ClosingID);
                NetPositionGridColumns.Add(ClosingConstants.COL_ClosingMode);
                NetPositionGridColumns.Add(ClosingConstants.COL_ClosingTag);
                NetPositionGridColumns.Add(ClosingConstants.COL_TradeDatePosition);
                NetPositionGridColumns.Add(ClosingConstants.COL_NotionalValue);
                NetPositionGridColumns.Add(ClosingConstants.COL_AccountID);
                NetPositionGridColumns.Add(ClosingConstants.COL_IsExpired_Settled);
                NetPositionGridColumns.Add(ClosingConstants.COL_CloseDate);
                NetPositionGridColumns.Add(ClosingConstants.COL_StrategyID);
                NetPositionGridColumns.Add(ClosingConstants.COL_CurrencyID);
                NetPositionGridColumns.Add(ClosingConstants.COL_AUECID);
                NetPositionGridColumns.Add(ClosingConstants.COL_Description);
                NetPositionGridColumns.Add(ClosingConstants.COL_Multiplier);
                NetPositionGridColumns.Add(ClosingConstants.COL_ClosingAlgo);
                for (int i = 1; i <= 45; i++)
                {
                    NetPositionGridColumns.Add(OrderFields.PROPERTY_TRADEATTRIBUTE + i);
                }
                return NetPositionGridColumns;
            }
        }

        public delegate void DisableEnableFormHandler(string message, bool Flag, bool TimerFlag);
        public event EventHandler<EventArgs<string, bool, bool>> DisableEnableParentForm;

        ProxyBase<IClosingServices> _closingServices = null;
        public ProxyBase<IClosingServices> ClosingServices
        {
            set { _closingServices = value; }
        }

        ProxyBase<IAllocationManager> _allocationServices = null;
        public ProxyBase<IAllocationManager> AllocationServices
        {
            set { _allocationServices = value; }
        }

        #region Constructor
        public CtrlCloseTrade()
        {
            if (!(DesignMode || CustomThemeHelper.IsDesignMode()))
            {
                //_user = new Prana.BusinessObjects.CompanyUser();
            }
            InitializeComponent();
        }
        #endregion

        void CtrlCloseTrade_Load(object sender, System.EventArgs e)
        {
            if (!(DesignMode || CustomThemeHelper.IsDesignMode()))
            {
                SetUp();
                SetButtonsColor();
                CustomThemeHelper.SetThemeProperties(sender as Form, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_CLOSE_TRADE);
            }
        }

        private void SetButtonsColor()
        {
            try
            {
                btnRun.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
                btnRun.ForeColor = System.Drawing.Color.White;
                btnRun.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnRun.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnRun.UseAppStyling = false;
                btnRun.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnClearFilter.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnClearFilter.ForeColor = System.Drawing.Color.White;
                btnClearFilter.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnClearFilter.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnClearFilter.UseAppStyling = false;
                btnClearFilter.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                ultraButton1.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                ultraButton1.ForeColor = System.Drawing.Color.White;
                ultraButton1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                ultraButton1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                ultraButton1.UseAppStyling = false;
                ultraButton1.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                ultraButton2.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                ultraButton2.ForeColor = System.Drawing.Color.White;
                ultraButton2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                ultraButton2.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                ultraButton2.UseAppStyling = false;
                ultraButton2.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                ultraButton3.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                ultraButton3.ForeColor = System.Drawing.Color.White;
                ultraButton3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                ultraButton3.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                ultraButton3.UseAppStyling = false;
                ultraButton3.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                ultraButton4.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                ultraButton4.ForeColor = System.Drawing.Color.White;
                ultraButton4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                ultraButton4.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                ultraButton4.UseAppStyling = false;
                ultraButton4.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                ultraButton5.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                ultraButton5.ForeColor = System.Drawing.Color.White;
                ultraButton5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                ultraButton5.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                ultraButton5.UseAppStyling = false;
                ultraButton5.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                ultraButton6.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                ultraButton6.ForeColor = System.Drawing.Color.White;
                ultraButton6.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                ultraButton6.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                ultraButton6.UseAppStyling = false;
                ultraButton6.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                ultraButton7.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                ultraButton7.ForeColor = System.Drawing.Color.White;
                ultraButton7.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                ultraButton7.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                ultraButton7.UseAppStyling = false;
                ultraButton7.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                ultraButton8.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                ultraButton8.ForeColor = System.Drawing.Color.White;
                ultraButton8.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                ultraButton8.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                ultraButton8.UseAppStyling = false;
                ultraButton8.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                ultraButton9.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                ultraButton9.ForeColor = System.Drawing.Color.White;
                ultraButton9.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                ultraButton9.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                ultraButton9.UseAppStyling = false;
                ultraButton9.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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

        private void BindCombos()
        {
            try
            {
                cmbMethodology.DataSource = null;
                List<EnumerationValue> lst = Prana.Utilities.MiscUtilities.EnumHelper.ConvertEnumForBindingWithAssignedValues(typeof(PostTradeEnums.CloseTradeMethodology));
                //add select option at the start of the combo-box items
                lst.Insert(0, new EnumerationValue(ApplicationConstants.C_COMBO_SELECT, -1));
                cmbMethodology.DataSource = lst;
                cmbMethodology.DisplayMember = "DisplayText";
                cmbMethodology.ValueMember = "Value";
                cmbMethodology.Value = -1;
                Utils.UltraComboFilter(cmbMethodology, "DisplayText");

                BindSecondarySortCombo();
                BindClosingFieldCombo();
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

        private void BindSecondarySortCombo()
        {
            try
            {
                List<EnumerationValue> SecondarySortCriteria = Prana.Utilities.MiscUtilities.EnumHelper.ConvertEnumForBindingWithAssignedValues(typeof(PostTradeEnums.SecondarySortCriteria));
                cmbSecondarySort.DataSource = null;
                cmbSecondarySort.DataSource = SecondarySortCriteria;
                cmbSecondarySort.ValueMember = "Value";
                cmbSecondarySort.DisplayMember = "DisplayText";
                cmbSecondarySort.DisplayLayout.Bands[0].Columns["Value"].Hidden = true;
                cmbSecondarySort.Value = 0;
                cmbSecondarySort.DisplayLayout.Bands[0].ColHeadersVisible = false;
                int i = 0;
                //Narendra Kumar Jangir 2012/08/17 
                //show tooltip to show Secondary Sort Criteria
                foreach (EnumerationValue SecondarySortName in SecondarySortCriteria)
                {
                    cmbSecondarySort.Rows[i].ToolTipText = SecondarySortName.DisplayText;
                    i++;
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

        private void BindClosingFieldCombo()
        {
            try
            {
                List<EnumerationValue> ClosingField = Prana.Utilities.MiscUtilities.EnumHelper.ConvertEnumForBindingWithAssignedValues(typeof(PostTradeEnums.ClosingField));

                cmbClosingField.DataSource = null;
                cmbClosingField.DataSource = ClosingField;
                cmbClosingField.ValueMember = "Value";
                cmbClosingField.DisplayMember = "DisplayText";
                cmbClosingField.DisplayLayout.Bands[0].Columns["Value"].Hidden = true;
                cmbClosingField.Value = 0;
                cmbClosingField.DisplayLayout.Bands[0].ColHeadersVisible = false;
                int i = 0;

                //show tooltip to show Closing Field
                foreach (EnumerationValue ClosingFieldName in ClosingField)
                {
                    cmbClosingField.Rows[i].ToolTipText = ClosingFieldName.DisplayText;
                    i++;
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

        #region Private Methods
        /// <summary>
        /// Sets the columns for position grid.
        /// </summary>
        /// <param name="gridBand">The grid band.</param>
        /// <param name="childGridBand">The child grid band.</param>
        /// <param name="grd">The GRD.</param>
        /// <param name="isAccountData">if set to <c>true</c> [is account data].</param>
        private void SetColumnsForPositionGrid(UltraGridBand gridBand)
        {
            try
            {
                Infragistics.Win.UltraWinGrid.ColumnsCollection columns = gridBand.Columns;
                UltraWinGridUtils.SetColumns(DisplayableNetPositionGridColumns, grdNetPosition);
                foreach (UltraGridColumn col in columns)
                {
                    col.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                }
                foreach (string col in AllNetPositionGridColumns)
                {
                    if (columns.Exists(col))
                    {
                        UltraGridColumn column = columns[col];
                        column.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                    }
                }
                int visiblePosition = 1;
                //http://jira.nirvanasolutions.com:8080/browse/PRANA-10508
                UltraGridColumn colClosingTradeDate = gridBand.Columns[ClosingConstants.COL_ClosingTradeDate];
                colClosingTradeDate.Header.Caption = ClosingConstants.CAP_CloseDt;
                colClosingTradeDate.Header.VisiblePosition = visiblePosition++;
                colClosingTradeDate.Width = 80;
                colClosingTradeDate.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DateTimeWithoutDropDown;
                colClosingTradeDate.CellActivation = Activation.NoEdit;

                UltraGridColumn colAccount = gridBand.Columns[ClosingConstants.COL_AccountValue];
                colAccount.Header.Caption = ClosingConstants.CAP_Account;
                colAccount.Header.VisiblePosition = visiblePosition++;
                colAccount.Width = 100;
                colAccount.CellActivation = Activation.NoEdit;

                UltraGridColumn colStrategyIDValue = gridBand.Columns[ClosingConstants.COL_Strategy];
                colStrategyIDValue.Header.Caption = ClosingConstants.CAP_Strategy;
                colStrategyIDValue.Header.VisiblePosition = visiblePosition++;
                colStrategyIDValue.Width = 120;
                colStrategyIDValue.CellActivation = Activation.NoEdit;

                UltraGridColumn colPositionTag = gridBand.Columns[ClosingConstants.COL_PositionTag];
                colPositionTag.Header.Caption = ClosingConstants.CAP_PositionType;
                colPositionTag.Header.VisiblePosition = visiblePosition++;
                colPositionTag.Width = 70;
                colPositionTag.CellActivation = Activation.NoEdit;

                UltraGridColumn colAssetCategoryValue = gridBand.Columns[ClosingConstants.COL_AssetCategoryValue];
                colAssetCategoryValue.Header.Caption = ClosingConstants.CAP_AssetCategory;
                colAssetCategoryValue.Header.VisiblePosition = visiblePosition++;
                colAssetCategoryValue.Width = 60;
                colAssetCategoryValue.CellActivation = Activation.NoEdit;

                UltraGridColumn colSymbol = gridBand.Columns[ClosingConstants.COL_Symbol];
                colSymbol.Header.Caption = ClosingConstants.CAP_Symbol;
                colSymbol.Header.VisiblePosition = visiblePosition++;
                colSymbol.Width = 70;
                colSymbol.CellActivation = Activation.NoEdit;

                UltraGridColumn colClosingQuantity = gridBand.Columns[ClosingConstants.COL_ClosedQty];
                colClosingQuantity.Header.Caption = ClosingConstants.CAP_CloseQty;
                colClosingQuantity.Header.VisiblePosition = visiblePosition++;
                colClosingQuantity.Width = 70;
                colClosingQuantity.Format = _quantityColumnFormat;
                colClosingQuantity.CellActivation = Activation.NoEdit;

                UltraGridColumn colOpenAveragePrice = gridBand.Columns[ClosingConstants.COL_OpenAveragePrice];
                colOpenAveragePrice.Header.Caption = ClosingConstants.CAP_AvgPrice;
                colOpenAveragePrice.Header.VisiblePosition = visiblePosition++;
                colOpenAveragePrice.Width = 75;
                colOpenAveragePrice.Format = _currencyColumnFormat;
                colOpenAveragePrice.CellActivation = Activation.NoEdit;

                UltraGridColumn colClosedAveragePrice = gridBand.Columns[ClosingConstants.COL_ClosedAveragePrice];
                colClosedAveragePrice.Header.Caption = ClosingConstants.CAP_AvgClosingPrice;
                colClosedAveragePrice.Header.VisiblePosition = visiblePosition++;
                colClosedAveragePrice.Width = 70;
                colClosedAveragePrice.Format = _currencyColumnFormat;
                colClosedAveragePrice.CellActivation = Activation.NoEdit;

                UltraGridColumn colCommision = gridBand.Columns[ClosingConstants.COL_ClosingTotalCommissionandFees];
                colCommision.Header.Caption = ClosingConstants.COL_ClosingTotalCommissionandFees;
                colCommision.Header.VisiblePosition = visiblePosition++;
                colCommision.Width = 80;
                colCommision.Format = _currencyColumnFormat;
                colCommision.Hidden = true;
                colCommision.CellActivation = Activation.NoEdit;

                UltraGridColumn colRealizedPNL = gridBand.Columns[ClosingConstants.COL_RealizedPNL];
                colRealizedPNL.Header.Caption = ClosingConstants.CAP_RealizedPNL;
                colRealizedPNL.Header.VisiblePosition = visiblePosition++;
                colRealizedPNL.Width = 90;
                colRealizedPNL.Format = _currencyColumnFormat;
                colRealizedPNL.CellActivation = Activation.NoEdit;

                UltraGridColumn colExchange = gridBand.Columns[ClosingConstants.COL_Exchange];
                colExchange.Header.Caption = ClosingConstants.CAP_Exchange;
                colExchange.Hidden = true;

                UltraGridColumn colSide = gridBand.Columns[ClosingConstants.COL_PositionSide];
                colSide.Header.Caption = ClosingConstants.CAP_OpeningSide;
                colSide.Hidden = true;

                UltraGridColumn colPositionCommision = gridBand.Columns[ClosingConstants.COL_PositionCommission];
                colPositionCommision.Header.Caption = ClosingConstants.CAP_PositionCommission;
                colPositionCommision.Hidden = true;
                colPositionCommision.Format = _currencyColumnFormat;

                UltraGridColumn colClosingSide = gridBand.Columns[ClosingConstants.COL_ClosingSide];
                colClosingSide.Header.Caption = ClosingConstants.CAP_ClosingSide;
                colClosingSide.Hidden = true;

                UltraGridColumn colPositionID = gridBand.Columns[ClosingConstants.COL_ID];
                colPositionID.Header.Caption = ClosingConstants.CAP_TaxlotId;
                colPositionID.Hidden = true;

                UltraGridColumn colCurrency = gridBand.Columns[ClosingConstants.COL_Currency];
                colCurrency.Header.Caption = ClosingConstants.CAP_Currency;
                colCurrency.Hidden = true;
                colCurrency.Format = _currencyColumnFormat;

                UltraGridColumn colUnderlying = gridBand.Columns[ClosingConstants.COL_Underlying];
                colUnderlying.Header.Caption = ClosingConstants.CAP_Underlying;
                colUnderlying.Hidden = true;

                UltraGridColumn colClosingID = gridBand.Columns[ClosingConstants.COL_ClosingID];
                colClosingID.Header.Caption = ClosingConstants.CAP_ClosingID;
                colClosingID.Hidden = true;

                UltraGridColumn colClosingMode = gridBand.Columns[ClosingConstants.COL_ClosingMode];
                colClosingMode.Header.Caption = ClosingConstants.CAP_ClosingMode;
                colClosingMode.Hidden = true;

                UltraGridColumn colClosingTag = gridBand.Columns[ClosingConstants.COL_ClosingTag];
                colClosingTag.Header.Caption = ClosingConstants.CAP_ClosedPositionType;
                colClosingTag.Hidden = true;

                UltraGridColumn colTradeDate = gridBand.Columns[ClosingConstants.COL_TradeDatePosition];
                colTradeDate.Header.Caption = ClosingConstants.CAP_TradeDate;
                colTradeDate.Hidden = true;
                colTradeDate.CellActivation = Activation.NoEdit;

                UltraGridColumn colNotionalValue = gridBand.Columns[ClosingConstants.COL_NotionalValue];
                colNotionalValue.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                colNotionalValue.Hidden = true;
                colNotionalValue.Format = _currencyColumnFormat;

                UltraGridColumn colAccountID = gridBand.Columns[ClosingConstants.COL_AccountID];
                colAccountID.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                colAccountID.Hidden = true;

                UltraGridColumn colSettleExpire = gridBand.Columns[ClosingConstants.COL_IsExpired_Settled];
                colSettleExpire.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                colSettleExpire.Hidden = true;

                UltraGridColumn colTimeOfSaveUTC = gridBand.Columns[ClosingConstants.COL_CloseDate];
                colTimeOfSaveUTC.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                colTimeOfSaveUTC.Hidden = true;

                UltraGridColumn colStrategyID = gridBand.Columns[ClosingConstants.COL_StrategyID];
                colStrategyID.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                colStrategyID.Hidden = true;

                UltraGridColumn colCurrencyID = gridBand.Columns[ClosingConstants.COL_CurrencyID];
                colCurrencyID.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                colCurrencyID.Hidden = true;

                UltraGridColumn ColAUECID = gridBand.Columns[ClosingConstants.COL_AUECID];
                ColAUECID.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                ColAUECID.Hidden = true;

                UltraGridColumn ColDescription = gridBand.Columns[ClosingConstants.COL_Description];
                ColDescription.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                ColDescription.Hidden = true;

                UltraGridColumn ColMultiplier = gridBand.Columns[ClosingConstants.COL_Multiplier];
                ColMultiplier.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                ColMultiplier.Hidden = true;

                //added by amit 24.03.2015
                //http://jira.nirvanasolutions.com:8080/browse/CHMW-3136
                UltraGridColumn colClosingAlgo = gridBand.Columns[ClosingConstants.COL_ClosingAlgo];
                colClosingAlgo.Header.Caption = ClosingConstants.CAP_ClosingAlgo;
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
        /// Grids the initialize layout.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs"/> instance containing the event data.</param>
        private void GridInitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                UltraGrid ug = (UltraGrid)sender;
                e.Layout.Override.SelectTypeCell = SelectType.SingleAutoDrag;
                e.Layout.Override.CellClickAction = CellClickAction.RowSelect;
                ug.AllowDrop = true;
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
        /// mouse down event of the grid.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        private void GridMouseDown(object sender, MouseEventArgs e)
        {
            UltraGrid ug = sender as UltraGrid;
            try
            {
                if (ug == null)
                {
                    return;
                }
                switch (ug.Name)
                {
                    case GRIDSHORTPOSITIONNAME:
                        if (e.Button == MouseButtons.Left)
                        {
                            c_blnGridShortLeftMousedown = true;
                        }
                        gridSelected = "Short";
                        break;
                    case GRIDLONGPOSITIONNAME:
                        if (e.Button == MouseButtons.Left)
                        {
                            c_blnGridLongLeftMousedown = true;
                        }
                        gridSelected = "Long";
                        break;
                }
            }
            catch (Exception ex)
            {
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// mouse move event of the grid.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        private void GridMouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                UltraGrid ug = sender as UltraGrid;
                if (e.Button == MouseButtons.Left)
                {
                    if (_hasAccess)
                    {
                        switch (ug.Name)
                        {
                            case GRIDSHORTPOSITIONNAME:
                                //if left mouse down and mouse move then do drag drop
                                if (c_blnGridShortLeftMousedown == true)
                                    if (GetSelectedData(this.grdShort) != null)
                                    {
                                        this.grdShort.DoDragDrop(GetSelectedData(this.grdShort), DragDropEffects.Copy);
                                    }
                                break;
                            case GRIDLONGPOSITIONNAME:
                                //if left mouse down and mouse move then do drag drop
                                if (c_blnGridLongLeftMousedown == true)
                                    if (GetSelectedData(this.grdLong) != null)
                                    {
                                        this.grdLong.DoDragDrop(GetSelectedData(this.grdLong), DragDropEffects.Copy);
                                    }
                                break;
                        }
                    }
                    else
                    {
                        c_blnGridShortLeftMousedown = false;
                        c_blnGridLongLeftMousedown = false;
                        MessageBox.Show("You do not have permission to close the trades.\nPlease contact Admin", "Permission Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
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
        /// mouse up event on the grid.
        /// </summary>
        /// <param name="sender">The sender.</param>
        private void GridMouseUp(object sender)
        {
            UltraGrid ug = sender as UltraGrid;
            try
            {
                switch (ug.Name)
                {
                    case GRIDSHORTPOSITIONNAME:
                        //turn off mouse down property
                        c_blnGridShortLeftMousedown = false;
                        break;
                    case GRIDLONGPOSITIONNAME:
                        //turn off mouse down property
                        c_blnGridLongLeftMousedown = false;
                        break;
                }
            }
            catch (Exception ex)
            {
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// drag enter event on the grid.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.DragEventArgs"/> instance containing the event data.</param>
        private void GridDragEnter(object sender, DragEventArgs e)
        {
            //on drag enter, turn on copy drag drop effect
            try
            {
                DataObject doDrop = (DataObject)e.Data;
                if (doDrop.GetDataPresent(typeof(TaxLot)) == true)
                    e.Effect = DragDropEffects.Copy;
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
        /// drag over on the grid.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.DragEventArgs"/> instance containing the event data.</param>
        private void GridDragOver(object sender, DragEventArgs e)
        {
            try
            {
                UltraGrid ug = (UltraGrid)sender;

                //retirieve drag drop data
                DataObject doDrop = (DataObject)e.Data;

                //only do this if there is CSV data
                if (doDrop.GetDataPresent(typeof(TaxLot)) == true)
                {
                    //retrieve reference to cell under mouse pointer
                    UIElement uieleMouseOver = ug.DisplayLayout.UIElement.ElementFromPoint(ug.PointToClient(new Point(e.X, e.Y)));
                    ///Need to fetch for row
                    UltraGridRow rowMouseOver = uieleMouseOver.GetContext(typeof(UltraGridRow)) as UltraGridRow;
                    if (rowMouseOver != null)
                    {
                        e.Effect = DragDropEffects.Copy;
                    }
                    else
                    {
                        e.Effect = DragDropEffects.None;
                    }
                }
                else
                {
                    e.Effect = DragDropEffects.None;
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
        /// This method initiate the postion closing manually on dropping.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridDragDrop(object sender, DragEventArgs e)
        {
            try
            {
                //retrieve reference to grid
                UltraGrid ug = (UltraGrid)sender;

                //retrieve reference to cell
                UIElement uieleMouseOver = ug.DisplayLayout.UIElement.ElementFromPoint(ug.PointToClient(new Point(e.X, e.Y)));
                UltraGridRow rowMouseOver = uieleMouseOver.GetContext(typeof(UltraGridRow)) as UltraGridRow;
                TaxLot draggedAllocatedTrade = null;
                if (rowMouseOver != null)
                {
                    TaxLot targetAllocatedTrade = rowMouseOver.ListObject as TaxLot;
                    DataObject doClipboard = (DataObject)e.Data;
                    //test for CSV data available
                    if (doClipboard.GetDataPresent(typeof(TaxLot)) == true)
                    {
                        BinaryFormatter binaryFormatter = new BinaryFormatter();
                        using (MemoryStream msClipBoard = (MemoryStream)doClipboard.GetData(typeof(TaxLot))) //new MemoryStream(bytes,0,Convert.ToInt32(ms.Length)))
                        {
                            msClipBoard.Position = 0;
                            draggedAllocatedTrade = binaryFormatter.Deserialize(msClipBoard) as TaxLot;
                        }
                        if (targetAllocatedTrade != null && draggedAllocatedTrade != null)
                        {
                            ///If dragged allocated trade is equal to dropped allocated trade side (sell and buy are only sides), then return;

                            if (targetAllocatedTrade.OrderSide.Equals(draggedAllocatedTrade.OrderSide))
                            {
                                return;
                            }
                            else if ((targetAllocatedTrade.OrderSide.ToUpperInvariant().Equals(SIDEBUY) && draggedAllocatedTrade.OrderSide.ToUpperInvariant().Equals(SIDEBUYTOCLOSE)) || (targetAllocatedTrade.OrderSide.ToUpperInvariant().Equals(SIDEBUYTOCLOSE) && draggedAllocatedTrade.OrderSide.ToUpperInvariant().Equals(SIDEBUY)))
                            {
                                return;
                            }
                            else if ((targetAllocatedTrade.OrderSide.ToUpperInvariant().Equals(SIDESELLSHORT) && draggedAllocatedTrade.OrderSide.ToUpperInvariant().Equals(SIDESELL)) || (targetAllocatedTrade.OrderSide.ToUpperInvariant().Equals(SIDESELL) && draggedAllocatedTrade.OrderSide.ToUpperInvariant().Equals(SIDESELLSHORT)))
                            {
                                return;
                            }
                            else if ((targetAllocatedTrade.OrderSide.ToUpperInvariant().Equals(SIDEBUY)
                                                && draggedAllocatedTrade.OrderSide.ToUpperInvariant().Equals(SIDEBUYTOOPEN)) ||
                                    (targetAllocatedTrade.OrderSide.ToUpperInvariant().Equals(SIDEBUYTOOPEN) &&
                                                draggedAllocatedTrade.OrderSide.ToUpperInvariant().Equals(SIDEBUY)))
                            {
                                MessageBox.Show("Trade with sides Buy and Buy to Open cannot be closed.", "Close Trade Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                return;
                            }
                            else if ((targetAllocatedTrade.OrderSide.ToUpperInvariant().Equals(SIDEBUYTOOPEN)
                                            && draggedAllocatedTrade.OrderSide.ToUpperInvariant().Equals(SIDEBUYTOCLOSE)) ||
                                (targetAllocatedTrade.OrderSide.ToUpperInvariant().Equals(SIDEBUYTOCLOSE) &&
                                            draggedAllocatedTrade.OrderSide.ToUpperInvariant().Equals(SIDEBUYTOOPEN)))
                            {
                                MessageBox.Show("Trade with sides Buy to Open and Buy to Close cannot be closed.", "Close Trade Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                return;
                            }
                            else if ((targetAllocatedTrade.OrderSide.ToUpperInvariant().Equals(SIDEBUYTOOPEN)
                                            && draggedAllocatedTrade.OrderSide.ToUpperInvariant().Equals(SIDESELLTOOPEN)) ||
                                (targetAllocatedTrade.OrderSide.ToUpperInvariant().Equals(SIDESELLTOOPEN) &&
                                            draggedAllocatedTrade.OrderSide.ToUpperInvariant().Equals(SIDEBUYTOOPEN)))
                            {
                                MessageBox.Show("Trade with side Buy to Open and Sell to Open cannot be closed.", "Close Trade Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                return;
                            }
                            else if (!CheckifTaxlotAreValidForSwapClosing(targetAllocatedTrade, draggedAllocatedTrade))
                            {
                                MessageBox.Show("TaxLot/Position pair to close must belong to same Asset Class.");
                                return;
                            }


                            targetAllocatedTrade.ClosingMode = ClosingMode.None;
                            targetAllocatedTrade.TaxLotClosingId = null;

                            draggedAllocatedTrade.ClosingMode = ClosingMode.None;
                            draggedAllocatedTrade.TaxLotClosingId = null;

                            List<TaxLot> targetAllocatedTrades = new List<TaxLot>();
                            List<TaxLot> draggedAllocatedTrades = new List<TaxLot>();

                            targetAllocatedTrades.Add(targetAllocatedTrade);
                            draggedAllocatedTrades.Add(draggedAllocatedTrade);
                            //Narendra Kumar Jangir, Oct 02 2013
                            //When trade are manually closed using drag and drop than closing algo should be manual
                            bool isAutoCloseStrategy = chkBxIsAutoCloseStrategy.Checked;

                            ClosingParameters closingParams = new ClosingParameters();
                            closingParams.BuyTaxLotsAndPositions = targetAllocatedTrades;
                            closingParams.SellTaxLotsAndPositions = draggedAllocatedTrades;
                            closingParams.Algorithm = PostTradeEnums.CloseTradeAlogrithm.MANUAL;
                            closingParams.IsShortWithBuyAndBuyToCover = ClosingPrefManager.IsShortWithBuyAndBuyToCover;
                            closingParams.IsSellWithBuyToClose = ClosingPrefManager.IsSellWithBuyToClose;
                            closingParams.IsManual = false;
                            closingParams.IsDragDrop = true;
                            closingParams.IsFromServer = false;
                            closingParams.SecondarySort = PostTradeEnums.SecondarySortCriteria.None;
                            closingParams.IsVirtualClosingPopulate = false;
                            closingParams.IsOverrideWithUserClosing = false;
                            closingParams.IsMatchStrategy = !isAutoCloseStrategy;
                            closingParams.ClosingField = PostTradeEnums.ClosingField.Default;
                            closingParams.IsCopyOpeningTradeAttributes = chkCopyOpeningTradeAttributes.Checked;

                            ClosingData closingdata = _closingServices.InnerChannel.AutomaticClosingOnManualOrPresetBasis(closingParams);

                            if (closingdata != null)
                            {
                                if (closingdata.IsNavLockFailed)
                                {
                                    MessageBox.Show(closingdata.ErrorMsg.ToString(), "NAV Lock", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                else if (!closingdata.ErrorMsg.ToString().Equals(string.Empty))
                                {
                                    MessageBox.Show(closingdata.ErrorMsg.ToString(), "Close Trade Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                }
                                else
                                {
                                    if (closingdata.IsDataClosed)
                                    {
                                        AddClosingAuditEntry(closingdata, Prana.BusinessObjects.TradeAuditActionType.ActionType.Closing, TradeAuditActionType.AllocationAuditComments.DataClosed.ToString(), CachedDataManager.GetInstance.LoggedInUser.CompanyUserID);
                                        AuditManager.Instance.SaveAuditList(_tradeAuditCollection_Closing);
                                        _tradeAuditCollection_Closing.Clear();
                                        InformationMessageBox.Display("Close Trade Data Saved");
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
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// We can close swap with other Swap lot and not with Equity
        /// </summary>
        /// <param name="targetAllocatedTrade"></param>
        /// <param name="draggedAllocatedTrade"></param>
        /// <returns></returns>
        private bool CheckifTaxlotAreValidForSwapClosing(TaxLot targetAllocatedTrade, TaxLot draggedAllocatedTrade)
        {
            try
            {
                if (targetAllocatedTrade.AssetID == (int)AssetCategory.Equity && draggedAllocatedTrade.AssetID == (int)AssetCategory.Equity)
                {
                    if (targetAllocatedTrade.ISSwap && draggedAllocatedTrade.ISSwap)
                    {
                        return true;
                    }
                    else if (!targetAllocatedTrade.ISSwap && !draggedAllocatedTrade.ISSwap)
                    {
                        return true;
                    }
                    return false;
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
            return true;
        }

        /// <summary>
        /// Gets the selected data.
        /// </summary>
        /// <param name="ugData">The ug data.</param>
        /// <returns></returns>
        private DataObject GetSelectedData(UltraGrid ugData)
        {
            ///If user wants to drag multiple rows, allow him to do so
            try
            {
                if (ugData != null && ugData.Selected.Rows.Count > 0)
                {
                    //TODO : Make it selected later on
                    UltraGridRow activeRow = ugData.ActiveRow;
                    TaxLot selectedAllocatedTrade = activeRow.ListObject as TaxLot;
                    if (selectedAllocatedTrade == null)
                    {
                        MessageBox.Show("Error in getting the selected data of drag drop.");
                        return null;
                    }
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    //put memory stream into data object as csv format
                    DataObject doClipboard = new DataObject();
                    ///Need to keep the memorystream open until we retrieve the object on the drop,
                    ///hence removed the using statement
                    MemoryStream msData = new MemoryStream();
                    binaryFormatter.Serialize(msData, selectedAllocatedTrade);
                    doClipboard.SetData(typeof(TaxLot), msData);
                    return doClipboard;
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
        /// Populates the close trades interface.
        /// input parameters to be added in some time...
        /// </summary>
        /// <param name="isInternal">if set to <c>true</c> </param>
        public void PopulateCloseTradesInterface()
        {
            try
            {
                //_userID = user.CompanyUserID;
                //this._user = user;
                cmbMethodology.Enabled = true;
                btnClearFilter.Enabled = true;
                cbSyncFilter.Enabled = true;

                if (ClosingPrefManager.DefaultMethodology == PostTradeEnums.CloseTradeMethodology.Manual)
                {
                    grdLong.AllowDrop = true;
                    grdShort.AllowDrop = true;
                }
                else
                {
                    grdLong.AllowDrop = false;
                    grdShort.AllowDrop = false;

                }
                if (preferences != null && preferences.ClosingMethodology.GlobalClosingMethodology == (int)(PostTradeEnums.CloseTradeMethodology.Automatic))
                {
                    cmbAlgorithm.Enabled = true;
                    cmbSecondarySort.Enabled = true;
                    cmbClosingField.Enabled = true;
                }
                //Bhupesh Commented 20-02-2008
                //Not used for now in this Yunzei release.
                // _forexConvertor = ForexConverter.GetInstance(_user.CompanyID);
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
        /// Sets the grid columns.
        /// </summary>
        /// <param name="gridBand">The grid band.</param>
        /// <param name="isAccountData">if set to <c>true</c> [is account data].</param>
        private void SetGridColumns(UltraGrid grid)
        {
            try
            {
                int visiblePosition = 0;
                Infragistics.Win.UltraWinGrid.ColumnsCollection columns = grid.DisplayLayout.Bands[0].Columns;

                UltraGridBand gridBand = grid.DisplayLayout.Bands[0];
                UltraWinGridUtils.SetColumns(DisplayableOpenTaxlotGridColumns, grid);
                foreach (UltraGridColumn col in columns)
                {
                    col.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                }
                foreach (string col in AllOpenTaxlotGridColumns)
                {
                    if (columns.Exists(col))
                    {
                        UltraGridColumn column = columns[col];
                        column.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                    }
                }

                UltraGridColumn colAllocationID = gridBand.Columns[ClosingConstants.COL_AllocationID];
                colAllocationID.Width = 45;
                colAllocationID.Header.Caption = ClosingConstants.CAP_TaxlotId;
                colAllocationID.Header.VisiblePosition = visiblePosition++;

                UltraGridColumn colTradeDate = gridBand.Columns[ClosingConstants.COL_TradeDate];
                colTradeDate.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DateTimeWithoutDropDown;
                colTradeDate.Width = 140;
                colTradeDate.Header.Caption = ClosingConstants.CAP_TradeDate;
                colTradeDate.Header.VisiblePosition = visiblePosition++;
                colTradeDate.CellClickAction = CellClickAction.Default;

                UltraGridColumn colProcessDate = gridBand.Columns[ClosingConstants.COL_ProcessDate];
                colProcessDate.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DateTimeWithoutDropDown;
                colProcessDate.Width = 140;
                colProcessDate.Header.Caption = ClosingConstants.CAP_ProcessDate;
                colProcessDate.Header.VisiblePosition = visiblePosition++;
                colProcessDate.CellClickAction = CellClickAction.Default;

                UltraGridColumn colOriginalPurchaseDate = gridBand.Columns[ClosingConstants.COL_OriginalPurchaseDate];
                colOriginalPurchaseDate.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DateTimeWithoutDropDown;
                colOriginalPurchaseDate.Width = 140;
                colOriginalPurchaseDate.Header.Caption = ClosingConstants.CAP_OriginalPurchaseDate;
                colOriginalPurchaseDate.Header.VisiblePosition = visiblePosition++;
                colOriginalPurchaseDate.CellClickAction = CellClickAction.Default;

                UltraGridColumn ColSide = gridBand.Columns[ClosingConstants.COL_Side];
                ColSide.Width = 65;
                ColSide.Header.Caption = ClosingConstants.CAP_Side;
                ColSide.Header.VisiblePosition = visiblePosition++;

                UltraGridColumn colSymbol = gridBand.Columns[ClosingConstants.COL_Symbol];
                colSymbol.Width = 50;
                colSymbol.Header.Caption = ClosingConstants.CAP_Symbol;
                colSymbol.Header.VisiblePosition = visiblePosition++;

                UltraGridColumn colSecurityFullName = gridBand.Columns[ClosingConstants.COL_SecurityFullName];
                colSecurityFullName.Width = 100;
                colSecurityFullName.Header.Caption = ClosingConstants.CAP_SecurityFullName;
                colSecurityFullName.Header.VisiblePosition = visiblePosition++;

                UltraGridColumn colOpenQuantity = gridBand.Columns[ClosingConstants.COL_OpenQuantity];
                colOpenQuantity.Width = 65;
                colOpenQuantity.Header.Caption = "Quantity";
                colOpenQuantity.Format = _quantityColumnFormat;
                colOpenQuantity.Header.VisiblePosition = visiblePosition++;

                UltraGridColumn colAveragePrice = gridBand.Columns[ClosingConstants.COL_AveragePrice];
                colAveragePrice.Width = 80;
                colAveragePrice.Format = _currencyColumnFormat;
                colAveragePrice.Header.Caption = ClosingConstants.CAP_AvgPrice;
                colAveragePrice.Header.VisiblePosition = visiblePosition++;
                colAveragePrice.Format = _currencyColumnFormat;

                UltraGridColumn colOpenCommision = gridBand.Columns[ClosingConstants.COL_OpenCommission];
                colOpenCommision.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CurrencyNonNegative;
                colOpenCommision.Format = _currencyColumnFormat;
                colOpenCommision.Width = 50;
                colOpenCommision.Header.Caption = ClosingConstants.CAP_Commission;
                colOpenCommision.Header.VisiblePosition = visiblePosition++;

                UltraGridColumn colNetNotionalValue = gridBand.Columns[ClosingConstants.COL_NetNotionalValue];
                colNetNotionalValue.Width = 60;
                colNetNotionalValue.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CurrencyNonNegative;
                colNetNotionalValue.Format = _currencyColumnFormat;
                colNetNotionalValue.Header.Caption = ClosingConstants.CAP_NetNotional;
                colNetNotionalValue.Header.VisiblePosition = visiblePosition++;

                UltraGridColumn colAccount = gridBand.Columns[ClosingConstants.COL_Account];
                colAccount.Width = 80;
                colAccount.Header.Caption = ClosingConstants.CAP_Account;
                colAccount.Header.VisiblePosition = visiblePosition++;

                UltraGridColumn colIsSwapped = gridBand.Columns[ClosingConstants.COL_IsSwap];
                colIsSwapped.Hidden = true;
                colIsSwapped.Header.Caption = ClosingConstants.CAP_IsSwapped;

                UltraGridColumn colStrategyIDValue = gridBand.Columns[ClosingConstants.COL_StrategyValue];
                colStrategyIDValue.Header.Caption = ClosingConstants.CAP_Strategy;
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
        /// 
        /// </summary>
        private void SetUp()
        {
            try
            {
                BindCombos();
                DisableEnableForm(false);
                ClosingPrefManager.IsShortWithBuyAndBuyToCover = chkBoxBuyAndBuyToCover.Checked;
                if (_closingServices != null)
                {
                    preferences = _closingServices.InnerChannel.GetPreferences();
                    if (preferences != null)
                    {
                        cmbSecondarySort.Value = (int)preferences.ClosingMethodology.SecondarySort;
                        cmbMethodology.Value = (int)(preferences.ClosingMethodology.GlobalClosingMethodology);
                        cmbSecondarySort.Value = (int)(preferences.ClosingMethodology.SecondarySort);
                        cmbClosingField.Value = (int)(preferences.ClosingMethodology.ClosingField);
                        cmbAlgorithm.Enabled = false;
                        cmbSecondarySort.Enabled = false;
                        cmbClosingField.Enabled = false;
                        chkBxIsAutoCloseStrategy.Checked = preferences.ClosingMethodology.IsAutoCloseStrategy;
                        _quantityColumnFormat = "N" + preferences.QtyRoundoffDigits.ToString();
                        _currencyColumnFormat = "N" + preferences.PriceRoundOffDigits.ToString();
                        chkCopyOpeningTradeAttributes.Checked = preferences.CopyOpeningTradeAttributes;
                    }
                }

                //Narendra Kumar jangir, May 22 2013
                //When Closing UI is opened both the checkboxes(Can Close SellShort with Buy,Can Close Sell with BuyToCover) should be checked/unchecked on the basis of closing preferences
                chkBoxSellWithBTC.Checked = ClosingPrefManager.GetPreferences().ClosingMethodology.IsSellWithBTC;
                chkBoxBuyAndBuyToCover.Checked = ClosingPrefManager.GetPreferences().ClosingMethodology.IsShortWithBuyandBTC;
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
        /// Sets the allocated trade grids row appearance.
        /// </summary>
        /// <param name="e">The <see cref="Infragistics.Win.UltraWinGrid.InitializeRowEventArgs"/> instance containing the event data.</param>
        /// <param name="isLongAllocatedTradesGrid">if set to <c>true</c> [is long allocated trades grid].</param>
        private void SetGridsRowAppearance(InitializeRowEventArgs e, bool isLongAllocatedTradesGrid, UltraGrid grdName)
        {
            try
            {
                bool isPosition = false;
                string side = string.Empty;
                if (e.Row.Cells[ClosingConstants.COL_IsPosition] != null && e.Row.Cells[ClosingConstants.COL_Side] != null)
                {
                    isPosition = Convert.ToBoolean(e.Row.Cells[ClosingConstants.COL_IsPosition].Text);

                    side = Convert.ToString(e.Row.Cells[ClosingConstants.COL_Side].Text).ToUpperInvariant();
                    if (isLongAllocatedTradesGrid)
                    {
                        if (bool.Equals(isPosition, true))
                        {
                            e.Row.Appearance.ForeColor = Color.Green;
                            e.Row.Appearance.BackColor = Color.Lavender;
                            grdName.DisplayLayout.Bands[0].Override.CellClickAction = CellClickAction.RowSelect;
                        }
                        else
                        {
                            e.Row.Appearance.ForeColor = Color.FromArgb(51, 153, 102);
                            e.Row.Cells[COL_TransactionType].Appearance.ForeColor = Color.FromArgb(51, 153, 102);
                        }
                    }
                    else
                    {
                        if (bool.Equals(isPosition, true))
                        {
                            grdName.DisplayLayout.Bands[0].Override.CellClickAction = CellClickAction.RowSelect;
                        }
                        if (bool.Equals(isPosition, true) || string.Equals(side, SIDESELLSHORT))
                        {
                            e.Row.Appearance.ForeColor = Color.GreenYellow;
                            e.Row.Appearance.BackColor = Color.Chocolate;
                        }
                        else
                        {
                            if (!CustomThemeHelper.ApplyTheme)
                            {
                                e.Row.Appearance.ForeColor = Color.Orange;
                            }
                            else
                            {
                                e.Row.Appearance.ForeColor = System.Drawing.Color.FromArgb(192, 57, 43);
                            }
                        }
                    }
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
        /// Sets the grid data sources. This method was being called on 
        /// </summary>
        internal void SetGridDataSources()
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        MethodInvoker mi = new MethodInvoker(SetGridDataSources);
                        this.BeginInvoke(mi);
                    }
                    else
                    {
                        grdLong.DataSource = ClosingClientSideMapper.OpenTaxlots;
                        grdShort.DataSource = ClosingClientSideMapper.OpenTaxlots;
                        grdNetPosition.DataSource = ClosingClientSideMapper.Netpositions;
                        SetDefaultFilters();
                    }
                }
            }
            catch (Exception ex)
            {
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Sets the default filters.
        /// </summary>
        private void SetDefaultFilters()
        {
            try
            {
                if (grdLong.DisplayLayout.Bands[0].ColumnFilters.Count > 0)
                {
                    grdLong.DisplayLayout.Bands[0].ColumnFilters[ClosingConstants.COL_SideID].LogicalOperator = FilterLogicalOperator.Or;
                    grdLong.DisplayLayout.Bands[0].ColumnFilters[ClosingConstants.COL_SideID].FilterConditions.Add(FilterComparisionOperator.Equals, FIXConstants.SIDE_Buy);
                    grdLong.DisplayLayout.Bands[0].ColumnFilters[ClosingConstants.COL_SideID].FilterConditions.Add(FilterComparisionOperator.Equals, FIXConstants.SIDE_Buy_Closed);
                    grdLong.DisplayLayout.Bands[0].ColumnFilters[ClosingConstants.COL_SideID].FilterConditions.Add(FilterComparisionOperator.Equals, FIXConstants.SIDE_Buy_Open);
                    grdLong.DisplayLayout.Bands[0].ColumnFilters[ClosingConstants.COL_SideID].FilterConditions.Add(FilterComparisionOperator.Equals, FIXConstants.SIDE_Buy_Cover);

                    grdShort.DisplayLayout.Bands[0].ColumnFilters[ClosingConstants.COL_SideID].LogicalOperator = FilterLogicalOperator.Or;
                    grdShort.DisplayLayout.Bands[0].ColumnFilters[ClosingConstants.COL_SideID].FilterConditions.Add(FilterComparisionOperator.Equals, FIXConstants.SIDE_Sell);
                    grdShort.DisplayLayout.Bands[0].ColumnFilters[ClosingConstants.COL_SideID].FilterConditions.Add(FilterComparisionOperator.Equals, FIXConstants.SIDE_SellShort);
                    grdShort.DisplayLayout.Bands[0].ColumnFilters[ClosingConstants.COL_SideID].FilterConditions.Add(FilterComparisionOperator.Equals, FIXConstants.SIDE_Sell_Open);
                    grdShort.DisplayLayout.Bands[0].ColumnFilters[ClosingConstants.COL_SideID].FilterConditions.Add(FilterComparisionOperator.Equals, FIXConstants.SIDE_Sell_Closed);

                    grdLong.DisplayLayout.Bands[0].ColumnFilters[ClosingConstants.COL_AssetCategoryValue].LogicalOperator = FilterLogicalOperator.And;
                    grdLong.DisplayLayout.Bands[0].ColumnFilters[ClosingConstants.COL_AssetCategoryValue].FilterConditions.Add(FilterComparisionOperator.NotEquals, AssetCategory.FX);
                    grdLong.DisplayLayout.Bands[0].ColumnFilters[ClosingConstants.COL_AssetCategoryValue].FilterConditions.Add(FilterComparisionOperator.NotEquals, AssetCategory.FXForward);

                    grdShort.DisplayLayout.Bands[0].ColumnFilters[ClosingConstants.COL_AssetCategoryValue].LogicalOperator = FilterLogicalOperator.And;
                    grdShort.DisplayLayout.Bands[0].ColumnFilters[ClosingConstants.COL_AssetCategoryValue].FilterConditions.Add(FilterComparisionOperator.NotEquals, AssetCategory.FX);
                    grdShort.DisplayLayout.Bands[0].ColumnFilters[ClosingConstants.COL_AssetCategoryValue].FilterConditions.Add(FilterComparisionOperator.NotEquals, AssetCategory.FXForward);
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
        /// Sets the conditional filters.
        /// </summary>
        /// function is not being called anywhere
        //private void SetConditionalFilters()
        //{
        //    grdLong.DisplayLayout.Bands[0].ColumnFilters.ClearAllFilters();
        //    grdShort.DisplayLayout.Bands[0].ColumnFilters.ClearAllFilters();
        //    SetDefaultFilters();
        //}
        #endregion

        #region Button Clicks
        //public event EventHandler GetCloseExpiryData;
        /// <summary>
        /// Handles the Click event of the btnUpdateFilter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        //void btnUpdateFilter_Click(object sender, System.EventArgs e)
        //{
        //    try
        //    {
        //        this.UseWaitCursor = true;
        //        GetCloseExpiryData(this, new EventArgs());
        //        this.UseWaitCursor = false;
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        /// <summary>
        /// Handles the Click event of the btnCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        //private void btnCancel_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        DialogResult result = ConfirmationMessageBox.Display("Click Yes to close the Form");
        //        if (result == DialogResult.Yes)
        //        {
        //            this.FindForm().Close();
        //        }
        //        else
        //        {
        //            GetCloseExpiryData(this, new EventArgs());
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        /// <summary>
        /// Handles the Click event of the btnRun control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnRun_Click(object sender, EventArgs e)
        {
            try
            {
                if (ClosingPrefManager.DefaultMethodology == PostTradeEnums.CloseTradeMethodology.Automatic)
                {
                    // Closing should be applied only on filtered taxlots so first get them based on grid filters
                    // SK 20080823
                    UltraGridRow[] grdLongFilteredRows = grdLong.Rows.GetFilteredInNonGroupByRows();
                    //UltraGridRow[] grdLongFilteredRows1 = grdLong.Rows.GetFilteredInNonGroupByRows();
                    UltraGridRow[] grdShortFilteredRows = grdShort.Rows.GetFilteredInNonGroupByRows();
                    object[] arguments = new object[2];
                    arguments[0] = grdLongFilteredRows;
                    arguments[1] = grdShortFilteredRows;
                    //wingrid performance improve
                    //http://help.infragistics.com/Help/NetAdvantage/WinForms/2012.1/CLR2.0/html/WinGrid_Formatting_and_Appearance_based_Performance_Improvement.html
                    //set displaylayout.maxbanddepth  between 5 to 8
                    SuspendDrawing();
                    DisableEnableForm(false);
                    _statusMessage = "Closing Data Please Wait";

                    //condition to check if it has atleast one listner..
                    if (DisableEnableParentForm != null)
                    {
                        DisableEnableParentForm(this, new EventArgs<string, bool, bool>(_statusMessage, false, true));
                    }
                    RunClosing(arguments);
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
        /// Suspend Drawing
        /// </summary>
        private void SuspendDrawing()
        {
            try
            {
                this.grdLong.Enabled = false;
                SafeNativeMethods.ControlDrawing.SuspendDrawing(grdLong);
                this.grdLong.BeginUpdate();
                this.grdLong.SuspendRowSynchronization();

                this.grdShort.Enabled = false;
                Prana.ClientCommon.SafeNativeMethods.ControlDrawing.SuspendDrawing(grdShort);
                this.grdShort.BeginUpdate();
                this.grdShort.SuspendRowSynchronization();

                this.grdNetPosition.Enabled = false;
                Prana.ClientCommon.SafeNativeMethods.ControlDrawing.SuspendDrawing(grdNetPosition);
                this.grdNetPosition.BeginUpdate();
                this.grdNetPosition.SuspendRowSynchronization();
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
        /// Handles the Click event of the btnViewReport control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        //private void btnViewReport_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (_frmCloseTradeMatchedTradeReport == null)
        //        {
        //            _frmCloseTradeMatchedTradeReport = new Prana.PM.Client.UI.Forms.CloseTradeMatchedTradeReport();
        //        }

        //        _frmCloseTradeMatchedTradeReport.ShowDialog();
        //        _frmCloseTradeMatchedTradeReport.Disposed += new EventHandler(_frmCloseTradeMatchedTradeReport_Disposed);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        /// <summary>
        /// Handles the Click event of the btnSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// 
        //private void SaveData(ClosingData ClosedData)
        //{
        //    int numberOfRowsEffected;
        //    try
        //    {
        //        this.UseWaitCursor = true;
        //        numberOfRowsEffected = _closingServices.InnerChannel.SaveCloseTradesData(ClosedData);
        //        if (!int.Equals(numberOfRowsEffected, 0))
        //        {
        //            InformationMessageBox.Display("Close Trade Data Saved");
        //        }
        //        else
        //        {
        //            InformationMessageBox.Display("Nothing to Save");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //    finally
        //    {
        //        this.UseWaitCursor = false;
        //    }
        //}

        /// <summary>
        /// Brings up the UI to create a new position and add to this UI.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        //private void btnAddPosition_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (frmcreatePosition == null)
        //        {
        //            frmcreatePosition = new Prana.PM.Client.UI.Forms.CreatePosition(_user, true);
        //            frmcreatePosition.Owner = this.FindForm();
        //            frmcreatePosition.ShowInTaskbar = false;
        //        }
        //        frmcreatePosition.Show();
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}
        #endregion

        #region Other events
        /// <summary>
        /// Handles the Disposed event of the _frmPopUpMessage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        //void _frmPopUpMessage_Disposed(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        _frmCloseTradeMatchedTradeReport = null;
        //    }
        //    catch (Exception ex)
        //    {
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        /// <summary>
        /// Handles the Disposed event of the _frmCloseTradeMatchedTradeReport control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        //void _frmCloseTradeMatchedTradeReport_Disposed(object sender, EventArgs e)
        //{
        //    _frmCloseTradeMatchedTradeReport = null;
        //}

        /// <summary>
        /// Handles the ExpandedStateChanged event of the grdBoxLongPosition control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void grdBoxLongPosition_ExpandedStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (grpLongPosition.Expanded)
                {
                    if (grpShortPosition.Expanded)
                        splitContainer6.SplitterDistance = this.FindForm().Width / 2;
                    else
                        splitContainer6.SplitterDistance = this.FindForm().Width - 25;
                }
                else
                {
                    if (grpShortPosition.Expanded)
                        splitContainer6.SplitterDistance = 25;
                    else
                        splitContainer6.SplitterDistance = this.FindForm().Width / 2;
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
        /// Handles the ExpandedStateChanged event of the grdBoxShortPosition control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void grdBoxShortPosition_ExpandedStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (grpShortPosition.Expanded)
                {
                    if (grpLongPosition.Expanded)
                        splitContainer6.SplitterDistance = this.FindForm().Width / 2;
                    else
                        splitContainer6.SplitterDistance = 25;
                }
                else
                {
                    splitContainer6.SplitterDistance = this.FindForm().Width - 25;
                }
            }
            catch (Exception ex)
            {
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Handles the ExpandedStateChanged event of the grpBoxAllocatedTradesAndOpenPositions control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void grpBoxNetPosition_ExpandedStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (grpBoxNetPosition.Expanded)
                {
                    splitContainer2.SplitterDistance = 250;
                }
                else
                {
                    splitContainer2.SplitterDistance = splitContainer2.Height - 150;
                }
            }
            catch (Exception ex)
            {

                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        #endregion

        #region Grid Events

        #region Account Net position grid events
        /// <summary>
        /// Handles the InitializeLayout event of the grdNetPosition control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs"/> instance containing the event data.</param>

        /// <summary>
        /// Handles the MouseDown event of the grdNetPosition control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        private void grdNetPosition_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    Point mousepoint = new Point(e.X, e.Y);
                    UIElement element = ((UltraGrid)sender).DisplayLayout.UIElement.ElementFromPoint(mousepoint);
                    if (element == null)
                    {
                        grdNetPosition.ActiveRow = null;
                    }
                    UltraGridCell cell = element.GetContext(typeof(UltraGridCell)) as UltraGridCell;
                    if (cell != null)
                    {
                        cell.Row.Activate();
                    }
                    else
                    {
                        grdNetPosition.ActiveRow = null;
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
        #endregion

        #region Account Short position grid events
        /// <summary>
        /// Handles the InitializeLayout event of the grdEligiblePositions control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs"/> instance containing the event data.</param>
        private void grdShort_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                if (grdShort.DisplayLayout.Bands[0].Columns.Exists(ClosingConstants.COL_BloombergExCode))
                {
                    grdShort.DisplayLayout.Bands[0].Columns[ClosingConstants.COL_BloombergExCode].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                }
                if (bool.Equals(_isShortPositionGridInitialized, false))
                {
                    // Get Column Chooser for the grid
                    e.Layout.Override.RowSelectors = DefaultableBoolean.True;
                    e.Layout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;
                    UltraGridLayout gridLayout = grdShort.DisplayLayout;
                    gridLayout.Override.RowAppearance.BackColor = Color.FromArgb(64, 64, 64);
                    GridInitializeLayout(sender, e);
                    _gridBandShortPositions = grdShort.DisplayLayout.Bands[0];
                    // if grid layout is saved by the user
                    if (ClosingPrefManager.ClosingLayout.ShortPositionColumns.Count > 0)
                    {
                        List<ColumnData> listShortColData = ClosingPrefManager.ClosingLayout.ShortPositionColumns;
                        ClosingPrefManager.SetGridColumnLayout(grdShort, listShortColData);
                        foreach (string col in AllOpenTaxlotGridColumns)
                        {
                            if (_gridBandShortPositions.Columns.Exists(col))
                            {
                                UltraGridColumn column = _gridBandShortPositions.Columns[col];
                                column.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                            }
                        }
                    }
                    else
                    {
                        SetGridColumns(grdShort);
                    }
                    _isShortPositionGridInitialized = true;
                    SetCaptionForGridColumns(grdShort);
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

        private void SetCaptionForGridColumns(UltraGrid grd)
        {
            try
            {
                UltraGridBand gridBand = grd.DisplayLayout.Bands[0];

                UltraGridColumn colAllocationID = gridBand.Columns[ClosingConstants.COL_AllocationID];
                colAllocationID.Header.Caption = ClosingConstants.CAP_TaxlotId;

                UltraGridColumn colTradeDate = gridBand.Columns[ClosingConstants.COL_TradeDate]; ;
                colTradeDate.Header.Caption = ClosingConstants.CAP_TradeDate;

                UltraGridColumn colProcessDate = gridBand.Columns[ClosingConstants.COL_ProcessDate];
                colProcessDate.Header.Caption = ClosingConstants.CAP_ProcessDate;

                UltraGridColumn colOriginalPurchaseDate = gridBand.Columns[ClosingConstants.COL_OriginalPurchaseDate];
                colOriginalPurchaseDate.Header.Caption = ClosingConstants.CAP_OriginalPurchaseDate;

                UltraGridColumn ColSide = gridBand.Columns[ClosingConstants.COL_Side];
                ColSide.Header.Caption = ClosingConstants.CAP_Side;

                UltraGridColumn colSymbol = gridBand.Columns[ClosingConstants.COL_Symbol];
                colSymbol.Header.Caption = ClosingConstants.CAP_Symbol;

                UltraGridColumn colSecurityFullName = gridBand.Columns[ClosingConstants.COL_SecurityFullName];
                colSecurityFullName.Header.Caption = ClosingConstants.CAP_SecurityFullName;

                UltraGridColumn colOpenQuantity = gridBand.Columns[ClosingConstants.COL_OpenQuantity];
                colOpenQuantity.Header.Caption = "Quantity";
                colOpenQuantity.Format = _quantityColumnFormat;

                UltraGridColumn colAveragePriceBase = gridBand.Columns[ClosingConstants.CAP_AvgPriceBase];
                colAveragePriceBase.Header.Caption = ClosingConstants.CAP_AvgPriceBase;
                colAveragePriceBase.Format = _currencyColumnFormat;

                UltraGridColumn colUnitCost = gridBand.Columns[ClosingConstants.COL_UnitCost];
                colUnitCost.Format = _quantityColumnFormat;

                UltraGridColumn colAveragePrice = gridBand.Columns[ClosingConstants.COL_AveragePrice];
                colAveragePrice.Header.Caption = ClosingConstants.CAP_AvgPrice;
                colAveragePrice.Format = _currencyColumnFormat;

                UltraGridColumn colOpenCommision = gridBand.Columns[ClosingConstants.COL_OpenCommission];
                colOpenCommision.Header.Caption = ClosingConstants.CAP_Commission;
                colOpenCommision.Format = _currencyColumnFormat;

                UltraGridColumn colNetNotionalValue = gridBand.Columns[ClosingConstants.COL_NetNotionalValue];
                colNetNotionalValue.Header.Caption = ClosingConstants.CAP_NetNotional;
                colNetNotionalValue.Format = _currencyColumnFormat;

                UltraGridColumn colAccount = gridBand.Columns[ClosingConstants.COL_Account];
                colAccount.Header.Caption = ClosingConstants.CAP_Account;

                UltraGridColumn colIsSwapped = gridBand.Columns[ClosingConstants.COL_IsSwap];
                colIsSwapped.Header.Caption = ClosingConstants.CAP_IsSwapped;

                UltraGridColumn colStrategyIDValue = gridBand.Columns[ClosingConstants.COL_StrategyValue];
                colStrategyIDValue.Header.Caption = ClosingConstants.CAP_Strategy;
                // Added By : Manvendra P.
                // Jira : http://jira.nirvanasolutions.com:8080/browse/CHMW-3665
                UltraGridColumn colOptionPremiumAdjustment = gridBand.Columns["OptionPremiumAdjustment"];
                colOptionPremiumAdjustment.Hidden = true;
                colOptionPremiumAdjustment.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                colOptionPremiumAdjustment.Format = _currencyColumnFormat;

                //Transaction type on Closing UI is not looking good
                //http://jira.nirvanasolutions.com:8080/browse/PRANA-5499

                UltraGridColumn colTransactionType = gridBand.Columns[COL_TransactionType];
                colTransactionType.Header.Caption = CAP_TransactionType;
                colTransactionType.ValueList = ValueListHelper.GetInstance.GetTransactionTypeValueList().Clone();
                colTransactionType.CellActivation = Activation.NoEdit;

                Dictionary<int, string> dictCurrencies = CachedDataManager.GetInstance.GetAllCurrencies();
                ValueList currencies = new ValueList();
                foreach (KeyValuePair<int, string> item in dictCurrencies)
                {
                    currencies.ValueListItems.Add(item.Key, item.Value);
                }
                currencies.ValueListItems.Add(0, ApplicationConstants.C_COMBO_NONE);
                UltraGridColumn colSETTLEMENTCURRENCY = gridBand.Columns[OrderFields.PROPERTY_SETTLEMENTCURRENCYID];
                colSETTLEMENTCURRENCY.Header.Caption = OrderFields.CAPTION_SETTLEMENT_CURRENCY;
                colSETTLEMENTCURRENCY.ValueList = currencies;
                colSETTLEMENTCURRENCY.CellActivation = Activation.NoEdit;

                UltraGridColumn colUNITCOSTBASE = gridBand.Columns[OrderFields.PROPERTY_UNITCOSTBASE];
                colUNITCOSTBASE.Header.Caption = OrderFields.CAPTION_UNITCOSEBASE;
                colUNITCOSTBASE.Format = _currencyColumnFormat;

                UltraGridColumn colAssetName = gridBand.Columns[ClosingConstants.COL_AssetName];
                colAssetName.Header.Caption = ClosingConstants.CAP_AssetCategory;
                colAssetName.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                UltraGridColumn colAssetCategoryValue = gridBand.Columns[ClosingConstants.COL_AssetCategoryValue];
                colAssetCategoryValue.Hidden = true;
                colAssetCategoryValue.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                for (int i = 1; i <= 45; i++)
                {
                    UltraGridColumn tradeAttributeCol = gridBand.Columns[OrderFields.PROPERTY_TRADEATTRIBUTE + i];
                    tradeAttributeCol.Header.Caption = CachedDataManager.GetInstance.GetAttributeNameForValue(ClosingConstants.CAPTION_TRADEATTRIBUTE + i);
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
        /// Handles the MouseDown event of the grdShort control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        void grdShort_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            try
            {
                GridMouseDown(sender, e);
            }
            catch (Exception ex)
            {
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Handles the MouseUp event of the grdShort control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        void grdShort_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            try
            {
                c_blnGridShortLeftMousedown = false;
                GridMouseUp(sender);
            }
            catch (Exception ex)
            {
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Handles the MouseMove event of the grdShort control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        void grdShort_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            try
            {
                UIElement elementEntered = this.grdShort.DisplayLayout.UIElement.LastElementEntered;
                if (elementEntered == null) return;
                if ((elementEntered is RowUIElement) || (elementEntered.GetAncestor(typeof(RowUIElement)) != null))
                    GridMouseMove(sender, e);
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
        /// Handles the DragOver event of the grdShort control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.DragEventArgs"/> instance containing the event data.</param>
        void grdShort_DragOver(object sender, System.Windows.Forms.DragEventArgs e)
        {
            try
            {
                if ((PostTradeEnums.CloseTradeMethodology)cmbMethodology.Value == PostTradeEnums.CloseTradeMethodology.Manual)
                {
                    GridDragOver(sender, e);
                }
            }
            catch (Exception ex)
            {
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Handles the DragEnter event of the grdShort control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.DragEventArgs"/> instance containing the event data.</param>
        void grdShort_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            try
            {
                if ((PostTradeEnums.CloseTradeMethodology)cmbMethodology.Value == PostTradeEnums.CloseTradeMethodology.Manual)
                {
                    GridDragEnter(sender, e);
                }
            }
            catch (Exception ex)
            {
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Handles the DragDrop event of the grdShort control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.DragEventArgs"/> instance containing the event data.</param>
        void grdShort_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            try
            {
                if ((PostTradeEnums.CloseTradeMethodology)cmbMethodology.Value == PostTradeEnums.CloseTradeMethodology.Manual)
                {
                    GridDragDrop(sender, e);
                }
            }
            catch (Exception ex)
            {
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Handles the InitializeRow event of the grdShort control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Infragistics.Win.UltraWinGrid.InitializeRowEventArgs"/> instance containing the event data.</param>
        private void grdShort_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                SetGridsRowAppearance(e, false, grdShort);
            }
            catch (Exception ex)
            {
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        void grdShort_AfterRowFilterChanged(object sender, Infragistics.Win.UltraWinGrid.AfterRowFilterChangedEventArgs e)
        {
            try
            {
                if ((e.Column.Key.Equals(ClosingConstants.COL_TradeDate) || e.Column.Key.Equals(ClosingConstants.COL_ProcessDate)) && e.NewColumnFilter.FilterConditions != null && e.NewColumnFilter.FilterConditions.Count == 1 && e.NewColumnFilter.FilterConditions[0].CompareValue.Equals("(Today)"))
                {
                    grdShort.DisplayLayout.Bands[0].ColumnFilters[e.Column.Key].FilterConditions.Clear();
                    grdShort.DisplayLayout.Bands[0].ColumnFilters[e.Column.Key].FilterConditions.Add(FilterComparisionOperator.StartsWith, DateTime.Now.Date.ToString(DateTimeConstants.DateformatForClosing));
                }
                if (cbSyncFilter.Checked)
                {
                    grdLong.DisplayLayout.Bands[0].ColumnFilters[e.Column.Key].ClearFilterConditions();
                    foreach (FilterCondition fc in e.NewColumnFilter.FilterConditions)
                    {
                        grdLong.DisplayLayout.Bands[0].ColumnFilters[e.Column.Key].FilterConditions.Add((FilterCondition)fc.Clone());
                    }
                }
            }
            catch (Exception ex)
            {
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        #endregion

        #region Account Long position grid events
        /// <summary>
        /// Handles the InitializeLayout event of the grdTodaysAllocatedTrades control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs"/> instance containing the event data.</param>
        void grdLong_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                if (grdLong.DisplayLayout.Bands[0].Columns.Exists(ClosingConstants.COL_BloombergExCode))
                {
                    grdLong.DisplayLayout.Bands[0].Columns[ClosingConstants.COL_BloombergExCode].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                }
                if (bool.Equals(_isLongPositionGridInitialized, false))
                {
                    e.Layout.Override.RowSelectors = DefaultableBoolean.True;
                    e.Layout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;
                    //UltraGridLayout gridLayout = grdLong.DisplayLayout;
                    grdLong.DisplayLayout.Override.RowAppearance.BackColor = Color.Black;
                    GridInitializeLayout(sender, e);
                    _gridBandLongPositions = grdLong.DisplayLayout.Bands[0];

                    // if grid layout is saved by the user
                    if (ClosingPrefManager.ClosingLayout.LongPositionColumns.Count > 0)
                    {
                        List<ColumnData> listLongColData = ClosingPrefManager.ClosingLayout.LongPositionColumns;
                        ClosingPrefManager.SetGridColumnLayout(grdLong, listLongColData);
                        foreach (string col in AllOpenTaxlotGridColumns)
                        {
                            if (_gridBandLongPositions.Columns.Exists(col))
                            {
                                UltraGridColumn column = _gridBandLongPositions.Columns[col];
                                column.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                            }
                        }
                    }
                    else
                    {
                        SetGridColumns(grdLong);
                    }
                    SetCaptionForGridColumns(grdLong);
                    _isLongPositionGridInitialized = true;
                }
            }
            catch (Exception ex)
            {
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Handles the DragOver event of the grdLong control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.DragEventArgs"/> instance containing the event data.</param>
        //void grdLong_DragOver(object sender, System.Windows.Forms.DragEventArgs e)
        //{
        //    try
        //    {
        //        if ((PostTradeEnums.CloseTradeMethodology)cmbMethodology.Value == PostTradeEnums.CloseTradeMethodology.Manual)
        //        {
        //            GridDragOver(sender, e);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        /// <summary>
        /// Handles the DragEnter event of the grdLong control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.DragEventArgs"/> instance containing the event data.</param>
        void grdLong_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            try
            {
                if ((PostTradeEnums.CloseTradeMethodology)cmbMethodology.Value == PostTradeEnums.CloseTradeMethodology.Manual)
                {
                    GridDragEnter(sender, e);
                }
            }
            catch (Exception ex)
            {
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Handles the DragDrop event of the grdLong control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.DragEventArgs"/> instance containing the event data.</param>
        void grdLong_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            try
            {
                if ((PostTradeEnums.CloseTradeMethodology)cmbMethodology.Value == PostTradeEnums.CloseTradeMethodology.Manual)
                {
                    GridDragDrop(sender, e);
                }
            }
            catch (Exception ex)
            {
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Handles the MouseMove event of the grdLong control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        void grdLong_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            UIElement elementEntered = this.grdLong.DisplayLayout.UIElement.LastElementEntered;
            try
            {
                if (elementEntered == null) return;

                if ((elementEntered is RowUIElement) || (elementEntered.GetAncestor(typeof(RowUIElement)) != null))
                    GridMouseMove(sender, e);
            }
            catch (Exception ex)
            {
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Handles the MouseUp event of the grdLong control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        void grdLong_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            try
            {
                c_blnGridLongLeftMousedown = false;
                GridMouseUp(sender);
            }
            catch (Exception ex)
            {
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Handles the MouseDown event of the grdLong control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        void grdLong_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            try
            {
                GridMouseDown(sender, e);
            }
            catch (Exception ex)
            {
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Handles the InitializeRow event of the grdLong control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Infragistics.Win.UltraWinGrid.InitializeRowEventArgs"/> instance containing the event data.</param>
        private void grdLong_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                SetGridsRowAppearance(e, true, grdLong);
            }
            catch (Exception ex)
            {
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        void grdLong_AfterRowFilterChanged(object sender, Infragistics.Win.UltraWinGrid.AfterRowFilterChangedEventArgs e)
        {
            try
            {
                if ((e.Column.Key.Equals(ClosingConstants.COL_TradeDate) || e.Column.Key.Equals(ClosingConstants.COL_ProcessDate)) && e.NewColumnFilter.FilterConditions != null && e.NewColumnFilter.FilterConditions.Count == 1 && e.NewColumnFilter.FilterConditions[0].CompareValue.Equals("(Today)"))
                {
                    grdLong.DisplayLayout.Bands[0].ColumnFilters[e.Column.Key].FilterConditions.Clear();
                    grdLong.DisplayLayout.Bands[0].ColumnFilters[e.Column.Key].FilterConditions.Add(FilterComparisionOperator.StartsWith, DateTime.Now.Date.ToString(DateTimeConstants.DateformatForClosing));
                }
                if (cbSyncFilter.Checked)
                {
                    grdShort.DisplayLayout.Bands[0].ColumnFilters[e.Column.Key].ClearFilterConditions();

                    foreach (FilterCondition fc in e.NewColumnFilter.FilterConditions)
                    {
                        grdShort.DisplayLayout.Bands[0].ColumnFilters[e.Column.Key].FilterConditions.Add((FilterCondition)fc.Clone());
                    }
                }
            }
            catch (Exception ex)
            {
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        void grdNetPosition_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
        {
            try
            {
                if ((ClosingMode)e.Row.Cells[ClosingConstants.COL_ClosingMode].Value == ClosingMode.CorporateAction)
                {
                    switch ((PositionTag)e.Row.Cells[ClosingConstants.COL_PositionTag].Value)
                    {
                        case PositionTag.Long:
                        case PositionTag.Short:
                            {
                                switch ((PositionTag)e.Row.Cells[ClosingConstants.COL_ClosingTag].Value)
                                {
                                    case PositionTag.LongWithdrawal:
                                    case PositionTag.ShortWithdrawal:
                                        {
                                            e.Row.Hidden = true;
                                            break;
                                        }
                                    default:
                                        e.Row.Hidden = true;
                                        e.Row.Cells["checkBox"].Value = false;
                                        break;
                                }
                                break;
                            }
                        default:
                            e.Row.Hidden = true;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        #endregion
        #endregion

        #region Unwinding Click functionality
        //private bool ArePositionElligibleToUnwind(string taxlotID, ClosingMode settlementmode)
        //{
        //    bool bValidated = true;
        //    StringBuilder alreadyClosedError = new StringBuilder();
        //    Dictionary<string, DateTime> taxlotIdToCloseDateDict = null;
        //    try
        //    {
        //        if (taxlotIdToCloseDateDict.ContainsKey(taxlotID) && (settlementmode.Equals(ClosingMode.Physical) || settlementmode.Equals(ClosingMode.Exercise)))
        //        {
        //            alreadyClosedError.Append("TaxlotId : ");
        //            alreadyClosedError.Append(taxlotID);
        //            alreadyClosedError.Append(" is closed on AUEC date : ");
        //            alreadyClosedError.Append(taxlotIdToCloseDateDict[taxlotID]);
        //            alreadyClosedError.Append(".");
        //            alreadyClosedError.Append(Environment.NewLine);
        //            alreadyClosedError.Append("Please Unclosed taxlots before attempting to Unwind.");
        //            InformationMessageBox.Display(alreadyClosedError.ToString());
        //            bValidated = false; ;
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
        //    return bValidated;
        //}
        #endregion

        private void unwindToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                grdNetPosition.UpdateData();
                UltraGridRow[] filteredRows = grdNetPosition.Rows.GetFilteredInNonGroupByRows();
                //Disable unwind menu for cost adjusted trades: http://jira.nirvanasolutions.com:8080/browse/PRANA-6869
                foreach (UltraGridRow row in filteredRows)
                {
                    if (Convert.ToBoolean(row.Cells["checkBox"].Value.ToString()))
                    {
                        Position selectedPos = (Position)row.ListObject;
                        if (selectedPos.ClosingMode == ClosingMode.CorporateAction || selectedPos.ClosingMode == ClosingMode.CostBasisAdjustment)
                        {
                            MessageBox.Show(this, "Cannot unwind data for " + StringUtilities.SplitCamelCase(selectedPos.ClosingMode.ToString()) + " taxlots.", "Warning!", MessageBoxButtons.OK);
                            return;
                        }
                    }
                }
                object[] arguments = new object[1];
                arguments[0] = filteredRows;
                //wingrid performance increase
                //http://help.infragistics.com/Help/NetAdvantage/WinForms/2012.1/CLR2.0/html/WinGrid_Formatting_and_Appearance_based_Performance_Improvement.html
                DisableEnableForm(false);
                SuspendDrawing();
                //condition to check if it has at least one listener.
                if (DisableEnableParentForm != null)
                {
                    _statusMessage = string.Empty;
                    DisableEnableParentForm(this, new EventArgs<string, bool, bool>(_statusMessage, false, false));
                }
                UnwindClosing(arguments);
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

        private void grdNetPosition_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            e.Layout.Override.RowSelectors = DefaultableBoolean.True;
            e.Layout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;
            e.Layout.Override.CellClickAction = CellClickAction.Edit;
            try
            {
                if (bool.Equals(_isNetPositionsGridInitialized, false))
                {
                    UltraGridLayout gridLayout = grdNetPosition.DisplayLayout;
                    grdNetPosition.DisplayLayout.Override.AllowGroupBy = DefaultableBoolean.True;
                    // if grid layout is saved by the user
                    if (ClosingPrefManager.ClosingLayout.NetPositionColumns.Count > 0)
                    {
                        List<ColumnData> listNetColData = ClosingPrefManager.ClosingLayout.NetPositionColumns;
                        ClosingPrefManager.SetGridColumnLayout(grdNetPosition, listNetColData);
                        foreach (string col in AllNetPositionGridColumns)
                        {
                            if (gridLayout.Bands[0].Columns.Exists(col))
                            {
                                UltraGridColumn column = gridLayout.Bands[0].Columns[col];
                                column.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                            }
                        }
                    }
                    else
                    {
                        //load default layout
                        _grdBandNetPositions = grdNetPosition.DisplayLayout.Bands[0];
                        SetColumnsForPositionGrid(_grdBandNetPositions);
                    }
                    SetCaptionAndFormatForColumns(grdNetPosition.DisplayLayout.Bands[0]);
                    if (!gridLayout.Bands[0].Columns.Exists("checkBox"))
                    {
                        AddCheckBoxinGrid(grdNetPosition, headerCheckBoxUnWind);
                    }

                    //added by amit 24.03.2015
                    //http://jira.nirvanasolutions.com:8080/browse/CHMW-3136
                    if (gridLayout.Bands[0].Columns.Exists(ClosingConstants.COL_ClosingAlgo))
                    {
                        UltraGridColumn colClosingAlgo = gridLayout.Bands[0].Columns[ClosingConstants.COL_ClosingAlgo];
                        List<EnumerationValue> closingAlgoType = Prana.Utilities.MiscUtilities.EnumHelper.ConvertEnumForBindingWithSelectValue(typeof(PostTradeEnums.CloseTradeAlogrithm));
                        ValueList closingAlgoValueList = new ValueList();
                        foreach (EnumerationValue value in closingAlgoType)
                        {
                            closingAlgoValueList.ValueListItems.Add(value.Value, value.DisplayText);
                        }
                        colClosingAlgo.ValueList = closingAlgoValueList;
                    }

                    if (gridLayout.Bands[0].Columns.Exists(ClosingConstants.COL_AssetCategoryValue))
                    {
                        UltraGridColumn colAssetClass = gridLayout.Bands[0].Columns[ClosingConstants.COL_AssetCategoryValue];
                        List<EnumerationValue> assetCategoryType = Prana.Utilities.MiscUtilities.EnumHelper.ConvertEnumForBindingWithSelectValue(typeof(Prana.BusinessObjects.AppConstants.AssetCategory));
                        ValueList assetClassValueList = new ValueList();
                        foreach (EnumerationValue value in assetCategoryType)
                        {
                            assetClassValueList.ValueListItems.Add(value.Value, value.DisplayText);
                        }
                        colAssetClass.ValueList = assetClassValueList;
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

        private void SetCaptionAndFormatForColumns(UltraGridBand gridBand)
        {
            try
            {
                UltraGridColumn colClosingTradeDate = gridBand.Columns[ClosingConstants.COL_ClosingTradeDate];
                colClosingTradeDate.Header.Caption = ClosingConstants.CAP_CloseDt;

                UltraGridColumn colAccount = gridBand.Columns[ClosingConstants.COL_AccountValue];
                colAccount.Header.Caption = ClosingConstants.CAP_Account;

                UltraGridColumn colStrategyIDValue = gridBand.Columns[ClosingConstants.COL_Strategy];
                colStrategyIDValue.Header.Caption = ClosingConstants.CAP_Strategy;

                UltraGridColumn colPositionTag = gridBand.Columns[ClosingConstants.COL_PositionTag];
                colPositionTag.Header.Caption = ClosingConstants.CAP_PositionType;

                UltraGridColumn colAssetCategoryValue = gridBand.Columns[ClosingConstants.COL_AssetCategoryValue];
                colAssetCategoryValue.Header.Caption = ClosingConstants.CAP_AssetCategory;

                UltraGridColumn colSymbol = gridBand.Columns[ClosingConstants.COL_Symbol];
                colSymbol.Header.Caption = ClosingConstants.CAP_Symbol;

                UltraGridColumn colClosingQuantity = gridBand.Columns[ClosingConstants.COL_ClosedQty];
                colClosingQuantity.Header.Caption = ClosingConstants.CAP_CloseQty;
                colClosingQuantity.Format = _quantityColumnFormat;

                UltraGridColumn colOpenAveragePrice = gridBand.Columns[ClosingConstants.COL_OpenAveragePrice];
                colOpenAveragePrice.Header.Caption = ClosingConstants.CAP_AvgPrice;
                colOpenAveragePrice.Format = _currencyColumnFormat;

                UltraGridColumn colClosedAveragePrice = gridBand.Columns[ClosingConstants.COL_ClosedAveragePrice];
                colClosedAveragePrice.Header.Caption = ClosingConstants.CAP_AvgClosingPrice;
                colClosedAveragePrice.Format = _currencyColumnFormat;

                UltraGridColumn colCommision = gridBand.Columns[ClosingConstants.COL_ClosingTotalCommissionandFees];
                colCommision.Header.Caption = ClosingConstants.COL_ClosingTotalCommissionandFees;
                colCommision.Format = _currencyColumnFormat;

                UltraGridColumn colRealizedPNL = gridBand.Columns[ClosingConstants.COL_RealizedPNL];
                colRealizedPNL.Header.Caption = ClosingConstants.CAP_RealizedPNL;
                colRealizedPNL.Format = _currencyColumnFormat;

                UltraGridColumn colExchange = gridBand.Columns[ClosingConstants.COL_Exchange];
                colExchange.Header.Caption = ClosingConstants.CAP_Exchange;

                UltraGridColumn colSide = gridBand.Columns[ClosingConstants.COL_PositionSide];
                colSide.Header.Caption = ClosingConstants.CAP_OpeningSide;

                UltraGridColumn colPositionCommision = gridBand.Columns[ClosingConstants.COL_PositionCommission];
                colPositionCommision.Header.Caption = ClosingConstants.CAP_PositionCommission;
                colPositionCommision.Format = _currencyColumnFormat;

                UltraGridColumn colClosingSide = gridBand.Columns[ClosingConstants.COL_ClosingSide];
                colClosingSide.Header.Caption = ClosingConstants.CAP_ClosingSide;

                UltraGridColumn colPositionID = gridBand.Columns[ClosingConstants.COL_ID];
                colPositionID.Header.Caption = ClosingConstants.CAP_TaxlotId;

                UltraGridColumn colCurrency = gridBand.Columns[ClosingConstants.COL_Currency];
                colCurrency.Header.Caption = ClosingConstants.CAP_Currency;

                UltraGridColumn colUnderlying = gridBand.Columns[ClosingConstants.COL_Underlying];
                colUnderlying.Header.Caption = ClosingConstants.CAP_Underlying;

                UltraGridColumn colClosingID = gridBand.Columns[ClosingConstants.COL_ClosingID];
                colClosingID.Header.Caption = ClosingConstants.CAP_ClosingID;

                UltraGridColumn colClosingMode = gridBand.Columns[ClosingConstants.COL_ClosingMode];
                colClosingMode.Header.Caption = ClosingConstants.CAP_ClosingMode;

                UltraGridColumn colClosingTag = gridBand.Columns[ClosingConstants.COL_ClosingTag];
                colClosingTag.Header.Caption = ClosingConstants.CAP_ClosedPositionType;

                //Modified By : Manvendra P.
                //Jira : http://jira.nirvanasolutions.com:8080/browse/PRANA-9008
                UltraGridColumn colTradeDate = gridBand.Columns[ClosingConstants.COL_TradeDatePosition];
                colTradeDate.Header.Caption = ClosingPrefManager.GetCaptionBasedonClosingDateType();  //ClosingConstants.CAP_TradeDate;
                //added by amit 24.03.2015
                //http://jira.nirvanasolutions.com:8080/browse/CHMW-3136
                UltraGridColumn colClosingAlgo = gridBand.Columns[ClosingConstants.COL_ClosingAlgo];
                colClosingAlgo.Header.Caption = ClosingConstants.CAP_ClosingAlgo;

                for (int i = 1; i <= 45; i++)
                {
                    UltraGridColumn tradeAttributeCol = gridBand.Columns[OrderFields.PROPERTY_TRADEATTRIBUTE + i];
                    tradeAttributeCol.Header.Caption = CachedDataManager.GetInstance.GetAttributeNameForValue(ClosingConstants.CAPTION_TRADEATTRIBUTE + i);
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
        CheckBoxOnHeader_CreationFilter headerCheckBoxUnWind = new CheckBoxOnHeader_CreationFilter();

        private void AddCheckBoxinGrid(UltraGrid grid, CheckBoxOnHeader_CreationFilter headerCheckBox)
        {
            try
            {
                grid.CreationFilter = headerCheckBox;
                grid.DisplayLayout.Bands[0].Columns.Add("checkBox", "");
                grid.DisplayLayout.Bands[0].Columns["checkBox"].DataType = typeof(bool);
                SetCheckBoxAtFirstPosition(grid);
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
        private void SetCheckBoxAtFirstPosition(UltraGrid grid)
        {
            try
            {
                grid.DisplayLayout.Bands[0].Columns["checkBox"].Hidden = false;
                grid.DisplayLayout.Bands[0].Columns["checkBox"].Header.VisiblePosition = 0;
                grid.DisplayLayout.Bands[0].Columns["checkBox"].Width = 10;
                grid.DisplayLayout.Bands[0].Columns["checkBox"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
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

        private void chkBoxBuyAndBuyToCover_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ClosingPrefManager.IsShortWithBuyAndBuyToCover = chkBoxBuyAndBuyToCover.Checked;
                //Narendra Kumar Jangir 2012 Oct 24
                //value of both the checkboxes should remain same either true or false.
                chkBoxSellWithBTC.Checked = chkBoxBuyAndBuyToCover.Checked;
                ClosingPrefManager.IsSellWithBuyToClose = chkBoxSellWithBTC.Checked;
            }
            catch (Exception ex)
            {
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        void cmbMethodology_MouseHover(object sender, System.EventArgs e)
        {
            try
            {
                Infragistics.Win.UltraWinToolTip.UltraToolTipInfo toolTipInfo = this.ultraToolTipManager1.GetUltraToolTip(cmbMethodology);
                toolTipInfo.ToolTipText = cmbMethodology.Text;
            }
            catch (Exception ex)
            {
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        void cmbSecondarySort_MouseHover(object sender, System.EventArgs e)
        {
            try
            {
                Infragistics.Win.UltraWinToolTip.UltraToolTipInfo toolTipInfo = this.ultraToolTipManager1.GetUltraToolTip(cmbSecondarySort);
                toolTipInfo.ToolTipText = cmbSecondarySort.Text;
            }
            catch (Exception ex)
            {
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        void cmbAlgorithm_MouseHover(object sender, System.EventArgs e)
        {
            try
            {
                Infragistics.Win.UltraWinToolTip.UltraToolTipInfo toolTipInfo = this.ultraToolTipManager1.GetUltraToolTip(cmbAlgorithm);
                toolTipInfo.ToolTipText = cmbAlgorithm.Text;
            }
            catch (Exception ex)
            {
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        void cmbClosingField_MouseHover(object sender, System.EventArgs e)
        {
            try
            {
                Infragistics.Win.UltraWinToolTip.UltraToolTipInfo toolTipInfo = this.ultraToolTipManager1.GetUltraToolTip(cmbClosingField);
                toolTipInfo.ToolTipText = cmbClosingField.Text;
            }
            catch (Exception ex)
            {
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void cmbMethodology_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbMethodology.Value != null && (PostTradeEnums.CloseTradeMethodology)
                       cmbMethodology.Value == PostTradeEnums.CloseTradeMethodology.Manual)
                {
                    //bind combo with none
                    List<EnumerationValue> list = new List<EnumerationValue>();
                    list.Add(new EnumerationValue(ApplicationConstants.C_COMBO_NONE, -1));
                    cmbAlgorithm.DataSource = null;
                    cmbAlgorithm.DataSource = list;
                    cmbAlgorithm.DisplayMember = "DisplayText";
                    cmbAlgorithm.ValueMember = "Value";
                    cmbAlgorithm.Enabled = false;
                    cmbAlgorithm.Value = -1;
                    cmbAlgorithm.Refresh();
                    cmbSecondarySort.Value = 0;
                    cmbSecondarySort.Enabled = false;
                    cmbSecondarySort.Refresh();
                    cmbClosingField.Value = 0;
                    cmbClosingField.Enabled = false;
                    cmbClosingField.Refresh();

                    //Need to do remove setter from here 
                    ClosingPrefManager.DefaultMethodology = PostTradeEnums.CloseTradeMethodology.Manual;
                    DisableEnableForm(true);
                    btnRun.Enabled = false;
                    //btnSaveACA.Enabled = false;
                    cmbAlgorithm.Enabled = false;
                    cmbSecondarySort.Enabled = false;
                    cmbClosingField.Enabled = false;
                    chkBoxBuyAndBuyToCover.Enabled = true;
                    chkBoxSellWithBTC.Enabled = true;
                    grdLong.AllowDrop = true;
                    grdShort.AllowDrop = true;
                }
                else if (cmbMethodology.Value != null && (PostTradeEnums.CloseTradeMethodology)
                       cmbMethodology.Value == PostTradeEnums.CloseTradeMethodology.Automatic)
                {
                    cmbAlgorithm.Enabled = true;
                    cmbAlgorithm.DataSource = null;
                    List<EnumerationValue> lst = Prana.Utilities.MiscUtilities.EnumHelper.ConvertEnumForBindingWithAssignedValues(typeof(PostTradeEnums.CloseTradeAlogrithm));
                    List<EnumerationValue> closingAlgos = new List<EnumerationValue>();
                    foreach (EnumerationValue value in lst)
                    {
                        //Narendra 2014/08/13
                        //added aca to close trade combo for testing purpose
                        //if ((!value.DisplayText.Equals(PostTradeEnums.CloseTradeAlogrithm.ACA.ToString())) && (!value.DisplayText.Equals(PostTradeEnums.CloseTradeAlogrithm.NONE.ToString())) && (!value.DisplayText.Equals(PostTradeEnums.CloseTradeAlogrithm.MANUAL.ToString())))
                        //Modified By : Manvendra Jira : PRANA-10341
                        if ((!value.DisplayText.Equals(PostTradeEnums.CloseTradeAlogrithm.NONE.ToString())) && (!value.DisplayText.Equals(PostTradeEnums.CloseTradeAlogrithm.MANUAL.ToString())) && (!value.DisplayText.Equals(PostTradeEnums.CloseTradeAlogrithm.Multiple.ToString())))
                        {
                            closingAlgos.Add(value);
                        }
                    }
                    cmbAlgorithm.DataSource = closingAlgos;
                    cmbAlgorithm.DisplayMember = "DisplayText";
                    cmbAlgorithm.ValueMember = "Value";
                    cmbAlgorithm.Value = (int)preferences.ClosingMethodology.ClosingAlgo;
                    Utils.UltraComboFilter(cmbAlgorithm, "DisplayText");

                    cmbSecondarySort.Value = (int)preferences.ClosingMethodology.SecondarySort;
                    Utils.UltraComboFilter(cmbSecondarySort, "DisplayText");
                    cmbSecondarySort.Value = 0;
                    cmbSecondarySort.Enabled = true;
                    cmbSecondarySort.Refresh();

                    cmbClosingField.Value = (int)preferences.ClosingMethodology.ClosingField;
                    Utils.UltraComboFilter(cmbClosingField, "DisplayText");
                    cmbClosingField.Value = 0;
                    cmbClosingField.Enabled = true;
                    cmbClosingField.Refresh();

                    ClosingPrefManager.DefaultMethodology = PostTradeEnums.CloseTradeMethodology.Automatic;
                    btnRun.Enabled = true;
                    grdLong.AllowDrop = false;
                    grdShort.AllowDrop = false;
                    DisableEnableForm(true);
                }
                else
                {
                    List<EnumerationValue> list = new List<EnumerationValue>();
                    list.Add(new EnumerationValue(ApplicationConstants.C_COMBO_NONE, -1));
                    cmbAlgorithm.DataSource = null;
                    cmbAlgorithm.DataSource = list;
                    cmbAlgorithm.DisplayMember = "DisplayText";
                    cmbAlgorithm.ValueMember = "Value";
                    cmbAlgorithm.Enabled = false;
                    cmbAlgorithm.Value = -1;
                    cmbAlgorithm.Refresh();

                    cmbSecondarySort.Value = 0;
                    cmbSecondarySort.Enabled = false;
                    cmbSecondarySort.Refresh();

                    cmbClosingField.Value = 0;
                    cmbClosingField.Enabled = false;
                    cmbClosingField.Refresh();

                    btnRun.Enabled = false;
                    //closing by drag and drop is allowed only in manual closing method
                    grdLong.AllowDrop = false;
                    grdShort.AllowDrop = false;
                    chkBoxBuyAndBuyToCover.Enabled = false;
                    chkBoxSellWithBTC.Enabled = false;
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

        private void cmbAlgorithm_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbAlgorithm.Value != null)
                {
                    ClosingPrefManager.Algorithm = (PostTradeEnums.CloseTradeAlogrithm)cmbAlgorithm.Value;
                    if ((PostTradeEnums.CloseTradeAlogrithm)cmbAlgorithm.Value == PostTradeEnums.CloseTradeAlogrithm.NONE)
                    {
                        cmbSecondarySort.Value = 0;
                        cmbSecondarySort.Enabled = false;
                    }
                    else
                    {
                        cmbSecondarySort.Enabled = true;
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

        public void ClearAllGridFilters()
        {
            try
            {
                grdLong.DisplayLayout.Bands[0].ColumnFilters.ClearAllFilters();
                grdShort.DisplayLayout.Bands[0].ColumnFilters.ClearAllFilters();
                grdNetPosition.DisplayLayout.Bands[0].ColumnFilters.ClearAllFilters();
                SetDefaultFilters();
            }
            catch (Exception ex)
            {
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public bool CheckSaved()
        {
            return true;
        }

        private void cbSyncFilter_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (!cbSyncFilter.Checked)
                {
                    ClearAllGridFilters();
                }
            }
            catch (Exception ex)
            {
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void btnClearFilter_Click(object sender, EventArgs e)
        {
            try
            {
                ClearAllGridFilters();
            }
            catch (Exception ex)
            {
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void cntxtMenuStripClosedPosition_Opening(object sender, CancelEventArgs e)
        {
            try
            {
                if (grdNetPosition.ActiveRow != null)
                {
                    UltraGridRow row = grdNetPosition.ActiveRow;
                    Position selectedPos = (Position)row.ListObject;
                    if (selectedPos.ClosingMode == ClosingMode.CorporateAction || ClosingClientSideMapper.IsUnsavedData())
                    {
                        unwindToolStripMenuItem.Enabled = false;
                    }
                    else if (selectedPos.ClosingMode == ClosingMode.CostBasisAdjustment)        //Disable unwind menu for cost adjusted trades: http://jira.nirvanasolutions.com:8080/browse/PRANA-6869
                    {
                        unwindToolStripMenuItem.Enabled = false;
                    }
                    unwindToolStripMenuItem.Enabled = true;
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

        void grd_BeforeRowFilterDropDown(object sender, Infragistics.Win.UltraWinGrid.BeforeRowFilterDropDownEventArgs e)
        {
            try
            {
                if (e.Column.Key.Equals(ClosingConstants.COL_TradeDate) || e.Column.Key.Equals(ClosingConstants.COL_ProcessDate) || e.Column.Key.Equals(ClosingConstants.COL_ClosingTradeDate))
                {
                    e.ValueList.ValueListItems.Insert(4, "(Today)", "(Today)");
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

        void grdNetPosition_AfterRowFilterChanged(object sender, Infragistics.Win.UltraWinGrid.AfterRowFilterChangedEventArgs e)
        {
            try
            {
                if ((e.Column.Key.Equals(ClosingConstants.COL_ClosingTradeDate)) && e.NewColumnFilter.FilterConditions != null && e.NewColumnFilter.FilterConditions.Count == 1 && e.NewColumnFilter.FilterConditions[0].CompareValue.Equals("(Today)"))
                {
                    grdNetPosition.DisplayLayout.Bands[0].ColumnFilters[e.Column.Key].FilterConditions.Clear();
                    grdNetPosition.DisplayLayout.Bands[0].ColumnFilters[e.Column.Key].FilterConditions.Add(FilterComparisionOperator.StartsWith, DateTime.Now.Date.ToString(DateTimeConstants.DateformatForClosing));
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

        private void grdLong_BeforeColumnChooserDisplayed(object sender, BeforeColumnChooserDisplayedEventArgs e)
        {
            try
            {
                e.Cancel = true;
                (this.FindForm()).AddCustomColumnChooser(this.grdLong);
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

        private void grdShort_BeforeColumnChooserDisplayed(object sender, BeforeColumnChooserDisplayedEventArgs e)
        {
            try
            {
                e.Cancel = true;
                (this.FindForm()).AddCustomColumnChooser(this.grdShort);
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

        private void grdNetPosition_BeforeColumnChooserDisplayed(object sender, BeforeColumnChooserDisplayedEventArgs e)
        {
            try
            {
                e.Cancel = true;
                (this.FindForm()).AddCustomColumnChooser(this.grdNetPosition);
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

        private void chkboxSellWithBTC_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ClosingPrefManager.IsSellWithBuyToClose = chkBoxSellWithBTC.Checked;
                //Narendra Kumar Jangir 2012 Oct 24
                //value of both the checkboxes should remain same either true or false.
                chkBoxBuyAndBuyToCover.Checked = chkBoxSellWithBTC.Checked;
                ClosingPrefManager.IsShortWithBuyAndBuyToCover = chkBoxBuyAndBuyToCover.Checked;
            }
            catch (Exception ex)
            {
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void ExportToExcel(List<UltraGrid> lstGrids)
        {
            try
            {
                ExcelAndPrintUtilities excelUtils = new ExcelAndPrintUtilities();
                excelUtils.SetExcelLayoutAndWrite(lstGrids, false, null);
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

        private void exportToExcelShortLongStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                List<UltraGrid> lstGrids = new List<UltraGrid>();
                if (gridSelected == "Long")
                {
                    lstGrids.Add(grdLong);
                }
                else
                {
                    lstGrids.Add(grdShort);
                }
                ExportToExcel(lstGrids);
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

        private void exportToExcelStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                List<UltraGrid> lstGrids = new List<UltraGrid>();
                lstGrids.Add(grdNetPosition);
                ExportToExcel(lstGrids);
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

        // save layout menu item for grdNetposition
        private void saveLayoutStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SaveLayoutforGrids();
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

        // Save layout Menu Item for grdshort and grdlong
        private void saveLayoutShortLongtoolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SaveLayoutforGrids();
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

        private void SaveLayoutforGrids()
        {
            try
            {
                // grids layout save method is moved to main form as now there are more than one controls on to save layout
                if (SaveLayout != null)
                {
                    SaveLayout(null, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        internal UltraGrid GetLongGrid()
        {
            try
            {
                return grdLong;
            }
            catch (Exception ex)
            {
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        internal UltraGrid GetShortGrid()
        {
            try
            {
                return grdShort;
            }
            catch (Exception ex)
            {
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        internal UltraGrid GetNetPositionsGrid()
        {
            try
            {
                return grdNetPosition;
            }
            catch (Exception ex)
            {
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        private void cmbSecondarySort_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if ((int)(cmbAlgorithm.Value) != -1 && cmbSecondarySort.Value != null)
                {
                    ClosingPrefManager.SecondarySort = (PostTradeEnums.SecondarySortCriteria)cmbSecondarySort.Value;
                }
            }
            catch (Exception ex)
            {
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void cmbClosingField_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if ((int)(cmbAlgorithm.Value) != -1 && cmbSecondarySort.Value != null && cmbClosingField.Value != null)
                {
                    ClosingPrefManager.ClosingField = (PostTradeEnums.ClosingField)cmbClosingField.Value;
                }
            }
            catch (Exception ex)
            {
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void UnwireGridEvents()
        {
            grdLong.InitializeRow -= new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(grdLong_InitializeRow);
            grdShort.InitializeRow -= new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(grdShort_InitializeRow);
        }

        public async Task<ClosingData> RunClosing_DoWork(object[] arguments)
        {
            ClosingData ClosedData = null;
            try
            {
                UnwireGridEvents();
                await System.Threading.Tasks.Task.Run(() =>
                {
                    UltraGridRow[] grdLongFilteredRows = arguments[0] as UltraGridRow[];
                    UltraGridRow[] grdShortFilteredRows = arguments[1] as UltraGridRow[];
                    List<TaxLot> buyTaxLotsAndPositions = new List<TaxLot>();
                    List<TaxLot> sellTaxLotsAndPositions = new List<TaxLot>();

                    //PostTradeEnums.CloseTradeMethodology methodology = ClosingPrefManager.DefaultMethodology;
                    PostTradeEnums.CloseTradeAlogrithm algorithm = ClosingPrefManager.Algorithm;

                    foreach (UltraGridRow row in grdLongFilteredRows)
                    {
                        TaxLot t = (TaxLot)row.ListObject;
                        t.TaxLotClosingId = null;
                        t.ClosingMode = ClosingMode.None;
                        buyTaxLotsAndPositions.Add(t);
                    }

                    foreach (UltraGridRow row in grdShortFilteredRows)
                    {
                        TaxLot t = (TaxLot)row.ListObject;
                        t.TaxLotClosingId = null;
                        t.ClosingMode = ClosingMode.None;
                        sellTaxLotsAndPositions.Add(t);
                    }

                    if (sellTaxLotsAndPositions.Count != 0 && buyTaxLotsAndPositions.Count != 0)
                    {
                        bool isAutoCloseStrategy = chkBxIsAutoCloseStrategy.Checked;
                        ClosingParameters closingParams = new ClosingParameters();
                        closingParams.BuyTaxLotsAndPositions = buyTaxLotsAndPositions;
                        closingParams.SellTaxLotsAndPositions = sellTaxLotsAndPositions;
                        closingParams.Algorithm = algorithm;
                        closingParams.IsShortWithBuyAndBuyToCover = ClosingPrefManager.IsShortWithBuyAndBuyToCover;
                        closingParams.IsSellWithBuyToClose = ClosingPrefManager.IsSellWithBuyToClose;
                        closingParams.IsDragDrop = false;
                        closingParams.IsFromServer = false;
                        closingParams.SecondarySort = ClosingPrefManager.SecondarySort;
                        closingParams.IsVirtualClosingPopulate = false;
                        closingParams.IsOverrideWithUserClosing = false;
                        closingParams.IsMatchStrategy = !isAutoCloseStrategy;
                        closingParams.ClosingField = ClosingPrefManager.ClosingField;
                        closingParams.IsCopyOpeningTradeAttributes = chkCopyOpeningTradeAttributes.Checked;

                        switch (ClosingPrefManager.Algorithm)
                        {
                            case PostTradeEnums.CloseTradeAlogrithm.PRESET:
                                closingParams.IsManual = false;
                                ClosedData = _closingServices.InnerChannel.AutomaticClosingOnManualOrPresetBasis(closingParams);
                                break;

                            default:
                                closingParams.IsManual = true;
                                ClosedData = _closingServices.InnerChannel.AutomaticClosingOnManualOrPresetBasis(closingParams);
                                break;
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                _isCancel = true;
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return ClosedData;
        }

        /// <summary>
        /// Added by Faisal Shah 18/09/14
        /// To return only those Taxlots on which Account Lock has been taken
        /// </summary>
        /// <param name="grdFilteredRows"></param>
        /// <param name="isMessageRequired"></param>
        /// <returns></returns>
        private bool GetTaxlotsAndPositions(UltraGridRow[] grdLongFilteredRows, UltraGridRow[] grdShortFilteredRows, ref List<TaxLot> buyTaxLotsAndPositions, ref List<TaxLot> sellTaxLotsAndPositions)
        {
            bool userResponse = false;
            try
            {
                StringBuilder listOfUnLockedAccounts = new StringBuilder();
                StringBuilder listOfLockedAccounts = new StringBuilder();
                List<int> availableaccounts = new List<int>();
                List<int> allAccounts = new List<int>();
                List<int> userLockedAccounts = new List<int>();
                foreach (UltraGridRow row in grdLongFilteredRows)
                {
                    string accountName = row.Cells[ClosingConstants.COL_Account].Value.ToString();
                    int accountID = CachedDataManager.GetInstance.GetAccountID(accountName);
                    if (!allAccounts.Contains(accountID))
                    {
                        allAccounts.Add(accountID);
                    }
                }
                // availableaccounts = AllocationManager.GetListOfUnlockedAccounts(allAccounts);
                foreach (int accountID in allAccounts)
                {
                    string accountName = CachedDataManager.GetInstance.GetAccountText(accountID);
                    if (!availableaccounts.Contains(accountID))
                    {
                        if (CachedDataManager.GetInstance.isAccountLocked(accountID))
                        {
                            userLockedAccounts.Add(accountID);
                            continue;
                        }
                        listOfLockedAccounts.Append(accountName + ", ");
                    }
                    else
                    {
                        listOfUnLockedAccounts.Append(accountName + ", ");
                    }
                }
                if (availableaccounts.Count == 0 && userLockedAccounts.Count == 0)
                {
                    MessageBox.Show("All accounts are currently locked by other User/Users\n Please refer to account Lock UI to check the same", "Account Lock", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    if (listOfUnLockedAccounts.Length > 0)
                    {
                        String message = "Closing is subject to having AccountLock.\n Do You want to proceed with Locking accounts " + listOfUnLockedAccounts.ToString().Substring(0, listOfUnLockedAccounts.Length - 2) + "?";
                        if (listOfLockedAccounts.Length > 0)
                            message = "Closing is subject to having Account Lock. Following accounts:\n" + listOfLockedAccounts.ToString().Substring(0, listOfLockedAccounts.Length - 2) + " " + "are locked by other User/Users.\n Do You want to proceed with Locking accounts " + listOfUnLockedAccounts.ToString().Substring(0, listOfUnLockedAccounts.Length - 2) + "?";
                        if (MessageBox.Show(message, "Account Lock", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            userResponse = true;
                        }
                    }
                    if ((userResponse && availableaccounts.Count > 0) || userLockedAccounts.Count > 0)
                    {
                        if (userResponse == true)
                        {
                            foreach (int accountID in userLockedAccounts)
                            {
                                if (!availableaccounts.Contains(accountID))
                                    availableaccounts.Add(accountID);
                            }
                        }
                        else
                        {
                            availableaccounts = userLockedAccounts;
                        }
                        if (AccountLockManager.SetAccountsLockStatus(availableaccounts))
                        {
                            foreach (UltraGridRow row in grdLongFilteredRows)
                            {
                                int accountID = CachedDataManager.GetInstance.GetAccountID(row.Cells[ClosingConstants.COL_Account].Value.ToString());
                                if ((availableaccounts.Contains(accountID) || userLockedAccounts.Contains(accountID)) && !buyTaxLotsAndPositions.Contains((TaxLot)row.ListObject))
                                    buyTaxLotsAndPositions.Add((TaxLot)row.ListObject);
                            }
                            foreach (UltraGridRow row in grdShortFilteredRows)
                            {
                                int accountID = CachedDataManager.GetInstance.GetAccountID(row.Cells[ClosingConstants.COL_Account].Value.ToString());
                                if ((availableaccounts.Contains(accountID) || userLockedAccounts.Contains(accountID)) && !sellTaxLotsAndPositions.Contains((TaxLot)row.ListObject))
                                    sellTaxLotsAndPositions.Add((TaxLot)row.ListObject);
                            }
                            userResponse = true;
                        }
                        else
                        {
                            MessageBox.Show("Some problem in locking Current CashAccounts.\n Please retry to Continue", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            userResponse = false;
                        }
                    }
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
            return userResponse; ;
        }

        /// <summary>
        /// RunClosing Completed
        /// </summary>
        /// <param name="ClosedData"></param>
        void RunClosing_Completed(ClosingData ClosedData)
        {
            try
            {
                _statusMessage = string.Empty;
                if (_isCancel)//it doesn't matter if the BG worker ends normally, or gets cancelled,
                {              //both cases RunWorkerCompleted is invoked, so we need to check what has happened
                    _isCancel = false;
                    MessageBox.Show("Operation has been cancelled!", "Close Trade Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    //dowork method return the object closed data to mainipulate the error message
                    if (ClosedData != null)
                    {
                        if (ClosedData.IsDataClosed)
                        {
                            //show warning message if closing algorithm is PRESET, in this case data will be closed for accounts which have invalid secondary sort criteria
                            if (ClosedData.IsNavLockFailed)
                            {
                                MessageBox.Show(ClosedData.ErrorMsg.ToString(), "Nav Lock", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else if (!ClosedData.ErrorMsg.ToString().Equals(string.Empty))
                            {
                                MessageBox.Show(ClosedData.ErrorMsg.ToString(), "Close Trade Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else if (ClosedData.IsPartial)
                            {
                                ClosingClientSideMapper.UpdateRepository(ClosedData);
                                AddClosingAuditEntry(ClosedData, Prana.BusinessObjects.TradeAuditActionType.ActionType.Closing, TradeAuditActionType.AllocationAuditComments.PartialDataClosed.ToString(), CachedDataManager.GetInstance.LoggedInUser.CompanyUserID);
                                AuditManager.Instance.SaveAuditList(_tradeAuditCollection_Closing);
                                _tradeAuditCollection_Closing.Clear();
                                _statusMessage = "Partial Close Trade Data Saved";
                            }
                            else
                            {
                                ClosingClientSideMapper.UpdateRepository(ClosedData);
                                AddClosingAuditEntry(ClosedData, Prana.BusinessObjects.TradeAuditActionType.ActionType.Closing, TradeAuditActionType.AllocationAuditComments.DataClosed.ToString(), CachedDataManager.GetInstance.LoggedInUser.CompanyUserID);
                                AuditManager.Instance.SaveAuditList(_tradeAuditCollection_Closing);
                                _tradeAuditCollection_Closing.Clear();
                                _statusMessage = "Close Trade Data Saved";
                            }
                        }
                        else
                        {
                            //show warning message if closing algorithm is not PRESET, in this case data will be closed if valid secondary sort criteria given from the closing UI
                            if (ClosedData.IsNavLockFailed)
                            {
                                MessageBox.Show(ClosedData.ErrorMsg.ToString(), "Nav Lock", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else if (!ClosedData.ErrorMsg.ToString().Equals(string.Empty))
                            {
                                MessageBox.Show(ClosedData.ErrorMsg.ToString(), "Close Trade Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                            {
                                _statusMessage = "Nothing to Save";
                            }
                        }
                    }
                    else
                        InformationMessageBox.Display("Nothing to Close");
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
            finally
            {
                //condition to check if it has atleast one listner..
                if (DisableEnableParentForm != null)
                {
                    DisableEnableParentForm(this, new EventArgs<string, bool, bool>(_statusMessage, true, false));
                }
                //enable this form
                DisableEnableForm(true);
                WireEvents();
                ResumePainting();
            }
        }

        private void WireEvents()
        {
            grdLong.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(grdLong_InitializeRow);
            grdShort.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(grdShort_InitializeRow);
        }

        public async Task<string> UnwindClosing_DoWork(object[] arguments)
        {
            try
            {
                UltraGridRow[] filteredRows = arguments[0] as UltraGridRow[];
                GenericBindingList<Position> posList = new GenericBindingList<Position>();
                Dictionary<string, DateTime> dictTaxlotIds = new Dictionary<string, DateTime>();
                Dictionary<int, string> lstUnLockedAccountIDs = new Dictionary<int, string>();
                foreach (UltraGridRow row in filteredRows)
                {
                    if (row.Cells["checkBox"].Value.ToString().ToLower() == "true")
                    {
                        Position position = row.ListObject as Position;

                        Position pos = (Position)row.ListObject;
                        posList.Add((Position)row.ListObject);
                        if (!dictTaxlotIds.ContainsKey(pos.ID))
                            dictTaxlotIds.Add(pos.ID, pos.ClosingTradeDate);
                        if (!dictTaxlotIds.ContainsKey(pos.ClosingID))
                            dictTaxlotIds.Add(pos.ClosingID, pos.ClosingTradeDate);
                    }
                }
                if (posList.Count == 0)
                {
                    return string.Empty;
                }

                if (lstUnLockedAccountIDs.Count > 0)
                {
                    string strUnLockedAccountIDs = String.Join(",", lstUnLockedAccountIDs.Values.ToList());
                    if (MessageBox.Show("The ability to make changes to a account can only be granted to one user at a time, would you like to proceed in locking following accounts " + strUnLockedAccountIDs + "?", "Account Lock", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        List<int> newAccountsToBelocked = new List<int>();
                        newAccountsToBelocked.AddRange(lstUnLockedAccountIDs.Keys.ToList());
                        newAccountsToBelocked.AddRange(CachedDataManager.GetInstance.GetLockedAccounts());
                        if (AccountLockManager.SetAccountsLockStatus(newAccountsToBelocked))
                        {
                            MessageBox.Show("The lock for accounts has been acquired, you may continue.", "Account Lock", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            //update Locks in cache
                            CachedDataManager.GetInstance.SetLockedAccounts(newAccountsToBelocked);
                        }
                        else
                        {
                            MessageBox.Show("CashAccounts are currently locked by another user, please refer to the Account Lock screen for more information.", "Account Lock", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return string.Empty;
                        }
                    }
                    else
                    {
                        return string.Empty;
                    }
                }

                string result = string.Empty;
                await System.Threading.Tasks.Task.Run(() =>
                {
                    UltraGridRow[] filteredRowsNew = new UltraGridRow[filteredRows.Length];
                    filteredRows.CopyTo(filteredRowsNew, 0);
                    StringBuilder s = new StringBuilder();
                    StringBuilder taxlotId = new StringBuilder();
                    StringBuilder taxlotClosingIDWithClosingDate = new StringBuilder();
                    List<string> taxlotidList = new List<string>();
                    DialogResult userChoice = MessageBox.Show("This will unwind the closing,Would you like to proceed ?", "Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                    if (userChoice == DialogResult.Yes)
                    {
                        if (UIValidation.GetInstance().validate(this))
                        {
                            if (this.InvokeRequired)
                            {
                                if (DisableEnableParentForm != null)
                                {
                                    _statusMessage = "Unwinding Data Please Wait";
                                    this.BeginInvoke(DisableEnableParentForm, this, new EventArgs<string, bool, bool>(_statusMessage, false, true));
                                }
                            }
                        }
                        StringBuilder message = new StringBuilder();
                        Dictionary<string, StatusInfo> positionStatusDict = _closingServices.InnerChannel.ArePositionEligibletoUnwind(dictTaxlotIds);
                        List<DateTime> closingDates = new List<DateTime>();
                        foreach (Position position in posList)
                        {
                            if (position != null)
                            {
                                if (!((positionStatusDict.ContainsKey(position.ID)) || (positionStatusDict.ContainsKey(position.ClosingID))))
                                {
                                    s.Append(position.TaxLotClosingId.ToString());
                                    s.Append(",");

                                    taxlotClosingIDWithClosingDate.Append(position.TaxLotClosingId.ToString());
                                    taxlotClosingIDWithClosingDate.Append('_');
                                    taxlotClosingIDWithClosingDate.Append(position.ClosingTradeDate.ToString());
                                    taxlotClosingIDWithClosingDate.Append('_');

                                    //added by: Bharat raturi, 28 oct 2014
                                    //accountID must be sent, so that the revaluation date can be managed account-wise
                                    taxlotClosingIDWithClosingDate.Append(position.AccountValue.ID.ToString());
                                    taxlotClosingIDWithClosingDate.Append(",");
                                    if (!taxlotidList.Contains(position.ID.ToString()))
                                    {
                                        taxlotId.Append(position.ID.ToString());
                                        taxlotId.Append(",");
                                    }
                                    if (!taxlotidList.Contains(position.ClosingID.ToString()))
                                    {
                                        taxlotId.Append(position.ClosingID.ToString());
                                        taxlotId.Append(",");
                                    }
                                    closingDates.Add(position.ClosingTradeDate);
                                }
                                else
                                {
                                    if (positionStatusDict.ContainsKey(position.ID) || positionStatusDict.ContainsKey(position.ClosingID))
                                    {
                                        if ((positionStatusDict.ContainsKey(position.ID) && positionStatusDict[position.ID].Status.Equals(PostTradeEnums.Status.CorporateAction)) || (positionStatusDict.ContainsKey(position.ClosingID) && positionStatusDict[position.ClosingID].Status.Equals(PostTradeEnums.Status.CorporateAction)))
                                        {
                                            message.Append("TaxlotID : ");
                                            message.Append(position.ID.ToString());
                                            message.Append(",");
                                            message.Append(position.ClosingID.ToString());
                                            message.Append(" has corporate action on future date, First undo Corporate Action then unwind");
                                            message.Append(Environment.NewLine);
                                        }

                                        // If cost adjustment has been applied on taxlotID, then closing should not be unwinded
                                        // http://jira.nirvanasolutions.com:8080/browse/PRANA-7515
                                        if ((positionStatusDict.ContainsKey(position.ID) && positionStatusDict[position.ID].Status.Equals(PostTradeEnums.Status.CostBasisAdjustment)) || (positionStatusDict.ContainsKey(position.ClosingID) && positionStatusDict[position.ClosingID].Status.Equals(PostTradeEnums.Status.CostBasisAdjustment)))
                                        {
                                            message.Append("Cost Adjustment has been applied on TaxlotID(s) : ");
                                            message.Append(position.ID.ToString());
                                            message.Append(",");
                                            message.Append(position.ClosingID.ToString());
                                            message.Append(" ,First undo Cost Adjustment then unwind");
                                            message.Append(Environment.NewLine);
                                        }
                                    }
                                    if (positionStatusDict.ContainsKey(position.ID))
                                    {
                                        foreach (KeyValuePair<string, PostTradeEnums.Status> kp in positionStatusDict[position.ID].ExercisedUnderlying)
                                        {
                                            message.Append("Exercised Underlying IDs : ");
                                            if (positionStatusDict[position.ID].ExercisedUnderlying.Keys.Count > 0)
                                            {
                                                foreach (string key in positionStatusDict[position.ID].ExercisedUnderlying.Keys)
                                                {
                                                    string id = key;
                                                    message.Append(id);
                                                    message.Append(" , ");
                                                }
                                                message.Append("generated by TaxlotID : ");
                                                message.Append(position.ID.ToString());
                                                message.Append("  is closed, First unwind the underlying to continue");
                                                message.Append(Environment.NewLine);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (message.Length > 0)
                        {
                            result = message.ToString();
                            _statusMessage = "Unwinding Canceled.";
                        }
                        // Sandeep,11 NOV 2014: This code is added here to check if any taxlot is closed or CA is applied in future date.
                        // Before unwinding, some checks are already there on closing and corporate action cache. Assume any symbol is closed in future date or
                        // CA is applied after closing, then there is no entry in the closing cache as there might be new taxlotID generated for the same symbol
                        else
                        {
                            Dictionary<string, StatusInfo> dictFutureDateClosedInfo = _closingServices.InnerChannel.GetFutureDateClosingInfo(s.ToString());
                            if (dictFutureDateClosedInfo != null && dictFutureDateClosedInfo.Count > 0)
                            {
                                foreach (KeyValuePair<string, StatusInfo> kp in dictFutureDateClosedInfo)
                                {
                                    message.Append(kp.Value.Details);
                                    if (kp.Value.Status.Equals(PostTradeEnums.Status.CorporateAction))
                                    {
                                        message.Append("  (Corporate Action)");
                                    }
                                    else
                                    {
                                        message.Append("  (closed)");
                                    }
                                    message.Append(Environment.NewLine);
                                }
                                result = message.ToString();
                            }
                            else
                            {
                                bool isNavLockValidationFailed = false;
                                if (CachedDataManager.GetInstance.NAVLockDate.HasValue)
                                {
                                    foreach (var closingDate in closingDates)
                                    {
                                        if (!CachedDataManager.GetInstance.ValidateNAVLockDate(closingDate))
                                        {
                                            MessageBox.Show("The date you’ve chosen for this action precedes your NAV Lock date (" + CachedDataManager.GetInstance.NAVLockDate.Value.ToShortDateString()
                                                + "). Please reach out to your Support Team for further assistance", "NAV Lock", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            isNavLockValidationFailed = true;
                                            break;
                                        }
                                    }
                                }
                                if (!isNavLockValidationFailed)
                                {
                                    ClosingData closingData = _allocationServices.InnerChannel.UnWindClosing(s.ToString(), taxlotId.ToString(), taxlotClosingIDWithClosingDate.ToString());
                                    if (closingData != null)
                                    {
                                        ClosingClientSideMapper.UpdateRepository(closingData);
                                        _statusMessage = "Data Unwinded Successfully";
                                        AddUnwindingAuditEntry(closingData, posList, Prana.BusinessObjects.TradeAuditActionType.ActionType.Unwinding, TradeAuditActionType.AllocationAuditComments.DataUnwinded.ToString(), CachedDataManager.GetInstance.LoggedInUser.CompanyUserID);
                                        AuditManager.Instance.SaveAuditList(_tradeAuditCollection_Closing);
                                        _tradeAuditCollection_Closing.Clear();
                                    }
                                    else
                                    {
                                        _statusMessage = string.Empty;
                                    }
                                }
                            }
                        }
                    }
                    else //update status message if user select no on dialog box
                    {
                        _statusMessage = "Unwinding Canceled";
                        result = "Unwinding_Canceled";
                    }
                });
                return result;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
                return "Cancel";
            }
        }

        void UnwindClosing_Completed(string result)
        {
            try
            {
                if (result.Equals("Cancel"))//it doesn't matter if the BG worker ends normally, or gets cancelled,
                {              //both cases RunWorkerCompleted is invoked, so we need to check what has happened
                    MessageBox.Show("Operation has been canceled!", "Close Trade Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if (result != string.Empty && !(result.Equals("Unwinding_Canceled")))
                {
                    if (myerror != null)
                    {
                        myerror.Close();
                    }
                    myerror = new ErrorTextBox();
                    myerror.SetUp("There are some closing trade in future date. Unwind them first.", result.ToString());
                    myerror.ShowDialog();
                    myerror.BringToFront();
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
            finally
            {
                //condition to check if it has atleast one listner..
                if (DisableEnableParentForm != null)
                {
                    DisableEnableParentForm(this, new EventArgs<string, bool, bool>(_statusMessage, true, false));
                }
                //enable this form
                DisableEnableForm(true);
                ResumePainting();
            }
        }

        /// <summary>
        /// this method disables or enables control based on bool value, false=>disable, true=>enable
        /// </summary>
        /// <param name="Flag"></param>
        public void DisableEnableForm(bool Flag)
        {
            try
            {
                cmbMethodology.Enabled = Flag;
                btnClearFilter.Enabled = Flag;
                chkBoxBuyAndBuyToCover.Enabled = Flag;
                chkBoxSellWithBTC.Enabled = Flag;
                cbSyncFilter.Enabled = Flag;
                btnRun.Enabled = Flag;
                // http://jira.nirvanasolutions.com:8080/browse/PRANA-10505
                if (cmbMethodology.Value != null && ((int)(cmbMethodology.Value)) == 1)
                {
                    cmbAlgorithm.Enabled = true;
                    cmbSecondarySort.Enabled = true;
                    cmbClosingField.Enabled = true;
                }
                else
                {
                    cmbAlgorithm.Enabled = false;
                    cmbSecondarySort.Enabled = false;
                    cmbClosingField.Enabled = false;
                    btnRun.Enabled = false;
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

        public void ResumePainting()
        {
            try
            {
                if (_hasAccess)
                {
                    this.grdShort.Enabled = true;
                    this.grdNetPosition.Enabled = true;
                    this.grdLong.Enabled = true;
                }
                this.grdLong.ResumeRowSynchronization();
                this.grdLong.EndUpdate();
                Prana.ClientCommon.SafeNativeMethods.ControlDrawing.ResumeDrawing(grdLong);

                this.grdShort.ResumeRowSynchronization();
                this.grdShort.EndUpdate();
                Prana.ClientCommon.SafeNativeMethods.ControlDrawing.ResumeDrawing(grdShort);

                this.grdNetPosition.ResumeRowSynchronization();
                this.grdNetPosition.EndUpdate();
                Prana.ClientCommon.SafeNativeMethods.ControlDrawing.ResumeDrawing(grdNetPosition);
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


        internal void SetControlsAsReadOnly()
        {
            try
            {
                _hasAccess = false;
                grdLong.DisplayLayout.Bands[0].Override.AllowUpdate = DefaultableBoolean.False;
                grdNetPosition.DisplayLayout.Bands[0].Override.AllowUpdate = DefaultableBoolean.False;
                grdShort.DisplayLayout.Bands[0].Override.AllowUpdate = DefaultableBoolean.False;
                btnRun.Enabled = false;
                cmbMethodology.Enabled = false;
                foreach (ToolStripItem item in cntxtMenuStripClosedPosition.Items)
                {
                    item.Enabled = false;
                }
                foreach (ToolStripItem item in contextMenuShortLongPosition.Items)
                {
                    item.Enabled = false;
                }
                unwindToolStripMenuItem.Enabled = false;
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

        private void grdNetPosition_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            (e.CustomRowFiltersDialog as Form).PaintDynamicForm();
        }

        private void grdShort_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            (e.CustomRowFiltersDialog as Form).PaintDynamicForm();
        }

        private void grdLong_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            (e.CustomRowFiltersDialog as Form).PaintDynamicForm();
        }

        /// <summary>
        /// Adds entry to the Audit List for the Data Closed from Closing UI
        /// </summary>
        /// <param name="closeData">Not Null, Object from which the data has to be extracted</param>
        /// <param name="action">TradeAuditActionType </param>
        /// <param name="comment">Not Null, comment of the action by the user</param>
        /// <param name="companyUserId">The company user id of the user doing the changes</param>
        /// <returns></returns>
        public bool AddClosingAuditEntry(ClosingData closeData, TradeAuditActionType.ActionType action, string comment, int currentUserID)
        {
            try
            {
                if (closeData != null && comment != null)
                {
                    comment = "Taxlot Trade closed using Closing method :";
                    comment = cmbMethodology.Value != null && (PostTradeEnums.CloseTradeMethodology)
                       cmbMethodology.Value == PostTradeEnums.CloseTradeMethodology.Automatic ? comment + "AUTOMATIC" : comment + "MANUAL";

                    foreach (TaxLot taxlot in closeData.UnSavedTaxlots)
                    {
                        TradeAuditEntry newEntry = new TradeAuditEntry();
                        DateTime auecLocalDate = DateTime.Now;
                        newEntry.Action = action;
                        newEntry.Comment = comment;
                        newEntry.CompanyUserId = currentUserID;
                        newEntry.Symbol = taxlot.Symbol;
                        newEntry.Level1ID = taxlot.Level1ID;
                        newEntry.TaxLotID = taxlot.TaxLotID;
                        newEntry.GroupID = taxlot.GroupID;
                        newEntry.TaxLotClosingId = taxlot.TaxLotClosingId;
                        newEntry.OrderSideTagValue = taxlot.OrderSideTagValue;
                        newEntry.OriginalValue = "OPEN";
                        newEntry.NewValue = taxlot.ClosingStatus.Equals(ClosingStatus.PartiallyClosed) ? "PARTIALLY CLOSED" : "CLOSED";
                        newEntry.AUECLocalDate = auecLocalDate;
                        newEntry.OriginalDate = taxlot.AUECModifiedDate;
                        newEntry.Level1AllocationID = taxlot.Level1AllocationID;
                        newEntry.Source = TradeAuditActionType.ActionSource.Closing;
                        _tradeAuditCollection_Closing.Add(newEntry);
                    }
                }
                else
                    throw new NullReferenceException("The Closed Data to add in audit dictionary is null");
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
            return true;
        }

        /// <summary>
        /// Adds entry to the Audit List for the Un-Winding from Closing UI
        /// </summary>
        /// <param name="taxlot">Not Null, Generic Binding List from which the data has to be extracted</param>
        /// <param name="action">TradeAuditActionType </param>
        /// <param name="comment">Not Null, comment of the action by the user</param>
        /// <param name="companyUserId">The company user id of the user doing the changes</param>
        /// <returns></returns>
        public bool AddUnwindingAuditEntry(ClosingData closeData, GenericBindingList<Position> positionList, TradeAuditActionType.ActionType action, string comment, int currentUserID)
        {
            try
            {
                if (closeData != null && positionList != null && comment != null)
                {
                    comment = "Taxlot Trade closing unwinded using Closing method :NONE";

                    foreach (TaxLot pos in closeData.Taxlots)
                    {
                        TradeAuditEntry newEntry = new TradeAuditEntry();
                        DateTime auecLocalDate = DateTime.Now;
                        int lengthGroupId = pos.GroupID.Length;
                        newEntry.Action = action;
                        newEntry.Comment = comment;
                        newEntry.CompanyUserId = currentUserID;
                        newEntry.Symbol = pos.Symbol;
                        newEntry.Level1ID = pos.Level1ID;
                        newEntry.TaxLotID = pos.TaxLotID;
                        newEntry.GroupID = pos.GroupID;
                        Position ClosedPosition = positionList.FirstOrDefault(x => (x.ID.Substring(0, lengthGroupId) == pos.GroupID || x.ClosingID.Substring(0, lengthGroupId) == pos.GroupID));
                        if (ClosedPosition != null)
                        {
                            newEntry.TaxLotClosingId = ClosedPosition.TaxLotClosingId;
                            newEntry.OrderSideTagValue = pos.OrderSideTagValue;
                            newEntry.OriginalValue = pos.TaxLotQty != ClosedPosition.ClosedQty ? "PARTIALLY CLOSED" : "CLOSED";
                            newEntry.NewValue = pos.ClosingStatus.Equals(ClosingStatus.PartiallyClosed) ? "PARTIALLY CLOSED" : "OPEN";
                            newEntry.AUECLocalDate = auecLocalDate;
                            newEntry.OriginalDate = ClosedPosition.ClosingTradeDate;
                            newEntry.Level1AllocationID = pos.Level1AllocationID;
                            newEntry.Source = TradeAuditActionType.ActionSource.Closing;
                            _tradeAuditCollection_Closing.Add(newEntry);
                        }
                    }
                }
                else
                    throw new NullReferenceException("The Generic Binding List to add in audit dictionary is null");
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
            return true;
        }

        //RunClosing using async await
        public async void RunClosing(object[] arguments)
        {
            try
            {
                Task<ClosingData> task = RunClosing_DoWork(arguments);
                ClosingData closingData = await task;
                RunClosing_Completed(closingData);
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

        //Unwind closing using async await 
        public async void UnwindClosing(object[] arguments)
        {
            try
            {
                Task<string> task = UnwindClosing_DoWork(arguments);
                string result = await task;
                UnwindClosing_Completed(result);
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
        /// This method is use to clear all the filters from long and short grid of closing UI.
        /// </summary>
        private void ClearLongAndShortGridFiltersStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                grdShort.DisplayLayout.Bands[0].ColumnFilters.ClearAllFilters();
                grdLong.DisplayLayout.Bands[0].ColumnFilters.ClearAllFilters();
                SetDefaultFilters();
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
        /// This method is use to clear all the filters from net postion grid of closing UI.
        /// </summary>
        public void ClearNetPositionGridFiltersStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                grdNetPosition.DisplayLayout.Bands[0].ColumnFilters.ClearAllFilters();
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

        public void ExportDataForAutomation(string gridName, string filePath)
        {
            try
            {
                string folder = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                UltraGridExcelExporter exporter = new UltraGridExcelExporter();
                if (gridName == "grdLong")
                {
                    exporter.Export(grdLong, filePath);
                }
                else if (gridName == "grdShort")
                {
                    exporter.Export(grdShort, filePath);
                }
                else if (gridName == "grdNetPosition")
                {
                    exporter.Export(grdNetPosition, filePath);
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
    }
}
