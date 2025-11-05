using Infragistics.Win.UltraWinGrid;
using Prana.CommonDatabaseAccess;
using Prana.CommonDataCache;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Prana.Analytics
{
    public partial class SymbolMappingUI : Form
    {
        public SymbolMappingUI()
        {
            try
            {
                InitializeComponent();

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

        private void SymbolMappingUI_Load(object sender, EventArgs e)
        {
            try
            {
                SetupSymbolMapping();
                CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_RISK_MANAGEMENT);
                if (CustomThemeHelper.ApplyTheme)
                {
                    this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                    this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, this.Text, CustomThemeHelper.UsedFont);
                }
                if (!string.IsNullOrEmpty(CustomThemeHelper.WHITELABELTHEME) && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
                {
                    SetButtonsColor();
                }
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

        private void SetButtonsColor()
        {
            try
            {
                btnSave.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
                btnSave.ForeColor = System.Drawing.Color.White;
                btnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnSave.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnSave.UseAppStyling = false;
                btnSave.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

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

        private bool isSymbolDeleted = false;
        private void SetupSymbolMapping()
        {
            try
            {
                DataSet dtSymbolMapping = RiskPreferenceManager.RiskPrefernece.SymbolMappingTable;
                grdSymbolMapping.DataSource = null;
                grdSymbolMapping.DataSource = dtSymbolMapping;
                grdSymbolMapping.Refresh();

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

        private ProxyBase<IPranaPositionServices> CreatePositionManagementProxy()
        {
            return new ProxyBase<IPranaPositionServices>("TradePositionServiceEndpointAddress");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                bool isDatachanged = false;
                statusLabel.Text = string.Empty;
                ProxyBase<IPranaPositionServices> positionManagementServices = CreatePositionManagementProxy();
                DataSet dsSymbolMapping = (DataSet)grdSymbolMapping.DataSource;

                if (dsSymbolMapping.Tables.Count > 0)
                {
                    DataTable dtSymbolMapping = dsSymbolMapping.Tables[0];
                    List<DataRow> emptyRows = new List<DataRow>();
                    foreach (DataRow dr in dtSymbolMapping.Rows)
                    {
                        if (!dr.RowState.Equals(DataRowState.Deleted))
                        {
                            if (dr["Symbol"].Equals(System.DBNull.Value))
                            {
                                emptyRows.Add(dr);
                            }
                        }
                    }
                    foreach (DataRow dr in emptyRows)
                    {
                        dtSymbolMapping.Rows.Remove(dr);
                    }

                    WindsorContainerManager.SavePSSymbolMappingToDB(dtSymbolMapping);
                    //updating the cache on trade server
                    isDatachanged = dsSymbolMapping.HasChanges();
                    if (isDatachanged == true)
                    {
                        positionManagementServices.InnerChannel.RefershPSSymbolMappingCache(dsSymbolMapping.GetChanges());
                        statusLabel.ForeColor = System.Drawing.Color.Green;
                        statusLabel.Text = "Data Saved";
                    }
                    else
                    {
                        statusLabel.ForeColor = System.Drawing.Color.Red;
                        statusLabel.Text = "Nothing to Save";
                    }
                    dsSymbolMapping.AcceptChanges();
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

        private void grdSymbolMapping_AfterCellUpdate(object sender, CellEventArgs e)
        {
            try
            {
                if (e.Cell.Column.Key.Equals("Symbol"))
                {
                    UltraGridRow Activerow = e.Cell.Row;
                    foreach (UltraGridRow row in grdSymbolMapping.Rows)
                    {
                        if (!row.Equals(Activerow))
                        {
                            if (row.Cells["Symbol"].Value.ToString().Equals(Activerow.Cells["Symbol"].Value.ToString()))
                            {
                                MessageBox.Show("The Symbol you have entered already exists,please enter a different Symbol", "Symbol Mapping", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                e.Cell.Value = e.Cell.OriginalValue;
                                return;
                            }
                        }
                    }
                }
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

        private void btnAddRow_Click(object sender, EventArgs e)
        {
            try
            {
                grdSymbolMapping.DisplayLayout.Bands[0].AddNew();
                grdSymbolMapping.Rows.AddRowModifiedByUser = true;
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdSymbolMapping.ActiveRow != null)
                {
                    grdSymbolMapping.ActiveRow.Delete(true);

                    if (grdSymbolMapping.ActiveRow == null)
                    {
                        isSymbolDeleted = true;
                    }
                    statusLabel.Text = string.Empty;
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

        private void grdInterestRate_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                UltraGridBand band = grdSymbolMapping.DisplayLayout.Bands[0];
                band.Columns["PSSymbol"].Header.Caption = "Risk Symbol";
                band.Columns["Symbol"].CharacterCasing = CharacterCasing.Upper;
                band.Columns["PSSymbol"].CharacterCasing = CharacterCasing.Upper;
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

        private void SymbolMappingUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (isSymbolDeleted.Equals(true))
                {
                    btnSave_Click(this.btnSave, e);
                }
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
    }
}