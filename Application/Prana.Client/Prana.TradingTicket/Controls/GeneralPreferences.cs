using Infragistics.Win;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.ClientCommon;
using Prana.ClientPreferences;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.MiscUtilities;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Prana.TradingTicket.Controls
{
    public partial class TTGeneralPreferencesControl : UserControl
    {
        public TTGeneralPreferencesControl()
        {
            InitializeComponent();
        }

        public void Setup(bool isDefaultClick)
        {
            try
            {
                BindSymbologyCodes();
                BindOptionTypeCombo();
                SetPreferenceToUI(isDefaultClick);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        private void SetPreferenceToUI(bool isDefaultClick)
        {
            try
            {
                if (TradingTktPrefs.TTGeneralPrefs != null && !isDefaultClick)
                {
                    cmbSymbology.Value = TradingTktPrefs.TTGeneralPrefs.DefaultSymbology;
                    cmbOptionType.Value = TradingTktPrefs.TTGeneralPrefs.DefaultOptionType;

                    chkShowOptionDetails.Checked = TradingTktPrefs.TTGeneralPrefs.IsShowOptionDetails;
                    chkKeepTTOpen.Checked = TradingTktPrefs.TTGeneralPrefs.IsSaveChecked;
                    chkPopulatelastPriceInPriceWhenAskORBidIsZero.Checked = TradingTktPrefs.TTGeneralPrefs.IsPopulatelastPriceInPriceWhenAskORBidIsZero;
                    txtDefaultICs.Text = TradingTktPrefs.TTGeneralPrefs.DefaultInternalComments;
                    txtBrokerComments.Text = TradingTktPrefs.TTGeneralPrefs.DefaultBrokerComments;
                    chkUseCustodianAsExecutingBroker.Checked = TradingTktPrefs.TTGeneralPrefs.IsUseCustodianAsExecutingBroker;
                }
                else
                {
                    TTGeneralPrefs ttGeneralPrefs = new TTGeneralPrefs();
                    cmbSymbology.Value = ttGeneralPrefs.DefaultSymbology;
                    cmbOptionType.Value = ttGeneralPrefs.DefaultOptionType;

                    chkShowOptionDetails.Checked = ttGeneralPrefs.IsShowOptionDetails;
                    chkKeepTTOpen.Checked = ttGeneralPrefs.IsSaveChecked;
                    chkPopulatelastPriceInPriceWhenAskORBidIsZero.Checked = ttGeneralPrefs.IsPopulatelastPriceInPriceWhenAskORBidIsZero;
                    txtDefaultICs.Text = ttGeneralPrefs.DefaultInternalComments;
                    txtBrokerComments.Text = ttGeneralPrefs.DefaultBrokerComments;
                    chkUseCustodianAsExecutingBroker.Checked = ttGeneralPrefs.IsUseCustodianAsExecutingBroker;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        private void BindOptionTypeCombo()
        {
            try
            {
                List<EnumerationValue> ds = EnumHelper.ConvertEnumForBindingWithAssignedValues(typeof(OptionType));
                ds.RemoveAt(ds.Count - 1); //remove the NONE
                cmbOptionType.DataSource = ds;
                cmbOptionType.DataBind();
                cmbOptionType.DisplayMember = "DisplayText";
                cmbOptionType.ValueMember = "Value";
                cmbOptionType.DropDownStyle = DropDownStyle.DropDownList;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        private void BindSymbologyCodes()
        {
            try
            {
                if (cmbSymbology.DataSource == null)
                    cmbSymbology.DataSource = SymbologyHelper.GetAvailableSymbologies();
                cmbSymbology.DisplayMember = "DisplayText";
                cmbSymbology.ValueMember = "Value";
                cmbSymbology.DataBind();
                cmbOptionType.DropDownStyle = DropDownStyle.DropDownList;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        public void Save()
        {
            try
            {
                SymbologyHelper.UpdateDefaultSymbology((ApplicationConstants.SymbologyCodes)int.Parse(cmbSymbology.Value.ToString()));
                TradingTktPrefs.TTGeneralPrefs.DefaultOptionType = int.Parse(cmbOptionType.Value.ToString());
                TradingTktPrefs.TTGeneralPrefs.IsShowOptionDetails = chkShowOptionDetails.Checked;
                TradingTktPrefs.TTGeneralPrefs.IsSaveChecked = chkKeepTTOpen.Checked;
                TradingTktPrefs.TTGeneralPrefs.IsPopulatelastPriceInPriceWhenAskORBidIsZero = chkPopulatelastPriceInPriceWhenAskORBidIsZero.Checked; 
                TradingTktPrefs.TTGeneralPrefs.DefaultInternalComments = txtDefaultICs.Text ?? string.Empty;
                TradingTktPrefs.TTGeneralPrefs.DefaultBrokerComments = txtBrokerComments.Text ?? string.Empty;
                TradingTktPrefs.TTGeneralPrefs.IsUseCustodianAsExecutingBroker = chkUseCustodianAsExecutingBroker.Checked;
                TradingTktPrefs.SaveGeneralPrefs();
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