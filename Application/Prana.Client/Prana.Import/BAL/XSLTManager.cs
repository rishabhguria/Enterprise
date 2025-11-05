using Prana.BusinessObjects.PositionManagement;
using Prana.Global;
using Prana.LogManager;
using Prana.TaskManagement.Definition.Definition;
using Prana.Utilities.XMLUtilities;
using System;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;


namespace Prana.Import
{
    public class XSLTManager
    {
        //Source and File Name  are appended in the serializedXml and mappedxml file names to keep the uniqueness
        /// <summary>
        /// Get transformed data for the passed run upload object
        /// </summary>
        /// <param name="data"></param>
        /// <param name="runUpload"></param>
        /// <returns></returns>
        public static DataSet GetData(DataTable data, RunUpload runUpload, string HanderXSDName, out string errorMsg, out bool isValidated, TaskResult taskResult)
        {
            errorMsg = string.Empty;
            isValidated = false;
            try
            {
                DataSet dataSet = null;

                //create incoming and outgoing folder
                if (!Directory.Exists(Application.StartupPath + @"\Incoming"))
                    Directory.CreateDirectory(Application.StartupPath + @"\Incoming");

                if (!Directory.Exists(Application.StartupPath + @"\Outgoing"))
                    Directory.CreateDirectory(Application.StartupPath + @"\Outgoing");

                if (!Directory.Exists(Application.StartupPath + @"\Incoming\ValidationFiles"))
                    Directory.CreateDirectory(Application.StartupPath + @"\Incoming\ValidationFiles");

                if (!Directory.Exists(Application.StartupPath + @"\Incoming\MappingXML"))
                    Directory.CreateDirectory(Application.StartupPath + @"\Incoming\MappingXML");

                if (!Directory.Exists(Application.StartupPath + @"\Incoming\" + runUpload.DataSourceNameIDValue.ShortName))
                    Directory.CreateDirectory(Application.StartupPath + @"\Incoming\" + runUpload.DataSourceNameIDValue.ShortName);

                if (!Directory.Exists(Application.StartupPath + @"\Incoming\" + runUpload.DataSourceNameIDValue.ShortName + @"\DataFiles"))
                    Directory.CreateDirectory(Application.StartupPath + @"\Incoming\" + runUpload.DataSourceNameIDValue.ShortName + @"\DataFiles");

                if (!Directory.Exists(Application.StartupPath + @"\Incoming\" + runUpload.DataSourceNameIDValue.ShortName + @"\ValidationFiles"))
                    Directory.CreateDirectory(Application.StartupPath + @"\Incoming\" + runUpload.DataSourceNameIDValue.ShortName + @"\ValidationFiles");

                if (!Directory.Exists(Application.StartupPath + @"\Incoming\" + runUpload.DataSourceNameIDValue.ShortName + @"\MappingXML"))
                    Directory.CreateDirectory(Application.StartupPath + @"\Incoming\" + runUpload.DataSourceNameIDValue.ShortName + @"\MappingXML");

                if (!Directory.Exists(Application.StartupPath + @"\Incoming\" + runUpload.DataSourceNameIDValue.ShortName + @"\DataFiles\" + runUpload.ImportTypeAcronym.ToString()))
                    Directory.CreateDirectory(Application.StartupPath + @"\Incoming\" + runUpload.DataSourceNameIDValue.ShortName + @"\DataFiles\" + runUpload.ImportTypeAcronym.ToString());

                if (!Directory.Exists(Application.StartupPath + @"\Incoming\" + runUpload.DataSourceNameIDValue.ShortName + @"\DataFiles\" + runUpload.ImportTypeAcronym.ToString() + @"\xmls"))
                    Directory.CreateDirectory(Application.StartupPath + @"\Incoming\" + runUpload.DataSourceNameIDValue.ShortName + @"\DataFiles\" + runUpload.ImportTypeAcronym.ToString() + @"\xmls");

                string serializedXmlPath = Application.StartupPath + @"\Incoming\" + runUpload.DataSourceNameIDValue.ShortName + @"\DataFiles\" + runUpload.ImportTypeAcronym.ToString() + @"\xmls\SerializedXMLfor " + runUpload.Key + " .xml";

                // now generate the xml of table dataSource
                //string serializedXmlPath = Application.StartupPath + "\\xmls\\Transformation\\Temp\\SerializedXMLfor " + runUpload.Key + " .xml";

                try
                {
                    using (XmlTextWriter xmlWriter = new XmlTextWriter(serializedXmlPath, Encoding.UTF8))
                    {
                        data.WriteXml(xmlWriter);
                    }
                }
                catch (Exception ex)
                {
                    Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                    if (taskResult != null)
                    {
                        errorMsg = "The process cannot access the file because it is being used by another process.";
                        return null;
                    }
                }
                // data.WriteXml(serializedXmlPath);

                // get a new mapped xml
                string convertedXmlPath = Application.StartupPath + @"\Incoming\" + runUpload.DataSourceNameIDValue.ShortName + @"\DataFiles\" + runUpload.ImportTypeAcronym.ToString() + @"\xmls\ConvertedXMLfor " + runUpload.Key + " .xml";

                string xsdPath = string.Empty;

                //string xsltPath = Application.StartupPath + "\\" + ApplicationConstants.MAPPING_FILE_DIRECTORY + "\\" + ApplicationConstants.MappingFileType.PMImportXSLT.ToString() + "\\" + runUpload.DataSourceXSLT;
                string xsltPath = string.Empty;
                string importTagXSLTPath = string.Empty;
                string mappedFilePath = string.Empty;

                // check if user have defined the xsd path. or the is to be taken default(from handler)s
                //todo: Improve directory structure and remove the release type check
                //Modified by omshiv, removed release check, using ExecutionInfo.IsAutoImport 
                Boolean isAutoImport = false;
                if (taskResult != null)
                {
                    isAutoImport = taskResult.ExecutionInfo.IsAutoImport;
                }
                if (isAutoImport)// (CommonDataCache.CachedDataManager.GetPranaReleaseType().Equals(PranaReleaseViewType.CHMiddleWare))
                {
                    if (!Directory.Exists(Application.StartupPath + @"\Incoming\" + runUpload.DataSourceNameIDValue.ShortName + @"\TransformationFiles"))
                    {
                        Directory.CreateDirectory(Application.StartupPath + @"\Incoming\" + runUpload.DataSourceNameIDValue.ShortName + @"\TransformationFiles");
                    }
                    xsltPath = Application.StartupPath + @"\Incoming\" + runUpload.DataSourceNameIDValue.ShortName + @"\TransformationFiles\" + runUpload.DataSourceXSLT;
                    if (string.IsNullOrEmpty(runUpload.XSDName))
                    {
                        xsdPath = Application.StartupPath + @"\Incoming\ValidationFiles\" + HanderXSDName;

                    }
                    else
                    {
                        xsdPath = Application.StartupPath + @"\Incoming\" + runUpload.DataSourceNameIDValue.ShortName + @"\ValidationFiles\" + runUpload.XSDName;
                    }
                    //CHMW-2438 [Import] - Apply a middle level transformation of Input XML of import
                    importTagXSLTPath = Path.Combine(Path.GetDirectoryName(xsltPath), Path.GetFileNameWithoutExtension(xsltPath) + "_ImportTag.xslt");
                }
                else
                {
                    xsltPath = Application.StartupPath + "\\" + ApplicationConstants.MAPPING_FILE_DIRECTORY + @"\" + ApplicationConstants.MappingFileType.PMImportXSLT.ToString() + "\\" + runUpload.DataSourceXSLT;
                    xsdPath = Application.StartupPath + "\\" + ApplicationConstants.MAPPING_FILE_DIRECTORY + "\\" + ApplicationConstants.MappingFileType.PranaXSD.ToString() + "\\" + HanderXSDName;
                    //runUpload.ProcessedFilePath = runUpload.FilePath;
                }

                if (!string.IsNullOrEmpty(xsltPath))
                {
                    mappedFilePath = XMLUtilities.GetTransformed(serializedXmlPath, convertedXmlPath, xsltPath);
                }
                ////CHMW-2438 [Import] - Apply a middle level transformation of Input XML of import
                if (!string.IsNullOrEmpty(importTagXSLTPath) && File.Exists(importTagXSLTPath))
                {
                    string importTagFileFolder = Path.Combine(Path.GetDirectoryName(taskResult.GetDashBoardXmlPath()), "RefData");
                    string importTagFilePath = Path.Combine(importTagFileFolder, Path.GetFileName(taskResult.GetDashBoardXmlPath()) + "_ImportTag.xml");
                    taskResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("ImportTagFilePath", importTagFilePath, null);
                    importTagFilePath = XMLUtilities.GetTransformed(serializedXmlPath, Application.StartupPath + importTagFilePath, importTagXSLTPath);
                }
                if (!string.IsNullOrEmpty(xsdPath))
                {
                    string tempError;
                    bool isXmlValidated = Prana.Utilities.UI.XMLUtilities.XMLUtilities.ValidateXML(mappedFilePath, xsdPath, "", out tempError, false);
                    isValidated = isXmlValidated;
                    if (!isXmlValidated)
                    {
                        errorMsg = tempError;
                        return null;
                    }
                }

                if (!mappedFilePath.Equals(string.Empty))
                {
                    dataSet = new DataSet();
                    dataSet.ReadXml(mappedFilePath);
                }

                //We may need transformation files for analysis, so need to delete these files

                //if (File.Exists(serializedXmlPath))
                //    File.Delete(serializedXmlPath);
                //if (File.Exists(convertedXmlPath))
                //    File.Delete(convertedXmlPath);

                return dataSet;
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
            return null;
        }
    }
}