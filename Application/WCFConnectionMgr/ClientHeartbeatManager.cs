using Prana.CoreService.Interfaces;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Configuration;
using System.ServiceModel;
using System.Threading;

namespace Prana.WCFConnectionMgr
{
    [CallbackBehaviorAttribute(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    public class ClientHeartbeatManager<T> : IDisposable, IServiceStatusCallback where T : IServiceStatus
    {
        private string _currentApplicationName = System.Diagnostics.Process.GetCurrentProcess().ProcessName.Replace(".vshost", "").Replace("Host", "").Replace("Prana.", "");
        private int _disconnectionInterval = int.Parse(ConfigurationManager.AppSettings["DisconnectionInterval"].ToString());
        private Timer _disconnectionTimer = null;
        private DuplexProxyBase<T> _serviceProxy = null;
        private bool _isConnectedToServer;
        private string _endpointConfigurationName;
        private bool _isRetryRequest;

        private readonly object _locker = new object();

        public event EventHandler ConnectedEvent;
        public event EventHandler DisconnectedEvent;
        public event EventHandler<EventArgs<string, string>> AnotherInstanceSubscribedEvent;

        #region Constructor
        public ClientHeartbeatManager(string endpointConfigurationName)
        {
            try
            {
                _endpointConfigurationName = endpointConfigurationName;
                _serviceProxy = new DuplexProxyBase<T>(endpointConfigurationName, this);
                _disconnectionTimer = new Timer(DisconnectionTimer_Elapsed, null, _disconnectionInterval * 2, Timeout.Infinite);

                System.Threading.Tasks.Task.Factory.StartNew(() => Subscribe()).ConfigureAwait(false);
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
        #endregion

        #region Private Methods
        private void DisconnectionTimer_Elapsed(object state)
        {
            _isRetryRequest = true;
            DisconnectionHandling();
        }

        private void DisconnectionHandling()
        {
            lock (_locker)
            {
                try
                {
                    _disconnectionTimer.Change(Timeout.Infinite, Timeout.Infinite);

                    if (DisconnectedEvent != null && _isConnectedToServer)
                    {
                        System.Threading.Tasks.Task.Factory.StartNew(() => ConnectionMgr.SetServiceConnectionStatus(_endpointConfigurationName, BusinessObjects.PranaInternalConstants.ConnectionStatus.DISCONNECTED)).ConfigureAwait(false);

                        DisconnectedEvent(this, null);
                    }
                    _isConnectedToServer = false;
                    System.Threading.Tasks.Task.Factory.StartNew(() => Subscribe()).ConfigureAwait(false);
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
                finally
                {
                    _disconnectionTimer.Change(_disconnectionInterval, Timeout.Infinite);
                }
            }
        }

        private async System.Threading.Tasks.Task Subscribe()
        {
            try
            {
                if (!_isConnectedToServer)
                {
                    if (await _serviceProxy.InnerChannel.Subscribe(_currentApplicationName, _isRetryRequest) == false)
                    {
                        _disconnectionTimer.Change(Timeout.Infinite, Timeout.Infinite);
                        _disconnectionTimer.Dispose();

                        if (AnotherInstanceSubscribedEvent != null)
                            AnotherInstanceSubscribedEvent(this, new EventArgs<string, string>(_currentApplicationName, _endpointConfigurationName));
                    }
                }
            }
            catch
            {
            }
            finally
            {
                if (_disconnectionTimer != null)
                    _disconnectionTimer.Change(_disconnectionInterval, Timeout.Infinite); ;
            }
        }
        #endregion

        #region Public Methods
        public async System.Threading.Tasks.Task UnSubscribe(string subscriber)
        {
            try
            {
                if (_isConnectedToServer)
                    await _serviceProxy.InnerChannel.UnSubscribe(subscriber);
            }
            catch
            {
            }
        }
        #endregion

        #region IServiceStatusCallback Methods
        public void HeartbeatReceived()
        {
            lock (_locker)
            {
                try
                {
                    _disconnectionTimer.Change(Timeout.Infinite, Timeout.Infinite);

                    if (ConnectedEvent != null && !_isConnectedToServer)
                    {
                        _isConnectedToServer = true;

                        System.Threading.Tasks.Task.Factory.StartNew(() => ConnectionMgr.SetServiceConnectionStatus(_endpointConfigurationName, BusinessObjects.PranaInternalConstants.ConnectionStatus.CONNECTED)).ConfigureAwait(false); ;

                        ConnectedEvent(this, null);
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
                finally
                {
                    _disconnectionTimer.Change(_disconnectionInterval, Timeout.Infinite);
                }
            }
        }

        public void ServiceClosed()
        {
            _isRetryRequest = true;
            DisconnectionHandling();
        }
        #endregion

        #region IDisposable Methods
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual async void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (_disconnectionTimer != null)
                        _disconnectionTimer.Dispose();
                    if (_serviceProxy != null)
                        _serviceProxy.Dispose();
                    await UnSubscribe(_currentApplicationName);
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
        #endregion
    }
}
