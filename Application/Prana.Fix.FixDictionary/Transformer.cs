using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.FIX;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml;

namespace Prana.Fix.FixDictionary
{
    public class Transformer
    {
        static IEnumerable<PropertyInfo> propertiesOrder = null;
        static PropertyInfo[] propertiesOrderSingle = null;
        static PropertyInfo[] propertiesDropCpyOrder = null;
        static Dictionary<PropertyInfo, Func<Order, object>> propertiesOrderTree = new Dictionary<PropertyInfo, Func<Order, object>>();

        static Transformer()
        {
            OrderSingle orderSingle = new OrderSingle();
            DropCopyOrder dropCopyOrder = new DropCopyOrder();
            propertiesOrder = typeof(Order).GetProperties().Where(p => p.CanRead);
            propertiesOrderSingle = orderSingle.GetType().GetProperties();
            propertiesDropCpyOrder = dropCopyOrder.GetType().GetProperties();
            foreach (PropertyInfo propertyInfo in propertiesOrder)
            {
                MethodInfo info = propertyInfo.GetGetMethod();
                ParameterExpression param = Expression.Parameter(typeof(Order));
                MethodCallExpression call = Expression.Call(param, info);

                UnaryExpression castToObject = Expression.Convert(call, typeof(object));
                LambdaExpression lambda = Expression.Lambda(castToObject, param);

                var functionThatGetsValue = (Func<Order, object>)lambda.Compile();
                propertiesOrderTree.Add(propertyInfo, functionThatGetsValue);
            }
        }

        public static PranaMessage CreatePranaMessageThroughReflection(Order order)
        {
            PranaMessage pranaMessage = new PranaMessage();
            pranaMessage.MessageType = order.MsgType;
            foreach (var kvp in propertiesOrderTree)
            {
                //property.Name 
                FixFields fixfield = FixDictionaryHelper.GetTagFieldByTagName(kvp.Key.Name);
                if (fixfield != null)
                {
                    if (fixfield.Tag != string.Empty)
                    {
                        object objValue = kvp.Value(order);

                        if (objValue != null)
                        {
                            string value = objValue.ToString();
                            if (!(value == string.Empty || value == double.Epsilon.ToString() || value == int.MinValue.ToString() || value == double.MinValue.ToString()))
                            {
                                if (fixfield.IsExternal)
                                {
                                    pranaMessage.FIXMessage.ExternalInformation.AddField(fixfield.Tag, value);
                                }
                                else
                                {
                                    pranaMessage.FIXMessage.InternalInformation.AddField(fixfield.Tag, value);
                                }
                            }
                        }
                    }
                }
            }

            CreateOpenCloseTags(pranaMessage);
            return pranaMessage;
        }

        public static PranaMessage CreatePranaMessageThroughReflection(OrderSingle order)
        {
            PranaMessage pranaMessage = new PranaMessage();
            try
            {
                pranaMessage.MessageType = order.MsgType;
                foreach (PropertyInfo property in propertiesOrderSingle)
                {
                    //property.Name 
                    FixFields fixfield = FixDictionaryHelper.GetTagFieldByTagName(property.Name);
                    if (fixfield != null)
                    {
                        if (fixfield.Tag != string.Empty)
                        {
                            object objValue = property.GetValue(order, null);

                            if (objValue != null)
                            {
                                string value = objValue.ToString();

                                if (value == string.Empty || value == double.Epsilon.ToString() || value == int.MinValue.ToString() || value == double.MinValue.ToString())
                                {
                                }
                                else
                                {
                                    if (fixfield.IsExternal)
                                    {
                                        pranaMessage.FIXMessage.ExternalInformation.AddField(fixfield.Tag, value);
                                    }
                                    else
                                    {
                                        pranaMessage.FIXMessage.InternalInformation.AddField(fixfield.Tag, value);
                                    }
                                }
                            }
                        }
                    }
                }
                CreateOpenCloseTags(pranaMessage);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return pranaMessage;
        }

        public static PranaMessage CreatePranaMessageThroughReflection(BasketDetail basket, OrderCollection orders, string msgType)
        {
            PranaMessage pranaMessage = new PranaMessage();
            pranaMessage.MessageType = msgType;
            pranaMessage.FIXMessageList.GroupID = basket.CurrentGroupID;
            pranaMessage.FIXMessageList.WaveID = basket.CurrentWaveID;
            pranaMessage.FIXMessageList.UserID = basket.UserID.ToString();
            pranaMessage.FIXMessageList.BasketID = basket.BasketID;
            pranaMessage.FIXMessageList.TradedBasketID = basket.TradedBasketID;
            pranaMessage.TradingAccountID = basket.TradingAccountID.ToString();
            foreach (Order order in orders)
            {
                PranaMessage singleMessageOrder = CreatePranaMessageThroughReflection(order);
                pranaMessage.FIXMessageList.AddMessage(singleMessageOrder.FIXMessage);
            }
            return pranaMessage;
        }

        public static PranaMessage CreatePranaMessageThroughReflection(DropCopyOrder order)
        {
            PranaMessage pranaMessage = new PranaMessage();
            try
            {
                pranaMessage.MessageType = order.MsgType;
                foreach (PropertyInfo property in propertiesDropCpyOrder)
                {
                    //property.Name 
                    FixFields fixfield = FixDictionaryHelper.GetTagFieldByTagName(property.Name);
                    if (fixfield != null)
                    {
                        if (fixfield.Tag != string.Empty)
                        {
                            object objValue = property.GetValue(order, null);

                            if (objValue != null)
                            {
                                string value = objValue.ToString();

                                if (value == string.Empty || value == double.Epsilon.ToString() || value == int.MinValue.ToString() || value == double.MinValue.ToString())
                                {
                                }
                                else
                                {
                                    if (fixfield.IsExternal)
                                    {
                                        pranaMessage.FIXMessage.ExternalInformation.AddField(fixfield.Tag, value);
                                    }
                                    else
                                    {
                                        pranaMessage.FIXMessage.InternalInformation.AddField(fixfield.Tag, value);
                                    }
                                }
                            }
                        }
                    }
                }

                CreateOpenCloseTags(pranaMessage);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return pranaMessage;
        }

        public static PranaMessage CreatePranaMessageThroughReflection(DataRow row)
        {
            PranaMessage pranaMsg = new PranaMessage();
            foreach (DataColumn column in row.Table.Columns)
            {
                FixFields fixfield = FixDictionaryHelper.GetTagFieldByTagName(column.Caption);
                if (fixfield != null)
                {
                    if (fixfield.Tag != string.Empty)
                    {
                        if (fixfield.IsExternal)
                        {
                            pranaMsg.FIXMessage.ExternalInformation.AddField(fixfield.Tag, row[column.Caption].ToString());
                        }
                        else
                        {
                            pranaMsg.FIXMessage.InternalInformation.AddField(fixfield.Tag, row[column.Caption].ToString());
                        }
                    }
                }
            }
            return pranaMsg;
        }

        public static void CreateObjThroughReflection(DataRow dr, object target)
        {
            try
            {
                Dictionary<string, object> nameValueColl = new Dictionary<string, object>();
                foreach (DataColumn column in dr.Table.Columns)
                {
                    if (dr[column.Caption] != System.DBNull.Value)
                    {
                        nameValueColl.Add(column.Caption, dr[column.Caption]);
                    }
                }

                PropertyInfo[] propertiestarget = target.GetType().GetProperties();
                foreach (PropertyInfo pi in propertiestarget)
                {
                    if (pi.CanWrite && nameValueColl.ContainsKey(pi.Name))
                    {
                        Type type = pi.PropertyType;
                        Type typeOfData = nameValueColl[pi.Name].GetType();
                        object value = nameValueColl[pi.Name];

                        if (type.BaseType.Name.Equals("Enum"))
                        {
                            value = Enum.ToObject(type, Convert.ToInt32(value));
                        }
                        else if (type.Equals(typeof(SerializableDictionary<string, object>)) && typeOfData.Equals(typeof(string)))
                        {
                            value = GetSerializableDictionaryFromXML(value.ToString());
                        }
                        else
                        {
                            value = Convert.ChangeType(value, type);
                        }
                        pi.SetValue(target, value, null);
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

        /// <summary>
        /// fill Object value to other object throught reflection
        /// created by - om shiv, nov 2013
        /// </summary>
        /// <param name="from"></param>
        /// <param name="target"></param>
        public static void CreateObjFromObjThroughReflection(object from, object target)
        {
            try
            {
                if (from != null && target != null)
                {
                    PropertyInfo[] propertiesFrom = from.GetType().GetProperties();
                    Dictionary<string, object> nameValueColl = new Dictionary<string, object>();
                    foreach (PropertyInfo pi in propertiesFrom)
                    {
                        object value = pi.GetValue(from, null);
                        if (value != null)
                        {
                            nameValueColl.Add(pi.Name, value);
                        }
                    }

                    PropertyInfo[] propertiestarget = target.GetType().GetProperties();
                    foreach (PropertyInfo pi in propertiestarget)
                    {
                        try
                        {
                            if (pi.CanWrite && nameValueColl.ContainsKey(pi.Name))
                            {
                                Type type = pi.PropertyType;
                                object value = nameValueColl[pi.Name];
                                if (type.BaseType.Name.Equals("Enum"))
                                {
                                    value = Enum.ToObject(type, value);
                                }
                                else
                                {
                                    value = Convert.ChangeType(value, type);
                                }
                                if (value != null)
                                {
                                    pi.SetValue(target, value, null);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                        }
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

        public static Order CreateOrder(PranaMessage pranaMessage)
        {
            Order order = new Order();
            try
            {
                foreach (PropertyInfo property in propertiesOrder)
                {
                    //property.Name 
                    FixFields fixfield = FixDictionaryHelper.GetTagFieldByTagName(property.Name);
                    if (fixfield != null)
                    {
                        string tag = fixfield.Tag;
                        if (tag != string.Empty)
                        {
                            if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(tag))
                            {
                                string value = pranaMessage.FIXMessage.ExternalInformation[tag].Value;
                                object actValue = GetObjectValue(property.PropertyType.FullName, value, fixfield);
                                if (actValue != null)
                                    property.SetValue(order, actValue, null);
                            }
                            else if (pranaMessage.FIXMessage.InternalInformation.ContainsKey(tag))
                            {
                                string value = pranaMessage.FIXMessage.InternalInformation[tag].Value;
                                object actValue = GetObjectValue(property.PropertyType.FullName, value, fixfield);
                                if (actValue != null)
                                    property.SetValue(order, actValue, null);
                            }
                        }
                    }
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
            return order;
        }

        public static OrderSingle CreateOrderSingle(PranaMessage pranaMessage)
        {
            OrderSingle order = new OrderSingle();
            try
            {
                foreach (PropertyInfo property in propertiesOrderSingle)
                {
                    //property.Name 
                    FixFields fixfield = FixDictionaryHelper.GetTagFieldByTagName(property.Name);
                    if (fixfield != null)
                    {
                        string tag = fixfield.Tag;
                        if (tag != string.Empty)
                        {
                            if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(tag))
                            {
                                string value = pranaMessage.FIXMessage.ExternalInformation[tag].Value;
                                object actValue = GetObjectValue(property.PropertyType.FullName, value, fixfield);
                                if (actValue != null)
                                    property.SetValue(order, actValue, null);
                            }
                            else if (pranaMessage.FIXMessage.InternalInformation.ContainsKey(tag))
                            {
                                string value = pranaMessage.FIXMessage.InternalInformation[tag].Value;
                                object actValue = GetObjectValue(property.PropertyType.FullName, value, fixfield);
                                if (actValue != null)
                                    property.SetValue(order, actValue, null);
                            }
                        }
                    }
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
            return order;
        }

        public static DropCopyOrder CreateDropCopy(PranaMessage pranaMessage)
        {
            DropCopyOrder dropCopyOrder = new DropCopyOrder();
            foreach (PropertyInfo property in propertiesDropCpyOrder)
            {
                //property.Name 
                FixFields fixfield = FixDictionaryHelper.GetTagFieldByTagName(property.Name);
                if (fixfield != null)
                {

                    string tag = fixfield.Tag;
                    if (tag != string.Empty)
                    {
                        if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(tag))
                        {
                            string value = pranaMessage.FIXMessage.ExternalInformation[tag].Value;
                            object actValue = GetObjectValue(property.PropertyType.FullName, value, fixfield);
                            if (actValue != null)
                                property.SetValue(dropCopyOrder, actValue, null);
                        }
                        else if (pranaMessage.FIXMessage.InternalInformation.ContainsKey(tag))
                        {
                            string value = pranaMessage.FIXMessage.InternalInformation[tag].Value;
                            object actValue = GetObjectValue(property.PropertyType.FullName, value, fixfield);
                            if (actValue != null)
                                property.SetValue(dropCopyOrder, actValue, null);
                        }
                    }
                }
            }
            return dropCopyOrder;
        }

        public static BasketDetail CreateBasket(PranaMessage pranaMessage)
        {
            BasketDetail basket = new BasketDetail();
            basket.BasketID = pranaMessage.FIXMessageList.BasketID;
            basket.TradedBasketID = pranaMessage.FIXMessageList.TradedBasketID;
            int tradingID = int.MinValue;
            int.TryParse(pranaMessage.TradingAccountID, out tradingID);
            basket.TradingAccountID = tradingID;
            int userID = int.MinValue;
            int.TryParse(pranaMessage.FIXMessageList.UserID, out userID);
            basket.UserID = userID;

            foreach (FIXMessage fixMsg in pranaMessage.FIXMessageList.ListMessages)
            {
                PranaMessage PranaMsgOrder = new PranaMessage();
                PranaMsgOrder.FIXMessage = fixMsg;
                PranaMsgOrder.MessageType = fixMsg.ExternalInformation[FIXConstants.TagMsgType].Value;
                Order order = CreateOrder(PranaMsgOrder);
                basket.BasketOrders.Add(order);
            }
            return basket;
        }

        public static object GetObjectValue(string name, string value, FixFields fixfield)
        {
            object valueActual = null;
            try
            {
                if (name == "System.Double")
                {
                    valueActual = double.Parse(value);
                }
                else if (name == "System.Int32")
                {
                    valueActual = Int32.Parse(value);
                }
                else if (name == "System.Int64")
                {
                    valueActual = Int64.Parse(value);
                }
                else if (name == "System.Boolean")
                {
                    valueActual = bool.Parse(value);
                }
                else if (name == "System.Single")
                {
                    valueActual = Single.Parse(value);
                }
                else if (name == "System.DateTime")
                {
                    try
                    {
                        valueActual = DateTime.Parse(value);
                    }
                    catch (Exception ex)
                    {
                        string format = fixfield.Format;
                        valueActual = DateTime.ParseExact(value, string.IsNullOrEmpty(format) ? DateTimeConstants.NirvanaDateTimeFormat : format, null);
                        Logger.LoggerWrite("Unable to parse value: " + valueActual + " and processed this value: " + value + ex.Message);
                    }
                }
                else if (name == "Prana.BusinessObjects.OrderAlgoStartegyParameters")
                {
                    valueActual = new OrderAlgoStartegyParameters(value);
                }
                else if (name == "Prana.BusinessObjects.SwapParameters")
                {
                    valueActual = new SwapParameters(value);
                }
                else if (name == "Prana.BusinessObjects.ShortLocateListParameter")
                {
                    valueActual = new ShortLocateListParameter(value);
                }
                else if (name == "Prana.BusinessObjects.OTCTradeData")
                {
                    var instrumentType = OTCTradeData.GetInstrumentType(value);
                    if (instrumentType == "1")
                        valueActual = new EquitySwapTradeData(value);
                    else if (instrumentType == "2")
                        valueActual = new CFDTradeData(value);
                    else
                        valueActual = new ConvertibleBondTradeData(value);
                }
                else if (name == "Prana.BusinessObjects.AppConstants.CalculationBasis")
                {
                    valueActual = (CalculationBasis)Enum.Parse(typeof(CalculationBasis), value, true);
                }
                else if (name == "System.Decimal")
                {
                    valueActual = Decimal.Parse(value);
                }
                else if (name == "Prana.BusinessObjects.AppConstants.AccrualBasis")
                {
                    valueActual = (AccrualBasis)Enum.Parse(typeof(AccrualBasis), value);
                }
                else if (name == "Prana.BusinessObjects.AppConstants.CouponFrequency")
                {
                    valueActual = (CouponFrequency)Enum.Parse(typeof(CouponFrequency), value);
                }
                else
                {
                    valueActual = value;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(new Exception("Error in parsing ..Wrong Value in field" + fixfield.Tag.ToString() + " Value=" + value), LoggingConstants.POLICY_LOGANDSHOW);
                rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return valueActual;
        }

        public static DataTable CreateDataTable(List<PranaMessage> lstMsg)
        {
            List<string> colnames = GetColumnsNames(lstMsg);
            DataTable dataToBind = CreateDataTable(colnames, lstMsg);
            return dataToBind;
        }

        public static DataTable CreateDataTable(List<string> columns, List<PranaMessage> PranaMsgCollection)
        {
            DataTable dt = new DataTable();
            foreach (string column in columns)
            {
                dt.Columns.Add(column);
            }

            foreach (PranaMessage pranaMsg in PranaMsgCollection)
            {
                object[] row = new object[columns.Count];
                int i = 0;

                foreach (string column in columns)
                {
                    FixFields fixField = FixDictionaryHelper.GetTagFieldByTagName(column);
                    if (fixField != null)
                    {
                        string tag = fixField.Tag;
                        if (fixField.IsExternal)
                        {
                            if (pranaMsg.FIXMessage.ExternalInformation.ContainsKey(tag))
                            {
                                row[i] = pranaMsg.FIXMessage.ExternalInformation[tag].Value;
                            }
                            else
                            {
                                row[i] = string.Empty;
                            }
                        }
                        else
                        {
                            if (pranaMsg.FIXMessage.InternalInformation.ContainsKey(tag))
                            {
                                row[i] = pranaMsg.FIXMessage.InternalInformation[tag].Value;
                            }
                            else
                            {
                                row[i] = string.Empty;
                            }
                        }
                    }
                    else
                    {
                        row[i] = string.Empty;
                    }
                    i++;

                }
                dt.Rows.Add(row);
            }
            return dt;
        }

        public static DataTable CreateDataTable(List<string> columns, List<MessageFieldCollection> msgCollection)
        {
            DataTable dt = new DataTable();
            foreach (string column in columns)
            {
                dt.Columns.Add(column);
            }

            foreach (MessageFieldCollection msgFieldColl in msgCollection)
            {
                object[] row = new object[columns.Count];
                int i = 0;

                foreach (string column in columns)
                {
                    FixFields fixField = FixDictionaryHelper.GetTagFieldByTagName(column);
                    if (fixField != null)
                    {
                        if (msgFieldColl.ContainsKey(fixField.Tag))
                        {
                            row[i] = msgFieldColl[fixField.Tag].Value;
                        }
                        else
                        {
                            row[i] = string.Empty;
                        }
                    }
                    else
                    {
                        row[i] = string.Empty;
                    }
                    i++;
                }
                dt.Rows.Add(row);
            }
            return dt;
        }

        /// <summary>
        // /Get Column's Names from PranaMessage 
        /// </summary>
        /// <param name="PranaMsgCollection"></param>
        /// <returns></returns>
        public static List<string> GetColumnsNames(List<PranaMessage> PranaMsgCollection)
        {
            List<string> columns = new List<string>();
            foreach (PranaMessage pranaMsg in PranaMsgCollection)
            {
                foreach (MessageField msgfield in pranaMsg.FIXMessage.ExternalInformation.MessageFields)
                {
                    FixFields fixField = FixDictionaryHelper.GetTagFieldByTagValue(msgfield.Tag);
                    if (fixField != null)
                    {
                        if (!columns.Contains(fixField.FieldName))
                            columns.Add(fixField.FieldName);
                    }
                }
                foreach (MessageField msgfield in pranaMsg.FIXMessage.InternalInformation.MessageFields)
                {
                    FixFields fixField = FixDictionaryHelper.GetTagFieldByTagValue(msgfield.Tag);
                    if (fixField != null)
                    {
                        if (!columns.Contains(fixField.FieldName))
                            columns.Add(fixField.FieldName);
                    }
                }
            }
            return columns;
        }

        public static List<string> GetColumnsNames(List<MessageFieldCollection> msgCollection)
        {
            List<string> columns = new List<string>();
            foreach (MessageFieldCollection msgColl in msgCollection)
            {
                foreach (MessageField msgfield in msgColl.MessageFields)
                {
                    FixFields fixField = FixDictionaryHelper.GetTagFieldByTagValue(msgfield.Tag);
                    if (fixField != null)
                    {
                        if (!columns.Contains(fixField.FieldName))
                            columns.Add(fixField.FieldName);
                    }
                }
            }
            return columns;
        }

        private static void CreateOpenCloseTags(PranaMessage pranaMessage)
        {
            if (!pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagSide))
                return;

            switch (pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagSide].Value)
            {
                case FIXConstants.SIDE_Buy_Open:

                    pranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagOpenClose, FIXConstants.Open);
                    break;
                case FIXConstants.SIDE_Buy_Closed:

                    pranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagOpenClose, FIXConstants.Close);
                    break;
                case FIXConstants.SIDE_Sell_Open:

                    pranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagOpenClose, FIXConstants.Open);
                    break;
                case FIXConstants.SIDE_Sell_Closed:

                    pranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagOpenClose, FIXConstants.Close);
                    break;
            }
        }

        public static List<PranaMessage> CreatePranaMessages(DataTable dt, bool skipEmptyFields = false)
        {
            List<PranaMessage> pranaMsglist = new List<PranaMessage>();
            foreach (DataRow dr in dt.Rows)
            {
                PranaMessage pranaMsg = new PranaMessage();
                int Count = 0;
                foreach (object item in dr.ItemArray)
                {
                    string value = item.ToString();
                    if (!(skipEmptyFields && string.IsNullOrWhiteSpace(value)))
                    {
                        string tagName = dt.Columns[Count].ToString();
                        FixFields fixField = FixDictionaryHelper.GetTagFieldByTagName(tagName);

                        if (fixField != null)
                        {
                            string tag = fixField.Tag;
                            if (fixField.IsExternal)
                            {
                                pranaMsg.FIXMessage.ExternalInformation.AddField(tag, value);
                            }
                            else
                            {
                                pranaMsg.FIXMessage.InternalInformation.AddField(tag, value);
                            }
                        }
                    }
                    Count++;
                }
                pranaMsglist.Add(pranaMsg);
            }
            return pranaMsglist;
        }

        public static List<Dictionary<string, string>> GetFixFieldsFromDataColoumnList(List<DataColumn> corporateActionColList, DataRowCollection rows)
        {
            List<Dictionary<string, string>> rowWiseFixFields = new List<Dictionary<string, string>>();

            try
            {
                foreach (DataRow row in rows)
                {
                    Dictionary<string, string> fixFieldsDict = new Dictionary<string, string>();
                    foreach (DataColumn datacoloumn in corporateActionColList)
                    {
                        if (datacoloumn != null)
                        {
                            string tagName = datacoloumn.ColumnName;

                            FixFields fixfield = FixDictionaryHelper.GetTagFieldByTagName(tagName);
                            if (fixfield != null)
                            {
                                if (fixfield.Tag != string.Empty)
                                {
                                    fixFieldsDict.Add(fixfield.Tag, row[datacoloumn.ColumnName].ToString());
                                }
                            }
                        }
                    }

                    rowWiseFixFields.Add(fixFieldsDict);
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

            return rowWiseFixFields;
        }

        public static List<MessageField> GetMessageFieldsFromDataColoumnList(List<DataColumn> corporateActionColList, DataRow corporateActionRow, bool skipEmptyFields = false)
        {
            List<MessageField> additionalFixFields = new List<MessageField>();
            try
            {
                foreach (DataColumn datacoloumn in corporateActionColList)
                {
                    if (datacoloumn != null && !String.IsNullOrEmpty(datacoloumn.ColumnName))
                    {
                        FixFields fixfield = FixDictionaryHelper.GetTagFieldByTagName(datacoloumn.ColumnName);

                        if (fixfield != null && fixfield.Tag != string.Empty)
                        {
                            string value = corporateActionRow[datacoloumn.ColumnName].ToString();
                            if (!(skipEmptyFields && string.IsNullOrWhiteSpace(value)))
                            {
                                additionalFixFields.Add(new MessageField(fixfield.Tag, value));
                            }
                        }
                    }
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

            return additionalFixFields;
        }

        public static void FillCARowFromString(string caString, DataRow resultantCARow)
        {
            try
            {
                string[] caArr = caString.Split(',');
                foreach (string str in caArr)
                {
                    string[] keyValue = str.Split('=');
                    string tag = keyValue[0];
                    string colValue = keyValue[1];
                    string colName = string.Empty;

                    if (keyValue.Length == 2)
                    {
                        FixFields fixfield = FixDictionaryHelper.GetTagFieldByTagValue(tag);
                        colName = fixfield.FieldName;
                        if (resultantCARow.Table.Columns.Contains(colName))
                        {
                            DataColumn col = resultantCARow.Table.Columns[colName];
                            if (col.DataType == typeof(Prana.BusinessObjects.AppConstants.CorporateActionType))
                            {
                                resultantCARow[colName] = (CorporateActionType)Enum.Parse(typeof(CorporateActionType), colValue);
                            }
                            else if (col.DataType == typeof(System.Guid))
                            {
                                resultantCARow[colName] = new Guid(colValue);
                            }
                            else
                            {
                                resultantCARow[colName] = Convert.ChangeType(colValue, col.DataType);
                            }
                        }
                    }
                    else
                    {
                        continue;
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
        /// Converting XML to Dictionary
        /// </summary>
        /// <param name="xml"></param>
        private static object GetSerializableDictionaryFromXML(string xml)
        {
            SerializableDictionary<string, object> serDictionary = new SerializableDictionary<string, object>();
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xml);
                foreach (XmlElement node in xmlDoc.DocumentElement)
                {
                    serDictionary.Add(node.Name, node.InnerText);
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
            return serDictionary as object;
        }
    }
}