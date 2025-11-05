#region using

using Prana.LogManager;
using System;
using System.Data;

#endregion

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for RiskUserLevelManager.
    /// </summary>
    public class RiskUserLevelManager
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public RiskUserLevelManager()
        {

        }
        #endregion

        /*USERLEVEL OVERALLLIMITS*/

        #region Basic methods like Save/Get/Fill for UserOverallLimit

        //fill UserOverallLimits

        /// <summary>
        /// The method is used to fill the UserOverallLimits Details.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="offSet"></param>
        /// <returns></returns>
        public static UserLevelOverallLimit FillUserLevelOverallLimit(object[] row, int offSet)
        {
            if (offSet < 0)
            {
                offSet = 0;
            }

            UserLevelOverallLimit _userLevelOverallLimit = null;

            try
            {
                if (row != null)
                {
                    _userLevelOverallLimit = new UserLevelOverallLimit();

                    int rMCompanyUserID = offSet + 0;
                    int companyUserID = offSet + 1;
                    int exposureLimit = offSet + 2;
                    int maximumPNLLoss = offSet + 3;
                    int maximumSizePerOrder = offSet + 4;
                    int maximumSizePerBasket = offSet + 5;
                    int companyID = offSet + 6;
                    //int user = offSet + 7;
                    //int userUIControlsColl = offSet + 8;
                    //int userUIControl = offSet + 9;


                    _userLevelOverallLimit.RMCompanyUserID = int.Parse(row[rMCompanyUserID].ToString());
                    _userLevelOverallLimit.CompanyUserID = int.Parse(row[companyUserID].ToString());
                    _userLevelOverallLimit.UserExposureLimit = int.Parse(row[exposureLimit].ToString());
                    _userLevelOverallLimit.MaximumPNLLoss = int.Parse(row[maximumPNLLoss].ToString());
                    _userLevelOverallLimit.MaximumSizePerOrder = int.Parse(row[maximumSizePerOrder].ToString());
                    _userLevelOverallLimit.MaximumSizePerBasket = int.Parse(row[maximumSizePerBasket].ToString());
                    _userLevelOverallLimit.CompanyID = int.Parse(row[companyID].ToString());
                    //_userLevelOverallLimit.ShortName = row[user].ToString();

                    //_userLevelOverallLimit.userUIControlsColl = GetUserUIControlsperUser(_userLevelOverallLimit.CompanyID,_userLevelOverallLimit.CompanyUserID);

                }

            }
            #region Catch
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {

                    throw;

                }
            }
            #endregion
            return _userLevelOverallLimit;

        }

        //save UserOverallLimits

        /// <summary>
        /// the method is used to save UserOverallLimits details
        /// </summary>
        /// <param name="userLevelOverallLimit"></param>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public static int SaveUserLevelOverallLimit(UserLevelOverallLimit userLevelOverallLimit, int companyID)
        {
            int result = int.MinValue;

            try
            {
                object[] parameter = new object[8];

                parameter[0] = userLevelOverallLimit.RMCompanyUserID;
                parameter[1] = userLevelOverallLimit.CompanyUserID;
                parameter[2] = userLevelOverallLimit.UserExposureLimit;
                parameter[3] = userLevelOverallLimit.MaximumPNLLoss;
                parameter[4] = userLevelOverallLimit.MaximumSizePerOrder;
                parameter[5] = userLevelOverallLimit.MaximumSizePerBasket;
                parameter[6] = companyID;
                parameter[7] = int.MinValue;

                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveUserLevelOverallLimit", parameter).ToString());

            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {

                    throw;

                }
            }
            #endregion
            return result;
        }

        //Get all UserOverallLimits

        /// <summary>
        /// fetching all the existing data to bind in grid
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public static UserLevelOverallLimits GetAllUserLevelOverallLimits(int companyID)
        {
            UserLevelOverallLimits userLevelOverallLimits = new UserLevelOverallLimits();

            Object[] parameter = new object[1];
            parameter[0] = companyID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetAllUserLevelOverallLimits", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        Prana.Admin.BLL.UserLevelOverallLimit _dshfsdh = FillUserLevelOverallLimit(row, 0);

                        //						Prana.Admin.BLL.UserLevelOverallLimits _hhhh = new UserLevelOverallLimits();
                        //						_hhhh.

                        userLevelOverallLimits.Add(_dshfsdh);
                    }
                }
            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {

                    throw;

                }
            }
            #endregion
            return userLevelOverallLimits;
        }

        //Get a single UserOverallLimit object

        /// <summary>
        /// the method is used to fetch a particular user's overalllimits details.
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="companyUserID"></param>
        /// <returns></returns>
        public static UserLevelOverallLimit GetUserLevelOverallLimit(int companyID, int companyUserID)
        {
            UserLevelOverallLimit userLevelOverallLimit = new UserLevelOverallLimit();

            object[] parameter = new object[2];
            parameter[0] = companyID;
            parameter[1] = companyUserID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetUserLevelOverallLimitsbyCompanyUserID", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        userLevelOverallLimit = FillUserLevelOverallLimit(row, 0);
                    }
                }
            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {

                    throw;

                }
            }
            #endregion
            return userLevelOverallLimit;
        }

        //Delete one RM UserOverallLimit

        /// <summary>
        /// the method is used to delete a particular UserOverallLimit
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static bool DeleteUserOverallLimit(int companyId, int userID)
        {
            bool result = false;
            Object[] parameter = new object[2];
            parameter[0] = companyId;
            parameter[1] = userID;

            try
            {
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteUserOverallLimit", parameter) > 0)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {

                    throw;

                }
            }
            #endregion
            return result;
        }

        //Delete all users RM details

        /// <summary>
        ///the method is used to delete all UseroverallLimits for a company.
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public static bool DeleteAllRMUserOverallLimits(int companyID)
        {
            bool result = false;
            Object[] parameter = new object[1];
            parameter[0] = companyID;

            try
            {
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteAllRMUserOverallLimits", parameter) > 0)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {

                    throw;

                }
            }
            #endregion
            return result;
        }
        #endregion

        /*USER UI CONTROL*/

        #region Basic methods like Save/Get/Fill for UserUI

        //FillUserLevelUI

        /// <summary>
        /// the method is used to fill a User UI details.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="offSet"></param>
        /// <returns></returns>
        public static UserUIControl FillUserLevelUIControl(object[] row, int offSet)
        {
            int companyID = 0 + offSet;
            int rMCompanyUserUIID = 1 + offSet;
            int companyUserID = 2 + offSet;
            int notifyUserWhenLiveFeedsAreDown = 3 + offSet;
            int allowUsertoOverwrite = 4 + offSet;
            int priceDeviation = 5 + offSet;
            int ticketSize = 6 + offSet;
            //int user =7 + offSet;
            int companyUserAUECID = 7 + offSet;

            UserUIControl userLevelUIControl = new UserUIControl();

            try
            {
                if (!(row[companyID] is System.DBNull))
                {
                    userLevelUIControl.CompanyID = int.Parse(row[companyID].ToString());
                }
                if (row[rMCompanyUserUIID] != null)
                {
                    userLevelUIControl.RMCompanyUserUIID = int.Parse(row[rMCompanyUserUIID].ToString());
                }
                if (row[companyUserID] != null)
                {
                    userLevelUIControl.CompanyUserID = int.Parse(row[companyUserID].ToString());
                }
                if (row[notifyUserWhenLiveFeedsAreDown] != null)
                {
                    userLevelUIControl.NotifyUserWhenLiveFeedsAreDown = int.Parse(row[notifyUserWhenLiveFeedsAreDown].ToString());
                }
                if (row[allowUsertoOverwrite] != null)
                {
                    userLevelUIControl.AllowUsertoOverwrite = int.Parse(row[allowUsertoOverwrite].ToString());
                }
                if (row[priceDeviation] != null)
                {
                    userLevelUIControl.PriceDeviation = int.Parse(row[priceDeviation].ToString());
                }
                if (row[ticketSize] != null)
                {
                    userLevelUIControl.TicketSize = int.Parse(row[ticketSize].ToString());
                }
                //if(row[user] != null)
                //{
                //    userLevelUIControl.ShortName = row[user].ToString();
                //}
                if (row[companyUserAUECID] != null)
                {
                    userLevelUIControl.CompanyUserAUECID = int.Parse(row[companyUserAUECID].ToString());
                }
            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {

                    throw;

                }
            }
            #endregion
            return userLevelUIControl;

        }

        //save method

        /// <summary>
        /// The method is used to Save userUI control details
        /// </summary>
        /// <param name="userLevelUIControl"></param>
        /// <param name="companyID"></param>
        /// <param name="companyUserID"></param>
        /// <returns></returns>
        public static int SaveUserLevelUI(UserUIControl userLevelUIControl, int companyID, int companyUserID)
        {
            int result = int.MinValue;

            try
            {
                object[] parameter = new object[9];

                parameter[0] = userLevelUIControl.RMCompanyUserUIID;
                parameter[1] = companyID;
                parameter[2] = companyUserID;
                parameter[3] = userLevelUIControl.CompanyUserAUECID;
                parameter[4] = userLevelUIControl.TicketSize;
                parameter[5] = userLevelUIControl.PriceDeviation;
                parameter[6] = userLevelUIControl.AllowUsertoOverwrite;
                parameter[7] = userLevelUIControl.NotifyUserWhenLiveFeedsAreDown;
                parameter[8] = int.MinValue;

                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveUserLevelUI", parameter).ToString());
                //				userLevelUIControl.RMCompanyUserUIID = result;
                //				}
            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {

                    throw;

                }
            }
            #endregion
            return result;
        }

        //Get UserUI Control Collection

        /// <summary>
        /// Gets all userLevelUI' details.
        /// </summary>
        /// <returns>Object of <see cref="UserLevelUI"/> collection.</returns>
        public static UserUIControls GetAllUserUIControls(int companyID, int userID)
        {
            UserUIControls userUIControls = new UserUIControls();

            Object[] parameter = new object[2];
            parameter[0] = companyID;
            parameter[1] = userID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetAllUserLevelUI", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        userUIControls.Add(FillUserLevelUIControl(row, 0));
                    }
                }
            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {

                    throw;

                }
            }
            #endregion
            return userUIControls;
        }

        //Get one UserUIControl details

        /// <summary>
        /// the method is ued to get a particular UserAuec RM details
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="companyUserID"></param>
        /// <param name="auecID"></param>
        /// <returns></returns>
        public static UserUIControl GetUserUIControl(int companyID, int companyUserID, int auecID)
        {
            UserUIControl userUIControl = new UserUIControl();

            object[] parameter = new object[3];
            parameter[0] = companyID;
            parameter[1] = companyUserID;
            parameter[2] = auecID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetRMUserUIControl", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        userUIControl = FillUserLevelUIControl(row, 0);
                    }
                }
            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {

                    throw;

                }
            }
            #endregion
            return userUIControl;
        }

        //Delete UserUIControl object

        /// <summary>
        /// The method is to delete a particular UserUi details.
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="auecID"></param>
        /// <returns></returns>
        public static bool DeleteUserUIControl(int userID, int auecID)
        {
            bool result = false;
            Object[] parameter = new object[2];
            parameter[0] = userID;
            parameter[1] = auecID;

            try
            {
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteUserUIControl", parameter) > 0)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {

                    throw;

                }
            }
            #endregion
            return result;
        }

        //Delete All UserUI control

        /// <summary>
        /// the method is used to delete all UserUi details for a aprticular User.
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static bool DeleteAllRMUserUIControls(int userID)
        {
            bool result = false;
            Object[] parameter = new object[1];
            parameter[0] = userID;

            try
            {
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteAllRMUserUIControls", parameter) > 0)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {

                    throw;

                }
            }
            #endregion
            return result;

        }

        #endregion Basic methods like Save/Get/Fill for UserUI
    }
}
