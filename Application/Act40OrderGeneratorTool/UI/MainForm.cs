using Act40OrderGeneratorTool.Classes;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Act40OrderGeneratorTool
{
    public partial class MainForm : Form, IPluggableTools
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_RISK_MANAGEMENT);
            if (CustomThemeHelper.ApplyTheme)
            {
                this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, this.Text, CustomThemeHelper.UsedFont);
            }

            InitialiseData();

            if (!Manager.GetInstance().LoadRules())
            {
                MessageBox.Show(this, "No rules found. Loaded Default Rules.", "Order Generator", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            ultraGridOrders.DisplayLayout.Bands[0].Columns["SelectedFeedPrice"].SortComparer = new SortComparerDouble();
            ultraGridOrders.DisplayLayout.Bands[0].Columns["OriginalValueString"].SortComparer = new SortComparerDouble();
            ultraGridOrders.DisplayLayout.Bands[0].Columns["OriginalContributionString"].SortComparer = new SortComparerDouble();
            ultraGridOrders.DisplayLayout.Bands[0].Columns["OriginalQuantityString"].SortComparer = new SortComparerDouble();
            ultraGridOrders.DisplayLayout.Bands[0].Columns["TargetValueString"].SortComparer = new SortComparerDouble();
            ultraGridOrders.DisplayLayout.Bands[0].Columns["TargetContributionString"].SortComparer = new SortComparerDouble();
            ultraGridOrders.DisplayLayout.Bands[0].Columns["TargetQuantityString"].SortComparer = new SortComparerDouble();
            ultraGridOrders.DisplayLayout.Bands[0].Columns["TradeQuantityString"].SortComparer = new SortComparerDouble();
        }

        private void InitialiseData()
        {
            // Initialise the cache
            if (!Manager.GetInstance().LoadData())
            {
                MessageBox.Show(this, "Failed to load portfolio data.", "Order Generator", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // disable it all
                ultraButtonPreference.Enabled = false;
                ultraButtonGenerate.Enabled = false;
                ultraButtonExport.Enabled = false;
                ultraButtonViewDestination.Enabled = false;
                ultraComboTargetAccount.Enabled = false;
                ultraComboDestinationAccount.Enabled = false;
                return;
            }
            // Bind stuff to UI
            if (Preference.GetInstance().ModelPrefrence == ModelPrefrence.Account)
            {
                Misc.SetUpComboBox(ultraComboDestinationAccount, Act40OrderGeneratorTool.Cache.Account.GetInstance().GetAccounts());
                Misc.SetUpComboBox(ultraComboTargetAccount, Act40OrderGeneratorTool.Cache.Account.GetInstance().GetAccounts());

                if (Act40OrderGeneratorTool.Cache.Account.GetInstance().AccountExists(Preference.GetInstance().StartupDestination))
                    ultraComboDestinationAccount.Value = Preference.GetInstance().StartupDestination;

                if (Act40OrderGeneratorTool.Cache.Account.GetInstance().AccountExists(Preference.GetInstance().StartUpModel))
                    ultraComboTargetAccount.Value = Preference.GetInstance().StartUpModel;
            }
            else
            {
                Misc.SetUpComboBox(ultraComboDestinationAccount, Act40OrderGeneratorTool.Cache.MasterFund.GetInstance().GetMasterFunds());
                Misc.SetUpComboBox(ultraComboTargetAccount, Act40OrderGeneratorTool.Cache.MasterFund.GetInstance().GetMasterFunds());

                if (Act40OrderGeneratorTool.Cache.MasterFund.GetInstance().MasterFundExists(Preference.GetInstance().StartupDestination))
                    ultraComboDestinationAccount.Value = Preference.GetInstance().StartupDestination;

                if (Act40OrderGeneratorTool.Cache.MasterFund.GetInstance().MasterFundExists(Preference.GetInstance().StartUpModel))
                    ultraComboTargetAccount.Value = Preference.GetInstance().StartUpModel;
            }
        }

        private void ultraButtonPreference_Click(object sender, EventArgs e)
        {
            PrefrenceForm pf = new PrefrenceForm();
            pf.ShowDialog(this);
            InitialiseData();
        }

        private void ultraNumericEditorNav_ValueChanged(object sender, EventArgs e)
        {
            ultraNumericEditorCash.Value = Convert.ToDouble(ultraNumericEditorNav.Value) - Convert.ToDouble(ultraLabelCurrNav.Text);
        }

        private void ultraNumericEditorCash_ValueChanged(object sender, EventArgs e)
        {
            ultraNumericEditorNav.Value = Convert.ToDouble(ultraLabelCurrNav.Text) + Convert.ToDouble(ultraNumericEditorCash.Value);
        }

        private void ultraComboDestination_ValueChanged(object sender, EventArgs e)
        {
            ultraLabelCurrNav.Text = Manager.GetInstance().GetNav(ultraComboDestinationAccount.Value.ToString()).ToString();
            ultraNumericEditorCash.Value = 0.0;
            ultraNumericEditorNav.Value = Manager.GetInstance().GetNav(ultraComboDestinationAccount.Value.ToString());
        }

        private void Export_Click(object sender, EventArgs e)
        {
            ExportTrades();
        }

        /// <summary>
        /// Exports the trade to a Excel file
        /// </summary>
        private void ExportTrades()
        {
            try
            {
                if (ultraGridOrders.Rows.Count > 0)
                {
                    String folderPath = "";
                    SaveFileDialog dialog = new SaveFileDialog();
                    dialog.Filter = "CSV Files|*.csv;|Excel Files|*.xls;|All Files|*.*";
                    dialog.Title = "Export Trades";
                    dialog.FileName = String.Format("Order_Generation_Export_{0:dd_MMM_yyyy}", DateTime.Now);

                    if (dialog.ShowDialog(this) == DialogResult.OK)
                    {
                        folderPath = dialog.FileName;
                    }
                    if (!String.IsNullOrEmpty(folderPath))
                    {
                        if (folderPath.ToLower().EndsWith(".xls"))
                        {
                            ultraGridExcelExporter1.Export(ultraGridOrders, folderPath);
                        }
                        else
                        {
                            List<String> columnList = new List<String>();
                            List<String> columnHeaderList = new List<String>();
                            foreach (var column in ultraGridOrders.DisplayLayout.Bands[0].Columns)
                            {
                                columnList.Add(column.Key);
                                columnHeaderList.Add(column.Header.Caption);
                            }

                            if (File.Exists(folderPath))
                                File.Delete(folderPath);
                            //File.CreateText(folderPath);



                            using (StreamWriter sw = File.AppendText(folderPath))
                            {
                                sw.WriteLine(String.Join(",", columnHeaderList));
                                foreach (var row in ultraGridOrders.Rows)
                                {
                                    foreach (var column in columnList)
                                        sw.Write(String.Format(@"""{0}"",", row.GetCellValue(column)));
                                    sw.WriteLine();
                                }
                            }
                        }
                    }
                }
                else
                    MessageBox.Show(this, "Nothing To Export.", "Order Generator", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show(this, "Export failed.", "Order Generator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region IPluggableToolsMembers
        public void SetUP()
        {
            //throw new NotImplementedException();
        }

        public Form Reference()
        {
            return this;
        }

        public event EventHandler PluggableToolsClosed;

        public ISecurityMasterServices SecurityMaster
        {
            set { }
        }

        public IPostTradeServices PostTradeServices
        {
            set { }
        }

        public IPricingAnalysis PricingAnalysis
        {
            set { }
        }
        #endregion

        private void ViewModel_Click(object sender, EventArgs e)
        {
            try
            {
                ModelViewer mv = new ModelViewer();
                mv.SetUp(Manager.GetInstance().GetModelAccount(ultraComboTargetAccount.Value.ToString()), Manager.GetInstance().GetNav(ultraComboTargetAccount.Value.ToString()));
                mv.ShowDialog(this);
            }
            catch
            {
                MessageBox.Show(this, "An error occured while generating the Model Account", "Order Generator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GenerateOrder_Click(object sender, EventArgs e)
        {
            List<Trade> generatedOrders = Manager.GetInstance().Rebalance(ultraComboTargetAccount.Value.ToString(), ultraComboDestinationAccount.Value.ToString(), Manager.GetInstance().GetNav(ultraComboTargetAccount.Value.ToString()), Convert.ToDouble(ultraNumericEditorNav.Value), Manager.GetInstance().GetNav(ultraComboDestinationAccount.Value.ToString()));
            if (generatedOrders == null)
                MessageBox.Show(this, "An error occured while generating orders.", "Order Generator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                List<Trade> tradelist = generatedOrders.Where(x => x.TradeQuantity != 0 && !Double.IsInfinity(x.TradeQuantity) && !Double.IsNaN(x.TradeQuantity)).ToList();
                if (tradelist.Count == 0)
                    MessageBox.Show(this, "The calculations returned no trades.", "Order Generator", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    ultraGridOrders.DataSource = tradelist;
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (PluggableToolsClosed != null)
                PluggableToolsClosed(this, null);
        }

        private void ultraButton1_Click(object sender, EventArgs e)
        {
            String destination = ultraComboDestinationAccount.Value.ToString();
            String source = ultraComboTargetAccount.Value.ToString();
            InitialiseData();
            ultraComboTargetAccount.Value = source;
            ultraComboDestinationAccount.Value = destination;

            ultraLabelCurrNav.Text = Manager.GetInstance().GetNav(destination).ToString();
            ultraNumericEditorCash.Value = 0.0;
            ultraNumericEditorNav.Value = Manager.GetInstance().GetNav(destination);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.Alt | Keys.D))
            {
                ModelViewer mv = new ModelViewer();
                mv.SetUp(Manager.GetInstance().GetAccountPositions(ultraComboDestinationAccount.Value.ToString()), Manager.GetInstance().GetNav(ultraComboDestinationAccount.Value.ToString()));
                mv.ShowDialog(this);
                return true;
            }
            if (keyData == (Keys.Control | Keys.S))
            {
                ExportTrades();
                return true;
            }
            if (keyData == (Keys.F5))
            {
                InitialiseData();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

    }
}
