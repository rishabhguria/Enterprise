//using Prana.Reconciliation;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Windows.Forms;

namespace Prana.Tools
{
    public partial class MatchingRuleForm : Form
    {
        /// <summary>
        /// modified by: sachin mishra,02 Feb 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        public MatchingRuleForm()
        {
            try
            {
                InitializeComponent();
                if (!CustomThemeHelper.IsDesignMode() && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
                {
                    SetButtonsColor();
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

        private void SetButtonsColor()
        {
            try
            {
                btnRefresh.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnRefresh.ForeColor = System.Drawing.Color.White;
                btnRefresh.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnRefresh.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnRefresh.UseAppStyling = false;
                btnRefresh.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnOpenXML.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnOpenXML.ForeColor = System.Drawing.Color.White;
                btnOpenXML.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnOpenXML.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnOpenXML.UseAppStyling = false;
                btnOpenXML.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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

        /// <summary>
        /// modified by: sachin mishra,02 Feb 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        public void SetUp()
        {
            try
            {
                lstbxRules.Items.Clear();
                //foreach (string item in ReconPrefManager.RuleNames )
                //{
                //    lstbxRules.Items.Add(item);

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

        private void btnRefersh_Click(object sender, EventArgs e)
        {
            //ReconPrefManager.SetUp(Application.StartupPath + "/xmls/Rules/XmlMatchingRule.xml");
            //SetUp();
        }
        /// <summary>
        /// modified by: sachin mishra,02 Feb 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenXML_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog1.InitialDirectory = Application.StartupPath + "\\xmls\\Rules";
                openFileDialog1.Title = "Xml Rule Files";
                openFileDialog1.Filter = openFileDialog1.Filter = "Rule Files (*.xml)|*.xml";
                openFileDialog1.InitialDirectory = Application.StartupPath;
                DialogResult importResult = openFileDialog1.ShowDialog();
                if (importResult == DialogResult.OK)
                {

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

        private void lstbxRules_Click(object sender, EventArgs e)
        {
            //string name=lstbxRules.SelectedItem.ToString();
            //List<MatchingRule>  listDataTable=ReconPrefManager.GetListOfRules(name);
            //pnlRules.Controls.Clear();
            //int i = 0;
            //foreach (MatchingRule rule in listDataTable)
            //{
            //    RuleUserControl usrControl = new RuleUserControl();
            //    int ruleSeq = i + 1;
            //    usrControl.SetUp(rule.Data, "Rule" + ruleSeq.ToString());
            //    pnlRules.Controls.Add(usrControl);
            //    usrControl.Location = new Point(usrControl.Width * i + 5, 0);
            //    usrControl.Height = pnlRules.Height;
            //    i++;
            //}
        }
    }
}