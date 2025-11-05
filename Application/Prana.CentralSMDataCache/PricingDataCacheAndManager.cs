using Prana.CentralSMDataCache.DAL;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.BusinessObjects.NewLiveFeed;
using Prana.Global;
using Prana.Global.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Prana.CentralSMDataCache
{
    public class PricingDataCacheAndManager
    {
        #region singleton

        private static volatile PricingDataCacheAndManager instance;
        private static object syncRoot = new Object();

        private PricingDataCacheAndManager() { }

        public static PricingDataCacheAndManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new PricingDataCacheAndManager();
                    }
                }

                return instance;
            }
        }
        #endregion

        /// <summary>
        /// Keeps the cache of pricing data. Data when fetched from DB or Bloomberg is filled in this cache.
        /// key=symbolPK+Date+DataSource ,fields,value
        /// </summary>
        ConcurrentDictionary<string, ConcurrentDictionary<string, string>> _pricingDataCache = new ConcurrentDictionary<string, ConcurrentDictionary<string, string>>();

        public ConcurrentDictionary<string, ConcurrentDictionary<string, string>> PricingDataCache
        {
            get { return _pricingDataCache; }
            private set { _pricingDataCache = value; }
        }

        /// <summary>
        /// keeps the cache of keys which are in process to be fetched from the Bloomberg/CentralServer
        /// </summary>
        ConcurrentDictionary<string, ConcurrentDictionary<string, string>> _requestedInProcessSymbolsCache = new ConcurrentDictionary<string, ConcurrentDictionary<string, string>>();

        /// <summary>
        /// keeps the cache of keys which are in process to be fetched from the Bloomberg/CentralServer. Key,Field,BloombergRequestId through which it is in process.
        /// </summary>
        public ConcurrentDictionary<string, ConcurrentDictionary<string, string>> RequestedInProcessSymbolsCache
        {
            get { return _requestedInProcessSymbolsCache; }
            private set { _requestedInProcessSymbolsCache = value; }
        }

        public void FillDataFromPricingCacheAndDB(PricingRequestMappings mappingObjForRequest)
        {
            try
            {
                //RequestedInProcessSymbolsCache
                HashSet<Tuple<string, string, string, String>> symbolsNotFound = new HashSet<Tuple<string, string, string, string>>();
                HashSet<string> fieldsNotFound = new HashSet<string>();

                foreach (String key in mappingObjForRequest.RequestKeyMapping)
                {
                    List<string> keyComponents = EscapedDelimiter.SplitDelimitedString(key);
                    if (PricingDataCache.ContainsKey(key))
                    {
                        mappingObjForRequest.ResponseTable.AsEnumerable().Where(x => x["SymbolPK"].ToString().Equals(keyComponents[0], StringComparison.InvariantCultureIgnoreCase) && x["Date"].ToString().Equals(keyComponents[1], StringComparison.InvariantCultureIgnoreCase) && x["DataSource"].ToString().Equals(keyComponents[2], StringComparison.InvariantCultureIgnoreCase) && x["SecondarySource"].ToString().Equals(keyComponents[3], StringComparison.InvariantCultureIgnoreCase)).ToList<DataRow>().ForEach(r =>
                        {
                            foreach (DataColumn dataCol in r.Table.Columns)
                            {
                                if (r[dataCol] == null || r[dataCol] == DBNull.Value)
                                {
                                    if (PricingDataCache[key].ContainsKey(dataCol.ColumnName))
                                    {
                                        r[dataCol] = PricingDataCache[key][dataCol.ColumnName];
                                    }
                                    else
                                    {
                                        symbolsNotFound.Add(new Tuple<string, string, string, string>(keyComponents[0], keyComponents[2], keyComponents[1], keyComponents[3]));
                                        fieldsNotFound.Add(dataCol.ColumnName);
                                    }
                                }
                            }
                        });
                    }
                    else
                    {
                        symbolsNotFound.Add(new Tuple<string, string, string, string>(keyComponents[0], keyComponents[2], keyComponents[1], keyComponents[3]));
                        fieldsNotFound.UnionWith(mappingObjForRequest.ResponseTable.Columns.Cast<DataColumn>().Select(x => x.ColumnName).Except(new List<string>() { "Symbol","Date","Symbology","SymbolPK","DataSource","SecondarySource"}));
                    }
                }
                Boolean allDataPresent = false;
                if (symbolsNotFound.Count > 0)
                {
                    string param1ForSP = XmlForQuery(symbolsNotFound);
                    string param2ForSP = string.Join(",", fieldsNotFound.ToArray());
                    DataSet resultFromDB = CentralSMMiscDBRequestManager.Instance.GetSecurityPricingData(param1ForSP, param2ForSP);
                    if (resultFromDB.Tables.Count > 0 && resultFromDB.Tables[0].Rows.Count>0)
                        AddDataTableFromDBInPricingCacheAndResponseTable(resultFromDB.Tables[0], mappingObjForRequest);
                    if (HasNull(mappingObjForRequest.ResponseTable))
                        allDataPresent = false;
                    else
                        allDataPresent = true;
                }
                else
                {
                    allDataPresent = true;
                }
                mappingObjForRequest.IsFull = allDataPresent;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public static bool HasNull(DataTable table)
        {
            try
            {
                foreach (DataColumn column in table.Columns)
                {
                    if (table.Rows.OfType<DataRow>().Any(r => r.IsNull(column)))
                        return true;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return false;
        }

        string XmlForQuery(HashSet<Tuple<string, string, string, string>> lst)
        {
            string name = "Row";

            XElement root = new XElement("Rowset");
            try
            {
                int cnt = 0;
                foreach (var item in lst)
                {
                    XElement row = new XElement(name);
                    root.Add(row);
                    {
                        row.Add(new XElement("Symbol_PK", item.Item1));
                        row.Add(new XElement("Source", item.Item2));
                        row.Add(new XElement("Date", item.Item3));
                       //modified by omshiv, Added secondary pricing source 
                        row.Add(new XElement("SecondarySource", item.Item4));
                    }
                    cnt++;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return root.ToString();
        }

        /// <summary>
        /// Updates cache from the table received from DB. Does not update already present entries.
        /// </summary>
        /// <param name="tableFromDB"></param>
        private void AddDataTableFromDBInPricingCacheAndResponseTable(DataTable tableFromDB, PricingRequestMappings mappingObjForRequest)
        {
            try
            {
                if (tableFromDB.Columns.Contains("Symbol_PK") && tableFromDB.Columns.Contains("Date") && tableFromDB.Columns.Contains("Source"))
                {
                    foreach (DataRow dr in tableFromDB.Rows)
                    {
                        if (string.IsNullOrWhiteSpace(dr["PricingXML"].ToString()))
                            continue;

                       //modified by omshiv, changed the request key by adding pricing source
                        DateTime date = DateTime.Now;
                        bool isParsed = DateTime.TryParse(dr["Date"].ToString(), out date);
                        string key = EscapedDelimiter.CombineStrings(dr["Symbol_PK"].ToString(), date.ToString("yyyy-MM-dd HH:mm:ss.000"), Prana.BusinessObjects.SecMasterConstants.SecMasterSourceOfData.BloombergDLWS.ToString(), dr["SecondarySource"].ToString());

                         
                        if (!PricingDataCache.ContainsKey(key))
                        {
                            PricingDataCache.TryAdd(key, new ConcurrentDictionary<string, string>());
                        }
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(dr["PricingXML"].ToString());
                        XmlNode node = xmlDoc.SelectSingleNode("Fields");
                        foreach (XmlNode colNode in node.ChildNodes)
                        {
                            string fieldName = colNode.Name;
                            string fieldValue = colNode.InnerText;
                            if (PricingDataCache[key].ContainsKey(fieldName.Trim()))
                            {
                                if (String.IsNullOrWhiteSpace(PricingDataCache[key][fieldName.Trim()]))
                                    PricingDataCache[key][fieldName.Trim()] = fieldValue.Trim();
                            }
                            else
                            {
                                PricingDataCache[key].TryAdd(fieldName.Trim(), fieldValue.Trim());
                            }
                        }
                        mappingObjForRequest.ResponseTable.AsEnumerable().Where(x => x["SymbolPK"].ToString().Equals(dr["Symbol_PK"].ToString(), StringComparison.InvariantCultureIgnoreCase) && x["Date"].ToString().Equals(date.ToString("yyyy-MM-dd HH:mm:ss.000"), StringComparison.InvariantCultureIgnoreCase) && x["DataSource"].ToString().Equals(dr["Source"].ToString(), StringComparison.InvariantCultureIgnoreCase)).ToList<DataRow>().ForEach(r =>
                        {
                            foreach (DataColumn dataCol in r.Table.Columns)
                            {
                                if (r[dataCol] == null || r[dataCol] == DBNull.Value)
                                {
                                    if (PricingDataCache[key].ContainsKey(dataCol.ColumnName))
                                    {
                                        r[dataCol] = PricingDataCache[key][dataCol.ColumnName];
                                    }
                                }
                            }
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Updates the pricingDataMasterCache for the symbols validated
        /// </summary>
        /// <param name="tableFromBloomberg"></param>
        public void UpdateDBandCacheOnResponse(DataTable tableFromBloomberg)
        {
            try
            {
                if (tableFromBloomberg.Rows.Count == 0)
                    return;
                List<string> colOtherThanFields = new List<string>() { "Symbol", "Date", "Symbology", "SymbolPK", "DataSource", "SecondarySource" };
                foreach (DataRow row in tableFromBloomberg.Rows)
                {
                    if (row["Date"].ToString().StartsWith("1/1/0001"))
                        continue;
                    //modified by omshiv, changed the request key by adding pricing source
                    DateTime date = DateTime.Now;
                    bool isParsed = DateTime.TryParse(row["Date"].ToString(), out date);
                    string key = EscapedDelimiter.CombineStrings(row["SymbolPK"].ToString(), date.ToString("yyyy-MM-dd HH:mm:ss.000"), Prana.BusinessObjects.SecMasterConstants.SecMasterSourceOfData.BloombergDLWS.ToString(), row["SecondarySource"].ToString());

                    
                   
                    if (!PricingDataCache.ContainsKey(key))
                    {
                        PricingDataCache.TryAdd(key, new ConcurrentDictionary<string, string>());
                    }

                    foreach (DataColumn col in tableFromBloomberg.Columns)
                    {
                        if (!colOtherThanFields.Contains(col.ColumnName))
                        {
                            if (row[col.ColumnName] != null && row[col.ColumnName] != DBNull.Value && !String.IsNullOrWhiteSpace(row[col.ColumnName].ToString()))
                            {
                                string fieldName = col.ColumnName;
                                string fieldValue = row[col.ColumnName].ToString();
                                if (PricingDataCache[key].ContainsKey(fieldName.Trim()))
                                {
                                    if (String.IsNullOrWhiteSpace(PricingDataCache[key][fieldName.Trim()]))
                                        PricingDataCache[key][fieldName.Trim()] = fieldValue.Trim();
                                }
                                else
                                {
                                    PricingDataCache[key].TryAdd(fieldName.Trim(), fieldValue.Trim());
                                }
                            }
                        }
                    }
                }
                DataSet ds = new DataSet("PricingTable");
                ds.Tables.Add(tableFromBloomberg);
                tableFromBloomberg.TableName = "PricingData";
                XElement xelem = new XElement("PricingTable");
                using (XmlWriter w = xelem.CreateWriter())
                {
                    tableFromBloomberg.DataSet.WriteXml(w, System.Data.XmlWriteMode.IgnoreSchema);
                }
                
                IEnumerable<XElement> symbols = xelem.Elements().First().Elements();
                DataSet dsToSaveinDB = new DataSet("PricingTable");
                DataTable dtSaveToDB =dsToSaveinDB.Tables.Add("PricingData");
                dtSaveToDB.Columns.Add("Symbol_PK",typeof(string));
                dtSaveToDB.Columns.Add("Source",typeof(string));
                dtSaveToDB.Columns.Add("Date", typeof(string));
                dtSaveToDB.Columns.Add("NirvanaSymbol", typeof(string));
                dtSaveToDB.Columns.Add("PrimarySymbology", typeof(string));
                dtSaveToDB.Columns.Add("Fields", typeof(string));
                dtSaveToDB.Columns.Add("SecondarySource", typeof(string));
                foreach (XElement symbolRow in symbols)
                {
                    if (symbolRow.Element("Date").Value.StartsWith("1/1/0001"))
                        continue;
                    DataRow dr = dtSaveToDB.NewRow();
                    dr["Symbol_PK"] = symbolRow.Element("SymbolPK").Value;
                    dr["Source"] = Prana.BusinessObjects.SecMasterConstants.SecMasterSourceOfData.BloombergDLWS.ToString();
                    dr["Date"] = symbolRow.Element("Date").Value;
                    dr["NirvanaSymbol"] = symbolRow.Element("Symbol").Value;
                    dr["PrimarySymbology"] = symbolRow.Element("Symbology").Value;
                    //modified by omshiv, added column for secondary source
                    dr["SecondarySource"] = symbolRow.Element("SecondarySource").Value;
      
                    StringBuilder strB = new StringBuilder();
                    foreach (XElement elem in symbolRow.Elements().Where(e => (!colOtherThanFields.Contains(e.Name.ToString()))))
                    {
                        if (!String.IsNullOrWhiteSpace(elem.Value))
                            strB.Append(elem.ToString());
                    }
                    dr["Fields"] = strB.ToString();
                    if (String.IsNullOrWhiteSpace(dr["Fields"].ToString()))
                        continue;
                    dtSaveToDB.Rows.Add(dr);
                }
                StringBuilder xmlString =new StringBuilder(dsToSaveinDB.GetXml());
                xmlString=xmlString.Replace("&lt;", "<");
                xmlString=xmlString.Replace("&gt;", ">");
                CentralSMMiscDBRequestManager.SaveGenericPricingData(xmlString.ToString());
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }




    }
}
