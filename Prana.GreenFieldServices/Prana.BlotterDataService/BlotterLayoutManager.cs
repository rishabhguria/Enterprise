using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Prana.BusinessLogic;
using Prana.BusinessObjects.BlotterDataService;
using Prana.KafkaWrapper;
using Prana.KafkaWrapper.Extension.Classes;
using Prana.LogManager;

namespace Prana.BlotterDataService
{
    public class BlotterLayoutManager
    {
        public const string CONST_WEB_PREFERENCES = @"Nirvana_Web_Preferences\";
        public const string CONST_BLOTTER_GRID_LAYOUT = "BlotterGridLayout";
        public const string CONST_TXT = ".txt";
        public const string CONST_PATH_SEPARATOR = "\\";
        public const string CONST_ORDERS_GRIDNAME = "OrdersList";
        public const string CONST_SUBORDERS_GRIDNAME = "SubOrdersList";
        public const string CONST_WORKINGTAB_GRIDNAME = "WorkingTabList";
        public const string CONST_SUMMARYTAB_GRIDNAME = "SummaryTabList";
        public const string CONST_ORDER_SEPARATOR = "_Order_";
        public const string CONST_SUBORDER_SEPARATOR = "_SubOrder_";
        public const string CONST_TAB_NAME_DYNAMIC_ORDER = "Dynamic_Order_";
        public const string CONST_TAB_OLD_NAME = "TabOldName";
        public const string CONST_TAB_NEW_NAME = "TabNewName";
        public const string CONST_ALL = "*";
        public const string CONST_BLANK = "";
        public const string CONST_BLOTTER = "Blotter";

        #region SingletonInstance

        /// <summary>
        /// Locker object
        /// </summary>
        private static readonly Object _lock = new Object();

        /// <summary>
        /// The singilton instance
        /// </summary>
        private static BlotterLayoutManager _blotterLayoutManager = null;
        /// <summary>
        /// Singilton instance
        /// </summary>
        /// <returns></returns>
        public static BlotterLayoutManager GetInstance()
        {
            lock (_lock)
            {
                if (_blotterLayoutManager == null)
                    _blotterLayoutManager = new BlotterLayoutManager();
                return _blotterLayoutManager;
            }
        }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        private BlotterLayoutManager()
        {
            try
            {
                #region SubscribeAndConsume
                //KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_SaveLayoutRequest, KafkaManager_SaveBlotterLayoutRequest);
                //KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_LoadLayoutRequest, KafkaManager_LoadBlotterLayoutRequest);
                //KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_RenameBlotterCustomTabRequest, KafkaManager_RenameBlotterCustomTabRequest);
                //KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_RemoveBlotterCustomTabRequest, KafkaManager_RemoveBlotterCustomTabRequest);
                #endregion
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
        /// Loads layouts for logged in User from database
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        internal void LoadLayoutsForLoggedInUser(int companyUserID)
        {
            try
            {
                FileAndDbSyncManager.SyncFileWithDataBase(CONST_WEB_PREFERENCES, companyUserID, true, CONST_BLOTTER);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// KafkaManager_LoadBlotterLayoutRequest
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private async void KafkaManager_LoadBlotterLayoutRequest(string topic, RequestResponseModel message)
        {
            try
            {
                InformationReporter.GetInstance.Write(BlotterDataConstants.MSG_LoadLayoutRequestedBy + message.CompanyUserID);
                int companyUserId = message.CompanyUserID;
                Dictionary<string, string> gridLayouts = new Dictionary<string, string>();
                string blotterPreferencePath = AppContext.BaseDirectory + CONST_WEB_PREFERENCES + companyUserId;
                if (Directory.Exists(blotterPreferencePath))
                {
                    DirectoryInfo info = new DirectoryInfo(blotterPreferencePath);
                    FileInfo[] files = info.GetFiles(CONST_ALL + CONST_BLOTTER_GRID_LAYOUT + CONST_TXT).OrderBy(p => p.CreationTime).ToArray();
                    foreach (FileInfo file in files)
                    {
                        string tabKey = Path.GetFileName(file.Name).Replace(CONST_BLOTTER_GRID_LAYOUT + CONST_TXT, CONST_BLANK);
                        string gridLayout = File.ReadAllText(file.FullName);
                        if (gridLayout != null)
                            gridLayouts.Add(tabKey, gridLayout);
                    }
                }
                message.Data = JsonConvert.SerializeObject(gridLayouts);
                await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_LoadLayoutResponse, message);
                InformationReporter.GetInstance.Write(BlotterDataConstants.MSG_LoadLayoutResponseFor + message.CompanyUserID);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// KafkaManager_SaveBlotterLayoutRequest
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private async void KafkaManager_SaveBlotterLayoutRequest(string topic, RequestResponseModel message)
        {
            InformationReporter.GetInstance.Write(BlotterDataConstants.MSG_SaveLayoutRequestedBy + message.CompanyUserID);
            string errorMessage = string.Empty;
            try
            {
                Dictionary<string, string> layouts = JsonConvert.DeserializeObject<Dictionary<string, string>>(message.Data);
                foreach (KeyValuePair<string, string> layout in layouts)
                {
                    string gridName = layout.Key;
                    string savedLayoutText = layout.Value;

                    if (string.IsNullOrEmpty(gridName))
                        errorMessage = BlotterDataConstants.MSG_ErrorForSaveLayout;

                    if (string.IsNullOrEmpty(errorMessage))
                    {
                        string startupPath = AppContext.BaseDirectory + CONST_WEB_PREFERENCES + message.CompanyUserID;
                        if (!Directory.Exists(startupPath))
                        {
                            Directory.CreateDirectory(startupPath);
                        }
                        try
                        {
                            string fileName = gridName + CONST_BLOTTER_GRID_LAYOUT + CONST_TXT;
                            string file = startupPath + CONST_PATH_SEPARATOR + fileName;
                            TextWriter txt = new StreamWriter(file);
                            txt.Write(savedLayoutText);
                            txt.Close();

                        FileAndDbSyncManager.SyncDataBaseWithFile(CONST_WEB_PREFERENCES, message.CompanyUserID, true, CONST_BLOTTER, "", fileName);
                        }
                        catch (Exception ex)
                        {
                            errorMessage = ex.Message;
                            break;
                        }
                    }
                }
                message.Data = JsonConvert.SerializeObject(errorMessage);
                await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_SaveLayoutResponse, message);
                InformationReporter.GetInstance.Write(BlotterDataConstants.MSG_SaveLayoutResponseFor + message.CompanyUserID);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// KafkaManager_RenameBlotterCustomTabRequest
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private async void KafkaManager_RenameBlotterCustomTabRequest(string topic, RequestResponseModel message)
        {
            InformationReporter.GetInstance.Write(BlotterDataConstants.MSG_RenameCustomTabRequestedBy + message.CompanyUserID);
            string errorMessage = string.Empty;
            try
            {
                try
                {
                    Dictionary<string, string> customTabDetails = JsonConvert.DeserializeObject<Dictionary<string, string>>(message.Data);

                    string oldTabName = customTabDetails[CONST_TAB_OLD_NAME];
                    string newTabName = customTabDetails[CONST_TAB_NEW_NAME];

                    bool finalResult = RenameOldLayout(message.CompanyUserID, oldTabName, newTabName);
                    if (!finalResult)
                        errorMessage = BlotterDataConstants.MSG_ErrorFailedToRenameTab;
                }
                catch(Exception ex)
                {
                    if (ex.Message.Contains(BlotterDataConstants.MSG_ERROR_EXPECTED_DELIMITER))
                        errorMessage = BlotterDataConstants.MSG_ERROR_ILLEGAL_CHARS_IN_PATH;
                    else
                        errorMessage = ex.Message;
                }

                message.Data = JsonConvert.SerializeObject(errorMessage);
                await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_RenameBlotterCustomTabResponse, message);
                InformationReporter.GetInstance.Write(BlotterDataConstants.MSG_RenameCustomTabResponseFor + message.CompanyUserID);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// KafkaManager_RemoveBlotterCustomTabRequest
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private async void KafkaManager_RemoveBlotterCustomTabRequest(string topic, RequestResponseModel message)
        {
            InformationReporter.GetInstance.Write(BlotterDataConstants.MSG_RemoveCustomTabRequestedBy + message.CompanyUserID);
            string errorMessage = string.Empty;
            try
            {
                string tabNameToRemove = message.Data;
                bool finalResult = RemoveTabData(message.CompanyUserID, tabNameToRemove);
                if (!finalResult)
                    errorMessage = BlotterDataConstants.MSG_ErrorFailedToRemoveTab;
                message.Data = JsonConvert.SerializeObject(errorMessage);
                await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_RemoveBlotterCustomTabResponse, message);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            InformationReporter.GetInstance.Write(BlotterDataConstants.MSG_RemoveCustomTabResponseFor + message.CompanyUserID);
        }

        /// <summary>
        /// RenameOldLayout
        /// </summary>
        /// <param name="companyUserId"></param>
        /// <param name="oldTabName"></param>
        /// <param name="newTabName"></param>
        private bool RenameOldLayout(int companyUserId, string oldTabName, string newTabName)
        {
            bool renameResult = false;
            try
            {
                string newFileName = newTabName + CONST_BLOTTER_GRID_LAYOUT + CONST_TXT;
                string startupPath = AppContext.BaseDirectory + CONST_WEB_PREFERENCES + companyUserId;
                string oldLayoutFile = startupPath + CONST_PATH_SEPARATOR + oldTabName + CONST_BLOTTER_GRID_LAYOUT + CONST_TXT;
                string newLayoutFile = startupPath + CONST_PATH_SEPARATOR + newFileName;

                Task.Run(() => FileAndDbSyncManager.DeleteCompanyUserLayout(companyUserId, oldTabName + CONST_BLOTTER_GRID_LAYOUT + CONST_TXT));
                if (File.Exists(oldLayoutFile))
                    File.Move(oldLayoutFile, newLayoutFile);

                FileAndDbSyncManager.SyncDataBaseWithFile(CONST_WEB_PREFERENCES, companyUserId, true, CONST_BLOTTER, "", newFileName);

                #region For sub order tab
                if (oldTabName.StartsWith(CONST_TAB_NAME_DYNAMIC_ORDER))
                {
                    oldTabName = oldTabName.Replace(CONST_ORDER_SEPARATOR, CONST_SUBORDER_SEPARATOR);
                    newTabName = newTabName.Replace(CONST_ORDER_SEPARATOR, CONST_SUBORDER_SEPARATOR);
                    newFileName = newTabName + CONST_BLOTTER_GRID_LAYOUT + CONST_TXT;
                    oldLayoutFile = startupPath + CONST_PATH_SEPARATOR + oldTabName + CONST_BLOTTER_GRID_LAYOUT + CONST_TXT;
                    newLayoutFile = startupPath + CONST_PATH_SEPARATOR + newFileName;

                    Task.Run(() => FileAndDbSyncManager.DeleteCompanyUserLayout(companyUserId, oldTabName + CONST_BLOTTER_GRID_LAYOUT + CONST_TXT));
                    if (File.Exists(oldLayoutFile))
                        File.Move(oldLayoutFile, newLayoutFile);

                    FileAndDbSyncManager.SyncDataBaseWithFile(CONST_WEB_PREFERENCES, companyUserId, true, CONST_BLOTTER, "", newFileName);
                }
                #endregion
                renameResult = true;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return renameResult;
        }

        /// <summary>
        /// RemoveTabData
        /// </summary>
        /// <param name="companyUserID"></param>
        /// <param name="tabNameToRemove"></param>
        private bool RemoveTabData(int companyUserID, string tabNameToRemove)
        {
            bool removeResult = false;
            try
            {
                string startupPath = AppContext.BaseDirectory + CONST_WEB_PREFERENCES + companyUserID;
                string layoutFileToRemove = startupPath + CONST_PATH_SEPARATOR + tabNameToRemove + CONST_BLOTTER_GRID_LAYOUT + CONST_TXT;
                Task.Run(() => FileAndDbSyncManager.DeleteCompanyUserLayout(companyUserID, tabNameToRemove + CONST_BLOTTER_GRID_LAYOUT + CONST_TXT));
                if (File.Exists(layoutFileToRemove))
                    File.Delete(layoutFileToRemove);

                #region For sub order tab
                if (tabNameToRemove.StartsWith(CONST_TAB_NAME_DYNAMIC_ORDER))
                {
                    tabNameToRemove = tabNameToRemove.Replace(CONST_ORDER_SEPARATOR, CONST_SUBORDER_SEPARATOR);
                    layoutFileToRemove = startupPath + CONST_PATH_SEPARATOR + tabNameToRemove + CONST_BLOTTER_GRID_LAYOUT + CONST_TXT;

                    Task.Run(() => FileAndDbSyncManager.DeleteCompanyUserLayout(companyUserID, tabNameToRemove + CONST_BLOTTER_GRID_LAYOUT + CONST_TXT));
                    if (File.Exists(layoutFileToRemove))
                        File.Delete(layoutFileToRemove);
                }
                removeResult = true;
                #endregion
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return removeResult;
        }
    }
}
