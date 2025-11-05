using Prana.LogManager;
using System;
using System.Configuration;
namespace Prana.ServerCommon
{
    public class UniqueIDGenerator
    {
        private static readonly Object thisLockClOrderID = new Object();
        private static readonly Object thisLockOrderSeqNo = new Object();
        private static Int64 _clOrderID = 0;
        private static Int64 _orderSeqNumber;
        private static Int64 _tradedBasketID = 0;
        private static readonly object lockBasketTradingID = new object();
        private static bool _addZeroPrefixToInIDs = false;

        static UniqueIDGenerator()
        {
            _addZeroPrefixToInIDs = Convert.ToBoolean(ConfigurationManager.AppSettings["AddZeroPrefixToInIDs"]);
            _clOrderID = Int64.Parse(DateTime.Now.ToString("yyyyMMddHHmmssff"));

            _tradedBasketID = Int64.Parse(DateTime.Now.ToString("yyyyMMddHHmmssff"));
        }

        public static void Initlise(Int64 seqNumber)
        {
            _orderSeqNumber = seqNumber;
        }

        public static string GetClOrderID()
        {
            lock (thisLockClOrderID)
            {
                if (_clOrderID == Int64.MaxValue)
                {
                    //TODO : This code will generate duplicate ids as yyyyMMddHHmmssff is incremented one by one to Int64.MaxValue
                    //So regenerated ID's will be duplicated 
                    _clOrderID = Int64.Parse(DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                }
                _clOrderID++;

                Logger.LoggerWrite("ClOrderID generated=" + _clOrderID, LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information);
                return _addZeroPrefixToInIDs ? "0" + _clOrderID.ToString() : _clOrderID.ToString();
            }
        }

        public static string GetOrderSeqNumber()
        {
            lock (thisLockOrderSeqNo)
            {
                if (_orderSeqNumber < Int64.MaxValue)
                {
                    _orderSeqNumber++;
                }
                else
                {
                    throw new Exception("Please Reset Message SeqNumer");

                }
                return _orderSeqNumber.ToString();
            }
        }

        public static string GenerateListID()
        {
            lock (lockBasketTradingID)
            {
                _tradedBasketID++;
                return _addZeroPrefixToInIDs ? "0" + _tradedBasketID.ToString() : _tradedBasketID.ToString();
            }
        }

        /// <summary>
        /// Gaurav: To avoid duplicate key generation . maxIDFromDB is the max id saved in db.
        /// Currently this class is being used by Cash management only so we are passing maxid in T_Order only       
        /// </summary>
        /// <param name="maxIDFromDB"></param>
        public static void SetMaxGeneratedIDFromDB(Int64 maxIDFromDB)
        {
            try
            {
                lock (thisLockClOrderID)
                {
                    if (_clOrderID < maxIDFromDB)
                        _clOrderID = maxIDFromDB;
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
