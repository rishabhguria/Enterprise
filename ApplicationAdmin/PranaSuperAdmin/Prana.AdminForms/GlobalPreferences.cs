using Infragistics.Win;
using Prana.BusinessObjects.AppConstants;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Windows.Forms;

namespace Prana.AdminForms
{
    public partial class GlobalPreferences : Form
    {

        public GlobalPreferences()
        {
            InitializeComponent();
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            try
            {
                bool isPermanentDeletion = CachedDataManager.GetInstance.IsPermanentDeletionEnabled();
                bool isShowMasterFundonTT = CachedDataManager.GetInstance.IsShowMasterFundonTT();
                bool isShowmasterFundAsClient = CachedDataManager.GetInstance.IsShowmasterFundAsClient();
                bool isEquityOptionManualValidation = CachedDataManager.GetInstance.IsEquityOptionManualValidation();
                bool isCollateralMarkPriceValidation = CachedDataManager.GetInstance.IsCollateralMarkPriceValidation();
                bool isShowTillSettlementDate = CachedDataManager.GetInstance.IsShowTillSettlementDate();
                int settlementCombovalue = (int)(SettlementAutoCalculateField)Enum.Parse(typeof(SettlementAutoCalculateField), cmbAutoCalculateFields.Value.ToString());
                bool isZeroCommissionForSwaps = chkBoxSwapCommission.Checked;
                CommonDataCache.CachedDataManager.GetInstance.UpdateandSavePranaPreference(null, null, null, null, int.Parse(cmbPricingSource.Value.ToString()), CachedDataManager.GetInstance.IsAccountLockingEnabled(), isPermanentDeletion, settlementCombovalue, isZeroCommissionForSwaps, null, isShowmasterFundAsClient, isShowMasterFundonTT, isEquityOptionManualValidation, null, null);
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

        private void GlobalPreferences_Load(object sender, EventArgs e)
        {
            try
            {
                #region vlPricingSource
                ValueList vlPricingSource = new ValueList();
                vlPricingSource.ValueListItems.Add("0", "Esignal");
                vlPricingSource.ValueListItems.Add("1", "Bloomberg");
                cmbPricingSource.ValueList = vlPricingSource;
                cmbPricingSource.Value = CommonDataCache.CachedDataManager.GetInstance.GetPranaPreferenceByKey(ApplicationConstants.CONST_PRICINGSOURCE);
                #endregion

                #region Settlement Fields calculation Field
                cmbAutoCalculateFields.ValueList = Prana.Utilities.UI.MiscUtilities.EnumHelper.ConvertEnumForBindingGridColumn(typeof(SettlementAutoCalculateField));
                cmbAutoCalculateFields.Value = ((SettlementAutoCalculateField)Convert.ToInt32(CommonDataCache.CachedDataManager.GetInstance.GetPranaPreferenceByKey(ApplicationConstants.CONST_SettlementAutoCalculateField))).ToString();
                #endregion

                chkBoxSwapCommission.Checked = Convert.ToBoolean(Convert.ToInt32(CachedDataManager.GetInstance.GetPranaPreferenceByKey(ApplicationConstants.CONST_ZEROCOMMISSIONFORSWAPS)));
                CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_TRADING_TICKET);
                //cmbPricingSource.Value = Preferences.Instance.GetPranaPreferenceForKey(Preferences.PRANA_PRICING_SOURCE);
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
