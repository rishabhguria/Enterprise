using Prana.BusinessObjects.AppConstants;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml;

namespace Prana.BusinessObjects.SecurityMasterBusinessObjects
{
    [Serializable]
    [KnownType(typeof(SecMasterEquityObj))]
    [KnownType(typeof(SecMasterFixedIncome))]
    [KnownType(typeof(SecMasterFutObj))]
    [KnownType(typeof(SecMasterFXForwardObj))]
    [KnownType(typeof(SecMasterFxObj))]
    [KnownType(typeof(SecMasterIndexObj))]
    [KnownType(typeof(SecMasterOptObj))]

    public abstract class SecMasterBaseObj
    {
        //SecMasterCoreObject _SecMasterCoreObj=null ;
        // string _tickerSymbol = string.Empty;
        protected string _underLyingSymbol = string.Empty;
        protected string _factSetSymbol = string.Empty;
        protected string _activSymbol = string.Empty;

        int _auecID = int.MinValue;
        int _assetID = int.MinValue;
        int _underlyingID = int.MinValue;
        int _exchangeID = int.MinValue;
        int _currencyID = int.MinValue;
        private List<string> _symbologyMapping = new List<string>();
        protected double _multiplier = 0;
        protected decimal _roundLot = 1;
        private SerializableDictionary<String, Object> _dynamicUDA = new SerializableDictionary<string, object>();
        public SerializableDictionary<String, Object> DynamicUDA { get { return _dynamicUDA; } }

        public virtual void FillData(object[] row, int offset)
        {

            // these int values are the position column position in DB of symbollookup table 
            // index to get data from data reader
            int AssetID = 0;

            int UnderLyingSymbol = 1;
            int AUECID = 2;
            int UnderLyingID = 3;
            int ExchangeID = 4;
            int CurrencyID = 5;
            int Reuterssymbol = 7;
            int ISINsymbol = 8;
            int Sedolsymbol = 9;
            int Cusipsymbol = 10;
            int Bloombergsymbol = 11;
            int OSIOptionSymbol = 12;
            int IDCOOptionSymbol = 13;
            int OPRASymbol = 14;
            int FactSetSymbol = 15;
            int ActivSymbol = 16;
            int delta = 20;
            //Round Lot
            int RoundLot = 56;
            int ProxySymbol = 57;

            // approved status fields
            int IsSecApproved = 58;
            int ApprovalDate = 59;
            int ApprovedBy = 60;
            int Comment = 61;

            // UDA related fields 
            int UDAAssetClassID = 62;
            int UDASecurityTypeID = 63;
            int UDASectorID = 64;
            int UDASubSectorID = 65;
            int UDACountryID = 66;
            int UDAAssetName = 67;
            int UDASecurityTypeName = 68;
            int UDASectorName = 69;
            int UDASubSectorName = 70;
            int UDACountryName = 71;

            int CreatedBy = 72;
            int ModifiedBy = 73;
            int PrimarySymbology = 74;
            int BBGID = 75;
            int CreationDate = 76;
            int ModifiedDate = 77;
            int StrikePriceMultiplier = 78;
            int DataSource = 79;
            // 78 is for DataSource Field
            int EsignalOptionRoot = 80;
            int BloombergOptionRoot = 81;
            int dynamicUDA = 82;
            int SharesOutstandingID = 85;
            int BloombergSymbolWithExCode = 86;
            int LongName = Prana.Global.ApplicationConstants.SymbologyCodesCount + 6;
            int Sector = Prana.Global.ApplicationConstants.SymbologyCodesCount + 7;
            int Symbol_PK = Prana.Global.ApplicationConstants.SymbologyCodesCount + 8;

            try
            {
                if (row[AssetID] != System.DBNull.Value)
                {
                    _assetID = int.Parse(row[AssetID].ToString());
                }

                if (row[UnderLyingSymbol] != System.DBNull.Value)
                {
                    _underLyingSymbol = row[UnderLyingSymbol].ToString();
                }
                if (row[FactSetSymbol] != System.DBNull.Value)
                {
                    _factSetSymbol = row[FactSetSymbol].ToString();
                }
                if (row[ActivSymbol] != System.DBNull.Value)
                {
                    _activSymbol = row[ActivSymbol].ToString();
                }
                if (row[AUECID] != System.DBNull.Value)
                {
                    _auecID = int.Parse(row[AUECID].ToString());
                }
                if (row[UnderLyingID] != System.DBNull.Value)
                {
                    _underlyingID = int.Parse(row[UnderLyingID].ToString());
                }
                if (row[ExchangeID] != System.DBNull.Value)
                {
                    _exchangeID = int.Parse(row[ExchangeID].ToString());
                }
                if (row[CurrencyID] != System.DBNull.Value)
                {
                    _currencyID = int.Parse(row[CurrencyID].ToString());
                }
                if (row[Reuterssymbol] != System.DBNull.Value)
                {

                    _reutersSymbol = row[Reuterssymbol].ToString();
                }

                if (row[ISINsymbol] != System.DBNull.Value)
                {

                    _isinSymbol = row[ISINsymbol].ToString();
                }
                if (row[Sedolsymbol] != System.DBNull.Value)
                {

                    _sedolSymbol = row[Sedolsymbol].ToString();
                }
                if (row[Cusipsymbol] != System.DBNull.Value)
                {

                    _cusipSymbol = row[Cusipsymbol].ToString();
                }
                if (row[Bloombergsymbol] != System.DBNull.Value)
                {
                    _bloombergSymbol = row[Bloombergsymbol].ToString();
                }

                if (row[OSIOptionSymbol] != System.DBNull.Value)
                {
                    _osiOptionSymbol = row[OSIOptionSymbol].ToString();
                }
                if (row[IDCOOptionSymbol] != System.DBNull.Value)
                {
                    _idcoOptionSymbol = row[IDCOOptionSymbol].ToString();
                }
                if (row[OPRASymbol] != System.DBNull.Value)
                {
                    _opraSymbol = row[OPRASymbol].ToString();
                }
                if (row[RoundLot] != System.DBNull.Value)
                {
                    _roundLot = Convert.ToDecimal(row[RoundLot]);
                }

                for (int count = 6; count < (Prana.Global.ApplicationConstants.SymbologyCodesCount + 6); count++)
                {
                    if (row[count] != null)
                        _symbologyMapping.Add(row[count].ToString().ToUpper());
                    else
                        _symbologyMapping.Add(string.Empty);
                }

                if (row[LongName] != System.DBNull.Value)
                {
                    _longName = row[LongName].ToString();
                }

                if (row[Sector] != System.DBNull.Value)
                {
                    _sector = row[Sector].ToString();
                }

                if (row[Symbol_PK] != System.DBNull.Value)
                {
                    _symbol_PK = Convert.ToInt64(row[Symbol_PK].ToString());
                }
                //_multiplier = 1;
                if (row[ProxySymbol] != System.DBNull.Value)
                {
                    _proxySymbol = row[ProxySymbol].ToString();
                }


                #region Sec approved status related fields

                if (row[IsSecApproved] != System.DBNull.Value)
                {
                    _isSecApproved = Boolean.Parse(row[IsSecApproved].ToString());
                }

                if (row[ApprovalDate] != System.DBNull.Value)
                {
                    _approvalDate = DateTime.Parse(row[ApprovalDate].ToString());
                }

                if (row[ApprovedBy] != System.DBNull.Value)
                {
                    _approvedBy = row[ApprovedBy].ToString();
                }

                if (row[Comment] != System.DBNull.Value)
                {
                    _comments = row[Comment].ToString();
                }
                if (row[delta] != System.DBNull.Value)
                {
                    _delta = float.Parse(row[delta].ToString());
                }

                #endregion

                // UDA related fields 


                #region UDA data related fields
                SymbolUDAData = new UDAData();
                SymbolUDAData.Symbol = TickerSymbol;
                SymbolUDAData.CompanyName = _longName;
                SymbolUDAData.UnderlyingSymbol = _underLyingSymbol;
                if (row[UDAAssetClassID] != System.DBNull.Value)
                {
                    SymbolUDAData.AssetID = int.Parse(row[UDAAssetClassID].ToString());
                }

                if (row[UDASecurityTypeID] != System.DBNull.Value)
                {
                    SymbolUDAData.SecurityTypeID = int.Parse(row[UDASecurityTypeID].ToString());
                }

                if (row[UDASectorID] != System.DBNull.Value)
                {
                    SymbolUDAData.SectorID = int.Parse(row[UDASectorID].ToString());
                }

                if (row[UDASubSectorID] != System.DBNull.Value)
                {
                    SymbolUDAData.SubSectorID = int.Parse(row[UDASubSectorID].ToString());
                }
                if (row[UDACountryID] != System.DBNull.Value)
                {
                    SymbolUDAData.CountryID = int.Parse(row[UDACountryID].ToString());
                }

                if (row[UDAAssetName] != System.DBNull.Value)
                {
                    SymbolUDAData.UDAAsset = row[UDAAssetName].ToString();
                }
                if (row[UDASecurityTypeName] != System.DBNull.Value)
                {
                    SymbolUDAData.UDASecurityType = row[UDASecurityTypeName].ToString();
                }
                if (row[UDASectorName] != System.DBNull.Value)
                {
                    SymbolUDAData.UDASector = row[UDASectorName].ToString();
                }
                if (row[UDASubSectorName] != System.DBNull.Value)
                {
                    SymbolUDAData.UDASubSector = row[UDASubSectorName].ToString();
                }
                if (row[UDACountryName] != System.DBNull.Value)
                {
                    SymbolUDAData.UDACountry = row[UDACountryName].ToString();
                }

                #endregion




                if (row[CreatedBy] != System.DBNull.Value)
                {
                    _createdBy = row[CreatedBy].ToString();
                }
                if (row[ModifiedBy] != System.DBNull.Value)
                {
                    _modifiedBy = row[ModifiedBy].ToString();
                }
                if (row[PrimarySymbology] != System.DBNull.Value)
                {
                    _primarySymbology = int.Parse(row[PrimarySymbology].ToString());
                }
                if (row[BBGID] != System.DBNull.Value)
                {
                    _BBGID = row[BBGID].ToString();
                }
                if (row[CreationDate] != System.DBNull.Value)
                {
                    _creationDate = DateTime.Parse(row[CreationDate].ToString());
                }

                if (row[ModifiedDate] != System.DBNull.Value)
                {
                    _modifiedDate = DateTime.Parse(row[ModifiedDate].ToString());
                }

                if (row[StrikePriceMultiplier] != System.DBNull.Value)
                {
                    _strikePriceMultiplier = double.Parse(row[StrikePriceMultiplier].ToString());
                }

                if (row[DataSource] != System.DBNull.Value)
                {
                    _sourceOfDataID = int.Parse(row[DataSource].ToString());
                }

                if (row[EsignalOptionRoot] != System.DBNull.Value)
                {
                    _esignalOptionRoot = row[EsignalOptionRoot].ToString();
                }

                if (row[BloombergOptionRoot] != System.DBNull.Value)
                {
                    _bloombergOptionRoot = row[BloombergOptionRoot].ToString();
                }
                if (row[dynamicUDA] != System.DBNull.Value)
                {
                    UpdateDynamicDictionary(row[dynamicUDA].ToString());
                }
                if (row[SharesOutstandingID] != System.DBNull.Value)
                {
                    _SharesOutstanding = Double.Parse(row[SharesOutstandingID].ToString());
                }
                if (row[BloombergSymbolWithExCode] != System.DBNull.Value)
                {
                    _bloombergSymbolWithExchangeCode = row[BloombergSymbolWithExCode].ToString();
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
        /// Converting XML to Dictionary
        /// </summary>
        /// <param name="dynamicXml"></param>
        private void UpdateDynamicDictionary(string dynamicXml)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(dynamicXml);
                //Dictionary<string, string> dict = new Dictionary<string, string>();
                foreach (XmlElement node in xmlDoc.DocumentElement)
                {
                    if (!_dynamicUDA.ContainsKey(node.Name))
                        _dynamicUDA.Add(node.Name, node.InnerText);
                    else
                        _dynamicUDA[node.Name] = node.InnerText;
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

        public virtual void FillData(SymbolData level1Data)
        {
            _underlyingID = (int)level1Data.UnderlyingCategory;
            _assetID = (int)level1Data.CategoryCode;
            _auecID = level1Data.AUECID;
            _longName = level1Data.FullCompanyName;
            _BBGID = level1Data.BBGID;
            if (level1Data.UnderlyingSymbol != null)
            {
                _underLyingSymbol = level1Data.UnderlyingSymbol;
            }
            // _tickerSymbol = level1Data.Symbol;
            _exchangeID = level1Data.ExchangeID;
            _bloombergSymbol = level1Data.BloombergSymbol;
            _symbologyMapping.Add(level1Data.Symbol);
            _symbologyMapping.Add(level1Data.ReuterSymbol);
            _symbologyMapping.Add(level1Data.ISIN);
            _symbologyMapping.Add(level1Data.SedolSymbol);
            _symbologyMapping.Add(level1Data.CusipNo);
            _symbologyMapping.Add(level1Data.BloombergSymbol);
            _symbologyMapping.Add(level1Data.OSIOptionSymbol);
            _symbologyMapping.Add(level1Data.IDCOOptionSymbol);
            _symbologyMapping.Add(level1Data.OpraSymbol);
            _symbologyMapping.Add(level1Data.FactSetSymbol);
            _symbologyMapping.Add(level1Data.ActivSymbol);

            _osiOptionSymbol = level1Data.OSIOptionSymbol;
            _idcoOptionSymbol = level1Data.IDCOOptionSymbol;
            _opraSymbol = level1Data.OpraSymbol;
            _factSetSymbol = level1Data.FactSetSymbol;
            _cusipSymbol = level1Data.CusipNo;

            _requestedSymbology = (int)level1Data.RequestedSymbology;
            _activSymbol = level1Data.ActivSymbol;
            _SharesOutstanding = level1Data.SharesOutstanding;
            _bloombergSymbolWithExchangeCode = level1Data.BloombergSymbolWithExchangeCode;
            //for (int i = 1; i < Prana.Global.ApplicationConstants.SymbologyCodesCount; i++)
            //{
            //    _symbologyMapping.Add(string.Empty);
            //}

            /// Adding Options symbology changes (Should go under options but to keep those at other symbology level
            /// to avoid any instant issue) 


        }


        /// <summary>
        /// For Filling SecMaster UI Data in base object to save in DB
        /// </summary>
        /// <param name="uiObj"></param>
        public virtual void FillUIData(SecMasterUIObj uiObj)
        {
            _underlyingID = uiObj.UnderLyingID;
            _assetID = uiObj.AssetID;
            _auecID = uiObj.AUECID;
            _longName = uiObj.LongName;
            if (uiObj.UnderLyingSymbol != null)
            {
                _underLyingSymbol = uiObj.UnderLyingSymbol;
            }
            //_tickerSymbol = uiObj.TickerSymbol;
            _exchangeID = uiObj.ExchangeID;

            _currencyID = uiObj.CurrencyID;
            _symbol_PK = uiObj.Symbol_PK;
            _factSetSymbol = uiObj.FactSetSymbol;
            _activSymbol = uiObj.ActivSymbol;
            _reutersSymbol = uiObj.ReutersSymbol;
            _isinSymbol = uiObj.ISINSymbol;
            _cusipSymbol = uiObj.CusipSymbol;
            _sedolSymbol = uiObj.SedolSymbol;
            _bloombergSymbol = uiObj.BloombergSymbol;
            _symbolType = (int)uiObj.SymbolType;
            _sector = uiObj.Sector;
            _multiplier = uiObj.Multiplier;
            _osiOptionSymbol = uiObj.OSIOptionSymbol;
            _idcoOptionSymbol = uiObj.IDCOOptionSymbol;
            _opraSymbol = uiObj.OPRAOptionSymbol;
            _roundLot = uiObj.RoundLot;
            _proxySymbol = uiObj.ProxySymbol;

            #region fill approved status fields
            _isSecApproved = uiObj.IsSecApproved;

            //TODO Approvedby and Approval date change if user aprrove the security.
            _approvedBy = uiObj.ApprovedBy;
            _approvalDate = uiObj.ApprovalDate;
            _comments = uiObj.Comments;
            #endregion

            _createdBy = uiObj.CreatedBy;
            _modifiedBy = uiObj.ModifiedBy;
            _primarySymbology = uiObj.PrimarySymbology;
            _modifiedDate = uiObj.ModifiedDate;
            _creationDate = uiObj.CreationDate;
            _BBGID = uiObj.BBGID;
            _strikePriceMultiplier = uiObj.StrikePriceMultiplier;
            _esignalOptionRoot = uiObj.EsignalOptionRoot;
            _bloombergOptionRoot = uiObj.BloombergOptionRoot;
            _sourceOfDataID = (int)uiObj.DataSource;
            _SharesOutstanding = uiObj.SharesOutstanding;
            _bloombergSymbolWithExchangeCode = uiObj.BloombergSymbolWithExchangeCode;
            #region fill UDA Symbol Data object from sec master UI object

            _symbolUDAData = new UDAData();
            _symbolUDAData.Symbol = uiObj.TickerSymbol;
            _symbolUDAData.AssetID = uiObj.UDAAssetClassID;
            _symbolUDAData.CountryID = uiObj.UDACountryID;
            _symbolUDAData.SectorID = uiObj.UDASectorID;
            _symbolUDAData.SubSectorID = uiObj.UDASubSectorID;
            _symbolUDAData.SecurityTypeID = uiObj.UDASecurityTypeID;
            _symbolUDAData.CompanyName = uiObj.LongName;
            #endregion

            _useUDAFromUnderlyingOrRoot = uiObj.UseUDAFromUnderlyingOrRoot;

            _symbologyMapping.Add(uiObj.TickerSymbol);
            _symbologyMapping.Add(uiObj.ReutersSymbol);
            _symbologyMapping.Add(uiObj.ISINSymbol);
            _symbologyMapping.Add(uiObj.SedolSymbol);
            _symbologyMapping.Add(uiObj.CusipSymbol);
            _symbologyMapping.Add(uiObj.BloombergSymbol);
            _symbologyMapping.Add(uiObj.OSIOptionSymbol);
            _symbologyMapping.Add(uiObj.IDCOOptionSymbol);
            _symbologyMapping.Add(uiObj.OPRAOptionSymbol);
            _symbologyMapping.Add(uiObj.FactSetSymbol);
            _symbologyMapping.Add(uiObj.ActivSymbol);
            //for (int i = 1; i < Prana.Global.ApplicationConstants.SymbologyCodesCount; i++)
            //{
            //    _symbologyMapping.Add(uiob.Empty);
            //}

            //  _SecMasterCoreObj = new SecMasterCoreObject();
            //  _SecMasterCoreObj.FillData(level1Data);

            // Update dynamic UDA dictionary
            _dynamicUDA = uiObj.DynamicUDA;

        }
        public virtual void UpDateData(object secMasterObj)
        {
            SecMasterBaseObj secMasterOptObj = (SecMasterBaseObj)secMasterObj;

            _auecID = secMasterOptObj.AUECID;

            _assetID = secMasterOptObj.AssetID;

            _underlyingID = secMasterOptObj.UnderLyingID;

            _underLyingSymbol = secMasterOptObj.UnderLyingSymbol;

            _exchangeID = secMasterOptObj.ExchangeID;
            _isSecApproved = secMasterOptObj._isSecApproved;

            _longName = secMasterOptObj.LongName;

            _osiOptionSymbol = secMasterOptObj.OSIOptionSymbol;
            _idcoOptionSymbol = secMasterOptObj.IDCOOptionSymbol;
            _opraSymbol = secMasterOptObj.OpraSymbol;

        }

        public int AssetID
        {
            get { return _assetID; }
            set { _assetID = value; }

        }

        public int UnderLyingID
        {
            get { return _underlyingID; }
            set { _underlyingID = value; }

        }
        public int AUECID
        {
            get { return _auecID; }
            set
            {
                _auecID = value;
            }

        }



        public int ExchangeID
        {
            get { return _exchangeID; }
            set { _exchangeID = value; }

        }
        public int CurrencyID
        {
            get { return _currencyID; }
            set { _currencyID = value; }

        }
        public string UnderLyingSymbol
        {
            get { return _underLyingSymbol; }
            set { _underLyingSymbol = value; }

        }

        public string TickerSymbol
        {
            get { return _symbologyMapping[(int)ApplicationConstants.SymbologyCodes.TickerSymbol]; }
            // set { _tickerSymbol = value; }


        }

        public string FactSetSymbol
        {
            get { return _factSetSymbol; }
            set { _factSetSymbol = value; }
        }

        public string ActivSymbol
        {
            get { return _activSymbol; }
            set { _activSymbol = value; }
        }

        public override string ToString()
        {
            return " Ticker Symbol=" + TickerSymbol + " Underlying Symbol=" + _underLyingSymbol + " UnderlyingID=" + _underlyingID + " AssetID=" + _assetID.ToString() + " AUECID=" + _auecID.ToString() + "ExchangeID" + _exchangeID.ToString();
        }

        private int _requestedSymbology;

        public int RequestedSymbology
        {
            get { return _requestedSymbology; }
            set { _requestedSymbology = value; }
        }

        public List<string> SymbologyMapping
        {
            get { return _symbologyMapping; }
        }
        //private string _requestedSymbol;

        public string RequestedSymbol
        {
            get { return _symbologyMapping[(int)_requestedSymbology]; }

        }

        protected string _longName = string.Empty;

        public string LongName
        {
            get { return _longName; }
            set { _longName = value; }
        }
        private string _sector;

        public string Sector
        {
            get { return _sector; }
            set { _sector = value; }
        }

        public AssetCategory AssetCategory
        {
            get { return (AssetCategory)_assetID; }
        }

        private Int64 _symbol_PK;

        public Int64 Symbol_PK
        {
            get { return _symbol_PK; }
            set { _symbol_PK = value; }
        }

        private string _reutersSymbol = string.Empty;

        public string ReutersSymbol
        {
            get { return _reutersSymbol; }
            set { _reutersSymbol = value; }
        }

        private string _bloombergSymbol = string.Empty;

        public string BloombergSymbol
        {
            get { return _bloombergSymbol; }
            set { _bloombergSymbol = value; }
        }

        private string _bloombergSymbolWithExchangeCode = string.Empty;

        public string BloombergSymbolWithExchangeCode
        {
            get { return _bloombergSymbolWithExchangeCode; }
            set { _bloombergSymbolWithExchangeCode = value; }
        }
        private string _isinSymbol = string.Empty;

        public string ISINSymbol
        {
            get { return _isinSymbol; }
            set { _isinSymbol = value; }
        }
        private string _sedolSymbol = string.Empty;

        public string SedolSymbol
        {
            get { return _sedolSymbol; }
            set { _sedolSymbol = value; }
        }

        private string _osiOptionSymbol = string.Empty;

        public string OSIOptionSymbol
        {
            get { return _osiOptionSymbol; }
            set { _osiOptionSymbol = value; }
        }

        private string _idcoOptionSymbol = string.Empty;

        public string IDCOOptionSymbol
        {
            get { return _idcoOptionSymbol; }
            set { _idcoOptionSymbol = value; }
        }

        private string _opraSymbol = string.Empty;

        public string OpraSymbol
        {
            get { return _opraSymbol; }
            set { _opraSymbol = value; }
        }

        private string _cusipSymbol = string.Empty;

        public string CusipSymbol
        {
            get { return _cusipSymbol; }
            set { _cusipSymbol = value; }
        }

        private string _proxySymbol = string.Empty;

        public string ProxySymbol
        {
            get { return _proxySymbol; }
            set { _proxySymbol = value; }
        }
        private int _symbolType;
        public int SymbolType
        {
            get { return _symbolType; }
            set { _symbolType = value; }
        }

        public double Multiplier
        {
            get { return _multiplier; }
            set { _multiplier = value; }
        }
        protected float _delta = 1;

        public float Delta
        {
            get { return _delta; }
            set { _delta = value; }
        }

        public decimal RoundLot
        {
            get { return _roundLot; }
            set { _roundLot = value; }
        }


        #region Security Approval Status fields
        Boolean _isSecApproved = false;
        public Boolean IsSecApproved
        {
            get
            {
                return _isSecApproved;
            }
            set
            {
                _isSecApproved = value;
            }

        }

        private string _approvedBy = String.Empty;

        public string ApprovedBy
        {
            get { return _approvedBy; }
            set { _approvedBy = value; }
        }

        private DateTime _approvalDate = DateTimeConstants.MinValue;
        public DateTime ApprovalDate
        {
            get { return _approvalDate; }
            set { _approvalDate = value; }
        }

        private string _comments = String.Empty;
        public string Comments
        {
            get { return _comments; }
            set { _comments = value; }
        }

        //Added by Bhavana on July 2014
        // Purpose : To set SourceOfDataID for saving datasource in DB
        private int _sourceOfDataID = (int)SecMasterConstants.SecMasterSourceOfData.None;
        public int SourceOfDataID
        {
            get { return _sourceOfDataID; }
            set { _sourceOfDataID = value; }
        }

        private SecMasterConstants.SecMasterSourceOfData _sourceOfData = SecMasterConstants.SecMasterSourceOfData.None;
        public SecMasterConstants.SecMasterSourceOfData SourceOfData
        {
            get { return _sourceOfData; }
            set { _sourceOfData = value; }
        }



        #endregion

        #region UDA Symbol Data Object
        //UDA data object  property for UDA informations

        UDAData _symbolUDAData = null;
        public UDAData SymbolUDAData
        {
            get
            {
                return _symbolUDAData;
            }
            set
            {
                _symbolUDAData = value;
            }

        }

        #endregion



        private DateTime _creationDate = DateTimeConstants.MinValue;
        public DateTime CreationDate
        {
            get { return _creationDate; }
            set { _creationDate = value; }
        }

        private DateTime _modifiedDate = DateTimeConstants.MinValue;
        public DateTime ModifiedDate
        {
            get { return _modifiedDate; }
            set { _modifiedDate = value; }
        }


        private String _createdBy = String.Empty;
        public String CreatedBy
        {
            get { return _createdBy; }
            set { _createdBy = value; }
        }

        private String _modifiedBy = String.Empty;
        public String ModifiedBy
        {
            get { return _modifiedBy; }
            set { _modifiedBy = value; }
        }

        private String _BBGID = String.Empty;
        public String BBGID
        {
            get { return _BBGID; }
            set { _BBGID = value; }
        }

        private int _primarySymbology;
        public int PrimarySymbology
        {
            get { return _primarySymbology; }
            set { _primarySymbology = value; }
        }

        private double _SharesOutstanding;
        public double SharesOutstanding
        {
            get { return _SharesOutstanding; }
            set { _SharesOutstanding = value; }
        }

        private String _errorMessage = String.Empty;
        public String ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; }
        }
        //  cehck for setting default UDA from underlyig symbol or root symbol or not
        private bool _useUDAFromUnderlyingOrRoot = false;
        public bool UseUDAFromUnderlyingOrRoot
        {
            get { return _useUDAFromUnderlyingOrRoot; }
            set { _useUDAFromUnderlyingOrRoot = value; }
        }

        private double _strikePriceMultiplier = 1;
        public double StrikePriceMultiplier
        {
            get { return _strikePriceMultiplier; }
            set { _strikePriceMultiplier = value; }
        }

        private string _esignalOptionRoot = string.Empty;
        public string EsignalOptionRoot
        {
            get { return _esignalOptionRoot; }
            set { _esignalOptionRoot = value; }
        }

        private string _bloombergOptionRoot = string.Empty;
        public string BloombergOptionRoot
        {
            get { return _bloombergOptionRoot; }
            set { _bloombergOptionRoot = value; }
        }

        private string _requestedUserID = string.Empty;
        public string RequestedUserID
        {
            get { return _requestedUserID; }
            set { _requestedUserID = value; }
        }

        private string _requestedHashcode = string.Empty;
        public string RequestedHashcode
        {
            get { return _requestedHashcode; }
            set { _requestedHashcode = value; }
        }

        public object Clone()
        {
            return Prana.Global.Utilities.DeepCopyHelper.Clone(this);
        }

        public void AddData(string symbol, ApplicationConstants.SymbologyCodes symbology)
        {

            _symbologyMapping[(int)symbology] = symbol;

        }
    }
}
