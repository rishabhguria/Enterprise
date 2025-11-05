//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Text;
//using System.Windows.Forms;
//using Infragistics.Win.UltraWinGrid;
//using Prana.BusinessObjects;
//using Prana.Global;
//using Prana.LiveFeedProvider;
//using Infragistics.Win;
//using Infragistics.Win.UltraWinDock;
//using Prana.Logging;
//using Prana.Interfaces;
//using Prana.InstanceCreator;
////using Prana.InstanceCreator;


//namespace Prana.LiveFeed.UI
//{
//    public class AnalyticsForm : Form
//    {



//        #region Private Variables

//        CompanyUser _companyUser;
//        private List<string> _underlyingSymbolsList = new List<string>();
//        //public event EventHandler TradeClick = null;
//        private List<string> _openOptionsPositionsSymbols = new List<string>();

//        #endregion

//        #region Singleton Implementation
//        private static AnalyticsForm _analyticsForm;
//        /// <summary>
//        /// Singleton instance of AnalyticsForm class
//        /// </summary>
//        /// <returns></returns>
//        public static AnalyticsForm GetInstance(CompanyUser user, List<string> underlyingSymbols, string selectedSymbol)
//        {
//            if (_analyticsForm == null)
//            {
//                _analyticsForm = new AnalyticsForm(user, underlyingSymbols, selectedSymbol);
//            }
//            return _analyticsForm;
//        }

//        private AnalyticsForm(CompanyUser user, List<string> underlyingSymbols, string selectedSymbol)
//        {
//            InitializeComponent();
//            _companyUser = user;

//            InitControl(underlyingSymbols, selectedSymbol);
//        }

//        #endregion        

//        private void InitControl(List<string> underlyingSymbols,string selectedSymbol)
//        {
//            GetOptionsPositionSymbols();
//            DockAreaPane top = new DockAreaPane(DockedLocation.DockedTop,"top");
//            top.ChildPaneStyle = ChildPaneStyle.TabGroup;
//            top.Maximized = true;
//            top.Settings.AllowFloating = DefaultableBoolean.False;

//            foreach (string existingsymbol in underlyingSymbols)
//            {
//               // AnalyticsUnderlyingSymbolControl underlyingControl = new AnalyticsUnderlyingSymbolControl(_companyUser, existingsymbol);
//                underlyingControl.TradeClick += new EventHandler<TradeParametersArgs>(underlyingControl_TradeClick);
//                DockableControlPane UnderlyingSymbolPane = new DockableControlPane(existingsymbol, underlyingControl);

//                UnderlyingSymbolPane.TextTab = existingsymbol.ToUpper();
//                top.Panes.Add(UnderlyingSymbolPane);
//                _underlyingSymbolsList.Add(existingsymbol);
//            }

//            ultraDockManager1.DockAreas.Add(top);
//            ultraDockManager1.DefaultPaneSettings.AllowMaximize = DefaultableBoolean.True;
//            ultraDockManager1.LayoutStyle = DockAreaLayoutStyle.FillContainer;
//            ultraDockManager1.DockAreas["top"].Panes[selectedSymbol].IsSelectedTab = true;
//        }

//        private void GetOptionsPositionSymbols()
//        {
//            //IPositionManagement pmInstance = PMInstanceCreator.GetPMInteractionInstance();
//            //List<string> _openPositionsSymbols = pmInstance.GetSymbolListForOpenPositionsAndTrades(_companyUser.CompanyID);
//            //foreach (string positionSymbol in _openPositionsSymbols)
//            //{
//            //    //if (positionSymbol)
//            //    //{
//            //    //    _openOptionsPositionsSymbols.Add(positionSymbol);
//            //    //}
//            //}
//        }


//        void underlyingControl_TradeClick(object sender, TradeParametersArgs e)
//        {
//            if (SendTrade != null)
//            {
//                SendTrade(this, e);
//            }
//        }
//        public event EventHandler<TradeParametersArgs> SendTrade = null;

//        #region Events of UnderlyingSymbol changes in Option Chain
//        /// <summary>
//        /// adds new tab for newly added underlying symbol.
//        /// </summary>
//        /// <param name="underlyingSymbol"></param>
//        public void AddUnderlyingSymbol(string underlyingSymbol)
//        {
//            if (_underlyingSymbolsList.Contains(underlyingSymbol))
//            {
//                //((DockableControlPane)ultraDockManager1.ControlPanes[underlyingSymbol]).Activate();

//            }
//            else
//            {

//                AnalyticsUnderlyingSymbolControl underlyingControl = new AnalyticsUnderlyingSymbolControl(_companyUser, underlyingSymbol);
//                //underlyingControl.Name = underlyingSymbol;
//                DockableControlPane UnderlyingSymbolPane = new DockableControlPane(underlyingSymbol, underlyingControl);

//                UnderlyingSymbolPane.TextTab = underlyingSymbol.ToUpper();

//                ultraDockManager1.DockAreas["top"].Panes.Add(UnderlyingSymbolPane);
//                ultraDockManager1.DockAreas["top"].ChildPaneStyle = ChildPaneStyle.TabGroup;
//                _underlyingSymbolsList.Add(underlyingSymbol);
//            }
//        }

//        /// <summary>
//        /// removes the tab for the underlying symbol
//        /// </summary>
//        /// <param name="underlyingSymbol"></param>
//        public void RemoveUnderlyingSymbol(string underlyingSymbol)
//        {
//            if (_underlyingSymbolsList.Contains(underlyingSymbol))
//            {
//                //remove tab

//                DockablePaneBase DockableControlToRemove = null;
//                if (ultraDockManager1.DockAreas["top"].Panes.Exists(underlyingSymbol))
//                {
//                    DockableControlToRemove = ultraDockManager1.DockAreas["top"].Panes[underlyingSymbol];
//                }
//                if (DockableControlToRemove != null)
//                {
//                    //DockableControlToRemove = ultraDockManager1.DockAreas["top"].Panes[underlyingSymbol] as DockablePaneBase;
//                }

//                ultraDockManager1.DockAreas["top"].Panes.Remove(underlyingSymbol);
//                if (DockableControlToRemove != null)
//                {
//                    DockableControlToRemove = null;
//                }

//                ((AnalyticsUnderlyingSymbolControl)ultraDockManager1.ControlPanes[underlyingSymbol].Control).Visible = false;
//                //System.Runtime.Remoting.ObjRef referencetodispose = CreateObjRef(typeof(Control));
//                ultraDockManager1.ControlPanes[underlyingSymbol].Close(true);
//                ultraDockManager1.ControlPanes[underlyingSymbol].Dispose();
//                ultraDockManager1.ControlPanes.Remove(underlyingSymbol);

//                //.Remove(underlyingSymbol);
//                ultraDockManager1.DockAreas["top"].ChildPaneStyle = ChildPaneStyle.TabGroup;
//                //ultraDockManager1.ControlPanes[underlyingSymbol].Dispose();
//                ultraDockManager1.DockAreas["top"].SelectedTabIndex = 0;

//                _underlyingSymbolsList.Remove(underlyingSymbol);
//                this.Refresh();

//            }
//        }

//        /// <summary>
//        /// removes old tab and adds a new one for the new underlying
//        /// </summary>
//        /// <param name="underlyingSymbol"></param>
//        public void UpdateUnderlyingSymbol(string oldUnderlyingSymbol, string underlyingSymbol)
//        {
//            if (_underlyingSymbolsList.Contains(oldUnderlyingSymbol))
//            {
//                RemoveUnderlyingSymbol(oldUnderlyingSymbol);
//                AddUnderlyingSymbol(underlyingSymbol);
//            }
//        }

//        #endregion

//        #region Designer Code

//        /// <summary>
//        /// Required designer variable.
//        /// </summary>
//        private System.ComponentModel.IContainer components = null;

//        /// <summary>
//        /// Clean up any resources being used.
//        /// </summary>
//        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
//        protected override void Dispose(bool disposing)
//        {
//            if (disposing && (components != null))
//            {
//                components.Dispose();
//            }
//            base.Dispose(disposing);
//            _analyticsForm = null;
//        }

//        #region Windows Form Designer generated code

//        /// <summary>
//        /// Required method for Designer support - do not modify
//        /// the contents of this method with the code editor.
//        /// </summary>
//        private void InitializeComponent()
//        {
//            this.components = new System.ComponentModel.Container();
//            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AnalyticsForm));
//            this.ultraDockManager1 = new Infragistics.Win.UltraWinDock.UltraDockManager(this.components);
//            this._AnalyticsFormUnpinnedTabAreaLeft = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
//            this._AnalyticsFormUnpinnedTabAreaRight = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
//            this._AnalyticsFormUnpinnedTabAreaTop = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
//            this._AnalyticsFormUnpinnedTabAreaBottom = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
//            this._AnalyticsFormAutoHideControl = new Infragistics.Win.UltraWinDock.AutoHideControl();
//            ((System.ComponentModel.ISupportInitialize)(this.ultraDockManager1)).BeginInit();
//            this.SuspendLayout();
//            // 
//            // ultraDockManager1
//            // 
//            this.ultraDockManager1.DefaultPaneSettings.AllowDockBottom = Infragistics.Win.DefaultableBoolean.False;
//            this.ultraDockManager1.DefaultPaneSettings.AllowDockLeft = Infragistics.Win.DefaultableBoolean.False;
//            this.ultraDockManager1.DefaultPaneSettings.AllowDockRight = Infragistics.Win.DefaultableBoolean.False;
//            this.ultraDockManager1.DefaultPaneSettings.AllowDockTop = Infragistics.Win.DefaultableBoolean.False;
//            this.ultraDockManager1.DefaultPaneSettings.ShowCaption = Infragistics.Win.DefaultableBoolean.False;
//            this.ultraDockManager1.HostControl = this;
//            this.ultraDockManager1.ShowCloseButton = false;
//            this.ultraDockManager1.ShowPinButton = false;
//            this.ultraDockManager1.UseDefaultContextMenus = false;
//            this.ultraDockManager1.BeforeToggleDockState += new Infragistics.Win.UltraWinDock.CancelablePaneEventHandler(this.ultraDockManager1_BeforeToggleDockState);
//            this.ultraDockManager1.BeforePaneButtonClick += new Infragistics.Win.UltraWinDock.CancelablePaneButtonEventHandler(this.ultraDockManager1_BeforePaneButtonClick);
//            this.ultraDockManager1.AfterToggleDockState += new Infragistics.Win.UltraWinDock.PaneEventHandler(this.ultraDockManager1_AfterToggleDockState);
//            // 
//            // _AnalyticsFormUnpinnedTabAreaLeft
//            // 
//            this._AnalyticsFormUnpinnedTabAreaLeft.Dock = System.Windows.Forms.DockStyle.Left;
//            this._AnalyticsFormUnpinnedTabAreaLeft.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
//            this._AnalyticsFormUnpinnedTabAreaLeft.Location = new System.Drawing.Point(0, 0);
//            this._AnalyticsFormUnpinnedTabAreaLeft.Name = "_AnalyticsFormUnpinnedTabAreaLeft";
//            this._AnalyticsFormUnpinnedTabAreaLeft.Owner = this.ultraDockManager1;
//            this._AnalyticsFormUnpinnedTabAreaLeft.Size = new System.Drawing.Size(0, 471);
//            this._AnalyticsFormUnpinnedTabAreaLeft.TabIndex = 0;
//            // 
//            // _AnalyticsFormUnpinnedTabAreaRight
//            // 
//            this._AnalyticsFormUnpinnedTabAreaRight.Dock = System.Windows.Forms.DockStyle.Right;
//            this._AnalyticsFormUnpinnedTabAreaRight.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
//            this._AnalyticsFormUnpinnedTabAreaRight.Location = new System.Drawing.Point(730, 0);
//            this._AnalyticsFormUnpinnedTabAreaRight.Name = "_AnalyticsFormUnpinnedTabAreaRight";
//            this._AnalyticsFormUnpinnedTabAreaRight.Owner = this.ultraDockManager1;
//            this._AnalyticsFormUnpinnedTabAreaRight.Size = new System.Drawing.Size(0, 471);
//            this._AnalyticsFormUnpinnedTabAreaRight.TabIndex = 1;
//            // 
//            // _AnalyticsFormUnpinnedTabAreaTop
//            // 
//            this._AnalyticsFormUnpinnedTabAreaTop.Dock = System.Windows.Forms.DockStyle.Top;
//            this._AnalyticsFormUnpinnedTabAreaTop.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
//            this._AnalyticsFormUnpinnedTabAreaTop.Location = new System.Drawing.Point(0, 0);
//            this._AnalyticsFormUnpinnedTabAreaTop.Name = "_AnalyticsFormUnpinnedTabAreaTop";
//            this._AnalyticsFormUnpinnedTabAreaTop.Owner = this.ultraDockManager1;
//            this._AnalyticsFormUnpinnedTabAreaTop.Size = new System.Drawing.Size(730, 0);
//            this._AnalyticsFormUnpinnedTabAreaTop.TabIndex = 2;
//            // 
//            // _AnalyticsFormUnpinnedTabAreaBottom
//            // 
//            this._AnalyticsFormUnpinnedTabAreaBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
//            this._AnalyticsFormUnpinnedTabAreaBottom.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
//            this._AnalyticsFormUnpinnedTabAreaBottom.Location = new System.Drawing.Point(0, 471);
//            this._AnalyticsFormUnpinnedTabAreaBottom.Name = "_AnalyticsFormUnpinnedTabAreaBottom";
//            this._AnalyticsFormUnpinnedTabAreaBottom.Owner = this.ultraDockManager1;
//            this._AnalyticsFormUnpinnedTabAreaBottom.Size = new System.Drawing.Size(730, 0);
//            this._AnalyticsFormUnpinnedTabAreaBottom.TabIndex = 3;
//            // 
//            // _AnalyticsFormAutoHideControl
//            // 
//            this._AnalyticsFormAutoHideControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
//            this._AnalyticsFormAutoHideControl.Location = new System.Drawing.Point(0, 0);
//            this._AnalyticsFormAutoHideControl.Name = "_AnalyticsFormAutoHideControl";
//            this._AnalyticsFormAutoHideControl.Owner = this.ultraDockManager1;
//            this._AnalyticsFormAutoHideControl.Size = new System.Drawing.Size(0, 0);
//            this._AnalyticsFormAutoHideControl.TabIndex = 4;
//            // 
//            // AnalyticsForm
//            // 
//            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
//            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
//            this.ClientSize = new System.Drawing.Size(730, 471);
//            this.Controls.Add(this._AnalyticsFormAutoHideControl);
//            this.Controls.Add(this._AnalyticsFormUnpinnedTabAreaLeft);
//            this.Controls.Add(this._AnalyticsFormUnpinnedTabAreaTop);
//            this.Controls.Add(this._AnalyticsFormUnpinnedTabAreaBottom);
//            this.Controls.Add(this._AnalyticsFormUnpinnedTabAreaRight);
//            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
//            this.MinimumSize = new System.Drawing.Size(700, 500);
//            this.Name = "AnalyticsForm";
//            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
//            this.Text = "AnalyticsForm";
//            ((System.ComponentModel.ISupportInitialize)(this.ultraDockManager1)).EndInit();
//            this.ResumeLayout(false);

//        }

//        private Infragistics.Win.UltraWinDock.UltraDockManager ultraDockManager1;
//        private Infragistics.Win.UltraWinDock.AutoHideControl _AnalyticsFormAutoHideControl;
//        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _AnalyticsFormUnpinnedTabAreaLeft;
//        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _AnalyticsFormUnpinnedTabAreaTop;
//        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _AnalyticsFormUnpinnedTabAreaBottom;
//        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _AnalyticsFormUnpinnedTabAreaRight;


//        #endregion

//        #endregion

//        #region Ultra Dock Manager Events
//        private void ultraDockManager1_AfterToggleDockState(object sender, PaneEventArgs e)
//        {
//            try
//            {
//                /// In this we have not used the ActivePane property because it is returned null sometimes.
//                /// which causes problem. Hence now we are setting the show default property to false for every
//                /// docked window. Needs to be optimized
//                int _count = 0;

//                foreach (DockableControlPane pane in ultraDockManager1.ControlPanes)
//                {
//                    if (pane.DockedState.ToString() == "Docked")
//                    {
//                        _count++;
//                        pane.Settings.ShowCaption = DefaultableBoolean.False;
//                        pane.Settings.AllowDragging = DefaultableBoolean.False;
//                    }
//                    else
//                    {
//                        pane.Text = ((DockablePaneBase)(pane)).TextTab;
//                        pane.Settings.ShowCaption = DefaultableBoolean.True;
//                        pane.Settings.AllowDragging = DefaultableBoolean.True;
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
//                if (rethrow)
//                {
//                    throw;
//                }
//            }
//        }

//        private void ultraDockManager1_BeforeToggleDockState(object sender, CancelablePaneEventArgs e)
//        {
//            int _count = 0;
//            try
//            {

//                _count = e.Pane.DockAreaPane.Panes.Count;

//                if (_count == 1 && e.Pane.DockedState != DockedState.Floating)
//                {

//                    e.Pane.Settings.ShowCaption = DefaultableBoolean.True;
//                    e.Cancel = true;
//                    return;
//                }
//            }
//            catch (Exception ex)
//            {
//                string s = ex.Message + ex.StackTrace;
//            }


//        }

//        private void ultraDockManager1_BeforePaneButtonClick(object sender, CancelablePaneButtonEventArgs e)
//        {
//            if (e.Button.ToString().Equals("close", StringComparison.OrdinalIgnoreCase))
//            {
//                e.Pane.Dock(true);
//                if (e.Pane.DockedState.ToString().Equals("docked", StringComparison.OrdinalIgnoreCase))
//                {
//                    e.Pane.Settings.ShowCaption = DefaultableBoolean.False;
//                    e.Pane.Settings.AllowDragging = DefaultableBoolean.False;
//                }
//                e.Cancel = true;
//            }
//        }

//        #endregion
//    }
//}
