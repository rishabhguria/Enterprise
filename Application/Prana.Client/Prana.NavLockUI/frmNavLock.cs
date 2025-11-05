using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.TradeManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Prana.NavLockUI
{
    /// <summary>
    /// Form to Add or delete NAVLocks in system
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Form" />
    /// <seealso cref="Prana.Interfaces.INAVLockUI" />
    public partial class frmNavLock : Form, INAVLockUI
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="frmNavLock"/> class.
        /// </summary>
        public frmNavLock()
        {
            InitializeComponent();
            this.Load += FrmNavLock_Load;
            this.FormClosed += FrmNavLock_FormClosed;
        }
        /// <summary>
        /// The nav lock datas
        /// </summary>
        private List<NavLockData> navLockDatas = new List<NavLockData>();
        /// <summary>
        /// Gets or sets the login user.
        /// </summary>
        /// <value>
        /// The login user.
        /// </value>
        public CompanyUser LoginUser { get; set; }

        /// <summary>
        /// Occurs when [form closed handler].
        /// </summary>
        public event EventHandler FormClosedHandler;

        /// <summary>
        /// Handles the FormClosed event of the FrmNavLock control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.FormClosedEventArgs"/> instance containing the event data.</param>
        private void FrmNavLock_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            if (FormClosedHandler != null)
                FormClosedHandler(this, e);
        }

        /// <summary>
        /// Handles the Load event of the FrmNavLock control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void FrmNavLock_Load(object sender, System.EventArgs e)
        {
            try
            {
                if (CustomThemeHelper.ApplyTheme)
                {
                    CustomThemeHelper.SetThemeProperties(sender as Form, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_PRANA_SHORTCUTS);
                    CustomThemeHelper.SetThemeProperties(pranaUltraGrid1 as UltraGrid, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLE_NAME_THIRD_PARTY_CUSTOM);
                    this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                    this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, "NAV Lock", CustomThemeHelper.UsedFont);
                }
                dtLockDate.ValueChanged += DtLockDate_ValueChanged;
                navLockDatas = NavLockDataManager.GetNavLockDates();
                pranaUltraGrid1.InitializeRow += PranaUltraGrid1_InitializeRow;
                pranaUltraGrid1.ClickCellButton += PranaUltraGrid1_ClickCellButton;
                btnAddLock.BackColor = Color.FromArgb(72, 99, 160);
                btnAddLock.ForeColor = Color.White;
                btnAddLock.ButtonStyle = UIElementButtonStyle.Button3D;
                btnAddLock.UseAppStyling = false;
                btnAddLock.UseOsThemes = DefaultableBoolean.False;
                EnableDisableAddLock();
                InitializeGridLayout();
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
        /// Initializes the grid layout.
        /// </summary>
        private void InitializeGridLayout()
        {
            try
            {
                pranaUltraGrid1.DataSource = null;
                pranaUltraGrid1.DataSource = navLockDatas;
                pranaUltraGrid1.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
                pranaUltraGrid1.DisplayLayout.Bands[0].Columns["LockId"].Hidden = true;
                pranaUltraGrid1.DisplayLayout.Bands[0].Columns["LockedById"].Hidden = true;
                pranaUltraGrid1.DisplayLayout.Bands[0].Columns["LockDate"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
                pranaUltraGrid1.DisplayLayout.Bands[0].Columns["LockedByName"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
                pranaUltraGrid1.DisplayLayout.Bands[0].Columns["LockCreationDate"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
                pranaUltraGrid1.DisplayLayout.Bands[0].Columns["LockDate"].Header.Caption = "Lock Date";
                pranaUltraGrid1.DisplayLayout.Bands[0].Columns["LockedByName"].Header.Caption = "Locked By";
                pranaUltraGrid1.DisplayLayout.Bands[0].Columns["LockCreationDate"].Header.Caption = "Lock Creation Date";
                Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn = pranaUltraGrid1.DisplayLayout.Bands[0].Columns.Add("Delete");
                ultraGridColumn.Header.Caption = String.Empty;
                ultraGridColumn.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                ultraGridColumn.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
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
        /// Handles the ValueChanged event of the DtLockDate control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void DtLockDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                EnableDisableAddLock();
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
        /// Enables the disable add lock.
        /// </summary>
        private void EnableDisableAddLock()
        {
            try
            {
                if(dtLockDate.Value != null)
                {
                    DateTime dt = (DateTime)dtLockDate.Value;
                    if (navLockDatas != null && navLockDatas.Count > 0)
                    {
                        DateTime lockDate = Convert.ToDateTime(navLockDatas[0].LockDate);
                        if (dt <= lockDate)
                            btnAddLock.Enabled = false;
                        else
                            btnAddLock.Enabled = true;
                    }
                    else
                    {
                        btnAddLock.Enabled = true;
                    }
                }
                else
                {
                    btnAddLock.Enabled = false;
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
        /// Handles the ClickCellButton event of the PranaUltraGrid1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Infragistics.Win.UltraWinGrid.CellEventArgs"/> instance containing the event data.</param>
        private void PranaUltraGrid1_ClickCellButton(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            try
            {
                NavLockData lockData = (NavLockData)e.Cell.Row.ListObject;
                PromptWindow promptWin = new PromptWindow("This action will delete the lock on " + lockData.LockDate + ". Please confirm before proceeding", "Nirvana");
                promptWin.BtnContinue.Text = "Delete";
                promptWin.BtnEdit.Text = "Cancel";
                promptWin.ShowDialog();
                if (promptWin.ShouldTrade)
                {
                    NavLockDataManager.DeleteNavLockDate(lockData.LockId, LoginUser.CompanyUserID);
                    navLockDatas.Remove(lockData);
                    InitializeGridLayout();
                    EnableDisableAddLock();
                    string lockDate = navLockDatas.Count > 0 ? navLockDatas[0].LockDate : string.Empty;
                    TradeManager.TradeManager.GetInstance().SendNAVLockDateUpdate(lockDate);
                }
                promptWin.Close();
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
        /// Handles the InitializeRow event of the PranaUltraGrid1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="InitializeRowEventArgs"/> instance containing the event data.</param>
        private void PranaUltraGrid1_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                if (e.Row.Index == 0)
                {
                    e.Row.Cells["Delete"].Value = "Delete";
                    e.Row.Cells["Delete"].ButtonAppearance.BackGradientStyle = GradientStyle.None;
                    e.Row.Cells["Delete"].ButtonAppearance.BackColor = Color.FromArgb(140, 5, 5);
                    e.Row.Cells["Delete"].ButtonAppearance.ForeColor = Color.White;
                }
                else
                {
                    e.Row.Cells["Delete"].Activation = Activation.Disabled;
                    e.Row.Cells["Delete"].Hidden = true;
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
        /// References this instance.
        /// </summary>
        /// <returns></returns>
        public Form Reference()
        {
            return this;
        }

        /// <summary>
        /// Handles the Click event of the btnAddLock control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnAddLock_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime lockDate = (DateTime)dtLockDate.Value;
                PromptWindow promptWin = new PromptWindow("This action will lock all modifications before " + lockDate.ToShortDateString() + ". Please confirm before proceeding", "Nirvana");
                promptWin.BtnContinue.Text = "Confirm";
                promptWin.BtnEdit.Text = "Cancel";
                promptWin.ShowDialog();
                if (promptWin.ShouldTrade)
                {
                    DateTime lockCreationDate = DateTime.Now;
                    int id = NavLockDataManager.AddNavLockDate(lockDate, LoginUser.CompanyUserID, lockCreationDate);
                    if (id >= 0)
                    {
                        NavLockData lockData = new NavLockData
                        {
                            LockId = id,
                            LockDate = lockDate.ToShortDateString(),
                            LockCreationDate = lockCreationDate.ToString(),
                            LockedById = LoginUser.CompanyUserID,
                            LockedByName = LoginUser.ShortName
                        };
                        navLockDatas.Insert(0, lockData);
                        InitializeGridLayout();
                        EnableDisableAddLock();
                        CommonDataCache.CachedDataManager.GetInstance.NAVLockDate = lockDate;
                        TradeManager.TradeManager.GetInstance().SendNAVLockDateUpdate(lockDate.ToShortDateString());
                    }
                }
                promptWin.Close();
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
