using Prana.CalculationService.Constants;
using Prana.CalculationService.Models;
using Prana.DatabaseManager;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
namespace Prana.CalculationService
{
    public class DbManager
    {
        #region SingletonInstance

        /// <summary>
        /// Locker object
        /// </summary>
        private static readonly Object _lock = new Object();

        /// <summary>
        /// The singleton instance
        /// </summary>
        private static DbManager _dbManager = null;
        /// <summary>
        /// Singleton instance
        /// </summary>
        /// <returns></returns>
        public static DbManager GetInstance()
        {
            lock (_lock)
            {
                if (_dbManager == null)
                    _dbManager = new DbManager();
                return _dbManager;
            }
        }
        #endregion

        /// <summary>
        /// Gets Widget data for CompanyUserId.
        /// </summary>
        /// <param name="companyUserId"></param>
        public List<RtpnlResponse> GetWidgetData(int userID)
        {
            List<RtpnlResponse> widgetConfigData = new List<RtpnlResponse>();

            QueryData queryData = new QueryData
            {
                StoredProcedureName = RtpnlConstants.CONST_P_RTPNL_GetUserWidgetConfigDetails,
                CommandTimeout = 200
            };

            queryData.DictionaryDatabaseParameter.Add(RtpnlConstants.CONST_UserID, new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = RtpnlConstants.CONST_UserID,
                ParameterType = DbType.Int32,
                ParameterValue = userID
            });

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);

                        RtpnlResponse configDTO = FillWidgetDetails(reader);
                        widgetConfigData.Add(configDTO);
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
            return widgetConfigData;
        }

        /// <summary>
        /// Populates an instance of the RtpnlResponse class with data from an IDataReader.
        /// </summary>
        /// <param name="reader"></param>
        private RtpnlResponse FillWidgetDetails(IDataReader reader)
        {
            RtpnlResponse widgetDTO = new RtpnlResponse();
            try
            {
                widgetDTO.pageId = reader.GetString(reader.GetOrdinal(RtpnlConstants.CONST_PageId));
                widgetDTO.widgetName = reader.GetString(reader.GetOrdinal(RtpnlConstants.CONST_WidgetName));
                widgetDTO.widgetType = reader.GetString(reader.GetOrdinal(RtpnlConstants.CONST_WidgetType));
                widgetDTO.defaultColumns = reader.GetString(reader.GetOrdinal(RtpnlConstants.CONST_DefaultColumns));
                widgetDTO.coloredColumns = reader.GetString(reader.GetOrdinal(RtpnlConstants.CONST_ColoredColumns));
                widgetDTO.graphType = reader.GetString(reader.GetOrdinal(RtpnlConstants.CONST_GraphType));
                widgetDTO.isFlashColorEnabled = reader.GetBoolean(reader.GetOrdinal(RtpnlConstants.CONST_IsFlashColorEnabled));
                widgetDTO.channelDetail = reader.GetString(reader.GetOrdinal(RtpnlConstants.CONST_ChannelDetail));
                widgetDTO.linkedWidget = reader.GetString(reader.GetOrdinal(RtpnlConstants.CONST_LinkedWidget));
                widgetDTO.viewName = reader.GetString(reader.GetOrdinal(RtpnlConstants.CONST_ViewName));
                widgetDTO.widgetId = reader.GetString(reader.GetOrdinal(RtpnlConstants.CONST_WidgetKey));
                widgetDTO.primaryMetric = reader.IsDBNull(reader.GetOrdinal(RtpnlConstants.CONST_PrimaryMetric)) ? null : reader.GetString(reader.GetOrdinal(RtpnlConstants.CONST_PrimaryMetric));
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return widgetDTO;
        }

        /// <summary>
        /// Delete the unsaved widgets from Views 
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="viewName"></param>
        /// <param name="savedWidgets"></param>
        /// <param name="pageId"></param>
        public void DeleteRemovedWidgetsFromView(int userID, string viewName, string removedWidgets, string pageId)
        {
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = RtpnlConstants.CONST_P_DeleteUnsavedWidgetsFromView;
                queryData.CommandTimeout = 200;

                queryData.DictionaryDatabaseParameter.Add(RtpnlConstants.CONST_UserID, new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = RtpnlConstants.CONST_UserID,
                    ParameterType = DbType.Int32,
                    ParameterValue = userID
                });

                queryData.DictionaryDatabaseParameter.Add(RtpnlConstants.CONST_AT_ViewName, new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = RtpnlConstants.CONST_AT_ViewName,
                    ParameterType = DbType.String,
                    ParameterValue = viewName
                });

                queryData.DictionaryDatabaseParameter.Add(RtpnlConstants.CONST_AT_Removed_Widgets, new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = RtpnlConstants.CONST_AT_Removed_Widgets,
                    ParameterType = DbType.String,
                    ParameterValue = removedWidgets
                });
                queryData.DictionaryDatabaseParameter.Add(RtpnlConstants.CONST_AT_PageId, new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = RtpnlConstants.CONST_AT_PageId,
                    ParameterType = DbType.String,
                    ParameterValue = pageId
                });
                DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);
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
        /// Renames the view Name for already created widgets when name in layout is renamed
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="oldTitle"></param>
        /// <param name="newTitle"></param>
        public int UpdateViewNameforWidgets(int userID, string oldTitle, string newTitle, string pageId)
        {
            int rowsAffected = 0;
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = RtpnlConstants.CONST_P_UpdateViewNameWidgetConfigDetails;
                queryData.CommandTimeout = 200;

                queryData.DictionaryDatabaseParameter.Add(RtpnlConstants.CONST_UserID, new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = RtpnlConstants.CONST_UserID,
                    ParameterType = DbType.Int32,
                    ParameterValue = userID
                });

                queryData.DictionaryDatabaseParameter.Add(RtpnlConstants.CONST_AT_OldViewName, new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = RtpnlConstants.CONST_AT_OldViewName,
                    ParameterType = DbType.String,
                    ParameterValue = oldTitle
                });

                queryData.DictionaryDatabaseParameter.Add(RtpnlConstants.CONST_AT_NewViewName, new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = RtpnlConstants.CONST_AT_NewViewName,
                    ParameterType = DbType.String,
                    ParameterValue = newTitle
                });

                queryData.DictionaryDatabaseParameter.Add(RtpnlConstants.CONST_PageID, new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = RtpnlConstants.CONST_PageID,
                    ParameterType = DbType.String,
                    ParameterValue = pageId
                });

                rowsAffected = DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return rowsAffected;
        }

        /// <summary>
        /// Saves the channel linking colour from Widget header
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="widgetKeys"></param>
        /// <param name="channelDetails"></param>
        public int UpdateChannelColourfromHeaderforWidgets(string widgetKeys, string channelDetails, int userID)
        {
            int rowsAffected = 0;
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = RtpnlConstants.CONST_P_SavedcolourDetailsFromHeaderForWidgets;
                queryData.CommandTimeout = 200;

                queryData.DictionaryDatabaseParameter.Add(RtpnlConstants.CONST_UserID, new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = RtpnlConstants.CONST_UserID,
                    ParameterType = DbType.Int32,
                    ParameterValue = userID
                });

                queryData.DictionaryDatabaseParameter.Add(RtpnlConstants.CONST_AT_OldViewName, new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = RtpnlConstants.CONST_AT_WidgetKeys,
                    ParameterType = DbType.String,
                    ParameterValue = widgetKeys
                });

                queryData.DictionaryDatabaseParameter.Add(RtpnlConstants.CONST_AT_NewViewName, new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = RtpnlConstants.CONST_AT_ChannelDetails,
                    ParameterType = DbType.String,
                    ParameterValue = channelDetails
                });

                rowsAffected = DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return rowsAffected;
        }

        /// <summary>
        /// Save/Update the configuration details in database.
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="data"></param>
        public int SaveUpdateWidgetData(int userID, dynamic data)
        {
            int rowsAffected = 0;
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = RtpnlConstants.CONST_P_RTPNL_SaveUserWidgetConfigDetails;
                queryData.CommandTimeout = 200;

                queryData.DictionaryDatabaseParameter.Add(RtpnlConstants.CONST_UserID, new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = RtpnlConstants.CONST_UserID,
                    ParameterType = DbType.Int32,
                    ParameterValue = userID
                });

                queryData.DictionaryDatabaseParameter.Add(RtpnlConstants.CONST_AT_PageId, new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = RtpnlConstants.CONST_AT_PageId,
                    ParameterType = DbType.String,
                    ParameterValue = data.pageId ?? Guid.NewGuid().ToString()
                });

                queryData.DictionaryDatabaseParameter.Add(RtpnlConstants.CONST_AT_ViewName, new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = RtpnlConstants.CONST_AT_ViewName,
                    ParameterType = DbType.String,
                    ParameterValue = data.viewName
                });

                queryData.DictionaryDatabaseParameter.Add(RtpnlConstants.CONST_AT_WidgetName, new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = RtpnlConstants.CONST_AT_WidgetName,
                    ParameterType = DbType.String,
                    ParameterValue = data.widgetName
                });
                queryData.DictionaryDatabaseParameter.Add(RtpnlConstants.CONST_AT_WidgetType, new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = RtpnlConstants.CONST_AT_WidgetType,
                    ParameterType = DbType.String,
                    ParameterValue = data.widgetType
                });
                queryData.DictionaryDatabaseParameter.Add(RtpnlConstants.CONST_AT_DefaultColumns, new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = RtpnlConstants.CONST_AT_DefaultColumns,
                    ParameterType = DbType.String,
                    ParameterValue = data.defaultColumns
                });
                queryData.DictionaryDatabaseParameter.Add(RtpnlConstants.CONST_AT_ColoredColumns, new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = RtpnlConstants.CONST_AT_ColoredColumns,
                    ParameterType = DbType.String,
                    ParameterValue = data.coloredColumns
                });
                queryData.DictionaryDatabaseParameter.Add(RtpnlConstants.CONST_AT_GraphType, new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = RtpnlConstants.CONST_AT_GraphType,
                    ParameterType = DbType.String,
                    ParameterValue = data.graphType
                });
                queryData.DictionaryDatabaseParameter.Add(RtpnlConstants.CONST_AT_IsFlashColorEnabled, new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = RtpnlConstants.CONST_AT_IsFlashColorEnabled,
                    ParameterType = DbType.Boolean,
                    ParameterValue = data.isFlashColorEnabled
                });
                queryData.DictionaryDatabaseParameter.Add(RtpnlConstants.CONST_AT_ChannelDetail, new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = RtpnlConstants.CONST_AT_ChannelDetail,
                    ParameterType = DbType.String,
                    ParameterValue = data.channelDetail
                });
                queryData.DictionaryDatabaseParameter.Add(RtpnlConstants.CONST_AT_LinkedWidget, new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = RtpnlConstants.CONST_AT_LinkedWidget,
                    ParameterType = DbType.String,
                    ParameterValue = data.linkedWidget
                });
                queryData.DictionaryDatabaseParameter.Add(RtpnlConstants.CONST_AT_WidgetKey, new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = RtpnlConstants.CONST_AT_WidgetKey,
                    ParameterType = DbType.String,
                    ParameterValue = data.widgetId
                });
                queryData.DictionaryDatabaseParameter.Add(RtpnlConstants.CONST_AT_PrimaryMetric, new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = RtpnlConstants.CONST_AT_PrimaryMetric,
                    ParameterType = DbType.String,
                    ParameterValue = data.primaryMetric
                });

                rowsAffected = DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return rowsAffected;
        }

        /// <summary>
        /// Save/Update the OpenfinPageInfo in database.
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="data"></param>
        public int SaveUpdateOpenfinPageInfo(int userID, OpenFinPageInfo data)
        {
            int rowsAffected = 0;
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = RtpnlConstants.CONST_P_Samsara_SaveOpenfinPageInfo;
                queryData.CommandTimeout = 200;

                queryData.DictionaryDatabaseParameter.Add(RtpnlConstants.CONST_UserID, new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = RtpnlConstants.CONST_UserID,
                    ParameterType = DbType.Int32,
                    ParameterValue = userID
                });

                queryData.DictionaryDatabaseParameter.Add(RtpnlConstants.CONST_AT_PageName, new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = RtpnlConstants.CONST_AT_PageName,
                    ParameterType = DbType.String,
                    ParameterValue = data.pageName
                });

                queryData.DictionaryDatabaseParameter.Add(RtpnlConstants.CONST_AT_OldPageName, new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = RtpnlConstants.CONST_AT_OldPageName,
                    ParameterType = DbType.String,
                    ParameterValue = data.oldPageName
                });
                queryData.DictionaryDatabaseParameter.Add(RtpnlConstants.CONST_AT_PageTag, new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = RtpnlConstants.CONST_AT_PageTag,
                    ParameterType = DbType.String,
                    ParameterValue = data.pageTag
                });
                queryData.DictionaryDatabaseParameter.Add(RtpnlConstants.CONST_AT_PageLayout, new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = RtpnlConstants.CONST_AT_PageLayout,
                    ParameterType = DbType.String,
                    ParameterValue = data.pageLayout
                });
                queryData.DictionaryDatabaseParameter.Add(RtpnlConstants.CONST_AT_PageId, new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = RtpnlConstants.CONST_AT_PageId,
                    ParameterType = DbType.String,
                    ParameterValue = data.pageId
                });

                rowsAffected = DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return rowsAffected;
        }

        /// <summary>
        /// Populates an instance of the OpenFinPageInfo class with data from an IDataReader.
        /// </summary>
        /// <param name="reader"></param>
        private OpenFinPageInfo FillOpenfinPageInfo(IDataReader reader)
        {
            OpenFinPageInfo OpenFinPageInfoDTO = new OpenFinPageInfo();
            try
            {
                if (reader[RtpnlConstants.CONST_PageId] == DBNull.Value)
                {
                    return null;
                }

                if (reader[RtpnlConstants.CONST_PageName] == DBNull.Value)
                {
                    return null;
                }

                if (reader[RtpnlConstants.CONST_PageLayout] == DBNull.Value)
                {
                    return null;
                }

                OpenFinPageInfoDTO.pageName = reader.GetString(reader.GetOrdinal(RtpnlConstants.CONST_PageName));
                OpenFinPageInfoDTO.pageId = reader.GetString(reader.GetOrdinal(RtpnlConstants.CONST_PageId));
                OpenFinPageInfoDTO.pageLayout = reader.GetString(reader.GetOrdinal(RtpnlConstants.CONST_PageLayout));
                OpenFinPageInfoDTO.pageTag = reader.GetString(reader.GetOrdinal(RtpnlConstants.CONST_PageTag));
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return OpenFinPageInfoDTO;
        }

        /// <summary>
        /// Gets Openfin Page Information for CompanyUserId.
        /// </summary>
        /// <param name="companyUserId"></param>
        public List<OpenFinPageInfo> GetOpenfinPageInfo(int userID)
        {
            List<OpenFinPageInfo> openfinPageInfo = new List<OpenFinPageInfo>();

            QueryData queryData = new QueryData
            {
                StoredProcedureName = RtpnlConstants.CONST_P_Samsara_GetOpenfinPageInfo,
                CommandTimeout = 200
            };

            queryData.DictionaryDatabaseParameter.Add(RtpnlConstants.CONST_UserID, new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = RtpnlConstants.CONST_UserID,
                ParameterType = DbType.Int32,
                ParameterValue = userID
            });

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);

                        OpenFinPageInfo OpenFinPageInfoDTO = FillOpenfinPageInfo(reader);
                        if (OpenFinPageInfoDTO != null)
                        {
                            openfinPageInfo.Add(OpenFinPageInfoDTO);
                        }
                        else
                        {
                            InformationReporter.GetInstance.Write(RtpnlConstants.Msg_Error_DB_Null_OpenFinPageInfoDTO);
                        }
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
            return openfinPageInfo;
        }

        /// <summary>
        /// Populates an instance of the InternalPageInfo class with data from an IDataReader.
        /// </summary>
        /// <param name="reader"></param>
        private InternalPageInfo FillInternalPageInfo(IDataReader reader)
        {
            InternalPageInfo InternalPageInfoDTO = new InternalPageInfo();
            try
            {
                if (reader[RtpnlConstants.CONST_PageId] == DBNull.Value)
                {
                    return null;
                }

                if (reader[RtpnlConstants.CONST_PageLayout] == DBNull.Value)
                {
                    return null;
                }

                if (reader[RtpnlConstants.CONST_PageName] == DBNull.Value)
                {
                    return null;
                }

                InternalPageInfoDTO.pageId = reader.GetString(reader.GetOrdinal(RtpnlConstants.CONST_PageId));
                InternalPageInfoDTO.description = reader.GetString(reader.GetOrdinal(RtpnlConstants.CONST_Description));
                byte[] fileDataBytes = (byte[])reader[RtpnlConstants.CONST_FileData];
                InternalPageInfoDTO.layout = Encoding.UTF8.GetString(fileDataBytes);
                InternalPageInfoDTO.pageLayout = reader.GetString(reader.GetOrdinal(RtpnlConstants.CONST_PageLayout));
                InternalPageInfoDTO.pageName = reader.GetString(reader.GetOrdinal(RtpnlConstants.CONST_PageName));
                InternalPageInfoDTO.pageTag = reader.GetString(reader.GetOrdinal(RtpnlConstants.CONST_PageTag));
                InternalPageInfoDTO.viewId = reader.GetString(reader.GetOrdinal(RtpnlConstants.CONST_ViewId));
                int index = reader.GetString(reader.GetOrdinal(RtpnlConstants.CONST_FileName)).IndexOf(RtpnlConstants.CONST_RTPNL_VIEW_LAYOUT);
                if (index != -1)
                {
                    InternalPageInfoDTO.title = reader.GetString(reader.GetOrdinal(RtpnlConstants.CONST_FileName)).Substring(0, index);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return InternalPageInfoDTO;
        }

        /// <summary>
        /// Gets Internal Page (Views) Information for CompanyUserId.
        /// </summary>
        /// <param></param>
        public List<InternalPageInfo> GetInternalPageInfo(int userID, char separator1, char separator2, string moduleName)
        {
            List<InternalPageInfo> internalPageInfo = new List<InternalPageInfo>();

            QueryData queryData = new QueryData
            {
                StoredProcedureName = RtpnlConstants.CONST_P_Samsara_GetCompanyUserLayouts,
                CommandTimeout = 200
            };

            queryData.DictionaryDatabaseParameter.Add(RtpnlConstants.CONST_UserID, new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = RtpnlConstants.CONST_UserID,
                ParameterType = DbType.Int32,
                ParameterValue = userID
            });

            queryData.DictionaryDatabaseParameter.Add("@fileNameTimeStampPair", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@fileNameTimeStampPair",
                ParameterType = DbType.String,
                ParameterValue = ""
            });

            queryData.DictionaryDatabaseParameter.Add("@seperator1", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@seperator1",
                ParameterType = DbType.String,
                ParameterValue = separator1
            });

            queryData.DictionaryDatabaseParameter.Add("@seperator2", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@seperator2",
                ParameterType = DbType.String,
                ParameterValue = separator2
            });

            queryData.DictionaryDatabaseParameter.Add("@moduleName", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@moduleName",
                ParameterType = DbType.String,
                ParameterValue = "RTPNL"
            });

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);

                        InternalPageInfo InternalPageInfoDTO = FillInternalPageInfo(reader);
                        if (InternalPageInfoDTO != null)
                        {
                            internalPageInfo.Add(InternalPageInfoDTO);
                        }
                        else
                        {
                            InformationReporter.GetInstance.Write(RtpnlConstants.Msg_Error_DB_Null_InternalPageInfoDTO);
                        }
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
            return internalPageInfo;
        }
    }
}