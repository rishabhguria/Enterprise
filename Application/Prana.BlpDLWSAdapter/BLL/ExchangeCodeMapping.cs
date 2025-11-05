using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.BusinessObjects.AppConstants;
using Prana.Global;
using Prana.Utilities.ImportExportUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prana.BlpDLWSAdapter.BusinessObject.Mappings
{
    public sealed class ExchangeCodeMapping
    {
        private static volatile ExchangeCodeMapping instance;
        private static object syncRoot = new Object();

        private ExchangeCodeMapping() { }

        public static ExchangeCodeMapping Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new ExchangeCodeMapping();
                            instance.FillExchangeMappingDictionary();
                            instance.FillDefaultExchangeMappingDictionary();
                        }
                    }
                }
                return instance;
            }
        }

        Dictionary<string, string> _bbExchangeToEsignalCode = new Dictionary<string, string>();
        Dictionary<AssetCategory, string> _bbDefaultAssetExchangeMapping = new Dictionary<AssetCategory, string>();

        public void ReloadCache()
        {
            try
            {
                lock (_bbDefaultAssetExchangeMapping)
                {
                    lock (_bbExchangeToEsignalCode)
                    {
                        _bbDefaultAssetExchangeMapping.Clear();
                        _bbExchangeToEsignalCode.Clear();
                        FillExchangeMappingDictionary();
                        FillDefaultExchangeMappingDictionary();
                    }
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
        }

        void FillDefaultExchangeMappingDictionary()
        {
            try
            {
                string exchangeMappingFilePath = AppDomain.CurrentDomain.BaseDirectory + @"\xmls";
                DataTable dataSource = FileReaderFactory.GetDataTableFromDifferentFileFormats(exchangeMappingFilePath + @"\DefaultExchangeForAssets.csv");
                foreach (DataRow dr in dataSource.Rows)
                {
                    if (string.Compare(dr["COL1"].ToString(), "Asset", true) == 0)
                    {
                        continue;
                    }
                    AssetCategory MyStatus;
                    if (Enum.TryParse<AssetCategory>(dr["COL1"].ToString().Trim(' ', '"', '\''), true, out MyStatus))
                    {
                        if (!_bbDefaultAssetExchangeMapping.ContainsKey(MyStatus))
                        {
                            _bbDefaultAssetExchangeMapping.Add(MyStatus, dr["COL2"].ToString().Trim(' ', '"', '\''));
                        }
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

        void FillExchangeMappingDictionary()
        {
            try
            {
                string exchangeMappingFilePath = AppDomain.CurrentDomain.BaseDirectory + @"xmls";
                DataTable dataSource = FileReaderFactory.GetDataTableFromDifferentFileFormats(exchangeMappingFilePath + @"\BloombergNirvanaExchangeMapping.csv");
                foreach (DataRow dr in dataSource.Rows)
                {
                    if (string.Compare(dr["COL1"].ToString(),"BloombergExchangeCode",true)==0)
                    {
                    continue;
                    }
                    if (!_bbExchangeToEsignalCode.ContainsKey(dr["COL1"].ToString().Trim(' ', '"', '\'')))
                    {
                        _bbExchangeToEsignalCode.Add(dr["COL1"].ToString().Trim(' ', '"', '\''), dr["COL2"].ToString().Trim(' ', '"', '\''));
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

        public string GetExchangeCodeForBBExchangeCode(string BBExchCode)
        {
            string exchCode;
            if (_bbExchangeToEsignalCode.TryGetValue(BBExchCode, out exchCode))
                return exchCode;
            else
                return "";
        }

        public string GetDefaultExchangeCodeForAsset(AssetCategory asset)
        {
            if (_bbDefaultAssetExchangeMapping.ContainsKey(asset))
                return _bbDefaultAssetExchangeMapping[asset];
            else
                return "";
        }

    }
}
