using Infragistics.Win.UltraWinGrid;
using Prana.Admin.BLL;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Prana.Admin.Controls.DefaultAUECMapping
{
    public partial class ctrlDefaultAUECMapping : UserControl
    {
        public ctrlDefaultAUECMapping()
        {
            InitializeComponent();
            grdDefaultAUECMappingSetup();
            BindComboEditorsGrid();
            GetDefaultAUECGrid();
        }

        /// <summary>
        /// Add the column structure to the Datatable
        /// </summary>
        /// <returns></returns>
        private DataTable GetDefaultAUECScheme()
        {
            DataTable dt = new DataTable();
            DataColumn dcCountries = new DataColumn("Country", System.Type.GetType("System.String"));
            dt.Columns.Add(dcCountries);
            DataColumn dcCurrency = new DataColumn("Currency", System.Type.GetType("System.String"));
            dt.Columns.Add(dcCurrency);
            DataColumn dcAUEC = new DataColumn("AUEC", System.Type.GetType("System.String"));
            dt.Columns.Add(dcAUEC);

            return dt;
        }

        /// <summary>
        /// Setting up the grid
        /// </summary>
        private void grdDefaultAUECMappingSetup()
        {
            try
            {
                DataTable dt = GetDefaultAUECScheme();
                this.grdDefaultAUECMapping.DataSource = dt;

                UltraGridColumn colCountry = grdDefaultAUECMapping.DisplayLayout.Bands[0].Columns["Country"];
                colCountry.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                colCountry.ButtonDisplayStyle = ButtonDisplayStyle.Always;
                colCountry.Header.VisiblePosition = 2;

                UltraGridColumn colCurrency = grdDefaultAUECMapping.DisplayLayout.Bands[0].Columns["Currency"];
                colCurrency.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                colCurrency.ButtonDisplayStyle = ButtonDisplayStyle.Always;
                colCurrency.Header.VisiblePosition = 3;

                UltraGridColumn colAUEC = grdDefaultAUECMapping.DisplayLayout.Bands[0].Columns["AUEC"];
                colAUEC.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                colAUEC.ButtonDisplayStyle = ButtonDisplayStyle.Always;
                colAUEC.Header.VisiblePosition = 4;


                this.grdDefaultAUECMapping.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.Horizontal;
                this.grdDefaultAUECMapping.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
                this.grdDefaultAUECMapping.DisplayLayout.Override.SupportDataErrorInfo = Infragistics.Win.UltraWinGrid.SupportDataErrorInfo.RowsAndCells;
                this.grdDefaultAUECMapping.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
                this.grdDefaultAUECMapping.DisplayLayout.Override.HeaderAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
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
        /// Bind the data in dropdown for countries, Currencies and AUECs columns 
        /// </summary>
        private void BindComboEditorsGrid()
        {
            try
            {
                Countries countries = GeneralManager.GetCountries();
                //Inserting the - Select - option in the Combo Box at the top.
                UltraCombo uc;
                uc = new UltraCombo();
                uc.DisplayMember = "Name";
                uc.ValueMember = "CountryID";
                uc.DataSource = null;
                uc.DataSource = countries;
                uc.Value = int.MinValue;
                foreach (Infragistics.Win.UltraWinGrid.UltraGridColumn column in uc.DisplayLayout.Bands[0].Columns)
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

                uc.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
                uc.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
                uc.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
                uc.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
                this.grdDefaultAUECMapping.DisplayLayout.Bands[0].Columns[0].EditorComponent = uc;

                Dictionary<int, string> dt1 = Prana.CommonDataCache.CachedDataManager.GetInstance.GetAllCurrencies();
                UltraCombo uc1;
                uc1 = new UltraCombo();
                uc1.BindingContext = this.BindingContext;
                uc1.DataSource = new BindingSource(dt1, null);
                uc1.DisplayMember = "Value";
                uc1.ValueMember = "Key";

                foreach (UltraGridColumn column in uc1.DisplayLayout.Bands[0].Columns)
                {
                    if (column.Key != "Value")
                    {
                        column.Hidden = true;
                    }
                }

                uc1.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
                uc1.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
                uc1.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
                uc1.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
                this.grdDefaultAUECMapping.DisplayLayout.Bands[0].Columns[1].EditorComponent = uc1;

                int companyID = int.Parse(Prana.CommonDataCache.CachedDataManager.GetInstance.GetCompany().Rows[0]["CompanyID"].ToString());
                AUECs dt2 = new AUECs();
                dt2 = AUECManager.GetCompanyAUEC(companyID);

                foreach (AUEC entry in dt2)
                {
                    AUEC newAUEC = new AUEC();
                    newAUEC = AUECManager.GetAUECDetails(entry.AUECID);
                    entry.ExchangeIdentifier = newAUEC.ExchangeIdentifier;
                }
                UltraCombo uc2;
                uc2 = new UltraCombo();
                uc2.BindingContext = this.BindingContext;
                uc2.DataSource = new BindingSource(dt2, null);
                uc2.DisplayMember = "ExchangeIdentifier";
                uc2.ValueMember = "AUECID";

                foreach (Infragistics.Win.UltraWinGrid.UltraGridColumn column in uc2.DisplayLayout.Bands[0].Columns)
                {
                    if (column.Key.Equals("ExchangeIdentifier"))
                    {
                        column.Hidden = false;
                    }
                    else
                    {
                        column.Hidden = true;
                    }
                }

                uc2.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
                uc2.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
                uc2.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
                uc2.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
                this.grdDefaultAUECMapping.DisplayLayout.Bands[0].Columns[2].EditorComponent = uc2;
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
        /// Gets the data to the grid
        /// </summary>
        private void GetDefaultAUECGrid()
        {
            DataTable dt = GetDefaultAUECScheme();

            object[] parameter = new object[1];
            parameter[0] = int.Parse(Prana.CommonDataCache.CachedDataManager.GetInstance.GetCompany().Rows[0]["CompanyID"].ToString());

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetDefaultAUECData", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        DataRow dr = dt.NewRow();
                        dr["Country"] = row[0];
                        dr["Currency"] = row[1];
                        dr["AUEC"] = row[2];
                        dt.Rows.Add(dr);
                    }
                }
                grdDefaultAUECMapping.DataSource = dt;
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

        /// <summary>
        /// Adding new rows on the grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addRowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.grdDefaultAUECMapping.DisplayLayout.Bands[0].AddNew();
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
        /// Save the data of the grid
        /// </summary>
        public void Save()
        {
            try
            {
                DataTable NewDefaultAUEC = (DataTable)grdDefaultAUECMapping.DataSource;
                DataSet ds = new DataSet();
                ds.Tables.Add(ValidationForRows(NewDefaultAUEC.Copy()));
                this.grdDefaultAUECMapping.DataSource = ds.Tables[0];

                if (ds.Tables[0].HasErrors)
                {
                    MessageBox.Show(this, "Default AUEC Mapping not saved", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    Prana.Admin.BLL.CompanyManager.SaveDefaultAUECData(ds.Tables[0]);
                    MessageBox.Show(this, "Default AUEC Mapping saved", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        /// Providing validation messages for rows
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private DataTable ValidationForRows(DataTable dt)
        {
            try
            {
                foreach (DataRow row in dt.Rows)
                {
                    bool isbreakactivated = false;
                    if (!(row["Country"].Equals(DBNull.Value) || row["Currency"].Equals(DBNull.Value) || row["AUEC"].Equals(DBNull.Value)))
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            if (!(dr["Country"].Equals(DBNull.Value) || dr["Currency"].Equals(DBNull.Value) || dr["AUEC"].Equals(DBNull.Value)))
                            {
                                if (Convert.ToInt32(row["Country"]) == Convert.ToInt32(dr["Country"]) && Convert.ToInt32(row["Currency"]) == Convert.ToInt32(dr["Currency"]))
                                {
                                    if (dt.Rows.IndexOf(row) != dt.Rows.IndexOf(dr))
                                    {
                                        if (Convert.ToInt32(row["AUEC"]) == Convert.ToInt32(dr["AUEC"]))
                                            row.RowError = "The mapping is already defined, please select another mapping";
                                        else
                                            row.RowError = "The default AUEC for country and currency pair, already exists, please choose another country or currency";
                                        isbreakactivated = true;
                                        break;
                                    }
                                }
                                else if (Convert.ToInt32(row["AUEC"]) == Convert.ToInt32(dr["AUEC"]))
                                {
                                    if (dt.Rows.IndexOf(row) != dt.Rows.IndexOf(dr))
                                    {
                                        row.RowError = "The AUEC is already mapped to a country and currency pair, please select another AUEC";
                                        isbreakactivated = true;
                                        break;
                                    }
                                }
                            }

                        }
                        if (row.HasErrors && !isbreakactivated)
                            row.ClearErrors();
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
            return dt;
        }

        /// <summary>
        /// Providing validation messages for columns
        /// </summary>
        /// <param name="row"></param>
        private void ValidationForColumns(DataRow row)
        {
            try
            {
                foreach (DataColumn dc in row.Table.Columns)
                {
                    if (row[dc].Equals(DBNull.Value))
                    {
                        row.SetColumnError(dc.Caption, "Select " + dc.Caption + "!");
                    }
                    else
                    {
                        if (row.HasErrors)
                            row.ClearErrors();
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
        /// Delete the seleced row from the grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteRowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdDefaultAUECMapping.ActiveRow != null)
                {
                    grdDefaultAUECMapping.ActiveRow.Delete(true);
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
        /// Intialize the row when added to the grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdDefaultAUECMapping_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                if (grdDefaultAUECMapping.ActiveRow != null)
                {
                    DataRow row = ((System.Data.DataRowView)(e.Row.ListObject)).Row;
                    ValidationForColumns(row);
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
