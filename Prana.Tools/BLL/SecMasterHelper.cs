using Infragistics.Win;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.MiscUtilities;
using System;
using System.Collections.Generic;

namespace Prana.Tools
{
    internal class SecMasterHelper : IDisposable
    {

        private static SecMasterHelper _instance = null;
        public static SecMasterHelper getInstance()
        {
            try
            {
                if (_instance == null)
                {
                    _instance = new SecMasterHelper();

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
            return _instance;
        }

        SecMasterHelper()
        {
            try
            {
                GetAllDefaults();
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


        private ValueList _assets = new ValueList();
        public ValueList Assets
        {
            get { return _assets; }
            set { _assets = value; }
        }


        private ValueList _underLying = new ValueList();
        public ValueList UnderLyings
        {
            get { return _underLying; }
            set { _underLying = value; }
        }
        private ValueList _exchanges = new ValueList();
        public ValueList Exchanges
        {
            get { return _exchanges; }
            set { _exchanges = value; }
        }
        private ValueList _currencies = new ValueList();
        public ValueList Currencies
        {
            get { return _currencies; }
            set { _currencies = value; }
        }

        //Added for LeadCurrency and VsCurrency proper dropdown,PRANA-10861
        private ValueList _leadCurrencies = new ValueList();
        public ValueList LeadCurrencies
        {
            get { return _leadCurrencies; }
            set { _leadCurrencies = value; }
        }
        private ValueList _vsCurrencies = new ValueList();
        public ValueList VsCurrencies
        {
            get { return _vsCurrencies; }
            set { _vsCurrencies = value; }
        }

        private ValueList _optionType = new ValueList();
        public ValueList OptionTypes
        {
            get { return _optionType; }
            set { _optionType = value; }
        }

        private ValueList _frequency = new ValueList();
        public ValueList Frequencies
        {
            get { return _frequency; }
            set { _frequency = value; }
        }

        private ValueList _securityType = new ValueList();
        public ValueList SecurityTypes
        {
            get { return _securityType; }
            set { _securityType = value; }
        }

        private ValueList _accrualBasis = new ValueList();
        public ValueList AccrualBasis
        {
            get { return _accrualBasis; }
            set { _accrualBasis = value; }
        }

        private ValueList _collateralType = new ValueList();
        public ValueList CollateralType
        {
            get { return _collateralType; }
            set { _collateralType = value; }
        }

        private ValueList _UDAAssets = new ValueList();
        public ValueList UDAAssets
        {
            get { return _UDAAssets; }
            set { _UDAAssets = value; }
        }
        private ValueList _UDASecurityTypes = new ValueList();
        public ValueList UDASecurityTypes
        {
            get { return _UDASecurityTypes; }
            set { _UDASecurityTypes = value; }
        }
        private ValueList _UDASectors = new ValueList();
        public ValueList UDASectors
        {
            get { return _UDASectors; }
            set { _UDASectors = value; }
        }

        private ValueList _UDASubSectors = new ValueList();
        public ValueList UDASubSectors
        {
            get { return _UDASubSectors; }
            set { _UDASubSectors = value; }
        }
        private ValueList _UDACountry = new ValueList();
        public ValueList UDACountries
        {
            get { return _UDACountry; }
            set { _UDACountry = value; }
        }
        private ValueList _approvalSatus = new ValueList();
        public ValueList ApprovalSatus
        {
            get { return _approvalSatus; }
            set { _approvalSatus = value; }
        }
        private ValueList _createdBy = new ValueList();
        public ValueList CreatedBy
        {
            get { return _createdBy; }
            set { _createdBy = value; }
        }
        private ValueList _modifiedBy = new ValueList();
        public ValueList ModifiedBy
        {
            get { return _modifiedBy; }
            set { _modifiedBy = value; }
        }
        private ValueList _approvedBy = new ValueList();
        public ValueList ApprovedBy
        {
            get { return _approvedBy; }
            set { _approvedBy = value; }
        }
        private ValueList _sourceOfData = new ValueList();
        public ValueList SourceOfData
        {
            get { return _sourceOfData; }
            set { _sourceOfData = value; }
        }
        private ValueList _bondType = new ValueList();
        public ValueList BondType
        {
            get { return _bondType; }
            set { _bondType = value; }
        }

        /// <summary>
        /// Set Defaults values from cache
        ///modified by: sachin mishra 28 jan 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        internal void GetAllDefaults()
        {

            try
            {
                Dictionary<int, string> dictAssets = CachedDataManager.GetInstance.GetAllAssets();
                Dictionary<int, string> dictUnderlyings = CachedDataManager.GetInstance.GetAllUnderlyings();
                Dictionary<int, string> dictExchanges = CachedDataManager.GetInstance.GetAllExchanges();
                Dictionary<int, string> dictCurrencies = CachedDataManager.GetInstance.GetAllCurrencies();
                Dictionary<int, string> dictCompanyUsers = PranaDataManager.GetAllCompanyUsers();

                _assets.ValueListItems.Clear();
                _assets.ValueListItems.Add(int.MinValue, ApplicationConstants.C_COMBO_SELECT);
                foreach (KeyValuePair<int, string> item in dictAssets)
                {
                    _assets.ValueListItems.Add(item.Key, item.Value);
                }

                _underLying.ValueListItems.Clear();
                _underLying.ValueListItems.Add(int.MinValue, ApplicationConstants.C_COMBO_SELECT);
                foreach (KeyValuePair<int, string> item in dictUnderlyings)
                {
                    _underLying.ValueListItems.Add(item.Key, item.Value);
                }

                _exchanges.ValueListItems.Clear();
                _exchanges.ValueListItems.Add(int.MinValue, ApplicationConstants.C_COMBO_SELECT);
                foreach (KeyValuePair<int, string> item in dictExchanges)
                {
                    _exchanges.ValueListItems.Add(item.Key, item.Value);
                }

                _currencies.ValueListItems.Clear();
                _currencies.ValueListItems.Add(int.MinValue, ApplicationConstants.C_COMBO_SELECT);
                foreach (KeyValuePair<int, string> item in dictCurrencies)
                {
                    _currencies.ValueListItems.Add(item.Key, item.Value);
                }


                //Added for leadCurrency and VsCurrency proper dropdown, PRANA-10861
                _leadCurrencies.ValueListItems.Clear();
                _leadCurrencies.ValueListItems.Add(int.MinValue, ApplicationConstants.C_COMBO_SELECT);
                foreach (KeyValuePair<int, string> item in dictCurrencies)
                {
                    _leadCurrencies.ValueListItems.Add(item.Key, item.Value);
                }

                _vsCurrencies.ValueListItems.Clear();
                _vsCurrencies.ValueListItems.Add(int.MinValue, ApplicationConstants.C_COMBO_SELECT);
                foreach (KeyValuePair<int, string> item in dictCurrencies)
                {
                    _vsCurrencies.ValueListItems.Add(item.Key, item.Value);
                }

                string[] members = Enum.GetNames(typeof(BusinessObjects.AppConstants.OptionType));
                _optionType.ValueListItems.Clear();
                foreach (string member in members)
                {
                    string name = member;
                    int i = Convert.ToInt32(Enum.Parse(typeof(BusinessObjects.AppConstants.OptionType), name));
                    _optionType.ValueListItems.Add(i, name);

                }
                List<EnumerationValue> securityType = EnumHelper.ConvertEnumForBindingWithSelectValue(typeof(SecurityType));
                _securityType.ValueListItems.Clear();
                foreach (EnumerationValue value in securityType)
                {
                    _securityType.ValueListItems.Add(value.Value, value.DisplayText);
                }
                _securityType.ValueListItems.RemoveAt(0);

                List<EnumerationValue> approvalSatus = EnumHelper.ConvertEnumForBindingWithSelectValue(typeof(ApprovalStatus));
                _approvalSatus.ValueListItems.Clear();
                foreach (EnumerationValue value in approvalSatus)
                {
                    _approvalSatus.ValueListItems.Add(value.DisplayText, value.DisplayText);
                }

                List<EnumerationValue> accrualBasis = EnumHelper.ConvertEnumForBindingWithSelectValue(typeof(AccrualBasis));
                _accrualBasis.ValueListItems.Clear();
                foreach (EnumerationValue value in accrualBasis)
                {
                    string displayText = value.DisplayText;
                    int valueint = Convert.ToInt32(value.Value);
                    if (valueint == 2 || valueint == 3)
                    {
                        string[] values = value.DisplayText.Split('_');
                        if (values.Length > 1)
                        {
                            displayText = values[1] + '_' + values[2];
                        }

                    }
                    _accrualBasis.ValueListItems.Add(value.Value, displayText);
                }

                EnumerationValueList frequency = EnumHelper.ConvertEnumForBindingWitouthSelect(typeof(CouponFrequency));
                _frequency.ValueListItems.Clear();
                foreach (EnumerationValue value in frequency)
                {
                    _frequency.ValueListItems.Add(value.Value, value.DisplayText);
                }

                List<EnumerationValue> collateralType = EnumHelper.ConvertEnumForBindingWithSelectValue(typeof(CollateralType));
                _collateralType.ValueListItems.Clear();
                foreach (EnumerationValue value in collateralType)
                {
                    _collateralType.ValueListItems.Add(value.Value, value.DisplayText);
                }
                _collateralType.ValueListItems.RemoveAt(0);


                _createdBy.ValueListItems.Clear();
                _createdBy.ValueListItems.Add(int.MinValue, ApplicationConstants.C_COMBO_SELECT);
                foreach (KeyValuePair<int, string> item in dictCompanyUsers)
                {
                    _createdBy.ValueListItems.Add(item.Value, item.Value);
                }

                _modifiedBy.ValueListItems.Clear();
                _modifiedBy.ValueListItems.Add(int.MinValue, ApplicationConstants.C_COMBO_SELECT);
                foreach (KeyValuePair<int, string> item in dictCompanyUsers)
                {
                    _modifiedBy.ValueListItems.Add(item.Value, item.Value);
                }

                _approvedBy.ValueListItems.Clear();
                _approvedBy.ValueListItems.Add(int.MinValue, ApplicationConstants.C_COMBO_SELECT);
                foreach (KeyValuePair<int, string> item in dictCompanyUsers)
                {
                    _approvedBy.ValueListItems.Add(item.Value, item.Value);
                }

                List<EnumerationValue> sourceOfData = EnumHelper.ConvertEnumForBindingWithSelectValueAndCaptionSortedByCaption(typeof(Prana.BusinessObjects.SecMasterConstants.SecMasterSourceOfData));
                _sourceOfData.ValueListItems.Clear();
                foreach (EnumerationValue value in sourceOfData)
                {
                    _sourceOfData.ValueListItems.Add(value.Value, value.DisplayText);
                }

                List<EnumerationValue> bondType = EnumHelper.ConvertEnumForBindingWithSelectValueAndCaptionSortedByCaption(typeof(SecurityType));
                _bondType.ValueListItems.Clear();
                foreach (EnumerationValue value in bondType)
                {
                    _bondType.ValueListItems.Add(value.Value, value.DisplayText);
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
        ///modified by: sachin mishra 28 jan 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        /// <param name="globalvalueList"></param>
        /// <param name="dictData"></param>
        private void FillCommonDataToValueList(ValueList globalvalueList, Dictionary<int, string> dictData)
        {
            try
            {
                globalvalueList.ValueListItems.Clear();
                //  globalvalueList.ValueListItems.Add(int.MinValue, ApplicationConstants.C_COMBO_SELECT);
                foreach (KeyValuePair<int, string> item in dictData)
                {
                    globalvalueList.ValueListItems.Add(item.Key, item.Value);
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

        internal static void SetAssetWiseSMFileds(SecMasterBaseObj SMObj, SecMasterUIObj secMasterUIobj)
        {
            try
            {
                AssetCategory baseAssetCategory = Mapper.GetBaseAssetCategory((AssetCategory)SMObj.AssetID);

                switch (baseAssetCategory)
                {
                    case BusinessObjects.AppConstants.AssetCategory.Option:
                        SecMasterOptObj secMasterOptObj = SMObj as SecMasterOptObj;
                        if (secMasterOptObj != null)
                        {
                            //secMasterUIobj.ExpirationDate = secMasterOptObj.ExpirationDate;
                            //secMasterUIobj.LongName = secMasterOptObj.LongName;
                            secMasterUIobj.PutOrCall = secMasterOptObj.PutOrCall;
                            secMasterUIobj.IDCOOptionSymbol = secMasterOptObj.IDCOOptionSymbol;
                            secMasterUIobj.OPRAOptionSymbol = secMasterOptObj.OpraSymbol;
                            secMasterUIobj.Multiplier = secMasterOptObj.Multiplier;
                            // Set expiration date
                            // http://jira.nirvanasolutions.com:8080/browse/PRANA-8981
                            secMasterUIobj.ExpirationDate = secMasterOptObj.ExpirationDate;
                            secMasterUIobj.StrikePrice = secMasterOptObj.StrikePrice;
                            secMasterUIobj.UnderLyingSymbol = secMasterOptObj.UnderLyingSymbol;
                        }

                        break;

                    case BusinessObjects.AppConstants.AssetCategory.Future:

                        if ((AssetCategory)secMasterUIobj.AssetID == AssetCategory.FXForward)
                        {
                            SecMasterFXForwardObj secMasterFxFwdObj = SMObj as SecMasterFXForwardObj;
                            if (secMasterFxFwdObj != null)
                            {
                                //secMasterUIobj.ExpirationDate = secMasterFutObj.ExpirationDate;
                                //secMasterUIobj.LongName = secMasterFutObj.LongName;
                                secMasterUIobj.IsNDF = secMasterFxFwdObj.IsNDF;
                                secMasterUIobj.FixingDate = secMasterFxFwdObj.FixingDate;
                                secMasterUIobj.Multiplier = secMasterFxFwdObj.Multiplier;
                            }

                        }
                        break;
                    case BusinessObjects.AppConstants.AssetCategory.FX:

                        SecMasterFxObj secMasterFxObj = SMObj as SecMasterFxObj;
                        if (secMasterFxObj != null)
                        {
                            //secMasterUIobj.ExpirationDate = secMasterFutObj.ExpirationDate;
                            //secMasterUIobj.LongName = secMasterFutObj.LongName;
                            secMasterUIobj.IsNDF = secMasterFxObj.IsNDF;
                            secMasterUIobj.FixingDate = secMasterFxObj.FixingDate;
                            secMasterUIobj.Multiplier = secMasterFxObj.Multiplier;
                        }

                        break;
                    case BusinessObjects.AppConstants.AssetCategory.Indices:

                        break;
                    case BusinessObjects.AppConstants.AssetCategory.FixedIncome:
                    case BusinessObjects.AppConstants.AssetCategory.ConvertibleBond:
                        // set expiration date eual to maturity date
                        // http://jira.nirvanasolutions.com:8080/browse/PRANA-8981
                        SecMasterFixedIncome SecMasterFIObj = SMObj as SecMasterFixedIncome;
                        if (SecMasterFIObj != null)
                        {
                            secMasterUIobj.Multiplier = SecMasterFIObj.Multiplier;
                            secMasterUIobj.ExpirationDate = SecMasterFIObj.MaturityDate;
                        }

                        break;


                }

                #region UDA data merge to UI object
                if (SMObj.SymbolUDAData != null)
                {
                    UDAData udaData = SMObj.SymbolUDAData;
                    secMasterUIobj.UDAAssetClassID = udaData.AssetID;
                    secMasterUIobj.UDACountryID = udaData.CountryID;
                    secMasterUIobj.UDASectorID = udaData.SectorID;
                    secMasterUIobj.UDASubSectorID = udaData.SubSectorID;
                    secMasterUIobj.UDASecurityTypeID = udaData.SecurityTypeID;

                }

                #endregion
                secMasterUIobj.DataSource = (SecMasterConstants.SecMasterSourceOfData)Enum.ToObject(typeof(SecMasterConstants.SecMasterSourceOfData), SMObj.SourceOfDataID);

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
        /// <returns></returns>
        internal Dictionary<String, ValueList> GetRequiredValueListDict()
        {
            Dictionary<String, ValueList> dataValuesList = new Dictionary<string, ValueList>();
            try
            {
                //SecMasterUIObj assssss = new SecMasterUIObj();

                if (_assets != null)
                    dataValuesList.Add("AssetID", _assets);

                if (_currencies != null)
                    dataValuesList.Add("CurrencyID", _currencies);

                //Added leadcurrency, vscurrency,AccuralBasis and Coupon Frequency for proper dropdown list, PRANA-10861
                if (_leadCurrencies != null)
                    dataValuesList.Add("LeadCurrencyID", _leadCurrencies);

                if (_vsCurrencies != null)
                    dataValuesList.Add("VsCurrencyID", _vsCurrencies);

                if (_accrualBasis != null)
                    dataValuesList.Add("AccrualBasisID", _accrualBasis);

                if (_collateralType != null)
                    dataValuesList.Add("CollateralTypeID", _collateralType);

                if (_frequency != null)
                    dataValuesList.Add("CouponFrequencyID", _frequency);

                if (_exchanges != null)
                    dataValuesList.Add("ExchangeID", _exchanges);

                if (_underLying != null)
                    dataValuesList.Add("UnderLyingID", _underLying);

                if (_UDAAssets != null)
                    dataValuesList.Add("UDAAssetClassID", _UDAAssets);

                if (_UDACountry != null)
                    dataValuesList.Add("UDACountryID", _UDACountry);

                if (_UDASectors != null)
                    dataValuesList.Add("UDASectorID", _UDASectors);

                if (_UDASubSectors != null)
                    dataValuesList.Add("UDASubSectorID", _UDASubSectors);

                if (_UDASecurityTypes != null)
                    dataValuesList.Add("UDASecurityTypeID", _UDASecurityTypes);

                //renamed PutOrCall to Type as it is defined as 'Type' in database
                if (_optionType != null)
                    dataValuesList.Add("Type", _optionType);

                if (_createdBy != null)
                    dataValuesList.Add("CreatedBy", _createdBy);

                if (_modifiedBy != null)
                    dataValuesList.Add("ModifiedBy", _modifiedBy);

                if (_approvedBy != null)
                    dataValuesList.Add("ApprovedBy", _approvedBy);

                if (_sourceOfData != null)
                    dataValuesList.Add("DataSource", _sourceOfData);

                if (_bondType != null)
                    dataValuesList.Add("BondTypeID", _bondType);

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
            return dataValuesList;
        }
        /// <summary>
        ///modified by: sachin mishra 28 jan 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        /// <param name="UDADataCol"></param>
        internal void SetUDAValueLists(Dictionary<string, Dictionary<int, string>> UDADataCol)
        {
            try
            {
                foreach (KeyValuePair<string, Dictionary<int, string>> UDAData in UDADataCol)
                {
                    switch (UDAData.Key)
                    {
                        case SecMasterConstants.CONST_UDASector:
                            FillCommonDataToValueList(_UDASectors, UDAData.Value);
                            break;

                        case SecMasterConstants.CONST_UDASecurityType:
                            FillCommonDataToValueList(_UDASecurityTypes, UDAData.Value);
                            break;

                        case SecMasterConstants.CONST_UDASubSector:
                            FillCommonDataToValueList(_UDASubSectors, UDAData.Value);
                            break;

                        case SecMasterConstants.CONST_UDAAsset:
                            FillCommonDataToValueList(_UDAAssets, UDAData.Value);

                            break;

                        case SecMasterConstants.CONST_UDACountry:
                            FillCommonDataToValueList(_UDACountry, UDAData.Value);
                            break;

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

        public void Dispose()
        {
            if (_instance != null)
            {
                _instance.Dispose();
                _UDAAssets.Dispose();
                _UDACountry.Dispose();
                _UDASectors.Dispose();
                _UDASecurityTypes.Dispose();
                _UDASubSectors.Dispose();
                _approvalSatus.Dispose();
                _assets.Dispose();
                _createdBy.Dispose();
                _exchanges.Dispose();
                _modifiedBy.Dispose();
                _securityType.Dispose();
                _underLying.Dispose();
                _sourceOfData.Dispose();
                _optionType.Dispose();
                _frequency.Dispose();
                _currencies.Dispose();
                _bondType.Dispose();
                _approvedBy.Dispose();
                _accrualBasis.Dispose();
                _collateralType.Dispose();
                _leadCurrencies.Dispose();
                _vsCurrencies.Dispose();
            }
        }

        /// <summary>
        /// Returns symbology code according to pass search criteria.
        /// </summary>
        /// <param name="symbology"></param>
        /// <returns></returns>
        internal static int GetSymbology(string symbology)
        {
            try
            {
                SecMasterConstants.SearchCriteria searchCriteria = (SecMasterConstants.SearchCriteria)Enum.Parse(typeof(SecMasterConstants.SearchCriteria), symbology);

                switch (searchCriteria)
                {
                    case SecMasterConstants.SearchCriteria.Ticker:
                        return (int)ApplicationConstants.SymbologyCodes.TickerSymbol;
                    case SecMasterConstants.SearchCriteria.Bloomberg:
                        return (int)ApplicationConstants.SymbologyCodes.BloombergSymbol;
                    case SecMasterConstants.SearchCriteria.FactSetSymbol:
                        return (int)ApplicationConstants.SymbologyCodes.FactSetSymbol;
                    case SecMasterConstants.SearchCriteria.ActivSymbol:
                        return (int)ApplicationConstants.SymbologyCodes.ActivSymbol;
                    case SecMasterConstants.SearchCriteria.CUSIP:
                        return (int)ApplicationConstants.SymbologyCodes.CUSIPSymbol;
                    case SecMasterConstants.SearchCriteria.ISIN:
                        return (int)ApplicationConstants.SymbologyCodes.ISINSymbol;
                    case SecMasterConstants.SearchCriteria.SEDOL:
                        return (int)ApplicationConstants.SymbologyCodes.SEDOLSymbol;
                    case SecMasterConstants.SearchCriteria.ReutersSymbol:
                        return (int)ApplicationConstants.SymbologyCodes.ReutersSymbol;
                    case SecMasterConstants.SearchCriteria.OSIOption:
                        return (int)ApplicationConstants.SymbologyCodes.OSIOptionSymbol;
                    case SecMasterConstants.SearchCriteria.IDCOOption:
                        return (int)ApplicationConstants.SymbologyCodes.IDCOOptionSymbol;
                    case SecMasterConstants.SearchCriteria.OPRAOption:
                        return (int)ApplicationConstants.SymbologyCodes.OPRAOptionSymbol;
                    case SecMasterConstants.SearchCriteria.BBGID:
                        return (int)SecMasterConstants.SearchCriteria.BBGID;
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
            return (int)ApplicationConstants.SymbologyCodes.TickerSymbol;
        }

        /// <summary>
        /// get list of columns which need to be removed from search in Advance search UI
        /// </summary>
        /// <returns>list of columns</returns>
        internal List<string> GetRemoveColumnsList()
        {
            List<string> columnList = new List<string>();
            try
            {
                columnList.Add("AUECID");
                columnList.Add("Multiplier");
                columnList.Add("LongName");
                columnList.Add("SymbolType");
                columnList.Add("DynamicUDA");
                columnList.Add("UseUDAFromUnderlyingOrRoot");
                columnList.Add("MaturityDay");
                columnList.Add("MaturityMonth");
                columnList.Add("RequestedSymbol");
                columnList.Add("RequestedSymbology");
                columnList.Add("SecApprovalStatus");
                columnList.Add("CountryCode");
                columnList.Add("Sector");
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return columnList;
        }

        /// <summary>
        /// gets ambigousColumn and tableNameIdentifier dictionary for advanced search
        /// </summary>
        /// <returns></returns>
        internal Dictionary<string, string> GetAmbigousColumnsDictionary()
        {
            Dictionary<string, string> columnsDictionary = new Dictionary<string, string>();
            try
            {
                columnsDictionary.Add("ExchangeID", "SM");
                columnsDictionary.Add("RoundLot", "SM");
                columnsDictionary.Add("Symbol_PK", "SM");
                columnsDictionary.Add("VsCurrencyID", "FxData,FxForwardData");
                columnsDictionary.Add("LeadCurrencyID", "FxData,FxForwardData");
                columnsDictionary.Add("FixingDate", "FxData,FxForwardData");
                columnsDictionary.Add("IsNDF", "FxData,FxForwardData");
                columnsDictionary.Add("ExpirationDate", "OPT,FUT,FxForwardData,FxData");
                columnsDictionary.Add("IsCurrencyFuture", "OPT,FUT");
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return columnsDictionary;
        }

        /// <summary>
        /// gets column to be renamed dictionary for advanced search
        /// </summary>
        /// <returns></returns>
        internal Dictionary<string, string> GetRenameColumnsDictionary()
        {
            Dictionary<string, string> columnsDictionary = new Dictionary<string, string>();
            try
            {
                columnsDictionary.Add("IDCOOptionSymbol", "IDCOSymbol");
                columnsDictionary.Add("OPRAOptionSymbol", "OPRASymbol");
                columnsDictionary.Add("OSIOptionSymbol", "OSISymbol");
                columnsDictionary.Add("PutOrCall", "Type");
                columnsDictionary.Add("StrikePrice", "Strike");
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return columnsDictionary;
        }
    }
}
