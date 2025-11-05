using Prana.BusinessObjects;
using Prana.LogManager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace Prana.Utilities.MiscUtilities
{
    public static class GeneralUtilities
    {
        /// <summary>
        /// creates List<string> from string which is separated by separator
        /// </summary>
        /// <param name="names"></param>
        /// <param name="seperator"></param>
        /// <returns></returns>
        public static List<string> GetListFromString(string names, char seperator)
        {
            if (names == null)
                return null;
            List<string> list = new List<string>();
            string[] columns = names.Split(seperator);
            foreach (string columnName in columns)
            {
                if (columnName != string.Empty)
                {
                    list.Add(columnName);
                }
            }
            return list;
        }
        /// <summary>
        /// Creates String from List<string> separated by separator
        /// </summary>
        /// <param name="list"></param>
        /// <param name="seperator"></param>
        public static string GetStringFromList(List<string> list, char seperator)
        {
            StringBuilder sBuilder = new StringBuilder();
            foreach (string columnName in list)
            {
                sBuilder.Append(seperator);
                sBuilder.Append(columnName);
            }
            string name = sBuilder.ToString();
            if (name != string.Empty)
            {
                name = name.Substring(1, name.Length - 1);
            }
            return name;
        }

        public static List<string> GetListFromArrayList(ArrayList list)
        {
            List<string> genericList = new List<string>();
            foreach (object obj in list)
            {
                genericList.Add(obj != null ? obj.ToString() : null);
            }
            return genericList;
        }
        public static string GetStringFromArrayList(ArrayList list)
        {
            if (list.Count > 0)
            {
                StringBuilder sBuilder = new StringBuilder();
                foreach (object obj in list)
                {
                    sBuilder.Append(',');
                    sBuilder.Append(obj);
                }
                string name = sBuilder.ToString();

                if (name != string.Empty)
                {
                    name = name.Substring(1, name.Length - 1);
                }
                return name;
            }
            else
            {
                return string.Empty;
            }
        }

        public static List<string> CloneList(List<string> list)
        {
            List<string> clonedList = new List<string>();
            foreach (string name in list)
            {
                clonedList.Add(name);
            }
            return clonedList;
        }

        public static void Copy(object srcInstance, object destInstance)
        {
            PropertyInfo[] srcPropInfos = srcInstance.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            PropertyInfo[] destPropInfos = srcInstance.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo pi in srcPropInfos)
            {
                if (pi.CanWrite)
                {
                    PropertyInfo destPI = (PropertyInfo)Array.Find(destPropInfos, delegate (PropertyInfo dpi)
                    {
                        return (0 == string.Compare(dpi.Name, pi.Name, true));
                    });
                    destPI.SetValue(destInstance, pi.GetValue(srcInstance, null), null);
                }
            }
        }

        public static void InsertDataIntoOneTableFromAnotherTable(DataTable sourecTable, DataTable targetTable, Dictionary<string, XmlNode> SMMappingCOLList)
        {
            try
            {
                foreach (DataRow drSource in sourecTable.Rows)
                {
                    DataRow drTarget = targetTable.NewRow();
                    foreach (DataColumn column in sourecTable.Columns)
                    {
                        if (SMMappingCOLList.ContainsKey(column.Caption))
                        {
                            XmlNode node = SMMappingCOLList[column.Caption];
                            if (!targetTable.Rows.Contains(drSource[column.Caption]))
                            {
                                if (!string.IsNullOrEmpty(drSource[column.Caption].ToString()))
                                    drTarget[node.Attributes["SMCOLName"].Value] = drSource[column.Caption];
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(drTarget["TickerSymbol"].ToString()))
                    {
                        targetTable.Rows.Add(drTarget);
                    }
                }
            }
            catch //(Exception ex)
            {
                throw;
            }
        }

        public static DataRow CreateDataRowFromObject<T>(T _data, List<string> ListOfProperties)
        {
            DataRow dr = null;
            try
            {
                DataTable dt = new DataTable("Sheet1");
                Type typeObject = typeof(T);
                Dictionary<string, PropertyInfo> _dictPropertiesToGet = new Dictionary<string, PropertyInfo>();
                #region Creating Dictionary And DataTable's Header Based On the Specified columns
                if (ListOfProperties == null)
                {
                    PropertyInfo[] listOfProperties = typeObject.GetProperties();
                    foreach (PropertyInfo property in listOfProperties)
                    {
                        dt.Columns.Add(property.Name);
                        _dictPropertiesToGet.Add(property.Name, property);
                    }
                }
                else
                {
                    foreach (string propertyName in ListOfProperties)
                    {
                        PropertyInfo property = typeObject.GetProperty(propertyName);
                        _dictPropertiesToGet.Add(propertyName, property);
                        dt.Columns.Add(propertyName, property.PropertyType);
                    }
                }
                #endregion

                dr = dt.NewRow();
                foreach (string currentProperty in _dictPropertiesToGet.Keys)
                {
                    if (_data != null)
                    {
                        object objValue = _dictPropertiesToGet[currentProperty].GetValue(_data, null);
                        if (objValue != null)
                        {
                            string value = objValue.ToString();
                            dr[currentProperty] = value;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw new Exception("Error in creating DataRow From Object");
            }
            return dr;
        }

        public static DataSet CreateTableStructureFromObject(IList list)
        {
            DataSet ds = new DataSet();
            try
            {
                DataTable dt = new DataTable("Sheet1");
                if (list != null)
                {
                    Type typeObject = list.GetType().GetProperty("Item").PropertyType;
                    PropertyInfo[] listOfProperties = typeObject.GetProperties();
                    foreach (PropertyInfo property in listOfProperties)
                    {
                        dt.Columns.Add(property.Name);
                    }
                }
                ds.Tables.Add(dt.Copy());
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return ds;
        }

        /// <summary>
        /// Creates a table structure fromt he object passed and the properties of that object
        /// </summary>
        /// <param name="obj">object for which the table structure is to be made</param>
        /// <returns></returns>
        public static DataSet CreateTableStructureFromObject(Object obj)
        {
            DataSet ds = new DataSet();
            try
            {
                DataTable dt = new DataTable("Sheet1");
                if (obj != null)
                {
                    Type typeObject = obj.GetType();
                    PropertyInfo[] listOfProperties = typeObject.GetProperties();
                    foreach (PropertyInfo property in listOfProperties)
                    {
                        dt.Columns.Add(property.Name);
                    }
                }
                ds.Tables.Add(dt.Copy());
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
            return ds;
        }

        public static void FillDataSetFromCollection(IList list, ref DataSet ds, bool nullForEmptyString, bool stringValueForEnums)
        {
            try
            {
                DataTable dt = ds.Tables[0];
                dt.TableName = "Sheet1";
                if (list != null)
                {
                    Type typeObject = list.GetType().GetProperty("Item").PropertyType;
                    PropertyInfo[] propertiesTaxlot = typeObject.GetProperties();
                    Dictionary<string, PropertyInfo> _dictPropertiesToGet = new Dictionary<string, PropertyInfo>();
                    #region Creating Dictionary And DataTable's Header Based On the Specified columns
                    PropertyInfo[] listOfProperties = typeObject.GetProperties();
                    foreach (PropertyInfo property in listOfProperties)
                    {
                        _dictPropertiesToGet.Add(property.Name, property);
                    }
                    foreach (object obj in list)
                    {
                        DataRow dr = dt.NewRow();
                        foreach (string currentProperty in _dictPropertiesToGet.Keys)
                        {
                            object objValue = null;
                            Type dataType = _dictPropertiesToGet[currentProperty].PropertyType;
                            if (dataType.BaseType.Equals(typeof(System.Enum)))
                            {
                                if (!stringValueForEnums)
                                {
                                    objValue = Convert.ToInt32(Enum.Parse(dataType, _dictPropertiesToGet[currentProperty].GetValue(obj, null).ToString(), true));
                                }
                                else
                                {
                                    objValue = _dictPropertiesToGet[currentProperty].GetValue(obj, null);
                                }
                            }
                            else
                            {
                                objValue = _dictPropertiesToGet[currentProperty].GetValue(obj, null);
                            }
                            if (objValue != null)
                            {
                                string value = objValue.ToString();
                                if (!nullForEmptyString)
                                {
                                    dr[currentProperty] = value;
                                }
                                else
                                {
                                    if (String.IsNullOrEmpty(value))
                                    {
                                        dr[currentProperty] = DBNull.Value;
                                    }
                                    else
                                    {
                                        dr[currentProperty] = value;
                                    }
                                }
                            }
                        }
                        dt.Rows.Add(dr);
                    }
                }
                #endregion
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

        public static DataSet CreateDataSetFromCollection(IList list, List<string> columns)
        {
            DataSet ds = new DataSet();
            try
            {
                if (list != null)
                {
                    DataTable dt = new DataTable("Sheet1");
                    Type typeObject = list.GetType().GetProperty("Item").PropertyType;
                    PropertyInfo[] propertiesTaxlot = typeObject.GetProperties();
                    Dictionary<string, PropertyInfo> _dictPropertiesToGet = new Dictionary<string, PropertyInfo>();
                    #region Creating Dictionary And DataTable's Header Based On the Specified columns
                    if (columns == null)
                    {
                        PropertyInfo[] listOfProperties = typeObject.GetProperties();
                        foreach (PropertyInfo property in listOfProperties)
                        {
                            dt.Columns.Add(property.Name);
                            _dictPropertiesToGet.Add(property.Name, property);
                        }
                        foreach (object obj in list)
                        {
                            if (obj != null)
                            {
                                DataRow dr = dt.NewRow();

                                foreach (string currentProperty in _dictPropertiesToGet.Keys)
                                {
                                    object objValue = _dictPropertiesToGet[currentProperty].GetValue(obj, null);
                                    if (objValue != null)
                                    {
                                        string value = objValue.ToString();
                                        dr[currentProperty] = value;
                                    }
                                }
                                dt.Rows.Add(dr);
                            }
                        }
                    }
                    else
                    {
                        foreach (string propertyName in columns)
                        {
                            PropertyInfo property = typeObject.GetProperty(propertyName);
                            _dictPropertiesToGet.Add(propertyName, property);
                            //Header
                            dt.Columns.Add(propertyName, property.PropertyType);
                        }
                        foreach (object obj in list)
                        {
                            if (obj != null)
                            {
                                DataRow dr = dt.NewRow();

                                foreach (string currentProperty in _dictPropertiesToGet.Keys)
                                {
                                    object objValue = _dictPropertiesToGet[currentProperty].GetValue(obj, null);
                                    if (objValue != null)
                                    {
                                        string value = objValue.ToString();
                                        dr[currentProperty] = value;
                                    }
                                }
                                dt.Rows.Add(dr);
                            }
                        }
                    }
                    #endregion
                    ds.Tables.Add(dt.Copy());
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw new Exception("Error in creating DataSet From Collection");
            }
            return ds;
        }

        public static DataSet CreateDataSetFromCollectionWithHeaders(IList list, Dictionary<string, string> columnsWithSpecifiedNames)
        {
            DataSet ds = new DataSet();
            try
            {
                if (list != null)
                {
                    DataTable dt = new DataTable("Sheet1");
                    Type typeObject = list.GetType().GetProperty("Item").PropertyType;
                    PropertyInfo[] propertiesTaxlot = typeObject.GetProperties();
                    Dictionary<string, PropertyInfo> _dictPropertiesToGet = new Dictionary<string, PropertyInfo>();
                    #region Creating Dictionary And DataTable's Header Based On the Specified columns
                    if (columnsWithSpecifiedNames == null)
                    {
                        PropertyInfo[] listOfProperties = typeObject.GetProperties();
                        foreach (PropertyInfo property in listOfProperties)
                        {
                            dt.Columns.Add(property.Name);
                            _dictPropertiesToGet.Add(property.Name, property);
                        }
                    }
                    else
                    {
                        foreach (string propertyName in columnsWithSpecifiedNames.Keys)
                        {
                            PropertyInfo property = typeObject.GetProperty(propertyName);
                            _dictPropertiesToGet.Add(propertyName, property);
                            //Header
                            dt.Columns.Add(columnsWithSpecifiedNames[propertyName], property.PropertyType);
                        }
                    }
                    #endregion

                    if (columnsWithSpecifiedNames != null)
                    {
                        foreach (object obj in list)
                        {
                            DataRow dr = dt.NewRow();
                            foreach (string currentProperty in _dictPropertiesToGet.Keys)
                            {
                                object objValue = _dictPropertiesToGet[currentProperty].GetValue(obj, null);
                                if (objValue != null)
                                {
                                    string value = objValue.ToString();
                                    dr[columnsWithSpecifiedNames[currentProperty]] = value;
                                }
                            }
                            dt.Rows.Add(dr);
                        }
                    }
                    else
                    {
                        foreach (object obj in list)
                        {
                            DataRow dr = dt.NewRow();
                            foreach (string currentProperty in _dictPropertiesToGet.Keys)
                            {
                                object objValue = _dictPropertiesToGet[currentProperty].GetValue(obj, null);
                                if (objValue != null)
                                {
                                    string value = objValue.ToString();
                                    dr[currentProperty] = value;
                                }
                            }
                            dt.Rows.Add(dr);
                        }

                    }
                    ds.Tables.Add(dt.Copy());
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
            return ds;
        }

        public static DataTable ArrangeTable(DataTable dt, string tableName)
        {
            try
            {
                dt.TableName = tableName;
                for (int irow = 0; irow < dt.Rows.Count; irow++)
                {
                    for (int icol = 0; icol < dt.Columns.Count; icol++)
                    {
                        string val = dt.Rows[irow].ItemArray[icol].ToString();
                        if (String.IsNullOrEmpty(val.Trim()))
                        {
                            dt.Rows[irow][icol] = "*";
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
            return dt;
        }

        public static DataTable ReArrangeTable(DataTable dt)
        {
            try
            {
                for (int irow = 0; irow < dt.Rows.Count; irow++)
                {
                    for (int icol = 0; icol < dt.Columns.Count; icol++)
                    {
                        string val = dt.Rows[irow].ItemArray[icol].ToString();
                        if (val.Equals("*"))
                        {
                            dt.Rows[irow][icol] = string.Empty;
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
            return dt;
        }

        public static IList CreateCollectionFromDataTable(DataTable dt, Type targettype)
        {
            IList collection = new List<object>();
            try
            {
                List<string> columnsInDataTable = new List<string>();
                foreach (DataColumn column in dt.Columns)
                    columnsInDataTable.Add(column.Caption);

                PropertyInfo[] propertiestarget = targettype.GetProperties();
                foreach (DataRow dr in dt.Rows)
                {
                    Object targetobj = Activator.CreateInstance(targettype);
                    foreach (PropertyInfo pi in propertiestarget)
                    {
                        if (pi.CanWrite && columnsInDataTable.Contains(pi.Name))
                        {
                            Type type = pi.PropertyType;
                            object value = dr[pi.Name];
                            if (value is DBNull)
                                value = null;
                            if (value != null)
                            {
                                if (type.BaseType == typeof(Enum))
                                {
                                    value = Enum.Parse(type, value.ToString(), true);
                                }
                                else
                                {
                                    value = Convert.ChangeType(value, type);
                                }
                                pi.SetValue(targetobj, value, null);
                            }
                            else
                            {
                                pi.SetValue(targetobj, value, null);
                            }
                        }
                    }
                    collection.Add(targetobj);
                }
            }
            catch //(Exception ex)
            {
                throw;
            }
            return collection;
        }

        public static DataTable CreateTableFromCollection<T>(DataTable dt, List<T> list)
        {
            Type typeObject = typeof(T);
            PropertyInfo[] properties = typeObject.GetProperties();
            Dictionary<string, PropertyInfo> dictProperties = new Dictionary<string, PropertyInfo>();
            if (list != null && properties.Length > 0)
            {
                foreach (DataColumn column in dt.Columns)
                {
                    foreach (PropertyInfo property in properties)
                    {
                        if ((column.ColumnName.Equals(property.Name)) && (column.DataType.Equals(property.PropertyType)))
                        {
                            dictProperties.Add(property.Name, property);
                        }
                    }
                }
                foreach (object obj in list)
                {
                    DataRow dr = dt.NewRow();
                    foreach (string currentProperty in dictProperties.Keys)
                    {
                        object objValue = dictProperties[currentProperty].GetValue(obj, null);
                        if (objValue != null)
                        {
                            string value = objValue.ToString();
                            dr[currentProperty] = value;
                        }
                    }
                    dt.Rows.Add(dr);
                    dt.AcceptChanges();
                }
            }
            return dt;
        }

        public static IList CreateCollectionFromDataTableForSecMaster(DataTable dt, Type targettype)
        {
            IList collection = new List<object>();
            try
            {
                List<string> columnsInDataTable = new List<string>();
                foreach (DataColumn column in dt.Columns)
                    columnsInDataTable.Add(column.Caption);

                PropertyInfo[] propertiestarget = targettype.GetProperties();
                int counter = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    counter = counter + 1;
                    Object targetobj = Activator.CreateInstance(targettype);
                    foreach (PropertyInfo pi in propertiestarget)
                    {
                        if (pi.CanWrite && columnsInDataTable.Contains(pi.Name))
                        {
                            Type type = pi.PropertyType;
                            object value;
                            if (pi.Name.Equals("Symbol_PK"))
                            {
                                value = Int64.Parse(DateTime.Now.ToString("yyyyMMddHHmmss")) + counter;
                            }
                            else
                            {
                                value = dr[pi.Name];
                            }
                            if (value is DBNull)
                                value = null;
                            if (value != null)
                            {
                                if (type.BaseType == typeof(Enum))
                                {
                                    value = Enum.Parse(type, value.ToString(), true);
                                }
                                //modified by omshiv, 4, March 2014, 
                                // Handling date parsing issue if value is blank or coming from .XLs files (formate issue)
                                else if (type.Name == "DateTime")
                                {
                                    bool blnIsTrue;
                                    DateTime result;
                                    blnIsTrue = DateTime.TryParse(value.ToString(), out result);
                                    if (blnIsTrue)
                                    {
                                        value = result;
                                    }
                                    else
                                    {
                                        // coming from .XLs files (formate issue)
                                        bool isParsed = false;
                                        double outResult;
                                        isParsed = double.TryParse(value.ToString(), out outResult);
                                        if (isParsed)
                                        {
                                            DateTime datetime = DateTime.FromOADate(outResult);
                                            value = datetime;
                                        }
                                        else
                                        {
                                            value = DateTimeConstants.MinValue;
                                        }
                                    }
                                }
                                else
                                {
                                    value = Convert.ChangeType(value, type);
                                }
                                pi.SetValue(targetobj, value, null);
                            }
                            else
                            {
                                pi.SetValue(targetobj, value, null);
                            }
                        }
                    }
                    collection.Add(targetobj);
                }
            }
            catch //(Exception ex)
            {
                throw;
            }
            return collection;
        }

        public static TKey FindKeyByValue<TKey, TValue>(IDictionary<TKey, TValue> dictionary, TValue value)
        {
            try
            {
                if (dictionary != null)
                {
                    foreach (KeyValuePair<TKey, TValue> pair in dictionary)
                        if (value != null && value.Equals(pair.Value)) return pair.Key;
                }
                throw new Exception("the value is not found in the dictionary");
            }
            catch //(Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Narendra Kumar Jangir, May 21 2013
        /// This method pares csv file to datatable
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static DataTable GetDataTableFromUploadedDataFileBulkRead(string fileName)
        {
            DataTable dtCSVToDataTable = new DataTable();
            try
            {
                // extract the fields based on regular expression               
                Regex regex = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
                //read all lines from csv file
                string[] lines = File.ReadAllLines(fileName);
                //columns will be picked from the first line of the csv file
                string[] Columns = (lines[0].Split(','));
                //add columns to the datatable
                //no of columns will be added same as of items in the first line of csv file
                for (int i = 0; i < Columns.Length; i++)
                {
                    dtCSVToDataTable.Columns.Add("COL" + (i + 1).ToString(), typeof(string));
                }
                //add rows to the datatable
                for (int i = 0; i < lines.Length; i++)
                {
                    //read each line and add to the DataTable
                    string[] items = regex.Split(lines[i]);
                    DataRow row = dtCSVToDataTable.NewRow();
                    for (int j = 0; j < items.Length; j++)
                    {
                        //adjust/add no. of columns on the basis of fields in the data row
                        //it may be possible in production files that no of columns to the csv file will not remain same as of first line, may be increaesd
                        //first line may contain date/time, client name or file details
                        if (j > (dtCSVToDataTable.Columns.Count - 1))
                        {
                            dtCSVToDataTable.Columns.Add("COL" + (j + 1).ToString(), typeof(string));
                        }
                        items[j] = items[j].TrimStart(' ', '"').TrimEnd('"');
                        row[j] = items[j].Replace("\"\"", "\"");
                    }
                    dtCSVToDataTable.Rows.Add(row);
                }

                // in case no data was parsed, create a single column
                if (dtCSVToDataTable.Columns.Count == 0)
                    dtCSVToDataTable.Columns.Add("NoData");
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return dtCSVToDataTable;
        }

        public static DataTable GetDataTableFromList<T>(this IList<T> data)
        {
            DataTable table = new DataTable();
            try
            {
                PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
                for (int i = 0; i < props.Count; i++)
                {
                    PropertyDescriptor prop = props[i];
                    Type propertyType = prop.PropertyType;
                    if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        propertyType = propertyType.GetGenericArguments()[0];
                        var column = new DataColumn(prop.Name, propertyType) { AllowDBNull = true };
                        table.Columns.Add(column);
                    }
                    else
                    {
                        table.Columns.Add(prop.Name, propertyType);
                    }
                }
                object[] values = new object[props.Count];
                if (data != null)
                {
                    foreach (T item in data)
                    {
                        for (int i = 0; i < values.Length; i++)
                        {
                            values[i] = props[i].GetValue(item) ?? DBNull.Value;
                        }
                        table.Rows.Add(values);
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
            return table;
        }

        /// <summary>
        /// converts list of tuples to datatable
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <param name="data"></param>
        /// <param name="t2Name"></param>
        /// <param name="t3Name"></param>
        /// <returns></returns>
        public static DataTable GetDataTableFromList<T1, T2, T3>(this IList<Tuple<T1, T2, T3>> data, string t2Name, string t3Name)
        {
            DataTable table = new DataTable();
            try
            {
                if (data != null)
                {
                    PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T1));
                    for (int i = 0; i < props.Count; i++)
                    {
                        PropertyDescriptor prop = props[i];
                        table.Columns.Add(prop.Name, prop.PropertyType);
                    }
                    table.Columns.Add(t2Name, typeof(T2));
                    table.Columns.Add(t3Name, typeof(T3));

                    object[] values = new object[props.Count + 2];
                    foreach (Tuple<T1, T2, T3> item in data)
                    {
                        for (int i = 0; i < values.Length - 2; i++)
                        {
                            values[i] = props[i].GetValue(item.Item1);
                        }
                        values[props.Count] = item.Item2;
                        values[props.Count + 1] = item.Item3;

                        table.Rows.Add(values);
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
            return table;
        }

        public static DataSet CreateTableStructureFromObject(IList list, List<string> lsRequiredColumns)
        {
            DataSet ds = new DataSet();
            try
            {
                DataTable dt = new DataTable("Sheet1");
                if (list != null)
                {

                    Type typeObject = list.GetType().GetProperty("Item").PropertyType;
                    PropertyInfo[] listOfProperties = typeObject.GetProperties();
                    if (lsRequiredColumns == null || lsRequiredColumns.Count == 0)
                    {
                        foreach (PropertyInfo property in listOfProperties)
                        {
                            dt.Columns.Add(property.Name);
                        }
                    }
                    else
                    {
                        foreach (PropertyInfo property in listOfProperties)
                        {
                            if (lsRequiredColumns.Contains(property.Name))
                            {
                                dt.Columns.Add(property.Name);
                            }
                        }
                    }
                }
                ds.Tables.Add(dt.Copy());
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }

            return ds;
        }

        public static void FillDataSetFromCollection(IList list, ref DataSet ds, bool nullForEmptyString, bool stringValueForEnums, List<string> lsRequiredColumns)
        {
            try
            {
                DataTable dt = ds.Tables[0];
                dt.TableName = "Sheet1";
                if (list != null)
                {

                    Type typeObject = list.GetType().GetProperty("Item").PropertyType;
                    PropertyInfo[] propertiesTaxlot = typeObject.GetProperties();
                    Dictionary<string, PropertyInfo> _dictPropertiesToGet = new Dictionary<string, PropertyInfo>();
                    #region Creating Dictionary And DataTable's Header Based On the Specified columns
                    PropertyInfo[] listOfProperties = typeObject.GetProperties();
                    foreach (PropertyInfo property in listOfProperties)
                    {
                        _dictPropertiesToGet.Add(property.Name, property);
                    }
                    foreach (object obj in list)
                    {
                        DataRow dr = dt.NewRow();
                        foreach (string currentProperty in _dictPropertiesToGet.Keys)
                        {
                            if (lsRequiredColumns.Contains(currentProperty))
                            {
                                object objValue = null;
                                Type dataType = _dictPropertiesToGet[currentProperty].PropertyType;
                                if (dataType.BaseType.Equals(typeof(System.Enum)))
                                {
                                    if (!stringValueForEnums)
                                    {
                                        objValue = Convert.ToInt32(Enum.Parse(dataType, _dictPropertiesToGet[currentProperty].GetValue(obj, null).ToString(), true));
                                    }
                                    else
                                    {
                                        objValue = _dictPropertiesToGet[currentProperty].GetValue(obj, null);
                                    }
                                }
                                else
                                {
                                    objValue = _dictPropertiesToGet[currentProperty].GetValue(obj, null);
                                }
                                if (objValue != null)
                                {
                                    string value = objValue.ToString();
                                    if (!nullForEmptyString)
                                    {
                                        dr[currentProperty] = value;
                                    }
                                    else
                                    {
                                        if (String.IsNullOrEmpty(value))
                                        {
                                            dr[currentProperty] = DBNull.Value;
                                        }
                                        else
                                        {
                                            dr[currentProperty] = value;
                                        }
                                    }
                                }
                            }
                        }
                        dt.Rows.Add(dr);
                    }
                }
                #endregion
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

        /// <summary>
        /// http://stackoverflow.com/questions/2538477/changing-populated-datatable-column-data-types
        /// Change the datatype of datatable that is already loaded with data.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="columnname"></param>
        /// <param name="newtype"></param>
        /// <returns></returns>
        public static bool ChangeColumnDataType(DataTable table, string columnname, Type newtype)
        {
            try
            {
                if (table.Columns.Contains(columnname) == false)
                {
                    return false;
                }
                DataColumn column = table.Columns[columnname];
                if (column.DataType == newtype)
                {
                    return true;
                }
                try
                {
                    DataColumn newcolumn = new DataColumn("temporary", newtype);
                    table.Columns.Add(newcolumn);
                    foreach (DataRow row in table.Rows)
                    {
                        try
                        {
                            row["temporary"] = Convert.ChangeType(row[columnname], newtype);
                        }
                        catch
                        {
                        }
                    }
                    table.Columns.Remove(columnname);
                    newcolumn.ColumnName = columnname;
                }
                catch (Exception)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return false;
        }

        /// <summary>
        /// This methods sorts a generic dictionary by values that is passed in arguments.
        /// Added by Ankit Gupta on 14th Jan, 2015.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public static Dictionary<TKey, TValue> SortDictionaryByValues<TKey, TValue>(Dictionary<TKey, TValue> dictionary)
        {
            Dictionary<TKey, TValue> dict = new Dictionary<TKey, TValue>();
            try
            {
                if (dictionary != null && dictionary.Keys.Count > 0)
                {
                    IEnumerable<KeyValuePair<TKey, TValue>> sortedDict = from entry in dictionary orderby entry.Value ascending select entry;
                    dict = sortedDict.ToDictionary(pair => pair.Key, pair => pair.Value);
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
            return dict;
        }

        /// <summary>
        /// return IP Address 
        /// </summary>
        /// <returns></returns>
        public static string GetIPAddress()
        {
            string server = string.Empty;
            try
            {
                string strHostName = Dns.GetHostName();

                IPHostEntry ipEntry = Dns.GetHostEntry(strHostName);
                IPAddress[] aryLocalAddr = (from g in ipEntry.AddressList
                                            where g.AddressFamily == AddressFamily.InterNetwork
                                            select g).ToArray();
                if (aryLocalAddr.Count() > 1)
                {
                    server = aryLocalAddr[1].ToString();
                }
                else if (aryLocalAddr.Count() > 0)
                {
                    server = aryLocalAddr[0].ToString();
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
            return server;
        }
    }
}
