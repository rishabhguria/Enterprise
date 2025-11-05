using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.Admin.BLL;
using Prana.BusinessObjects.Enums;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Prana.Admin.Controls
{
    /// <summary>
    /// class for user permissions
    /// </summary>
    public partial class CtrlUserPermissions : UserControl
    {
        // global variables

        public bool _isSaveRequired = false;
        private ValueList _vlPrincipalType = new ValueList();
        private ValueList _vlAuthRoles = new ValueList();
        private ValueList _vlResourceType = new ValueList();
        private ValueList _vlResourceValueAccountGroup = new ValueList();
        private ValueList _vlResourceValueModule = new ValueList();
        private ValueList _vlAuthActionValue = new ValueList();
        private ValueList _vlAuthActionValueForAdmin = new ValueList();

        public CtrlUserPermissions()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialize the control with the data
        /// </summary>
        public void InitializeControl()
        {
            try
            {
                UserPermissionManager.GetAuthData();
                InitializeValueLists();
                grdPermissions.DataSource = UserPermissionManager.GetUserPermissions();

                SetResourceValues();
                SetAuthorizationValues();
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
        /// set the value for authorization
        /// </summary>
        private void SetAuthorizationValues()
        {
            foreach (UltraGridRow row in grdPermissions.Rows)
            {
                int i = 0;
                if (!string.IsNullOrWhiteSpace(row.Cells["PrincipalType"].Value.ToString()))
                {
                    i = Convert.ToInt32(row.Cells["PrincipalType"].Value);
                }
                if (i == 1)
                {
                    //row.Cells["ResourceDataValue"].ValueList = _vlResourceValueAccountGroup;
                }
                else if (i == 2)
                {
                    row.Cells["PricipalValue"].ValueList = _vlAuthRoles;
                }
                else if (i == 3)
                {
                }
            }
        }

        /// <summary>
        /// Set the resource value
        /// </summary>
        private void SetResourceValues()
        {
            foreach (UltraGridRow row in grdPermissions.Rows)
            {
                int i = 0;
                if (!string.IsNullOrWhiteSpace(row.Cells["ResourceDataType"].Value.ToString()))
                {
                    i = Convert.ToInt32(row.Cells["ResourceDataType"].Value);
                }
                if (i == 1)
                {
                    row.Cells["ResourceDataValue"].ValueList = _vlResourceValueAccountGroup;
                }
                else if (i == 2)
                {
                    row.Cells["ResourceDataValue"].ValueList = _vlResourceValueModule;
                }
            }
        }

        /// <summary>
        /// define the layout of the grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdPermissions_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                e.Layout.Override.ActiveRowAppearance.BackColor = Color.Black;
                e.Layout.Override.ActiveRowAppearance.BackColor2 = Color.Black;
                e.Layout.Override.ActiveRowAppearance.ForeColor = Color.White;

                e.Layout.Override.RowAppearance.BackColor = Color.Black;
                e.Layout.Override.RowAppearance.BackColor2 = Color.Black;
                e.Layout.Override.RowAppearance.ForeColor = Color.White;

                e.Layout.Override.ActiveCellAppearance.BackColor = Color.Black;
                e.Layout.Override.ActiveCellAppearance.BackColor2 = Color.Black;
                e.Layout.Override.ActiveCellAppearance.ForeColor = Color.White;

                foreach (UltraGridColumn column in e.Layout.Bands[0].Columns)
                {
                    //following line auto adjust width of columns of ultragrid accocrding to text size of header.
                    column.PerformAutoResize(PerformAutoSizeType.AllRowsInBand, AutoResizeColumnWidthOptions.All);
                }
                if (grdPermissions.DataSource != null)
                {
                    UltraGridBand band = grdPermissions.DisplayLayout.Bands[0];
                    if (band.Columns.Exists("PermissionId"))
                    {
                        band.Columns["PermissionId"].Hidden = true;
                    }
                    if (band.Columns.Exists("PrincipalType"))
                    {
                        UltraGridColumn colPrincipalType = band.Columns["PrincipalType"];
                        colPrincipalType.Header.Caption = "Principal Type";
                        colPrincipalType.ValueList = _vlPrincipalType;
                        colPrincipalType.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                        colPrincipalType.Header.VisiblePosition = 1;
                    }
                    if (band.Columns.Exists("PricipalValue"))
                    {
                        UltraGridColumn colPrincipalValue = band.Columns["PricipalValue"];
                        colPrincipalValue.Header.Caption = "Principal Value";
                        colPrincipalValue.NullText = "-Select-";
                        colPrincipalValue.Header.VisiblePosition = 2;
                        colPrincipalValue.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                    }
                    if (band.Columns.Exists("ResourceDataType"))
                    {
                        UltraGridColumn colResourceDataType = band.Columns["ResourceDataType"];
                        colResourceDataType.Header.Caption = "Resource Type";
                        colResourceDataType.ValueList = _vlResourceType;
                        colResourceDataType.Header.VisiblePosition = 3;
                        colResourceDataType.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                    }
                    if (band.Columns.Exists("ResourceDataValue"))
                    {
                        UltraGridColumn colResourceDataValue = band.Columns["ResourceDataValue"];
                        colResourceDataValue.Header.Caption = "Resource";
                        colResourceDataValue.Header.VisiblePosition = 4;
                        colResourceDataValue.NullText = "-Select-";
                        colResourceDataValue.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                    }
                    if (band.Columns.Exists("AuthActionValue"))
                    {
                        UltraGridColumn colAuthActionValue = band.Columns["AuthActionValue"];
                        colAuthActionValue.Header.Caption = "Authorization Action";
                        colAuthActionValue.ValueList = _vlAuthActionValue;
                        colAuthActionValue.Header.VisiblePosition = 5;
                        colAuthActionValue.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
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
        /// Added By Faisal Shah 17/09/14
        /// Purpose was to set SystemAdmin permissions to Read and Write only while loading Saved data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdPermissions_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
        {
            try
            {
                if (e.Row.Cells.Exists("PricipalValue"))
                {
                    string authRole = string.Empty;
                    if (!string.IsNullOrEmpty(e.Row.Cells["PricipalValue"].Value.ToString()))
                        authRole = Enum.GetName(typeof(NirvanaRoles), e.Row.Cells["PricipalValue"].Value);
                    if (!string.IsNullOrWhiteSpace(authRole) && authRole == NirvanaRoles.SystemAdministrator.ToString())
                    {
                        e.Row.Cells["AuthActionValue"].ValueList = _vlAuthActionValueForAdmin;
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
        /// initlaize the value lists for the permissions
        /// </summary>
        private void InitializeValueLists()
        {
            try
            {

                Dictionary<int, string> dictPrincipalType = UserPermissionManager.GetPrincipalTypes();
                Dictionary<int, string> dictAuthRoles = UserPermissionManager.GetAuthRoles();
                Dictionary<int, string> dictResourceType = UserPermissionManager.GetResourceTypes();
                Dictionary<int, string> dictResourceValueAccount = UserPermissionManager.GetResourceValuesAccountGroups();
                Dictionary<int, string> dictResourceValueModule = UserPermissionManager.GetResourceValuesModule();
                Dictionary<int, string> dictAuthAction = UserPermissionManager.GetAuthActions();

                ClearValueLists();
                AddDefaultItemToValueLists();

                foreach (int pTypeID in dictPrincipalType.Keys)
                {
                    _vlPrincipalType.ValueListItems.Add(pTypeID, dictPrincipalType[pTypeID]);
                }
                foreach (int pValueID in dictAuthRoles.Keys)
                {
                    _vlAuthRoles.ValueListItems.Add(pValueID, dictAuthRoles[pValueID]);
                }
                foreach (int rTypeID in dictResourceType.Keys)
                {
                    // purpose: ignore the resource type 'accountgroup'
                    if (!dictResourceType[rTypeID].Trim().ToLower().Equals("accountgroup"))
                    {
                        _vlResourceType.ValueListItems.Add(rTypeID, dictResourceType[rTypeID]);
                    }
                }
                foreach (int rValueID in dictResourceValueAccount.Keys)
                {
                    _vlResourceValueAccountGroup.ValueListItems.Add(rValueID, dictResourceValueAccount[rValueID]);
                }
                foreach (int rValueID in dictResourceValueModule.Keys)
                {
                    _vlResourceValueModule.ValueListItems.Add(rValueID, dictResourceValueModule[rValueID]);
                }
                foreach (int aValueID in dictAuthAction.Keys)
                {
                    //added by: Bharat raturi, 13 jun 2014
                    //purpose: ignore the authorization action 'none' if that occurs
                    if (!dictAuthAction[aValueID].Trim().ToLower().Equals("none"))
                    {
                        _vlAuthActionValue.ValueListItems.Add(aValueID, dictAuthAction[aValueID]);
                    }
                    //added by: Bharat raturi, 13 jun 2014
                    //purpose: create a separate valuelist for the administrator and above roles
                    if (aValueID > 3)
                    {
                        _vlAuthActionValueForAdmin.ValueListItems.Add(aValueID, dictAuthAction[aValueID]);
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
        /// add default value to the value lists
        /// </summary>
        private void AddDefaultItemToValueLists()
        {
            try
            {
                _vlAuthActionValue.ValueListItems.Add(-1, "-Select-");
                _vlAuthActionValueForAdmin.ValueListItems.Add(-1, "-Select-");
                _vlPrincipalType.ValueListItems.Add(-1, "-Select-");
                _vlAuthRoles.ValueListItems.Add(-1, "-Select-");
                _vlResourceType.ValueListItems.Add(-1, "-Select-");
                _vlResourceValueAccountGroup.ValueListItems.Add(-1, "-Select-");
                _vlResourceValueModule.ValueListItems.Add(-1, "-Select-");
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
        /// clear value lists
        /// </summary>
        private void ClearValueLists()
        {
            try
            {
                _vlAuthActionValueForAdmin.ValueListItems.Clear();
                _vlAuthActionValue.ValueListItems.Clear();
                _vlPrincipalType.ValueListItems.Clear();
                _vlAuthRoles.ValueListItems.Clear();
                _vlResourceType.ValueListItems.Clear();
                _vlResourceValueAccountGroup.ValueListItems.Clear();
                _vlResourceValueModule.ValueListItems.Clear();
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
        /// bind the values to the cells when the content is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdPermissions_CellChange(object sender, CellEventArgs e)
        {
            try
            {
                if (!_isSaveRequired)
                {
                    _isSaveRequired = true;
                }
                if (e.Cell.Column.Key.Equals("PrincipalType"))
                {
                    UltraGridColumn gridColumn = e.Cell.Column;
                    String ColumnText = e.Cell.Text;
                    EmbeddableEditorBase editor = e.Cell.EditorResolved;
                    object changedValue = editor.IsValid ? editor.Value : editor.CurrentEditText;
                    UltraGridRow actRow = grdPermissions.ActiveRow;
                    if (ColumnText.Equals("-Select-"))
                    {
                        actRow.Cells["PricipalValue"].Value = -1;
                        actRow.Cells["PricipalValue"].Activation = Activation.Disabled;
                        return;
                    }
                    actRow.Cells["PricipalValue"].Activation = Activation.AllowEdit;
                    int i = Convert.ToInt32(changedValue);
                    if (i == 1)
                    {
                    }
                    else if (i == 2)
                    {
                        actRow.Cells["PricipalValue"].Value = -1;
                        actRow.Cells["PricipalValue"].ValueList = _vlAuthRoles;
                    }
                    else if (i == 3)
                    {
                    }
                }
                else if (e.Cell.Column.Key.Equals("ResourceDataType"))
                {
                    UltraGridColumn gridColumn = e.Cell.Column;
                    String ColumnText = e.Cell.Text;
                    EmbeddableEditorBase editor = e.Cell.EditorResolved;
                    object changedValue = editor.IsValid ? editor.Value : editor.CurrentEditText;
                    UltraGridRow actRow = grdPermissions.ActiveRow;
                    if (ColumnText.Equals("-Select-"))
                    {
                        actRow.Cells["ResourceDataValue"].Value = -1;
                        actRow.Cells["ResourceDataValue"].Activation = Activation.Disabled;
                        return;
                    }
                    actRow.Cells["ResourceDataValue"].Activation = Activation.AllowEdit;
                    int i = Convert.ToInt32(changedValue);
                    if (i == 1)
                    {
                        actRow.Cells["ResourceDataValue"].ValueList = _vlResourceValueAccountGroup;
                    }
                    else if (i == 2)
                    {
                        actRow.Cells["ResourceDataValue"].ValueList = _vlResourceValueModule;
                    }
                    actRow.Cells["ResourceDataValue"].Value = -1;
                }
                //added by: Bharat Raturi, 13 jun 2014
                //purpose: auth actions would only have write and approve value for admin and above roles
                if (e.Cell.Column.Key.Equals("PricipalValue"))
                {
                    UltraGridColumn gridColumn = e.Cell.Column;
                    String ColumnText = e.Cell.Text;
                    EmbeddableEditorBase editor = e.Cell.EditorResolved;
                    object changedValue = editor.IsValid ? editor.Value : editor.CurrentEditText;
                    if (!string.IsNullOrWhiteSpace(changedValue.ToString()) && Convert.ToInt32(changedValue) > 3)
                    {
                        e.Cell.Row.Cells["AuthActionValue"].Value = -1;
                        e.Cell.Row.Cells["AuthActionValue"].ValueList = _vlAuthActionValueForAdmin;
                    }
                    else
                    {
                        e.Cell.Row.Cells["AuthActionValue"].Value = -1;
                        e.Cell.Row.Cells["AuthActionValue"].ValueList = _vlAuthActionValue;
                    }
                }
                // To reflect the changes made by the user, into data source.
                grdPermissions.UpdateData();
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
        /// add new row to the grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            grdPermissions.DisplayLayout.Bands[0].AddNew();
            if (grdPermissions.ActiveRow != null)
            {
                UltraGridRow row = grdPermissions.ActiveRow;
                row.Cells["PrincipalType"].Value = -1;
                row.Cells["ResourceDataType"].Value = -1;
                row.Cells["AuthActionValue"].Value = -1;
                row.Cells["PricipalValue"].Activation = Activation.Disabled;
                row.Cells["ResourceDataValue"].Activation = Activation.Disabled;
            }
        }

        /// <summary>
        /// Delete the active row from the grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (grdPermissions.ActiveRow != null)
            {
                UltraGridRow row = grdPermissions.ActiveRow;
                if (!string.IsNullOrEmpty(row.Cells["PricipalValue"].Value.ToString()) && !string.IsNullOrEmpty(row.Cells["ResourceDataValue"].Value.ToString()))
                {
                    int principalValue = Convert.ToInt32(row.Cells["PricipalValue"].Value);
                    int resourceValue = Convert.ToInt32(row.Cells["ResourceDataValue"].Value);
                    if (principalValue >= 3 && (resourceValue == 7 || resourceValue == 74))
                    {
                        MessageBox.Show("This permission cannot be deleted", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                if (!row.Cells["PrincipalType"].Text.Equals("-Select-") && !row.Cells["AuthActionValue"].Text.Equals("-Select-") && !row.Cells["ResourceDataType"].Text.Equals("-Select-"))
                {
                    DialogResult dr = MessageBox.Show("Do you want to delete the selected permission?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dr == DialogResult.No)
                    {
                        return;
                    }
                    if (!_isSaveRequired)
                    {
                        _isSaveRequired = true;
                    }

                    grdPermissions.ActiveRow.Delete(false);
                    return;
                    //DataTable dt = grdPermissions.DataSource as DataTable;
                    //dt.AcceptChanges();
                }
            }
            else
            {
                MessageBox.Show("Select a row to delete", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Save the permissions to the database
        /// </summary>
        /// <returns>number of affected rows</returns>
        public int SavePermissions()
        {
            int i = 0;
            try
            {
                if (grdPermissions.DisplayLayout.Bands[0].Override.AllowUpdate == DefaultableBoolean.False)
                {
                    return i;
                }
                if (_isSaveRequired)
                {
                    _isSaveRequired = false;
                }
                if (HasEmpty())
                {
                    MessageBox.Show("Blank permissions cannot be added.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return i;
                }
                DataTable dtPermissions = grdPermissions.DataSource as DataTable;
                i = UserPermissionManager.SaveUserPermissions(dtPermissions);
                if (i == -1)
                {
                    MessageBox.Show("Duplicate permissions cannot be defined.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //Purpose : To delete duplicate row grom grid.
                    grdPermissions.ActiveRow.Delete(false);
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
            return i;
        }

        /// <summary>
        /// check if there are empty records
        /// </summary>
        /// <returns>true if there are empty records</returns>
        public bool HasEmpty()
        {
            try
            {
                for (int i = grdPermissions.Rows.Count - 1; i >= 0; i--)
                {
                    UltraGridRow row = grdPermissions.Rows[i];
                    if (row.Cells["PrincipalType"].Text.Equals("-Select-") && row.Cells["ResourceDataType"].Text.Equals("-Select-") && row.Cells["AuthActionValue"].Text.Equals("-Select-"))
                    {
                        row.Delete(false);
                        continue;
                    }
                    if (row.Cells["PrincipalType"].Text.Equals("-Select-") || row.Cells["ResourceDataType"].Text.Equals("-Select-") || row.Cells["AuthActionValue"].Text.Equals("-Select-"))
                    {
                        return true;
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
            return false;
        }

        /// <summary>
        /// added by: Bharat Raturi, 13 jun 2014
        /// make the controls read only if the user does not have write permission
        /// </summary>
        /// <param name="isActive"></param>
        public void SetGridAccess(bool isActive)
        {
            try
            {
                if (!isActive)
                {
                    grdPermissions.DisplayLayout.Bands[0].Override.AllowUpdate = DefaultableBoolean.False;
                    //grdPermissions.DisplayLayout.Bands[0].Override.AllowAddNew = DefaultableBoolean.False;
                    //grdPermissions.DisplayLayout.Bands[0].Override.AllowDelete = DefaultableBoolean.False;
                    btnAdd.Enabled = btnDelete.Enabled = false;
                }
                else
                {
                    grdPermissions.DisplayLayout.Bands[0].Override.AllowUpdate = DefaultableBoolean.True;
                    btnAdd.Enabled = btnDelete.Enabled = true;
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
