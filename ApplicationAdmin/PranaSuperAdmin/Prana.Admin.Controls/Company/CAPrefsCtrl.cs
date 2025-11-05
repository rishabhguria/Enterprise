using Prana.BusinessObjects;
using Prana.LogManager;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Prana.Admin.Controls
{
    public partial class CAPrefsCtrl : UserControl
    {
        public CAPrefsCtrl()
        {
            InitializeComponent();
        }

        private void CAPrefsCtrl_Load(object sender, EventArgs e)
        {
            BindClosingAlgoCombo();
            BindSecondarySortCombo();
        }

        private void BindClosingAlgoCombo()
        {
            try
            {
                List<EnumerationValue> ClosingAlgos = EnumHelper.ConvertEnumForBindingWithAssignedValues(typeof(PostTradeEnums.CloseTradeAlogrithm));
                List<EnumerationValue> ClosingAlgosWithoutPreset = new List<EnumerationValue>();
                foreach (EnumerationValue value in ClosingAlgos)
                {
                    if (!value.DisplayText.Equals(PostTradeEnums.CloseTradeAlogrithm.PRESET.ToString()) && (!value.DisplayText.Equals(PostTradeEnums.CloseTradeAlogrithm.ACA.ToString())) && (!value.DisplayText.Equals(PostTradeEnums.CloseTradeAlogrithm.NONE.ToString())))
                    {
                        ClosingAlgosWithoutPreset.Add(value);
                    }
                }
                cmbClosingAlgo.DataSource = null;
                cmbClosingAlgo.DataSource = ClosingAlgosWithoutPreset;
                cmbClosingAlgo.ValueMember = "Value";
                cmbClosingAlgo.DisplayMember = "DisplayText";
                cmbClosingAlgo.Value = 3;
                cmbClosingAlgo.Text = PostTradeEnums.CloseTradeAlogrithm.FIFO.ToString();
                Utils.UltraComboFilter(cmbClosingAlgo, "DisplayText");
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }

            }
        }

        private void BindSecondarySortCombo()
        {
            try
            {
                List<EnumerationValue> SecondarySortCriteria = EnumHelper.ConvertEnumForBindingWithAssignedValues(typeof(PostTradeEnums.SecondarySortCriteria));
                // List<EnumerationValue> ClosingAlgosWithoutPreset = new List<EnumerationValue>();
                //foreach (EnumerationValue value in SecondarySortCriteria)
                //{

                //    SecondarySortCriteria.Add(value);

                //}
                cmbSecondarySort.DataSource = null;
                cmbSecondarySort.DataSource = SecondarySortCriteria;
                cmbSecondarySort.ValueMember = "Value";
                cmbSecondarySort.DisplayMember = "DisplayText";
                //cmbSecondarySort.DisplayLayout.Bands[0].Columns["Value"].Hidden = true;
                cmbSecondarySort.Value = 0;
                //  cmbSecondarySort.Enabled = false;
                Utils.UltraComboFilter(cmbSecondarySort, "DisplayText");
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void chkAdjustShares_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAdjustShares.Checked)
                this.grpBoxClosing.Enabled = true;
            else
                this.grpBoxClosing.Enabled = false;
        }

        public CAPreferences GetCAPrefsFromUI()
        {
            CAPreferences caPrefs = new CAPreferences();
            try
            {

                caPrefs.UseNetNotional = chkUseNetNotional.Checked;
                caPrefs.AdjustFractionalSharesAtAccountPositionLevel = chkAdjustShares.Checked;
                if (caPrefs.AdjustFractionalSharesAtAccountPositionLevel)
                {
                    if (cmbClosingAlgo.Value == null)
                    {
                        MessageBox.Show("Please set the Primary Closing Algo", "Corporate Action!");
                        return null;
                    }
                    caPrefs.ClosingAlgo = (Prana.BusinessObjects.PostTradeEnums.CloseTradeAlogrithm)cmbClosingAlgo.Value;
                    caPrefs.SecondarySort = (Prana.BusinessObjects.PostTradeEnums.SecondarySortCriteria)cmbSecondarySort.Value;
                }
                else
                {
                    caPrefs.ClosingAlgo = Prana.BusinessObjects.PostTradeEnums.CloseTradeAlogrithm.NONE;
                    caPrefs.SecondarySort = Prana.BusinessObjects.PostTradeEnums.SecondarySortCriteria.None;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return caPrefs;
        }

        /// <summary>
        /// Saving Primary closing algo for CH Release
        /// </summary>
        /// <returns></returns>
        public CAPreferences GetCAPrefsFromUIForCH()
        {
            CAPreferences caPrefs = new CAPreferences();
            try
            {
                caPrefs.UseNetNotional = chkUseNetNotional.Checked;
                caPrefs.AdjustFractionalSharesAtAccountPositionLevel = chkAdjustShares.Checked;
                caPrefs.ClosingAlgo = Prana.BusinessObjects.PostTradeEnums.CloseTradeAlogrithm.NONE;
                caPrefs.SecondarySort = Prana.BusinessObjects.PostTradeEnums.SecondarySortCriteria.None;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return caPrefs;
        }

        public void SetCAUIFromPrefs(CAPreferences caPreferences)
        {
            try
            {
                this.chkUseNetNotional.Checked = caPreferences.UseNetNotional;
                this.chkAdjustShares.Checked = caPreferences.AdjustFractionalSharesAtAccountPositionLevel;
                if (this.chkAdjustShares.Checked)
                {
                    this.grpBoxClosing.Enabled = true;
                    this.cmbClosingAlgo.Value = caPreferences.ClosingAlgo;
                    this.cmbSecondarySort.Value = caPreferences.SecondarySort;
                }
                else
                {
                    this.grpBoxClosing.Enabled = false;
                    this.cmbClosingAlgo.Value = Prana.BusinessObjects.PostTradeEnums.CloseTradeAlogrithm.NONE;
                    this.cmbSecondarySort.Value = Prana.BusinessObjects.PostTradeEnums.SecondarySortCriteria.None;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}
