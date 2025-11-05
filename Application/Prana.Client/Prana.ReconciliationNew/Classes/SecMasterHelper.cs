using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;


namespace Prana.ReconciliationNew
{
    public static class SecMasterHelper
    {
        static ISecurityMasterServices _securityMaster = null;
        public static ISecurityMasterServices SecurityMaster
        {
            set
            {
                _securityMaster = value;
            }
            get
            {
                return _securityMaster;
            }
        }

        public static DataTable GetSMData(Dictionary<int, List<string>> dictUniqueSymbols, DataTable dt, int hashCode)
        {
            try
            {
                if (dictUniqueSymbols != null && dt != null)
                {
                    if (dictUniqueSymbols.Count > 0)
                    {
                        SecMasterRequestObj secMasterRequestObj = new SecMasterRequestObj();

                        foreach (KeyValuePair<int, List<string>> kvp in dictUniqueSymbols)
                        {
                            ApplicationConstants.SymbologyCodes symbology = (ApplicationConstants.SymbologyCodes)kvp.Key;

                            List<string> symbolList = dictUniqueSymbols[kvp.Key];

                            if (symbolList.Count > 0)
                            {
                                foreach (string symbol in symbolList)
                                {
                                    secMasterRequestObj.AddData(symbol, symbology);

                                    //  secMasterRequestObj.AddNewRow();
                                }
                            }
                        }

                        secMasterRequestObj.IsSearchInLocalOnly = !CachedDataManager.GetInstance.IsMarketDataPermissionEnabled;
                        secMasterRequestObj.HashCode = hashCode;
                        List<SecMasterBaseObj> secMasterCollection = _securityMaster.SendRequestList(secMasterRequestObj);

                        if (secMasterCollection != null && secMasterCollection.Count > 0)
                        {
                            foreach (SecMasterBaseObj secMasterObj in secMasterCollection)
                            {
                                string pranaSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.PranaSymbology].ToString();

                                int requestedSymbologyID = secMasterObj.RequestedSymbology;
                                string requestedSymbology = "Symbol";
                                switch (requestedSymbologyID)
                                {
                                    case 0:
                                        requestedSymbology = "Symbol";
                                        break;
                                    case 1:
                                        requestedSymbology = "RIC";
                                        break;
                                    case 2:
                                        requestedSymbology = "ISIN";
                                        break;
                                    case 3:
                                        requestedSymbology = "SEDOL";
                                        break;
                                    case 4:
                                        requestedSymbology = "CUSIP";
                                        break;
                                    case 5:
                                        requestedSymbology = "Bloomberg";
                                        break;
                                    case 6:
                                        requestedSymbology = "OSIOptionSymbol";
                                        break;
                                    case 7:
                                        requestedSymbology = "IDCOOptionSymbol";
                                        break;
                                    case 8:
                                        requestedSymbology = "OPRAOptionSymbol";
                                        break;

                                    default:
                                        break;
                                }

                                if (dictUniqueSymbols.ContainsKey(requestedSymbologyID))
                                {
                                    List<string> symbolList = dictUniqueSymbols[requestedSymbologyID];
                                    if (symbolList.Contains(secMasterObj.RequestedSymbol))
                                    {
                                        DataRow[] rows = dt.Select(requestedSymbology + " = '" + secMasterObj.RequestedSymbol + "'");
                                        foreach (DataRow dataRow in rows)
                                        {
                                            dataRow["Symbol"] = pranaSymbol;
                                            // dataRow["CompanyName"] = secMasterObj.LongName;
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
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return dt;
        }
    }
}
