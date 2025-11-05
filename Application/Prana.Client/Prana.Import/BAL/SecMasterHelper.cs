using Infragistics.Win;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.CommonDataCache;
using Prana.Fix.FixDictionary;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.MiscUtilities;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.Import
{
    public class SecMasterHelper : IDisposable
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
        private ValueList _sourceOfData = new ValueList();
        public ValueList SourceOfData
        {
            get { return _sourceOfData; }
            set { _sourceOfData = value; }
        }
        private Dictionary<int, string> _dictUdaAsset = new Dictionary<int, string>();
        public Dictionary<int, string> DictUdaAsset
        {
            get { return _dictUdaAsset; }
            set { _dictUdaAsset = value; }
        }
        private Dictionary<int, string> _dictUdaSector = new Dictionary<int, string>();
        public Dictionary<int, string> DictUdaSector
        {
            get { return _dictUdaSector; }
            set { _dictUdaSector = value; }
        }
        private Dictionary<int, string> _dictUdaSubSector = new Dictionary<int, string>();
        public Dictionary<int, string> DictUdaSubSector
        {
            get { return _dictUdaSubSector; }
            set { _dictUdaSubSector = value; }
        }
        private Dictionary<int, string> _dictUdaCountries = new Dictionary<int, string>();
        public Dictionary<int, string> DictUdaCountries
        {
            get { return _dictUdaCountries; }
            set { _dictUdaCountries = value; }
        }
        private Dictionary<int, string> _dictUdaSecurityType = new Dictionary<int, string>();
        public Dictionary<int, string> DictUdaSecurityType
        {
            get { return _dictUdaSecurityType; }
            set { _dictUdaSecurityType = value; }
        }
        /// <summary>
        /// Set Defaults values from cache
        /// </summary>
        internal void GetAllDefaults()
        {

            try
            {
                //call GetAllDefaults only when instance is created
                Dictionary<int, string> dictAssets = CachedDataManager.GetInstance.GetAllAssets();
                Dictionary<int, string> dictUnderlyings = CachedDataManager.GetInstance.GetAllUnderlyings();
                Dictionary<int, string> dictExchanges = CachedDataManager.GetInstance.GetAllExchanges();
                Dictionary<int, string> dictCurrencies = CachedDataManager.GetInstance.GetAllCurrencies();

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

                _optionType.ValueListItems.Clear();
                string[] members = Enum.GetNames(typeof(Prana.BusinessObjects.AppConstants.OptionType));
                foreach (string member in members)
                {
                    string name = member;
                    int i = Convert.ToInt32(Enum.Parse(typeof(Prana.BusinessObjects.AppConstants.OptionType), name));
                    _optionType.ValueListItems.Add(i, name);

                }

                _securityType.ValueListItems.Clear();
                List<EnumerationValue> securityType = EnumHelper.ConvertEnumForBindingWithSelectValue(typeof(SecurityType));
                foreach (EnumerationValue value in securityType)
                {
                    _securityType.ValueListItems.Add(value.Value, value.DisplayText);
                }

                _sourceOfData.ValueListItems.Clear();
                List<EnumerationValue> sourceOfData = EnumHelper.ConvertEnumForBindingWithSelectValue(typeof(SecMasterConstants.SecMasterSourceOfData));
                foreach (EnumerationValue value in sourceOfData)
                {
                    //proper info is not coming from sec master response, so we are setting general info, secmaster or api
                    if (value.DisplayText.Equals(SecMasterConstants.SecMasterSourceOfData.ImportData.ToString()) || value.DisplayText.Equals(SecMasterConstants.SecMasterSourceOfData.SymbolLookup.ToString()) || value.DisplayText.Equals(SecMasterConstants.SecMasterSourceOfData.Database.ToString()))
                        _sourceOfData.ValueListItems.Add(value.Value, "Sec Master");
                    else if (value.DisplayText.Equals(SecMasterConstants.SecMasterSourceOfData.None.ToString()))
                        _sourceOfData.ValueListItems.Add(value.Value, "Validation Failed(API)");
                    else
                        _sourceOfData.ValueListItems.Add(value.Value, "API");
                }

                _approvalSatus.ValueListItems.Clear();
                List<EnumerationValue> approvalSatus = EnumHelper.ConvertEnumForBindingWithSelectValue(typeof(ApprovalStatus));
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

                List<EnumerationValue> frequency = EnumHelper.ConvertEnumForBindingWithSelectValue(typeof(CouponFrequency));
                foreach (EnumerationValue value in frequency)
                {
                    _frequency.ValueListItems.Add(value.Value, value.DisplayText);
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
        ///
        /// </summary>
        /// <param name="UDADataCol"></param>
        public void SetUDAValueLists(Dictionary<string, Dictionary<int, string>> UDADataCol)
        {
            try
            {
                foreach (KeyValuePair<string, Dictionary<int, string>> UDAData in UDADataCol)
                {
                    switch (UDAData.Key)
                    {
                        case SecMasterConstants.CONST_UDASector:
                            FillCommonDataToValueList(_UDASectors, UDAData.Value);
                            DictUdaSector = UDAData.Value;
                            break;

                        case SecMasterConstants.CONST_UDASecurityType:
                            FillCommonDataToValueList(_UDASecurityTypes, UDAData.Value);
                            DictUdaSecurityType = UDAData.Value;
                            break;

                        case SecMasterConstants.CONST_UDASubSector:
                            FillCommonDataToValueList(_UDASubSectors, UDAData.Value);
                            DictUdaSubSector = UDAData.Value;
                            break;

                        case SecMasterConstants.CONST_UDAAsset:
                            FillCommonDataToValueList(_UDAAssets, UDAData.Value);
                            DictUdaAsset = UDAData.Value;

                            break;

                        case SecMasterConstants.CONST_UDACountry:
                            FillCommonDataToValueList(_UDACountry, UDAData.Value);
                            DictUdaCountries = UDAData.Value;
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
                        //modified by omshiv, handle expiration date for  fixed income
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
        //internal Dictionary<String, ValueList> GetRequiredValueListDict()
        //{
        //    Dictionary<String, ValueList> dataValuesList = new Dictionary<string, ValueList>();
        //    try
        //    {
        //        //SecMasterUIObj assssss = new SecMasterUIObj();

        //        if (_assets != null)
        //            dataValuesList.Add("AssetID", _assets);

        //        if (_currencies != null)
        //            dataValuesList.Add("CurrencyID", _currencies);

        //        if (_exchanges != null)
        //            dataValuesList.Add("ExchangeID", _exchanges);

        //        if (_underLying != null)
        //            dataValuesList.Add("UnderLyingID", _underLying);

        //        if (_UDAAssets != null)
        //            dataValuesList.Add("UDAAssetClassID", _UDAAssets);

        //        if (_UDACountry != null)
        //            dataValuesList.Add("UDACountryID", _UDACountry);

        //        if (_UDASectors != null)
        //            dataValuesList.Add("UDASectorID", _UDASectors);

        //        if (_UDASubSectors != null)
        //            dataValuesList.Add("UDASubSectorID", _UDASubSectors);

        //        if (_UDASecurityTypes != null)
        //            dataValuesList.Add("UDASecurityTypeID", _UDASecurityTypes);

        //        if (_optionType != null)
        //            dataValuesList.Add("PutOrCall", _optionType);




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
        //    return dataValuesList;
        //}

        //internal void SetUDAValueLists(Dictionary<string, Dictionary<int, string>> UDADataCol)
        //{
        //    try
        //    {
        //        foreach (KeyValuePair<string, Dictionary<int, string>> UDAData in UDADataCol)
        //        {
        //            switch (UDAData.Key)
        //            {
        //                case SecMasterConstants.CONST_UDASector:
        //                    FillCommonDataToValueList(_UDASectors, UDAData.Value);
        //                    break;

        //                case SecMasterConstants.CONST_UDASecurityType:
        //                    FillCommonDataToValueList(_UDASecurityTypes, UDAData.Value);
        //                    break;

        //                case SecMasterConstants.CONST_UDASubSector:
        //                    FillCommonDataToValueList(_UDASubSectors, UDAData.Value);
        //                    break;

        //                case SecMasterConstants.CONST_UDAAsset:
        //                    FillCommonDataToValueList(_UDAAssets, UDAData.Value);

        //                    break;

        //                case SecMasterConstants.CONST_UDACountry:
        //                    FillCommonDataToValueList(_UDACountry, UDAData.Value);
        //                    break;

        //            }
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
        /// Convert List of SecMasterBaseObj> to List of SecMasterUIObj
        /// </summary>
        /// <param name="secMasterBaseObjCollection"></param>
        /// <returns></returns>
        internal List<SecMasterUIObj> ConvertSecmasterBaseObjCollectionToUICollection(List<SecMasterBaseObj> secMasterBaseObjCollection)
        {
            List<SecMasterUIObj> secMasterUIObject = new List<SecMasterUIObj>();
            try
            {
                foreach (SecMasterBaseObj SMObj in secMasterBaseObjCollection)
                {
                    SecMasterUIObj smUIObj = new SecMasterUIObj();
                    Transformer.CreateObjFromObjThroughReflection(SMObj, smUIObj);
                    SecMasterHelper.SetAssetWiseSMFileds(SMObj, smUIObj);
                    smUIObj.SymbolType = SymbolType.Unchanged;
                    smUIObj.DataSource = SMObj.SourceOfData;

                    secMasterUIObject.Add(smUIObj);
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
            return secMasterUIObject;
        }

        /// <summary>
        /// This Method Converts SecMaster Base Object into SecMasterUIObject which we convert to DataTable.
        /// </summary>
        /// <param name="_secMasterResponseCollection"></param>
        /// <returns>DataTable created from SecMasterUiObjects List</returns>
        internal DataTable ConvertSecMasterBaseObjCollectionToUIObjDataTable(List<SecMasterBaseObj> secMasterBaseObjCollection)
        {
            DataTable dtValidatedSymbols = null;
            try
            {
                List<SecMasterUIObj> secMasterUIObject = ConvertSecmasterBaseObjCollectionToUICollection(secMasterBaseObjCollection);
                if (secMasterUIObject != null && secMasterUIObject.Count > 0)
                {
                    dtValidatedSymbols = GeneralUtilities.GetDataTableFromList(secMasterUIObject);
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
            return dtValidatedSymbols;
        }

        /// <summary>
        /// Convert sec master base object to sec master UI object
        /// UI object can be binded to UI easily
        /// </summary>
        /// <param name="dtSymbols"></param>
        internal List<SecMasterUIObj> ConvertSecMasterBaseObjDataTableToUIObjCollection(DataTable dtSymbols)
        {
            List<SecMasterUIObj> lstSecMasterUIobj = new List<SecMasterUIObj>();
            try
            {

                foreach (DataRow dr in dtSymbols.Rows)
                {
                    SecMasterUIObj secMasterUIobj = new SecMasterUIObj();
                    Transformer.CreateObjThroughReflection(dr, secMasterUIobj);
                    AssetCategory baseAssetCategory = Mapper.GetBaseAssetCategory((AssetCategory)secMasterUIobj.AssetID);

                    switch (baseAssetCategory)
                    {
                        case BusinessObjects.AppConstants.AssetCategory.Option:

                            if (dr.Table.Columns.Contains("OPTExpiration") && dr["OPTExpiration"] != System.DBNull.Value)
                            {
                                DateTime expirationDate = DateTimeConstants.MinValue;
                                DateTime.TryParse(dr["OPTExpiration"].ToString(), out expirationDate);
                                secMasterUIobj.ExpirationDate = expirationDate;
                            }

                            if (dr.Table.Columns.Contains("OptionName") && dr["OptionName"] != System.DBNull.Value)
                            {
                                secMasterUIobj.LongName = dr["OptionName"].ToString();
                            }
                            if (dr.Table.Columns.Contains("Type") && dr["Type"] != System.DBNull.Value)
                            {
                                secMasterUIobj.PutOrCall = (Convert.ToInt32(dr["Type"]));
                            }
                            if (dr.Table.Columns.Contains("OSISymbol") && dr["OSISymbol"] != System.DBNull.Value)
                            {
                                secMasterUIobj.OSIOptionSymbol = dr["OSISymbol"].ToString();
                            }
                            if (dr.Table.Columns.Contains("IDCOSymbol") && dr["IDCOSymbol"] != System.DBNull.Value)
                            {
                                secMasterUIobj.IDCOOptionSymbol = dr["IDCOSymbol"].ToString();
                            }
                            if (dr.Table.Columns.Contains("OPRASymbol") && dr["OPRASymbol"] != System.DBNull.Value)
                            {
                                secMasterUIobj.OPRAOptionSymbol = dr["OPRASymbol"].ToString();
                            }
                            if (dr.Table.Columns.Contains("OPTMultiplier") && dr["OPTMultiplier"] != System.DBNull.Value)
                            {
                                secMasterUIobj.Multiplier = double.Parse(dr["OPTMultiplier"].ToString());
                            }

                            break;

                        case BusinessObjects.AppConstants.AssetCategory.Future:

                            if ((AssetCategory)secMasterUIobj.AssetID == AssetCategory.FXForward)
                            {
                                if (dr.Table.Columns.Contains("FUTExpiration") && dr["FUTExpiration"] != System.DBNull.Value)
                                {
                                    secMasterUIobj.ExpirationDate = Convert.ToDateTime(dr["FUTExpiration"].ToString());
                                }
                                if (dr.Table.Columns.Contains("FxContractName") && dr["FxContractName"] != System.DBNull.Value)
                                {
                                    secMasterUIobj.LongName = dr["FxContractName"].ToString();
                                }
                                if (dr.Table.Columns.Contains("FxForwardMultiplier") && dr["FxForwardMultiplier"] != System.DBNull.Value)
                                {
                                    secMasterUIobj.Multiplier = double.Parse(dr["FxForwardMultiplier"].ToString());
                                }
                                if (dr.Table.Columns.Contains("IsNDF") && dr["IsNDF"] != System.DBNull.Value)
                                {
                                    secMasterUIobj.IsNDF = Convert.ToBoolean(dr["IsNDF"].ToString());
                                }
                                if (dr.Table.Columns.Contains("FixingDate") && dr["FixingDate"] != System.DBNull.Value)
                                {
                                    secMasterUIobj.FixingDate = Convert.ToDateTime(dr["FixingDate"].ToString());
                                }
                            }
                            else
                            {
                                if (dr.Table.Columns.Contains("FUTExpiration") && dr["FUTExpiration"] != System.DBNull.Value)
                                {
                                    secMasterUIobj.ExpirationDate = Convert.ToDateTime(dr["FUTExpiration"].ToString());
                                }
                                if (dr.Table.Columns.Contains("FUTMultiplier") && dr["FUTMultiplier"] != System.DBNull.Value)
                                {
                                    secMasterUIobj.Multiplier = double.Parse(dr["FUTMultiplier"].ToString());
                                }
                                if (dr.Table.Columns.Contains("FutureName") && dr["FutureName"] != System.DBNull.Value)
                                {
                                    secMasterUIobj.LongName = dr["FutureName"].ToString();
                                }
                            }
                            break;
                        case BusinessObjects.AppConstants.AssetCategory.FX:

                            if (dr.Table.Columns.Contains("FxContractName") && dr["FxContractName"] != System.DBNull.Value)
                            {
                                secMasterUIobj.LongName = dr["FxContractName"].ToString();
                            }
                            if (dr.Table.Columns.Contains("FxMultiplier") && dr["FxMultiplier"] != System.DBNull.Value)
                            {
                                secMasterUIobj.Multiplier = Convert.ToDouble(dr["FxMultiplier"].ToString());
                            }
                            if (dr.Table.Columns.Contains("IsNDF") && dr["IsNDF"] != System.DBNull.Value)
                            {
                                secMasterUIobj.IsNDF = Convert.ToBoolean(dr["IsNDF"].ToString());
                            }
                            if (dr.Table.Columns.Contains("FixingDate") && dr["FixingDate"] != System.DBNull.Value)
                            {
                                secMasterUIobj.FixingDate = Convert.ToDateTime(dr["FixingDate"].ToString());
                            }
                            if (dr.Table.Columns.Contains("FxExpirationDate") && dr["FxExpirationDate"] != System.DBNull.Value)
                            {
                                secMasterUIobj.ExpirationDate = Convert.ToDateTime(dr["FxExpirationDate"].ToString());
                            }
                            else
                            {

                            }
                            break;
                        case BusinessObjects.AppConstants.AssetCategory.Indices:
                            if (dr.Table.Columns.Contains("IndexLongName") && dr["IndexLongName"] != System.DBNull.Value)
                            {
                                secMasterUIobj.LongName = dr["IndexLongName"].ToString();
                            }
                            break;
                        case BusinessObjects.AppConstants.AssetCategory.FixedIncome:
                        case BusinessObjects.AppConstants.AssetCategory.ConvertibleBond:
                            {
                                if (dr.Table.Columns.Contains("FixedIncomeLongName") && dr["FixedIncomeLongName"] != System.DBNull.Value)
                                {
                                    secMasterUIobj.LongName = dr["FixedIncomeLongName"].ToString();
                                }
                                if (dr.Table.Columns.Contains("FIMultiplier") && dr["FIMultiplier"] != System.DBNull.Value)
                                {
                                    secMasterUIobj.Multiplier = Convert.ToDouble(dr["FIMultiplier"].ToString());
                                }
                                if (dr.Table.Columns.Contains("MaturityDate") && dr["MaturityDate"] != System.DBNull.Value)
                                {
                                    secMasterUIobj.ExpirationDate = Convert.ToDateTime(dr["MaturityDate"].ToString());
                                }
                                break;
                                //if (dr["CouponFrequency"] != System.DBNull.Value)
                                //{
                                //    secMasterUIobj.FreqID = Convert.ToInt32(dr["CouponFrequency"].ToString());
                                //}
                            }

                    }
                    lstSecMasterUIobj.Insert(lstSecMasterUIobj.Count, secMasterUIobj);// can use Add but left it unchanged

                }
                dtSymbols = GeneralUtilities.GetDataTableFromList(lstSecMasterUIobj);
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
            return lstSecMasterUIobj;
        }

        /// <summary>
        /// Add securities to the datatable which stores the validation information whether securities validated or not for securties for which the validation response has not been received like in esignal.
        ///  Now we receive response of all the symbols from BB api, either they are validated or non validated.
        ///  To handle worst case if we didn't receive response from api, we need this method
        /// </summary>
        /// <param name="dtValidatedSymbols"></param>
        internal void AddNotExistSecuritiesToSecMasterCollection(DataTable dtValidatedSymbols, Dictionary<string, PositionMaster> dictRequestedSymbol)
        {
            try
            {
                foreach (KeyValuePair<string, PositionMaster> item in dictRequestedSymbol)
                {
                    DataRow row = dtValidatedSymbols.NewRow();
                    //if (row.Table.Columns.Contains("IsSecApproved"))
                    //{
                    //    row["IsSecApproved"] = item.Value.Symbol;
                    //}
                    switch (item.Value.Symbology)
                    {
                        case "Symbol":
                            if (row.Table.Columns.Contains("TickerSymbol"))
                            {
                                row["TickerSymbol"] = item.Value.Symbol;
                            }
                            if (row.Table.Columns.Contains("RequestedSymbol"))
                            {
                                row["RequestedSymbol"] = item.Value.Symbol;
                            }
                            if (row.Table.Columns.Contains("RequestedSymbology"))
                            {
                                row["RequestedSymbology"] = "0";
                            }

                            break;
                        case "RIC":
                            if (row.Table.Columns.Contains("TickerSymbol"))
                            {
                                row["ReutersSymbol"] = item.Value.RIC;
                            }
                            if (row.Table.Columns.Contains("RequestedSymbol"))
                            {
                                row["RequestedSymbol"] = item.Value.RIC;
                            }
                            if (row.Table.Columns.Contains("RequestedSymbology"))
                            {
                                row["RequestedSymbology"] = "1";
                            }
                            break;
                        case "Bloomberg":
                            if (row.Table.Columns.Contains("TickerSymbol"))
                            {
                                row["BloombergSymbol"] = item.Value.Bloomberg;
                            }
                            if (row.Table.Columns.Contains("RequestedSymbol"))
                            {
                                row["RequestedSymbol"] = item.Value.Bloomberg;
                            }
                            if (row.Table.Columns.Contains("RequestedSymbology"))
                            {
                                row["RequestedSymbology"] = "5";
                            }
                            break;
                        case "CUSIP":
                            if (row.Table.Columns.Contains("TickerSymbol"))
                            {
                                row["CUSIPSymbol"] = item.Value.CUSIP;
                            }
                            if (row.Table.Columns.Contains("RequestedSymbol"))
                            {
                                row["RequestedSymbol"] = item.Value.CUSIP;
                            }
                            if (row.Table.Columns.Contains("RequestedSymbology"))
                            {
                                row["RequestedSymbology"] = "4";
                            }
                            break;
                        case "ISIN":
                            if (row.Table.Columns.Contains("TickerSymbol"))
                            {
                                row["ISINSymbol"] = item.Value.ISIN;
                            }
                            if (row.Table.Columns.Contains("RequestedSymbol"))
                            {
                                row["RequestedSymbol"] = item.Value.ISIN;
                            }
                            if (row.Table.Columns.Contains("RequestedSymbology"))
                            {
                                row["RequestedSymbology"] = "2";
                            }
                            break;
                        case "SEDOL":
                            if (row.Table.Columns.Contains("TickerSymbol"))
                            {
                                row["SEDOLSymbol"] = item.Value.SEDOL;
                            }
                            if (row.Table.Columns.Contains("RequestedSymbol"))
                            {
                                row["RequestedSymbol"] = item.Value.SEDOL;
                            }
                            if (row.Table.Columns.Contains("RequestedSymbology"))
                            {
                                row["RequestedSymbology"] = "3";
                            }
                            break;
                        case "OSIOptionSymbol":
                            if (row.Table.Columns.Contains("TickerSymbol"))
                            {
                                row["OSIOptionSymbol"] = item.Value.OSIOptionSymbol;
                            }
                            if (row.Table.Columns.Contains("RequestedSymbol"))
                            {
                                row["RequestedSymbol"] = item.Value.OSIOptionSymbol;
                            }
                            if (row.Table.Columns.Contains("RequestedSymbology"))
                            {
                                row["RequestedSymbology"] = "6";
                            }
                            break;
                        case "IDCOOptionSymbol":
                            if (row.Table.Columns.Contains("TickerSymbol"))
                            {
                                row["IDCOOptionSymbol"] = item.Value.IDCOOptionSymbol;
                            }
                            if (row.Table.Columns.Contains("RequestedSymbol"))
                            {
                                row["RequestedSymbol"] = item.Value.IDCOOptionSymbol;
                            }
                            if (row.Table.Columns.Contains("RequestedSymbology"))
                            {
                                row["RequestedSymbology"] = "7";
                            }
                            break;
                        case "OpraOptionSymbol":
                            if (row.Table.Columns.Contains("TickerSymbol"))
                            {
                                row["OPRAOptionSymbol"] = item.Value.OpraOptionSymbol;
                            }
                            if (row.Table.Columns.Contains("RequestedSymbol"))
                            {
                                row["RequestedSymbol"] = item.Value.OpraOptionSymbol;
                            }
                            if (row.Table.Columns.Contains("RequestedSymbology"))
                            {
                                row["RequestedSymbology"] = "8";
                            }
                            break;
                    }
                    dtValidatedSymbols.Rows.Add(row);
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

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {

                _assets.Dispose();
                _currencies.Dispose();
                _exchanges.Dispose();
                _frequency.Dispose();
                _optionType.Dispose();
                _securityType.Dispose();
                _sourceOfData.Dispose();
                _UDAAssets.Dispose();
                _UDACountry.Dispose();
                _UDASectors.Dispose();
                _UDASecurityTypes.Dispose();
                _UDASubSectors.Dispose();
                _underLying.Dispose();
                _approvalSatus.Dispose();
                _accrualBasis.Dispose();
            }
        }

        #endregion
    }
}
