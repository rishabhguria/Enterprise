using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.Global;
using Prana.LogManager;
using System;

namespace Prana.SecurityMasterNew
{
    internal class SecMasterPreferenceManager
    {
        private Boolean _isAutoApproved = false;
        internal SecMasterPreferenceManager()
        {
            InitializePreferences();
        }

        private void InitializePreferences()
        {
            try
            {
                _isAutoApproved = Boolean.Parse(ConfigurationHelper.Instance.GetAppSettingValueByKey("SecAutoApproved").ToString());
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

        private static SecMasterPreferenceManager _secMasterPreferenceManager = null;
        static readonly object _locker = new object();

        /// <summary>
        /// Get Instance of SecMasterPrefrenceManager
        /// </summary>
        public static SecMasterPreferenceManager GetInstance
        {
            get
            {
                if (_secMasterPreferenceManager == null)
                {
                    lock (_locker)
                    {
                        if (_secMasterPreferenceManager == null)
                        {
                            _secMasterPreferenceManager = new SecMasterPreferenceManager();
                        }
                    }
                }
                return _secMasterPreferenceManager;
            }
        }

        internal void SetPreferences(SecMasterBaseObj secMasterObj)
        {
            try
            {
                secMasterObj.IsSecApproved = _isAutoApproved;
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
