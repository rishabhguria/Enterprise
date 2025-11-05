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
using System.Threading.Tasks;

namespace Prana.TradingService
{
    public class UserTradeCacheManager
    {
        #region SingletonInstance

        /// <summary>
        /// Locker object
        /// </summary>
        private static readonly Object _lock = new Object();

        /// <summary>
        /// The duplicate trade expiration timer
        /// </summary>
        double _duplicateTradeExpirationTimer = 0;

        private static Dictionary<int, MemoryCache> _userWiseTradeCache = new Dictionary<int, MemoryCache>();
        /// <summary>
        /// The singilton instance
        /// </summary>
        private static UserTradeCacheManager userTradeCacheManager = null;
        /// <summary>
        /// Singilton instance
        /// </summary>
        /// <returns></returns>
        public static UserTradeCacheManager GetInstance()
        {
            lock (_lock)
            {
                if (userTradeCacheManager == null)
                    userTradeCacheManager = new UserTradeCacheManager();
                return userTradeCacheManager;
            }
        }
        #endregion

        public Dictionary<int, MemoryCache> UserWiseTradeCache
        {
            get { return _userWiseTradeCache; }
        }
            
        /// <summary>
        /// Constructor
        /// </summary>
        private UserTradeCacheManager()
        {
            try
            {
                _duplicateTradeExpirationTimer = Convert.ToDouble(ConfigurationManager.AppSettings[TradeManagerConstants.DUPLICATE_TRADE_TIMER]);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Adds the trades to user trades cache.
        /// </summary>
        /// <param name="or">The or.</param>
        /// <param name="userAction">The user action.</param>
        /// <param name="actionType">Type of the action.</param>
        internal async void AddTradesToUserTradesCache(OrderSingle or, UserAction userAction, string actionType, int companyUserID)
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
                        lock (_userWiseTradeCache)
                        {
                            if (_userWiseTradeCache.ContainsKey(companyUserID))
                            {
                                var policy = new CacheItemPolicy { AbsoluteExpiration = DateTime.Now.AddSeconds(_duplicateTradeExpirationTimer) };
                                _userWiseTradeCache[companyUserID].Add(od.GetUniqueKey(), od, policy);
                            }
                        }
                    }
                    //Log trade in file in asynchronous pattern, set await to false so that method will not wait for this operation's completion and return flow to calling method
                    await LogUserEnteredTradeDetails(od,companyUserID).ConfigureAwait(false);
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
        /// Existses the in user trades cache.
        /// </summary>
        /// <param name="or">The or.</param>
        /// <returns></returns>
        public bool ExistsInUserTradesCache(OrderSingle or, int timeInterval , int companyuserID)
        {
            bool isAlreadyExists = false;
            try
            {
                lock (_userWiseTradeCache)
                {
                    if (_userWiseTradeCache.ContainsKey(companyuserID))
                    {
                        OrderDetails od = new OrderDetails(or, UserAction.None, string.Empty);
                        if (_userWiseTradeCache[companyuserID].Any(o => ((OrderDetails)o.Value).CheckForDuplicateOrder(od, timeInterval)))
                            isAlreadyExists = true;
                    }
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
        /// Logs the user entered trade details.
        /// </summary>
        /// <param name="od">The od.</param>
        /// <returns></returns>
        private System.Threading.Tasks.Task LogUserEnteredTradeDetails(OrderDetails od ,int companyUserID)
        {
            try
            {
                string filePath = GetTradedOrdersFilePath(DateTime.Now.Date, companyUserID);

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
        /// Adds the User in UserWiseTradeCache
        /// </summary>
        /// <param name="companyUserID">The CompanyUserID.</param>
        /// <returns></returns>
        public void AddUserInUserWiseTradeCache(int companyUserID)
        {
            try 
            {
                if(!_userWiseTradeCache.ContainsKey(companyUserID))
                {
                    MemoryCache _userEnteredTradesCache = MemoryCache.Default;
                    _userWiseTradeCache.Add(companyUserID, _userEnteredTradesCache);
                }
                //get number of days from duplicate trade timer and load each day trades from traded orders files
                int days = TimeSpan.FromSeconds(_duplicateTradeExpirationTimer).Days;
                for (int i = 0; i <= days; i++)
                {
                    ExtractTTOrdersFromFile(GetTradedOrdersFilePath(DateTime.Now.Date.AddDays(i * -1), companyUserID), companyUserID);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Removes the User from UserWiseTradeCache
        /// </summary>
        /// <param name="companyUserID">The CompanyUserID.</param>
        /// <returns></returns>
        public void RemoveUserFromUserWiseTradeCache(int companyUserID)
        {
            try
            {
                if (_userWiseTradeCache.ContainsKey(companyUserID))
                {
                    _userWiseTradeCache[companyUserID].Dispose();
                    _userWiseTradeCache.Remove(companyUserID);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets the traded orders file path.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        private string GetTradedOrdersFilePath(DateTime date,int companyUserID)
        {
            string filePath = string.Empty;
            try
            {
                string directoryPath = AppContext.BaseDirectory + "\\" + ApplicationConstants.TRADED_ORDERS_FOLDER_NAME + "\\" + companyUserID.ToString();
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
        private void ExtractTTOrdersFromFile(string filePath, int companyUserID)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    CustomXmlSerializer _xmlSerializer = new CustomXmlSerializer();
                    string orderDetailsXML = File.ReadAllText(filePath);
                    List<string> orderDetailsArray = orderDetailsXML.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    orderDetailsArray.RemoveAt(0);
                    lock (_userWiseTradeCache)
                    {
                        orderDetailsArray.ForEach(orderXml =>
                        {
                            OrderDetails od = (OrderDetails)_xmlSerializer.ReadXml(new StringBuilder(orderXml), new OrderDetails());
                            if (od.UserAction != UserAction.No && DateTime.Now.Subtract(od.TradeTime).TotalSeconds < _duplicateTradeExpirationTimer && _userWiseTradeCache.ContainsKey(companyUserID))
                            {
                                if (!_userWiseTradeCache[companyUserID].Contains(od.GetUniqueKey()))
                                {
                                    var policy = new CacheItemPolicy { AbsoluteExpiration = DateTime.Now.AddSeconds(_duplicateTradeExpirationTimer) };
                                    _userWiseTradeCache[companyUserID].Add(od.GetUniqueKey(), od, policy);
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
    }
}
