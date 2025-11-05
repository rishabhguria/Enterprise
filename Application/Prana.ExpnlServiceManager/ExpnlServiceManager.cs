using Prana.BusinessObjects;
using Prana.LogManager;
using Prana.CoreService.Interfaces;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.ExpnlServiceManager
{
    public class ExpnlServiceManager : IDisposable, IServiceStatusCallback
    {
        private static ExpnlServiceManager _expnlServiceManager = null;
        private DuplexProxyBase<IExpnlService> _expnlServiceProxy = null;

        private ExpnlServiceManager()
        {
            try
            {
                _expnlServiceProxy = new DuplexProxyBase<IExpnlService>("ExpnlServiceEndpointAddress", this);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public static ExpnlServiceManager GetInstance
        {
            get
            {
                if (_expnlServiceManager == null)
                {
                    _expnlServiceManager = new ExpnlServiceManager();
                }
                return _expnlServiceManager;
            }
        }

        #region IServiceStatusCallback Methods
        public void HeartbeatReceived()
        {
        }

        public void ServiceClosed()
        {
        }
        #endregion

        #region IContainerService Methods
        public async System.Threading.Tasks.Task RequestStartupData()
        {
            try
            {
                await _expnlServiceProxy.InnerChannel.RequestStartupData();
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

        public async System.Threading.Tasks.Task<byte[]> OpenLog()
        {
            try
            {
                return await _expnlServiceProxy.InnerChannel.OpenLog();
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

                return null;
            }
        }

        public async System.Threading.Tasks.Task<byte[]> LoadLog()
        {
            try
            {
                return await _expnlServiceProxy.InnerChannel.LoadLog();
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

            return null;
        }

        public async System.Threading.Tasks.Task StopService()
        {
            try
            {
                await _expnlServiceProxy.InnerChannel.StopService();
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

        public async System.Threading.Tasks.Task<List<HostedService>> GetClientServicesStatus()
        {
            try
            {
                return await _expnlServiceProxy.InnerChannel.GetClientServicesStatus();
            }
            catch
            {
                return new List<HostedService>();
            }
        }

        public async System.Threading.Tasks.Task SetDebugModeStatus(bool isDebugModeEnabled)
        {
            try
            {
                await _expnlServiceProxy.InnerChannel.SetDebugModeStatus(isDebugModeEnabled);
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

        public async System.Threading.Tasks.Task<bool> GetDebugModeStatus()
        {
            try
            {
                return await _expnlServiceProxy.InnerChannel.GetDebugModeStatus();
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
            return false;
        }
        #endregion

        #region IExpnlService Methods
        public async System.Threading.Tasks.Task UpdateRefreshTimeInterval(int seconds)
        {
            try
            {
                await _expnlServiceProxy.InnerChannel.UpdateRefreshTimeInterval(seconds);
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

        public async System.Threading.Tasks.Task RefreshData()
        {
            try
            {
                await _expnlServiceProxy.InnerChannel.RefreshData();
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

        public async System.Threading.Tasks.Task StartDataDumper()
        {
            try
            {
                await _expnlServiceProxy.InnerChannel.StartDataDumper();
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

        public async System.Threading.Tasks.Task StopDataDumper()
        {
            try
            {
                await _expnlServiceProxy.InnerChannel.StopDataDumper();
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

        public async System.Threading.Tasks.Task<bool> IsDataDumperEnabled()
        {
            try
            {
                return await _expnlServiceProxy.InnerChannel.IsDataDumperEnabled();
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

                return false;
            }
        }

        public async System.Threading.Tasks.Task<bool> IsDataDumperRunning()
        {
            try
            {
                return await _expnlServiceProxy.InnerChannel.IsDataDumperRunning();
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

                return false;
            }
        }

        public async System.Threading.Tasks.Task<int> GetRefreshTimeInterval()
        {
            try
            {
                return await _expnlServiceProxy.InnerChannel.GetRefreshTimeInterval();
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

                return Int32.MinValue;
            }
        }

        public async System.Threading.Tasks.Task<TimeZoneAndTime> GetBaseTimeZoneAndBaseTimeZoneTime()
        {
            try
            {
                return await _expnlServiceProxy.InnerChannel.GetBaseTimeZoneAndBaseTimeZoneTime();
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

                return null;
            }
        }

        public async System.Threading.Tasks.Task SaveBaseTimeZoneAndBaseTimeZoneTime(string timeZone, DateTime dateTime)
        {
            try
            {
                await _expnlServiceProxy.InnerChannel.SaveBaseTimeZoneAndBaseTimeZoneTime(timeZone, dateTime);
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

        public async System.Threading.Tasks.Task SaveClearanceTime(DataTable clearanceTable)
        {
            try
            {
                await _expnlServiceProxy.InnerChannel.SaveClearanceTime(clearanceTable);
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

        public async System.Threading.Tasks.Task<Dictionary<int, DateTime>> GetDBClearanceTime()
        {
            try
            {
                return await _expnlServiceProxy.InnerChannel.GetDBClearanceTime();
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

                return null;
            }
        }

        public async System.Threading.Tasks.Task UpdateClearance(Dictionary<int, DateTime> dictionary)
        {
            try
            {
                await _expnlServiceProxy.InnerChannel.UpdateClearance(dictionary);
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

        public async System.Threading.Tasks.Task<Dictionary<int, MarketTimes>> GetMarketStartEndTime()
        {
            try
            {
                return await _expnlServiceProxy.InnerChannel.GetMarketStartEndTime();
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

                return null;
            }
        }

        public async System.Threading.Tasks.Task<Dictionary<int, DateTime>> FetchClearanceTime()
        {
            try
            {
                return await _expnlServiceProxy.InnerChannel.FetchClearanceTime();
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

                return null;
            }
        }

        public async System.Threading.Tasks.Task<Dictionary<int, BusinessObjects.TimeZone>> GetAllAUECTimeZones()
        {
            try
            {
                return await _expnlServiceProxy.InnerChannel.GetAllAUECTimeZones();
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

                return null;
            }
        }

        public async System.Threading.Tasks.Task<string> GetAUECText(int auecID)
        {
            try
            {
                return await _expnlServiceProxy.InnerChannel.GetAUECText(auecID);
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

                return string.Empty;
            }
        }

        public async System.Threading.Tasks.Task<BusinessObjects.TimeZone> GetAUECTimeZone(int auecID)
        {
            try
            {
                return await _expnlServiceProxy.InnerChannel.GetAUECTimeZone(auecID);
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

                return null;
            }
        }
        #endregion

        #region IDisposable Methods
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _expnlServiceProxy.Dispose();
            }
        }
        #endregion
    }
}
