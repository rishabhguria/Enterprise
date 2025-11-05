using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;
using Infragistics.Win;

namespace Prana.CashManagement
{
    public partial class ctrlCashAccounts : UserControl
    {
        private const string ROOT_NODE = "Accounts";
        private Dictionary<string, CashAccount> _cashAccountsDict = new Dictionary<string, CashAccount>();
        Infragistics.Win.UltraWinTree.UltraTreeNode _rootNode;
        Infragistics.Win.UltraWinTree.UltraTreeNode _newNode;
        Infragistics.Win.UltraWinTree.UltraTreeNode _currentNode;
        
        public ctrlCashAccounts()
        {
            InitializeComponent();
        }


        public void SetUp()
        {
            BindAccountTypeCombo();
            CreateRootNode();
            _cashAccountsDict = CashAccountDataManager.GetCashAccountsFromDB();
            LoadTreeFromAccountsDict();
            _rootNode.Selected = true;
        }
        private void CreateRootNode()
        {
            _rootNode = treeAccounts.Nodes.Add(ROOT_NODE);
        }
        private void LoadTreeFromAccountsDict()
        {
            foreach (KeyValuePair<string, CashAccount> item in _cashAccountsDict)
            {
                CashAccount cashAcc = item.Value;
                Infragistics.Win.UltraWinTree.UltraTreeNode childNode = _rootNode.Nodes.Add(cashAcc.Name);
                foreach (KeyValuePair<string, CashSubAccount> itemSub in cashAcc.SubAccounts)
                {
                    CashSubAccount cashSubAcc = itemSub.Value;
                    childNode.Nodes.Add(cashSubAcc.Name);
                }
            }
        }
        private void BindAccountTypeCombo()
        {
            try
            {
                ValueList valList = new ValueList();
                valList = CashAccountDataManager.GetAllAccountTypes();
                cmbAccountType.DataSource = valList.ValueListItems;
                cmbAccountType.ValueMember = "DataValue";
                cmbAccountType.DisplayMember = "DisplayText";
                cmbAccountType.DataBind();
               // cmbAccountType.Value = valList.ValueListItems[0];
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }


        private void treeAccounts_AfterSelect(object sender, Infragistics.Win.UltraWinTree.SelectEventArgs e)
        {
            if (e.NewSelections.Count > 0)
            {

                _currentNode = (Infragistics.Win.UltraWinTree.UltraTreeNode)e.NewSelections[0];
                switch(_currentNode.Level)
                {
                    case 0:
                        lblInfo.Text = "Select To view Account Details or Add new Account by right clikcing on the Account node";

                        lblInfo.Visible = true;
                        lblName.Visible = false;
                        lblAcronym.Visible = false;
                        lblSubAccType.Visible = false;

                        txtBoxName.Visible = false;
                        txtBoxAcronym.Visible = false;
                        cmbAccountType.Visible = false;
                        break;

                    case 1:

                        CashAccount cashAcc = _cashAccountsDict[_currentNode.Key];
                        txtBoxName.Text = cashAcc.Name;
                        txtBoxAcronym.Text = cashAcc.Acronym;

                        lblInfo.Visible = false;
                        lblName.Visible = true;
                        lblAcronym.Visible = true;
                        lblSubAccType.Visible = false;

                        txtBoxName.Visible = true;
                        txtBoxAcronym.Visible = true;
                        cmbAccountType.Visible = false;
                        break;

                    case 2:

                        Infragistics.Win.UltraWinTree.UltraTreeNode nodeParent = _currentNode.Parent;
                        CashAccount cashAcc2 = _cashAccountsDict[nodeParent.Key];
                        CashSubAccount cashSubAcc = cashAcc2.SubAccounts[_currentNode.Key];
                        txtBoxName.Text = cashSubAcc.Name;
                        txtBoxAcronym.Text = cashSubAcc.Acronym;
                        cmbAccountType.Value = (object)cashSubAcc.TypeID;
                        
                        lblInfo.Visible = false;
                        lblName.Visible = true;
                        lblAcronym.Visible = true;
                        lblSubAccType.Visible = true;

                        txtBoxName.Visible = true;
                        txtBoxAcronym.Visible = true;
                        cmbAccountType.Visible = true;
                        break;

                    default:
                        MessageBox.Show("ERROR! ERROR!");
                        break;

                }
            }
        }

        private void txtBoxName_TextChanged(object sender, EventArgs e)
        {
            _currentNode.Selected = true;
            switch (_currentNode.Level)
            {
                case 1:
                    CashAccount cashAcc = _cashAccountsDict[_currentNode.Key];
                    cashAcc.Name = txtBoxName.Text;
                    _currentNode.Text = txtBoxName.Text;
                    break;
                    
                case 2:
                    Infragistics.Win.UltraWinTree.UltraTreeNode nodeParent = _currentNode.Parent;
                    CashAccount cashAcc2 = _cashAccountsDict[nodeParent.Key];
                    CashSubAccount cashSubAcc = cashAcc2.SubAccounts[_currentNode.Key];
                    cashSubAcc.Name = txtBoxName.Text;
                    _currentNode.Text = txtBoxName.Text;
                    break;

                default:
                    MessageBox.Show("ERROR, ERROR !!");
                    break;
            }
        }
        private void txtBoxAcronym_TextChanged(object sender, EventArgs e)
        {
            _currentNode.Selected = true;
            switch (_currentNode.Level)
            {
                case 1:
                    CashAccount cashAcc = _cashAccountsDict[_currentNode.Key];
                    cashAcc.Acronym = txtBoxAcronym.Text;
                    break;

                case 2:
                    Infragistics.Win.UltraWinTree.UltraTreeNode nodeParent = _currentNode.Parent;
                    CashAccount cashAcc2 = _cashAccountsDict[nodeParent.Key];
                    CashSubAccount cashSubAcc = cashAcc2.SubAccounts[_currentNode.Key];
                    cashSubAcc.Acronym = txtBoxAcronym.Text;
                    break;

                default:
                    MessageBox.Show("ERROR, ERROR !!");
                    break;
            }
        }
        private void cmbAccountType_ValueChanged(object sender, EventArgs e)
        {
            _currentNode.Selected = true;
            switch (_currentNode.Level)
            {

                case 2:
                    Infragistics.Win.UltraWinTree.UltraTreeNode nodeParent = _currentNode.Parent;
                    CashAccount cashAcc2 = _cashAccountsDict[nodeParent.Key];
                    CashSubAccount cashSubAcc = cashAcc2.SubAccounts[_currentNode.Key];
                    cashSubAcc.TypeID = Convert.ToInt32(cmbAccountType.Value);
                    break;

                default:
                    MessageBox.Show("ERROR, ERROR !!");
                    break;
            }
        }
    }
}
