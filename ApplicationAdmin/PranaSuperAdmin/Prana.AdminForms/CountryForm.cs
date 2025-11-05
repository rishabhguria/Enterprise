using Infragistics.Win.UltraWinGrid;
using Prana.Admin.BLL;
using System;
using System.Windows.Forms;

namespace Prana.AdminForms
{
    public partial class CountryForm : Form
    {
        public CountryForm()
        {
            InitializeComponent();
        }

        private static CountryForm _frmCountry = null;
        private static object locker = new object();
        public static CountryForm GetInstance()
        {
            lock (locker)
            {
                if (_frmCountry == null)
                {
                    _frmCountry = new CountryForm();
                }
            }
            return _frmCountry;
        }

        private bool _checkEdit = false;
        private int _countryID = int.MinValue;
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (grdCountry.Rows.Count > 0)
            {
                txtCountry.Text = grdCountry.ActiveRow.Cells["Name"].Value.ToString();
                _countryID = int.Parse(grdCountry.ActiveRow.Cells["CountryID"].Value.ToString());
                _checkEdit = true;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdCountry.Rows.Count > 0)
                {
                    string countryName = grdCountry.ActiveRow.Cells["Name"].Text.ToString();
                    if (countryName != "")
                    {
                        if (MessageBox.Show(this, "Do you want to delete this Country?", "Prana Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            int id = int.Parse(grdCountry.ActiveRow.Cells["CountryID"].Value.ToString());
                            AUECManager.DeleteCountry(id);
                            AUECManager.DeleteSM_AECCS(id, 3);
                            BindCountryGrid();
                        }
                    }
                    else
                    {
                        MessageBox.Show("No Data Available!");
                    }

                }
            }
            catch (Exception)
            {
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Countries countries = ((Countries)grdCountry.DataSource);
                errorProvider1.SetError(txtCountry, "");
                if ((txtCountry.Text.Trim() == ""))
                {
                    errorProvider1.SetError(txtCountry, "Provide State!");
                    txtCountry.Focus();
                }
                else
                {
                    bool flag = false;
                    if (_checkEdit == false)
                    {
                        foreach (Country country in countries)
                        {
                            if (country.Name.Trim().ToUpper() == txtCountry.Text.Trim().ToUpper())
                            {
                                flag = true;
                                break;
                            }
                        }
                    }
                    if (flag == false)
                    {
                        Country newCountry = new Country(_countryID, txtCountry.Text);
                        int result = AUECManager.SaveCountry(newCountry);
                        if (result < 0)
                        {
                            MessageBox.Show("Country already exists in the database");
                        }
                        else
                        {
                            AUECManager.SaveSMCountryDetails(newCountry, result);
                            MessageBox.Show("Country saved !");
                        }
                        BindCountryGrid();
                        txtCountry.Text = "";
                    }
                    else
                    {
                        MessageBox.Show("Country already exists in the database");
                    }
                    _checkEdit = false;
                }
            }
            catch (Exception)
            {
            }

        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            txtCountry.Text = "";
            _countryID = int.MinValue;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
            _frmCountry = null;
        }

        private void CountryForm_Load(object sender, EventArgs e)
        {
            BindCountryGrid();

        }

        private void BindCountryGrid()
        {
            Prana.Admin.BLL.Countries countries = GeneralManager.GetCountries();
            if (countries.Count > 0)
            {
                grdCountry.DataSource = null;
                grdCountry.DataSource = countries;
                foreach (UltraGridColumn column in grdCountry.DisplayLayout.Bands[0].Columns)
                {
                    if (column.Key.Equals("Name"))
                    {
                        column.Hidden = false;
                    }
                    else
                    {
                        column.Hidden = true;
                    }
                }
            }
        }

        private void CountryForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _frmCountry = null;
        }
    }
}