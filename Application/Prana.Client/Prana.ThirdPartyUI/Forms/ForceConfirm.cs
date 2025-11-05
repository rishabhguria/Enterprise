using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.ThirdParty.Tables;
using Prana.ClientCommon;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.UI.ImportExportUtilities;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using static Prana.Global.ApplicationConstants;

namespace Prana.ThirdPartyUI.Forms
{
    public partial class ForceConfirm : Form
    {
        #region Column Titles
        private const string HEADCOL_CHECKBOX = "HEADERCHECK";
        private const string HEADCOL_USERID = "USERID";
        private const string HEADCOL_CONFIRMATIONDATETIME = "CONFIRMATIONDATETIME";
        private const string HEADCOL_SYMBOL = "SYMBOL";
        private const string HEADCOL_SIDE = "SIDE";
        private const string HEADCOL_QUANTITY = "QUANTITY";
        private const string HEADCOL_ALLOCATIONID = "ALLOCATIONID";
        private const string HEADCOL_THIRDPARTYBATCHID = "THIRDPARTYBATCHID";
        private const string HEADCOL_COMMENT = "COMMENT";
        private const string HEADCOL_BROKER = "BROKER";
        private const string HEADCOL_AVERAGEPX = "AVERAGEPX";
        private const string HEADCOL_TRADEDATE = "TRADEDATE";
        private const string HEADCOL_MATCHSTATUS = "MATCHSTATUS";
        private const string HEADCOL_ACCOUNT = "ACCOUNT";
        private const string HEADCOL_COMMISSION = "COMMISSION";
        private const string HEADCOL_MISCFEES = "MISCFEES";
        private const string HEADCOL_NETMONEY = "NETMONEY";
        private const string HEADCOL_USERNAME = "USERNAME";
        #endregion

        private ThirdPartyBatch _incomingBatch;
        private bool _isCommentBoxCleared = false;
        private HashSet<int> _validRowIds;

        /// <summary>
        /// Initializes DataSource of GrdForceConfirm
        /// </summary>
        /// <param name="data"></param>
        public void InitializeDataSource(List<ThirdPartyForceConfirm> data)
        {
            try
            {
                GrdForceConfirm.DataSource = null;
                GrdForceConfirm.DataSource = data;
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

        public ForceConfirm(ThirdPartyBatch thirdPartyBatch)
        {
            InitializeComponent();
            _incomingBatch = thirdPartyBatch;
        }

        /// <summary>
        /// Handles Load event of ForceConfirm
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ForceConfirm_Load(object sender, EventArgs e)
        {
            try
            {
                _validRowIds = new HashSet<int>();
                CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_PRANA_TRADING_TICKET);
                this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, "Force Match", CustomThemeHelper.UsedFont);
                CustomThemeHelper.SetThemeProperties(GrdForceConfirm as UltraGrid, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLE_NAME_THIRD_PARTY_CUSTOM);
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
        /// Handles InitializeLayout event of GrdForceConfirm
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GrdForceConfirm_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                UltraGridBand band = GrdForceConfirm.DisplayLayout.Bands[0];

                band.Override.AllowColMoving = AllowColMoving.Default;
                GrdForceConfirm.DisplayLayout.Override.AllowColMoving = AllowColMoving.Default;
                GrdForceConfirm.DisplayLayout.GroupByBox.Hidden = true;
                GrdForceConfirm.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
                GrdForceConfirm.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;

                band.Columns.Add(HEADCOL_CHECKBOX, "");
                band.Columns[HEADCOL_CHECKBOX].DataType = typeof(bool);
                band.Columns[HEADCOL_CHECKBOX].CellClickAction = CellClickAction.EditAndSelectText;
                band.Columns[HEADCOL_CHECKBOX].Hidden = false;
                band.Columns[HEADCOL_CHECKBOX].Header.VisiblePosition = 0;
                band.Columns[HEADCOL_CHECKBOX].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
                band.Columns[HEADCOL_CHECKBOX].SortIndicator = SortIndicator.Disabled;
                band.Columns[HEADCOL_CHECKBOX].Header.CheckBoxVisibility = HeaderCheckBoxVisibility.WhenUsingCheckEditor;
                band.Columns[HEADCOL_CHECKBOX].Header.CheckBoxAlignment = HeaderCheckBoxAlignment.Center;
                band.Columns[HEADCOL_CHECKBOX].Header.CheckBoxSynchronization = HeaderCheckBoxSynchronization.RowsCollection;
                band.Columns[HEADCOL_CHECKBOX].SetHeaderCheckedState(GrdForceConfirm.Rows, true);

                band.Columns[HEADCOL_ACCOUNT].Header.Caption = "Account";
                band.Columns[HEADCOL_ACCOUNT].Header.VisiblePosition = 1;
                band.Columns[HEADCOL_ACCOUNT].CellActivation = Activation.NoEdit;

                band.Columns[HEADCOL_SYMBOL].Header.Caption = "Symbol";
                band.Columns[HEADCOL_SYMBOL].Header.VisiblePosition = 2;
                band.Columns[HEADCOL_SYMBOL].CellActivation = Activation.NoEdit;

                band.Columns[HEADCOL_SIDE].Header.Caption = "Side";
                band.Columns[HEADCOL_SIDE].Header.VisiblePosition = 3;
                band.Columns[HEADCOL_SIDE].CellActivation = Activation.NoEdit;

                band.Columns[HEADCOL_QUANTITY].Header.Caption = "Qty";
                band.Columns[HEADCOL_QUANTITY].Header.VisiblePosition = 4;
                band.Columns[HEADCOL_QUANTITY].CellActivation = Activation.NoEdit;

                band.Columns[HEADCOL_BROKER].Header.Caption = "Broker";
                band.Columns[HEADCOL_BROKER].Header.VisiblePosition = 5;
                band.Columns[HEADCOL_BROKER].CellActivation = Activation.NoEdit;

                band.Columns[HEADCOL_AVERAGEPX].Header.Caption = "Avg PX";
                band.Columns[HEADCOL_AVERAGEPX].Header.VisiblePosition = 6;
                band.Columns[HEADCOL_AVERAGEPX].CellActivation = Activation.NoEdit;

                band.Columns[HEADCOL_COMMISSION].Header.Caption = "Commission";
                band.Columns[HEADCOL_COMMISSION].Header.VisiblePosition = 7;
                band.Columns[HEADCOL_COMMISSION].CellActivation = Activation.NoEdit;

                band.Columns[HEADCOL_MISCFEES].Header.Caption = "Misc Fees";
                band.Columns[HEADCOL_MISCFEES].Header.VisiblePosition = 8;
                band.Columns[HEADCOL_MISCFEES].CellActivation = Activation.NoEdit;

                band.Columns[HEADCOL_NETMONEY].Header.Caption = "Net Money";
                band.Columns[HEADCOL_NETMONEY].Header.VisiblePosition = 9;
                band.Columns[HEADCOL_NETMONEY].CellActivation = Activation.NoEdit;

                band.Columns[HEADCOL_TRADEDATE].Header.Caption = "Trade Date";
                band.Columns[HEADCOL_TRADEDATE].Header.VisiblePosition = 10;
                band.Columns[HEADCOL_TRADEDATE].CellActivation = Activation.NoEdit;

                band.Columns[HEADCOL_ALLOCATIONID].Header.Caption = "Alloc ID";
                band.Columns[HEADCOL_ALLOCATIONID].Header.VisiblePosition = 11;
                band.Columns[HEADCOL_ALLOCATIONID].CellActivation = Activation.NoEdit;

                band.Columns[HEADCOL_MATCHSTATUS].Header.Caption = "Status";
                band.Columns[HEADCOL_MATCHSTATUS].Header.VisiblePosition = 12;
                band.Columns[HEADCOL_MATCHSTATUS].CellActivation = Activation.NoEdit;

                band.Columns[HEADCOL_COMMENT].Header.Caption = "Comment";
                band.Columns[HEADCOL_COMMENT].Header.VisiblePosition = 13;
                band.Columns[HEADCOL_COMMENT].CellActivation = Activation.NoEdit;

                band.Columns[HEADCOL_USERID].Hidden = true;
                band.Columns[HEADCOL_CONFIRMATIONDATETIME].Hidden = true;
                band.Columns[HEADCOL_THIRDPARTYBATCHID].Hidden = true;
                band.Columns[HEADCOL_USERNAME].Hidden = true;

                UltraGridLayout layout = e.Layout;
                layout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
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
        /// Handles Click event of btnSubmitAndDownload
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSubmitAndDownload_Click(object sender, EventArgs e)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke((MethodInvoker)(delegate ()
                        {
                            btnSubmitAndDownload_Click(sender, e);
                        }));
                    }
                    else
                    {
                        UltraGridRow[] rows = GrdForceConfirm.Rows.GetFilteredInNonGroupByRows();
                        List<ThirdPartyForceConfirm> dataSaveList = new List<ThirdPartyForceConfirm>();
                        List<string> allocIds = new List<string>();
                        Dictionary<string, string> allocIdAllocReportIdPairs = new Dictionary<string, string>();
                        foreach (UltraGridRow row in rows)
                        {
                            ThirdPartyForceConfirm data = row.ListObject as ThirdPartyForceConfirm;
                            data.UserID = CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
                            data.ThirdPartyBatchID = _incomingBatch.ThirdPartyBatchId;
                            if (data.MsgType == FIXConstants.MSGAllocationReport)
                            {
                                allocIdAllocReportIdPairs[data.AllocId] = data.AllocReportId;
                            }
                            else
                            {
                                if (!allocIds.Contains(data.AllocId))
                                    allocIds.Add(data.AllocId);
                            }
                            dataSaveList.Add(data);
                        }
                        if (ThirdPartyClientManager.ServiceInnerChannel.SaveThirdPartyForceConfirmAuditData(dataSaveList, _incomingBatch) > 0)
                        {
                            foreach (UltraGridRow row in rows)
                            {
                                ThirdPartyForceConfirm data = row.ListObject as ThirdPartyForceConfirm;
                                data.MatchStatus = EnumHelper.GetDescriptionWithDescriptionAttribute(ApplicationConstants.BlockMatchStatus.Accepted);
                                row.Refresh();
                            }
                            string downloadsFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
                            string fileName = _incomingBatch.Description + "_forcematchdata_" + DateTime.Now.ToString("yyyyMMddhhmmssff");
                            string filePath = Path.Combine(downloadsFolderPath, fileName);
                            UltraGridFileExporter.ExportFile(this.GrdForceConfirm, filePath, AutomationEnum.FileFormat.csv);
                            Close();
                        }
                        ThirdPartyClientManager.ServiceInnerChannel.SendAUMsg(allocIds, _incomingBatch.CounterPartyID, (int)AffirmStatus.Affirmed);
                        ThirdPartyClientManager.ServiceInnerChannel.SendATMsg(allocIdAllocReportIdPairs, _incomingBatch.CounterPartyID, (int)BlockMatchStatus.Accepted);
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
        /// Handles KeyPressed event of commentBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void commentBox_KeyPressed(object sender, KeyPressEventArgs e)
        {
            try
            {
                _isCommentBoxCleared = false;
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
        /// Handles commentBox_TextChanged event of commentBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void commentBox_TextChanged(object sender, System.EventArgs e)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke((MethodInvoker)(delegate ()
                        {
                            commentBox_TextChanged(sender, e);
                        }));
                    }
                    else
                    {
                        UltraGridRow[] rows = GrdForceConfirm.Rows.GetFilteredInNonGroupByRows();
                        if (!_isCommentBoxCleared)
                        {
                            foreach (UltraGridRow row in rows)
                            {
                                if (row.Cells[HEADCOL_CHECKBOX].Text.Equals("True"))
                                {
                                    UpdateCommentAndRowColours(row, commentBox.Text.Length >= 5);
                                }
                            }
                        }
                        btnSubmitAndDownload.Enabled = _validRowIds.Count == GrdForceConfirm.Rows.Count;
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
        /// Updates comments and colours for selected rows
        /// </summary>
        /// <param name="row"></param>
        /// <param name="isUpdateNeeded"></param>
        private void UpdateCommentAndRowColours(UltraGridRow row, bool isUpdateNeeded)
        {
            try
            {
                if (isUpdateNeeded)
                {
                    row.Cells[HEADCOL_COMMENT].Value = commentBox.Text;
                    row.Appearance.ForeColor = System.Drawing.Color.White;
                    row.Appearance.BackColor = System.Drawing.Color.Green;
                    _validRowIds.Add(row.Index);
                }
                else
                {
                    row.Cells[HEADCOL_COMMENT].Value = string.Empty;
                    row.Appearance.ResetForeColor();
                    row.Appearance.ResetBackColor();
                    _validRowIds.Remove(row.Index);
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
        /// Handles ClickCell event of GrdForceConfirm
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GrdForceConfirm_ClickCell(object sender, ClickCellEventArgs e)
        {
            try
            {
                string HeaderText = e.Cell.Column.Header.Caption;
                if (HeaderText == "")
                {
                    _isCommentBoxCleared = true;
                    commentBox.Clear();
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
        /// Handles Click event of btnClose
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                Close();
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
    }
}