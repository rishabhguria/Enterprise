using Prana.Utilities.ImportExportUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prana.BlpDLWSAdapter;
using Prana.BusinessObjects;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;
using Prana.BusinessObjects.AppConstants;
using System.Collections.Concurrent;

namespace Prana.BlpDLWSAdapter.BusinessObject.Mappings
{
    public sealed class BloombergSecurityTypeMapping
    {
        private static volatile BloombergSecurityTypeMapping instance;
        private static object syncRoot = new Object();

        private BloombergSecurityTypeMapping() { }

        public static BloombergSecurityTypeMapping Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new BloombergSecurityTypeMapping();
                            instance.FillSecurityToSymbolDataMappingDictionary();
                        }
                    }
                }
                return instance;
            }
        }
        ConcurrentDictionary<string, AssetCategory> _secTypeSymbolDataInfoMapping = new ConcurrentDictionary<string, AssetCategory>();

        public ConcurrentDictionary<string, AssetCategory> SecTypeSymbolDataInfoMapping
        {
            get { return _secTypeSymbolDataInfoMapping; }
            //set { _secTypeSymbolDataInfoMapping = value; }
        }

        public void ReloadCache()
        {
            try
            {
                    lock (_secTypeSymbolDataInfoMapping)
                    {
                        _secTypeSymbolDataInfoMapping.Clear();
                        FillSecurityToSymbolDataMappingDictionary();
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

        private void FillSecurityToSymbolDataMappingDictionary()
        {
            try
            {
                string exchangeMappingFilePath = AppDomain.CurrentDomain.BaseDirectory + @"xmls";
                DataTable dtSource = FileReaderFactory.GetDataTableFromDifferentFileFormats(exchangeMappingFilePath + @"\BloombergSecurityTypeToAssetMapping.csv");
                foreach (DataRow dr in dtSource.Rows)
                {
                    if (string.Compare(dr["COL1"].ToString(), "SecurityType", true) == 0)
                    {
                        continue;
                    }
                    if (!_secTypeSymbolDataInfoMapping.ContainsKey(dr["COL1"].ToString().Trim(' ', '"', '\'')))
                    {
                        Prana.BusinessObjects.AppConstants.AssetCategory asset = BusinessObjects.AppConstants.AssetCategory.None;
                        Enum.TryParse<Prana.BusinessObjects.AppConstants.AssetCategory>(dr["COL2"].ToString().Trim(' ', '"', '\''), out asset);
                            _secTypeSymbolDataInfoMapping.TryAdd(dr["COL1"].ToString().Trim(' ', '"', '\''), asset);
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
    }
}
