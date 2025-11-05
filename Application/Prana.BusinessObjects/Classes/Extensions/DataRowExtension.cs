using Prana.LogManager;
using System;
using System.Data;

namespace Prana.BusinessObjects
{
    public static class DataRowExtension
    {
        public static double GetDouble(this DataRow row, string columnName, double defaultValue)
        {
            try
            {
                if (row.Table.Columns.Contains(columnName))
                {
                    double value = double.MinValue;
                    if (double.TryParse(row[columnName].ToString(), out value))
                    {
                        return value;
                    }
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
            return defaultValue;
        }

        public static int GetInteger(this DataRow row, string columnName, int defaultValue)
        {
            try
            {
                int value = int.MinValue;
                if (row.Table.Columns.Contains(columnName))
                {
                    if (int.TryParse(row[columnName].ToString(), out value))
                    {
                        return value;
                    }
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
            return defaultValue;
        }

        public static string GetString(this DataRow row, string columnName, string defaultValue)
        {
            try
            {
                if (row.Table.Columns.Contains(columnName))
                    return row[columnName] != DBNull.Value ? row[columnName].ToString() : string.Empty;
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
            return defaultValue;
        }
        public static Boolean GetBool(this DataRow row, string columnName, Boolean defaultValue)
        {
            try
            {
                if (row.Table.Columns.Contains(columnName))
                {
                    Boolean value = false;
                    if (Boolean.TryParse(row["IsZero"].ToString(), out value))
                    {
                        return value;
                    }
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
            return defaultValue;
        }

        public static T GetEnum<T>(this DataRow row, string columnName, T defaultValue)
        {
            try
            {
                string str = row.GetString(columnName, defaultValue.ToString());
                if (!str.Equals(defaultValue.ToString()))
                {
                    return (T)Enum.Parse(typeof(T), str);
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
            return defaultValue;
        }

        public static DateTime GetDate(this DataRow row, string columnName, DateTime defaultValue)
        {
            try
            {
                if (row.Table.Columns.Contains(columnName))
                {
                    DateTime date = DateTimeConstants.MinValue;
                    if (DateTime.TryParse(row[columnName].ToString(), out date))
                    {
                        return date;
                    }
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
            return defaultValue;
        }
    }
}
