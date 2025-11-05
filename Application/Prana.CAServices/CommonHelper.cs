using Prana.LogManager;
using Prana.Utilities.XMLUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Xml;

namespace Prana.CAServices
{
    internal class CommonHelper
    {
        private static int _requesterHashCode;

        public static int REQUESTER_HASHCODE
        {
            get { return _requesterHashCode; }
            set { _requesterHashCode = value; }
        }



        internal static string GetCorporateActionString(DataTable dt)
        {
            TextWriter tw = new StringWriter();
            ///While writing to xml we don't want the time information but just the date, so we have created a special class for the same.

            ///Should be able to pick up all date properties from xml rathern than hard coding these here.
            List<string> dateProperties = new List<string>();
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
    }
}
