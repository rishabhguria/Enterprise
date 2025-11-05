using Infragistics.Win;
using Prana.HeatMap.Enums;
using Prana.LogManager;
using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Xml.Serialization;

namespace Prana.HeatMap.BLL
{
    class DataCache : IDisposable
    {
        private DataTable _data = new DataTable();

        private bool _dataModified = false;

        /// <summary>
        /// Locker object
        /// </summary>
        private static ReaderWriterLock _locker = new ReaderWriterLock();

        /// <summary>
        /// The singleton instance
        /// </summary>
        private static DataCache singiltonInstance;

        /// <summary>
        /// Private Constructor
        /// </summary>
        private DataCache()
        {
            foreach (GroupingAttributes attribute in Enum.GetValues(typeof(GroupingAttributes)))
            {
                String columnName = ((XmlEnumAttribute)typeof(GroupingAttributes)
                    .GetMember(attribute.ToString())[0]
                    .GetCustomAttributes(typeof(XmlEnumAttribute), false)[0])
                    .Name;
                _data.Columns.Add(new DataColumn(columnName, typeof(System.String)));
            }

            foreach (Heats heat in Enum.GetValues(typeof(Heats)))
            {
                String columnName = ((XmlEnumAttribute)typeof(Heats)
                    .GetMember(heat.ToString())[0]
                    .GetCustomAttributes(typeof(XmlEnumAttribute), false)[0])
                    .Name;
                _data.Columns.Add(new DataColumn(columnName, typeof(System.Double)));
            }
        }

        /// <summary>
        /// Provides the singiltan instance
        /// </summary>
        /// <returns></returns>
        internal static DataCache GetInstance()
        {
            if (singiltonInstance == null)
            {
                _locker.AcquireWriterLock(Timeout.Infinite);
                if (singiltonInstance == null)
                    singiltonInstance = new DataCache();
                _locker.ReleaseWriterLock();
            }
            return singiltonInstance;
        }

        /// <summary>
        /// Upserts the account-symbol row in the data cache
        /// </summary>
        /// <param name="row"></param>
        internal void UpdateDataCache(DataRow row)
        {
            try
            {
                _locker.AcquireWriterLock(Timeout.Infinite);
                _dataModified = true;

                String accountLongName = row["accountLongName"].ToString();
                String symbol = row["symbol"].ToString();
                DataRow[] datarow = _data.Select(String.Format(@"symbol = '{0}' AND accountLongName = '{1}'", symbol, accountLongName));
                DataRow rowToBeUpdate;
                if (datarow.Count() != 0)
                {
                    rowToBeUpdate = datarow[0];
                }
                else
                {
                    rowToBeUpdate = _data.NewRow();
                    _data.Rows.Add(rowToBeUpdate);
                }

                foreach (GroupingAttributes attribute in Enum.GetValues(typeof(GroupingAttributes)))
                {
                    String columnName = ((XmlEnumAttribute)typeof(GroupingAttributes)
                        .GetMember(attribute.ToString())[0]
                        .GetCustomAttributes(typeof(XmlEnumAttribute), false)[0])
                        .Name;
                    rowToBeUpdate[columnName] = row[columnName];
                }
                foreach (Heats heat in Enum.GetValues(typeof(Heats)))
                {
                    String columnName = ((XmlEnumAttribute)typeof(Heats)
                        .GetMember(heat.ToString())[0]
                        .GetCustomAttributes(typeof(XmlEnumAttribute), false)[0])
                        .Name;
                    rowToBeUpdate[columnName] = Convert.ToDouble(row[columnName]);
                }

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
        /// returns a copy of the data cache after applying the filter
        /// returns null if there is no data received or all of it is filtered
        /// </summary>
        /// <returns></returns>
        internal DataTable GetDataCache()
        {
            try
            {
                _locker.AcquireReaderLock(Timeout.Infinite);
                if (_data == null || _data.Rows.Count == 0)
                {
                    _locker.ReleaseReaderLock();
                    return null;
                }
                DataTable copy = _data.Copy();
                _locker.ReleaseReaderLock();

                var datarows = copy.Copy().Select(HeatMapFilterCache.GetInstance().GetFilterAsQuery());
                if (datarows.Length > 0)
                    return datarows.CopyToDataTable();
                else
                {
                    return null;
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
                return null;
            }
        }

        /// <summary>
        /// Returns all the values in the portfolio for the given attribute
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        internal ValueList GetDistinctColumnValues(GroupingAttributes attribute)
        {
            try
            {
                _locker.AcquireReaderLock(Timeout.Infinite);
                String attributeValue = ((XmlEnumAttribute)typeof(GroupingAttributes)
                            .GetMember(attribute.ToString())[0]
                            .GetCustomAttributes(typeof(XmlEnumAttribute), false)[0])
                            .Name;

                DataView view = new DataView(_data);
                DataTable distinctValues = new DataTable();
                distinctValues = view.ToTable(true, attributeValue);
                ValueList values = new ValueList();

                foreach (DataRow row in distinctValues.Rows)
                {
                    values.ValueListItems.Add(row[attributeValue]);
                }
                _locker.ReleaseReaderLock();
                return values;
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

        /// <summary>
        /// Returns if the data was modified since last fetch
        /// </summary>
        /// <returns></returns>
        internal Boolean IsNewDataAvailable()
        {
            return _dataModified;
        }

        public void Dispose()
        {
            try
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (_data != null)
                        _data.Dispose();
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}
