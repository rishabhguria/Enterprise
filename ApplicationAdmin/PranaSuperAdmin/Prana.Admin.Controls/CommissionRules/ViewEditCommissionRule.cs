#region Using
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.Admin.BLL;
using System.Drawing;
using System.Windows.Forms;
#endregion

namespace Prana.Admin.Controls
{
    /// <summary>
    /// Summary description for ViewEditCommissionRule.
    /// </summary>
    public class ViewEditCommissionRule : System.Windows.Forms.UserControl
    {
        private const string FORM_NAME = "ViewEditCommissionRule: ";

        private Infragistics.Win.UltraWinGrid.UltraGrid grdCommissionRule;
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public ViewEditCommissionRule()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            // TODO: Add any initialization after the InitializeComponent call


        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if (grdCommissionRule != null)
                {
                    grdCommissionRule.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("RuleID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("AUECID", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("RuleName", 2);
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("View", 3);
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Edit", 4);
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn6 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ApplyRuleID", 5);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn7 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("RuleDescription", 6);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn8 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CalculationID", 7);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn9 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CurrencyID", 8);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn10 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CommissionRateID", 9);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn11 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Commission", 10);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn12 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ApplyCriteria", 11);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn13 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CommissionCriteriaID", 12);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn14 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CommissionCalculationID_FK", 13);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn15 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CommissionRuleCriteriaID", 14);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn16 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CommissionCriteriaID_FK", 15);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn17 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("OperatorID_FK", 16);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn18 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Value", 17);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn19 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CommissionRateID_FK", 18);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn20 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CommisionRate", 19);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn21 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("RuleID_FK", 20);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn22 = new Infragistics.Win.UltraWinGrid.UltraGridColumn(" MinimumCommissionRate", 21);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn23 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("AUECName", 22);
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            this.grdCommissionRule = new Infragistics.Win.UltraWinGrid.UltraGrid();
            ((System.ComponentModel.ISupportInitialize)(this.grdCommissionRule)).BeginInit();
            this.SuspendLayout();
            // 
            // grdCommissionRule
            // 
            this.grdCommissionRule.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.Hidden = true;
            ultraGridColumn1.Width = 325;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn2.Hidden = true;
            ultraGridColumn2.Width = 140;
            appearance1.FontData.BoldAsString = "False";
            appearance1.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn3.CellAppearance = appearance1;
            appearance2.FontData.BoldAsString = "True";
            appearance2.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn3.Header.Appearance = appearance2;
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridColumn3.Width = 125;
            appearance3.FontData.BoldAsString = "False";
            appearance3.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn4.CellAppearance = appearance3;
            appearance4.FontData.BoldAsString = "True";
            appearance4.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn4.Header.Appearance = appearance4;
            ultraGridColumn4.Header.VisiblePosition = 3;
            ultraGridColumn4.Hidden = true;
            ultraGridColumn4.NullText = "View";
            ultraGridColumn4.Width = 85;
            appearance5.FontData.BoldAsString = "False";
            appearance5.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn5.CellAppearance = appearance5;
            appearance6.FontData.BoldAsString = "True";
            appearance6.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn5.Header.Appearance = appearance6;
            ultraGridColumn5.Header.VisiblePosition = 4;
            ultraGridColumn5.NullText = "Edit";
            ultraGridColumn5.Width = 93;
            ultraGridColumn6.Header.VisiblePosition = 5;
            ultraGridColumn6.Hidden = true;
            ultraGridColumn6.Width = 66;
            ultraGridColumn7.Header.VisiblePosition = 6;
            ultraGridColumn7.Hidden = true;
            ultraGridColumn7.Width = 66;
            ultraGridColumn8.Header.VisiblePosition = 7;
            ultraGridColumn8.Hidden = true;
            ultraGridColumn8.Width = 71;
            ultraGridColumn9.Header.VisiblePosition = 8;
            ultraGridColumn9.Hidden = true;
            ultraGridColumn9.Width = 76;
            ultraGridColumn10.Header.VisiblePosition = 9;
            ultraGridColumn10.Hidden = true;
            ultraGridColumn10.Width = 17;
            ultraGridColumn11.Header.VisiblePosition = 10;
            ultraGridColumn11.Hidden = true;
            ultraGridColumn11.Width = 19;
            ultraGridColumn12.Header.VisiblePosition = 11;
            ultraGridColumn12.Hidden = true;
            ultraGridColumn12.Width = 21;
            ultraGridColumn13.Header.VisiblePosition = 12;
            ultraGridColumn13.Hidden = true;
            ultraGridColumn13.Width = 24;
            ultraGridColumn14.Header.VisiblePosition = 13;
            ultraGridColumn14.Hidden = true;
            ultraGridColumn14.Width = 28;
            ultraGridColumn15.Header.VisiblePosition = 14;
            ultraGridColumn15.Hidden = true;
            ultraGridColumn15.Width = 36;
            ultraGridColumn16.Header.VisiblePosition = 15;
            ultraGridColumn16.Hidden = true;
            ultraGridColumn16.Width = 40;
            ultraGridColumn17.Header.VisiblePosition = 16;
            ultraGridColumn17.Hidden = true;
            ultraGridColumn17.Width = 59;
            ultraGridColumn18.Header.VisiblePosition = 17;
            ultraGridColumn18.Hidden = true;
            ultraGridColumn18.Width = 67;
            ultraGridColumn19.Header.VisiblePosition = 18;
            ultraGridColumn19.Hidden = true;
            ultraGridColumn19.Width = 76;
            ultraGridColumn20.Header.VisiblePosition = 19;
            ultraGridColumn20.Hidden = true;
            ultraGridColumn20.Width = 87;
            ultraGridColumn21.Header.VisiblePosition = 20;
            ultraGridColumn21.Hidden = true;
            ultraGridColumn21.Width = 87;
            ultraGridColumn22.Header.VisiblePosition = 21;
            ultraGridColumn22.Hidden = true;
            appearance7.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn23.CellAppearance = appearance7;
            appearance8.FontData.BoldAsString = "True";
            appearance8.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn23.Header.Appearance = appearance8;
            ultraGridColumn23.Header.VisiblePosition = 22;
            ultraGridColumn23.Width = 237;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3,
            ultraGridColumn4,
            ultraGridColumn5,
            ultraGridColumn6,
            ultraGridColumn7,
            ultraGridColumn8,
            ultraGridColumn9,
            ultraGridColumn10,
            ultraGridColumn11,
            ultraGridColumn12,
            ultraGridColumn13,
            ultraGridColumn14,
            ultraGridColumn15,
            ultraGridColumn16,
            ultraGridColumn17,
            ultraGridColumn18,
            ultraGridColumn19,
            ultraGridColumn20,
            ultraGridColumn21,
            ultraGridColumn22,
            ultraGridColumn23});
            ultraGridBand1.Header.Enabled = false;
            ultraGridBand1.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdCommissionRule.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdCommissionRule.DisplayLayout.GroupByBox.Hidden = true;
            this.grdCommissionRule.DisplayLayout.MaxColScrollRegions = 1;
            this.grdCommissionRule.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdCommissionRule.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdCommissionRule.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdCommissionRule.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdCommissionRule.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdCommissionRule.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this.grdCommissionRule.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdCommissionRule.UseFlatMode = DefaultableBoolean.True;
            this.grdCommissionRule.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.grdCommissionRule.Location = new System.Drawing.Point(2, 8);
            this.grdCommissionRule.Name = "grdCommissionRule";
            this.grdCommissionRule.Size = new System.Drawing.Size(476, 252);
            this.grdCommissionRule.TabIndex = 55;
            this.grdCommissionRule.MouseUp += new System.Windows.Forms.MouseEventHandler(this.grdCommissionRule_MouseUp);
            // 
            // ViewEditCommissionRule
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.grdCommissionRule);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Name = "ViewEditCommissionRule";
            this.Size = new System.Drawing.Size(482, 264);
            this.Load += new System.EventHandler(this.ViewEditCommissionRule_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdCommissionRule)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private int _ruleID = int.MinValue;
        public int RuleID
        {
            set { _ruleID = value; }
            get { return _ruleID; }
        }
        private AllCommissionRules _allCommissionRules;


        // private bool _nullRow = false;
        //private AUECCommissionRule nullAUECCommissionRule;
        private bool bNullExist = false;

        private void ViewEditCommissionRule_Load(object sender, System.EventArgs e)
        {
            SetUp();

        }
        public void SetUp()
        {
            _allCommissionRules = CommissionRuleManager.GetAllCommissionRules();
            BindCommissionRuleGrid();
            grdCommissionRule.DisplayLayout.Bands[0].Columns["View"].CellAppearance.Cursor = Cursors.Hand;
            grdCommissionRule.DisplayLayout.Bands[0].Columns["View"].CellAppearance.ForeColor = Color.FromArgb(0, 0, 255);
            grdCommissionRule.DisplayLayout.Bands[0].Columns["View"].CellAppearance.FontData.Underline = DefaultableBoolean.True;
            grdCommissionRule.DisplayLayout.Bands[0].Columns["Edit"].CellAppearance.Cursor = Cursors.Hand;
            grdCommissionRule.DisplayLayout.Bands[0].Columns["Edit"].CellAppearance.ForeColor = Color.Red;
            grdCommissionRule.DisplayLayout.Bands[0].Columns["Edit"].CellAppearance.FontData.Underline = DefaultableBoolean.True;
        }
        public void BindCommissionRuleGrid()
        {
            _allCommissionRules = CommissionRuleManager.GetAllCommissionRules();

            AllCommissionRules newCommissionRules = new AllCommissionRules();
            foreach (AllCommissionRule allCommissionRule in _allCommissionRules)
            {
                int auecID = int.Parse(allCommissionRule.AUECID.ToString());
                AUEC auec = AUECManager.GetAUEC(auecID);
                //				auec.AUECID = auecID;
                //				auec.AssetID = 

                //SK 2061009 removed Compliance class
                //Currency currency = new Currency();
                //currency = AUECManager.GetCurrency(auec.Compliance.BaseCurrencyID);
                //
                //string auecName = auec.Asset.Name.ToString() + "/" + auec.UnderLying.Name.ToString() + "/" + auec.Exchange.DisplayName.ToString() + "/" + auec.Currency.CurrencySymbol.ToString();
                string auecName = auec.AUECString;

                //AllCommissionRule allCommissonRule = new AllCommissionRule();
                allCommissionRule.AUECName = auecName.ToString();
                newCommissionRules.Add(allCommissionRule);
            }
            grdCommissionRule.DataSource = _allCommissionRules;
            //grdCommissionRule.DataBind();

            ColumnsCollection columns = grdCommissionRule.DisplayLayout.Bands[0].Columns;
            foreach (UltraGridColumn column in columns)
            {
                if (column.Key != "RuleName" && column.Key != "View" && column.Key != "Edit" && column.Key != "AUECName")
                {
                    column.Hidden = true;
                }
            }

        }

        private void grdCommissionRule_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {

            if (bNullExist)
                return;
            UIElement objUIElement;
            UltraGridCell objUltraGridCell;
            objUIElement = grdCommissionRule.DisplayLayout.UIElement.ElementFromPoint(new Point(e.X, e.Y));
            if (objUIElement == null)
                return;
            objUltraGridCell = (UltraGridCell)objUIElement.GetContext(typeof(UltraGridCell));
            if (objUltraGridCell == null)
                return;
            int RuleID = int.Parse(objUltraGridCell.Row.Cells["RuleID"].Text.ToString());

            string Rulename = objUltraGridCell.Row.Cells["RuleName"].Text.ToString();









            // tbcCompanyMaster.SelectedTab = tbcCompanyMaster.Tabs[2];

            //if (objUltraGridCell.Text == "View")
            //{


            //AllCommissionRule allCommissionRuleView = new AllCommissionRule();
            //allCommissionRuleView.RuleID = int.Parse(RuleID.ToString());
            //CreateCommissionRules createCommissionRules = new CreateCommissionRules();

            //if (createCommissionRules != null)
            //{
            //    if ((grdCommissionRule.Rows.Count > 0) && (_nullRow == false))
            //    {
            //        createCommissionRules = new CreateCommissionRules();
            //        createCommissionRules.OpenView = true;
            //        createCommissionRules.DisableView = true;

            //        AllCommissionRule allCommissionRuleForView = new AllCommissionRule();
            //        allCommissionRuleForView = (AllCommissionRule)((Prana.Admin.BLL.AllCommissionRules)grdCommissionRule.DataSource)[grdCommissionRule.ActiveRow.Index];
            //        //int ruleID = int.Parse(allCommissionRuleForView.RuleID.ToString());
            //        int ruleID = RuleID;
            //        createCommissionRules.allCommissionRule = CommissionRuleManager.GetCommissionRuleByID(ruleID);

            //        createCommissionRules.Text = "View Commission Rule";
            //        createCommissionRules.ShowDialog(this.Parent);
            //        createCommissionRules = null;


            //    }
            //}
            //}

            if (objUltraGridCell.Text == "Edit")
            {
                //int RuleID = int.Parse(objUltraGridCell.Row.Cells["RuleID"].Text.ToString());


                //AllCommissionRule allCommissionRule = new AllCommissionRule();
                //allCommissionRule.RuleID = int.Parse(RuleID.ToString());
                //CreateCommissionRules createCommissionRules = new CreateCommissionRules();

                //if (createCommissionRules != null)
                //{
                //    if ((grdCommissionRule.Rows.Count > 0) && (_nullRow == false))
                //    {
                //        createCommissionRules = new CreateCommissionRules();
                //        createCommissionRules.DisableView = false;

                //        AllCommissionRule allCommissionRuleForEdit = new AllCommissionRule();
                //        allCommissionRuleForEdit = (AllCommissionRule)((Prana.Admin.BLL.AllCommissionRules)grdCommissionRule.DataSource)[grdCommissionRule.ActiveRow.Index];
                //        //int ruleID = int.Parse(allCommissionRuleForEdit.RuleID.ToString());
                //        int ruleID = RuleID;

                //        createCommissionRules.RuleID = RuleID;
                //        createCommissionRules.allCommissionRule = CommissionRuleManager.GetCommissionRuleByID(ruleID);


                //        //createCommissionRules.allCommissionRule = (AllCommissionRule)((Prana.Admin.BLL.AllCommissionRules) grdCommissionRule.DataSource)[grdCommissionRule.ActiveRow.Index];

                //        //						createCommissionRules.CurrentAllCommissionRules = (Prana.Admin.BLL.AllCommissionRules) grdCommissionRule.DataSource;
                //        //						createCommissionRules.allCommissionRule= (AllCommissionRule)((Prana.Admin.BLL.AllCommissionRules) grdCommissionRule.DataSource)[grdCommissionRule.ActiveRow.Index];

                //        createCommissionRules.Text = "Edit Commission Rule";
                //        createCommissionRules.ShowDialog(this.Parent);
                //        createCommissionRules = null;
                //        BindCommissionRuleGrid();
                //    }
                //}

                Form parent = this.FindForm();
                foreach (Control control in parent.Controls)
                {
                    if (control.GetType() == typeof(TreeView))
                    {
                        TreeView trv = (TreeView)control;
                        foreach (TreeNode node in trv.Nodes[0].Nodes)
                        {
                            if (Rulename == node.Text)
                            {
                                trv.SelectedNode = node;
                                break;
                            }
                        }
                        break;
                    }

                }

                foreach (Control control in parent.Controls)
                {
                    if (control.GetType() == typeof(Infragistics.Win.UltraWinTabControl.UltraTabControl))
                    {
                        Infragistics.Win.UltraWinTabControl.UltraTabControl tabc = (Infragistics.Win.UltraWinTabControl.UltraTabControl)control;
                        tabc.SelectedTab = tabc.Tabs[0];
                        break;
                    }

                }

            }



        }

    }
}
