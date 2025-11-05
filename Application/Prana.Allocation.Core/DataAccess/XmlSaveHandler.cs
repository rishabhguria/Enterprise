// ***********************************************************************
// Assembly         : Prana.Allocation.Core
// Author           : dewashish
// Created          : 09-08-2014
//
// Last Modified By : dewashish
// Last Modified On : 09-08-2014
// ***********************************************************************
// <copyright file="XmlSaveHandler.cs" company="Nirvana">
//     Copyright (c) Nirvana. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Newtonsoft.Json;
using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.DatabaseManager;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.XMLUtilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
/// <summary>
/// The DataAccess namespace.
/// </summary>
namespace Prana.Allocation.Core.DataAccess
{
    /// <summary>
    /// Class XmlSaveHandler.
    /// </summary>
    public class XmlSaveHandler
    {
        /// <summary>
        /// The _deleted groups XML
        /// </summary>
        StringBuilder _deletedGroupsXML = new StringBuilder();
        /// <summary>
        /// The _ungrouped groups XML
        /// </summary>
        StringBuilder _ungroupedGroupsXML = new StringBuilder();
        /// <summary>
        /// The _new groups XML
        /// </summary>
        StringBuilder _newGroupsXML = new StringBuilder();
        /// <summary>
        /// The _re allocated XML
        /// </summary>
        StringBuilder _reAllocatedXML = new StringBuilder();
        /// <summary>
        /// The _new position XML
        /// </summary>
        StringBuilder _newPositionXml = new StringBuilder();
        /// <summary>
        /// The _corporate action XML
        /// </summary>
        StringBuilder _corporateActionXml = new StringBuilder();

        StringBuilder _taxlotToDelete = new StringBuilder();

        /// <summary>
        /// The cons t_ initialstrin g_ groups
        /// </summary>
        const string CONST_INITIALSTRING_GROUPS = "<Groups>";
        /// <summary>
        /// The cons t_ endstrin g_ groups
        /// </summary>
        const string CONST_ENDSTRING_GROUPS = "</Groups>";
        /// <summary>
        /// The cons t_ initialstrin g_ positions
        /// </summary>
        const string CONST_INITIALSTRING_POSITIONS = "<Positions>";

        const string CONST_INITIALSTRING_TAXLOTS = "<TaxLots>";
        const string CONST_ENDING_TAXLOTS = "</TaxLots>";

        /// <summary>
        /// stores error Number from database
        /// </summary>
        private static int _errorNumber = 0;

        /// <summary>
        /// stores error Message from database
        /// </summary>
        private static string _errorMessage = string.Empty;

        /// <summary>
        /// HeavySaveTimeout
        /// </summary>
        private readonly int _heavySaveTimeout = 300;

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlSaveHandler"/> class.
        /// </summary>
        public XmlSaveHandler()
        {
            /// Since this is in constructor, hence just assigned a value. Initial value would be assigned each time, whenever object of the class is created.
            //_newGroupsXML.Remove(0, _newGroupsXML.Length - 1);
            _newGroupsXML.Append(CONST_INITIALSTRING_GROUPS);
            _deletedGroupsXML.Append(CONST_INITIALSTRING_GROUPS);
            _ungroupedGroupsXML.Append(CONST_INITIALSTRING_GROUPS);
            _reAllocatedXML.Append(CONST_INITIALSTRING_GROUPS);
            _newPositionXml.Append(CONST_INITIALSTRING_POSITIONS);
            _corporateActionXml.Append(CONST_INITIALSTRING_GROUPS);
            _taxlotToDelete.Append(CONST_INITIALSTRING_TAXLOTS);
            _heavySaveTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["HeavySaveTimeout"]);
        }

        /// <summary>
        /// Saves the group XMLS.
        /// </summary>
        /// <param name="connString">The connection string.</param>
        /// <returns>System.Int32.</returns>
        public int SaveGroupXmls(string connString)
        {
            SqlConnection conn = DatabaseManager.DatabaseManager.CreateConnection(connString);

            int rowsAffected = 0;
            SqlTransaction transaction = null;

            try
            {
                using (conn)
                {
                    conn.Open();

                    transaction = DatabaseManager.DatabaseManager.BeginTransaction(conn);

                    QueryData queryData = new QueryData();

                    /// every string builder is initialized by the CONST_INITIALSTRING_GROUPS hence treating it as initial length
                    int initialLength = CONST_INITIALSTRING_GROUPS.Length;
                    ///Applid logic on the length as earlier the logic was like _updatedGroupsXML.ToString() != CONST_INITIALSTRING_GROUPS, and it was converting the 
                    ///big sb into string first to compare. Now it would save some memory due to avoiding the creation of the string.
                    if (_newGroupsXML.Length > initialLength)
                    {
                        _newGroupsXML.Append(CONST_ENDSTRING_GROUPS);
                        queryData.StoredProcedureName = "P_SaveGroup_XML";
                        queryData.DictionaryDatabaseParameter.Add("@Xml", new DatabaseParameter()
                        {
                            IsOutParameter = false,
                            ParameterName = "@Xml",
                            ParameterType = DbType.String,
                            ParameterValue = _newGroupsXML.ToString()
                        });
                        AddOutErrorParameters(queryData);
                    }
                    if (_ungroupedGroupsXML.Length > initialLength)
                    {
                        _ungroupedGroupsXML.Append(CONST_ENDSTRING_GROUPS);
                        queryData.StoredProcedureName = "P_DeleteGroups";
                        queryData.DictionaryDatabaseParameter.Add("@Xml", new DatabaseParameter()
                        {
                            IsOutParameter = false,
                            ParameterName = "@Xml",
                            ParameterType = DbType.String,
                            ParameterValue = _ungroupedGroupsXML.ToString()
                        });
                        AddOutErrorParameters(queryData);
                        //db.AddInParameter(commandDelete, "@groupXml", DbType.String, _ungroupedGroupsXML);
                        //AddOutErrorParameters(db, command);

                    }
                    if (_deletedGroupsXML.Length > initialLength)
                    {
                        _deletedGroupsXML.Append(CONST_ENDSTRING_GROUPS);
                        queryData.StoredProcedureName = "P_DeleteOmittedGroups";
                        queryData.DictionaryDatabaseParameter.Add("@Xml", new DatabaseParameter()
                        {
                            IsOutParameter = false,
                            ParameterName = "@Xml",
                            ParameterType = DbType.String,
                            ParameterValue = _deletedGroupsXML.ToString()
                        });
                        AddOutErrorParameters(queryData);
                    }
                    if (_reAllocatedXML.Length > initialLength)
                    {
                        _reAllocatedXML.Append(CONST_ENDSTRING_GROUPS);
                        queryData.StoredProcedureName = "P_ReAllocateGroup_XML";
                        queryData.DictionaryDatabaseParameter.Add("@Xml", new DatabaseParameter()
                        {
                            IsOutParameter = false,
                            ParameterName = "@Xml",
                            ParameterType = DbType.String,
                            ParameterValue = _reAllocatedXML.ToString()
                        });
                        AddOutErrorParameters(queryData);
                    }
                    if (_taxlotToDelete.Length > CONST_INITIALSTRING_TAXLOTS.Length)
                    {
                        _taxlotToDelete.Append(CONST_ENDING_TAXLOTS);
                        queryData.StoredProcedureName = "P_CleanDeletedTaxlots";
                        queryData.DictionaryDatabaseParameter.Add("@Xml", new DatabaseParameter()
                        {
                            IsOutParameter = false,
                            ParameterName = "@Xml",
                            ParameterType = DbType.String,
                            ParameterValue = _taxlotToDelete.ToString()
                        });
                        AddOutErrorParameters(queryData);
                    }
                    if (queryData.DictionaryDatabaseParameter.Count > 0)
                    {
                        rowsAffected = DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData, string.Empty, transaction);
                        //done changes to get error returned from failure of query run in database and log into server log, PRANA-3596
                        XMLSaveManager.GetErrorParameterValues(ref _errorMessage, ref _errorNumber, queryData.DictionaryDatabaseParameter);
                    }

                    transaction.Commit();
                    //transaction.Rollback();
                }

            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(_reAllocatedXML);
                sb.AppendLine();
                sb.Append(_newGroupsXML);

                Logger.HandleException(new Exception(sb.ToString() + "Error Message=" + _errorMessage), LoggingConstants.POLICY_LOGANDSHOW);
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                // transaction.Rollback();
                if (rethrow)
                {
                    throw;
                }
                sb = null;
            }
            finally
            {

                ClearXmls();
            }
            return rowsAffected;
        }

        /// <summary>
        /// Saves the group XMLS.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public int SaveGroupXmls()
        {
            int rowsAffected = 0;
            DbTransaction transaction = null;

            try
            {
                DbConnection conn = DatabaseManager.DatabaseManager.CreateConnection();

                using (conn)
                {
                    conn.Open();
                    transaction = conn.BeginTransaction();
                    /// every string builder is initialized by the CONST_INITIALSTRING_GROUPS hence treating it as initial length
                    int initialLength = CONST_INITIALSTRING_GROUPS.Length;
                    ///Applid logic on the length as earlier the logic was like _updatedGroupsXML.ToString() != CONST_INITIALSTRING_GROUPS, and it was converting the 
                    ///big sb into string first to compare. Now it would save some memory due to avoiding the creation of the string.

                    //done changes to get error returned from failure of query run in database and log into server log, PRANA-3596
                    if (_newGroupsXML.Length > initialLength)
                    {
                        _newGroupsXML.Append(CONST_ENDSTRING_GROUPS);

                        QueryData queryData = new QueryData();
                        queryData.CommandTimeout = 50000;
                        queryData.StoredProcedureName = "P_SaveGroup_XML";
                        queryData.DictionaryDatabaseParameter.Add("@Xml", new DatabaseParameter()
                        {
                            IsOutParameter = false,
                            ParameterName = "@Xml",
                            ParameterType = DbType.String,
                            ParameterValue = _newGroupsXML.ToString()
                        });
                        AddOutErrorParameters(queryData);

                        rowsAffected = DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData, string.Empty, transaction);
                        XMLSaveManager.GetErrorParameterValues(ref _errorMessage, ref _errorNumber, queryData.DictionaryDatabaseParameter);
                    }
                    if (_ungroupedGroupsXML.Length > initialLength)
                    {
                        _ungroupedGroupsXML.Append(CONST_ENDSTRING_GROUPS);

                        QueryData queryData = new QueryData();
                        queryData.StoredProcedureName = "P_DeleteGroups";
                        queryData.CommandTimeout = 3000;
                        queryData.DictionaryDatabaseParameter.Add("@groupXml", new DatabaseParameter()
                        {
                            IsOutParameter = false,
                            ParameterName = "@groupXml",
                            ParameterType = DbType.String,
                            ParameterValue = _ungroupedGroupsXML.ToString()
                        });

                        AddOutErrorParameters(queryData);

                        rowsAffected = DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData, string.Empty, transaction);
                        XMLSaveManager.GetErrorParameterValues(ref _errorMessage, ref _errorNumber, queryData.DictionaryDatabaseParameter);
                    }
                    if (_deletedGroupsXML.Length > initialLength)
                    {
                        _deletedGroupsXML.Append(CONST_ENDSTRING_GROUPS);

                        QueryData queryData = new QueryData();
                        queryData.StoredProcedureName = "P_DeleteOmittedGroups";
                        queryData.CommandTimeout = _heavySaveTimeout;
                        queryData.DictionaryDatabaseParameter.Add("@Xml", new DatabaseParameter()
                        {
                            IsOutParameter = false,
                            ParameterName = "@Xml",
                            ParameterType = DbType.String,
                            ParameterValue = _deletedGroupsXML.ToString()
                        });

                        AddOutErrorParameters(queryData);

                        rowsAffected = DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData, string.Empty, transaction);
                        XMLSaveManager.GetErrorParameterValues(ref _errorMessage, ref _errorNumber, queryData.DictionaryDatabaseParameter);
                    }
                    if (_reAllocatedXML.Length > initialLength)
                    {
                        _reAllocatedXML.Append(CONST_ENDSTRING_GROUPS);

                        QueryData queryData = new QueryData();
                        queryData.StoredProcedureName = "P_ReAllocateGroup_XML";
                        queryData.CommandTimeout = _heavySaveTimeout;
                        queryData.DictionaryDatabaseParameter.Add("@Xml", new DatabaseParameter()
                        {
                            IsOutParameter = false,
                            ParameterName = "@Xml",
                            ParameterType = DbType.String,
                            ParameterValue = _reAllocatedXML.ToString()
                        });

                        AddOutErrorParameters(queryData);

                        rowsAffected = DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData, string.Empty, transaction);
                        XMLSaveManager.GetErrorParameterValues(ref _errorMessage, ref _errorNumber, queryData.DictionaryDatabaseParameter);
                    }
                    if (_taxlotToDelete.Length > CONST_INITIALSTRING_TAXLOTS.Length)
                    {
                        _taxlotToDelete.Append(CONST_ENDING_TAXLOTS);

                        QueryData queryData = new QueryData();
                        queryData.StoredProcedureName = "P_CleanDeletedTaxlots";
                        queryData.CommandTimeout = _heavySaveTimeout;
                        queryData.DictionaryDatabaseParameter.Add("@Xml", new DatabaseParameter()
                        {
                            IsOutParameter = false,
                            ParameterName = "@Xml",
                            ParameterType = DbType.String,
                            ParameterValue = _taxlotToDelete.ToString()
                        });

                        AddOutErrorParameters(queryData);

                        rowsAffected = DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData, string.Empty, transaction);
                        XMLSaveManager.GetErrorParameterValues(ref _errorMessage, ref _errorNumber, queryData.DictionaryDatabaseParameter);
                    }
                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(_reAllocatedXML);
                sb.AppendLine();
                sb.Append(_newGroupsXML);

                Logger.HandleException(new Exception(sb.ToString() + "Error Message=" + _errorMessage), LoggingConstants.POLICY_LOGANDSHOW);
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                // transaction.Rollback();
                if (rethrow)
                {
                    throw;
                }
                sb = null;
            }
            finally
            {
                ClearXmls();
            }
            return rowsAffected;
        }

        /// <summary>
        /// Adds the out parameters.
        /// </summary>
        /// <param name="db">The db.</param>
        /// <param name="commandSP">The command SP.</param>
        private void AddOutErrorParameters(QueryData queryData)
        {
            queryData.DictionaryDatabaseParameter.Add("@ErrorMessage", new DatabaseParameter()
            {
                IsOutParameter = true,
                ParameterName = "@ErrorMessage",
                ParameterType = DbType.String,
                OutParameterSize = -1
            });

            queryData.DictionaryDatabaseParameter.Add("@ErrorNumber", new DatabaseParameter()
            {
                IsOutParameter = true,
                ParameterName = "@ErrorNumber",
                ParameterType = DbType.Int32,
                ParameterValue = 0,
                OutParameterSize = sizeof(Int32)
            });
        }

        /// <summary>
        /// Clears the corporate action XML.
        /// </summary>
        public void ClearCorporateActionXML()
        {
            _corporateActionXml.Remove(0, _corporateActionXml.Length);
            _corporateActionXml.Append(CONST_INITIALSTRING_GROUPS);
        }

        /// <summary>
        /// Clears the XMLS.
        /// </summary>
        public void ClearXmls()
        {
            try
            {
                _newGroupsXML = null;
                //_newGroupsXML.Remove(0, _newGroupsXML.Length);
                _newGroupsXML = new StringBuilder();
                _newGroupsXML.Append(CONST_INITIALSTRING_GROUPS);

                //_deletedGroupsXML.Remove(0, _deletedGroupsXML.Length);
                _deletedGroupsXML = null;
                _deletedGroupsXML = new StringBuilder();
                _deletedGroupsXML.Append(CONST_INITIALSTRING_GROUPS);

                _ungroupedGroupsXML = null;
                //_ungroupedGroupsXML.Remove(0, _ungroupedGroupsXML.Length);
                _ungroupedGroupsXML = new StringBuilder();
                _ungroupedGroupsXML.Append(CONST_INITIALSTRING_GROUPS);

                //_newPositionXml.Remove(0, _newPositionXml.Length);
                _newPositionXml = null;
                _newPositionXml = new StringBuilder();

                //_reAllocatedXML.Remove(0, _reAllocatedXML.Length);
                _reAllocatedXML = null;
                _reAllocatedXML = new StringBuilder();
                _reAllocatedXML.Append(CONST_INITIALSTRING_GROUPS);

                _taxlotToDelete = null;
                _taxlotToDelete = new StringBuilder();
                _taxlotToDelete.Append(CONST_INITIALSTRING_TAXLOTS);
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

        /// <summary>
        /// Creates the XMLS.
        /// </summary>
        /// <param name="group">The group.</param>
        public void CreateXmls(AllocationGroup group)
        {

            try
            {
                CustomXmlSerializer _Xml = new CustomXmlSerializer();

                switch (group.PersistenceStatus)
                {
                    case ApplicationConstants.PersistenceStatus.UnGrouped:
                        _ungroupedGroupsXML.Append("<AllocationGroup GroupID =\"");
                        _ungroupedGroupsXML.Append(group.GroupID);
                        _ungroupedGroupsXML.Append("\"/>");
                        break;
                    case ApplicationConstants.PersistenceStatus.New:
                    case ApplicationConstants.PersistenceStatus.CorporateAction:
                        _newGroupsXML.Append(_Xml.WriteString(group));
                        break;
                    case ApplicationConstants.PersistenceStatus.Deleted:
                        _deletedGroupsXML.Append("<AllocationGroup GroupID =\"");
                        _deletedGroupsXML.Append(group.GroupID);
                        _deletedGroupsXML.Append("\"/>");
                        break;

                    case ApplicationConstants.PersistenceStatus.ReAllocated:
                        _reAllocatedXML.Append(_Xml.WriteString(group));
                        foreach (TaxLot item in group.TaxLots)
                        {
                            _taxlotToDelete.Append(String.Format("<TaxLot TaxLotID =\"{0}\"/>", item.TaxLotID));
                        }
                        var updatedXml = _reAllocatedXML.Append("</Groups>").ToString();
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(updatedXml);

                        ReplaceTradeAttributes(doc.DocumentElement);
                        string result = doc.OuterXml;
                        int lastIndex = result.LastIndexOf("</Groups>");
                        if (lastIndex >= 0)
                        {
                            result = result.Substring(0, lastIndex);
                        }
                        _reAllocatedXML = _reAllocatedXML.Clear();
                        _reAllocatedXML.Append(result);
                        break;
                    case ApplicationConstants.PersistenceStatus.NotChanged:
                        break;
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

        /// <summary>
        /// This method replace individual tags for trade attributes (7-45) with a single tag
        /// </summary>
        /// <param name="node"></param>
        private void ReplaceTradeAttributes(XmlNode node)
        {
            try
            {
                if (node == null) return;

                // Work on a copy of child nodes to avoid collection modification issues
                var childNodes = node.ChildNodes.Cast<XmlNode>().ToList();

                var tradeAttributes = new List<XmlNode>();

                foreach (var child in childNodes)
                {
                    // Check if the node name matches TradeAttribute[7-45]
                    if (child.Name.StartsWith("TradeAttribute"))
                    {
                        if (int.TryParse(child.Name.Substring("TradeAttribute".Length), out int num) &&
                            num >= 7 && num <= 45)
                        {
                            tradeAttributes.Add(child);
                        }
                    }
                }

                if (tradeAttributes.Any())
                {
                    var attributeList = new List<Dictionary<string, string>>();

                    foreach (var attr in tradeAttributes)
                    {
                        if (!string.IsNullOrEmpty(attr.InnerText))
                        {
                            attributeList.Add(new Dictionary<string, string>
                        {
                            { "Name", attr.Name },
                            { "Value", attr.InnerText }
                        });
                        }
                        node.RemoveChild(attr); // Remove old element
                    }

                    string json = JsonConvert.SerializeObject(attributeList);

                    XmlElement newElement = node.OwnerDocument.CreateElement("AdditionalTradeAttributes");
                    newElement.InnerText = json;
                    node.AppendChild(newElement);
                }

                // Recurse into children
                foreach (XmlNode child in node.ChildNodes)
                {
                    ReplaceTradeAttributes(child);
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
        /// Gets the and clear corporate action XML.
        /// </summary>
        /// <returns>System.String.</returns>
        public string GetAndClearCorporateActionXML()
        {
            try
            {
                //if (corporateActionXml != CONST_INITIALSTRING_GROUPS)
                //{
                _corporateActionXml.Append(CONST_ENDSTRING_GROUPS);
                //}
                return _corporateActionXml.ToString();
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
                return string.Empty;
            }
            finally
            {

                ClearCorporateActionXML();
            }
        }

        /// <summary>
        /// Creates the positions XML.
        /// </summary>
        /// <param name="pmList">The pm list.</param>
        public void CreatePositionsXml(IList pmList)
        {
            try
            {
                _newPositionXml.Remove(0, _newPositionXml.Length);
                _newPositionXml.Append(XMLUtilities.SerializeToXML(pmList));
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

        /// <summary>
        /// Saves the position throuh XML.
        /// </summary>
        /// <returns></returns>
        public int SavePositionThrouhXml()
        {
            int rowsAffected = 0;
            try
            {
                rowsAffected = XMLSaveManager.SaveThroughXML("P_SaveExternalGroup", _newPositionXml.ToString());
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
            finally
            {
                ClearXmls();
            }
            return rowsAffected;

        }

        /// <summary>
        /// Saves the position throuh XML.
        /// </summary>
        /// <param name="connStr">The connection string.</param>
        /// <returns></returns>
        public int SavePositionThrouhXml(string connStr)
        {
            int rowsAffected = 0;
            try
            {
                rowsAffected = XMLSaveManager.SaveThroughXML("P_SaveExternalGroup", _newPositionXml.ToString(), connStr);
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
            finally
            {
                ClearXmls();
            }
            return rowsAffected;

        }
    }
}
