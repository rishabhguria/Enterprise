using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.Global.Utilities
{
    public class EscapedDelimiter
    {
        public static string CombineStrings(params string[] arrayOfString)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                foreach (string item in arrayOfString)
                {
                    sb.Append(item.Replace("\\", "\\\\").Replace(",", "\\,"));
                    sb.Append(",");
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
            return (sb.Length > 0) ? sb.ToString(0, sb.Length - 1) : string.Empty;
        }

        public static string CombineStrings(char splitter, char escapeChar, params string[] arrayOfString)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                foreach (string item in arrayOfString)
                {
                    sb.Append(item.Replace(new string(new char[] { escapeChar }), new string(new char[] { escapeChar, escapeChar })).Replace(new string(new char[] { splitter }), new string(new char[] { escapeChar, splitter })));
                    sb.Append(splitter);
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
            return (sb.Length > 0) ? sb.ToString(0, sb.Length - 1) : string.Empty;
        }

        //public static List<string> SplitDelimitedString(string delimitedString)
        //{
        //    bool escaped = false;
        //    List<string> listOfString = new List<string>();
        //    StringBuilder sb = new StringBuilder();
        //    try
        //    {
        //        foreach (char c in delimitedString)
        //        {
        //            if ((c == '\\') && !escaped)
        //            {
        //                escaped = true;
        //            }
        //            else if ((c == ',') && !escaped)
        //            {
        //                listOfString.Add(sb.ToString());
        //                sb.Length = 0;
        //            }
        //            else
        //            {
        //                sb.Append(c);
        //                escaped = false;
        //            }
        //        }
        //        listOfString.Add(sb.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = EnterpriseLibraryManager.HandleException(ex, EnterpriseLibraryConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //    return listOfString;
        //}

        public static List<string> SplitDelimitedString(string delimitedString, char splitter = ',', char delimiter = '\\')
        {
            bool escaped = false;
            List<string> listOfString = new List<string>();
            StringBuilder sb = new StringBuilder();
            try
            {
                foreach (char c in delimitedString)
                {
                    if ((c == delimiter) && !escaped)
                    {
                        escaped = true;
                    }
                    else if ((c == splitter) && !escaped)
                    {
                        listOfString.Add(sb.ToString());
                        sb.Length = 0;
                    }
                    else
                    {
                        sb.Append(c);
                        escaped = false;
                    }
                }
                listOfString.Add(sb.ToString());
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
            return listOfString;
        }


    }
}
