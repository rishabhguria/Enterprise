using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Nirvana.Admin.Controls;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Nirvana.Global;
using Nirvana.BusinessObjects;
using Nirvana.CommissionRules;
using Nirvana.BusinessObjects.AppConstants;
//using Nirvana.Admin.BLL;

namespace Nirvana.AdminForms
{
    public partial class CommissionRulesNew : Form
    {
        #region  Private Variables

        private const string FORM_NAME = "Commission Rule : ";
        private const string PARENTNODE_NAME = "CommissionRules";
        int _maxRuleSuffix = 0;
        private const string AUTO_RULENAME = "CommissionRule";
        Nirvana.BusinessObjects.CommissionRule _previousNode = null;
        // this variable checks that messagebox shown to the user or not,
        bool _blnWantToSave = false;

        #endregion  Private Variables

        public CommissionRulesNew()
        {
            InitializeComponent();
            BindCommissionRuleTree();
        }

        #region Bind CommissionRule Tree

        private void BindCommissionRuleTree()
        {
           CommissionDBManager.GetAllSavedCommissionRules();          

            RebindTreeNodes(null);           
        }

        #endregion Bind CommissionRule Tree

        #region  Private Functions

        private void CreateNewCommissionRule()
        {
            Nirvana.BusinessObjects.CommissionRule newCommissionRule = ctrlCommissionRuleobj.AddNewCommissionRuleOnUI(null);

            if (newCommissionRule != null)
            {
                // get the Auto generated Commission Rule Name
                string ruleName = GetNewUniqueNameForCommissionRule();

                newCommissionRule.RuleName = ruleName;

                CommissionRulesCacheManager.GetInstance().AddCommissionRule(newCommissionRule);
                RebindTreeNodes(newCommissionRule);

                //TreeNode newTreeNodeCommission = new TreeNode(newCommissionRule.RuleName);
                //newTreeNodeCommission.Tag = newCommissionRule;
                //trvCommissionRule.Nodes[0].Nodes.Add(newTreeNodeCommission);

                //trvCommissionRule.SelectedNode = newTreeNodeCommission;
               // trvCommissionRule.ExpandAll();
            }

           
            
        }       
        
        private string GetNewUniqueNameForCommissionRule()
        {
            #region Check for the New Commission Rule Name

            bool isAutoRuleNameExists = false;
            string ruleName = string.Empty;

            List<Nirvana.BusinessObjects.CommissionRule> AllcommissionRuleCollections = CommissionRulesCacheManager.GetInstance().GetAllCommissionRules();

            foreach (Nirvana.BusinessObjects.CommissionRule commRule in AllcommissionRuleCollections)
            {
                ruleName = commRule.RuleName;
                isAutoRuleNameExists = ruleName.Contains(AUTO_RULENAME);

                //if (isAutoRuleNameExists)
                //{
                //    string remainingChars = ruleName.Substring(ruleName.Length);
                //    int result = -1;
                //    int.TryParse(remainingChars, out result);
                //    if (result > 0)
                //    {
                //        _maxRuleSuffix = _maxRuleSuffix < result ? result : _maxRuleSuffix;
                //    }
                //}
                GetMaxRuleSuffix(isAutoRuleNameExists, ruleName);
            }

            _maxRuleSuffix = _maxRuleSuffix + 1;

            string newCommissionRuleName = AUTO_RULENAME + _maxRuleSuffix;
            return newCommissionRuleName;

            #endregion Check for the New Commission Rule Name
        }

        private void GetMaxRuleSuffix(bool isAutoNameExists, string ruleName)
        {
            if (isAutoNameExists)
            {
                string remainingChars = ruleName.Substring(AUTO_RULENAME.Length);
                int result = -1;
                int.TryParse(remainingChars, out result);
                if (result > 0)
                {
                    _maxRuleSuffix = _maxRuleSuffix < result ? result : _maxRuleSuffix;
                }
            }
        }

        private bool IsCommissionRuleNameExists(Nirvana.BusinessObjects.CommissionRule commissionRule)
        {
            List<Nirvana.BusinessObjects.CommissionRule> commissionRuleCollections = CommissionRulesCacheManager.GetInstance().GetAllCommissionRules();

            foreach (Nirvana.BusinessObjects.CommissionRule commRule in commissionRuleCollections)
            {
                if (commRule.RuleName == commissionRule.RuleName  && commRule.RuleID != commissionRule.RuleID)
                {
                    MessageBox.Show("Commission Rule with the same Name already exists.", "Nirvana Admin ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }
            return true;
        }

        private void RebindTreeNodes(Nirvana.BusinessObjects.CommissionRule addedCommissionRule)
        {
            List<Nirvana.BusinessObjects.CommissionRule> AllCommissionRules = CommissionRulesCacheManager.GetInstance().GetAllCommissionRules();

            trvCommissionRule.Nodes.Clear();          

            Font font = new Font("Vedana", 8.25F, System.Drawing.FontStyle.Bold);
            TreeNode treeNodeParent = new TreeNode(PARENTNODE_NAME);
            treeNodeParent.NodeFont = font;
            treeNodeParent.Tag = new Nirvana.BusinessObjects.CommissionRule();
            Nirvana.BusinessObjects.CommissionRule commissionRuleNew = new Nirvana.BusinessObjects.CommissionRule();

            foreach (Nirvana.BusinessObjects.CommissionRule objcommissionRule in AllCommissionRules)
            {
                TreeNode treeNodeCommissionT = new TreeNode(objcommissionRule.RuleName);
                treeNodeCommissionT.Tag = objcommissionRule;
                treeNodeParent.Nodes.Add(treeNodeCommissionT);
            }

            trvCommissionRule.Nodes.Add(treeNodeParent);

            trvCommissionRule.ExpandAll();

            bool blnMakeDefaultNodeSelected = false;
          
            if (addedCommissionRule == null)
            {
                if (treeNodeParent.Nodes.Count > 0)
                {
                    trvCommissionRule.SelectedNode = trvCommissionRule.Nodes[0].Nodes[0];
                    blnMakeDefaultNodeSelected = true;  
                }
                else
                {
                    trvCommissionRule.SelectedNode = trvCommissionRule.Nodes[0];
                    blnMakeDefaultNodeSelected = true;  
                }
            }
            else
            {
                foreach (TreeNode  treeNodeforSelect in trvCommissionRule.Nodes[0].Nodes )
                {
                    if (treeNodeforSelect.Text == addedCommissionRule.RuleName)
                    {
                        trvCommissionRule.SelectedNode = treeNodeforSelect;
                        blnMakeDefaultNodeSelected = true;  
                        break;
                    }                    
                }
            }
            if (blnMakeDefaultNodeSelected == false)
            {
                if (treeNodeParent.Nodes.Count > 0)
                {
                    trvCommissionRule.SelectedNode = trvCommissionRule.Nodes[0].Nodes[0];
                    blnMakeDefaultNodeSelected = true;
                }
                else
                {
                    trvCommissionRule.SelectedNode = trvCommissionRule.Nodes[0];
                    blnMakeDefaultNodeSelected = true;
                }
            }
        }

        private void RefreshCommissionRule()
        {
            ctrlCommissionRuleobj.RefreshControl();           
        }
        // if no Node is selected i.e. if paent node is selected then make the controls diable
        private void EnableDiableTheControls(bool blnFlag)
        {
            ctrlCommissionRuleobj.Enabled = blnFlag;
            btnDeleteCommissionRule.Enabled = blnFlag;
            btnSaveCommissionRule.Enabled = blnFlag;

        }

        private bool OnClosingSaveOrNot()
        {
            Nirvana.BusinessObjects.CommissionRule lastSelectedNode = (Nirvana.BusinessObjects.CommissionRule)trvCommissionRule.SelectedNode.Tag;
            // retain the value of current Rule Name
            string prevRuleName = lastSelectedNode.RuleName;
            // check for validation of the Selected Node
            Nirvana.BusinessObjects.CommissionRule checkedCommissionRule = ctrlCommissionRuleobj.AddNewCommissionRuleOnUI(lastSelectedNode);
            // get Modified Rules if ...
            List<Nirvana.BusinessObjects.CommissionRule> AllModifiedRules = CommissionRulesCacheManager.GetInstance().GetAllModifiedCommissionRules();
            // if Rule is incomplete, then it will return null and a validation check for Parent node, 
            //if parent node is selected , it will return null and name of the parent we set empty, so && check for it            
            if (checkedCommissionRule == null && !prevRuleName.Equals(""))
            {
                if (!_blnWantToSave)
                {
                    if (MessageBox.Show("Do you want to save the changes ?", "Nirvana Admin", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        _blnWantToSave = true;
                        return true;

                    }
                    else
                    {
                        // close without saved                      
                        return false;
                    }
                }
                else
                {
                    return true;
                }
            }
            else if (checkedCommissionRule != null && AllModifiedRules.Count > 0)
            {
                if (!_blnWantToSave)
                {
                    if (MessageBox.Show("Do you want to save the changes ?", "Nirvana Admin", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        _blnWantToSave = true;
                        bool isRuleNameExists = IsCommissionRuleNameExists(checkedCommissionRule);

                        if (isRuleNameExists == false)
                        {
                            checkedCommissionRule.RuleName = prevRuleName;
                            return true;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    bool isRuleNameExists = IsCommissionRuleNameExists(checkedCommissionRule);

                    if (isRuleNameExists == false)
                    {
                        checkedCommissionRule.RuleName = prevRuleName;
                        return true;
                    }
                }
            }


            // List<Nirvana.BusinessObjects.CommissionRule> AllModifiedRules = CommissionRulesCacheManager.GetInstance().GetAllModifiedCommissionRules();
            if (checkedCommissionRule == null && prevRuleName.Equals(""))
            {
                if (AllModifiedRules.Count > 0)
                {
                    if (MessageBox.Show("Do you want to save the changes ?", "Nirvana Admin", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        int result = 0;
                        result = CommissionDBManager.SaveAndUpdateCommissionRule(AllModifiedRules);

                        if (result > 0)
                        {
                            //MessageBox.Show("Commission Rule(s) saved.", "Nirvana Admin");
                            return false;
                        }
                    }
                }
            }
            else
            {
                int result = 0;
                result = CommissionDBManager.SaveAndUpdateCommissionRule(AllModifiedRules);

                if (result > 0)
                {
                   // MessageBox.Show("Commission Rule(s) saved.", "Nirvana Admin");
                    return false;
                }
            }
            return false;

        }

        #endregion  Private Functions

        #region  Button Click Events

        private void btnAddNewRule_Click(object sender, EventArgs e)
        {
            try
            {
                if (trvCommissionRule.SelectedNode != null)
                {
                    if (trvCommissionRule.SelectedNode == trvCommissionRule.Nodes[0])
                    {
                        CreateNewCommissionRule();
                    }
                    else
                    {
                        // get the Selected commission rule 
                        Nirvana.BusinessObjects.CommissionRule commissionRule = (Nirvana.BusinessObjects.CommissionRule)trvCommissionRule.SelectedNode.Tag;
                       // retain the value of current Rule Name
                        string currentRuleName = commissionRule.RuleName;
                        // check the validation on the Control except the Rule Name
                        Nirvana.BusinessObjects.CommissionRule addedCommissionRule = ctrlCommissionRuleobj.AddNewCommissionRuleOnUI(commissionRule);

                        if (addedCommissionRule != null)
                        {
                            // check the Rule Name exists or Not
                            bool isRuleNameExists = IsCommissionRuleNameExists(addedCommissionRule);

                            if (isRuleNameExists == false)
                            {
                                addedCommissionRule.RuleName = currentRuleName;
                                return;
                            }
                            else
                            {
                                // increment the Auto generated Commission Rule Name Suffix
                                bool isAutoRuleNameExists = false;
                                string ruleName = string.Empty;
                            
                                ruleName = addedCommissionRule.RuleName;
                                isAutoRuleNameExists = ruleName.Contains(AUTO_RULENAME);
                                GetMaxRuleSuffix(isAutoRuleNameExists, ruleName);

                                // add that rule to the collection i.e Updated the Commission Rule Collections
                                CommissionRulesCacheManager.GetInstance().AddCommissionRule(addedCommissionRule);

                                CreateNewCommissionRule();
                            }
                        }
                        else if (addedCommissionRule == null)
                        {
                            return;
                        }
                    }
                }
            }

            #region Catch
            catch (Exception ex)
            {
                string formattedInfo = ex.StackTrace.ToString();
                Logger.Write(formattedInfo, Common.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error,
                    FORM_NAME);
                AppMessageExceptionHandler appMessageExceptionHandler = new AppMessageExceptionHandler(null);
                appMessageExceptionHandler.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), System.Guid.NewGuid());
            }
            #endregion
            finally
            {
                #region LogEntry

                LogEntry logEntry = new LogEntry("btnAdd_Click",
                    Common.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                    FORM_NAME + "btnAdd_Click", null);
                Logger.Write(logEntry);

                #endregion
            }
        }

        private void btnDeleteCommissionRule_Click(object sender, EventArgs e)
        {
            int result = 0;
            if (trvCommissionRule.SelectedNode != null)
            {
                if (trvCommissionRule.SelectedNode == trvCommissionRule.Nodes[0])
                {
                    MessageBox.Show("Please select a Valid Rule to delete", "Nirvana Admin", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                else if (MessageBox.Show(this, "Do you want to delete the selected Rule ?", "Nirvana Admin", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Nirvana.BusinessObjects.CommissionRule selectedCommissionRule = (Nirvana.BusinessObjects.CommissionRule)trvCommissionRule.SelectedNode.Tag;

                     result= CommissionDBManager.DeleteCommissionRule(selectedCommissionRule.RuleID);

                     if (result == 1)
                     {
                         CommissionRulesCacheManager.GetInstance().DeleteCommissionRuleFromCollections(selectedCommissionRule);                         
                         RebindTreeNodes(_previousNode);
                         //MessageBox.Show("Selected commission rule deleted.", "Nirvana Alert");
                     }
                     else if (result == -1)
                     {
                         MessageBox.Show("Selected commission rule can not be deleted, it is referenced.", "Nirvana Admin");
                         return;
                     }

                }
            }            
           
        }

        private void btnSaveCommissionRule_Click(object sender, EventArgs e)
        {
            Nirvana.BusinessObjects.CommissionRule lastSelectedNode = (Nirvana.BusinessObjects.CommissionRule)trvCommissionRule.SelectedNode.Tag ;
            // retain the value of current Rule Name
            string currentRuleName = lastSelectedNode.RuleName;
            Nirvana.BusinessObjects.CommissionRule checkedCommissionRule = ctrlCommissionRuleobj.AddNewCommissionRuleOnUI(lastSelectedNode);
            if (checkedCommissionRule == null)
            {
                return;
            }
            else
            {
                bool isRuleNameExists = IsCommissionRuleNameExists(checkedCommissionRule);

                if (isRuleNameExists == false)
                {
                    checkedCommissionRule.RuleName = currentRuleName;
                    return;
                }                
            }
            List<Nirvana.BusinessObjects.CommissionRule> AllModifiedRules = CommissionRulesCacheManager.GetInstance().GetAllModifiedCommissionRules();
            if (AllModifiedRules.Count > 0)
            {
                int result=0;
                result = CommissionDBManager.SaveAndUpdateCommissionRule(AllModifiedRules);

                if (result > 0)
                {
                    MessageBox.Show("Commission Rule(s) saved.", "Nirvana Admin");
                    RebindTreeNodes(checkedCommissionRule);
                }
            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {  
            this.Close();
        }

        #endregion  Button Click Events

        #region Tree Events

        private void trvCommissionRule_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {
            try
            {               
                if (trvCommissionRule.SelectedNode != null)
                {                  

                    #region Get the values of Selected Node else Refresh the Control

                    if (trvCommissionRule.SelectedNode != trvCommissionRule.Nodes[0])
                    {
                        Nirvana.BusinessObjects.CommissionRule currentNodeValue= (Nirvana.BusinessObjects.CommissionRule)trvCommissionRule.SelectedNode.Tag;

                        _previousNode = currentNodeValue;

                        //ctrlCommissionRuleobj.CommissionRuleProperties = currentNodeValue;
                        ctrlCommissionRuleobj.CommissionRuleProperties(currentNodeValue);

                        // enable the controls
                        EnableDiableTheControls(true);                       
                    }
                    else
                    {
                        ctrlCommissionRuleobj.RefreshControl();
                        // disable the controls
                        EnableDiableTheControls(false);
                    }

                    #endregion Get the values of Selected Node else Refresh the Control
                }
            }
            #region Catch
            catch (Exception ex)
            {
                string formattedInfo = ex.StackTrace.ToString();
                Logger.Write(formattedInfo, Common.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error,
                    FORM_NAME);
                AppMessageExceptionHandler appMessageExceptionHandler = new AppMessageExceptionHandler(null);
                appMessageExceptionHandler.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), System.Guid.NewGuid());
            }
            #endregion

            finally
            {
                #region LogEntry

                LogEntry logEntry = new LogEntry("trvCommissionRule_AfterSelect",
                    Common.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                    FORM_NAME + "trvCommissionRule_AfterSelect", null);
                Logger.Write(logEntry);

                #endregion
            }

        }

        private void trvCommissionRule_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            #region Update Previous Selected Node

            if (trvCommissionRule.SelectedNode != trvCommissionRule.Nodes[0] && trvCommissionRule.SelectedNode != null)
            {
                try
                {
                    if (_previousNode != null)
                    {
                        // retain the previous value of Commission Rule 

                        string preRuleName = _previousNode.RuleName;

                        Nirvana.BusinessObjects.CommissionRule updatedCommissionRule = ctrlCommissionRuleobj.AddNewCommissionRuleOnUI(_previousNode);

                        if (updatedCommissionRule == null)
                        {
                            e.Cancel = true;
                            return;
                        }
                        else
                        {
                            bool isRuleNameExists = IsCommissionRuleNameExists(updatedCommissionRule);
                            if (isRuleNameExists == false)
                            {
                                updatedCommissionRule.RuleName = preRuleName;
                                e.Cancel = true;
                                return;
                            }
                            else
                            {
                            //RebindTreeNodes(updatedCommissionRule);
                              RebindTreeNodes(null);
                            }
                        }
                    }
                }

                #region Catch
                catch (Exception ex)
                {
                    string formattedInfo = ex.StackTrace.ToString();
                    Logger.Write(formattedInfo, Common.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error,
                        FORM_NAME);
                    AppMessageExceptionHandler appMessageExceptionHandler = new AppMessageExceptionHandler(null);
                    appMessageExceptionHandler.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), System.Guid.NewGuid());
                }
                #endregion

                finally
                {
                    #region LogEntry

                    LogEntry logEntry = new LogEntry("trvCommissionRule_BeforeSelect",
                        Common.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                        FORM_NAME + "trvCommissionRule_BeforeSelect", null);
                    Logger.Write(logEntry);

                    #endregion
                }

            #endregion Update Previous Selected Node
            }
        }

        #endregion Tree Events      

        #region Form Events

        private void CommissionRulesNew_FormClosing(object sender, FormClosingEventArgs e)
        {
           bool closeOrNot= OnClosingSaveOrNot();
           if (closeOrNot)
           {
               e.Cancel = true;
           }
           else
           {
               e.Cancel = false;
           }
       }

       #endregion Form Events

   }
}