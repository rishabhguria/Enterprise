using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Data;
using System.Windows.Forms;

namespace Prana.CashManagement
{
    public partial class Accruals : Form, IAccruals
    {

        public Accruals()
        {
            InitializeComponent();
            this.Disposed += new EventHandler(Accruals_Disposed);
        }

        void Accruals_Disposed(object sender, EventArgs e)
        {
            if (FormClosedHandler != null)
            {
                FormClosedHandler(this, e);
            }
        }

        private void btnGet_Click(object sender, EventArgs e)
        {
            // DataSet  ds = CashAccountDataManager.GetAccrualsValueForGiveDate( dtFrom.DateTime, dtTo.DateTime);
            grdAccruals.SetDataBinding(CashAccountDataManager.GetAccrualsValueForGiveDate(dtFrom.DateTime, dtTo.DateTime), "Accruals");
            grdAccruals.DisplayLayout.Bands[0].Columns["FundID"].Header.Caption = "AccountID";
        }
        ValueList currencyValList = new ValueList();

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                DataSet dataSet = (DataSet)grdAccruals.DataSource;
                if (!dataSet.HasErrors)
                {
                    bool Changes = dataSet.HasChanges();
                    CashAccountDataManager.UpdateAccrualsValuesInDB(dataSet);
                    if (Changes)
                    {
                        MessageBox.Show("Data Saved");
                    }

                    dataSet.AcceptChanges();
                    btnGet_Click(null, null);

                }
                else
                {
                    MessageBox.Show("Data not Saved", "Error");
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

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (ExportToExcel())
            {
                MessageBox.Show("Report Succesfully saved.", "Confirmation", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private bool ExportToExcel()
        {
            bool result = false;
            try
            {
                Infragistics.Documents.Excel.Workbook workBook = new Infragistics.Documents.Excel.Workbook();
                string pathName = null;
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.InitialDirectory = Application.StartupPath;
                saveFileDialog1.Filter = "Excel WorkBook Files (*.xls)|*.xls|All Files (*.*)|*.*";
                saveFileDialog1.RestoreDirectory = true;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    pathName = saveFileDialog1.FileName;
                }
                else
                {
                    return result;
                }
                string workbookName = "Report" + DateTime.Now.Date.ToString("yyyyMMdd");
                workBook.Worksheets.Add(workbookName);

                workBook.WindowOptions.SelectedWorksheet = workBook.Worksheets[workbookName];

                workBook = this.ultraGridExcelExporter1.Export(this.grdAccruals, workBook.Worksheets[workbookName]);
                workBook.Save(pathName);
                result = true;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }

            }
            return result;
        }


        #region IAccruals Members

        public Form Reference()
        {
            return this;
        }

        public event EventHandler FormClosedHandler;

        #endregion

        private void addRowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (grdAccruals.DataSource == null)
            {
                btnGet_Click(null, null);

            }
            DataSet ds = ((DataSet)grdAccruals.DataSource);
            DataRow dr = ds.Tables[0].NewRow();
            dr[OrderFields.PROPERTY_CURRENCYID] = 1;
            ds.Tables[0].Rows.Add(dr);

        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (grdAccruals.ActiveRow != null)
            {
                DataRow row = ((System.Data.DataRowView)(grdAccruals.ActiveRow.ListObject)).Row;
                // ((DataSet)grdAccruals.DataSource).Tables[0].Rows.Remove(row);
                if (!row[0].Equals(DBNull.Value))
                {
                    row.Delete();
                }
                else
                {
                    ((DataSet)grdAccruals.DataSource).Tables[0].Rows.Remove(row);
                }
            }
        }

        private void grdAccruals_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            if (!e.ReInitialize)
            {
                DataRow row = ((System.Data.DataRowView)(e.Row.ListObject)).Row;

                if (row[OrderFields.CAPTION_LEVEL1ID].ToString().Equals(String.Empty))
                {
                    row.SetColumnError(OrderFields.CAPTION_LEVEL1ID, "Select  Account!");
                }
                else
                {
                    row.SetColumnError(OrderFields.CAPTION_LEVEL1ID, "");
                }

                if (row["CashValue"].ToString().Equals(String.Empty))
                {
                    row.SetColumnError("CashValue", "Cash Value can't be null!");
                }
                else
                {
                    row.SetColumnError("CashValue", "");
                }
                //if (row[OrderFields.PROPERTY_CurrencyID].ToString().Equals(String.Empty))
                //{
                //    row.SetColumnError(OrderFields.PROPERTY_CurrencyID, "Select  Currency !");
                //}
                //else
                //{
                //    row.SetColumnError(OrderFields.PROPERTY_CurrencyID, "");
                //}

                if (row["Date"].ToString().Equals(String.Empty))
                {
                    row.SetColumnError("Date", "Select  Date !");
                }
                else
                {
                    row.SetColumnError("Date", "");
                }
                if (row["SubAccountID"].ToString().Equals(String.Empty))
                {
                    row.SetColumnError("SubAccountID", "Select  SubAccount !");
                }
                else
                {
                    row.SetColumnError("SubAccountID", "");
                }
            }
        }

        private void Accruals_Load(object sender, EventArgs e)
        {
            try
            {
                btnGet_Click(null, null);
                if (!string.IsNullOrEmpty(CustomThemeHelper.WHITELABELTHEME) && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
                {
                    SetButtonsColor();
                }
                CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_CASH_MANAGEMENT);
                this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, this.Text, CustomThemeHelper.UsedFont);
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
            //BindGridData(grdAccruals);
        }

        /// <summary>
        /// Used for changing the color of buttons. The indices and their colors are as follows:
        /// 0 & 3: For the Green Shade
        /// 1 & 4: For the Neutral Shade
        /// 2 & 5: For the Red Shade 
        /// </summary>
        private void SetButtonsColor()
        {
            try
            {
                btnGet.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnGet.ForeColor = System.Drawing.Color.White;
                btnGet.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnGet.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnGet.UseAppStyling = false;
                btnGet.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnSave.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
                btnSave.ForeColor = System.Drawing.Color.White;
                btnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnSave.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnSave.UseAppStyling = false;
                btnSave.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnExport.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnExport.ForeColor = System.Drawing.Color.White;
                btnExport.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnExport.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnExport.UseAppStyling = false;
                btnExport.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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


        private void SetError(string caption, DataRow row, CellEventArgs e)
        {
            if (row[caption].ToString().Equals(String.Empty))
            {
                if (e.Cell.Text == string.Empty)
                {
                    row.SetColumnError(caption, ApplicationConstants.C_COMBO_SELECT + caption + "!");
                }
                else
                {
                    row.SetColumnError(caption, "");
                }
            }

            else
            {
                row.SetColumnError(caption, "");
            }

        }

        private void grdAccruals_CellChange(object sender, CellEventArgs e)
        {
            DataRow row = ((System.Data.DataRowView)(e.Cell.Row.ListObject)).Row;
            string column = e.Cell.Column.Key;

            switch (column)
            {

                case OrderFields.CAPTION_LEVEL1ID:
                    SetError(column, row, e);
                    break;
                case "SubAccountID":
                    SetError(column, row, e);
                    break;
                case "CashValue":
                    SetError(column, row, e);
                    break;
                case OrderFields.PROPERTY_CURRENCYID:
                    SetError(column, row, e);
                    break;
                case "Date":
                    SetError(column, row, e);
                    break;
                default:
                    break;
            }
        }

    }
}