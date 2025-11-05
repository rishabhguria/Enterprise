using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.Global;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Prana.CentralSM
{
    class SecMasterEnrichData
    {
        static SecMasterEnrichData _SecMasterEnrichData = null;
        private static object _lockerOnInstance = new object();
        private static object _lockerOnTable = new object();
        DataTable _dtUpdateSMFields = null;

        private SecMasterEnrichData()
        {

        }

        public static SecMasterEnrichData GetInstance
        {
            get
            {
                lock (_lockerOnInstance)
                {
                    if (_SecMasterEnrichData == null)
                    {
                        _SecMasterEnrichData = new SecMasterEnrichData();
                    }
                    return _SecMasterEnrichData;
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
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

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
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

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
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

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
                                if (propInfo != null && !column.Caption.Equals("TickerSymbol"))
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
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

    }
}