using Prana.LogManager;
using Prana.Utilities.MiscUtilities.ImportExportUtilities;
using System;
using System.Collections;
using System.Data;

namespace Prana.Utilities.ImportExportUtilities
{
    public class FileReaderFactory
    {
        private static ArrayList _registeredImplementations;

        static FileReaderFactory()
        {
            _registeredImplementations = new ArrayList();
            RegisterClass(typeof(ExcelReadingStrategy));
            RegisterClass(typeof(CsvReadingStrategy));
            RegisterClass(typeof(TextReadingStrategy));
            RegisterClass(typeof(DefaultReadingStrategy));
            RegisterClass(typeof(NpoiXlsReadingStrategy));
            RegisterClass(typeof(NpoiXlsxReadingStrategy));
        }

        /// <summary>
        /// Registers the class.
        /// </summary>
        /// <param name="requestStrategyImpl">The request strategy impl.</param>
        public static void RegisterClass(Type requestStrategyImpl)
        {
            if (!requestStrategyImpl.IsSubclassOf(typeof(FileFormatStrategy)))
                throw new Exception("ArithmiticStrategy must inherit from " +
                                                  "class AbstractArithmiticStrategy");

            _registeredImplementations.Add(requestStrategyImpl);
        }

        public static FileFormatStrategy Create(DataSourceFileFormat formatType)
        {
            // loop thru all registered implementations
            foreach (Type impl in _registeredImplementations)
            {
                // get attributes for this type
                object[] attrlist = impl.GetCustomAttributes(true);

                // loop thru all attributes for this class
                foreach (object attr in attrlist)
                {
                    if (attr is FormattingAttribute)
                    {
                        if (((FormattingAttribute)attr).DataSourceFileFormat.Equals(formatType))
                        {
                            return
                             (FileFormatStrategy)System.Activator.CreateInstance(impl);
                        }
                    }
                }
            }
            throw new Exception("Could not find a FileFormatStrategy implementation for this fileFormatType");
        }

        public static DataTable GetDataTableFromDifferentFileFormats(string fileName)
        {
            DataTable dTable = null;
            try
            {
                string fileFormat = fileName.Substring(fileName.LastIndexOf(".") + 1);

                switch (fileFormat.ToUpperInvariant())
                {
                    case "CSV":
                        dTable = FileReaderFactory.Create(DataSourceFileFormat.Csv).GetDataTableFromUploadedDataFile(fileName);
                        break;
                    case "XLS":
                        dTable = FileReaderFactory.Create(DataSourceFileFormat.Xls).GetDataTableFromUploadedDataFile(fileName);
                        break;
                    default:
                        dTable = FileReaderFactory.Create(DataSourceFileFormat.Default).GetDataTableFromUploadedDataFile(fileName);
                        break;
                }
            }
            catch (System.IO.IOException ex)
            {
                Logger.LoggerWrite(ex, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, "File in use! Please close the file and retry.");
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
            return dTable;
        }

        /// <summary>
        /// This method is created to handle the reading of xls and xlsx files using npoi nuget package.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static DataTable GetDataTableFromDifferentFileFormatsNew(string fileName)
        {
            DataTable dTable = null;
            try
            {
                string fileFormat = fileName.Substring(fileName.LastIndexOf(".") + 1);

                switch (fileFormat.ToUpperInvariant())
                {
                    case "CSV":
                        dTable = Create(DataSourceFileFormat.Csv).GetDataTableFromUploadedDataFile(fileName);
                        break;
                    case "XLS":
                        dTable = Create(DataSourceFileFormat.Xls).GetDataTableFromUploadedDataFile(fileName);
                        break;
                    case "XLSX":
                        dTable = Create(DataSourceFileFormat.Xlsx).GetDataTableFromUploadedDataFile(fileName);
                        break;
                    default:
                        dTable = Create(DataSourceFileFormat.Default).GetDataTableFromUploadedDataFile(fileName);
                        break;
                }
            }
            catch (System.IO.IOException ex)
            {
                Logger.LoggerWrite(ex, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, "File in use! Please close the file and retry.");
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
            return dTable;
        }
    }
}
