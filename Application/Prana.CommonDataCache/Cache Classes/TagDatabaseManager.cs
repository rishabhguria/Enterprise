using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
namespace Prana.CommonDataCache
{
    /// <summary>
    /// Summary description for TagDatabaseManager.
    /// </summary>
    public class TagDatabaseManager
    {
        private static TagDatabaseManager _tagDatabaseManager = null;
        static readonly object _locker = new object();

        private TagDatabaseManager()
        {
        }

        public static TagDatabaseManager GetInstance
        {
            get
            {
                if (_tagDatabaseManager == null)
                {
                    lock (_locker)
                    {
                        if (_tagDatabaseManager == null)
                        {
                            _tagDatabaseManager = new TagDatabaseManager();
                        }
                    }
                }
                return _tagDatabaseManager;
            }
        }

        #region OrderSide
        public string GetOrderSideText(string orderSideValue)
        {
            try
            {
                if (!String.IsNullOrEmpty(orderSideValue.Trim()))
                {
                    Dictionary<string, string> dt = TagDatabase.GetInstance().OrderSide;
                    if (dt.ContainsKey(orderSideValue.Trim().ToUpper()))
                    {
                        return dt[orderSideValue.Trim().ToUpper()];
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
                else
                {
                    return string.Empty;
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
            return string.Empty;
        }

        public string GetOrderSideTextBasedOnID(string orderSideId)
        {
            try
            {
                if (orderSideId.Trim().Length > 0)
                {
                    DataTable dt = TagDatabase.GetInstance().OrderSideWithID;
                    DataRow[] row = dt.Select(OrderFields.PROPERTY_ORDER_SIDEID + "= '" + orderSideId + "'");
                    if (row.Length > 0)
                        return row[0][OrderFields.CAPTION_ORDER_SIDE].ToString();
                    else
                        return "";
                }
                else
                {
                    return "";
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
            return string.Empty;
        }

        public string GetOrderSideValue(string orderSideText)
        {
            try
            {
                if (!String.IsNullOrEmpty(orderSideText))
                {
                    Dictionary<string, string> dt = TagDatabase.GetInstance().OrderSide;
                    foreach (KeyValuePair<string, string> keyValPair in dt)
                    {
                        if (string.Compare(keyValPair.Value, orderSideText, true) == 0)
                        {
                            return keyValPair.Key;
                        }
                    }
                    return string.Empty;
                }
                else
                {
                    return string.Empty;
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
            return string.Empty;
        }

        public string GetOrderSideTagValueBasedOnId(string orderSideId)
        {
            try
            {
                if (orderSideId.Trim().Length > 0)
                {
                    DataTable dt = TagDatabase.GetInstance().OrderSideWithID;
                    DataRow[] row = dt.Select(OrderFields.PROPERTY_ORDER_SIDEID + "= '" + orderSideId + "'");
                    if (row.Length > 0)
                        return row[0][OrderFields.PROPERTY_ORDER_SIDETAGVALUE].ToString();
                    else
                        return "";
                }
                else
                {
                    return "";
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
            return "";
        }

        public string GetOrderSideIdBasedOnSideTagValue(string orderSideTagValue)
        {
            try
            {
                if (orderSideTagValue.Trim().Length > 0)
                {
                    DataTable dt = TagDatabase.GetInstance().OrderSideWithID;
                    DataRow[] row = dt.Select(OrderFields.PROPERTY_ORDER_SIDETAGVALUE + "= '" + orderSideTagValue + "'");
                    if (row.Length > 0)
                        return row[0][OrderFields.PROPERTY_ORDER_SIDEID].ToString();
                    else
                        return "";
                }
                else
                {
                    return "";
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
            return "";
        }
        #endregion

        #region Put/Call
        public string GetPutOrCallText(string value)
        {
            try
            {
                if (value.Trim().Length > 0)
                {
                    DataTable dt = TagDatabase.GetInstance().PutOrCall;
                    DataRow[] row = dt.Select("Value = '" + value + "'");
                    if (row.Length > 0)
                        return row[0]["Description"].ToString();
                    else
                        return "";
                }
                else
                {
                    return "";
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
            return "";
        }

        public string GetPutOrCallValue(string text)
        {
            try
            {
                if (text.Trim().Length > 0)
                {
                    DataTable dt = TagDatabase.GetInstance().PutOrCall;
                    DataRow[] row = dt.Select("Description = '" + text + "'");
                    if (row.Length > 0)
                        return row[0]["Value"].ToString();
                    else
                        return "";
                }
                else
                {
                    return "";
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
            return "";
        }
        #endregion

        #region Put/Call
        public string GetOpenCloseText(string value)
        {
            try
            {
                if (value.Trim().Length > 0)
                {
                    DataTable dt = TagDatabase.GetInstance().OpenClose;
                    DataRow[] row = dt.Select("Value = '" + value + "'");
                    if (row.Length > 0)
                        return row[0]["Description"].ToString();
                    else
                        return "";
                }
                else
                {
                    return "";
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
            return "";
        }

        public string GetOpenCloseValue(string text)
        {
            try
            {
                if (text.Trim().Length > 0)
                {
                    DataTable dt = TagDatabase.GetInstance().OpenClose;
                    DataRow[] row = dt.Select("Description = '" + text + "'");
                    if (row.Length > 0)
                        return row[0]["Value"].ToString();
                    else
                        return "";
                }
                else
                {
                    return "";
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
            return "";
        }
        #endregion

        #region Order Type
        public string GetOrderTypeText(string orderTypeValue)
        {
            try
            {
                if (orderTypeValue.Trim().Length > 0)
                {
                    DataTable dt = TagDatabase.GetInstance().OrderType;
                    DataRow[] row = dt.Select(OrderFields.PROPERTY_ORDER_TYPETAGVALUE + " = '" + orderTypeValue + "'");
                    if (row.Length > 0)
                        return row[0][OrderFields.PROPERTY_ORDER_TYPE].ToString();
                    else
                        return "";
                }
                else
                {
                    return "";
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
            return "";
        }

        public string GetOrderTypeValueBasedOnID(string orderTypeId)
        {
            try
            {
                if (orderTypeId.Trim().Length > 0)
                {
                    DataTable dt = TagDatabase.GetInstance().OrderType;
                    DataRow[] row = dt.Select(OrderFields.PROPERTY_ORDER_TYPE_ID + " = '" + orderTypeId + "'");
                    if (row.Length > 0)
                        return row[0][OrderFields.PROPERTY_ORDER_TYPETAGVALUE].ToString();
                    else
                        return "";
                }
                else
                {
                    return "";
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
            return "";
        }

        public string GetOrderTypeTextBasedOnID(string orderTypeId)
        {
            try
            {
                if (orderTypeId.Trim().Length > 0)
                {
                    DataTable dt = TagDatabase.GetInstance().OrderType;
                    DataRow[] row = dt.Select(OrderFields.PROPERTY_ORDER_TYPE_ID + " = '" + orderTypeId + "'");
                    if (row.Length > 0)
                        return row[0][OrderFields.PROPERTY_ORDER_TYPE].ToString();
                    else
                        return "";
                }
                else
                {
                    return "";
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
            return "";
        }

        public string GetOrderTypeValue(string orderTypeText)
        {
            try
            {
                if (orderTypeText.Trim().Length > 0)
                {
                    DataTable dt = TagDatabase.GetInstance().OrderType;
                    DataRow[] row = dt.Select(OrderFields.PROPERTY_ORDER_TYPE + " = '" + orderTypeText + "'");
                    if (row.Length > 0)
                        return row[0][OrderFields.PROPERTY_ORDER_TYPETAGVALUE].ToString();
                    else
                        return "";
                }
                else
                {
                    return "";
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
            return "";
        }

        public string GetOrderTypeIdBasedOnTagValue(string orderTypeTagValue)
        {
            try
            {
                if (orderTypeTagValue.Trim().Length > 0)
                {
                    DataTable dt = TagDatabase.GetInstance().OrderType;
                    DataRow[] row = dt.Select(OrderFields.PROPERTY_ORDER_TYPETAGVALUE + " = '" + orderTypeTagValue + "'");
                    if (row.Length > 0)
                        return row[0][OrderFields.PROPERTY_ORDER_TYPE_ID].ToString();
                    else
                        return "";
                }
                else
                {
                    return "";
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
            return "";
        }
        #endregion

        #region TIF
        public string GetTIFText(string tifValue)
        {
            try
            {
                if (tifValue.Trim().Length > 0)
                {
                    DataTable dt = TagDatabase.GetInstance().TIF;
                    DataRow[] row = dt.Select(OrderFields.PROPERTY_TIF_TAGVALUE + " = '" + tifValue + "'");
                    if (row.Length > 0)
                        return row[0][OrderFields.PROPERTY_TIF].ToString();
                    else
                        return "";
                }
                else
                {
                    return "";
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
            return "";
        }

        public string GetTIFTextBasedOnID(string tifId)
        {
            try
            {
                if (tifId.Trim().Length > 0)
                {
                    DataTable dt = TagDatabase.GetInstance().TIF;
                    DataRow[] row = dt.Select(OrderFields.PROPERTY_TIFID + " = '" + tifId + "'");
                    if (row.Length > 0)
                        return row[0][OrderFields.PROPERTY_TIF].ToString();
                    else
                        return "";
                }
                else
                {
                    return "";
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
            return "";
        }

        public string GetTIFValueBasedOnID(string tifId)
        {
            try
            {
                if (tifId.Trim().Length > 0)
                {
                    DataTable dt = TagDatabase.GetInstance().TIF;
                    DataRow[] row = dt.Select(OrderFields.PROPERTY_TIFID + " = '" + tifId + "'");
                    if (row.Length > 0)
                        return row[0][OrderFields.PROPERTY_TIF_TAGVALUE].ToString();
                    else
                        return "";
                }
                else
                {
                    return "";
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
            return "";
        }

        public string GetTIFIdBasedOnTagValue(string tifTagValue)
        {
            try
            {
                if (tifTagValue.Trim().Length > 0)
                {
                    DataTable dt = TagDatabase.GetInstance().TIF;
                    DataRow[] row = dt.Select(OrderFields.PROPERTY_TIF_TAGVALUE + " = '" + tifTagValue + "'");
                    if (row.Length > 0)
                        return row[0][OrderFields.PROPERTY_TIFID].ToString();
                    else
                        return "";
                }
                else
                {
                    return "";
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
            return "";
        }

        public string GetTIFValue(string tifText)
        {
            try
            {
                if (tifText.Trim().Length > 0)
                {
                    DataTable dt = TagDatabase.GetInstance().TIF;
                    DataRow[] row = dt.Select("TIF = '" + tifText + "'");
                    if (row.Length > 0)
                        return row[0]["TIFID"].ToString();
                    else
                        return "";
                }
                else
                {
                    return "";
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
            return "";
        }


        public string GetTIFTagValueBasedOnText(string tifText)
        {
            try
            {
                if (tifText.Trim().Length > 0)
                {
                    DataTable dt = TagDatabase.GetInstance().TIF;
                    DataRow[] row = dt.Select("TIFText = '" + tifText + "'");
                    if (row.Length > 0)
                        return row[0]["TIF"].ToString();
                    else
                        return "";
                }
                else
                {
                    return "";
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
            return "";
        }
        #endregion

        #region OrderStatus
        public string GetOrderStatusText(string orderStatusValue)
        {
            try
            {
                if (orderStatusValue.Trim().Length > 0)
                {
                    DataTable dt = TagDatabase.GetInstance().OrderStatus;
                    DataRow[] row = dt.Select("OrderStatusID = '" + orderStatusValue + "'");
                    if (row.Length > 0)
                        return row[0]["OrderStatus"].ToString();
                    else
                        return "";
                }
                else
                {
                    return "";
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
            return "";
        }

        public string GetOrderStatusValue(string orderStatusText)
        {
            try
            {
                if (orderStatusText.Trim().Length > 0)
                {
                    DataTable dt = TagDatabase.GetInstance().OrderStatus;
                    DataRow[] row = dt.Select("OrderStatus = '" + orderStatusText + "'");
                    if (row.Length > 0)
                        return row[0]["OrderStatusID"].ToString();
                    else
                        return "";
                }
                else
                {
                    return "";
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
            return "";
        }
        #endregion

        #region HandlingInstruction
        public string GetHandlingInstructionText(string HandlingInstructionValue)
        {
            try
            {
                if (HandlingInstructionValue.Trim().Length > 0)
                {
                    DataTable dt = TagDatabase.GetInstance().HandlingInstruction;
                    DataRow[] row = dt.Select(OrderFields.PROPERTY_HANDLING_INST_TagValue + "= '" + HandlingInstructionValue + "'");
                    if (row.Length > 0)
                        return row[0][OrderFields.PROPERTY_HANDLING_INST].ToString();
                    else
                        return "";
                }
                else
                {
                    return "";
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
            return "";
        }

        public string GetHandlingInstructionTextBasedOnId(string HandlingInstructionId)
        {
            try
            {
                if (HandlingInstructionId.Trim().Length > 0)
                {
                    DataTable dt = TagDatabase.GetInstance().HandlingInstruction;
                    DataRow[] row = dt.Select(OrderFields.PROPERTY_HANDLING_INSTID + "= '" + HandlingInstructionId + "'");
                    if (row.Length > 0)
                        return row[0][OrderFields.PROPERTY_HANDLING_INST].ToString();
                    else
                        return "";
                }
                else
                {
                    return "";
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
            return "";
        }

        public string GetHandlingInstructionValueBasedOnId(string HandlingInstructionId)
        {
            try
            {
                if (HandlingInstructionId.Trim().Length > 0)
                {
                    DataTable dt = TagDatabase.GetInstance().HandlingInstruction;
                    DataRow[] row = dt.Select(OrderFields.PROPERTY_HANDLING_INSTID + "= '" + HandlingInstructionId + "'");
                    if (row.Length > 0)
                        return row[0][OrderFields.PROPERTY_HANDLING_INST_TagValue].ToString();
                    else
                        return "";
                }
                else
                {
                    return "";
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
            return "";
        }

        public string GetHandlingInstructionIdBasedOnTagValue(string HandlingInstructionTagValue)
        {
            try
            {
                if (HandlingInstructionTagValue.Trim().Length > 0)
                {
                    DataTable dt = TagDatabase.GetInstance().HandlingInstruction;
                    DataRow[] row = dt.Select(OrderFields.PROPERTY_HANDLING_INST_TagValue + "= '" + HandlingInstructionTagValue + "'");
                    if (row.Length > 0)
                        return row[0][OrderFields.PROPERTY_HANDLING_INSTID].ToString();
                    else
                        return "";
                }
                else
                {
                    return "";
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
            return "";
        }


        public string GetHandlingInstructionValue(string HandlingInstructionText)
        {
            try
            {
                if (HandlingInstructionText.Trim().Length > 0)
                {
                    DataTable dt = TagDatabase.GetInstance().HandlingInstruction;
                    DataRow[] row = dt.Select(OrderFields.PROPERTY_HANDLING_INST + "= '" + HandlingInstructionText + "'");
                    if (row.Length > 0)
                        return row[0][OrderFields.PROPERTY_HANDLING_INST_TagValue].ToString();
                    else
                        return "";
                }
                else
                {
                    return "";
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
            return "";
        }
        #endregion

        #region ExecutionInstruction
        public string GetExecutionInstructionText(string ExecutionInstructionValue)
        {
            try
            {
                if (ExecutionInstructionValue.Trim().Length > 0)
                {
                    DataTable dt = TagDatabase.GetInstance().ExecutionInstruction;
                    DataRow[] row = dt.Select(OrderFields.PROPERTY_EXECUTION_INST_TagValue + "= '" + ExecutionInstructionValue + "'");
                    if (row.Length > 0)
                        return row[0][OrderFields.PROPERTY_EXECUTION_INST].ToString();
                    else
                        return "";
                }
                else
                {
                    return "";
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
            return "";
        }

        public string GetExecutionInstructionTextBasedOnID(string ExecutionInstructionID)
        {
            try
            {
                if (ExecutionInstructionID.Trim().Length > 0)
                {
                    DataTable dt = TagDatabase.GetInstance().ExecutionInstruction;
                    DataRow[] row = dt.Select(OrderFields.PROPERTY_EXECUTION_INSTID + "= '" + ExecutionInstructionID + "'");
                    if (row.Length > 0)
                        return row[0][OrderFields.PROPERTY_EXECUTION_INST].ToString();
                    else
                        return "";
                }
                else
                {
                    return "";
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
            return "";
        }


        public string GetExecutionInstructionIdBasedOnTagValue(string ExecutionInstructionTagValue)
        {
            try
            {
                if (ExecutionInstructionTagValue.Trim().Length > 0)
                {
                    DataTable dt = TagDatabase.GetInstance().ExecutionInstruction;
                    DataRow[] row = dt.Select(OrderFields.PROPERTY_EXECUTION_INST_TagValue + "= '" + ExecutionInstructionTagValue + "'");
                    if (row.Length > 0)
                        return row[0][OrderFields.PROPERTY_EXECUTION_INSTID].ToString();
                    else
                        return "";
                }
                else
                {
                    return "";
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
            return "";
        }

        public string GetExecutionInstructionValueBasedOnID(string ExecutionInstructionValue)
        {
            try
            {
                if (ExecutionInstructionValue.Trim().Length > 0)
                {
                    DataTable dt = TagDatabase.GetInstance().ExecutionInstruction;
                    DataRow[] row = dt.Select(OrderFields.PROPERTY_EXECUTION_INSTID + "= '" + ExecutionInstructionValue + "'");
                    if (row.Length > 0)
                        return row[0][OrderFields.PROPERTY_EXECUTION_INST_TagValue].ToString();
                    else
                        return "";
                }
                else
                {
                    return "";
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
            return "";
        }
        public string GetExecutionInstructionValue(string ExecutionInstructionText)
        {
            try
            {
                if (ExecutionInstructionText.Trim().Length > 0)
                {
                    DataTable dt = TagDatabase.GetInstance().ExecutionInstruction;
                    DataRow[] row = dt.Select(OrderFields.PROPERTY_EXECUTION_INST + "= '" + ExecutionInstructionText + "'");
                    if (row.Length > 0)
                        return row[0][OrderFields.PROPERTY_EXECUTION_INST_TagValue].ToString();
                    else
                        return "";
                }
                else
                {
                    return "";
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
            return "";
        }
        #endregion

        #region CMTA
        public string GetCMTAText(string cmtaValue)
        {
            try
            {
                if (cmtaValue.Trim().Length > 0)
                {
                    DataTable dt = TagDatabase.GetInstance().CMTA;
                    DataRow[] row = dt.Select(OrderFields.PROPERTY_CMTAID + " = '" + cmtaValue + "'");
                    if (row.Length > 0)
                        return row[0][OrderFields.PROPERTY_CMTA].ToString();
                    else
                        return "";
                }
                else
                {
                    return "";
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
            return "";
        }

        public string GetCMTAValue(string CMTAText)
        {
            try
            {
                if (CMTAText.Trim().Length > 0)
                {
                    DataTable dt = TagDatabase.GetInstance().CMTA;
                    DataRow[] row = dt.Select("CMTA = '" + CMTAText + "'");
                    if (row.Length > 0)
                        return row[0][OrderFields.PROPERTY_CMTAID].ToString();
                    else
                        return "";
                }
                else
                {
                    return "";
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
            return "";
        }
        #endregion

        #region CMTA
        public string GetGiveUpText(string GiveUpValue)
        {
            try
            {
                if (GiveUpValue.Trim().Length > 0)
                {
                    DataTable dt = TagDatabase.GetInstance().GiveUp;
                    DataRow[] row = dt.Select(OrderFields.PROPERTY_GIVEUPID + " = '" + GiveUpValue + "'");
                    if (row.Length > 0)
                        return row[0][OrderFields.PROPERTY_GIVEUP].ToString();
                    else
                        return "";
                }
                else
                {
                    return "";
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
            return "";
        }

        public string GetGiveUpValue(string GiveUpText)
        {
            try
            {
                if (GiveUpText.Trim().Length > 0)
                {
                    DataTable dt = TagDatabase.GetInstance().GiveUp;
                    DataRow[] row = dt.Select(" = '" + GiveUpText + "'");
                    if (row.Length > 0)
                        return row[0][OrderFields.PROPERTY_GIVEUPID].ToString();
                    else
                        return "";
                }
                else
                {
                    return "";
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
            return "";
        }
        #endregion

        public System.Collections.Generic.Dictionary<string, string> GetAllOrderSides()
        {
            return TagDatabase.GetInstance().OrderSide;
        }

        public DataTable GetAllExecutionInstructions()
        {
            return TagDatabase.GetInstance().ExecutionInstruction;
        }

        public Dictionary<string, string> GetAllOrderTypeTags()
        {
            Dictionary<string, string> orderTypeTags = new Dictionary<string, string>();
            try
            {
                foreach (DataRow dr in TagDatabase.GetInstance().OrderType.Rows)
                {
                    if (!orderTypeTags.ContainsKey(dr[OrderFields.PROPERTY_ORDER_TYPETAGVALUE].ToString()))
                    {
                        orderTypeTags.Add(dr[OrderFields.PROPERTY_ORDER_TYPETAGVALUE].ToString(), dr[OrderFields.PROPERTY_ORDER_TYPE].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                //throwing exception up to the stack
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return orderTypeTags;
        }
    }
}
