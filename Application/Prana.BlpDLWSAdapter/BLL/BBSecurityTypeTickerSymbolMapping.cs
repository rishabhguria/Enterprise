using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.BusinessObjects;
using Prana.Global;
using Prana.Utilities.ImportExportUtilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prana.BlpDLWSAdapter.BLL
{
    internal sealed class BBSecurityTypeTickerSymbolMapping
    {
        #region singleton
        private static volatile BBSecurityTypeTickerSymbolMapping instance;
        private static object syncRoot = new Object();

        private BBSecurityTypeTickerSymbolMapping() { }

        public static BBSecurityTypeTickerSymbolMapping Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new BBSecurityTypeTickerSymbolMapping();
                            instance.FillSecurityTypeSymbolMapping();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion singleton

        Dictionary<string, string> _securityTypeTickerMapping = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        private void FillSecurityTypeSymbolMapping()
        {
            try
            {
                string exchangeMappingFilePath = AppDomain.CurrentDomain.BaseDirectory + @"xmls";
                DataTable dtSource = FileReaderFactory.GetDataTableFromDifferentFileFormats(exchangeMappingFilePath + @"\SecurityTypeTickerSymbolMapping.csv");
                foreach (DataRow dr in dtSource.Rows)
                {
                    if (string.Compare(dr["COL1"].ToString(), "SecurityType", StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        continue;
                    }
                    if (!_securityTypeTickerMapping.ContainsKey(dr["COL1"].ToString().Trim(' ', '"', '\'')))
                    {
                        _securityTypeTickerMapping.Add(dr["COL1"].ToString().Trim(' ', '"', '\''), dr["COL2"].ToString().Trim(' ', '"', '\''));
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Returns the BB ticker for the requested symbol according to format specified for our system in the SecurityTypeTickerSymbolMapping. 
        /// Replaces 1 with ticker , 2 with exchange code, 3 with market sector 
        /// </summary>
        /// <param name="securityType">Security type of the bloomberg security</param>
        /// <param name="ID_BB_SEC_NUM_DES">ticker as returned from BB</param>
        /// <param name="EXCH_CODE">exchange code as returned from BB</param>
        /// <param name="MARKET_SECTOR_DES">sector as returned from BB</param>
        /// <returns>complete BB ticker used in Nirvana</returns>
        public string GetBloombergSymbolAccordingToAssetAndSecurityType(string securityType, string ID_BB_SEC_NUM_DES, string EXCH_CODE, string MARKET_SECTOR_DES)
        {
            string symbol = "";
            try
            {
                if (_securityTypeTickerMapping.ContainsKey(securityType))
                {
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < _securityTypeTickerMapping[securityType].Length; i++)
                    {
                        switch (_securityTypeTickerMapping[securityType][i])
                        {
                            case '1': sb.Append(" " + ID_BB_SEC_NUM_DES);
                                break;
                            case '2': sb.Append(" " + EXCH_CODE);
                                break;
                            case '3': sb.Append(" " + MARKET_SECTOR_DES);
                                break;
                        }
                    }
                    symbol = sb.Replace("  ", " \t").Replace("\t ", "").Replace("\t", "").ToString().Trim().ToUpper();
                }
                else
                {
                    symbol = string.Join(" ", new string[3] { ID_BB_SEC_NUM_DES, EXCH_CODE, MARKET_SECTOR_DES });
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return symbol.ToUpper();
        }


    }
}
