using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinTree;
using Prana.CommonDatabaseAccess;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;


namespace Prana.CashManagement
{
    public partial class ctrlAccounts : UserControl
    {
        //private DataSet _dataSet = new DataSet();
        private ValueList _accountTypes = new ValueList();
        //private string _mappingDirPath = Application.StartupPath + @"\" + ApplicationConstants.MAPPING_FILE_DIRECTORY + @"\" + ApplicationConstants.MappingFileType.ReconMappingXml.ToString();
        //private DataTable _dtMappingFile = null;
        private DataSet _dataSetMasterCategory = new DataSet();

        private ValueList _vlsubAccountType = new ValueList();
        private ValueList _transactionType = new ValueList();
        private ValueList _masterCategoryType = new ValueList();

        static int _maxSubAccountID = 0;
        static int _maxSubCategoryID = 0;

        public bool isUnSavedChanges = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="ctrlAccounts"/> class.
        /// </summary>
        public ctrlAccounts()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Handles the AfterSelect event of the accTree control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SelectEventArgs"/> instance containing the event data.</param>
        private void accTree_AfterSelect(object sender, SelectEventArgs e)
        {
            try
            {
                if (e.NewSelections.Count > 0)
                {
                    UltraTreeNode currentNode = e.NewSelections[0];
                    currentNode.BringIntoView();

                    if (currentNode.Level != 0)
                    {
                        grdData.Visible = true;
                        if (!currentNode.DataColumnSetResolved.Key.Equals(CashManagementConstants.TABLE_MASTERCATEGORY) && !currentNode.DataColumnSetResolved.Key.Equals(CashManagementConstants.RELATION_MASTERCATEGORYSUBCATEGORY))
                        {
                            DataRow row = ((System.Data.DataRowView)(currentNode.ListObject)).Row;
                            BindGrid(row);
                            foreach (UltraGridRow gr in grdData.Rows)
                            {
                                if (row.RowState.ToString() == "Added")
                                    gr.Band.Columns[CashManagementConstants.COLUMN_ACRONYM].CellActivation = Activation.AllowEdit;
                                else
                                    gr.Band.Columns[CashManagementConstants.COLUMN_ACRONYM].CellActivation = Activation.NoEdit;
                            }

                        }
                        else if (currentNode.DataColumnSetResolved.Key.Equals(CashManagementConstants.RELATION_MASTERCATEGORYSUBCATEGORY))
                        {
                            DataRow row = ((System.Data.DataRowView)(currentNode.ListObject)).Row;
                            int subCategoryID = Convert.ToInt32(row[CashManagementConstants.COLUMN_SUBCATEGORYID].ToString());
                            DataRow[] result = row.Table.DataSet.Tables[CashManagementConstants.TABLE_SUBACCOUNTS].Select(CashManagementConstants.COLUMN_SUBCATEGORYID + " = " + subCategoryID + "");
                            bool blnResult = false;
                            if (result.Length == 0)
                            {
                                DataRow newRow = ((System.Data.DataRowView)(currentNode.ListObject)).Row;
                                BindGrid(newRow);
                                foreach (UltraGridRow gr in grdData.Rows)
                                {
                                    //if (((System.Data.DataRowView)(gr.ListObject)).Row != row)
                                    //    gr.Band.Columns[CashManagementConstants.COLUMN_MASTERCATEGORYID].CellActivation = Activation.NoEdit;   

                                    if (row.RowState.ToString() == "Added")
                                        gr.Band.Columns[CashManagementConstants.COLUMN_MASTERCATEGORYID].CellActivation = Activation.AllowEdit;
                                    else
                                        gr.Band.Columns[CashManagementConstants.COLUMN_MASTERCATEGORYID].CellActivation = Activation.NoEdit;
                                }
                            }

                            else if (result.Length > 0)
                            {
                                foreach (DataRow dr in result)
                                {
                                    int subAccountID = Convert.ToInt32(dr.ItemArray[0]);
                                    if (CashAccountDataManager.IfSubAccountIsInUse(subAccountID))
                                    {
                                        blnResult = true;
                                        break;
                                    }
                                    else
                                        blnResult = false;
                                }
                            }
                            if (blnResult == false)
                            {
                                DataRow newRow = ((System.Data.DataRowView)(currentNode.ListObject)).Row;
                                BindGrid(newRow);
                                foreach (UltraGridRow gr in grdData.Rows)
                                {
                                    //if (((System.Data.DataRowView)(gr.ListObject)).Row != row)
                                    //    gr.Band.Columns[CashManagementConstants.COLUMN_MASTERCATEGORYID].CellActivation = Activation.NoEdit;                                          

                                    if (row.RowState.ToString() == "Added")
                                        gr.Band.Columns[CashManagementConstants.COLUMN_MASTERCATEGORYID].CellActivation = Activation.AllowEdit;
                                    else
                                        gr.Band.Columns[CashManagementConstants.COLUMN_MASTERCATEGORYID].CellActivation = Activation.NoEdit;
                                }

                            }
                            else
                            {
                                DataRow newRow = ((System.Data.DataRowView)(currentNode.ListObject)).Row;
                                BindGrid(newRow);
                                foreach (UltraGridRow gr in grdData.Rows)
                                {
                                    //if (((System.Data.DataRowView)(gr.ListObject)).Row != row)
                                    //    gr.Band.Columns[CashManagementConstants.COLUMN_MASTERCATEGORYID].CellActivation = Activation.NoEdit;
                                    //gr.Hidden = true;
                                    if (row.RowState.ToString() == "Added")
                                        gr.Band.Columns[CashManagementConstants.COLUMN_MASTERCATEGORYID].CellActivation = Activation.AllowEdit;
                                    else
                                        gr.Band.Columns[CashManagementConstants.COLUMN_MASTERCATEGORYID].CellActivation = Activation.NoEdit;
                                }
                            }

                        }
                        else
                        {
                            grdData.Hide();

                        }
                    }

                    else
                    {
                        //foreach (UltraGridRow gr in grdData.Rows)
                        //    gr.Hidden = true;
                        grdData.Hide();

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Selects the default tree node.
        /// </summary>
        internal void SelectDefaultTreeNode()
        {
            try
            {
                if (accTree.Nodes.Count > 0)
                {
                    accTree.Nodes[0].Selected = true;
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
        }

        /// <summary>
        /// Handles the ColumnSetGenerated event of the accTree control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Infragistics.Win.UltraWinTree.ColumnSetGeneratedEventArgs"/> instance containing the event data.</param>
        private void accTree_ColumnSetGenerated(object sender, Infragistics.Win.UltraWinTree.ColumnSetGeneratedEventArgs e)
        {

            try
            {
                switch (e.ColumnSet.Key)
                {
                    //case "CashAccounts":

                    //    e.ColumnSet.NodeTextColumn = e.ColumnSet.Columns[CashManagementConstants.COLUMN_NAME];

                    //    break;

                    case CashManagementConstants.COLUMN_SUBACCOUNTS:

                        e.ColumnSet.NodeTextColumn = e.ColumnSet.Columns[CashManagementConstants.COLUMN_NAME];

                        break;

                    case CashManagementConstants.COLUMN_MASTERCATEGORY:
                        e.ColumnSet.NodeTextColumn = e.ColumnSet.Columns[CashManagementConstants.COLUMN_MASTERCATEGORYNAME];
                        break;

                    case CashManagementConstants.RELATION_MASTERCATEGORYSUBCATEGORY:
                        e.ColumnSet.NodeTextColumn = e.ColumnSet.Columns[1];
                        break;

                    case CashManagementConstants.RELATION_SUBCATEGORYSUBACCOUNTS:

                        e.ColumnSet.NodeTextColumn = e.ColumnSet.Columns[CashManagementConstants.COLUMN_NAME];
                        break;

                    case "subAccountsTransactiontype":
                        e.ColumnSet.NodeTextColumn = e.ColumnSet.Columns[CashManagementConstants.TABLE_ACCOUNTTYPE];
                        break;
                }

                foreach (Infragistics.Win.UltraWinTree.UltraTreeNodeColumn column in e.ColumnSet.Columns)
                {
                    switch (column.Key.ToLower())
                    {
                        case "mastercategoryaccountside":
                            column.Visible = false;
                            break;

                        case CashManagementConstants.RELATION_MASTERCATEGORYSUBCATEGORY:

                            break;

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


        Infragistics.Win.UltraWinTree.UltraTreeNode _mouseNode;
        /// <summary>
        /// Handles the MouseDown event of the accTree control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void accTree_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    Point mousePoint = new Point(e.X, e.Y);
                    _mouseNode = (UltraTreeNode)accTree.GetNodeFromPoint(mousePoint);


                    if (_mouseNode != null && _mouseNode.Level == 1)
                    {
                        mnuItemAddSubAccount.Visible = true;
                        mnuItemAddAccount.Visible = false;
                        mnuItemDeleteSubAccount.Visible = false;
                        mnuItemDeleteAccount.Visible = false;
                        //mnuItemAddJournalCodeToolStripMenuItem.Visible = false;
                        _mouseNode.Selected = true;
                        accTree.ActiveNode = _mouseNode;

                        //Check System Generated or User generated Sub category
                        DataRow row = ((System.Data.DataRowView)(_mouseNode.ListObject)).Row;
                        int subCategoryID = Convert.ToInt32(row[CashManagementConstants.COLUMN_SUBCATEGORYID].ToString());
                        DataRow[] result = row.Table.DataSet.Tables[CashManagementConstants.TABLE_SUBACCOUNTS].Select(CashManagementConstants.COLUMN_SUBCATEGORYID + " = " + subCategoryID + "");
                        bool blnResult = false;
                        if (result.Length == 0)
                            mnuItemDeleteAccount.Visible = true;

                        else if (result.Length > 0)
                        {
                            foreach (DataRow dr in result)
                            {
                                int subAccountID = Convert.ToInt32(dr.ItemArray[0]);
                                if (CashAccountDataManager.IfSubAccountIsInUse(subAccountID))
                                {
                                    blnResult = true;
                                    break;
                                }
                                else
                                {
                                    blnResult = false;
                                }
                            }
                            if (blnResult == false)
                            {
                                mnuItemDeleteAccount.Visible = true;
                            }
                        }
                        else
                            mnuItemDeleteAccount.Visible = false;
                    }


                    else if (_mouseNode != null && _mouseNode.Level == 0)
                    {
                        mnuItemAddAccount.Visible = true;
                        mnuItemDeleteAccount.Visible = false;
                        mnuItemAddSubAccount.Visible = false;
                        mnuItemDeleteSubAccount.Visible = false;
                        //mnuItemAddJournalCodeToolStripMenuItem.Visible = false;

                        _mouseNode.Selected = true;
                        accTree.ActiveNode = _mouseNode;

                    }
                    else if (_mouseNode != null && _mouseNode.Level == 2)
                    {
                        mnuItemAddAccount.Visible = false;
                        mnuItemDeleteAccount.Visible = false;
                        mnuItemAddSubAccount.Visible = false;
                        mnuItemDeleteSubAccount.Visible = true;
                        //mnuItemAddJournalCodeToolStripMenuItem.Visible = false;

                    }
                    else
                    {
                        mnuItemAddAccount.Visible = false;
                        mnuItemDeleteAccount.Visible = false;
                        mnuItemAddSubAccount.Visible = false;
                        mnuItemDeleteSubAccount.Visible = false;
                        //mnuItemAddJournalCodeToolStripMenuItem.Visible = false;
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
        /// Call on Cell Change of grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdData_CellChange(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            ValidateRow(e.Cell);
            isUnSavedChanges = true;
        }

        /// <summary>
        /// Validates the row.
        /// </summary>
        /// <param name="cell">The cell.</param>
        private void ValidateRow(UltraGridCell cell)
        {
            try
            {
                DataRow row = ((System.Data.DataRowView)(cell.Row.ListObject)).Row;
                string columnModified = cell.Column.Key.ToString();

                switch (columnModified)
                {
                    case CashManagementConstants.COLUMN_NAME:
                        if (cell.Text.Equals(String.Empty))
                        {
                            row.SetColumnError(columnModified, "Name can not be Empty !");
                        }
                        else if (IsRepeatedName(cell.Text, columnModified, row))
                        {
                            row.SetColumnError(columnModified, "Name Already Exists !");
                        }
                        else
                        {
                            row.SetColumnError(columnModified, "");
                        }
                        if (row[CashManagementConstants.COLUMN_ACRONYM].ToString().Equals(String.Empty))
                        {
                            row.SetColumnError(CashManagementConstants.COLUMN_ACRONYM, "Acronym can not be Empty !");
                        }
                        if (row.Table.Columns.Contains(CashManagementConstants.COLUMN_TYPEID) && row[CashManagementConstants.COLUMN_TYPEID].ToString().Equals(String.Empty))
                        {
                            row.SetColumnError(CashManagementConstants.COLUMN_TYPEID, "Select a Type !");
                        }
                        else if (row.Table.Columns.Contains(CashManagementConstants.COLUMN_TYPEID) && row[CashManagementConstants.COLUMN_TYPEID].ToString() == int.MinValue.ToString())
                        {
                            row.SetColumnError(CashManagementConstants.COLUMN_TYPEID, "Select a valid Type !");
                        }
                        break;

                    case CashManagementConstants.COLUMN_ACRONYM:

                        if (cell.Text.Equals(String.Empty))
                        {
                            row.SetColumnError(columnModified, "Acronym can not be Empty !");
                        }
                        else if (IsRepeatedName(cell.Text, columnModified, row))
                        {
                            row.SetColumnError(columnModified, "Acronym Already Exists !");
                        }
                        else
                        {
                            row.SetColumnError(columnModified, "");
                        }
                        if (row[CashManagementConstants.COLUMN_NAME].ToString().Equals(String.Empty))
                        {
                            row.SetColumnError(CashManagementConstants.COLUMN_NAME, "Acronym can not be Empty !");
                        }

                        //if (row[CashManagementConstants.COLUMN_SUBCATEGORY].ToString().Equals(String.Empty))
                        //{
                        //    row.SetColumnError(CashManagementConstants.COLUMN_NAME, "Subcategory can not be Empty !");
                        //}
                        if (row.Table.Columns.Contains(CashManagementConstants.COLUMN_TYPEID) && row[CashManagementConstants.COLUMN_TYPEID].ToString().Equals(String.Empty))
                        {
                            row.SetColumnError(CashManagementConstants.COLUMN_TYPEID, "Select a Type !");
                        }
                        else if (row.Table.Columns.Contains(CashManagementConstants.COLUMN_TYPEID) && row[CashManagementConstants.COLUMN_TYPEID].ToString() == int.MinValue.ToString())
                        {
                            row.SetColumnError(CashManagementConstants.COLUMN_TYPEID, "Select a valid Type !");
                        }
                        break;


                    case CashManagementConstants.COLUMN_ACCOUNTTYPEIDCAPITAL:
                        if (cell.Text.Equals(String.Empty))
                        {
                            row.SetColumnError(columnModified, "Select a Type !");
                        }
                        else if (cell.Text == ApplicationConstants.C_COMBO_SELECT)
                        {
                            row.SetColumnError(columnModified, "Select a valid Type !");
                        }
                        else
                        {
                            row.SetColumnError(columnModified, "");
                        }
                        if (row[CashManagementConstants.COLUMN_NAME].ToString().Equals(String.Empty))
                        {
                            row.SetColumnError(CashManagementConstants.COLUMN_NAME, "Acronym can not be Empty !");
                        }
                        if (row[CashManagementConstants.COLUMN_ACRONYM].ToString().Equals(String.Empty))
                        {
                            row.SetColumnError(CashManagementConstants.COLUMN_ACRONYM, "Acronym can not be Empty !");
                        }
                        break;
                    case CashManagementConstants.COLUMN_SUBCATEGORY:
                        if (cell.Text.Equals(String.Empty))
                        {
                            row.SetColumnError(CashManagementConstants.COLUMN_SUBCATEGORY, "Subcategory can not be Empty !");
                        }
                        else
                        {
                            row.SetColumnError(CashManagementConstants.COLUMN_SUBCATEGORY, "");
                        }
                        break;

                    //added by: Bharat Raturi, 8 dec 2014
                    //prevent duplicate sub category
                    //http://jira.nirvanasolutions.com:8080/browse/PRANA-5560
                    case CashManagementConstants.COLUMN_SUBCATEGORYNAME:
                        if (cell.Text.Equals(String.Empty))
                        {
                            row.SetColumnError(columnModified, "Sub category name can not be Empty !");
                        }
                        else if (IsRepeatedName(cell.Text, columnModified, row))
                        {
                            row.SetColumnError(columnModified, "Sub category name Already Exists !");
                        }
                        else
                        {
                            row.SetColumnError(columnModified, "");
                        }
                        break;
                    default:
                        break;
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
        /// Handles the InitializeLayout event of the grdData control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="InitializeLayoutEventArgs"/> instance containing the event data.</param>
        private void grdData_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                UltraGridBand band = e.Layout.Bands[0];

                grdData.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
                //set view of ultragrid as cardview
                band.CardView = true;
                //Set the spacing bettween cards in pixel.
                band.Override.CardSpacing = 20;
                band.CardSettings.Width = 170;
                // Set various appearance related properties.
                //band.Override.BorderStyleCardArea = UIElementBorderStyle.Raised;

                // CardAreaAppearance applies to the whole card area.
                band.Override.CardAreaAppearance.BackColor = Color.Black;
                // SelectedCardCaptionAppearance appearance applies to the card captions of selected 
                // cards in card-view.
                band.Override.SelectedCardCaptionAppearance.BackColor = Color.Black;
                band.Override.SelectedCardCaptionAppearance.ForeColor = Color.White;

                // ActiveCardCaptionAppearance appearance applies to the card caption of the active
                // card in card-view. 
                band.Override.ActiveCardCaptionAppearance.BackColor = Color.Black;
                band.Override.ActiveCardCaptionAppearance.ForeColor = Color.White;

                // Also various appearance and border related properties used for setting up objects like
                // cells, rows, headers in non-card view also apply in card-view. 

                // BorderStyleRow sets the border style of cards.
                //band.Override.BorderStyleRow = UIElementBorderStyle.Raised;

                // BorderStyleCell applies to cells in the cards.
                band.Override.BorderStyleCell = UIElementBorderStyle.Inset;
                band.Override.HeaderAppearance.BackColor = Color.Black;
                band.Override.HeaderAppearance.BackColor2 = Color.Black;
                band.Override.HeaderAppearance.ForeColor = Color.White;
                // BorderStyleHeader applies to column captions in the card.
                //band.Override.BorderStyleHeader = UIElementBorderStyle.Raised;

                // CellAppearance applies to cells in the cards.
                band.Override.CellAppearance.BackColor = Color.White;

                // HeaderAppearance applies to column captions in the card.
                band.Override.HeaderAppearance.FontData.Bold = DefaultableBoolean.True;

                //SetGridRowLayout(band);
                grdData.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnCellChangeOrLostFocus;
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

        //private static void SetGridRowLayout(UltraGridBand band)
        //{
        //band.RowLayouts.Clear();
        //RowLayout rowLayout1 = band.RowLayouts.Add("RowLayout1");
        //if (band.Columns.Exists(CashManagementConstants.COLUMN_NAME))
        //{
        //    rowLayout1.ColumnInfos[CashManagementConstants.COLUMN_NAME].Initialize(2, 0, 0, 0);
        //}
        //if (band.Columns.Exists(CashManagementConstants.COLUMN_ACRONYM))
        //{
        //    rowLayout1.ColumnInfos[CashManagementConstants.COLUMN_ACRONYM].Initialize(2, 2, 0, 0);
        //}
        //if (band.Columns.Exists("Type"))
        //{
        //    rowLayout1.ColumnInfos["Type"].Initialize(2, 6, 0, 0);
        //}

        //rowLayout1.CardView = true;
        //rowLayout1.RowLayoutLabelStyle = RowLayoutLabelStyle.Separate;
        //// CardViewStyle only applies in card-view. 
        //rowLayout1.CardViewStyle = CardStyle.StandardLabels;
        //rowLayout1.RowLayoutLabelPosition = LabelPosition.Left;
        //band.Override.HeaderAppearance.BorderAlpha = Alpha.Transparent;
        //band.Override.HeaderAppearance.FontData.Bold = DefaultableBoolean.True;
        //}
        /// <summary>
        /// Handles the Load event of the ctrlAccounts control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ctrlAccounts_Load(object sender, EventArgs e)
        {
            try
            {
                if (!CustomThemeHelper.ApplyTheme)
                {
                    Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
                    appearance1.BackColor = System.Drawing.Color.Black;
                    appearance1.BackColor2 = System.Drawing.Color.White;
                    appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
                    appearance1.BorderColor = System.Drawing.Color.Black;
                    appearance1.FontData.BoldAsString = "True";
                    appearance1.FontData.SizeInPoints = 9F;
                    appearance1.ForeColor = System.Drawing.Color.White;
                    appearance1.TextVAlignAsString = "Middle";
                    //this.lblActivityJournalMapping.Appearance = appearance1;

                    Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
                    appearance4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
                    appearance4.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(167)))), ((int)(((byte)(155)))));
                    appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
                    appearance4.FontData.BoldAsString = "True";
                    appearance4.FontData.SizeInPoints = 9F;
                    appearance4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(64)))), ((int)(((byte)(52)))));
                    appearance4.TextVAlignAsString = "Middle";
                    //this.ultraLabel1.Appearance = appearance4;

                    Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
                    appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
                    appearance3.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(167)))), ((int)(((byte)(155)))));
                    appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
                    appearance3.FontData.BoldAsString = "True";
                    appearance3.FontData.SizeInPoints = 9F;
                    appearance3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(64)))), ((int)(((byte)(52)))));
                    appearance3.TextVAlignAsString = "Middle";
                    this.ultraLabel2.Appearance = appearance3;

                    appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
                    appearance1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(167)))), ((int)(((byte)(155)))));
                    appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
                    appearance1.FontData.BoldAsString = "True";
                    appearance1.FontData.SizeInPoints = 9F;
                    appearance1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(64)))), ((int)(((byte)(52)))));
                    appearance1.TextVAlignAsString = "Middle";
                    //this.ultraLabel3.Appearance = appearance1;
                }

                if (!CustomThemeHelper.IsDesignMode())
                {
                    InitializeAccountsControl();
                    //CreateNodesList();
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

        //private void CreateNodesList()
        //{
        //    try
        //    {
        //        if (dtKids != null && dtKids.Rows.Count > 0)
        //            dtKids.Rows.Clear();
        //        dtKids = _dataSetMasterCategory.Tables["SubCashAccounts"].Copy();
        //        _dataSetMasterCategory.Tables["SubCashAccounts"].PrimaryKey = new DataColumn[] { _dataSetMasterCategory.Tables["SubCashAccounts"].Columns["SubAccountID"] };
        //        dtKids.PrimaryKey = new DataColumn[] { dtKids.Columns["SubAccountID"] };

        //        dtKids.AcceptChanges();
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

        /// <summary>
        /// Initialize the details in the control from the DB
        /// </summary>
        internal void InitializeAccountsControl()
        {
            try
            {
                // Fill DataSet From DB
                InitializeDataSet();
                _vlsubAccountType = CashAccountDataManager.GetAllSubAccountTypes();
                _transactionType = CashAccountDataManager.GetAllAccountTypes();
                _masterCategoryType = CashAccountDataManager.GetAllMasterTypes();

                //this line directly binds ultratree to dataset so no need of root node(_rootNode)
                accTree.Nodes.SetDataBinding(this._dataSetMasterCategory, CashManagementConstants.TABLE_MASTERCATEGORY);
                accTree.Nodes.Override.Sort = SortType.Ascending;
                accTree.ViewStyle = Infragistics.Win.UltraWinTree.ViewStyle.Standard;
                accTree.ColumnSettings.BorderStyleCell = UIElementBorderStyle.None;
                accTree.HideSelection = false;
                accTree.Override.ShowExpansionIndicator = Infragistics.Win.UltraWinTree.ShowExpansionIndicator.CheckOnDisplay;
                accTree.SynchronizeCurrencyManager = true;
                txtFilterAccount.Text = "";
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
        /// Bind the data to the grid
        /// </summary>
        /// <param name="row"></param>
        private void BindGrid(DataRow row)
        {
            try
            {
                if (row.Table.TableName == CashManagementConstants.TABLE_SUBACCOUNTS)
                {
                    grdData.DataSource = row.Table.DataSet;
                    grdData.DataMember = row.Table.TableName;
                    ValueList lsSubCategory = new ValueList();
                    lsSubCategory.ValueListItems.Add(int.MinValue, ApplicationConstants.C_COMBO_SELECT);
                    string subCatID = row[CashManagementConstants.COLUMN_SUBCATEGORYID].ToString();

                    DataRow[] result = row.Table.DataSet.Tables[CashManagementConstants.COLUMN_SUBCATEGORY].Select(CashManagementConstants.COLUMN_SUBCATEGORYID + " = " + subCatID + "");
                    DataRow[] List = row.Table.DataSet.Tables[CashManagementConstants.COLUMN_SUBCATEGORY].Select(CashManagementConstants.COLUMN_MASTERCATEGORYID + " = " + Convert.ToInt32(result[0].ItemArray[2].ToString()) + "");


                    foreach (DataRow newRow in List)
                    {
                        lsSubCategory.ValueListItems.Add(Convert.ToInt32(newRow.ItemArray[0]), newRow.ItemArray[1].ToString());
                    }

                    List<string> lsColumnsToDisplay = new List<string>(new string[] { CashManagementConstants.COLUMN_NAME, CashManagementConstants.COLUMN_ACRONYM, CashManagementConstants.COLUMN_SUBCATEGORYID, CashManagementConstants.COLUMN_ACCOUNTTYPEID, CashManagementConstants.COLUMN_SUBACCOUNTTYPEID });
                    UltraWinGridUtils.HideColumns(grdData.DisplayLayout.Bands[0]);
                    UltraWinGridUtils.SetBand(lsColumnsToDisplay, grdData.DisplayLayout.Bands[0]);

                    grdData.DisplayLayout.Bands[0].Columns[CashManagementConstants.COLUMN_SUBCATEGORYID].ValueList = lsSubCategory;
                    grdData.DisplayLayout.Bands[0].Columns[CashManagementConstants.COLUMN_SUBCATEGORYID].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                    grdData.DisplayLayout.Bands[0].Columns[CashManagementConstants.COLUMN_SUBCATEGORYID].Header.Caption = CashManagementConstants.CAPTION_SUBCATEGORY;

                    grdData.DisplayLayout.Bands[0].Columns[CashManagementConstants.COLUMN_ACCOUNTTYPEID].ValueList = _transactionType;
                    grdData.DisplayLayout.Bands[0].Columns[CashManagementConstants.COLUMN_ACCOUNTTYPEID].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                    grdData.DisplayLayout.Bands[0].Columns[CashManagementConstants.COLUMN_ACCOUNTTYPEID].Header.Caption = CashManagementConstants.CAPTION_ACCOUNTTYPE;

                    grdData.DisplayLayout.Bands[0].Columns[CashManagementConstants.COLUMN_SUBACCOUNTTYPEID].ValueList = _vlsubAccountType;
                    grdData.DisplayLayout.Bands[0].Columns[CashManagementConstants.COLUMN_SUBACCOUNTTYPEID].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                    grdData.DisplayLayout.Bands[0].Columns[CashManagementConstants.COLUMN_SUBACCOUNTTYPEID].Header.Caption = CashManagementConstants.CAPTION_SUBACCOUNTTYPE;


                    foreach (UltraGridRow gr in grdData.Rows)
                    {
                        if (((System.Data.DataRowView)(gr.ListObject)).Row != row)
                        {
                            gr.Band.Columns[CashManagementConstants.COLUMN_ACRONYM].CellActivation = Activation.AllowEdit;
                            gr.Hidden = true;
                        }
                        else
                        {
                            // gr.Band.Columns[CashManagementConstants.COLUMN_ACRONYM].CellActivation = Activation.AllowEdit;
                            gr.Hidden = false;
                            ValidateRow(gr.Cells[1]);
                            ValidateRow(gr.Cells[4]);
                        }
                    }
                }
                else if (row.Table.TableName == CashManagementConstants.COLUMN_SUBCATEGORY)
                {
                    grdData.DataSource = row.Table.DataSet;
                    grdData.DataMember = row.Table.TableName;

                    List<string> lsColumnsToDisplay = new List<string>(new string[] { CashManagementConstants.COLUMN_SUBCATEGORYNAME, CashManagementConstants.COLUMN_MASTERCATEGORYID });
                    UltraWinGridUtils.HideColumns(grdData.DisplayLayout.Bands[0]);
                    UltraWinGridUtils.SetBand(lsColumnsToDisplay, grdData.DisplayLayout.Bands[0]);

                    // grdData.DisplayLayout.Bands[0].Columns[CashManagementConstants.COLUMN_MASTERCATEGORYID].Hidden = true;
                    grdData.DisplayLayout.Bands[0].Columns[CashManagementConstants.COLUMN_SUBCATEGORYNAME].Header.Caption = CashManagementConstants.CAPTION_SUBCATEGORY;
                    grdData.DisplayLayout.Bands[0].Columns[CashManagementConstants.COLUMN_MASTERCATEGORYID].ValueList = _masterCategoryType;
                    grdData.DisplayLayout.Bands[0].Columns[CashManagementConstants.COLUMN_MASTERCATEGORYID].CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.CellSelect;
                    grdData.DisplayLayout.Bands[0].Columns[CashManagementConstants.COLUMN_MASTERCATEGORYID].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                    grdData.DisplayLayout.Bands[0].Columns[CashManagementConstants.COLUMN_MASTERCATEGORYID].Header.Caption = CashManagementConstants.CAPTION_MASTERCATEGORY;

                    foreach (UltraGridRow gr in grdData.Rows)
                    {
                        if (((System.Data.DataRowView)(gr.ListObject)).Row != row)
                        {
                            gr.Hidden = true;
                            gr.Band.Columns[CashManagementConstants.COLUMN_MASTERCATEGORYID].CellActivation = Activation.AllowEdit;
                        }
                        else
                        {
                            gr.Hidden = false;
                            ValidateRow(gr.Cells[1]);
                            //ValidateRow(gr.Cells[1]);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

            }

        }

        /// <summary>
        /// Initializes the data set.
        /// </summary>
        public void InitializeDataSet()
        {
            try
            {
                List<string> relationsToRemove = new List<string>();
                //Here must not be DB call, should be fetched from Cache
                _dataSetMasterCategory = WindsorContainerManager.GetAllAccountsWithRelation(WindsorContainerManager.GetAllAccountTablesFromDB());

                if (_dataSetMasterCategory.Tables.Contains(CashManagementConstants.TABLE_MASTERCATEGORY) && _dataSetMasterCategory.Tables.Contains(CashManagementConstants.TABLE_SUBCATEGORY) && _dataSetMasterCategory.Tables.Contains(CashManagementConstants.TABLE_ACCOUNTTYPE))
                {
                    foreach (DataRelation relation in _dataSetMasterCategory.Relations)
                    {
                        if (relation.RelationName != CashManagementConstants.RELATION_MASTERCATEGORYSUBCATEGORY && relation.RelationName != CashManagementConstants.RELATION_SUBCATEGORYSUBACCOUNTS)
                            relationsToRemove.Add(relation.RelationName);
                    }
                    if (relationsToRemove.Count > 0)
                        foreach (string relation in relationsToRemove)
                            _dataSetMasterCategory.Relations.Remove(relation);


                    if (_dataSetMasterCategory != null && _dataSetMasterCategory.Tables.Count > 0 && _dataSetMasterCategory.Tables[3].Rows.Count > 0)
                    {
                        _maxSubAccountID = Convert.ToInt32(_dataSetMasterCategory.Tables[3].AsEnumerable().Max(r => r.Field<int>("SUBACCOUNTID")));
                        _maxSubCategoryID = Convert.ToInt32(_dataSetMasterCategory.Tables[1].AsEnumerable().Max(r => r.Field<int>("SUBCATEGORYID")));
                        //_maxSubAccountID = Convert.ToInt32(_dataSetMasterCategory.Tables[3].Rows[_dataSetMasterCategory.Tables[3].Rows.Count - 1][CashManagementConstants.COLUMN_SUBACCOUNTID]);
                        //_maxSubCategoryID = Convert.ToInt32(_dataSetMasterCategory.Tables[1].Rows[_dataSetMasterCategory.Tables[1].Rows.Count - 1][CashManagementConstants.COLUMN_SUBCATEGORYID]);
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
        /// Handles the Click event of the mnuItemAddSubAccount control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void mnuItemAddSubAccount_Click(object sender, EventArgs e)
        {
            try
            {
                DataRow row = _dataSetMasterCategory.Tables[3].NewRow();

                row[CashManagementConstants.COLUMN_SUBACCOUNTID] = GetNewSubAccountID();
                row[CashManagementConstants.COLUMN_SUBCATEGORYID] = Convert.ToInt32(((System.Data.DataRowView)_mouseNode.ListObject).Row[CashManagementConstants.COLUMN_SUBCATEGORYID]);
                row[CashManagementConstants.COLUMN_NAME] = CashManagementConstants.COLUMN_SUBACCOUNT + _mouseNode.Nodes.Count.ToString();
                row[CashManagementConstants.COLUMN_ISFIXEDACCOUNT] = 0;
                // row["IsFixedAccount"] = false;
                _dataSetMasterCategory.Tables[3].Rows.Add(row);
                //dtKids.ImportRow(row);
                BindGrid(row);
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
        /// Handles the Click event of the mnuItemDeleteSubAccount control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void mnuItemDeleteSubAccount_Click(object sender, EventArgs e)
        {
            try
            {
                DataRow row = ((System.Data.DataRowView)(_mouseNode.ListObject)).Row;

                int subAccountID = Convert.ToInt32(row[CashManagementConstants.COLUMN_SUBACCOUNTID].ToString());
                //string acronym = row[CashManagementConstants.COLUMN_ACRONYM].ToString();

                if (CashAccountDataManager.IfSubAccountIsInUse(subAccountID))
                {
                    MessageBox.Show("SubAccount is in Use, Can not Delete !", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                //else if (IfSubAccountIsInMappingFile(acronym))
                //{
                //    MessageBox.Show("SubAccount is in SubAccountMapping.xml File, Can not Delete !", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //}
                else
                {
                    row.Delete();
                    //dtKids.Rows.Remove(row);
                    accTree.ActiveNode.Selected = true;
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
        /// Handles the Click event of the mnuItemDeleteAccount control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void mnuItemDeleteAccount_Click(object sender, EventArgs e)
        {
            try
            {
                DataRow row = ((System.Data.DataRowView)(_mouseNode.ListObject)).Row;
                row.Delete();
                accTree.ActiveNode.Selected = true;
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
        /// Handles the Click event of the mnuItemAddAccount control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void mnuItemAddAccount_Click(object sender, EventArgs e)
        {
            try
            {
                DataRow row = _dataSetMasterCategory.Tables[1].NewRow();

                row[CashManagementConstants.COLUMN_SUBCATEGORYID] = GetNewMasterFundID();
                row["MasterCategoryId"] = Convert.ToInt32(((System.Data.DataRowView)_mouseNode.ListObject).Row["MasterCategoryId"]);
                row["SubCategoryName"] = "SubCategory" + _mouseNode.Nodes.Count.ToString();
                // row["IsFixedAccount"] = false;
                _dataSetMasterCategory.Tables[1].Rows.Add(row);

                BindGrid(row);
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
        /// Determines whether [is repeated name] [the specified cell text].
        /// </summary>
        /// <param name="cellText">The cell text.</param>
        /// <param name="column">The column.</param>
        /// <param name="currentRow">The current row.</param>
        /// <returns>
        ///   <c>true</c> if [is repeated name] [the specified cell text]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsRepeatedName(string cellText, string column, DataRow currentRow)
        {
            bool isRepeated = false;
            try
            {


                foreach (DataRow row in currentRow.Table.Rows)
                {
                    if (row.RowState != DataRowState.Deleted)
                    {
                        if (row == currentRow) // Skip the current row check in other rows
                        {
                            continue;
                        }

                        if (string.Compare(row[column].ToString().Trim(), cellText.Trim(), true) == 0)
                        {
                            isRepeated = true;
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
            return isRepeated;
        }

        /// <summary>
        /// Gets the new sub account identifier.
        /// </summary>
        /// <returns></returns>
        private int GetNewSubAccountID()
        {
            return ++_maxSubAccountID;
        }

        /// <summary>
        /// Gets the new master fund identifier.
        /// </summary>
        /// <returns></returns>
        private int GetNewMasterFundID()
        {
            return ++_maxSubCategoryID;
        }

        /// <summary>
        /// Gets the data set.
        /// </summary>
        /// <returns></returns>
        internal DataSet GetDataSet()
        {
            grdData.UpdateData();
            //RestoreAccountTable();
            return _dataSetMasterCategory;
        }

        /// <summary>
        /// Sets the data set.
        /// </summary>
        /// <param name="dataSetMasterCategory">The data set master category.</param>
        internal void SetDataSet(DataSet dataSetMasterCategory)
        {
            _dataSetMasterCategory = dataSetMasterCategory;
        }

        /// <summary>
        /// Handles the ValueChanged event of the txtFilterAccount control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void txtFilterAccount_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                FilterAccountNodes(txtFilterAccount.Text.ToLower().Trim());
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
        /// Filters the account nodes.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        private void FilterAccountNodes(string searchText)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(searchText) && searchText != CashManagementConstants.DEFAULTACCOUNTSEARCHTEXT.ToLower())
                {
                    foreach (UltraTreeNode node in accTree.Nodes)
                    {
                        foreach (UltraTreeNode nd in node.Nodes)
                        {
                            HideNodes(nd, searchText);

                            if (nd.Nodes.All.Cast<UltraTreeNode>().ToList().Where(t => t.Visible == true).ToList().Count > 0)
                                nd.Visible = true;
                            else
                                nd.Visible = false;
                        }

                    }
                }
                else
                {
                    foreach (UltraTreeNode node in accTree.Nodes)
                    {
                        node.Nodes.All.Cast<UltraTreeNode>().ToList().Where(t => t.Visible == false).ToList().ForEach(t => t.Visible = true);
                        foreach (UltraTreeNode nd in node.Nodes)
                            nd.Nodes.All.Cast<UltraTreeNode>().ToList().Where(t => t.Visible == false).ToList().ForEach(t => t.Visible = true);
                    }
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
        }

        /// <summary>
        /// Hides the nodes.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="searchText">The search text.</param>
        private void HideNodes(UltraTreeNode node, string searchText)
        {
            node.Nodes.All.Cast<UltraTreeNode>().ToList().ForEach(t => t.Visible = false);
            node.Nodes.All.Cast<UltraTreeNode>().ToList().Where(t => t.Text.ToLower().Contains(searchText)).ToList().ForEach(n => n.Visible = true);
        }

        /// <summary>
        /// Gets the grid data to export to excel.
        /// </summary>
        /// <returns></returns>
        public Infragistics.Documents.Excel.Workbook GetGridDataToExportToExcel()
        {
            Infragistics.Documents.Excel.Workbook workBook = new Infragistics.Documents.Excel.Workbook();
            try
            {
                DataTable dataTable = WindsorContainerManager.getSubAccountsForExport().Tables[0];

                // Create the worksheet to represent this data table
                Infragistics.Documents.Excel.Worksheet worksheet = workBook.Worksheets.Add(dataTable.TableName);

                // Create column headers for each column
                for (int columnIndex = 0; columnIndex < dataTable.Columns.Count; columnIndex++)
                {
                    worksheet.Rows[0].Cells[columnIndex].Value = dataTable.Columns[columnIndex].ColumnName;
                }

                // Starting at row index 1, copy all data rows in
                // the data table to the worksheet
                int rowIndex = 1;
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    Infragistics.Documents.Excel.WorksheetRow row = worksheet.Rows[rowIndex++];

                    for (int columnIndex = 0; columnIndex < dataRow.ItemArray.Length; columnIndex++)
                    {
                        row.Cells[columnIndex].Value = dataRow.ItemArray[columnIndex];
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
            return workBook;
        }
    }
}
