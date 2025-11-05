using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.ClientCommon;
using Prana.LogManager;
using Prana.Utilities.UI.MiscUtilities;
using Prana.Utilities.UI.UIUtilities;
using Prana.Utilities.XMLUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Xml;



namespace Prana.CorporateActionNew.Classes
{
    internal class CommonHelper
    {
        private static int _requesterHashCode;

        public static int REQUESTER_HASHCODE
        {
            get { return _requesterHashCode; }
            set { _requesterHashCode = value; }
        }

        internal static void BindCACombo(UltraCombo cmbCA)
        {
            try
            {
                List<EnumerationValue> corpActions = ClientEnumHelper.ConvertEnumForBindingWithSelectValueAndCaptionSortedByCaption(typeof(CorporateActionType));// EnumHelper.ConvertEnumForBindingWithSelectValue(typeof(CorporateActionType));
                List<EnumerationValue> corpActionsToRemove = new List<EnumerationValue>();

                foreach (EnumerationValue eVal in corpActions)
                {
                    int corpAction = (int)eVal.Value;
                    if (corpAction.Equals((int)CorporateActionType.All))
                    {
                        corpActionsToRemove.Add(eVal);
                    }
                }

                foreach (EnumerationValue enumValToRemove in corpActionsToRemove)
                {
                    corpActions.Remove(enumValToRemove);
                }

                cmbCA.DataSource = corpActions;
                cmbCA.DisplayMember = "DisplayText";
                cmbCA.ValueMember = "Value";
                Utils.UltraComboFilter(cmbCA, "DisplayText");
                cmbCA.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
                cmbCA.SelectedRow = cmbCA.Rows[1];
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

        internal static void BindCAComboWithAll(UltraCombo cmbCA)
        {
            try
            {
                List<EnumerationValue> corpActions = ClientEnumHelper.ConvertEnumForBindingWithSelectValueAndCaptionSortedByCaption(typeof(CorporateActionType));
                //List<EnumerationValue> corpActionsToRemove = new List<EnumerationValue>();

                //foreach (EnumerationValue eVal in corpActions)
                //{
                //    int corpAction = (int)eVal.Value;
                //    if (corpAction.Equals((int)CorporateActionType.Merger) || corpAction.Equals((int)CorporateActionType.SpinOff) || corpAction.Equals((int)CorporateActionType.StockDividend))
                //    {
                //        corpActionsToRemove.Add(eVal);
                //    }
                //}

                //foreach (EnumerationValue enumValToRemove in corpActionsToRemove)
                //{
                //    corpActions.Remove(enumValToRemove);
                //}

                cmbCA.DataSource = corpActions;
                cmbCA.DisplayMember = "DisplayText";
                cmbCA.ValueMember = "Value";
                Utils.UltraComboFilter(cmbCA, "DisplayText");
                cmbCA.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
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
            cmbCA.SelectedRow = cmbCA.Rows[1];
        }

        internal static string GetCorporateActionString(DataTable dt, CorporateActionType caType)
        {
            TextWriter tw = new StringWriter();
            ///While writing to xml we don't want the time information but just the date, so we have created a special class for the same.

            ///Should be able to pick up all date properties from xml rathern than hard coding these here.
            List<string> dateProperties = new List<string>();
            if (caType.Equals(CorporateActionType.CashDividend) || caType.Equals(CorporateActionType.StockDividend))
            {
                dt.Rows[0]["EffectiveDate"] = dt.Rows[0]["ExDivDate"];
                dt.AcceptChanges();
            }
            dateProperties.Add("EffectiveDate");
            dateProperties.Add("DivDeclarationDate");
            dateProperties.Add("ExDivDate");
            dateProperties.Add("RecordDate");
            dateProperties.Add("DivPayoutDate");

            DsDateFilterXmlTextWriter writer = new DsDateFilterXmlTextWriter(tw, dateProperties);
            dt.WriteXml(writer, true);
            string caXml = tw.ToString();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(caXml);

            return xmlDoc.InnerXml;
        }

        //internal static string GetCorporateActionString(CorporateActionType corporateActionType, DataRowCollection rows)
        //{
        //    DataTable dt = rows[0].Table;
        //    List<DataColumn> corporateActionColoumnsList = XMLCacheManager.GetCorporateActionColumnList(dt, corporateActionType);
        //    List<Dictionary<string, string>> dicPranaFixFields = Transformer.GetFixFieldsFromDataColoumnList(corporateActionColoumnsList, rows);
        //    List<List<MessageField>> corporateActionList = GetCorporateActionList(dicPranaFixFields);
        //    return GetXMLFromMsgFieldList(corporateActionList);
        //}

        //private static List<List<MessageField>> GetCorporateActionList(List<Dictionary<string, string>> listOfdictPranaFixFields)
        //{

        //    List<List<MessageField>> listOfMsgFields = new List<List<MessageField>>();

        //    try
        //    {
        //        foreach (Dictionary<string, string> dictPranaFixFields in listOfdictPranaFixFields)
        //        {
        //            List<MessageField> CorporateActionInfoList = new List<MessageField>();
        //            foreach (KeyValuePair<string, string> var in dictPranaFixFields)
        //            {
        //                MessageField msgField = new MessageField(var.Key, var.Value);
        //                CorporateActionInfoList.Add(msgField);
        //            }
        //            listOfMsgFields.Add(CorporateActionInfoList);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw ex;
        //        }
        //        return listOfMsgFields;
        //    }

        //    return listOfMsgFields;
        //}

        //private static string GetXMLFromMsgFieldList(List<List<MessageField>> corporateActionList)
        //{
        //    StringBuilder xmlString = new StringBuilder();

        //    System.IO.StringWriter sw = new System.IO.StringWriter(xmlString);
        //    string str = string.Empty;

        //    try
        //    {
        //        XmlTextWriter writer = new XmlTextWriter(sw); //docPath, null);

        //        writer.Formatting = Formatting.Indented;
        //        ///WriteStartDocument does not work as it adds up encoding as utf-16, Hence used WriteProcessingInstruction
        //        //writer.WriteStartDocument();
        //        writer.WriteProcessingInstruction("xml", "version=\"1.0\"");
        //        writer.WriteStartElement("CorporateActionList");

        //        foreach (List<MessageField> msgFieldList in corporateActionList)
        //        {
        //            StringBuilder corpActionStr = new StringBuilder();
        //            writer.WriteStartElement("CorporateAction");
        //            foreach (MessageField msgField in msgFieldList)
        //            {
        //                FixFields fixField = FixDictionaryHelper.GetTagFieldByTagValue(msgField.Tag);
        //                writer.WriteElementString(fixField.FieldName, msgField.Value);

        //                corpActionStr.Append(msgField.Tag);
        //                corpActionStr.Append("=");
        //                corpActionStr.Append(msgField.Value);
        //                corpActionStr.Append(",");
        //            }

        //            string corpActionString = corpActionStr.ToString();
        //            //To remove the last comma in the string we have used substring.
        //            writer.WriteElementString("CorporateActionString", corpActionString.Substring(0, corpActionString.Length - 1));

        //            writer.WriteEndElement();
        //        }

        //        writer.WriteEndElement();
        //        //writer.WriteEndDocument();
        //        writer.Flush();
        //        writer.Close();

        //        str = sw.ToString();

        //    }
        //    catch (Exception ex)
        //    {
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw ex;
        //        }
        //        return str;
        //    }

        //    return str;

        //}

        //internal static void FillCARowFromString(string caString, DataRow resultantCARow)
        //{
        //    try
        //    {
        //        Transformer.FillCARowFromString(caString, resultantCARow);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        internal static void FillCARowFromInputRow(DataRow inputRow, DataRow resultantCARow)
        {
            try
            {
                foreach (DataColumn col in inputRow.Table.Columns)
                {
                    if (resultantCARow.Table.Columns.Contains(col.ColumnName))
                    {
                        if (col.DataType == typeof(System.String))
                        {
                            resultantCARow[col.ColumnName] = inputRow[col.ColumnName].ToString().Trim();
                        }
                        else
                        {
                            resultantCARow[col.ColumnName] = inputRow[col.ColumnName];
                        }
                    }
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
        /// Binds the counter party combo.
        /// </summary>
        /// <param name="cmbCounterParty">The CMB counter party.</param>
        /// <param name="dictCounterParty">The dictionary counter party.</param>
        internal static void BindCounterPartyCombo(UltraCombo cmbCounterParty, Dictionary<int, string> dictCounterParty)
        {
            try
            {
                List<EnumerationValue> listCounterParty = new List<EnumerationValue>();
                EnumerationValue itemAll = new EnumerationValue("-Select-", 0);
                listCounterParty.Add(itemAll);
                foreach (KeyValuePair<int, string> kvp in dictCounterParty)
                {
                    listCounterParty.Add(new EnumerationValue(kvp.Value, kvp.Key));
                }
                cmbCounterParty.DataSource = listCounterParty;
                cmbCounterParty.DisplayMember = "DisplayText";
                cmbCounterParty.ValueMember = "Value";
                cmbCounterParty.LimitToList = true;
                Utils.UltraComboFilter(cmbCounterParty, "DisplayText");
                cmbCounterParty.DropDownStyle = UltraComboStyle.DropDownList;
                if (cmbCounterParty.Rows.Count > 0)
                    cmbCounterParty.Value = 0;
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
    }
}
