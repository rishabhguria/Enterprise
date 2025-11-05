using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.ThirdParty;
using Prana.BusinessObjects.Classes.ThirdParty.Tables;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Prana.ThirdPartyUI.Forms
{
    public partial class ViewGroupedDetails : Form
    {
        #region Column Titles
        private const string COLUMN_CLORDERID = "ClOrderID";
        private const string COLUMN_ORDERID = "OrderID";
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewGroupedDetails"/> class.
        /// </summary>
        public ViewGroupedDetails(List<ThirdPartyOrderDetail> detail)
        {
            try
            {
                InitializeComponent();
                grdViewGroupedDetails.DataSource = detail;
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
        /// Set the theme and appearance of GroupedDetails form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void GroupedDetails_Load(object sender, EventArgs e)
        {
            try
            {
                CustomThemeHelper.SetThemeProperties(sender as Form, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_PRANA_SHORTCUTS);
                CustomThemeHelper.SetThemeProperties(grdViewGroupedDetails as PranaUltraGrid, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLE_NAME_THIRD_PARTY_CUSTOM);
                this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, "View Grouped Details", CustomThemeHelper.UsedFont);
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
        /// Handles InitializeLayout event of grdViewGroupedDetails
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdViewGroupedDetails_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                UltraGridBand band = grdViewGroupedDetails.DisplayLayout.Bands[0];

                band.Columns[COLUMN_CLORDERID].Header.Caption = "ClOrderID";
                band.Columns[COLUMN_CLORDERID].Header.VisiblePosition = 1;
                band.Columns[COLUMN_CLORDERID].CellActivation = Activation.NoEdit;

                band.Columns[COLUMN_ORDERID].Header.Caption = "OrderID";
                band.Columns[COLUMN_ORDERID].Header.VisiblePosition = 2;
                band.Columns[COLUMN_ORDERID].CellActivation = Activation.NoEdit;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}
