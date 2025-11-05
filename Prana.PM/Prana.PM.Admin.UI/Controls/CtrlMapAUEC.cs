using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.Global;
using Prana.LogManager;
using Prana.PM.BLL;
//using Prana.PM.Common;
using Prana.PM.DAL;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Prana.PM.Admin.UI.Controls
{
    public partial class CtrlMapAUEC : UserControl
    {
        #region Grid Column Names

        const string COL_SourceItemID = "SourceItemID";
        const string COL_SourceItemName = "SourceItemName";
        const string COL_SourceItemFullName = "SourceItemFullName";
        const string COL_ApplicationItemId = "ApplicationItemId";
        const string COL_ApplicationItemName = "ApplicationItemName";
        const string COL_ApplicationItemFullName = "ApplicationItemFullName";
        const string COL_Lock = "Lock";

        const string TABKEY_Asset = "Asset";
        const string TABKEY_Underlying = "Underlying";
        const string TABKEY_Exchange = "Exchange";
        const string TABKEY_Currency = "Currency";

        #endregion Grid Column Names

        BindingSource _assetBindingSource = new BindingSource();
        BindingSource _underlyingBindingSource = new BindingSource();
        BindingSource _exchangeBindingSource = new BindingSource();
        BindingSource _currencyBindingSource = new BindingSource();

        MapColumns _assetMapping = new MapColumns();
        MapColumns _underlyingMapping = new MapColumns();
        MapColumns _exchangeMapping = new MapColumns();
        MapColumns _currencyMapping = new MapColumns();

        MappingItemList _assetMappingList = new MappingItemList();
        MappingItemList _underlyingMappingList = new MappingItemList();
        MappingItemList _exchangeMappingList = new MappingItemList();
        MappingItemList _currencyMappingList = new MappingItemList();

        private Dictionary<string, string> _currencyLookupTable = null;

        private Dictionary<string, string> _exchangeLookupTable = null;

        public CtrlMapAUEC()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            // Close the Container Form
            FindForm().Close();
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
        public void InitControl(Prana.BusinessObjects.PositionManagement.ThirdPartyNameID dataSourceNameID)
        {
            try
            {
                SetupBinding(dataSourceNameID);
                _isInitialized = true;

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
        /// Setups the binding.
        /// </summary>
        /// <param name="dataSourceNameID">The data source name ID.</param>
        private void SetupBinding(Prana.BusinessObjects.PositionManagement.ThirdPartyNameID dataSourceNameID)
        {
            //grdAUECMapping.DataBindings.Clear();

            try
            {
                //_assetMapping.BeginEdit();
                _assetMapping.DataSourceNameID = dataSourceNameID;
                _underlyingMapping.DataSourceNameID = dataSourceNameID;
                _exchangeMapping.DataSourceNameID = dataSourceNameID;
                _currencyMapping.DataSourceNameID = dataSourceNameID;

                AssignCurrentTabMappingList(dataSourceNameID);

                //_assetMapping.BeginEdit();
                //_underlyingMapping.BeginEdit();
                //_exchangeMapping.BeginEdit();
                //_currencyMapping.BeginEdit();

                //_assetMappingList = AUECManager.GetAssetMappings(dataSourceNameID);
                //_underlyingMappingList = AUECManager.GetUnderlyingMappings(dataSourceNameID);
                //_exchangeMappingList = AUECManager.GetExchangeMappings(dataSourceNameID);
                //_currencyMappingList = AUECManager.GetCurrencyMappings(dataSourceNameID);

                //_assetMapping.MappingItems = _assetMappingList;
                //_underlyingMapping.MappingItems = _underlyingMappingList;
                //_exchangeMapping.MappingItems = _exchangeMappingList;
                //_currencyMapping.MappingItems = _currencyMappingList;

                _assetBindingSource.DataSource = _assetMapping;
                _underlyingBindingSource.DataSource = _underlyingMapping;
                _exchangeBindingSource.DataSource = _exchangeMapping;
                _currencyBindingSource.DataSource = _currencyMapping;

                //RetrieveMapAUEC(dataSourceNameID); // newInfo;
                lblDataSourceNameAsset.DataBindings.Add("Text", _assetBindingSource, "DataSourceNameID");

                if (!_isInitialized)
                {
                    BindAllGridComboBoxes();
                }

                grdAssetMapping.DataMember = "MappingItems";
                grdAssetMapping.DataSource = _assetBindingSource;

                grdUnderlyingMapping.DataMember = "MappingItems";
                grdUnderlyingMapping.DataSource = _underlyingBindingSource;

                grdExchangeMapping.DataMember = "MappingItems";
                grdExchangeMapping.DataSource = _exchangeBindingSource;

                grdCurrencyMapping.DataMember = "MappingItems";
                grdCurrencyMapping.DataSource = _currencyBindingSource;


                //fetch Currency lookup
                _currencyLookupTable = AUECManager.GetAllCurrenciesForLookup();

                _exchangeLookupTable = AUECManager.GetAllExchangesForLookup();
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

        private void BindAllGridComboBoxes()
        {
            try
            {

                List<EnumerationValue> assetList = AUECManager.GetAllAssets();
                //assetList.Add(new EnumerationValue(Constants.C_COMBO_SELECT, -1));
                assetList.Insert(0, new EnumerationValue(Global.ApplicationConstants.C_COMBO_SELECT, -1));
                cmbApplicationColumnAsset.DataBindings.Clear();
                cmbApplicationColumnAsset.DisplayMember = "DisplayText";
                cmbApplicationColumnAsset.ValueMember = "Value";
                cmbApplicationColumnAsset.DataSource = null;
                cmbApplicationColumnAsset.DataSource = assetList;
                Utils.UltraDropDownFilter(cmbApplicationColumnAsset, "DisplayText");

                List<EnumerationValue> underlyingList = AUECManager.GetAllUnderlyings();
                //underlyingList.Add(new EnumerationValue(Constants.C_COMBO_SELECT, -1));
                underlyingList.Insert(0, new EnumerationValue(ApplicationConstants.C_COMBO_SELECT, -1));
                cmbApplicationColumnUnderlying.DataBindings.Clear();
                cmbApplicationColumnUnderlying.DisplayMember = "DisplayText";
                cmbApplicationColumnUnderlying.ValueMember = "Value";
                cmbApplicationColumnUnderlying.DataSource = null;
                cmbApplicationColumnUnderlying.DataSource = underlyingList;
                Utils.UltraDropDownFilter(cmbApplicationColumnUnderlying, "DisplayText");

                List<EnumerationValue> exchangeList = AUECManager.GetAllExchanges();
                //exchangeList.Add(new EnumerationValue(Constants.C_COMBO_SELECT, -1));
                exchangeList.Insert(0, new EnumerationValue(ApplicationConstants.C_COMBO_SELECT, -1));
                cmbApplicationColumnExchange.DataBindings.Clear();
                cmbApplicationColumnExchange.DisplayMember = "DisplayText";
                cmbApplicationColumnExchange.ValueMember = "Value";
                cmbApplicationColumnExchange.DataSource = null;
                cmbApplicationColumnExchange.DataSource = exchangeList;
                Utils.UltraDropDownFilter(cmbApplicationColumnExchange, "DisplayText");

                List<EnumerationValue> currencyList = AUECManager.GetAllCurrencies();
                //currencyList.Add(new EnumerationValue(Constants.C_COMBO_SELECT, -1));
                currencyList.Insert(0, new EnumerationValue(ApplicationConstants.C_COMBO_SELECT, -1));
                cmbApplicationColumnCurrency.DataBindings.Clear();
                cmbApplicationColumnCurrency.DisplayMember = "DisplayText";
                cmbApplicationColumnCurrency.ValueMember = "Value";
                cmbApplicationColumnCurrency.DataSource = null;
                cmbApplicationColumnCurrency.DataSource = currencyList;
                Utils.UltraDropDownFilter(cmbApplicationColumnCurrency, "DisplayText");
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

        private void AssignCurrentTabMappingList(Prana.BusinessObjects.PositionManagement.ThirdPartyNameID dataSourceNameID)
        {
            try
            {
                string selectedTabKey = tabMapAUEC.SelectedTab != null ? tabMapAUEC.SelectedTab.Key : TABKEY_Asset;

                MappingItemList tempAssetMappingList = null;
                MappingItemList tempUnderlyingMappingList = null;
                MappingItemList tempExchangeMappingList = null;
                MappingItemList tempCurrencyMappingList = null;

                switch (selectedTabKey)
                {
                    case TABKEY_Asset:
                        ///Ask the database only for the first time.
                        if (_assetMappingList != null && _assetMappingList.Count == 0)
                        {
                            //_assetMapping.BeginEdit();
                            tempAssetMappingList = AUECManager.GetAssetMappings(dataSourceNameID);
                            if (tempAssetMappingList != null)
                            {
                                _assetMappingList = tempAssetMappingList;
                            }
                            _assetMapping.MappingItems = _assetMappingList;
                            _assetMapping.BeginEdit();
                        }

                        //tabMapAUEC.Tabs[TABKEY_Asset].Selected = true;
                        break;

                    case TABKEY_Underlying:

                        //Ask the database only for the first time.
                        if (_underlyingMappingList != null && _underlyingMappingList.Count == 0)
                        {
                            tempUnderlyingMappingList = AUECManager.GetUnderlyingMappings(dataSourceNameID);
                            if (tempUnderlyingMappingList != null)
                            {
                                //_underlyingMapping.BeginEdit();
                                _underlyingMappingList = tempUnderlyingMappingList;
                            }
                            _underlyingMapping.MappingItems = _underlyingMappingList;
                            _underlyingMapping.BeginEdit();
                        }

                        //tabMapAUEC.Tabs[TABKEY_Underlying].Selected = true;
                        break;

                    case TABKEY_Exchange:

                        ///Ask the database only for the first time.
                        if (_exchangeMappingList != null && _exchangeMappingList.Count == 0)
                        {
                            tempExchangeMappingList = AUECManager.GetExchangeMappings(dataSourceNameID);
                            if (tempExchangeMappingList != null)
                            {
                                //_exchangeMapping.BeginEdit();
                                _exchangeMappingList = tempExchangeMappingList;
                            }
                            _exchangeMapping.MappingItems = _exchangeMappingList;
                            _exchangeMapping.BeginEdit();
                        }

                        //tabMapAUEC.Tabs[TABKEY_Exchange].Selected = true;
                        break;

                    case TABKEY_Currency:

                        ///Ask the database only for the first time.
                        if (_currencyMappingList != null && _currencyMappingList.Count == 0)
                        {
                            tempCurrencyMappingList = AUECManager.GetCurrencyMappings(dataSourceNameID);
                            if (tempCurrencyMappingList != null)
                            {
                                //_currencyMapping.BeginEdit();
                                _currencyMappingList = tempCurrencyMappingList;
                            }
                            _currencyMapping.MappingItems = _currencyMappingList;
                            _currencyMapping.BeginEdit();
                        }

                        break;
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

        private void tabMapAUEC_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            try
            {
                AssignCurrentTabMappingList(_assetMapping.DataSourceNameID);
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

        private void btnAddMapping_Click(object sender, EventArgs e)
        {
            try
            {
                string selectedTabKey = tabMapAUEC.SelectedTab != null ? tabMapAUEC.SelectedTab.Key : TABKEY_Asset;
                MappingItem newMapping = new MappingItem();
                newMapping.ApplicationItemId = -1;
                newMapping.ApplicationItemName = ApplicationConstants.C_COMBO_SELECT;
                newMapping.SourceItemName = string.Empty;
                switch (selectedTabKey)
                {
                    case TABKEY_Asset:
                        _assetMappingList.Add(newMapping);
                        _assetMapping.MappingItems = _assetMappingList;
                        //_assetBindingSource.ResetBindings(false);
                        break;

                    case TABKEY_Underlying:
                        _underlyingMappingList.Add(newMapping);
                        _underlyingMapping.MappingItems = _underlyingMappingList;
                        //_underlyingBindingSource.ResetBindings(false);
                        break;

                    case TABKEY_Exchange:
                        _exchangeMappingList.Add(newMapping);
                        _exchangeMapping.MappingItems = _exchangeMappingList;
                        //_exchangeBindingSource.ResetBindings(false);
                        break;

                    case TABKEY_Currency:
                        _currencyMappingList.Add(newMapping);
                        _currencyMapping.MappingItems = _currencyMappingList;
                        //_currencyBindingSource.ResetBindings(false);
                        break;
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

        #region Grid Initializations
        private void grdAssetMapping_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                UltraGridBand band = grdAssetMapping.DisplayLayout.Bands[0];
                grdAssetMapping.DisplayLayout.Override.WrapHeaderText = Infragistics.Win.DefaultableBoolean.True;
                grdAssetMapping.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
                grdAssetMapping.DisplayLayout.ScrollBounds = ScrollBounds.ScrollToLastItem;
                grdAssetMapping.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;
                grdAssetMapping.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
                grdAssetMapping.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
                grdAssetMapping.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
                grdAssetMapping.DisplayLayout.Override.AllowColSwapping = AllowColSwapping.NotAllowed;
                grdAssetMapping.DisplayLayout.Override.HeaderAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
                band.Override.AllowColSwapping = AllowColSwapping.NotAllowed;

                UltraGridColumn colSourceItemFullName = band.Columns[COL_SourceItemFullName];
                colSourceItemFullName.Hidden = true;

                UltraGridColumn colSourceItemID = band.Columns[COL_SourceItemID];
                colSourceItemID.Hidden = true;

                UltraGridColumn colApplicationItemFullName = band.Columns[COL_ApplicationItemFullName];
                colApplicationItemFullName.Hidden = true;

                UltraGridColumn colLock = band.Columns[COL_Lock];
                colLock.Hidden = true;

                UltraGridColumn colSourceItemName = band.Columns[COL_SourceItemName];
                colSourceItemName.Header.Caption = "Source Asset Class";
                colSourceItemName.Header.VisiblePosition = 1;
                colSourceItemName.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;

                UltraGridColumn colApplicationItemId = band.Columns[COL_ApplicationItemId];
                colApplicationItemId.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                colApplicationItemId.ButtonDisplayStyle = ButtonDisplayStyle.Always;
                colApplicationItemId.Header.VisiblePosition = 2;
                colApplicationItemId.ValueList = cmbApplicationColumnAsset;

                colApplicationItemId.Header.Caption = "Application Asset Class";

                UltraGridColumn colApplicationItemName = band.Columns[COL_ApplicationItemName];
                colApplicationItemName.Hidden = true;

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

        private void grdUnderlyingMapping_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                UltraGridBand band = grdUnderlyingMapping.DisplayLayout.Bands[0];
                grdUnderlyingMapping.DisplayLayout.Override.WrapHeaderText = Infragistics.Win.DefaultableBoolean.True;
                grdUnderlyingMapping.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
                grdUnderlyingMapping.DisplayLayout.ScrollBounds = ScrollBounds.ScrollToLastItem;
                grdUnderlyingMapping.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;
                grdUnderlyingMapping.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
                grdUnderlyingMapping.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
                grdUnderlyingMapping.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
                grdUnderlyingMapping.DisplayLayout.Override.AllowColSwapping = AllowColSwapping.NotAllowed;
                grdUnderlyingMapping.DisplayLayout.Override.HeaderAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
                band.Override.AllowColSwapping = AllowColSwapping.NotAllowed;

                UltraGridColumn colSourceItemFullName = band.Columns[COL_SourceItemFullName];
                colSourceItemFullName.Hidden = true;

                UltraGridColumn colSourceItemID = band.Columns[COL_SourceItemID];
                colSourceItemID.Hidden = true;

                UltraGridColumn colApplicationItemFullName = band.Columns[COL_ApplicationItemFullName];
                colApplicationItemFullName.Hidden = true;

                UltraGridColumn colLock = band.Columns[COL_Lock];
                colLock.Hidden = true;

                UltraGridColumn colSourceItemName = band.Columns[COL_SourceItemName];
                colSourceItemName.Header.VisiblePosition = 1;
                colSourceItemName.Header.Caption = "Source Underlying";
                colSourceItemName.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;

                UltraGridColumn colApplicationItemId = band.Columns[COL_ApplicationItemId];
                colApplicationItemId.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                colApplicationItemId.ButtonDisplayStyle = ButtonDisplayStyle.Always;
                colApplicationItemId.Header.VisiblePosition = 2;
                colApplicationItemId.ValueList = cmbApplicationColumnUnderlying;
                colApplicationItemId.Header.Caption = "Application Underlying";

                UltraGridColumn colApplicationItemName = band.Columns[COL_ApplicationItemName];
                colApplicationItemName.Hidden = true;
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

        private void grdExchangeMapping_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                UltraGridBand band = grdExchangeMapping.DisplayLayout.Bands[0];
                grdExchangeMapping.DisplayLayout.Override.WrapHeaderText = Infragistics.Win.DefaultableBoolean.True;
                grdExchangeMapping.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
                grdExchangeMapping.DisplayLayout.ScrollBounds = ScrollBounds.ScrollToLastItem;
                grdExchangeMapping.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;
                grdExchangeMapping.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
                grdExchangeMapping.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
                grdExchangeMapping.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
                grdExchangeMapping.DisplayLayout.Override.AllowColSwapping = AllowColSwapping.NotAllowed;
                grdExchangeMapping.DisplayLayout.Override.HeaderAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
                band.Override.AllowColSwapping = AllowColSwapping.NotAllowed;

                UltraGridColumn colSourceItemID = band.Columns[COL_SourceItemID];
                colSourceItemID.Hidden = true;

                UltraGridColumn colSourceItemFullName = band.Columns[COL_SourceItemFullName];
                colSourceItemFullName.Hidden = true;

                UltraGridColumn colLock = band.Columns[COL_Lock];
                colLock.Hidden = true;

                UltraGridColumn colSourceItemName = band.Columns[COL_SourceItemName];
                colSourceItemName.Header.VisiblePosition = 1;
                colSourceItemName.Header.Caption = "Source Exchange";
                colSourceItemName.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;

                UltraGridColumn colApplicationItemFullName = band.Columns[COL_ApplicationItemFullName];
                colApplicationItemFullName.Header.Caption = "Exchange Full Name";
                colApplicationItemFullName.CellActivation = Activation.NoEdit;
                colApplicationItemFullName.Header.VisiblePosition = 2;

                UltraGridColumn colApplicationItemId = band.Columns[COL_ApplicationItemId];
                colApplicationItemId.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                colApplicationItemId.ButtonDisplayStyle = ButtonDisplayStyle.Always;
                colApplicationItemId.Header.VisiblePosition = 3;
                colApplicationItemId.ValueList = cmbApplicationColumnExchange;
                colApplicationItemId.Header.Caption = "Application Exchange";

                UltraGridColumn colApplicationItemName = band.Columns[COL_ApplicationItemName];
                colApplicationItemName.Hidden = true;
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

        private void grdCurrencyMapping_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                UltraGridBand band = grdCurrencyMapping.DisplayLayout.Bands[0];
                grdCurrencyMapping.DisplayLayout.Override.WrapHeaderText = Infragistics.Win.DefaultableBoolean.True;
                grdCurrencyMapping.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
                grdCurrencyMapping.DisplayLayout.ScrollBounds = ScrollBounds.ScrollToLastItem;
                grdCurrencyMapping.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;
                grdCurrencyMapping.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
                grdCurrencyMapping.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
                grdCurrencyMapping.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
                grdCurrencyMapping.DisplayLayout.Override.AllowColSwapping = AllowColSwapping.NotAllowed;
                grdCurrencyMapping.DisplayLayout.Override.HeaderAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
                band.Override.AllowColSwapping = AllowColSwapping.NotAllowed;

                UltraGridColumn colSourceItemID = band.Columns[COL_SourceItemID];
                colSourceItemID.Hidden = true;

                UltraGridColumn colSourceItemFullName = band.Columns[COL_SourceItemFullName];
                colSourceItemFullName.Hidden = true;

                UltraGridColumn colLock = band.Columns[COL_Lock];
                colLock.Hidden = true;

                UltraGridColumn colSourceItemName = band.Columns[COL_SourceItemName];
                colSourceItemName.Header.VisiblePosition = 1;
                colSourceItemName.Header.Caption = "Source Currency";
                colSourceItemName.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;

                UltraGridColumn colApplicationItemFullName = band.Columns[COL_ApplicationItemFullName];
                colApplicationItemFullName.Header.Caption = "Currency Full Name";
                colApplicationItemFullName.CellActivation = Activation.NoEdit;
                colApplicationItemFullName.Header.VisiblePosition = 2;

                UltraGridColumn colApplicationItemId = band.Columns[COL_ApplicationItemId];
                colApplicationItemId.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                colApplicationItemId.ButtonDisplayStyle = ButtonDisplayStyle.Always;
                colApplicationItemId.Header.VisiblePosition = 3;
                colApplicationItemId.ValueList = cmbApplicationColumnCurrency;
                colApplicationItemId.Header.Caption = "Application Currency";

                UltraGridColumn colApplicationItemName = band.Columns[COL_ApplicationItemName];
                colApplicationItemName.Hidden = true;
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            bool isCollectionValid = true;

            try
            {
                string selectedTabKey = tabMapAUEC.SelectedTab != null ? tabMapAUEC.SelectedTab.Key : TABKEY_Asset;

                switch (selectedTabKey)
                {
                    case TABKEY_Asset:
                        if (_assetMapping.MappingItems != null && _assetMapping.MappingItems.Count > 0)
                        {
                            foreach (MappingItem mappingItem in _assetMapping.MappingItems)
                            {
                                if (!mappingItem.IsValid)
                                {
                                    isCollectionValid = false;
                                    break;
                                }
                            }

                            if (isCollectionValid)
                            {
                                bool isRepeated = false;
                                foreach (MappingItem mappingItem in _assetMapping.MappingItems)
                                {
                                    foreach (UltraGridRow row in grdAssetMapping.Rows)
                                    {
                                        if (mappingItem.ApplicationItemId == int.Parse(row.Cells["ApplicationItemID"].Value.ToString()) && mappingItem.SourceItemID != int.Parse(row.Cells["SourceItemID"].Value.ToString()) && mappingItem.ApplicationItemId != -1)
                                        {
                                            MessageBox.Show(this, "Application asset column is repeated i.e. assigned more than one time to source asset column. Please remove the repeatition!", "Import Setup", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            isRepeated = true;
                                            return;
                                        }
                                    }
                                    //if (isRepeated == true)
                                    //    break;
                                }
                                if (isRepeated == false)
                                {
                                    _assetMapping.ApplyEdit();
                                    AUECManager.SaveAssetMappings(_assetMapping.MappingItems, _assetMapping.DataSourceNameID);
                                    _assetMapping.MappingItems = AUECManager.GetAssetMappings(_assetMapping.DataSourceNameID);
                                    _assetBindingSource.ResetBindings(false);
                                    _assetMapping.BeginEdit();
                                    MessageBox.Show(this, "Details regarding Asset mapping saved only!", "Import Setup", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
                            else
                            {
                                MessageBox.Show(this, "Unable to save !", "Import Setup", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show(this, "Please map some Asset columns before saving !", "Import Setup", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                        break;

                    case TABKEY_Underlying:
                        if (_underlyingMapping.MappingItems != null && _underlyingMapping.MappingItems.Count > 0)
                        {
                            foreach (MappingItem mappingItem in _underlyingMapping.MappingItems)
                            {
                                if (!mappingItem.IsValid)
                                {
                                    isCollectionValid = false;
                                    break;
                                }
                            }

                            if (isCollectionValid)
                            {
                                bool isRepeated = false;
                                foreach (MappingItem mappingItem in _underlyingMapping.MappingItems)
                                {
                                    foreach (UltraGridRow row in grdUnderlyingMapping.Rows)
                                    {
                                        if (mappingItem.ApplicationItemId == int.Parse(row.Cells["ApplicationItemID"].Value.ToString()) && mappingItem.SourceItemID != int.Parse(row.Cells["SourceItemID"].Value.ToString()) && mappingItem.ApplicationItemId != -1)
                                        {
                                            MessageBox.Show(this, "Application underlying column is repeated i.e. assigned more than one time to source underlying column. Please remove the repeatition!", "Import Setup", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            isRepeated = true;
                                            return;
                                        }
                                    }
                                    //if (isRepeated == true)
                                    //    break;
                                }
                                if (isRepeated == false)
                                {
                                    _underlyingMapping.ApplyEdit();
                                    AUECManager.SaveUnderlyingMappings(_underlyingMapping.MappingItems, _underlyingMapping.DataSourceNameID);
                                    _underlyingMapping.MappingItems = AUECManager.GetUnderlyingMappings(_underlyingMapping.DataSourceNameID);
                                    _underlyingBindingSource.ResetBindings(false);
                                    _underlyingMapping.BeginEdit();
                                    MessageBox.Show(this, "Details regarding Underlying mapping saved only!", "Import Setup", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
                            else
                            {
                                MessageBox.Show(this, "Unable to save !", "Import Setup", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show(this, "Please map some Underlying columns before saving !", "Import Setup", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        break;

                    case TABKEY_Exchange:
                        if (_exchangeMapping.MappingItems != null && _exchangeMapping.MappingItems.Count > 0)
                        {
                            foreach (MappingItem mappingItem in _exchangeMapping.MappingItems)
                            {
                                if (!mappingItem.IsValid)
                                {
                                    isCollectionValid = false;
                                    break;
                                }
                            }

                            if (isCollectionValid)
                            {
                                bool isRepeated = false;
                                foreach (MappingItem mappingItem in _exchangeMapping.MappingItems)
                                {
                                    foreach (UltraGridRow row in grdExchangeMapping.Rows)
                                    {
                                        if (mappingItem.ApplicationItemId == int.Parse(row.Cells["ApplicationItemID"].Value.ToString()) && mappingItem.SourceItemID != int.Parse(row.Cells["SourceItemID"].Value.ToString()) && mappingItem.ApplicationItemId != -1)
                                        {
                                            MessageBox.Show(this, "Application exchange column is repeated i.e. assigned more than one time to source exchange column. Please remove the repeatition!", "Import Setup", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            isRepeated = true;
                                            return;
                                        }
                                    }
                                    //if (isRepeated == true)
                                    //    break;
                                }
                                if (isRepeated == false)
                                {
                                    _exchangeMapping.ApplyEdit();
                                    AUECManager.SaveExchangeMappings(_exchangeMapping.MappingItems, _exchangeMapping.DataSourceNameID);
                                    _exchangeMapping.MappingItems = AUECManager.GetExchangeMappings(_exchangeMapping.DataSourceNameID);
                                    //_exchangeBindingSource.ResetBindings(false); //Y this method is used ?
                                    _exchangeMapping.BeginEdit();
                                    MessageBox.Show(this, "Details regarding Exchange mapping saved only!", "Import Setup", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
                            else
                            {
                                MessageBox.Show(this, "Unable to save !", "Import Setup", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show(this, "Please map some Exchange columns before saving !", "Import Setup", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        break;

                    case TABKEY_Currency:
                        if (_currencyMapping.MappingItems != null && _currencyMapping.MappingItems.Count > 0)
                        {
                            foreach (MappingItem mappingItem in _currencyMapping.MappingItems)
                            {
                                if (!mappingItem.IsValid)
                                {
                                    isCollectionValid = false;
                                    break;
                                }
                            }

                            if (isCollectionValid)
                            {
                                bool isRepeated = false;
                                foreach (MappingItem mappingItem in _currencyMapping.MappingItems)
                                {
                                    foreach (UltraGridRow row in grdCurrencyMapping.Rows)
                                    {
                                        if (mappingItem.ApplicationItemId == int.Parse(row.Cells["ApplicationItemID"].Value.ToString()) && mappingItem.SourceItemID != int.Parse(row.Cells["SourceItemID"].Value.ToString()) && mappingItem.ApplicationItemId != -1)
                                        {
                                            MessageBox.Show(this, "Application currency column is repeated i.e. assigned more than one time to source currecny column. Please remove the repeatition!", "Import Setup", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            isRepeated = true;
                                            return;
                                        }
                                    }
                                    //if (isRepeated == true)
                                    //    break;
                                }
                                if (isRepeated == false)
                                {
                                    _currencyMapping.ApplyEdit();
                                    AUECManager.SaveCurrencyMappings(_currencyMapping.MappingItems, _currencyMapping.DataSourceNameID);
                                    _currencyMapping.MappingItems = AUECManager.GetCurrencyMappings(_currencyMapping.DataSourceNameID);
                                    _currencyBindingSource.ResetBindings(false);
                                    _currencyMapping.BeginEdit();
                                    MessageBox.Show(this, "Details regarding Currency mapping saved only!", "Import Setup", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
                            else
                            {
                                MessageBox.Show(this, "Unable to save !", "Import Setup", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show(this, "Please map some Currency columns before saving !", "Import Setup", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        break;

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



        private void grdCurrencyMapping_CellListSelect(object sender, CellEventArgs e)
        {
            try
            {
                //To Do: Though this is not the best way but we are having two collections one for filling another for lookup !!
                //it will break if more than one currency have same symbol !!

                //e.Cell.Row.Cells["ApplicationItemFullName"].Value = _currencyLookupTable[e.Cell.Row.Cells["ApplicationItemID"].Text] ?? ""; //BB
                //e.Cell.Row.Cells["ApplicationItemID"].Value = _currencyLookupTable[e.Cell.Row.Cells["ApplicationItemID"].Value] ?? ""; //BB
                //e.Cell.Row.Cells["ApplicationItemID"].Value = 
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

        private void grdExchangeMapping_CellListSelect(object sender, CellEventArgs e)
        {
            try
            {
                //e.Cell.Row.Cells["ApplicationItemFullName"].Value = _exchangeLookupTable[e.Cell.Row.Cells["ApplicationItemID"].Text] ?? "";
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

        private void btnClear_Click(object sender, EventArgs e)
        {
            //AssignCurrentTabMappingList(_assetMapping.DataSourceNameID);

            _assetMapping.CancelEdit();
            _underlyingMapping.CancelEdit();
            _exchangeMapping.CancelEdit();
            _currencyMapping.CancelEdit();
        }

    }
}
