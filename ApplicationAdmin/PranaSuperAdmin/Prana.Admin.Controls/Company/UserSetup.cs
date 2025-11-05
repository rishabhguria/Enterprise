using Prana.Admin.BLL;
using Prana.AuditManager.Definitions.Interface;
using Prana.BusinessObjects.Enums;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Prana.Admin.Controls.Company
{
    /// <summary>
    /// Summary description for CompanyUser.
    /// </summary>
    [AuditManager.Attributes.Audit(AuditManager.Definitions.Enum.AuditAction.UserCreated, ShowAuditUI = true)]
    [AuditManager.Attributes.Audit(AuditManager.Definitions.Enum.AuditAction.UserUpdated, ShowAuditUI = true)]
    [AuditManager.Attributes.Audit(AuditManager.Definitions.Enum.AuditAction.UserDeleted, ShowAuditUI = true)]
    public partial class UserSetup : UserControl, IAuditSource
    {

        /// <summary>
        /// ID of the selected User
        /// </summary>
        public int _userID = int.MinValue;
        /// <summary>
        /// User object to hold all user relevant details
        /// </summary>
        public static User user;

        /// <summary>
        /// users company Id
        /// </summary>
        int _companyID;



        /// <summary>
        /// Constructor to initialize user setup
        /// </summary>
        [AuditManager.Attributes.AuditSourceConstAttri]
        public UserSetup()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Applying Black Gray Theme
        /// </summary>
        public void ApplyTheme()
        {
            try
            {
                this.userPermission1.ApplyTheme();
                this.userSetupDetails1.ApplyTheme();
                this.ForeColor = System.Drawing.Color.White;
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
        }

        /// <summary>
        /// Function to initialize user details along with mapping from group
        /// </summary>
        /// <param name="userID"></param>
        [AuditManager.Attributes.AuditRefreshMethAttri(AuditManager.Attributes.AuditMehodType.Arguments, 0)]
        public void InitializeControl(int userID, int RoleID)
        {
            try
            {
                _userID = userID;
                //BindClientName(dtClientsList);
                //BindUserRole();
                //cmbUserRole.Value = RoleID;
                if (_userID != int.MinValue)
                {
                    // New instance of user is created in GetUserDetails function and value is retrieved from userSetupDetails UI
                    user = userSetupDetails1.GetUserDetails(false);
                    // Reset userID
                    user.UserID = userID;
                    Prana.Admin.BLL.User userDetail = UserSetupManager.GetCompanyUserDetailsFromDB(userID);
                    cmbClientName.Value = userDetail.CompanyID;

                    _companyID = userDetail.CompanyID;
                    //cmbUserRole.Value = userDetail.RoleID;
                }
                userSetupDetails1.InitalizeControl(userID, RoleID);
                userPermission1.InitializeControl(userID);
                if (RoleID == (int)NirvanaRoles.User || RoleID == (int)NirvanaRoles.PowerUser)
                {
                    userPermission1.Visible = true;
                }
                else
                {
                    userPermission1.Visible = false;
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
        }

        /// <summary>
        /// save all changes in data base and clean back up of changes
        /// </summary>
        /// <param name="sender">button save</param>
        /// <param name="e">e</param>
        public int uBtnSaveUserSetup_Click(Dictionary<int, string> dicTradingAccounts)
        {
            bool isSaved = true;
            int errorNumber = 0;
            try
            {
                //if (UserSetupManager.isbackInitialied && user!=null)
                //{
                user = userSetupDetails1.GetUserDetails(isSaved);
                if (user == null)
                {
                    return -2;
                }
                if (AuthorizationManager.GetInstance()._authorizedPrincipal.Role.Equals(NirvanaRoles.User))
                {
                    return -1;
                }
                //Dictionary<int, string> dicTradingAccounts = userSetupDetails1.GetUserTradingAccounts(isSaved);                
                if (user != null && ValidateUserRoleUserCompany())
                {
                    UserSetupManager.CleanBackUp();

                    if (_userID == int.MinValue)
                    {
                        int maxUserID = UserSetupManager.GetMaxUserID();
                        if (maxUserID > 0)
                        {
                            //Added By Faisal Shah
                            _userID = maxUserID + 1;
                            user.UserID = maxUserID + 1;
                            AuditManager.BLL.AuditHandler.GetInstance().AuditDataForGivenInstance(this, user, AuditManager.Definitions.Enum.AuditAction.UserCreated);
                        }
                    }
                    else
                    {
                        AuditManager.BLL.AuditHandler.GetInstance().AuditDataForGivenInstance(this, user, AuditManager.Definitions.Enum.AuditAction.UserUpdated);
                    }
                    errorNumber = UserSetupManager.SaveMapping(_userID, user, dicTradingAccounts);
                    if (errorNumber == -7)
                    {
                        MessageBox.Show("Duplicate login for users cannot be saved.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return errorNumber;
                    }
                    else if (errorNumber == -5)
                    {
                        MessageBox.Show("There is already a User with the Same Short Name. Please choose other Short Name", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return errorNumber;
                    }
                    return _userID;
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Cannot insert duplicate key in object 'dbo.T_CompanyUser'"))
                {
                    MessageBox.Show("Duplicate login for users cannot be inserted.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
            }
            return 0;
        }

        /// <summary>
        /// Check if user details are not empty
        /// </summary>
        /// <returns>false if there are empty fields</returns>
        public bool ValidateUserRoleUserCompany()
        {
            try
            {
                //if (Convert.ToInt32(cmbUserRole.Value) == (int)NirvanaRoles.None || cmbUserRole.Value == null)
                //{
                //    MessageBox.Show("Please select User Role.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return false;
                //}
                //else
                //{
                //    user.RoleID = Convert.ToInt32(cmbUserRole.Value);
                //}
                if (cmbClientName.Value != null)
                {
                    user.CompanyID = Convert.ToInt32(cmbClientName.Value);
                }
                else
                {
                    //added by: Bharat Raturi
                    //Date: 07 may 2014
                    //purpose: to send default value while client combobox is hidden 
                    user.CompanyID = -1;
                    //MessageBox.Show("Please select Client name.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //return false;
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
            return true;
        }

        ///// <summary>
        ///// to bind all available clients in client dropdown
        ///// </summary>
        //public void BindClientName(DataTable _dtClientsName)
        //{
        //    try
        //    {
        //        cmbClientName.Refresh();
        //        cmbClientName.DataSource = null;
        //        cmbClientName.DisplayMember = "CompanyName";
        //        cmbClientName.ValueMember = "CompanyID";
        //        cmbClientName.DataSource = _dtClientsName;
        //        cmbClientName.Value = int.MinValue;
        //        cmbClientName.DisplayLayout.Bands[0].Columns["CompanyID"].Hidden = true;
        //        cmbClientName.DisplayLayout.Bands[0].ColHeadersVisible = false;
        //        cmbClientName.NullText = "--Select--";
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        /// <summary>
        /// to bind all user roles in dropdown
        /// </summary>
        //public void BindUserRole()
        //{
        //    try
        //    {
        //        cmbUserRole.Refresh();
        //        cmbUserRole.DataSource = null;
        //        cmbUserRole.DataSource = Enum.GetValues(typeof(NirvanaRoles));
        //        cmbUserRole.DisplayLayout.Bands[0].ColHeadersVisible = false;
        //        cmbUserRole.Value = int.MinValue;
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        /// <summary>
        /// Added By Faisal Shah
        /// Dated 30/06/14 
        /// </summary>
        /// <returns></returns>
        public int getUserCompanyID()
        {
            try
            {
                return _companyID;
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
                return int.MinValue;
            }
        }

        /// <summary>
        /// Audit for user deletion
        /// </summary>
        /// <param name="userID"></param>
        public void AuditUserDeletion(int userID)
        {
            try
            {
                if (userID != int.MinValue)
                {
                    Prana.Admin.BLL.User userDetail = UserSetupManager.GetCompanyUserDetailsFromDB(userID);
                    AuditManager.BLL.AuditHandler.GetInstance().AuditDataForGivenInstance(this, userDetail, AuditManager.Definitions.Enum.AuditAction.UserDeleted);
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
        }
    }
}
