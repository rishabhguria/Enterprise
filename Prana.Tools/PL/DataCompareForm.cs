using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.ReconciliationNew;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.UI;
using Prana.Utilities.UI.UIUtilities;
using Prana.Utilities.XMLUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
//using Excel = Microsoft.Office.Interop.Excel;
namespace Prana.Tools
{
    public partial class DataCompareForm : Form, IPluggableTools, ILaunchForm
    {
        private IPluggableUserControl dataCtrlBrokerOrig;
        private IPluggableUserControl dataCtrlAppMatched;
        private IPluggableUserControl dataCtrlBrokerMatched;
        private IPluggableUserControl dataCtrlAppOrig;


        BackgroundWorker _bgReconcile = null;



        //List<MatchingRule> ruleList = new List<MatchingRule>();
        public DataCompareForm()
        {
            try
            {
                InitializeComponent();
                ReadComparisionXsltSettigns();
                BindRuleCombo();
                ChangeDisplayStatus();
                SetToolSetting();
                SetupSnapshotControl();
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
        private void SetToolSetting()
        {
            try
            {
                //subMenuExcelExport.Visible = false;
                removeDuplicateToolStripMenuItem.Visible = false;
                // chkBoxClrExpCache.Checked = ReconPrefManager.ReconPreferences.IsClearExpCache;
                //btnFastCompare.Visible = true;
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
        private void ReadComparisionXsltSettigns()
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("DisplayName");
                dt.Columns.Add("Name");
                string path = Application.StartupPath + @"\" + ApplicationConstants.MAPPING_FILE_DIRECTORY + @"\" + ApplicationConstants.MappingFileType.ReconXSLT.ToString();
                DirectoryInfo dir = new DirectoryInfo(path);
                dt.Rows.Add(new object[] { ApplicationConstants.C_COMBO_SELECT, int.MinValue });
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                FileAndDbSyncManager.SyncFileWithDataBase(Application.StartupPath, ApplicationConstants.MappingFileType.ReconXSLT);
                FileAndDbSyncManager.SyncFileWithDataBase(Application.StartupPath, ApplicationConstants.MappingFileType.ReconMappingXml);
                FileAndDbSyncManager.SyncFileWithDataBase(Application.StartupPath, ApplicationConstants.MappingFileType.ReconRulesFile);
                string dllPathXML = Application.StartupPath + @"\" + ApplicationConstants.MAPPING_FILE_DIRECTORY + @"\" + ApplicationConstants.MappingFileType.ReconRulesFile.ToString() + "\\PluggableControls.xml";
                XmlDocument xmldocDll = new XmlDocument();
                xmldocDll.Load(dllPathXML);
                XmlNodeList xmlNodesDlls = xmldocDll.SelectNodes("CompareToolConfig/PluggableControls/PluggableControl");
                List<IPluggableUserControl> controlList = new List<IPluggableUserControl>();
                foreach (XmlNode xmlNode in xmlNodesDlls)
                {
                    Assembly asmAssemblyContainingForm = Assembly.LoadFrom(Application.StartupPath + "/Modules/" + xmlNode.Attributes["Dll"].Value);
                    Type typeToLoad = asmAssemblyContainingForm.GetType(xmlNode.Attributes["ClassName"].Value);
                    IPluggableUserControl ctrl = (IPluggableUserControl)Activator.CreateInstance(typeToLoad);
                    controlList.Add(ctrl);
                }

                foreach (FileInfo f in dir.GetFiles("*.*"))
                {
                    dt.Rows.Add(new object[] { f.Name.Substring(0, f.Name.Split('.')[0].Length), f.Name });
                }

                dataCtrlAppOrig = controlList[0];
                dataCtrlBrokerOrig = controlList[1];
                dataCtrlBrokerMatched = controlList[2];
                dataCtrlAppMatched = controlList[3];
                this.splitContainer3.Panel1.Controls.Add(dataCtrlBrokerMatched.Control);
                this.splitContainer3.Panel2.Controls.Add(dataCtrlBrokerOrig.Control);
                this.splitContainer2.Panel1.Controls.Add(this.dataCtrlAppMatched.Control);
                this.splitContainer2.Panel2.Controls.Add(this.dataCtrlAppOrig.Control);
                dataCtrlAppMatched.Control.Dock = DockStyle.Fill;
                dataCtrlBrokerMatched.Control.Dock = DockStyle.Fill;
                dataCtrlBrokerOrig.Control.Dock = DockStyle.Fill;
                dataCtrlAppOrig.Control.Dock = DockStyle.Fill;
                dataCtrlBrokerOrig.SetUp(dt, "Un-Matched Broker Data");
                dataCtrlAppOrig.SetUp(dt, "Un-Matched Nirvana Data");
                dataCtrlBrokerMatched.SetUp(null, "Matched Broker Data");
                dataCtrlAppMatched.SetUp(null, "Matched Nirvana Data");
                dataCtrlAppOrig.DataReloaded += new EventHandler(ClearMatchedDataGrids);
                dataCtrlBrokerOrig.DataReloaded += new EventHandler(ClearMatchedDataGrids);
                //dataCtrlAppMatched.DataReloaded += new EventHandler(ClearMatchedDataGrids);
                //dataCtrlBrokerMatched.DataReloaded += new EventHandler(ClearMatchedDataGrids);

                // PranaPositionViewer dataCtrlAppOrig1 = (PranaPositionViewer)dataCtrlAppOrig;
                // DataImportCtrl dataCtrlBrokerOrig1 = (DataImportCtrl)dataCtrlBrokerOrig;
                // DataImportCtrl dataCtrlAppMatched1 = (DataImportCtrl)dataCtrlAppMatched;
                //DataImportCtrl dataCtrlBrokerMatched1 = (DataImportCtrl)dataCtrlBrokerMatched; 
                dataCtrlAppOrig.SelectedValueChanged += new EventHandler(dataCtrlBrokerOrig.ValueSelected);
                //dataCtrlAppOrig.SelectedValueChanged += new EventHandler(dataCtrlAppMatched.ValueSelected);
                //dataCtrlAppOrig.SelectedValueChanged += new EventHandler(dataCtrlBrokerMatched.ValueSelected);
                //dataCtrlAppOrig.SelectedValueChanged += new EventHandler(dataCtrlBrokerOrig.ValueSelected);

                dataCtrlAppOrig.FilterChanged += new EventHandler(dataCtrlBrokerOrig.ApplyFilters);
                dataCtrlBrokerOrig.FilterChanged += new EventHandler(dataCtrlAppOrig.ApplyFilters);

                dataCtrlAppMatched.FilterChanged += new EventHandler(dataCtrlBrokerMatched.ApplyFilters);
                dataCtrlBrokerMatched.FilterChanged += new EventHandler(dataCtrlAppMatched.ApplyFilters);

                //XmlNodeList xmlLeftColumns = xmldocDll.SelectNodes("CompareToolConfig/LeftGridColumns/Column");
                //XmlNodeList xmlRightColumns = xmldocDll.SelectNodes("CompareToolConfig/RightGridColumns/Column");

                //List<string> leftColumnList = new List<string>();
                //foreach (XmlNode colNode in xmlLeftColumns)
                //{
                //    string col = colNode.Attributes["Name"].Value;
                //    leftColumnList.Add(col);
                //}
                //List<string> rightColumnList = new List<string>();
                //foreach (XmlNode colNode in xmlRightColumns)
                //{
                //    string col = colNode.Attributes["Name"].Value;
                //    rightColumnList.Add(col);
                //}

                //dataCtrlBrokerMatched.DisplayedColumnList = rightColumnList;
                //dataCtrlAppMatched.DisplayedColumnList = leftColumnList;

                //List<string> leftColumnListTemp = new List<string>();
                //List<string> rightColumnListTemp = new List<string>();
                //leftColumnListTemp.AddRange(leftColumnList);
                //rightColumnListTemp.AddRange(rightColumnList);
                //leftColumnListTemp.Remove("Mismatch");
                //rightColumnListTemp.Remove("Mismatch");

                //dataCtrlBrokerOrig.DisplayedColumnList = rightColumnListTemp;
                //dataCtrlAppOrig.DisplayedColumnList = leftColumnListTemp;

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


        private void btnCompare_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataCtrlBrokerOrig.Validate() && dataCtrlAppOrig.Validate())
                {
                    //TODO:Make template key generation code generic, Here we are making template key from clientid, recontype and template name each time
                    //we can make this generic by addding key to template combobox                    
                    string clientID = dataCtrlAppOrig.GetSelectedValue(0);
                    string reconType = dataCtrlAppOrig.GetSelectedValue(1);
                    string templateName = dataCtrlAppOrig.GetSelectedValue(2);
                    string templateKey = ReconUtilities.GetTemplateKeyFromParameters(clientID, reconType, templateName);
                    List<MatchingRule> rules = ReconPrefManager.ReconPreferences.GetListOfRules(templateKey);
                    if (rules.Count == 0)
                    {
                        MessageBox.Show("Please select a Rule");
                        return;
                    }
                    else if (rules[0].ComparisonFields.Count == 0)
                    {
                        MessageBox.Show("Please select a Rule");
                        return;

                    }

                    if (chkBoxClrExpCache.Checked)
                    {
                        // it will reload the data  if the data is not freshly loaded
                        if (dataCtrlBrokerOrig.Data.Columns.Count == 0)
                            dataCtrlBrokerOrig.Reload();
                        dataCtrlAppOrig.Reload();
                        // It Clears the older data from matched grids
                        ClearMatchedDataGrids(null, null);
                    }

                    //if (cmbbxRules.Value != null)
                    //{
                    DataTable dt1 = dataCtrlAppOrig.Data.Copy();

                    //Fix for Recon report generating blank when we have trade in Nirvana side but the import file is blank from PB
                    if (dataCtrlBrokerOrig.Data.Rows.Count == 0)
                        dataCtrlBrokerOrig.Data = dt1.Clone();

                    DataTable dt2 = dataCtrlBrokerOrig.Data.Copy();

                    if (dt1.Columns.Contains("MismatchDetails"))
                    {
                        DataRow row;
                        for (int rowno = 0; rowno < dt1.Rows.Count; rowno++)                         // this is to clear Mismatch details data before recon.
                        {
                            row = dt1.Rows[rowno];
                            if (!String.IsNullOrEmpty(row["MismatchDetails"].ToString()))
                                row["MismatchDetails"] = String.Empty;
                            dt1.AcceptChanges();
                        }
                    }

                    object[] arguments = new object[4];
                    arguments[0] = templateKey;
                    arguments[1] = dt1;
                    arguments[2] = dt2;
                    arguments[3] = chkBoxClrExpCache.Checked;

                    if (!_bgReconcile.IsBusy)
                        _bgReconcile.RunWorkerAsync(arguments);
                    else
                        MessageBox.Show("Process is still running, Please Wait!", "Warning!", MessageBoxButtons.OK);
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
            // comparision logic 2



        }

        private void ChangeDisplayStatus()
        {
            //  label1.Text = "Matched/Unmatched:=" + dataCtrlAppMatched.Data.Rows.Count.ToString() + "/" + dataCtrlAppOrig.Data.Rows.Count.ToString();
            // label2.Text = "Matched/Unmatched:=" + dataCtrlBrokerMatched.Data.Rows.Count.ToString() + "/" + dataCtrlBrokerOrig.Data.Rows.Count.ToString();
        }

        private void applicationMatchedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                dataCtrlAppMatched.ExportToExcel();
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

        private void applicationUnmatchedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                dataCtrlAppOrig.ExportToExcel();
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

        private void brokerMatchedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                dataCtrlBrokerMatched.ExportToExcel();
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

        private void brokerUnMatchedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                dataCtrlBrokerOrig.ExportToExcel();
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
        private void BindRuleCombo()
        {
            //try
            //{
            //    string XmlMatchingRulePath = Application.StartupPath + @"\" + ApplicationConstants.MAPPING_FILE_DIRECTORY + @"\" + ApplicationConstants.MappingFileType.ReconRulesFile.ToString();
            //    ReconPrefManager.SetUp(XmlMatchingRulePath);
            //    cmbbxRules.DataSource = ReconPrefManager.RuleNames;
            //    cmbbxRules.DataBind();
            //    cmbbxRules.Value = ReconPrefManager.RuleNames[0];
            //    cmbbxRules.Visible = false;
            //}
            //catch (Exception ex)
            //{

            //    // Invoke our policy that is responsible for making sure no secure information
            //    // gets out of our layer.
            //    bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

            //    if (rethrow)
            //    {
            //        throw;
            //    }
            //}
        }

        private void fastCompareToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void mnuXmlAppUnMatched_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog.RestoreDirectory = true;
                openFileDialog.CheckFileExists = true;

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string FilePath = saveFileDialog1.FileName;
                    FileStream fs = new FileStream(FilePath, FileMode.Create, FileAccess.ReadWrite);
                    dataCtrlAppOrig.Data.TableName = "SecMasterTable";
                    dataCtrlAppOrig.Data.WriteXml(fs, XmlWriteMode.IgnoreSchema);

                    fs.Close();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void mnuXmlBrokerUnMatched_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog.RestoreDirectory = true;
                openFileDialog.CheckFileExists = true;

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string FilePath = saveFileDialog1.FileName;
                    FileStream fs = new FileStream(FilePath, FileMode.Create, FileAccess.ReadWrite);
                    dataCtrlBrokerOrig.Data.TableName = "SecMasterTable";
                    dataCtrlBrokerOrig.Data.WriteXml(fs, XmlWriteMode.IgnoreSchema);

                    fs.Close();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void mnuXmlAppMatched_Click(object sender, EventArgs e)
        {

        }

        private void mnuXmlBrokerMatched_Click(object sender, EventArgs e)
        {

        }

        private void btnFastCompare_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    if (cmbbxRules.Value != null)
            //    {
            //        DataTable dt1 = dataCtrlAppOrig.Data;
            //        DataTable dt2 = dataCtrlBrokerOrig.Data;
            //        dataCtrlAppMatched.Data = new DataTable();
            //        dataCtrlBrokerMatched.Data = new DataTable();
            //       // List<MatchingRule> rules = ReconPrefManager.GetListOfRules(dataCtrlAppOrig.GetSelectedValue(0));
            //        //foreach (MatchingRule rule in rules)
            //        //{
            //        //    ComparingLogic.FastCompare(dt1, dt2, dataCtrlAppMatched.Data, dataCtrlBrokerMatched.Data, rule);
            //        //}
            //        dataCtrlAppOrig.BindData();
            //        dataCtrlBrokerOrig.BindData();
            //        dataCtrlAppMatched.BindData();
            //        dataCtrlBrokerMatched.BindData();
            //        ChangeDisplayStatus();
            //    }
            //    else
            //    {
            //        MessageBox.Show("Please select a Rule");
            //    }
            //}
            //catch (Exception ex)
            //{

            //    // Invoke our policy that is responsible for making sure no secure information
            //    // gets out of our layer.
            //    bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

            //    if (rethrow)
            //    {
            //        throw;
            //    }
            //}
        }

        private void mappingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (LaunchForm != null)
                {
                    ListEventAargs args = new ListEventAargs();
                    args.listOfValues.Add(ApplicationConstants.CONST_DataMapping_UI.ToString());
                    LaunchForm(this, args);
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

        private void copyUnMappedDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string path = Application.StartupPath + @"\" + ApplicationConstants.MAPPING_FILE_DIRECTORY + @"\" + ApplicationConstants.MappingFileType.ReconMappingXml.ToString() + "//SymbolMapping.xml";
                DataTable dtSymbolMapping = new DataTable();
                DataTable dtPrimeBrokers = new DataTable();
                DataSet ds = new DataSet();
                //CHMW-2181	[Reconciliation] [Code Review] Replace dataset read xml with BufferedStream
                //ds.ReadXml(path);
                ds = XMLUtilities.ReadXmlUsingBufferedStream(path);
                dtPrimeBrokers = ds.Tables[0];
                dtSymbolMapping = ds.Tables[1];

                string primeBroker = String.Empty;
                int primeBrokerID = int.MinValue;

                // To Retrieve Prime Broker Name from Prana Original control
                //  PranaPositionViewer positionViewerControl = (PranaPositionViewer)dataCtrlAppOrig;
                //Infragistics.Win.UltraWinGrid.UltraCombo primeBrokerCombo = positionViewerControl.PrimeBrokerCombo;
                string selectedValue = dataCtrlAppOrig.GetSelectedValue(1);
                string selectedText = dataCtrlAppOrig.GetSelectedValue(2).ToUpper();
                if (selectedValue != int.MinValue.ToString())
                {
                    primeBroker = selectedText;

                    // To Retrieve Prime Broker ID from ds (or xml)
                    DataRowCollection primeBrokerListXML = ds.Tables[0].Rows;
                    bool IsBrokerFound = false;
                    for (int i = 0; i < primeBrokerListXML.Count; i++)
                    {
                        DataRow row = primeBrokerListXML[i];
                        if (row[1].ToString() == primeBroker)
                        {
                            primeBrokerID = i;
                            IsBrokerFound = true;
                            break;
                        }
                    }
                    if (!IsBrokerFound)
                    {
                        DataRow dr = dtPrimeBrokers.NewRow();
                        primeBrokerID = primeBrokerListXML.Count;
                        dr[0] = primeBrokerID;
                        dr[1] = primeBroker;
                        dtPrimeBrokers.Rows.Add(dr);
                    }

                    //Get symbol mappings for the prime broker
                    DataTable dtSymbolMappingPB = new DataTable();
                    dtSymbolMappingPB = dtSymbolMapping.Clone();
                    for (int i = 0; i < dtSymbolMapping.Rows.Count; i++)
                    {
                        DataRow row = dtSymbolMapping.Rows[i];
                        string pbIDColumnName = dtPrimeBrokers.Columns[0].Caption;
                        if (Convert.ToInt32(row[pbIDColumnName]) == primeBrokerID)
                        {
                            dtSymbolMappingPB.ImportRow(dtSymbolMapping.Rows[i]);
                        }
                    }

                    // Gets a list of unmapped names from unmapped PB contol
                    List<string> unmatchedPBNames = new List<string>();
                    foreach (DataRow drUnmatched in dataCtrlBrokerOrig.Data.Rows)
                    {
                        if (drUnmatched.Table.Columns.Contains("Description"))
                        {
                            string unmappedPBCompanyName = drUnmatched["Description"].ToString().Trim();
                            bool matched = false;
                            foreach (DataRow drMapping in dtSymbolMappingPB.Rows)
                            {
                                if (drMapping.Table.Columns.Contains("PBCompanyName"))
                                {
                                    string mappedPBCompanyName = drMapping["PBCompanyName"].ToString().Trim();
                                    if (unmappedPBCompanyName.Equals(mappedPBCompanyName))
                                    {
                                        matched = true;
                                    }
                                }
                            }
                            if (!matched)
                            {
                                if (!unmatchedPBNames.Contains(unmappedPBCompanyName))
                                    unmatchedPBNames.Add(unmappedPBCompanyName);
                            }
                        }
                    }

                    // Adds unmapped names to the ds(or xml)
                    foreach (string name in unmatchedPBNames)
                    {
                        DataRow dr = dtSymbolMapping.NewRow();
                        dr["PBCompanyName"] = name;
                        dr[dtPrimeBrokers.Columns[0].Caption] = primeBrokerID;
                        dtSymbolMapping.Rows.Add(dr);
                    }
                    ds.WriteXml(path);
                }
                else
                {
                    MessageBox.Show("Select broker !", "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        #region IPluggableTools Members

        public Form Reference()
        {
            return this;
        }

        public event EventHandler PluggableToolsClosed;

        ISecurityMasterServices _secMaster = null;

        public ISecurityMasterServices SecurityMaster
        {
            set
            {
                _secMaster = value;
                NewUtilities.SecurityMaster = _secMaster;
                //CHMW-3328	Recon not working properly in first attempt
                Prana.ReconciliationNew.SecMasterHelper.SecurityMaster = _secMaster;
            }
        }
        public void SetUP()
        {
            try
            {
                _bgReconcile = new BackgroundWorker();
                _bgReconcile.DoWork += new DoWorkEventHandler(_bgReconcile_DoWork);
                _bgReconcile.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_bgReconcile_RunWorkerCompleted);

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

        public IPostTradeServices PostTradeServices
        {
            set {; }
        }

        public IPricingAnalysis PricingAnalysis
        {
            set {; }
        }

        #endregion


        void _bgReconcile_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                object[] results = e.Result as object[];
                DataTable dtExceptions = results[0] as DataTable;
                DataTable dt1 = results[1] as DataTable;
                DataTable dt2 = results[2] as DataTable;
                DataTable dtAppMatchedData = results[3] as DataTable;
                DataTable dtBrokerMatchedData = results[4] as DataTable;
                //TODO: Make template key generation code generic, Here we are making template key from clientid, recontype and template name each time
                //we can make this generic by addding key to template combobox
                string clientID = dataCtrlAppOrig.GetSelectedValue(0);
                string reconType = dataCtrlAppOrig.GetSelectedValue(1);
                string templateName = dataCtrlAppOrig.GetSelectedValue(2);
                // List<MatchingRule> rules = ReconPrefManager.ReconPreferences.GetListOfRules(templateName);
                string templateKey = ReconUtilities.GetTemplateKeyFromParameters(clientID, reconType, templateName);
                List<string> listNirvanaGridColumns = ReconPrefManager.ReconPreferences.GetNirvanaGridDisplayColumnNames(templateKey);
                List<string> listPbGridColumns = ReconPrefManager.ReconPreferences.GetPBGridDisplayColumnNames(templateKey);
                // List<MasterColumn> listMasterColumns = ReconPrefManager.ReconPreferences.GetNirvanaMasterColumns(templateName);

                dataCtrlAppOrig.Data = dt1;
                dataCtrlBrokerOrig.Data = dt2;
                dataCtrlAppMatched.Data = dtAppMatchedData;
                dataCtrlBrokerMatched.Data = dtBrokerMatchedData;

                dataCtrlAppOrig.BindData();
                dataCtrlBrokerOrig.BindData();

                dataCtrlAppMatched.DisplayedColumnList = listNirvanaGridColumns;
                dataCtrlBrokerMatched.DisplayedColumnList = listPbGridColumns;

                dataCtrlAppMatched.BindData();
                dataCtrlBrokerMatched.BindData();
                ChangeDisplayStatus();
                //btnCompare.Enabled = false;

                #region Generate Exception Report xml
                string exceptionFileName = string.Empty;
                //ReconTemplate reconTemplate = new ReconTemplate();
                //reconTemplate = ReconPrefManager.ReconPreferences.GetTemplates(templateKey);
                ReconParameters reconParameters = new ReconParameters();
                reconParameters.TemplateKey = templateKey;
                reconParameters.DTFromDate = DateTime.Parse(dataCtrlAppOrig.GetSelectedValue(3));
                reconParameters.DTToDate = DateTime.Parse(dataCtrlAppOrig.GetSelectedValue(4));

                //http://jira.nirvanasolutions.com:8080/browse/CHMW-1383
                //if (string.IsNullOrEmpty(reconTemplate.ExceptionReportSavePath))
                //{
                exceptionFileName = ReconUtilities.GetReconFilePath(ReconConstants.ReconDataDirectoryPath, reconParameters);
                //}
                //else
                //{
                //    exceptionFileName = ReconUtilities.GetReconFilePath(reconTemplate.ExceptionReportSavePath, reconParameters);
                //}
                //This path will be used to save data in dashboard.
                // string relativeReconFilePath = exceptionFileName.Substring(Application.StartupPath.Length);
                ReconUtilities.CreateDirectoryIfNotExists(exceptionFileName);

                Logger.LoggerWrite("Exception report file path: " + exceptionFileName);
                //generate the exception report
                Logger.LoggerWrite("Started generating exception report");
                //false is passed because files are not to be generated on runrecon_click   
                Logger.LoggerWrite("Done generating exception report");
                DataSet dsXml = new DataSet();
                dsXml.Tables.Add(dtExceptions);
                //XMLUtilities.WriteXMLWithSchema(dsXml, exceptionFileName + ".xml");
                dsXml.WriteXml(exceptionFileName + ".xml", XmlWriteMode.WriteSchema);
                //add info to task statistics  
                #endregion

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
                DisableEnableButton(true);
            }
        }

        private void DisableEnableButton(bool flag)
        {
            try
            {
                if (flag)
                {
                    btnCompare.Text = "Run Recon";
                    btnCompare.Enabled = true;
                    btnCompare.Appearance.BackColor = Color.FromArgb(192, 192, 255);
                    btnCompare.Appearance.BackColor2 = Color.FromArgb(192, 192, 255);
                }
                else
                {
                    btnCompare.Text = "Reconciling Data...";
                    btnCompare.Enabled = false;
                    btnCompare.Appearance.BackColor = Color.Red;
                    btnCompare.Appearance.BackColor2 = Color.Red;
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

        void _bgReconcile_DoWork(object sender, DoWorkEventArgs e)
        {

            try
            {
                object[] arguments = e.Argument as object[];

                DataTable dtExceptions = new DataTable();
                string templateKey = arguments[0].ToString();
                DataTable dt1 = arguments[1] as DataTable;
                DataTable dt2 = arguments[2] as DataTable;

                bool isClearExpCache = bool.Parse(arguments[3].ToString());

                DataTable dtAppMatchedData = new DataTable();
                DataTable dtBrokerMatchedData = new DataTable();

                ReconTemplate template = ReconPrefManager.ReconPreferences.GetTemplates(templateKey);
                ReconPrefManager.ReconPreferences.SetDefaultReportGeneratedProperty();
                List<MatchingRule> rules = ReconPrefManager.ReconPreferences.GetListOfRules(templateKey);
                //List<ColumnInfo> listMasterColumns = ReconPrefManager.ReconPreferences.GetNirvanaMasterColumns(templateKey);

                // http://jira.nirvanasolutions.com:8080/browse/PRANA-9948
                if (!string.IsNullOrWhiteSpace(template.SortingColumnOrder) && template.RulesList.Count > 0)
                {
                    dt1 = ReconManager.sortDataTable(dt1, template.SortingColumnOrder, template.RulesList);
                    dt2 = ReconManager.sortDataTable(dt2, template.SortingColumnOrder, template.RulesList);
                }

                foreach (MatchingRule rule in rules)
                {
                    //This method is called in DataReconciler.Reconcile() method
                    //DataReconciler.GenerateExceptionsDataTableSchema(template.SelectedColumnList);

                    // dtExceptions = ReconUtilities.GetExceptionsDataTableSchema(rule.ComparisonFields, listMasterColumns);
                    template.isReconReportToBeGenerated = true;
                    string errMsg = string.Empty;
                    HashSet<string> hSetCommonColumn = GetCommonRule(template.AvailableColumnList, template.SelectedColumnList);
                    dtExceptions = DataReconciler.Reconcile(dt1, dt2, out dtAppMatchedData, out dtBrokerMatchedData, rule, template.IsIncludeMatchedItems, template.IsIncludeToleranceMacthedItems, template.SelectedColumnList, isClearExpCache, template.ReconType, ref errMsg, template.IsReconTemplateGroupping(), hSetCommonColumn);
                    if (!string.IsNullOrEmpty(errMsg))
                    {
                        MessageBox.Show(errMsg, ReconConstants.MismatchReason_MatchingRuleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                object[] results = new object[5];
                results[0] = dtExceptions;
                results[1] = dt1;
                results[2] = dt2;
                results[3] = dtAppMatchedData;
                results[4] = dtBrokerMatchedData;
                e.Result = results;
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                e.Cancel = true;
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Create the hset for name of selected columns
        /// </summary>
        /// <param name="AvailableColumnList"></param>
        /// <param name="SelectedColumnList"></param>
        /// <returns></returns>
        private HashSet<string> GetCommonRule(List<ColumnInfo> AvailableColumnList, List<ColumnInfo> SelectedColumnList)
        {
            HashSet<string> hSet = new HashSet<string>();
            try
            {
                AvailableColumnList.ForEach(x =>
                {
                    if (x.GroupType.ToString().Equals("Common"))
                    {
                        hSet.Add(x.ColumnName);
                    }
                });

                SelectedColumnList.ForEach(x =>
                {
                    if (x.GroupType.ToString().Equals("Common"))
                    {
                        hSet.Add(x.ColumnName);
                    }
                });
                hSet.Remove("MismatchType");
                hSet.Remove("Matched");
                hSet.Remove("MismatchDetails");

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return hSet;
        }

        /// <summary>
        ///modified by: sachin mishra 02 Feb 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        private void GenerateExceptionReport()
        {

            try
            {
                //TODO: Make template key generation code generic, Here we are making template key from clientid, recontype and template name each time
                //we can make this generic by addding key to template combobox
                string clientID = dataCtrlAppOrig.GetSelectedValue(0);
                string reconType = dataCtrlAppOrig.GetSelectedValue(1);
                string templateName = dataCtrlAppOrig.GetSelectedValue(2);
                string templateKey = ReconUtilities.GetTemplateKeyFromParameters(clientID, reconType, templateName);

                if (!string.IsNullOrEmpty(templateName))
                {
                    ReconTemplate template = ReconPrefManager.ReconPreferences.GetTemplates(templateKey);
                    //List<MatchingRule> rules = ReconPrefManager.ReconPreferences.GetListOfRules(templateKey);
                    //List<ColumnInfo> listMasterColumns = ReconPrefManager.ReconPreferences.GetNirvanaMasterColumns(templateKey);

                    ReconParameters reconParameters = new ReconParameters();
                    reconParameters.TemplateKey = templateKey;
                    reconParameters.DTFromDate = DateTime.Parse(dataCtrlAppOrig.GetSelectedValue(3));
                    reconParameters.DTToDate = DateTime.Parse(dataCtrlAppOrig.GetSelectedValue(4));
                    string exceptionFileName = string.Empty;
                    string exceptionXMLFileName = string.Empty;
                    exceptionXMLFileName = ReconUtilities.GetReconFilePath(ReconConstants.ReconDataDirectoryPath, reconParameters);

                    if (!string.IsNullOrEmpty(template.ExceptionReportSavePath))
                    {
                        exceptionFileName = ReconUtilities.GetReconFilePath(template.ExceptionReportSavePath, reconParameters);
                    }
                    else
                    {
                        exceptionFileName = exceptionXMLFileName;
                    }

                    //generate the exception report
                    DataTable dtExceptions = new DataTable();
                    if (File.Exists(exceptionXMLFileName + ".xml"))
                    {
                        DataSet ds = new DataSet();
                        //CHMW-2181	[Reconciliation] [Code Review] Replace dataset read xml with BufferedStream
                        //ds.ReadXml(exceptionFileName + ".xml");
                        ds = XMLUtilities.ReadXmlUsingBufferedStream(exceptionXMLFileName + ".xml");
                        if (ds.Tables.Count > 0)
                            dtExceptions = ds.Tables[0];
                    }
                    //http://jira.nirvanasolutions.com:8080/browse/CI-465
                    // Remove the columns from the report that are not to be visible in prana mode
                    List<string> str = new List<string>();
                    foreach (DataColumn col in dtExceptions.Columns)
                    {
                        if (col.ColumnName.StartsWith("ToleranceValue") || col.ColumnName.StartsWith("ToleranceType") || col.ColumnName.StartsWith("OriginalValue") || col.ColumnName.StartsWith("ReconStatus"))
                        {
                            str.Add(col.ColumnName);
                        }
                        else if (!(template.SelectedColumnList.Count(cus => col.ColumnName.Contains(cus.ColumnName)) > 0))
                        {
                            str.Add(col.ColumnName);
                        }
                    }
                    foreach (string col in str)
                    {
                        if (dtExceptions.Columns.Contains(col))
                        {
                            dtExceptions.Columns.Remove(col);
                        }
                    }
                    //if (dtExceptions.Columns.Contains(ReconConstants.COLUMN_TaxLotID))
                    //{
                    //dtExceptions.Columns.Remove(ReconConstants.COLUMN_TaxLotID);
                    //}
                    DataReconciler.GenerateExceptionsReport(dtExceptions, exceptionFileName, template.ExpReportFormat, template.SelectedColumnList, template.ListSortByColumns, template.ListGroupByColumns, template.isReconReportToBeGenerated);

                    Type officeType = Type.GetTypeFromProgID("Excel.Application");
                    if (!string.IsNullOrEmpty(template.ExceptionReportPassword))
                    {

                        if (officeType == null)
                        {
                            MessageBox.Show("Microsoft Office Excel is not installed on the machine,password protection cannot be applied on exception Report.", "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        }
                        else
                        {
                            ReconUtilities.AddPassword(exceptionFileName, template.ExceptionReportPassword);
                        }
                    }
                    //Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
                    //excelApp.Workbooks.Open(exceptionFileName, false, false, false, false, false, false, false, false, false, false, false, false, false, false);
                    //excelApp.Visible = true;
                    String fileNameToBeDisplayed = templateName + '_' + reconParameters.FromDate + '_' + reconParameters.ToDate + "." + template.ExpReportFormat;
                    String filePath = exceptionFileName + "." + template.ExpReportFormat;
                    if (File.Exists(filePath) && template.isReconReportToBeGenerated == true)
                    {
                        FileOpenDialogue frmTPOpenDialogue = new FileOpenDialogue(fileNameToBeDisplayed, filePath);
                        //DialogResult result = MessageBox.Show("Exception Report Successfully Generated", "Reconciliation", MessageBoxButtons.YesNoCancel);
                        frmTPOpenDialogue.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Invalid Process Followed, Firstly Run Recon Process!!!");
                    }

                    //delete temp files 
                    string tempPath = ReconConstants.ReconDataDirectoryPath + @"\" + templateName + @"\" + reconParameters.FromDate + "\\xmls\\Transformation\\Temp";
                    // string tempPath = Application.StartupPath + "\\xmls\\Transformation\\Temp";
                    //DirectoryInfo dir = new DirectoryInfo(tempPath);
                    if (Directory.Exists(tempPath))
                    {
                        string inputXML = tempPath + "\\InputXML.xml";
                        string outputXML = tempPath + "\\OutPutXML.xml";

                        FileInfo infoInputXml = new FileInfo(inputXML);
                        if (infoInputXml.Exists)
                        {
                            infoInputXml.Delete();
                        }
                        FileInfo infoOutputXml = new FileInfo(outputXML);
                        if (infoOutputXml.Exists)
                        {
                            infoOutputXml.Delete();
                        }
                    }



                }
                ReconPrefManager.ReconPreferences.SetDefaultReportGeneratedProperty();
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

            //Infragistics.Documents.Excel.Application excelApp = new Excel.Application();
            //ReconUtilities.GenerateExceptionsReport(dtExceptions, exceptionFileName, listMasterColumns, rules[0].ComparisonFields);
        }

        private void SetupSnapshotControl()
        {
            try
            {
                System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataCompareForm));
                this.toolStripScreenshot = SnapShotManager.GetInstance().toolStripButton;
                this.toolStrip1.Items.Add(this.toolStripScreenshot);
                // 
                // toolStripScreenshot
                // 
                this.toolStripScreenshot.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
                this.toolStripScreenshot.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
                this.toolStripScreenshot.ForeColor = System.Drawing.SystemColors.ButtonFace;
                this.toolStripScreenshot.ImageTransparentColor = System.Drawing.Color.Magenta;
                this.toolStripScreenshot.Name = "toolStripScreenshot";
                this.toolStripScreenshot.Size = new System.Drawing.Size(147, 22);
                this.toolStripScreenshot.Click += new System.EventHandler(this.toolStripScreenshot_Click);
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

        private void DataCompareForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (PluggableToolsClosed != null)
                {
                    PluggableToolsClosed(this, EventArgs.Empty);
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

        private void ClearMatchedDataGrids(object sender, EventArgs e)
        {
            try
            {
                dataCtrlAppMatched.Data = new DataTable();
                dataCtrlBrokerMatched.Data = new DataTable();

                dataCtrlAppMatched.BindData();
                dataCtrlBrokerMatched.BindData();
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

        private void mnuPreferences_Click(object sender, EventArgs e)
        {
            try
            {
                ReconPrefForm reconPrefForm = new ReconPrefForm();
                reconPrefForm.StartPosition = FormStartPosition.CenterParent;
                reconPrefForm.ShowDialog(this);
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


        //public PranaPositionViewer DataCtrlAppOrig
        //{
        //    get { return (PranaPositionViewer)dataCtrlAppOrig; }
        //}




        #region ILaunchForm Members

        public event EventHandler LaunchForm;

        #endregion

        private void xMLBrokerMatched_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog.RestoreDirectory = true;
                openFileDialog.CheckFileExists = true;

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string FilePath = saveFileDialog1.FileName;
                    FileStream fs = new FileStream(FilePath, FileMode.Create, FileAccess.ReadWrite);
                    dataCtrlBrokerMatched.Data.TableName = "SecMasterTable";
                    dataCtrlBrokerMatched.Data.WriteXml(fs, XmlWriteMode.IgnoreSchema);

                    fs.Close();
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

        private void xMLAppMatchedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog.RestoreDirectory = true;
                openFileDialog.CheckFileExists = true;

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string FilePath = saveFileDialog1.FileName;
                    FileStream fs = new FileStream(FilePath, FileMode.Create, FileAccess.ReadWrite);
                    dataCtrlAppMatched.Data.TableName = "SecMasterTable";
                    dataCtrlAppMatched.Data.WriteXml(fs, XmlWriteMode.IgnoreSchema);

                    fs.Close();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void xMLBrokerUnmatchedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog.RestoreDirectory = true;
                openFileDialog.CheckFileExists = true;

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string FilePath = saveFileDialog1.FileName;
                    FileStream fs = new FileStream(FilePath, FileMode.Create, FileAccess.ReadWrite);
                    dataCtrlBrokerOrig.Data.TableName = "SecMasterTable";
                    dataCtrlBrokerOrig.Data.WriteXml(fs, XmlWriteMode.IgnoreSchema);

                    fs.Close();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void xMLAppUnMatchedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog.RestoreDirectory = true;
                openFileDialog.CheckFileExists = true;

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string FilePath = saveFileDialog1.FileName;
                    FileStream fs = new FileStream(FilePath, FileMode.Create, FileAccess.ReadWrite);
                    dataCtrlAppOrig.Data.TableName = "SecMasterTable";
                    dataCtrlAppOrig.Data.WriteXml(fs, XmlWriteMode.IgnoreSchema);

                    fs.Close();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }


        private void label2_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// modified by: sachin mishra,02 Feb 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                GenerateExceptionReport();
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
        /// <summary>
        /// modified by: sachin mishra,02 Feb 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripScreenshot_Click(object sender, EventArgs e)
        {
            try
            {
                SnapShotManager.GetInstance().TakeSnapshot(this);
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
        private void chkBoxClrExpCache_CheckedChanged(object sender, EventArgs e)
        {
            // ReconPrefManager.ReconPreferences.IsClearExpCache = chkBoxClrExpCache.Checked;

        }

        private void DataCompareForm_Load(object sender, EventArgs e)
        {
            try
            {
                SetButtonsColor();
                CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_RECONCILATION);
                if (CustomThemeHelper.ApplyTheme)
                {
                    this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                    this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, this.Text, CustomThemeHelper.UsedFont);
                    menuStrip1.BackColor = System.Drawing.Color.FromArgb(88, 88, 90);
                    menuStrip1.ForeColor = System.Drawing.Color.WhiteSmoke;
                    toolStrip1.BackColor = System.Drawing.Color.FromArgb(88, 88, 90);
                    toolStrip1.ForeColor = System.Drawing.Color.WhiteSmoke;
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
                btnCompare.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
                btnCompare.ForeColor = System.Drawing.Color.White;
                btnCompare.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnCompare.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnCompare.UseAppStyling = false;
                btnCompare.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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
    }
}