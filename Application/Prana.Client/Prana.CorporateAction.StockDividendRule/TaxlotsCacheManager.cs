using Prana.BusinessObjects;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Xml;

namespace Prana.CorporateAction.StockDividendRule
{
    class TaxlotsCacheManager
    {

        Dictionary<string, TaxlotBaseCollection> _caWiseTaxlots = new Dictionary<string, TaxlotBaseCollection>();
        Dictionary<string, string> _caIDWiseXML = new Dictionary<string, string>();
        Dictionary<string, DataRow> _caIDWiseRow = new Dictionary<string, DataRow>();

        #region Singleton Implementation
        private static TaxlotsCacheManager _taxlotsCacheManager = new TaxlotsCacheManager();

        internal static TaxlotsCacheManager Instance
        {
            get
            {
                return _taxlotsCacheManager;
            }
        }
        #endregion

        /// <summary>
        /// Add ca id wise affected taxlots info
        /// </summary>
        /// <param name="caID"></param>
        /// <param name="taxlots"></param>
        internal void AddTaxlots(string caID, TaxlotBaseCollection taxlots)
        {
            try
            {
                if (_caWiseTaxlots.ContainsKey(caID))
                {
                    _caWiseTaxlots[caID].AddRange(taxlots);
                }
                else
                {
                    _caWiseTaxlots.Add(caID, taxlots);
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

        internal Dictionary<string, TaxlotBaseCollection> GetCAWiseTaxlots()
        {
            return _caWiseTaxlots;
        }

        /// <summary>
        /// Save CA Id wise corporate action information
        /// </summary>
        /// <param name="caID"></param>
        /// <param name="caRow"></param>
        internal void AddCARow(string caID, DataRow caRow)
        {
            try
            {
                if (_caIDWiseRow.ContainsKey(caID))
                {
                    _caIDWiseRow[caID] = caRow;
                }
                else
                {
                    _caIDWiseRow.Add(caID, caRow);
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

        internal DataRow GetCARowByID(string caID)
        {
            try
            {
                if (_caIDWiseRow.ContainsKey(caID))
                {
                    return _caIDWiseRow[caID];
                }
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

        internal void ClearAll()
        {
            try
            {
                _caWiseTaxlots.Clear();
                _caIDWiseRow.Clear();
                _caIDWiseXML.Clear();
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


        internal void FillCAIDWiseXML(string caStr)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(caStr);
                XmlNodeList xmlCorporateActionNodeList = xmlDoc.SelectNodes("DocumentElement/CaFullTable");

                foreach (XmlNode xmlNode in xmlCorporateActionNodeList)
                {
                    string caIDStr = xmlNode.SelectSingleNode("CorpActionID").InnerText.ToString();

                    if (!_caIDWiseXML.ContainsKey(caIDStr))
                    {
                        _caIDWiseXML.Add(caIDStr, xmlNode.OuterXml);
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
        }

        internal string GetCAStrByID(string caID)
        {
            try
            {
                if (_caIDWiseXML.ContainsKey(caID))
                {
                    return _caIDWiseXML[caID];
                }
                else
                {
                    return string.Empty;
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

    }
}
