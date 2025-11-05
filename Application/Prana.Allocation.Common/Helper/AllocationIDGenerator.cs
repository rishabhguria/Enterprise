using Prana.LogManager;
using System;
using System.Configuration;

namespace Prana.Allocation.Common.Helper
{
    /// <summary>
    /// The AllocationIDGenerator class
    /// </summary>
    public class AllocationIDGenerator
    {
        /// <summary>
        /// current user Id
        /// </summary>
        private static string _currenUserID = "0";

        /// <summary>
        /// locker object for ClOrder
        /// </summary>
        private static readonly object thisLockClOrderID = new object();

        /// <summary>
        /// locker object for ExternalOrder
        /// </summary>
        private static readonly object thisLockExternalOrderID = new object();

        /// <summary>
        /// The group id
        /// </summary>
        private static Int64 _groupID = 0;

        /// <summary>
        /// Account Id
        /// </summary>
        private static int _tempAccountID = 0;

        /// <summary>
        /// The external order Id
        /// </summary>
        private static Int64 _externalOrderID = 0;

        /// <summary>
        /// add Prefix In IDs
        /// </summary>
        private static bool _addZeroPrefixToInIDs;

        /// <summary>
        /// Default constructor
        /// </summary>
        static AllocationIDGenerator()
        {
            try
            {

                _addZeroPrefixToInIDs = Convert.ToBoolean(ConfigurationManager.AppSettings["AddZeroPrefixToInIDs"]);

                _groupID = Int64.Parse(DateTime.Now.ToString("yyMMddHHmmss"));
                _externalOrderID = Int64.Parse(DateTime.Now.ToString("yyMMddHHmmss"));

                //Commenting as logged in user is always null at server.
                // SetUserID();
            }
            catch (Exception ex)
            {
                //ex.Message += "  Error while initializing id.";
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
        /// sets current user Id
        /// Shagoon: Commenting as Loggedinuser is always null on server.
        /// </summary>
        //public static void SetUserID()
        //{
        //    try
        //    {
        //        
        //        //if (CommonDataCache.CachedDataManager.GetInstance.LoggedInUser != null
        //        //      && CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID != int.MinValue)
        //        //{
        //        //    _currenUserID = CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID.ToString();
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = EnterpriseLibraryManager.HandleException(ex, EnterpriseLibraryConstants.POLICY_LOGANDSHOW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}
        //public static string GenerateTaxLotID()
        //{
        //    _taxLotID++;
        //    return _taxLotID.ToString();
        //}

        /// <summary>
        /// Generates new guid for order group
        /// </summary>
        /// <returns>The guid</returns>
        public static string GenerateOrderGroupID()
        {
            try
            {
                return System.Guid.NewGuid().ToString();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// Generates new guid for basket group
        /// </summary>
        /// <returns>The guid</returns>
        public static string GenerateBasketGroupID()
        {
            try
            {
                return System.Guid.NewGuid().ToString();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// used from server when with savedFromServer  set to true
        /// </summary>
        /// <param name="savedFromServer"></param>
        /// <returns></returns>
        public static string GenerateGroupID()
        {
            try
            {
                lock (thisLockClOrderID)
                {
                    _groupID++;
                    return _addZeroPrefixToInIDs ? "0" + _groupID.ToString() + _currenUserID.ToString() : _groupID.ToString() + _currenUserID.ToString();
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
            return string.Empty;
        }

        /// <summary>
        /// used from server when with savedFromServer  set to true
        /// </summary>
        /// <param name="savedFromServer"></param>
        /// <returns></returns>
        public static string GenerateExternalOrderID()
        {
            try
            {
                lock (thisLockExternalOrderID)
                {
                    _externalOrderID++;
                    return _addZeroPrefixToInIDs ? "0" + _externalOrderID.ToString() : _externalOrderID.ToString();
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
                throw;
            }
        }

        /// <summary>
        /// get temp account Id
        /// </summary>
        /// <returns>the TempAccountID</returns>
        public static int GetTempAccountID()
        {

            try
            {
                _tempAccountID++;
                return _tempAccountID;
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
            return 0;
        }

        /// <summary>
        /// Gaurav: To avoid duplicate key generation . maxGroupIDFromDB is the max id saved in db.
        /// Currently this class is being used by allocation only so we are passing maxid in T_Group       
        /// </summary>
        /// <param name="maxGroupIDFromDB"></param>
        public static void SetMaxGeneratedIDFromDB(string maxGroupIDFromDB)
        {
            try
            {
                lock (thisLockClOrderID)
                {
                    if (!string.IsNullOrEmpty(maxGroupIDFromDB) && maxGroupIDFromDB.Length > 0)
                    {
                        //we have to remove _currenUserID from the persisted id to check it against new generated id
                        string maxGroupIDWithoutUserID = maxGroupIDFromDB.Substring(0, maxGroupIDFromDB.Length - 1);
                        Int64 maxGroupIDFromDBAltered = 0;
                        Int64.TryParse(maxGroupIDWithoutUserID, out maxGroupIDFromDBAltered);
                        if (_groupID < maxGroupIDFromDBAltered)
                        {
                            _groupID = maxGroupIDFromDBAltered;
                        }
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
    }
}
