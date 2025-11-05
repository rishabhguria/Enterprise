using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Prana.TaskManagement.Definition.Definition
{
    public class TaskSpecificDataPoints
    {
        private Object _lockerObject = new object();
        private Dictionary<String, String> statisticsData = new Dictionary<string, string>();
        private Dictionary<String, Object> statisticsRefData = new Dictionary<string, Object>();


        public virtual void AddOrUpdateDataPoint(String key, Object value, Object refData)
        {
            try
            {
                lock (_lockerObject)
                {
                    if (statisticsData.ContainsKey(key))
                        statisticsData.Remove(key);
                    if (statisticsRefData.ContainsKey(key))
                        statisticsRefData.Remove(key);

                    statisticsData.Add(key, value.ToString());
                    statisticsRefData.Add(key, refData);

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

        //method AddOrUpdateDataPoint will handle adding or updating data
        //if ref data not available then it will be null

        #region commented
        //public virtual void AddDataPoint(String key, Object value)
        //{
        //    try
        //    {
        //        lock (_lockerObject)
        //        {
        //            if (statisticsData.ContainsKey(key))
        //                throw new Exception("Key already exists");
        //            else
        //                statisticsData.Add(key, value.ToString());
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        //public virtual void UpdateDataPoint(String key, Object value)
        //{
        //    try
        //    {
        //        lock (_lockerObject)
        //        {
        //            if (statisticsData.ContainsKey(key))
        //                statisticsData[key] = value.ToString();
        //            else
        //                throw new KeyNotFoundException("Given key not found");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}
        #endregion

        public virtual Object GetValueForKey(String key)
        {
            Object obj = null;
            try
            {
                lock (_lockerObject)
                {
                    if (statisticsData.ContainsKey(key))
                        obj = statisticsData[key];
                    //else
                    //    throw new KeyNotFoundException("Given key not found");
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
            return obj;
        }

        public virtual Object GetRefValueForKey(String key)
        {
            Object obj = null;
            try
            {
                lock (_lockerObject)
                {
                    if (statisticsRefData.ContainsKey(key))
                        obj = statisticsRefData[key];
                    //else
                    //    throw new KeyNotFoundException("Given key not found");
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
            return obj;
        }

        public virtual List<String> GetKeySet()
        {
            List<string> lstKeySet = new List<string>();
            try
            {
                lock (_lockerObject)
                {
                    lstKeySet = statisticsData.Keys.ToList();
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
            return lstKeySet;
        }

        public Dictionary<String, Object> AsDictionary
        {
            get { return GetClonedData(); }
        }

        public String AsXML
        {
            get { return GetXMLString(); }
        }


        protected virtual String GetXMLString()
        {
            lock (_lockerObject)
            {
                XElement el = new XElement("Data", statisticsData.Select(kv => new XElement(kv.Key, kv.Value)));
                return el.ToString();
            }
        }

        protected virtual Dictionary<string, object> GetClonedData()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {
                lock (_lockerObject)
                {
                    foreach (String key in statisticsData.Keys)
                    {
                        dict.Add(key, statisticsData[key]);
                    }
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
            return dict;
        }

        public Dictionary<string, object> GetRefStatisticsData()
        {
            return statisticsRefData;
        }
    }
}