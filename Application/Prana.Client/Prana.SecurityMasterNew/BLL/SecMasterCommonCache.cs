using Prana.BusinessObjects.SMObjects;
using Prana.Interfaces;
using Prana.LogManager;
using System;
using System.Collections.Concurrent;

namespace Prana.SecurityMasterNew.BLL
{
    public sealed class SecMasterCommonCache
    {
        #region singleton
        private static volatile SecMasterCommonCache instance;
        private static readonly object syncRoot = new Object();

        public static SecMasterCommonCache Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new SecMasterCommonCache();
                    }
                }
                return instance;
            }
        }
        #endregion

        ConcurrentDictionary<string, StructPricingField> _pricingField = new ConcurrentDictionary<string, StructPricingField>();

        public ConcurrentDictionary<string, StructPricingField> PricingField
        {
            get
            {
                if (_pricingField.Count == 0 && _secMasterServices != null)
                {
                    _secMasterServices.RequestFieldsDictionary();
                }
                return _pricingField;
            }
        }

        /// <summary>
        /// Used in trade server only as SMDB available there
        /// </summary>
        public void FillSecMasterCommonCache()
        {
            try
            {
                _pricingField = SecMasterDataManager.GetPricingFields();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Used in client side to get data from the security service
        /// </summary>
        /// <param name="fieldDict"></param>
        public void FillSecMasterCommonCache(ConcurrentDictionary<string, StructPricingField> fieldDict)
        {
            try
            {
                _pricingField = fieldDict;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        ISecurityMasterServices _secMasterServices = null;

        /// <summary>
        /// Function to initialize and check connection 
        /// </summary>
        /// <param name="secMasterServices"></param>
        public void Initialize(Interfaces.ISecurityMasterServices secMasterServices)
        {
            try
            {
                _secMasterServices = secMasterServices;
                secMasterServices.Connected += secMasterServices_Connected;
                if (_secMasterServices.IsConnected)
                    _secMasterServices.RequestFieldsDictionary();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Function to load FieldsDictionary on secMasterService connection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void secMasterServices_Connected(object sender, EventArgs e)
        {
            try
            {
                if (_pricingField.Count == 0 && _secMasterServices != null)
                {
                    _secMasterServices.RequestFieldsDictionary();
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}
