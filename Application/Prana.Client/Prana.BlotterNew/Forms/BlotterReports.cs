using ExportGridsData;
using Prana.BusinessObjects;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.ComponentModel;
using System.Windows.Forms;


namespace Prana.Blotter
{
    /// <summary>
    /// Summary description for BlotterReports.
    /// </summary>
    public class BlotterReports : System.Windows.Forms.Form, Prana.Interfaces.IBlotterReports, IExportGridData
    {
        private CompanyUser _loginUser;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl1;
        private ExecutionReport executionReport2;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl ultraTabControl1;
        private Infragistics.Win.Misc.UltraButton ultraButton1;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _BlotterReports_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _BlotterReports_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _BlotterReports_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _BlotterReports_UltraFormManager_Dock_Area_Bottom;
        private IContainer components;

        public BlotterReports()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            SetupSnapshotControl();
            if (!string.IsNullOrEmpty(CustomThemeHelper.WHITELABELTHEME) && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
            {
                SetButtonsColor();
            }
            InitControl();
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        private void SetButtonsColor()
        {
            try
            {
                ultraButton1.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                ultraButton1.ForeColor = System.Drawing.Color.White;
                ultraButton1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                ultraButton1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                ultraButton1.UseAppStyling = false;
                ultraButton1.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

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

        private void InitControl()
        {
            CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_BLOTTER_NEW);
            this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
            this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, this.Text, CustomThemeHelper.UsedFont);
            InstanceManager.RegisterInstance(this);
        }
        public Prana.BusinessObjects.CompanyUser LoginUser
        {
            get { return _loginUser; }
            set
            {
                _loginUser = value;
                executionReport2.LoginUser = _loginUser;
            }
        }
        public Form Reference()
        {
            return this;
        }
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if (ultraTabControl1 != null)
                {
                    ultraTabControl1.Dispose();
                }
                if (executionReport2 != null)
                {
                    executionReport2.Dispose();
                }
                if (ultraTabPageControl1 != null)
                {
                    ultraTabPageControl1.Dispose();
                }
                if (ultraFormManager1 != null)
                {
                    ultraFormManager1.Dispose();
                }
                if (ultraTabSharedControlsPage1 != null)
                {
                    ultraTabSharedControlsPage1.Dispose();
                }
                if (_BlotterReports_UltraFormManager_Dock_Area_Right != null)
                {
                    _BlotterReports_UltraFormManager_Dock_Area_Right.Dispose();
                }
                if (_BlotterReports_UltraFormManager_Dock_Area_Bottom != null)
                {
                    _BlotterReports_UltraFormManager_Dock_Area_Bottom.Dispose();
                }
                if (_BlotterReports_UltraFormManager_Dock_Area_Left != null)
                {
                    _BlotterReports_UltraFormManager_Dock_Area_Left.Dispose();
                }
                if (_BlotterReports_UltraFormManager_Dock_Area_Top != null)
                {
                    _BlotterReports_UltraFormManager_Dock_Area_Top.Dispose();
                }
            }
            base.Dispose(disposing);
            InstanceManager.ReleaseInstance(typeof(BlotterReports));
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BlotterReports));
            this.ultraTabPageControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.ultraTabControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.executionReport2 = new Prana.Blotter.ExecutionReport();
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._BlotterReports_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._BlotterReports_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._BlotterReports_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._BlotterReports_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTabControl1)).BeginInit();
            this.ultraTabControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraTabPageControl1
            // 
            this.ultraTabPageControl1.Location = new System.Drawing.Point(1, 23);
            this.ultraTabPageControl1.Name = "ultraTabPageControl1";
            this.ultraTabPageControl1.Size = new System.Drawing.Size(672, 460);
            // 
            // ultraTabSharedControlsPage1
            // 
            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(672, 460);
            // 
            // ultraTabControl1
            // 
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(10)))));
            appearance1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ultraTabControl1.ActiveTabAppearance = appearance1;
            this.ultraTabControl1.Controls.Add(this.ultraTabSharedControlsPage1);
            this.ultraTabControl1.Controls.Add(this.ultraTabPageControl1);
            this.ultraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraTabControl1.Location = new System.Drawing.Point(4, 27);
            this.ultraTabControl1.Name = "ultraTabControl1";
            this.ultraTabControl1.NavigationStyle = Infragistics.Win.UltraWinTabControl.NavigationStyle.Activate;
            this.ultraTabControl1.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this.ultraTabControl1.Size = new System.Drawing.Size(676, 486);
            this.ultraTabControl1.TabIndex = 0;
            ultraTab1.TabPage = this.ultraTabPageControl1;
            ultraTab1.Text = "Execution Report";
            this.ultraTabControl1.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab1});
            // 
            // executionReport2
            // 
            this.executionReport2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.executionReport2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.executionReport2.Location = new System.Drawing.Point(0, 0);
            this.executionReport2.LoginUser = null;
            this.executionReport2.Name = "executionReport2";
            this.executionReport2.Size = new System.Drawing.Size(680, 496);
            this.executionReport2.TabIndex = 3;
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // _BlotterReports_UltraFormManager_Dock_Area_Left
            // 
            this._BlotterReports_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._BlotterReports_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._BlotterReports_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._BlotterReports_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._BlotterReports_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._BlotterReports_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 4;
            this._BlotterReports_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 27);
            this._BlotterReports_UltraFormManager_Dock_Area_Left.Name = "_BlotterReports_UltraFormManager_Dock_Area_Left";
            this._BlotterReports_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(4, 486);
            // 
            // _BlotterReports_UltraFormManager_Dock_Area_Right
            // 
            this._BlotterReports_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._BlotterReports_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._BlotterReports_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._BlotterReports_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._BlotterReports_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._BlotterReports_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 4;
            this._BlotterReports_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(680, 27);
            this._BlotterReports_UltraFormManager_Dock_Area_Right.Name = "_BlotterReports_UltraFormManager_Dock_Area_Right";
            this._BlotterReports_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(4, 486);
            // 
            // _BlotterReports_UltraFormManager_Dock_Area_Top
            // 
            this._BlotterReports_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._BlotterReports_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._BlotterReports_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._BlotterReports_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._BlotterReports_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._BlotterReports_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._BlotterReports_UltraFormManager_Dock_Area_Top.Name = "_BlotterReports_UltraFormManager_Dock_Area_Top";
            this._BlotterReports_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(684, 27);
            // 
            // _BlotterReports_UltraFormManager_Dock_Area_Bottom
            // 
            this._BlotterReports_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._BlotterReports_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._BlotterReports_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._BlotterReports_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._BlotterReports_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._BlotterReports_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 4;
            this._BlotterReports_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 513);
            this._BlotterReports_UltraFormManager_Dock_Area_Bottom.Name = "_BlotterReports_UltraFormManager_Dock_Area_Bottom";
            this._BlotterReports_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(684, 4);
            // 
            // BlotterReports
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ClientSize = new System.Drawing.Size(684, 517);
            this.Controls.Add(this.ultraTabControl1);
            this.Controls.Add(this._BlotterReports_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._BlotterReports_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._BlotterReports_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._BlotterReports_UltraFormManager_Dock_Area_Bottom);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "BlotterReports";
            this.Text = "Trade Report";
            ((System.ComponentModel.ISupportInitialize)(this.ultraTabControl1)).EndInit();
            this.ultraTabControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            this.ResumeLayout(false);

        }
        public void SetupSnapshotControl()
        {
            this.ultraButton1 = SnapShotManager.GetInstance().ultraButton;
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            // 
            // ultraTabPageControl1
            // 
            this.ultraTabPageControl1.Controls.Add(this.ultraButton1);
            this.ultraTabPageControl1.Controls.Add(this.executionReport2);
            this.ultraTabPageControl1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl1.Name = "ultraTabPageControl1";
            this.ultraTabPageControl1.Size = new System.Drawing.Size(680, 496);
            // 
            // ultraButton1
            // 
            this.ultraButton1.Appearance = appearance2;
            this.ultraButton1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.ultraButton1.Location = new System.Drawing.Point(577, 14);
            this.ultraButton1.Name = "ultraButton1";
            this.ultraButton1.Size = new System.Drawing.Size(69, 23);
            this.ultraButton1.TabIndex = 11;
            this.ultraButton1.Click += new System.EventHandler(this.ultraButton1_Click);
        }
        #endregion
        private void ultraButton1_Click(object sender, EventArgs e)
        {
            SnapShotManager.GetInstance().TakeSnapshot(this);
        }

        public void ExportData(string gridName, string WindowName, string tabName, string filePath)
        {
            this.executionReport2.ExportData(gridName, filePath);
        }
    }
}
