using Prana.Admin.BLL;
using Prana.BusinessObjects.Enums;
using Prana.LogManager;
using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Prana.Admin.Controls.Company
{
    public partial class UserSetupDetails : UserControl
    {
        /// <summary>
        /// ID of selected user
        /// </summary>
        public int _userID = int.MinValue;

        public UserSetupDetails()
        {
            InitializeComponent();
            //BindTradingAccount();
        }

        /// <summary>
        /// Applying Black Gray Theme
        /// </summary>
        public void ApplyTheme()
        {
            try
            {
                this.groupBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
                this.groupBox1.ForeColor = System.Drawing.Color.White;

                this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
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
        /// For initializing user details
        /// </summary>
        /// <param name="userID"></param>
        public void InitalizeControl(int userID, int roleId)
        {
            try
            {
                _userID = userID;
                Prana.Admin.BLL.User user = UserSetupManager.GetCompanyUserDetailsFromDB(userID);
                if (userID == int.MinValue)
                {
                    user.RoleID = roleId;
                }
                SetUserDetails(user);
                SetUIBasedonRoleID(roleId);
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
        /// Set visibility of usertype and AllGroupAccess checkbox to false when user role is user or poweruser
        /// </summary>
        /// <param name="roleId"></param>
        private void SetUIBasedonRoleID(int roleId)
        {
            try
            {
                if (roleId == (int)NirvanaRoles.User)
                {
                    rdbUser.Visible = true;
                    chkAllGrpAccess.Visible = true;
                    if (AuthorizationManager.GetInstance()._authorizedPrincipal.Role.Equals(NirvanaRoles.User))
                    {
                        rdbPowerUser.Visible = false;
                    }
                    else
                    {
                        rdbPowerUser.Visible = true;
                    }
                    rdbAdministrator.Visible = false;
                    rdbSysAdmin.Visible = false;
                    rdbAdministrator.Checked = false;
                    rdbSysAdmin.Checked = false;
                    rdbPowerUser.Checked = false;
                }
                else if (roleId == (int)NirvanaRoles.PowerUser)
                {
                    rdbUser.Visible = true;
                    rdbUser.Checked = false;
                    rdbPowerUser.Visible = true;
                    rdbPowerUser.Checked = true;
                    chkAllGrpAccess.Visible = true;
                    rdbAdministrator.Visible = false;
                    rdbSysAdmin.Visible = false;
                    rdbAdministrator.Checked = false;
                    rdbSysAdmin.Checked = false;
                    if (AuthorizationManager.GetInstance()._authorizedPrincipal.Role.Equals(NirvanaRoles.PowerUser))
                    {
                        rdbPowerUser.Enabled = false;
                        rdbUser.Enabled = false;
                    }
                }
                else if (roleId == (int)NirvanaRoles.Administrator)
                {
                    chkAllGrpAccess.Visible = false;
                    chkAllGrpAccess.Checked = true;
                    rdbAdministrator.Visible = true;
                    rdbAdministrator.Checked = true;
                    rdbSysAdmin.Visible = false;
                    rdbPowerUser.Visible = false;
                    rdbUser.Visible = false;
                    rdbSysAdmin.Checked = false;
                    rdbPowerUser.Checked = false;
                    rdbUser.Checked = false;
                }
                else if (roleId == (int)NirvanaRoles.SystemAdministrator)
                {
                    chkAllGrpAccess.Visible = false;
                    chkAllGrpAccess.Checked = true;
                    rdbSysAdmin.Visible = true;
                    rdbSysAdmin.Checked = true;
                    rdbAdministrator.Visible = false;
                    rdbPowerUser.Visible = false;
                    rdbUser.Visible = false;
                    rdbAdministrator.Checked = false;
                    rdbPowerUser.Checked = false;
                    rdbUser.Checked = false;
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
        /// To show selected user details
        /// </summary>
        /// <param name="user"></param>
        private void SetUserDetails(User user)
        {
            try
            {
                txtFirstName.Text = user.FirstName;
                txtLastName.Text = user.LastName;
                txtLoginName.Text = user.LoginName;
                txtPassword.Text = user.Password;
                txtEMail.Text = user.EMail;
                txtRegion.Text = user.Region;
                txtShortName.Text = user.ShortName;
                if (user.RoleID == (int)NirvanaRoles.User)
                    rdbUser.Checked = true;
                else if (user.RoleID == (int)NirvanaRoles.PowerUser)
                    rdbPowerUser.Checked = true;
                else
                {
                    rdbUser.Checked = false;
                    rdbPowerUser.Checked = false;
                }
                if (user.IsAllGroupsAccess)
                    chkAllGrpAccess.Checked = true;
                else
                    chkAllGrpAccess.Checked = false;

                //Dictionary<int, string> dictTAuserMapping = UserSetupManager.GetAllMappedTradingAccounts(user.UserID);
                //if (dictTAuserMapping.Count == 0)
                //{
                //    //multiSelectDropDown1.SelectUnselectAll(CheckState.Checked);
                //    //multiSelectDropDown1.SelectUnselectAll(CheckState.Unchecked);
                //}
                //else
                //    multiSelectDropDown1.SelectUnselectItems(dictTAuserMapping, CheckState.Checked);
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
        /// Function to get user details for saving data
        /// </summary>
        /// <returns></returns>
        public User GetUserDetails(bool isSaved)
        {
            User user = new User();
            try
            {
                if (isSaved)
                {
                    string emailCheck = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
                    Regex emailRegex = new Regex(emailCheck);
                    Match emailMatch = emailRegex.Match(txtEMail.Text.ToString());

                    errorProvider1.SetError(txtLoginName, "");
                    errorProvider1.SetError(txtPassword, "");
                    errorProvider1.SetError(txtFirstName, "");
                    errorProvider1.SetError(txtEMail, "");
                    errorProvider1.SetError(txtLastName, "");
                    // errorProvider1.SetError(multiSelectDropDown1, "");
                    errorProvider1.SetError(txtShortName, "");

                    if (txtLoginName.Text.Trim() == "")
                    {
                        errorProvider1.SetError(txtLoginName, "Please enter Login Name in details!");
                        txtLoginName.Focus();
                        return null;
                    }
                    else if (txtPassword.Text.Trim() == "")
                    {
                        errorProvider1.SetError(txtPassword, "Please enter Password!");
                        txtPassword.Focus();
                        return null;
                    }
                    else if (int.Parse(txtPassword.Text.Trim().Length.ToString()) < 4)
                    {
                        errorProvider1.SetError(txtPassword, "Please enter password having at least four characters !");
                        txtPassword.Focus();
                        return null;
                    }
                    else if (txtFirstName.Text.Trim() == "")
                    {
                        errorProvider1.SetError(txtFirstName, "Please enter First Name!");
                        txtFirstName.Focus();
                        return null;
                    }
                    //added By: Bharat Raturi, 12-may-2014
                    //Purpose: make the last name field mandatory
                    else if (txtLastName.Text.Trim() == "")
                    {
                        errorProvider1.SetError(txtLastName, "Please enter Last Name!");
                        txtLastName.Focus();
                        return null;
                    }
                    else if (!emailMatch.Success)
                    {
                        errorProvider1.SetError(txtEMail, "Please enter valid Email address!");
                        txtEMail.Focus();
                        return null;
                    }
                    else if (txtShortName.Text.Trim() == "")
                    {
                        errorProvider1.SetError(txtShortName, "Please enter Short Name!");
                        txtShortName.Focus();
                        return null;
                    }
                    else if (txtShortName.Text.Length > 5)
                    {
                        //added by: Sachin Mishra, 27 jan 2015
                        //purpose: To validate the length of the short name JIRA-CHMW-2441
                        errorProvider1.SetError(txtShortName, "Short Name cannot be more than 5 characters!");
                        txtShortName.Focus();
                        return null;
                    }
                    //else if (multiSelectDropDown1.GetNoOfCheckedItems() == 0)
                    //{
                    //    errorProvider1.SetError(multiSelectDropDown1, "Please select atleast one Trading Account!");
                    //    multiSelectDropDown1.Focus();
                    //    return null;
                    //}
                    else
                    {
                        FillUser(user);
                    }
                }
                else
                {
                    FillUser(user);
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
            return user;
        }

        /// <summary>
        /// Function to get trading accounts for saving data
        /// </summary>
        /// <returns></returns>
        //public Dictionary<int, string> GetUserTradingAccounts(bool isSaved)
        //{
        //    Dictionary<int, string> dicTradingAccounts = new Dictionary<int, string>();
        //    try
        //    {
        //        if (isSaved)
        //        {
        //            foreach (KeyValuePair<int, string> kvp in multiSelectDropDown1.GetSelectedItemsInDictionary())
        //            {
        //                dicTradingAccounts.Add(kvp.Key, kvp.Value);
        //            }
        //        }
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
        //    return dicTradingAccounts;
        //}

        /// <summary>
        /// Set user object with user details
        /// </summary>
        /// <param name="user"></param>
        public void FillUser(User user)
        {
            try
            {
                user.UserID = _userID;
                user.FirstName = txtFirstName.Text.Trim();
                user.ShortName = txtLoginName.Text.Trim();
                user.LastName = txtLastName.Text.Trim();
                user.LoginName = txtLoginName.Text.Trim();
                user.Password = txtPassword.Text.Trim();
                user.EMail = txtEMail.Text.Trim();
                user.Region = txtRegion.Text.Trim();
                user.ShortName = txtShortName.Text.Trim();
                if (rdbUser.Checked == true)
                    user.RoleID = (int)NirvanaRoles.User;
                else if (rdbPowerUser.Checked == true)
                    user.RoleID = (int)NirvanaRoles.PowerUser;
                else if (rdbAdministrator.Checked == true)
                    user.RoleID = (int)NirvanaRoles.Administrator;
                else if (rdbSysAdmin.Checked == true)
                    user.RoleID = (int)NirvanaRoles.SystemAdministrator;
                if (chkAllGrpAccess.Checked == true)
                    user.IsAllGroupsAccess = true;
                else
                    user.IsAllGroupsAccess = false;
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

        private void txtLoginName_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtLoginName.Text.Trim() != "")
                {
                    errorProvider1.SetError(txtLoginName, "");
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtPassword.Text.Trim().Length >= 4)
                {
                    errorProvider1.SetError(txtPassword, "");
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void txtFirstName_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtFirstName.Text.Trim() != "")
                {
                    errorProvider1.SetError(txtFirstName, "");
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void txtLastName_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtLastName.Text.Trim() != "")
                {
                    errorProvider1.SetError(txtLastName, "");
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void txtEMail_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string emailCheck = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
                Regex emailRegex = new Regex(emailCheck);
                Match emailMatch = emailRegex.Match(txtEMail.Text.ToString());

                if (emailMatch.Success)
                {
                    errorProvider1.SetError(txtEMail, "");
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void txtShortName_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtShortName.Text.Trim() != "")
                {
                    errorProvider1.SetError(txtShortName, "");
                }
                //added by: Sachin Mishra, 27 jan 2015
                //purpose: To validate the length of the short name JIRA-CHMW-2441
                if (txtShortName.Text.Length > 5)
                {
                    errorProvider1.SetError(txtShortName, "Short Name cannot be more than 5 characters!");
                    txtShortName.Focus();
                }

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        //public void BindTradingAccount()
        //{
        //    Dictionary<int, string> dicTradingAccounts = new Dictionary<int, string>();
        //    dicTradingAccounts = UserSetupManager.GetTradingAccounts();
        //    multiSelectDropDown1.SetManualTheme(false);
        //    //add Assets to the check list default value will be unchecked
        //    multiSelectDropDown1.AddItemsToTheCheckList(dicTradingAccounts, CheckState.Unchecked);

        //    //adjust checklistbox width according to the longest Asset Name
        //    multiSelectDropDown1.AdjustCheckListBoxWidth();
        //    multiSelectDropDown1.TitleText = "";
        //    multiSelectDropDown1.SetTitleText(0);
        //}
    }
}
