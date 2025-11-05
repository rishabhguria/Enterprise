using Prana.DatabaseManager;
using Prana.LayoutService.Models;
using Prana.LayoutService.Utility;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prana.LayoutService
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
        /// Getting the page info from Database
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Dictionary<string, PageInfo> GetPageInfo(int userId)
        {
            Dictionary<string, PageInfo> pageInfoDict = new Dictionary<string, PageInfo>();

            QueryData queryData = new QueryData
            {
                StoredProcedureName = LayoutServiceConstants.CONST_P_Samsara_GetPageInfo,
                CommandTimeout = 200
            };

            queryData.DictionaryDatabaseParameter.Add(LayoutServiceConstants.CONST_UserID, new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = LayoutServiceConstants.CONST_UserID,
                ParameterType = DbType.Int32,
                ParameterValue = userId
            });

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);

                        PageInfo PageInfoDTO = FillPageInfo(reader);
                        if (PageInfoDTO != null)
                        {
                            if (!string.IsNullOrEmpty(PageInfoDTO.pageId))
                            {
                                pageInfoDict[PageInfoDTO.pageId] = PageInfoDTO;
                            }
                            else
                            {
                                InformationReporter.GetInstance.Write(LayoutServiceConstants.Msg_Error_DB_Null_PageInfoDTO);
                            }
                        }
                        else
                        {
                            InformationReporter.GetInstance.Write(LayoutServiceConstants.Msg_Error_DB_Null_PageInfoDTO);
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
            return pageInfoDict;
        }

        public Dictionary<string, ViewInfo> GetViewInfo(int userId)
        {
            Dictionary<string, ViewInfo> viewInfoDict = new Dictionary<string, ViewInfo>();

            try
            {
                QueryData queryData = new QueryData
                {
                    StoredProcedureName = LayoutServiceConstants.CONST_P_Samsara_GetCompanyUserLayouts,
                    CommandTimeout = 200
                };

                queryData.DictionaryDatabaseParameter.Add(LayoutServiceConstants.CONST_UserID, new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = LayoutServiceConstants.CONST_UserID,
                    ParameterType = DbType.Int32,
                    ParameterValue = userId
                });                

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);

                        ViewInfo viewInfoDTO = FillInternalViewInfo(reader);
                        if (viewInfoDTO != null)
                        {
                            viewInfoDict.Add(viewInfoDTO.viewId,viewInfoDTO);
                        }
                        else
                        {
                            InformationReporter.GetInstance.Write(LayoutServiceConstants.Msg_Error_DB_Null_InternalPageInfoDTO);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return viewInfoDict;
        }

        private PageInfo FillPageInfo(IDataReader reader)
        {
            PageInfo PageInfoDTO = new PageInfo();
            try
            {
                if (reader[LayoutServiceConstants.CONST_PageId] == DBNull.Value)
                {
                    return null;
                }

                if (reader[LayoutServiceConstants.CONST_PageName] == DBNull.Value)
                {
                    return null;
                }

                if (reader[LayoutServiceConstants.CONST_PageLayout] == DBNull.Value)
                {
                    return null;
                }

                PageInfoDTO.pageName = reader.GetString(reader.GetOrdinal(LayoutServiceConstants.CONST_PageName));
                PageInfoDTO.pageId = reader.GetString(reader.GetOrdinal(LayoutServiceConstants.CONST_PageId));
                int pageLayoutOrdinal = reader.GetOrdinal(LayoutServiceConstants.CONST_PageLayout);
                // Read the binary data for pageLayout
                byte[] compressedLayoutData = (byte[])reader.GetValue(pageLayoutOrdinal);
                // Decompress the binary data to a string
                PageInfoDTO.pageLayout = DataCompressor.DecompressData(compressedLayoutData);
                PageInfoDTO.pageTag = reader.GetString(reader.GetOrdinal(LayoutServiceConstants.CONST_PageTag));
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return PageInfoDTO;
        }

        /// <summary>
        /// Populates an instance of the InternalPageInfo class with data from an IDataReader.
        /// </summary>
        /// <param name="reader"></param>
        private ViewInfo FillInternalViewInfo(IDataReader reader)
        {
            ViewInfo InternalViewInfoDTO = new ViewInfo();
            try
            {
                if (reader[LayoutServiceConstants.CONST_ViewId] == DBNull.Value)
                {
                    return null;
                }

                InternalViewInfoDTO.viewId = reader.GetString(reader.GetOrdinal(LayoutServiceConstants.CONST_ViewId));
                InternalViewInfoDTO.viewName = reader.GetString(reader.GetOrdinal(LayoutServiceConstants.CONST_ViewName));
                byte[] viewLayoutBytes = (byte[])reader[LayoutServiceConstants.CONST_ViewLayout];
                InternalViewInfoDTO.viewLayout = System.Text.Encoding.UTF8.GetString(viewLayoutBytes);

                int viewLayoutOrdinal = reader.GetOrdinal(LayoutServiceConstants.CONST_ViewLayout);
                // Read the binary data for pageLayout
                byte[] compressedLayoutData = (byte[])reader.GetValue(viewLayoutOrdinal);
                // Decompress the binary data to a string
                InternalViewInfoDTO.viewLayout = DataCompressor.DecompressData(compressedLayoutData);

                InternalViewInfoDTO.description = reader.GetString(reader.GetOrdinal(LayoutServiceConstants.CONST_Description));
                InternalViewInfoDTO.moduleName = reader.GetString(reader.GetOrdinal(LayoutServiceConstants.CONST_ModuleName));
                
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return InternalViewInfoDTO;
        }

        /// <summary>
        /// Save/Update the OpenfinPageInfo in database.
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="data"></param>
        public int SaveUpdatePageInfo(int userID, PageInfo data)
        {
            int rowsAffected = 0;
            try
            {
                //compressing the string data into byte[] format
                byte[] compressedLayoutData = DataCompressor.CompressData(data.pageLayout);
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = LayoutServiceConstants.CONST_P_Samsara_SavePageInfo;
                queryData.CommandTimeout = 200;

                queryData.DictionaryDatabaseParameter.Add(LayoutServiceConstants.CONST_UserID, new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = LayoutServiceConstants.CONST_UserID,
                    ParameterType = DbType.Int32,
                    ParameterValue = userID
                });

                queryData.DictionaryDatabaseParameter.Add(LayoutServiceConstants.CONST_AT_PageName, new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = LayoutServiceConstants.CONST_AT_PageName,
                    ParameterType = DbType.String,
                    ParameterValue = data.pageName
                });

                queryData.DictionaryDatabaseParameter.Add(LayoutServiceConstants.CONST_AT_OldPageName, new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = LayoutServiceConstants.CONST_AT_OldPageName,
                    ParameterType = DbType.String,
                    ParameterValue = data.oldPageName
                });
                queryData.DictionaryDatabaseParameter.Add(LayoutServiceConstants.CONST_AT_PageTag, new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = LayoutServiceConstants.CONST_AT_PageTag,
                    ParameterType = DbType.String,
                    ParameterValue = data.pageTag
                });
                queryData.DictionaryDatabaseParameter.Add(LayoutServiceConstants.CONST_AT_PageLayout, new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = LayoutServiceConstants.CONST_AT_PageLayout,
                    ParameterType = DbType.Binary,
                    ParameterValue = compressedLayoutData
                });
                queryData.DictionaryDatabaseParameter.Add(LayoutServiceConstants.CONST_AT_PageId, new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = LayoutServiceConstants.CONST_AT_PageId,
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
        /// Save/Update the OpenfinViewInfo in database.
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="data"></param>
        public int SaveUpdateViewInfo(int userID, ViewInfo data)
        {
            int rowsAffected = 0;
            try
            {
                //compressing the string data into byte[] format
                byte[] compressedLayoutData = DataCompressor.CompressData(data.viewLayout);
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = LayoutServiceConstants.CONST_P_Samsara_SaveCompanyUserLayouts;
                queryData.CommandTimeout = 200;

                queryData.DictionaryDatabaseParameter.Add(LayoutServiceConstants.CONST_UserID, new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = LayoutServiceConstants.CONST_UserID,
                    ParameterType = DbType.Int32,
                    ParameterValue = userID
                });               

                queryData.DictionaryDatabaseParameter.Add(LayoutServiceConstants.CONST_AT_ModuleName, new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = LayoutServiceConstants.CONST_AT_ModuleName,
                    ParameterType = DbType.String,
                    ParameterValue = data.moduleName
                });
                queryData.DictionaryDatabaseParameter.Add(LayoutServiceConstants.CONST_AT_Description, new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = LayoutServiceConstants.CONST_AT_Description,
                    ParameterType = DbType.String,
                    ParameterValue = data.description
                });
                queryData.DictionaryDatabaseParameter.Add(LayoutServiceConstants.CONST_AT_ViewId, new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = LayoutServiceConstants.CONST_AT_ViewId,
                    ParameterType = DbType.String,
                    ParameterValue = data.viewId
                });
                queryData.DictionaryDatabaseParameter.Add(LayoutServiceConstants.CONST_AT_ViewName, new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = LayoutServiceConstants.CONST_AT_ViewName,
                    ParameterType = DbType.String,
                    ParameterValue = data.viewName
                });
                queryData.DictionaryDatabaseParameter.Add(LayoutServiceConstants.CONST_AT_ViewLayout, new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = LayoutServiceConstants.CONST_AT_ViewLayout,
                    ParameterType = DbType.Binary,
                    ParameterValue = compressedLayoutData
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

    }
}
