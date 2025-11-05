using Infragistics.Win.UltraWinListView;
using Prana.Admin.BLL;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Prana.Admin.Controls.Company
{
    public partial class UserPermission : UserControl
    {
        #region Declaration

        /// <summary>
        /// ID of selected user
        /// </summary>
        public int _userID = int.MinValue;

        #endregion

        public UserPermission()
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
                this.UBtnUnSelectAvailableGroups.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
                this.uBtnAllUnSelectAvailableGroups.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
                this.uBtnUnSelectUserGroups.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
                this.uBtnAllUnSelectUserGroups.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));

                this.grpPermission.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
                this.grpPermission.ForeColor = System.Drawing.Color.White;

                this.groupBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
                this.groupBox2.ForeColor = System.Drawing.Color.White;
                this.groupBox3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
                this.groupBox3.ForeColor = System.Drawing.Color.White;
                this.groupBox4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
                this.groupBox4.ForeColor = System.Drawing.Color.White;

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

        #region Functions

        /// <summary>
        /// initialise data on page load
        /// </summary>
        public void InitializeControl(int userID)
        {
            try
            {
                _userID = userID;
                UserSetupManager.InitialiseData(userID);
                BindUnMappedGroups();
                if (_userID != int.MinValue)
                {
                    BindMappedGroups(UserSetupManager.GetGroupNamesForCompanyUser(_userID));
                }
                else
                {
                    BindMappedGroups(null);
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
        /// Show unmapped groups in ultra list view 
        /// </summary>
        public void BindUnMappedGroups()
        {
            try
            {
                List<String> unMappedGroupNames = new List<string>();
                //if (_userID != int.MinValue)
                //{
                unMappedGroupNames = UserSetupManager.GetUnmappedGroups(_userID);
                listAvailableGroups.Items.Clear();
                foreach (String groupName in unMappedGroupNames)
                {
                    UltraListViewItem item = new UltraListViewItem();
                    item.Tag = item.Key = groupName;
                    item.Value = groupName;
                    listAvailableGroups.Items.Add(item);
                }
                //}
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Show mapped groups in ultra list view 
        /// </summary>
        /// <param name="accountNames">List of mappedGroup Names for selected Company user </param>
        public void BindMappedGroups(List<String> groupNames)
        {
            try
            {
                if (groupNames == null)
                    uLstUserGroups.Items.Clear();
                else
                {

                    uLstUserGroups.Items.Clear();
                    foreach (String groupName in groupNames)
                    {
                        UltraListViewItem item = new UltraListViewItem();
                        item.Tag = item.Key = groupName;
                        item.Value = groupName;
                        uLstUserGroups.Items.Add(item);
                    }
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
        /// Show only Searched item in Available group view for a seleccted company user
        /// </summary>
        public void AddSearchedItemAvailableGroups()
        {
            try
            {
                //if (_userID != int.MinValue)
                //{
                List<String> searchingList = UserSetupManager.GetUnmappedGroups(_userID);
                List<String> result = UserSetupManager.SerachForKeyword(uTxtSearchGroups.Text, searchingList);
                listAvailableGroups.Items.Clear();
                if (result.Count > 0)
                {
                    foreach (String foundItem in result)
                    {
                        UltraListViewItem item = new UltraListViewItem(foundItem);
                        item.Key = foundItem;
                        item.Tag = foundItem;
                        item.Value = foundItem;
                        listAvailableGroups.Items.Add(item);
                    }
                }
                // }
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
        /// Show only Searched item in Mapped groups view for a selected company user
        /// </summary>
        public void AddSearchedItemMappedGroups()
        {
            try
            {
                //if (_userID != int.MinValue)
                //{
                List<String> searchingList = UserSetupManager.GetGroupNamesForCompanyUser(_userID);
                List<String> result = UserSetupManager.SerachForKeyword(uTxtSearchUserGroups.Text, searchingList);
                uLstUserGroups.Items.Clear();
                if (result.Count > 0)
                {
                    foreach (String foundItem in result)
                    {
                        UltraListViewItem item = new UltraListViewItem(foundItem);
                        item.Key = foundItem;
                        item.Tag = foundItem;
                        item.Value = foundItem;
                        uLstUserGroups.Items.Add(item);
                    }
                }
                //}
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

        #endregion

        #region CommonFunctions

        /// <summary>
        /// add group names in list to be mapped 
        /// </summary>
        /// <param name="isOnlySelectedRequired">true of false only selected Require </param>
        /// <returns>List of toBeMappedGroups</returns>
        private List<String> GetToBeUnMappedGroups(bool isOnlySelectedRequired)
        {
            int count;
            List<string> toBeUnMappedGroups = new List<string>();
            try
            {
                if (isOnlySelectedRequired)
                {
                    count = uLstUserGroups.SelectedItems.Count;
                    for (int i = 0; i < count; i++)
                    {
                        toBeUnMappedGroups.Add(uLstUserGroups.SelectedItems[i].Text);
                    }
                }
                else
                {
                    count = uLstUserGroups.Items.Count;
                    for (int i = 0; i < count; i++)
                    {
                        toBeUnMappedGroups.Add(uLstUserGroups.Items[i].Text);
                    }
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
            return toBeUnMappedGroups;
        }

        /// <summary>
        /// add group names in list to be Unmapped 
        /// </summary>
        /// <param name="isOnlySelectedRequired">true of false only selected Require </param>
        /// <returns>List of toBeUnMappedGroups</returns>
        private List<string> GetToBeMappedGroups(bool isOnlySelectedRequired)
        {
            int count;
            List<string> toBeMappedGroups = new List<string>();
            try
            {
                if (isOnlySelectedRequired)
                {
                    count = listAvailableGroups.SelectedItems.Count;
                    for (int i = 0; i < count; i++)
                    {
                        toBeMappedGroups.Add(listAvailableGroups.SelectedItems[i].Text);
                    }
                }
                else
                {
                    count = listAvailableGroups.Items.Count;
                    for (int i = 0; i < count; i++)
                    {
                        toBeMappedGroups.Add(listAvailableGroups.Items[i].Text);
                    }
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
            return toBeMappedGroups;
        }

        #endregion

        #region Events

        /// <summary>
        /// Function to unselect available groups selected in list view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UBtnUnSelectAvailableGroups_Click(object sender, EventArgs e)
        {
            try
            {
                //if (_userID != int.MinValue)
                //{
                int userID = _userID;
                List<string> unSelectGroups = GetToBeMappedGroups(true);
                UserSetupManager.MapGroupsToUser(userID, unSelectGroups);
                List<String> groupNames = UserSetupManager.GetGroupNamesForCompanyUser(userID);
                listAvailableGroups.Items.Clear();
                BindMappedGroups(groupNames);
                BindUnMappedGroups();
                //Contain search to assigned key in AvailableGroups list view
                uTxtSearchGroups_TextChanged(uTxtSearchGroups, null);
                //Contain search to assigned key in Mapped Groups List view
                uTxtSearchUserGroups_TextChanged(uTxtSearchUserGroups, null);
                // }
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
        ///  Function to unselect all available groups from view list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uBtnAllUnSelectAvailableGroups_Click(object sender, EventArgs e)
        {
            try
            {
                //if (_userID != int.MinValue)
                //{
                int userID = _userID;
                List<string> unSelectGroups = GetToBeMappedGroups(false);
                UserSetupManager.MapGroupsToUser(userID, unSelectGroups);
                List<String> groupNames = UserSetupManager.GetGroupNamesForCompanyUser(userID);
                listAvailableGroups.Items.Clear();
                BindMappedGroups(groupNames);
                BindUnMappedGroups();
                //Contain search to assigned key in AvailableGroups list view
                uTxtSearchGroups_TextChanged(uTxtSearchGroups, null);
                //Contain search to assigned key in Mapped Groups List view
                uTxtSearchUserGroups_TextChanged(uTxtSearchUserGroups, null);
                // }
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
        /// Function to unselect mapped groups selected in list view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uBtnSelectUserGroups_Click(object sender, EventArgs e)
        {
            try
            {
                //if (_userID != int.MinValue)
                // {
                int userID = _userID;
                List<string> unSelectGroups = GetToBeUnMappedGroups(true);
                UserSetupManager.UnMapGroupsFromUser(userID, unSelectGroups);
                List<String> groupNames = UserSetupManager.GetGroupNamesForCompanyUser(userID);
                uLstUserGroups.Items.Clear();
                BindMappedGroups(groupNames);
                BindUnMappedGroups();
                //Contain search to assigned key in AvailableGroups list view
                uTxtSearchGroups_TextChanged(uTxtSearchGroups, null);
                //Contain search to assigned key in Mapped Groups List view
                uTxtSearchUserGroups_TextChanged(uTxtSearchUserGroups, null);
                // }
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
        /// Function to unselect all mapped groups from in list view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uBtnAllUnSelectUserGroups_Click(object sender, EventArgs e)
        {
            try
            {
                //if (_userID != int.MinValue)
                // {
                int userID = _userID;
                List<string> unSelectGroups = GetToBeUnMappedGroups(false);
                UserSetupManager.UnMapGroupsFromUser(userID, unSelectGroups);
                List<String> groupNames = UserSetupManager.GetGroupNamesForCompanyUser(userID);
                uLstUserGroups.Items.Clear();
                BindMappedGroups(groupNames);
                BindUnMappedGroups();
                //Contain search to assigned key in AvailableGroups list view
                uTxtSearchGroups_TextChanged(uTxtSearchGroups, null);
                //Contain search to assigned key in Mapped Groups List view
                uTxtSearchUserGroups_TextChanged(uTxtSearchUserGroups, null);
                // }
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
        ///  searching in available groups
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uTxtSearchGroups_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(uTxtSearchGroups.Text.ToUpper()) && uTxtSearchGroups.Text.ToUpper() != "SEARCH")
                    AddSearchedItemAvailableGroups();
                else
                {
                    listAvailableGroups.Items.Clear();
                    BindUnMappedGroups();
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
        ///  searching in mapped clients
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uTxtSearchUserGroups_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(uTxtSearchUserGroups.Text.ToUpper()) && uTxtSearchUserGroups.Text.ToUpper() != "SEARCH")
                    AddSearchedItemMappedGroups();
                else
                {
                    uLstUserGroups.Items.Clear();
                    //if (_userID != int.MinValue)
                    //{
                    List<String> groupNames = UserSetupManager.GetGroupNamesForCompanyUser(_userID);
                    if (groupNames != null)
                    {
                        foreach (String foundItem in groupNames)
                        {
                            UltraListViewItem item = new UltraListViewItem();
                            item.Key = foundItem;
                            item.Tag = foundItem;
                            item.Value = foundItem;
                            uLstUserGroups.Items.Add(item);
                        }
                    }
                    else
                    {
                        uLstUserGroups.Items.Clear();
                    }
                    //}
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

        #endregion

        /// <summary>
        /// To unselect Assigned groups
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listAvailableGroups_ItemSelectionChanged(object sender, ItemSelectionChangedEventArgs e)
        {
            try
            {
                if (e.SelectedItems.Count > 0)
                {
                    uLstUserGroups.SelectedItems.Clear();
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
        /// To unselect Available groups
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uLstUserGroups_ItemSelectionChanged(object sender, ItemSelectionChangedEventArgs e)
        {
            try
            {
                if (e.SelectedItems.Count > 0)
                {
                    listAvailableGroups.SelectedItems.Clear();
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
