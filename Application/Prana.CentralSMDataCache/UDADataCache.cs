using System;
using System.Collections.Generic;
using System.Text;

using Prana.BusinessObjects;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.CentralSMDataCache.DAL;
using System.Linq;
namespace Prana.CentralSMDataCache
{
    internal class UDADataCache
    {
        private static object _lockerObject = new object();
        static UDADataCache _UDADataCache = null;
        public static UDADataCache GetInstance
        {
            get
            {

                lock (_lockerObject)
                {
                    if (_UDADataCache == null)
                    {
                        _UDADataCache = new UDADataCache();
                    }
                    return _UDADataCache;
                }
            }
        }

        #region UDA DATA Dictiories

        static Dictionary<int, string> _UDAAssets = new Dictionary<int, string>();
        static Dictionary<int, string> _UDASectors = new Dictionary<int, string>();
        static Dictionary<int, string> _UDASubSectors = new Dictionary<int, string>();
        static Dictionary<int, string> _UDASecurityTypes = new Dictionary<int, string>();
        static Dictionary<int, string> _UDACountries = new Dictionary<int, string>();

        static Dictionary<String, Dictionary<int, string>> _allUDAAttributesDict = null;

        /// <summary>
        /// All uda attributes cached data
        /// </summary>
        public Dictionary<String, Dictionary<int, string>> AllUDAAttributesDict
        {
            get { return _allUDAAttributesDict; }
            set { _allUDAAttributesDict = value; }
        }


        public Dictionary<int, string> UDAAssets
        {
            get { return _UDAAssets; }
        }

        public Dictionary<int, string> UDASectors
        {
            get { return _UDASectors; }
        }
        public Dictionary<int, string> UDASubSectors
        {
            get { return _UDASubSectors; }
        }
        public Dictionary<int, string> UDASecurityTypes
        {
            get { return _UDASecurityTypes; }
        }
        public Dictionary<int, string> UDACountries
        {
            get { return _UDACountries; }
        }


        #endregion

        #region GetAll_UDAAssets_UDASectors_UDA SubSectors_UDASecurityTypes_UDACountries
        public Dictionary<int, string> GetAllUDAAssets()
        {
            Dictionary<int, string> dictUDAAssets = UDAAssets;
            return dictUDAAssets;
        }
        public Dictionary<int, string> GetAllUDASectors()
        {
            Dictionary<int, string> dictUDASectors = UDASectors;
            return dictUDASectors;
        }
        public Dictionary<int, string> GetAllUDASubSectors()
        {
            Dictionary<int, string> dictUDASubSectors = UDASubSectors;
            return dictUDASubSectors;
        }
        public Dictionary<int, string> GetAllUDASecurityTypes()
        {
            Dictionary<int, string> dictUDASecurityTypes = UDASecurityTypes;
            return dictUDASecurityTypes;
        }

        public Dictionary<int, string> GetAllUDACountries()
        {
            Dictionary<int, string> dictUDACountries = UDACountries;
            return dictUDACountries;
        }

        #endregion

        public void SetCommonUDAData()
        {
            try
            {
                _UDAAssets = UDADataManager.GetUDAAttributeData(SecMasterConstants.CONST_GET_UDAASSET_SP); //P_UDAGetAllAssets
                _UDASectors = UDADataManager.GetUDAAttributeData(SecMasterConstants.CONST_GET_UDASectors_SP); //P_UDAGetAllSectors
                _UDASubSectors = UDADataManager.GetUDAAttributeData(SecMasterConstants.CONST_GET_UDASubSectors_SP); //P_UDAGetAllSubSector
                _UDASecurityTypes = UDADataManager.GetUDAAttributeData(SecMasterConstants.CONST_GET_SecurityTypes_SP); //P_UDAGetAllSecurityType
                _UDACountries = UDADataManager.GetUDAAttributeData(SecMasterConstants.CONST_GET_UDACountries_SP); //P_UDAGetAllCountry

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
        /// Get In Used Uda Ids
        /// Created by: omshiv, nov,2013
        /// </summary>
        internal Dictionary<string, Dictionary<int, string>> GetInUseUDAsIDList()
        {
            Dictionary<string, Dictionary<int, string>> inUsedUDAIdsDict = new Dictionary<string, Dictionary<int, string>>();

            try
            {
                Dictionary<int, string> UDAAssetIDsInUse = UDADataManager.GetInUseUDAIDs(SecMasterConstants.CONST_INUSED_UDAASSET_SP);
                Dictionary<int, string> UDASectorIDsInUse = UDADataManager.GetInUseUDAIDs(SecMasterConstants.CONST_INUSED_UDASectors_SP);
                Dictionary<int, string> UDASubSectorIDsInUse = UDADataManager.GetInUseUDAIDs(SecMasterConstants.CONST_INUSED_UDASubSectors_SP);
                Dictionary<int, string> UDASecTypeIDsInUse = UDADataManager.GetInUseUDAIDs(SecMasterConstants.CONST_INUSED_UDASecurityTypes_SP);
                Dictionary<int, string> UDACountryIDsInUse = UDADataManager.GetInUseUDAIDs(SecMasterConstants.CONST_INUSED_UDACountries_SP);

                inUsedUDAIdsDict.Add(SecMasterConstants.CONST_UDAAsset, UDAAssetIDsInUse);
                inUsedUDAIdsDict.Add(SecMasterConstants.CONST_UDASecurityType, UDASecTypeIDsInUse);
                inUsedUDAIdsDict.Add(SecMasterConstants.CONST_UDASector, UDASectorIDsInUse);
                inUsedUDAIdsDict.Add(SecMasterConstants.CONST_UDASubSector, UDASubSectorIDsInUse);
                inUsedUDAIdsDict.Add(SecMasterConstants.CONST_UDACountry, UDACountryIDsInUse);
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
            return inUsedUDAIdsDict;

        }

        /// <summary>
        /// Get UDA text by ID of particular UDA type like UDASector, UDA Subsector 
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="UDAType"></param>
        /// <returns></returns>
        public string GetUDATextFromID(int ID, String UDAType)
        {
            String attributeValue = String.Empty;
            try
            {

                Dictionary<int, string> dt = new Dictionary<int, string>();
                switch (UDAType)
                {
                    case SecMasterConstants.CONST_UDAAsset:
                        dt = UDAAssets;
                        break;

                    case SecMasterConstants.CONST_UDASector:
                        dt = UDASectors;
                        break;

                    case SecMasterConstants.CONST_UDASubSector:
                        dt = UDASubSectors;
                        break;

                    case SecMasterConstants.CONST_UDASecurityType:
                        dt = UDASecurityTypes;
                        break;

                    case SecMasterConstants.CONST_UDACountry:
                        dt = UDACountries;
                        break;


                }

                if (dt.ContainsKey(ID))
                {
                    attributeValue = dt[ID];
                }
                else
                {
                    attributeValue = "";
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
            return attributeValue;
        }


        /// <summary>
        /// Get UDA text by ID of particular UDA type like UDASector, UDA Subsector 
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="UDAType"></param>
        /// <returns></returns>
        public int GetUDAIDFromText(String UDAText, String UDAType)
        {
            String spName = String.Empty;
            try
            {

                Dictionary<int, string> dt = new Dictionary<int, string>();
                switch (UDAType)
                {
                    case SecMasterConstants.CONST_UDAAsset:
                        dt = UDAAssets;
                        spName = SecMasterConstants.CONST_INSERT_UDAASSET_SP;
                        break;

                    case SecMasterConstants.CONST_UDASector:
                        dt = UDASectors;
                        spName = SecMasterConstants.CONST_INSERT_UDASectors_SP;
                        break;

                    case SecMasterConstants.CONST_UDASubSector:
                        dt = UDASubSectors;
                        spName = SecMasterConstants.CONST_INSERT_UDASubSectors_SP;
                        break;

                    case SecMasterConstants.CONST_UDASecurityType:
                        dt = UDASecurityTypes;
                        spName = SecMasterConstants.CONST_INSERT_UDASecurityTypes_SP;
                        break;

                    case SecMasterConstants.CONST_UDACountry:
                        dt = UDACountries;
                        spName = SecMasterConstants.CONST_INSERT_UDACountries_SP;
                        break;


                }
               
                Boolean isFound = false;
                foreach (int udaId in dt.Keys)
                {
                    if (dt[udaId].ToString().Trim().Equals(UDAText.Trim(), StringComparison.OrdinalIgnoreCase))
                    {
                        isFound = true;
                        return udaId;
                    }
                }
                 var max = dt.Max(kvp => kvp.Key);
                if (!isFound)
                { 
                    int newUDAID= Convert.ToInt32(max+1);

                    //Adding new UDA in cache
                    if (!dt.ContainsKey(newUDAID))
                    {
                        dt.Add(newUDAID, UDAText);
                    }

                    //Saving new UDA in DB
                    UDACollection udaCollection = new UDACollection();
                    udaCollection.Add(new UDA{ID=newUDAID, Name= UDAText});
                    UDADataManager.SaveInformation(spName,udaCollection);

                   

                    return newUDAID;

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
                return 0;
            }
            return 0;
        }


        internal void AddUDAinCache(string UDAType, UDACollection udaCol)
        {
            try
            {
                SecMasterConstants.UDATypes udaTypesEnum = (SecMasterConstants.UDATypes)Enum.Parse(typeof(SecMasterConstants.UDATypes), UDAType);
                switch (udaTypesEnum)
                {
                    case SecMasterConstants.UDATypes.AssetClass:
                        AddUDAIds(_UDAAssets, udaCol);
                        break;
                    case SecMasterConstants.UDATypes.Country:
                        AddUDAIds(_UDACountries, udaCol);
                        break;
                    case SecMasterConstants.UDATypes.Sector:
                        AddUDAIds(_UDASectors, udaCol);
                        break;
                    case SecMasterConstants.UDATypes.SubSector:
                        AddUDAIds(_UDASubSectors, udaCol);
                        break;
                    case SecMasterConstants.UDATypes.SecurityType:
                        AddUDAIds(_UDASecurityTypes, udaCol);
                        break;

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

        private void AddUDAIds(Dictionary<int, string> cachedUDADict, UDACollection udaCol)
        {
            try
            {
                foreach (UDA udaItem in udaCol)
                {
                    if (!cachedUDADict.ContainsKey(udaItem.ID))
                    {
                        cachedUDADict.Add(udaItem.ID, udaItem.Name);
                    }
                    else
                    {
                        cachedUDADict[udaItem.ID] = udaItem.Name;
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



        internal void RemoveDeletedUDAFrmCache(string UDAType, List<int> udaIdsToDelete)
        {
            try
            {
                SecMasterConstants.UDATypes udaTypesEnum = (SecMasterConstants.UDATypes)Enum.Parse(typeof(SecMasterConstants.UDATypes), UDAType);
                switch (udaTypesEnum)
                {

                    case SecMasterConstants.UDATypes.AssetClass:
                        RemoveUDAIds(_UDAAssets, udaIdsToDelete);
                        break;
                    case SecMasterConstants.UDATypes.Country:
                        RemoveUDAIds(_UDACountries, udaIdsToDelete);
                        break;
                    case SecMasterConstants.UDATypes.Sector:
                        RemoveUDAIds(_UDASectors, udaIdsToDelete);
                        break;
                    case SecMasterConstants.UDATypes.SubSector:
                        RemoveUDAIds(_UDASubSectors, udaIdsToDelete);
                        break;
                    case SecMasterConstants.UDATypes.SecurityType:
                        RemoveUDAIds(_UDASecurityTypes, udaIdsToDelete);
                        break;

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

        private void RemoveUDAIds(Dictionary<int, string> cachedUDADict, List<int> udaIdsToDelete)
        {
            try
            {
                foreach (int udaItem in udaIdsToDelete)
                {
                    if (cachedUDADict.ContainsKey(udaItem))
                    {
                        cachedUDADict.Remove(udaItem);
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
    }
}
