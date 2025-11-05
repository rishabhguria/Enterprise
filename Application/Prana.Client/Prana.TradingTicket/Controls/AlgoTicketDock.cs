using Infragistics.Win;
using Infragistics.Win.UltraWinDock;
using Prana.LogManager;
using System;
using System.Windows.Forms;

namespace Prana.TradingTicket
{
    /// <summary>
    /// Summary description for AlgoTicketDock.
    /// </summary>
    public class AlgoTicketDock : System.Windows.Forms.UserControl
    {
        private Infragistics.Win.UltraWinDock.UltraDockManager ultraDockManager1;
        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _AlgoTicketDockUnpinnedTabAreaLeft;
        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _AlgoTicketDockUnpinnedTabAreaRight;
        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _AlgoTicketDockUnpinnedTabAreaTop;
        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _AlgoTicketDockUnpinnedTabAreaBottom;
        private Infragistics.Win.UltraWinDock.AutoHideControl _AlgoTicketDockAutoHideControl;
        private Prana.TradingTicket.AlgoTicket algoBernstien;
        private Prana.TradingTicket.AlgoTicket algoPipperJaffray;
        private Prana.TradingTicket.AlgoTicket algoCredit;
        private Infragistics.Win.UltraWinDock.WindowDockingArea windowDockingArea1;
        private Infragistics.Win.UltraWinDock.DockableWindow dockableWindow1;
        private Infragistics.Win.UltraWinDock.DockableWindow dockableWindow2;
        private Infragistics.Win.UltraWinDock.DockableWindow dockableWindow3;
        private System.ComponentModel.IContainer components;

        public AlgoTicketDock()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            // TODO: Add any initialization after the InitializeComponent call

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
                if (ultraDockManager1 != null)
                {
                    ultraDockManager1.Dispose();
                }
                if (_AlgoTicketDockUnpinnedTabAreaLeft != null)
                {
                    _AlgoTicketDockUnpinnedTabAreaLeft.Dispose();
                }
                if (_AlgoTicketDockUnpinnedTabAreaRight != null)
                {
                    _AlgoTicketDockUnpinnedTabAreaRight.Dispose();
                }
                if (_AlgoTicketDockUnpinnedTabAreaTop != null)
                {
                    _AlgoTicketDockUnpinnedTabAreaTop.Dispose();
                }
                if (_AlgoTicketDockUnpinnedTabAreaBottom != null)
                {
                    _AlgoTicketDockUnpinnedTabAreaBottom.Dispose();
                }
                if (_AlgoTicketDockAutoHideControl != null)
                {
                    _AlgoTicketDockAutoHideControl.Dispose();
                }
                if (algoBernstien != null)
                {
                    algoBernstien.Dispose();
                }
                if (algoPipperJaffray != null)
                {
                    algoPipperJaffray.Dispose();
                }
                if (algoCredit != null)
                {
                    algoCredit.Dispose();
                }
                if (windowDockingArea1 != null)
                {
                    windowDockingArea1.Dispose();
                }
                if (dockableWindow1 != null)
                {
                    dockableWindow1.Dispose();
                }
                if (dockableWindow2 != null)
                {
                    dockableWindow2.Dispose();
                }
                if (dockableWindow3 != null)
                {
                    dockableWindow3.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinDock.DockAreaPane dockAreaPane1 = new Infragistics.Win.UltraWinDock.DockAreaPane(Infragistics.Win.UltraWinDock.DockedLocation.DockedTop, new System.Guid("ff3651d5-dd52-42e7-9d0b-aee2987b9580"));
            Infragistics.Win.UltraWinDock.DockableControlPane dockableControlPane1 = new Infragistics.Win.UltraWinDock.DockableControlPane(new System.Guid("c4ea3556-66ae-408b-881a-673f662a1e28"), new System.Guid("00000000-0000-0000-0000-000000000000"), -1, new System.Guid("ff3651d5-dd52-42e7-9d0b-aee2987b9580"), -1);
            Infragistics.Win.UltraWinDock.DockableControlPane dockableControlPane2 = new Infragistics.Win.UltraWinDock.DockableControlPane(new System.Guid("fd9b9586-01af-4264-9e27-69dbd63d99de"), new System.Guid("00000000-0000-0000-0000-000000000000"), -1, new System.Guid("ff3651d5-dd52-42e7-9d0b-aee2987b9580"), -1);
            Infragistics.Win.UltraWinDock.DockableControlPane dockableControlPane3 = new Infragistics.Win.UltraWinDock.DockableControlPane(new System.Guid("4b43b248-66db-40ae-ad38-36a5a924a06f"), new System.Guid("00000000-0000-0000-0000-000000000000"), -1, new System.Guid("ff3651d5-dd52-42e7-9d0b-aee2987b9580"), -1);
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            this.algoBernstien = new Prana.TradingTicket.AlgoTicket();
            this.algoPipperJaffray = new Prana.TradingTicket.AlgoTicket();
            this.algoCredit = new Prana.TradingTicket.AlgoTicket();
            this.ultraDockManager1 = new Infragistics.Win.UltraWinDock.UltraDockManager(this.components);
            this._AlgoTicketDockUnpinnedTabAreaLeft = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._AlgoTicketDockUnpinnedTabAreaRight = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._AlgoTicketDockUnpinnedTabAreaTop = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._AlgoTicketDockUnpinnedTabAreaBottom = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._AlgoTicketDockAutoHideControl = new Infragistics.Win.UltraWinDock.AutoHideControl();
            this.windowDockingArea1 = new Infragistics.Win.UltraWinDock.WindowDockingArea();
            this.dockableWindow1 = new Infragistics.Win.UltraWinDock.DockableWindow();
            this.dockableWindow2 = new Infragistics.Win.UltraWinDock.DockableWindow();
            this.dockableWindow3 = new Infragistics.Win.UltraWinDock.DockableWindow();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDockManager1)).BeginInit();
            this.windowDockingArea1.SuspendLayout();
            this.dockableWindow1.SuspendLayout();
            this.dockableWindow2.SuspendLayout();
            this.dockableWindow3.SuspendLayout();
            this.SuspendLayout();
            // 
            // algoBernstien
            // 
            this.algoBernstien.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(205)), ((System.Byte)(241)), ((System.Byte)(169)));
            this.algoBernstien.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.algoBernstien.Location = new System.Drawing.Point(72, 0);
            this.algoBernstien.Name = "algoBernstien";
            this.algoBernstien.Size = new System.Drawing.Size(434, 216);
            this.algoBernstien.TabIndex = 5;
            this.algoBernstien.CloseClick += new EventHandler(algoTicket_CloseClick);
            // 
            // algoPipperJaffray
            // 
            this.algoPipperJaffray.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(205)), ((System.Byte)(241)), ((System.Byte)(169)));
            this.algoPipperJaffray.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.algoPipperJaffray.Location = new System.Drawing.Point(72, 0);
            this.algoPipperJaffray.Name = "algoPipperJaffray";
            this.algoPipperJaffray.Size = new System.Drawing.Size(434, 216);
            this.algoPipperJaffray.TabIndex = 6;
            this.algoPipperJaffray.CloseClick += new EventHandler(algoTicket_CloseClick);
            // 
            // algoCredit
            // 
            this.algoCredit.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(205)), ((System.Byte)(241)), ((System.Byte)(169)));
            this.algoCredit.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.algoCredit.Location = new System.Drawing.Point(0, 0);
            this.algoCredit.Name = "algoCredit";
            this.algoCredit.Size = new System.Drawing.Size(568, 204);
            this.algoCredit.TabIndex = 7;
            this.algoCredit.CloseClick += new EventHandler(algoTicket_CloseClick);
            // 
            // ultraDockManager1
            // 
            this.ultraDockManager1.CaptionStyle = Infragistics.Win.UltraWinDock.CaptionStyle.VSNet;
            appearance1.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
            this.ultraDockManager1.DefaultGroupSettings.TabAreaAppearance = appearance1;
            this.ultraDockManager1.DefaultGroupSettings.TabLocation = Infragistics.Win.UltraWinDock.Location.Top;
            this.ultraDockManager1.DefaultGroupSettings.TabStyle = Infragistics.Win.UltraWinTabs.TabStyle.Excel;
            appearance2.FontData.BoldAsString = "True";
            appearance2.ForeColor = System.Drawing.Color.White;
            this.ultraDockManager1.DefaultPaneSettings.ActiveCaptionAppearance = appearance2;
            appearance3.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
            this.ultraDockManager1.DefaultPaneSettings.ActivePaneAppearance = appearance3;
            appearance4.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(152)), ((System.Byte)(10)));
            appearance4.BackColor2 = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance4.FontData.BoldAsString = "True";
            this.ultraDockManager1.DefaultPaneSettings.ActiveTabAppearance = appearance4;
            this.ultraDockManager1.DefaultPaneSettings.AllowClose = Infragistics.Win.DefaultableBoolean.True;
            this.ultraDockManager1.DefaultPaneSettings.AllowDockBottom = Infragistics.Win.DefaultableBoolean.False;
            this.ultraDockManager1.DefaultPaneSettings.AllowDockLeft = Infragistics.Win.DefaultableBoolean.False;
            this.ultraDockManager1.DefaultPaneSettings.AllowDockRight = Infragistics.Win.DefaultableBoolean.False;
            this.ultraDockManager1.DefaultPaneSettings.AllowDockTop = Infragistics.Win.DefaultableBoolean.False;
            this.ultraDockManager1.DefaultPaneSettings.AllowDragging = Infragistics.Win.DefaultableBoolean.False;
            this.ultraDockManager1.DefaultPaneSettings.AllowFloating = Infragistics.Win.DefaultableBoolean.True;
            this.ultraDockManager1.DefaultPaneSettings.AllowMinimize = Infragistics.Win.DefaultableBoolean.True;
            this.ultraDockManager1.DefaultPaneSettings.AllowPin = Infragistics.Win.DefaultableBoolean.True;
            appearance5.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
            this.ultraDockManager1.DefaultPaneSettings.Appearance = appearance5;
            appearance6.FontData.BoldAsString = "False";
            appearance6.FontData.Name = "Verdana";
            appearance6.FontData.SizeInPoints = 8F;
            appearance6.ForeColor = System.Drawing.Color.Black;
            this.ultraDockManager1.DefaultPaneSettings.CaptionAppearance = appearance6;
            this.ultraDockManager1.DefaultPaneSettings.PaddingBottom = 0;
            this.ultraDockManager1.DefaultPaneSettings.PaddingLeft = 0;
            this.ultraDockManager1.DefaultPaneSettings.PaddingRight = 0;
            this.ultraDockManager1.DefaultPaneSettings.PaddingTop = 0;
            appearance7.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
            this.ultraDockManager1.DefaultPaneSettings.PaneAppearance = appearance7;
            appearance8.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(152)), ((System.Byte)(10)));
            appearance8.BackColor2 = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
            appearance8.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance8.FontData.BoldAsString = "True";
            this.ultraDockManager1.DefaultPaneSettings.SelectedTabAppearance = appearance8;
            this.ultraDockManager1.DefaultPaneSettings.ShowCaption = Infragistics.Win.DefaultableBoolean.False;
            appearance9.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
            appearance9.FontData.Name = "Verdana";
            appearance9.FontData.SizeInPoints = 8.25F;
            appearance9.ForeColor = System.Drawing.Color.Black;
            this.ultraDockManager1.DefaultPaneSettings.TabAppearance = appearance9;
            dockAreaPane1.ChildPaneStyle = Infragistics.Win.UltraWinDock.ChildPaneStyle.TabGroup;
            dockableControlPane1.Control = this.algoBernstien;
            dockableControlPane1.OriginalControlBounds = new System.Drawing.Rectangle(314, 210, 434, 236);
            dockableControlPane1.Size = new System.Drawing.Size(100, 100);
            dockableControlPane1.Text = "Bernstein";
            dockableControlPane2.Control = this.algoPipperJaffray;
            dockableControlPane2.OriginalControlBounds = new System.Drawing.Rectangle(-192, 196, 434, 236);
            dockableControlPane2.Size = new System.Drawing.Size(100, 100);
            dockableControlPane2.Text = "PiperJaffray";
            dockableControlPane3.Control = this.algoCredit;
            dockableControlPane3.OriginalControlBounds = new System.Drawing.Rectangle(326, -60, 434, 236);
            dockableControlPane3.Size = new System.Drawing.Size(100, 100);
            dockableControlPane3.Text = "Credit Suisse";
            dockAreaPane1.Panes.AddRange(new Infragistics.Win.UltraWinDock.DockablePaneBase[] {
                                                                                                  dockableControlPane1,
                                                                                                  dockableControlPane2,
                                                                                                  dockableControlPane3});
            dockAreaPane1.SelectedTabIndex = 2;
            dockAreaPane1.Size = new System.Drawing.Size(568, 224);
            dockAreaPane1.UnfilledSize = new System.Drawing.Size(568, 207);
            this.ultraDockManager1.DockAreas.AddRange(new Infragistics.Win.UltraWinDock.DockAreaPane[] {
                                                                                                           dockAreaPane1});
            this.ultraDockManager1.DragWindowOpacity = 0.3;
            this.ultraDockManager1.DragWindowStyle = Infragistics.Win.UltraWinDock.DragWindowStyle.LayeredWindow;
            this.ultraDockManager1.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.ultraDockManager1.HostControl = this;
            this.ultraDockManager1.LayoutStyle = Infragistics.Win.UltraWinDock.DockAreaLayoutStyle.FillContainer;
            this.ultraDockManager1.ShowDisabledButtons = false;
            this.ultraDockManager1.ShowMinimizeButton = true;
            appearance10.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
            this.ultraDockManager1.UnpinnedTabAreaAppearance = appearance10;
            this.ultraDockManager1.WindowStyle = Infragistics.Win.UltraWinDock.WindowStyle.Windows;
            this.ultraDockManager1.AfterToggleDockState += new Infragistics.Win.UltraWinDock.PaneEventHandler(this.ultraDockManager1_AfterToggleDockState);
            this.ultraDockManager1.BeforeToggleDockState += new Infragistics.Win.UltraWinDock.CancelablePaneEventHandler(this.ultraDockManager1_BeforeToggleDockState);
            this.ultraDockManager1.BeforePaneButtonClick += new CancelablePaneButtonEventHandler(ultraDockManager1_BeforePaneButtonClick);
            // 
            // _AlgoTicketDockUnpinnedTabAreaLeft
            // 
            this._AlgoTicketDockUnpinnedTabAreaLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this._AlgoTicketDockUnpinnedTabAreaLeft.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((System.Byte)(0)));
            this._AlgoTicketDockUnpinnedTabAreaLeft.Location = new System.Drawing.Point(0, 0);
            this._AlgoTicketDockUnpinnedTabAreaLeft.Name = "_AlgoTicketDockUnpinnedTabAreaLeft";
            this._AlgoTicketDockUnpinnedTabAreaLeft.Owner = this.ultraDockManager1;
            this._AlgoTicketDockUnpinnedTabAreaLeft.Size = new System.Drawing.Size(0, 224);
            this._AlgoTicketDockUnpinnedTabAreaLeft.TabIndex = 0;
            // 
            // _AlgoTicketDockUnpinnedTabAreaRight
            // 
            this._AlgoTicketDockUnpinnedTabAreaRight.Dock = System.Windows.Forms.DockStyle.Right;
            this._AlgoTicketDockUnpinnedTabAreaRight.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((System.Byte)(0)));
            this._AlgoTicketDockUnpinnedTabAreaRight.Location = new System.Drawing.Point(568, 0);
            this._AlgoTicketDockUnpinnedTabAreaRight.Name = "_AlgoTicketDockUnpinnedTabAreaRight";
            this._AlgoTicketDockUnpinnedTabAreaRight.Owner = this.ultraDockManager1;
            this._AlgoTicketDockUnpinnedTabAreaRight.Size = new System.Drawing.Size(0, 224);
            this._AlgoTicketDockUnpinnedTabAreaRight.TabIndex = 1;
            // 
            // _AlgoTicketDockUnpinnedTabAreaTop
            // 
            this._AlgoTicketDockUnpinnedTabAreaTop.Dock = System.Windows.Forms.DockStyle.Top;
            this._AlgoTicketDockUnpinnedTabAreaTop.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((System.Byte)(0)));
            this._AlgoTicketDockUnpinnedTabAreaTop.Location = new System.Drawing.Point(0, 0);
            this._AlgoTicketDockUnpinnedTabAreaTop.Name = "_AlgoTicketDockUnpinnedTabAreaTop";
            this._AlgoTicketDockUnpinnedTabAreaTop.Owner = this.ultraDockManager1;
            this._AlgoTicketDockUnpinnedTabAreaTop.Size = new System.Drawing.Size(568, 0);
            this._AlgoTicketDockUnpinnedTabAreaTop.TabIndex = 2;
            // 
            // _AlgoTicketDockUnpinnedTabAreaBottom
            // 
            this._AlgoTicketDockUnpinnedTabAreaBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._AlgoTicketDockUnpinnedTabAreaBottom.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((System.Byte)(0)));
            this._AlgoTicketDockUnpinnedTabAreaBottom.Location = new System.Drawing.Point(0, 224);
            this._AlgoTicketDockUnpinnedTabAreaBottom.Name = "_AlgoTicketDockUnpinnedTabAreaBottom";
            this._AlgoTicketDockUnpinnedTabAreaBottom.Owner = this.ultraDockManager1;
            this._AlgoTicketDockUnpinnedTabAreaBottom.Size = new System.Drawing.Size(568, 0);
            this._AlgoTicketDockUnpinnedTabAreaBottom.TabIndex = 3;
            // 
            // _AlgoTicketDockAutoHideControl
            // 
            this._AlgoTicketDockAutoHideControl.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((System.Byte)(0)));
            this._AlgoTicketDockAutoHideControl.Location = new System.Drawing.Point(0, 0);
            this._AlgoTicketDockAutoHideControl.Name = "_AlgoTicketDockAutoHideControl";
            this._AlgoTicketDockAutoHideControl.Owner = this.ultraDockManager1;
            this._AlgoTicketDockAutoHideControl.TabIndex = 4;
            // 
            // windowDockingArea1
            // 
            this.windowDockingArea1.Controls.Add(this.dockableWindow1);
            this.windowDockingArea1.Controls.Add(this.dockableWindow2);
            this.windowDockingArea1.Controls.Add(this.dockableWindow3);
            this.windowDockingArea1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.windowDockingArea1.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((System.Byte)(0)));
            this.windowDockingArea1.Location = new System.Drawing.Point(0, 0);
            this.windowDockingArea1.Name = "windowDockingArea1";
            this.windowDockingArea1.Owner = this.ultraDockManager1;
            this.windowDockingArea1.Size = new System.Drawing.Size(568, 224);
            this.windowDockingArea1.TabIndex = 8;
            // 
            // dockableWindow1
            // 
            this.dockableWindow1.Controls.Add(this.algoBernstien);
            this.dockableWindow1.Location = new System.Drawing.Point(-10000, 20);
            this.dockableWindow1.Name = "dockableWindow1";
            this.dockableWindow1.Owner = this.ultraDockManager1;
            this.dockableWindow1.Size = new System.Drawing.Size(568, 216);
            this.dockableWindow1.TabIndex = 0;
            // 
            // dockableWindow2
            // 
            this.dockableWindow2.Controls.Add(this.algoPipperJaffray);
            this.dockableWindow2.Location = new System.Drawing.Point(-10000, 20);
            this.dockableWindow2.Name = "dockableWindow2";
            this.dockableWindow2.Owner = this.ultraDockManager1;
            this.dockableWindow2.Size = new System.Drawing.Size(568, 216);
            this.dockableWindow2.TabIndex = 1;
            // 
            // dockableWindow3
            // 
            this.dockableWindow3.Controls.Add(this.algoCredit);
            this.dockableWindow3.Location = new System.Drawing.Point(0, 20);
            this.dockableWindow3.Name = "dockableWindow3";
            this.dockableWindow3.Owner = this.ultraDockManager1;
            this.dockableWindow3.Size = new System.Drawing.Size(568, 204);
            this.dockableWindow3.TabIndex = 2;
            // 
            // AlgoTicketDock
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
            this.Controls.Add(this._AlgoTicketDockAutoHideControl);
            this.Controls.Add(this.windowDockingArea1);
            this.Controls.Add(this._AlgoTicketDockUnpinnedTabAreaTop);
            this.Controls.Add(this._AlgoTicketDockUnpinnedTabAreaBottom);
            this.Controls.Add(this._AlgoTicketDockUnpinnedTabAreaLeft);
            this.Controls.Add(this._AlgoTicketDockUnpinnedTabAreaRight);
            this.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((System.Byte)(0)));
            this.Name = "AlgoTicketDock";
            this.Size = new System.Drawing.Size(568, 224);
            this.Load += new System.EventHandler(this.AlgoTicketDock_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ultraDockManager1)).EndInit();
            this.windowDockingArea1.ResumeLayout(false);
            this.dockableWindow1.ResumeLayout(false);
            this.dockableWindow2.ResumeLayout(false);
            this.dockableWindow3.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion
        public void SetUpControls()
        {
            //this.algoBernstein.
        }

        //private void ultraDockManager1_InitializePane(object sender, Infragistics.Win.UltraWinDock.InitializePaneEventArgs e)
        //{

        //}

        private void AlgoTicketDock_Load(object sender, System.EventArgs e)
        {
            this.algoBernstien.TicketType(AlgoTicketTypes.Bernstein);
            this.algoCredit.TicketType(AlgoTicketTypes.CreditSusie);
            this.algoPipperJaffray.TicketType(AlgoTicketTypes.PiperJaffray);
        }
        private void ultraDockManager1_AfterToggleDockState(object sender, Infragistics.Win.UltraWinDock.PaneEventArgs e)
        {
            try
            {
                /// In this we have not used the ActivePane property because it is returned null sometimes.
                /// which causes problem. Hence now we are setting the show default property to false for every
                /// docked window. Needs to be optimized
                /// 
                /// 
                int _count = 0;

                foreach (DockableControlPane pane in ultraDockManager1.ControlPanes)
                {

                    if (pane.DockedState.ToString() == "Docked")
                    {
                        _count++;
                        pane.Settings.ShowCaption = DefaultableBoolean.False;
                        pane.Settings.AllowDragging = DefaultableBoolean.False;
                    }
                    else
                    {
                        pane.Settings.ShowCaption = DefaultableBoolean.True;
                        pane.Settings.AllowDragging = DefaultableBoolean.True;
                    }

                }

                //				if (ultraDockManager1.ActivePane !=null)
                //				{
                //				if (ultraDockManager1.ActivePane.DockedState.ToString() == "Floating")
                //				{
                //					ultraDockManager1.ActivePane.Settings.AllowDragging = DefaultableBoolean.True;
                //					ultraDockManager1.ActivePane.Settings.ShowCaption = DefaultableBoolean.True;
                //										
                //				}
                //				else
                //				{
                //					ultraDockManager1.ActivePane.Settings.AllowDragging = DefaultableBoolean.False;
                //					ultraDockManager1.ActivePane.Settings.ShowCaption = DefaultableBoolean.False;
                //				}
                ////					if (e.Pane.DockedState.ToString()== "Floating")
                ////					{
                ////						e.Pane.Settings.ShowCaption = DefaultableBoolean.True;
                ////						e.Pane.Settings.AllowDragging = DefaultableBoolean.True;
                ////					}
                ////					else
                ////					{
                ////						e.Pane.Settings.ShowCaption = DefaultableBoolean.False;
                ////						e.Pane.Settings.AllowDragging = DefaultableBoolean.False;
                ////					}
                //					//Some changes need to be made so that we can show the caption of the only available window in the TradingTicketwindow
                //					//	if (e.Pane.DockAreaPane.Panes.Count == 1)
                //					//	{
                //					//	//e.Pane.DockAreaPane.Settings.ShowCaption = DefaultableBoolean.True;
                //					//	}
                //					//			else
                //					//			{
                //					//			this.ultraDockManager1.DefaultPaneSettings.ShowCaption = DefaultableBoolean.False;
                //					//			}
                //				}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }



        }
        private void ultraDockManager1_BeforeToggleDockState(object sender, Infragistics.Win.UltraWinDock.CancelablePaneEventArgs e)
        {
            try
            {
                int _count = 0;
                try
                {
                    _count = e.Pane.DockAreaPane.Panes.Count;

                    if (_count == 1 && e.Pane.DockedState != Infragistics.Win.UltraWinDock.DockedState.Floating)
                    {

                        e.Pane.Settings.ShowCaption = DefaultableBoolean.True;
                        e.Cancel = true;
                        return;
                    }
                }
                catch (Exception ex)
                {
                    //string s = ex.Message + ex.StackTrace;
                    Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
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

        private void ultraDockManager1_BeforePaneButtonClick(object sender, CancelablePaneButtonEventArgs e)
        {
            try
            {
                if (e.Button.ToString().Equals("close", StringComparison.OrdinalIgnoreCase))
                {
                    e.Pane.Dock(true);
                    if (e.Pane.DockedState.ToString().Equals("docked", StringComparison.OrdinalIgnoreCase))
                    {
                        e.Pane.Settings.ShowCaption = DefaultableBoolean.False;
                        e.Pane.Settings.AllowDragging = DefaultableBoolean.False;
                    }
                    e.Cancel = true;
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
        public event EventHandler CloseClick = null;

        private void algoTicket_CloseClick(object sender, EventArgs e)
        {
            //			if (e.Pane.DockedState == DockedState.Floating) //.ToString().ToLower() == "docked")
            //			{
            //				
            //			}
            if (CloseClick != null)
            {
                CloseClick(sender, e);
            }

        }
    }

    public enum AlgoTicketTypes
    {
        Bernstein,
        PiperJaffray,
        CreditSusie,
        NotSet
    }
}
