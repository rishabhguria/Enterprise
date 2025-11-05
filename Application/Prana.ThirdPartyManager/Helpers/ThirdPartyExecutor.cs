using Prana.BusinessObjects;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.ThirdPartyManager.DataAccess;
using Prana.Utilities.XMLUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading.Tasks;
using Prana.LogManager;
using Prana.BusinessObjects.AppConstants;
using System.Text.RegularExpressions;
using Infragistics.Win.UltraWinGrid;
using Prana.Global.Utilities;
using static Prana.Global.ApplicationConstants;

namespace Prana.ThirdPartyManager.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public class ThirdPartyExecutor : ThirdPartyCommon, IDisposable
    {
        public EventHandler<MessageEventArgs> OnMessage;

        private const string HEADCOL_GROUPENDS = "GROUPENDS";
        private const string HEADCOL_PBUNIQUEID = "PBUniqueID";
        private const string HEADCOL_ROWHEADER = "RowHeader";
        private const string HEADCOL_GroupAllocationReq = "GroupAllocationReq";
        private const string HEADCOL_FILEHEADER = "FileHeader";
        private const string HEADCOL_FILEFOOTER = "FileFooter";
        private const string HEADCOL_TAXLOTSTATE = "TAXLOTSTATE";
        private const string HEADCOL_ALLOCQTY = "ALLOCQTY";
        private const string HEADCOL_XMLMAINTAG = "XMLMAINTAG";
        private const string HEADCOL_XMLCHILDTAG = "XMLCHILDTAG";
        private const string HEADCOL_XMLFOOTERMAINTAG = "XMLFOOTERMAINTAG";
        private const string HEADCOL_XMLFOOTERCHILDTAG = "XMLFOOTERCHILDTAG";
        private const string HEADCOL_XMLHEADERMAINTAG = "XMLHEADERMAINTAG";
        private const string HEADCOL_XMLHEADERCHILDTAG = "XMLHEADERCHILDTAG";
        private const string HEADCOL_INTERNALNETNOTIONAL = "InternalNetNotional";
        private const string HEADCOL_INTERNALGROSSAMOUNT = "InternalGrossAmount";

        public int ThirdPartyTypeId;
        public int ThirdPartyId;
        public int FileType;
        public string StartupPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThirdPartyExecutor"/> class.
        /// </summary>
        /// <param name="batch">The batch.</param>
        /// <param name="startupPath">The startup path.</param>
        /// <param name="status">The status.</param>
        /// <param name="message">The message.</param>
        /// <remarks></remarks>
        public ThirdPartyExecutor(ThirdPartyBatch batch)
        {
            FileType = 1;
            ThirdPartyId = batch.ThirdPartyId;
            ThirdPartyTypeId = batch.ThirdPartyTypeId;
            StartupPath = Directory.GetCurrentDirectory();
        }

        /// <summary>
        /// Gets the prana path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="date">The date.</param>
        /// <param name="pranaFilePath">The prana file path.</param>
        /// <param name="pranaFileName">Name of the prana file.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private bool GetPranaPath(string filePath, string fileName, DateTime date, ref string pranaFilePath, ref string pranaFileName)
        {

            if (Ext.IsNull(fileName))
            {
                OnMessage(this, new MessageEventArgs("The Naming Convention of The File to be saved is not found."));
                return false;
            }

            string strFileNameAfterClosingBraces = string.Empty;

            int startIndex = fileName.IndexOf("{");
            int lastIndex = fileName.IndexOf("}");
            if (fileName.Contains("{") || fileName.Contains("}"))
            {
                if (startIndex == -1 || lastIndex == -1)
                {
                    OnMessage(this, new MessageEventArgs("The Naming Convention of The File to be saved is not correct."));
                    return false;
                }
                int lengthOfFile = (lastIndex - startIndex) - 1;

                if (lengthOfFile <= 0)
                {
                    OnMessage(this, new MessageEventArgs("The Naming Convention of The File to be saved is not correct."));
                    return false;
                }

                string FileDateFormat = fileName.Substring(startIndex + 1, lengthOfFile);

                string strFileNameBeforeStartBraces = fileName.Substring(0, fileName.IndexOf("{"));
                string strFileNameBeforeClosingBraces = fileName.Substring(0, fileName.IndexOf("}"));
                strFileNameAfterClosingBraces = fileName.Substring(strFileNameBeforeClosingBraces.Length + 1);

                var dateWithCurrentTime = date.Date.AddTicks(DateTime.Now.TimeOfDay.Ticks);

                string DateFormat = dateWithCurrentTime.ToString(FileDateFormat);

                pranaFileName = strFileNameBeforeStartBraces + DateFormat + strFileNameAfterClosingBraces;
                pranaFilePath = String.Format("{0}\\{1}", filePath, pranaFileName);
            }
            else
            {
                pranaFileName = fileName;
                pranaFilePath = String.Format("{0}\\{1}", filePath, fileName);
            }

            CreatePath(pranaFilePath);

            return true;
        }

        /// <summary>
        /// Creates the path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool CreatePath(string path)
        {
            try
            {
                string folder = Path.GetDirectoryName(path);

                if (Directory.Exists(folder) == false)
                {
                    Directory.CreateDirectory(folder);
                }
                return true;
            }
            catch (Exception ex)
            {
                OnMessage(this, new MessageEventArgs(ex.Message));
                return false;
            }
        }

        /// <summary>
        /// Validates the tokens.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool ValidateTokens(ThirdPartyFileFormat format)
        {
            // make sure delimiter is correct
            // format.Delimiter = format.Delimiter.Replace("\\T", "\t");

            //check if delimiter is tab or New line
            if (format.Delimiter.ToUpper().Equals("\\T"))
            {
                format.Delimiter = "\t";
            }
            else if (format.Delimiter.ToUpper().Equals("\\N"))
            {
                format.Delimiter = "\n";
            }

            if (Ext.IsNull(format.Delimiter) || Ext.IsNull(format.DelimiterName))
            {
                OnMessage(this, new MessageEventArgs("Delimiter or Delimiter dispaly name of the file not set."));
                return false;
            }
            return true;
        }

        /// <summary>
        /// Gets the execution info.
        /// </summary>
        /// <param name="batch">The batch.</param>
        /// <param name="format">The format.</param>
        /// <param name="date">The date.</param>
        /// <param name="thirdPartyUserDefinedFormat">The executor.</param>
        /// <param name="startupPath">The startup path.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool CustomiseFormat(ThirdPartyBatch batch, ThirdPartyFileFormat format, DateTime date)
        {
            if (ValidateTokens(format) == false) return false;

            ThirdPartyFlatFileSaveDetail tPFFsaveDetail = ThirdPartyDataManager.GetThirdPartyFlatFileSaveDetail(batch.ThirdPartyCompanyId);

            batch.Format = new ThirdPartyUserDefinedFormat();
            batch.Format.Formatter = format;
            batch.Format.Date = date;
            batch.Format.FileSpec = tPFFsaveDetail;
            batch.Format.StartupPath = Directory.GetCurrentDirectory();

            string thirdPartyShortName = CachedDataManager.GetInstance.GetThirdPartyShortNameByID(format.ThirdPartyID);
            string filePath;
            filePath = String.Format("{0}\\{1}\\{2}", StartupPath, "EOD", thirdPartyShortName);
            string fileName = format.FileDisplayName;

            string pranaFileName = string.Empty;
            string pranaFilePath = string.Empty;

            if (GetPranaPath(filePath, fileName, date, ref pranaFilePath, ref pranaFileName) == false) return false;
            if (GetPranaPath(filePath, fileName, date, ref pranaFilePath, ref pranaFileName) == false) return false;
            if (Ext.IsNull(pranaFilePath)) return false;

            if (FileType == 1)
            {
                if (Ext.IsNotNull(format.FileExtension.Trim()))
                {
                    pranaFileName = String.Format("{0}.{1}", pranaFileName, format.FileExtension.Trim());
                    pranaFilePath = String.Format("{0}.{1}", pranaFilePath, format.FileExtension.Trim());
                }
                batch.Format.FilePath = pranaFilePath;
                batch.Format.FileName = pranaFileName;

            }
            else if (FileType == 2)
            {
                batch.Format.FilePath = pranaFilePath;
                batch.Format.FileName = pranaFileName + ".xls";
            }

            batch.Format.ArchivePath = string.Format("{0}\\{1}\\{2}\\{3}\\{4}", StartupPath, "EOD", thirdPartyShortName, "Archives", batch.Format.FileName);
            batch.Format.LogPath = string.Format("{0}\\{1}\\{2}\\{3}\\{4}", StartupPath, "EOD", thirdPartyShortName, "Archives", string.IsNullOrEmpty(format.FileExtension.Trim()) == true ? batch.Format.FileName + ".Log" : batch.Format.FileName.Replace(format.FileExtension.Trim(), "Log"));

            return true;
        }

        /// <summary>
        /// This method executes when needed to generate a XLS format file
        /// </summary>
        /// <param name="batch"></param>
        public void GenerateExcelFile(ThirdPartyBatch batch)
        {
            try
            {
                string filePath = batch.Format.FilePath;
                DateTime dateHeader = batch.Format.Date;
                ThirdPartyFileFormat thirdPartyFileFormat = batch.Format.Formatter;
                string dateFormat = dateHeader.ToString("MM/dd/yyyy:HHmmss");

                int recordCount = 0;
                
                var dataSet = new DataSet();
                if (!string.IsNullOrEmpty(batch.SerializedDataSet))
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(batch.SerializedDataSet);

                    DataTable dataTable = new DataTable();

                    var listXMLNode = xmlDoc.SelectNodes("//Group");
                    if (listXMLNode != null && listXMLNode.Count > 0)
                    {
                        foreach (XmlNode groupNode in xmlDoc.SelectNodes("//Group"))
                        {
                            DataRow row = dataTable.NewRow();

                            foreach (XmlAttribute attribute in groupNode.Attributes)
                            {
                                if (!dataTable.Columns.Contains(attribute.Name.Replace("_x0020_", " ")))
                                {
                                    // Add DataColumn to DataTable with the name of the XML attribute
                                    dataTable.Columns.Add(attribute.Name.Replace("_x0020_", " "));
                                }

                                // Set the value of the corresponding column in the DataRow
                                row[attribute.Name.Replace("_x0020_", " ")] = attribute.Value;
                            }

                            foreach (XmlNode childNode in groupNode.ChildNodes)
                            {
                                if (!childNode.Name.Equals("ThirdPartyFlatFileDetail"))
                                {
                                    if (childNode.NodeType == XmlNodeType.Element && !dataTable.Columns.Contains(childNode.Name.Replace("_x0020_", " ")))
                                    {
                                        dataTable.Columns.Add(childNode.Name.Replace("_x0020_", " "), typeof(string));
                                    }

                                    row[childNode.Name.Replace("_x0020_", " ")] = childNode.InnerText;
                                }
                            }

                            dataTable.Rows.Add(row);

                            foreach (XmlNode detailNode in groupNode.SelectNodes("ThirdPartyFlatFileDetail"))
                            {
                                DataRow detailRow = dataTable.NewRow();

                                foreach (XmlAttribute attribute in detailNode.Attributes)
                                {
                                    if (!dataTable.Columns.Contains(attribute.Name.Replace("_x0020_", " ")))
                                    {
                                        // Add DataColumn to DataTable with the name of the XML attribute
                                        dataTable.Columns.Add(attribute.Name.Replace("_x0020_", " "));
                                    }

                                    // Set the value of the corresponding column in the DataRow
                                    detailRow[attribute.Name.Replace("_x0020_", " ")] = attribute.Value;
                                }

                                foreach (XmlNode childNode in detailNode.ChildNodes)
                                {
                                    if (childNode.NodeType == XmlNodeType.Element && !dataTable.Columns.Contains(childNode.Name.Replace("_x0020_", " ")))
                                    {
                                        dataTable.Columns.Add(childNode.Name.Replace("_x0020_", " "), typeof(string));
                                    }

                                    detailRow[childNode.Name.Replace("_x0020_", " ")] = childNode.InnerText;
                                }

                                // Add the DataRow to the DataTable
                                dataTable.Rows.Add(detailRow);
                            }
                        }
                    }
                    else
                    {
                        foreach (XmlNode detailNode in xmlDoc.SelectNodes("//ThirdPartyFlatFileDetail"))
                        {
                            DataRow detailRow = dataTable.NewRow();

                            foreach (XmlAttribute attribute in detailNode.Attributes)
                            {
                                if (!dataTable.Columns.Contains(attribute.Name.Replace("_x0020_", " ")))
                                {
                                    // Add DataColumn to DataTable with the name of the XML attribute
                                    dataTable.Columns.Add(attribute.Name.Replace("_x0020_", " "));
                                }

                                // Set the value of the corresponding column in the DataRow
                                detailRow[attribute.Name.Replace("_x0020_", " ")] = attribute.Value;
                            }

                            foreach (XmlNode childNode in detailNode.ChildNodes)
                            {
                                if (childNode.NodeType == XmlNodeType.Element && !dataTable.Columns.Contains(childNode.Name.Replace("_x0020_", " ")))
                                {
                                    dataTable.Columns.Add(childNode.Name.Replace("_x0020_", " "), typeof(string));
                                }

                                detailRow[childNode.Name.Replace("_x0020_", " ")] = childNode.InnerText;
                            }

                            // Add the DataRow to the DataTable
                            dataTable.Rows.Add(detailRow);
                        }
                    }

                    dataSet.Tables.Add(dataTable);
                }           

                if (Directory.Exists(Path.GetDirectoryName(filePath)) == false)
                {
                    OnMessage(this, new MessageEventArgs(filePath, "Directory does not exist"));
                    return;
                }

                DataSet dsGrid = dataSet;             

                if (dsGrid == null)
                {
                    return;
                }

                recordCount = dsGrid.Tables[0].Rows.Count;

                bool blntotalQtyFieldContains = dsGrid.Tables[0].Columns.Contains(HEADCOL_ALLOCQTY);
                bool blnnetNotionalFieldContains = dsGrid.Tables[0].Columns.Contains(HEADCOL_INTERNALNETNOTIONAL);
                bool blngrossAmountFieldContains = dsGrid.Tables[0].Columns.Contains(HEADCOL_INTERNALGROSSAMOUNT);

                double internalNetNotionaltoSend = 0.0;
                double internalGrossAmounttoSend = 0.0;

                double totalQtytoSend = 0.0;
                if (blntotalQtyFieldContains)
                {
                    foreach (DataRow row in dsGrid.Tables[0].Rows)
                    {
                        totalQtytoSend = totalQtytoSend + Convert.ToDouble(row[HEADCOL_ALLOCQTY]);
                    }
                }


                if (blnnetNotionalFieldContains)
                {
                    foreach (DataRow row in dsGrid.Tables[0].Rows)
                    {
                        internalNetNotionaltoSend = internalNetNotionaltoSend + Convert.ToDouble(row[HEADCOL_INTERNALNETNOTIONAL]);
                    }
                }

                if (blngrossAmountFieldContains)
                {
                    foreach (DataRow row in dsGrid.Tables[0].Rows)
                    {
                        internalGrossAmounttoSend = internalGrossAmounttoSend + Convert.ToDouble(row[HEADCOL_INTERNALGROSSAMOUNT]);
                    }
                }

                DataTable dsToExport = new DataTable();
                DataTable dtHeader = null;

                #region Header Code

                string fileHeaderReq = string.Empty;
                bool blnFileHeaderContains = dsGrid.Tables[0].Columns.Contains(HEADCOL_FILEHEADER);
                if (blnFileHeaderContains)
                {
                    fileHeaderReq = dsGrid.Tables[0].Rows[0][HEADCOL_FILEHEADER].ToString();
                }
                if (fileHeaderReq.ToUpper().Equals("TRUE"))
                {
                    if (Ext.IsNull(thirdPartyFileFormat.HeaderFile))
                    {
                        OnMessage(this, new MessageEventArgs("No Header XSLT file name is available for the selected ThirdParty. Processing without header."));

                        // sw.Close();
                        //return;
                    }
                    else
                    {

                        string headerXsltPath = thirdPartyFileFormat.HeaderFile;
                        // get the XSLT Name
                        string headerXsltName = headerXsltPath.Substring(headerXsltPath.LastIndexOf("\\") + 1);
                        // complete the path from StartUp Path
                        string xsltForHeaderStartUpPath = String.Format(@"{0}\{1}\{2}\{3}", StartupPath, ApplicationConstants.MAPPING_FILE_DIRECTORY, ApplicationConstants.MappingFileType.ThirdPartyXSLT, headerXsltName);

                        ThirdPartyFlatFileHeader thirdPartyFlatFileHeader = new ThirdPartyFlatFileHeader();

                        thirdPartyFlatFileHeader.Date = Ext.ShortDateStr(batch.Format.Date);
                        thirdPartyFlatFileHeader.DateAndTime = dateFormat;
                        thirdPartyFlatFileHeader.RecordCount = recordCount;

                        string serializeXMLforHeader = XMLUtilities.SerializeToXML(thirdPartyFlatFileHeader);

                        string convertedXMLforHeader = StartupPath + @"\ConvertedThirdPartyXMLforHeader.xml";
                        StringReader sr = new StringReader(serializeXMLforHeader);
                        XmlTextReader xreader = new System.Xml.XmlTextReader(sr);
                        string mappedfilePathforHeader = XMLUtilities.GetTransformed(xreader, convertedXMLforHeader, xsltForHeaderStartUpPath);

                        if (!mappedfilePathforHeader.Equals(""))
                        {
                            DataSet dsHeader = new DataSet();
                            dsHeader.ReadXml(mappedfilePathforHeader);
                            if (dsHeader.Tables.Count <= 0)
                            {
                                OnMessage(this, new MessageEventArgs("No data available for Header"));
                                return;
                            }
                            dtHeader = dsHeader.Tables[0];
                        }
                    }
                }

                #endregion Header Code

                string groupAllocationRequired = string.Empty;
                // check group and allocation requires
                bool blnGroupAlloReq = dsGrid.Tables[0].Columns.Contains(HEADCOL_GroupAllocationReq);
                if (blnGroupAlloReq)
                {
                    groupAllocationRequired = dsGrid.Tables[0].Rows[0][HEADCOL_GroupAllocationReq].ToString();
                }

                // general delimited file generation code
                #region general delimited file generation code
                if (groupAllocationRequired.Equals(string.Empty) || groupAllocationRequired.ToUpper().Equals("FALSE"))
                {
                    string rowHeaderReq = string.Empty;
                    bool blnRowHeaderContains = dsGrid.Tables[0].Columns.Contains(HEADCOL_ROWHEADER);
                    if (blnRowHeaderContains)
                    {
                        rowHeaderReq = dsGrid.Tables[0].Rows[0][HEADCOL_ROWHEADER].ToString();
                    }

                    DataTable dt = dsGrid.Tables[0];
                    int iColCount = dt.Columns.Count;
                    int ind = 0;
                    // check column header requires, if true the write
                    if (string.IsNullOrEmpty(rowHeaderReq) || rowHeaderReq.ToUpper().Equals("TRUE"))
                    {
                        //if required, First of all write the column header. 
                        for (int i = 0; i < iColCount; i++)
                        {
                            if (!ColumnExists(dt.Columns[i].ColumnName) &&
                                !dt.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_TAXLOTSTATE) &&
                                !dsToExport.Columns.Contains(dt.Columns[i].ColumnName))
                            {
                                dsToExport.Columns.Add(dt.Columns[i].ColumnName);
                            }
                        }
                    }
                    else
                    {
                        DataRow dr = dt.Rows[0];
                        for (int i = 0; i < iColCount; i++)
                        {
                            if (!ColumnExists(dt.Columns[i].ColumnName) &&
                                !dt.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_TAXLOTSTATE) &&
                                !dsToExport.Columns.Contains(dt.Columns[i].ColumnName))
                            {
                                dsToExport.Columns.Add(dt.Columns[i].ColumnName);
                                dsToExport.Columns[dt.Columns[i].ColumnName].Caption = dr[dt.Columns[i].ColumnName].ToString();
                            }
                        }
                        ind = 1;
                    }

                    // Now write all the data rows.
                    for (; ind < dt.Rows.Count; ind++)
                    {
                        DataRow dr = dt.Rows[ind];
                        DataRow row = dsToExport.Rows.Add();
                        bool isValueInert = false;

                        for (int i = 0; i < iColCount; i++)
                        {

                            if (!ColumnExists(dt.Columns[i].ColumnName) &&
                                 !dt.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_TAXLOTSTATE) &&
                                 dsToExport.Columns.Contains(dt.Columns[i].ColumnName))
                            {
                                if (!Convert.IsDBNull(dr[i]))
                                {
                                    row[dt.Columns[i].ColumnName] = dr[i].ToString();
                                    isValueInert = true;
                                }
                            }
                        }
                        if (!isValueInert)
                        {
                            dsToExport.Rows.Remove(row);
                        }
                    }
                }

                #endregion general delimited file generation code

                // group and allocation generation code
                #region group and allocation generation code
                else if (groupAllocationRequired.ToUpper().Equals("TRUE"))
                {
                    string rowHeaderReq = string.Empty;
                    bool blnRowHeaderContains = dsGrid.Tables[0].Columns.Contains(HEADCOL_ROWHEADER);
                    if (blnRowHeaderContains)
                    {
                        rowHeaderReq = dsGrid.Tables[0].Rows[0][HEADCOL_ROWHEADER].ToString();
                    }

                    List<string> groupHeadingColl = new List<string>();
                    List<string> allocationHeadingColl = new List<string>();

                    DataTable dt = dsGrid.Tables[0];
                    bool groupEndsPassed = false;
                    int iColCount = dt.Columns.Count;
                    // collect headers for Group and Allocations
                    for (int i = 0; i < iColCount; i++)
                    {
                        if (dt.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_GROUPENDS))
                        {
                            groupEndsPassed = true;
                        }
                        if (!dt.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_GROUPENDS) && groupEndsPassed.Equals(false))
                        {
                            if (!ColumnExists(dt.Columns[i].ColumnName) &&
                                 !dt.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_TAXLOTSTATE))
                            {
                                if (!groupHeadingColl.Contains(Convert.ToString(dt.Columns[i].ColumnName)))
                                {
                                    groupHeadingColl.Add(dt.Columns[i].ToString());
                                }
                            }
                        }
                        else
                        {
                            if (!ColumnExists(dt.Columns[i].ColumnName) &&
                                 !dt.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_TAXLOTSTATE))
                            {
                                if (!allocationHeadingColl.Contains(Convert.ToString(dt.Columns[i].ColumnName)))
                                {
                                    allocationHeadingColl.Add(dt.Columns[i].ToString());
                                }
                            }
                        }
                    }

                    Dictionary<long, DataRow> groupDataDict = new Dictionary<long, DataRow>();
                    Dictionary<long, List<DataRow>> allocationDataDict = new Dictionary<long, List<DataRow>>();

                    foreach (DataRow dr in dt.Rows)
                    {
                        for (int i = 0; i < iColCount; i++)
                        {
                            if (dt.Columns[i].ColumnName.Equals(HEADCOL_PBUNIQUEID))
                            {
                                if (!groupDataDict.ContainsKey(Convert.ToInt64(dr[i].ToString())))
                                {
                                    groupDataDict.Add(Convert.ToInt64(dr[i].ToString()), dr);
                                    recordCount = recordCount + 1;
                                    List<DataRow> datarowcoll = new List<DataRow>();
                                    datarowcoll.Add(dr);
                                    allocationDataDict.Add(Convert.ToInt64(dr[i].ToString()), datarowcoll);
                                    recordCount = recordCount + 1;
                                }
                                else
                                {
                                    List<DataRow> datarowList = allocationDataDict[Convert.ToInt64(dr[i].ToString())];
                                    datarowList.Add(dr);
                                    allocationDataDict[Convert.ToInt64(dr[i].ToString())] = datarowList;
                                    recordCount = recordCount + 1;
                                }
                            }
                        }
                    }

                    bool groupEnds = false;
                    foreach (KeyValuePair<long, DataRow> kvp in groupDataDict)
                    {
                        List<DataRow> allocationList = new List<DataRow>();
                        groupEnds = false;
                        if (groupDataDict.ContainsKey(kvp.Key))
                        {
                            if (allocationDataDict.ContainsKey(kvp.Key))
                            {
                                allocationList = allocationDataDict[kvp.Key];
                            }

                            // check row header requires, if true then write
                            if (string.IsNullOrEmpty(rowHeaderReq) || rowHeaderReq.ToUpper().Equals("TRUE"))
                            {
                                //write header for group  
                                int grpHeadcolCount = groupHeadingColl.Count;
                                for (int i = 0; i < grpHeadcolCount; i++)
                                {
                                    if (!dsToExport.Columns.Contains(groupHeadingColl[i]))
                                        dsToExport.Columns.Add(groupHeadingColl[i]);
                                }
                            }

                            // write row data for group
                            DataRow groupRow = groupDataDict[kvp.Key];
                            int colCount = groupRow.Table.Columns.Count;
                            for (int i = 0; i < colCount; i++)
                            {
                                if (groupRow.Table.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_GROUPENDS))
                                {
                                    groupEnds = true;
                                }
                                if (!groupRow.Table.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_GROUPENDS) && groupEnds.Equals(false))
                                {
                                    if (!ColumnExists(groupRow.Table.Columns[i].ColumnName) &&
                                        !groupRow.Table.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_TAXLOTSTATE) &&
                                        !dsToExport.Columns.Contains(groupRow.Table.Columns[i].ColumnName))
                                    {
                                        dsToExport.Columns.Add(groupRow.Table.Columns[i].ColumnName);
                                    }
                                }
                            }

                            // check row header requires, if true then write
                            if (string.IsNullOrEmpty(rowHeaderReq) || rowHeaderReq.ToUpper().Equals("TRUE"))
                            {
                                DataRow row = dsToExport.Rows.Add();
                                //write header for allocation 
                                int allHeadcolCount = allocationHeadingColl.Count;
                                for (int i = 0; i < allHeadcolCount; i++)
                                {
                                    row[i] = allocationHeadingColl[i];
                                }
                            }

                            //allocationList
                            foreach (DataRow dtRow in allocationList)
                            {
                                bool alloends = false;
                                int allColCount = dtRow.Table.Columns.Count;
                                DataRow row = dsToExport.Rows.Add();
                                bool isValueInert = false;
                                for (int i = 0; i < allColCount; i++)
                                {
                                    if (dtRow.Table.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_GROUPENDS))
                                    {
                                        alloends = true;
                                    }
                                    if (!dtRow.Table.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_GROUPENDS) && alloends.Equals(false))
                                    {

                                    }
                                    else
                                    {
                                        if (!ColumnExists(dtRow.Table.Columns[i].ColumnName) &&
                                            !dtRow.Table.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_TAXLOTSTATE) &&
                                            dsToExport.Columns.Contains(dtRow.Table.Columns[i].ColumnName))
                                        {
                                            row[dtRow.Table.Columns[i].ColumnName] = dtRow[i].ToString();
                                            isValueInert = true;
                                        }
                                    }
                                }
                                if (!isValueInert)
                                {
                                    dsToExport.Rows.Remove(row);
                                }
                            }
                        }
                    }
                }
                #endregion group  and allocation generation code

                #region Footer Code

                string fileFooterReq = string.Empty;

                bool blnFileFooterContains = dsGrid.Tables[0].Columns.Contains(HEADCOL_FILEFOOTER);
                if (blnFileFooterContains)
                {
                    fileFooterReq = dsGrid.Tables[0].Rows[0][HEADCOL_FILEFOOTER].ToString();
                }
                if (fileFooterReq.ToUpper().Equals("TRUE"))
                {
                    if (string.IsNullOrEmpty(thirdPartyFileFormat.FooterFile))
                    {
                        OnMessage(this, new MessageEventArgs("No Footer XSLT name is available for the selected ThirdParty. Generating file without Footer."));
                    }
                    else
                    {
                        string footerXsltPath = thirdPartyFileFormat.FooterFile;
                        // get the XSLT Name
                        string footerXsltName = footerXsltPath.Substring(footerXsltPath.LastIndexOf("\\") + 1);
                        // complete the path from StartUp Path
                        string xsltForFooterStartUpPath = String.Format(@"{0}\{1}\{2}\{3}", StartupPath, ApplicationConstants.MAPPING_FILE_DIRECTORY, ApplicationConstants.MappingFileType.ThirdPartyXSLT, footerXsltName);
                        ThirdPartyFlatFileFooter thirdPartyFlatFileFooter = new ThirdPartyFlatFileFooter();
                        thirdPartyFlatFileFooter.RecordCount = recordCount;
                        thirdPartyFlatFileFooter.TotalQty = totalQtytoSend;
                        thirdPartyFlatFileFooter.Date = Ext.ShortDateStr(batch.Format.Date);
                        thirdPartyFlatFileFooter.DateAndTime = dateFormat;
                        thirdPartyFlatFileFooter.InternalNetNotional = internalNetNotionaltoSend;
                        thirdPartyFlatFileFooter.InternalGrossAmount = internalGrossAmounttoSend;
                        string serializeXMLforFooter = XMLUtilities.SerializeToXML(thirdPartyFlatFileFooter);
                        string convertedXMLforFooter = StartupPath + @"\ConvertedThirdPartyXMLforFooter.xml";
                        StringReader sreader = new StringReader(serializeXMLforFooter);
                        XmlTextReader xmlreader = new XmlTextReader(sreader);
                        string mappedfilePathforFooter = XMLUtilities.GetTransformed(xmlreader, convertedXMLforFooter, xsltForFooterStartUpPath);

                        if (!mappedfilePathforFooter.Equals(""))
                        {
                            DataSet dsFooter = new DataSet();
                            dsFooter.ReadXml(mappedfilePathforFooter);
                            if (dsFooter.Tables.Count <= 0)
                            {
                                OnMessage(this, new MessageEventArgs("No data available for Footer"));
                                return;
                            }
                            DataTable dtFooter = dsFooter.Tables[0];

                            string rowHeaderReq = string.Empty;
                            bool blnRowHeaderContains = dtFooter.Columns.Contains(HEADCOL_ROWHEADER);
                            if (blnRowHeaderContains)
                            {
                                rowHeaderReq = dtFooter.Rows[0][HEADCOL_ROWHEADER].ToString();
                            }

                            int iColCountFooter = dtFooter.Columns.Count;
                            int indColCounter = 0;

                            // check column header requires, if true the write
                            if (string.IsNullOrEmpty(rowHeaderReq) || rowHeaderReq.ToUpper().Equals("TRUE"))
                            {
                                DataRow row = dsToExport.Rows.Add();
                                // First we will write the headers.    
                                for (int i = 0; i < iColCountFooter; i++)
                                {
                                    if (!ColumnExists(dtFooter.Columns[i].ColumnName) &&
                                        !dtFooter.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_TAXLOTSTATE))
                                    {
                                        row[indColCounter] = dtFooter.Columns[i].ColumnName;
                                        indColCounter++;
                                        if (indColCounter >= dsToExport.Columns.Count)
                                            dsToExport.Columns.Add();
                                    }
                                }
                            }
                            // Now write all the rows.
                            foreach (DataRow dr in dtFooter.Rows)
                            {
                                DataRow row = dsToExport.Rows.Add();
                                indColCounter = 0;
                                for (int i = 0; i < iColCountFooter; i++)
                                {
                                    if (!ColumnExists(dtFooter.Columns[i].ColumnName) &&
                                        !dtFooter.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_TAXLOTSTATE))
                                    {
                                        if (!Convert.IsDBNull(dr[i]))
                                        {

                                            string value = dr[i].ToString();
                                            while (value.Length > 2 && (value.StartsWith("\n") || value.StartsWith("\t")))
                                            {
                                                if (value.StartsWith("\n"))
                                                {
                                                    row = dsToExport.Rows.Add();
                                                    indColCounter = 0;
                                                }
                                                value = value.Substring(1);
                                            }
                                            if (indColCounter >= dsToExport.Columns.Count)
                                                dsToExport.Columns.Add();
                                            row[indColCounter] = value;
                                            indColCounter++;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion Footer Code

                if (dsToExport != null)
                {
                    string rowHeaderReq = string.Empty;
                    bool blnRowHeaderContains = false;
                    if (dtHeader != null)
                    {
                        blnRowHeaderContains = dtHeader.Columns.Contains(HEADCOL_ROWHEADER);
                        if (blnRowHeaderContains)
                            rowHeaderReq = dtHeader.Rows[0][HEADCOL_ROWHEADER].ToString();
                    }

                    Infragistics.Documents.Excel.Workbook workBook = new Infragistics.Documents.Excel.Workbook();
                    string fileName = Path.GetFileNameWithoutExtension(filePath);
                    var worksheets = workBook.Worksheets.Add(fileName);
                    for (int i = 0; i < dsToExport.Columns.Count; i++)
                    {
                        worksheets.Rows[0].Cells[i].Value = dsToExport.Columns[i].ColumnName;
                    }
                    for (int i = 0; i < dsToExport.Rows.Count; i++)
                    {
                        for (int j = 0; j < dsToExport.Columns.Count; j++)
                        {
                            worksheets.Rows[i + 1].Cells[j].Value = dsToExport.Rows[i][j];
                        }
                    }

                    if (dtHeader != null && dtHeader.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtHeader.Columns.Count; i++)
                        {
                            string colName = dtHeader.Columns[i].ColumnName;
                            if (ColumnExists(colName) ||
                                colName.ToUpper().Equals(HEADCOL_ROWHEADER) ||
                                colName.ToUpper().Equals(HEADCOL_TAXLOTSTATE))
                            {
                                dtHeader.Columns.Remove(colName);
                                i--;
                            }
                        }
                        int rowCounter = 0;
                        if (string.IsNullOrEmpty(rowHeaderReq) || rowHeaderReq.ToUpper().Equals("TRUE"))
                        {
                            workBook.Worksheets[0].Rows.Insert(0, dtHeader.Rows.Count + 1);
                            int colCounter = 0;
                            foreach (DataColumn col in dtHeader.Columns)
                            {
                                workBook.Worksheets[0].Rows[rowCounter].Cells[colCounter].Value = col.ColumnName;
                                colCounter++;
                            }
                            rowCounter++;
                        }
                        else
                            workBook.Worksheets[0].Rows.Insert(0, dtHeader.Rows.Count);
                        foreach (DataRow row in dtHeader.Rows)
                        {
                            int colCounter = 0;
                            foreach (DataColumn col in dtHeader.Columns)
                            {
                                workBook.Worksheets[0].Rows[rowCounter].Cells[colCounter].Value = row[col].ToString();
                                colCounter++;
                            }
                            rowCounter++;
                        }
                    }
                    workBook.Save(filePath);
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
        }

        /// <summary>
        /// This method executes when needed to generate a CSV format file
        /// </summary>
        /// <param name="batch"></param>
        public void GenerateUserDefindFormat(ThirdPartyBatch batch)
        {
            try
            {
                string filePath = batch.Format.FilePath;
                DateTime dateHeader = batch.Format.Date;
                ThirdPartyFileFormat thirdPartyFileFormat = batch.Format.Formatter;
                string delimiterSymbol = thirdPartyFileFormat.Delimiter;
                string dateFormat = dateHeader.ToString("MM/dd/yyyy:HHmmss");

                var dataSet = new DataSet();
                if (!string.IsNullOrEmpty(batch.SerializedDataSet))
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(batch.SerializedDataSet);

                    DataTable dataTable = new DataTable();

                    var listXMLNode = xmlDoc.SelectNodes("//Group");
                    if (listXMLNode != null && listXMLNode.Count > 0)
                    {
                        foreach (XmlNode groupNode in xmlDoc.SelectNodes("//Group"))
                        {
                            DataRow row = dataTable.NewRow();

                            foreach (XmlAttribute attribute in groupNode.Attributes)
                            {
                                if (!dataTable.Columns.Contains(attribute.Name.Replace("_x0020_", " ")))
                                {
                                    // Add DataColumn to DataTable with the name of the XML attribute
                                    dataTable.Columns.Add(attribute.Name.Replace("_x0020_", " "));
                                }

                                // Set the value of the corresponding column in the DataRow
                                row[attribute.Name.Replace("_x0020_", " ")] = attribute.Value;
                            }

                            foreach (XmlNode childNode in groupNode.ChildNodes)
                            {
                                if (!childNode.Name.Equals("ThirdPartyFlatFileDetail"))
                                {
                                    if (childNode.NodeType == XmlNodeType.Element && !dataTable.Columns.Contains(childNode.Name.Replace("_x0020_", " ")))
                                    {
                                        dataTable.Columns.Add(childNode.Name.Replace("_x0020_", " "), typeof(string));
                                    }

                                    row[childNode.Name.Replace("_x0020_", " ")] = childNode.InnerText;
                                }
                            }

                            dataTable.Rows.Add(row);

                            foreach (XmlNode detailNode in groupNode.SelectNodes("ThirdPartyFlatFileDetail"))
                            {
                                DataRow detailRow = dataTable.NewRow();

                                foreach (XmlAttribute attribute in detailNode.Attributes)
                                {
                                    if (!dataTable.Columns.Contains(attribute.Name.Replace("_x0020_", " ")))
                                    {
                                        // Add DataColumn to DataTable with the name of the XML attribute
                                        dataTable.Columns.Add(attribute.Name.Replace("_x0020_", " "));
                                    }

                                    // Set the value of the corresponding column in the DataRow
                                    detailRow[attribute.Name.Replace("_x0020_", " ")] = attribute.Value;
                                }

                                foreach (XmlNode childNode in detailNode.ChildNodes)
                                {
                                    if (childNode.NodeType == XmlNodeType.Element && !dataTable.Columns.Contains(childNode.Name.Replace("_x0020_", " ")))
                                    {
                                        dataTable.Columns.Add(childNode.Name.Replace("_x0020_", " "), typeof(string));
                                    }

                                    detailRow[childNode.Name.Replace("_x0020_", " ")] = childNode.InnerText;
                                }

                                // Add the DataRow to the DataTable
                                dataTable.Rows.Add(detailRow);
                            }
                        }
                    }
                    else
                    {
                        foreach (XmlNode detailNode in xmlDoc.SelectNodes("//ThirdPartyFlatFileDetail"))
                        {
                            DataRow detailRow = dataTable.NewRow();

                            foreach (XmlAttribute attribute in detailNode.Attributes)
                            {
                                if (!dataTable.Columns.Contains(attribute.Name.Replace("_x0020_", " ")))
                                {
                                    // Add DataColumn to DataTable with the name of the XML attribute
                                    dataTable.Columns.Add(attribute.Name.Replace("_x0020_", " "));
                                }

                                // Set the value of the corresponding column in the DataRow
                                detailRow[attribute.Name.Replace("_x0020_", " ")] = attribute.Value;
                            }

                            foreach (XmlNode childNode in detailNode.ChildNodes)
                            {
                                if (childNode.NodeType == XmlNodeType.Element && !dataTable.Columns.Contains(childNode.Name.Replace("_x0020_", " ")))
                                {
                                    dataTable.Columns.Add(childNode.Name.Replace("_x0020_", " "), typeof(string));
                                }

                                detailRow[childNode.Name.Replace("_x0020_", " ")] = childNode.InnerText;
                            }

                            // Add the DataRow to the DataTable
                            dataTable.Rows.Add(detailRow);
                        }
                    }

                    dataSet.Tables.Add(dataTable);
                }

                if (dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
                {
                    if (batch.TransmissionType.Equals(((int)TransmissionType.File).ToString()))
                    {
                        OnMessage(this, new MessageEventArgs("No Data Found"));
                    }                  
                    return;
                }

                if (Directory.Exists(Path.GetDirectoryName(filePath)) == false)
                {
                    OnMessage(this, new MessageEventArgs(filePath, "Directory does not exist"));
                    return;
                }

                int recordCount = 0;
                StringBuilder s = new StringBuilder();

                DataSet dsGrid = dataSet;
                if (dsGrid == null)
                {
                    return;
                }

                recordCount = dsGrid.Tables[0].Rows.Count;

                bool blntotalQtyFieldContains = dsGrid.Tables[0].Columns.Contains(HEADCOL_ALLOCQTY);
                bool blnnetNotionalFieldContains = dsGrid.Tables[0].Columns.Contains(HEADCOL_INTERNALNETNOTIONAL);
                bool blngrossAmountFieldContains = dsGrid.Tables[0].Columns.Contains(HEADCOL_INTERNALGROSSAMOUNT);

                double internalNetNotionaltoSend = 0.0;
                double internalGrossAmounttoSend = 0.0;

                double totalQtytoSend = 0.0;
                if (blntotalQtyFieldContains)
                {
                    foreach (DataRow row in dsGrid.Tables[0].Rows)
                    {
                        double allocQty = 0.0;
                        if (double.TryParse(row[HEADCOL_ALLOCQTY].ToString(), out allocQty))
                            totalQtytoSend = totalQtytoSend + allocQty;
                    }
                }

                if (blnnetNotionalFieldContains)
                {
                    foreach (DataRow row in dsGrid.Tables[0].Rows)
                    {
                        internalNetNotionaltoSend = internalNetNotionaltoSend + Convert.ToDouble(row[HEADCOL_INTERNALNETNOTIONAL]);
                    }
                }

                if (blngrossAmountFieldContains)
                {
                    foreach (DataRow row in dsGrid.Tables[0].Rows)
                    {
                        internalGrossAmounttoSend = internalGrossAmounttoSend + Convert.ToDouble(row[HEADCOL_INTERNALGROSSAMOUNT]);
                    }
                }

                StreamWriter sw = new StreamWriter(filePath);

                #region Header Code

                string fileHeaderReq = string.Empty;
                bool blnFileHeaderContains = dsGrid.Tables[0].Columns.Contains(HEADCOL_FILEHEADER);
                if (blnFileHeaderContains)
                {
                    fileHeaderReq = dsGrid.Tables[0].Rows[0][HEADCOL_FILEHEADER].ToString();
                }
                if (fileHeaderReq.ToUpper().Equals("TRUE"))
                {
                    if (Ext.IsNull(thirdPartyFileFormat.HeaderFile))
                    {
                        OnMessage(this, new MessageEventArgs("No Header XSLT file name is available for the selected ThirdParty. Processing without header."));
                    }
                    else
                    {
                        string headerXsltPath = thirdPartyFileFormat.HeaderFile;
                        // get the XSLT Name
                        string headerXsltName = headerXsltPath.Substring(headerXsltPath.LastIndexOf("\\") + 1);
                        // complete the path from StartUp Path
                        string xsltForHeaderStartUpPath = String.Format(@"{0}\{1}\{2}\{3}", StartupPath, ApplicationConstants.MAPPING_FILE_DIRECTORY, ApplicationConstants.MappingFileType.ThirdPartyXSLT, headerXsltName);

                        ThirdPartyFlatFileHeader thirdPartyFlatFileHeader = new ThirdPartyFlatFileHeader();

                        thirdPartyFlatFileHeader.Date = Ext.ShortDateStr(batch.Format.Date);
                        thirdPartyFlatFileHeader.DateAndTime = dateFormat;
                        thirdPartyFlatFileHeader.RecordCount = recordCount;

                        string serializeXMLforHeader = XMLUtilities.SerializeToXML(thirdPartyFlatFileHeader);

                        string convertedXMLforHeader = StartupPath + @"\ConvertedThirdPartyXMLforHeader.xml";
                        StringReader sr = new StringReader(serializeXMLforHeader);
                        XmlTextReader xreader = new XmlTextReader(sr);
                        string mappedfilePathforHeader = XMLUtilities.GetTransformed(xreader, convertedXMLforHeader, xsltForHeaderStartUpPath);

                        if (!mappedfilePathforHeader.Equals(""))
                        {
                            DataSet dsHeader = new DataSet();
                            dsHeader.ReadXml(mappedfilePathforHeader);
                            if (dsHeader.Tables.Count <= 0)
                            {
                                sw.Close();
                                OnMessage(this, new MessageEventArgs("No data available for Header"));
                                return;
                            }
                            DataTable dtHeader = dsHeader.Tables[0];

                            string rowHeaderReq = string.Empty;
                            bool blnRowHeaderContains = dtHeader.Columns.Contains(HEADCOL_ROWHEADER);
                            if (blnRowHeaderContains)
                            {
                                rowHeaderReq = dtHeader.Rows[0][HEADCOL_ROWHEADER].ToString();
                            }

                            int iColCountHeader = dtHeader.Columns.Count;
                            // check row header requires, if true the write
                            if (string.IsNullOrEmpty(rowHeaderReq) || rowHeaderReq.ToUpper().Equals("TRUE"))
                            {
                                // First of all write the column headers.
                                for (int i = 0; i < iColCountHeader; i++)
                                {
                                    if (!ColumnExists(dtHeader.Columns[i].ColumnName) &&
                                        !dtHeader.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_TAXLOTSTATE))
                                    {
                                        s.Append(dtHeader.Columns[i]).Append(delimiterSymbol);
                                    }
                                }
                                s.Remove(s.Length - 1, 1);
                                sw.WriteLine(s.ToString());
                                s = new StringBuilder();
                            }
                            // Now write all the row data.
                            foreach (DataRow dr in dtHeader.Rows)
                            {
                                for (int i = 0; i < iColCountHeader; i++)
                                {
                                    if (!ColumnExists(dtHeader.Columns[i].ColumnName) &&
                                        !dtHeader.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_TAXLOTSTATE))
                                    {
                                        if (!Convert.IsDBNull(dr[i]))
                                        {
                                            s.Append(dr[i].ToString()).Append(delimiterSymbol);
                                        }
                                    }
                                }
                                s.Remove(s.Length - 1, 1);
                            }
                            sw.WriteLine(s.ToString());
                            s = new StringBuilder();
                        }
                    }
                }

                #endregion Header Code

                string groupAllocationRequired = string.Empty;
                // check group and allocation requires
                bool blnGroupAlloReq = dsGrid.Tables[0].Columns.Contains(HEADCOL_GroupAllocationReq);
                if (blnGroupAlloReq)
                {
                    groupAllocationRequired = dsGrid.Tables[0].Rows[0][HEADCOL_GroupAllocationReq].ToString();
                }

                // general delimited file generation code
                #region general delimited file generation code
                if (groupAllocationRequired.Equals(string.Empty) || groupAllocationRequired.ToUpper().Equals("FALSE"))
                {
                    string rowHeaderReq = string.Empty;
                    bool blnRowHeaderContains = dsGrid.Tables[0].Columns.Contains(HEADCOL_ROWHEADER);
                    if (blnRowHeaderContains)
                    {
                        rowHeaderReq = dsGrid.Tables[0].Rows[0][HEADCOL_ROWHEADER].ToString();
                    }

                    DataTable dt = dsGrid.Tables[0];
                    int iColCount = dt.Columns.Count;

                    // check column header requires, if true the write
                    if (string.IsNullOrEmpty(rowHeaderReq) || rowHeaderReq.ToUpper().Equals("TRUE"))
                    {
                        //if required, First of all write the column header. 
                        for (int i = 0; i < iColCount; i++)
                        {
                            if (!ColumnExists(dt.Columns[i].ColumnName) &&
                                !dt.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_TAXLOTSTATE))
                            {
                                s.Append(dt.Columns[i]).Append(delimiterSymbol);
                            }
                        }
                        s.Remove(s.Length - 1, 1);
                        sw.WriteLine(s.ToString());
                        s = new StringBuilder();
                    }
                    // Now write all the data rows.
                    foreach (DataRow dr in dt.Rows)
                    {
                        for (int i = 0; i < iColCount; i++)
                        {

                            if (!ColumnExists(dt.Columns[i].ColumnName) &&
                                 !dt.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_TAXLOTSTATE))
                            {
                                if (!Convert.IsDBNull(dr[i]))
                                {
                                    s.Append(dr[i].ToString()).Append(delimiterSymbol);
                                }
                            }
                        }
                        s.Remove(s.Length - 1, 1);
                        //NewLine does the job, and writing is done only once
                        s.Append(Environment.NewLine);
                    }
                    sw.Write(s.ToString());
                    s = new StringBuilder();
                }

                #endregion general delimited file generation code

                // group and allocation generation code
                #region group and allocation generation code
                else if (groupAllocationRequired.ToUpper().Equals("TRUE"))
                {
                    string rowHeaderReq = string.Empty;
                    bool blnRowHeaderContains = dsGrid.Tables[0].Columns.Contains(HEADCOL_ROWHEADER);
                    if (blnRowHeaderContains)
                    {
                        rowHeaderReq = dsGrid.Tables[0].Rows[0][HEADCOL_ROWHEADER].ToString();
                    }

                    List<string> groupHeadingColl = new List<string>();
                    List<string> allocationHeadingColl = new List<string>();

                    DataTable dt = dsGrid.Tables[0];
                    bool groupEndsPassed = false;
                    int iColCount = dt.Columns.Count;
                    // collect headers for Group and Allocations
                    for (int i = 0; i < iColCount; i++)
                    {
                        if (dt.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_GROUPENDS))
                        {
                            groupEndsPassed = true;
                        }
                        if (!dt.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_GROUPENDS) && groupEndsPassed.Equals(false))
                        {
                            if (!ColumnExists(dt.Columns[i].ColumnName) &&
                                 !dt.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_TAXLOTSTATE))
                            {
                                if (!groupHeadingColl.Contains(Convert.ToString(dt.Columns[i].ColumnName)))
                                {
                                    groupHeadingColl.Add(dt.Columns[i].ToString());
                                }
                            }
                        }
                        else
                        {
                            if (!ColumnExists(dt.Columns[i].ColumnName) &&
                                 !dt.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_TAXLOTSTATE))
                            {
                                if (!allocationHeadingColl.Contains(Convert.ToString(dt.Columns[i].ColumnName)))
                                {
                                    allocationHeadingColl.Add(dt.Columns[i].ToString());
                                }
                            }
                        }
                    }

                    Dictionary<long, DataRow> groupDataDict = new Dictionary<long, DataRow>();
                    Dictionary<long, List<DataRow>> allocationDataDict = new Dictionary<long, List<DataRow>>();

                    foreach (DataRow dr in dt.Rows)
                    {
                        for (int i = 0; i < iColCount; i++)
                        {
                            if (dt.Columns[i].ColumnName.Equals(HEADCOL_PBUNIQUEID))
                            {
                                if (!groupDataDict.ContainsKey(Convert.ToInt64(dr[i].ToString())))
                                {
                                    groupDataDict.Add(Convert.ToInt64(dr[i].ToString()), dr);
                                    recordCount = recordCount + 1;
                                    List<DataRow> datarowcoll = new List<DataRow>();
                                    datarowcoll.Add(dr);
                                    allocationDataDict.Add(Convert.ToInt64(dr[i].ToString()), datarowcoll);
                                    recordCount = recordCount + 1;
                                }
                                else
                                {
                                    List<DataRow> datarowList = allocationDataDict[Convert.ToInt64(dr[i].ToString())];
                                    datarowList.Add(dr);
                                    allocationDataDict[Convert.ToInt64(dr[i].ToString())] = datarowList;
                                    recordCount = recordCount + 1;
                                }
                            }
                        }
                    }

                    bool groupEnds = false;
                    foreach (KeyValuePair<long, DataRow> kvp in groupDataDict)
                    {
                        List<DataRow> allocationList = new List<DataRow>();
                        groupEnds = false;
                        if (groupDataDict.ContainsKey(kvp.Key))
                        {
                            if (allocationDataDict.ContainsKey(kvp.Key))
                            {
                                allocationList = allocationDataDict[kvp.Key];
                            }

                            // check row header requires, if true then write
                            if (string.IsNullOrEmpty(rowHeaderReq) || rowHeaderReq.ToUpper().Equals("TRUE"))
                            {
                                //write header for group  
                                int grpHeadcolCount = groupHeadingColl.Count;
                                for (int i = 0; i < grpHeadcolCount; i++)
                                {
                                    s.Append(groupHeadingColl[i]).Append(delimiterSymbol);
                                }
                                s.Remove(s.Length - 1, 1);
                                sw.WriteLine(s.ToString());
                                s = new StringBuilder();
                            }
                            // write row data for group
                            DataRow groupRow = groupDataDict[kvp.Key];
                            int colCount = groupRow.Table.Columns.Count;
                            for (int i = 0; i < colCount; i++)
                            {
                                if (groupRow.Table.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_GROUPENDS))
                                {
                                    groupEnds = true;
                                }
                                if (!groupRow.Table.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_GROUPENDS) && groupEnds.Equals(false))
                                {
                                    if (!ColumnExists(groupRow.Table.Columns[i].ColumnName) &&
                                        !groupRow.Table.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_TAXLOTSTATE))
                                    {
                                        s.Append(groupRow[i].ToString()).Append(delimiterSymbol);
                                    }
                                }
                            }
                            s.Remove(s.Length - 1, 1);
                            sw.WriteLine(s.ToString());
                            s = new StringBuilder();

                            // check row header requires, if true then write
                            if (string.IsNullOrEmpty(rowHeaderReq) || rowHeaderReq.ToUpper().Equals("TRUE"))
                            {
                                //write header for allocation 
                                int allHeadcolCount = allocationHeadingColl.Count;
                                for (int i = 0; i < allHeadcolCount; i++)
                                {
                                    s.Append(allocationHeadingColl[i]).Append(delimiterSymbol);
                                }
                                s.Remove(s.Length - 1, 1);
                                sw.WriteLine(s.ToString());
                                s = new StringBuilder();
                            }
                            //allocationList
                            foreach (DataRow dtRow in allocationList)
                            {
                                bool alloends = false;
                                int allColCount = dtRow.Table.Columns.Count;
                                for (int i = 0; i < allColCount; i++)
                                {
                                    if (dtRow.Table.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_GROUPENDS))
                                    {
                                        alloends = true;
                                    }
                                    if (!dtRow.Table.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_GROUPENDS) && alloends.Equals(false))
                                    {

                                    }
                                    else
                                    {
                                        if (!ColumnExists(dtRow.Table.Columns[i].ColumnName) &&
                                            !dtRow.Table.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_TAXLOTSTATE))
                                        {
                                            s.Append(dtRow[i].ToString()).Append(delimiterSymbol);
                                        }
                                    }
                                }

                                s.Remove(s.Length - 1, 1);
                                //NewLine does the job, and writing is done only once
                                s.Append(Environment.NewLine);
                            }
                            sw.Write(s.ToString());
                            s = new StringBuilder();
                        }
                    }
                }
                #endregion group  and allocation generation code

                #region Footer Code


                string fileFooterReq = string.Empty;

                bool blnFileFooterContains = dsGrid.Tables[0].Columns.Contains(HEADCOL_FILEFOOTER);
                if (blnFileFooterContains)
                {
                    fileFooterReq = dsGrid.Tables[0].Rows[0][HEADCOL_FILEFOOTER].ToString();
                }
                if (fileFooterReq.ToUpper().Equals("TRUE"))
                {
                    if (string.IsNullOrEmpty(thirdPartyFileFormat.FooterFile))
                    {
                        OnMessage(this, new MessageEventArgs("No Footer XSLT name is available for the selected ThirdParty. Generating file without Footer."));
                    }
                    else
                    {
                        string footerXsltPath = thirdPartyFileFormat.FooterFile;
                        // get the XSLT Name
                        string footerXsltName = footerXsltPath.Substring(footerXsltPath.LastIndexOf("\\") + 1);
                        // complete the path from StartUp Path
                        string xsltForFooterStartUpPath = String.Format(@"{0}\{1}\{2}\{3}", StartupPath, ApplicationConstants.MAPPING_FILE_DIRECTORY, ApplicationConstants.MappingFileType.ThirdPartyXSLT, footerXsltName);
                        ThirdPartyFlatFileFooter thirdPartyFlatFileFooter = new ThirdPartyFlatFileFooter();
                        thirdPartyFlatFileFooter.RecordCount = recordCount;
                        thirdPartyFlatFileFooter.TotalQty = totalQtytoSend;
                        thirdPartyFlatFileFooter.Date = Ext.ShortDateStr(batch.Format.Date);
                        thirdPartyFlatFileFooter.DateAndTime = dateFormat;
                        thirdPartyFlatFileFooter.InternalNetNotional = internalNetNotionaltoSend;
                        thirdPartyFlatFileFooter.InternalGrossAmount = internalGrossAmounttoSend;
                        string serializeXMLforFooter = XMLUtilities.SerializeToXML(thirdPartyFlatFileFooter);
                        string convertedXMLforFooter = StartupPath + @"\ConvertedThirdPartyXMLforFooter.xml";
                        StringReader sreader = new StringReader(serializeXMLforFooter);
                        XmlTextReader xmlreader = new XmlTextReader(sreader);
                        string mappedfilePathforFooter = XMLUtilities.GetTransformed(xmlreader, convertedXMLforFooter, xsltForFooterStartUpPath);

                        if (!mappedfilePathforFooter.Equals(""))
                        {
                            DataSet dsFooter = new DataSet();
                            dsFooter.ReadXml(mappedfilePathforFooter);
                            if (dsFooter.Tables.Count <= 0)
                            {
                                OnMessage(this, new MessageEventArgs("No data available for Footer"));
                                sw.Close();
                                return;
                            }
                            DataTable dtFooter = dsFooter.Tables[0];

                            string rowHeaderReq = string.Empty;
                            bool blnRowHeaderContains = dtFooter.Columns.Contains(HEADCOL_ROWHEADER);
                            if (blnRowHeaderContains)
                            {
                                rowHeaderReq = dtFooter.Rows[0][HEADCOL_ROWHEADER].ToString();
                            }

                            int iColCountFooter = dtFooter.Columns.Count;
                            // check column header requires, if true the write
                            if (string.IsNullOrEmpty(rowHeaderReq) || rowHeaderReq.ToUpper().Equals("TRUE"))
                            {
                                // First we will write the headers.    
                                for (int i = 0; i < iColCountFooter; i++)
                                {
                                    if (!ColumnExists(dtFooter.Columns[i].ColumnName) &&
                                        !dtFooter.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_TAXLOTSTATE))
                                    {
                                        s.Append(dtFooter.Columns[i]).Append(delimiterSymbol);
                                    }
                                }

                                s.Remove(s.Length - 1, 1);
                                sw.WriteLine(s.ToString());
                                s = new StringBuilder();
                            }
                            // Now write all the rows.
                            foreach (DataRow dr in dtFooter.Rows)
                            {
                                for (int i = 0; i < iColCountFooter; i++)
                                {
                                    if (!ColumnExists(dtFooter.Columns[i].ColumnName) &&
                                        !dtFooter.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_TAXLOTSTATE))
                                    {
                                        if (!Convert.IsDBNull(dr[i]))
                                        {
                                            s.Append(dr[i].ToString()).Append(delimiterSymbol);
                                        }
                                    }
                                }
                                s.Remove(s.Length - 1, 1);
                            }
                            sw.Write(s.ToString());
                            s = new StringBuilder();
                        }
                    }
                }
                #endregion Footer Code

                if (sw != null)
                {
                    sw.Flush();
                    sw.Close();

                    if (!thirdPartyFileFormat.DoNotShowFileOpenDialogue)
                    {
                        PromptEventArgs e = new PromptEventArgs();
                        e.Formatter = batch.Format;
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
        }

        /// <summary>
        /// This method executes when needed to generate a XML format file
        /// </summary>
        /// <param name="batch"></param>
        public void GenerateUserDefindFormatForXML(ThirdPartyBatch batch)
        {
            try
            {
                string filePath = batch.Format.FilePath;
                DateTime dateHeader = batch.Format.Date;
                ThirdPartyFileFormat thirdPartyFileFormat = batch.Format.Formatter;
                string dateFormat = dateHeader.ToString("MM/dd/yyyy:HHmmss");

                var dataSet = new DataSet();
                if (!string.IsNullOrEmpty(batch.SerializedDataSet))
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(batch.SerializedDataSet);

                    DataTable dataTable = new DataTable();
                    var listXMLNode = xmlDoc.SelectNodes("//Group");
                    if (listXMLNode != null && listXMLNode.Count > 0)
                    {
                        foreach (XmlNode groupNode in xmlDoc.SelectNodes("//Group"))
                        {
                            DataRow row = dataTable.NewRow();

                          foreach (XmlAttribute attribute in groupNode.Attributes)
                            {
                                if (!dataTable.Columns.Contains(attribute.Name.Replace("_x0020_", " ")))
                                {
                                    dataTable.Columns.Add(attribute.Name.Replace("_x0020_", " "));
                                }
                                row[attribute.Name.Replace("_x0020_", " ")] = attribute.Value;
                            }

                            foreach (XmlNode childNode in groupNode.ChildNodes)
                            {
                                if (!childNode.Name.Equals("ThirdPartyFlatFileDetail"))
                                {
                                    if (childNode.NodeType == XmlNodeType.Element && !dataTable.Columns.Contains(childNode.Name.Replace("_x0020_", " ")))
                                    {
                                        dataTable.Columns.Add(childNode.Name.Replace("_x0020_", " "), typeof(string));
                                    }
                                    row[childNode.Name.Replace("_x0020_", " ")] = childNode.InnerText;
                                }
                            }
                            dataTable.Rows.Add(row);

                            foreach (XmlNode detailNode in groupNode.SelectNodes("ThirdPartyFlatFileDetail"))
                            {
                                DataRow detailRow = dataTable.NewRow();

                                foreach (XmlAttribute attribute in detailNode.Attributes)
                                {
                                    if (!dataTable.Columns.Contains(attribute.Name.Replace("_x0020_", " ")))
                                    {
                                        dataTable.Columns.Add(attribute.Name.Replace("_x0020_", " "));
                                    }
                                    detailRow[attribute.Name.Replace("_x0020_", " ")] = attribute.Value;
                                }
                                foreach (XmlNode childNode in detailNode.ChildNodes)
                                {
                                    if (childNode.NodeType == XmlNodeType.Element && !dataTable.Columns.Contains(childNode.Name.Replace("_x0020_", " ")))
                                    {
                                        dataTable.Columns.Add(childNode.Name.Replace("_x0020_", " "), typeof(string));
                                    }
                                    detailRow[childNode.Name.Replace("_x0020_", " ")] = childNode.InnerText;
                                }
                                dataTable.Rows.Add(detailRow);
                            }
                        }
                    }
                    else
                    {
                        foreach (XmlNode detailNode in xmlDoc.SelectNodes("//ThirdPartyFlatFileDetail"))
                        {
                            DataRow detailRow = dataTable.NewRow();

                            foreach (XmlAttribute attribute in detailNode.Attributes)
                            {
                                if (!dataTable.Columns.Contains(attribute.Name.Replace("_x0020_", " ")))
                                {
                                    // Add DataColumn to DataTable with the name of the XML attribute
                                    dataTable.Columns.Add(attribute.Name.Replace("_x0020_", " "));
                                }

                                // Set the value of the corresponding column in the DataRow
                                detailRow[attribute.Name.Replace("_x0020_", " ")] = attribute.Value;
                            }

                            foreach (XmlNode childNode in detailNode.ChildNodes)
                            {
                                if (childNode.NodeType == XmlNodeType.Element && !dataTable.Columns.Contains(childNode.Name.Replace("_x0020_", " ")))
                                {
                                    dataTable.Columns.Add(childNode.Name.Replace("_x0020_", " "), typeof(string));
                                }

                                detailRow[childNode.Name.Replace("_x0020_", " ")] = childNode.InnerText;
                            }
                            dataTable.Rows.Add(detailRow);
                        }
                    }
                    dataSet.Tables.Add(dataTable);
                }

                if (dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
                {
                    if (batch.TransmissionType.Equals(((int)TransmissionType.File).ToString()))
                    {
                        OnMessage(this, new MessageEventArgs("No Data Found"));
                    }
                    return;
                }

                if (Directory.Exists(Path.GetDirectoryName(filePath)) == false)
                {
                    OnMessage(this, new MessageEventArgs(filePath, "Directory does not exist"));
                    return;
                }

                DataSet ds = new DataSet();
                string xmlString = null;
                int recordCount = 0;

                DataSet dsHeader = new DataSet();
                DataSet dsFooter = new DataSet();
                DataSet ds_Updated = new DataSet();

                DataSet dsGrid = dataSet;
                if (dsGrid == null)
                {
                    return;
                }

                #region Footer File Fields Calculations
                recordCount = dsGrid.Tables[0].Rows.Count;
                bool blntotalQtyFieldContains = dsGrid.Tables[0].Columns.Contains(HEADCOL_ALLOCQTY);

                bool blnnetNotionalFieldContains = dsGrid.Tables[0].Columns.Contains(HEADCOL_INTERNALNETNOTIONAL);
                bool blngrossAmountFieldContains = dsGrid.Tables[0].Columns.Contains(HEADCOL_INTERNALGROSSAMOUNT);

                double totalQtytoSend = 0.0;

                double internalNetNotionaltoSend = 0.0;
                double internalGrossAmounttoSend = 0.0;

                if (blntotalQtyFieldContains)
                {
                    foreach (DataRow row in dsGrid.Tables[0].Rows)
                    {
                        totalQtytoSend = totalQtytoSend + Convert.ToDouble(row[HEADCOL_ALLOCQTY]);
                    }
                }



                if (blnnetNotionalFieldContains)
                {
                    foreach (DataRow row in dsGrid.Tables[0].Rows)
                    {
                        internalNetNotionaltoSend = internalNetNotionaltoSend + Convert.ToDouble(row[HEADCOL_INTERNALNETNOTIONAL]);
                    }
                }

                if (blngrossAmountFieldContains)
                {
                    foreach (DataRow row in dsGrid.Tables[0].Rows)
                    {
                        internalGrossAmounttoSend = internalGrossAmounttoSend + Convert.ToDouble(row[HEADCOL_INTERNALGROSSAMOUNT]);
                    }
                }

                #endregion Footer File Fields Calculations

                StreamWriter sw = new StreamWriter(filePath);

                #region Header Code
                string fileHeaderReq = string.Empty;
                bool blnFileHeaderContains = dsGrid.Tables[0].Columns.Contains(HEADCOL_FILEHEADER);
                if (blnFileHeaderContains)
                {
                    fileHeaderReq = dsGrid.Tables[0].Rows[0][HEADCOL_FILEHEADER].ToString();
                }
                if (fileHeaderReq.ToUpper().Equals("TRUE"))
                {
                    if (Ext.IsNull(thirdPartyFileFormat.HeaderFile))
                    {
                        OnMessage(this, new MessageEventArgs("No Header XSLT file name is available for the selected ThirdParty. Processing without header."));
                    }
                    else
                    {
                        dsHeader = HeaderDataSet(batch.Format, dateFormat, recordCount, thirdPartyFileFormat);
                        if (dsHeader.Tables.Count <= 0)
                        {
                            sw.Close();
                            OnMessage(this, new MessageEventArgs("No data available for Header"));
                            return;
                        }
                        else
                        {
                            string xmlHeaderMainTag = string.Empty;
                            bool blnHeaderMainTag = dsHeader.Tables[0].Columns.Contains(HEADCOL_XMLHEADERMAINTAG);
                            if (blnHeaderMainTag)
                            {
                                xmlHeaderMainTag = dsHeader.Tables[0].Rows[0][HEADCOL_XMLHEADERMAINTAG].ToString();
                            }

                            string xmlHeaderChildTag = string.Empty;
                            bool blnHeaderChildTag = dsHeader.Tables[0].Columns.Contains(HEADCOL_XMLHEADERCHILDTAG);
                            if (blnHeaderChildTag)
                            {
                                xmlHeaderChildTag = dsHeader.Tables[0].Rows[0][HEADCOL_XMLHEADERCHILDTAG].ToString();
                            }

                            dsHeader = RemoveColumn(dsHeader);
                            dsHeader = RenameDataSetAndTable(xmlHeaderMainTag, xmlHeaderChildTag, dsHeader);

                            //ds = thirdPartyReportControl.RemoveColumn(dsHeader.Copy());
                            //ds = thirdPartyReportControl.RenameDataSetAndTable(xmlHeaderMainTag, xmlHeaderChildTag, ds);                                
                            //xmlString = thirdPartyReportControl.ConvertDataSetIntoXML(ds);
                            //sw.WriteLine(xmlString);
                            //xmlString = null;
                            //ds = null;
                        }
                    }
                }

                #endregion Header Code

                #region Footer Code

                string fileFooterReq = string.Empty;
                bool blnFileFooterContains = dsGrid.Tables[0].Columns.Contains(HEADCOL_FILEFOOTER);
                if (blnFileFooterContains)
                {
                    fileFooterReq = dsGrid.Tables[0].Rows[0][HEADCOL_FILEFOOTER].ToString();
                }
                if (fileFooterReq.ToUpper().Equals("TRUE"))
                {
                    if (string.IsNullOrEmpty(thirdPartyFileFormat.FooterFile))
                    {
                        OnMessage(this, new MessageEventArgs("No Footer XSLT name is available for the selected ThirdParty. Generating file without Footer."));
                    }
                    else
                    {
                        dsFooter = FileFooterDataSet(batch.Format, dateFormat, recordCount, totalQtytoSend, thirdPartyFileFormat, internalNetNotionaltoSend, internalGrossAmounttoSend);
                        if (dsFooter.Tables.Count <= 0)
                        {
                            OnMessage(this, new MessageEventArgs("No data available for Footer"));
                            sw.Close();
                            return;
                        }
                        else
                        {
                            string xmlFooterMainTag = string.Empty;
                            bool blnFooterMainTag = dsFooter.Tables[0].Columns.Contains(HEADCOL_XMLFOOTERMAINTAG);
                            if (blnFooterMainTag)
                            {
                                xmlFooterMainTag = dsFooter.Tables[0].Rows[0][HEADCOL_XMLFOOTERMAINTAG].ToString();
                            }

                            string xmlFooterChildTag = string.Empty;
                            bool blnFooterChildTag = dsFooter.Tables[0].Columns.Contains(HEADCOL_XMLFOOTERMAINTAG);
                            if (blnFooterChildTag)
                            {
                                xmlFooterChildTag = dsFooter.Tables[0].Rows[0][HEADCOL_XMLFOOTERCHILDTAG].ToString();
                            }

                            dsFooter = RemoveColumn(dsFooter);
                            dsFooter = RenameDataSetAndTable(xmlFooterMainTag, xmlFooterChildTag, dsFooter);

                            //ds = thirdPartyReportControl.RemoveColumn(dsFooter.Copy());
                            //ds = thirdPartyReportControl.RenameDataSetAndTable(xmlFooterMainTag, xmlFooterChildTag, ds);
                            //xmlString = thirdPartyReportControl.ConvertDataSetIntoXML(ds);
                            //sw.Write(xmlString);
                            //xmlString = null;
                            //ds = null;
                        }
                    }
                }

                #endregion Footer Code

                #region general XML file generation code
                string xmlMainTag = string.Empty;
                bool blnMainTag = dsGrid.Tables[0].Columns.Contains(HEADCOL_XMLMAINTAG);
                if (blnMainTag)
                {
                    xmlMainTag = dsGrid.Tables[0].Rows[0][HEADCOL_XMLMAINTAG].ToString();
                }

                string xmlChildTag = string.Empty;
                bool blnChildTag = dsGrid.Tables[0].Columns.Contains(HEADCOL_XMLCHILDTAG);
                if (blnChildTag)
                {
                    xmlChildTag = dsGrid.Tables[0].Rows[0][HEADCOL_XMLCHILDTAG].ToString();
                }
                ds = RemoveColumn(dsGrid.Copy());
                ds = RenameDataSetAndTable(xmlMainTag, xmlChildTag, ds);

                if (dsHeader.Tables.Count > 0)
                {
                    ds_Updated.Tables.Add(dsHeader.Tables[0].Copy());
                }
                if (dsGrid.Tables.Count > 0)
                {
                    ds_Updated.Tables.Add(ds.Tables[0].Copy());
                }
                if (dsFooter.Tables.Count > 0)
                {
                    ds_Updated.Tables.Add(dsFooter.Tables[0].Copy());
                }

                ds_Updated = RenameDataSetAndTable(xmlMainTag, "", ds_Updated);

                xmlString = ConvertDataSetIntoXML(ds_Updated);
                sw.WriteLine(xmlString);

                xmlString = null;
                ds = null;
                dsHeader = null;
                dsFooter = null;
                ds_Updated = null;

                #endregion general XML file generation code

                if (sw != null)
                {
                    sw.Flush();
                    sw.Close();
                    if (!thirdPartyFileFormat.DoNotShowFileOpenDialogue)
                    {
                        PromptEventArgs e = new PromptEventArgs();
                        e.Formatter = batch.Format;
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
        }

        /// <summary>
        /// Convert Data Set into XML String
        /// </summary>
        /// <param name="ds">Data Set </param>
        /// <returns>xml String</returns>
        public string ConvertDataSetIntoXML(DataSet ds)
        {
            string xmlString = null;
            try
            {
                StringBuilder writer = new StringBuilder();
                ThirdPartyStringWriterWithEncoding stringWriter = new ThirdPartyStringWriterWithEncoding(writer, Encoding.UTF8);

                XmlWriterSettings settings = new XmlWriterSettings
                {
                    Encoding = Encoding.UTF8,
                    ConformanceLevel = ConformanceLevel.Document,
                    OmitXmlDeclaration = false,
                    CloseOutput = true,
                    Indent = true,
                    IndentChars = "  ",
                    NewLineHandling = NewLineHandling.Replace
                };

                XmlWriter xmlWriter = XmlWriter.Create(stringWriter, settings);

                ds.WriteXml(xmlWriter);
                xmlWriter.Close();
                xmlString = writer.ToString();
                writer = null;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            return xmlString;
        }

        /// <summary>
        /// Rename Data Set name and Table name
        /// </summary>
        /// <param name="xmlMainTag">Data Set name to rename the dataSet </param>
        /// <param name="xmlChildTag">DataTable name to rename the datatable</param>
        /// <param name="ds">Data Set</param>
        /// <returns>DataSet after rename with its table table[0]</returns>

        public DataSet RenameDataSetAndTable(string xmlMainTag, string xmlChildTag, DataSet ds)
        {
            try
            {
                if (!String.IsNullOrEmpty(xmlMainTag))
                    ds.DataSetName = xmlMainTag;
                if (!String.IsNullOrEmpty(xmlChildTag))
                    ds.Tables[0].TableName = xmlChildTag;
                return ds;
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
        /// Rmove columns and data which are not required into ouput third party data
        /// </summary>
        /// <param name="ds">complate converted Data Set from xslt </param>
        /// <returns>Data Set after remove not required columns</returns>
        public DataSet RemoveColumn(DataSet ds)
        {
            DataTable dt = ds.Tables[0];
            try
            {
                int iColCount = dt.Columns.Count;
                for (int i = 0; i < iColCount; i++)
                {
                    if (ColumnExists(dt.Columns[i].ColumnName) ||
                     dt.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_TAXLOTSTATE) ||
                     dt.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_XMLMAINTAG) ||
                     dt.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_XMLCHILDTAG) ||
                     dt.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_XMLFOOTERMAINTAG) ||
                     dt.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_XMLFOOTERCHILDTAG) ||
                     dt.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_XMLHEADERMAINTAG) ||
                     dt.Columns[i].ColumnName.ToUpper().Equals(HEADCOL_XMLHEADERCHILDTAG))
                    {
                        dt.Columns.RemoveAt(i);
                        iColCount--;
                        i--;
                    }
                    dt.AcceptChanges();
                }
                return ds;
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
        /// Convert file footer Data into Data Set
        /// </summary>
        /// <param name="executor">Object of ThirdPartyUserDefinedFormat </param>
        /// <param name="dateFormat">Date format to show in file footer</param>
        /// <param name="recordCount">Total record count </param>
        /// <param name="totalQtytoSend">Internal  totol qty to show in File footer</param>
        /// <param name="thirdPartyFileFormat">Third party file footer</param>
        /// <returns>Data Set for file footer</returns>
        private DataSet FileFooterDataSet(ThirdPartyUserDefinedFormat executor, string dateFormat, int recordCount, double totalQtytoSend, ThirdPartyFileFormat thirdPartyFileFormat, double internalNetNotionaltoSend, double internalGrossAmounttoSend)
        {
            DataSet dsFooter = new DataSet();
            string footerXsltPath = thirdPartyFileFormat.FooterFile;
            string footerXsltName = footerXsltPath.Substring(footerXsltPath.LastIndexOf("\\") + 1);
            string xsltForFooterStartUpPath = String.Format(@"{0}\{1}\{2}\{3}", StartupPath, ApplicationConstants.MAPPING_FILE_DIRECTORY, ApplicationConstants.MappingFileType.ThirdPartyXSLT, footerXsltName);
            ThirdPartyFlatFileFooter thirdPartyFlatFileFooter = new ThirdPartyFlatFileFooter();
            thirdPartyFlatFileFooter.RecordCount = recordCount;
            thirdPartyFlatFileFooter.TotalQty = totalQtytoSend;
            thirdPartyFlatFileFooter.Date = Ext.ShortDateStr(executor.Date);
            thirdPartyFlatFileFooter.DateAndTime = dateFormat;
            thirdPartyFlatFileFooter.InternalNetNotional = internalNetNotionaltoSend;
            thirdPartyFlatFileFooter.InternalGrossAmount = internalGrossAmounttoSend;
            string serializeXMLforFooter = XMLUtilities.SerializeToXML(thirdPartyFlatFileFooter);
            string convertedXMLforFooter = StartupPath + @"\ConvertedThirdPartyXMLforFooter.xml";
            StringReader sreader = new StringReader(serializeXMLforFooter);
            XmlTextReader xmlreader = new XmlTextReader(sreader);
            string mappedfilePathforFooter = XMLUtilities.GetTransformed(xmlreader, convertedXMLforFooter, xsltForFooterStartUpPath);
            if (!mappedfilePathforFooter.Equals(""))
            {
                dsFooter.ReadXml(mappedfilePathforFooter);
            }
            return dsFooter;
        }

        /// <summary>
        /// Convert file Header Data into Data Set
        /// </summary>
        /// <param name="dateFormat">Date format to show in file footer</param>
        /// <param name="recordCount">Total record count </param>
        /// <param name="thirdPartyFileFormat">Third Party file format</param>
        /// <returns>DataSet of file Header</returns>          

        private DataSet HeaderDataSet(ThirdPartyUserDefinedFormat executor, string dateFormat, int recordCount, ThirdPartyFileFormat thirdPartyFileFormat)
        {
            DataSet dsHeader = new DataSet();
            try
            {
                string headerXsltPath = thirdPartyFileFormat.HeaderFile;
                string headerXsltName = headerXsltPath.Substring(headerXsltPath.LastIndexOf("\\") + 1);
                string xsltForHeaderStartUpPath = String.Format(@"{0}\{1}\{2}\{3}", StartupPath, ApplicationConstants.MAPPING_FILE_DIRECTORY, ApplicationConstants.MappingFileType.ThirdPartyXSLT, headerXsltName);
                ThirdPartyFlatFileHeader thirdPartyFlatFileHeader = new ThirdPartyFlatFileHeader();
                thirdPartyFlatFileHeader.Date = Ext.ShortDateStr(executor.Date);
                thirdPartyFlatFileHeader.DateAndTime = dateFormat;
                thirdPartyFlatFileHeader.RecordCount = recordCount;
                string serializeXMLforHeader = XMLUtilities.SerializeToXML(thirdPartyFlatFileHeader);
                string convertedXMLforHeader = StartupPath + @"\ConvertedThirdPartyXMLforHeader.xml";
                StringReader sr = new StringReader(serializeXMLforHeader);
                XmlTextReader xreader = new XmlTextReader(sr);
                string mappedfilePathforHeader = XMLUtilities.GetTransformed(xreader, convertedXMLforHeader, xsltForHeaderStartUpPath);
                if (!mappedfilePathforHeader.Equals(""))
                {
                    dsHeader.ReadXml(mappedfilePathforHeader);
                }
                return dsHeader;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            return dsHeader;
        }

        /// <summary>
        /// Columns the exists.
        /// </summary>
        /// <param name="colName">Name of the col.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private bool ColumnExists(string colName)
        {
            // string[] columns = { "CompanyID", "ThirdPartyID", "CompanyAccountID" };

            if (colName.Equals("CompanyID") || colName.Equals("ThirdPartyID") || colName.Equals("CompanyAccountID") ||
                colName.Equals("AssetID") || colName.Equals("UnderLyingID") || colName.Equals("CurrencyID") ||
                colName.Equals("ExchangeID") || colName.Equals("AUECID") || colName.Equals("CompanyAccountTypeID") ||
                colName.Equals("CommissionRateTypeID") || colName.Equals("ThirdPartyTypeId") || colName.Equals("CompanyCVID") ||
                colName.Equals("VenueID") || colName.Equals("EntityID") || colName.Equals("CounterPartyID") ||
                colName.Equals("TradAccntID") || colName.Equals("GroupEnds") || colName.Equals(HEADCOL_GroupAllocationReq) ||
                colName.Equals(HEADCOL_FILEHEADER) || colName.Equals(HEADCOL_FILEFOOTER) || colName.Equals(HEADCOL_PBUNIQUEID) ||
                colName.Equals(HEADCOL_ROWHEADER) || colName.Equals("TaxLotStateID") || colName.ToUpper().Equals(HEADCOL_ALLOCQTY) ||
                colName.Equals("TaxLots_Id") || colName.Equals("Group_Id") || colName.Equals("TaxLots_ThirdPartyFlatFileDetail") ||
                colName.Equals("TaxLotState1") || colName.Equals("IsCaptionChangeRequired") || colName.Equals("FromDeleted") ||
                colName.ToUpper().Equals("XMLMAINTAG") || colName.ToUpper().Equals("XMLCHILDTAG"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <remarks></remarks>
        #region IDisposable Members
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {

                }
            }
            catch (Exception)
            {
                return;
            }
        }
        #endregion
    }
}
