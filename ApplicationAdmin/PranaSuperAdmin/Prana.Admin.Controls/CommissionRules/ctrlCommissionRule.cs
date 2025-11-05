using Prana.BusinessObjects;
//using Prana.PM.Common;
using Prana.BusinessObjects.AppConstants;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Prana.Admin.Controls
{
    public partial class ctrlCommissionRule : UserControl
    {
        const string C_COMBO_SELECT = "-Select-";
        //Variable to maintaining the status of unsaved changes
        private bool _isUnSavedChanges = false;
        public bool IsUnSavedChanges
        {
            get { return _isUnSavedChanges; }
            set { _isUnSavedChanges = value; }
        }

        #region Constructor Region
        public ctrlCommissionRule()
        {
            InitializeComponent();
        }
        #endregion Constructor Region

        private void CommissionRuleNew_Load(object sender, EventArgs e)
        {
            bool tempValue = _isUnSavedChanges;
            InitControls();
            BindApplyRuleToCombo();
            BindAssetsList();
            BindAllBasedOnCombos();
            ctrlCommission1.CommissionRuleLoad(CommissionType.Commission);
            ctrlCommission2.CommissionRuleLoad(CommissionType.SoftCommission);

            if (!tempValue)
                _isUnSavedChanges = false;
        }

        #region Combo, Checked list box Binding

        private void BindApplyRuleToCombo()
        {
            EnumerationValueList lstApplyRuleTo = new EnumerationValueList();
            // Bind Apply rule to Combo
            cmbApplyRuleto.DisplayMember = "DisplayText";
            cmbApplyRuleto.ValueMember = "Value";
            lstApplyRuleTo = EnumHelper.ConvertEnumForBindingWithSelect(typeof(TradeType));
            //cmbApplyRuleto.DataSource = null;
            cmbApplyRuleto.DataSource = lstApplyRuleTo;
            Utils.UltraComboFilter(cmbApplyRuleto, "DisplayText");
            cmbApplyRuleto.Value = int.MinValue;
        }

        private void BindAssetsList()
        {
            EnumerationValueList lstForlistAsset = new EnumerationValueList();
            // get collection of all the Enum i.e Asset Category values
            lstForlistAsset = EnumHelper.ConvertEnumForBindingWitouthSelect(typeof(AssetCategory));
            // now remove the AssetCategory.None from the lstForlistAsset lisct collections
            string assetCategoryNone = AssetCategory.None.ToString();
            string assetCategoryOption = AssetCategory.Option.ToString();
            // create a object of type EnumerationValue and thru loop get the same object to be deleted
            EnumerationValue noneEnumerationValue = new EnumerationValue();
            EnumerationValue optionEnumerationValue = new EnumerationValue();
            foreach (EnumerationValue enumerationValueObj in lstForlistAsset)
            {
                if (enumerationValueObj.DisplayText == assetCategoryNone)
                {
                    noneEnumerationValue = enumerationValueObj;
                }
                if (enumerationValueObj.DisplayText == assetCategoryOption)
                {
                    optionEnumerationValue = enumerationValueObj;
                }
            }
            // remove the AssetCategory.None object from the list collection
            lstForlistAsset.Remove(noneEnumerationValue);
            lstForlistAsset.Remove(optionEnumerationValue);
            // now assign the List collection of Enum values to the checked list box
            checkedlstAsset.DataSource = lstForlistAsset;
            checkedlstAsset.DisplayMember = "DisplayText";
            checkedlstAsset.ValueMember = "Value";
        }

        private void BindAllBasedOnCombos()
        {
            ctrlCommission1.BindAllBasedOnCombos();
            ctrlCommission2.BindAllBasedOnCombos();
        }
        #endregion Combo, Checked list box Binding

        #region Public Functions
        public void InitControls()
        {
            cmbApplyRuleto.Text = C_COMBO_SELECT;

            ctrlCommission1.InitControls();
            ctrlCommission2.InitControls();

            txtNameRule.Text = string.Empty;
            txtDescription.Text = string.Empty;

            grpClearingFee.InitControls();

            grpClearingBrokerFee.InitControls();

            // uncheck all the checked items in the Asset Check list box
            for (int j = 0; j < checkedlstAsset.Items.Count; j++)
            {
                checkedlstAsset.SetItemChecked(j, false);
            }
        }

        public Prana.BusinessObjects.CommissionRule AddNewCommissionRuleOnUI(Prana.BusinessObjects.CommissionRule commissionRuleParam)
        {
            if (commissionRuleParam == null)
            {
                InitControls();
                Prana.BusinessObjects.CommissionRule commissionRule = new Prana.BusinessObjects.CommissionRule();

                string newCommissionRule = string.Empty;
                commissionRule.RuleID = Guid.NewGuid();
                commissionRule.RuleName = newCommissionRule;

                #region Set Default Values to the New Commission Rule

                commissionRule.RuleDescription = txtDescription.Text.Trim();
                commissionRule.ApplyRuleForTrade = (TradeType)cmbApplyRuleto.Value;

                ctrlCommission1.AddNewCommissionRuleOnUI(commissionRule.Commission);
                ctrlCommission2.AddNewCommissionRuleOnUI(commissionRule.SoftCommission);

                commissionRule.IsClearingFeeApplied = false;

                grpClearingFee.AddNewClearingFeeOnUI(commissionRule.ClearingFeeObj);

                commissionRule.IsClearingBrokerFeeApplied = false;
                grpClearingBrokerFee.AddNewClearingFeeOnUI(commissionRule.ClearingBrokerFeeObj);
                #endregion Set Default Values

                #region Commented Code
                //


                //if (rdbtnParameters.Checked == true)
                //{
                //    
                //}
                //if (rdbtnCriteria.Checked == true)
                //{
                //    commissionRule.RuleAppliedOn = (CommissionCalculationBasis)cmbBasedOnCriteria.Value;
                //    commissionRule.IsCriteriaApplied = true;
                //    commissionRule.MinCommission = float.Parse(nudMiniCommCriteria.Value.ToString());
                //}
                //if (chkApplyClearFee.Checked == true)
                //{
                //    
                //}
                //List<AssetCategory> listAssetCat = new List<AssetCategory>();
                //for (int j = 0; j < checkedlstAsset.CheckedItems.Count; j++)
                //{
                //    EnumerationValue enumValue = ((EnumerationValue)checkedlstAsset.CheckedItems[j]);
                //    listAssetCat.Add((AssetCategory)enumValue.Value);

                //}
                //if (listAssetCat.Count > 0)
                //{
                //    commissionRule.AssetIdList = listAssetCat;
                //}
                #endregion Commented Code

                return commissionRule;
            }
            else
            {
                #region Validation Check Region

                errorProvider1.SetError(txtNameRule, "");
                errorProvider1.SetError(cmbApplyRuleto, "");
                errorProvider1.SetError(checkedlstAsset, "");

                if (txtNameRule.Text.Trim().Equals(""))
                {
                    errorProvider1.SetError(txtNameRule, "Please enter the Commission Rule Name.");
                    txtNameRule.Focus();
                    return null;
                }
                if (checkedlstAsset.CheckedItems.Count == 0)
                {
                    errorProvider1.SetError(checkedlstAsset, "Please select the Asset.");
                    checkedlstAsset.Focus();
                    return null;
                }
                if ((int)cmbApplyRuleto.Value == int.MinValue)
                {
                    errorProvider1.SetError(cmbApplyRuleto, "Please select Trade Type.");
                    cmbApplyRuleto.Focus();
                    return null;
                }

                if (!ctrlCommission1.CheckValidation(errorProvider1))
                    return null;
                if (!ctrlCommission2.CheckValidation(errorProvider1))
                    return null;

                if (chkApplyClearFee.Checked == true)
                {
                    if (!grpClearingFee.CheckValidation(errorProvider1))
                        return null;
                }

                if (chkApplyClearingBrokerFee.Checked == true)
                {
                    if (!grpClearingBrokerFee.CheckValidation(errorProvider1))
                        return null;
                }

                #endregion Validation Region

                #region Value Updation Reqion to the Existing Rule

                // Prana.BusinessObjects.CommissionRule existingCommissionRuleObj = CommissionRulesCacheManager.GetInstance().GetCommissionRuleByRuleId(commissionRuleParam.RuleID);

                if (commissionRuleParam != null)
                {
                    commissionRuleParam.RuleName = txtNameRule.Text.Trim();
                    commissionRuleParam.RuleDescription = txtDescription.Text.Trim();
                    commissionRuleParam.ApplyRuleForTrade = (TradeType)cmbApplyRuleto.Value;

                    List<AssetCategory> listAssetCat = new List<AssetCategory>();
                    for (int j = 0; j < checkedlstAsset.CheckedItems.Count; j++)
                    {
                        EnumerationValue enumValue = ((EnumerationValue)checkedlstAsset.CheckedItems[j]);
                        listAssetCat.Add((AssetCategory)enumValue.Value);
                    }
                    if (listAssetCat.Count > 0)
                    {
                        commissionRuleParam.AssetIdList = listAssetCat;
                    }

                    ctrlCommission1.SetValues(commissionRuleParam.Commission);
                    ctrlCommission2.SetValues(commissionRuleParam.SoftCommission);

                    if (chkApplyClearFee.Checked == true)
                        commissionRuleParam.IsClearingFeeApplied = true;
                    else
                        commissionRuleParam.IsClearingFeeApplied = false;

                    grpClearingFee.GetDetails(commissionRuleParam.ClearingFeeObj, commissionRuleParam.IsClearingFeeApplied);

                    if (chkApplyClearingBrokerFee.Checked == true)
                        commissionRuleParam.IsClearingBrokerFeeApplied = true;
                    else
                        commissionRuleParam.IsClearingBrokerFeeApplied = false;

                    grpClearingBrokerFee.GetDetails(commissionRuleParam.ClearingBrokerFeeObj, commissionRuleParam.IsClearingBrokerFeeApplied);

                    commissionRuleParam.IsModified = true;

                    return commissionRuleParam;
                }
                #endregion Value Updation Reqion to the Existing Rule 
            }
            return null;
        }

        public void SetCommissionRuleDetails(Prana.BusinessObjects.CommissionRule commissionRule)
        {
            bool tempValue = _isUnSavedChanges;
            txtNameRule.Text = commissionRule.RuleName;
            txtDescription.Text = commissionRule.RuleDescription;
            cmbApplyRuleto.Value = commissionRule.ApplyRuleForTrade;
            errorProvider1.Clear();
            if (commissionRule.AssetIdList != null && commissionRule.AssetIdList.Count > 0)
            {
                // first of all unchecked all items from the Asset List
                for (int j = 0; j < checkedlstAsset.Items.Count; j++)
                {
                    checkedlstAsset.SetItemChecked(j, false);
                }
                // then check the items that are in the the commission object
                foreach (AssetCategory assetSaved in commissionRule.AssetIdList)
                {
                    for (int j = 0; j < checkedlstAsset.Items.Count; j++)
                    {
                        EnumerationValue enumValue = ((EnumerationValue)checkedlstAsset.Items[j]);
                        AssetCategory assetType = ((AssetCategory)enumValue.Value);
                        if (assetType == assetSaved)
                        {
                            checkedlstAsset.SetItemChecked(j, true);
                        }
                    }
                }

            }
            // if asset list of commission rule object is null then unchecked all the Items
            else
            {
                for (int j = 0; j < checkedlstAsset.Items.Count; j++)
                {
                    checkedlstAsset.SetItemChecked(j, false);
                }
            }

            ctrlCommission1.SetCommissionRuleDetails(commissionRule.Commission, CommissionType.Commission);
            ctrlCommission2.SetCommissionRuleDetails(commissionRule.SoftCommission, CommissionType.SoftCommission);

            if (commissionRule.IsClearingFeeApplied == true)
                chkApplyClearFee.Checked = true;
            else
            {
                chkApplyClearFee.Checked = false;
                grpClearingFee.EnableDisableCtrl(false);
            }

            grpClearingFee.SetDetails(commissionRule.ClearingFeeObj);

            if (commissionRule.IsClearingBrokerFeeApplied == true)
                chkApplyClearingBrokerFee.Checked = true;
            else
            {
                chkApplyClearingBrokerFee.Checked = false;
                grpClearingBrokerFee.EnableDisableCtrl(false);
            }
            grpClearingBrokerFee.SetDetails(commissionRule.ClearingBrokerFeeObj);

            if (!tempValue)
                _isUnSavedChanges = false;
        }
        #endregion Public Functions

        #region Commented Code
        //int _maxRuleSuffix = 0;
        //private const string AUTO_RULENAME = "CommissionRule";
        //private string GetNewUniqueNameForCommissionRule()
        //{
        //    #region Check for the New Commission Rule Name

        //    bool isAutoRuleNameExists = false;
        //    string ruleName = string.Empty;

        //    List<Prana.BusinessObjects.CommissionRule> AllcommissionRuleCollections = CommissionRulesCacheManager.GetInstance().GetAllCommissionRules();

        //    foreach (Prana.BusinessObjects.CommissionRule commRule in AllcommissionRuleCollections)
        //    {
        //        ruleName = commRule.RuleName;
        //        isAutoRuleNameExists = ruleName.Contains(AUTO_RULENAME);

        //        //if (isAutoRuleNameExists)
        //        //{
        //        //    string remainingChars = ruleName.Substring(ruleName.Length);
        //        //    int result = -1;
        //        //    int.TryParse(remainingChars, out result);
        //        //    if (result > 0)
        //        //    {
        //        //        _maxRuleSuffix = _maxRuleSuffix < result ? result : _maxRuleSuffix;
        //        //    }
        //        //}
        //        GetMaxRuleSuffix(isAutoRuleNameExists, ruleName);
        //    }         

        //    _maxRuleSuffix = _maxRuleSuffix + 1;

        //    string newCommissionRuleName = AUTO_RULENAME + _maxRuleSuffix; 
        //    return newCommissionRuleName;

        //    #endregion Check for the New Commission Rule Name
        //}

        //private void GetMaxRuleSuffix(bool isAutoNameExists,string ruleName)
        //{
        //    if (isAutoNameExists)
        //    {
        //        string remainingChars = ruleName.Substring(AUTO_RULENAME.Length);
        //        int result = -1;
        //        int.TryParse(remainingChars, out result);
        //        if (result > 0)
        //        {
        //            _maxRuleSuffix = _maxRuleSuffix < result ? result : _maxRuleSuffix;
        //        }
        //    }
        //}
        #endregion Commented Code



        #region Check Change Events
        private void chkApplyClearFee_CheckedChanged(object sender, EventArgs e)
        {
            _isUnSavedChanges = true;
            grpClearingFee.EnableDisableCtrl(chkApplyClearFee.Checked);
        }

        private void chkApplyClearingBrokerFee_CheckedChanged(object sender, EventArgs e)
        {
            _isUnSavedChanges = true;
            grpClearingBrokerFee.EnableDisableCtrl(chkApplyClearingBrokerFee.Checked);
        }
        #endregion Check Change Events

        private void ultraTabControl1_SelectedTabChanging(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangingEventArgs e)
        {
            if (!ctrlCommission1.CheckValidation(errorProvider1) && ultraTabControl1.SelectedTab.Text.Equals("Commission"))
                e.Cancel = true;
            if (!ctrlCommission2.CheckValidation(errorProvider1) && ultraTabControl1.SelectedTab.Text.Equals("Soft Commission"))
                e.Cancel = true;
        }
    }
}
