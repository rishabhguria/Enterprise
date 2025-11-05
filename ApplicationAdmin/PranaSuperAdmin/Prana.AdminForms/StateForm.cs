using Infragistics.Win.UltraWinGrid;
using Prana.Admin.BLL;
using System;
using System.Windows.Forms;

namespace Prana.AdminForms
{
    public partial class StateForm : Form
    {
        public StateForm()
        {
            InitializeComponent();
        }


        public bool _checkEdit = false;
        public int _stateID = int.MinValue;
        private static object locker = new object();

        private static StateForm _frmState = null;
        public static StateForm GetInstance()
        {
            lock (locker)
            {
                if (_frmState == null)
                {
                    _frmState = new StateForm();
                }
            }
            return _frmState;
        }

        private void BindCountryCombo()
        {
            // Bind Country Combo to the Combo Box
            Prana.Admin.BLL.Countries countries = GeneralManager.GetCountries();
            if (countries.Count > 0)
            {
                this.cmbCountry.DataSource = null;
                this.cmbCountry.DataSource = countries;
                this.cmbCountry.DisplayMember = "Name";
                this.cmbCountry.ValueMember = "CountryID";
                foreach (UltraGridColumn column in cmbCountry.DisplayLayout.Bands[0].Columns)
                {
                    column.Hidden = true;
                    if (column.Key.Equals("Name"))
                    {
                        column.Hidden = false;
                    }
                }
                this.cmbCountry.SelectedRow = cmbCountry.Rows[0];
            }

        }

        private void BindStateGrid(int countryID)
        {
            Prana.Admin.BLL.States states = GeneralManager.GetStates(countryID);
            if (states.Count > 0)
            {
                this.grdState.DataSource = null;
                this.grdState.DataSource = states;

            }
            else
            {
                State state = new State(int.MinValue, string.Empty, countryID);
                States nullStates = new States();
                nullStates.Add(state);
                grdState.DataSource = null;
                grdState.DataSource = nullStates;
            }
            foreach (UltraGridColumn column in grdState.DisplayLayout.Bands[0].Columns)
            {
                if (column.Key.Equals("StateName"))
                {
                    column.Hidden = false;
                    column.Header.Caption = "State Name";
                }
                else
                {
                    column.Hidden = true;
                }
            }
        }

        private void cmbCountry_ValueChanged(object sender, EventArgs e)
        {
            int countryID = int.Parse(cmbCountry.SelectedRow.Cells["CountryID"].Value.ToString());
            BindStateGrid(countryID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
            _frmState = null;
        }

        private void StateForm_Load(object sender, EventArgs e)
        {
            BindCountryCombo();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            States states = ((States)grdState.DataSource);
            errorProvider1.SetError(txtState, "");
            if ((txtState.Text.Trim() == ""))
            {
                errorProvider1.SetError(txtState, "Provide State!");
                txtState.Focus();
            }
            else
            {
                bool flag = false;
                if (_checkEdit == false)
                {
                    foreach (State state in states)
                    {
                        if (state.StateName.Trim().ToUpper() == txtState.Text.Trim().ToUpper())
                        {
                            flag = true;
                            break;
                        }
                    }
                }
                if (flag == false)
                {
                    int countryID = int.Parse(cmbCountry.SelectedRow.Cells["CountryID"].Value.ToString());
                    State newState = new State(_stateID, txtState.Text, countryID);
                    int result = AUECManager.SaveState(newState);
                    if (result < 0)
                    {
                        MessageBox.Show("State already exists for this country");
                    }
                    else
                    {
                        AUECManager.SaveSMStateDetails(newState, result);
                        MessageBox.Show("State saved !");
                    }
                    BindStateGrid(countryID);
                    txtState.Text = "";
                }
                else
                {
                    MessageBox.Show("State already exists for this country");
                }
                _checkEdit = false;
            }


        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            txtState.Text = "";
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (grdState.Rows.Count > 0)
            {
                txtState.Text = grdState.ActiveRow.Cells["StateName"].Value.ToString();
                _stateID = int.Parse(grdState.ActiveRow.Cells["StateID"].Value.ToString());
                _checkEdit = true;
            }

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (grdState.Rows.Count > 0)
            {
                string stateName = grdState.ActiveRow.Cells["StateName"].Text.ToString();
                int countryID = int.Parse(cmbCountry.SelectedRow.Cells["CountryID"].Value.ToString());
                if (stateName != "")
                {
                    if (MessageBox.Show(this, "Do you want to delete this State?", "Prana Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        int id = int.Parse(grdState.ActiveRow.Cells["StateID"].Value.ToString());
                        AUECManager.DeleteState(id);
                        AUECManager.DeleteSM_AECCS(id, 4);
                        BindStateGrid(countryID);
                    }
                }
                else
                {
                    MessageBox.Show("No Data Available!");
                }

            }
        }

        private void StateForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _frmState = null;
        }

    }
}