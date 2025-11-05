using System;
using System.Collections.Generic;
using System.Text;
using Prana.Global;

namespace Prana.SecurityMaster
{
    public class SecurityIdentifierCache
    {
        static SecurityIdentifierCache _securityIdentifierCache = null;



        private SecurityIdentifierCache()
        {
            MapExchangeSymbolCodesFromRICToPranaForEquity();
            MapExchangeSymbolCodesPranaToRICForEquity();
            MapExchangeSymbolCodesFromRICToPranaForFuture();
            MapFutureSymbolToExchangeFromRICToPrana();
            FillBlankExchanges();
        }

        public static SecurityIdentifierCache GetInstance()
        {
            if (_securityIdentifierCache == null)
            {
                _securityIdentifierCache = new SecurityIdentifierCache();
            }
            return _securityIdentifierCache;
        }

        /// <summary>
        /// 
        /// </summary>
        Dictionary<string, SecurityIdentifier> _pranaSymbolDict = new Dictionary<string, SecurityIdentifier>();

        public Dictionary<string, SecurityIdentifier> PranaSymbolDict
        {
            get
            {
                return _pranaSymbolDict;
            }
            set
            {
                _pranaSymbolDict = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        Dictionary<string, SecurityIdentifier> _rICSymbolDict = new Dictionary<string, SecurityIdentifier>();
        public Dictionary<string, SecurityIdentifier> RICSymbolDict
        {
            get
            {
                return _rICSymbolDict;
            }
            set
            {
                _rICSymbolDict = value;
            }
        }

        //                                                                   RIC Equity Code,Prana Exchange Code
        Dictionary<string, string> _exchangeRICSymbolCodeToPranaDict = new Dictionary<string, string>();


        public Dictionary<string, string> ExchangeRICSymbolCodeToPranaDict
        {
            get { return _exchangeRICSymbolCodeToPranaDict; }
            set { _exchangeRICSymbolCodeToPranaDict = value; }
        }

        /// <summary>
        ///the given collection contains the symbol mapping from RIC to Prana symbol convention For Equity
        /// </summary>
        private void MapExchangeSymbolCodesFromRICToPranaForEquity()
        {
            // load RIC Code from App.Config
            //System.Collections.Specialized.NameValueCollection RICExchangeMappingCode = new System.Collections.Specialized.NameValueCollection();
            //System.Collections.Specialized.NameValueCollection RICExchangeMappingCode = (System.Collections.Specialized.NameValueCollection)System.Configuration.ConfigurationManager.GetSection("RICExchangeMapping");
            System.Collections.Specialized.NameValueCollection RICExchangeMappingCode = ConfigurationHelper.Instance.LoadSectionBySectionName("RICExchangeMapping");
            if (RICExchangeMappingCode == null)
            {
                throw new Exception("Error in RIC to Prana symbol exchange mapping. Please check the configuration.");
            }
            
            for (int iIndex = 0, count = RICExchangeMappingCode.Count - 1; iIndex <= count; iIndex++)
            {
                string RIC = RICExchangeMappingCode.GetKey(iIndex);
                string RICValue = RICExchangeMappingCode[RIC];
                if (!_exchangeRICSymbolCodeToPranaDict.ContainsKey(RIC))
                {
                    _exchangeRICSymbolCodeToPranaDict.Add(RIC, RICValue);
                }
            }

        }

        List<string> _blankExchangesList = new List<string>();
        private void FillBlankExchanges()
        {
            //string strBlankExchangesForTickers = System.Configuration.ConfigurationManager.AppSettings["BlankExchangesForTickers"].ToString();
            string strBlankExchangesForTickers = ConfigurationHelper.Instance.GetAppSettingValueByKey("BlankExchangesForTickers").ToString();
            string[] splitString = strBlankExchangesForTickers.Split(',');
            for (int i = 0; i < splitString.Length; i++)
            {
                if (!_blankExchangesList.Contains(splitString[i]))
                {
                    _blankExchangesList.Add(splitString[i]);
                }
            }           

        }

        public List<string> BlankExchanges
        {
            get { return _blankExchangesList; }
            set { _blankExchangesList = value; }
        }

      
        //                                                         Prana Exchange Code, Other Exchange Code
        Dictionary<string, string> _exchangePranaToRICCodeDict = new Dictionary<string, string>();

        /// <summary>
        ///the collection contains the symbol mapping from Prana symbol convention to Other EMS 
        /// </summary>     
        public Dictionary<string ,string> ExchangePranaToRICCodeDict
        {
            get { return _exchangePranaToRICCodeDict; }
            set { _exchangePranaToRICCodeDict = value; }
        }
	

        /// <summary>
        ///the given collection contains the symbol mapping from Prana symbol convention to Other EMS 
        /// </summary>
        private void MapExchangeSymbolCodesPranaToRICForEquity()
        {
            // load Prana Symbol Convetion from App.Config
            //System.Collections.Specialized.NameValueCollection PranaExchangeMappingCode = new System.Collections.Specialized.NameValueCollection();
            //System.Collections.Specialized.NameValueCollection PranaExchangeMappingCode = (System.Collections.Specialized.NameValueCollection)System.Configuration.ConfigurationManager.GetSection("PranaExchangeMapping");
            System.Collections.Specialized.NameValueCollection PranaExchangeMappingCode = ConfigurationHelper.Instance.LoadSectionBySectionName("PranaExchangeMapping");
            if (PranaExchangeMappingCode == null)
            {
                throw new Exception("Error in RIC to Prana symbol exchange mapping. Please check the configuration.");
            }
            for (int iIndex = 0, count = PranaExchangeMappingCode.Count - 1; iIndex <= count; iIndex++)
            {
                string PranaKey = PranaExchangeMappingCode.GetKey(iIndex);
                string PranaValue = PranaExchangeMappingCode[PranaKey];
                if (!_exchangePranaToRICCodeDict.ContainsKey(PranaKey))
                {
                    _exchangePranaToRICCodeDict.Add(PranaKey, PranaValue);
                }
            }                      
        }


        //                                                             RIC Future Code,Prana Exchange Code
        Dictionary<string, string> _ricToPranaFutureRootSymbolDict = new Dictionary<string, string>();

        public Dictionary<string, string> RicToPranaFutureRootSymbolDict
        {
            get { return _ricToPranaFutureRootSymbolDict; }
            set { _ricToPranaFutureRootSymbolDict = value; }
        }

        Dictionary<string, string> _pranaToRICFutureRootSymbolDict = new Dictionary<string, string>();

        public Dictionary<string, string> PranaToRICFutureRootSymbolDict
        {
            get { return _pranaToRICFutureRootSymbolDict; }
            set { _pranaToRICFutureRootSymbolDict = value; }
        }

        /// <summary>
        ///the given collection contains the symbol mapping from RIC to Prana symbol convention for Futures
        /// </summary>
        private void MapExchangeSymbolCodesFromRICToPranaForFuture()
        {
            // load RIC Future Root Symbol from App.Config
            //System.Collections.Specialized.NameValueCollection RICFutureRootSymbolMapping = new System.Collections.Specialized.NameValueCollection();
            //System.Collections.Specialized.NameValueCollection RICFutureRootSymbolMapping = (System.Collections.Specialized.NameValueCollection)System.Configuration.ConfigurationManager.GetSection("RICFutureRootSymbolMapping");
            System.Collections.Specialized.NameValueCollection RICFutureRootSymbolMapping = ConfigurationHelper.Instance.LoadSectionBySectionName("RICFutureRootSymbolMapping");
            if (RICFutureRootSymbolMapping == null)
            {
                throw new Exception("Error in RIC to Prana symbol root mapping. Please check the configuration.");
            }
            for (int iIndex = 0, count = RICFutureRootSymbolMapping.Count - 1; iIndex <= count; iIndex++)
            {
                string ricFutureRoot = RICFutureRootSymbolMapping.GetKey(iIndex);
                string pranaFutureRoot = RICFutureRootSymbolMapping[ricFutureRoot];
                if (!_ricToPranaFutureRootSymbolDict.ContainsKey(ricFutureRoot))
                {
                    _ricToPranaFutureRootSymbolDict.Add(ricFutureRoot, pranaFutureRoot);
                    _pranaToRICFutureRootSymbolDict.Add(pranaFutureRoot, ricFutureRoot);
                }
            }
        }


        Dictionary<string, string> _RICFutureCodeToPranaExchange = new Dictionary<string, string>();

        public Dictionary<string, string> RICFutureCodeToPranaExchange
        {
            get { return _RICFutureCodeToPranaExchange; }
            set { _RICFutureCodeToPranaExchange = value; }
        }

        /// <summary>
        ///the given collection contains the symbol mapping from RIC to Prana exchange convention for Futures
        /// </summary>
        private void MapFutureSymbolToExchangeFromRICToPrana()
        {
            // load RIC Future and PranaExchanges from App.Config
            //System.Collections.Specialized.NameValueCollection RICFuturetoPranaExchange = new System.Collections.Specialized.NameValueCollection();
            //System.Collections.Specialized.NameValueCollection RICFuturetoPranaExchange = (System.Collections.Specialized.NameValueCollection)System.Configuration.ConfigurationManager.GetSection("RICFutureSymbolToExchange");
            System.Collections.Specialized.NameValueCollection RICFuturetoPranaExchange = ConfigurationHelper.Instance.LoadSectionBySectionName("RICFutureSymbolToExchange");

            if (RICFuturetoPranaExchange == null)
            {
                throw new Exception("Error while creating RIC to Prana symbol exchange mapping. Please check the configuration.");
            }

            for (int iIndex = 0, count = RICFuturetoPranaExchange.Count - 1; iIndex <= count; iIndex++)
            {
                string RICFutureCode = RICFuturetoPranaExchange.GetKey(iIndex);
                string PranaExchange = RICFuturetoPranaExchange[RICFutureCode];
                if (!_RICFutureCodeToPranaExchange.ContainsKey(RICFutureCode))
                {
                    _RICFutureCodeToPranaExchange.Add(RICFutureCode, PranaExchange);
                }
            }
        }




    }
}
