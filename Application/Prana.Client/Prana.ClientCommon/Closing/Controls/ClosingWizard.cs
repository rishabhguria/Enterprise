using Infragistics.Win.UltraWinTree;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Constants;
using Prana.LogManager;
using Prana.PubSubService.Interfaces;
using Prana.Utilities.UI;
using Prana.Utilities.UI.UIUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Prana.ClientCommon
{
    public partial class ClosingWizard : Form, IPublishing
    {
        string _previousText = string.Empty;
        bool isRootNode = false;
        private const string TAB_Main = "TabMain";
        private const string TAB_CLOSINGALGO = "TabClosingAlgo";
        private const string TAB_STANDARDFILTER = "TabStandardFilter";
        private const string TAB_CUSTOMFILTER = "TabCustomFilter";
        private const string BTN_BACK = "< Back";
        private const string BTN_PREV = "< Prev";
        // private bool _isViewMode = false;
        private const int CP_NOCLOSE_BUTTON = 0x200;


        //private Point _parentFormLocation = new Point(0,0);

        public event EventHandler RunClosing;
        public event EventHandler RunUnwinding;
        public event EventHandler PreviewDataBasedOnTemplate;
        private delegate Point GetFormLocationInvoker(Form form);
        ClosingTemplate _previewTemplate = new ClosingTemplate();

        ActivityIndicator _controlActivityIndicator = null;

        public ClosingWizard()
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(CustomThemeHelper.WHITELABELTHEME) && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
            {
                SetButtonsColor();
            }
        }

        private void SetButtonsColor()
        {
            try
            {
                btnCancel.BackColor = System.Drawing.Color.FromArgb(140, 5, 5);
                btnCancel.ForeColor = System.Drawing.Color.White;
                btnCancel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnCancel.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnCancel.UseAppStyling = false;
                btnCancel.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnNext.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnNext.ForeColor = System.Drawing.Color.White;
                btnNext.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnNext.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnNext.UseAppStyling = false;
                btnNext.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnBack.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnBack.ForeColor = System.Drawing.Color.White;
                btnBack.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnBack.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnBack.UseAppStyling = false;
                btnBack.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnFinish.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnFinish.ForeColor = System.Drawing.Color.White;
                btnFinish.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnFinish.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnFinish.UseAppStyling = false;
                btnFinish.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }


        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        }
        private void InitializeTemplates()
        {
            try
            {
                BindTree();
                this.optionMain.ClosingOptionSelected += new EventHandler(optionMain_ClosingOptionSelected);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }

            }
        }

        public void CancelOperation()
        {


            ActivityIndicatorHelper.StopActivityIndicator(_controlActivityIndicator);

        }

        private void BindTree()
        {

            try
            {

                Dictionary<string, List<ClosingTemplate>> dictTemplates = ClosingPrefManager.DictClosingTemplates;
                //To clear the tree of any node before binding it afresh.
                treeViewClosing.Nodes.Clear();

                UltraTreeNode treeNodeClosing = new UltraTreeNode(ClosingType.Closing.ToString(), ClosingType.Closing.ToString());
                treeNodeClosing.Override.NodeAppearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;
                UltraTreeNode treeNodeUnwinding = new UltraTreeNode(ClosingType.UnWinding.ToString(), ClosingType.UnWinding.ToString());
                treeNodeUnwinding.Override.NodeAppearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;


                treeViewClosing.Nodes.Add(treeNodeClosing);
                treeViewClosing.Nodes.Add(treeNodeUnwinding);

                if (dictTemplates.Count > 0)
                {
                    foreach (KeyValuePair<string, List<ClosingTemplate>> kvp in dictTemplates)
                    {
                        foreach (ClosingTemplate template in kvp.Value)
                        {
                            string templateName = template.TemplateName;
                            string rootTemplate = kvp.Key;


                            string key = rootTemplate + '_' + templateName;

                            //add child node
                            UltraTreeNode treeNode = new UltraTreeNode(key, templateName);

                            //treeNode.Override.NodeAppearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.False;
                            //treeNode.
                            treeViewClosing.Nodes[rootTemplate].Nodes.Add(treeNode);
                        }
                    }

                }
                else
                {
                    AddDefaultNodes();
                }
                if (treeViewClosing.Nodes[0].Nodes.Count > 0)
                    treeViewClosing.Nodes[0].Nodes[0].Selected = true;
                else if (treeViewClosing.Nodes[1].Nodes.Count > 0)
                    treeViewClosing.Nodes[1].Nodes[0].Selected = true;
                treeViewClosing.ExpandAll();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }


        private void AddDefaultNodes()
        {
            try
            {
                foreach (string rootTemplate in ClosingPrefManager.getRootTemplates())
                {
                    UltraTreeNode treeNodeDefault = new UltraTreeNode(rootTemplate + '_' + rootTemplate + "_NEW", rootTemplate + "_NEW");
                    //treeNodeDefault.Override.NodeAppearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.False;
                    treeViewClosing.Nodes[rootTemplate].Nodes.Add(treeNodeDefault);
                    ClosingPrefManager.AddTemplate(rootTemplate, rootTemplate + "_NEW");
                }

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            treeViewClosing.DoubleClick += new EventHandler(treeViewClosing_DoubleClick);
            try
            {
                if ((this.optionMain.rdBtnRunClosing.Checked) && this.tabClosingWizard.SelectedTab.Key.Equals(TAB_Main))
                {
                    RunOperation();
                }
                else
                {
                    this.tabClosingWizard.PerformAction(Infragistics.Win.UltraWinTabControl.UltraTabControlAction.SelectNextTab);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }

            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.Compare(tabClosingWizard.SelectedTab.Key, TAB_Main, true) == 0)
                {
                    this.optionMain.rdBtnPreviewData.Checked = true;
                    // EnableDisableUI(true);

                }
                else
                {
                    this.tabClosingWizard.PerformAction(Infragistics.Win.UltraWinTabControl.UltraTabControlAction.SelectPreviousTab);
                }
                //}

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }

            }
        }

        private void tabClosingWizard_ActiveTabChanged(object sender, Infragistics.Win.UltraWinTabControl.ActiveTabChangedEventArgs e)
        {

        }

        private void tabClosingWizard_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            try
            {
                if (treeViewClosing.SelectedNodes.Count > 0)
                {
                    LoadDataForSelectedTab();
                }
                btnBack.Enabled = true;
                btnNext.Enabled = true;
                btnFinish.Enabled = true;

                if (string.Compare(e.Tab.Key, TAB_Main, true) == 0)
                {
                    //EnableDisableUI(true);
                    this.optionMain.rdBtnPreviewData.Checked = true;
                    if (e.PreviousSelectedTab == null)
                    {
                        EnableDisableUI();
                    }
                    this.Text = "Closing Wizard";

                    //btnNext.Enabled = true;
                    //btnBack.Enabled = true;
                    // EnableDisableUI(true);
                }
                //else
                //{
                //    EnableDisableUI(true);
                //    //btnNext.Enabled = true;
                //    //btnBack.Enabled = false;
                //    this.Text = "Closing Wizard";
                //}
                else if (string.Compare(e.Tab.Key, TAB_CLOSINGALGO, true) == 0)
                {
                    btnNext.Enabled = false;
                    // btnFinish.Enabled = true;
                    if (!optionMain.rdBtnPreviewData.Checked)
                    {
                        if (optionMain.rdBtnDeleteTemplate.Checked)
                        {
                            this.Text = "Delete Template";
                        }
                        else if (optionMain.rdbtnAddClosingTemplate.Checked || optionMain.rdBtnAddUnwindingTemplate.Checked || optionMain.rdbtnEditTemplate.Checked)
                        {
                            this.Text = "Create/Edit Templates: Set Closing Algo";
                        }
                    }

                    return;
                }
                //else
                //{
                //    btnNext.Enabled = true;
                //    btnFinish.Enabled = false;

                //    return;
                //}
                else if (string.Compare(e.Tab.Key, TAB_STANDARDFILTER, true) == 0)
                {
                    btnFinish.Enabled = false;
                    if (!optionMain.rdBtnPreviewData.Checked)
                    {
                        if (optionMain.rdBtnDeleteTemplate.Checked)
                        {
                            this.Text = "Delete Template";
                        }
                        else if (optionMain.rdbtnAddClosingTemplate.Checked || optionMain.rdBtnAddUnwindingTemplate.Checked || optionMain.rdbtnEditTemplate.Checked)
                        {
                            this.Text = "Create/Edit Templates: Set Standard Filters";
                        }
                    }
                    else
                    {
                        this.Text = "Preview Data: Set Standard Filters";
                    }
                }
                else if (e.Tab.Key == TAB_CUSTOMFILTER)
                {
                    if (this.optionMain.rdBtnPreviewData.Checked || treeViewClosing.ActiveNode.RootNode.Key.Equals(ClosingType.UnWinding.ToString()))
                    {
                        btnNext.Enabled = false;
                        // btnFinish.Enabled = true;
                    }
                    else
                    {
                        btnFinish.Enabled = false;
                    }
                    if (!optionMain.rdBtnPreviewData.Checked)
                    {

                        if (optionMain.rdBtnDeleteTemplate.Checked)
                        {
                            this.Text = "Delete Template";
                        }
                        else if (optionMain.rdbtnAddClosingTemplate.Checked || optionMain.rdBtnAddUnwindingTemplate.Checked || optionMain.rdbtnEditTemplate.Checked)
                        {
                            this.Text = "Create/Edit Templates: Set Custom Filters";
                        }
                    }
                    else
                    {
                        this.Text = "Preview Data: Set Custom Filters";
                    }
                }

                if (this.optionMain.rdBtnPreviewData.Checked)
                {
                    ClosingPrefManager.LoadDefaultDataForTemplate(_previewTemplate);
                    LoadDataForSelectedTab();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }

            }
        }

        internal void setup(CompanyUser _user)
        {
            try
            {
                CreateSubscriptionServicesProxy();
                ctrlClosingAlgo1.closingPreferencesUsrCtr.SetUp(_user);
                InitializeTemplates();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }

            }
        }

        private void DisposeProxies()
        {
            try
            {
                if (_proxy != null)
                {
                    _proxy.InnerChannel.UnSubscribe(Topics.Topic_ClosingStatus);
                    _proxy.Dispose();
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

        // bool _isNewTemplate = false;
        private void addTemplateMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                treeViewClosing.DoubleClick -= new EventHandler(treeViewClosing_DoubleClick);
                string closingType = treeViewClosing.ActiveNode.RootNode.Key;

                if (sender.ToString().Equals(ClosingType.Closing.ToString()))
                {
                    closingType = ClosingType.Closing.ToString();
                }
                else if (sender.ToString().Equals(ClosingType.UnWinding.ToString()))
                {
                    closingType = ClosingType.UnWinding.ToString();
                }
                UltraTreeNode newNode = new UltraTreeNode();
                newNode.Text = string.Empty;
                treeViewClosing.Nodes[closingType].Nodes.Add(newNode);
                treeViewClosing.ActiveNode = newNode;
                treeViewClosing.ActiveNode.Selected = true;
                treeViewClosing.ActiveNode.BeginEdit();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }

            }
        }
        void runToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            RunOperation();
        }


        //private void runAllTemplateMenuItem_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        //List<string> listSelectedNodes = new List<string>();
        //        //List<ClosingTemplate> closingTemplates = new List<ClosingTemplate>();

        //        //foreach (UltraTreeNode node in treeViewClosing.ActiveNode.RootNode.Nodes)
        //        //{
        //        //    if (node.CheckedState == CheckState.Checked)
        //        //    {
        //        //        listSelectedNodes.Add(node.Text);
        //        //    }
        //        //}


        //        //foreach (string nodetext in listSelectedNodes)
        //        //{
        //        //    closingTemplates.AddRange(ClosingPrefManager.GetClosingTemplates(treeViewClosing.ActiveNode.RootNode.Text, nodetext));
        //        //}

        //        //RunOperation(closingTemplates);
        //    }
        //    catch (Exception ex)
        //    {
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }

        //}

        private void LaunchActivityIndicator()
        {
            try
            {
                if (_controlActivityIndicator == null)
                {
                    _controlActivityIndicator = new ActivityIndicator();
                    _controlActivityIndicator.Location = new Point(80, 188);
                }
                this.optionMain.Controls.Add(_controlActivityIndicator);
                ActivityIndicatorHelper.StartActivityIndicator(_controlActivityIndicator);


            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }

            }
        }

        private Point GetFormNewLocation(Form parentForm)
        {
            Point formLocation = new Point();
            try
            {

                bool isParentFormNull = false;
                if (parentForm == null)
                {
                    parentForm = this;
                    isParentFormNull = true;
                }
                if (UIValidation.GetInstance().validate(parentForm))
                {
                    if (parentForm.InvokeRequired)
                    {
                        GetFormLocationInvoker mi = new GetFormLocationInvoker(GetFormNewLocation);
                        parentForm.Invoke(mi, parentForm);
                    }
                    else
                    {
                        if (!isParentFormNull)
                        {
                            formLocation = new Point(parentForm.Location.X + 50, parentForm.Location.Y + 50);
                        }
                        else
                        {
                            formLocation = new Point(this.Location.X, this.Location.Y + this.Size.Height);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }

            }
            return formLocation;
        }


        //void _formActivityIndicator_FormClosed(object sender, FormClosedEventArgs e)
        //{
        //    try
        //    {

        //        _controlActivityIndicator.FormClosed -= new FormClosedEventHandler(_formActivityIndicator_FormClosed);
        //        _controlActivityIndicator = null;
        //    }
        //    catch (Exception ex)
        //    {
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }

        //    }
        //}


        private void BringFormToFront()
        {
            try
            {
                if (UIValidation.GetInstance().validate(_controlActivityIndicator))
                {
                    if (_controlActivityIndicator.InvokeRequired)
                    {
                        _controlActivityIndicator.BeginInvoke(new MethodInvoker(BringFormToFront));
                    }
                }
                else
                {
                    _controlActivityIndicator.BringToFront();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }

            }
        }
        private void deleteTemplateMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DeleteSelectedTemplates();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }

            }
        }


        private bool DeleteSelectedTemplates()
        {
            try
            {
                List<UltraTreeNode> listSelectedNodes = new List<UltraTreeNode>();
                string rootNode = string.Empty;
                foreach (UltraTreeNode node in treeViewClosing.Nodes)
                {

                    foreach (UltraTreeNode childNode in node.Nodes)
                    {
                        if (childNode.CheckedState == CheckState.Checked)
                        {
                            rootNode = node.RootNode.Key;
                            listSelectedNodes.Add(childNode);
                        }

                    }

                }
                if (listSelectedNodes.Count > 0)
                {

                    // string name = string.Empty;

                    // UltraTreeNode node = treeViewClosing.ActiveNode;.

                    DialogResult dialog = MessageBox.Show("Do you want to delete the selected Template(s)?", "Closing Templates", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialog.ToString().Equals(DialogResult.Yes.ToString()))
                    {
                        // node.RootNode.Selected = true;
                        //= treeViewClosing.ActiveNode.RootNode.Key;


                        foreach (UltraTreeNode node in listSelectedNodes)
                        {
                            treeViewClosing.Nodes[rootNode].Nodes.Remove(node);
                            ClosingPrefManager.RemoveTemplate(rootNode, node.Text);
                        }


                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("Nothing to Delete. Please Select Some Template(s).", "Closing Templates", MessageBoxButtons.OK);
                    return false;

                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }

            }

            return true;

        }

        private void RunOperation()
        {
            try
            {
                string rootNode = string.Empty;
                List<string> listSelectedNodes = new List<string>();
                List<ClosingTemplate> closingTemplates = new List<ClosingTemplate>();

                foreach (UltraTreeNode node in treeViewClosing.Nodes)
                {
                    foreach (UltraTreeNode childNode in node.Nodes)
                    {
                        if (childNode.CheckedState == CheckState.Checked)
                        {
                            rootNode = node.RootNode.Text;
                            listSelectedNodes.Add(childNode.Text);
                        }

                    }

                }

                foreach (string nodetext in listSelectedNodes)
                {
                    closingTemplates.AddRange(ClosingPrefManager.GetClosingTemplates(rootNode, nodetext));
                }
                if (closingTemplates.Count > 0)
                {

                    this.optionMain.rdbtnAddClosingTemplate.Visible = false;
                    // this.optionMain.rdBtnPreviewData.Visible = false;
                    this.optionMain.rdBtnRunClosing.Visible = false;
                    this.optionMain.label2.Visible = false;
                    this.optionMain.ultraLabel2.Text = string.Empty;
                    btnCancel.Enabled = false;
                    btnNext.Enabled = false;
                    btnBack.Enabled = false;
                    //_parentFormLocation = new Point(this.Location.X, this.Location.Y);
                    //Thread ActivityIndicatorThread = new Thread(LaunchActivityIndicator);
                    //ActivityIndicatorThread.IsBackground = true;
                    //ActivityIndicatorThread.SetApartmentState(ApartmentState.STA);
                    //ActivityIndicatorThread.Start();
                    LaunchActivityIndicator();
                    if (rootNode.Equals(ClosingType.Closing.ToString()))
                    {
                        if (RunClosing != null)
                        {
                            RunClosing(closingTemplates, null);
                        }
                    }
                    else
                    {
                        if (RunUnwinding != null)
                        {
                            RunUnwinding(closingTemplates, null);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please select some Templates", "Closing Wizard");
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

        private void optionMain_ClosingOptionSelected(object sender, EventArgs e)
        {
            try
            {
                /// if (this.optionMain.rdbtnAddTemplate.Checked)
                //  _isViewMode = false;
                //else
                //    _isViewMode = true;

                // string selectedOption = ((System.Windows.Forms.ButtonBase)(sender)).Text;


                EnableDisableUI();
                PerformAction();


            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }

            }
        }

        public void EnableDisableUI()
        {
            try
            {
                mnuRootTemplate.Items[0].Visible = false;
                mnuRootTemplate.Items[1].Visible = false;
                btnBack.Enabled = false;
                btnNext.Enabled = true;
                btnFinish.Enabled = true;
                btnCancel.Enabled = true;
                this.treeViewClosing.Hide();


                // this.optionMain.rdBtnPreviewData.Checked = true;
                //show all options...
                this.optionMain.rdbtnAddClosingTemplate.Visible = true;
                this.optionMain.rdBtnAddUnwindingTemplate.Visible = true;
                this.optionMain.rdBtnPreviewData.Visible = true;
                this.optionMain.rdBtnRunClosing.Visible = true;
                this.optionMain.rdBtnDeleteTemplate.Visible = true;
                this.optionMain.rdbtnEditTemplate.Visible = true;



                this.optionMain.label2.Visible = true;
                this.optionMain.label2.Location = new Point(38, 360);
                StringBuilder textString = new StringBuilder();

                textString.Append("Note: This Wizard will help you create a custom template for the automated closing and unwinding of transactions.");
                textString.Append(Environment.NewLine);
                textString.Append("This is very helpful in case you have a large volume of data or complex closing methods.");

                this.optionMain.label2.Text = textString.ToString();
                this.optionMain.ultraLabel2.Text = "Wizard Options";

                if (this.optionMain.Controls.Contains(_controlActivityIndicator))
                {
                    this.optionMain.Controls.Remove(_controlActivityIndicator);
                }


                if (this.optionMain.rdBtnPreviewData.Checked)
                {
                    //this.ultraPanel2.Visible = false;
                    // this.ultraPanel2.Dock = DockStyle.None;
                    this.ultraPanel1.Visible = false;
                    this.treeViewClosing.Hide();
                    this.treeViewClosing.Dock = DockStyle.None;
                    this.ctrlClosingAlgo1.Dock = DockStyle.Fill;
                    this.ctrlCustomFilters.Dock = DockStyle.Fill;
                    this.ctrlStandardFilters.Dock = DockStyle.Fill;
                    this.optionMain.Dock = DockStyle.Fill;
                    this.pnlWizardBtns.Dock = DockStyle.Bottom;
                    btnBack.Enabled = false;
                    btnFinish.Enabled = false;
                }
                else
                {

                    this.optionMain.label2.Visible = false;


                    //hide all the options...
                    this.optionMain.rdBtnPreviewData.Visible = false;
                    this.optionMain.rdbtnAddClosingTemplate.Visible = false;
                    this.optionMain.rdBtnPreviewData.Visible = false;
                    this.optionMain.rdBtnRunClosing.Visible = false;
                    this.optionMain.rdBtnDeleteTemplate.Visible = false;
                    this.optionMain.rdbtnEditTemplate.Visible = false;
                    this.optionMain.rdBtnAddUnwindingTemplate.Visible = false;

                    if (this.optionMain.rdBtnRunClosing.Checked)
                    {

                        btnBack.Enabled = true;
                        btnNext.Enabled = true;
                        btnFinish.Enabled = false;
                        this.optionMain.label2.Visible = false;
                        this.treeViewClosing.Show();
                        this.optionMain.label2.Visible = true;
                        this.optionMain.label2.Text = "Please Select some Templates and click Next...";
                        this.optionMain.label2.Location = new Point(46, 250); ;
                        this.treeViewClosing.Override.NodeStyle = NodeStyle.CheckBox;
                        this.optionMain.ultraLabel2.Text = "Run Closing/Unwinding";
                    }
                    else
                    {

                        //mnuRootTemplate.Items[0].Visible = true;
                        //mnuRootTemplate.Items[1].Visible = false;
                        // btnBack.Enabled = false;
                        btnFinish.Enabled = false;
                        btnBack.Enabled = true;
                        this.treeViewClosing.Show();
                        this.treeViewClosing.Size = new Size(163, 512);

                        if (optionMain.rdBtnDeleteTemplate.Checked)
                        {
                            this.optionMain.label2.Visible = true;
                            this.optionMain.label2.Text = "Please Select the template(s) to delete and click Finish...";
                            this.optionMain.label2.Location = new Point(46, 250);
                            this.optionMain.ultraLabel2.Text = "Delete Template";

                            btnNext.Enabled = false;
                            btnFinish.Enabled = true;
                            this.treeViewClosing.Override.NodeStyle = NodeStyle.CheckBox;
                        }
                        else
                        {
                            this.treeViewClosing.Override.NodeStyle = NodeStyle.Default;
                        }

                    }
                    //this.ultraPanel2.Dock = DockStyle.Top;
                    this.ultraPanel1.Dock = DockStyle.Left;
                    this.treeViewClosing.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
                    this.ultraPanel1.Visible = true;
                    //this.ultraPanel2.Visible = true;
                    this.treeViewClosing.Show();

                    //this.optionMain.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                    this.optionMain.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
                    this.ctrlClosingAlgo1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
                    this.ctrlCustomFilters.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
                    this.ctrlStandardFilters.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

                    this.pnlWizardBtns.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }

            }
        }


        private void PerformAction()
        {
            try
            {
                if (this.optionMain.rdbtnAddClosingTemplate.Checked)
                {
                    addTemplateMenuItem_Click(ClosingType.Closing.ToString(), null);
                    //   btnNext_Click(null, null);

                }
                else if (this.optionMain.rdBtnAddUnwindingTemplate.Checked)
                {
                    addTemplateMenuItem_Click(ClosingType.UnWinding.ToString(), null);
                    //  btnNext_Click(null, null);
                }
                else if (this.optionMain.rdBtnDeleteTemplate.Checked)
                {


                    // btnNext_Click(null, null);

                }
                else if (this.optionMain.rdbtnEditTemplate.Checked)
                {

                    btnNext_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }

            }

        }
        private void treeViewClosing_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    treeViewClosing.PointToScreen(e.Location);
                    UltraTreeNode selectedNode = treeViewClosing.GetNodeFromPoint(e.Location);
                    treeViewClosing.ActiveNode = selectedNode;
                    treeViewClosing.ActiveNode.Selected = true;
                    if (treeViewClosing.ActiveNode.IsRootLevelNode)
                    {
                        // mnuRootTemplate.Show(pt);
                    }
                    else
                    {

                        // mnuTemplate.Show(pt);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void treeViewClosing_AfterLabelEdit(object sender, NodeEventArgs e)
        {
            try
            {
                if (!isRootNode)
                {
                    string closingType = treeViewClosing.ActiveNode.RootNode.Key;
                    string currentText = e.TreeNode.Text;
                    string key = closingType + '_' + currentText;
                    if (!string.Equals(_previousText, e.TreeNode.Text, StringComparison.OrdinalIgnoreCase) && treeViewClosing.ActiveNode.RootNode.Nodes.Exists(key))
                    {
                        MessageBox.Show("Template with the same name already exists, Please enter a different name", "Closing Templates", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        e.TreeNode.Text = _previousText;
                        treeViewClosing.ActiveNode.Remove();
                        return;
                    }
                    if (string.IsNullOrEmpty(treeViewClosing.ActiveNode.Text) || string.IsNullOrWhiteSpace(treeViewClosing.ActiveNode.Text))
                    {
                        MessageBox.Show("Template name cannot be empty or blank spaces.", "Closing Templates", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        treeViewClosing.ActiveNode.Remove();
                        return;
                    }
                    if (ClosingPrefManager.CheckTemplateExists(closingType, _previousText))
                    {
                        e.TreeNode.Key = string.Empty;
                        e.TreeNode.Key = string.Concat(e.TreeNode.RootNode.Text, "_", e.TreeNode.Text);
                        ClosingPrefManager.UpdateTemplateName(closingType, _previousText, currentText);
                    }
                    //if (ClosingPrefManager.CheckTemplateExists(closingType, treeViewClosing.ActiveNode.Text))
                    //{
                    //    ClosingPrefManager.UpdateTemplateName(closingType, _previousText, currentText);
                    //    return;
                    //}
                    else
                    {
                        ClosingPrefManager.AddTemplate(closingType, treeViewClosing.ActiveNode.Text);
                    }
                }
                else
                {
                    e.TreeNode.Text = _previousText;
                    isRootNode = false;
                    return;
                }

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }

            }
        }

        private void treeViewClosing_BeforeLabelEdit(object sender, CancelableNodeEventArgs e)
        {
            try
            {

                if (e.TreeNode.Text == ClosingType.Closing.ToString() || e.TreeNode.Text == ClosingType.UnWinding.ToString())
                {
                    isRootNode = true;
                    e.Cancel = true;
                    return;

                }

                else
                {
                    _previousText = e.TreeNode.Text;
                    isRootNode = false;
                }

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

        private void treeViewClosing_BeforeSelect(object sender, BeforeSelectEventArgs e)
        {
            try
            {
                if (e.NewSelections.Count > 0 && !e.NewSelections[0].IsRootLevelNode)
                {
                    if (treeViewClosing.SelectedNodes.Count > 0)
                    {
                        UpdateDataForSelectedTab();

                    }
                }
                else
                {
                    e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }

            }
        }

        private void UpdateDataForSelectedTab()
        {
            ClosingTemplate closingTemplate = null;
            try
            {
                if (!this.optionMain.rdBtnPreviewData.Checked)
                {
                    if (treeViewClosing.SelectedNodes.Count > 0 && !ClosingPrefManager.getRootTemplates().Contains(treeViewClosing.SelectedNodes[0].Key))
                    {
                        string TemplateName = treeViewClosing.SelectedNodes[0].Text;
                        string closingType = treeViewClosing.SelectedNodes[0].RootNode.Text;
                        List<ClosingTemplate> closingTemplates = ClosingPrefManager.GetClosingTemplates(closingType, TemplateName);
                        if (closingTemplates.Count > 0)
                        {
                            closingTemplate = closingTemplates[0];
                        }
                    }
                }
                else
                {
                    closingTemplate = _previewTemplate;
                }
                if (tabClosingWizard.SelectedTab != null && closingTemplate != null)
                {
                    string selectedTabKey = tabClosingWizard.SelectedTab.Key;
                    switch (selectedTabKey)
                    {
                        case TAB_Main:
                            break;

                        case TAB_STANDARDFILTER:
                            ctrlStandardFilters.UpdateStandardFilters(closingTemplate);

                            break;
                        case TAB_CUSTOMFILTER:
                            ctrlCustomFilters.UpdateCustomFilters(closingTemplate);

                            break;

                        case TAB_CLOSINGALGO:
                            ctrlClosingAlgo1.UpdateClosingPreferences(closingTemplate);
                            break;

                    }
                    if (!this.optionMain.rdBtnPreviewData.Checked)
                    {
                        ClosingPrefManager.UpdateTemplates(closingTemplate);
                    }
                }


            }




            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }

            }
        }

        private void treeViewClosing_AfterSelect(object sender, SelectEventArgs e)
        {
            try
            {
                if (e.NewSelections.Count > 0)
                {
                    LoadDataForSelectedTab();
                }


                if (!this.optionMain.rdBtnPreviewData.Checked && tabClosingWizard.SelectedTab != null)
                {
                    if (tabClosingWizard.SelectedTab.Key == TAB_CUSTOMFILTER)
                    {
                        if (treeViewClosing.ActiveNode.RootNode.Key.Equals(ClosingType.UnWinding.ToString()))
                        {
                            btnNext.Enabled = false;
                            btnFinish.Enabled = true;
                        }
                        else
                        {
                            btnNext.Enabled = true;
                            btnFinish.Enabled = false;
                        }

                    }
                    else if (tabClosingWizard.SelectedTab.Key == TAB_CLOSINGALGO)
                    {
                        if (treeViewClosing.ActiveNode.RootNode.Key.Equals(ClosingType.UnWinding.ToString()))
                        {
                            btnBack_Click(null, null);
                            btnNext.Enabled = false;
                            btnFinish.Enabled = true;
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }

            }
        }

        private void LoadDataForSelectedTab()
        {
            try
            {
                ClosingTemplate template = null;
                if (!this.optionMain.rdBtnPreviewData.Checked)
                {


                    string TemplateName = treeViewClosing.SelectedNodes[0].Text;
                    string closingType = treeViewClosing.SelectedNodes[0].RootNode.Text;
                    List<ClosingTemplate> templates = ClosingPrefManager.GetClosingTemplates(closingType, TemplateName);

                    if (templates.Count > 0)
                        template = templates[0];
                }
                else
                {
                    template = _previewTemplate;

                }
                if (template != null)
                {
                    if (tabClosingWizard.SelectedTab != null)
                    {
                        string selectedTabKey = tabClosingWizard.SelectedTab.Key;
                        switch (selectedTabKey)
                        {
                            case TAB_STANDARDFILTER:
                                ctrlStandardFilters.LoadStandardFilters(template);
                                ctrlStandardFilters.EnableDisableControl(template.ClosingType.ToString(), this.optionMain.rdBtnPreviewData.Checked);
                                break;

                            case TAB_CUSTOMFILTER:
                                ctrlCustomFilters.LoadCustomFilters(template);
                                break;

                            case TAB_CLOSINGALGO:
                                ctrlClosingAlgo1.LoadClosingAlgo(template);
                                break;

                                //case TAB_Main:
                                //    this.optionMain.rdBtnPreviewData.Checked = true;
                                //    break;


                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void ClosingWizardUI_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                _controlActivityIndicator = null;
                //this.Size
                DisposeProxies();
                ClosingPrefManager.Dispose();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }

            }
        }


        void btnCancel_Click(object sender, System.EventArgs e)
        {

            try
            {
                ClosingPrefManager.GetExistingTemplates();
                this.Close();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }

            }

        }

        private void btnFinish_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = DialogResult.Yes;
                if (!this.optionMain.rdBtnPreviewData.Checked && !this.optionMain.rdBtnDeleteTemplate.Checked)
                {

                    result = MessageBox.Show("Do you want to exit the wizard or go back to the options page Yes/No?", "Closing Wizard", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);
                }

                if (this.optionMain.rdBtnDeleteTemplate.Checked)
                {
                    if (!DeleteSelectedTemplates())
                    {
                        return;
                    }
                }

                UpdateDataForSelectedTab();
                ClosingPrefManager.SaveClosingTemplates();
                if (result.ToString().Equals(DialogResult.Yes.ToString()))
                {
                    if (!this.optionMain.rdBtnPreviewData.Checked)
                    {
                        ClosingPrefManager.Dispose();
                    }
                    else
                    {
                        if (PreviewDataBasedOnTemplate != null && _previewTemplate != null)
                        {
                            PreviewDataBasedOnTemplate(_previewTemplate, null);
                        }
                    }
                    this.Close();
                }
                else
                {

                    if (this.tabClosingWizard.SelectedTab.Key != TAB_Main)
                    {
                        this.tabClosingWizard.SelectedTab = tabClosingWizard.Tabs[0];
                    }
                    else
                    {
                        this.optionMain.rdBtnPreviewData.Checked = true;

                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }

            }
        }


        public void OperationCompleted()
        {
            // this.tabClosingWizard.SelectedTab = tabClosingWizard.Tabs[0];
            //EnableDisableUI(true);
            btnBack.Enabled = false;
            btnFinish.Enabled = true;
            btnNext.Enabled = false;
        }
        private void optionMain_Load(object sender, EventArgs e)
        {

        }


        private void tabClosingWizard_SelectedTabChanging(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangingEventArgs e)
        {
            try
            {
                //if (treeViewClosing.SelectedNodes.Count > 0)
                //{
                UpdateDataForSelectedTab();
                //}
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

        DuplexProxyBase<ISubscription> _proxy;
        public void CreateSubscriptionServicesProxy()
        {
            try
            {
                _proxy = new DuplexProxyBase<ISubscription>("TradeSubscriptionEndpointAddress", this);
                _proxy.Subscribe(Topics.Topic_ClosingStatus, null);
            }

            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
        #region IPublishing Members

        public void Publish(Prana.BusinessObjects.MessageData e, string topicName)
        {
            try
            {
                if (UIValidation.GetInstance().validate(_controlActivityIndicator))
                {
                    UIThreadMarshellerPublish mi = new UIThreadMarshellerPublish(Publish);
                    if (_controlActivityIndicator.InvokeRequired)
                    {
                        _controlActivityIndicator.BeginInvoke(mi, new object[] { e, topicName });
                    }
                    else
                    {
                        System.Object[] dataList = null;

                        switch (topicName)
                        {
                            case Topics.Topic_ClosingStatus:
                                dataList = (System.Object[])e.EventData;
                                foreach (Object obj in dataList)
                                {
                                    ProgressInfo info = (ProgressInfo)obj;
                                    if (!string.IsNullOrEmpty(info.HeaderText))
                                    {
                                        ActivityIndicatorHelper.UpdateProgress(info, _controlActivityIndicator);
                                        Application.DoEvents();

                                        if (info.IsTaskCompleted)
                                        {
                                            Thread.Sleep(500);
                                            OperationCompleted();
                                            // btnCancel.Enabled = true;
                                            // btnFinish.Enabled = true;
                                        }
                                    }

                                }
                                break;
                        }
                    }
                }
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
            finally
            {


            }
        }


        public string getReceiverUniqueName()
        {
            return "Closing Wizard";
        }

        #endregion

        private void ctrlStandardFilters_Load(object sender, EventArgs e)
        {

        }

        bool ishandled = false;
        private void treeViewClosing_AfterCheck(object sender, NodeEventArgs e)
        {
            try
            {

                if (ishandled)
                {
                    return;
                }

                if (e.TreeNode.RootNode.Text.Equals(ClosingType.Closing.ToString()) && e.TreeNode.CheckedState == CheckState.Checked)
                {
                    treeViewClosing.Nodes[ClosingType.UnWinding.ToString()].CheckedState = CheckState.Unchecked;
                }
                else if (e.TreeNode.RootNode.Text.Equals(ClosingType.UnWinding.ToString()) && e.TreeNode.CheckedState == CheckState.Checked)
                {
                    treeViewClosing.Nodes[ClosingType.Closing.ToString()].CheckedState = CheckState.Unchecked;
                }
                //treeViewClosing.AfterCheck -= treeViewClosing_AfterCheck;
                if (e.TreeNode.IsRootLevelNode && e.TreeNode.CheckedState != CheckState.Indeterminate)
                {
                    foreach (UltraTreeNode node in e.TreeNode.RootNode.Nodes)
                    {
                        ishandled = true;
                        node.CheckedState = e.TreeNode.RootNode.CheckedState;

                    }
                }
                else
                {
                    //treeViewClosing.AfterCheck -= treeViewClosing_AfterCheck;
                    int i = 0;
                    foreach (UltraTreeNode node in e.TreeNode.RootNode.Nodes)
                    {
                        if (node.CheckedState == CheckState.Unchecked)
                        {
                            i++;
                        }
                    }
                    if (i == e.TreeNode.RootNode.Nodes.Count)
                    {
                        ishandled = true;
                        e.TreeNode.RootNode.CheckedState = CheckState.Unchecked;
                    }
                    else if (i > 0)
                    {
                        ishandled = true;
                        e.TreeNode.RootNode.CheckedState = CheckState.Indeterminate;
                    }
                    else
                    {
                        ishandled = true;
                        e.TreeNode.RootNode.CheckedState = CheckState.Checked;
                    }
                    //treeViewClosing.AfterCheck += treeViewClosing_AfterCheck;
                }
                ishandled = false;
                // treeViewClosing.Refresh();
                // treeViewClosing.Update();
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
        private void treeViewClosing_BeforeCheck(object sender, BeforeCheckEventArgs e)
        {
            try
            {
                //if (e.TreeNode.IsRootLevelNode)
                //{
                //    if (e.TreeNode.RootNode.Text.Equals(ClosingType.Closing))
                //    {
                //        treeViewClosing.Nodes[ClosingType.UnWinding.ToString()].CheckedState = CheckState.Unchecked;
                //    }
                //    else
                //    {
                //        treeViewClosing.Nodes[ClosingType.Closing.ToString()].CheckedState = CheckState.Unchecked;
                //    }

                //}

                //if (isHandled)
                //{
                //    e.Cancel = true;
                //}
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

        private void ClosingWizard_Load(object sender, EventArgs e)
        {
            try
            {
                CustomThemeHelper.SetThemeProperties(this, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_CLOSING_WIZARD);
                this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, this.Text, CustomThemeHelper.UsedFont);
                if (!CustomThemeHelper.ApplyTheme)
                {
                    Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
                    appearance1.BackColor = System.Drawing.Color.SteelBlue;
                    appearance1.BackColor2 = System.Drawing.Color.White;
                    appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
                    appearance1.FontData.BoldAsString = "True";
                    appearance1.ForeColor = System.Drawing.Color.White;
                    this.ultraLabel1.Appearance = appearance1;
                }
                else if (CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
                {
                    Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
                    appearance1.BackColor = System.Drawing.Color.FromArgb(210, 211, 212);
                    appearance1.BackColor2 = System.Drawing.Color.DimGray;
                    appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
                    appearance1.FontData.BoldAsString = "True";
                    appearance1.ForeColor = System.Drawing.Color.Black;
                    this.ultraLabel1.Appearance = appearance1;
                }
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

        private void treeViewClosing_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                treeViewClosing.ActiveNode.BeginEdit();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        #region IServiceOnDemandStatus Members
        public System.Threading.Tasks.Task<bool> HealthCheck()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
