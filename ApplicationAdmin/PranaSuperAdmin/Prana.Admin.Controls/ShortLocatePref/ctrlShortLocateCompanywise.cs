using Infragistics.Win.UltraWinGrid;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Prana.Admin.Controls.ShortLocatePref
{
    public partial class ctrlShortLocateCompanywise : UserControl
    {
        Dictionary<int, int> grdBorrowerCustodyAccountMapping = null;
        private static DataTable GetdatafromDB = null;

        public ctrlShortLocateCompanywise()
        {
            InitializeComponent();
            IntialSetUP();
        }

        private void IntialSetUP()
        {
            grdBorrowerBrokerSetup();
            BindComboEditorsGrid();
            GetPreferenceFromDB();
        }

        private void GetPreferenceFromDB()
        {
            try
            {
                SetShortLocateGrid();
                grdBorrowerCustodyAccountMapping = new Dictionary<int, int>();
                if (GetdatafromDB != null)
                {
                    this.grdBorrowerBrokerCustodyAccount.DataSource = GetdatafromDB;
                    foreach (DataRow dr in GetdatafromDB.Rows)
                    {
                        grdBorrowerCustodyAccountMapping[Convert.ToInt32(dr["BorrowerBroker"])] = Convert.ToInt32(dr["Account"]);
                    }
                }
                SetShortLocateParameters();
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

        private void SetShortLocateParameters()
        {
            Prana.DatabaseManager.QueryData queryData = new Prana.DatabaseManager.QueryData();
            queryData.StoredProcedureName = "GetShortLocateParametersDB";
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        if (Convert.ToBoolean(row[0]))
                            cmbShortLocate.Value = "MasterAccount/Clientwise";
                        else
                            cmbShortLocate.Value = "Company Level";
                        if (Convert.ToBoolean(row[1]))
                            cmbImportOverride.Value = "True";
                        else
                            cmbImportOverride.Value = "False";
                    }
                }
            }
            catch (Exception ex)
            {  // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private static void SetShortLocateGrid()
        {
            DataTable dt = GetShortLocateGridScheme();

            Prana.DatabaseManager.QueryData queryData = new Prana.DatabaseManager.QueryData();
            queryData.StoredProcedureName = "GetShortLocateGridParametersDB";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        DataRow dr = dt.NewRow();
                        dr["BorrowerBroker"] = row[1];
                        dr["Account"] = row[2];
                        dt.Rows.Add(dr);
                    }
                }
                if (dt.Rows.Count != 0)
                    GetdatafromDB = dt;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private static DataTable GetShortLocateGridScheme()
        {
            DataTable dt = new DataTable("");
            try
            {
                dt.Columns.Add(new DataColumn("BorrowerBroker", typeof(string)));
                dt.Columns.Add(new DataColumn("Account", typeof(string)));
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return null;
            }
            return dt;
        }

        private void grdBorrowerBrokerSetup()
        {
            try
            {
                DataColumn[] Keys = new DataColumn[2];
                DataTable dt = new DataTable();
                DataColumn dcBorrowerBroker = new DataColumn("BorrowerBroker", System.Type.GetType("System.String"));
                dt.Columns.Add(dcBorrowerBroker);
                DataColumn dcAccount = new DataColumn("Account", System.Type.GetType("System.String"));
                dt.Columns.Add(dcAccount);

                this.grdBorrowerBrokerCustodyAccount.DataSource = dt;
                this.grdBorrowerBrokerCustodyAccount.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.Horizontal;
                this.grdBorrowerBrokerCustodyAccount.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
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

        private void BindComboEditorsGrid()
        {
            try
            {
                Dictionary<int, string> dt1 = Prana.CommonDataCache.CachedDataManager.GetInstance.GetAccounts();
                UltraCombo uc;
                uc = new UltraCombo();
                uc.BindingContext = this.BindingContext;
                uc.DataSource = new BindingSource(dt1, null);
                uc.DisplayMember = "Value";
                uc.ValueMember = "Key";
                foreach (UltraGridColumn column in uc.DisplayLayout.Bands[0].Columns)
                {
                    if (column.Header.Caption.ToString() != "Value")
                    {
                        column.Hidden = true;
                    }
                }

                this.grdBorrowerBrokerCustodyAccount.DisplayLayout.Bands[0].Columns[1].EditorComponent = uc;


                Dictionary<int, string> dt = Prana.CommonDataCache.CachedDataManager.GetInstance.GetAllThirdPartiesWithShortName();
                UltraCombo uc1;
                uc1 = new UltraCombo();
                uc1.BindingContext = this.BindingContext;
                uc1.DataSource = new BindingSource(dt, null);
                uc1.DisplayMember = "Value";
                uc1.ValueMember = "Key";

                foreach (UltraGridColumn column in uc1.DisplayLayout.Bands[0].Columns)
                {
                    if (column.Key != "Value")
                    {
                        column.Hidden = true;
                    }
                }

                this.grdBorrowerBrokerCustodyAccount.DisplayLayout.Bands[0].Columns[0].EditorComponent = uc1;
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

        public void Save()
        {
            try
            {
                object[] parameter = new object[2];
                parameter[0] = cmbShortLocate.SelectedItem.ToString();
                parameter[1] = cmbImportOverride.SelectedItem.ToString();

                if (parameter[0] != null && parameter[1] != null)
                {
                    Prana.Admin.BLL.CompanyManager.SaveShortLocatePreferenceParameters(parameter);
                }

                DataTable dtBorrowerBrokerCustodyAccount = (DataTable)grdBorrowerBrokerCustodyAccount.DataSource;

                DataTable dt = new DataTable();
                DataColumn dcBorrowerBroker = new DataColumn("BorrowerBroker", System.Type.GetType("System.String"));
                dt.Columns.Add(dcBorrowerBroker);
                DataColumn dcAccount = new DataColumn("Account", System.Type.GetType("System.String"));
                dt.Columns.Add(dcAccount);
                foreach (DataRow row in dtBorrowerBrokerCustodyAccount.Rows)
                {
                    if (!grdBorrowerCustodyAccountMapping.ContainsKey(Convert.ToInt32(row["BorrowerBroker"])))
                    {
                        dt.Rows.Add(row.ItemArray);
                    }
                }

                if (dt.Rows.Count != 0)
                {
                    Prana.Admin.BLL.CompanyManager.SaveShortLocatePreferencesDB(dt);
                }

                MessageBox.Show(this, "Client saved", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void btnAddRow_Click(object sender, EventArgs e)
        {
            try
            {
                this.grdBorrowerBrokerCustodyAccount.DisplayLayout.Bands[0].AddNew();
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdBorrowerBrokerCustodyAccount.ActiveRow != null)
                {
                    string BorrowerBroker = grdBorrowerBrokerCustodyAccount.ActiveRow.Cells["BorrowerBroker"].Value.ToString();
                    if (BorrowerBroker != String.Empty)
                    {
                        int ID = Convert.ToInt32(BorrowerBroker);
                        Prana.Admin.BLL.CompanyManager.DeleteBorrowerBrokerAccountDB(ID);
                    }
                    grdBorrowerBrokerCustodyAccount.ActiveRow.Delete(true);
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
