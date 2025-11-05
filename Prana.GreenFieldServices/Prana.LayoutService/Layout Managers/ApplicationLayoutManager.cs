using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Prana.DatabaseManager;
using Prana.Global.Utilities;
using Prana.KafkaWrapper;
using Prana.KafkaWrapper.Extension.Classes;
using Prana.LayoutService.Contracts;
using Prana.LayoutService.Utility;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Prana.LayoutService.Layout_Managers
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single, UseSynchronizationContext = false)]
    public class ApplicationLayoutManager : ILayoutManager
    {
        #region variables
        /// <summary>
        /// User Wise Dictionary for managing Applicaiton Level Layout
        /// </summary>
        private static Dictionary<int, Dictionary<string,string>> _userWiseApplicationLayoutInfo = new Dictionary<int, Dictionary<string, string>>();


        /// <summary>
        /// Locker object
        /// </summary>
        private static readonly Object _lock = new Object();

        /// <summary>
        /// The singleton instance
        /// </summary>
        private static ApplicationLayoutManager _applicationLayoutManager = null;

        /// <summary>
        /// Singleton instance
        /// </summary>
        /// <returns>ApplicationLayoutManager object</returns>
        public static ApplicationLayoutManager GetInstance()
        {
            lock (_lock)
            {
                if (_applicationLayoutManager == null)
                    _applicationLayoutManager = new ApplicationLayoutManager();
                return _applicationLayoutManager;
            }
        }
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        private ApplicationLayoutManager()
        {
            try
            {
                //Creation of SubcribeAndConsume listeners for application layout management 
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_GetOpenfinWorkspaceLayoutRequest, KafkaManager_GetUserSpecificLayout);

                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_SaveOpenfinWorkspaceLayoutRequest, KafkaManager_SaveUserSpecificLayout);

                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_DeleteOpenfinWorkspaceLayoutRequest, KafkaManager_DeleteUserSpecificLayout);
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
        #endregion


        #region Interface Methods

        /// <summary>
        /// Loads the layout for a logged-in user based on their company user Id.
        /// </summary>
        /// <param name="companyUserID">The Id of the company user.</param>
        public void LoadLayoutForLoggedInUser(int companyUserId)
        {
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = LayoutServiceConstants.CONST_P_Samsara_GetWorkspaceInfo;

                queryData.DictionaryDatabaseParameter.Add(LayoutServiceConstants.CONST_UserID, new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = LayoutServiceConstants.CONST_UserID,
                    ParameterType = DbType.Int32,
                    ParameterValue = companyUserId
                });

                int userID = default;
                byte[] compressedData;
                String decompressedData = String.Empty;
                String layoutId = String.Empty;

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        if (reader[0] != DBNull.Value && reader[1] != DBNull.Value)
                        {
                            userID = Convert.ToInt16(reader[0]);
                            compressedData = (byte[])reader[1];
                            layoutId = reader[3].ToString();

                            // Decompress the compressed data
                            decompressedData = DataCompressor.DecompressData(compressedData);
                        }



                        if (_userWiseApplicationLayoutInfo.ContainsKey(userID))
                        {
                            if (!String.IsNullOrEmpty(decompressedData))
                            {
                                //Entry for this user exists , so adding another entry of layout for the user
                                if (_userWiseApplicationLayoutInfo[userID].ContainsKey(layoutId))
                                {
                                    _userWiseApplicationLayoutInfo[userID][layoutId] = decompressedData;
                                }
                                else
                                {
                                    _userWiseApplicationLayoutInfo[userID].Add(layoutId, decompressedData);
                                }
                            }
                        }
                        else
                        {
                            //Dictionary does not contain entry for this user
                            if (!String.IsNullOrEmpty(decompressedData))
                            {
                                Dictionary<string,string> tempLayoutInfo = new Dictionary<string,string>();
                                tempLayoutInfo.Add(layoutId, decompressedData);
                                //Added Entry for this user.
                                _userWiseApplicationLayoutInfo.Add(userID, tempLayoutInfo);
                            }
                        }
                    }
                }
                Logger.LoggerWrite(LayoutServiceConstants.MSG_ApplicationLayoutCacheUpdated);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
            }
        }

        /// <summary>
        /// Removes the layout from the cache for a logged-in user based on their company user Id.
        /// </summary>
        /// <param name="companyUserID">The Id of the company user.</param>
        public void RemoveLayoutForLoggedOutUser(int companyUserId)
        {
            try
            {
                if (_userWiseApplicationLayoutInfo.ContainsKey(companyUserId))
                {
                    // Remove the entry for the given companyUserId
                    //_userWiseApplicationLayoutInfo.Remove(companyUserId);
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
        /// Saves the user specific layout in the cache.
        /// </summary>
        /// <param name="topic">The topic of the kafka request.</param>
        /// <param name="message">Object contains details related to this request (RequestID ,CompanyUserId ,Data). </param>
        public async void KafkaManager_SaveUserSpecificLayout(string topic, RequestResponseModel message)
        {
            try
            {
                Logger.LoggerWrite(LayoutServiceConstants.MSG_SaveApplicationLayoutRequestReceived + message.CompanyUserID);
                string errorMessage = string.Empty;
                //TODO : After testing remove this dynamic 
                dynamic layoutDataObject = JsonConvert.DeserializeObject<dynamic>(message.Data);
                string worksapceId = layoutDataObject.workspaceId.ToString();
                string workspaceName = layoutDataObject.workspaceName.ToString();
                try
                {
                    //compressing the string data into byte[] format
                    byte[] compressedLayoutData = DataCompressor.CompressData(message.Data.ToString());
                    // Saving the Updated Workspace Information in DB
                    object[] parameters = new object[4];
                    parameters[0] = message.CompanyUserID;
                    parameters[1] = compressedLayoutData;
                    parameters[2] = workspaceName;
                    parameters[3] = worksapceId;

                    DatabaseManager.DatabaseManager.ExecuteNonQuery(LayoutServiceConstants.CONST_P_Samsara_SaveWorkspaceInfo, parameters);

                    // Saving/Updating Information in Dictionary
                    if (_userWiseApplicationLayoutInfo.ContainsKey(message.CompanyUserID))
                    {
                        Dictionary<string,string> dictApplicaitonLayoutInfo = _userWiseApplicationLayoutInfo[message.CompanyUserID];

                        //checking if the workspace Id to store already exists or not
                        if (dictApplicaitonLayoutInfo.ContainsKey(worksapceId))
                        {
                            dictApplicaitonLayoutInfo[worksapceId] = message.Data;
                        }
                        else
                        {
                            //creating entry of this layoutin dictionary
                            dictApplicaitonLayoutInfo.Add(worksapceId, message.Data);
                        }
                    }
                    else
                    {
                        //User level dictionary doesn't exist , so creating an entry for the user
                        Dictionary<string,string> dictLayoutToSave = new Dictionary<string,string>();
                        dictLayoutToSave.Add(worksapceId, message.Data);
                        _userWiseApplicationLayoutInfo.Add(message.CompanyUserID, dictLayoutToSave);
                    }
                    Logger.LoggerWrite(LayoutServiceConstants.MSG_ApplicationLayoutCacheUpdatedWithData);
                }
                catch (Exception ex1)
                {
                    errorMessage = ex1.Message;
                    Logger.LoggerWrite(LayoutServiceConstants.MSG_ExceptionThrown + JsonConvert.SerializeObject(ex1));
                }

                message.Data = JsonHelper.SerializeObject(errorMessage);
                await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_SaveOpenfinWorkspaceLayoutResponse, message);
                Logger.LoggerWrite(LayoutServiceConstants.MSG_SaveApplicationLayoutRequestProcessed + JsonConvert.SerializeObject(message));
            }
            catch (Exception ex)
            {
                await ProduceTopicNHandleException(message, ex, KafkaConstants.TOPIC_SaveOpenfinWorkspaceLayoutResponse);
            }
        }

        /// <summary>
        /// Deletes the user specific layout in the cache.
        /// </summary>
        /// <param name="topic">The topic of the kafka request.</param>
        /// <param name="message">Object contains details related to this request (RequestID ,CompanyUserId ,Data). </param>
        public async void KafkaManager_DeleteUserSpecificLayout(string topic, RequestResponseModel message)
        {
            try
            {
                Logger.LoggerWrite(LayoutServiceConstants.MSG_DeleteApplicationLayoutRequestReceived + message.CompanyUserID + JsonConvert.SerializeObject(message));
                string errorMessage = string.Empty;
                dynamic layoutDataObject = JsonConvert.DeserializeObject<dynamic>(message.Data);
                string worksapceId = layoutDataObject.workspaceId.ToString();
                try
                {
                    object[] parameters = new object[2];
                    parameters[0] = message.CompanyUserID;
                    parameters[1] = worksapceId;
                    if (_userWiseApplicationLayoutInfo.ContainsKey(message.CompanyUserID))
                    {
                        Dictionary<string,string> dictApplicationLayouts = _userWiseApplicationLayoutInfo[message.CompanyUserID];
                        if (dictApplicationLayouts.ContainsKey(worksapceId))
                        {
                            dictApplicationLayouts.Remove(worksapceId);

                            // If the inner dictionary is empty after removal, remove the outer dictionary entry
                            if (dictApplicationLayouts.Count == 0)
                            {
                                _userWiseApplicationLayoutInfo.Remove(message.CompanyUserID);
                            }
                        }
                    }
                    Logger.LoggerWrite(LayoutServiceConstants.MSG_ApplicationLayoutCacheUpdatedWithData + JsonConvert.SerializeObject(_userWiseApplicationLayoutInfo[message.CompanyUserID]));
                }
                catch (Exception ex1)
                {
                    errorMessage = ex1.Message;
                    Logger.LoggerWrite(LayoutServiceConstants.MSG_ExceptionThrown + JsonConvert.SerializeObject(ex1));
                }

                message.Data = JsonHelper.SerializeObject(errorMessage);
                await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_DeleteOpenfinWorkspaceLayoutResponse, message);
                InformationReporter.GetInstance.Write(LayoutServiceConstants.MSG_DeleteApplicationLayoutRequestProcessed + JsonConvert.SerializeObject(message));
            }
            catch (Exception ex)
            {
                await ProduceTopicNHandleException(message, ex, KafkaConstants.TOPIC_DeleteOpenfinWorkspaceLayoutResponse);
            }
        }


        /// <summary>
        /// Removes the data of a deleted page from the user's layout in the cache.
        /// </summary>
        /// <param name="topic">The topic of the Kafka request.</param>
        /// <param name="pageId">The ID of the page to be removed.</param>
        /// <param name="message">Object containing details related to this request (RequestID, CompanyUserID, Data).</param>
        public void RemoveDeletedPageDataFromlayout(string topic, string pageId, RequestResponseModel message)
        {
            try
            {
                // Retrieve the workspace data for the user from teh dictionary
                string workspaceData = _userWiseApplicationLayoutInfo[message.CompanyUserID]["0"];

                // Deserialize the workspace data to a dynamic JSON object
                dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(workspaceData);

                // Get the windows array from the JSON object
                var windowsArray = jsonObject[LayoutServiceConstants.CONST_WORKSPACE_INFO]?[LayoutServiceConstants.CONST_SNAPSHOT]?[LayoutServiceConstants.CONST_WINDOWS] as JArray;

                if (windowsArray != null)
                {
                    JToken windowToRemove = null;

                    // Iterate through the windows array to find the window to remove
                    foreach (var window in windowsArray)
                    {
                        // Check if the window contains the pageId to be removed
                        if ((string)window[LayoutServiceConstants.CONST_WORKSPACE_PLATFORM]?[LayoutServiceConstants.CONST_PAGES][0]?[LayoutServiceConstants.CONST_PAGE_ID] == pageId)
                        {
                            windowToRemove = window;
                            break;
                        }
                    }

                    // If the window to remove is found, remove it from the array
                    if (windowToRemove != null)
                    {
                        windowsArray.Remove(windowToRemove);

                        // Update the JSON object with the modified windows array
                        jsonObject[LayoutServiceConstants.CONST_WORKSPACE_INFO][LayoutServiceConstants.CONST_SNAPSHOT][LayoutServiceConstants.CONST_WINDOWS] = windowsArray;

                        // Convert the updated JSON object to a string
                        string updatedJsonString = jsonObject.ToString();

                        // Create a new RequestResponseModel with the updated layout data
                        RequestResponseModel requestResponseObj = new RequestResponseModel(message.CompanyUserID, updatedJsonString);

                        // Save the updated user-specific layout using KafkaManager
                        KafkaManager_SaveUserSpecificLayout(topic, requestResponseObj);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur during the process
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Gets the user specific layout in the cache.
        /// </summary>
        /// <param name="topic">The topic of the kafka request.</param>
        /// <param name="message">Object contains details related to this request (RequestID ,CompanyUserId ,Data). </param>
        public async void KafkaManager_GetUserSpecificLayout(string topic, RequestResponseModel message)
        {
            try
            {
                Logger.LoggerWrite(LayoutServiceConstants.MSG_GetApplicationLayoutReceived + message.CompanyUserID + JsonConvert.SerializeObject(message));
                int companyUserId = message.CompanyUserID;

                //Checkng the dictionary cache for the user data
                if (_userWiseApplicationLayoutInfo != null && _userWiseApplicationLayoutInfo.ContainsKey(companyUserId))
                {
                    Dictionary<string,string> dictLayoutsForUser = _userWiseApplicationLayoutInfo[companyUserId];
                    //creating list of all layouts of the companyUserId to send
                    List<string> listLayoutsForUser = new List<string>(dictLayoutsForUser.Values);
                    message.Data = JsonHelper.SerializeObject(listLayoutsForUser);
                }

                await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_GetOpenfinWorkspaceLayoutResponse, message);
                Logger.LoggerWrite(LayoutServiceConstants.MSG_GetApplicationLayoutProcessed + message.CompanyUserID);
            }
            catch (Exception ex)
            {
                await ProduceTopicNHandleException(message, ex, KafkaConstants.TOPIC_GetOpenfinWorkspaceLayoutResponse);
            }
        }

        private static async System.Threading.Tasks.Task ProduceTopicNHandleException(
           RequestResponseModel message,
           Exception ex,
           string topicName)
        {
            try
            {
                message.Data = null;
                message.ErrorMsg = $"Error while producing to topic {topicName}, err msg:{ex.Message}";
                await KafkaManager.Instance.Produce(topicName, message);
                Logger.LogError(ex, $"Error while producing to topic {topicName}");
            }
            catch (Exception ex2)
            {
                Logger.LogError(ex2, $"ProduceTopicNHandleException encountered an error,  message might not have been published to event {topicName}");
            }
        }
        #endregion
    }
}
