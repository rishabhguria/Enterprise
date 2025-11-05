using Prana.Admin.BLL;
using Prana.ThirdPartyManager.DataAccess;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Prana.Admin.Controls.ThirdParty
{
    public partial class ThirdPartyVendor : UserControl
    {
        const string C_COMBO_SELECT = "- Select -";
        const int TYPE_VENDOR = 2;

        public ThirdPartyVendor()
        {
            InitializeComponent();
        }

        private void BindThirdPartyType()
        {
            List<BusinessObjects.ThirdPartyType> thirdPartyTypes = ThirdPartyDataManager.GetThirdPartyTypes();
            Prana.BusinessObjects.ThirdPartyTypes thirdPartyVendorTypes = new Prana.BusinessObjects.ThirdPartyTypes();
            thirdPartyVendorTypes.Insert(0, new Prana.BusinessObjects.ThirdPartyType(int.MinValue, C_COMBO_SELECT));
            foreach (Prana.BusinessObjects.ThirdPartyType thirdPartyType in thirdPartyTypes)
            {
                if (thirdPartyType.ThirdPartyTypeID == TYPE_VENDOR)
                {
                    thirdPartyVendorTypes.Add(thirdPartyType);
                    break;
                }
            }
            cmbThirdPartyType.DataSource = null;
            cmbThirdPartyType.DataSource = thirdPartyVendorTypes;
            cmbThirdPartyType.DisplayMember = "ThirdPartyTypeName";
            cmbThirdPartyType.ValueMember = "ThirdPartyTypeID";
            foreach (Infragistics.Win.UltraWinGrid.UltraGridColumn column in cmbThirdPartyType.DisplayLayout.Bands[0].Columns)
            {
                if (!column.Key.Equals("ThirdPartyTypeName"))
                {
                    column.Hidden = true;
                }
            }
            cmbThirdPartyType.DisplayLayout.Bands[0].ColHeadersVisible = false;

        }

        public Prana.BusinessObjects.ThirdParty VendorProperty
        {
            get
            {
                Prana.BusinessObjects.ThirdParty vendor = new Prana.BusinessObjects.ThirdParty();
                GetVendorDetailsForSave(vendor);
                return vendor;
            }
            set
            {
                //SetVendorDetails(value);
            }
        }

        public void SetupControl()
        {
            BindThirdPartyType();
            BindCountries();
            BindStates();
        }

        /// <summary>
        /// This method binds the existing <see cref="Countries"/> in the ComboBox control by assigning the 
        /// countries object to its datasource property.
        /// </summary>
        private void BindCountries()
        {
            //GetCountries method fetches the existing countries from the database.
            Countries countries = GeneralManager.GetCountries();
            //Inserting the - Select - option in the Combo Box at the top.
            countries.Insert(0, new Country(int.MinValue, C_COMBO_SELECT));
            cmbCountry.DisplayMember = "Name";
            cmbCountry.ValueMember = "CountryID";
            cmbCountry.DataSource = null;
            cmbCountry.DataSource = countries;
            cmbCountry.Value = int.MinValue;
            foreach (Infragistics.Win.UltraWinGrid.UltraGridColumn column in cmbCountry.DisplayLayout.Bands[0].Columns)
            {
                if (!column.Key.Equals("Name"))
                {
                    column.Hidden = true;
                }
            }
            cmbCountry.DisplayLayout.Bands[0].ColHeadersVisible = false;
        }

        /// <summary>
        /// This method binds the existing <see cref="States"/> in the ComboBox control by assigning the 
        /// states object to its datasource property.
        /// </summary>
        private void BindStates()
        {
            //GetStates method fetches the existing states from the database.
            States states = GeneralManager.GetStates();
            //Inserting the - Select - option in the Combo Box at the top.
            states.Insert(0, new State(int.MinValue, C_COMBO_SELECT, int.MinValue));
            cmbState.DisplayMember = "StateName";
            cmbState.ValueMember = "StateID";
            cmbState.DataSource = null;
            cmbState.DataSource = states;
            cmbState.Value = int.MinValue;
            foreach (Infragistics.Win.UltraWinGrid.UltraGridColumn column in cmbState.DisplayLayout.Bands[0].Columns)
            {
                if (!column.Key.Equals("StateName"))
                {
                    column.Hidden = true;
                }
            }
            cmbState.DisplayLayout.Bands[0].ColHeadersVisible = false;
        }

        /// <summary>
        /// This method empties the ComboBox from any state by assigning the states object to null value.
        /// </summary>
        private void BindEmptyStates()
        {
            //GetStates method fetches the existing states from the database.
            States states = new States();
            //Inserting the - Select - option in the Combo Box at the top.
            states.Insert(0, new State(int.MinValue, C_COMBO_SELECT, int.MinValue));
            cmbState.DisplayMember = "StateName";
            cmbState.ValueMember = "StateID";
            cmbState.DataSource = null;
            cmbState.DataSource = states;
            cmbState.Value = int.MinValue;
            foreach (Infragistics.Win.UltraWinGrid.UltraGridColumn column in cmbState.DisplayLayout.Bands[0].Columns)
            {
                if (!column.Key.Equals("StateName"))
                {
                    column.Hidden = true;
                }
            }
            cmbState.DisplayLayout.Bands[0].ColHeadersVisible = false;
        }

        public bool GetVendorDetailsForSave(Prana.BusinessObjects.ThirdParty thirdParty)
        {
            bool result = false;
            //Regex emailRegex = new Regex("(?<user>[^@]+)@(?<host>.+)");
            string emailCheck = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
            Regex emailRegex = new Regex(emailCheck);
            Match emailMatch = emailRegex.Match(txtEmail.Text.ToString());

            errorProvider1.SetError(txtThirdPartyName, "");
            errorProvider1.SetError(txtShortName, "");
            errorProvider1.SetError(cmbThirdPartyType, "");
            errorProvider1.SetError(txtAddress1, "");
            errorProvider1.SetError(txtContactPerson, "");
            errorProvider1.SetError(txtWorkTele, "");
            errorProvider1.SetError(txtEmail, "");
            errorProvider1.SetError(cmbCountry, "");
            errorProvider1.SetError(cmbState, "");
            errorProvider1.SetError(txtPCWorkTele, "");

            if (txtThirdPartyName.Text.Trim() == "")
            {
                errorProvider1.SetError(txtThirdPartyName, "Please enter display name!");
                txtThirdPartyName.Focus();
            }
            else if (int.Parse(cmbThirdPartyType.Value.ToString()) == int.MinValue)
            {
                errorProvider1.SetError(cmbThirdPartyType, "Please vendor type!");
                cmbThirdPartyType.Focus();
            }
            else if (txtShortName.Text.Trim() == "")
            {
                errorProvider1.SetError(txtShortName, "Please enter short name!");
                txtShortName.Focus();
            }
            else if (txtAddress1.Text.Trim() == "")
            {
                errorProvider1.SetError(txtAddress1, "Please enter Address1!");
                txtAddress1.Focus();
            }
            else if (int.Parse(cmbCountry.Value.ToString()) == int.MinValue)
            {
                errorProvider1.SetError(cmbCountry, "Please select Country!");
                cmbCountry.Focus();
            }
            else if (int.Parse(cmbState.Value.ToString()) == int.MinValue)
            {
                errorProvider1.SetError(cmbState, "Please select State!");
                cmbState.Focus();
            }
            else if (txtWorkTele.Text.Trim() == "")
            {
                errorProvider1.SetError(txtWorkTele, "Please enter Work phone no!");
                txtWorkTele.Focus();
            }
            else if (txtContactPerson.Text.Trim() == "")
            {
                errorProvider1.SetError(txtContactPerson, "Please enter Primary Contact person first name!");
                txtContactPerson.Focus();
            }
            //else if (txtEmail.Text.Trim() == "")
            else if (!emailMatch.Success)
            {
                errorProvider1.SetError(txtEmail, "Please enter valid Email address!");
                txtEmail.Focus();
            }
            else if (txtPCWorkTele.Text.Trim() == "")
            {
                errorProvider1.SetError(txtPCWorkTele, "Please enter Primary Contact Work phone no!");
                txtPCWorkTele.Focus();
            }

            else
            {
                thirdParty.ThirdPartyName = txtThirdPartyName.Text.ToString();
                thirdParty.ThirdPartyTypeID = int.Parse(cmbThirdPartyType.Value.ToString());
                thirdParty.ShortName = txtShortName.Text.ToString();
                thirdParty.Address1 = txtAddress1.Text.ToString();
                thirdParty.Address2 = txtAddress2.Text.ToString();
                thirdParty.ContactPerson = txtContactPerson.Text.ToString();
                thirdParty.CellPhone = txtCellPhone.Text.ToString();
                thirdParty.WorkTelephone = txtWorkTele.Text.ToString();
                thirdParty.Fax = txtCFax.Text.ToString();
                thirdParty.Email = txtEmail.Text.ToString();
                thirdParty.CountryID = int.Parse(cmbCountry.Value.ToString());
                thirdParty.StateID = int.Parse(cmbState.Value.ToString());
                thirdParty.Zip = txtZip.Text.ToString();

                thirdParty.PrimaryContactLastName = txtLastName.Text.ToString();
                thirdParty.PrimaryContactTitle = txtTitle.Text.ToString();
                thirdParty.PrimaryContactWorkTelephone = txtPCWorkTele.Text.ToString();
                thirdParty.PrimaryContactFax = txtPCFax.Text.ToString();
                return result = true;
            }
            return result;
        }

        public void SetVendorDetails(Prana.BusinessObjects.ThirdParty thirdParty)
        {
            txtThirdPartyName.Text = thirdParty.ThirdPartyName;
            cmbThirdPartyType.Value = ThirdPartyDataManager.GetThirdPartyTypeId(thirdParty);
            txtShortName.Text = thirdParty.ShortName;
            txtAddress1.Text = thirdParty.Address1;
            txtAddress2.Text = thirdParty.Address2;
            txtContactPerson.Text = thirdParty.ContactPerson;
            txtCellPhone.Text = thirdParty.CellPhone;
            txtWorkTele.Text = thirdParty.WorkTelephone;
            txtCFax.Text = thirdParty.Fax;
            txtEmail.Text = thirdParty.Email;
            cmbCountry.Value = int.Parse(thirdParty.CountryID.ToString());
            if (int.Parse(cmbCountry.Value.ToString()) <= 0)
            {
                BindEmptyStates();
            }
            txtPCWorkTele.Text = thirdParty.PrimaryContactWorkTelephone;
            cmbState.Value = int.Parse(thirdParty.StateID.ToString());
            txtZip.Text = thirdParty.Zip;

            txtPCFax.Text = thirdParty.PrimaryContactFax;
            txtTitle.Text = thirdParty.PrimaryContactTitle;
            txtLastName.Text = thirdParty.PrimaryContactLastName;
        }

        public void RefreshVendorDetails()
        {
            txtAddress1.Text = "";
            txtAddress2.Text = "";
            txtCellPhone.Text = "";
            txtContactPerson.Text = "";
            txtEmail.Text = "";
            txtCFax.Text = "";
            txtShortName.Text = "";
            txtThirdPartyName.Text = "";
            txtWorkTele.Text = "";
            cmbCountry.Value = int.MinValue;
            cmbState.Value = int.MinValue;
            txtZip.Text = "";

            txtPCFax.Text = "";
            txtTitle.Text = "";
            txtLastName.Text = "";
        }



        private void cmbCountry_ValueChanged(object sender, System.EventArgs e)
        {
            if (cmbCountry.Value != null)
            {
                int countryID = int.Parse(cmbCountry.Value.ToString());
                if (countryID > 0)
                {
                    //GetStates method fetches the existing states from the database.
                    States states = GeneralManager.GetStates(countryID);
                    if (states.Count > 0)
                    {
                        states.Insert(0, new State(int.MinValue, C_COMBO_SELECT, int.MinValue));
                        cmbState.DisplayMember = "StateName";
                        cmbState.ValueMember = "StateID";
                        cmbState.DataSource = null;
                        cmbState.DataSource = states;
                        cmbState.Text = C_COMBO_SELECT;
                        foreach (Infragistics.Win.UltraWinGrid.UltraGridColumn column in cmbState.DisplayLayout.Bands[0].Columns)
                        {
                            if (!column.Key.Equals("StateName"))
                            {
                                column.Hidden = true;
                            }
                        }
                        cmbState.DisplayLayout.Bands[0].ColHeadersVisible = false;
                    }
                }
                else
                {
                    BindEmptyStates();
                }

            }
        }

        public void DisableVendorControls()
        {
            txtAddress1.Enabled = false;
            txtAddress2.Enabled = false;
            txtCellPhone.Enabled = false;
            txtContactPerson.Enabled = false;
            txtEmail.Enabled = false;
            txtFax.Enabled = false;
            txtShortName.Enabled = false;
            txtThirdPartyName.Enabled = false;
            txtPCWorkTele.Enabled = false;
            cmbCountry.Enabled = false;
            cmbState.Enabled = false;
            txtZip.Enabled = false;

            txtPCFax.Enabled = false;
            txtTitle.Enabled = false;
            txtLastName.Enabled = false;
        }

        public void EnableVendorControls()
        {
            txtAddress1.Enabled = true;
            txtAddress2.Enabled = true;
            txtCellPhone.Enabled = true;
            txtContactPerson.Enabled = true;
            txtEmail.Enabled = true;
            txtFax.Enabled = true;
            txtShortName.Enabled = true;
            txtThirdPartyName.Enabled = true;
            txtPCWorkTele.Enabled = true;
            cmbCountry.Enabled = true;
            cmbState.Enabled = true;
            txtZip.Enabled = true;

            txtPCFax.Enabled = true;
            txtTitle.Enabled = true;
            txtLastName.Enabled = true;
        }

        #region Focus Colors
        private void cmbThirdPartyType_GotFocus(object sender, System.EventArgs e)
        {
            cmbThirdPartyType.Appearance.BackColor = Color.FromArgb(255, 250, 205);
        }
        private void cmbThirdPartyType_LostFocus(object sender, System.EventArgs e)
        {
            cmbThirdPartyType.Appearance.BackColor = Color.White;
        }
        private void txtAddress1_GotFocus(object sender, System.EventArgs e)
        {
            txtAddress1.BackColor = Color.LemonChiffon;
        }
        private void txtAddress1_LostFocus(object sender, System.EventArgs e)
        {
            txtAddress1.BackColor = Color.White;
        }
        private void txtAddress2_GotFocus(object sender, System.EventArgs e)
        {
            txtAddress2.BackColor = Color.LemonChiffon;
        }
        private void txtAddress2_LostFocus(object sender, System.EventArgs e)
        {
            txtAddress2.BackColor = Color.White;
        }
        private void txtCellPhone_GotFocus(object sender, System.EventArgs e)
        {
            txtCellPhone.BackColor = Color.LemonChiffon;
        }
        private void txtCellPhone_LostFocus(object sender, System.EventArgs e)
        {
            txtCellPhone.BackColor = Color.White;
        }
        private void txtContactPerson_GotFocus(object sender, System.EventArgs e)
        {
            txtContactPerson.BackColor = Color.LemonChiffon;
        }
        private void txtContactPerson_LostFocus(object sender, System.EventArgs e)
        {
            txtContactPerson.BackColor = Color.White;
        }
        private void txtEmail_GotFocus(object sender, System.EventArgs e)
        {
            txtEmail.BackColor = Color.LemonChiffon;
        }
        private void txtEmail_LostFocus(object sender, System.EventArgs e)
        {
            txtEmail.BackColor = Color.White;
        }
        private void txtCFax_GotFocus(object sender, System.EventArgs e)
        {
            txtCFax.BackColor = Color.LemonChiffon;
        }
        private void txtCFax_LostFocus(object sender, System.EventArgs e)
        {
            txtCFax.BackColor = Color.White;
        }
        private void txtShortName_GotFocus(object sender, System.EventArgs e)
        {
            txtShortName.BackColor = Color.LemonChiffon;
        }
        private void txtShortName_LostFocus(object sender, System.EventArgs e)
        {
            txtShortName.BackColor = Color.White;
        }
        private void txtThirdPartyName_GotFocus(object sender, System.EventArgs e)
        {
            txtThirdPartyName.BackColor = Color.LemonChiffon;
        }
        private void txtThirdPartyName_LostFocus(object sender, System.EventArgs e)
        {
            txtThirdPartyName.BackColor = Color.White;
        }
        private void txtWorkTele_GotFocus(object sender, System.EventArgs e)
        {
            txtWorkTele.BackColor = Color.LemonChiffon;
        }
        private void txtWorkTele_LostFocus(object sender, System.EventArgs e)
        {
            txtWorkTele.BackColor = Color.White;
        }
        private void txtZip_GotFocus(object sender, System.EventArgs e)
        {
            txtZip.BackColor = Color.LemonChiffon;
        }
        private void txtZip_LostFocus(object sender, System.EventArgs e)
        {
            txtZip.BackColor = Color.White;
        }
        private void cmbCountry_GotFocus(object sender, System.EventArgs e)
        {
            cmbCountry.BackColor = Color.LemonChiffon;
        }
        private void cmbCountry_LostFocus(object sender, System.EventArgs e)
        {
            cmbCountry.BackColor = Color.White;
        }
        private void cmbState_GotFocus(object sender, System.EventArgs e)
        {
            cmbState.BackColor = Color.LemonChiffon;
        }
        private void cmbState_LostFocus(object sender, System.EventArgs e)
        {
            cmbState.BackColor = Color.White;
        }
        #endregion
    }
}
