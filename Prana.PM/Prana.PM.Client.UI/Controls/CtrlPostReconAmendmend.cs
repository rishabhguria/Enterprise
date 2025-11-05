using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes;
using Prana.BusinessObjects.Constants;
using Prana.BusinessObjects.PositionManagement;
using Prana.ClientCommon;
using Prana.ClientCommon.BLL;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.PM.Client.UI.Classes;
using Prana.PubSubService.Interfaces;
using Prana.Utilities;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.UI.ImportExportUtilities;
using Prana.Utilities.UI.UIUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace Prana.PM.Client.UI.Controls
{
    public partial class CtrlPostReconAmendmend : UserControl, ILiveFeedCallback
    {
        public CtrlPostReconAmendmend()
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(CustomThemeHelper.WHITELABELTHEME) && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
            {
                SetButtonsColor();
            }
        }

        private void SetButtonsColor()
        {
            try
            {
                btnRestoreDefaultAlgo.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnRestoreDefaultAlgo.ForeColor = System.Drawing.Color.White;
                btnRestoreDefaultAlgo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnRestoreDefaultAlgo.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnRestoreDefaultAlgo.UseAppStyling = false;
                btnRestoreDefaultAlgo.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnGetData.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnGetData.ForeColor = System.Drawing.Color.White;
                btnGetData.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnGetData.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnGetData.UseAppStyling = false;
                btnGetData.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnPostTransaction.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnPostTransaction.ForeColor = System.Drawing.Color.White;
                btnPostTransaction.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnPostTransaction.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnPostTransaction.UseAppStyling = false;
                btnPostTransaction.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnCancel.BackColor = System.Drawing.Color.FromArgb(140, 5, 5);
                btnCancel.ForeColor = System.Drawing.Color.White;
                btnCancel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnCancel.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnCancel.UseAppStyling = false;
                btnCancel.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnSave.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
                btnSave.ForeColor = System.Drawing.Color.White;
                btnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnSave.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnSave.UseAppStyling = false;
                btnSave.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnReverse.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnReverse.ForeColor = System.Drawing.Color.White;
                btnReverse.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnReverse.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnReverse.UseAppStyling = false;
                btnReverse.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnPreview.BackColor = System.Drawing.Color.FromArgb(140, 5, 5);
                btnPreview.ForeColor = System.Drawing.Color.White;
                btnPreview.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnPreview.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnPreview.UseAppStyling = false;
                btnPreview.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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
        /// 
        /// </summary>
        /// 
        UltraGridCell _lastModifiedCell = null;
        Dictionary<string, Infragistics.Win.UltraWinToolTip.UltraToolTipInfo> _dictToolTipInfo = new Dictionary<string, Infragistics.Win.UltraWinToolTip.UltraToolTipInfo>();

        DateTime _unwindingStartDate = DateTimeConstants.MinValue;
        private bool _isCommentsUpdated = false;
        private bool _isUnwindingClosingTobeDone = false;
        private double _markPriceForAccountSymbol = 0.00;

        const string SIDEBUY = "BUY";
        const string SIDEBUYTOCLOSE = "BUY TO CLOSE";
        const string SIDEBUYTOOPEN = "BUY TO OPEN";
        const string SIDESELL = "SELL";
        const string SIDESELLSHORT = "SELL SHORT";
        const string SIDESELLTOOPEN = "SELL TO SHORT";

        public event EventHandler launchForm;
        static int _userID = int.MinValue;
        static string _postReconLayoutFilePath = string.Empty;
        static string _postReconLayoutDirectoryPath = string.Empty;
        static PostReconLayout _postReconLayout = null;
        bool _grdDataStartUp = true;
        static int _accountID = int.MinValue;
        //to cache the unrealized pnl in case of amendments of trade
        double _taxLotUnRealizedPNL = double.MinValue;

        //bool _isCloseTradeInitialized = false;


        //public delegate void FormCloseHandler();
        public event FormClosedEventHandler formCloseHandler;
        EventHandler UpdateCommentsFromPostReconAmendmentsInReconOutput;
        //string _statusMessage = string.Empty;
        const string COL_AccountValue = "AccountValue";
        const string COL_ClosingTradeDate = "ClosingTradeDate";

        private DataTable dtMarkPrice = new DataTable();
        ProxyBase<IAllocationManager> _allocationServices = null;

        DuplexProxyBase<ISubscription> _proxy;

        DuplexProxyBase<IPricingService> _pricingServicesProxy = null;

        BackgroundWorker backgroundWorker = new BackgroundWorker();

        //public delegate void DisableEnableFormHandler(string message, bool Flag, bool TimerFlag);
        //public event DisableEnableFormHandler DisableEnableParentForm;

        public ProxyBase<IAllocationManager> AllocationServices
        {
            set { _allocationServices = value; }
        }

        public static PostReconLayout PostReconLayout
        {
            get
            {
                if (_postReconLayout == null)
                {
                    _postReconLayout = GetPostReconLayout();
                }
                return _postReconLayout;
            }
        }
        ProxyBase<IClosingServices> _closingServices = null;
        ClosingPreferences preferences = null;

        /// <summary>
        /// 
        /// </summary>
        public ProxyBase<IClosingServices> ClosingServices
        {
            set { _closingServices = value; }
        }

        string _quantityColumnFormat = "0.00000000";
        string _currencyColumnFormat = "0.00000000";

        #region UI methods
        private void btnPreview_Click(object sender, EventArgs e)
        {
            try
            {
                //check for account lock
                if (!IsAccountLocked())
                {
                    return;
                }
                //_isCloseTradeInitialized = true;

                ClosingData ClosedData = new ClosingData();

                PostTradeEnums.CloseTradeAlogrithm closingAlgo = (PostTradeEnums.CloseTradeAlogrithm)Enum.Parse(typeof(PostTradeEnums.CloseTradeAlogrithm), lblAccountingRuleValue.Text);

                PostTradeEnums.SecondarySortCriteria secondarySort = (PostTradeEnums.SecondarySortCriteria)Enum.Parse(typeof(PostTradeEnums.SecondarySortCriteria), lblSecondaySortValue.Text);

                List<TaxLot> buyTaxLotsAndPositions = GetBuyTaxlotsFromPositionGrid();

                List<TaxLot> SellTaxLotsAndPositions = GetSellTaxlotsFromPositionGrid();
                if (_lastModifiedCell != null && _lastModifiedCell.Value != null && !_lastModifiedCell.Value.ToString().Equals(PostTradeEnums.CloseTradeAlogrithm.NONE))
                {
                    closingAlgo = (PostTradeEnums.CloseTradeAlogrithm)Enum.Parse(typeof(PostTradeEnums.CloseTradeAlogrithm), _lastModifiedCell.Value.ToString());
                    TaxLot taxlot = (TaxLot)_lastModifiedCell.Row.ListObject;
                    //http://jira.nirvanasolutions.com:8080/browse/CHMW-1964
                    // [Post Recon Amendments] Closing single row with Preview is taking into account Secondary Sort criteria set for general closing
                    //Secondary sort should be none if closing algo is set in row manually
                    secondarySort = PostTradeEnums.SecondarySortCriteria.None;
                    if (taxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_Sell) || taxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_Sell_Closed))
                    {
                        SellTaxLotsAndPositions.Clear();
                        SellTaxLotsAndPositions.Add(taxlot);
                    }
                    else if (taxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy_Closed) || taxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy_Cover))
                    {
                        buyTaxLotsAndPositions.Clear();
                        buyTaxLotsAndPositions.Add(taxlot);
                    }
                }
                if (buyTaxLotsAndPositions.Count > 0 && SellTaxLotsAndPositions.Count > 0 && !(closingAlgo.Equals(PostTradeEnums.CloseTradeAlogrithm.NONE)))
                {
                    bool isAutoCloseStrategy = chkBxIsAutoCloseStrategy.Checked;

                    ClosingParameters closingParams = new ClosingParameters();
                    //buyTaxLotsAndPositions, sellTaxLotsAndPositions, algorithm = selected from algo drop-down, IsShortWithBuyAndBuyToCover=false, IsSellWithBuyToClose = false, isManual = true(not from preset algo), isDragDrop = false, isFromServer = false, SecondarySortCriteria=None, isVirtualClosingPopulate = true, isOverrideWithUserClosing = false ,isMatchStrategy=according to checkbox 

                    closingParams.BuyTaxLotsAndPositions = buyTaxLotsAndPositions;
                    closingParams.SellTaxLotsAndPositions = SellTaxLotsAndPositions;
                    closingParams.Algorithm = closingAlgo;
                    closingParams.IsShortWithBuyAndBuyToCover = false;
                    closingParams.IsSellWithBuyToClose = false;
                    closingParams.IsDragDrop = false;
                    closingParams.IsFromServer = false;
                    closingParams.SecondarySort = secondarySort;
                    closingParams.IsVirtualClosingPopulate = true;
                    closingParams.IsOverrideWithUserClosing = false;
                    closingParams.IsMatchStrategy = !isAutoCloseStrategy;

                    if (PostReconClosingData.TaxlotIDList.Length > 0)
                    {
                        string[] VirtualUnwindedTaxlots = PostReconClosingData.TaxlotIDList.ToString().Split(',');
                        closingParams.VirtualUnwidedTaxlots = VirtualUnwindedTaxlots.ToList();
                    }

                    if (closingAlgo.Equals(PostTradeEnums.CloseTradeAlogrithm.PRESET))
                    {

                        closingParams.IsManual = false;
                        //ClosedData = _closingServices.InnerChannel.AutomaticClosingOnManualOrPresetBasis(SellTaxLotsAndPositions, buyTaxLotsAndPositions, closingAlgo, false, false, false, false, false, secondarySort, true, false, !isAutoCloseStrategy);
                    }
                    else
                    {
                        closingParams.IsManual = true;
                        // ClosedData = _closingServices.InnerChannel.AutomaticClosingOnManualOrPresetBasis(buyTaxLotsAndPositions, SellTaxLotsAndPositions, closingAlgo, false, false, true, false, false, secondarySort, true, false, !isAutoCloseStrategy);
                    }
                    ClosedData = _closingServices.InnerChannel.AutomaticClosingOnManualOrPresetBasis(closingParams);
                }
                bool isError = CheckForCloseTradeError(ClosedData);

                if (!isError)
                {
                    if (ClosedData.ClosedPositions.Count > 0)
                        PostReconClosingData.IsUnsavedChanges = true;
                    PostReconClosingData.UpdateRepository(ClosedData);
                    PostReconClosingData.UpdateUnsavedClosingData(ClosedData);
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
        /// Check for close trade error in closed data
        /// </summary>
        /// <param name="ClosedData"></param>
        /// <returns></returns>
        private bool CheckForCloseTradeError(ClosingData ClosedData)
        {
            try
            {
                if (ClosedData != null)
                {
                    if (ClosedData.IsNavLockFailed)
                    {
                        MessageBox.Show(ClosedData.ErrorMsg.ToString(), "Nav Lock", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return true;
                    }
                    if (ClosedData.ClosedPositions.Count > 0)
                    {
                        //show warning message if closing algorithm is PRESET, in this case data will be closed for accounts which have invalid secondary sort criteria
                        if (!ClosedData.ErrorMsg.ToString().Equals(string.Empty))
                        {
                            MessageBox.Show(this, ClosedData.ErrorMsg.ToString(), "Close Trade Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return true;
                        }
                        else
                        {
                            //InformationMessageBox.Display("Close Trade Data Saved!!!");
                            return false;
                        }
                    }
                    else
                    {
                        //show warning message if closing algorithm is not PRESET, in this case data will be closed if valid secondary sort criteria given from the closing UI
                        if (!ClosedData.ErrorMsg.ToString().Equals(string.Empty))
                        {
                            MessageBox.Show(this, ClosedData.ErrorMsg.ToString(), "Close Trade Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return true;
                        }
                        else
                        {
                            InformationMessageBox.Display("Nothing to Close");
                            return true;
                        }
                    }
                }
                else
                    InformationMessageBox.Display("Nothing to Close");
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
            return false;
        }

        /// <summary>
        /// Get all taxlots from grid which have all sell trades (Sell, SellToOpen, SellToClose, SellPlus)
        /// </summary>
        /// <returns></returns>
        private List<TaxLot> GetSellTaxlotsFromPositionGrid()
        {
            List<TaxLot> listSellTaxlots = new List<TaxLot>();
            try
            {
                UltraGridRow[] grdFilteredRows = grdData.Rows.GetFilteredInNonGroupByRows();
                foreach (UltraGridRow row in grdFilteredRows)
                {
                    TaxLot taxlot = (TaxLot)row.ListObject;

                    if (taxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_Sell) || taxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_Sell_Closed) || taxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_Sell_Open) || taxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_SellPlus) || taxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_SellShort))
                    {
                        listSellTaxlots.Add(taxlot);
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
            return listSellTaxlots;
        }

        /// <summary>
        /// Get all taxlots from grid which have all buy trades (Buy, ButToOpen, BuyToClose, BuyToCover,BuyMinus)
        /// </summary>
        /// <returns></returns>
        private List<TaxLot> GetBuyTaxlotsFromPositionGrid()
        {
            List<TaxLot> listBuyTaxlots = new List<TaxLot>();
            try
            {
                UltraGridRow[] grdFilteredRows = grdData.Rows.GetFilteredInNonGroupByRows();
                foreach (UltraGridRow row in grdFilteredRows)
                {
                    TaxLot taxlot = (TaxLot)row.ListObject;

                    if (taxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy) || taxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy_Closed) || taxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy_Open) || taxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy_Cover) || taxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_BuyMinus))
                    {
                        listBuyTaxlots.Add(taxlot);
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
            return listBuyTaxlots;
        }

        /// <summary>
        /// Revert the preview closing done by user and fetch the open and closed positions of symbol+account
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnReverse_Click(object sender, EventArgs e)
        {
            try
            {
                PostReconClosingData.IsUnsavedChanges = false;
                AmendmentsHelper.ClearAmendments();

                bool isReverseToBEeDone = false;

                if (sender == null)
                {
                    //to determine if the call is because of trade server connect disconnect
                    isReverseToBEeDone = true;
                }
                else if (MessageBox.Show(this, "Do you want to reverse ?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    isReverseToBEeDone = true;
                }
                if (isReverseToBEeDone)
                {
                    btnGetData_Click(null, null);
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
        /// Save the closed data in database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                DisableEnableForm(false);
                List<Position> lstPosition = GetClosedDataInformation();
                if (_unwindingStartDate != DateTimeConstants.MinValue || AmendmentsHelper.IsAmendmentsToSave() || PostReconClosingData.IsUnsavedChanges || _isCommentsUpdated)
                {
                    //check for account lock, check if there are  unsaved changes
                    if (!IsAccountLocked())
                    {
                        DisableEnableForm(true);
                        return;
                    }
                    UpdateCommentsInReconOutput();
                    object[] arguments = new object[1];
                    arguments[0] = lstPosition;
                    bgWorkerUnwindingClosing.RunWorkerAsync(arguments);
                    if (grdData.DisplayLayout.Bands[0].Columns.Exists(ClosingConstants.COL_ClosingAlgoChooser))
                    {
                        grdData.DisplayLayout.Bands[0].Columns[ClosingConstants.COL_ClosingAlgoChooser].NullText = lblAccountingRuleValue.Text;
                    }
                }
                else
                {
                    InformationMessageBox.Display("Nothing to Save");
                    DisableEnableForm(true);
                }
                SetColorAndFormattingForPNLFields();
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

        private void UpdateCommentsInReconOutput()
        {
            try
            {
                #region Update Comments in ReconOutput UI
                //CHMW-1620 [Closing] - Add Comments field in PostReconAmendenmtsUI
                ListEventAargs listEventAargs = new BusinessObjects.ListEventAargs();
                listEventAargs.listOfValues.Add(tbComments.Text);
                UpdateCommentsFromPostReconAmendmentsInReconOutput(this, listEventAargs);
                _isCommentsUpdated = false;
                #endregion
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
        /// Create template for closing and unwinding
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="symbol"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="accountID"></param>
        /// <param name="closingAlgo"></param>
        /// <returns></returns>
        //private ClosingTemplate CreateTemplate(string columnName, string symbol, DateTime startDate, DateTime endDate, int accountID, string closingAlgo)
        //{
        //    ClosingTemplate template = new ClosingTemplate();
        //    try
        //    {
        //        template.FromDate = startDate;

        //        template.ToDate = endDate;

        //        template.ListAccountFliters.Add(accountID);

        //        template.ClosingMeth.ClosingAlgo = (PostTradeEnums.CloseTradeAlogrithm)Enum.Parse(typeof(PostTradeEnums.CloseTradeAlogrithm), closingAlgo, true);

        //        CustomCondition condition = new CustomCondition();

        //        condition.ColumnName = columnName;

        //        condition.ConditionOperatorType = EnumDescriptionAttribute.ConditionOperator.Equals;

        //        condition.compareValue = symbol;


        //        List<CustomCondition> lstCustomConditions = new List<CustomCondition>();

        //        lstCustomConditions.Add(condition);

        //        template.DictCustomConditions.Add(columnName, lstCustomConditions);

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
        //    return template;
        //}
        /// <summary>
        /// Close the current form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show(this, "Do you want to close ?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.Yes)
                {
                    if (formCloseHandler != null)
                    {
                        formCloseHandler(sender, new FormClosedEventArgs(CloseReason.None));
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

        //#region Open Trades columns

        //private const string COLUMN_LEVEL1NAME = "Level1Name";
        //private const string COLUMN_AUECLOCALDATE = "AUECLocalDate";
        //private const string COLUMN_ORIGPURCHASEDATE = "OriginalPurchaseDate";
        //private const string COLUMN_TAXLOTQTY = "TaxLotQty";
        //private const string COLUMN_AVGPRICE = "AvgPrice";
        //private const string COLUMN_LEVEL2NAME = "Level2Name";
        //private const string COLUMN_ORDERSIDE = "OrderSide";
        //private const string COLUMN_QUANTITYTOCLOSE = "TaxLotQtyToClose";

        //private const string CAPTION_LEVEL1NAME = "Account";
        //private const string CAPTION_AUECLOCALDATE = "Trade Date";
        //private const string CAPTION_ORIGPURCHASEDATE = "Original Purchase Date";
        //private const string CAPTION_TAXLOTQTY = "Quantity";
        //private const string CAPTION_AVGPRICE = "Unit Cost";
        //private const string CAPTION_LEVEL2NAME = "Strategy";
        //private const string CAPTION_ORDERSIDE = "Side";
        //private const string CAPTION_QUANTITYTOCLOSE = "Close Quantity";
        //#endregion

        //#region Grid Columns for Close Trades

        //const string COL_ID = "ID";

        //const string COL_StartDate = "StartDate";
        //const string COL_LastActivityDate = "LastActivityDate";
        //const string COL_PositionTag = "PositionalTag";
        //const string COL_ClosingTag = "ClosingPositionTag";
        //const string COL_AccountValue = "AccountValue";
        //const string COL_PNLPOSITION = "PNLWhenTaxLotsPopulated";
        //const string COL_PNL = "CostBasisRealizedPNL";
        //const string COL_StartTaxLotID = "StartTaxLotID";
        //const string COL_PositionStartQuantity = "PositionStartQty";
        //const string COL_AccountID = "AccountID";
        //const string COL_Multiplier = "Multiplier";
        //const string COL_AUECID = "AUECID";
        //const string COL_RealizedPNL = "CostBasisRealizedPNL";
        //const string COL_RecordType = "RecordType";
        //const string COL_Status = "Status";
        //const string COL_EndDate = "EndDate";
        //const string COL_Description = "Description";
        //const string COL_Strategy = "Strategy";
        //const string COL_StrategyID = "StrategyID";
        //const string COL_MarkPriceForMonth = "MarkPriceForMonth";
        //const string COL_MonthToDateRealizedProfit = "MonthToDateRealizedProfit";
        //const string COL_NotionalValue = "NotionalValue";
        //const string COL_AvgPriceRealizedPL = "AvgPriceRealizedPL";
        //const string COL_SymbolAveragePrice = "SymbolAveragePrice";
        //const string COL_AUECLocalCloseDate = "AUECLocalCloseDate";
        //const string COL_CloseDate = "TimeOfSaveUTC";

        //const string COL_GeneratedTaxlotSymbol = "GeneratedTaxlotSymbol";
        //const string COL_Exchange = "Exchange";
        //const string COL_OpenQty = "OpenQty";
        //const string COL_CurrencyID = "CurrencyID";
        //const string COL_Currency = "Currency";
        ////const string COL_UnderlyingName = "Underlying";
        //const string COL_ClosingID = "ClosingID";

        //const string PositionalSide_Long = "Long";
        //const string PositionalSide_Short = "Short";
        //const string COL_PositionSide = "Side";
        //const string COL_TradeDatePosition = "TradeDate";

        //const string COL_AllocationID = "TaxLotID";
        //const string COL_TradeDate = "AUECLocalDate";
        //const string COL_ProcessDate = "ProcessDate";
        //const string COL_OriginalPurchaseDate = "OriginalPurchaseDate";
        //const string COL_ClosingTradeDate = "ClosingTradeDate";
        //const string COL_TradeDateUTC = "TradeDateUTC";
        //const string COL_Side = "OrderSide";
        //const string COL_ClosingSide = "ClosingSide";
        //const string COL_Symbol = "Symbol";
        //const string COL_SecurityFullName = "CompanyName";
        //const string COL_OpenQuantity = "TaxLotQty";
        //const string COL_ClosedQty = "ClosedQty";
        //const string COL_AveragePrice = "AvgPrice";
        //const string COL_OpenAveragePrice = "OpenAveragePrice";
        //const string COL_ClosedAveragePrice = "ClosedAveragePrice";
        //const string COL_Account = "Level1Name";
        //const string COL_SideID = "OrderSideTagValue";
        //const string COL_IsPosition = "IsPosition";
        //const string COL_AUEC = "AUECID";
        //const string COL_PositionTaxlotID = "PositionTaxlotID";
        //const string COL_OpenCommission = "OpenTotalCommissionandFees";
        //const string COL_PositionCommission = "PositionTotalCommissionandFees";
        //const string COL_OpenFees = "OtherBrokerOpenFees";
        //const string COL_PositionFees = "PositionOtherBrokerFees";
        //const string COL_ClosedCommission = "ClosedTotalCommissionandFees";
        //const string COL_ClosingTotalCommissionandFees = "ClosingTotalCommissionandFees";
        //const string COL_NetNotionalValue = "NetNotionalValue";
        //const string COL_StrategyValue = "Level2Name";
        //const string COL_SettledQty = "SettledQty";
        //const string COL_CashSettledPrice = "CashSettledPrice";
        //const string COL_ClosingMode = "ClosingMode";
        //const string COL_IsExpired_Settled = "IsExpired_Settled";
        //const string COL_AssetCategoryValue = "AssetCategoryValue";
        //const string COL_ExpiryDate = "ExpirationDate";
        //const string COL_Underlying = "UnderlyingName";
        //const string COL_UnitCost = "UnitCost";
        //const string COL_PositionTagValue = "PositionTag";
        //const string COL_IsSwap = "ISSwap";
        //const string COL_LotId = "LotId";
        //const string COL_ExternalTransId = "ExternalTransId";

        //const string CAP_TaxlotId = "Taxlot ID";
        //const string CAP_Account = "Account";
        //const string CAP_Strategy = "Strategy";
        //const string CAP_TradeDate = "Trade Date";
        //const string CAP_ProcessDate = "Process Date";
        //const string CAP_OriginalPurchaseDate = "OriginalPurchase Date";
        //const string CAP_PositionType = "Position Type";
        //const string CAP_ClosedPositionType = "Closing Position Type";
        //const string CAP_Symbol = "Symbol";
        //const string CAP_StartQty = "Start Quantity";
        //const string CAP_CloseQty = "Qty Closed";
        //const string CAP_OpenQty = "Open Qty";
        //const string CAP_AvgPrice = "Opening Price";
        //const string CAP_AvgClosingPrice = "Closing Price";
        //const string CAP_Commission = "Total Fees and Commission";
        //const string CAP_Fees = "OtherBrokerFees";
        //const string CAP_OtherFees = "Other Fees";
        //const string CAP_RealizedPNL = "Realized PNL(C.B.)";
        //const string CAP_AUECCloseDt = "AUEC Close Date";
        //const string CAP_CloseDt = "Closing Date";
        //const string CAP_Side = "Transaction Type";
        //const string CAP_OpeningSide = "Opening Side";
        //const string CAP_ClosingSide = "Closing Side";
        //const string CAP_NetNotional = "Net Notional";
        //const string CAP_SecurityFullName = "Security Name";
        //const string CAP_ClosingMode = "Closing Mode";
        //const string CAP_SettlementPrice = "Settlement Price";
        //const string CAP_AssetCategory = "Asset";
        //const string CAP_Exchange = "Exchange";
        //const string CAP_PositionCommission = "Opening Fees & Commission";
        //const string CAP_Currency = "Currency";
        //const string CAP_Underlying = "Underlying";
        //const string CAP_ClosingID = "ClosingID";
        //const string CAP_IsSwapped = "IsSwapped";


        //const string _currencyColumnFormat = "#,#.00";

        //#endregion


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
                        string assetName = string.Empty;
                        int accountID = int.MinValue;

                        if (PostReconClosingData.OpenTaxlots.Count > 0)
                        {
                            assetName = PostReconClosingData.OpenTaxlots[0].AssetName;
                            accountID = PostReconClosingData.OpenTaxlots[0].Level1ID;
                        }

                        if (!string.IsNullOrWhiteSpace(assetName) && accountID != int.MinValue)
                        {

                            DataSet ds = _closingServices.InnerChannel.GetPreferences().ClosingMethodology.AccountingMethodsTable;

                            DataRow[] results = ds.Tables[0].Select(ClosingConstants.COL_AccountID + " = '" + accountID.ToString() + "' AND AssetName = '" + assetName + "'");
                            foreach (DataRow row in results)
                            {
                                if (row[ClosingConstants.COL_ClosingAlgo] != DBNull.Value && !string.IsNullOrWhiteSpace(row[ClosingConstants.COL_ClosingAlgo].ToString()))
                                {
                                    lblAccountingRuleValue.Text = ((PostTradeEnums.CloseTradeAlogrithm)(int.Parse(row[ClosingConstants.COL_ClosingAlgo].ToString()))).ToString();
                                }
                                else
                                {
                                    lblAccountingRuleValue.Text = PostTradeEnums.CloseTradeAlogrithm.NONE.ToString();
                                    MessageBox.Show(this, "Default closing algorithm is not available for the account " + lblAccountValue.Text + ".", "Closing Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                int secondarySort = int.MinValue;
                                if (int.TryParse(row["SecondarySort"].ToString(), out secondarySort) && secondarySort != int.MinValue)
                                {
                                    lblSecondaySortValue.Text = ((PostTradeEnums.SecondarySortCriteria)(secondarySort)).ToString();
                                    SetLabelText(lblSecondaySortValue, lblSecondaySortValue.Text, true, "lblSecondaySortValue");
                                }
                                else
                                {
                                    lblSecondaySortValue.Text = PostTradeEnums.SecondarySortCriteria.None.ToString();
                                    SetLabelText(lblSecondaySortValue, lblSecondaySortValue.Text, true, "lblSecondaySortValue");
                                }
                            }
                        }

                        lblSymbolPNLPreAmendmentValue.StyleLibraryName = string.Empty;
                        lblSymbolPNLPostAmendmentValue.StyleLibraryName = string.Empty;
                        //lblSymbolPNLPreAmendmentValue.Appearance.ForeColor = Color.Green;
                        //lblSymbolPNLPostAmendmentValue.Appearance.ForeColor = Color.Green;

                        lblAccountPNLPreAmendmentValue.StyleLibraryName = string.Empty;
                        lblAccountPNLPostAmendmentValue.StyleLibraryName = string.Empty;
                        //lblAccountPNLPreAmendmentValue.Appearance.ForeColor = Color.Green;
                        //lblAccountPNLPostAmendmentValue.Appearance.ForeColor = Color.Green;

                        grdData.DataSource = PostReconClosingData.OpenTaxlots;

                        grdClosed.DataSource = PostReconClosingData.Netpositions;

                        if (grdData.DisplayLayout.Bands[0].Columns.Exists(ClosingConstants.COL_ClosingAlgoChooser))
                        {
                            grdData.DisplayLayout.Bands[0].Columns[ClosingConstants.COL_ClosingAlgoChooser].NullText = lblAccountingRuleValue.Text;
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

        private void grdData_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                //add filter row
                //UltraWinGridUtils.EnableFixedFilterRow(e);

                //e.Layout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;


                #region Set Summary

                // Purpose to calculate Summary with a formula.



                SummaryHelper.AddColumnSummary(e, ClosingConstants.COL_OpenQuantity, ClosingConstants.COL_SideMultiplier, SummaryType.Custom);

                SummaryHelper.AddColumnSummary(e, ClosingConstants.COL_NetNotionalValue, ClosingConstants.COL_SideMultiplier, SummaryType.Custom);

                SummaryHelper.AddColumnSummary(e, ClosingConstants.COL_ExecutedQty, ClosingConstants.COL_SideMultiplier, SummaryType.Custom);

                SummaryHelper.AddColumnSummary(e, ClosingConstants.COL_AveragePrice, ClosingConstants.COLUMN_TAXLOTQTY, SummaryType.Average);

                SummaryHelper.AddColumnSummary(e, ClosingConstants.COL_UnRealizedPNL, string.Empty, SummaryType.Sum);

                SummaryHelper.AddColumnSummary(e, ClosingConstants.COL_MarkPrice, ClosingConstants.COL_ExecutedQty, SummaryType.Average);

                SummaryHelper.AddColumnSummary(e, ClosingConstants.COL_OpenCommission, string.Empty, SummaryType.Sum);

                SetSummaryProperties(e);

                #endregion

                e.Layout.Override.FormulaRowIndexSource = FormulaRowIndexSource.ListIndex;
                e.Layout.Override.SelectTypeCell = SelectType.SingleAutoDrag;
                e.Layout.Override.CellClickAction = CellClickAction.RowSelect;
                grdData.AllowDrop = true;

                // Set the RowSelectorHeaderStyle to ColumnChooserButton.
                e.Layout.Override.RowSelectorHeaderStyle = RowSelectorHeaderStyle.ColumnChooserButton;
                // Enable the RowSelectors. This is necessary because the column chooser
                // button is displayed over the row selectors in the column headers area.
                e.Layout.Override.RowSelectors = DefaultableBoolean.True;

                e.Layout.Override.CellClickAction = CellClickAction.Edit;
                e.Layout.Override.RowAppearance.BackColor = Color.Black;

                if (!e.Layout.Bands[0].Columns.Exists("checkBox"))
                {
                    AddCheckBoxinGrid(grdData, headerCheckBoxUnWind);
                }

                //add delete button columns
                if (!e.Layout.Bands[0].Columns.Exists("btnDelete"))
                {
                    e.Layout.Bands[0].Columns.Add("btnDelete", "Delete");
                }
                UltraGridColumn colbtnDelete = e.Layout.Bands[0].Columns["btnDelete"];
                colbtnDelete.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                colbtnDelete.Width = 50;
                colbtnDelete.NullText = "Delete";
                colbtnDelete.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;


                if (!e.Layout.Bands[0].Columns.Exists(ClosingConstants.COL_ClosingAlgoChooser))
                {
                    e.Layout.Bands[0].Columns.Add(ClosingConstants.COL_ClosingAlgoChooser, ClosingConstants.CAP_ClosingAlgo);
                }

                UltraGridColumn colClosingAlgo = e.Layout.Bands[0].Columns[ClosingConstants.COL_ClosingAlgoChooser];
                colClosingAlgo.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                colClosingAlgo.Width = 50;

                colClosingAlgo.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                ValueList _vlClosingAlgo = new ValueList();
                List<string> lstClosingAlgo = Enum.GetNames(typeof(PostTradeEnums.CloseTradeAlogrithm)).ToList();
                lstClosingAlgo.Remove(PostTradeEnums.CloseTradeAlogrithm.NONE.ToString());
                lstClosingAlgo.Remove(PostTradeEnums.CloseTradeAlogrithm.MANUAL.ToString());
                lstClosingAlgo.Remove(PostTradeEnums.CloseTradeAlogrithm.PRESET.ToString());
                //Modified By : Manvendra Jira : PRANA-10341
                lstClosingAlgo.Remove(PostTradeEnums.CloseTradeAlogrithm.Multiple.ToString());
                foreach (string closingAlgo in lstClosingAlgo)
                {
                    _vlClosingAlgo.ValueListItems.Add(closingAlgo, closingAlgo);
                }
                colClosingAlgo.ValueList = _vlClosingAlgo;
                colClosingAlgo.NullText = lblAccountingRuleValue.Text;

                //e.Layout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
                #region Load previous save layout
                grdDataSetColumns();
                #endregion

                //     grdNetPosition.DisplayLayout.Load(saveLayoutforNetPosition, PropertyCategories.All);                
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

        private void SetSummaryProperties(InitializeLayoutEventArgs e)
        {
            try
            {
                //allign summary text to right
                foreach (SummarySettings summarySettings in e.Layout.Bands[0].Summaries)
                {
                    summarySettings.DisplayFormat = "{0:#,#0.0000}";
                    summarySettings.Appearance.TextHAlign = HAlign.Right;
                }

                // To display summary footer on the top of the row collections set the 
                // SummaryDisplayArea property to a value that has the Top or TopFixed flag
                // set. TopFixed will make the summary fixed (non-scrolling). Note that 
                // summaries are not fixed in the child rows. TopFixed setting behaves
                // the same way as Top in child rows. Default is resolved to Bottom (and
                // InGroupByRows more about which follows).
                e.Layout.Override.SummaryDisplayArea = SummaryDisplayAreas.BottomFixed;




                // To allow the user to be able to add/remove summaries set the 
                // AllowRowSummaries property. This does not have to be set to summarize
                // data in code.
                //e.Layout.Override.AllowRowSummaries = AllowRowSummaries.True;




                // To display summary footer on the top of the row collections set the 
                // SummaryDisplayArea property to a value that has the Top or TopFixed flag
                // set. TopFixed will make the summary fixed (non-scrolling). Note that 
                // summaries are not fixed in the child rows. TopFixed setting behaves
                // the same way as Top in child rows. Default is resolved to Bottom (and
                // InGroupByRows more about which follows).
                //e.Layout.Override.SummaryDisplayArea = SummaryDisplayAreas.TopFixed;

                // By default UltraGrid does not display summary footers or headers of
                // group-by row islands. To display summary footers or headers of group-by row
                // islands set the SummaryDisplayArea to a value that has GroupByRowsFooter
                // flag set.
                //e.Layout.Override.SummaryDisplayArea |= SummaryDisplayAreas.GroupByRowsFooter;

                // If you want to to display summaries of child rows in each group-by row
                // set the SummaryDisplayArea to a value that has SummaryDisplayArea flag
                // set. If SummaryDisplayArea is left to Default then the UltraGrid by
                // default displays summaries in group-by rows.
                //e.Layout.Override.SummaryDisplayArea |= SummaryDisplayAreas.InGroupByRows;

                // SummaryDisplayArea property is expossed on the SummarySettings object as
                // well allowing to control if and where the summary gets displayed on a
                // per summary basis.


                // By default any summaries to be displayed in the group-by rows are displayed
                // as text appended to the group-by row's description. You can set the 
                // GroupBySummaryDisplayStyle property to SummaryCells or 
                // SummaryCellsAlwaysBelowDescription to display summary values as a separate
                // ui element (cell like element with border, to which the summary value related
                // appearances are applied). Default value of GroupBySummaryDisplayStyle is resolved
                // to Text.
                //e.Layout.Override.GroupBySummaryDisplayStyle = GroupBySummaryDisplayStyle.SummaryCells;

                // Appearance of the summary area can be controlled using the 
                // SummaryFooterAppearance. Even though the property's name contains the
                // word 'footer', this appearance applies to summary area that is displayed
                // on top as well (summary headers).
                // e.Layout.Override.SummaryFooterAppearance.BackColor = SystemColors.Info;

                // Appearance of summary values can be controlled using the 
                // SummaryValueAppearance property.
                //e.Layout.Override.SummaryValueAppearance.BackColor = SystemColors.Window;
                //e.Layout.Override.SummaryValueAppearance.FontData.Bold = DefaultableBoolean.True;

                // Appearance of summary values that are displayed inside of group-by rows can 
                // be controlled using the GroupBySummaryValueAppearance property. Note that
                // this has effect only when the GroupBySummaryDisplayStyle is set to SummaryCells
                // or SummaryCellsAlwaysBelowDescription.
                //e.Layout.Override.GroupBySummaryValueAppearance.BackColor = SystemColors.Window;
                //e.Layout.Override.GroupBySummaryValueAppearance.TextHAlign = HAlign.Right;

                // Caption of the summary area can be set using the SummaryFooterCaption
                // proeprty of the band.
                e.Layout.Bands[0].SummaryFooterCaption = "Summary";

                // Caption's appearance can be controlled using the SummaryFooterCaptionAppearance
                // property.
                //e.Layout.Override.SummaryFooterCaptionAppearance.FontData.Bold = DefaultableBoolean.True;

                // By default summary footer caption is visible. You can hide it using the
                // SummaryFooterCaptionVisible property.
                //e.Layout.Override.SummaryFooterCaptionVisible = DefaultableBoolean.False;

                // SummaryFooterSpacingAfter and SummaryFooterSpacingBefore properties can be used
                // to leave spacing before and after the summary footer.
                //e.Layout.Override.SummaryFooterSpacingAfter = 5;
                //e.Layout.Override.SummaryFooterSpacingBefore = 5;
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
        private void grdData_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
        {
            try
            {
                TaxLot taxlot = e.Row.ListObject as TaxLot;
                if (taxlot != null)
                {
                    SetOpenTradesGridRowAppearance(e, taxlot);
                    if (e.Row.Cells.Exists("btnDelete")
                        && !NAVLockManager.GetInstance.ValidateTrade(
                        (taxlot).Level1ID, (taxlot).AUECLocalDate))
                    {
                        e.Row.Cells["btnDelete"].Activation = Activation.Disabled;
                    }
                    if (!(taxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_Sell) || taxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_Sell_Closed)
                        || taxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy_Closed) || taxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy_Cover)))
                    {
                        e.Row.Cells[ClosingConstants.COL_ClosingAlgoChooser].Value = string.Empty;
                        e.Row.Cells[ClosingConstants.COL_ClosingAlgoChooser].Activation = Activation.Disabled;
                    }
                    else
                    {
                        if (e.Row.Cells[ClosingConstants.COL_ClosingAlgoChooser].Value != null && e.Row.Cells[ClosingConstants.COL_ClosingAlgoChooser].Value.ToString().Equals(string.Empty))
                        {
                            e.Row.Cells[ClosingConstants.COL_ClosingAlgoChooser].Value = null;
                        }
                        e.Row.Cells[ClosingConstants.COL_ClosingAlgoChooser].Activation = Activation.AllowEdit;
                    }
                }
                // e.Row.Cells[ClosingConstants.COL_ClosingAlgoChoosert].Value = "";
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
        private void grdData_AfterCellUpdate(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            try
            {

                TaxLot taxlot = e.Cell.Row.ListObject as TaxLot;
                //handle here if the method is invoked due to filtering
                if (e.Cell.GetType() == typeof(UltraGridFilterCell) || taxlot == null || e.Cell.Column.Key.Contains("CalculationColumn"))
                {
                    return;
                }
                else if (e.Cell.Column.Key.Equals(ClosingConstants.COL_AveragePrice) || e.Cell.Column.Key.Equals(ClosingConstants.COL_ExecutedQty) || e.Cell.Column.Key.Equals(ClosingConstants.COL_MarkPrice))
                {

                    #region check closing status and max closing date of taxlot
                    //for post recon amendment UI we have to fetch all closed data.
                    //If data is not fetched and user do amendments in taxlot then warn the user that data cannot be amended
                    ClosingStatus closingStatus;
                    DateTime AuecModifiedDate;

                    string closingError = string.Empty;

                    _closingServices.InnerChannel.GetTaxlotClosingStatusWithMaxModifiedDate(taxlot, out closingStatus, out AuecModifiedDate);

                    if (closingStatus != ClosingStatus.Open && AuecModifiedDate.Date > ((DateTime)dtEndDate.Value).Date)
                    {
                        closingError = "Taxlot cannot be updated as taxlot have closing on " + AuecModifiedDate.Date + " but data is fetched till " + ((DateTime)dtEndDate.Value).Date + " ,First fetch data till " + AuecModifiedDate.Date + ".";
                    }
                    #endregion

                    if (e.Cell.Column.Key.Equals("ExecutedQty"))
                    {
                        double originalValue = Convert.ToDouble(e.Cell.OriginalValue.ToString());
                        double updatedValue = Convert.ToDouble(e.Cell.Value.ToString());

                        if (!string.IsNullOrWhiteSpace(closingError))
                        {
                            MessageBox.Show(this, closingError, "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            taxlot.ExecutedQty = originalValue;
                            e.Cell.CancelUpdate();
                            return;
                        }

                        bool isValuesCanbeUpdated = AmendmentsHelper.UpdateExecutedQty(taxlot, originalValue, updatedValue);
                        if (!isValuesCanbeUpdated)
                        {
                            MessageBox.Show(this, "Executed quantity for TaxlotID: " + taxlot.TaxLotID + " cannot be updated, " + (originalValue - taxlot.TaxLotQty) + " quantity is already closed which is greater than updated executed quantity " + taxlot.ExecutedQty + ", first unwind data from " + taxlot.OriginalPurchaseDate.Date, "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            taxlot.ExecutedQty = originalValue;
                            e.Cell.CancelUpdate();
                            return;
                        }
                    }
                    if (e.Cell.Column.Key.Equals("AvgPrice"))
                    {
                        double originalValue = Convert.ToDouble(e.Cell.OriginalValue.ToString());
                        if (!string.IsNullOrWhiteSpace(closingError))
                        {
                            MessageBox.Show(this, closingError, "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            taxlot.AvgPrice = originalValue;
                            e.Cell.CancelUpdate();
                            return;
                        }
                    }
                    if (e.Cell.Column.Key.Equals("MarkPrice"))
                    {
                        double originalValue = Convert.ToDouble(e.Cell.OriginalValue.ToString());
                        if (!string.IsNullOrWhiteSpace(closingError))
                        {
                            MessageBox.Show(this, closingError, "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            taxlot.MarkPrice = _markPriceForAccountSymbol = originalValue;
                            e.Cell.CancelUpdate();
                            return;
                        }
                        Double.TryParse(e.Cell.Row.Cells["MarkPrice"].Value.ToString(), out _markPriceForAccountSymbol);
                        UpdateMarkPriceForOpenTaxlots();
                        UpdateMarkPriceChangesToDataTable(taxlot);

                    }
                    PostReconClosingData.IsUnsavedChanges = true;
                    AmendmentsHelper.UpdateAmendmetsDictionary(e.Cell.Column.Key, e.Cell.Value.ToString(), e.Cell.OriginalValue.ToString(), taxlot);
                    if (_unwindingStartDate == DateTimeConstants.MinValue || _unwindingStartDate > taxlot.AUECLocalDate)
                    {
                        _unwindingStartDate = taxlot.AUECLocalDate;
                    }
                    //currently closing status is not updated correctly when data is fetched from DB
                    if ((taxlot.ClosingStatus != ClosingStatus.Open) || taxlot.ExecutedQty != taxlot.TaxLotQty)
                    {
                        _isUnwindingClosingTobeDone = true;
                    }

                    SetColorAndFormattingForPNLFields();

                    if (_taxLotUnRealizedPNL != double.MinValue)
                    {
                        AmendmentsHelper.UpdateUnRealizedPNL(taxlot.UnRealizedPNL - _taxLotUnRealizedPNL);
                        UpdatelblPNLPostAmendmentValue();
                        _taxLotUnRealizedPNL = double.MinValue;
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
        /// Added by Faisal Shah
        /// Purpose to Update MarkPrice in all grid cells
        /// OpenTaxlots is a Generic Binding list
        /// </summary>
        private void UpdateMarkPriceForOpenTaxlots()
        {
            try
            {
                foreach (TaxLot taxlott in PostReconClosingData.OpenTaxlots)
                {
                    taxlott.MarkPrice = _markPriceForAccountSymbol;
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
        /// Added BY Faisal Shah
        /// Updates MarkPrice DataTable to be saved in DataBase
        /// </summary>
        /// <param name="taxlot"></param>
        private void UpdateMarkPriceChangesToDataTable(TaxLot taxlot)
        {
            try
            {
                dtMarkPrice.Rows.Clear();
                if (dtMarkPrice.TableName == string.Empty)
                {
                    SetDataTableSchema();
                }
                DateTime date = new DateTime();
                bool isDateParsed = DateTime.TryParse(dtEndDate.Value.ToString(), out date);
                if (isDateParsed)
                {
                    double markPrice;
                    double.TryParse(taxlot.MarkPrice.ToString(), out markPrice);
                    //only non-zero mark prices will be saved.  

                    if (markPrice != 0)
                    {
                        DataRow drNew = dtMarkPrice.NewRow();
                        drNew["IsApproved"] = 1;
                        drNew["Date"] = Convert.ToDateTime(dtEndDate.Value);
                        drNew["MarkPrice"] = markPrice;//  dr[dc.ColumnName];
                        // this column value has been fixed to differentiate whether data save into the DB from Import module or Mark price UI
                        // L stands for Live feed Data
                        drNew["MarkPriceImportType"] = Prana.BusinessObjects.AppConstants.MarkPriceImportType.P.ToString();

                        drNew["Symbol"] = taxlot.Symbol.ToString().ToUpper();
                        drNew["FundID"] = taxlot.Level1ID.ToString();
                        drNew["Source"] = (int)Enum.Parse(typeof(PricingSource), PricingSource.UserDefined.ToString());
                        drNew["ForwardPoints"] = 0;
                        drNew["AUECID"] = taxlot.AUECID.ToString().ToUpper();
                        //drNew["AUECIdentifier"] = taxlot.Iden.ToString();
                        dtMarkPrice.Rows.Add(drNew);
                        dtMarkPrice.AcceptChanges();
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

        private void SetDataTableSchema()
        {
            try
            {
                dtMarkPrice.TableName = "MarkPriceImport";
                dtMarkPrice.Columns.Add(new DataColumn("Symbol"));
                dtMarkPrice.Columns.Add(new DataColumn("Date"));
                dtMarkPrice.Columns.Add(new DataColumn("MarkPrice"));
                dtMarkPrice.Columns.Add(new DataColumn("MarkPriceImportType"));
                dtMarkPrice.Columns.Add(new DataColumn("ForwardPoints"));
                dtMarkPrice.Columns.Add(new DataColumn("IsApproved"));

                //Added AUECID as it will be used at pricing server end to update cache
                dtMarkPrice.Columns.Add(new DataColumn("AUECID"));
                dtMarkPrice.Columns.Add(new DataColumn("AUECIdentifier"));
                dtMarkPrice.Columns.Add(new DataColumn("FundID"));
                dtMarkPrice.Columns.Add(new DataColumn("Source"));
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
        /// Update lable PNL Post Amendmen tValue
        /// </summary>
        private void UpdatelblPNLPostAmendmentValue()
        {
            try
            {
                double accountPNL = 0;
                double symbolPNL = 0;
                AmendmentsHelper.GetTotalPNL(out accountPNL, out symbolPNL);

                SetLabelText(lblSymbolPNLPostAmendmentValue, Math.Round(symbolPNL, 4).ToString(), true, "lblSymbolPNLPostAmendmentValue");

                SetLabelText(lblAccountPNLPostAmendmentValue, Math.Round(accountPNL, 4).ToString(), true, "lblAccountPNLPostAmendmentValue");
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
        /// Sets the allocated trade grids row appearance.
        /// </summary>
        /// <param name="e">The <see cref="Infragistics.Win.UltraWinGrid.InitializeRowEventArgs"/> instance containing the event data.</param>
        /// <param name="isLongAllocatedTradesGrid">if set to <c>true</c> [is long allocated trades grid].</param>
        private void SetOpenTradesGridRowAppearance(InitializeRowEventArgs e, TaxLot taxlot)
        {
            try
            {
                //Check if the grid is loaded for the first time
                if (e.Row.Index == 0)
                {
                    _grdDataStartUp = true;
                }
                if (_grdDataStartUp && e.Row.Appearance.ForeColor != Color.Yellow)
                {
                    if (taxlot.OrderSideTagValue == FIXConstants.SIDE_Buy
                        || taxlot.OrderSideTagValue == FIXConstants.SIDE_BuyMinus
                        || taxlot.OrderSideTagValue == FIXConstants.SIDE_Buy_Open
                        || taxlot.OrderSideTagValue == FIXConstants.SIDE_Buy_Closed
                        || taxlot.OrderSideTagValue == FIXConstants.SIDE_Buy_Cover)
                    {
                        e.Row.Appearance.ForeColor = Color.GreenYellow;
                    }
                    else
                    {
                        e.Row.Appearance.ForeColor = Color.OrangeRed;
                    }
                }
                else
                {
                    e.Row.Appearance.ForeColor = Color.Yellow;
                }
                grdData.DisplayLayout.Bands[0].Override.CellClickAction = CellClickAction.EditAndSelectText;
                //reset the bool variable if the grid is completely loaded so to change the row colors for new rows
                if (e.Row.Index == grdData.Rows.Count - 1)
                {
                    _grdDataStartUp = false;
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

        #region manage columns
        private void grdDataSetColumns()
        {
            try
            {
                if (PostReconLayout.PostReconDataColumns.Count > 0)
                {
                    List<ColumnData> listColData = PostReconLayout.PostReconDataColumns;
                    SetGridColumnLayout(grdData, listColData);
                    foreach (string col in GetAllDefaultColumns(true))
                    {
                        if (grdData.DisplayLayout.Bands[0].Columns.Exists(col))
                        {
                            UltraGridColumn column = grdData.DisplayLayout.Bands[0].Columns[col];
                            column.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                        }
                    }
                }
                else
                {
                    LoadColumns(grdData, true);
                }
                SetGridDataColumnFormatting(grdData);
                SetGridDataColumnCustomizations(grdData);
                //SetColumnSummaries(grdData);
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

        private void LoadColumns(UltraGrid grid, bool isgrdData)
        {
            try
            {

                List<string> colAll = GetAllDisplayableColumns(isgrdData);
                List<string> colDefault = GetAllDefaultColumns(isgrdData);
                List<string> colVisible = GetAllDefaultColumns(isgrdData);

                if (colVisible.Count < 1) // PrefFile Has No Columns
                {
                    colVisible.AddRange(colDefault);
                }

                ColumnsCollection gridColumns = grid.DisplayLayout.Bands[0].Columns;

                foreach (UltraGridColumn gridCol in gridColumns)
                {
                    gridCol.Hidden = true;

                    if (!colAll.Contains(gridCol.Key))
                    {
                        gridCol.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    }
                }
                int visiblePos = 1;
                foreach (string col in colVisible)
                {
                    if (!String.IsNullOrEmpty(col) && gridColumns.Exists(col))
                    {
                        gridColumns[col].Hidden = false;
                        gridColumns[col].Header.VisiblePosition = visiblePos;
                        gridColumns[col].Width = 100;
                        visiblePos++;
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

        private List<string> GetAllDefaultColumns(bool isgrdData)
        {
            List<string> colDefault = new List<string>();
            try
            {
                if (isgrdData)
                {
                    colDefault.Add(ClosingConstants.COL_TradeDate);
                    colDefault.Add(ClosingConstants.COL_ProcessDate);
                    colDefault.Add(ClosingConstants.COL_OriginalPurchaseDate);
                    colDefault.Add(ClosingConstants.COL_Side);
                    colDefault.Add(ClosingConstants.COL_ExecutedQty);
                    colDefault.Add(ClosingConstants.COL_OpenQuantity);
                    colDefault.Add(ClosingConstants.COL_AveragePrice);
                    colDefault.Add(ClosingConstants.COL_MarkPrice);
                    colDefault.Add(ClosingConstants.COL_UnRealizedPNL);
                    colDefault.Add(ClosingConstants.COL_OpenCommission);
                    colDefault.Add(ClosingConstants.COL_NetNotionalValue);
                    colDefault.Add("btnDelete");
                    colDefault.Add(ClosingConstants.COL_ClosingAlgoChooser);
                    colDefault.Add(ClosingConstants.COL_ClosingStatus);
                }
                else
                {
                    colDefault.Add("ClosingTradeDate");
                    colDefault.Add("ClosedQty");
                    colDefault.Add("OpenAveragePrice");
                    colDefault.Add("ClosedAveragePrice");
                    colDefault.Add("CostBasisRealizedPNL");
                    colDefault.Add("NotionalChange");
                    colDefault.Add("CostBasisGrossPNL");
                    colDefault.Add("FundValue");
                    colDefault.Add("Strategy");
                    colDefault.Add("PositionalTag");
                    colDefault.Add("AssetCategoryValue");
                    colDefault.Add("Symbol");
                    colDefault.Add("PositionSide");
                    colDefault.Add(ClosingConstants.COL_ClosingAlgoChooser);
                    colDefault.Add(ClosingConstants.COL_ClosingID);
                    colDefault.Add(ClosingConstants.COL_ID);
                    colDefault.Add(ClosingConstants.COL_PositionCommission);
                    colDefault.Add(ClosingConstants.COL_ClosingTotalCommissionandFees);
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
            return colDefault;
        }

        private List<string> GetAllDisplayableColumns(bool isgrdData)
        {
            List<string> colAll = new List<string>();
            try
            {
                List<string> colDefault = GetAllDefaultColumns(isgrdData);
                colAll.AddRange(colDefault);
                #region Commented
                // #already added in GetAllDefaultColumns()
                //colAll.Add(COL_TradeDate);
                //colAll.Add(COL_ProcessDate);
                //colAll.Add(COL_OriginalPurchaseDate);
                //colAll.Add("TransactionType");
                //colAll.Add("ExecutedQty");
                //colAll.Add(COL_OpenQuantity);
                //colAll.Add(COL_AveragePrice);
                //colAll.Add("ClosingMark");
                //colAll.Add(COL_OpenCommission);
                //colAll.Add(COL_NetNotionalValue);
                //colAll.Add("RealizedPNL");
                //colAll.Add("UnRealizedPNL");
                #endregion
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
            return colAll;
        }

        private void SetGridDataColumnFormatting(UltraGrid grid)
        {
            try
            {
                ColumnsCollection columns = grid.DisplayLayout.Bands[0].Columns;

                columns[ClosingConstants.COL_TradeDate].Format = "MM/dd/yyyy";
                columns[ClosingConstants.COL_ProcessDate].Format = "MM/dd/yyyy";
                columns[ClosingConstants.COL_OriginalPurchaseDate].Format = "MM/dd/yyyy";

                //columns["ExecutedQty"].Format = "{0:#,0.#}";
                columns[ClosingConstants.COL_OpenQuantity].Format = _quantityColumnFormat;
                columns[ClosingConstants.COL_AveragePrice].Format = _currencyColumnFormat;
                // columns["ClosingMark"].Format = "#,#.0000";
                columns[ClosingConstants.COL_OpenCommission].Format = _currencyColumnFormat;
                columns[ClosingConstants.COL_NetNotionalValue].Format = _currencyColumnFormat;
                // columns["RealizedPNL"].Format = "{0:#,0}";
                columns[ClosingConstants.COL_ExecutedQty].Format = _quantityColumnFormat;
                columns[ClosingConstants.COL_MarkPrice].Format = _currencyColumnFormat;
                columns[ClosingConstants.COL_UnRealizedPNL].Format = _currencyColumnFormat;

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

        private void SetGridDataColumnCustomizations(UltraGrid Grid)
        {
            try
            {
                //Grid.CreationFilter = headerCheckBox;

                UltraGridBand band = Grid.DisplayLayout.Bands[0];

                band.Columns[ClosingConstants.COL_TradeDate].CellActivation = Activation.NoEdit;
                band.Columns[ClosingConstants.COL_ProcessDate].CellActivation = Activation.NoEdit;
                band.Columns[ClosingConstants.COL_OriginalPurchaseDate].CellActivation = Activation.NoEdit;
                band.Columns[ClosingConstants.COL_Side].CellActivation = Activation.NoEdit;
                band.Columns[ClosingConstants.COL_OpenQuantity].CellActivation = Activation.NoEdit;
                band.Columns[ClosingConstants.COL_AveragePrice].CellActivation = Activation.AllowEdit;
                band.Columns[ClosingConstants.COL_OpenCommission].CellActivation = Activation.NoEdit;
                band.Columns[ClosingConstants.COL_NetNotionalValue].CellActivation = Activation.NoEdit;
                band.Columns[ClosingConstants.COL_ExecutedQty].CellActivation = Activation.AllowEdit;
                band.Columns[ClosingConstants.COL_MarkPrice].CellActivation = Activation.AllowEdit;
                band.Columns[ClosingConstants.COL_UnRealizedPNL].CellActivation = Activation.NoEdit;
                band.Columns["btnDelete"].CellActivation = Activation.AllowEdit;
                band.Columns[ClosingConstants.COL_ClosingAlgoChooser].CellActivation = Activation.AllowEdit;

                band.Columns[ClosingConstants.COL_TradeDate].Header.Caption = ClosingConstants.CAP_TradeDate;
                band.Columns[ClosingConstants.COL_ProcessDate].Header.Caption = ClosingConstants.CAP_ProcessDate;
                band.Columns[ClosingConstants.COL_OriginalPurchaseDate].Header.Caption = ClosingConstants.CAP_OriginalPurchaseDate;
                band.Columns[ClosingConstants.COL_Side].Header.Caption = ClosingConstants.CAP_Side;
                band.Columns[ClosingConstants.COL_OpenQuantity].Header.Caption = ClosingConstants.CAP_OpenQty;
                band.Columns[ClosingConstants.COL_AveragePrice].Header.Caption = ClosingConstants.CAP_AvgPrice;
                band.Columns[ClosingConstants.COL_OpenCommission].Header.Caption = ClosingConstants.CAP_PositionCommission;
                band.Columns[ClosingConstants.COL_NetNotionalValue].Header.Caption = ClosingConstants.CAP_NetNotional;
                band.Columns[ClosingConstants.COL_ExecutedQty].Header.Caption = ClosingConstants.CAPTION_ExecutedQty;
                band.Columns[ClosingConstants.COL_MarkPrice].Header.Caption = ClosingConstants.CAPTION_MarkPrice;
                band.Columns[ClosingConstants.COL_UnRealizedPNL].Header.Caption = ClosingConstants.CAPTION_UnRealizedPNL;
                band.Columns["btnDelete"].Header.Caption = "Delete";
                band.Columns[ClosingConstants.COL_ClosingAlgoChooser].Header.Caption = ClosingConstants.CAP_ClosingAlgo;
                band.Columns[ClosingConstants.COL_ClosingStatus].Header.Caption = ClosingConstants.CAPTION_ClosingStatus;


                Grid.DisplayLayout.Override.CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
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

        //private void SetColumnSummaries(UltraGrid grdPosition)
        //{
        //    try
        //    {
        //UltraGridBand band = grdData.DisplayLayout.Bands[0];
        //UltraGridColumn colTradeDate = band.Columns[COL_TradeDate];
        //UltraGridColumn colProcessDate = band.Columns[COL_ProcessDate];
        //UltraGridColumn colOriginalPurchaseDate = band.Columns[COL_OriginalPurchaseDate];
        //UltraGridColumn colTransactionType = band.Columns["TransactionType"];
        //UltraGridColumn colExecutedQty = band.Columns["ExecutedQty"];
        //UltraGridColumn colOpenQuantity = band.Columns[COL_OpenQuantity];
        //UltraGridColumn colAveragePrice = band.Columns[COL_AveragePrice];
        //UltraGridColumn colClosingMark = band.Columns["ClosingMark"];
        //UltraGridColumn colOpenCommission = band.Columns[COL_OpenCommission];
        //UltraGridColumn colNetNotionalValue = band.Columns[COL_NetNotionalValue];
        //UltraGridColumn colRealizedPNL = band.Columns["RealizedPNL"];
        //UltraGridColumn colUnRealizedPNL = band.Columns["UnRealizedPNL"];
        //band.Summaries.Add(colTradeDate.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcSum"), colTradeDate, SummaryPosition.UseSummaryPositionColumn, colTradeDate);
        //band.Summaries.Add(colProcessDate.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcNum"), colProcessDate, SummaryPosition.UseSummaryPositionColumn, colProcessDate);
        //band.Summaries.Add(colOriginalPurchaseDate.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcNum"), colOriginalPurchaseDate, SummaryPosition.UseSummaryPositionColumn, colOriginalPurchaseDate);
        //band.Summaries.Add(colTransactionType.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcNum"), colTransactionType, SummaryPosition.UseSummaryPositionColumn, colTransactionType);
        //band.Summaries.Add(colExecutedQty.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcNum"), colExecutedQty, SummaryPosition.UseSummaryPositionColumn, colExecutedQty);
        //band.Summaries.Add(colOpenQuantity.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcNum"), colOpenQuantity, SummaryPosition.UseSummaryPositionColumn, colOpenQuantity);

        //band.Summaries.Add(colAveragePrice.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcSum"), colAveragePrice, SummaryPosition.UseSummaryPositionColumn, colAveragePrice);
        //band.Summaries.Add(colClosingMark.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcSum"), colClosingMark, SummaryPosition.UseSummaryPositionColumn, colClosingMark);
        //band.Summaries.Add(colOpenCommission.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcSum"), colOpenCommission, SummaryPosition.UseSummaryPositionColumn, colOpenCommission);
        //band.Summaries.Add(colNetNotionalValue.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcSum"), colNetNotionalValue, SummaryPosition.UseSummaryPositionColumn, colNetNotionalValue);
        //band.Summaries.Add(colRealizedPNL.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcSum"), colRealizedPNL, SummaryPosition.UseSummaryPositionColumn, colRealizedPNL);
        //band.Summaries.Add(colUnRealizedPNL.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcSum"), colUnRealizedPNL, SummaryPosition.UseSummaryPositionColumn, colUnRealizedPNL);


        //foreach (SummarySettings summary in band.Summaries)
        //{
        //    summary.DisplayFormat = "{0}";
        //}
        //grdData.DisplayLayout.Override.SummaryDisplayArea |= SummaryDisplayAreas.Bottom;
        //grdData.DisplayLayout.Override.SummaryDisplayArea |= SummaryDisplayAreas.InGroupByRows;
        //grdData.DisplayLayout.Override.SummaryDisplayArea |= SummaryDisplayAreas.RootRowsFootersOnly;
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
        private void btnPostTransaction_Click(object sender, EventArgs e)
        {
            try
            {
                if (launchForm != null)
                {
                    ListEventAargs args = new ListEventAargs();
                    args.listOfValues.Add(ApplicationConstants.CONST_MANUAL_TRADING_TICKET_UI.ToString());
                    launchForm(this, args);
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

        internal void SetUp(int accountID, string symbol, DateTime date, string comment, EventHandler UpdateCommentsFromPostReconAmendmentsUI)
        {
            try
            {
                SetLabelText(lblSymbolValue, symbol, true, "lblSymbolValue");

                SetLabelText(lblAccountValue, CachedDataManager.GetInstance.GetAccountText(accountID), true, "lblAccountValue");

                _accountID = accountID;
                dtEndDate.Value = date;
                dtStartDate.Value = date;
                //CHMW-1620 [Closing] - Add Comments field in PostReconAmendenmtsUI
                tbComments.Text = comment;
                _isCommentsUpdated = false;
                UpdateCommentsFromPostReconAmendmentsInReconOutput = UpdateCommentsFromPostReconAmendmentsUI;
                DateTime NAVlockAppliedDate = new DateTime();
                //Set NAV lock Period label and nav lock Date label
                NAVLockItem accountNAVlockDetail = NAVLockManager.GetInstance.getNAVLockItemDetails(accountID);
                if (accountNAVlockDetail.LastLockDate.Date != DateTime.MinValue.Date)
                {
                    lblLockDateValue.Text = accountNAVlockDetail.LockAppliedDate.ToString(ApplicationConstants.DateFormat);
                    dtStartDate.Value = accountNAVlockDetail.LockAppliedDate.AddDays(1);
                    dtStartDate.Enabled = false;
                    if (Enum.IsDefined(typeof(Utilities.UI.CronUtility.ScheduleType), accountNAVlockDetail.LockSchedule))
                    {
                        lblNAVPeriodValue.Text = ((Utilities.UI.CronUtility.ScheduleType)accountNAVlockDetail.LockSchedule).ToString();
                    }
                    else
                    {
                        lblNAVPeriodValue.Text = "None";
                    }
                    NAVlockAppliedDate = accountNAVlockDetail.LockAppliedDate;
                }
                else
                {
                    lblLockDateValue.Text = "N/A";
                    lblNAVPeriodValue.Text = "UnLocked";
                    dtStartDate.Value = DateTimeConstants.MinValue;
                    dtStartDate.Enabled = false;
                    NAVlockAppliedDate = Convert.ToDateTime(DateTimeConstants.DateTimeMinVal);
                }
                RefreshProxies();
                ReloadPNLFromDB();

                backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);
                backgroundWorker.DoWork += new DoWorkEventHandler(backgroundWorker_DoWork);

                if (!backgroundWorker.IsBusy)
                {
                    backgroundWorker.RunWorkerAsync();
                }

                SetColorAndFormattingForPNLFields();

                #region Load previous save layout(Splitter Position)
                int splitterDistanceSaved = 0;
                int splitterHeightSaved = 0;
                if (int.TryParse(PostReconLayout.SplitterLocation, out splitterDistanceSaved) && int.TryParse(PostReconLayout.SplitterHeight, out splitterHeightSaved))
                {
                    splitContainer3.SplitterDistance = splitterDistanceSaved * splitContainer3.Height / splitterHeightSaved;
                }
                #endregion

                if (_closingServices != null)
                {
                    preferences = _closingServices.InnerChannel.GetPreferences();

                    if (preferences != null)
                    {
                        chkBxIsAutoCloseStrategy.Checked = preferences.ClosingMethodology.IsAutoCloseStrategy;
                        int i = 0;
                        _quantityColumnFormat = "0." + i.ToString().PadLeft(preferences.QtyRoundoffDigits, '0');//_quantityColumnFormat = i.ToString("D4");
                        _currencyColumnFormat = "0." + i.ToString().PadLeft(preferences.PriceRoundOffDigits, '0');

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

        private void RefreshProxies()
        {
            try
            {
                #region Create filter
                List<int> accountID = new List<int>();
                accountID.Add(_accountID);

                FilterDataByExactAccount accountFilterdata = new FilterDataByExactAccount();
                accountFilterdata.GivenAccountID = accountID;

                FilterDataByExactSymbol symbolFilterdata = new FilterDataByExactSymbol();
                symbolFilterdata.GivenSymbol = lblSymbolValue.Text;


                FilterDataByToDate filterdata = new FilterDataByToDate();
                filterdata.ToDate = (DateTime)dtEndDate.Value;


                //filter date modified
                FilterDataForLastDateModified filterdataDateModified = new FilterDataForLastDateModified();
                filterdataDateModified.TillDate = (DateTime)dtStartDate.Value;

                List<FilterData> filter = new List<FilterData>();
                filter.Add(filterdata);
                filter.Add(filterdataDateModified);
                filter.Add(accountFilterdata);
                filter.Add(symbolFilterdata);
                #endregion

                if (_proxy != null)
                {
                    _proxy.InnerChannel.UnSubscribe(Topics.Topic_Allocation);
                    _proxy.InnerChannel.UnSubscribe(Topics.Topic_Closing);
                    _proxy.Subscribe(Topics.Topic_Allocation, filter);
                    _proxy.Subscribe(Topics.Topic_Closing, filter);
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
        /// Reload PNL From DB
        /// </summary>
        private void ReloadPNLFromDB()
        {
            try
            {
                DateTime NAVlockAppliedDate = Convert.ToDateTime(DateTimeConstants.DateTimeMinVal);
                if (!lblLockDateValue.Text.Equals("N/A"))
                {
                    DateTime.TryParseExact(lblLockDateValue.Text, ApplicationConstants.DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out NAVlockAppliedDate);
                }

                object[] arguments = new object[4];
                arguments[0] = CachedDataManager.GetInstance.GetAccountText(_accountID);
                arguments[1] = lblSymbolValue.Text;
                arguments[2] = NAVlockAppliedDate;
                arguments[3] = (DateTime)dtEndDate.Value;

                BackgroundWorker bgFetchPNL = new BackgroundWorker();
                bgFetchPNL.DoWork += new DoWorkEventHandler(bgFetchPNL_DoWork);
                bgFetchPNL.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgFetchPNL_RunWorkerCompleted);
                bgFetchPNL.RunWorkerAsync(arguments);
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
        /// set text on label and ToolTip(Optional)
        /// </summary>
        /// <param name="lbl"></param>
        /// <param name="value"></param>
        /// <param name="isToolTipToBeAdded"></param>
        /// <param name="LabelName"></param>
        private void SetLabelText(Infragistics.Win.Misc.UltraLabel lbl, string value, bool isToolTipToBeAdded, string LabelName)
        {
            try
            {
                double number;
                if (Double.TryParse(value, out number))
                {
                    lbl.Text = string.Format("{0:#,##0.00}", double.Parse(value));
                }
                else
                {
                    lbl.Text = value;
                }

                if (isToolTipToBeAdded)
                {
                    Infragistics.Win.UltraWinToolTip.UltraToolTipInfo ultraToolTipInfo;
                    if (!_dictToolTipInfo.ContainsKey(LabelName))
                    {
                        ultraToolTipInfo = new Infragistics.Win.UltraWinToolTip.UltraToolTipInfo("0", Infragistics.Win.ToolTipImage.Default, null, Infragistics.Win.DefaultableBoolean.Default);
                        _dictToolTipInfo.Add(LabelName, ultraToolTipInfo);
                    }
                    else
                    {
                        ultraToolTipInfo = _dictToolTipInfo[LabelName];
                    }
                    ultraToolTipInfo.ToolTipText = lbl.Text;
                    ultraToolTipManager1.SetUltraToolTip(lbl, ultraToolTipInfo);
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
        /// Returns the Layout as read from the Xml
        /// </summary>
        /// <returns></returns>
        private static PostReconLayout GetPostReconLayout()
        {
            _userID = CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
            _postReconLayoutDirectoryPath = Application.StartupPath + @"\" + ApplicationConstants.PREFS_FOLDER_NAME + @"\" + _userID;
            _postReconLayoutFilePath = _postReconLayoutDirectoryPath + @"\PostReconLayout.xml";

            PostReconLayout importLayout = new PostReconLayout();
            try
            {
                if (!Directory.Exists(_postReconLayoutDirectoryPath))
                {
                    Directory.CreateDirectory(_postReconLayoutDirectoryPath);
                }
                if (File.Exists(_postReconLayoutFilePath))
                {
                    using (FileStream fs = File.OpenRead(_postReconLayoutFilePath))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(PostReconLayout));
                        importLayout = (PostReconLayout)serializer.Deserialize(fs);
                    }
                }

                _postReconLayout = importLayout;
            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            #endregion

            return importLayout;
        }

        private void saveLayoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdData != null)
                {
                    if (grdData.DisplayLayout.Bands[0].Columns.Count > 0)
                    {

                        PostReconLayout.PostReconDataColumns = GetGridColumnLayout(grdData);
                        PostReconLayout.PostReconClosedColumns = GetGridColumnLayout(grdClosed);
                    }
                }
                PostReconLayout.SplitterLocation = splitContainer3.SplitterDistance.ToString();
                PostReconLayout.SplitterHeight = splitContainer3.Height.ToString();
                SaveImportReportLayout();
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
        /// Function Returns a list of Columns of Grid grdReport with Properties as set.
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        public static List<ColumnData> GetGridColumnLayout(UltraGrid grid)
        {
            List<ColumnData> listGridCols = new List<ColumnData>();
            UltraGridBand band = grid.DisplayLayout.Bands[0];
            try
            {
                foreach (UltraGridColumn gridCol in band.Columns)
                {
                    ColumnData colData = new ColumnData();
                    colData.Key = gridCol.Key;
                    colData.Caption = gridCol.Header.Caption;
                    colData.Format = gridCol.Format;
                    colData.Hidden = gridCol.Hidden;
                    colData.VisiblePosition = gridCol.Header.VisiblePosition;
                    colData.Width = gridCol.Width;
                    colData.ExcludeFromColumnChooser = gridCol.ExcludeFromColumnChooser;
                    colData.IsGroupByColumn = gridCol.IsGroupByColumn;
                    colData.Fixed = gridCol.Header.Fixed;
                    colData.CellActivation = gridCol.CellActivation;

                    // Sorted Columns
                    colData.SortIndicator = gridCol.SortIndicator;

                    //// Summary Settings
                    //if (band.Summaries.Exists(gridCol.Key))
                    //{
                    //    string colSummKey = band.Summaries[gridCol.Key].CustomSummaryCalculator.ToString();
                    //    colData.ColSummaryKey = (colSummKey.Contains(".")) ? colSummKey.Split('.')[2] : String.Empty;
                    //    colData.ColSummaryFormat = band.Summaries[gridCol.Key].DisplayFormat;
                    //}

                    //Filter Settings
                    foreach (FilterCondition fCond in band.ColumnFilters[gridCol.Key].FilterConditions)
                    {
                        colData.FilterConditionList.Add(fCond);
                    }
                    colData.FilterLogicalOperator = band.ColumnFilters[gridCol.Key].LogicalOperator;

                    listGridCols.Add(colData);
                }
            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
            return listGridCols;
        }
        /// <summary>
        /// Function Writes to the XMl the Layout(Columns and associated Properties) as User is using
        /// </summary>
        public static void SaveImportReportLayout()
        {
            try
            {

                using (XmlTextWriter writer = new XmlTextWriter(_postReconLayoutFilePath, Encoding.UTF8))
                {
                    writer.Formatting = Formatting.Indented;
                    XmlSerializer serializer;
                    serializer = new XmlSerializer(typeof(PostReconLayout));
                    serializer.Serialize(writer, _postReconLayout);

                    writer.Flush();
                }
            }
            #region catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }
        /// <summary>
        /// Function Sets the Grid Layout as it reads from the List of Columns Layout which are Columns read from XML
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="listColData"></param>
        public static void SetGridColumnLayout(UltraGrid grid, List<ColumnData> listColData)
        {
            List<ColumnData> listSortedGridCols = new List<ColumnData>();
            UltraGridBand band = grid.DisplayLayout.Bands[0];
            ColumnsCollection gridColumns = band.Columns;// Just for readability ;)
            listColData.Sort();
            try
            {
                // Hide All
                foreach (UltraGridColumn gridCol in gridColumns)
                {
                    gridCol.Hidden = true;
                    gridCol.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                }
                //Set Columns Properties
                foreach (ColumnData colData in listColData)
                {
                    if (gridColumns.Exists(colData.Key))
                    {
                        UltraGridColumn gridCol = gridColumns[colData.Key];
                        gridCol.Width = colData.Width;
                        gridCol.Format = colData.Format;
                        gridCol.Header.Caption = colData.Caption;
                        gridCol.Header.VisiblePosition = colData.VisiblePosition;
                        gridCol.Hidden = colData.Hidden;
                        //gridCol.ExcludeFromColumnChooser = colData.ExcludeFromColumnChooser;
                        gridCol.Header.Fixed = colData.Fixed;
                        gridCol.SortIndicator = colData.SortIndicator;
                        gridCol.CellActivation = Activation.NoEdit;

                        // Sorted Columns
                        if (colData.SortIndicator == SortIndicator.Descending || colData.SortIndicator == SortIndicator.Ascending)
                        {
                            listSortedGridCols.Add(colData);
                        }
                        //Summary Settings
                        //if (colData.ColSummaryKey != String.Empty)
                        //{
                        //    SummarySettings summary = band.Summaries.Add(gridCol.Key, SummaryType.Custom, riskSummFactory.GetSummaryCalculator(colData.ColSummaryKey), gridCol, SummaryPosition.UseSummaryPositionColumn, gridCol);
                        //    summary.DisplayFormat = colData.ColSummaryFormat;
                        //}
                        // Filter Settings
                        if (colData.FilterConditionList.Count > 0)
                        {
                            band.ColumnFilters[colData.Key].LogicalOperator = colData.FilterLogicalOperator;
                            foreach (FilterCondition fCond in colData.FilterConditionList)
                            {
                                band.ColumnFilters[colData.Key].FilterConditions.Add(fCond);
                            }
                        }
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
            // Sorted Columns are returned as they need to be handled after data is binded.
            //  return listSortedGridCols;
        }

        private void grdClosed_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {

                //add filter row
                //UltraWinGridUtils.EnableFixedFilterRow(e);                

                //e.Layout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
                SummaryHelper.AddColumnSummary(e, "CostBasisRealizedPNL", string.Empty, SummaryType.Sum);
                SummaryHelper.AddColumnSummary(e, ClosingConstants.COL_ClosedQty, string.Empty, SummaryType.Sum);
                SummaryHelper.AddColumnSummary(e, ClosingConstants.COL_OpenAveragePrice, ClosingConstants.COL_ClosedQty, SummaryType.Average);
                SummaryHelper.AddColumnSummary(e, ClosingConstants.COL_ClosedAveragePrice, ClosingConstants.COL_ClosedQty, SummaryType.Average);

                SummaryHelper.AddColumnSummary(e, "NotionalChange", string.Empty, SummaryType.Sum);
                SummaryHelper.AddColumnSummary(e, "CostBasisGrossPNL", string.Empty, SummaryType.Sum);

                SetSummaryProperties(e);

                e.Layout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;
                e.Layout.Override.RowSelectors = DefaultableBoolean.True;
                e.Layout.Override.CellClickAction = CellClickAction.Edit;





                //if (bool.Equals(_isClosedGridInitialized, false))
                //{
                UltraGridLayout gridLayout = grdClosed.DisplayLayout;
                //    //grdLong.DisplayLayout.Override.RowAppearance.BackColor = Color.Gray;
                //    // deactivate the grid - so that is cannot be edited
                //    //grdClosed.DisplayLayout.Override.AllowGroupBy = DefaultableBoolean.True;

                //    //_grdBandNetPositions.Override.AllowColSwapping = AllowColSwapping.NotAllowed;

                //    // if grid layout is saved by the user
                //    if (ClosingPrefManager.ClosingLayout.NetPositionColumns.Count > 0)
                //    {
                //        List<ColumnData> listNetColData = ClosingPrefManager.ClosingLayout.NetPositionColumns;
                //        ClosingPrefManager.SetGridColumnLayout(grdClosed, listNetColData);
                //    }
                //    else
                //    {

                //    }

                //    //     grdNetPosition.DisplayLayout.Load(saveLayoutforNetPosition, PropertyCategories.All);
                //}
                //grdClosed.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
                #region Load previous save layout(Grid Layout)
                grdClosedSetColumns();
                SetCaptionAndFormatForColumns(grdClosed.DisplayLayout.Bands[0]);
                if (!gridLayout.Bands[0].Columns.Exists("checkBox"))
                {
                    AddCheckBoxinGrid(grdClosed, headerCheckBoxUnWind);
                }
                #endregion

                if (grdClosed.DisplayLayout.Bands[0].Columns.Exists("checkBox"))
                {
                    grdClosed.DisplayLayout.Bands[0].Columns["checkBox"].DataType = typeof(bool);
                    grdClosed.DisplayLayout.Bands[0].Columns["checkBox"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                    grdClosed.DisplayLayout.Bands[0].Columns["checkBox"].CellActivation = Activation.AllowEdit;
                    grdClosed.DisplayLayout.Bands[0].Columns["checkBox"].Header.Caption = string.Empty;
                    grdClosed.DisplayLayout.Bands[0].Columns["checkBox"].Hidden = false;
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
        /// show summary in ultragrid
        /// </summary>
        /// <param name="name"></param>
        /// <param name="col"></param>
        //private void BuildCurrencySummary(UltraGrid grd ,string name, UltraGridColumn col)
        //{
        //    SummarySettings ss = grd.DisplayLayout.Bands[0].Summaries.Add(name, SummaryType.Sum, col);
        //    ss.SummaryPositionColumn = col;
        //    ss.SummaryPosition = SummaryPosition.UseSummaryPositionColumn;
        //    ss.Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;
        //    ss.Appearance.ForeColor = Color.Black;
        //    ss.Appearance.TextHAlign = HAlign.Right;
        //    ss.DisplayFormat = "{0:C}";
        //}

        private void grdClosedSetColumns()
        {
            try
            {
                if (PostReconLayout.PostReconClosedColumns.Count > 0)
                {
                    List<ColumnData> listColData = PostReconLayout.PostReconClosedColumns;
                    SetGridColumnLayout(grdClosed, listColData);
                    foreach (string col in GetAllDisplayableColumns(false))
                    {
                        if (grdClosed.DisplayLayout.Bands[0].Columns.Exists(col))
                        {
                            UltraGridColumn column = grdClosed.DisplayLayout.Bands[0].Columns[col];
                            column.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                        }
                    }
                }
                else
                {
                    //SetColumnsForClosedGrid(grdClosed.DisplayLayout.Bands[0]);
                    LoadColumns(grdClosed, false);
                }
                SetGridClosedColumnFormatting(grdClosed);
                SetGridClosedColumnCustomizations(grdClosed);
                //SetColumnSummaries(grdData);
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

        private void SetGridClosedColumnCustomizations(UltraGrid Grid)
        {
            try
            {
                //Grid.CreationFilter = headerCheckBox;

                ColumnsCollection Columns = Grid.DisplayLayout.Bands[0].Columns;

                Columns["ClosingTradeDate"].CellActivation = Activation.NoEdit;
                Columns["ClosedQty"].CellActivation = Activation.NoEdit;
                Columns["OpenAveragePrice"].CellActivation = Activation.NoEdit;
                Columns["ClosedAveragePrice"].CellActivation = Activation.NoEdit;
                Columns["CostBasisRealizedPNL"].CellActivation = Activation.NoEdit;
                Columns["NotionalChange"].CellActivation = Activation.NoEdit;
                Columns["CostBasisGrossPNL"].CellActivation = Activation.NoEdit;
                Columns["FundValue"].CellActivation = Activation.NoEdit;
                Columns["Strategy"].CellActivation = Activation.NoEdit;
                Columns["PositionalTag"].CellActivation = Activation.NoEdit;
                Columns["AssetCategoryValue"].CellActivation = Activation.NoEdit;
                Columns["Symbol"].CellActivation = Activation.NoEdit;
                Columns["PositionSide"].CellActivation = Activation.NoEdit;

                Columns["ClosingTradeDate"].Header.Caption = "Closing Date";
                Columns["ClosedQty"].Header.Caption = "Qty Closed";
                Columns["OpenAveragePrice"].Header.Caption = "Opening Price";
                Columns["ClosedAveragePrice"].Header.Caption = "Closing Price";
                Columns["CostBasisRealizedPNL"].Header.Caption = "Realized PNL(C.B.)";
                Columns["NotionalChange"].Header.Caption = "Notional Change";
                Columns["CostBasisGrossPNL"].Header.Caption = "Gross PNL(C.B.)";
                Columns["AccountValue"].Header.Caption = "Account";
                Columns["Strategy"].Header.Caption = "Strategy";
                Columns["PositionalTag"].Header.Caption = "Position Type";
                Columns["AssetCategoryValue"].Header.Caption = "Asset";
                Columns["Symbol"].Header.Caption = "Symbol";
                Columns["PositionSide"].Header.Caption = "Position Side";




                Grid.DisplayLayout.Override.CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
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

        private void SetGridClosedColumnFormatting(UltraGrid Grid)
        {
            try
            {

                ColumnsCollection columns = Grid.DisplayLayout.Bands[0].Columns;

                columns["ClosingTradeDate"].Format = "MM/dd/yyyy";
                columns["ClosedQty"].Format = _quantityColumnFormat;
                columns["OpenAveragePrice"].Format = _currencyColumnFormat;
                columns["ClosedAveragePrice"].Format = _currencyColumnFormat;
                columns["CostBasisRealizedPNL"].Format = _currencyColumnFormat;
                columns["NotionalChange"].Format = _currencyColumnFormat;
                columns["CostBasisGrossPNL"].Format = _currencyColumnFormat;
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
        /// Sets the columns for closed grid.
        /// </summary>
        /// <param name="gridBand">The grid band.</param>
        /// <param name="childGridBand">The child grid band.</param>
        /// <param name="grd">The GRD.</param>
        /// <param name="isAccountData">if set to <c>true</c> [is account data].</param>
        //private void SetColumnsForClosedGrid(UltraGridBand gridBand)
        //{
        //    try
        //    {
        //        foreach (UltraGridColumn column in gridBand.Columns)
        //        {
        //            //column.CellActivation = Activation.NoEdit;
        //            column.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
        //        }

        //        int visiblePosition = 1;

        //        UltraGridColumn colClosingTradeDate = gridBand.Columns[ClosingConstants.COL_ClosingTradeDate];
        //        colClosingTradeDate.Header.Caption = ClosingConstants.CAP_CloseDt;
        //        colClosingTradeDate.Header.VisiblePosition = visiblePosition++;
        //        colClosingTradeDate.Width = 80;
        //        colClosingTradeDate.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DateTimeWithoutDropDown;
        //        //colClosingTradeDate.CellActivation = Activation.NoEdit;

        //        UltraGridColumn colAccount = gridBand.Columns[ClosingConstants.COL_AccountValue];
        //        colAccount.Header.Caption = ClosingConstants.CAP_Account;
        //        colAccount.Header.VisiblePosition = visiblePosition++;
        //        colAccount.Width = 100;
        //        //colAccount.CellActivation = Activation.NoEdit;

        //        UltraGridColumn colStrategyIDValue = gridBand.Columns[ClosingConstants.COL_Strategy];
        //        colStrategyIDValue.Header.Caption = ClosingConstants.CAP_Strategy;
        //        colStrategyIDValue.Header.VisiblePosition = visiblePosition++;
        //        colStrategyIDValue.Width = 120;
        //        //colStrategyIDValue.CellActivation = Activation.NoEdit;


        //        UltraGridColumn colPositionTag = gridBand.Columns[ClosingConstants.COL_PositionTag];
        //        colPositionTag.Header.Caption = ClosingConstants.CAP_PositionType;
        //        colPositionTag.Header.VisiblePosition = visiblePosition++;
        //        colPositionTag.Width = 70;
        //        //colPositionTag.CellActivation = Activation.NoEdit;


        //UltraGridColumn colAssetCategoryValue = gridBand.Columns[ClosingConstants.COL_AssetCategoryValue];
        //colAssetCategoryValue.Header.Caption = ClosingConstants.CAP_AssetCategory;
        //colAssetCategoryValue.Header.VisiblePosition = visiblePosition++;
        //colAssetCategoryValue.Width = 60;
        //colAssetCategoryValue.CellActivation = Activation.NoEdit;


        //        UltraGridColumn colSymbol = gridBand.Columns[ClosingConstants.COL_Symbol];
        //        colSymbol.Header.Caption = ClosingConstants.CAP_Symbol;
        //        colSymbol.Header.VisiblePosition = visiblePosition++;
        //        colSymbol.Width = 70;
        //        //colSymbol.CellActivation = Activation.NoEdit;


        //        UltraGridColumn colClosingQuantity = gridBand.Columns[ClosingConstants.COL_ClosedQty];
        //        colClosingQuantity.Header.Caption = ClosingConstants.CAP_CloseQty;
        //        colClosingQuantity.Header.VisiblePosition = visiblePosition++;
        //        colClosingQuantity.Width = 70;
        //        colClosingQuantity.Format = _quantityColumnFormat;
        //        //colClosingQuantity.CellActivation = Activation.NoEdit;


        //        UltraGridColumn colOpenAveragePrice = gridBand.Columns[ClosingConstants.COL_OpenAveragePrice];
        //        colOpenAveragePrice.Header.Caption = ClosingConstants.CAP_AvgPrice;
        //        colOpenAveragePrice.Header.VisiblePosition = visiblePosition++;
        //        colOpenAveragePrice.Width = 75;
        //        //colOpenAveragePrice.Format = ClosingConstants._currencyColumnFormat;
        //        //colOpenAveragePrice.CellActivation = Activation.NoEdit;


        //        UltraGridColumn colClosedAveragePrice = gridBand.Columns[ClosingConstants.COL_ClosedAveragePrice];
        //        colClosedAveragePrice.Header.Caption = ClosingConstants.CAP_AvgClosingPrice;
        //        colClosedAveragePrice.Header.VisiblePosition = visiblePosition++;
        //        colClosedAveragePrice.Width = 70;
        //        //colClosedAveragePrice.Format = ClosingConstants._currencyColumnFormat;
        //        //colClosedAveragePrice.CellActivation = Activation.NoEdit;


        //        UltraGridColumn colCommision = gridBand.Columns[ClosingConstants.COL_ClosingTotalCommissionandFees];
        //        colCommision.Header.Caption = ClosingConstants.COL_ClosingTotalCommissionandFees;
        //        colCommision.Header.VisiblePosition = visiblePosition++;
        //        colCommision.Width = 80;
        //        //colCommision.Format = ClosingConstants._currencyColumnFormat;
        //        //colCommision.Hidden = true;
        //        //colCommision.CellActivation = Activation.NoEdit;

        //        UltraGridColumn colRealizedPNL = gridBand.Columns[ClosingConstants.COL_RealizedPNL];
        //        colRealizedPNL.Header.Caption = ClosingConstants.CAP_RealizedPNL;
        //        colRealizedPNL.Header.VisiblePosition = visiblePosition++;
        //        colRealizedPNL.Width = 90;
        //        //colRealizedPNL.Format = ClosingConstants._currencyColumnFormat;
        //        //colRealizedPNL.CellActivation = Activation.NoEdit;

        //        UltraGridColumn colExchange = gridBand.Columns[ClosingConstants.COL_Exchange];
        //        colExchange.Header.Caption = ClosingConstants.CAP_Exchange;
        //        colExchange.Hidden = true;

        //        UltraGridColumn colSide = gridBand.Columns[ClosingConstants.COL_PositionSide];
        //        colSide.Header.Caption = ClosingConstants.CAP_OpeningSide;
        //        colSide.Hidden = true;


        //        //UltraGridColumn colPositionCommision = gridBand.Columns[ClosingConstants.COL_PositionCommission];
        //        //colPositionCommision.Header.Caption = ClosingConstants.CAP_PositionCommission;
        //        //colPositionCommision.Hidden = true;

        //        UltraGridColumn colClosingSide = gridBand.Columns[ClosingConstants.COL_ClosingSide];
        //        colClosingSide.Header.Caption = ClosingConstants.CAP_ClosingSide;
        //        colClosingSide.Hidden = true;

        //        //UltraGridColumn colPositionID = gridBand.Columns[ClosingConstants.COL_ID];
        //        //colPositionID.Header.Caption = ClosingConstants.CAP_TaxlotId;
        //        //colPositionID.Hidden = true;

        //        UltraGridColumn colCurrency = gridBand.Columns[ClosingConstants.COL_Currency];
        //        colCurrency.Header.Caption = ClosingConstants.CAP_Currency;
        //        colCurrency.Hidden = true;

        //        UltraGridColumn colUnderlying = gridBand.Columns[ClosingConstants.COL_Underlying];
        //        colUnderlying.Header.Caption = ClosingConstants.CAP_Underlying;
        //        colUnderlying.Hidden = true;

        //        //UltraGridColumn colClosingID = gridBand.Columns[ClosingConstants.COL_ClosingID];
        //        //colClosingID.Header.Caption = ClosingConstants.CAP_ClosingID;
        //        //colClosingID.Hidden = true;

        //        UltraGridColumn colClosingMode = gridBand.Columns[ClosingConstants.COL_ClosingMode];
        //        colClosingMode.Header.Caption = ClosingConstants.CAP_ClosingMode;
        //        colClosingMode.Hidden = true;

        //        UltraGridColumn colClosingTag = gridBand.Columns[ClosingConstants.COL_ClosingTag];
        //        colClosingTag.Header.Caption = ClosingConstants.CAP_ClosedPositionType;
        //        colClosingTag.Hidden = true;

        //        UltraGridColumn colTradeDate = gridBand.Columns[ClosingConstants.COL_TradeDatePosition];
        //        colTradeDate.Header.Caption = ClosingConstants.CAP_TradeDate;
        //        colTradeDate.Hidden = true;
        //        colTradeDate.CellActivation = Activation.NoEdit;

        //        UltraGridColumn colNotionalValue = gridBand.Columns[ClosingConstants.COL_NotionalValue];
        //        colNotionalValue.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
        //        colNotionalValue.Hidden = true;

        //        UltraGridColumn colAccountID = gridBand.Columns[ClosingConstants.COL_AccountID];
        //        colAccountID.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
        //        colAccountID.Hidden = true;

        //        UltraGridColumn colSettleExpire = gridBand.Columns[ClosingConstants.COL_IsExpired_Settled];
        //        colSettleExpire.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
        //        colSettleExpire.Hidden = true;

        //        UltraGridColumn colTimeOfSaveUTC = gridBand.Columns[ClosingConstants.COL_CloseDate];
        //        colTimeOfSaveUTC.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
        //        colTimeOfSaveUTC.Hidden = true;

        //        UltraGridColumn colStrategyID = gridBand.Columns[ClosingConstants.COL_StrategyID];
        //        colStrategyID.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
        //        colStrategyID.Hidden = true;

        //        UltraGridColumn colCurrencyID = gridBand.Columns[ClosingConstants.COL_CurrencyID];
        //        colCurrencyID.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
        //        colCurrencyID.Hidden = true;

        //        UltraGridColumn ColAUECID = gridBand.Columns[ClosingConstants.COL_AUECID];
        //        ColAUECID.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
        //        ColAUECID.Hidden = true;

        //        UltraGridColumn ColDescription = gridBand.Columns[ClosingConstants.COL_Description];
        //        ColDescription.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
        //        ColDescription.Hidden = true;

        //        UltraGridColumn ColMultiplier = gridBand.Columns[ClosingConstants.COL_Multiplier];
        //        ColMultiplier.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
        //        ColMultiplier.Hidden = true;

        //    }
        //    catch (Exception ex)
        //    {
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }

        //}

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
                //colOpenAveragePrice.Format = ClosingConstants._currencyColumnFormat;

                UltraGridColumn colClosedAveragePrice = gridBand.Columns[ClosingConstants.COL_ClosedAveragePrice];
                colClosedAveragePrice.Header.Caption = ClosingConstants.CAP_AvgClosingPrice;
                //colClosedAveragePrice.Format = ClosingConstants._currencyColumnFormat;

                UltraGridColumn colCommision = gridBand.Columns[ClosingConstants.COL_ClosingTotalCommissionandFees];
                colCommision.Header.Caption = ClosingConstants.COL_ClosingTotalCommissionandFees;
                //colCommision.Format = ClosingConstants._currencyColumnFormat;

                UltraGridColumn colRealizedPNL = gridBand.Columns[ClosingConstants.COL_RealizedPNL];
                colRealizedPNL.Header.Caption = ClosingConstants.CAP_RealizedPNL;
                colRealizedPNL.Format = "0.00";

                UltraGridColumn colExchange = gridBand.Columns[ClosingConstants.COL_Exchange];
                colExchange.Header.Caption = ClosingConstants.CAP_Exchange;

                UltraGridColumn colSide = gridBand.Columns[ClosingConstants.COL_PositionSide];
                colSide.Header.Caption = ClosingConstants.CAP_OpeningSide;

                UltraGridColumn colPositionCommision = gridBand.Columns[ClosingConstants.COL_PositionCommission];
                colPositionCommision.Header.Caption = ClosingConstants.CAP_PositionCommission;

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

                UltraGridColumn colClosingAlgo = gridBand.Columns[ClosingConstants.COL_ClosingAlgo];
                colClosingAlgo.Header.Caption = ClosingConstants.CAP_ClosingAlgo;

                List<EnumerationValue> closingAlgoType = EnumHelper.ConvertEnumForBindingWithSelectValue(typeof(PostTradeEnums.CloseTradeAlogrithm));
                ValueList closingAlgoValueList = new ValueList();
                foreach (EnumerationValue value in closingAlgoType)
                {
                    closingAlgoValueList.ValueListItems.Add(value.Value, value.DisplayText);
                }
                colClosingAlgo.ValueList = closingAlgoValueList;

                UltraGridColumn colClosingMode = gridBand.Columns[ClosingConstants.COL_ClosingMode];
                colClosingMode.Header.Caption = ClosingConstants.CAP_ClosingMode;

                UltraGridColumn colClosingTag = gridBand.Columns[ClosingConstants.COL_ClosingTag];
                colClosingTag.Header.Caption = ClosingConstants.CAP_ClosedPositionType;

                //Modified By : Manvendra Prjapati
                //Jira : http://jira.nirvanasolutions.com:8080/browse/PRANA-9018
                UltraGridColumn colTradeDate = gridBand.Columns[ClosingConstants.COL_TradeDatePosition];
                colTradeDate.Header.Caption = ClosingPrefManager.GetCaptionBasedonClosingDateType(); //ClosingConstants.CAP_TradeDate;
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
        private void grdClosed_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                Position position = e.Row.ListObject as Position;
                if (position != null)
                {
                    if (position.IsClosingSaved)
                    {
                        e.Row.Appearance.ForeColor = Color.White;
                    }
                    else
                    {
                        e.Row.Appearance.ForeColor = Color.Violet;
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
        //private void SetCloseTradesGridRowAppearance(InitializeRowEventArgs e)
        //{
        //    try
        //    {
        //        string side = string.Empty;
        //        if (_isCloseTradeInitialized && !e.Row.Appearance.ForeColor.Equals(Color.White))
        //            e.Row.Appearance.ForeColor = Color.Violet;
        //        else
        //            e.Row.Appearance.ForeColor = Color.White;

        //        //e.Row.Appearance.BackColor = Color.Lavender;
        //        //grdCloseTrades.DisplayLayout.Bands[0].Override.CellClickAction = CellClickAction.RowSelect;
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
        /// Delete trade selected
        /// trade will be selected on right click as well
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                TaxLot taxLot = (TaxLot)grdData.ActiveRow.ListObject;
                if (!IsAccountLocked())
                {
                    DialogResult dr = MessageBox.Show("Do you want to delete the selected trade(s)?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dr == DialogResult.Yes)
                    {
                        if (taxLot != null && ValidateTaxLotToDelete(taxLot))
                        {
                            AmendmentsHelper.DeleteTaxLot(taxLot);
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
        /// Check if the taxLot can be deleted or not
        /// </summary>
        /// <param name="taxLot"></param>
        /// <returns></returns>
        private bool ValidateTaxLotToDelete(TaxLot taxLot)
        {
            try
            {

                List<string> lstTaxLotID = new List<string>();
                lstTaxLotID.Add(taxLot.TaxLotID);
                //List<AllocationGroup> listAllocationGroup = AllocationManager.GetInstance().GetGroups(lstTaxLotID, true);
                List<AllocationGroup> listAllocationGroup = new List<AllocationGroup>();
                if (listAllocationGroup != null && listAllocationGroup.Count > 0)
                {
                    if (taxLot.TaxLotQty > 0 && taxLot.TaxLotQtyToClose > 0 && taxLot.TaxLotQty != taxLot.ExecutedQty)
                    {
                        MessageBox.Show("This taxlot is Partially Closed.\nOnly open trades can be deleted.", "Close Trade Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                    //modified by amit 24.03.2015
                    //http://jira.nirvanasolutions.com:8080/browse/CHMW-2952
                    PostTradeEnums.Status groupStatus = listAllocationGroup[0].GroupStatus;

                    if (!IsAccountLocked())
                    {
                        return false;
                    }
                    if (groupStatus.Equals(PostTradeEnums.Status.Closed))
                    {
                        MessageBox.Show("Only open trades can be deleted.", "Close Trade Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                    else if (groupStatus.Equals(PostTradeEnums.Status.CorporateAction))
                    {
                        MessageBox.Show("Corporate  Action is applied. First undo the corporate action to delete the trade.", "Close Trade Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                    else if (groupStatus.Equals(PostTradeEnums.Status.Exercise) || groupStatus.Equals(PostTradeEnums.Status.IsExercised) || groupStatus.Equals(PostTradeEnums.Status.ExerciseAssignManually))
                    {
                        MessageBox.Show("Exercised trades cannot be deleted.", "Close Trade Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                    return true;
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
            return false;
        }


        #region MovedCode
        //Code moved to AmendmentsHelper Class
        //private void DeleteTaxLot(TaxLot taxLot)
        //{
        //    DialogResult dr = MessageBox.Show("Do you want to delete the selected trade(s)?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        //    if (dr == DialogResult.Yes)
        //    {
        //        if (taxLot != null && ValidateTaxLotToDelete(taxLot))
        //        {
        //            //create dictionary to delete the tax lot with tax lotID and tax lot status
        //            Dictionary<string, List<ApprovedChanges>> dictDeleteChanges = new Dictionary<string, List<ApprovedChanges>>();
        //            ApprovedChanges approvedChanges = new ApprovedChanges();
        //            approvedChanges.TaxlotID = taxLot.TaxLotID;
        //            approvedChanges.TaxlotStatus = BusinessObjects.AppConstants.AmendedTaxLotStatus.Deleted;
        //            List<ApprovedChanges> lstApprovedChanges = new List<ApprovedChanges>();
        //            lstApprovedChanges.Add(approvedChanges);
        //            dictDeleteChanges.Add(taxLot.TaxLotID, lstApprovedChanges);

        //            //Delete the amendment dictionary
        //            AllocationManager.GetInstance().MakeNewCancelAmendChanges(dictDeleteChanges);
        //        }
        //    }
        //}


        #endregion
        /// <summary>
        /// Set if the delete button should be visible or not
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            try
            {
                //http://jira.nirvanasolutions.com:8080/browse/CHMW-1602
                //moved to designer 
                //deleteToolStripMenuItem.Visible = false;
                ////validate if the nave for the tradedate is locked or not 
                //if (grdData.ContainsFocus && grdData.ActiveRow != null && grdData.Rows.Count > 0)
                //{
                //    deleteToolStripMenuItem.Visible = true;
                //    if (NAVLockManager.GetInstance.ValidateTrade(
                //        ((TaxLot)grdData.ActiveRow.ListObject).Level1ID,
                //        ((TaxLot)grdData.ActiveRow.ListObject).AUECLocalDate))
                //    {
                //        deleteToolStripMenuItem.Enabled = true;
                //    }
                //    else
                //    {
                //        deleteToolStripMenuItem.Enabled = false;
                //    }
                //}
                //else
                //{
                //deleteToolStripMenuItem.Visible = false;
                //}
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
        /// Drag Drop Handling
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdData_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                grdData.Focus();

                if (e.Button == MouseButtons.Right)
                {
                    UltraWinGridUtils.RightClickRowSelect(grdData, e.Location);
                }
                unwindToolStripMenuItem.Visible = false;
                exportToolStripMenuItem.Visible = false;
                //Code not required anymore
                //if (e.Button == MouseButtons.Left)
                //{
                //    _isGridLeftMousedown = true;
                //}
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
        /// Drag Drop Handling
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //public static void RightClickRowSelect(UltraGrid grid, Point mouseLocation)
        //{
        //    try
        //    {
        //        UIElement element = grid.DisplayLayout.UIElement.ElementFromPoint(mouseLocation);
        //        var row = element.GetContext(typeof(UltraGridRow)) as UltraGridRow;
        //        if (row != null && row.IsDataRow)
        //        {
        //            grid.ActiveRow = row;
        //            if (!row.Selected)
        //            {
        //                grid.Selected.Rows.Clear();
        //                row.Selected = true;
        //            }
        //        }
        //        else
        //        {
        //            grid.Selected.Rows.Clear();
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
        /// mouse down event of the grid.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        private void grdData_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                GridDragDrop(sender, e);
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
        /// Drag Drop Handling
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdData_DragEnter(object sender, DragEventArgs e)
        {
            try
            {
                GridDragEnter(sender, e);
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
        /// Drag Drop Handling
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdData_DragOver(object sender, DragEventArgs e)
        {
            try
            {
                GridDragOver(sender, e);

                // Added by Ankit Gupta on 31st Dec, 2014.
                // http://jira.nirvanasolutions.com:8080/browse/CHMW-1902
                // User should be able to scroll through open taxlots while manual closing
                // http://www.infragistics.com/community/forums/p/94055/465183.aspx
                UltraGrid grid = sender as UltraGrid;
                if (grid == null)
                {
                    return;
                }
                Point point = grid.PointToClient(new Point(e.X, e.Y));
                if (point.Y < 20)
                {
                    // To control the auto scroll speed, Sleep method was used, with argument 0.1 Seconds.
                    this.grdData.ActiveRowScrollRegion.Scroll(RowScrollAction.LineUp);
                    Thread.Sleep(100);
                }
                else if (point.Y > grid.Height - 20)
                {
                    this.grdData.ActiveRowScrollRegion.Scroll(RowScrollAction.LineDown);
                    Thread.Sleep(100);
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
        /// This method initiate the postion closing manually on dropping.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridDragDrop(object sender, DragEventArgs e)
        {
            try
            {
                //check for account lock
                if (!IsAccountLocked())
                {
                    return;
                }

                //retrieve reference to grid
                UltraGrid ug = (UltraGrid)sender;

                //retrieve reference to cell
                UIElement uieleMouseOver = ug.DisplayLayout.UIElement.ElementFromPoint(ug.PointToClient(new Point(e.X, e.Y)));
                UltraGridRow rowMouseOver = uieleMouseOver.GetContext(typeof(UltraGridRow)) as UltraGridRow;
                TaxLot draggedAllocatedTrade = null;
                if (rowMouseOver != null)
                {
                    TaxLot targetAllocatedTrade = rowMouseOver.ListObject as TaxLot;
                    //retrieve data
                    DataObject doClipboard = (DataObject)e.Data;

                    //print all available clipboard formats
                    //string[] arrayOfFormats = doClipboard.GetFormats(true);
                    //for (int i = 0; i < arrayOfFormats.Length; i++)
                    //{
                    //    Console.WriteLine(arrayOfFormats[i]);
                    //}

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
                            if (targetAllocatedTrade.TaxLotID.Equals(draggedAllocatedTrade.TaxLotID))
                            {
                                return;
                            }
                            if (targetAllocatedTrade.OrderSide.Equals(draggedAllocatedTrade.OrderSide))
                            {
                                MessageBox.Show("Trade with same side cannot be closed.", "Close Trade Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                return;
                            }
                            else if ((targetAllocatedTrade.OrderSide.ToUpperInvariant().Equals(SIDEBUY)
                                                && draggedAllocatedTrade.OrderSide.ToUpperInvariant().Equals(SIDEBUYTOCLOSE))
                                || (targetAllocatedTrade.OrderSide.ToUpperInvariant().Equals(SIDEBUYTOCLOSE)
                                                && draggedAllocatedTrade.OrderSide.ToUpperInvariant().Equals(SIDEBUY)))
                            {
                                MessageBox.Show("Trade with same side cannot be closed.", "Close Trade Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
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
                            //modified by sachin mishra 26-02-2015 Purpose: CHMW-2750 User is able to close trades of different asset class
                            else if (!(targetAllocatedTrade.AssetName.Equals(draggedAllocatedTrade.AssetName)))
                            {
                                MessageBox.Show("Trade with different Asset Class cannot be closed.", "Close Trade Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                return;
                            }

                            //if (_dictAmendments.Keys.Contains(draggedAllocatedTrade.TaxLotID) || _dictAmendments.Keys.Contains(targetAllocatedTrade.TaxLotID))
                            //{
                            //    MessageBox.Show("UnSaved trades cannot be closed.", "Close Trade Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            //    return;
                            //}

                            targetAllocatedTrade.ClosingMode = ClosingMode.None;
                            draggedAllocatedTrade.ClosingMode = ClosingMode.None;
                            //draggedAllocatedTrade = _closingServices.InnerChannel.GetAllocatedTrade(draggedAllocatedTrade);

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
                            closingParams.IsVirtualClosingPopulate = true;
                            closingParams.IsOverrideWithUserClosing = false;
                            closingParams.IsMatchStrategy = !isAutoCloseStrategy;
                            closingParams.ClosingField = PostTradeEnums.ClosingField.Default;

                            if (PostReconClosingData.TaxlotIDList.Length > 0)
                            {
                                string[] VirtualUnwindedTaxlots = PostReconClosingData.TaxlotIDList.ToString().Split(',');
                                closingParams.VirtualUnwidedTaxlots = VirtualUnwindedTaxlots.ToList();
                            }
                            ClosingData closingdata = _closingServices.InnerChannel.AutomaticClosingOnManualOrPresetBasis(closingParams);

                            if (closingdata != null)
                            {
                                if (closingdata.IsNavLockFailed)
                                {
                                    MessageBox.Show(closingdata.ErrorMsg.ToString(), "Nav Lock", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                else
                                {
                                    AmendmentsHelper.UpdateUnRealizedRealizedPNLAfterClosingUnwinding(closingdata);

                                    if (closingdata.ClosedPositions.Count > 0)
                                        PostReconClosingData.IsUnsavedChanges = true;
                                    PostReconClosingData.UpdateRepository(closingdata);
                                    PostReconClosingData.UpdateUnsavedClosingData(closingdata);
                                    if (!closingdata.ErrorMsg.ToString().Equals(string.Empty))
                                    {
                                        MessageBox.Show(this, closingdata.ErrorMsg.ToString(), "Close Trade Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                    }
                                    else
                                    {
                                        if (closingdata.IsDataClosed)
                                        {
                                            InformationMessageBox.Display("Close Trade Data Saved");
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
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
        /// <summary>
        /// check for account locked
        /// </summary>
        /// <returns></returns>
        private bool IsAccountLocked()
        {
            bool isAccountLocked = false;
            try
            {
                if (CachedDataManager.GetInstance.isAccountLocked(_accountID))
                {
                    isAccountLocked = true;
                }
                else
                {
                    if (MessageBox.Show("The ability to make changes to a account can only be granted to one user at a time, would you like to proceed in locking " + lblAccount.Text + "?", "Account Lock", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        List<int> newAccountsToBelocked = new List<int>();
                        newAccountsToBelocked.Add(_accountID);
                        newAccountsToBelocked.AddRange(CachedDataManager.GetInstance.GetLockedAccounts());
                        if (AccountLockManager.SetAccountsLockStatus(newAccountsToBelocked))
                        {
                            MessageBox.Show("The lock for " + lblAccount.Text + " has been acquired, you may continue.", "Account Lock", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            isAccountLocked = true;
                        }
                        else
                        {
                            MessageBox.Show(lblAccount.Text + " is currently locked by another user, please refer to the Account Lock screen for more information.", "Account Lock", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            isAccountLocked = false;
                        }
                    }
                    else
                    {
                        isAccountLocked = false;
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
            return isAccountLocked;
        }

        /// <summary>
        /// drag over on the grid.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.DragEventArgs"/> instance containing the event data.</param>
        private void GridDragOver(object sender, DragEventArgs e)
        {
            //on drag over, turn on copy drag drop effect if over a cell

            //get reference to grid
            try
            {
                UltraGrid ug = (UltraGrid)sender;

                //retrieve drag drop data
                DataObject doDrop = (DataObject)e.Data;

                //only do this if there is CSV data
                if (doDrop.GetDataPresent(typeof(TaxLot)) == true)
                {
                    //retrieve reference to cell under mouse pointer
                    UIElement uieleMouseOver = ug.DisplayLayout.UIElement.ElementFromPoint(ug.PointToClient(new Point(e.X, e.Y)));
                    //UltraGridCell cellMouseOver = (UltraGridCell)uieleMouseOver.GetContext(typeof(UltraGridCell));
                    ///Need to fetch for row
                    UltraGridRow rowMouseOver = uieleMouseOver.GetContext(typeof(UltraGridRow)) as UltraGridRow;

                    if (rowMouseOver != null)
                    {
                        //Console.WriteLine(rowMouseOver.Column.Key);
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

        private void grdData_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                UIElement elementEntered = this.grdData.DisplayLayout.UIElement.LastElementEntered;

                if (elementEntered == null)
                {
                    return;
                }
                if ((elementEntered is RowUIElement) || (elementEntered.GetAncestor(typeof(RowUIElement)) != null))
                {
                    //CHMW-2124	[Post Recon Amendments] On delete button,instead of deleting trade is closing in a scenario
                    RowUIElement rowUIElement = elementEntered.GetAncestor(typeof(RowUIElement)) as RowUIElement;
                    if (rowUIElement.Row.IsActiveRow)
                    {
                        GridMouseMove(sender, e);
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
        /// mouse move event of the grid.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        private void GridMouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                //UltraGrid ug = sender as UltraGrid;
                //if left mouse down and mouse move then do drag drop
                if (e.Button == MouseButtons.Left && GetSelectedData(grdData) != null)
                {
                    grdData.DoDragDrop(GetSelectedData(grdData), DragDropEffects.Copy);
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
        /// Gets the selected data.
        /// </summary>
        /// <param name="ugData">The ug data.</param>
        /// <returns></returns>
        private DataObject GetSelectedData(UltraGrid ugData)
        {
            ///If user wants to drag multiple rows, allow him to do so
            try
            {
                if (ugData != null && ugData.ActiveRow != null)
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
                    //using ()
                    //{
                    binaryFormatter.Serialize(msData, selectedAllocatedTrade);
                    //msData.Position = 0;
                    doClipboard.SetData(typeof(TaxLot), msData);

                    //}

                    //put byte array into memory stream
                    //= new MemoryStream(byteData);

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
        /// Handles the MouseUp event of the grdShort control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        //private void grdData_MouseUp(object sender, MouseEventArgs e)
        //{
        //    try
        //    {
        //        // _isGridLeftMousedown = false;
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

        private void grdClosed_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                grdClosed.Focus();
                unwindToolStripMenuItem.Visible = true;
                exportToolStripMenuItem.Visible = true;
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
        /// Delete button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdData_ClickCellButton(object sender, CellEventArgs e)
        {
            try
            {
                if (e.Cell.Text == "Delete")
                {
                    if (IsAccountLocked())
                    {
                        DialogResult dr = MessageBox.Show("Do you want to delete the selected trade(s)?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (dr == DialogResult.Yes)
                        {
                            TaxLot taxLot = (TaxLot)e.Cell.Row.ListObject;
                            if (taxLot != null && ValidateTaxLotToDelete(taxLot))
                            {
                                AmendmentsHelper.DeleteTaxLot(taxLot);
                            }
                        }

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

        private void btnGetData_Click(object sender, EventArgs e)
        {
            try
            {
                #region comments
                // http://jira.nirvanasolutions.com:8080/browse/CHMW-2075
                // When NAV is locked, dtStartDate displays NAV lock date. Earlier, values of dtStartDate & dtEndDate were being compared using date as well as time
                // therefore, even if the date was same, error message was displayed, if time of start date was greater than time of end date.
                // DateTime.Date method removes the time component from DateTime object, and initializes it to 12:00:00 AM.
                #endregion
                int compareDates = DateTime.Compare(((DateTime)dtStartDate.Value).Date, ((DateTime)dtEndDate.Value).Date);
                if (compareDates > 0)
                {
                    MessageBox.Show("End date cannot be less then or equal to NAVLock date.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                DisableEnableForm(false);

                if (backgroundWorker.IsBusy)
                {
                    MessageBox.Show(this.FindForm(), "Please Wait while Data is being fetched", "PostReconAmendment Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    backgroundWorker.RunWorkerAsync();
                }

                ReloadPNLFromDB();
                PostReconClosingData.IsUnsavedChanges = false;
                AmendmentsHelper.ClearAmendments();
                ultraStatusBar1.Text = string.Empty;

                SetColorAndFormattingForPNLFields();
                RefreshProxies();
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
        private void grdData_BeforeCellUpdate(object sender, BeforeCellUpdateEventArgs e)
        {
            try
            {
                if (e.Cell.GetType() == typeof(UltraGridFilterCell) || e.Cell.Column.Key.Contains("CalculationColumn") || e.Cell.Column.Key.Equals(ClosingConstants.COL_ClosingAlgoChooser))
                {
                    return;
                }
                if (!IsAccountLocked())
                {
                    e.Cancel = true;
                }
                else if (e.Cell.Column.Key.Equals(ClosingConstants.COL_AveragePrice) || e.Cell.Column.Key.Equals(ClosingConstants.COL_ExecutedQty))
                {
                    if (!(e.Cell.Row.Cells.Exists("UnRealizedPNL")
                        && e.Cell.Row.Cells["UnRealizedPNL"].Value != null
                        && !string.IsNullOrEmpty(e.Cell.Row.Cells["UnRealizedPNL"].Value.ToString())
                        && double.TryParse(e.Cell.Row.Cells["UnRealizedPNL"].Value.ToString(), out _taxLotUnRealizedPNL)))
                    {
                        _taxLotUnRealizedPNL = double.MinValue;
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
        /// save trades
        /// unwind and close if the amended trades are not open
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bgWorkerUnwindingClosing_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                int rowsAffected = 0;
                bool isChangesSaved = true;
                #region unwind data on clicking on save button for selected position

                //unwind data from the trade date of the closed trade till the last day from yesterday
                if (_isUnwindingClosingTobeDone)
                {
                    object[] arguments = e.Argument as object[];

                    List<Position> listPosition = arguments[0] as List<Position>;
                    //GetClosedDataInformation();
                    UnwindClosing(listPosition);
                }
                if (PostReconClosingData.IsUnsavedChanges)
                {
                    //GetClosedDataInformation();
                    //CHMW-2124	[Post Recon Amendments] On delete button,instead of deleting trade is closing in a scenario
                    if (!string.IsNullOrEmpty(PostReconClosingData.TaxlotClosingID.ToString().Trim()) && !string.IsNullOrEmpty(PostReconClosingData.TaxlotIDList.ToString()))
                    {
                        ClosingData closedData = _allocationServices.InnerChannel.UnWindClosing(PostReconClosingData.TaxlotClosingID.ToString(), PostReconClosingData.TaxlotIDList.ToString(), PostReconClosingData.TaxlotClosingIDWithClosingDate.ToString());

                        if (closedData != null)
                        {
                            PostReconClosingData.UpdateRepository(closedData);
                            if (!string.IsNullOrWhiteSpace((closedData.ErrorMsg.ToString())))
                            {
                                e.Result = closedData.ErrorMsg.ToString();
                                e.Cancel = true;
                            }
                            isChangesSaved = true;
                        }
                    }
                }
                //else
                //{
                //    GetClosedDataInformation();
                //    //PostReconClosingData.Netpositions.Clear();
                //}

                #endregion

                #region create ApprovedChanges Dictionary and save changes to database
                if (AmendmentsHelper.IsAmendmentsToSave())
                {
                    Dictionary<string, List<ApprovedChanges>> dictApprovedChanges = AmendmentsHelper.GetApprovedChangesDictionary();

                    if (dictApprovedChanges.Count > 0)
                    {
                        //As per discussion with Narender this is only required for CHMW so removing call from PRANA.
                        //AllocationManager.GetInstance().MakeNewCancelAmendChanges(dictApprovedChanges);
                        PostReconClosingData.UpdateTaxLotWithAmendments(dictApprovedChanges);
                    }
                }
                #endregion
                if (dtMarkPrice.Rows.Count > 0)
                {
                    rowsAffected = _pricingServicesProxy.InnerChannel.SaveMarkPrices(dtMarkPrice, true);
                    dtMarkPrice.Rows.Clear();
                }

                //save closed data which is available on post recon amendments UI
                if (PostReconClosingData.DictTaxlotsToClose.Count > 0 || _isUnwindingClosingTobeDone)
                {
                    //save closed data to database
                    //rowsAffected = _closingServices.InnerChannel.SaveClosedData(closingData);

                    PostTradeEnums.CloseTradeAlogrithm closingAlgo = (PostTradeEnums.CloseTradeAlogrithm)Enum.Parse(typeof(PostTradeEnums.CloseTradeAlogrithm), lblAccountingRuleValue.Text);

                    //PostTradeEnums.SecondarySortCriteria secondarySort = (PostTradeEnums.SecondarySortCriteria)Enum.Parse(typeof(PostTradeEnums.SecondarySortCriteria), lblSecondaySortValue.Text);

                    List<TaxLot> listPositionalTaxLots = GetBuyTaxlotsToClose();

                    List<TaxLot> listClosingTaxLots = GetSellTaxlotsToClose();

                    _closingServices.InnerChannel.UpdateClosingAlgoInfoCache(PostReconClosingData.DictClosingAlgoWithPositions);
                    bool isAutoCloseStrategy = chkBxIsAutoCloseStrategy.Checked;

                    ClosingParameters closingParams = new ClosingParameters();
                    closingParams.BuyTaxLotsAndPositions = listPositionalTaxLots;
                    closingParams.SellTaxLotsAndPositions = listClosingTaxLots;
                    closingParams.Algorithm = closingAlgo;
                    closingParams.IsShortWithBuyAndBuyToCover = false;
                    closingParams.IsSellWithBuyToClose = false;
                    closingParams.IsManual = false;
                    closingParams.IsDragDrop = false;
                    closingParams.IsFromServer = false;
                    closingParams.SecondarySort = PostTradeEnums.SecondarySortCriteria.None;
                    closingParams.IsVirtualClosingPopulate = false;
                    closingParams.IsOverrideWithUserClosing = true;
                    closingParams.IsMatchStrategy = !isAutoCloseStrategy;

                    ClosingData ClosedData = _closingServices.InnerChannel.AutomaticClosingOnManualOrPresetBasis(closingParams);
                    if (ClosedData.IsNavLockFailed)
                    {
                        e.Result = "NavLockFailed" + ClosedData.ErrorMsg.ToString();
                        e.Cancel = true;
                    }
                    else
                    {
                        PostReconClosingData.UpdateRepository(ClosedData);
                        if (!string.IsNullOrWhiteSpace((ClosedData.ErrorMsg.ToString())))
                        {
                            e.Result = ClosedData.ErrorMsg.ToString();
                            e.Cancel = true;
                        }
                    }
                }
                //check if there changes was saved then show information to user that changes was saved
                if (AmendmentsHelper.IsAmendmentsToSave() || rowsAffected > 0 || isChangesSaved)
                {
                    e.Result = "Data saved";
                    AmendmentsHelper.ClearAmendments();
                }
                else
                    e.Result = "No changes to be saved";
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
        /// 
        /// </summary>
        private List<Position> GetClosedDataInformation()
        {
            List<Position> lstPosition = new List<Position>();
            try
            {
                StringBuilder lstClosingIDs = new StringBuilder();
                foreach (Position position in PostReconClosingData.Netpositions)
                {
                    if ((_isUnwindingClosingTobeDone && position.ClosingTradeDate.Date >= _unwindingStartDate.Date) || (!_isUnwindingClosingTobeDone && !position.IsClosingSaved))
                    {
                        //add only those positions to unwind which are eligible to unwind
                        //earlier it fails in use case given on jira http://jira.nirvanasolutions.com:8080/browse/CHMW-2022

                        lstPosition.Add((Position)position.Clone());

                        lstClosingIDs.Append(position.TaxLotClosingId);
                        lstClosingIDs.Append(",");
                        if (!PostReconClosingData.DictTaxlotsToClose.ContainsKey(position.ID))
                        {
                            PostReconClosingData.DictTaxlotsToClose.Add(position.ID, position.ClosedQty);
                        }
                        else if (PostReconClosingData.DictTaxlotsToClose.ContainsKey(position.ID))
                        {
                            PostReconClosingData.DictTaxlotsToClose[position.ID] += position.ClosedQty;
                        }

                        if (!PostReconClosingData.DictTaxlotsToClose.ContainsKey(position.ClosingID))
                        {
                            PostReconClosingData.DictTaxlotsToClose.Add(position.ClosingID, position.ClosedQty);
                        }
                        else if (PostReconClosingData.DictTaxlotsToClose.ContainsKey(position.ClosingID))
                        {
                            PostReconClosingData.DictTaxlotsToClose[position.ClosingID] += position.ClosedQty;
                        }
                        //Update dictionary to store the closing algo information
                        PostReconClosingData.DictClosingAlgoWithPositions.Add(position.ID + "," + position.ClosingID, position.ClosingAlgo);
                    }
                }
                PostReconClosingData.UnwindUnSavedPositions(lstClosingIDs.ToString());

                List<string> lstClosigIds = lstClosingIDs.ToString().Split(',').ToList();

                foreach (string str in lstClosigIds)
                {
                    Position pos = PostReconClosingData.Netpositions.GetItem(str);
                    if (pos != null)
                    {
                        PostReconClosingData.Netpositions.Remove(pos);
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
            return lstPosition;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private List<TaxLot> GetBuyTaxlotsToClose()
        {
            List<TaxLot> butTaxlots = new List<TaxLot>();
            try
            {
                foreach (KeyValuePair<string, double> kvp in PostReconClosingData.DictTaxlotsToClose)
                {
                    TaxLot taxlot = PostReconClosingData.GetTaxlotByTaxLotID(kvp.Key);
                    if (taxlot != null && (taxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy)
                                                || taxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy_Closed)
                                || taxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy_Cover)
                                || taxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy_Open)
                                || taxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_BuyMinus)))
                    {
                        taxlot.TaxLotQtyToClose = kvp.Value;
                        //taxlot.TaxLotQty = taxlot.TaxLotQty + kvp.Value;
                        butTaxlots.Add(taxlot);
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
            return butTaxlots;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private List<TaxLot> GetSellTaxlotsToClose()
        {
            List<TaxLot> sellTaxlots = new List<TaxLot>();
            try
            {
                foreach (KeyValuePair<string, double> kvp in PostReconClosingData.DictTaxlotsToClose)
                {
                    TaxLot taxlot = PostReconClosingData.GetTaxlotByTaxLotID(kvp.Key);
                    if (taxlot != null && (taxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_Sell)
                                                || taxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_Sell_Open)
                                || taxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_SellPlus)
                                || taxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_SellShort)
                                || taxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_Sell_Closed)))
                    {
                        taxlot.TaxLotQtyToClose = kvp.Value;
                        //taxlot.TaxLotQty = taxlot.TaxLotQty + kvp.Value;
                        sellTaxlots.Add(taxlot);
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
            return sellTaxlots;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bgWorkerUnwindingClosing_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            try
            {
                string message = Convert.ToString(e.Result);

                if (e.Cancelled)
                {
                    if (message.StartsWith("NavLockFailed"))
                    {
                        message = message.Replace("NavLockFailed", "");
                        MessageBox.Show(this, message, "Nav Lock", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show(this, message, "Closing Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    
                }
                else
                {
                    MessageBox.Show(this, message, "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ReloadPNLFromDB();
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
            finally
            {
                //clear unsaved changes because data is saved in database
                PostReconClosingData.ClearUnSavedData();
                AmendmentsHelper.ClearAmendments();
                PostReconClosingData.DictTaxlotsToClose.Clear();
                DisableEnableForm(true);
                _isUnwindingClosingTobeDone = false;
                _unwindingStartDate = DateTimeConstants.MinValue;
                PostReconClosingData.IsUnsavedChanges = false;
            }
        }


        /// <summary>
        /// Disables or enables control based on Boolean value, false=>disable, true=>enable
        /// </summary>
        /// <param name="flag"></param>
        public void DisableEnableForm(bool flag)
        {
            try
            {
                //[Post Recon Amendments] Post Recon Amendments form is closing in between closing / unwinding process
                //http://jira.nirvanasolutions.com:8080/browse/CHMW-1946
                if (!this.FindForm().IsDisposed)
                {
                    this.FindForm().ControlBox = flag;
                }
                grdClosed.Enabled = flag;
                grdData.Enabled = flag;
                btnCancel.Enabled = flag;
                btnPostTransaction.Enabled = flag;
                btnPreview.Enabled = flag;
                btnSave.Enabled = flag;
                btnReverse.Enabled = flag;
                //dtStartDate.Enabled = Flag;
                dtEndDate.Enabled = flag;
                if (!flag)
                {
                    //grdClosed.BeginUpdate();
                    //grdData.BeginUpdate();

                    grdClosed.SuspendRowSynchronization();
                    grdData.SuspendRowSynchronization();
                }
                else
                {
                    grdClosed.ResumeRowSynchronization();
                    grdData.ResumeRowSynchronization();

                    //grdClosed.EndUpdate();
                    //grdData.EndUpdate();
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
        /// Disable enable button after publishing of data
        /// </summary>
        /// <param name="Flag"></param>
        public void DisableEnableGetDataButton(bool Flag)
        {
            try
            {
                btnGetData.Enabled = Flag;
                btnReverse.Enabled = Flag;
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
        /// Click event for Unwind tool strip menu item.
        /// Added by Ankit Gupta on 27 Oct, 2014.
        /// http://jira.nirvanasolutions.com:8080/browse/CHMW-1662
        /// Unwinding option in lower grid of post recon amendments.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void unwindToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                grdClosed.UpdateData();

                UltraGridRow[] filteredRows = grdClosed.Rows.GetFilteredInNonGroupByRows();
                List<Position> lstPosition = new List<Position>();
                //http://jira.nirvanasolutions.com:8080/browse/CHMW-1870
                List<PostTradeEnums.CloseTradeAlogrithm> lstClosingAlgos = new List<PostTradeEnums.CloseTradeAlogrithm>();
                foreach (UltraGridRow row in filteredRows)
                {
                    if (row.Cells["checkBox"].Value.ToString().ToLower() == "true")
                    {
                        Position pos = row.ListObject as Position;
                        if (pos != null)
                        {
                            lstPosition.Add((Position)pos.Clone());
                            int index = pos.ClosingAlgo;
                            if (Enum.IsDefined(typeof(PostTradeEnums.CloseTradeAlogrithm), index))
                            {
                                PostTradeEnums.CloseTradeAlogrithm closingAlgo = (PostTradeEnums.CloseTradeAlogrithm)index;
                                if (!lstClosingAlgos.Contains(closingAlgo))
                                {
                                    lstClosingAlgos.Add(closingAlgo);
                                }
                            }
                        }
                    }
                }

                if (lstClosingAlgos.Count > 1)
                {
                    StringBuilder algorithms = new StringBuilder();
                    foreach (PostTradeEnums.CloseTradeAlogrithm algo in lstClosingAlgos)
                    {
                        algorithms = algorithms.Append(algo.ToString() + ", ");
                    }

                    if (MessageBox.Show(this, "The data you are about to unwind has been closed using different algorithms: " + algorithms.ToString().Remove(algorithms.Length - 2)
                        + System.Environment.NewLine + "Would you like to export the closing information before unwinding ?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        UltraGridFileExporter.LoadFilePathAndExport(grdClosed, this);
                    }
                }
                else if (lstClosingAlgos.Count == 1)
                {
                    String algorithm = lstClosingAlgos[0].ToString();
                    if (!algorithm.Equals(lblAccountingRuleValue.Text, StringComparison.InvariantCultureIgnoreCase))
                        if (MessageBox.Show(this, "The data you are about to unwind has been closed using algorithm: " + algorithm
                            + ", which is different from preset accounting rule."
                            + System.Environment.NewLine + "Would you like to export the closing information before unwinding ?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                        {
                            UltraGridFileExporter.LoadFilePathAndExport(grdClosed, this);
                        }
                }

                object[] arguments = new object[1];
                arguments[0] = lstPosition;
                if (!_bgUnwindClosing.IsBusy)
                {
                    DisableEnableForm(false);
                    _bgUnwindClosing.RunWorkerAsync(arguments);
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


        void _bgUnwindClosing_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (!_bgUnwindClosing.CancellationPending)//checks for cancel request
                {
                    object[] arguments = e.Argument as object[];
                    List<Position> lstPosition = arguments[0] as List<Position>;
                    UnwindClosing(lstPosition);
                    UpdateMarkPriceForOpenTaxlots();
                }
                else
                {
                    e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                e.Cancel = true;
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Fe-factored method for unwinding
        /// </summary>
        /// <param name="filteredRows"></param>
        /// <param name="isUnwindingClosingTobeDone"></param>
        private void UnwindClosing(List<Position> lstPosition)
        {
            try
            {
                GenericBindingList<Position> posList = new GenericBindingList<Position>();
                //List<Position> listPositions = new List<Position>();
                Dictionary<string, DateTime> dictTaxlotIds = new Dictionary<string, DateTime>();

                Dictionary<int, string> lstUnLockedAccountIDs = new Dictionary<int, string>();
                foreach (Position pos in lstPosition)
                {
                    posList.Add(pos);
                    //if unwinding is done auto after amendment then unwind data after the amendment date
                    if (!dictTaxlotIds.ContainsKey(pos.ID))
                    {
                        if (_isUnwindingClosingTobeDone)
                        {
                            if (_unwindingStartDate.Date <= pos.ClosingTradeDate.Date)
                                dictTaxlotIds.Add(pos.ID, pos.ClosingTradeDate);
                        }
                        else
                            dictTaxlotIds.Add(pos.ID, pos.ClosingTradeDate);
                    }

                    if (!dictTaxlotIds.ContainsKey(pos.ClosingID))
                    {
                        if (_isUnwindingClosingTobeDone)
                        {
                            if (_unwindingStartDate.Date <= pos.ClosingTradeDate.Date)
                                dictTaxlotIds.Add(pos.ClosingID, pos.ClosingTradeDate);
                        }
                        else
                            dictTaxlotIds.Add(pos.ClosingID, pos.ClosingTradeDate);
                    }
                }
                if (dictTaxlotIds.Count == 0)
                {
                    return;
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
                            return;
                        }
                    }
                    else
                    {
                        return;
                    }
                }

                StringBuilder s = new StringBuilder();
                StringBuilder taxlotId = new StringBuilder();
                StringBuilder taxlotClosingIDWithClosingDate = new StringBuilder();
                List<string> taxlotidList = new List<string>();
                DialogResult userChoice = DialogResult.Yes;
                if (!_isUnwindingClosingTobeDone)
                    userChoice = MessageBox.Show("This will unwind the closing,Would you like to proceed ?", "Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                if (userChoice == DialogResult.Yes)
                {
                    StringBuilder message = new StringBuilder();

                    Dictionary<string, StatusInfo> positionStatusDict = _closingServices.InnerChannel.ArePositionEligibletoUnwind(dictTaxlotIds);

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
                                taxlotClosingIDWithClosingDate.Append(",");
                                //positionTowind.Add(position);
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
                                PostReconClosingData.UpdateDataToUnwind(position);

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
                                        message.Append(" has corporate action on future date,First undo Corporate action then unwind");
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
                        MessageBox.Show(message.ToString(), "Close trade Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
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

                            message.Append("in future date time. First unwind.");

                            MessageBox.Show(message.ToString(), "Close trade Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        }
                        else
                        {
                            ClosingData closedData = _closingServices.InnerChannel.VirtualUnWindClosing(s.ToString(), taxlotId.ToString(), taxlotClosingIDWithClosingDate.ToString());
                            if (closedData != null)
                            {
                                if (closedData.UpdatedTaxlots.Count > 0)
                                    PostReconClosingData.IsUnsavedChanges = true;
                                PostReconClosingData.UpdateUnsavedClosingData(closedData);
                                PostReconClosingData.UpdateRepository(closedData);

                                //PostReconClosingData.Update
                                AmendmentsHelper.UpdateUnRealizedRealizedPNLAfterClosingUnwinding(closedData);
                            }
                        }
                        //_statusMessage = "Data Unwinded Successfully";
                    }
                }
                else //update status message if user select no on dialog box
                {
                    //_statusMessage = "Unwinding Canceled";
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

        void _bgUnwindClosing_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled)//it doesn't matter if the BG worker ends normally, or gets cancelled,
                {              //both cases RunWorkerCompleted is invoked, so we need to check what has happened
                    MessageBox.Show(this, "Operation has been cancelled!", "Close Trade Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                DisableEnableForm(true);
            }
        }

        private void grdData_CellChange(object sender, CellEventArgs e)
        {

            try
            {
                if (e.Cell.GetType() == typeof(UltraGridFilterCell))
                {
                    return;
                }
                TaxLot taxLot = e.Cell.Row.ListObject as TaxLot;
                if (taxLot != null && (e.Cell.Column.Key.Equals(ClosingConstants.COL_AveragePrice) || e.Cell.Column.Key.Equals(ClosingConstants.COL_ExecutedQty)))
                {
                    if (!NAVLockManager.GetInstance.ValidateTrade(taxLot.Level1ID, taxLot.AUECLocalDate))
                    {
                        MessageBox.Show(this, "You need to Unlock NavLock before making any amendments.\nPlease contact System Admin for the same", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        e.Cell.CancelUpdate();
                        return;
                    }
                }
                if (e.Cell.Text != string.Empty)
                {
                    double updatedValue = 0.0;
                    bool temp = double.TryParse(e.Cell.Text, out updatedValue);
                    if (!temp)
                    {
                        //modified by amit on 18.03.2015
                        //http://jira.nirvanasolutions.com:8080/browse/CHMW-3001
                        if (e.Cell.Column.Key.Equals(ClosingConstants.COL_ExecutedQty) || e.Cell.Column.Key.Equals(ClosingConstants.COL_AveragePrice) || e.Cell.Column.Key.Equals(ClosingConstants.COL_MarkPrice))
                        {
                            grdData.CellChange -= new CellEventHandler(grdData_CellChange);
                            e.Cell.Value = e.Cell.OriginalValue;
                            grdData.CellChange += new CellEventHandler(grdData_CellChange);
                        }
                    }
                }
                else
                {
                    e.Cell.Value = 0.0;
                }
                // http://jira.nirvanasolutions.com:8080/browse/CHMW-2453
                if (e.Cell.Column.Key.Equals(ClosingConstants.COL_ClosingAlgoChooser))
                {
                    if (_lastModifiedCell != null && _lastModifiedCell != e.Cell)
                    {
                        _lastModifiedCell.Value = null;
                    }
                    if (e.Cell.Text.Equals(lblAccountingRuleValue.Text))
                    {
                        //e.Cell.Column.NullText = lblAccountingRuleValue.Text;
                    }
                    else
                    {
                        e.Cell.Column.NullText = PostTradeEnums.CloseTradeAlogrithm.NONE.ToString();
                    }
                    _lastModifiedCell = e.Cell;
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

        #region Todo remove batch code
        /// <summary>
        /// Run Batch If User wishes to for a particular account Symbol in Background Worker
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void btnRunBatch_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (MessageBox.Show("This may take some time.Do you want to continue ?", "Run Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        //        {
        //            BackgroundWorker bgFetchPNL = new BackgroundWorker();
        //            bgFetchPNL.DoWork += new DoWorkEventHandler(bgFetchPNL_DoWork);
        //            bgFetchPNL.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgFetchPNL_RunWorkerCompleted);
        //            bgFetchPNL.RunWorkerAsync();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        private void bgFetchPNL_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                object[] arguments = e.Argument as object[];

                double accountRealizedPNL = 0;
                double accountUnRealizedPNL = 0;
                double SymbolRealizedPNL = 0;
                double SymbolUnRealizedPNL = 0;

                string accountName = arguments[0] as string;
                string symbol = arguments[1] as string;
                DateTime NAVlockAppliedDate = (DateTime)arguments[2];
                DateTime endDate = (DateTime)arguments[3];

                _closingServices.InnerChannel.GetPNLForSymbol(accountName, symbol, NAVlockAppliedDate, endDate, out accountRealizedPNL, out accountUnRealizedPNL, out SymbolRealizedPNL, out SymbolUnRealizedPNL);
                AmendmentsHelper.SetRealizedUnRealizedPNL(accountRealizedPNL, accountUnRealizedPNL, SymbolRealizedPNL, SymbolUnRealizedPNL);

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

        private void bgFetchPNL_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                double accountPNL = 0;
                double symbolPNL = 0;
                AmendmentsHelper.GetTotalPNL(out accountPNL, out symbolPNL);



                SetLabelText(lblSymbolPNLPreAmendmentValue, Math.Round(symbolPNL, 4).ToString(), true, "lblSymbolPNLPreAmendmentValue");

                SetLabelText(lblAccountPNLPreAmendmentValue, Math.Round(accountPNL, 4).ToString(), true, "lblAccountPNLPreAmendmentValue");

                //lblSymbolPNLPreAmendmentValue.Appearance.ForeColor = Color.Green;
                //lblSymbolPNLPostAmendmentValue.Appearance.ForeColor = Color.Green;
                //lblAccountPNLPreAmendmentValue.Appearance.ForeColor = Color.Green;
                //lblAccountPNLPostAmendmentValue.Appearance.ForeColor = Color.Green;                
                UpdatelblPNLPostAmendmentValue();
                SetColorAndFormattingForPNLFields();
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
        /// opens file save dialog to save the grid table in pdf csv or xls format
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //string filePath = @"D:\Nirvana\NirvanaCode\SourceCode\Dev\Prana\Application\Prana.Client\Prana\bin\Debug\ReconData";
                UltraGridFileExporter.LoadFilePathAndExport(grdClosed, this);
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



        async void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (!backgroundWorker.CancellationPending)//checks for cancel request
                {
                    ClosingTemplate previewTemplate = e.Argument as ClosingTemplate;

                    ClosingData closingData = null;

                    closingData = await System.Threading.Tasks.Task.Run(() => GetFilteredData(previewTemplate));

                    e.Result = closingData;
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                e.Cancel = true;
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (!e.Cancelled)//it doesn't matter if the BG worker ends normally, or gets canceled,
                {

                    ClosingData closingData = e.Result as ClosingData;
                    if (closingData != null)
                    {
                        PostReconClosingData.ClearRepository();
                        PostReconClosingData.CreateRepository(closingData);

                        SetGridDataSources();
                        if (!PostReconClosingData.IsDataAvailabletoClose())
                        {
                            MessageBox.Show(this, "No data exists for the filters selected.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                else
                {
                    MessageBox.Show(this, "Operation has been canceled.", "Close Trade Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            finally
            {
                DisableEnableForm(true);
                _isUnwindingClosingTobeDone = false;
                _unwindingStartDate = DateTimeConstants.MinValue;
            }
        }

        internal async Task<ClosingData> GetFilteredData(ClosingTemplate template)
        {
            ClosingData data = new ClosingData();
            try
            {
                if (template == null)
                {
                    data = await System.Threading.Tasks.Task.Run(() => PostReconClosingData.GetAllClosingData(((DateTime)dtStartDate.Value).Date, ((DateTime)dtEndDate.Value).Date, false, _closingServices, _accountID.ToString(), string.Empty, lblSymbolValue.Text, string.Empty));
                }
                else
                {
                    Dictionary<int, List<int>> dictMasterFundSubAccountAssociation = CachedDataManager.GetInstance.GetMasterFundSubAccountAssociation();

                    DateTime toDate = template.ToDate;
                    if (template.UseCurrentDate)
                    {
                        toDate = DateTime.UtcNow;
                    }

                    data = await System.Threading.Tasks.Task.Run(() => PostReconClosingData.GetAllClosingData(template.FromDate, toDate, false, _closingServices, template.GetCommaSeparatedAccounts(dictMasterFundSubAccountAssociation), template.GetCommaSeparatedAssets(), template.GetCommaSeparatedSymbols(), SqlParser.GetDynamicConditionQuerry(template.DictCustomConditions)));
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
            return data;
        }

        /// <summary>
        /// Filter cell value changed event for grdData.
        /// /// Added by Ankit Gupta on 11 Nov, 2014.
        /// http://jira.nirvanasolutions.com:8080/browse/CHMW-1808
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdData_FilterCellValueChanged(object sender, FilterCellValueChangedEventArgs e)
        {
            try
            {
                String filterText = e.FilterCell.Text;
                if (String.IsNullOrEmpty(filterText))
                {
                    grdData.Rows.ColumnFilters[e.FilterCell.Column.Key].FilterConditions.Clear();
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
        /// Set color of PNL labels, according to their value.
        /// Text color will be RED, if the value is less than or equal to zero.
        /// Text color will be GREEN, if the value is greater than zero.
        /// Added by Ankit Gupta on 11 Nov, 2014.
        /// http://jira.nirvanasolutions.com:8080/browse/CHMW-1811
        /// </summary>
        private void SetColorAndFormattingForPNLFields()
        {
            try
            {
                #region set color
                if (Double.Parse(lblSymbolPNLPreAmendmentValue.Text) > 0)
                {
                    lblSymbolPNLPreAmendmentValue.Appearance.ForeColor = Color.Green;
                }
                else
                {
                    lblSymbolPNLPreAmendmentValue.Appearance.ForeColor = Color.Red;
                }
                if (Double.Parse(lblSymbolPNLPostAmendmentValue.Text) > 0)
                {
                    lblSymbolPNLPostAmendmentValue.Appearance.ForeColor = Color.Green;
                }
                else
                {
                    lblSymbolPNLPostAmendmentValue.Appearance.ForeColor = Color.Red;
                }
                if (Double.Parse(lblAccountPNLPreAmendmentValue.Text) > 0)
                {
                    lblAccountPNLPreAmendmentValue.Appearance.ForeColor = Color.Green;
                }
                else
                {
                    lblAccountPNLPreAmendmentValue.Appearance.ForeColor = Color.Red;
                }
                if (Double.Parse(lblAccountPNLPostAmendmentValue.Text) > 0)
                {
                    lblAccountPNLPostAmendmentValue.Appearance.ForeColor = Color.Green;
                }
                else
                {
                    lblAccountPNLPostAmendmentValue.Appearance.ForeColor = Color.Red;
                }
                #endregion

                #region set formatting & round off
                lblSymbolPNLPreAmendmentValue.Text = string.Format("{0:#,##0.00}", double.Parse(lblSymbolPNLPreAmendmentValue.Text));
                lblSymbolPNLPostAmendmentValue.Text = string.Format("{0:#,##0.00}", double.Parse(lblSymbolPNLPostAmendmentValue.Text));
                lblAccountPNLPreAmendmentValue.Text = string.Format("{0:#,##0.00}", double.Parse(lblAccountPNLPreAmendmentValue.Text));
                lblAccountPNLPostAmendmentValue.Text = string.Format("{0:#,##0.00}", double.Parse(lblAccountPNLPostAmendmentValue.Text));
                #endregion
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

        internal void SetProxy(DuplexProxyBase<ISubscription> proxy)
        {
            try
            {
                _proxy = proxy;
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

        private void tbComments_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                _isCommentsUpdated = true;
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

        private void btnRestoreDefaultAlgo_Click(object sender, EventArgs e)
        {
            try
            {
                if (_lastModifiedCell != null && grdData.DisplayLayout.Bands[0].Columns.Exists(ClosingConstants.COL_ClosingAlgoChooser))
                {
                    _lastModifiedCell.Value = null;
                    grdData.DisplayLayout.Bands[0].Columns[ClosingConstants.COL_ClosingAlgoChooser].NullText = lblAccountingRuleValue.Text;
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

        internal void CreatePricingServiceProxy()
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

        public void SnapshotResponse(SymbolData data, [Optional, DefaultParameterValue(null)] SnapshotResponseData snapshotResponseData)
        {
            //throw new NotImplementedException();
        }

        public void OptionChainResponse(string symbol, List<OptionStaticData> data)
        {
        }

        public void LiveFeedConnected()
        {
            //throw new NotImplementedException();
        }

        public void LiveFeedDisConnected()
        {
            //throw new NotImplementedException();
        }
    }
}
