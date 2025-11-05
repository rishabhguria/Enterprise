using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.PostTradeServices
{
    class SMHelper
    {
        private static Dictionary<string, SMData> _dictSymbolAUECID = new Dictionary<string, SMData>();

        public static void CreateDictionary()
        {
            try
            {
                _dictSymbolAUECID.Clear();
                DataTable dt = DataBaseManager.GetCurrentSymbolSMData();
                foreach (DataRow row in dt.Rows)
                {
                    SMData smData = new SMData();
                    smData.Symbol = row["TickerSymbol"].ToString();
                    smData.AUECID = Int32.Parse(row["AUECID"].ToString());
                    smData.AssetID = Int32.Parse(row["AssetID"].ToString());
                    smData.CurrencyID = Int32.Parse(row["CurrencyID"].ToString());
                    smData.Multiplier = Double.Parse(row["Multiplier"].ToString());

                    if (!_dictSymbolAUECID.ContainsKey(smData.Symbol))
                    {
                        _dictSymbolAUECID.Add(smData.Symbol, smData);
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

        public static int GetAUECIDForTickerSymbol(string symbol)
        {
            if (_dictSymbolAUECID.ContainsKey(symbol))
            {
                return _dictSymbolAUECID[symbol].AUECID;
            }
            else
            {
                return int.MinValue;
            }
        }
        public static int GetAssetIDForTickerSymbol(string symbol)
        {
            if (_dictSymbolAUECID.ContainsKey(symbol))
            {
                return _dictSymbolAUECID[symbol].AssetID;
            }
            else
            {
                return int.MinValue;
            }
        }
        //public static int GetCurrencyIDForTickerSymbol(string symbol)
        //{
        //    if (_dictSymbolAUECID.ContainsKey(symbol))
        //    {
        //        return _dictSymbolAUECID[symbol].CurrencyID;
        //    }
        //    else
        //    {
        //        return 1;
        //    }
        //}
        public static double GetMultiplierForTickerSymbol(string symbol)
        {
            if (_dictSymbolAUECID.ContainsKey(symbol))
            {
                return _dictSymbolAUECID[symbol].Multiplier;
            }
            else
            {
                return 1;
            }
        }
        public static SMData GetSMDataForTickerSymbol(string symbol)
        {
            if (_dictSymbolAUECID.ContainsKey(symbol))
            {
                return _dictSymbolAUECID[symbol];
            }
            else
            {
                return new SMData();
            }
        }
    }

    class SMData
    {
        private string _symbol = String.Empty;
        private int _auecID = int.MinValue;
        private int _assetID = int.MinValue;
        private int _currencyID = 1;
        private double _multiplier = 1;

        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }
        public int AUECID
        {
            get { return _auecID; }
            set { _auecID = value; }
        }
        public int AssetID
        {
            get { return _assetID; }
            set { _assetID = value; }
        }
        public int CurrencyID
        {
            get { return _currencyID; }
            set { _currencyID = value; }
        }
        public double Multiplier
        {
            get { return _multiplier; }
            set { _multiplier = value; }
        }
    }
}
