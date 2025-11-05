using Prana.Global;
using Prana.LogManager;
using System;
using System.Data;

namespace Prana.WatchListData
{
    /// <summary>
    /// This Class contains all the function which relates to databse. 
    /// </summary>
    public class DataBaseOperationHelper
    {
        private static DataBaseOperationHelper _instance;

        public static DataBaseOperationHelper Instance()
        {
            // Uses lazy initialization.

            // Note: this is not thread safe.

            if (_instance == null)
            {
                _instance = new DataBaseOperationHelper();
            }

            return _instance;
        }

        /// <summary>
        /// This static method is use to add the symbol details into DataBase
        /// </summary>
        /// <param name="symbol">string</param>
        /// <param name="tabName">string</param>
        /// <param name="userId">int</param>
        public async void AddSymbolIntoDatabase(string symbol, string tabName, int userId)
        {
            try
            {
                await System.Threading.Tasks.Task.Run(() =>
                {
                    object[] parameter = new object[3];
                    parameter[0] = symbol;
                    parameter[1] = tabName;
                    parameter[2] = userId;

                    DatabaseManager.DatabaseManager.ExecuteNonQuery("P_AddSymbolInTab_WatchList", parameter, ApplicationConstants.PranaConnectionString);
                });
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
        /// This method is use to add the tableName into dataBase
        /// </summary>
        /// <param name="tabName">string</param>
        /// <param name="userId">int</param>
        public async void AddNewTabIntoDatabase(string tabName, int userId)
        {
            try
            {
                await System.Threading.Tasks.Task.Run(() =>
                {

                    object[] parameter = new object[2];
                    parameter[0] = tabName;
                    parameter[1] = userId;

                    DatabaseManager.DatabaseManager.ExecuteNonQuery("P_NewTab_WatchList", parameter, ApplicationConstants.PranaConnectionString);
                });
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
        /// Gets the linked tab.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <returns></returns>
        public string GetLinkedTab(int userID)
        {
            string linkedTabName = string.Empty;
            try
            {
                object[] parameter = new object[1];
                parameter[0] = userID;

                linkedTabName = DatabaseManager.DatabaseManager.ExecuteScalar("P_GetLinkedTab_WatchList", parameter, ApplicationConstants.PranaConnectionString).ToString();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return linkedTabName;
        }

        /// <summary>
        /// Saves the linked tab.
        /// </summary>
        /// <param name="linkedTabindex">The linked tabindex.</param>
        /// <param name="userID">The user identifier.</param>
        public async void SaveLinkedTab(string linkedTabName, int companyUserID)
        {
            try
            {
                await System.Threading.Tasks.Task.Run(() =>
                {
                    object[] parameter = new object[2];
                    parameter[0] = linkedTabName;
                    parameter[1] = companyUserID;

                    DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveLinkedTab_Watchlist", parameter, ApplicationConstants.PranaConnectionString);
                });
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
        /// This method is used to get all the symbol details for specified user.
        /// </summary>
        /// <param name="userId">int</param>
        /// <returns></returns>
        public DataSet GetAllTabsDataForUser(int userId)
        {
            DataSet dataSet = null;
            try
            {
                object[] parameter = new object[1];
                parameter[0] = userId;

                dataSet = DatabaseManager.DatabaseManager.ExecuteDataSet("P_GetAllUserData_WatchList", parameter, ApplicationConstants.PranaConnectionString);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return dataSet;
        }

        /// <summary>
        /// This method is used to remove all the details reladed to symbol.
        /// </summary>
        /// <param name="symbol">string</param>
        /// <param name="userId">int</param>
        public async void RemoveSymbolFromDatabase(string symbol, string tabName, int userId)
        {
            try
            {
                await System.Threading.Tasks.Task.Run(() =>
                {
                    object[] parameter = new object[3];
                    parameter[0] = symbol;
                    parameter[1] = tabName;
                    parameter[2] = userId;

                    DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteSymbol_WatchList", parameter, ApplicationConstants.PranaConnectionString);
                });
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
        /// This method is used to delete all the details of a symbol which related to tab and tab too.
        /// </summary>
        /// <param name="tabName"></param>
        /// <param name="userId"></param>
        public async void RemoveTabFromDatabase(string tabName, int userId)
        {
            try
            {
                await System.Threading.Tasks.Task.Run(() =>
                {
                    object[] parameter = new object[2];
                    parameter[0] = tabName;
                    parameter[1] = userId;

                    DatabaseManager.DatabaseManager.ExecuteNonQuery("P_RemoveTab_WatchList", parameter, ApplicationConstants.PranaConnectionString);
                });
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

        public async void RenameTab(string NewName, string oldName, int companyUserID)
        {
            try
            {
                await System.Threading.Tasks.Task.Run(() =>
                {
                    object[] parameter = new object[3];
                    parameter[0] = NewName;
                    parameter[1] = oldName;
                    parameter[2] = companyUserID;

                    DatabaseManager.DatabaseManager.ExecuteNonQuery("P_RenameTab_Watchlist", parameter, ApplicationConstants.PranaConnectionString);
                });
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
