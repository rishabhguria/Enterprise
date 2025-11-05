using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinTree;
using Prana.BusinessObjects.AppConstants;
using Prana.Import.BAL;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using Prana.Utilities.XMLUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Prana.Import.Controls
{
    public partial class ctrlImportTag : UserControl
    {
        public ctrlImportTag()
        {
            InitializeComponent();
        }
        Dictionary<string, List<int>> _dictImportTagsRowIndex = new Dictionary<string, List<int>>();
        int _headerRowIndex = int.MinValue;
        ImportType? _importType;
        /// <summary>
        /// Set Tree View and add nodes in form
        /// </summary>
        /// <param name="fileName"></param>
        public void SetTreeView(string fileName, string rootNode, string ColumnName, Dictionary<string, string> _dictMapping, bool isFirstRowToBeUsedAsHeader, string fileType)
        {
            try
            {
                string rootNodeKey = rootNode.Replace(" ", string.Empty);
                _dictImportTagsRowIndex.Clear();
                treeViewImportTags.Nodes.Clear();

                if (isFirstRowToBeUsedAsHeader)
                {
                    //Do not prompt for node selection for first row as header
                    _headerRowIndex = -1;
                }

                DataSet ds = new DataSet();
                ds = XMLUtilities.ReadXmlUsingBufferedStream(fileName);
                if (fileType == null)
                {
                    _importType = null;
                }
                else
                {
                    _importType = (ImportType)Enum.Parse(typeof(ImportType), fileType, true);
                }

                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    grdReport.DataSource = null;
                    grdReport.DataSource = dt.Copy();
                    grdReport.DisplayLayout.Override.RowSelectorHeaderStyle = RowSelectorHeaderStyle.ColumnChooserButton;
                    grdReport.DisplayLayout.Bands[0].Override.AllowUpdate = DefaultableBoolean.False;
                    if (dt != null && dt.Columns != null && dt.Columns.Contains(ColumnName))
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            #region add Import Tag nodes
                            if (dt.Rows[i][ColumnName] != DBNull.Value && dt.Rows[i][ColumnName] != null && !string.IsNullOrEmpty(dt.Rows[i][ColumnName].ToString().Trim()))
                            {
                                string acronym = dt.Rows[i][ColumnName].ToString();

                                if (_dictImportTagsRowIndex.Keys.Contains(acronym))
                                {
                                    _dictImportTagsRowIndex[acronym].Add(i);
                                }
                                else
                                {
                                    UltraTreeNode node = new UltraTreeNode();
                                    if (_dictMapping.Keys.Contains(acronym))
                                    {
                                        node.Text = _dictMapping[acronym];
                                        node.Key = acronym;
                                    }
                                    else
                                    {
                                        node.Text = "Others";
                                        node.Key = "Others";
                                    }
                                    if (!_dictImportTagsRowIndex.ContainsKey(node.Key))
                                    {
                                        _dictImportTagsRowIndex.Add(node.Key, new List<int>() { i });
                                    }
                                    else
                                    {
                                        _dictImportTagsRowIndex[node.Key].Add(i);
                                    }


                                    #region add root tree node
                                    if (!treeViewImportTags.Nodes.Exists(rootNodeKey))
                                    {
                                        treeViewImportTags.Nodes.Clear();
                                        UltraTreeNode rootnode = new UltraTreeNode();
                                        rootnode.Key = rootNodeKey;
                                        rootnode.Text = rootNode;
                                        treeViewImportTags.Nodes.Add(rootnode);
                                    }
                                    #endregion
                                    if (!treeViewImportTags.Nodes[rootNodeKey].Nodes.Exists(node.Key))
                                    {
                                        treeViewImportTags.Nodes[rootNodeKey].Nodes.Add(node);
                                    }
                                }
                            }
                            #endregion

                            //#region add Import Status nodes
                            //if (dt.Rows[i]["ImportStatus"] != DBNull.Value && dt.Rows[i]["ImportStatus"] != null && !string.IsNullOrEmpty(dt.Rows[i]["ImportStatus"].ToString().Trim()))
                            //{
                            //    string importStatus = dt.Rows[i]["ImportStatus"].ToString();
                            //    if (string.IsNullOrEmpty(importStatus))
                            //    {
                            //        importStatus = "NonImported";
                            //    }
                            //    if (_dictImportTagsRowIndex.Keys.Contains(importStatus))
                            //    {
                            //        _dictImportTagsRowIndex[importStatus].Add(i);
                            //    }
                            //    else
                            //    {
                            //        _dictImportTagsRowIndex.Add(importStatus, new List<int>() { i });
                            //        UltraTreeNode node = new UltraTreeNode();
                            //        node.Key = importStatus;
                            //        node.Text = importStatus;
                            //        #region add root tree node
                            //        if (!treeViewImportTags.Nodes.Exists("ImportStatus"))
                            //        {
                            //            UltraTreeNode nodeImportStatus = new UltraTreeNode();
                            //            nodeImportStatus.Key = "ImportStatus";
                            //            nodeImportStatus.Text = "Import Status";
                            //            treeViewImportTags.Nodes.Add(nodeImportStatus);
                            //        }
                            //        #endregion
                            //        treeViewImportTags.Nodes["ImportStatus"].Nodes.Add(node);
                            //    }
                            //}
                            //#endregion
                        }
                    }
                }

                treeViewImportTags.ExpandAll();
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
        /// refresh rows to reintialize them
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeViewImportTags_AfterSelect(object sender, SelectEventArgs e)
        {
            try
            {
                if (_headerRowIndex == int.MinValue)
                {
                    if (DialogResult.Yes == MessageBox.Show(this, "Do you want to use first row as header", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    {
                        _headerRowIndex = 0;

                    }
                    else
                    {
                        _headerRowIndex = -1;
                    }
                }
                if (treeViewImportTags.SelectedNodes != null && treeViewImportTags.SelectedNodes.Count > 0)
                {
                    treeViewImportTags.ActiveNode = treeViewImportTags.SelectedNodes[0];
                }

                grdReport.Rows.Refresh(RefreshRow.FireInitializeRow);
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
        /// Hides rows if not in selected tag
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdReport_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
        {
            if (e.Row.ListIndex != _headerRowIndex)
            {
                if (treeViewImportTags.ActiveNode != null)
                {
                    if (!treeViewImportTags.ActiveNode.IsRootLevelNode && _dictImportTagsRowIndex.ContainsKey(treeViewImportTags.ActiveNode.Key) && !_dictImportTagsRowIndex[treeViewImportTags.ActiveNode.Key].Contains(e.Row.ListIndex))
                    {
                        e.Row.Hidden = true;
                    }
                    else
                    {
                        e.Row.Hidden = false;
                    }
                }
            }
            else
            {
                e.Row.Fixed = true;
                grdReport.Rows.Move(e.Row, 0);
            }
        }

        private void grdReport_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                if (_importType.HasValue)
                {
                    UltraWinGridUtils.EnableFixedFilterRow(e);
                    grdReport.DisplayLayout.Override.HeaderCheckBoxVisibility = HeaderCheckBoxVisibility.WhenUsingCheckEditor;
                    grdReport.DisplayLayout.Override.HeaderCheckBoxAlignment = HeaderCheckBoxAlignment.Right;
                    grdReport.DisplayLayout.Override.HeaderCheckBoxSynchronization = HeaderCheckBoxSynchronization.Band;
                    grdReport.DisplayLayout.Override.RowSelectorHeaderStyle = RowSelectorHeaderStyle.ColumnChooserButton;
                    grdReport.DisplayLayout.Override.RowSelectors = DefaultableBoolean.True;
                    UltraGridBand grdDataBand = null;
                    grdDataBand = grdReport.DisplayLayout.Bands[0];

                    SetGridColumns(grdDataBand);
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

        private void SetGridColumns(UltraGridBand gridBand)
        {
            try
            {
                foreach (UltraGridColumn column in gridBand.Columns)
                {
                    column.CellActivation = Activation.ActivateOnly;
                    column.Hidden = true;
                    column.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    column.PerformAutoResize(PerformAutoSizeType.AllRowsInBand, AutoResizeColumnWidthOptions.IncludeHeader);
                }
                if (!string.IsNullOrEmpty(_importType.ToString()))
                {
                    if (_importType.HasValue)
                    {
                        List<Prana.Import.BAL.ImportReportColumnDetails.SetColumnPropFunction> listFuncToCall = ImportReportColumnDetails.Instance.InitializeData(_importType.Value, false);
                        foreach (Prana.Import.BAL.ImportReportColumnDetails.SetColumnPropFunction deleg in listFuncToCall)
                        {
                            deleg.Invoke(gridBand);
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
    }
}
