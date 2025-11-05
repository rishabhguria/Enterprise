using Prana.BusinessObjects;
using Prana.LogManager;
using System;
using System.Linq;
using System.Text;

namespace Prana.Utilities.MiscUtilities
{
    public class FileNameParser
    {

        /// <summary>
        /// retuns parsed filename
        /// Name before parsing: "a{M/d/yyyy}a{M/d/yy}a{MM/dd/yy}a{MM/dd/yy}a{yy/MM/dd}a{yyyy-MM-dd}a{dd-MMM-yy}a{dd}a{mmm}a{yy}a{yyyy}a{mm}a{}.csv" 
        /// Name after parsing: a4/3/2014a4/3/14a04/03/14a04/03/14a14/04/03a2014-04-03a03-Apr-14a03aApra14a2014a04a.csv
        /// </summary>
        /// <param name="nameBeforeParsing"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string GetFileNameFromNamingConvention(string nameBeforeParsing, DateTime date)
        {
            string fileName = string.Empty;
            try
            {
                fileName = nameBeforeParsing;

                int startBracesCount = Convert.ToInt32(fileName.Count(c => c == '{').ToString());
                int endBreacesCount = Convert.ToInt32(fileName.Count(c => c == '}').ToString());

                if (startBracesCount == endBreacesCount)
                {
                    for (int i = 0; i < startBracesCount; i++)
                    {
                        int startIndexBraces = fileName.IndexOf("{");
                        int endIndexBraces = fileName.IndexOf("}");

                        //second condition is used for the following use case
                        //when there is {} in filename then {} is replaced with 4/3/2014 12:00:00 AM format
                        //&& ((endIndexBraces - startIndexBraces)!=1)
                        if ((startIndexBraces != -1 && endIndexBraces != -1))
                        {

                            int lengthOfFile = (endIndexBraces - startIndexBraces) - 1;
                            string FileDateFormat = fileName.Substring(startIndexBraces + 1, lengthOfFile);
                            string strFileNameBeforeStartBraces = fileName.Substring(0, fileName.IndexOf("{"));
                            string strFileNameBeforeClosingBraces = fileName.Substring(0, fileName.IndexOf("}"));
                            string strFileNameAfterClosingBraces = fileName.Substring(strFileNameBeforeClosingBraces.Length + 1);
                            string DateFormat = string.Empty;
                            //DateFormat can be empty when there is {} in name of file
                            if (!string.IsNullOrEmpty(FileDateFormat))
                            {
                                //We also need to handle time with file name
                                DateFormat = date.ToString(FileDateFormat);
                            }
                            fileName = strFileNameBeforeStartBraces + DateFormat + strFileNameAfterClosingBraces;
                        }
                    }
                }
                else
                {
                    LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("The Naming Convention of The File to be Imported is not correct.", LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information);
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
            return fileName;
        }

        /// <summary>
        /// get decrypted filename for encrypted file name
        /// While decryption we don't know that what will be filename after decryption
        /// we are returning filename after trimming file with extension
        /// </summary>
        /// <param name="originalFilePath">e.g. ENLANDER_trades_20140409.CSV.asc.14.04.04_01_2</param>
        /// <returns>e.g. ENLANDER_trades_20140409.CSV</returns>
        public static string GetFileNameUsingExtension(string originalFilePath)
        {
            string filePath = string.Empty;
            try
            {
                if (originalFilePath.ToLower().Contains(AutomationEnum.FileFormat.csv.ToString()))
                {
                    filePath = originalFilePath.Substring(0, originalFilePath.ToLower().LastIndexOf(AutomationEnum.FileFormat.csv.ToString()) + AutomationEnum.FileFormat.csv.ToString().Length);
                }
                else if (originalFilePath.ToLower().Contains(AutomationEnum.FileFormat.xls.ToString()))
                {
                    filePath = originalFilePath.Substring(0, originalFilePath.ToLower().LastIndexOf(AutomationEnum.FileFormat.xls.ToString()) + AutomationEnum.FileFormat.csv.ToString().Length);
                }
                else if (originalFilePath.ToLower().Contains(AutomationEnum.FileFormat.txt.ToString()))
                {
                    filePath = originalFilePath.Substring(0, originalFilePath.ToLower().LastIndexOf(AutomationEnum.FileFormat.txt.ToString()) + AutomationEnum.FileFormat.csv.ToString().Length);
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
            return filePath;
        }

        public static string GetDateStringFromFileName(string fileNameSyntax, string fileName)
        {
            StringBuilder dateFormat = new StringBuilder();
            try
            {
                int startBracesCount = Convert.ToInt32(fileNameSyntax.Count(c => c == '{').ToString());
                int endBreacesCount = Convert.ToInt32(fileNameSyntax.Count(c => c == '}').ToString());

                if (startBracesCount == endBreacesCount)
                {
                    for (int i = 0; i < startBracesCount; i++)
                    {
                        int startIndexBraces = fileNameSyntax.IndexOf("{");
                        int endIndexBraces = fileNameSyntax.IndexOf("}");
                        //second condition is used for the following use case
                        //when there is {} in filename then {} is replaced with 4/3/2014 12:00:00 AM format
                        //&& ((endIndexBraces - startIndexBraces)!=1)
                        if ((startIndexBraces != -1 && endIndexBraces != -1))
                        {
                            int lengthOfFile = (endIndexBraces - startIndexBraces) - 1;
                            string FileDateFormat = fileName.Substring(startIndexBraces, lengthOfFile);
                            string strFileNameBeforeStartBraces = fileNameSyntax.Substring(0, fileNameSyntax.IndexOf("{"));
                            string strFileNameBeforeClosingBraces = fileNameSyntax.Substring(0, fileNameSyntax.IndexOf("}"));
                            string strFileNameAfterClosingBraces = fileNameSyntax.Substring(strFileNameBeforeClosingBraces.Length + 1);
                            //DateFormat can be empty when there is {} in name of file
                            if (!string.IsNullOrEmpty(FileDateFormat))
                            {
                                dateFormat.Append(FileDateFormat);
                            }
                            fileNameSyntax = strFileNameBeforeStartBraces + FileDateFormat + strFileNameAfterClosingBraces;
                        }
                    }
                }
                else
                {
                    LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("The Naming Convention of The File to be Imported is not correct.", LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information);
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
            return dateFormat.ToString();
        }

        public static string GetDateFormatFromFileName(string fileName)
        {
            StringBuilder dateFormat = new StringBuilder();
            try
            {
                int startBracesCount = Convert.ToInt32(fileName.Count(c => c == '{').ToString());
                int endBreacesCount = Convert.ToInt32(fileName.Count(c => c == '}').ToString());

                if (startBracesCount == endBreacesCount)
                {
                    for (int i = 0; i < startBracesCount; i++)
                    {
                        int startIndexBraces = fileName.IndexOf("{");
                        int endIndexBraces = fileName.IndexOf("}");
                        //second condition is used for the following use case
                        //when there is {} in filename then {} is replaced with 4/3/2014 12:00:00 AM format
                        //&& ((endIndexBraces - startIndexBraces)!=1)
                        if ((startIndexBraces != -1 && endIndexBraces != -1))
                        {
                            int lengthOfFile = (endIndexBraces - startIndexBraces) - 1;
                            string FileDateFormat = fileName.Substring(startIndexBraces + 1, lengthOfFile);
                            string strFileNameBeforeStartBraces = fileName.Substring(0, fileName.IndexOf("{"));
                            string strFileNameBeforeClosingBraces = fileName.Substring(0, fileName.IndexOf("}"));
                            string strFileNameAfterClosingBraces = fileName.Substring(strFileNameBeforeClosingBraces.Length + 1);
                            //DateFormat can be empty when there is {} in name of file
                            if (!string.IsNullOrEmpty(FileDateFormat))
                            {
                                dateFormat.Append(FileDateFormat);
                            }
                            fileName = strFileNameBeforeStartBraces + dateFormat + strFileNameAfterClosingBraces;
                        }
                    }
                }
                else
                {
                    LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("The Naming Convention of The File to be Imported is not correct.", LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information);
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
            return dateFormat.ToString();
        }

    }
}
