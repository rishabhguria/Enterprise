using Prana.BusinessObjects.Classes.SecurityMasterBusinessObjects;
using Prana.HeatMap.Enums;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Xml.Serialization;

namespace Prana.HeatMap.BLL
{
    class HeatMapFilterCache
    {
        private List<SearchCondition> _conditions = new List<SearchCondition>();

        /// <summary>
        /// Locker object
        /// </summary>
        private static ReaderWriterLock _locker = new ReaderWriterLock();

        /// <summary>
        /// The singleton instance
        /// </summary>
        private static HeatMapFilterCache singiltonInstance;

        /// <summary>
        /// Private Constructor
        /// </summary>
        private HeatMapFilterCache()
        {

        }

        /// <summary>
        /// Provides the singiltan instance
        /// </summary>
        /// <returns></returns>
        internal static HeatMapFilterCache GetInstance()
        {
            if (singiltonInstance == null)
            {
                _locker.AcquireWriterLock(Timeout.Infinite);
                if (singiltonInstance == null)
                    singiltonInstance = new HeatMapFilterCache();
                _locker.ReleaseWriterLock();
            }
            return singiltonInstance;
        }

        /// <summary>
        /// updates the current filter cache
        /// </summary>
        /// <param name="conditions"></param>
        internal void UpdateFilterCache(List<SearchCondition> conditions)
        {
            try
            {
                _locker.AcquireWriterLock(Timeout.Infinite);
                _conditions = conditions;
                _locker.ReleaseWriterLock();
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

        /// <summary>
        /// Returns the current filter cache
        /// </summary>
        /// <returns></returns>
        internal List<SearchCondition> GetFilterCache()
        {
            return _conditions;
        }

        /// <summary>
        /// Returns the current filters as a query condition so that it can be used to select the data
        /// </summary>
        /// <returns></returns>
        internal String GetFilterAsQuery()
        {
            try
            {
                if (_conditions.Count == 0)
                    return String.Empty;

                StringBuilder query = new StringBuilder();
                for (int i = 0; i < _conditions.Count; i++)
                {
                    SearchCondition cndtn = _conditions[i];
                    String column = ((XmlEnumAttribute)typeof(GroupingAttributes)
                                .GetMember(cndtn.FieldName)[0]
                                .GetCustomAttributes(typeof(XmlEnumAttribute), false)[0])
                                .Name;
                    if (i == _conditions.Count - 1)
                        query.AppendFormat(" {0} = '{1}' ", column, cndtn.FieldValue.ToString());
                    else
                        query.AppendFormat(" {0} = '{1}' {2} ", column, cndtn.FieldValue.ToString(), cndtn.AndOr);
                }
                return query.ToString();
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
                return String.Empty;
            }
        }
    }
}
