using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle;
using ColumnStyle = Infragistics.Win.UltraWinGrid.ColumnStyle;

namespace Prana.NirvanaQualityChecker
{
    public partial class QualityCheck : Form
    {
        public static int IsConnected = 1;
        readonly XMLData _xmlDataValue = new XMLData();
        readonly string _path = AppDomain.CurrentDomain.BaseDirectory + "file.xml";
        //private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        protected string CurrentIp;
        protected bool CheckConnectionStatus;
        private Boolean _uiReadyforLoad;
        private TreeNode _startingNode;
        readonly List<String> _selectedModules = new List<string>();
        List<Script> _selectedScripts;
        public static DataSet ErrorDataSet;

        public QualityCheck()
        {
            InitializeComponent();
        }

        public void SetUpDataBaseRelatedField()
        {
            try
            {
                fundChooser.Items.Clear();
                ScriptManager.SetUpDataSource();
                ScriptManager.FundsDictionary.Clear();
                ScriptManager.Dictionaryofscripts.Clear();
                ScriptManager.ListDirectory(moduleTree);
                moduleTree.ExpandAll();
                RefreshFunds();
                _selectedModules.Clear();
                RefreshScripts();
                datagridview_ScriptSelecter.DataSource = new List<Script>();
                ColumnHeaderStyle();
                if (IsConnected == 1 && checkBox1.Checked)
                {
                    SavePreferences();
                }
                IsConnected = 1;
                ScriptManager.ReadScriptsOnPath();
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

        private void SavePreferences()
        {
            try
            {
                var doc = new XDocument(new XElement("Root", new XElement("ClinetDataBase", ScriptManager.ClientDatabase), new XElement("SMDataBase", ScriptManager.SmConnectionString), new XElement("IPAddress", ScriptManager.Datasource), new XElement("Checkbox", checkBox1.Checked)));
                doc.Save(_path);
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
        /// <summary>
        /// function for column header style like column header name, select all checkbox, sorting dissable, filter.
        /// </summary>
        public void ColumnHeaderStyle()
        {
            try
            {
                datagridview_ScriptSelecter.DisplayLayout.Bands[0].Columns["SelectScript"].Header.Caption = "Select Script";
                datagridview_ScriptSelecter.DisplayLayout.Bands[0].Columns["ScriptName"].Header.Caption = "Script Name";
                datagridview_ScriptSelecter.DisplayLayout.Bands[0].Columns["ScriptName"].CellAppearance.TextHAlign = HAlign.Right;
                datagridview_ScriptSelecter.DisplayLayout.Bands[0].Columns["ModuleName"].Header.Caption = "Module Name";
                datagridview_ScriptSelecter.DisplayLayout.Bands[0].Columns["ModuleName"].CellAppearance.TextHAlign = HAlign.Right;
                datagridview_ScriptSelecter.DisplayLayout.Bands[0].Columns["ErrorMessage"].Header.Caption = "Error Message";
                datagridview_ScriptSelecter.DisplayLayout.Bands[0].Columns["ErrorMessage"].CellAppearance.TextHAlign = HAlign.Right;

                datagridview_ScriptSelecter.DisplayLayout.Bands[0].Columns["SelectScript"].Header.CheckBoxVisibility = HeaderCheckBoxVisibility.WhenUsingCheckEditor;
                datagridview_ScriptSelecter.DisplayLayout.Bands[0].Columns["SelectScript"].Header.CheckBoxAlignment = HeaderCheckBoxAlignment.Right;
                datagridview_ScriptSelecter.DisplayLayout.Bands[0].Columns["SelectScript"].Header.CheckBoxSynchronization = HeaderCheckBoxSynchronization.RowsCollection;

                datagridview_ScriptSelecter.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.True;

                datagridview_ScriptSelecter.DisplayLayout.Bands[0].Columns[0].SortIndicator = SortIndicator.Disabled;
                datagridview_ScriptSelecter.DisplayLayout.Bands[0].Columns[1].SortIndicator = SortIndicator.Disabled;
                datagridview_ScriptSelecter.DisplayLayout.Bands[0].Columns[2].SortIndicator = SortIndicator.Disabled;
                datagridview_ScriptSelecter.DisplayLayout.Bands[0].Columns[3].SortIndicator = SortIndicator.Disabled;
                datagridview_ScriptSelecter.DisplayLayout.Bands[0].Columns[4].SortIndicator = SortIndicator.Disabled;
                datagridview_ScriptSelecter.DisplayLayout.Bands[0].Columns[5].SortIndicator = SortIndicator.Disabled;
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

        private void datePickerEnabler_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                dateTimePicker1.Enabled = datePickerEnabler.Checked;
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

        private void refreshButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckConnectionStatus == true)
                {
                    ScriptManager.ListDirectory(moduleTree);
                    moduleTree.ExpandAll();
                }
                else
                {
                    moduleTree.Nodes.Clear();
                }
                RefreshScripts();
                _selectedModules.Clear();
                datagridview_ScriptSelecter.DataSource = new List<Script>();
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


        // Code related to binding of Scripts
        #region Module Chooser UI
        private void scriptsList_AfterCheck(object sender, TreeViewEventArgs e)
        {

            if (_uiReadyforLoad)
            {
                LoadGrid();
                _uiReadyforLoad = false;
            }
        }

        private void CheckAllChildNodes(TreeNode treeNode, bool nodeChecked)
        {
            if (nodeChecked)
            {
                if (!_selectedModules.Contains(ScriptManager.RootPath + treeNode.FullPath))
                {
                    _selectedModules.Add(ScriptManager.RootPath + treeNode.FullPath);
                }
            }
            else
            {
                _selectedModules.Remove(ScriptManager.RootPath + treeNode.FullPath);
            }

            foreach (TreeNode node in treeNode.Nodes)
            {
                node.Checked = nodeChecked;

                // If the current node has child nodes, call the CheckAllChildsNodes method recursively. 
                CheckAllChildNodes(node, nodeChecked);
            }

            if (treeNode == _startingNode)
            {
                _uiReadyforLoad = true;
            }
        }

        private bool checkChildNodeState(List<string> selectedModules, string _treeNode)
        {
            selectedModules.Add(_treeNode);
            return true;
        }

        //This function checks all the selected scripts

        private void scriptsList_BeforeCheck(object sender, TreeViewCancelEventArgs e)
        {
            moduleTree.BeginUpdate();

            ///Changing cursor position afret single click.
            Cursor.Position = new Point(Cursor.Position.X, Cursor.Position.Y + 1);

            if (e.Action != TreeViewAction.Unknown)
            {
                /* Calls the CheckAllChildNodes method, passing in the current 
                Checked value of the TreeNode whose checked state changed. */
                _startingNode = e.Node;
                CheckAllChildNodes(e.Node, !e.Node.Checked);
            }

            moduleTree.EndUpdate();
        }

        private void LoadGrid()
        {
            datagridview_ScriptSelecter.DataSource = new List<Script>();
            _selectedScripts = new List<Script>();
            foreach (String module in _selectedModules)
            {
                List<Script> modulescripts = new List<Script>();
                if (ScriptManager.Dictionaryofscripts.TryGetValue(module, out modulescripts))
                {
                    _selectedScripts.AddRange(modulescripts);
                }
            }

            datagridview_ScriptSelecter.DataSource = _selectedScripts;
            ///adding button to view column and styling
            datagridview_ScriptSelecter.DisplayLayout.Bands[0].Columns["View"].Style = ColumnStyle.Button;
            datagridview_ScriptSelecter.DisplayLayout.Bands[0].Override.ButtonStyle = UIElementButtonStyle.Button3D;
            datagridview_ScriptSelecter.DisplayLayout.Bands[0].Columns["View"].ButtonDisplayStyle = ButtonDisplayStyle.Always;
        }

        #endregion

        private async void diagnose_Click(object sender, EventArgs e)
        {
            Logger.LoggerWrite("Running Scripts. . .", LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information);
            try
            {
                if (_selectedScripts != null)
                {

                    if (getCountOfSelectedScripts(_selectedScripts) != 0)
                    {

                        datagridview_ScriptSelecter.Visible = false;
                        searchinggif.Visible = true;
                        String commaSeparatedFundids = "";
                        String[] funds = fundChooser.Text.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        if (funds != null && funds.Length > 0)
                        {
                            StringBuilder sb = new StringBuilder("");
                            foreach (String fund in funds)
                            {
                                if (ScriptManager.FundsDictionary.ContainsValue(fund))
                                {
                                    int fundid = ScriptManager.FundsDictionary.Keys.Single(k => ScriptManager.FundsDictionary[k].Equals(fund));
                                    sb.Append("," + fundid);
                                }
                            }
                            commaSeparatedFundids = sb.ToString().Substring(1);
                        }
                        if (_selectedScripts != null && _selectedScripts.Count > 0)
                        {
                            if (!datePickerEnabler.Checked || DateTime.Compare((dateTimePicker2.Value).Date, (dateTimePicker1.Value).Date) >= 0)
                            {
                                btnDiagnose.Text = " Checking. . ";

                                foreach (Script s in _selectedScripts)
                                {
                                    s.DatabaseResult = null;
                                }

                                DisableUi();

                                bool b = await Task.Run(() => ScriptManager.ScriptsExecute(_selectedScripts, (datePickerEnabler.Checked) ? dateTimePicker1.Value.Date : dateTimePicker2.Value.Date, dateTimePicker2.Value.Date, String.Empty, commaSeparatedFundids));
                                if (b)
                                {
                                    EnableUi();
                                    btnDiagnose.Text = "Diagnose";
                                    datagridview_ScriptSelecter.Refresh();
                                    hideSearchingIcon();
                                }
                            }
                            else
                            {
                                hideSearchingIcon();
                                MessageBox.Show("From date can not be greater than To date.");
                            }
                        }
                        else
                        {
                            hideSearchingIcon();
                            MessageBox.Show("Nothing to diagnose.");
                        }

                        //for status cell painting red or green.
                        int count = datagridview_ScriptSelecter.DisplayLayout.Rows.Count;
                        for (int i = 0; i < count; i++)
                        {
                            UltraGridRow ultraGridRow = datagridview_ScriptSelecter.DisplayLayout.Rows[i];
                            UltraGridCell ultraGridCell = ultraGridRow.Cells["Status"];
                            UltraGridCell ultraGridCellScriptName = ultraGridRow.Cells["ScriptName"];
                            String ScriptName = ultraGridCellScriptName.Value.ToString();
                            if ((string)ultraGridCell.Value == "✔")
                            {
                                ultraGridCell.Appearance.ForeColor = Color.Green;
                                //Logger.LogQC(Level.Info, Status.Success, string.Empty, ErrorMessage, ModuleName, ScriptName);
                                //Log.Info(GetLogMessage(ModuleName, ScriptName, ErrorMessage));
                            }
                            else if ((string)ultraGridCell.Value == "!")
                            {
                                ultraGridCell.Appearance.ForeColor = Color.Orange;
                                ultraGridCell.Appearance.FontData.Bold = DefaultableBoolean.True;
                                ultraGridCell.Appearance.FontData.SizeInPoints = 15f;
                                //Logger.LogQC(Level.Info, Status.Success, string.Empty, ErrorMessage, ModuleName, ScriptName);
                                //Log.Info(GetLogMessage(ModuleName, ScriptName, ErrorMessage));
                            }
                            else if ((string)ultraGridCell.Value == "✘")
                            {
                                ultraGridCell.Appearance.ForeColor = Color.Red;
                                //Logger.LogQC(Level.Info, Status.Success, string.Empty, ErrorMessage, ModuleName, ScriptName);
                                //Log.Info(GetLogMessage(ModuleName, ScriptName, ErrorMessage));
                            }
                            ultraGridCell.Appearance.TextHAlign = HAlign.Right;
                        }
                    }
                    else
                    {
                        MessageBox.Show("No script selected.");
                    }
                }
                else
                {
                    MessageBox.Show("No script selected.");
                }
            }
            catch (Exception ex)
            {
                Logger.LoggerWrite(ex, LoggingConstants.CATEGORY_FLAT_FILE_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, "Running Script Failed.");
                btnDiagnose.Enabled = true;
                btnDiagnose.Text = "Diagnose";
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void RefreshScripts()
        {
            try
            {
                if (_selectedScripts != null)
                {
                    foreach (Script s in _selectedScripts)
                    {
                        s.DatabaseResult = null;
                    }
                }
                Logger.LoggerWrite("Scripts refreshed", LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information);
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

        private void RefreshFunds()
        {
            try
            {
                /* Adding checkboxes within the combo box with available funds */
                foreach (KeyValuePair<int, String> kvp in ScriptManager.FundsDictionary)
                {
                    fundChooser.Items.Add(new CheckBoxItem(kvp.Value, kvp.Key));
                }

                /* Select all the items in Combo Box */
                for (int i = 0; i < fundChooser.Items.Count; i++)
                {
                    fundChooser.SetItemChecked(i, true);
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
        /// <summary>
        /// cell painting, row height, cell activation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void datagridview_ScriptSelecter_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                //height of column
                datagridview_ScriptSelecter.DisplayLayout.Bands[0].Override.DefaultRowHeight = 22;

                // cell painting green and red
                int count = datagridview_ScriptSelecter.DisplayLayout.Rows.Count;
                for (int i = 0; i < count; i++)
                {
                    UltraGridRow ultraGridRow = datagridview_ScriptSelecter.DisplayLayout.Rows[i];
                    UltraGridCell ultraGridCell = ultraGridRow.Cells["Status"];
                    if ((string)ultraGridCell.Value == "✔")
                    {
                        ultraGridCell.Appearance.ForeColor = Color.Green;
                    }
                    else if ((string)ultraGridCell.Value == "✘")
                    {
                        ultraGridCell.Appearance.ForeColor = Color.Red;
                    }
                }

                //read only mode for column scriptname,errormessage,modulename
                e.Row.Cells[1].Activation = Activation.ActivateOnly;
                e.Row.Cells[2].Activation = Activation.ActivateOnly;
                e.Row.Cells[3].Activation = Activation.ActivateOnly;
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


        private void hideSearchingIcon()
        {
            datagridview_ScriptSelecter.Visible = true;
            searchinggif.Visible = false;
        }
        /// <summary>
        /// it show the particular error in error viewer form, with module name, script name, error message. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void datagridview_ScriptSelecter_ClickCellButton(object sender, CellEventArgs e)
        {
            try
            {
                if (datagridview_ScriptSelecter.DisplayLayout.Bands[0].Columns[e.Cell.Column.Index].ToString().Equals("View", StringComparison.InvariantCultureIgnoreCase) && e.Cell.Row.Index > -1)
                {
                    ErrorDataSet = _selectedScripts[e.Cell.Row.Index].DatabaseResult;
                    var ErrorSummaryForm = new ErrorViewer();
                    if (_selectedScripts[e.Cell.Row.Index].Status == "?")
                    {
                        MessageBox.Show("This script is not executed yet. Please diagnose it to view result.");
                    }
                    else if (_selectedScripts[e.Cell.Row.Index].Status == "✘" && _selectedScripts[e.Cell.Row.Index].ErrorMessage == "Script failure. Error running it.")
                    {
                        MessageBox.Show("Script did not run successfully.");
                    }
                    else if (_selectedScripts[e.Cell.Row.Index].Status == "✔")
                    {
                        MessageBox.Show("No error found.");
                    }
                    else if (_selectedScripts[e.Cell.Row.Index].Status != "✔")
                    {
                        //         List<Detail> ld = new List<Detail>();
                        Detail ds = new Detail();
                        ds.Module = _selectedScripts[e.Cell.Row.Index].ModuleName;
                        ds.Script = _selectedScripts[e.Cell.Row.Index].ScriptName;
                        ds.Error = _selectedScripts[e.Cell.Row.Index].ErrorMessage;
                        ErrorSummaryForm.Show();
                    }
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

        private int getCountOfSelectedScripts(IEnumerable<Script> selectedScripts)
        {
            int count = 0;
            try
            {
                count += selectedScripts.Count(S => S.SelectScript);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return count;
        }
        private void DisableUi()
        {
            try
            {
                datePickerEnabler.Enabled = false;
                dateTimePicker2.Enabled = false;
                btnDiagnose.Enabled = false;
                moduleTree.Enabled = false;
                dateTimePicker1.Enabled = false;
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

        private void EnableUi()
        {
            try
            {
                datePickerEnabler.Enabled = true;
                dateTimePicker2.Enabled = true;
                btnDiagnose.Enabled = true;
                moduleTree.Enabled = true;
                dateTimePicker1.Enabled = true;
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

        private void ErrorDetectorTool_Load(object sender, EventArgs e)
        {
            try
            {
                CustomThemeHelper.SetThemeProperties(FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_PRANA_TRADING_TICKET);
                ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, Text, CustomThemeHelper.UsedFont);

                GetXmlValues();
                bool state = _xmlDataValue._Checkbox;
                checkBox1.Checked = state;
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

        private void GetXmlValues()
        {
            try
            {
                if (File.Exists(_path))
                {
                    XmlDocument doc1 = new XmlDocument();
                    doc1.Load(_path);
                    XmlElement root = doc1.DocumentElement;
                    XmlNodeList nodes3 = root.SelectNodes("Checkbox"); // You can also use XPath here
                    foreach (XmlNode node in nodes3)
                    {
                        _xmlDataValue._Checkbox = Convert.ToBoolean(node.InnerText);
                    }
                    if (_xmlDataValue._Checkbox == true)
                    {
                        XmlNodeList nodes = root.SelectNodes("ClinetDataBase"); // You can also use XPath here
                        foreach (XmlNode node in nodes)
                        {
                            _xmlDataValue._ClinetDBName = node.InnerText;
                        }
                        XmlNodeList nodes1 = root.SelectNodes("SMDataBase"); // You can also use XPath here
                        foreach (XmlNode node in nodes1)
                        {
                            _xmlDataValue._SMDBName = node.InnerText;
                        }
                        XmlNodeList nodes2 = root.SelectNodes("IPAddress"); // You can also use XPath here
                        foreach (XmlNode node in nodes2)
                        {
                            _xmlDataValue._IPAddress = node.InnerText;
                        }
                    }
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

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                UpdateXmlData();
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
        /// <summary>
        /// Update Checkbox value on check changed
        /// </summary>
        private void UpdateXmlData()
        {
            try
            {
                if (!File.Exists(_path))
                {
                    SavePreferences();
                }
                XmlDocument doc = new XmlDocument();
                doc.Load(_path);
                doc.SelectSingleNode("//Root/Checkbox").InnerText = checkBox1.Checked.ToString();
                doc.Save(_path);
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
    }
}
