using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;
using Prana.BusinessObjects;
using System.Collections;
using System.Data;
using System.IO;
using Prana.Utilities.XMLUtilities;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.ImportExportUtilities;

namespace Prana.AutomationHandlers
{
    public class FileMapperComponent
    {
        #region Global Variables Section        

        static Dictionary<string, List<string>> dicClientCounterParties;

        #endregion

        static  FileMapperComponent()
        {
            if (dicClientCounterParties == null)
                dicClientCounterParties = AutomationHandlerDataManager.getClientThirdPartyDictionary();
        }

        #region File Components

        //To Create Date Directories In Specified Third Party's Dir According to specified clientSetting objects date
        public static void CreateStructure(ClientSettings objClientSettings, List<string> lsThirdPary)
        {
            try
            {                   
                string clientPath = RiskPathCreator.GetClientPath(objClientSettings);  

                string[] InOutDirectoriesList = Enum.GetNames(typeof(AutomationEnum.InOutDirectories));
                foreach (string thirdParty in lsThirdPary)
                {
                    //ThirdPartyPath With Date
                    string DatePath = RiskPathCreator.GetDatePath(objClientSettings,thirdParty);                    
                    foreach (string parameter in InOutDirectoriesList)
                        Directory.CreateDirectory(Path.Combine(DatePath, parameter));
                }

                //RiskReport Directory Is Created In Client Directory As It Doesn't belongs to any ThirdParty
                //riskReportPath =Path.Combine(Path.Combine(clientPath, ExportTypeEnum.RiskReport.ToString()),dateStringOfSpecificClient);
                string riskReportPath = RiskPathCreator.GetRiskReportPath(objClientSettings);
                Directory.CreateDirectory(riskReportPath);

                //Console.Write("Structure Created Successfully............");
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        //To Create Directories For All The Third Parties Of All the Client         
        private static void CreateFixFileStructure(ClientSettings objClientSettings)
        {
            try
            {
                //in dicClientCounterParties all the third parties of a client
                
                string thirdPartyPath, importTypePath, fileTypePath;
                string[] lsImortTypes = Enum.GetNames(typeof(AutomationEnum.ImportTypeEnum));
                string[] lsFileTypes = Enum.GetNames(typeof(AutomationEnum.FileTypeEnum));
                                
                
                foreach (string _clientName in dicClientCounterParties.Keys)
                {
                    objClientSettings.ClientName = _clientName;
                    string clientPath = RiskPathCreator.GetClientPath(objClientSettings);
                    List<string> lsThirdPartyNames = dicClientCounterParties[_clientName];
                    foreach (string thirdPartyName in lsThirdPartyNames)
                    {
                        thirdPartyPath = Path.Combine(clientPath, thirdPartyName);
                        foreach (string importType in lsImortTypes)
                        {
                            importTypePath = Path.Combine(thirdPartyPath, importType);
                            Directory.CreateDirectory(importTypePath);

                            foreach (string fileType in lsFileTypes)
                            {
                                fileTypePath = Path.Combine(importTypePath, fileType);
                                Directory.CreateDirectory(fileTypePath);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }

        }

        public static FileSetting GetFilePath(ClientSettings objClientSettings,string thirdPartyName)
        {
            FileSetting objFileSetting = new FileSetting();
            try
            {   
                string clientPath = RiskPathCreator.GetClientPath(objClientSettings);

                string fixPathTillThirdParty = RiskPathCreator.GetThirdPartyPath(objClientSettings, thirdPartyName);

                if (fixPathTillThirdParty == null)
                    throw new Exception("Directory Structure Not Created !");

                string fixPathTillImportType = Path.Combine(fixPathTillThirdParty, objClientSettings.ImportType.ToString());
                objFileSetting.XSDPath = Path.Combine(fixPathTillImportType, AutomationEnum.FileTypeEnum.XSD.ToString());
                objFileSetting.XsltPath = Path.Combine(fixPathTillImportType, AutomationEnum.FileTypeEnum.XSLT.ToString());

                string DatePath = RiskPathCreator.GetDatePath(objClientSettings, thirdPartyName);

                string FileNameToImport = objClientSettings.ImportType.ToString() +"."+ objClientSettings.FileFormatter;

                objFileSetting.InputFilePath = Path.Combine(Path.Combine(DatePath, AutomationEnum.InOutDirectories.Input.ToString()),FileNameToImport);

                objFileSetting.TempXmlPath = Path.Combine(DatePath, AutomationEnum.InOutDirectories.TempXML.ToString());

                if (objClientSettings.ReportType == AutomationEnum.ReprotTypeEnum.RiskReport)
                    objFileSetting.OutPutFilePath = RiskPathCreator.GetRiskReportPath(objClientSettings);
                else
                    objFileSetting.OutPutFilePath = Path.Combine(DatePath, AutomationEnum.InOutDirectories.Output.ToString());
                  
                
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return objFileSetting;
        }

        public static void CreateDateStructure(BaseSettings objBaseSettings,DateTime date)
        {
            try
            {
                ClientSettings objClientSettings = new ClientSettings();
                objClientSettings.BaseSettings = objBaseSettings;
                objClientSettings.Date = date;
                CreateFixFileStructure(objClientSettings);
                string[] InOutDirectoriesList = Enum.GetNames(typeof(AutomationEnum.InOutDirectories));
                foreach (string _clientName in dicClientCounterParties.Keys)
                {
                    objClientSettings.ClientName = _clientName;                    
                    List<string> lsThirdPartyNames = dicClientCounterParties[objClientSettings.ClientName];                   
                    foreach (string thirdPartyName in lsThirdPartyNames)
                    {
                        string DatePath = RiskPathCreator.GetDatePath(objClientSettings, thirdPartyName);
                        foreach (string parameter in InOutDirectoriesList)
                            Directory.CreateDirectory(Path.Combine(DatePath, parameter));
                    }
                }
                string riskReportPath = RiskPathCreator.GetRiskReportPath(objClientSettings);
                Directory.CreateDirectory(riskReportPath);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }

        }


        #endregion

        public static IList GetTransformedData(Type ImportType, FileSetting files)
        {
            IList toReturn = new List<object>();
            try
            {
                //FileSetting files = GetFilePath(objClientSettings);
                string inputFile = files.InputFilePath;
                if (!File.Exists(inputFile))
                    throw new Exception("Input File Doesn't Exist at:-- " + files.InputFilePath);

                
                DataTable dataSource = GetDataTableFromDifferentFileFormats(inputFile);
                if (dataSource != null)
                {
                    dataSource = ArrangeTable(dataSource);

                    // now generate the xml of table dataSource
                    string inputXmlPath = files.TempXmlPath + @"\inputXml.xml";
                    dataSource.WriteXml(inputXmlPath);
                    // get a new mapped xml
                    string outPutXmlPath = files.TempXmlPath + @"\OutPutXML.xml";


                    // get the XSLT name only
                    if (Directory.GetFiles(files.XsltPath).Length < 1)
                        throw new Exception("XSLT Doesn't Exist at:-- " + files.XsltPath);

                    string strXSLTName = Directory.GetFiles(files.XsltPath)[0];

                    //                                                  serializedXML,MappedserXML, XSLTPath             

                    #region XSD Region :--- Not in Use At Present

                    //if (Directory.GetFiles(files.XSDPath).Length < 0)
                    //    throw new Exception("XSD Does'nt Exist at:-- " + files.XSDPath);

                    //string strXSDName = Directory.GetFiles(files.XSDPath)[0];

                    //string xsdPath = files.XSDPath + "\\" + strXSDName;

                    #endregion

                    XMLUtilities.Transform(inputXmlPath, strXSLTName, outPutXmlPath);

                    string getMappedXML = string.Empty;
                    DataSet ds = new DataSet();
                    if (!outPutXmlPath.Equals(""))
                    {

                        ds.ReadXml(outPutXmlPath);
                        // Now we have arranged and updated XML
                        // as above we inserted "*" in the blank columns, but "*" needs extra treatment, so
                        // again we replace the "*" with blank string, the following looping does the same
                        for (int irow = 0; irow < ds.Tables[0].Rows.Count; irow++)
                        {
                            for (int icol = 0; icol < ds.Tables[0].Columns.Count; icol++)
                            {
                                string val = ds.Tables[0].Rows[irow].ItemArray[icol].ToString();
                                if (val.Equals("*"))
                                {
                                    ds.Tables[0].Rows[irow][icol] = string.Empty;
                                }
                            }
                        }
                    }
                    if (ds.Tables.Count > 0)
                        toReturn = GeneralUtilities.CreateCollectionFromDataTable(ds.Tables[0], ImportType);
                }


                //
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }

            return toReturn;

            ////
        }

        private static DataTable GetDataTableFromDifferentFileFormats(string fileName)
        {
            DataTable datasourceData = null;
            try
            {
                string fileFormat = fileName.Substring(fileName.LastIndexOf(".") + 1);

                switch (fileFormat.ToUpperInvariant())
                {
                    case "CSV":
                        datasourceData = FileReaderFactory.Create(DataSourceFileFormat.Csv).GetDataTableFromUploadedDataFile(fileName);
                        break;
                    case "XLS":
                        datasourceData = FileReaderFactory.Create(DataSourceFileFormat.Excel).GetDataTableFromUploadedDataFile(fileName);
                        break;
                    default:
                        datasourceData = FileReaderFactory.Create(DataSourceFileFormat.Default).GetDataTableFromUploadedDataFile(fileName);
                        break;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return datasourceData;
        }

        private static DataTable ArrangeTable(DataTable dataSource)
        {
            try
            {
                // what XML we will generate, all the tagname will be like COL1,COL2 .
                //string columnName = "COL";
                //for (int j = 0; j < dataSource.Columns.Count; j++)
                //{
                //    dataSource.Columns[j].ColumnName = columnName + (j + 1);
                //}
                dataSource.TableName = "PositionMaster";

                // update the Table columns value with "*" where columns value blank in the excel sheet
                // when we generate the XML for that table, the blank coluns do not comes in the generated XML
                // the indexing of the generated XML changed because of blank columns
                // so defalut value of the columns will be  "*"
                for (int irow = 0; irow < dataSource.Rows.Count; irow++)
                {
                    for (int icol = 0; icol < dataSource.Columns.Count; icol++)
                    {
                        string val = dataSource.Rows[irow].ItemArray[icol].ToString();
                        if (String.IsNullOrEmpty(val.Trim()))
                        {
                            dataSource.Rows[irow][icol] = "*";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return dataSource;
        }
    }
}
