//using Prana.Reconciliation;
using Infragistics.Win.UltraWinListView;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Prana.Tools
{
    public partial class ctrlReconReportLayout : UserControl
    {
        object lastMouseDownLocation = Point.Empty;
        //DataTable dtNirvanaMasterColumns = new DataTable();
        //DataTable dtNirvanaGridDisplayColumns = new DataTable();
        //DataTable dtPBMasterColumns = new DataTable();
        //DataTable dtPBGridDisplayColumns = new DataTable();

        public const string NirvanaNameAppend = " (Nirvana)";
        public const string PBNameAppend = " (PB)";
        public const string CommonNameAppend = " (Common)";
        public const string DiffNameAppend = " (Diff)";

        public const string NirvanaKeyAppend = "NirvanaColumn";
        public const string PBKeyAppend = "PBColumn";
        public const string CommonKeyAppend = "CommonColumn";
        public const string DiffKeyAppend = "DiffColumn";

        public const string NirvanaGroupKey = "NirvanaColumns";
        public const string NirvanaGroupText = "Nirvana Columns";

        public const string PBGroupKey = "PBColumns";
        public const string PBGroupText = "PB Columns";

        public const string CommonGroupText = "Common Columns";
        public const string CommonGroupKey = "CommonColumns";

        public const string DiffGroupText = "Diff Columns";
        public const string DiffGroupKey = "DiffColumns";

        private bool _isUnsavedChanges = false;
        private bool _isControlInitialized = false;

        /// <summary>
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        public ctrlReconReportLayout()
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
                btnSortAscending.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnSortAscending.ForeColor = System.Drawing.Color.White;
                btnSortAscending.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnSortAscending.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnSortAscending.UseAppStyling = false;
                btnSortAscending.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnSortDescending.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnSortDescending.ForeColor = System.Drawing.Color.White;
                btnSortDescending.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnSortDescending.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnSortDescending.UseAppStyling = false;
                btnSortDescending.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnFlipSortOrder.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnFlipSortOrder.ForeColor = System.Drawing.Color.White;
                btnFlipSortOrder.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnFlipSortOrder.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnFlipSortOrder.UseAppStyling = false;
                btnFlipSortOrder.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnDeleteSortOrder.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnDeleteSortOrder.ForeColor = System.Drawing.Color.White;
                btnDeleteSortOrder.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnDeleteSortOrder.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnDeleteSortOrder.UseAppStyling = false;
                btnDeleteSortOrder.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnGroupBy.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnGroupBy.ForeColor = System.Drawing.Color.White;
                btnGroupBy.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnGroupBy.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnGroupBy.UseAppStyling = false;
                btnGroupBy.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnDeleteGroupBy.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnDeleteGroupBy.ForeColor = System.Drawing.Color.White;
                btnDeleteGroupBy.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnDeleteGroupBy.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnDeleteGroupBy.UseAppStyling = false;
                btnDeleteGroupBy.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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

        internal void LoadReconReportLayout(ReconTemplate template)
        {
            try
            {
                _isControlInitialized = false;
                chkbxExactlyMatched.Checked = template.IsIncludeMatchedItems;
                chkbxMatchedinTol.Checked = template.IsIncludeToleranceMacthedItems;
                //file format type
                if (template.ExpReportFormat == AutomationEnum.FileFormat.csv)
                {
                    rbCSVFormat.Checked = true;
                }
                else if (template.ExpReportFormat == AutomationEnum.FileFormat.xls)
                    rbXLSFormat.Checked = true;
                else if (template.ExpReportFormat == AutomationEnum.FileFormat.pdf)                                                       //surendra
                    rbPDFFormat.Checked = true;
                //call method which display the list
                SetListView(template);
                _isControlInitialized = true;
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
        private void SetListView(ReconTemplate template)
        {
            try
            {
                //clear all the items from the both lists
                listviewAvailableColumns.Items.Clear();
                listviewSelectedColumns.Items.Clear();
                listviewColumnForSorting.Items.Clear();
                listviewGroupByColumns.Items.Clear();
                //clear all the groups
                listviewAvailableColumns.Groups.Clear();
                //image size empty to remove the title image
                listviewAvailableColumns.ViewSettingsDetails.ImageSize = Size.Empty;
                listviewSelectedColumns.ViewSettingsDetails.ImageSize = Size.Empty;
                listviewColumnForSorting.ViewSettingsDetails.ImageSize = Size.Empty;
                listviewGroupByColumns.ViewSettingsDetails.ImageSize = Size.Empty;
                //listview.ViewSettingsDetails.ImageSize = Size.Empty;

                //add common column group to the available column list
                UltraListViewGroup CommonGroup = new UltraListViewGroup();
                CommonGroup.Text = CommonGroupText;
                CommonGroup.Key = CommonGroupKey;
                listviewAvailableColumns.Groups.Add(CommonGroup);

                //add niravana group to the available column list
                UltraListViewGroup AvailNirvanaGroup = new UltraListViewGroup();
                AvailNirvanaGroup.Text = NirvanaGroupText;
                AvailNirvanaGroup.Key = NirvanaGroupKey;
                listviewAvailableColumns.Groups.Add(AvailNirvanaGroup);

                //add pb group to the available column list
                UltraListViewGroup AvailPBGroup = new UltraListViewGroup();
                AvailPBGroup.Text = PBGroupText;
                AvailPBGroup.Key = PBGroupKey;
                listviewAvailableColumns.Groups.Add(AvailPBGroup);

                //add diff column group to the available column list
                UltraListViewGroup DiffGroup = new UltraListViewGroup();
                DiffGroup.Text = DiffGroupText;
                DiffGroup.Key = DiffGroupKey;
                listviewAvailableColumns.Groups.Add(DiffGroup);

                UltraListViewItem item;
                //template.AvailableColumnList.Clear();
                //template.SelectedColumnList.Clear();
                if (template.AvailableColumnList.Count > 0)
                {
                    //add items to the available column list
                    foreach (ColumnInfo ColumnType in template.AvailableColumnList)
                    {
                        switch (ColumnType.GroupType)
                        {
                            case ColumnGroupType.Nirvana:
                                item = listviewAvailableColumns.Items.Add(NirvanaKeyAppend + ColumnType.ColumnName, ColumnType.ColumnName);
                                item.Appearance.Image = "";
                                item.Group = AvailNirvanaGroup;
                                break;
                            case ColumnGroupType.PrimeBroker:
                                item = listviewAvailableColumns.Items.Add(PBKeyAppend + ColumnType.ColumnName, ColumnType.ColumnName);
                                item.Appearance.Image = "";
                                item.Group = AvailPBGroup;
                                break;
                            case ColumnGroupType.Common:
                                item = listviewAvailableColumns.Items.Add(CommonKeyAppend + ColumnType.ColumnName, ColumnType.ColumnName);
                                item.Appearance.Image = "";
                                item.Group = CommonGroup;
                                break;
                            case ColumnGroupType.Both:
                                break;
                            case ColumnGroupType.Diff:
                                item = listviewAvailableColumns.Items.Add(DiffKeyAppend + ColumnType.ColumnName, ColumnType.ColumnName);
                                //remove title image from item
                                item.Group = DiffGroup;
                                item.Appearance.Image = "";
                                break;
                            default:
                                break;
                        }
                    }
                }
                //add items to the selected column list
                if (template.SelectedColumnList.Count > 0)
                {
                    foreach (ColumnInfo ColumnType in template.SelectedColumnList)
                    {
                        switch (ColumnType.GroupType)
                        {
                            case ColumnGroupType.Nirvana:
                                item = listviewSelectedColumns.Items.Add(NirvanaKeyAppend + ColumnType.ColumnName, ColumnType.ColumnName + NirvanaNameAppend);
                                item.Appearance.Image = "";
                                break;
                            case ColumnGroupType.PrimeBroker:
                                item = listviewSelectedColumns.Items.Add(PBKeyAppend + ColumnType.ColumnName, ColumnType.ColumnName + PBNameAppend);
                                item.Appearance.Image = "";
                                break;
                            case ColumnGroupType.Common:
                                item = listviewSelectedColumns.Items.Add(CommonKeyAppend + ColumnType.ColumnName, ColumnType.ColumnName + CommonNameAppend);
                                item.Appearance.Image = "";
                                break;
                            case ColumnGroupType.Both:
                                break;
                            case ColumnGroupType.Diff:
                                item = listviewSelectedColumns.Items.Add(DiffKeyAppend + ColumnType.ColumnName, ColumnType.ColumnName + DiffNameAppend);
                                //remove title image from item
                                item.Appearance.Image = "";
                                break;
                            default:
                                break;
                        }
                    }
                }
                if (template.ListSortByColumns.Count > 0)
                {
                    string columnName = string.Empty;
                    string columnKey = string.Empty;
                    foreach (ColumnInfo ColumnType in template.ListSortByColumns)
                    {
                        columnName = ColumnType.ColumnName;
                        //check group of item and add corresponding name
                        switch (ColumnType.GroupType)
                        {
                            case ColumnGroupType.Nirvana:
                                columnName = columnName + NirvanaNameAppend;
                                columnKey = NirvanaKeyAppend + ColumnType.ColumnName;
                                break;
                            case ColumnGroupType.PrimeBroker:
                                columnName = columnName + PBNameAppend;
                                columnKey = PBKeyAppend + ColumnType.ColumnName;
                                break;
                            case ColumnGroupType.Common:
                                columnName = columnName + CommonNameAppend;
                                columnKey = CommonKeyAppend + ColumnType.ColumnName;
                                break;
                            case ColumnGroupType.Both:
                                break;
                            case ColumnGroupType.Diff:
                                columnName = columnName + DiffNameAppend;
                                columnKey = DiffKeyAppend + ColumnType.ColumnName;
                                break;
                            default:
                                break;
                        }
                        if (ColumnType.SortOrder == SortingOrder.Ascending)
                        {
                            item = listviewColumnForSorting.Items.Add(columnKey, "Asc - " + columnName);
                            item.Appearance.Image = "";
                        }
                        else if (ColumnType.SortOrder == SortingOrder.Descending)
                        {
                            item = listviewColumnForSorting.Items.Add(columnKey, "Desc - " + columnName);
                            item.Appearance.Image = "";
                        }
                    }
                }
                if (template.ListGroupByColumns.Count > 0)
                {
                    string columnName = string.Empty;
                    string columnKey = string.Empty;
                    foreach (ColumnInfo ColumnType in template.ListGroupByColumns)
                    {
                        columnName = ColumnType.ColumnName;
                        //check group of item and add corresponding name
                        switch (ColumnType.GroupType)
                        {
                            case ColumnGroupType.Nirvana:
                                columnName = columnName + NirvanaNameAppend;
                                columnKey = NirvanaKeyAppend + ColumnType.ColumnName;
                                break;
                            case ColumnGroupType.PrimeBroker:
                                columnName = columnName + PBNameAppend;
                                columnKey = PBKeyAppend + ColumnType.ColumnName;
                                break;
                            case ColumnGroupType.Common:
                                columnName = columnName + CommonNameAppend;
                                columnKey = CommonKeyAppend + ColumnType.ColumnName;
                                break;
                            case ColumnGroupType.Both:
                                break;
                            case ColumnGroupType.Diff:
                                columnName = columnName + DiffNameAppend;
                                columnKey = DiffKeyAppend + ColumnType.ColumnName;
                                break;
                            default:
                                break;
                        }
                        item = listviewGroupByColumns.Items.Add(columnKey, columnName);
                        item.Appearance.Image = "";
                    }
                }
                this.listviewAvailableColumns.EndUpdate();
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


        internal void UpdateReconReportLayout(ReconTemplate template)
        {
            try
            {
                if (rbCSVFormat.Checked) /// csv checked
                {
                    template.ExpReportFormat = AutomationEnum.FileFormat.csv;
                }
                else if (rbXLSFormat.Checked) // excel checked
                {
                    template.ExpReportFormat = AutomationEnum.FileFormat.xls;
                }
                else if (rbPDFFormat.Checked)        // pdf checked
                {
                    template.ExpReportFormat = AutomationEnum.FileFormat.pdf;
                }
                //matched checkbox checked
                template.IsIncludeMatchedItems = chkbxExactlyMatched.Checked;
                template.IsIncludeToleranceMacthedItems = chkbxMatchedinTol.Checked;
                //clear the template list
                template.SelectedColumnList.Clear();
                template.AvailableColumnList.Clear();
                template.ListGroupByColumns.Clear();
                template.ListSortByColumns.Clear();

                //add items to the template.AvailableColumnList
                foreach (UltraListViewItem ListItem in listviewAvailableColumns.Items)
                {
                    ColumnInfo item = new ColumnInfo();
                    switch (ListItem.Group.Key)
                    {
                        case NirvanaGroupKey:
                            item.GroupType = ColumnGroupType.Nirvana;
                            break;
                        case PBGroupKey:
                            item.GroupType = ColumnGroupType.PrimeBroker;
                            break;
                        case CommonGroupKey:
                            item.GroupType = ColumnGroupType.Common;
                            break;
                        case DiffGroupKey:
                            item.GroupType = ColumnGroupType.Diff;
                            break;
                        default:
                            break;
                    }
                    item.ColumnName = ListItem.Text;
                    template.AvailableColumnList.Add(item);
                }
                //add items to the template.SelectedColumnList
                AddItemsToReconTemplate(template);
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
        /// Created by Faisal Shah
        /// Function Created as need for Refactoring
        /// </summary>
        /// <param name="template"></param>
        private void AddItemsToReconTemplate(ReconTemplate template)
        {

            foreach (UltraListViewItem ListItem in listviewSelectedColumns.Items)
            {
                ColumnInfo item = new ColumnInfo();
                String itemName = null;
                itemName = ListItem.Text;
                if (ListItem.Key.Contains(NirvanaKeyAppend))
                {
                    //this task is due to that common column may be on selected side
                    if (itemName.Contains(NirvanaNameAppend))
                    {
                        itemName = itemName.Replace(NirvanaNameAppend, "");
                    }
                    item.GroupType = ColumnGroupType.Nirvana;
                }
                else if (ListItem.Key.Contains(PBKeyAppend))
                {
                    if (itemName.Contains(PBNameAppend))
                    {
                        itemName = itemName.Replace(PBNameAppend, "");
                    }
                    item.GroupType = ColumnGroupType.PrimeBroker;
                }
                else if (ListItem.Key.Contains(CommonKeyAppend))
                {
                    if (itemName.Contains(CommonNameAppend))
                    {
                        itemName = itemName.Replace(CommonNameAppend, "");
                    }
                    item.GroupType = ColumnGroupType.Common;
                }
                else if (ListItem.Key.Contains(DiffKeyAppend))
                {
                    if (itemName.Contains(DiffNameAppend))
                    {
                        itemName = itemName.Replace(DiffNameAppend, "");
                    }
                    item.GroupType = ColumnGroupType.Diff;
                }
                //foreach (DataRow NirvanaRow in template.DsMasterColumns.Tables[ReconConstants.TABLENAME_NirvanaGridColumns])
                //{
                //    if ((string)NirvanaRow[ReconConstants.COLUMN_Name].Equals(itemName))
                //    {
                //        if (NirvanaRow[ReconConstants.COLUMN_FormulaExpression] != "")
                //        {
                //            item.FormulaExpression = NirvanaRow[ReconConstants.COLUMN_FormulaExpression];
                //        }
                //    }
                //}
                item.ColumnName = itemName;
                template.SelectedColumnList.Add(item);
            }
            foreach (UltraListViewItem ListItem in listviewGroupByColumns.Items)
            {
                ColumnInfo item = new ColumnInfo();
                String itemName = ListItem.Text;
                //check for item group
                //this task is due to that common column may be on selected side
                if (itemName.Contains(NirvanaNameAppend))
                {
                    itemName = itemName.Replace(NirvanaNameAppend, "");
                    item.GroupType = ColumnGroupType.Nirvana;
                }
                else if (itemName.Contains(PBNameAppend))
                {
                    itemName = itemName.Replace(PBNameAppend, "");
                    item.GroupType = ColumnGroupType.PrimeBroker;
                }
                else if (itemName.Contains(CommonNameAppend))
                {
                    itemName = itemName.Replace(CommonNameAppend, "");
                    item.GroupType = ColumnGroupType.Common;
                }
                else if (itemName.Contains(DiffNameAppend))
                {
                    itemName = itemName.Replace(DiffNameAppend, "");
                    item.GroupType = ColumnGroupType.Diff;
                }
                item.ColumnName = itemName;
                template.ListGroupByColumns.Add(item);
            }
            foreach (UltraListViewItem ListItem in listviewColumnForSorting.Items)
            {
                ColumnInfo item = new ColumnInfo();
                String itemName = ListItem.Text;
                if (ListItem.Text.Contains("Asc - "))
                {
                    itemName = itemName.Replace("Asc - ", "");
                    item.SortOrder = SortingOrder.Ascending;
                }
                else if (ListItem.Text.Contains("Desc - "))
                {
                    itemName = itemName.Replace("Desc - ", "");
                    item.SortOrder = SortingOrder.Descending;
                }
                //check for item group
                if (itemName.Contains(NirvanaNameAppend))
                {
                    itemName = itemName.Replace(NirvanaNameAppend, "");
                    item.GroupType = ColumnGroupType.Nirvana;
                }

                else if (itemName.Contains(PBNameAppend))
                {
                    itemName = itemName.Replace(PBNameAppend, "");
                    item.GroupType = ColumnGroupType.PrimeBroker;
                }


                else if (itemName.Contains(CommonNameAppend))
                {
                    itemName = itemName.Replace(CommonNameAppend, "");
                    item.GroupType = ColumnGroupType.Common;
                }


                else if (itemName.Contains(DiffNameAppend))
                {
                    itemName = itemName.Replace(DiffNameAppend, "");
                    item.GroupType = ColumnGroupType.Diff;
                }
                item.ColumnName = itemName;
                template.ListSortByColumns.Add(item);
            }
        }

        /// <summary>
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <returns></returns>
        public bool IsUnsavedChanges()
        {
            try
            {
                if (_isUnsavedChanges)
                {
                    _isUnsavedChanges = false;
                    return true;
                }
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
            return false;
        }

        //private void listAvailableColumns_MouseDown(object sender, MouseEventArgs e)
        //{
        //    try
        //    {
        //        WinListView_MouseDown(sender, e);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        //private void listAvailableColumns_MouseMove(object sender, MouseEventArgs e)
        //{
        //    try
        //    {
        //        WinListView_MouseMove(sender, e);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }

        //}

        //private void listAvailableColumns_DragDrop(object sender, DragEventArgs e)
        //{
        //    try
        //    {

        //        WinListView_DragDrop(sender, e);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }

        //}

        //private void listAvailableColumns_DragOver(object sender, DragEventArgs e)
        //{
        //    try
        //    {
        //        WinListView_DragOver(sender, e);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }

        //}

        /// <summary>
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listSelectedColumns_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                WinListView_DragDrop(sender, e);
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
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listSelectedColumns_DragOver(object sender, DragEventArgs e)
        {
            try
            {
                WinListView_DragOver(sender, e);
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
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listSelectedColumns_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                WinListView_MouseDown(sender, e);
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
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listSelectedColumns_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                WinListView_MouseMove(sender, e);
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
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WinListView_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                // Use the ItemFromPoint method to determine whether the cursor
                // is positioned over an item; if it is, record the cursor's location
                UltraListView listViewDrag = sender as UltraListView;
                UltraListViewItem itemAtPoint = listViewDrag.ItemFromPoint(e.X, e.Y,
                true);

                if (itemAtPoint != null)
                    this.lastMouseDownLocation = new Point(e.X, e.Y);
                else
                    this.lastMouseDownLocation = null;
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
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WinListView_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                UltraListView listViewDrag = sender as UltraListView;

                // If the left mouse button is pressed, and the recorded cursor
                // location is set to a valid value, continue...
                if (e.Button == MouseButtons.Left &&
                this.lastMouseDownLocation != null &&
                listViewDrag.SelectedItems.Count > 0)
                {
                    Point cursorPos = new Point(e.X, e.Y);
                    Point dragPoint = (Point)this.lastMouseDownLocation;

                    // Determine whether the cursor has moved outside the drag rectangle;
                    // if it has, initiate a drag drop operation.
                    Rectangle dragRect = new Rectangle(dragPoint,
                    SystemInformation.DragSize);
                    dragRect.X -= (int)(SystemInformation.DragSize.Width / 2);
                    dragRect.Y -= (int)(SystemInformation.DragSize.Height / 2);

                    if (dragRect.Contains(cursorPos) == false)
                    {
                        listViewDrag.Capture = false;
                        listViewDrag.DoDragDrop(listViewDrag.SelectedItems,
                        DragDropEffects.Move);
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
        private void WinListView_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                UltraListView listViewDrop = sender as UltraListView;

                IDataObject dataObject = e.Data;
                if (dataObject != null)
                {
                    // Reverse iterate the SelectedItems collection of the control from which
                    // the drag was initiated; add items to the control that received the drop
                    // and remove them from the control that initiated the drag.
                    UltraListViewSelectedItemsCollection selectedItems = dataObject.GetData(typeof(UltraListViewSelectedItemsCollection)) as
                    UltraListViewSelectedItemsCollection;
                    if (selectedItems != null)
                    {
                        for (int i = selectedItems.Count - 1; i >= 0; i--)
                        {
                            UltraListViewItem item = selectedItems[i];
                            int indexFromMoveTo = item.Index;
                            UltraListView listViewDrag = item.Control;

                            //prevent drag and drop from available and selected list to group and sorting list
                            if (listViewDrag.Equals(listviewAvailableColumns) || listViewDrag.Equals(listviewSelectedColumns))
                            {
                                if (listViewDrop.Equals(listviewGroupByColumns) || listViewDrop.Equals(listviewColumnForSorting))
                                    break;
                            }
                            //prevent drag and drop from group list to another list
                            if (listViewDrag.Equals(listviewGroupByColumns))
                                if (!listViewDrop.Equals(listviewGroupByColumns))
                                    break;
                            //prevent drag and drop from sort list to another list
                            if (listViewDrag.Equals(listviewColumnForSorting))
                                if (!listViewDrop.Equals(listviewColumnForSorting))
                                    break;
                            //disable drag and drop in the available column list from same list
                            if (listViewDrag.Equals(listviewAvailableColumns))
                                if (listViewDrop.Equals(listviewAvailableColumns))
                                    break;
                            // Deselect and remove the item from the source control;

                            listViewDrag.SelectedItems.Remove(item);
                            listViewDrag.Items.Remove(item);

                            //pointer location of the target item
                            UltraListViewItem it = listViewDrop.ItemFromPoint(listViewDrop.PointToClient(new Point(e.X, e.Y)));

                            //if item is dropped on an existing item
                            if (it != null)
                            {
                                int indexToMoveTo = it.Index;
                                if (indexToMoveTo < 0)
                                {
                                    listViewDrag.Items.Insert(indexFromMoveTo, item);
                                    break;
                                }

                                listViewDrop.Items.Insert(indexToMoveTo, item);
                            }
                            else //if item is dropped anywhere in the list, not the location of an item
                            {
                                listViewDrop.Items.Add(item);
                            }

                            //drop item to the available list
                            if (listViewDrop.Groups.Count > 0)
                            {
                                String itemName = item.Text;
                                if (itemName.Contains(NirvanaNameAppend))
                                {
                                    itemName = itemName.Replace(NirvanaNameAppend, "");
                                    item.Value = itemName;
                                }
                                else if (itemName.Contains(PBNameAppend))
                                {
                                    itemName = itemName.Replace(PBNameAppend, "");
                                    item.Value = itemName;
                                }
                                else if (itemName.Contains(CommonNameAppend))
                                {
                                    itemName = itemName.Replace(CommonNameAppend, "");
                                    item.Value = itemName;
                                }
                                else if (itemName.Contains(DiffNameAppend))
                                {
                                    itemName = itemName.Replace(DiffNameAppend, "");
                                    item.Value = itemName;
                                }

                                if (item.Key.Contains(CommonKeyAppend))
                                {
                                    item.Group = listViewDrop.Groups[0];
                                }
                                else if (item.Key.Contains(NirvanaKeyAppend))
                                {
                                    item.Group = listViewDrop.Groups[1];
                                }
                                else if (item.Key.Contains(PBKeyAppend))
                                {
                                    item.Group = listViewDrop.Groups[2];
                                }
                                else if (item.Key.Contains(DiffKeyAppend))
                                {
                                    item.Group = listViewDrop.Groups[3];
                                }

                            }
                            else //drop item to the selected list
                            {
                                if ((item.Key.Contains(NirvanaKeyAppend)) && (listViewDrop != listViewDrag))
                                {
                                    item.Value = item.Text + NirvanaNameAppend;
                                }
                                else if ((item.Key.Contains(PBKeyAppend)) && (listViewDrop != listViewDrag))
                                {
                                    item.Value = item.Text + PBNameAppend;
                                }
                                else if ((item.Key.Contains(CommonKeyAppend)) && (listViewDrop != listViewDrag))
                                {
                                    item.Value = item.Text + CommonNameAppend;
                                }
                                else if ((item.Key.Contains(DiffKeyAppend)) && (listViewDrop != listViewDrag))
                                {
                                    item.Value = item.Text + DiffNameAppend;
                                }
                                item.Group = null;
                            }
                        }
                        _isUnsavedChanges = true;
                    }
                    //dataObject.setData(null);
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
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WinListView_DragOver(object sender, DragEventArgs e)
        {
            try
            {
                e.Effect = DragDropEffects.All;
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

        private void listAvailableColumns_MouseHover(object sender, EventArgs e)
        {
            try
            {
                //TODO AMAN
                //set the message for tooltip
                //tooltipShowDragMessage.Show("Drag Columns From Available Columns to Selected Columns",listviewAvailableColumns,2000);
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

        private void btnSortDescending_Click(object sender, EventArgs e)
        {
            try
            {
                bool isItemAlreadyAvailable = false;
                List<string> CommonColumnKeys = new List<string>();
                UltraListViewSelectedItemsCollection selectedItems = null;
                selectedItems = listviewSelectedColumns.SelectedItems;
                if (selectedItems != null)
                {
                    foreach (UltraListViewItem item in listviewColumnForSorting.Items)
                    {
                        CommonColumnKeys.Add(item.Key);
                    }
                    foreach (UltraListViewItem SelectItem in selectedItems)
                    {
                        foreach (string key in CommonColumnKeys)
                        {
                            if (SelectItem.Key.Contains(key))
                                isItemAlreadyAvailable = true;
                        }
                        if (!isItemAlreadyAvailable)
                        {
                            listviewColumnForSorting.Items.Add(SelectItem.Key, "Desc - " + SelectItem.Text);
                            SelectItem.Appearance.Image = "";
                        }
                    }
                    _isUnsavedChanges = true;
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

        private void btnSortAscending_Click(object sender, EventArgs e)
        {
            try
            {
                bool isItemAlreadyAvailable = false;
                UltraListViewSelectedItemsCollection selectedItems = listviewSelectedColumns.SelectedItems;
                List<string> CommonColumnKeys = new List<string>();
                if (selectedItems != null)
                {
                    foreach (UltraListViewItem item in listviewColumnForSorting.Items)
                    {
                        CommonColumnKeys.Add(item.Key);
                    }
                    foreach (UltraListViewItem SelectItem in selectedItems)
                    {
                        foreach (string key in CommonColumnKeys)
                        {
                            if (SelectItem.Key.Contains(key))
                                isItemAlreadyAvailable = true;
                        }
                        if (!isItemAlreadyAvailable)
                        {
                            listviewColumnForSorting.Items.Add(SelectItem.Key, "Asc - " + SelectItem.Text);
                            SelectItem.Appearance.Image = "";
                        }
                    }
                    _isUnsavedChanges = true;
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
        private void btnDeleteSortOrder_Click(object sender, EventArgs e)
        {
            try
            {
                UltraListViewSelectedItemsCollection selectedItems = listviewColumnForSorting.SelectedItems;
                if (selectedItems != null)
                {
                    for (int i = 0; i < selectedItems.Count; i++)
                    {
                        UltraListViewItem item = selectedItems[i];
                        listviewColumnForSorting.SelectedItems.Remove(item);
                        listviewColumnForSorting.Items.Remove(item);
                    }
                    _isUnsavedChanges = true;
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
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFlipSortOrder_Click(object sender, EventArgs e)
        {
            try
            {
                UltraListViewSelectedItemsCollection selectedItems = listviewColumnForSorting.SelectedItems;
                if (selectedItems != null)
                {
                    foreach (UltraListViewItem selectedItem in selectedItems)
                    {
                        String nameAfterFlip = selectedItem.Text;
                        if (selectedItem.Text.Contains("Desc"))
                        {
                            nameAfterFlip = nameAfterFlip.Replace("Desc", "Asc");
                            selectedItem.Value = nameAfterFlip;
                        }
                        else if (selectedItem.Text.Contains("Asc"))
                        {
                            nameAfterFlip = nameAfterFlip.Replace("Asc", "Desc");
                            selectedItem.Value = nameAfterFlip;
                        }
                    }
                    _isUnsavedChanges = true;
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

        private void btnGroupBy_Click(object sender, EventArgs e)
        {
            try
            {
                bool isItemAlreadyAvailable = false;
                UltraListViewSelectedItemsCollection selectedItems = listviewSelectedColumns.SelectedItems;
                List<string> GroupColumnKeys = new List<string>();
                if (selectedItems != null)
                {
                    foreach (UltraListViewItem item in listviewGroupByColumns.Items)
                    {
                        GroupColumnKeys.Add(item.Key);
                    }
                    foreach (UltraListViewItem SelectItem in selectedItems)
                    {
                        foreach (string key in GroupColumnKeys)
                        {
                            if (SelectItem.Key.Contains(key))
                                isItemAlreadyAvailable = true;
                        }
                        if (!isItemAlreadyAvailable)
                        {
                            UltraListViewItem addItem = listviewGroupByColumns.Items.Add(SelectItem.Key, SelectItem.Text);
                            addItem.Appearance.Image = "";
                        }
                    }
                    _isUnsavedChanges = true;
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

        private void btnDeleteGroupBy_Click(object sender, EventArgs e)
        {
            try
            {
                UltraListViewSelectedItemsCollection selectedItems = listviewGroupByColumns.SelectedItems;
                if (selectedItems != null)
                {
                    for (int i = 0; i < selectedItems.Count; i++)
                    {
                        UltraListViewItem item = selectedItems[i];
                        listviewGroupByColumns.SelectedItems.Remove(item);
                        listviewGroupByColumns.Items.Remove(item);
                    }
                    _isUnsavedChanges = true;
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
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkbxExactlyMatched_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (_isControlInitialized)
                    _isUnsavedChanges = true;
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
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkbxMatchedinTol_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (_isControlInitialized)
                    _isUnsavedChanges = true;
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
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbXLSFormat_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rbXLSFormat.Checked)
                    btnGroupBy.Enabled = true;
                if (_isControlInitialized)
                    _isUnsavedChanges = true;
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
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbCSVFormat_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rbCSVFormat.Checked)
                {
                    //btnGroupBy.Enabled = false ;
                }
                if (_isControlInitialized)
                    _isUnsavedChanges = true;
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
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbPDFFormat_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rbPDFFormat.Checked)
                    btnGroupBy.Enabled = true;
                if (_isControlInitialized)
                    _isUnsavedChanges = true;
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
        /// Hide Controls which allow to Se Sorting & Grouping on Columns at Template Level for Exception Reports UI
        /// </summary>
        //internal void HideSortingAndGroupingControls()
        //{
        //    try
        //    {
        //        btnDeleteGroupBy.Visible = false;
        //        btnGroupBy.Visible = false;
        //        btnDeleteSortOrder.Visible = false;
        //        btnSortAscending.Visible = false;
        //        btnSortDescending.Visible = false;
        //        btnFlipSortOrder.Visible = false;
        //        listviewGroupByColumns.Visible = false;
        //        listviewColumnForSorting.Visible = false;
        //        lblColumntoSort.Visible = false;
        //        ultraLabel1.Visible = false;
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

    }
}
