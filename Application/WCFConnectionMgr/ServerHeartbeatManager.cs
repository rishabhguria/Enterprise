using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;

namespace Prana.WCFConnectionMgr
{
    public class ServerHeartbeatManager : IDisposable
    {
        private int _heartBeatInterval = int.Parse(ConfigurationManager.AppSettings["HeartBeatInterval"].ToString());
        private Timer _heartBeatTimer = null;

        public ServerHeartbeatManager()
        {
            try
            {
                _heartBeatTimer = new Timer(HeartBeatTimer_Elapsed, null, 0, _heartBeatInterval);
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void HeartBeatTimer_Elapsed(object state)
        {
            try
            {
                _heartBeatTimer.Change(Timeout.Infinite, Timeout.Infinite);

                List<string> faultedSubscribers = new List<string>();
                Parallel.ForEach(ServicesHeartbeatSubscribersCollection.GetInstance().Subscribers, kvp =>
                {
                    if (kvp.Value != null)
                    {
                        try
                        {
                            kvp.Value.HeartbeatReceived();
                        }
                        catch
                        {
                            lock (faultedSubscribers)
                            {
                                faultedSubscribers.Add(kvp.Key);
                            }
                        }
                    }
                });

                Parallel.ForEach(faultedSubscribers, faultedSubscriber =>
                {
                    ServicesHeartbeatSubscribersCollection.GetInstance().RemoveSubscriber(faultedSubscriber);
                });
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
                _heartBeatTimer.Change(_heartBeatInterval, _heartBeatInterval);
            }
        }

        #region IDisposable Methods
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (_heartBeatTimer != null)
                        _heartBeatTimer.Dispose();

                    Parallel.ForEach(ServicesHeartbeatSubscribersCollection.GetInstance().Subscribers, kvp =>
                    {
                        if (kvp.Value != null)
                        {
                            try
                            {
                                kvp.Value.ServiceClosed();
                            }
                            catch
                            {
                            }
                        }
                    });

                    ServicesHeartbeatSubscribersCollection.GetInstance().Subscribers.Clear();
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
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
