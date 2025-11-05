using Prana.BusinessObjects;
using Prana.LogManager;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Prana.ClientCommon
{
    public partial class ctrlClosingAlgo : UserControl
    {
        public ctrlClosingAlgo()
        {
            try
            {
                InitializeComponent();
                SetLayout();

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

        private void SetLayout()
        {
            try
            {
                closingPreferencesUsrCtr.chkBoxFetchDataAutomatically.Hide();
                closingPreferencesUsrCtr.grdClosingMethod.DisplayLayout.Appearance.BackColor = Color.White;
                closingPreferencesUsrCtr.splitContainer1.BackColor = Color.White;
                // closingPreferencesUsrCtr.grdClosingMethod.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.SteelBlue;
                closingPreferencesUsrCtr.grdClosingMethod.DisplayLayout.Override.ActiveCellAppearance.BackColor = Color.White;
                closingPreferencesUsrCtr.grdClosingMethod.DisplayLayout.Override.ActiveRowAppearance.BackColor = Color.White;
                closingPreferencesUsrCtr.grdClosingMethod.DisplayLayout.Override.FixedCellAppearance.BackColor = Color.White;
                closingPreferencesUsrCtr.grdClosingMethod.DisplayLayout.Override.FixedRowAppearance.BackColor = Color.White;
                closingPreferencesUsrCtr.grdClosingMethod.DisplayLayout.Override.RowAppearance.BackColor = Color.White;
                closingPreferencesUsrCtr.grdClosingMethod.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
                Infragistics.Win.Appearance appearance = new Infragistics.Win.Appearance();
                appearance.BackColor = Color.White;
                closingPreferencesUsrCtr.grdClosingMethod.DisplayLayout.Override.GroupByRowAppearance = appearance;
                closingPreferencesUsrCtr.grdClosingMethod.DisplayLayout.Override.SelectedRowAppearance.BackColor = Color.White;
                closingPreferencesUsrCtr.grdClosingMethod.DisplayLayout.Override.SelectedRowAppearance.ForeColor = Color.Black;
                closingPreferencesUsrCtr.grdClosingMethod.DisplayLayout.Appearance = appearance;
                closingPreferencesUsrCtr.grdClosingMethod.DisplayLayout.Override.GroupByColumnAppearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;
                closingPreferencesUsrCtr.grdClosingMethod.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
                closingPreferencesUsrCtr.grdClosingMethod.DisplayLayout.GroupByBox.Appearance.BackColor = Color.Transparent;
                closingPreferencesUsrCtr.grdClosingMethod.DisplayLayout.Bands[0].Override.HeaderAppearance.BackColor = Color.White;
                if (closingPreferencesUsrCtr.grdClosingMethod.DisplayLayout.Bands.Count > 1)
                {
                    closingPreferencesUsrCtr.grdClosingMethod.DisplayLayout.Bands[1].Override.HeaderAppearance.BackColor = Color.White;
                }

                //closingPreferencesUsrCtr.grdClosingMethod.DisplayLayout.Override.GroupByColumnsHidden = Infragistics.Win.DefaultableBoolean.False;
                this.closingPreferencesUsrCtr.BackColor = Color.White;
                this.closingPreferencesUsrCtr.ForeColor = Color.Black;
                this.closingPreferencesUsrCtr.ultraGroupBox2.Appearance = appearance;
                this.closingPreferencesUsrCtr.ultraGroupBox3.Appearance = appearance;
                this.closingPreferencesUsrCtr.ultraTabControl1.Appearance = appearance;
                this.closingPreferencesUsrCtr.ultraTabControl1.ActiveTabAppearance = appearance;
                this.closingPreferencesUsrCtr.ultraTabControl1.SelectedTabAppearance = appearance;
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

        internal void LoadClosingAlgo(ClosingTemplate template)
        {
            try
            {
                closingPreferencesUsrCtr.SetClosingMethodology(template.ClosingMeth);
                closingPreferencesUsrCtr.ApplyFilters(template);
                // closingPreferencesUsrCtr.chkBoxOverrideGlobal.Checked = true;
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

        internal void UpdateClosingPreferences(ClosingTemplate closingTemplate)
        {
            try
            {
                closingTemplate.ClosingMeth = closingPreferencesUsrCtr.GetClosingMethodology();
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
