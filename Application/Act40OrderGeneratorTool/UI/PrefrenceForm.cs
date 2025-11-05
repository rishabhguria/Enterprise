using Act40OrderGeneratorTool.Cache;
using Infragistics.Win;
using Infragistics.Win.UltraWinEditors;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Data;
using System.Windows.Forms;

namespace Act40OrderGeneratorTool
{
    public partial class PrefrenceForm : Form
    {
        public PrefrenceForm()
        {
            InitializeComponent();
        }

        private void PrefrenceForm_Load(object sender, EventArgs e)
        {
            CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_HEAT_MAP);
            if (CustomThemeHelper.ApplyTheme)
            {
                this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, this.Text, CustomThemeHelper.UsedFont);
            }
            SetupReplacementMatricGrid();
            ruleControlLong.SetUp(RuleCache.GetInstance().GetLongRule());
            ruleControlShort.SetUp(RuleCache.GetInstance().GetShortRule());

            ultraOptionSetModelPref.Value = Preference.GetInstance().ModelPrefrence.ToString();

            ultraOptionSetExposurePref.Value = Preference.GetInstance().CalculationPreference.ToString();

            ultraCheckEditorNakedSec.Checked = Preference.GetInstance().ExcludeNakedSecurities;

            ultraTextEditorSymbolList.Text = String.Join(",", Preference.GetInstance().ExcludedSymbols.ToArray());
        }

        private void SetupReplacementMatricGrid()
        {
            try
            {
                ultraGrid1.DataSource = ReplacementMatrix.GetInstance().Get();

                //Add replacement groups
                ValueList replacementGroups = new ValueList();
                foreach (ReplacementGroup op in Enum.GetValues(typeof(ReplacementGroup)))
                {
                    replacementGroups.ValueListItems.Add(op.ToString());
                }
                ultraGrid1.DisplayLayout.Bands[0].Columns["Group"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                ultraGrid1.DisplayLayout.Bands[0].Columns["Group"].ValueList = replacementGroups;
                ultraGrid1.DisplayLayout.Bands[0].Columns["Group"].DefaultCellValue = replacementGroups.ValueListItems[0].DisplayText;

                //Add side
                ValueList sides = new ValueList();
                foreach (Side op in Enum.GetValues(typeof(Side)))
                {
                    sides.ValueListItems.Add(op.ToString());
                }
                ultraGrid1.DisplayLayout.Bands[0].Columns["Side"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                ultraGrid1.DisplayLayout.Bands[0].Columns["Side"].ValueList = sides;
                ultraGrid1.DisplayLayout.Bands[0].Columns["Side"].DefaultCellValue = sides.ValueListItems[0].DisplayText;

                ultraGrid1.DisplayLayout.Bands[0].Columns["Price"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DoubleWithSpin;
                ultraGrid1.DisplayLayout.Bands[0].Columns["Price"].Format = "N";
            }
            catch
            {
                MessageBox.Show("Failed to load Replacement Matrix preference. The file may be corrupt. PLease contact support.");
            }
        }

        private void ultraButton4_Click(object sender, EventArgs e)
        {
            ultraGrid1.DisplayLayout.Bands[0].AddNew();
        }

        private void ultraButton3_Click(object sender, EventArgs e)
        {
            if (ultraGrid1.ActiveRow != null)
                ultraGrid1.ActiveRow.Delete(false);
        }

        private void ultraButton2_Click(object sender, EventArgs e)
        {
            ReplacementMatrix.GetInstance().Set(ultraGrid1.DataSource as DataTable);
            MessageBox.Show("Replacement Matrix : Save Complete");
        }

        private void ultraButton6_Click(object sender, EventArgs e)
        {
            try
            {
                Rule longRule = ruleControlLong.GetRule();
                Rule shortRule = ruleControlShort.GetRule();
                RuleCache.GetInstance().SaveRule(longRule, shortRule);
                MessageBox.Show("Rules : Save Complete");
            }
            catch (Exception)
            {
                MessageBox.Show("Save failed : The rules are not correctly defined.");
            }
        }

        private void ultraButton8_Click(object sender, EventArgs e)
        {
            try
            {
                ModelPrefrence pref = ultraOptionSetModelPref.Value.Equals("Account") ? ModelPrefrence.Account : ModelPrefrence.MasterFund;
                CalculationPreference calcPref = ultraOptionSetExposurePref.Value.Equals("Exposure") ? CalculationPreference.Exposure : CalculationPreference.BetaAdjExposure;
                Preference.GetInstance().Save(ultraComboDestinationAccount.Value.ToString(), ultraComboTargetAccount.Value.ToString(), ultraCheckEditorNakedSec.Checked, pref, ultraTextEditorSymbolList.Text, calcPref);
                MessageBox.Show("Preference : Save Complete");
            }
            catch (Exception)
            {
                MessageBox.Show("Save failed.");
            }
        }

        private void ultraOptionSetModelPref_ValueChanged(object sender, EventArgs e)
        {
            if ((sender as UltraOptionSet).Value.Equals("Account"))
            {
                Misc.SetUpComboBox(ultraComboDestinationAccount, Act40OrderGeneratorTool.Cache.Account.GetInstance().GetAccounts());
                Misc.SetUpComboBox(ultraComboTargetAccount, Act40OrderGeneratorTool.Cache.Account.GetInstance().GetAccounts());

                if (Act40OrderGeneratorTool.Cache.Account.GetInstance().AccountExists(Preference.GetInstance().StartupDestination))
                    ultraComboDestinationAccount.Value = Preference.GetInstance().StartupDestination;

                if (Act40OrderGeneratorTool.Cache.Account.GetInstance().AccountExists(Preference.GetInstance().StartUpModel))
                    ultraComboTargetAccount.Value = Preference.GetInstance().StartUpModel;
            }
            else if ((sender as UltraOptionSet).Value.Equals("MasterFund"))
            {
                Misc.SetUpComboBox(ultraComboDestinationAccount, Act40OrderGeneratorTool.Cache.MasterFund.GetInstance().GetMasterFunds());
                Misc.SetUpComboBox(ultraComboTargetAccount, Act40OrderGeneratorTool.Cache.MasterFund.GetInstance().GetMasterFunds());

                if (Act40OrderGeneratorTool.Cache.MasterFund.GetInstance().MasterFundExists(Preference.GetInstance().StartupDestination))
                    ultraComboDestinationAccount.Value = Preference.GetInstance().StartupDestination;

                if (Act40OrderGeneratorTool.Cache.MasterFund.GetInstance().MasterFundExists(Preference.GetInstance().StartUpModel))
                    ultraComboTargetAccount.Value = Preference.GetInstance().StartUpModel;
            }
        }
    }
}
