using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.XMLUtilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Text;

namespace Prana.TradeManager.Extension.CacheStore
{
    internal sealed class UserTradesCache : IDisposable
    {
        #region Members

        /// <summary>
        /// The instance
        /// </summary>
        private static UserTradesCache _instance = null;

        /// <summary>
        /// The singleton locker
        /// </summary>
        private static readonly object _singletonLocker = new object();

        /// <summary>
        /// The duplicate trade expiration timer
        /// </summary>
        double _duplicateTradeExpirationTimer = 0;

        /// <summary>
        /// The user entered trades cache
        /// </summary>
        private static MemoryCache _userEnteredTradesCache = MemoryCache.Default;

        /// <summary>
        /// The trades cache locker
        /// </summary>
        private object _tradesCacheLocker = new object();

        #endregion Members

        #region Properties

        /// <summary>
        /// Gets the get instance.
        /// </summary>
        /// <value>
        /// The get instance.
        /// </value>
        internal static UserTradesCache GetInstance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_singletonLocker)
                    {
                        if (_instance == null)
                            _instance = new UserTradesCache();
                    }
                }
                return _instance;
            }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Prevents a default instance of the <see cref="UserTradesCache"/> class from being created.
        /// </summary>
        private UserTradesCache()
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Adds the trades to user trades cache.
        /// </summary>
        /// <param name="or">The or.</param>
        /// <param name="userAction">The user action.</param>
        /// <param name="actionType">Type of the action.</param>
        internal async void AddTradesToUserTradesCache(OrderSingle or, UserAction userAction, string actionType)
        {
            try
            {
                //don't add to cache if this is replace trade request
                if (!or.MsgType.Equals(FIXConstants.MSGOrderCancelReplaceRequest))
                {
                    //get order details object and add it to cache
                    OrderDetails od = new OrderDetails(or, userAction, actionType);
                    if (userAction != UserAction.No)
                    {
                        lock (_tradesCacheLocker)
                        {
                            var policy = new CacheItemPolicy { AbsoluteExpiration = DateTime.Now.AddSeconds(_duplicateTradeExpirationTimer) };
                            _userEnteredTradesCache.Add(od.GetUniqueKey(), od, policy);
                        }
                    }
                    //Log trade in file in asynchronous pattern, set await to false so that method will not wait for this operation's completion and return flow to calling method
                    await LogUserEnteredTradeDetails(od).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Logs the user entered trade details.
        /// </summary>
        /// <param name="od">The od.</param>
        /// <returns></returns>
        private System.Threading.Tasks.Task LogUserEnteredTradeDetails(OrderDetails od)
        {
            try
            {
                string filePath = GetTradedOrdersFilePath(DateTime.Now.Date);

                //if 'TradesEnteredFromTT_ + time stamp' file doesn't exist then create this file
                if (!File.Exists(filePath))
                {
                    using (FileStream sourceStream = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true))
                    {
                        byte[] startString = Encoding.ASCII.GetBytes(TradeManagerConstants.XML_VERSION_TAG);
                        sourceStream.WriteAsync(startString, 0, startString.Length);
                    };
                }

                //append order details to the file
                CustomXmlSerializer _xmlSerializer = new CustomXmlSerializer();
                using (FileStream sourceStream = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true))
                {
                    byte[] orderString = Encoding.ASCII.GetBytes(Environment.NewLine + _xmlSerializer.WriteString(od));
                    return sourceStream.WriteAsync(orderString, 0, orderString.Length);
                };
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return null;
            }
        }

        /// <summary>
        /// Existses the in user trades cache.
        /// </summary>
        /// <param name="or">The or.</param>
        /// <returns></returns>
        internal bool ExistsInUserTradesCache(OrderSingle or, int timeInterval)
        {
            bool isAlreadyExists = false;
            try
            {
                lock (_tradesCacheLocker)
                {
                    OrderDetails od = new OrderDetails(or, UserAction.None, string.Empty);
                    if (_userEnteredTradesCache.Any(o => ((OrderDetails)o.Value).CheckForDuplicateOrder(od, timeInterval)))
                        isAlreadyExists = true;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return isAlreadyExists;
        }

        /// <summary>
        /// Loads the user entered trades cache.
        /// </summary>
        internal void LoadUserEnteredTradesCache()
        {
            try
            {
                _duplicateTradeExpirationTimer = Convert.ToDouble(ConfigurationManager.AppSettings[TradeManagerConstants.DUPLICATE_TRADE_TIMER]);

                //get number of days from duplicate trade timer and load each day trades from traded orders files
                int days = TimeSpan.FromSeconds(_duplicateTradeExpirationTimer).Days;
                for (int i = 0; i <= days; i++)
                {
                    ExtractTTOrdersFromFile(GetTradedOrdersFilePath(DateTime.Now.Date.AddDays(i * -1)));
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Gets the traded orders file path.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        private string GetTradedOrdersFilePath(DateTime date)
        {
            string filePath = string.Empty;
            try
            {
                string directoryPath = TradeManagerExtension.GetInstance().ApplicationPath + "\\" + ApplicationConstants.TRADED_ORDERS_FOLDER_NAME + "\\" + CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID.ToString();
                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);

                string fileName = TradeManagerConstants.TRADED_ORDERS_FILE_PREFIX + date.ToString("MM_dd_yyyy") + ".xml";
                filePath = directoryPath + fileName;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return filePath;
        }

        /// <summary>
        /// Extracts the tt orders from file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        private void ExtractTTOrdersFromFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    CustomXmlSerializer _xmlSerializer = new CustomXmlSerializer();
                    string orderDetailsXML = File.ReadAllText(filePath);
                    List<string> orderDetailsArray = orderDetailsXML.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    orderDetailsArray.RemoveAt(0);
                    lock (_tradesCacheLocker)
                    {
                        orderDetailsArray.ForEach(orderXml =>
                        {
                            OrderDetails od = (OrderDetails)_xmlSerializer.ReadXml(new StringBuilder(orderXml), new OrderDetails());
                            if (od.UserAction != UserAction.No && DateTime.Now.Subtract(od.TradeTime).TotalSeconds < _duplicateTradeExpirationTimer)
                            {
                                if (!_userEnteredTradesCache.Contains(od.GetUniqueKey()))
                                {
                                    var policy = new CacheItemPolicy { AbsoluteExpiration = DateTime.Now.AddSeconds(_duplicateTradeExpirationTimer) };
                                    _userEnteredTradesCache.Add(od.GetUniqueKey(), od, policy);
                                }
                            }
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Removes from user trades cache.
        /// </summary>
        /// <param name="or">The or.</param>
        internal void RemoveFromUserTradesCache(OrderSingle or)
        {
            try
            {
                OrderDetails od = new OrderDetails(or, UserAction.None, string.Empty);
                OrderDetails orderDetails = (OrderDetails)_userEnteredTradesCache.Where(o => ((OrderDetails)o.Value).GetComplianceRejectOrder(od)).OrderBy(x => ((OrderDetails)x.Value).TradeTime).FirstOrDefault().Value;
                if (orderDetails != null)
                {
                    lock (_tradesCacheLocker)
                    {
                        _userEnteredTradesCache.Remove(orderDetails.GetUniqueKey());
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        #endregion Methods

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="isDisposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        private void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                _userEnteredTradesCache.Dispose();
            }
        }

        #endregion
    }
}
