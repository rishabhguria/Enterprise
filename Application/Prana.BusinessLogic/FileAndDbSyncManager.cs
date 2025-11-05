using Prana.BusinessObjects;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace Prana.BusinessLogic
{
    public class FileAndDbSyncManager
    {
        const string CONST_BLANK = "";
        const string CONST_RTPNL = "RTPNL";

        static string _starpUpPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        static DateTime _dbAcessTime = DateTime.UtcNow;

        /// <summary>
        /// Get updated files from filesystem and update them in database
        /// </summary>
        public static void SyncDataBaseWithFile(string directoryPath, int userID, bool isOnlyLayouts = false, string moduleName = CONST_BLANK, string description = CONST_BLANK, string fileName = CONST_BLANK, string pageId = CONST_BLANK, string viewId = CONST_BLANK, string viewName = CONST_BLANK)
        {
            try
            {
                string path = string.Empty;
                if (userID != -1)
                {
                    path = _starpUpPath + "//" + directoryPath + "//" + userID;
                }
                else
                {
                    path = _starpUpPath + "//" + directoryPath;
                }
                DirectoryInfo dir = new DirectoryInfo(path);
                if (!Directory.Exists(path))
                {
                    return;
                }
                foreach (FileInfo f in dir.GetFiles("*.*"))
                {
                    DateTime dateTime = f.LastWriteTimeUtc;
                    if (dateTime > _dbAcessTime)
                    {
                        FileStream fs = null;
                        BinaryReader br = null;
                        try
                        {
                            fs = new FileStream(path + "//" + f.Name, FileMode.Open, FileAccess.Read, FileShare.Read);
                            br = new BinaryReader(fs);
                            byte[] data = new byte[fs.Length];
                            int length = (int)fs.Length;
                            br.Read(data, 0, length);

                            object[] parameters;
                            string spName;

                            if (isOnlyLayouts && fileName.Equals(f.Name))
                            {
                                parameters = new object[9];
                                parameters[0] = userID;
                                parameters[1] = f.Name;
                                parameters[2] = data;
                                parameters[3] = DateTime.UtcNow;
                                parameters[4] = moduleName;
                                parameters[5] = description;
                                parameters[6] = pageId;
                                parameters[7] = viewId;
                                parameters[8] = viewName;

                                spName = "P_Samsara_SaveCompanyUserLayouts";
                            }
                            else
                            {
                                parameters = new object[4];
                                parameters[0] = userID;
                                parameters[1] = f.Name;
                                parameters[2] = data;
                                parameters[3] = DateTime.UtcNow;

                                spName = "P_SavePranaUserPrefs";
                            }
                            DatabaseManager.DatabaseManager.ExecuteNonQuery(spName, parameters);
                        }
                        catch (Exception ex)
                        {
                            LogManager.LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex);
                        }
                        finally
                        {
                            if (fs != null)
                            {
                                fs.Close();
                                //br.Close();
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                LogManager.LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex);
            }

        }
        /// <summary>
        ///Read data from db and get file system in sync with database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static Dictionary<string, string> SyncFileWithDataBase(string directoryPath, int userID, bool isOnlyLayouts = false, string moduleName = CONST_BLANK)
        {
            Dictionary<string, string> layoutsInformation = new Dictionary<string, string>();
            try
            {
                _dbAcessTime = DateTime.UtcNow;
                string path = string.Empty;
                if (userID != -1)
                {
                    path = _starpUpPath + "//" + directoryPath + "//" + userID;
                }
                else
                {
                    path = _starpUpPath + "//" + directoryPath;
                }
                DirectoryInfo dir = new DirectoryInfo(path);
                string allFileDetails = string.Empty;
                char seperator1 = Seperators.SEPERATOR_5;
                char seperator2 = Seperators.SEPERATOR_6;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                else if (isOnlyLayouts)
                {
                    //This will only run for Samsara.
                    DirectoryInfo directory = new DirectoryInfo(path);
                    foreach (FileInfo file in directory.EnumerateFiles())
                        file.Delete();
                }
                foreach (FileInfo f in dir.GetFiles("*.*"))
                {
                    string fileDetails = f.Name + seperator2 + f.LastWriteTimeUtc.ToString();
                    if (allFileDetails != string.Empty)
                        allFileDetails = allFileDetails + seperator1 + fileDetails;
                    else
                        allFileDetails = fileDetails;
                }

                try
                {
                    if(string.Equals(moduleName, CONST_RTPNL, StringComparison.OrdinalIgnoreCase))
                        allFileDetails = string.Empty;
                    if (isOnlyLayouts)
                        layoutsInformation = GetWebUserLayouts(userID, path, allFileDetails, seperator1, seperator2, moduleName);
                    else
                        GetPranaUserPrefs(userID, path, allFileDetails, seperator1, seperator2);
                }
                catch (Exception ex)
                {
                    LogManager.LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return layoutsInformation;
        }

        /// <summary>
        ///Remove user layouts from preference folder
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <param name="userID"></param>
        public static void RemoveUserLayoutsFromPrefFolder(string directoryPath, int userID)
        {
            try
            {
                string path = string.Empty;
                if (userID != -1)
                    path = _starpUpPath + "//" + directoryPath + "//" + userID;

                if (Directory.Exists(path))
                {
                    DirectoryInfo directory = new DirectoryInfo(path);
                    foreach (FileInfo file in directory.EnumerateFiles())
                        file.Delete();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Get updated files from filesystem and update them in database
        /// </summary>
        public static void SyncDataBaseWithFile(string path, string spName)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(path);
                if (!Directory.Exists(path))
                {
                    return;
                }
                foreach (FileInfo f in dir.GetFiles("*.*"))
                {
                    DateTime dateTime = f.LastWriteTimeUtc;
                    if (dateTime > _dbAcessTime)
                    {
                        FileStream fs = null;
                        BinaryReader br = null;
                        try
                        {
                            fs = new FileStream(path + "//" + f.Name, FileMode.Open, FileAccess.Read, FileShare.Read);
                            br = new BinaryReader(fs);
                            byte[] data = new byte[fs.Length];
                            int length = (int)fs.Length;
                            br.Read(data, 0, length);


                            object[] parameters = new object[3];
                            parameters[0] = f.Name;
                            parameters[1] = data;
                            parameters[2] = DateTime.UtcNow;
                            DatabaseManager.DatabaseManager.ExecuteNonQuery(spName, parameters);
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
                        finally
                        {
                            if (fs != null)
                            {
                                fs.Close();
                                // br.Close();
                            }
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
        /// <summary>
        ///Read data from db and get file system in sync with database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void SyncFileWithDataBase(string startupPath, Prana.Global.ApplicationConstants.MappingFileType fileType)
        {
            string path = startupPath + @"\" + ApplicationConstants.MAPPING_FILE_DIRECTORY + @"\" + fileType.ToString();
            DirectoryInfo dir = new DirectoryInfo(path);
            string allFileDetails = string.Empty;
            char seperator1 = '^';
            char seperator2 = '~';
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            foreach (FileInfo f in dir.GetFiles("*.*"))
            {
                string fileDetails = f.Name + seperator2 + f.LastWriteTimeUtc.ToString();
                if (allFileDetails != string.Empty)
                    allFileDetails = allFileDetails + seperator1 + fileDetails;
                else
                    allFileDetails = fileDetails;
            }

            try
            {
                SyncFilesWithDB(fileType, path, allFileDetails, seperator1, seperator2);
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

        public static Dictionary<string, byte[]> GetRequestedPreference(int userID, string FileNames)
        {
            try
            {
                string[] ParsedFileNames = FileNames.Split('~');
                Dictionary<string, byte[]> result = new Dictionary<string, byte[]>();
                object[] parameter = new object[4];
                parameter[0] = userID;
                parameter[1] = "";
                parameter[2] = Seperators.SEPERATOR_5;
                parameter[3] = Seperators.SEPERATOR_6;
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetPranaUserPrefs", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        string fileName = row[0].ToString();
                        if(ParsedFileNames.Contains(fileName))
                        {
                            result.Add(fileName, (byte[])row[1]);
                        }
                    }
                }
                return result;
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

        /// <summary>
        /// Get the user preference from db for the Enterprise user
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="path"></param>
        /// <param name="allFileDetails"></param>
        /// <param name="seperator1"></param>
        /// <param name="seperator2"></param>
        private static void GetPranaUserPrefs(int userID, string path, string allFileDetails, char seperator1, char seperator2)
        {
            try
            {
                object[] parameter = new object[4];
                parameter[0] = userID;
                parameter[1] = allFileDetails;
                parameter[2] = seperator1;
                parameter[3] = seperator2;
                string spName = "P_GetPranaUserPrefs";
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(spName, parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        string fileName = row[0].ToString();
                        byte[] data = (byte[])row[1];
                        FileStream fs = null;
                        try
                        {
                            fs = new FileStream(path + "//" + fileName, FileMode.Create);
                            fs.Write(data, 0, data.Length);
                        }
                        catch (Exception ex)
                        {
                            LogManager.LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex);
                        }
                        finally
                        {
                            if (fs != null)
                                fs.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex);
            }
        }

        /// <summary>
        /// Get the layouts from db for the Web user
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="path"></param>
        /// <param name="allFileDetails"></param>
        /// <param name="separator1"></param>
        /// <param name="separator2"></param>
        /// <param name="moduleName"></param>
        private static Dictionary<string, string> GetWebUserLayouts(int userID, string path, string allFileDetails, char separator1, char separator2, string moduleName)
        {
            Dictionary<string, string> layoutsInfo = new Dictionary<string, string>();    
            try
            {
                object[] parameter = new object[5];
                parameter[0] = userID;
                parameter[1] = allFileDetails;
                parameter[2] = separator1;
                parameter[3] = separator2;
                parameter[4] = moduleName;
                string spName = "P_Samsara_GetCompanyUserLayouts";
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(spName, parameter))
                {
                    while (reader.Read())
                    {

                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        string fileName = row[0].ToString();
                        byte[] data = (byte[])row[1];
                        FileStream fs = null;
                        try
                        {
                            fs = new FileStream(path + "//" + fileName, FileMode.Create);
                            fs.Write(data, 0, data.Length);
                            if(!layoutsInfo.ContainsKey(fileName))
                                layoutsInfo.Add(fileName, row[4].ToString());
                        }
                        catch (Exception ex)
                        {
                            LogManager.LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex);
                        }
                        finally
                        {
                            if (fs != null)
                                fs.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex);
            }
            return layoutsInfo;
        }

        private static void SyncFilesWithDB(Prana.Global.ApplicationConstants.MappingFileType fileType, string path, string allFileDetails, char seperator1, char seperator2)
        {
            object[] parameter = new object[4];
            parameter[0] = allFileDetails;
            parameter[1] = seperator1;
            parameter[2] = seperator2;
            parameter[3] = Convert.ToInt32(fileType);

            string spName = "P_RetrieveMappingFilesFromDB";
            using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(spName, parameter))
            {
                while (reader.Read())
                {

                    object[] row = new object[reader.FieldCount];
                    reader.GetValues(row);
                    string fileName = row[0].ToString();
                    byte[] data = (byte[])row[1];
                    FileStream fs = null;
                    try
                    {
                        fs = new FileStream(path + "\\" + fileName, FileMode.Create);
                        fs.Write(data, 0, data.Length);
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
                    finally
                    {
                        if (fs != null)
                            fs.Close();
                    }
                }
            }
        }

        /// <summary>
        /// DeleteCompanyUserLayout
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="fileName"></param>
        public static void DeleteCompanyUserLayout(int userID, string fileName)
        {
            try
            {
                object[] param = new object[2];
                param[0] = userID;
                param[1] = fileName;
                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_Samsara_DeleteCompanyUserLayout", param);
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
    }
}
