using System;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for SymbolMapping.
    /// </summary>
    public class SymbolMapping
    {
        #region Private members

        private int _cvSymbolMappingID = int.MinValue;
        private int _cvAuecID = int.MinValue;
        private string _symbol = string.Empty;
        private string _mappedSymbol = string.Empty;
        private int _counterPartyVenueID = int.MinValue;

        private int _tempSymbolID = int.MinValue;
        private string _auec = string.Empty;
        private string _symbolName = string.Empty;

        int _auecID = int.MinValue;
        #endregion

        #region Constructors
        public SymbolMapping()
        {
        }
        //		public SymbolMapping(int symbolID, string symbolName)
        //		{
        //			_symbolID = symbolID;
        //			_symbolName = symbolName;	
        //		}

        public SymbolMapping(int tempSymbolID, string symbolName)
        {
            _tempSymbolID = tempSymbolID;
            _symbolName = symbolName;
        }

        public SymbolMapping(int cvSymbolMappingID, int cvAuecID, string symbol, string mappedSymbol, int counterPartyVenueID, string AUEC, string symbolName)
        {
            _cvSymbolMappingID = cvSymbolMappingID;
            _cvAuecID = cvAuecID;
            _symbol = symbol;
            _mappedSymbol = mappedSymbol;
            _counterPartyVenueID = counterPartyVenueID;
            _auec = AUEC;
            //_symbolName = symbolName;
        }
        #endregion

        #region Properties

        public int CVSymboMappingID
        {
            get { return _cvSymbolMappingID; }
            set { _cvSymbolMappingID = value; }
        }

        public int CVAUECID
        {
            get { return _cvAuecID; }
            set { _cvAuecID = value; }
        }

        public String Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }

        public string MappedSymbol
        {
            get { return _mappedSymbol; }
            set { _mappedSymbol = value; }
        }

        public int CounterPartyVenueID
        {
            get { return _counterPartyVenueID; }
            set { _counterPartyVenueID = value; }
        }

        public int AUECID
        {
            get
            {
                CounterPartyVenue cpv = new CounterPartyVenue();
                if (_cvAuecID != int.MinValue)
                {
                    cpv = CounterPartyManager.GetCVAUEC(_cvAuecID);
                }
                _auecID = cpv.AUECID;
                return _auecID;
            }
            set { _auecID = value; }
        }

        public string AUEC
        {
            get
            {
                //AUEC auec = AUECManager.GetAUEC(_cvAuecID);
                AUEC auec = AUECManager.GetAUEC(_auecID);
                if (auec == null)
                {
                    return "";
                }
                else
                {
                    //		return(auec.Asset.Name + ":" + auec.UnderLying.Name + ":" + auec.Exchange.DisplayName);

                    //SK 20061009 Compliance is removed from AUEC
                    //Currency currency = new Currency();
                    //currency = AUECManager.GetCurrency(auec.Currency);
                    //SK

                    //return(auec.Asset.Name + ":" + auec.UnderLying.Name + ":" + auec.Exchange.DisplayName + ":" + auec.Exchange.Currency);
                    //return(auec.Asset.Name + ":" + auec.UnderLying.Name + ":" + auec.Exchange.DisplayName + ":" + currency.CurrencySymbol.ToString());
                    _auec = auec.Asset.Name + ":" + auec.UnderLying.Name + ":" + auec.DisplayName + ":" + auec.Currency.CurrencySymbol.ToString();
                    return _auec;
                }
            }
            set { _auec = value; }
        }

        //		public string SymbolName
        //		{
        //			get
        //			{
        //				Prana.Admin.BLL.Symbol symbol = SymbolManager.GetSymbol(_symbolID);
        //				if(symbol == null)
        //				{
        //					return "";
        //				}
        //				else
        //				{
        //					return symbol.CompanySymbol;
        //				}
        //			}
        //			set{_symbolName = value;}
        //		}
        #endregion
    }
}
