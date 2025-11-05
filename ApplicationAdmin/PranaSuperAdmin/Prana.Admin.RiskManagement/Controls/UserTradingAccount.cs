using Infragistics.Win.UltraWinGrid;
using Prana.Admin.BLL;
using Prana.LogManager;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Prana.Admin.RiskManagement.Controls
{
    public delegate void RMUserTAChangedHandler(System.Object sender, UTAValueEventArgs e);

    public partial class UserTradingAccount : UserControl
    {
        private int _companyID = int.MinValue;
        const string C_COMBO_SELECT = "- Select -";
        private int _tradingAccountID = int.MinValue;
        private int _userID = int.MinValue;
        private int _userIDSelected = int.MinValue;
        public UserTradingAccount()
        {
            InitializeComponent();
        }

        public int CompanyID
        {
            set { _companyID = value; }
        }

        public int TradingAccountID
        {
            set { _tradingAccountID = value; }
        }
        public event RMUserTAChangedHandler RMUTAChangedHandled;
        public int UserID
        {
            set { _userID = value; }
        }

        #region Set Method

        public Prana.Admin.BLL.UserTradingAccount SetUserTradingAccount
        {
            set { SettingUserTradingAccount(value); }
        }

        private void SettingUserTradingAccount(Prana.Admin.BLL.UserTradingAccount userTradingAccount)
        {
            BindUsersOfTradingAccnt();
            BindUserTradAccntGrid();

            if (_companyID != int.MinValue && userTradingAccount.RMUserTradingAccntID != int.MinValue)
            {
                txtRMUserTradAccntID_invisible.Text = userTradingAccount.RMUserTradingAccntID.ToString();
                txtTradAccntUserExpLt.Text = userTradingAccount.UserTAExposureLimit.ToString();
                cmbUserTradAccnt.Value = userTradingAccount.CompanyUserID;
            }
            else
            {
                if (_userID != int.MinValue)
                {
                    RefreshUserTradingAccntDetails();
                    cmbUserTradAccnt.Value = _userID;
                }
                else
                {
                    RefreshUserTradingAccntDetails();
                    cmbUserTradAccnt.Text = C_COMBO_SELECT;
                }
            }
        }

        #endregion Set Method

        #region Refresh Method
        private void RefreshUserTradingAccntDetails()
        {
            txtTradAccntUserExpLt.Text = "";
            txtRMUserTradAccntID_invisible.Text = "-1";
        }

        #endregion Refresh Method

        #region BindUsersCombo
        private void BindUsersOfTradingAccnt()
        {
            try
            {
                //GetCompanyTradingAccnts method fetches the existing Trading Accounts from the database.
                Users users = RMAdminBusinessLogic.GetUsersforTradingAccount(_tradingAccountID);

                //Inserting the - Select - option in the Combo Box at the top.
                users.Insert(0, new Prana.Admin.BLL.TradingAccount(int.MinValue, C_COMBO_SELECT));

                this.cmbUserTradAccnt.ValueChanged -= new System.EventHandler(this.cmbUserTradAccnt_ValueChanged);
                this.cmbUserTradAccnt.DataSource = null;
                this.cmbUserTradAccnt.DataSource = users;
                this.cmbUserTradAccnt.DisplayMember = "ShortName";
                this.cmbUserTradAccnt.ValueMember = "UserID";
                this.cmbUserTradAccnt.Text = C_COMBO_SELECT;
                this.cmbUserTradAccnt.ValueChanged += new System.EventHandler(this.cmbUserTradAccnt_ValueChanged);

                ColumnsCollection columns = cmbUserTradAccnt.DisplayLayout.Bands[0].Columns;
                foreach (UltraGridColumn column in columns)
                {
                    if (column.Key != "ShortName")
                    {
                        column.Hidden = true;
                    }
                    else
                    {
                        cmbUserTradAccnt.DisplayLayout.Bands[0].ColHeadersVisible = false;
                    }
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

        #endregion BindUsersCombo

        #region BindTradAccntUserGrid

        private void BindUserTradAccntGrid()
        {

            UserTradingAccounts userTradingAccounts = RMAdminBusinessLogic.GetUserTradingAccounts(_companyID, _tradingAccountID);

            //Assigning rMTradingAccounts as the datasource to the RMTradingAccounts grid's ,if it contains valid values.
            if (userTradingAccounts.Count != 0)
            {
                foreach (Prana.Admin.BLL.UserTradingAccount userTradingAccnt in userTradingAccounts)
                {

                    User user = RMAdminBusinessLogic.GetCompanyUser(userTradingAccnt.CompanyUserID);
                    string userName = user.ShortName;

                    Prana.Admin.BLL.TradingAccount tradingAccnt = RMAdminBusinessLogic.GetCompanyTradingAccntDetail(_companyID, userTradingAccnt.UserTradingAccntID);
                    string tradAccntName = tradingAccnt.TradingShortName;

                    userTradingAccnt.User = userName;
                    userTradingAccnt.TradingAccount = tradAccntName;

                }

                grdUserTradingAccnt.DataSource = userTradingAccounts;
            }
            else
            {
                // Assign an empty row to the grid.
                userTradingAccounts = new UserTradingAccounts();
                userTradingAccounts.Add(new Prana.Admin.BLL.UserTradingAccount(int.MinValue, int.MinValue, int.MinValue, int.MinValue,
                    Int64.MinValue, "", ""));
                grdUserTradingAccnt.DataSource = userTradingAccounts;
                grdUserTradingAccnt.DisplayLayout.Rows[0].Delete(false);

                // and refresh the Form controls.
                RefreshUserTradingAccntDetails();
            }
        }

        #endregion BindTradAccntUserGrid

        #region Validation

        private bool ValidateUserTradingAccnt()
        {
            bool validationSuccess = true;

            errorProvider1.SetError(cmbUserTradAccnt, "");
            errorProvider1.SetError(txtTradAccntUserExpLt, "");

            if (int.Parse(cmbUserTradAccnt.Value.ToString()) == int.MinValue)
            {
                errorProvider1.SetError(cmbUserTradAccnt, "Please select a User !");
                validationSuccess = false;
                cmbUserTradAccnt.Focus();
                return validationSuccess;
            }
            else if (!DataTypeValidation.ValidateNumeric(txtTradAccntUserExpLt.Text.Trim()))
            {
                errorProvider1.SetError(txtTradAccntUserExpLt, "Please enter only numeric value!");
                validationSuccess = false;
                txtTradAccntUserExpLt.Focus();
                return validationSuccess;

            }
            return validationSuccess;

        }

        #endregion Validation

        #region Save Method

        public int SaveUserTradingDetails(Prana.Admin.BLL.UserTradingAccount userTradAccnt, int companyID)
        {
            int result = int.MinValue;
            bool Isvalid = ValidateUserTradingAccnt();
            if (Isvalid)
            {
                // Data as input by user is assigned to the respective fields for saving to DB.
                userTradAccnt.RMUserTradingAccntID = Convert.ToInt32(txtRMUserTradAccntID_invisible.Text);
                userTradAccnt.CompanyUserID = int.Parse(cmbUserTradAccnt.Value.ToString());
                userTradAccnt.UserTAExposureLimit = int.Parse(txtTradAccntUserExpLt.Text.Trim().ToString());

                // Save method is called from the BLL i.e RMAdminBusinessLogic which inturn calls it from DAL.
                int _rMUserTradAccntID = RMAdminBusinessLogic.SaveUserTradingAccount(userTradAccnt, _companyID, _tradingAccountID);

                if (_rMUserTradAccntID == -1)
                {
                    // existing data is updated.
                }
                else // New data is saved.
                {

                }
                result = userTradAccnt.CompanyUserID;
                BindUserTradAccntGrid();
            }
            return result;


        }

        #endregion Save Method

        #region combo Value Changed event

        private void cmbUserTradAccnt_ValueChanged(object sender, EventArgs e)
        {
            int userID = int.Parse(cmbUserTradAccnt.Value.ToString());
            if (userID != _userIDSelected)
            {
                if (userID != int.MinValue)
                {
                    _userIDSelected = int.Parse(cmbUserTradAccnt.Value.ToString());

                    UTAValueEventArgs uTAValueEventArgs = new UTAValueEventArgs();
                    uTAValueEventArgs.companyUserID = this.cmbUserTradAccnt.Value.ToString();

                    if (RMUTAChangedHandled != null)
                    {
                        RMUTAChangedHandled(this, uTAValueEventArgs);
                    }

                    bool IsNew = true;
                    Infragistics.Win.UltraWinGrid.RowsCollection _rwclc = grdUserTradingAccnt.Rows;
                    for (int i = 0; i < _rwclc.Count; i++)
                    {
                        if (_userIDSelected == Convert.ToInt32(_rwclc[i].Cells["CompanyUserID"].Value.ToString()))
                        {
                            (grdUserTradingAccnt.Rows)[i].Selected = true;
                            (grdUserTradingAccnt.Rows)[i].Activate();

                            SetForm();

                            IsNew = false;
                            break;

                        }

                        grdUserTradingAccnt.Rows[i].Selected = false;

                    }
                    if (IsNew)
                    {

                        RefreshUserTradingAccntDetails();

                    }
                }
                else
                {
                    RefreshUserTradingAccntDetails();
                    errorProvider1.SetError(txtTradAccntUserExpLt, "");

                }
            }

        }


        #endregion combo Value Changed event

        #region SetForm

        private void SetForm()
        {
            txtTradAccntUserExpLt.Text = grdUserTradingAccnt.Selected.Rows[0].Cells["UserTAExposureLimit"].Text;
            txtRMUserTradAccntID_invisible.Text = grdUserTradingAccnt.Selected.Rows[0].Cells["RMUserTradingAccntID"].Text;
        }

        #endregion SetForm

        #region Active Row event

        private void grdUserTradingAccnt_AfterRowActivate(object sender, EventArgs e)
        {

        }

        #endregion Active Row event

        #region Focus Property
        private void cmbUserTradAccnt_Enter(object sender, EventArgs e)
        {
            cmbUserTradAccnt.Appearance.BackColor = Color.FromArgb(255, 250, 205);
        }

        private void cmbUserTradAccnt_Leave(object sender, EventArgs e)
        {
            cmbUserTradAccnt.Appearance.BackColor = Color.White;
        }

        private void txtTradAccntUserExpLt_Enter(object sender, EventArgs e)
        {
            txtTradAccntUserExpLt.BackColor = Color.FromArgb(255, 250, 205);
        }

        private void txtTradAccntUserExpLt_Leave(object sender, EventArgs e)
        {
            txtTradAccntUserExpLt.BackColor = Color.White;
        }
        #endregion Focus Property

        #region Text Change Validation
        private void txtTradAccntUserExpLt_TextChanged(object sender, EventArgs e)
        {
            errorProvider1.SetError(cmbUserTradAccnt, "");
            errorProvider1.SetError(txtTradAccntUserExpLt, "");
            if (txtTradAccntUserExpLt.Text != "")
            {

                if (cmbUserTradAccnt.Text == C_COMBO_SELECT)
                {
                    txtTradAccntUserExpLt.Text = "";
                    errorProvider1.SetError(cmbUserTradAccnt, "Please select a UserTradAccnt before entering the ExposureLimit !");
                    cmbUserTradAccnt.Focus();
                }
                else
                {
                    bool IsValid = DataTypeValidation.ValidateNumeric(txtTradAccntUserExpLt.Text.Trim());
                    if (!IsValid)
                    {

                        errorProvider1.SetError(txtTradAccntUserExpLt, "Please enter only numeric values for ExposureLimit!");
                        txtTradAccntUserExpLt.Focus();
                        return;
                    }
                    else
                    {
                        Int64 chkvalue = 0;
                        Int64.TryParse(txtTradAccntUserExpLt.Text, out chkvalue);
                        if (txtTradAccntUserExpLt.Text != "0" && chkvalue == 0)
                        {
                            txtTradAccntUserExpLt.Text = "";
                            errorProvider1.SetError(txtTradAccntUserExpLt, "You cannot enter a value greater than 9223372036854775807!");
                            txtTradAccntUserExpLt.Focus();
                        }
                    }
                }
            }
        }
        #endregion Text Change Validation

        #region BaseCurrencyDisplay

        public void UpdateCurrencyText(System.Object sender, PassValueEventArgs e)
        {
            lblRMCurr.Text = e.rMCurrencySymbol;

        }

        #endregion BaseCurrencyDisplay

        #region DataEntryCheck
        /// <summary>
        /// The method is used to check whether the data has ben entered or not.
        /// </summary>
        /// <returns></returns>
        public bool CheckForDataEntered()
        {
            bool IsDataInput = false;

            if (cmbUserTradAccnt.Value != null)
            {
                IsDataInput = true;
            }
            else if (txtTradAccntUserExpLt.Text != "")
            {
                IsDataInput = true;
            }

            return IsDataInput;
        }

        #endregion DataEntryCheck
    }
    #region class UTAValueEventArgs

    /// <summary>
    /// This Class is used for the event to pass the UserId of the selected user in combo to other controls
    /// </summary>
    public class UTAValueEventArgs : System.EventArgs
    {
        private String str;

        public String companyUserID
        {
            get
            {
                return (str);
            }
            set
            {
                str = value;
            }
        }

    }  // AUECValueEventArgs

    #endregion class AUECValueEventArgs
}
