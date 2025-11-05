using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinTree;
using Prana.BusinessObjects.SMObjects;
using Prana.Global.Utilities;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using Prana.Utilities.XMLUtilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Prana.Tools
{
    public partial class ctrlSMBatchStatusReport : UserControl
    {
        private readonly string CONST_SMBatchDirectoryPath = Application.StartupPath + @"\DashBoardData\SMBatch";
        /// <summary>
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        public ctrlSMBatchStatusReport()
        {
            try
            {
                InitializeComponent();
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
        /// Initialize the batch data
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        public void InitializeData()
        {
            try
            {
                DateTime dtSelectedDate = (DateTime)udtSMBatchDate.Value;
                BindSMBatchTree(dtSelectedDate);

                //DataTable dtSMBatchData = SMBatchManager.GetSMBatchData();
                //ConcurrentDictionary<int, string> dicSMBatch = new ConcurrentDictionary<int, string>();

                //if (dtSMBatchData != null)
                //{
                //    foreach(DataRow dr in dtSMBatchData.Rows)
                //    {
                //        dicSMBatch.TryAdd(Convert.ToInt32(dr["SMBatchID"]), dr["SystemLevelName"].ToString());
                //    }
                //}
                //this.treeViewSMBatch.DataSource = dicSMBatch;

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
        /// Bind SM Batch tree'
        ///modified by: sachin mishra 28 jan 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        private void BindSMBatchTree(DateTime? dtSelectedDate, bool isShowMsg = false)
        {
            try
            {
                //To clear the tree of any node before binding.
                treeViewSMBatch.Nodes.Clear();
                UltraTreeNode treeRootNode = new UltraTreeNode("SMBatch", "SMBatch");
                treeRootNode.Override.NodeAppearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;
                treeViewSMBatch.Nodes.Add(treeRootNode);

                BindingList<SMBatch> smBatchList = new BindingList<SMBatch>();
                smBatchList = SMBatchManager.GetSMBatchData();
                List<string> lstBatch = new List<string>();
                ConcurrentDictionary<string, List<string>> dicSMBatch = new ConcurrentDictionary<string, List<string>>();

                if (smBatchList.Count > 0)
                {
                    foreach (SMBatch smBatch in smBatchList)
                    {
                        lstBatch.Add(smBatch.SystemLevelName);
                    }
                }

                dicSMBatch = GetBatchwiseResponseXML(lstBatch, dtSelectedDate, isShowMsg);

                if (dicSMBatch.Count > 0)
                {
                    foreach (KeyValuePair<string, List<string>> kvp in dicSMBatch)
                    {
                        UltraTreeNode treeNodeSMBatch = new UltraTreeNode(kvp.Key);
                        treeViewSMBatch.Nodes.Add(treeNodeSMBatch);

                        foreach (string batchFileName in kvp.Value)
                        {
                            //add child node
                            UltraTreeNode treeNode = new UltraTreeNode(batchFileName);

                            treeViewSMBatch.Nodes[kvp.Key].Nodes.Add(treeNode);
                        }
                    }

                }
                treeViewSMBatch.ExpandAll();
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
        /// returns dictionary of datewise response files of each sm batch
        ///modified by: sachin mishra 28 jan 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        /// <param name="lstBatch"></param>
        /// <returns></returns>
        private ConcurrentDictionary<string, List<string>> GetBatchwiseResponseXML(List<string> lstBatch, DateTime? dtSelectedDate, bool isShowMessage)
        {
            ConcurrentDictionary<string, List<string>> dicBatchwiseResponseXML = new ConcurrentDictionary<string, List<string>>();
            bool isXmlFilesExist = false;
            try
            {
                if (lstBatch != null)
                {
                    if (Directory.Exists(CONST_SMBatchDirectoryPath))
                    {
                        var directory = new DirectoryInfo(CONST_SMBatchDirectoryPath);
                        //DirectoryInfo di = new DirectoryInfo(CONST_SMBatchDirectoryPath);
                        //FileInfo[] files = di.GetFiles("*.xml");

                        var files = Directory.GetFiles(CONST_SMBatchDirectoryPath, "*.xml", SearchOption.AllDirectories)
                                    .Where(s => s.Contains(dtSelectedDate != null ? dtSelectedDate.Value.ToString("dd-MM-yyyy") : string.Empty));

                        if (directory.GetFiles().Length > 0)
                        {
                            foreach (string batch in lstBatch)
                            {
                                List<string> xmlFilesForBatch = new List<string>();

                                foreach (string xmlFileName in files)
                                {
                                    if (xmlFileName.Contains(EscapedDelimiter.CombineStrings('_', '^', batch, dtSelectedDate.Value.ToString("dd-MM-yyyy"))) && (EscapedDelimiter.SplitDelimitedString(Path.GetFileName(xmlFileName), '_', '^')[0].Equals(batch)))
                                    {
                                        if (!xmlFilesForBatch.Contains(EscapedDelimiter.SplitDelimitedString(Path.GetFileName(xmlFileName), '.', '^')[0].ToString()))
                                        {
                                            xmlFilesForBatch.Add(EscapedDelimiter.SplitDelimitedString(Path.GetFileName(xmlFileName), '.', '^')[0].ToString());
                                            isXmlFilesExist = true;
                                        }
                                    }
                                }
                                dicBatchwiseResponseXML.TryAdd(batch, xmlFilesForBatch);
                            }
                        }
                    }
                }

                if (!isXmlFilesExist && isShowMessage)
                    MessageBox.Show("No batch response file exists for selected date", "SMBatch Status Report Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            return dicBatchwiseResponseXML;
        }

        /// <summary>
        /// initialize the layout of the grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdSMBatch_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                grdSMBatch.DisplayLayout.UseFixedHeaders = true;

                if (grdSMBatch.DataSource != null)
                {
                    UltraGridBand band = grdSMBatch.DisplayLayout.Bands[0];

                    foreach (UltraGridColumn column in band.Columns)
                    {
                        //following line auto adjust width of columns of ultragrid accocrding to text size of header.
                        column.PerformAutoResize(PerformAutoSizeType.AllRowsInBand, AutoResizeColumnWidthOptions.All);
                        column.CellActivation = Activation.NoEdit;
                    }

                    if (band.Columns.Exists("Symbol"))
                    {
                        band.Columns["Symbol"].Header.VisiblePosition = 0;
                    }
                    if (band.Columns.Exists("Date"))
                    {
                        band.Columns["Date"].Header.VisiblePosition = 1;
                    }
                    if (band.Columns.Exists("DataSource"))
                    {
                        band.Columns["DataSource"].Header.VisiblePosition = 2;
                    }
                    if (band.Columns.Exists("SecondarySource"))
                    {
                        band.Columns["SecondarySource"].Header.VisiblePosition = 3;
                    }
                    if (band.Columns.Exists("SymbolPK"))
                    {
                        band.Columns["SymbolPK"].Hidden = true;
                    }
                    if (band.Columns.Exists("Symbology"))
                    {
                        band.Columns["Symbology"].Hidden = true;
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
        }

        /// <summary>
        /// Get all sm batch response files of specific date
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetData_Click(object sender, EventArgs e)
        {
            try
            {
                grdSMBatch.DataSource = null;
                grdSMBatch.DataBind();
                DateTime dtSelectedDate = (DateTime)udtSMBatchDate.Value;
                BindSMBatchTree(dtSelectedDate, true);
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

        private void ctrlSMBatchStatusReport_Load(object sender, EventArgs e)
        {
            try
            {
                if (!(DesignMode || CustomThemeHelper.IsDesignMode()))
                {
                    InitializeData();
                    if (!string.IsNullOrEmpty(CustomThemeHelper.WHITELABELTHEME) && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
                    {
                        SetButtonsColor();
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
        }

        private void SetButtonsColor()
        {
            try
            {
                btnGetData.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnGetData.ForeColor = System.Drawing.Color.White;
                btnGetData.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnGetData.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnGetData.UseAppStyling = false;
                btnGetData.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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

        private void treeViewSMBatch_AfterSelect(object sender, SelectEventArgs e)
        {
            try
            {
                DataSet dsBatch = null;
                if (e.NewSelections.Count > 0)
                {
                    if (treeViewSMBatch.SelectedNodes[0].Parent != null)
                    {
                        string selectedBatch = treeViewSMBatch.SelectedNodes[0].Text;
                        dsBatch = LoadDataForSelectedBatchFile(selectedBatch);
                    }
                    if (dsBatch != null)
                    {
                        grdSMBatch.DataSource = dsBatch;
                        grdSMBatch.DataBind();
                    }
                    else
                    {
                        grdSMBatch.DataSource = null;
                        grdSMBatch.DataBind();
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

        /// <summary>
        /// Get data from xml file for selected batch
        /// </summary>
        /// <param name="batchFile"></param>
        private DataSet LoadDataForSelectedBatchFile(string batchFile)
        {
            DataSet dsBatchResponseData = new DataSet();

            try
            {
                if (Directory.Exists(CONST_SMBatchDirectoryPath))
                {
                    var directory = new DirectoryInfo(CONST_SMBatchDirectoryPath);
                    DirectoryInfo di = new DirectoryInfo(CONST_SMBatchDirectoryPath);
                    FileInfo[] files = di.GetFiles("*.xml");

                    if (directory.GetFiles().Length > 0)
                    {
                        foreach (FileInfo xmlFileName in files)
                        {
                            string xmlFile = EscapedDelimiter.SplitDelimitedString(xmlFileName.ToString(), '.', '^')[0].ToString();
                            if (xmlFile.Contains(batchFile))
                            {
                                string SMBatchFilePath = CONST_SMBatchDirectoryPath + @"\" + xmlFileName;
                                if (File.Exists(SMBatchFilePath))
                                {
                                    //CHMW-2181	[Reconciliation] [Code Review] Replace dataset read xml with BufferedStream
                                    //dsBatchResponseData.ReadXml(SMBatchFilePath);
                                    dsBatchResponseData = XMLUtilities.ReadXmlUsingBufferedStream(SMBatchFilePath);
                                    return dsBatchResponseData;
                                }
                            }
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
            return dsBatchResponseData;
        }
    }
}
