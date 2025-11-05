using Prana.Admin.BLL;
using Prana.ThirdPartyManager.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Prana.Admin.Controls
{
    public partial class CompanyThirdPartyAccounts : UserControl
    {
        #region Wizard Stuff

        //private Infragistics.Win.Misc.UltraLabel lblThirdParty;
        //private Infragistics.Win.Misc.UltraLabel lblThirdPartyType;
        //private Infragistics.Win.Misc.UltraLabel lblThirdPartyName;
        //private Infragistics.Win.Misc.UltraLabel lblTPType;
        //private Infragistics.Win.Misc.UltraLabel lblCompanyAccounts;
        //private Infragistics.Win.Misc.UltraLabel lblThirdPartyuAccounts;
        //private Infragistics.Win.Misc.UltraGroupBox grpTPAccounts;
        //private UltraListView ListCompanyAccounts;
        //private UltraListView listThirdPartyAccount;
        //private Infragistics.Win.Misc.UltraGroupBox grpTPAccountMap;
        //private Infragistics.Win.Misc.UltraButton btnAllUnSelect;
        //private Infragistics.Win.Misc.UltraButton btnAllSelect;
        //private Infragistics.Win.Misc.UltraButton btnSingleUnselect;
        //private Infragistics.Win.Misc.UltraButton btnSingleSelect;

        //private void InitializeComponent()
        //{
        //    Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
        //    Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
        //    Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
        //    Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
        //    Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
        //    Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
        //    this.lblThirdParty = new Infragistics.Win.Misc.UltraLabel();
        //    this.lblThirdPartyType = new Infragistics.Win.Misc.UltraLabel();
        //    this.lblThirdPartyName = new Infragistics.Win.Misc.UltraLabel();
        //    this.lblTPType = new Infragistics.Win.Misc.UltraLabel();
        //    this.lblCompanyAccounts = new Infragistics.Win.Misc.UltraLabel();
        //    this.lblThirdPartyuAccounts = new Infragistics.Win.Misc.UltraLabel();
        //    this.grpTPAccounts = new Infragistics.Win.Misc.UltraGroupBox();
        //    this.ListCompanyAccounts = new Infragistics.Win.UltraWinListView.UltraListView();
        //    this.listThirdPartyAccount = new Infragistics.Win.UltraWinListView.UltraListView();
        //    this.grpTPAccountMap = new Infragistics.Win.Misc.UltraGroupBox();
        //    this.btnAllUnSelect = new Infragistics.Win.Misc.UltraButton();
        //    this.btnAllSelect = new Infragistics.Win.Misc.UltraButton();
        //    this.btnSingleUnselect = new Infragistics.Win.Misc.UltraButton();
        //    this.btnSingleSelect = new Infragistics.Win.Misc.UltraButton();
        //    ((System.ComponentModel.ISupportInitialize)(this.grpTPAccounts)).BeginInit();
        //    this.grpTPAccounts.SuspendLayout();
        //    ((System.ComponentModel.ISupportInitialize)(this.ListCompanyAccounts)).BeginInit();
        //    ((System.ComponentModel.ISupportInitialize)(this.listThirdPartyAccount)).BeginInit();
        //    ((System.ComponentModel.ISupportInitialize)(this.grpTPAccountMap)).BeginInit();
        //    this.grpTPAccountMap.SuspendLayout();
        //    this.SuspendLayout();
        //    // 
        //    // lblThirdParty
        //    // 
        //    appearance19.TextHAlign = Infragistics.Win.HAlign.Center;
        //    appearance19.TextVAlign = Infragistics.Win.VAlign.Middle;
        //    this.lblThirdParty.Appearance = appearance19;
        //    this.lblThirdParty.Location = new System.Drawing.Point(15, 15);
        //    this.lblThirdParty.Name = "lblThirdParty";
        //    this.lblThirdParty.Size = new System.Drawing.Size(66, 14);
        //    this.lblThirdParty.TabIndex = 0;
        //    this.lblThirdParty.Text = "Third Party :";
        //    // 
        //    // lblThirdPartyType
        //    // 
        //    appearance20.TextHAlign = Infragistics.Win.HAlign.Center;
        //    appearance20.TextVAlign = Infragistics.Win.VAlign.Middle;
        //    this.lblThirdPartyType.Appearance = appearance20;
        //    this.lblThirdPartyType.Location = new System.Drawing.Point(258, 17);
        //    this.lblThirdPartyType.Name = "lblThirdPartyType";
        //    this.lblThirdPartyType.Size = new System.Drawing.Size(94, 14);
        //    this.lblThirdPartyType.TabIndex = 1;
        //    this.lblThirdPartyType.Text = "Third Party Type : ";
        //    // 
        //    // lblThirdPartyName
        //    // 
        //    appearance21.TextHAlign = Infragistics.Win.HAlign.Center;
        //    appearance21.TextVAlign = Infragistics.Win.VAlign.Middle;
        //    this.lblThirdPartyName.Appearance = appearance21;
        //    this.lblThirdPartyName.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
        //    this.lblThirdPartyName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        //    this.lblThirdPartyName.Location = new System.Drawing.Point(87, 14);
        //    this.lblThirdPartyName.Name = "lblThirdPartyName";
        //    this.lblThirdPartyName.Size = new System.Drawing.Size(37, 16);
        //    this.lblThirdPartyName.TabIndex = 2;
        //    this.lblThirdPartyName.Text = "Name";
        //    // 
        //    // lblTPType
        //    // 
        //    appearance22.TextHAlign = Infragistics.Win.HAlign.Center;
        //    appearance22.TextVAlign = Infragistics.Win.VAlign.Middle;
        //    this.lblTPType.Appearance = appearance22;
        //    this.lblTPType.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
        //    this.lblTPType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        //    this.lblTPType.Location = new System.Drawing.Point(358, 15);
        //    this.lblTPType.Name = "lblTPType";
        //    this.lblTPType.Size = new System.Drawing.Size(32, 16);
        //    this.lblTPType.TabIndex = 3;
        //    this.lblTPType.Text = "Type";
        //    // 
        //    // lblCompanyAccounts
        //    // 
        //    appearance23.TextHAlign = Infragistics.Win.HAlign.Center;
        //    appearance23.TextVAlign = Infragistics.Win.VAlign.Middle;
        //    this.lblCompanyAccounts.Appearance = appearance23;
        //    this.lblCompanyAccounts.Location = new System.Drawing.Point(9, 11);
        //    this.lblCompanyAccounts.Name = "lblCompanyAccounts";
        //    this.lblCompanyAccounts.Size = new System.Drawing.Size(93, 14);
        //    this.lblCompanyAccounts.TabIndex = 4;
        //    this.lblCompanyAccounts.Text = "Company Accounts :";
        //    // 
        //    // lblThirdPartyuAccounts
        //    // 
        //    appearance24.TextHAlign = Infragistics.Win.HAlign.Center;
        //    appearance24.TextVAlign = Infragistics.Win.VAlign.Middle;
        //    this.lblThirdPartyuAccounts.Appearance = appearance24;
        //    this.lblThirdPartyuAccounts.Location = new System.Drawing.Point(260, 11);
        //    this.lblThirdPartyuAccounts.Name = "lblThirdPartyuAccounts";
        //    this.lblThirdPartyuAccounts.Size = new System.Drawing.Size(100, 14);
        //    this.lblThirdPartyuAccounts.TabIndex = 5;
        //    this.lblThirdPartyuAccounts.Text = "Third Party Accounts :";
        //    // 
        //    // grpTPAccounts
        //    // 
        //    this.grpTPAccounts.Controls.Add(this.lblThirdPartyName);
        //    this.grpTPAccounts.Controls.Add(this.lblThirdParty);
        //    this.grpTPAccounts.Controls.Add(this.lblTPType);
        //    this.grpTPAccounts.Controls.Add(this.lblThirdPartyType);
        //    this.grpTPAccounts.Location = new System.Drawing.Point(4, 6);
        //    this.grpTPAccounts.Name = "grpTPAccounts";
        //    this.grpTPAccounts.Size = new System.Drawing.Size(481, 40);
        //    this.grpTPAccounts.SupportThemes = false;
        //    this.grpTPAccounts.TabIndex = 6;
        //    // 
        //    // ListCompanyAccounts
        //    // 
        //    this.ListCompanyAccounts.Location = new System.Drawing.Point(9, 30);
        //    this.ListCompanyAccounts.Name = "ListCompanyAccounts";
        //    this.ListCompanyAccounts.Size = new System.Drawing.Size(121, 209);
        //    this.ListCompanyAccounts.TabIndex = 7;
        //    this.ListCompanyAccounts.Text = "ultraListView1";
        //    // 
        //    // listThirdPartyAccount
        //    // 
        //    this.listThirdPartyAccount.Location = new System.Drawing.Point(260, 31);
        //    this.listThirdPartyAccount.Name = "listThirdPartyAccount";
        //    this.listThirdPartyAccount.Size = new System.Drawing.Size(124, 208);
        //    this.listThirdPartyAccount.TabIndex = 8;
        //    this.listThirdPartyAccount.Text = "ultraListView2";
        //    // 
        //    // grpTPAccountMap
        //    // 
        //    this.grpTPAccountMap.Controls.Add(this.btnAllUnSelect);
        //    this.grpTPAccountMap.Controls.Add(this.btnAllSelect);
        //    this.grpTPAccountMap.Controls.Add(this.btnSingleUnselect);
        //    this.grpTPAccountMap.Controls.Add(this.btnSingleSelect);
        //    this.grpTPAccountMap.Controls.Add(this.listThirdPartyAccount);
        //    this.grpTPAccountMap.Controls.Add(this.lblCompanyAccounts);
        //    this.grpTPAccountMap.Controls.Add(this.ListCompanyAccounts);
        //    this.grpTPAccountMap.Controls.Add(this.lblThirdPartyuAccounts);
        //    this.grpTPAccountMap.Location = new System.Drawing.Point(47, 52);
        //    this.grpTPAccountMap.Name = "grpTPAccountMap";
        //    this.grpTPAccountMap.Size = new System.Drawing.Size(395, 249);
        //    this.grpTPAccountMap.SupportThemes = false;
        //    this.grpTPAccountMap.TabIndex = 9;
        //    // 
        //    // btnAllUnSelect
        //    // 
        //    this.btnAllUnSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        //    this.btnAllUnSelect.Location = new System.Drawing.Point(180, 177);
        //    this.btnAllUnSelect.Name = "btnAllUnSelect";
        //    this.btnAllUnSelect.Size = new System.Drawing.Size(35, 23);
        //    this.btnAllUnSelect.TabIndex = 12;
        //    this.btnAllUnSelect.Text = "<<";
        //    this.btnAllUnSelect.Click += new System.EventHandler(this.btnAllUnSelect_Click);
        //    // 
        //    // btnAllSelect
        //    // 
        //    this.btnAllSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        //    this.btnAllSelect.Location = new System.Drawing.Point(180, 142);
        //    this.btnAllSelect.Name = "btnAllSelect";
        //    this.btnAllSelect.Size = new System.Drawing.Size(35, 23);
        //    this.btnAllSelect.TabIndex = 11;
        //    this.btnAllSelect.Text = ">>";
        //    this.btnAllSelect.Click += new System.EventHandler(this.btnAllSelect_Click);
        //    // 
        //    // btnSingleUnselect
        //    // 
        //    this.btnSingleUnselect.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        //    this.btnSingleUnselect.Location = new System.Drawing.Point(180, 107);
        //    this.btnSingleUnselect.Name = "btnSingleUnselect";
        //    this.btnSingleUnselect.Size = new System.Drawing.Size(35, 23);
        //    this.btnSingleUnselect.TabIndex = 10;
        //    this.btnSingleUnselect.Text = "<";
        //    this.btnSingleUnselect.Click += new System.EventHandler(this.btnSingleUnselect_Click);
        //    // 
        //    // btnSingleSelect
        //    // 
        //    this.btnSingleSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        //    this.btnSingleSelect.Location = new System.Drawing.Point(180, 72);
        //    this.btnSingleSelect.Name = "btnSingleSelect";
        //    this.btnSingleSelect.Size = new System.Drawing.Size(35, 23);
        //    this.btnSingleSelect.TabIndex = 9;
        //    this.btnSingleSelect.Text = ">";
        //    this.btnSingleSelect.Click += new System.EventHandler(this.btnSingleSelect_Click);
        //    // 
        //    // CompanyThirdPartyAccounts
        //    // 
        //    this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        //    this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        //    this.Controls.Add(this.grpTPAccountMap);
        //    this.Controls.Add(this.grpTPAccounts);
        //    this.Name = "CompanyThirdPartyAccounts";
        //    this.Size = new System.Drawing.Size(488, 306);
        //    ((System.ComponentModel.ISupportInitialize)(this.grpTPAccounts)).EndInit();
        //    this.grpTPAccounts.ResumeLayout(false);
        //    ((System.ComponentModel.ISupportInitialize)(this.ListCompanyAccounts)).EndInit();
        //    ((System.ComponentModel.ISupportInitialize)(this.listThirdPartyAccount)).EndInit();
        //    ((System.ComponentModel.ISupportInitialize)(this.grpTPAccountMap)).EndInit();
        //    this.grpTPAccountMap.ResumeLayout(false);
        //    this.ResumeLayout(false);

        //}

        #endregion Wizard Stuff 

        public CompanyThirdPartyAccounts()
        {
            InitializeComponent();
        }

        private int _companyID = int.MinValue;
        private int _thirdPartyID = int.MinValue;
        DataTable dtCompanyaccounts = null;

        DataTable dtSelectedAccounts = new DataTable();
        public int CompanyID
        {
            set { _companyID = value; }
        }

        public int ThirdPartyID
        {
            set { _thirdPartyID = value; }
        }

        public Prana.BusinessObjects.ThirdPartyPermittedAccounts SetThirdPartyPermittedAccounts
        {
            set
            {
                SettingThirdPartyPermittedAccounts(value);
            }
        }

        /// <summary>
        /// This is used to set the fetched data in controls for display.
        /// </summary>
        /// <param name="thirdPartyPermittedAccounts"></param>
        private void SettingThirdPartyPermittedAccounts(Prana.BusinessObjects.ThirdPartyPermittedAccounts thirdPartyPermittedAccounts)
        {
            //dtCompanyaccounts.Rows.Clear();
            //dtSelectedAccounts.Rows.Clear();
            if (_thirdPartyID != int.MinValue)
            {

                Prana.BusinessObjects.ThirdParty thirdParty = ThirdPartyDataManager.GetCompanyThirdParty(_thirdPartyID);
                lblThirdPartyName.Text = thirdParty.ShortName;
                lblThirdPartyName.AutoSize = true;
                int thirdPartyTypeID = ThirdPartyDataManager.GetThirdPartyTypeId(thirdParty);
                Prana.BusinessObjects.ThirdPartyType thirdPartyType = ThirdPartyDataManager.GetThirdPartyType(thirdPartyTypeID);
                lblTPType.Text = thirdPartyType.ThirdPartyTypeName;
                lblTPType.AutoSize = true;

                if (thirdPartyPermittedAccounts.Count > 0)
                {
                    BindThirdPartyAccountList();
                    BindCompanyUnallocatedAccounts();
                }
                else
                {
                    BindCompanyAccountsList();
                    listThirdPartyAccounts.Items.Clear();
                }

            }
            else
            {
                lblThirdPartyName.Text = "";
                lblTPType.Text = "";
                BindCompanyAccountsList();
                listThirdPartyAccounts.Items.Clear();

            }

        }

        /// <summary>
        /// This is used to Fill the Listbox with all the Accounts of the selected company.
        /// </summary>
        private void BindCompanyAccountsList()
        {
            dtCompanyaccounts.Columns.Add("Data");
            dtCompanyaccounts.Columns.Add("Value");

            object[] row = new object[2];
            Accounts accounts = CompanyManager.GetAccount(_companyID);
            if (accounts.Count > 0)
            {
                foreach (Account account in accounts)
                {
                    string data = account.AccountShortName;
                    int value = account.AccountID;
                    row[0] = data;
                    row[1] = value;

                    dtCompanyaccounts.Rows.Add(row);
                }
                listCompanyAccounts.DataSource = dtCompanyaccounts;
                listCompanyAccounts.DisplayMember = "Data";
                listCompanyAccounts.ValueMember = "Value";
            }

        }

        /// <summary>
        /// This is used to fill the ThirdParty List box with accounts permitted to that third party.
        /// </summary>
        private void BindThirdPartyAccountList()
        {
            dtSelectedAccounts.Columns.Add("Data");
            dtSelectedAccounts.Columns.Add("Value");

            object[] row = new object[2];

            List <BusinessObjects.ThirdPartyPermittedAccount> thirdPartyPermittedAccounts = ThirdPartyDataManager.GetThirdPartyPermittedAccounts(_thirdPartyID);
            if (thirdPartyPermittedAccounts.Count > 0)
            {
                foreach (Prana.BusinessObjects.ThirdPartyPermittedAccount thirdPartyPermittedAccount in thirdPartyPermittedAccounts)
                {
                    string data = thirdPartyPermittedAccount.AccountName;
                    int value = thirdPartyPermittedAccount.CompanyAccountID;

                    row[0] = data;
                    row[1] = value;

                    dtSelectedAccounts.Rows.Add(row);
                }


                listThirdPartyAccounts.DataSource = dtSelectedAccounts;
                listThirdPartyAccounts.DisplayMember = "Data";
                listThirdPartyAccounts.ValueMember = "Value";
            }

        }

        /// <summary>
        /// This is used to populate the Company Account List with accounts unallocated w.r.t thirdParty Type.
        /// </summary>
        private void BindCompanyUnallocatedAccounts()
        {
            Prana.BusinessObjects.ThirdParty thirdParty = ThirdPartyDataManager.GetCompanyThirdParty(_thirdPartyID);
            int thirdPartyTypeID = ThirdPartyDataManager.GetThirdPartyTypeId(thirdParty);

            string thirdPartyType = lblTPType.Text;

            Accounts accounts = new Accounts();
            listCompanyAccounts.Items.Clear();

            dtCompanyaccounts.Columns.Add("data");
            dtCompanyaccounts.Columns.Add("value");
            // Add an array of row to the datatable.
            object[] row = new object[2];

            if (thirdPartyType != "PrimeBroker")
            {
                //ThirdParty could be custodian or administrator.
                accounts = CompanyManager.GetCompanyAccountsNotPermittedToThirdParty(_companyID, _thirdPartyID);
                foreach (Account account in accounts)
                {
                    string data = account.AccountShortName;
                    int value = account.AccountID;
                    row[0] = data;
                    row[1] = value;

                    dtCompanyaccounts.Rows.Add(row);
                }
                listCompanyAccounts.DataSource = dtCompanyaccounts;
                listCompanyAccounts.DisplayMember = "data";
                listCompanyAccounts.ValueMember = "value";


            }
            else
            {
                //Thirdparty is PrimeBroker.

                accounts = CompanyManager.GetThirdPartyCompanyUnallocatedAccounts(_companyID, thirdPartyTypeID);

                foreach (Account account in accounts)
                {
                    string data = account.AccountShortName;
                    int value = account.AccountID;
                    row[0] = data;
                    row[1] = value;

                    dtCompanyaccounts.Rows.Add(row);
                }
                listCompanyAccounts.DataSource = dtCompanyaccounts;
                listCompanyAccounts.DisplayMember = "data";
                listCompanyAccounts.ValueMember = "value";
            }




        }

        /// <summary>
        /// This used to save the ThirdPartyPermittedAccounts details.
        /// </summary>
        /// <param name="thirdPartyPermittedAccounts"></param>
        private void SaveThirdPartyPermittedAccounts(Prana.BusinessObjects.ThirdPartyPermittedAccounts thirdPartyPermittedAccounts)
        {

            if (_thirdPartyID != int.MinValue)
            {
                if (listThirdPartyAccounts.Items.Count > 0)
                {
                    Prana.BusinessObjects.ThirdPartyPermittedAccount thirdPartyPermittedAccount = new Prana.BusinessObjects.ThirdPartyPermittedAccount();

                    for (int i = 0, count = listThirdPartyAccounts.Items.Count; i < count; i++)
                    {
                        int accountID = (((Prana.BusinessObjects.ThirdPartyPermittedAccount)listThirdPartyAccounts.Items[i]).CompanyAccountID);
                        thirdPartyPermittedAccounts.Add(new Prana.BusinessObjects.ThirdPartyPermittedAccount(_thirdPartyID, accountID));
                    }

                    ThirdPartyDataManager.SaveThirdPartyPermittedAccounts(thirdPartyPermittedAccounts, _thirdPartyID);
                }
                else
                {
                    MessageBox.Show("Please select accounts for Third Party !");
                }
            }

        }

        /// <summary>
        /// the event is used to add all the selected companyAccounts to a Thirdparty Accounts list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSingleSelect_Click_1(object sender, EventArgs e)
        {
            if (listCompanyAccounts.SelectedItems.Count > 0)
            {
                for (int i = 0, count = listCompanyAccounts.SelectedItems.Count; i < count; i++)
                {

                    listThirdPartyAccounts.Items.Add(listCompanyAccounts.SelectedItems[i]);
                    listCompanyAccounts.Items.Remove(listCompanyAccounts.SelectedItems[i]);

                }

            }
        }

        /// <summary>
        /// this is used to remove all the selected thirdpartyaccounts and add them back to company account list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSingleUnselect_Click(object sender, EventArgs e)
        {
            if (listThirdPartyAccounts.SelectedItems.Count > 0)
            {
                for (int i = 0, count = listThirdPartyAccounts.SelectedItems.Count; i < count; i++)
                {
                    listCompanyAccounts.Items.Add(listThirdPartyAccounts.SelectedItems[i]);
                    listThirdPartyAccounts.Items.Remove(listThirdPartyAccounts.SelectedItems[i]);
                }
            }
        }

        /// <summary>
        /// The event is used to select all the available accounts of a company for a thirdParty.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAllSelect_Click_1(object sender, EventArgs e)
        {
            if (listCompanyAccounts.Items.Count > 0)
            {
                for (int i = 0, count = listCompanyAccounts.Items.Count; i < count; i++)
                {
                    listThirdPartyAccounts.Items.Add(listCompanyAccounts.Items[i]);
                    listCompanyAccounts.Items.Remove(listCompanyAccounts.Items[i]);
                }

            }
        }

        /// <summary>
        /// this event is used to unselect all the selected accounts for a thirdParty.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAllUnSelect_Click_1(object sender, EventArgs e)
        {
            if (listThirdPartyAccounts.Items.Count > 0)
            {
                for (int i = 0, count = listThirdPartyAccounts.Items.Count; i < count; i++)
                {
                    listCompanyAccounts.Items.Add(listThirdPartyAccounts.Items[i]);
                    listThirdPartyAccounts.Items.Remove(listThirdPartyAccounts.Items[i]);
                }

            }
        }

        /// <summary>
        /// This is to validate the control before saving.
        /// </summary>
        /// <returns></returns>
        public bool ValidationChk()
        {
            bool IsValidated = false;
            if (listThirdPartyAccounts.Items.Count == 0)
            {
                MessageBox.Show("Please Select Third Party Accounts");
                return IsValidated;
            }
            else
            {
                IsValidated = true;
                return IsValidated;
            }
        }

        public void ClearCtrl()
        {
            listThirdPartyAccounts.DataSource = null;
            listThirdPartyAccounts.Items.Clear();

            listCompanyAccounts.DataSource = null;
            //listCompanyAccounts.DataSource = availableColumnList;
            //BindCompanyAccountsList();

        }

        public DataTable SaveThirdPartyAccounts()
        {
            DataTable dtThirdPartyAccounts = new DataTable();
            //Add columns to the datatable.
            dtThirdPartyAccounts.Columns.Add("Data");
            dtThirdPartyAccounts.Columns.Add("Value");

            // Add an array of row to the datatable.
            object[] row = new object[2];
            for (int i = 0, count = listThirdPartyAccounts.Items.Count; i < count; i++)
            {
                string Data = listThirdPartyAccounts.DisplayMember[i].ToString();
                int value = listThirdPartyAccounts.ValueMember[i];
                row[0] = Data;
                row[1] = value;

                dtThirdPartyAccounts.Rows.Add(row);
            }

            return dtThirdPartyAccounts;
        }
    }
}
