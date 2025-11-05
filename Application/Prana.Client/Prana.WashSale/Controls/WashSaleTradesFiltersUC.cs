using Infragistics.Win.UltraWinGrid;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using Prana.WashSale.Classes;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Prana.WashSale.Controls
{
    public partial class WashSaleTradesFiltersUC : UserControl
    {
        /// <summary>
        /// BindWashSaleDataToGrid
        /// </summary>
        public event EventHandler<EventArgs<List<WashSaleTrades>>> BindDataToWashSaleGrid;
        /// <summary>
        ///Event to Update the Binding list of the Wash Sale Grid
        /// </summary>
        public event EventHandler UpdateWashSaleGridData;
        /// <summary>
        /// Constructor
        /// </summary>
        public WashSaleTradesFiltersUC()
        {
            InitializeComponent();
        }
        /// <summary>
        /// WashSaleTradesFiltersUC_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WashSaleTradesFiltersUC_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.DesignMode)
                {
                    FillAccountDropDown();
                    FillAssetClassDropDown();
                    FillCurrencyDropDown();
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
        /// Determine data on the grid
        /// </summary>
        public static bool IsDataLoadedOnGrid = false;

        /// <summary>
        /// Determine if Get data ,Upload or Save buttonClick
        /// </summary>
        public static bool IsGetDataOrUploadOrSaveClick = false;
        /// <summary>
        /// Disable/Enable the Grid data
        /// </summary>
        public Action<object, ClickCellEventArgs> DisableGridData { get; internal set; }
        /// <summary>
        /// Button Get Data Clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetData_Click(object sender, EventArgs e)
        {
            try
            {
                IsGetDataOrUploadOrSaveClick = true;
                WashSale.SetStatusBarText(WashSaleConstants.CONST_WASHSALE_LOADING_DATA);
                ChangeStatus(false);
                if (IsDataLoadedOnGrid && WashSaleTradesGridUC.isSaveGridData)
                {
                    CustomMessageBox customMessage = new CustomMessageBox(WashSaleConstants.CONST_WASHSALE_POPUP_MESSAGE_TITLE, WashSaleConstants.CONST_WASHSALE_POPUP_MESSAGE, true, CustomThemeHelper.PRODUCT_COMPANY_NAME, FormStartPosition.CenterScreen, MessageBoxButtons.YesNo);
                    DialogResult dialog = customMessage.ShowDialog();
                    if (dialog == DialogResult.Cancel)
                    {
                        WashSale.SetStatusBarText(WashSaleConstants.CONST_BLANK);
                        customMessage.Close();
                        IsGetDataOrUploadOrSaveClick = false;
                        ChangeStatus(true);
                        return;
                    }
                    else if (dialog == DialogResult.Yes)
                    {
                        if (WashSaleTradesGridUC.IsGridContainsError())
                        {
                            WashSale.SetStatusBarText(WashSaleConstants.CONST_WASHSALE_GRID_ERROR_MESSAGE);
                            MessageBox.Show(WashSaleConstants.CONST_WASHSALE_GRID_ERROR_MESSAGEBOX, WashSaleConstants.CONST_WASHSALE_MESSAGEBOX_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            customMessage.Close();
                            return;
                        }
                        WashSale.SetStatusBarText(WashSaleConstants.CONST_WASHSALE_SAVING_DATA);
                        if (UpdateWashSaleGridData != null)
                            UpdateWashSaleGridData(e, null);
                        WashSaleDataManager.SaveWashSaleTaxlotData();
                        WashSale.SetStatusBarText(WashSaleConstants.CONST_WASHSALE_DATA_SAVED);
                        WashSaleTradesGridUC.isSaveGridData = false;
                        WashSaleTradesGridUC._gridHasError.Clear();
                        customMessage.Close();
                    }
                    else
                    {
                        WashSale.SetStatusBarText(WashSaleConstants.CONST_BLANK);
                        WashSaleTradesGridUC.DiscardChanges();
                        WashSaleTradesGridUC._gridHasError.Clear();
                        customMessage.Close();
                    }
                }
                string accountIds = multiSelectDropDownAccount.GetCommaSeperatedAccountIds().TrimEnd(',');
                string assetIds = multiSelectDropDownAssetClass.GetCommaSeperatedIDs(WashSaleConstants.CONST_ASSET).TrimEnd(',');
                string currencyIds = multiSelectDropDownCurrency.GetCommaSeperatedIDs(WashSaleConstants.CONST_CURRENCY).TrimEnd(',');
                DisableGridData(null, null);
                WashSaleTradesGridUC._gridHasError.Clear();
                List<WashSaleTrades> wsList = WashSaleDataManager.GetWashSaleData(accountIds, assetIds, currencyIds);

                if (BindDataToWashSaleGrid != null)
                    BindDataToWashSaleGrid(this, new EventArgs<List<WashSaleTrades>>(wsList));
                IsDataLoadedOnGrid = true;
                WashSale.SetStatusBarText(WashSaleConstants.CONST_WASHSALE_DATA_LOADED);
                ChangeStatus(true);
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
        /// Fills account multi select drop down with account values.
        /// </summary>
        private void FillAccountDropDown()
        {
            try
            {
                Dictionary<int, string> _dictAccounts = new Dictionary<int, string>();
                _dictAccounts = CachedDataManager.GetInstance.GetUserAccountsAsDict();
                multiSelectDropDownAccount.SetManualTheme(false);

                //add accounts to the check list default value will be checked
                multiSelectDropDownAccount.AddItemsToTheCheckList(_dictAccounts, CheckState.Checked);

                //adjust checklistbox width according to the longest accountname
                multiSelectDropDownAccount.AdjustCheckListBoxWidth();
                multiSelectDropDownAccount.TitleText = "Account";
                multiSelectDropDownAccount.SetTextEditorText("All Account(s) Selected");
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Fills asset class multiselect drop down with asset class values.
        /// </summary>
        private void FillAssetClassDropDown()
        {
            try
            {
                Dictionary<int, string> _finaldictAssetClass = new Dictionary<int, string>();
                Dictionary<int, string> _dictAssetClass = CachedDataManager.GetInstance.GetAllAssets();

                // Have to show only Equity,Equity Option,Private Equity,Fixed Income,Equity Swap asset classes on UI
                foreach (var asset in _dictAssetClass)
                {
                    if (asset.Value.Contains(WashSaleConstants.CONST_EQUITY) || asset.Value.Contains(WashSaleConstants.CONST_EQUITYOPTION) || asset.Value.Contains(WashSaleConstants.CONST_PRIVATEEQUITY) || asset.Value.Contains(WashSaleConstants.CONST_FIXEDINCOME) || asset.Value.Contains(WashSaleConstants.CONST_EQUITYSWAP))
                        _finaldictAssetClass.Add(asset.Key, asset.Value);
                }
                if (!_finaldictAssetClass.ContainsValue(WashSaleConstants.CONST_EQUITYSWAP))
                    _finaldictAssetClass.Add(_dictAssetClass.Count + 1, WashSaleConstants.CONST_EQUITYSWAP);
                _dictAssetClass = null;
                multiSelectDropDownAssetClass.SetManualTheme(false);

                //add asset to the check list default value will be checked
                multiSelectDropDownAssetClass.AddItemsToTheCheckList(_finaldictAssetClass, CheckState.Checked);

                //adjust checklistbox width according to the longest assetname
                multiSelectDropDownAssetClass.AdjustCheckListBoxWidth();
                multiSelectDropDownAssetClass.TitleText = WashSaleConstants.CONST_ASSET;
                multiSelectDropDownAssetClass.SetTextEditorText(WashSaleConstants.CONST_ALL_ASSET_SELECTED);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Fills currency multi select drop down with currency values.
        /// </summary>
        private void FillCurrencyDropDown()
        {
            try
            {
                Dictionary<int, string> _dictCurrency = new Dictionary<int, string>();
                _dictCurrency = CachedDataManager.GetInstance.GetAllCurrencies();
                multiSelectDropDownCurrency.SetManualTheme(false);

                //add currency to the check list default value will be checked
                multiSelectDropDownCurrency.AddItemsToTheCheckList(_dictCurrency, CheckState.Checked);

                //adjust checklistbox width according to the longest currency
                multiSelectDropDownCurrency.AdjustCheckListBoxWidth();
                multiSelectDropDownCurrency.TitleText = WashSaleConstants.CONST_CURRENCY;
                multiSelectDropDownCurrency.SetTextEditorText(WashSaleConstants.CONST_ALL_CURRENCY_SELECTED);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }
        /// <summary>
        /// Handle when there is some unsave data and user tries to change top filter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectDropDownAccount(object sender, EventArgs e)
        {
            try
            {
                if (IsDataLoadedOnGrid && WashSaleTradesGridUC.isSaveGridData)
                {
                    CustomMessageBox customMessage = new CustomMessageBox(WashSaleConstants.CONST_WASHSALE_POPUP_MESSAGE_TITLE, WashSaleConstants.CONST_WASHSALE_POPUP_MESSAGE, true, CustomThemeHelper.PRODUCT_COMPANY_NAME, FormStartPosition.CenterScreen, MessageBoxButtons.YesNo);
                    DialogResult dialog = customMessage.ShowDialog();
                    if (dialog == DialogResult.Yes)
                    {
                        if (WashSaleTradesGridUC.IsGridContainsError())
                        {
                            WashSale.SetStatusBarText(WashSaleConstants.CONST_WASHSALE_GRID_ERROR_MESSAGE);
                            MessageBox.Show(WashSaleConstants.CONST_WASHSALE_GRID_ERROR_MESSAGEBOX, WashSaleConstants.CONST_WASHSALE_MESSAGEBOX_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            customMessage.Close();
                            return;
                        }
                        WashSale.SetStatusBarText(WashSaleConstants.CONST_WASHSALE_SAVING_DATA);
                        if (UpdateWashSaleGridData != null)
                            UpdateWashSaleGridData(e, null);
                        WashSaleDataManager.SaveWashSaleTaxlotData();
                        WashSale.SetStatusBarText(WashSaleConstants.CONST_WASHSALE_DATA_SAVED);
                        WashSaleTradesGridUC.isSaveGridData = false;
                        WashSaleTradesGridUC._gridHasError.Clear();
                        customMessage.Close();
                    }
                    else
                    {
                        WashSale.SetStatusBarText(WashSaleConstants.CONST_BLANK);
                        WashSaleTradesGridUC.DiscardChanges();
                        WashSaleTradesGridUC.isSaveGridData = false;
                        WashSaleTradesGridUC._gridHasError.Clear();
                        customMessage.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }
        /// <summary>
        /// Call method to change state of Get Data button
        /// </summary>
        private void ChangeStatus(bool isWorkCompleted)
        {
            try
            {
                if (isWorkCompleted)
                {
                    btnGetData.Text = "Get Data";
                    btnGetData.Enabled = true;
                }
                else
                {
                    btnGetData.Text = "Getting...";
                    btnGetData.Enabled = false;
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

    }
}
