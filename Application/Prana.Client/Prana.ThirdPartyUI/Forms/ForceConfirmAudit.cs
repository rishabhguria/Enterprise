using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.ThirdParty.Tables;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Prana.ThirdPartyUI.Forms
{
    public partial class ForceConfirmAudit : Form
    {
        #region Column Titles
        private const string HEADCOL_SYMBOL = "SYMBOL";
        private const string HEADCOL_SIDE = "SIDE";
        private const string HEADCOL_QUANTITY = "QUANTITY";
        private const string HEADCOL_BROKER = "BROKER";
        private const string HEADCOL_USERNAME = "USERNAME";
        private const string HEADCOL_CONFIRMATIONDATETIME = "CONFIRMATIONDATETIME";
        private const string HEADCOL_ALLOCATIONID = "ALLOCATIONID";
        private const string HEADCOL_COMMENT = "COMMENT";
        #endregion

        public ForceConfirmAudit()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Sets data for GrdForceConfirmAudit
        /// </summary>
        /// <param name="data"></param>
        public void InitializeDataSource(List<ThirdPartyForceConfirm> data)
        {
            try
            {
                GrdForceConfirmAudit.DataSource = null;
                GrdForceConfirmAudit.DataSource = data;
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
        /// Handles Load event of ForceConfirmAudit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ForceConfirmAudit_Load(object sender, EventArgs e)
        {
            try
            {
                CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_PRANA_TRADING_TICKET);
                this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, "Force Match Audit", CustomThemeHelper.UsedFont);
                CustomThemeHelper.SetThemeProperties(GrdForceConfirmAudit as UltraGrid, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLE_NAME_THIRD_PARTY_CUSTOM);
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
        /// Handles InitializeLayout event of GrdForceConfirmAudit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GrdForceConfirmAudit_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                UltraGridBand band = GrdForceConfirmAudit.DisplayLayout.Bands[0];

                band.Override.AllowColMoving = AllowColMoving.Default;
                GrdForceConfirmAudit.DisplayLayout.Override.AllowColMoving = AllowColMoving.NotAllowed;
                GrdForceConfirmAudit.DisplayLayout.GroupByBox.Hidden = true;
                GrdForceConfirmAudit.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
                GrdForceConfirmAudit.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;

                band.Columns[HEADCOL_USERNAME].Header.Caption = "User Name";
                band.Columns[HEADCOL_USERNAME].Header.VisiblePosition = 0;
                band.Columns[HEADCOL_USERNAME].CellActivation = Activation.NoEdit;

                band.Columns[HEADCOL_CONFIRMATIONDATETIME].Header.Caption = "Confirmation Date/Time";
                band.Columns[HEADCOL_CONFIRMATIONDATETIME].Header.VisiblePosition = 1;
                band.Columns[HEADCOL_CONFIRMATIONDATETIME].CellActivation = Activation.NoEdit;
                band.Columns[HEADCOL_CONFIRMATIONDATETIME].Format = DateTimeConstants.NirvanaDateTimeFormat;

                band.Columns[HEADCOL_BROKER].Header.Caption = "Broker";
                band.Columns[HEADCOL_BROKER].Header.VisiblePosition = 2;
                band.Columns[HEADCOL_BROKER].CellActivation = Activation.NoEdit;

                band.Columns[HEADCOL_SYMBOL].Header.Caption = "Symbol";
                band.Columns[HEADCOL_SYMBOL].Header.VisiblePosition = 3;
                band.Columns[HEADCOL_SYMBOL].CellActivation = Activation.NoEdit;

                band.Columns[HEADCOL_SIDE].Header.Caption = "Side";
                band.Columns[HEADCOL_SIDE].Header.VisiblePosition = 4;
                band.Columns[HEADCOL_SIDE].CellActivation = Activation.NoEdit;

                band.Columns[HEADCOL_QUANTITY].Header.Caption = "Quantity";
                band.Columns[HEADCOL_QUANTITY].Header.VisiblePosition = 5;
                band.Columns[HEADCOL_QUANTITY].CellActivation = Activation.NoEdit;

                band.Columns[HEADCOL_ALLOCATIONID].Header.Caption = "Allocation ID";
                band.Columns[HEADCOL_ALLOCATIONID].Header.VisiblePosition = 6;
                band.Columns[HEADCOL_ALLOCATIONID].CellActivation = Activation.NoEdit;

                band.Columns[HEADCOL_COMMENT].Header.Caption = "Comment";
                band.Columns[HEADCOL_COMMENT].Header.VisiblePosition = 7;
                band.Columns[HEADCOL_COMMENT].CellActivation = Activation.NoEdit;

                band.Columns["USERID"].Hidden = true;
                band.Columns["AveragePX"].Hidden = true;
                band.Columns["TradeDate"].Hidden = true;
                band.Columns["MatchStatus"].Hidden = true;
                band.Columns["ThirdPartyBatchID"].Hidden = true;
                band.Columns["Account"].Hidden = true;
                band.Columns["Commission"].Hidden = true;
                band.Columns["MiscFees"].Hidden = true;
                band.Columns["NetMoney"].Hidden = true;

                UltraGridLayout layout = e.Layout;
                layout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
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
