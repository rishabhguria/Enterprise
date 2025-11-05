using Newtonsoft.Json;
using Prana.DatabaseManager;
using Prana.Global.Utilities;
using Prana.KafkaWrapper;
using Prana.KafkaWrapper.Extension.Classes;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.ServiceModel;

namespace Prana.CommonDataService
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single, UseSynchronizationContext = false)]
    public class OpenfinDataManagerService
    {
        /// <summary>
        /// User Wise Openfin Default Workspace Information
        /// </summary>
        private static Dictionary<int, List<string>> _userWiseDefaultWorkspaceInformation = new Dictionary<int, List<string>>();

        /// <summary>
        /// Locker object
        /// </summary>
        private static readonly Object _lock = new Object();

        /// <summary>
        /// The singilton instance
        /// </summary>
        private static OpenfinDataManagerService _openfinDataManagerService = null;
        /// <summary>
        /// Singilton instance
        /// </summary>
        /// <returns></returns>
        public static OpenfinDataManagerService GetInstance()
        {
            lock (_lock)
            {
                if (_openfinDataManagerService == null)
                    _openfinDataManagerService = new OpenfinDataManagerService();
                return _openfinDataManagerService;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        private OpenfinDataManagerService()
        {
            try
            {
                #region SubscribeAndConsume
                //KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_GetOpenfinWorkspaceLayoutRequest, KafkaManager_GetUserSpecificWorkspaceInformation);
                //KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_SaveOpenfinWorkspaceLayoutRequest, KafkaManager_SaveUserSpecificWorkspaceInformation);
                //KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_DeleteOpenfinWorkspaceLayoutRequest, KafkaManager_DeleteUserSpecificWorkspaceInformation);
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
        /// KafkaManager_GetUserSpecificWorkspaceInformation - This Method will fetch all the user related workspaces and populate the dictionary.
        /// </summary>        
        //TODO : Future improvment - Move the Database related code to a separate class
        public static void CreateUserWiseWorkspaceInformation()
        {
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = CommonDataConstants.CONST_P_Samsara_GetOpenfinWorkspaceInfo;
               
                int userID = default;
                String row = String.Empty;
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        if (reader[0] != DBNull.Value && reader[1] != DBNull.Value)
                        {
                            userID = Convert.ToInt16(reader[0]);
                            row = reader[1].ToString();
                        }

                        if (!_userWiseDefaultWorkspaceInformation.ContainsKey(userID))
                        {
                            if (!String.IsNullOrEmpty(row))
                            {
                                List<string> userWiseWorkspaceList = new List<string>() { row };
                                _userWiseDefaultWorkspaceInformation.Add(userID, userWiseWorkspaceList);
                            }
                        }
                        else
                        {
                            if (!String.IsNullOrEmpty(row))
                            {
                                List<string> userWiseWorkspaceList = new List<string>();
                                userWiseWorkspaceList = _userWiseDefaultWorkspaceInformation[userID];
                                userWiseWorkspaceList.Add(row);
                                _userWiseDefaultWorkspaceInformation[userID] = userWiseWorkspaceList;
                            }
                        }
                    }
                }
                Logger.LoggerWrite(CommonDataConstants.MSG_CacheUpdated + JsonConvert.SerializeObject(_userWiseDefaultWorkspaceInformation));
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// KafkaManager_GetUserSpecificWorkspaceInformation - This Method will check if dictionary _userWiseDefaultWorkspaceInformation contains workspace information for the userID and return the same with Success : true . Otherwise returns Success : false
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="request"></param>
        private async void KafkaManager_GetUserSpecificWorkspaceInformation(string topic, RequestResponseModel message)
        {
            try
            {
                Logger.LoggerWrite(CommonDataConstants.MSG_GetOpenfinDefaultWorkspaceInformationReceived + message.CompanyUserID + JsonConvert.SerializeObject(message));
                int companyUserId = message.CompanyUserID;

                if (_userWiseDefaultWorkspaceInformation != null && _userWiseDefaultWorkspaceInformation.ContainsKey(companyUserId))
                {
                    message.Data = JsonHelper.SerializeObject(_userWiseDefaultWorkspaceInformation[companyUserId]);
                }

                await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_GetOpenfinWorkspaceLayoutResponse, message);
                Logger.LoggerWrite(CommonDataConstants.MSG_GetOpenfinDefaultWorkspaceInformationProcessed + message.CompanyUserID);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }


        /// <summary>
        /// KafkaManager_SaveUserSpecificWorkspaceInformation - This Method will be responsible for saving/updating the workspace information in the Database and Cache
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="request"></param>
        private async void KafkaManager_SaveUserSpecificWorkspaceInformation(string topic, RequestResponseModel message)
        {
            try
            {
                Logger.LoggerWrite(CommonDataConstants.MSG_SaveOpenfinDefaultWorkspaceInformationReceived + message.CompanyUserID + JsonConvert.SerializeObject(message));
                string workspaceInformationToSave = message.Data;
                List<string> workspaceInfo = new List<string>();
                workspaceInfo.Add(workspaceInformationToSave);
                string errorMessage = string.Empty;
                dynamic jsonDataObject = JsonConvert.DeserializeObject<dynamic>(message.Data);
                try
                {
                    // Saving the Updated Workspace Information in DB
                    object[] parameters = new object[4];
                    parameters[0] = message.CompanyUserID;
                    parameters[1] = workspaceInformationToSave;
                    parameters[2] = jsonDataObject.workspaceName;
                    parameters[3] = jsonDataObject.workspaceId;
                    DatabaseManager.DatabaseManager.ExecuteNonQuery(CommonDataConstants.CONST_P_Samsara_SaveOpenfinWorkspaceInfo, parameters);

                    // Saving/Updating Information in Dictionary
                    if (_userWiseDefaultWorkspaceInformation.ContainsKey(message.CompanyUserID))
                    {
                        List<string> workspaceInfoList = _userWiseDefaultWorkspaceInformation[message.CompanyUserID];
                        List<string> updatedWorkspaceInfoList = new List<string>();

                        foreach (string workspace in workspaceInfoList)
                        {
                            dynamic data = JsonConvert.DeserializeObject<dynamic>(workspace);
                            if (data.workspaceName == jsonDataObject.workspaceName)
                            {
                                continue;
                            }
                            updatedWorkspaceInfoList.Add(workspace);
                        }
                        updatedWorkspaceInfoList.Add(workspaceInformationToSave);
                        _userWiseDefaultWorkspaceInformation[message.CompanyUserID] = updatedWorkspaceInfoList;
                    }
                    else
                    {
                        _userWiseDefaultWorkspaceInformation.Add(message.CompanyUserID, workspaceInfo);
                    }
                    Logger.LoggerWrite(CommonDataConstants.MSG_CacheUpdatedWithData + JsonConvert.SerializeObject(_userWiseDefaultWorkspaceInformation[message.CompanyUserID]));
                }
                catch (Exception ex1)
                {
                    errorMessage = ex1.Message;
                    Logger.LoggerWrite(CommonDataConstants.MSG_ExceptionThrown + JsonConvert.SerializeObject(ex1));
                }

                message.Data = JsonHelper.SerializeObject(errorMessage);
                await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_SaveOpenfinWorkspaceLayoutResponse, message);
                Logger.LoggerWrite(CommonDataConstants.MSG_SaveOpenfinDefaultWorkspaceInformationRequestProcessed + JsonConvert.SerializeObject(message));
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// KafkaManager_DeleteUserSpecificWorkspaceInformation - This Method will be responsible for deleting the workspace information in the Database and Cache
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="request"></param>
        private async void KafkaManager_DeleteUserSpecificWorkspaceInformation(string topic, RequestResponseModel message)
        {
            try
            {
                Logger.LoggerWrite(CommonDataConstants.MSG_DeleteOpenfinDefaultWorkspaceInformationReceived + message.CompanyUserID + JsonConvert.SerializeObject(message));
                string errorMessage = string.Empty;
                dynamic jsonDataObject = JsonConvert.DeserializeObject<dynamic>(message.Data);
                try
                {
                    object[] parameters = new object[2];
                    parameters[0] = message.CompanyUserID;
                    parameters[1] = jsonDataObject.workspaceId;
                    if (_userWiseDefaultWorkspaceInformation.ContainsKey(message.CompanyUserID))
                    {
                        List<string> workspaceInfoList = _userWiseDefaultWorkspaceInformation[message.CompanyUserID];
                        List<string> updatedWorkspaceInfoList = new List<string>();

                        // Deleting the workspace information from DB
                        DatabaseManager.DatabaseManager.ExecuteNonQuery(CommonDataConstants.CONST_P_Samsara_DeleteOpenfinWorkspaceInfo, parameters);
                        foreach (string workspace in workspaceInfoList)
                        {
                            dynamic data = JsonConvert.DeserializeObject<dynamic>(workspace);
                            if (data.workspaceId == jsonDataObject.workspaceId)
                            {
                                continue;
                            }
                            updatedWorkspaceInfoList.Add(workspace);
                        }
                        // Deleting the workspace information from cache
                        _userWiseDefaultWorkspaceInformation[message.CompanyUserID] = updatedWorkspaceInfoList;
                    }
                    Logger.LoggerWrite(CommonDataConstants.MSG_CacheUpdatedWithData + JsonConvert.SerializeObject(_userWiseDefaultWorkspaceInformation[message.CompanyUserID]));
                }
                catch (Exception ex1)
                {
                    errorMessage = ex1.Message;
                    Logger.LoggerWrite(CommonDataConstants.MSG_ExceptionThrown + JsonConvert.SerializeObject(ex1));
                }

                message.Data = JsonHelper.SerializeObject(errorMessage);
                await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_DeleteOpenfinWorkspaceLayoutResponse, message);
                InformationReporter.GetInstance.Write(CommonDataConstants.MSG_DeleteOpenfinWorkspaceInformationProcessed + JsonConvert.SerializeObject(message));
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }
    }
}
