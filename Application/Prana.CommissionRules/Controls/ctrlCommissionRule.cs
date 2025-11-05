using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Nirvana.BusinessObjects;
using Nirvana.Utilities;
using Nirvana.PM.Common;
using Nirvana.BusinessObjects.AppConstants;
using Nirvana.CommissionRules;
using Infragistics.Win.UltraWinGrid;

namespace Nirvana.Admin.Controls
{
    public partial class ctrlCommissionRule : UserControl
    {
        const string C_COMBO_SELECT = "-Select-";

        #region Constructor Region

        public ctrlCommissionRule()
        {
            InitializeComponent();
        }

        #endregion Constructor Region

        private void CommissionRuleNew_Load(object sender, EventArgs e)
        {
            BindApplyRuleToCombo();
            BindAssetsList();
            BindAllBasedOnCombos();
            rdbtnParameters.Checked = true;
            BindCriteriaGrid();
        }

        #region Combo, Checked list box Binding

        private void BindCriteriaGrid()
        {
            List<CommissionRuleCriteria> commissioncriterias = new List<CommissionRuleCriteria>();
            Guid dummyRuleID = System.Guid.NewGuid();
            commissioncriterias = CommissionDBManager.GetCommissionRuleCriterias(dummyRuleID);
            grdCommissionRules.DataSource = commissioncriterias;
            AddNewTempRow();
            RefreshGrid();
        }
        private void AddNewTempRow()
        {
            List<CommissionRuleCriteria> commissionRuleCriteriacolls = new List<CommissionRuleCriteria>();

            CommissionRuleCriteria commrulecri = new CommissionRuleCriteria();
            commrulecri.CommissionCriteriaId  = int.MinValue;
            commrulecri.CommissionRate  = 0;
            commrulecri.ValueGreaterThan = 0;
            commrulecri.ValueLessThanOrEqual = 0;
            commissionRuleCriteriacolls.Add(commrulecri);
            grdCommissionRules.DataSource = commissionRuleCriteriacolls;
            //grdCommissionRules.DataBind();
        }
        private void AddNewRow()
        {            
            UltraGridCell prevActiveCell = grdCommissionRules.Rows[0].Cells[0];
            string cellText = string.Empty;
            int len = int.MinValue;
            if (grdCommissionRules.ActiveCell != null)
            {
                prevActiveCell = grdCommissionRules.ActiveCell;
                cellText = prevActiveCell.Text;
                len = cellText.Length;
            }
      
            UltraGridRow activeRow = grdCommissionRules.ActiveRow;
            double valueLessThanOrEqual = double.MinValue;
            if (activeRow != null)
            {
                valueLessThanOrEqual = Convert.ToDouble(activeRow.Cells["ValueLessThanOrEqual"].Value.ToString());
            }
            int rowsCount = grdCommissionRules.Rows.Count;
            UltraGridRow dr = grdCommissionRules.Rows[rowsCount - 1];          

            List<CommissionRuleCriteria> commrulecris = (List<CommissionRuleCriteria>)grdCommissionRules.DataSource;
            CommissionRuleCriteria commrulecri = new CommissionRuleCriteria();

            //The below varriables are taken from the last row of the grid before adding the new row.
            double  dblValueFrom = Convert.ToDouble(dr.Cells["ValueGreaterThan"].Value.ToString());
            double  dblValueTo = Convert.ToDouble(dr.Cells["ValueLessThanOrEqual"].Value.ToString());
            double CommRate = Convert.ToDouble(dr.Cells["CommissionRate"].Value.ToString());

            //Add one more condition to avoid adding 2nd new row when the below condition fullfills.
            if (dblValueTo > 0)
            {
                commrulecri.CommissionCriteriaId = int.MinValue;
                commrulecri.CommissionRate = 0;
                commrulecri.ValueGreaterThan =0;
                commrulecri.ValueLessThanOrEqual = 0;
               
                commrulecris.Add(commrulecri);
                grdCommissionRules.DataSource = commrulecris;
                grdCommissionRules.DataBind();
                grdCommissionRules.ActiveCell = prevActiveCell;
                grdCommissionRules.Focus();
                grdCommissionRules.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode);
                if (len != int.MinValue)
                {
                    //prevActiveCell.SelLength = 0;
                    //prevActiveCell.SelStart = len + 1;
                }               
            }

            grdCommissionRules.UpdateData();
            if (grdCommissionRules.ActiveRow != null)
            {
                int rowindex = grdCommissionRules.ActiveRow.Index;
                int rowsC = grdCommissionRules.Rows.Count;
                // assign value to next cloumn
                if ((rowindex + 1) != rowsC)
                {
                    UltraGridRow newDatarow = grdCommissionRules.Rows[rowindex + 1];
                    newDatarow.Cells["ValueGreaterThan"].Value = valueLessThanOrEqual;
                }
            }
        }
        private void RefreshGrid()
        {
            if (grdCommissionRules.Rows.Count > 0)
            {
                grdCommissionRules.DisplayLayout.Bands[0].Columns["ValueGreaterThan"].Header.VisiblePosition = 0;
                grdCommissionRules.DisplayLayout.Bands[0].Columns["ValueGreaterThan"].Header.Caption = "Value From(>)";
                // grdCommissionRules.DisplayLayout.Bands[0].Columns["ValueFrom"].DataType = typeof(Int64);
                //grdCommissionRules.DisplayLayout.Bands[0].Columns["ValueGreaterThan"].Format = "##,###.00";
                grdCommissionRules.DisplayLayout.Bands[0].Columns["ValueGreaterThan"].MaxLength = 14;
                grdCommissionRules.DisplayLayout.Bands[0].Columns["ValueGreaterThan"].Width = 111;
                grdCommissionRules.DisplayLayout.Bands[0].Columns["ValueGreaterThan"].SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled;
                grdCommissionRules.DisplayLayout.Bands[0].Columns["ValueGreaterThan"].InvalidValueBehavior = InvalidValueBehavior.RevertValue;
                //grdCommissionRules.DisplayLayout.Bands[0].Columns["ValueFrom"].Nullable = Infragistics.Win.UltraWinGrid.Nullable.Automatic;

                grdCommissionRules.DisplayLayout.Bands[0].Columns["ValueLessThanOrEqual"].Header.VisiblePosition = 1;
                // grdCommissionRules.DisplayLayout.Bands[0].Columns["ValueLessThanOrEqual"].DataType = typeof(Int64);
                //grdCommissionRules.DisplayLayout.Bands[0].Columns["ValueLessThanOrEqual"].Format = "##,###.00";
                grdCommissionRules.DisplayLayout.Bands[0].Columns["ValueLessThanOrEqual"].MaxLength = 14;
                grdCommissionRules.DisplayLayout.Bands[0].Columns["ValueLessThanOrEqual"].Header.Caption = "Value To(<=)";
                grdCommissionRules.DisplayLayout.Bands[0].Columns["ValueLessThanOrEqual"].Width = 111;
                grdCommissionRules.DisplayLayout.Bands[0].Columns["ValueLessThanOrEqual"].SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled;
                 grdCommissionRules.DisplayLayout.Bands[0].Columns["ValueLessThanOrEqual"].InvalidValueBehavior = InvalidValueBehavior.RevertValue;


                grdCommissionRules.DisplayLayout.Bands[0].Columns["CommissionRate"].Header.VisiblePosition = 3;
                grdCommissionRules.DisplayLayout.Bands[0].Columns["CommissionRate"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Default;
                grdCommissionRules.DisplayLayout.Bands[0].Columns["CommissionRate"].ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                grdCommissionRules.DisplayLayout.Bands[0].Columns["CommissionRate"].Format = "##,###.0000";
                grdCommissionRules.DisplayLayout.Bands[0].Columns["CommissionRate"].MaxLength = 10;
                grdCommissionRules.DisplayLayout.Bands[0].Columns["CommissionRate"].Width = 111;
                grdCommissionRules.DisplayLayout.Bands[0].Columns["CommissionRate"].Header.Caption = "Commission Rate";
                grdCommissionRules.DisplayLayout.Bands[0].Columns["CommissionRate"].SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled;
                grdCommissionRules.DisplayLayout.Bands[0].Columns["CommissionRate"].InvalidValueBehavior = InvalidValueBehavior.RevertValue;

                grdCommissionRules.DisplayLayout.Bands[0].Columns["DeleteButton"].Header.VisiblePosition = 5;
                grdCommissionRules.DisplayLayout.Bands[0].Columns["DeleteButton"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                grdCommissionRules.DisplayLayout.Bands[0].Columns["DeleteButton"].Width = 101;
                grdCommissionRules.DisplayLayout.Bands[0].Columns["DeleteButton"].SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled;

                grdCommissionRules.DisplayLayout.Bands[0].Columns["CommissionCriteriaID"].Header.VisiblePosition = 7;
                grdCommissionRules.DisplayLayout.Bands[0].Columns["CommissionCriteriaID"].Hidden = true;

            }
        }

        private void BindApplyRuleToCombo()
        {
            EnumerationValueList lstApplyRuleTo = new EnumerationValueList();
            // Bind Apply rule to Combo
            cmbApplyRuleto.DisplayMember = "DisplayText";
            cmbApplyRuleto.ValueMember = "Value";
            lstApplyRuleTo = EnumHelper.ConvertEnumForBindingWithSelect(typeof(TradeType));
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
            // create a object of type EnumerationValue and thru loop get the same object to be deleted
            EnumerationValue noneEnumerationValue = new EnumerationValue();
            foreach (EnumerationValue enumerationValueObj in lstForlistAsset)
            {
                if (enumerationValueObj.DisplayText == assetCategoryNone)
                {
                    noneEnumerationValue = enumerationValueObj;
                    break;
                }
            }
            // remove the AssetCategory.None object from the list collection
            lstForlistAsset.Remove(noneEnumerationValue);
            // now assign the List collection of Enum values to the checked list box
            checkedlstAsset.DataSource = lstForlistAsset;
            checkedlstAsset.DisplayMember = "DisplayText";
            checkedlstAsset.ValueMember = "Value";
        }

        private void BindAllBasedOnCombos()
        {
            EnumerationValueList lstBasedOn = new EnumerationValueList();

            // bind Based on Parameter combo
            
            lstBasedOn = EnumHelper.ConvertEnumForBindingWithSelect(typeof(CommissionCalculationBasis));
            cmbBasedOnParameter.DataSource = lstBasedOn;
            cmbBasedOnParameter.DisplayMember = "DisplayText";
            cmbBasedOnParameter.ValueMember = "Value";
            Utils.UltraComboFilter(cmbBasedOnParameter, "DisplayText");
            cmbBasedOnParameter.Value = int.MinValue;

            // bind Based on Criteria combo
        
            lstBasedOn = EnumHelper.ConvertEnumForBindingWithSelect(typeof(CommissionCalculationBasis));
            cmbBasedOnCriteria.DataSource = lstBasedOn;
            cmbBasedOnCriteria.DisplayMember = "DisplayText";
            cmbBasedOnCriteria.ValueMember = "Value";
            Utils.UltraComboFilter(cmbBasedOnCriteria, "DisplayText");
            cmbBasedOnCriteria.Value = int.MinValue;

            // bind Based on clearing Fee combo
          
            lstBasedOn = EnumHelper.ConvertEnumForBindingWithSelect(typeof(CommissionCalculationBasis));
            cmbBasedOnClearingFee.DataSource = lstBasedOn;
            cmbBasedOnClearingFee.DisplayMember = "DisplayText";
            cmbBasedOnClearingFee.ValueMember = "Value";
            Utils.UltraComboFilter(cmbBasedOnClearingFee, "DisplayText");
            cmbBasedOnClearingFee.Value = int.MinValue;
        }

        #endregion Combo, Checked list box Binding

        #region Private Functions

        public void RefreshControl()
        {          
            cmbApplyRuleto.Text = C_COMBO_SELECT;           
            cmbBasedOnCriteria.Text = C_COMBO_SELECT;            
            txtNameRule.Text = string.Empty;
            txtDescription.Text = string.Empty;

            // parameter box
            rdbtnParameters.Checked = true;
            grpParameters.Enabled = true;
            cmbBasedOnParameter.Text = C_COMBO_SELECT;
            nudCommissionRateParameter.Value = 0;
            nudMiniCommParameter.Value = 0;

            // Criteria box
            rdbtnCriteria.Checked = false;
            grpCriteria.Enabled = false;
            nudMiniCommCriteria.Value = 0;
            cmbBasedOnCriteria.Text = C_COMBO_SELECT;

            // clearing fee box
            chkApplyClearFee.Checked = false;
            cmbBasedOnClearingFee.Text = C_COMBO_SELECT;
            nudCommissionRateClear.Value = 0;
            nudMiniCommCleaingFee.Value = 0;
            grpClearingFee.Enabled = false;
            // uncheck all the checked items in the Asset Check list box
            for (int j = 0; j < checkedlstAsset.Items.Count; j++)
            {
                checkedlstAsset.SetItemChecked(j, false);
            }
        }

        private void SetCommissionRuleDetails(Nirvana.BusinessObjects.CommissionRule commissionRule)
        {
            txtNameRule.Text = commissionRule.RuleName;
            txtDescription.Text = commissionRule.RuleDescription;
            cmbApplyRuleto.Value = commissionRule.ApplyRuleForTrade;
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

            if (commissionRule.IsCriteriaApplied == false)
            {
                rdbtnParameters.Checked = true;
                cmbBasedOnParameter.Value = commissionRule.RuleAppliedOn;
                nudCommissionRateParameter.Value = decimal.Parse(commissionRule.CommissionRate.ToString());
                nudMiniCommParameter.Value = decimal.Parse(commissionRule.MinCommission.ToString());

                //make criteria section default
                rdbtnCriteria.Checked = false;
                cmbBasedOnCriteria.Text = C_COMBO_SELECT;
                nudMiniCommCriteria.Value = 0;
                BindCriteriaGrid();
            }
            else
            {
                rdbtnCriteria.Checked = true;
                cmbBasedOnCriteria.Value = commissionRule.RuleAppliedOn;
                nudMiniCommCriteria.Value = decimal.Parse(commissionRule.MinCommission.ToString());
                grdCommissionRules.DataSource = commissionRule.CommissionRuleCriteiaList;

                // make Parameter section default
                rdbtnParameters.Checked = false;
                cmbBasedOnParameter.Text = C_COMBO_SELECT;
                nudCommissionRateParameter.Value = 0;
                nudMiniCommParameter.Value = 0;
            }
            if (commissionRule.IsClearingFeeApplied == true)
            {
                chkApplyClearFee.Checked = true;
                cmbBasedOnClearingFee.Value = commissionRule.ClearingFeeCalculationBasedOn;
                nudCommissionRateClear.Value = decimal.Parse(commissionRule.ClearingFeeRate.ToString());
                nudMiniCommCleaingFee.Value = decimal.Parse(commissionRule.MinClearingFee.ToString());
            }
            else
            {
                chkApplyClearFee.Checked = false;
                cmbBasedOnClearingFee.Text = C_COMBO_SELECT;
                nudCommissionRateClear.Value = 0;
                nudMiniCommCleaingFee.Value = 0;
                grpClearingFee.Enabled = false;
            }
        }

        #endregion Private Functions

        #region Public Functions

        public Nirvana.BusinessObjects.CommissionRule AddNewCommissionRuleOnUI(Nirvana.BusinessObjects.CommissionRule commissionRuleParam)
        {
            if (commissionRuleParam == null)
            {
                RefreshControl();
                Nirvana.BusinessObjects.CommissionRule commissionRule = new Nirvana.BusinessObjects.CommissionRule();
               
                string newCommissionRule = string.Empty; 
                commissionRule.RuleID = Guid.NewGuid();
                commissionRule.RuleName = newCommissionRule;

                #region Set Default Values to the New Commission Rule

                commissionRule.RuleDescription = txtDescription.Text.Trim();
                commissionRule.ApplyRuleForTrade = (TradeType)cmbApplyRuleto.Value;
                commissionRule.IsCriteriaApplied = false;
                commissionRule.RuleAppliedOn = (CommissionCalculationBasis)cmbBasedOnParameter.Value;
                commissionRule.CommissionRate = float.Parse(nudCommissionRateParameter.Value.ToString());
                commissionRule.MinCommission = float.Parse(nudMiniCommParameter.Value.ToString());
                commissionRule.IsCriteriaApplied = false;
                commissionRule.IsClearingFeeApplied = false;
                //  commissionRule.ClearingFeeCalculationBasedOn = (CommissionCalculationBasis)cmbBasedOnClearingFee.Value;
                commissionRule.ClearingFeeRate = float.Parse(nudCommissionRateClear.Value.ToString());
                commissionRule.MinClearingFee = float.Parse(nudMiniCommCleaingFee.Value.ToString());

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

                errorProvider1.SetError(cmbBasedOnParameter, "");
                errorProvider1.SetError(nudCommissionRateParameter, "");

                errorProvider1.SetError(cmbBasedOnClearingFee, "");
                errorProvider1.SetError(nudCommissionRateClear, "");

                errorProvider1.SetError(cmbBasedOnCriteria, "");


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
                if (rdbtnParameters.Checked == true)
                {
                    if ((int)cmbBasedOnParameter.Value == int.MinValue)
                    {
                        errorProvider1.SetError(cmbBasedOnParameter, "Please select Commission Based on Parameter.");
                        cmbBasedOnParameter.Focus();
                        return null;
                    }
                    else if (nudCommissionRateParameter.Value <= 0)
                    {
                        errorProvider1.SetError(nudCommissionRateParameter, "Please select Commission Based on Parameter.");
                        nudCommissionRateParameter.Focus();
                        return null;
                    }
                }
                else if (rdbtnCriteria.Checked == true)
                {
                    int index = 1;

                    if ((int)cmbBasedOnCriteria.Value == int.MinValue)
                    {
                        errorProvider1.SetError(cmbBasedOnCriteria, "Please select Commission Based on Criteria.");
                        cmbBasedOnCriteria.Focus();
                        return null;
                    }
                    else if (grdCommissionRules.Rows.Count <= 1)
                    {
                        MessageBox.Show("Please enter atleast one Commission Rule Criteria.", "Nirvana Admin", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        grdCommissionRules.Focus();
                        return null;
                    }
                    else if (grdCommissionRules.Rows.Count > 1)
                    {
                        int rowcount = grdCommissionRules.Rows.Count;
                        // Validation to check whether any field in not blank              
                        foreach (UltraGridRow dr in grdCommissionRules.Rows)
                        {
                            double valuefrom = Convert.ToDouble(dr.Cells["ValueGreaterThan"].Value.ToString());
                            double valueto = Convert.ToDouble(dr.Cells["ValueLessThanOrEqual"].Value.ToString());
                            double commissionrate = double.Parse(dr.Cells["CommissionRate"].Value.ToString());

                            if (rowcount != index)
                            {
                                if (valueto > 0 || commissionrate <= 0)
                                {
                                    if (commissionrate <= 0)
                                    {
                                        MessageBox.Show("Please enter the Commission Rate in the row : " + index, "Nirvane Admin");
                                        grdCommissionRules.Focus();
                                        return null;
                                    }
                                }
                                if (valuefrom >= valueto)
                                {
                                    MessageBox.Show("' Value To(<=) ' should be greater than ' Value From(>) ' in the row : " + index, "Nirvane Admin");
                                    grdCommissionRules.Focus();
                                    return null;

                                } index = index + 1;
                            }
                            // check validation of commission rate for the last row
                            if (rowcount == index)
                            {
                                if (valuefrom > 0)
                                {
                                    if (commissionrate <= 0)
                                    {
                                        MessageBox.Show("Please enter the Commission Rate in the row : " + index, "Nirvane Admin");
                                        grdCommissionRules.Focus();
                                        return null;
                                    }
                                }
                            }
                        }
                    }
                }
                if (chkApplyClearFee.Checked == true)
                {
                    if ((int)cmbBasedOnClearingFee.Value == int.MinValue)
                    {
                        errorProvider1.SetError(cmbBasedOnClearingFee, "Please select Commission Based on Clearing Fee.");
                        chkApplyClearFee.Focus();
                        return null;
                    }
                    else if (nudCommissionRateClear.Value <= 0)
                    {
                        errorProvider1.SetError(nudCommissionRateClear, "Please select Commission Based on Clearing Fee.");
                        nudCommissionRateClear.Focus();
                        return null;
                    }
                }

                #endregion Validation Region

                #region Value Updation Reqion to the Existing Rule

               // Nirvana.BusinessObjects.CommissionRule existingCommissionRuleObj = CommissionRulesCacheManager.GetInstance().GetCommissionRuleByRuleId(commissionRuleParam.RuleID);

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

                    if (rdbtnParameters.Checked == true)
                    {
                        commissionRuleParam.IsCriteriaApplied = false;
                        commissionRuleParam.RuleAppliedOn = (CommissionCalculationBasis)cmbBasedOnParameter.Value;
                        commissionRuleParam.CommissionRate = float.Parse(nudCommissionRateParameter.Value.ToString());
                        commissionRuleParam.MinCommission = float.Parse(nudMiniCommParameter.Value.ToString());
                    }
                    if (rdbtnCriteria.Checked == true)
                    {
                        //List<CommissionRuleCriteria> commissionRuleCreteriaList = (List<CommissionRuleCriteria>)grdCommissionRules.DataSource;
                        //List<CommissionRuleCriteria> validCommissionRuleCreteriaList = new List<CommissionRuleCriteria>();
                        commissionRuleParam.RuleAppliedOn = (CommissionCalculationBasis)cmbBasedOnCriteria.Value;
                        commissionRuleParam.IsCriteriaApplied = true;
                        commissionRuleParam.MinCommission = float.Parse(nudMiniCommCriteria.Value.ToString());
                        commissionRuleParam.CommissionRuleCriteiaList = (List<CommissionRuleCriteria>)grdCommissionRules.DataSource;
                    }
                    if (chkApplyClearFee.Checked == true)
                    {
                        commissionRuleParam.IsClearingFeeApplied = true;
                        commissionRuleParam.ClearingFeeCalculationBasedOn = (CommissionCalculationBasis)cmbBasedOnClearingFee.Value;
                        commissionRuleParam.ClearingFeeRate = float.Parse(nudCommissionRateClear.Value.ToString());
                        commissionRuleParam.MinClearingFee = float.Parse(nudMiniCommCleaingFee.Value.ToString());
                    }
                    else
                    {
                        commissionRuleParam.IsClearingFeeApplied = false;
                        commissionRuleParam.ClearingFeeCalculationBasedOn = CommissionCalculationBasis.Shares;
                        commissionRuleParam.ClearingFeeRate = float.Parse(nudCommissionRateClear.Value.ToString());
                        commissionRuleParam.MinClearingFee = float.Parse(nudMiniCommCleaingFee.Value.ToString());
                    }

                    commissionRuleParam.IsModified = true;

                    return commissionRuleParam;
                }
                #endregion Value Updation Reqion to the Existing Rule
            }
            return null;
        }

        public void CommissionRuleProperties(Nirvana.BusinessObjects.CommissionRule commRule)
        {
            SetCommissionRuleDetails(commRule);
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

        //    List<Nirvana.BusinessObjects.CommissionRule> AllcommissionRuleCollections = CommissionRulesCacheManager.GetInstance().GetAllCommissionRules();

        //    foreach (Nirvana.BusinessObjects.CommissionRule commRule in AllcommissionRuleCollections)
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

        #region Combo Value Change event

        private void cmbBasedOnParameter_ValueChanged(object sender, EventArgs e)
        {
            if (cmbBasedOnParameter.Value != null)
            {
                if (cmbBasedOnParameter.Text.Trim().Equals(CommissionCalculationBasis.Shares.ToString()))
                {
                    lbldisplayParameter.Text = "Per Share";
                }
                else if (cmbBasedOnParameter.Text.Trim().Equals(CommissionCalculationBasis.Contracts.ToString()))
                {
                    lbldisplayParameter.Text = "per contract";
                }
                else if (cmbBasedOnParameter.Text.Trim().Equals(CommissionCalculationBasis.Notional.ToString()))
                {
                    lbldisplayParameter.Text = "Basis Points";
                }
                else
                {
                    lbldisplayParameter.Text = "";
                }
            }

        }

        private void cmbBasedOnClearingFee_ValueChanged(object sender, EventArgs e)
        {
            if (cmbBasedOnClearingFee.Value != null)
            {
                if (cmbBasedOnClearingFee.Text.Trim().Equals(CommissionCalculationBasis.Shares.ToString()))
                {
                    lbldisplayClearingFee.Text = "Per Share";
                }
                else if (cmbBasedOnClearingFee.Text.Trim().Equals(CommissionCalculationBasis.Contracts.ToString()))
                {
                    lbldisplayClearingFee.Text = "per contract";
                }
                else if (cmbBasedOnClearingFee.Text.Trim().Equals(CommissionCalculationBasis.Notional.ToString()))
                {
                    lbldisplayClearingFee.Text = "Basis Points";
                }
                else
                {
                    lbldisplayClearingFee.Text = "";
                }
            }

        }

        #endregion Combo Value Change event

        #region Check Change Events

        private void chkApplyClearFee_CheckedChanged(object sender, EventArgs e)
        {
            if (chkApplyClearFee.Checked == false)
            {
                cmbBasedOnClearingFee.Text = C_COMBO_SELECT;
                nudCommissionRateClear.Value = 0;
                nudMiniCommCleaingFee.Value = 0;
                grpClearingFee.Enabled = false;
            }
            else if (chkApplyClearFee.Checked == true)
            {
                cmbBasedOnClearingFee.Text = C_COMBO_SELECT;
                nudCommissionRateClear.Value = 0;
                nudMiniCommCleaingFee.Value = 0;
                grpClearingFee.Enabled = true;
            }
        }

        private void rdbtnParameters_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbtnParameters.Checked == false)
            {
                grpParameters.Enabled = false;
                cmbBasedOnParameter.Text = C_COMBO_SELECT;
                nudCommissionRateParameter.Value = 0;
                nudMiniCommParameter.Value = 0;
            }
            else if (rdbtnParameters.Checked == true)
            {
                grpParameters.Enabled = true;
                // make criteria group enable = false;
                grpCriteria.Enabled = false;
                nudMiniCommCriteria.Value = 0;
                cmbBasedOnCriteria.Text = C_COMBO_SELECT;
            }
        }

        private void rdbtnCriteria_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbtnCriteria.Checked == false)
            {
                grpCriteria.Enabled = false;
                nudMiniCommCriteria.Value = 0;               
                cmbBasedOnCriteria.Text = C_COMBO_SELECT;
                BindCriteriaGrid();
            }
            else if (rdbtnCriteria.Checked == true)
            {
                grpCriteria.Enabled = true ;
                // make Parameter group box enable=false and reset all the control of that group box
                grpParameters.Enabled = false;
                cmbBasedOnParameter.Text = C_COMBO_SELECT;
                nudCommissionRateParameter.Value = 0;
                nudMiniCommParameter.Value = 0;
            }
        }

        #endregion Check Change Events

        #region  Grid Events Region

        private void grdCommissionRules_ClickCellButton(object sender, CellEventArgs e)
        {
            grdCommissionRules.UpdateData();
            if (grdCommissionRules.Rows.Count == 1)
            {
               // MessageBox.Show("Nothing to delete.", "Nirvana Alter");
                return;
            }           
            if (e.Cell.Column.Key.Equals("DeleteButton"))
            {
                if (Convert.ToDouble(grdCommissionRules.ActiveRow.Cells["ValueLessThanOrEqual"].Value.ToString()) > 0 && Convert.ToDouble(grdCommissionRules.ActiveRow.Cells["ValueGreaterThan"].Value.ToString()) > 0)
                {
                    int activeRowIndex = grdCommissionRules.ActiveRow.Index;
                    double activeRowValueLessThanOrEqual = Convert.ToDouble(grdCommissionRules.ActiveRow.Cells["ValueLessThanOrEqual"].Value.ToString());
                    bool isDeleted = grdCommissionRules.ActiveRow.Delete();
                    if (isDeleted)
                    {
                        UltraGridRow previousDatarow = grdCommissionRules.Rows[activeRowIndex - 1];
                        previousDatarow.Cells["ValueLessThanOrEqual"].Value = activeRowValueLessThanOrEqual;
                    }


                }
            }
        }

        private void grdCommissionRules_Error(object sender, ErrorEventArgs e)
        {
            e.Cancel = true;
        }

        private void grdCommissionRules_CellChange(object sender, CellEventArgs e)
        {
            if (e.Cell.Column.Key.Equals("ValueLessThanOrEqual") || e.Cell.Column.Key.Equals("CommissionRate"))
            {
                double result;
                bool isdblParsed = double.TryParse(e.Cell.Text.ToString(), out result);
                if (isdblParsed)
                {
                    double dblValueTo = Convert.ToDouble(e.Cell.Text.ToString());
                    //grdCommissionRules.UpdateData();
                    //AddNewRow();
                    e.Cell.Refresh();
                }
            }
        }

        private void grdCommissionRules_AfterCellUpdate(object sender, CellEventArgs e)
        {
            AddNewRow();
        }
       
        #endregion  Grid Events Region

       

    }
}
