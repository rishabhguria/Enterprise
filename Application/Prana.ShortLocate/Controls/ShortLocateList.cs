using Infragistics.Win.UltraWinEditors;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.CommonDataCache;
using Prana.LogManager;
using Prana.ShortLocate.Classes;
using Prana.ShortLocate.Preferences;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Prana.ShortLocate.Controls
{
    public partial class ShortLocateList : UserControl
    {
        public ShortLocateList()
        {
            InitializeComponent();
            if (CustomThemeHelper.ApplyTheme)
            {
                CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_DAILY_PM_CLIENTUI);
            }
        }

        public void BindData(BindingList<ShortLocateListParameter> ShortLocateData, bool addCheck = false)
        {
            try
            {
                if (addCheck)
                {
                    grdShortLocateList.DataSource = ShortLocateData;
                    foreach (UltraGridRow row in grdShortLocateList.Rows)
                    {
                        row.Activation = Activation.NoEdit;
                        row.Cells["BorrowQuantity"].Activation = Activation.NoEdit;
                    }
                }
                else
                    grdShortLocateList.DataSource = ShortLocateData;

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


        private ShortLocateUIPreferences _shortLocatePreferences;
        private ctrlShortLocatePrefDataManager Dataobj = new ctrlShortLocatePrefDataManager();

        private void grdShortLocateList_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            _shortLocatePreferences = Dataobj.GetShortLocatePreferences(CachedDataManager.GetInstance.LoggedInUser.CompanyUserID);
            try
            {
                e.Layout.Bands[0].Columns[ShortLocateConstants.COL_Broker].Header.Caption = "Borrow Broker";
                e.Layout.Bands[0].Columns[ShortLocateConstants.COL_Broker].Header.VisiblePosition = 2;
                e.Layout.Bands[0].Columns[ShortLocateConstants.COL_Broker].Width = 100;
                e.Layout.Bands[0].Columns[ShortLocateConstants.COL_Broker].CellActivation = Activation.AllowEdit;
                e.Layout.Bands[0].Columns[ShortLocateConstants.COL_Broker].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                e.Layout.Bands[0].Columns[ShortLocateConstants.COL_Broker].AutoCompleteMode = Infragistics.Win.AutoCompleteMode.SuggestAppend;

                e.Layout.Bands[0].Columns[ShortLocateConstants.COL_BorrowSharesAvailable].Header.Caption = "Share Available";
                e.Layout.Bands[0].Columns[ShortLocateConstants.COL_BorrowSharesAvailable].Header.VisiblePosition = 3;
                e.Layout.Bands[0].Columns[ShortLocateConstants.COL_BorrowSharesAvailable].Width = 105;
                e.Layout.Bands[0].Columns[ShortLocateConstants.COL_BorrowSharesAvailable].CellActivation = Activation.AllowEdit;
                e.Layout.Bands[0].Columns[ShortLocateConstants.COL_BorrowSharesAvailable].Format = "#,####0.###########";
                e.Layout.Bands[0].Columns[ShortLocateConstants.COL_BorrowSharesAvailable].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;

                e.Layout.Bands[0].Columns[ShortLocateConstants.COL_BorrowQuantity].Header.Caption = "Quantity";
                e.Layout.Bands[0].Columns[ShortLocateConstants.COL_BorrowQuantity].Header.VisiblePosition = 4;
                e.Layout.Bands[0].Columns[ShortLocateConstants.COL_BorrowQuantity].Width = 115;
                e.Layout.Bands[0].Columns[ShortLocateConstants.COL_BorrowQuantity].CellActivation = Activation.AllowEdit;
                e.Layout.Bands[0].Columns[ShortLocateConstants.COL_BorrowQuantity].Format = "#,####0";
                e.Layout.Bands[0].Columns[ShortLocateConstants.COL_BorrowQuantity].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                if (_shortLocatePreferences.Rebatefees == ShortLocateRebateFee.BPS.ToString())
                    e.Layout.Bands[0].Columns[ShortLocateConstants.COL_BorrowRate].Header.Caption = "Rate(BPS)";
                else
                    e.Layout.Bands[0].Columns[ShortLocateConstants.COL_BorrowRate].Header.Caption = "Rate(%)";
                e.Layout.Bands[0].Columns[ShortLocateConstants.COL_BorrowRate].Header.VisiblePosition = 5;
                e.Layout.Bands[0].Columns[ShortLocateConstants.COL_BorrowRate].Width = 90;
                e.Layout.Bands[0].Columns[ShortLocateConstants.COL_BorrowRate].CellActivation = Activation.AllowEdit;
                e.Layout.Bands[0].Columns[ShortLocateConstants.COL_BorrowRate].Format = "#,####0.###########";
                e.Layout.Bands[0].Columns[ShortLocateConstants.COL_BorrowRate].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;

                e.Layout.Bands[0].Columns[ShortLocateConstants.COL_BorrowerId].Header.Caption = "Id";
                e.Layout.Bands[0].Columns[ShortLocateConstants.COL_BorrowerId].Header.VisiblePosition = 6;
                e.Layout.Bands[0].Columns[ShortLocateConstants.COL_BorrowerId].Width = 95;
                e.Layout.Bands[0].Columns[ShortLocateConstants.COL_BorrowerId].CellActivation = Activation.AllowEdit;
                e.Layout.Bands[0].Columns[ShortLocateConstants.COL_BorrowerId].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;

                e.Layout.Bands[0].Columns[ShortLocateConstants.COL_ReplaceQuantity].Hidden = true;

                e.Layout.Bands[0].Columns[ShortLocateConstants.COL_NirvanaLocateID].Hidden = true;
                e.Layout.Override.AllowAddNew = AllowAddNew.TemplateOnTopWithTabRepeat;
                e.Layout.Override.TemplateAddRowAppearance.BackColor = Color.FromArgb(245, 250, 255);
                e.Layout.Override.TemplateAddRowAppearance.ForeColor = SystemColors.GrayText;
                e.Layout.Override.AddRowAppearance.BackColor = Color.LightYellow;
                e.Layout.Override.AddRowAppearance.ForeColor = Color.Blue;
                e.Layout.Override.SpecialRowSeparator = SpecialRowSeparator.TemplateAddRow;
                e.Layout.Override.SpecialRowSeparatorAppearance.BackColor = SystemColors.Control;
                e.Layout.Override.TemplateAddRowPrompt = "Click here to add a new row...";
                e.Layout.Override.TemplateAddRowPromptAppearance.ForeColor = Color.Maroon;
                e.Layout.Override.TemplateAddRowPromptAppearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;
                e.Layout.ScrollStyle = ScrollStyle.Immediate;
                e.Layout.ScrollBounds = ScrollBounds.ScrollToFill;
                BindComboEditorToBroker();
                foreach (UltraGridRow row in grdShortLocateList.Rows)
                {
                    row.Activation = Activation.NoEdit;
                    row.Cells["BorrowQuantity"].IgnoreRowColActivation = true;
                    row.Cells["BorrowQuantity"].Activation = Activation.AllowEdit;
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
        /// Bind all brokers on shortlocate
        /// </summary>
        private void BindComboEditorToBroker()
        {
            try
            {
                Dictionary<int, string> dt1 = CommonDataCache.CachedDataManager.GetInstance.GetAllThirdPartiesWithShortName();
                UltraComboEditor uc = new UltraComboEditor();
                uc.BindingContext = this.BindingContext;
                uc.DataSource = new BindingSource(dt1, null);
                uc.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.SuggestAppend;
                uc.DropDownStyle = Infragistics.Win.DropDownStyle.DropDown;
                uc.AutoSuggestFilterMode = Infragistics.Win.AutoSuggestFilterMode.StartsWith;
                uc.DisplayMember = "Value";
                uc.ValueMember = "Value";
                grdShortLocateList.DisplayLayout.Bands[0].Columns[ShortLocateConstants.COL_Broker].EditorComponent = uc;
                grdShortLocateList.DisplayLayout.Override.SupportDataErrorInfo = SupportDataErrorInfo.RowsAndCells;
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
        /// GrdShortLocateList AfterCellUpdate
        /// </summary>Grd ShortLocateList  AfterCellUpdate
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GrdShortLocateList_AfterCellUpdate(object sender, CellEventArgs e)
        {
            try
            {
                if (e.Cell.Column.Key.Equals(ShortLocateConstants.COL_BorrowQuantity) || e.Cell.Column.Key.Equals(ShortLocateConstants.COL_Broker))
                {
                    BindingList<ShortLocateListParameter> shortLocateListRows = (BindingList<ShortLocateListParameter>)grdShortLocateList.DataSource;
                    var list = shortLocateListRows.Where(x => x.BorrowQuantity != 0).ToList();
                    if (list.Count == 1)
                    {
                        foreach (UltraGridRow row1 in grdShortLocateList.Rows)
                        {
                            UltraGridCell cell1 = row1.Cells[ShortLocateConstants.COL_BorrowQuantity];
                            double value;
                            if (double.TryParse(cell1.Text, out value) && value == 0)
                            {
                                row1.Cells["BorrowQuantity"].IgnoreRowColActivation = false;
                                row1.Activation = Activation.Disabled;

                            }
                        }
                    }
                    else if (list.Count > 1)
                    {
                        e.Cell.Value = 0;
                        e.Cell.Row.Activation = Activation.Disabled;
                    }
                    else
                    {
                        if (!e.Cell.Column.Key.Equals(ShortLocateConstants.COL_Broker))
                        {
                            foreach (UltraGridRow row in grdShortLocateList.Rows)
                            {
                                row.Activation = Activation.NoEdit;
                                row.Cells["BorrowQuantity"].IgnoreRowColActivation = true;
                                row.Cells["BorrowQuantity"].Activation = Activation.AllowEdit;
                            }
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

        /// <summary>
        /// GrdShortLocateList CellChange
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GrdShortLocateList_CellChange(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            try
            {
                UltraGridCell cell = e.Cell;
                if (cell.Column.Key.Equals(ShortLocateConstants.COL_Broker))
                {
                    UltraComboEditor Uc = (UltraComboEditor)e.Cell.Column.EditorComponent;
                    CheckIfErrorExists(Uc, cell);
                }
                double doubleValue;
                // Check if the cell value is DBNull or null
                if (cell.Column.Key.Equals(ShortLocateConstants.COL_BorrowQuantity) || cell.Column.Key.Equals(ShortLocateConstants.COL_BorrowSharesAvailable) || cell.Column.Key.Equals(ShortLocateConstants.COL_BorrowRate))
                {
                    if (!double.TryParse(cell.Text, out doubleValue) || string.IsNullOrEmpty(cell.Text) || cell.Value == DBNull.Value || cell.Value == null)
                    {
                        cell.Value = 0;
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
        /// Check If ErrorExists
        /// </summary>
        /// <param name="ultraComboEditor"></param>
        /// <param name="cell"></param>
        /// <returns></returns>
        private bool CheckIfErrorExists(UltraComboEditor ultraComboEditor, UltraGridCell cell)
        {
            bool isValidated = true;
            try
            {
                if (ultraComboEditor != null && !string.IsNullOrEmpty(cell.Text.ToString()))
                {
                    if (!Utilities.UI.ExtensionUtilities.ValueListUtilities.CheckIfValueExistsInValuelist(ultraComboEditor.ValueList, cell.Text.ToString()))
                    {
                        grdShortLocateList.Rows[cell.Row.Index].DataErrorInfo.SetColumnError(ShortLocateConstants.COL_Broker, "Value for this field is incorrect");
                        isValidated = false;
                    }
                    else
                    {
                        grdShortLocateList.Rows[cell.Row.Index].DataErrorInfo.SetColumnError(ShortLocateConstants.COL_Broker, string.Empty);
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
            return isValidated;
        }

        public ShortLocateListParameter GetSelectedRow()
        {
            try
            {

                BindingList<ShortLocateListParameter> shortLocateListRows = (BindingList<ShortLocateListParameter>)grdShortLocateList.DataSource;
                var list = shortLocateListRows.Where(x => x.BorrowQuantity != 0).ToList();
                Dictionary<int, string> dictBrokers = CommonDataCache.CachedDataManager.GetInstance.GetAllThirdPartiesWithShortName();
                if (list.Count == 1 && !dictBrokers.ContainsValue(list[0].Broker))
                {
                    MessageBox.Show(this, "Enter a valid broker", "ShortLocate List", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return null;
                }
                if (list.Count == 1)
                {
                    return list[0];
                }
                else if (list.Count > 1)
                {
                    MessageBox.Show("Multi Borrow Broker Selection not allowed!", "ShortLocate List", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Please Enter Quantity!", "ShortLocate List", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            return null;
        }

        public ShortLocateListParameter GetShortLocateParameter()
        {
            try
            {
                BindingList<ShortLocateListParameter> shortLocateListRows = (BindingList<ShortLocateListParameter>)grdShortLocateList.DataSource;
                if (shortLocateListRows == null)
                    return null;
                return shortLocateListRows.FirstOrDefault();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }
    }
}
