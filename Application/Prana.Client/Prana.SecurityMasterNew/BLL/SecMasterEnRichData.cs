using Prana.BusinessObjects;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace Prana.SecurityMasterNew
{
    class SecMasterEnRichData
    {
        static SecMasterEnRichData _secMasterEnRichData = null;
        private static readonly object _lockerOnInstance = new object();
        private static readonly object _lockerOnTable = new object();
        DataTable _dtUpdateSMFields = null;

        private SecMasterEnRichData()
        {

        }

        public static SecMasterEnRichData GetInstance
        {
            get
            {
                lock (_lockerOnInstance)
                {
                    if (_secMasterEnRichData == null)
                    {
                        _secMasterEnRichData = new SecMasterEnRichData();
                    }
                    return _secMasterEnRichData;
                }
            }
        }

        public void DeleteSMEnRichCachedData(string tickerSymbol)
        {
            try
            {
                lock (_lockerOnTable)
                {
                    if (_dtUpdateSMFields != null && _dtUpdateSMFields.Rows.Count > 0)
                    {

                        DataRow[] rows = _dtUpdateSMFields.Select("TickerSymbol=" + "'" + tickerSymbol + "'");
                        foreach (DataRow row in rows)
                        {
                            _dtUpdateSMFields.BeginInit();
                            _dtUpdateSMFields.Rows.Remove(row);
                            _dtUpdateSMFields.EndInit();
                            _dtUpdateSMFields.AcceptChanges();
                        }
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
        }

        public bool CheckSMEnRichRequires()
        {
            try
            {
                lock (_lockerOnTable)
                {
                    if (_dtUpdateSMFields != null && _dtUpdateSMFields.Rows.Count > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
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
            return false;
        }

        public void SetSMEnRichData(DataTable smData)
        {
            try
            {
                lock (_lockerOnTable)
                {
                    _dtUpdateSMFields = smData;
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


        public void EnRichSecMasterObject(SecMasterBaseObj secMasterBaseObj)
        {
            try
            {
                //removed commented part only-  omshiv
                lock (_lockerOnTable)
                {
                    if (_dtUpdateSMFields != null && _dtUpdateSMFields.Rows.Count > 0)
                    {
                        Type targettype = secMasterBaseObj.GetType();

                        DataRow[] rows = _dtUpdateSMFields.Select("TickerSymbol= " + "'" + secMasterBaseObj.TickerSymbol + "'");

                        foreach (DataRow dr in rows)
                        {
                            foreach (DataColumn column in _dtUpdateSMFields.Columns)
                            {

                                PropertyInfo propInfo = targettype.GetProperty(column.Caption);
                                if (propInfo != null && !column.Caption.Equals("TickerSymbol") && !column.Caption.Equals("DynamicUDA"))
                                {
                                    if (!String.IsNullOrEmpty(dr[column.Caption].ToString()))
                                        propInfo.SetValue(secMasterBaseObj, dr[column.Caption], null);
                                }
                            }
                        }
                    }
                }
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


        internal void EnrichUDAsandDynamicUDAs(SecMasterBaseObj secMasterBaseObj, SerializableDictionary<String, Object> dynamicUDADict)
        {
            lock (_lockerOnTable)
            {
                if (_dtUpdateSMFields != null && _dtUpdateSMFields.Rows.Count > 0)
                {
                    Type targettype = secMasterBaseObj.GetType();

                    DataRow[] rows = _dtUpdateSMFields.Select("TickerSymbol= " + "'" + secMasterBaseObj.TickerSymbol + "'");

                    foreach (DataRow dr in rows)
                    {

                        secMasterBaseObj.SymbolUDAData.FillSymbolUDAData(dr);
                    }

                    foreach (KeyValuePair<String, Object> kvp in dynamicUDADict)
                    {
                        if (!string.IsNullOrWhiteSpace(kvp.Value.ToString()))
                        {
                            if (secMasterBaseObj.DynamicUDA.ContainsKey(kvp.Key))
                            {
                                secMasterBaseObj.DynamicUDA[kvp.Key] = kvp.Value;
                            }
                            else
                            {
                                secMasterBaseObj.DynamicUDA.Add(kvp.Key, kvp.Value);
                            }
                        }
                    }
                }
            }
        }
    }
}
