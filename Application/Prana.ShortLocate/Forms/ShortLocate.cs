using ExportGridsData;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinGrid.ExcelExport;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using Prana.ShortLocate.Classes;
using Prana.ShortLocate.Forms;
using Prana.ShortLocate.Preferences;
using Prana.Utilities.UI;
using Prana.Utilities.UI.ExtensionUtilities;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;


namespace Prana.ShortLocate
{
    public partial class ShortLocate : Form, IExportGridData
    {
        ShortLocateImport SlImport = null;

        public static Dictionary<string, string> BorrowerAccountMappingDict = new Dictionary<string, string>();

        public event EventHandler MouseDoubleClickOnRow;

        private ShortLocateUIPreferences _shortLocatePreferences;

        public static bool TradeCheck = true;

        public const string ExportToolTipMsg = "Export is not allowed";

        private ctrlShortLocatePrefDataManager Dataobj = new ctrlShortLocatePrefDataManager();
        public ShortLocate()
        {
            InitializeComponent();
            if (CustomThemeHelper.ApplyTheme)
            {
                CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_PRANA_SHORTLOCATE);
                this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, "Short Locate", CustomThemeHelper.UsedFont);
            }
            SetButtonsColor();
            SetFundView();
            SetExportPermissions();
            TextEditorSymbol.Leave += TextEditorSymbol_Leave;
            numericEditorShares.Leave += numericEditorShares_Leave;
            InstanceManager.RegisterInstance(this);
        }

        /// <summary>
        ///To Disable Export functionallity for SAPI.
        /// </summary>
        private void SetExportPermissions()
        {
            try
            {
                if (CommonDataCache.CachedDataManager.CompanyMarketDataProvider == BusinessObjects.AppConstants.MarketDataProvider.SAPI && CommonDataCache.CachedDataManager.IsMarketDataBlocked)
                {
                    this.ultraPictureBoxDownload.Click -= new System.EventHandler(this.ultraPictureBoxDownload_Click);
                    this.ultraPictureBoxDownload.MouseHover += new System.EventHandler(this.ultraPictureBoxDownload_MouseHover);
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
            btnUpload.ButtonStyle = UIElementButtonStyle.Button3D;
            btnUpload.BackColor = Color.DimGray;
            btnUpload.ForeColor = Color.White;
            btnUpload.UseAppStyling = false;
            btnUpload.UseOsThemes = DefaultableBoolean.False;

            btnInquire.ButtonStyle = UIElementButtonStyle.Button3D;
            btnInquire.BackColor = Color.DimGray;
            btnInquire.ForeColor = Color.White;
            btnInquire.UseAppStyling = false;
            btnInquire.UseOsThemes = DefaultableBoolean.False;

            btnRefresh.ButtonStyle = UIElementButtonStyle.Button3D;
            btnRefresh.BackColor = Color.DimGray;
            btnRefresh.ForeColor = Color.White;
            btnRefresh.UseAppStyling = false;
            btnRefresh.UseOsThemes = DefaultableBoolean.False;

        }

        void numericEditorShares_Leave(object sender, EventArgs e)
        {
            try
            {
                UltraGridColumn AvailbleShareColumn = grdShortLocate.DisplayLayout.Bands[0].Columns[ShortLocateConstants.COL_BorrowSharesAvailable];
                ColumnFilter AvailbleShareColumnFilter = AvailbleShareColumn.Band.ColumnFilters[AvailbleShareColumn];
                //  AvailbleShareColumnFilter.ClearFilterConditions();
                var prevfilter = AvailbleShareColumnFilter;
                if (!prevfilter.Equals(Shares))
                    AvailbleShareColumnFilter.ClearFilterConditions();
                if (Shares != 0)
                    AvailbleShareColumnFilter.FilterConditions.Add(FilterComparisionOperator.GreaterThanOrEqualTo, Shares);
                else
                    AvailbleShareColumnFilter.ClearFilterConditions();
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

        void TextEditorSymbol_Leave(object sender, EventArgs e)
        {
            UltraGridColumn SymbolColumn = grdShortLocate.DisplayLayout.Bands[0].Columns[ShortLocateConstants.COL_Ticker];
            ColumnFilter SymbolColumnFilter = SymbolColumn.Band.ColumnFilters[SymbolColumn];
            //   SymbolColumnFilter.ClearFilterConditions();
            var prevfilter = SymbolColumnFilter;
            if (!prevfilter.Equals(SymbolText))
                SymbolColumnFilter.ClearFilterConditions();
            if (!string.IsNullOrEmpty(SymbolText))
                SymbolColumnFilter.FilterConditions.Add(FilterComparisionOperator.Equals, SymbolText);
            else
                SymbolColumnFilter.ClearFilterConditions();
        }

        /// <summary>
        /// Gets the symbol text.
        /// </summary>
        /// <value>
        /// The symbol text.
        /// </value>
        public string SymbolText
        {
            get
            {
                return TextEditorSymbol.Text;
            }
        }

        /// <summary>
        /// Gets or sets the target quantity.
        /// </summary>
        /// <value>
        /// The target quantity.
        /// </value>
        public double Shares
        {
            get
            {
                return Convert.ToInt32(numericEditorShares.Value);//.Value;
            }
            set
            {
                numericEditorShares.Value = value;
            }
        }

        public string ClientMasterFund
        {
            get
            {
                return cmbFund.SelectedItem != null && cmbFund.SelectedIndex > 0 ? cmbFund.SelectedItem.ToString() : string.Empty;
            }
            set
            {
                cmbFund.Value = value;
            }
        }

        private void SetupLayoutForShortLocateGrid()
        {
            try
            {
                UltraGridGroup ugGroupSecurity;
                UltraGridGroup ugGroupBorrowAvailble;
                UltraGridGroup ugGroupBorrowAvailed;
                UltraGridGroup ugGroupStartOfDayBorrow;
                UltraGridGroup ugGroupAction;

                _shortLocatePreferences = Dataobj.GetShortLocatePreferences(CachedDataManager.GetInstance.LoggedInUser.CompanyUserID);

                foreach (UltraGridBand band in grdShortLocate.DisplayLayout.Bands)
                {

                    #region Groups
                    //ugGroupSecurity
                    ugGroupSecurity = band.Groups.Add(ShortLocateConstants.COL_Security);
                    ugGroupSecurity.Header.Caption = ShortLocateConstants.CAP_Security;
                    ugGroupSecurity.Header.Appearance.BackColor = Color.LightSteelBlue;
                    //ugGroupBorrowAvailble
                    ugGroupBorrowAvailble = band.Groups.Add(ShortLocateConstants.COL_BorrowAvailble);
                    ugGroupBorrowAvailble.Header.Caption = ShortLocateConstants.CAP_BorrowAvailble;
                    ugGroupBorrowAvailble.Header.Appearance.BackColor = Color.WhiteSmoke;
                    //ugGroupBorrowAvailed
                    ugGroupBorrowAvailed = band.Groups.Add(ShortLocateConstants.COL_BorrowAvailed);
                    ugGroupBorrowAvailed.Header.Caption = ShortLocateConstants.CAP_BorrowAvailed;
                    ugGroupBorrowAvailed.Header.Appearance.BackColor = Color.LightSteelBlue;
                    //ugGroupStartOfDayBorrow
                    ugGroupStartOfDayBorrow = band.Groups.Add(ShortLocateConstants.COL_StartOfDayBorrow);
                    ugGroupStartOfDayBorrow.Header.Caption = ShortLocateConstants.CAP_StartOfDayBorrow;
                    ugGroupStartOfDayBorrow.Header.Appearance.BackColor = Color.WhiteSmoke;
                    //ugGroupAction
                    ugGroupAction = band.Groups.Add(ShortLocateConstants.COL_Action);
                    ugGroupAction.Header.Caption = ShortLocateConstants.CAP_Action;
                    ugGroupAction.Header.Appearance.BackColor = Color.LightSteelBlue;

                    band.Override.AllowRowFiltering = DefaultableBoolean.True;
                    //  band.GroupHeaderLines = 2;
                    band.ColHeaderLines = 2;
                    #endregion

                    #region SecurityColumns
                    //Ticker Column
                    band.Columns[ShortLocateConstants.COL_Ticker].Header.Caption = ShortLocateConstants.CAP_Ticker;
                    band.Columns[ShortLocateConstants.COL_Ticker].CellActivation = Activation.NoEdit;
                    band.Columns[ShortLocateConstants.COL_Ticker].Header.VisiblePosition = 1;
                    band.Columns[ShortLocateConstants.COL_Ticker].Header.Fixed = true;
                    band.Columns[ShortLocateConstants.COL_Ticker].Group = ugGroupSecurity;
                    band.Columns[ShortLocateConstants.COL_Ticker].Width = 80;
                    band.Columns[ShortLocateConstants.COL_Ticker].Header.Appearance.BackColor = Color.LightSteelBlue;
                    //Broker Column
                    band.Columns[ShortLocateConstants.COL_Broker].Header.Caption = ShortLocateConstants.CAP_Broker;
                    band.Columns[ShortLocateConstants.COL_Broker].SortIndicator = SortIndicator.Disabled;
                    band.Columns[ShortLocateConstants.COL_Broker].CellActivation = Activation.NoEdit;
                    band.Columns[ShortLocateConstants.COL_Broker].Header.VisiblePosition = 2;
                    band.Columns[ShortLocateConstants.COL_Broker].Header.Fixed = true;
                    band.Columns[ShortLocateConstants.COL_Broker].Group = ugGroupSecurity;
                    band.Columns[ShortLocateConstants.COL_Broker].Width = 80;
                    band.Columns[ShortLocateConstants.COL_Broker].Header.Appearance.BackColor = Color.LightSteelBlue;
                    //LastPx Column
                    band.Columns[ShortLocateConstants.COL_LastPx].Header.Caption = ShortLocateConstants.CAP_LastPx;
                    band.Columns[ShortLocateConstants.COL_LastPx].CellActivation = Activation.NoEdit;
                    band.Columns[ShortLocateConstants.COL_LastPx].CellAppearance.TextHAlign = HAlign.Right;
                    band.Columns[ShortLocateConstants.COL_LastPx].Header.VisiblePosition = 3;
                    band.Columns[ShortLocateConstants.COL_LastPx].Group = ugGroupSecurity;
                    band.Columns[ShortLocateConstants.COL_LastPx].Width = 100;
                    if (_shortLocatePreferences.LastPxDecimal != 0)
                        band.Columns[ShortLocateConstants.COL_LastPx].Format = SetDecimalPalaces(_shortLocatePreferences.LastPxDecimal);
                    band.Columns[ShortLocateConstants.COL_LastPx].Header.Appearance.BackColor = Color.LightSteelBlue;
                    //TradeQuantity Column
                    band.Columns[ShortLocateConstants.COL_TradeQuantity].Header.Caption = ShortLocateConstants.CAP_TradeQuantity;
                    band.Columns[ShortLocateConstants.COL_TradeQuantity].CellActivation = Activation.AllowEdit;
                    band.Columns[ShortLocateConstants.COL_TradeQuantity].Header.VisiblePosition = 4;
                    band.Columns[ShortLocateConstants.COL_TradeQuantity].Header.Fixed = true;
                    band.Columns[ShortLocateConstants.COL_TradeQuantity].Group = ugGroupSecurity;
                    band.Columns[ShortLocateConstants.COL_TradeQuantity].CellAppearance.TextHAlign = HAlign.Right;
                    band.Columns[ShortLocateConstants.COL_TradeQuantity].Width = 100;
                    band.Columns[ShortLocateConstants.COL_TradeQuantity].Header.Appearance.BackColor = Color.LightSteelBlue;
                    band.Columns[ShortLocateConstants.COL_TradeQuantity].Format = "#,####0.";
                    band.Columns[ShortLocateConstants.COL_TradeQuantity].DefaultCellValue = 0;

                    //ClientMasterfund Column
                    band.Columns[ShortLocateConstants.COL_ClientMasterfund].Header.Caption = ShortLocateConstants.CAP_ClientMasterfund;
                    band.Columns[ShortLocateConstants.COL_ClientMasterfund].CellActivation = Activation.NoEdit;
                    band.Columns[ShortLocateConstants.COL_ClientMasterfund].Header.VisiblePosition = 5;
                    band.Columns[ShortLocateConstants.COL_ClientMasterfund].Header.Fixed = true;
                    band.Columns[ShortLocateConstants.COL_ClientMasterfund].Group = ugGroupSecurity;
                    band.Columns[ShortLocateConstants.COL_ClientMasterfund].Width = 100;
                    band.Columns[ShortLocateConstants.COL_ClientMasterfund].Header.Appearance.BackColor = Color.LightSteelBlue;
                    if (!CachedDataManager.GetInstance.IsShowMasterFundonShortLocate)
                        band.Columns[ShortLocateConstants.COL_ClientMasterfund].Hidden = true;
                    //   band.Columns["ClientMasterfund"].CellAppearance.BorderColor = Color.Black;
                    #endregion

                    #region BorrowAvailbleColumns
                    band.Columns[ShortLocateConstants.COL_BorrowSharesAvailable].Header.Caption = ShortLocateConstants.CAP_BorrowSharesAvailable;
                    band.Columns[ShortLocateConstants.COL_BorrowSharesAvailable].CellActivation = Activation.NoEdit;
                    band.Columns[ShortLocateConstants.COL_BorrowSharesAvailable].CellAppearance.TextHAlign = HAlign.Right;
                    band.Columns[ShortLocateConstants.COL_BorrowSharesAvailable].Header.VisiblePosition = 6;
                    band.Columns[ShortLocateConstants.COL_BorrowSharesAvailable].Header.Fixed = true;
                    band.Columns[ShortLocateConstants.COL_BorrowSharesAvailable].Group = ugGroupBorrowAvailble;
                    band.Columns[ShortLocateConstants.COL_BorrowSharesAvailable].Width = 130;
                    band.Columns[ShortLocateConstants.COL_BorrowSharesAvailable].Header.Appearance.BackColor = Color.WhiteSmoke;
                    band.Columns[ShortLocateConstants.COL_BorrowSharesAvailable].Format = "#,####0.###########";

                    if (_shortLocatePreferences.Rebatefees == ShortLocateRebateFee.BPS.ToString())
                        band.Columns[ShortLocateConstants.COL_BorrowRate].Header.Caption = ShortLocateConstants.CAP_BorrowRateBPS;
                    else
                        band.Columns[ShortLocateConstants.COL_BorrowRate].Header.Caption = ShortLocateConstants.CAP_BorrowRatePercentage;
                    band.Columns[ShortLocateConstants.COL_BorrowRate].CellActivation = Activation.NoEdit;
                    band.Columns[ShortLocateConstants.COL_BorrowRate].CellAppearance.TextHAlign = HAlign.Right;
                    band.Columns[ShortLocateConstants.COL_BorrowRate].Header.VisiblePosition = 7;
                    band.Columns[ShortLocateConstants.COL_BorrowRate].Header.Fixed = true;
                    band.Columns[ShortLocateConstants.COL_BorrowRate].Group = ugGroupBorrowAvailble;
                    band.Columns[ShortLocateConstants.COL_BorrowRate].Width = 100;
                    band.Columns[ShortLocateConstants.COL_BorrowRate].Header.Appearance.BackColor = Color.WhiteSmoke;
                    if (_shortLocatePreferences.RebatefeesDecimal != 0)
                        band.Columns[ShortLocateConstants.COL_BorrowRate].Format = SetDecimalPalaces(_shortLocatePreferences.RebatefeesDecimal);

                    band.Columns[ShortLocateConstants.COL_TotalBorrowAmount].Header.Caption = ShortLocateConstants.CAP_TotalBorrowAmount;
                    band.Columns[ShortLocateConstants.COL_TotalBorrowAmount].CellActivation = Activation.NoEdit;
                    band.Columns[ShortLocateConstants.COL_TotalBorrowAmount].CellAppearance.TextHAlign = HAlign.Right;
                    band.Columns[ShortLocateConstants.COL_TotalBorrowAmount].Header.VisiblePosition = 8;
                    band.Columns[ShortLocateConstants.COL_TotalBorrowAmount].Header.Fixed = true;
                    band.Columns[ShortLocateConstants.COL_TotalBorrowAmount].Group = ugGroupBorrowAvailble;
                    band.Columns[ShortLocateConstants.COL_TotalBorrowAmount].Width = 130;
                    band.Columns[ShortLocateConstants.COL_TotalBorrowAmount].Header.Appearance.BackColor = Color.WhiteSmoke;
                    if (_shortLocatePreferences.TotalAmountDecimal != 0)
                        band.Columns[ShortLocateConstants.COL_TotalBorrowAmount].Format = SetDecimalPalaces(_shortLocatePreferences.TotalAmountDecimal);

                    band.Columns[ShortLocateConstants.COL_BorrowerId].Header.Caption = ShortLocateConstants.CAP_BorrowerId;
                    band.Columns[ShortLocateConstants.COL_BorrowerId].CellActivation = Activation.NoEdit;
                    //  band.Columns["OSIOptionSymbol"].CellAppearance.TextHAlign = HAlign.Left;
                    band.Columns[ShortLocateConstants.COL_BorrowerId].Header.VisiblePosition = 9;
                    band.Columns[ShortLocateConstants.COL_BorrowerId].Header.Fixed = true;
                    band.Columns[ShortLocateConstants.COL_BorrowerId].Group = ugGroupBorrowAvailble;
                    band.Columns[ShortLocateConstants.COL_BorrowerId].Header.Appearance.BackColor = Color.WhiteSmoke;
                    band.Columns[ShortLocateConstants.COL_BorrowerId].Width = 100;
                    #endregion

                    #region BorrowAvailedColumns
                    band.Columns[ShortLocateConstants.COL_BorrowedShare].Header.Caption = ShortLocateConstants.CAP_BorrowedShare;
                    band.Columns[ShortLocateConstants.COL_BorrowedShare].CellActivation = Activation.NoEdit;
                    band.Columns[ShortLocateConstants.COL_BorrowedShare].CellAppearance.TextHAlign = HAlign.Right;
                    band.Columns[ShortLocateConstants.COL_BorrowedShare].Header.VisiblePosition = 10;
                    band.Columns[ShortLocateConstants.COL_BorrowedShare].Header.Fixed = true;
                    band.Columns[ShortLocateConstants.COL_BorrowedShare].Group = ugGroupBorrowAvailed;
                    band.Columns[ShortLocateConstants.COL_BorrowedShare].Width = 100;
                    band.Columns[ShortLocateConstants.COL_BorrowedShare].Header.Appearance.BackColor = Color.LightSteelBlue;
                    band.Columns[ShortLocateConstants.COL_BorrowedShare].Format = "#,####0.###########";

                    if (_shortLocatePreferences.Rebatefees == ShortLocateRebateFee.BPS.ToString())
                        band.Columns[ShortLocateConstants.COL_BorrowedRate].Header.Caption = ShortLocateConstants.CAP_BorrowedRateBPS;
                    else
                        band.Columns[ShortLocateConstants.COL_BorrowedRate].Header.Caption = ShortLocateConstants.CAP_BorrowedRatePercentage;
                    band.Columns[ShortLocateConstants.COL_BorrowedRate].CellActivation = Activation.NoEdit;
                    band.Columns[ShortLocateConstants.COL_BorrowedRate].CellAppearance.TextHAlign = HAlign.Right;
                    band.Columns[ShortLocateConstants.COL_BorrowedRate].Header.VisiblePosition = 11;
                    band.Columns[ShortLocateConstants.COL_BorrowedRate].Header.Fixed = true;
                    band.Columns[ShortLocateConstants.COL_BorrowedRate].Group = ugGroupBorrowAvailed;
                    band.Columns[ShortLocateConstants.COL_BorrowedRate].Width = 100;
                    band.Columns[ShortLocateConstants.COL_BorrowedRate].Header.Appearance.BackColor = Color.LightSteelBlue;
                    band.Columns[ShortLocateConstants.COL_BorrowedRate].Format = "#,####0.###########";

                    band.Columns[ShortLocateConstants.COL_TotalBorrowedAmount].Header.Caption = ShortLocateConstants.CAP_TotalBorrowedAmount;
                    band.Columns[ShortLocateConstants.COL_TotalBorrowedAmount].CellActivation = Activation.NoEdit;
                    band.Columns[ShortLocateConstants.COL_TotalBorrowedAmount].CellAppearance.TextHAlign = HAlign.Right;
                    band.Columns[ShortLocateConstants.COL_TotalBorrowedAmount].Header.VisiblePosition = 12;
                    band.Columns[ShortLocateConstants.COL_TotalBorrowedAmount].Header.Fixed = true;
                    band.Columns[ShortLocateConstants.COL_TotalBorrowedAmount].Group = ugGroupBorrowAvailed;
                    band.Columns[ShortLocateConstants.COL_TotalBorrowedAmount].Width = 130;
                    band.Columns[ShortLocateConstants.COL_TotalBorrowedAmount].Header.Appearance.BackColor = Color.LightSteelBlue;
                    band.Columns[ShortLocateConstants.COL_TotalBorrowedAmount].Format = "#,####0.###########";
                    #endregion

                    #region StartOfDayBorrowColumns
                    band.Columns[ShortLocateConstants.COL_SODBorrowshareAvailable].Header.Caption = ShortLocateConstants.CAP_SODBorrowshareAvailable;
                    band.Columns[ShortLocateConstants.COL_SODBorrowshareAvailable].CellActivation = Activation.NoEdit;
                    band.Columns[ShortLocateConstants.COL_SODBorrowshareAvailable].CellAppearance.TextHAlign = HAlign.Right;
                    band.Columns[ShortLocateConstants.COL_SODBorrowshareAvailable].Header.VisiblePosition = 13;
                    band.Columns[ShortLocateConstants.COL_SODBorrowshareAvailable].Header.Fixed = true;
                    band.Columns[ShortLocateConstants.COL_SODBorrowshareAvailable].Group = ugGroupStartOfDayBorrow;
                    band.Columns[ShortLocateConstants.COL_SODBorrowshareAvailable].Width = 130;
                    band.Columns[ShortLocateConstants.COL_SODBorrowshareAvailable].Format = "#,####0.###########";
                    band.Columns[ShortLocateConstants.COL_SODBorrowshareAvailable].Header.Appearance.BackColor = Color.WhiteSmoke;

                    if (_shortLocatePreferences.Rebatefees == ShortLocateRebateFee.BPS.ToString())
                        band.Columns[ShortLocateConstants.COL_SODBorrowRate].Header.Caption = ShortLocateConstants.CAP_SODBorrowRateBPS;
                    else
                        band.Columns[ShortLocateConstants.COL_SODBorrowRate].Header.Caption = ShortLocateConstants.CAP_SODBorrowRatePercentage;
                    band.Columns[ShortLocateConstants.COL_SODBorrowRate].CellActivation = Activation.NoEdit;
                    band.Columns[ShortLocateConstants.COL_SODBorrowRate].CellAppearance.TextHAlign = HAlign.Right;
                    band.Columns[ShortLocateConstants.COL_SODBorrowRate].Header.VisiblePosition = 14;
                    band.Columns[ShortLocateConstants.COL_SODBorrowRate].Header.Fixed = true;
                    band.Columns[ShortLocateConstants.COL_SODBorrowRate].Group = ugGroupStartOfDayBorrow;
                    band.Columns[ShortLocateConstants.COL_SODBorrowRate].Width = 110;
                    band.Columns[ShortLocateConstants.COL_SODBorrowRate].Format = "#,####0.###########";
                    band.Columns[ShortLocateConstants.COL_SODBorrowRate].Header.Appearance.BackColor = Color.WhiteSmoke;
                    #endregion

                    #region ActionColumns
                    band.Columns[ShortLocateConstants.COL_StatusSource].Header.Caption = ShortLocateConstants.CAP_StatusSource;
                    band.Columns[ShortLocateConstants.COL_StatusSource].CellActivation = Activation.NoEdit;
                    band.Columns[ShortLocateConstants.COL_StatusSource].Header.VisiblePosition = 14;
                    band.Columns[ShortLocateConstants.COL_StatusSource].Header.Fixed = true;
                    band.Columns[ShortLocateConstants.COL_StatusSource].Group = ugGroupAction;
                    band.Columns[ShortLocateConstants.COL_StatusSource].Width = 100;
                    band.Columns[ShortLocateConstants.COL_StatusSource].Header.Appearance.BackColor = Color.LightSteelBlue;
                    #endregion

                    ugGroupSecurity.Header.Fixed = true;
                    grdShortLocate.DisplayLayout.Bands[0].Override.AllowColMoving = AllowColMoving.NotAllowed;
                    grdShortLocate.DisplayLayout.Bands[0].Override.HeaderAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
                    grdShortLocate.DisplayLayout.Bands[0].Columns[ShortLocateConstants.COL_TradeQuantity].Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                    grdShortLocate.DisplayLayout.Bands[0].Columns[ShortLocateConstants.COL_NirvanaLocateID].Hidden = true;
                    grdShortLocate.DisplayLayout.Bands[0].Columns[ShortLocateConstants.COL_NirvanaLocateID].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    grdShortLocate.DisplayLayout.Override.WrapHeaderText = Infragistics.Win.DefaultableBoolean.True;
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
        /// Set Fund View
        /// </summary>
        private void SetFundView()
        {
            try
            {
                //IsShowMasterFundonTT preference is true then Mastrefund (Fund) drop down
                //Show label and set label based on preference IsShowmasterFundAsClient

                if (CachedDataManager.GetInstance.IsShowMasterFundonShortLocate)
                {
                    cmbFund.Visible = true;
                    Dictionary<int, string> fundsDict = new Dictionary<int, string>();
                    fundsDict.Add(int.MinValue, ShortLocateConstants.SELECT);

                    //Getting masterfunds from cache and then sorting alphabatical order
                    var fundsDictForCache = CommonDataCache.CachedDataManager.GetInstance.GetUserMasterFunds();
                    var sortedDict = fundsDictForCache.OrderBy(x => x.Value).ToDictionary(x => x.Key, y => y.Value);
                    foreach (var item in sortedDict)
                    {
                        fundsDict.Add(item.Key, item.Value);
                    }
                    var fundsValueList = fundsDict.ToValueList();
                    if (fundsValueList != null)
                    {
                        cmbFund.ValueList = fundsValueList;
                        cmbFund.SelectedText = ShortLocateConstants.SELECT;
                    }
                }
                else
                {
                    cmbFund.Enabled = false;
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


        private void grdShortLocate_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {

                Prana.ShortLocate.Classes.Summary.ShortLocateSummarySetting(e);
                grdShortLocate.DisplayLayout.Override.SummaryDisplayArea = SummaryDisplayAreas.Bottom;
                SetupLayoutForShortLocateGrid();
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

        private void ShortLocate_Load(object sender, EventArgs e)
        {
            try
            {
                ShortLocateDataManager.GetInstance.ColorChangeonRow += shortLocate_ColorChangeonRow;
                BindingList<ShortLocateOrder> order = new BindingList<ShortLocateOrder>();
                order = ShortLocateDataManager.GetInstance.GetShortLocateCollection(ClientMasterFund);
                grdShortLocate.DataSource = order;
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
        /// This event is fired when data is updated on blotter and changes the color of every borrowed share cell text to red in the case of overborrow.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void shortLocate_ColorChangeonRow(object sender, EventArgs<int> e)
        {
            try
            {
                if (grdShortLocate!=null && grdShortLocate.Rows != null)
                {
                    foreach (UltraGridRow ugr in grdShortLocate.Rows)
                    {
                        if (int.Parse(ugr.GetCellValue(ShortLocateConstants.COL_NirvanaLocateID).ToString()) == e.Value)
                        {
                            if (int.Parse(ugr.GetCellValue(ShortLocateConstants.COL_BorrowedShare).ToString()) > int.Parse(ugr.GetCellValue(ShortLocateConstants.COL_SODBorrowshareAvailable).ToString()))
                            {
                                grdShortLocate.Rows[ugr.Index].Cells[ShortLocateConstants.COL_BorrowedShare].Appearance.ForeColor = Color.Red;
                            }
                            else
                            {
                                grdShortLocate.Rows[ugr.Index].Cells[ShortLocateConstants.COL_BorrowedShare].Appearance.ForeColor = Color.Black;
                            }
                            break;
                        }
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

        private void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                if (CachedDataManager.GetInstance.IsShowMasterFundonShortLocate)
                {
                    if (ClientMasterFund == string.Empty)
                    {
                        MessageBox.Show("Master fund is not selected", "Short Locate", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }
                SlImport = new ShortLocateImport();
                SlImport.BindCombo(ClientMasterFund);
                SlImport.ShowDialog();
                grdShortLocate.DataSource = null;
                grdShortLocate.DataSource = ShortLocateDataManager.GetInstance.GetShortLocateCollection(ClientMasterFund);
                if (grdShortLocate.DisplayLayout.Bands[0].Columns.Count > 0)
                {
                    UltraGridColumn CompanyMasterfundColumn = grdShortLocate.DisplayLayout.Bands[0].Columns[ShortLocateConstants.COL_ClientMasterfund];
                    ColumnFilter CompanyMasterfundColumnFilter = CompanyMasterfundColumn.Band.ColumnFilters[CompanyMasterfundColumn];
                    CompanyMasterfundColumnFilter.ClearFilterConditions();
                    if (!string.IsNullOrEmpty(ClientMasterFund))
                        CompanyMasterfundColumnFilter.FilterConditions.Add(FilterComparisionOperator.Equals, ClientMasterFund);
                }
                grdShortLocate.UpdateData();
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

        private void ultraPictureBoxEraser_Click(object sender, EventArgs e)
        {
            TextEditorSymbol.Value = "";
            numericEditorShares.Value = 0;
            grdShortLocate.DisplayLayout.Bands[0].ColumnFilters.ClearAllFilters();
        }

        /// <summary>
        /// This event is fired when short locate module is opened and changes the color of every borrowed share cell text to red in the case of overborrow.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdShortLocate_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                if (e.Row.Cells.Exists(ShortLocateConstants.COL_BorrowedShare))
                {
                    if (int.Parse(e.Row.GetCellValue(ShortLocateConstants.COL_BorrowedShare).ToString()) > int.Parse(e.Row.GetCellValue(ShortLocateConstants.COL_SODBorrowshareAvailable).ToString()))
                    {
                        grdShortLocate.Rows[e.Row.Index].Cells[ShortLocateConstants.COL_BorrowedShare].Appearance.ForeColor = Color.Red;
                    }
                    else
                    {
                        grdShortLocate.Rows[e.Row.Index].Cells[ShortLocateConstants.COL_BorrowedShare].Appearance.ForeColor = Color.Black;
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
        private void grdShortLocate_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                UltraGrid control = sender as UltraGrid;
                UIElement controlElement = control != null ? control.DisplayLayout.UIElement : null;
                UIElement elementAtPoint = controlElement != null ? controlElement.ElementFromPoint(e.Location) : null;

                UltraGridRow row = null;
                while (elementAtPoint != null)
                {
                    RowUIElement rowElement = elementAtPoint as RowUIElement;
                    if (rowElement != null)
                    {
                        row = rowElement.Row;
                        break;
                    }

                    elementAtPoint = elementAtPoint.Parent;
                }
                LaunchFormEventArgs eventArgs = null;
                if (row != null)
                {
                    OrderSingle os = new OrderSingle();
                    ShortLocateOrder slOrder = row.ListObject as ShortLocateOrder;
                    slOrder.TradeQuantity = Math.Truncate(slOrder.TradeQuantity);
                    os.MasterFund = slOrder.ClientMasterfund;
                    os.Symbol = slOrder.Ticker;
                    os.ShortLocateParameter = GetShortLocateParameterFromShortLocateOrder(slOrder);

                    int BrokerID = int.MinValue;
                    BorrowerAccountMappingDict = ShortLocateDataManager.BrokerAccountMapping;
                    Dictionary<int, string> dictBrokers = CommonDataCache.CachedDataManager.GetInstance.GetAllThirdPartiesWithShortName();
                    BrokerID = dictBrokers.FirstOrDefault(x => x.Value == os.ShortLocateParameter.Broker).Key;

                    if (BorrowerAccountMappingDict.ContainsKey(BrokerID.ToString()))
                    {
                        os.Account = CachedDataManager.GetInstance.GetAccountText(Convert.ToInt32(BorrowerAccountMappingDict[BrokerID.ToString()]));
                    }
                    else
                        os.Account = int.MinValue.ToString();
                    if (slOrder.TradeQuantity != 0)
                        os.Quantity = slOrder.TradeQuantity;
                    else
                    {
                        os.Quantity = slOrder.BorrowSharesAvailable;
                        os.ShortLocateParameter.BorrowQuantity = slOrder.BorrowSharesAvailable;
                    }
                    os.OrderSide = "Sell short";
                    os.OrderSideTagValue = "5";
                    os.Price = 0;

                    eventArgs = new LaunchFormEventArgs(os);
                    if (MouseDoubleClickOnRow != null)
                        MouseDoubleClickOnRow(sender, eventArgs);
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

        private ShortLocateListParameter GetShortLocateParameterFromShortLocateOrder(ShortLocateOrder order)
        {
            ShortLocateListParameter param = new ShortLocateListParameter();
            try
            {
                param.BorrowerId = order.BorrowerId;
                param.BorrowQuantity = order.TradeQuantity;
                param.BorrowRate = order.BorrowRate;
                param.BorrowSharesAvailable = order.BorrowSharesAvailable;
                param.Broker = order.Broker;
                param.NirvanaLocateID = order.NirvanaLocateID;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return param;
        }

        /// <summary>
        /// To show the tooltip on mouse hover event when export functiallity is disabled.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraPictureBoxDownload_MouseHover(object sender, EventArgs e)
        {
            try
            {
                Infragistics.Win.UltraWinToolTip.UltraToolTipInfo toolTipInfo = this.ultraToolTipManager1.GetUltraToolTip(ultraPictureBoxDownload);
                toolTipInfo.ToolTipText = ExportToolTipMsg;
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

        private void ultraPictureBoxDownload_Click(object sender, EventArgs e)
        {
            try
            {
                ShortLocateDownLoadType FormatObject = new ShortLocateDownLoadType();
                FormatObject.ShowDialog();
                int FormatChosen = int.MinValue;
                FormatChosen = FormatObject.FormatReturn();

                if (FormatChosen != int.MinValue)
                {
                    if (FormatChosen == 1)
                    {
                        FormatObject.ExcelExport(this.grdShortLocate);
                    }
                    else
                    {
                        FormatObject.CSVExport(this.grdShortLocate);
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

        private void ShortLocate_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                SlImport = null;
                MouseDoubleClickOnRow = null;
                TextEditorSymbol.Leave -= TextEditorSymbol_Leave;
                numericEditorShares.Leave -= numericEditorShares_Leave;
                ShortLocateDataManager.GetInstance.ColorChangeonRow -= shortLocate_ColorChangeonRow;
                InstanceManager.ReleaseInstance(typeof(ShortLocate));
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

        private void grdShortLocate_BeforeColumnChooserDisplayed(object sender, BeforeColumnChooserDisplayedEventArgs e)
        {
            try
            {
                e.Cancel = true;
                (this.FindForm()).AddCustomColumnChooser(this.grdShortLocate);
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

        private string SetDecimalPalaces(double noOfDecimal)
        {
            string result = "#,####0.";
            try
            {
                for (int count = 0; count < noOfDecimal; count++)
                {
                    result = result + '#';
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
            return result;
        }

        private void cmbFund_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (grdShortLocate.DisplayLayout.Bands[0].Columns.Count > 0)
                {
                    UltraGridColumn CompanyMasterfundColumn = grdShortLocate.DisplayLayout.Bands[0].Columns[ShortLocateConstants.COL_ClientMasterfund];
                    ColumnFilter CompanyMasterfundColumnFilter = CompanyMasterfundColumn.Band.ColumnFilters[CompanyMasterfundColumn];
                    CompanyMasterfundColumnFilter.ClearFilterConditions();
                    if (!string.IsNullOrEmpty(ClientMasterFund))
                        CompanyMasterfundColumnFilter.FilterConditions.Add(FilterComparisionOperator.Equals, ClientMasterFund);
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

        private void ultraPictureBoxEraser_MouseHover(object sender, EventArgs e)
        {
            try
            {
                Infragistics.Win.UltraWinToolTip.UltraToolTipInfo toolTipInfo = this.ultraToolTipManager1.GetUltraToolTip(ultraPictureBoxEraser);
                toolTipInfo.ToolTipText = "Click here to remove filter";
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

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                grdShortLocate.DataSource = null;
                TextEditorSymbol.Value = "";
                numericEditorShares.Value = 0;
                if (CachedDataManager.GetInstance.IsShowMasterFundonShortLocate && cmbFund.SelectedText != ShortLocateConstants.SELECT)
                {
                    cmbFund.SelectedText = cmbFund.SelectedText != "" ? ShortLocateConstants.SELECT : cmbFund.SelectedText;
                }
                grdShortLocate.DataSource = ShortLocateDataManager.GetInstance.GetShortLocateCollection(ClientMasterFund);
                foreach (UltraGridRow ugr in grdShortLocate.Rows)
                {
                    if (int.Parse(ugr.GetCellValue(ShortLocateConstants.COL_BorrowedShare).ToString()) > int.Parse(ugr.GetCellValue(ShortLocateConstants.COL_SODBorrowshareAvailable).ToString()))
                    {
                        grdShortLocate.Rows[ugr.Index].Cells[ShortLocateConstants.COL_BorrowedShare].Appearance.ForeColor = Color.Red;
                    }
                    else
                    {
                        grdShortLocate.Rows[ugr.Index].Cells[ShortLocateConstants.COL_BorrowedShare].Appearance.ForeColor = Color.Black;
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

        /// <summary>
        ///  used To export data for automation.
        /// </summary>
        /// <param name="gridName"></param>
        /// <param name="WindowName"></param>
        /// <param name="tabName"></param>
        /// <param name="filePath"></param>
        public void ExportData(string gridName, string WindowName, string tabName, string filePath)
        {
            try
            {
                // Create a new instance of the exporter
                UltraGridExcelExporter exporter = new UltraGridExcelExporter();
                string directoryPath = Path.GetDirectoryName(filePath);
                if (!System.IO.Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);
                // Perform the export
                if (gridName == "grdShortLocate")
                    exporter.Export(grdShortLocate, filePath);
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
    }
}
