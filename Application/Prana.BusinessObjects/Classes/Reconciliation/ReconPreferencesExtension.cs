using Prana.BusinessObjects.AppConstants;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Prana.BusinessObjects
{
    public static class ReconPreferencesExtension
    {


        static List<string> _listRootTemplates = null;

        static Dictionary<string, string> _dictSortedColumns = null;


        private static void FillTemplateDetails(ReconPreferences reconPreferences)
        {
            try
            {
                DataSet dsMasterColumnsDefault = ReadMasterColumnsDataSet(reconPreferences);
                _listRootTemplates = new List<string>();
                _dictSortedColumns = new Dictionary<string, string>();
                foreach (DataRow row in dsMasterColumnsDefault.Tables[0].Rows)
                {
                    #region Fill Root Templates
                    if (bool.Parse(row[2].ToString()) == true)
                    {
                        _listRootTemplates.Add(row[0].ToString());
                    }
                    #endregion
                    #region Fill Sorted Columns
                    string order = row[4].ToString();
                    //append the columns if it is to be sorted in ascending or Descending manner 
                    if (order == SortingOrder.Ascending.ToString())
                    {
                        order = "ASC";
                    }
                    else
                    {
                        order = "DESC";
                    }
                    string[] column = row[3].ToString().Split(',');
                    StringBuilder builder = new StringBuilder();
                    builder.Append(column[0]);
                    builder.Append(' ');
                    builder.Append(order);
                    //Append all the column to single string 
                    for (int i = 1; i < column.Length; i++)
                    {
                        builder.Append(',');
                        builder.Append(column[i]);
                        builder.Append(' ');
                        builder.Append(order);
                    }
                    _dictSortedColumns.Add(row[0].ToString(), builder.ToString());
                    #endregion
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

        /// <summary>
        /// TODO:In this method to get root templates we read MasterColumns xml each time, it should not be done, Read only once and keep in cache
        /// </summary>
        /// <returns></returns>
        public static List<string> getRootTemplates(this ReconPreferences reconPreferences)
        {
            try
            {
                if (_listRootTemplates == null)
                {
                    FillTemplateDetails(reconPreferences);
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
            return _listRootTemplates;
        }

        private static DataSet ReadMasterColumnsDataSet(ReconPreferences reconPreferences)
        {
            DataSet dsMasterColumnsDefault = new DataSet();
            try
            {
                string xmlMasterColumnsPath = reconPreferences.XmlRulePath + "//MasterColumns.xml";
                string xmlMasterColumnsSchema = reconPreferences.XmlRulePath + "//MasterColumns.xsd";
                dsMasterColumnsDefault.ReadXmlSchema(xmlMasterColumnsSchema);
                dsMasterColumnsDefault.ReadXml(xmlMasterColumnsPath);
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
            return dsMasterColumnsDefault;
        }

        /// <summary>
        /// gets the sorted column from for the recontype
        /// TODO:In this method to get sorted columns we read MasterColumns xml each time, it should not be done, Read only once and keep in cache
        /// </summary>
        /// <param name="reconType"></param>
        /// <returns></returns>
        public static string getSortedColumnsWithoutDataSet(this ReconPreferences reconPreferences, string reconType)
        {
            try
            {
                if (_dictSortedColumns == null)
                {
                    FillTemplateDetails(reconPreferences);
                }
                if (_dictSortedColumns.ContainsKey(reconType))
                {
                    return _dictSortedColumns[reconType];
                }
                //string xmlMasterColumnsPath = reconPreferences.XmlRulePath + "//MasterColumns.xml";
                //string xmlMasterColumnsSchema = reconPreferences.XmlRulePath + "//MasterColumns.xsd";
                //dsMasterColumnsDefault = new DataSet();
                //dsMasterColumnsDefault.ReadXmlSchema(xmlMasterColumnsSchema);
                //dsMasterColumnsDefault.ReadXml(xmlMasterColumnsPath);
                //return reconPreferences.getSortedColumns(reconType, dsMasterColumnsDefault.Tables[0]);
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
            return string.Empty;
        }

        /// <summary>
        /// gets the sorted column from MasterColumns.xml  for the recontype  and datatable
        /// </summary>
        /// <param name="reconType"></param>
        /// <returns></returns>
        ///public static string getSortedColumns(this ReconPreferences reconPreferences, string reconType, DataTable masterColumn)
        ///{
        ///    try
        ///    {
        ///        foreach (DataRow row in masterColumn.Rows)
        ///        {
        ///            if (row[0].ToString() == reconType)
        ///            {
        ///                string order = row[4].ToString();
        ///                //append the columns if it is to be sorted in ascending or Descending manner 
        ///                if (order == SortingOrder.Ascending.ToString())
        ///                {
        ///                    order = "ASC";
        ///               }
        ///              else
        ///                {
        ///                    order = "DESC";
        ///                }
        ///                string[] column = row[3].ToString().Split(',');
        ///                StringBuilder builder = new StringBuilder();
        ///                builder.Append(column[0]);
        ///                builder.Append(' ');
        ///                builder.Append(order);
        ///                //Append all the column to single string 
        ///                for (int i = 1; i < column.Length; i++)
        ///                {
        ///                    builder.Append(',');
        ///                    builder.Append(column[i]);
        ///                    builder.Append(' ');
        ///                    builder.Append(order);
        ///                }
        ///                return builder.ToString();
        ///            }
        ///        }
        ///    }
        ///    catch (Exception ex)
        ///    {
        ///        // Invoke our policy that is responsible for making sure no secure information
        ///        // gets out of our layer.
        ///        bool rethrow = EnterpriseLibraryManager.HandleException(ex, EnterpriseLibraryConstants.POLICY_LOGANDTHROW);
        ///        if (rethrow)
        ///        {
        ///            throw;
        ///        }
        ///    }
        ///    return SortedColumninXml;
        ///}
    }
}
