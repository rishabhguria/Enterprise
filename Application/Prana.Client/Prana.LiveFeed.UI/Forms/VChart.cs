//using System;
//using System.Drawing;
//using System.Collections;
//using System.ComponentModel;
//using System.Windows.Forms;
//using Prana.Interfaces;
//using Infragistics.Win; 
//using Infragistics.Win.UltraWinToolbars;
//using Prana.Global;
//using Prana.LiveFeedProvider;
//using Prana.BusinessObjects.LiveFeed;
////using Prana.Admin.Utility;
//using Prana.Logging;

//namespace Prana.LiveFeed.UI
//{
//    /// <summary>
//    /// Summary description for VChart.
//    /// </summary>
//    public class VChart : System.Windows.Forms.Form,ICharts
//    { 
//        #region Private Fields

//        private const string FORM_NAME = "VChart :";
//        public AxSTOCKCHARTXLib.AxStockChartX axStockChartX1; 
//        private System.Windows.Forms.MainMenu mainMenu1;
//        private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager ultraToolbarsManager1;
//        private System.Windows.Forms.Panel VChart_Fill_Panel;
//        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _VChart_Toolbars_Dock_Area_Left;
//        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _VChart_Toolbars_Dock_Area_Right;
//        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _VChart_Toolbars_Dock_Area_Top;
//        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _VChart_Toolbars_Dock_Area_Bottom;
//        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor ultraCboStartTime;
//        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor ultraCboEndTime;
//        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor ultraCboStratDate;
//        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor ultraCboEndDate;

//        private System.ComponentModel.IContainer components;
//        LiveFeedPreferenceManager _liveFeedPref = null;
//        ChartsPreferences _chartsPreferences = null;
//        private const string SUB_MODULE_NAME = "Charts";

//        bool fillCombo = false;

//        #endregion Private Fields

//        #region Constructor

//        public VChart()
//        {	
//            InitializeComponent();
//            InitChartsGridPreferences();

//            SyncServerManager.GetInstance().fireTicData +=new EventHandler(VChart_fireTicData);
//            SyncServerManager.GetInstance().fireTickBarData +=new EventHandler(VChart_fireTickBarData);
//            SyncServerManager.GetInstance().fireRealTicData +=new EventHandler(VChart_fireRealTicData);
//            fillCombo = true;	
//        } 

//        #endregion Constructor

//        #region Dispose

//        /// <summary>
//        /// Clean up any resources being used.
//        /// </summary>
//        protected override void Dispose( bool disposing )
//        {
//            if( disposing )
//            {
//                if(components != null)
//                {
//                    components.Dispose();
//                }
//            }
//            base.Dispose( disposing );
//            SyncServerManager.GetInstance().fireTickBarData -= new EventHandler(VChart_fireTickBarData);
//            SyncServerManager.GetInstance().fireTicData -=new EventHandler(VChart_fireTicData);
//            SyncServerManager.GetInstance().fireRealTicData -=new EventHandler(VChart_fireRealTicData);
//        }

//        #endregion Dispose

//        #region Windows Form Designer generated code
//        /// <summary>
//        /// Required method for Designer support - do not modify
//        /// the contents of this method with the code editor.
//        /// </summary>
//        private void InitializeComponent()
//        {
//            this.components = new System.ComponentModel.Container();
//            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(VChart));
//            Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar1 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("IconToolbar");
//            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnZoomIn");
//            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnZoomOut");
//            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool3 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnMoveLeft");
//            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool4 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnMoveRight");
//            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool5 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnPreferences");
//            Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar2 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("StandardToolBar");
//            Infragistics.Win.UltraWinToolbars.LabelTool labelTool1 = new Infragistics.Win.UltraWinToolbars.LabelTool("lblSymbol");
//            Infragistics.Win.UltraWinToolbars.TextBoxTool textBoxTool1 = new Infragistics.Win.UltraWinToolbars.TextBoxTool("txtSymbol");
//            Infragistics.Win.UltraWinToolbars.LabelTool labelTool2 = new Infragistics.Win.UltraWinToolbars.LabelTool("lblStartTime");
//            Infragistics.Win.UltraWinToolbars.ControlContainerTool controlContainerTool1 = new Infragistics.Win.UltraWinToolbars.ControlContainerTool("startTimeContainer");
//            Infragistics.Win.UltraWinToolbars.LabelTool labelTool3 = new Infragistics.Win.UltraWinToolbars.LabelTool("lblEndTime");
//            Infragistics.Win.UltraWinToolbars.ControlContainerTool controlContainerTool2 = new Infragistics.Win.UltraWinToolbars.ControlContainerTool("endTimeContainer");
//            Infragistics.Win.UltraWinToolbars.LabelTool labelTool4 = new Infragistics.Win.UltraWinToolbars.LabelTool("lblStartDate");
//            Infragistics.Win.UltraWinToolbars.ControlContainerTool controlContainerTool3 = new Infragistics.Win.UltraWinToolbars.ControlContainerTool("startDateContainer");
//            Infragistics.Win.UltraWinToolbars.LabelTool labelTool5 = new Infragistics.Win.UltraWinToolbars.LabelTool("lblEndDate");
//            Infragistics.Win.UltraWinToolbars.ControlContainerTool controlContainerTool4 = new Infragistics.Win.UltraWinToolbars.ControlContainerTool("endDateContainer");
//            Infragistics.Win.UltraWinToolbars.LabelTool labelTool6 = new Infragistics.Win.UltraWinToolbars.LabelTool("lblInterval");
//            Infragistics.Win.UltraWinToolbars.ComboBoxTool comboBoxTool1 = new Infragistics.Win.UltraWinToolbars.ComboBoxTool("cboInterval");
//            Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar3 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("Optional Toolbar");
//            Infragistics.Win.UltraWinToolbars.LabelTool labelTool7 = new Infragistics.Win.UltraWinToolbars.LabelTool("lblIndicator");
//            Infragistics.Win.UltraWinToolbars.ComboBoxTool comboBoxTool2 = new Infragistics.Win.UltraWinToolbars.ComboBoxTool("cboIndicators");
//            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool6 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnAddIndicator");
//            Infragistics.Win.UltraWinToolbars.LabelTool labelTool8 = new Infragistics.Win.UltraWinToolbars.LabelTool("lblPriceStyle");
//            Infragistics.Win.UltraWinToolbars.ComboBoxTool comboBoxTool3 = new Infragistics.Win.UltraWinToolbars.ComboBoxTool("cboPriceStyle");
//            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool7 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnZoomIn");
//            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
//            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool8 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnZoomOut");
//            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
//            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool9 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnMoveLeft");
//            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
//            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
//            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool10 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnMoveRight");
//            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
//            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool11 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnPreferences");
//            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
//            Infragistics.Win.UltraWinToolbars.LabelTool labelTool9 = new Infragistics.Win.UltraWinToolbars.LabelTool("lblSymbol");
//            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
//            Infragistics.Win.UltraWinToolbars.TextBoxTool textBoxTool2 = new Infragistics.Win.UltraWinToolbars.TextBoxTool("txtSymbol");
//            Infragistics.Win.UltraWinToolbars.LabelTool labelTool10 = new Infragistics.Win.UltraWinToolbars.LabelTool("lblStartTime");
//            Infragistics.Win.UltraWinToolbars.ControlContainerTool controlContainerTool5 = new Infragistics.Win.UltraWinToolbars.ControlContainerTool("startTimeContainer");
//            Infragistics.Win.UltraWinToolbars.LabelTool labelTool11 = new Infragistics.Win.UltraWinToolbars.LabelTool("lblEndTime");
//            Infragistics.Win.UltraWinToolbars.ControlContainerTool controlContainerTool6 = new Infragistics.Win.UltraWinToolbars.ControlContainerTool("endTimeContainer");
//            Infragistics.Win.UltraWinToolbars.LabelTool labelTool12 = new Infragistics.Win.UltraWinToolbars.LabelTool("lblStartDate");
//            Infragistics.Win.UltraWinToolbars.ControlContainerTool controlContainerTool7 = new Infragistics.Win.UltraWinToolbars.ControlContainerTool("startDateContainer");
//            Infragistics.Win.UltraWinToolbars.LabelTool labelTool13 = new Infragistics.Win.UltraWinToolbars.LabelTool("lblEndDate");
//            Infragistics.Win.UltraWinToolbars.ControlContainerTool controlContainerTool8 = new Infragistics.Win.UltraWinToolbars.ControlContainerTool("endDateContainer");
//            Infragistics.Win.UltraWinToolbars.LabelTool labelTool14 = new Infragistics.Win.UltraWinToolbars.LabelTool("lblInterval");
//            Infragistics.Win.UltraWinToolbars.ComboBoxTool comboBoxTool4 = new Infragistics.Win.UltraWinToolbars.ComboBoxTool("cboInterval");
//            Infragistics.Win.ValueList valueList1 = new Infragistics.Win.ValueList(0);
//            Infragistics.Win.ValueListItem valueListItem1 = new Infragistics.Win.ValueListItem();
//            Infragistics.Win.ValueListItem valueListItem2 = new Infragistics.Win.ValueListItem();
//            Infragistics.Win.ValueListItem valueListItem3 = new Infragistics.Win.ValueListItem();
//            Infragistics.Win.ValueListItem valueListItem4 = new Infragistics.Win.ValueListItem();
//            Infragistics.Win.ValueListItem valueListItem5 = new Infragistics.Win.ValueListItem();
//            Infragistics.Win.ValueListItem valueListItem6 = new Infragistics.Win.ValueListItem();
//            Infragistics.Win.ValueListItem valueListItem7 = new Infragistics.Win.ValueListItem();
//            Infragistics.Win.ValueListItem valueListItem8 = new Infragistics.Win.ValueListItem();
//            Infragistics.Win.ValueListItem valueListItem9 = new Infragistics.Win.ValueListItem();
//            Infragistics.Win.ValueListItem valueListItem10 = new Infragistics.Win.ValueListItem();
//            Infragistics.Win.ValueListItem valueListItem11 = new Infragistics.Win.ValueListItem();
//            Infragistics.Win.UltraWinToolbars.LabelTool labelTool15 = new Infragistics.Win.UltraWinToolbars.LabelTool("lblIndicator");
//            Infragistics.Win.UltraWinToolbars.ComboBoxTool comboBoxTool5 = new Infragistics.Win.UltraWinToolbars.ComboBoxTool("cboIndicators");
//            Infragistics.Win.ValueList valueList2 = new Infragistics.Win.ValueList(0);
//            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool12 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnAddIndicator");
//            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
//            Infragistics.Win.UltraWinToolbars.LabelTool labelTool16 = new Infragistics.Win.UltraWinToolbars.LabelTool("lblPriceStyle");
//            Infragistics.Win.UltraWinToolbars.ComboBoxTool comboBoxTool6 = new Infragistics.Win.UltraWinToolbars.ComboBoxTool("cboPriceStyle");
//            Infragistics.Win.ValueList valueList3 = new Infragistics.Win.ValueList(0);
//            this.ultraCboStartTime = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
//            this.ultraCboEndTime = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
//            this.ultraCboStratDate = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
//            this.ultraCboEndDate = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
//            this.axStockChartX1 = new AxSTOCKCHARTXLib.AxStockChartX();
//            this.mainMenu1 = new System.Windows.Forms.MainMenu();
//            this.ultraToolbarsManager1 = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
//            this.VChart_Fill_Panel = new System.Windows.Forms.Panel();
//            this._VChart_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
//            this._VChart_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
//            this._VChart_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
//            this._VChart_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
//            ((System.ComponentModel.ISupportInitialize)(this.ultraCboStartTime)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.ultraCboEndTime)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.ultraCboStratDate)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.ultraCboEndDate)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.axStockChartX1)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.ultraToolbarsManager1)).BeginInit();
//            this.VChart_Fill_Panel.SuspendLayout();
//            this.SuspendLayout();
//            // 
//            // ultraCboStartTime
//            // 
//            this.ultraCboStartTime.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2003;
//            this.ultraCboStartTime.DropDownButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Never;
//            this.ultraCboStartTime.FormatProvider = new System.Globalization.CultureInfo("en-US");
//            this.ultraCboStartTime.Location = new System.Drawing.Point(10, 130);
//            this.ultraCboStartTime.MaskInput = "{LOC}hh:mm:ss tt";
//            this.ultraCboStartTime.Name = "ultraCboStartTime";
//            this.ultraCboStartTime.Size = new System.Drawing.Size(72, 21);
//            this.ultraCboStartTime.TabIndex = 21;
//            // 
//            // ultraCboEndTime
//            // 
//            this.ultraCboEndTime.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2003;
//            this.ultraCboEndTime.DropDownButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Never;
//            this.ultraCboEndTime.FormatProvider = new System.Globalization.CultureInfo("en-US");
//            this.ultraCboEndTime.FormatString = "";
//            this.ultraCboEndTime.Location = new System.Drawing.Point(44, 174);
//            this.ultraCboEndTime.MaskInput = "{LOC}hh:mm:ss tt";
//            this.ultraCboEndTime.Name = "ultraCboEndTime";
//            this.ultraCboEndTime.Size = new System.Drawing.Size(72, 21);
//            this.ultraCboEndTime.TabIndex = 22;
//            // 
//            // ultraCboStratDate
//            // 
//            this.ultraCboStratDate.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2003;
//            this.ultraCboStratDate.FormatProvider = new System.Globalization.CultureInfo("en-US");
//            this.ultraCboStratDate.Location = new System.Drawing.Point(56, 94);
//            this.ultraCboStratDate.MaskInput = "{LOC}mm/dd/yyyy";
//            this.ultraCboStratDate.Name = "ultraCboStratDate";
//            this.ultraCboStratDate.Size = new System.Drawing.Size(90, 21);
//            this.ultraCboStratDate.TabIndex = 23;
//            // 
//            // ultraCboEndDate
//            // 
//            this.ultraCboEndDate.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2003;
//            this.ultraCboEndDate.FormatProvider = new System.Globalization.CultureInfo("en-US");
//            this.ultraCboEndDate.Location = new System.Drawing.Point(72, 104);
//            this.ultraCboEndDate.MaskInput = "{LOC}mm/dd/yyyy";
//            this.ultraCboEndDate.Name = "ultraCboEndDate";
//            this.ultraCboEndDate.Size = new System.Drawing.Size(90, 21);
//            this.ultraCboEndDate.TabIndex = 24;
//            // 
//            // axStockChartX1
//            // 
//            this.axStockChartX1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
//                | System.Windows.Forms.AnchorStyles.Left) 
//                | System.Windows.Forms.AnchorStyles.Right)));
//            this.axStockChartX1.ContainingControl = this;
//            this.axStockChartX1.Enabled = true;
//            this.axStockChartX1.Location = new System.Drawing.Point(0, 0);
//            this.axStockChartX1.Name = "axStockChartX1";
//            this.axStockChartX1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axStockChartX1.OcxState")));
//            this.axStockChartX1.Size = new System.Drawing.Size(940, 428);
//            this.axStockChartX1.TabIndex = 4;
//            this.axStockChartX1.EnumIndicator += new AxSTOCKCHARTXLib._DStockChartXEvents_EnumIndicatorEventHandler(this.axStockChartX1_EnumIndicators);
//            this.axStockChartX1.EnumPriceStyle += new AxSTOCKCHARTXLib._DStockChartXEvents_EnumPriceStyleEventHandler(this.axStockChartX1_EnumPriceStyles);
//            // 
//            // ultraToolbarsManager1
//            // 
//            this.ultraToolbarsManager1.DesignerFlags = 0;
//            this.ultraToolbarsManager1.DockWithinContainer = this;
//            this.ultraToolbarsManager1.ShowFullMenusDelay = 500;
//            ultraToolbar1.DockedColumn = 1;
//            ultraToolbar1.DockedRow = 1;
//            ultraToolbar1.FloatingLocation = new System.Drawing.Point(136, 370);
//            ultraToolbar1.FloatingSize = new System.Drawing.Size(100, 64);
//            ultraToolbar1.Text = "IconToolbar";
//            ultraToolbar1.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
//                                                                                              buttonTool1,
//                                                                                              buttonTool2,
//                                                                                              buttonTool3,
//                                                                                              buttonTool4,
//                                                                                              buttonTool5});
//            ultraToolbar2.DockedColumn = 0;
//            ultraToolbar2.DockedRow = 0;
//            ultraToolbar2.Text = "StandardToolBar";
//            labelTool1.InstanceProps.Width = 44;
//            textBoxTool1.InstanceProps.Width = 66;
//            labelTool2.InstanceProps.Width = 54;
//            controlContainerTool1.Control = this.ultraCboStartTime;
//            controlContainerTool1.InstanceProps.Width = 74;
//            labelTool3.InstanceProps.Width = 50;
//            controlContainerTool2.Control = this.ultraCboEndTime;
//            controlContainerTool2.InstanceProps.Width = 74;
//            labelTool4.InstanceProps.Width = 53;
//            controlContainerTool3.Control = this.ultraCboStratDate;
//            labelTool5.InstanceProps.Width = 47;
//            controlContainerTool4.Control = this.ultraCboEndDate;
//            labelTool6.InstanceProps.Width = 47;
//            comboBoxTool1.InstanceProps.Width = 91;
//            ultraToolbar2.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
//                                                                                              labelTool1,
//                                                                                              textBoxTool1,
//                                                                                              labelTool2,
//                                                                                              controlContainerTool1,
//                                                                                              labelTool3,
//                                                                                              controlContainerTool2,
//                                                                                              labelTool4,
//                                                                                              controlContainerTool3,
//                                                                                              labelTool5,
//                                                                                              controlContainerTool4,
//                                                                                              labelTool6,
//                                                                                              comboBoxTool1});
//            ultraToolbar3.DockedColumn = 0;
//            ultraToolbar3.DockedRow = 1;
//            ultraToolbar3.Text = "Optional Toolbar";
//            labelTool7.InstanceProps.Width = 51;
//            comboBoxTool2.InstanceProps.Width = 210;
//            labelTool8.InstanceProps.Width = 58;
//            ultraToolbar3.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
//                                                                                              labelTool7,
//                                                                                              comboBoxTool2,
//                                                                                              buttonTool6,
//                                                                                              labelTool8,
//                                                                                              comboBoxTool3});
//            this.ultraToolbarsManager1.Toolbars.AddRange(new Infragistics.Win.UltraWinToolbars.UltraToolbar[] {
//                                                                                                                  ultraToolbar1,
//                                                                                                                  ultraToolbar2,
//                                                                                                                  ultraToolbar3});
//            appearance1.Image = ((object)(resources.GetObject("appearance1.Image")));
//            buttonTool7.SharedProps.AppearancesSmall.Appearance = appearance1;
//            buttonTool7.SharedProps.Caption = "Zoom In";
//            appearance2.Image = ((object)(resources.GetObject("appearance2.Image")));
//            buttonTool8.SharedProps.AppearancesSmall.Appearance = appearance2;
//            buttonTool8.SharedProps.Caption = "Zoom Out";
//            appearance3.Image = ((object)(resources.GetObject("appearance3.Image")));
//            buttonTool9.SharedProps.AppearancesLarge.Appearance = appearance3;
//            appearance4.Image = ((object)(resources.GetObject("appearance4.Image")));
//            buttonTool9.SharedProps.AppearancesSmall.Appearance = appearance4;
//            buttonTool9.SharedProps.Caption = "Move Left";
//            appearance5.Image = ((object)(resources.GetObject("appearance5.Image")));
//            buttonTool10.SharedProps.AppearancesSmall.Appearance = appearance5;
//            buttonTool10.SharedProps.Caption = "Move Right";
//            appearance6.Image = ((object)(resources.GetObject("appearance6.Image")));
//            buttonTool11.SharedProps.AppearancesSmall.Appearance = appearance6;
//            buttonTool11.SharedProps.Caption = "Preferences";
//            appearance7.FontData.Name = "Tahoma";
//            labelTool9.SharedProps.AppearancesSmall.Appearance = appearance7;
//            labelTool9.SharedProps.Caption = "Symbol";
//            labelTool9.SharedProps.MaxWidth = 10;
//            labelTool9.SharedProps.MinWidth = 5;
//            textBoxTool2.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
//            textBoxTool2.SharedProps.Caption = "txtSymbol";
//            textBoxTool2.SharedProps.MaxWidth = 10;
//            textBoxTool2.SharedProps.MinWidth = 5;
//            textBoxTool2.Text = "MSFT";
//            labelTool10.SharedProps.Caption = "StartTime";
//            labelTool10.SharedProps.MaxWidth = 10;
//            labelTool10.SharedProps.MinWidth = 5;
//            controlContainerTool5.Control = this.ultraCboStartTime;
//            controlContainerTool5.SharedProps.Caption = "startTimeContainer";
//            controlContainerTool5.SharedProps.Width = 74;
//            labelTool11.SharedProps.Caption = "EndTime";
//            labelTool11.SharedProps.MaxWidth = 10;
//            labelTool11.SharedProps.MinWidth = 5;
//            controlContainerTool6.Control = this.ultraCboEndTime;
//            controlContainerTool6.SharedProps.Caption = "endTimeContainer";
//            controlContainerTool6.SharedProps.Width = 74;
//            labelTool12.SharedProps.Caption = "StartDate";
//            labelTool12.SharedProps.MaxWidth = 10;
//            labelTool12.SharedProps.MinWidth = 5;
//            controlContainerTool7.Control = this.ultraCboStratDate;
//            controlContainerTool7.SharedProps.Caption = "startDateContainer";
//            labelTool13.SharedProps.Caption = "EndDate";
//            labelTool13.SharedProps.MaxWidth = 10;
//            labelTool13.SharedProps.MinWidth = 5;
//            controlContainerTool8.Control = this.ultraCboEndDate;
//            controlContainerTool8.SharedProps.Caption = "endDateContainer";
//            labelTool14.SharedProps.Caption = "Interval";
//            labelTool14.SharedProps.MaxWidth = 10;
//            labelTool14.SharedProps.MinWidth = 5;
//            comboBoxTool4.SharedProps.Caption = "cboInterval";
//            valueListItem1.DataValue = ApplicationConstants.C_COMBO_SELECT;
//            valueListItem2.DataValue = "Tick";
//            valueListItem2.DisplayText = "Tick";
//            valueListItem3.DataValue = "RealTick";
//            valueListItem3.DisplayText = "RealTick";
//            valueListItem4.DataValue = "1";
//            valueListItem4.DisplayText = "1";
//            valueListItem5.DataValue = "3";
//            valueListItem5.DisplayText = "3";
//            valueListItem6.DataValue = "5";
//            valueListItem6.DisplayText = "5";
//            valueListItem7.DataValue = "10";
//            valueListItem7.DisplayText = "10";
//            valueListItem8.DataValue = "13";
//            valueListItem8.DisplayText = "13";
//            valueListItem9.DataValue = "15";
//            valueListItem9.DisplayText = "15";
//            valueListItem10.DataValue = "30";
//            valueListItem10.DisplayText = "30";
//            valueListItem11.DataValue = "60";
//            valueListItem11.DisplayText = "60";
//            valueList1.ValueListItems.Add(valueListItem1);
//            valueList1.ValueListItems.Add(valueListItem2);
//            valueList1.ValueListItems.Add(valueListItem3);
//            valueList1.ValueListItems.Add(valueListItem4);
//            valueList1.ValueListItems.Add(valueListItem5);
//            valueList1.ValueListItems.Add(valueListItem6);
//            valueList1.ValueListItems.Add(valueListItem7);
//            valueList1.ValueListItems.Add(valueListItem8);
//            valueList1.ValueListItems.Add(valueListItem9);
//            valueList1.ValueListItems.Add(valueListItem10);
//            valueList1.ValueListItems.Add(valueListItem11);
//            comboBoxTool4.ValueList = valueList1;
//            labelTool15.SharedProps.Caption = "Indicator";
//            labelTool15.SharedProps.MaxWidth = 10;
//            labelTool15.SharedProps.MinWidth = 5;
//            comboBoxTool5.SharedProps.Caption = "cboIndicators";
//            comboBoxTool5.ValueList = valueList2;
//            appearance8.Image = ((object)(resources.GetObject("appearance8.Image")));
//            buttonTool12.SharedProps.AppearancesSmall.Appearance = appearance8;
//            buttonTool12.SharedProps.Caption = "Add Indicator";
//            buttonTool12.SharedProps.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageOnlyOnToolbars;
//            labelTool16.SharedProps.Caption = "Price Style";
//            labelTool16.SharedProps.MaxWidth = 10;
//            labelTool16.SharedProps.MinWidth = 5;
//            labelTool16.SharedProps.Visible = false;
//            comboBoxTool6.SharedProps.Caption = "cboPriceStyle";
//            comboBoxTool6.SharedProps.Visible = false;
//            comboBoxTool6.ValueList = valueList3;
//            this.ultraToolbarsManager1.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
//                                                                                                           buttonTool7,
//                                                                                                           buttonTool8,
//                                                                                                           buttonTool9,
//                                                                                                           buttonTool10,
//                                                                                                           buttonTool11,
//                                                                                                           labelTool9,
//                                                                                                           textBoxTool2,
//                                                                                                           labelTool10,
//                                                                                                           controlContainerTool5,
//                                                                                                           labelTool11,
//                                                                                                           controlContainerTool6,
//                                                                                                           labelTool12,
//                                                                                                           controlContainerTool7,
//                                                                                                           labelTool13,
//                                                                                                           controlContainerTool8,
//                                                                                                           labelTool14,
//                                                                                                           comboBoxTool4,
//                                                                                                           labelTool15,
//                                                                                                           comboBoxTool5,
//                                                                                                           buttonTool12,
//                                                                                                           labelTool16,
//                                                                                                           comboBoxTool6});
//            this.ultraToolbarsManager1.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.ultraToolbarsManager1_ToolClick);
//            this.ultraToolbarsManager1.ToolValueChanged += new Infragistics.Win.UltraWinToolbars.ToolEventHandler(this.ultraToolbarsManager1_ToolValueChanged);
//            // 
//            // VChart_Fill_Panel
//            // 
//            this.VChart_Fill_Panel.Controls.Add(this.ultraCboEndDate);
//            this.VChart_Fill_Panel.Controls.Add(this.ultraCboStratDate);
//            this.VChart_Fill_Panel.Controls.Add(this.ultraCboEndTime);
//            this.VChart_Fill_Panel.Controls.Add(this.ultraCboStartTime);
//            this.VChart_Fill_Panel.Controls.Add(this.axStockChartX1);
//            this.VChart_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
//            this.VChart_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.VChart_Fill_Panel.Location = new System.Drawing.Point(0, 48);
//            this.VChart_Fill_Panel.Name = "VChart_Fill_Panel";
//            this.VChart_Fill_Panel.Size = new System.Drawing.Size(938, 429);
//            this.VChart_Fill_Panel.TabIndex = 0;
//            // 
//            // _VChart_Toolbars_Dock_Area_Left
//            // 
//            this._VChart_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
//            this._VChart_Toolbars_Dock_Area_Left.BackColor = System.Drawing.SystemColors.Control;
//            this._VChart_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
//            this._VChart_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
//            this._VChart_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 48);
//            this._VChart_Toolbars_Dock_Area_Left.Name = "_VChart_Toolbars_Dock_Area_Left";
//            this._VChart_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 429);
//            this._VChart_Toolbars_Dock_Area_Left.ToolbarsManager = this.ultraToolbarsManager1;
//            // 
//            // _VChart_Toolbars_Dock_Area_Right
//            // 
//            this._VChart_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
//            this._VChart_Toolbars_Dock_Area_Right.BackColor = System.Drawing.SystemColors.Control;
//            this._VChart_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
//            this._VChart_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
//            this._VChart_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(938, 48);
//            this._VChart_Toolbars_Dock_Area_Right.Name = "_VChart_Toolbars_Dock_Area_Right";
//            this._VChart_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 429);
//            this._VChart_Toolbars_Dock_Area_Right.ToolbarsManager = this.ultraToolbarsManager1;
//            // 
//            // _VChart_Toolbars_Dock_Area_Top
//            // 
//            this._VChart_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
//            this._VChart_Toolbars_Dock_Area_Top.BackColor = System.Drawing.SystemColors.Control;
//            this._VChart_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
//            this._VChart_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
//            this._VChart_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
//            this._VChart_Toolbars_Dock_Area_Top.Name = "_VChart_Toolbars_Dock_Area_Top";
//            this._VChart_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(938, 48);
//            this._VChart_Toolbars_Dock_Area_Top.ToolbarsManager = this.ultraToolbarsManager1;
//            // 
//            // _VChart_Toolbars_Dock_Area_Bottom
//            // 
//            this._VChart_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
//            this._VChart_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.SystemColors.Control;
//            this._VChart_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
//            this._VChart_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
//            this._VChart_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 477);
//            this._VChart_Toolbars_Dock_Area_Bottom.Name = "_VChart_Toolbars_Dock_Area_Bottom";
//            this._VChart_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(938, 0);
//            this._VChart_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.ultraToolbarsManager1;
//            // 
//            // VChart
//            // 
//            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
//            this.ClientSize = new System.Drawing.Size(938, 477);
//            this.Controls.Add(this.VChart_Fill_Panel);
//            this.Controls.Add(this._VChart_Toolbars_Dock_Area_Left);
//            this.Controls.Add(this._VChart_Toolbars_Dock_Area_Right);
//            this.Controls.Add(this._VChart_Toolbars_Dock_Area_Top);
//            this.Controls.Add(this._VChart_Toolbars_Dock_Area_Bottom);
//            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
//            this.Menu = this.mainMenu1;
//            this.Name = "VChart";
//            this.Text = "Charts";
//            this.Load += new System.EventHandler(this.VChart_Load);
//            this.Activated += new System.EventHandler(this.VChart_Activated);
//            ((System.ComponentModel.ISupportInitialize)(this.ultraCboStartTime)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.ultraCboEndTime)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.ultraCboStratDate)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.ultraCboEndDate)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.axStockChartX1)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.ultraToolbarsManager1)).EndInit();
//            this.VChart_Fill_Panel.ResumeLayout(false);
//            this.ResumeLayout(false);

//        }
//        #endregion

//        #region Public methods

//        /// <summary>
//        /// Load the color preferences
//        /// </summary>
//        public void InitChartsGridPreferences()
//        {
//            try
//            {
//                _liveFeedPref = LiveFeedPreferenceManager.GetInstance() ;
//                _liveFeedPref.SubModuleName = SUB_MODULE_NAME ;
//                _chartsPreferences = (ChartsPreferences)_liveFeedPref.GetPreferences();
//                ChartsPreferences.ChartsGeneralPreferences chartsGeneralPref =  _chartsPreferences.generalPreference; 
//                ChartsPreferences.ChartsColorPreferences chartsColorPref =  _chartsPreferences.colorPreference; 

//                #region General 

//                ///For darvas box 
//                axStockChartX1.DarvasBoxes = chartsGeneralPref.DisplayDarvasBox;
//                axStockChartX1.DarvasStopPercent = 0.01;

//                ///For Wide candles
//                if(chartsGeneralPref.ShowWideCandles)
//                {
//                    axStockChartX1.BarWidth = 10;
//                }
//                else
//                {
//                    axStockChartX1.BarWidth = 1;
//                }

//                // Change it's up/down colors
//                //			UInt32 red = System.Convert.ToUInt32(System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red));
//                //			UInt32 white = System.Convert.ToUInt32(System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White));
//                //			StockChartX1.SetSeriesUpDownColors("My SMA", red, white);

//                #endregion General 

//                #region Color

//                axStockChartX1.ChartForeColor = LiveFeedPreferenceManager.GetInstance().GetColorFromARGB(chartsColorPref.ForegroundColor);
//                axStockChartX1.ChartBackColor = LiveFeedPreferenceManager.GetInstance().GetColorFromARGB(chartsColorPref.BackgroundColor);
//                axStockChartX1.Gridcolor = LiveFeedPreferenceManager.GetInstance().GetColorFromARGB(chartsColorPref.GridColor);
//                axStockChartX1.UpColor = LiveFeedPreferenceManager.GetInstance().GetColorFromARGB(chartsColorPref.UpTickColor);
//                axStockChartX1.DownColor = LiveFeedPreferenceManager.GetInstance().GetColorFromARGB(chartsColorPref.DownTickColor);

//                ///For single bar color
//                //			int bar = 0;
//                //			string fetchedValue = InputBox.ShowInputBox("Bar Color");
//                //			if(fetchedValue == "") fetchedValue = "0";
//                //			bar = int.Parse(fetchedValue);
//                //
//                //			if(bar != 0 || bar < axStockChartX1.RecordCount)
//                //			{
//                //				fetchedValue = InputBox.ShowInputBox("Series Name");
//                //				if(fetchedValue == "") fetchedValue = "0";
//                //				if(bar != 0 )
//                //					axStockChartX1.set_BarColor(bar, axStockChartX1.Symbol + ".close", System.Drawing.ColorTranslator.ToOle(LiveFeedPreferenceManager.GetInstance().GetColorFromARGB(chartsColorPref.SingleBarColor))); 
//                //			}

//                axStockChartX1.BackGradientTop = LiveFeedPreferenceManager.GetInstance().GetColorFromARGB(chartsColorPref.GradientTopColor);
//                axStockChartX1.BackGradientBottom = LiveFeedPreferenceManager.GetInstance().GetColorFromARGB(chartsColorPref.GradientBottomColor);


//                // StockChartX1.Update(); // For VS.NET 2005
//                axStockChartX1.CtlUpdate(); // For VS.NET 2003 and under

//                #endregion Color 
//            }
//            catch (Exception ex)
//            {
//                // Invoke our policy that is responsible for making sure no secure information
//                // gets out of our layer.
//                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

//                if (rethrow)
//                {
//                    throw;
//                }
//            }


//        }

//        #endregion Public methods

//        #region Private methods

//        private double GetJDate(string szDate)
//        {
//            int hr = 0, mn = 0, sc = 0;
//            DateTime date;
//            try
//            {

//                try
//                {
//                    date = DateTime.Parse(szDate);
//                }
//                catch
//                {
//                    return -1;
//                }

//                // Default to midnight if no time
//                mn = date.Minute;
//                sc = date.Second;
//                if (hr == 0) hr = 12;

//                // Get a Julian date from the StockChartX control
//                szDate = axStockChartX1.ToJulianDate(date.Year, date.Month, date.Day, mn, hr, sc).ToString();

//                return Double.Parse(szDate);
//            }
//            catch (Exception ex)
//            {
//                // Invoke our policy that is responsible for making sure no secure information
//                // gets out of our layer.
//                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

//                if (rethrow)
//                {
//                    throw;
//                }
//                return double.MinValue;
//            }
//        }

//        private void SetSymbolFocus()
//        {
//            try
//            {
//                TextBoxTool textTool = this.ultraToolbarsManager1.Tools["txtSymbol"] as TextBoxTool; 
//                PutToolInEditMode(textTool); 
//            }
//            catch (Exception ex)
//            {
//                // Invoke our policy that is responsible for making sure no secure information
//                // gets out of our layer.
//                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

//                if (rethrow)
//                {
//                    throw;
//                }
//            }
//        }

//        private void PutToolInEditMode(ToolBase tool) 
//        {
//            try
//            {
//                foreach (ToolBase toolInstance in tool.SharedProps.ToolInstances)
//                {
//                    if (toolInstance.Key.Equals("txtSymbol"))
//                    {
//                        TextBoxTool textTool = toolInstance.UnderlyingTool as TextBoxTool;
//                        textTool.IsInEditMode = true;

//                        textTool.SelectionLength = 0;
//                        textTool.SelectionStart = 0;
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                // Invoke our policy that is responsible for making sure no secure information
//                // gets out of our layer.
//                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

//                if (rethrow)
//                {
//                    throw;
//                }
//            }
//        }

//        #endregion Private methods

//        #region Form Events

//        private void VChart_Activated(object sender, EventArgs e)
//        {
//            SetSymbolFocus();
//        }

//        private void VChart_Load(object sender, System.EventArgs e)
//        {
////			ultraToolbarsManager1.Toolbars[0].Visible = true;
////			ultraToolbarsManager1.Toolbars[1].Visible = true;
////			ultraToolbarsManager1.Toolbars[2].Visible = true;

//            try
//            {
//                ultraCboStratDate.Value = DateTime.Now.Subtract(TimeSpan.FromDays(1));
//                ultraCboEndDate.Value = DateTime.Today;
//                ultraCboStartTime.Value = DateTime.Now.Subtract(TimeSpan.FromHours(12));
//                ultraCboEndTime.Value = DateTime.Now;
//                ((Infragistics.Win.UltraWinToolbars.ComboBoxTool)ultraToolbarsManager1.Tools["cboInterval"]).SelectedIndex = 0;
//            }
//            catch (Exception ex)
//            {
//                // Invoke our policy that is responsible for making sure no secure information
//                // gets out of our layer.
//                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

//                if (rethrow)
//                {
//                    throw;
//                }
//            }
//        }

//        #endregion Form Events

//        #region Properties

//        string _symbol = string.Empty;
//        public string Symbol
//        {
//            get{return _symbol ;}
//            set{_symbol = value;}
//        }

//        #endregion

//        #region StockChartX Enum Indicators,PriceStyles Events

//        private void axStockChartX1_EnumIndicators(object sender, AxSTOCKCHARTXLib._DStockChartXEvents_EnumIndicatorEvent e)
//        {
//            Infragistics.Win.UltraWinToolbars.ComboBoxTool cboIndicatorTool = ((Infragistics.Win.UltraWinToolbars.ComboBoxTool)ultraToolbarsManager1.Tools["cboIndicators"]);
//            try
//            {
//                cboIndicatorTool.ValueList.ValueListItems.Add(e.indicatorName);
//                if (cboIndicatorTool.ValueList.ValueListItems.Count > 0)
//                    cboIndicatorTool.SelectedIndex = 0;
//            }
//            catch (Exception ex)
//            {
//                // Invoke our policy that is responsible for making sure no secure information
//                // gets out of our layer.
//                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

//                if (rethrow)
//                {
//                    throw;
//                }
//            }
//        }


//        private void axStockChartX1_EnumPriceStyles(object sender, AxSTOCKCHARTXLib._DStockChartXEvents_EnumPriceStyleEvent e)
//        {
//            try
//            {
//                ((Infragistics.Win.UltraWinToolbars.ComboBoxTool)ultraToolbarsManager1.Tools["cboPriceStyle"]).ValueList.ValueListItems.Add(e.priceStyleName);
//            }
//            catch (Exception ex)
//            {
//                // Invoke our policy that is responsible for making sure no secure information
//                // gets out of our layer.
//                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

//                if (rethrow)
//                {
//                    throw;
//                }
//            }

//        }


//        #endregion   

//        #region Received events from SyncServerManager

//        /// <summary>
//        /// For tick bar data
//        /// </summary>
//        /// <param name="sender"></param>
//        /// <param name="e"></param>
//        private void VChart_fireTickBarData(object sender, EventArgs e)
//        {
//            const int MAX_DISPLAY_RECORD = 50 ;
//             ArrayList receivedTicbarData = new ArrayList((ICollection)sender);
//            try
//            {

//                for (int i=0;i<receivedTicbarData.Count;i++)
//                {
//                    OHLCStruct barData = (OHLCStruct) receivedTicbarData[i];
//              //      double jdateMilli =( GetJDate(a.BarStartTime.ToString())*1000);
//                    double jdate = GetJDate(barData.BarStartTime.ToString());

//                    axStockChartX1.AppendValue(barData.symbol + ".open",jdate, barData.open);
//                    axStockChartX1.AppendValue(barData.symbol + ".high",jdate, barData.high);
//                    axStockChartX1.AppendValue(barData.symbol + ".low", jdate, barData.low);
//                    axStockChartX1.AppendValue(barData.symbol + ".close",jdate, barData.close);
//                    axStockChartX1.AppendValue(barData.symbol + ".volume",jdate, barData.volume);
//                }

//                axStockChartX1.MaxDisplayRecords = MAX_DISPLAY_RECORD;

//                // Change some display properties:

//                axStockChartX1.ThreeDStyle = true;

//                axStockChartX1.UpColor = System.Drawing.Color.Lime;
//                axStockChartX1.DownColor = System.Drawing.Color.Red;

//                axStockChartX1.DisplayTitles = true;
//                axStockChartX1.BarWidth = 1;
//                axStockChartX1.ScalePrecision = 1;
//                axStockChartX1.Alignment = 1 ;
//                axStockChartX1.RealTimeXLabels = true ;

//                // for custom Indicator
//                //double MyAlgorithm = CalculateMACDAlgorithm();
//                //axStockChartX1.SetCustomIndicatorData ("Moving Average Convergence / Divergence (MACD)", MyAlgorithm, false);


//                //Left or right scale alignment
//                //axStockChartX1.ScaleAlignment = 1; // Right (default)
//                //axStockChartX1.ScaleAlignment = 2; // Left
//                if(fillCombo)
//                { 
//                    fillCombo=false;

//                    axStockChartX1.EnumIndicators();
//                    axStockChartX1.EnumPriceStyles();
//                }

//                // Update the chart
//                // StockChartX1.Update(); // For VS.NET 2005
//                axStockChartX1.CtlUpdate(); // For VS.NET 2003 and under
//            }
//            catch (Exception ex)
//            {
//                // Invoke our policy that is responsible for making sure no secure information
//                // gets out of our layer.
//                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

//                if (rethrow)
//                {
//                    throw;
//                }
//            }
//        }



//        /// <summary>
//        /// For tick data
//        /// </summary>
//        /// <param name="sender"></param>
//        /// <param name="e"></param>
//        private void VChart_fireTicData(object sender, EventArgs e)
//        {   
//            const int MAX_DISPLAY_RECORD = 50;

//            ArrayList receivedTicData = new ArrayList((ICollection)sender);
//            try
//            {

//                for (int i=0;i<receivedTicData.Count;i++)
//                {
//                    TickStruct ticData = (TickStruct) receivedTicData[i];
//                    double jdate=GetJDate(ticData.TickTime.ToString());

//                    if(ticData.bidPrice != double.MinValue)
//                        axStockChartX1.AppendValue(ticData.symbol + ".BidPrice", jdate, double.Parse(ticData.bidPrice.ToString("F2")));
//                    else
//                        axStockChartX1.AppendValue(ticData.symbol + ".BidPrice", jdate,(double)STOCKCHARTXLib.DataType.dtNullValue);
//                    if(ticData.askPrice != double.MinValue)
//                        axStockChartX1.AppendValue(ticData.symbol + ".AskPrice",jdate,double.Parse(ticData.askPrice.ToString("F2"))); // double.Parse(b.askPrice.ToString("F2")));
//                    else
//                        axStockChartX1.AppendValue(ticData.symbol + ".AskPrice",jdate,(double)STOCKCHARTXLib.DataType.dtNullValue);

//                    if(ticData.tradePrice != double.MinValue)
//                        axStockChartX1.AppendValue(ticData.symbol + ".TradePrice",jdate,double.Parse(ticData.tradePrice.ToString("F2"))); // double.Parse(b.askPrice.ToString("F2")));
//                    else
//                        axStockChartX1.AppendValue(ticData.symbol + ".TradePrice",jdate,(double)STOCKCHARTXLib.DataType.dtNullValue);

////					Trade Volume is commented as it is not of same scale as of prices, Please don't add it back
////					if(b.tradeVolume != double.MinValue)
////						axStockChartX1.AppendValue(b.symbol + ".TradeVolume",jdate,Convert.ToInt32(b.tradeVolume.ToString())); // double.Parse(b.askPrice.ToString("F2")));
////					else
////						axStockChartX1.AppendValue(b.symbol + ".TradeVolume",jdate,(long)STOCKCHARTXLib.DataType.dtNullValue);

//                }

//                axStockChartX1.MaxDisplayRecords = MAX_DISPLAY_RECORD;

//                // Change some display properties:


//                axStockChartX1.UpColor = System.Drawing.Color.Lime;
//                axStockChartX1.DownColor = System.Drawing.Color.Red;


//                axStockChartX1.DisplayTitles = true;

//                axStockChartX1.ScalePrecision = 3;
//                axStockChartX1.Alignment = 1 ;
//                axStockChartX1.IgnoreSeriesLengthErrors = true ;

//                axStockChartX1.RealTimeXLabels = true ;
//                axStockChartX1.ResetYScale(0);
//                axStockChartX1.ResetYScale(1);axStockChartX1.ResetYScale(2);
//                //  axStockChartX1.YScaleMinTick =10;
////			axStockChartX1.ThreeDStyle = true;
//        //			axStockChartX1.BarWidth = 1;

//                // for custom Indicator
//                //	double MyAlgorithm = CalculateMACDAlgorithm();
//                //	axStockChartX1.SetCustomIndicatorData ("Moving Average Convergence / Divergence (MACD)", MyAlgorithm, false);


//                //Left or right scale alignment
//                //axStockChartX1.ScaleAlignment = 1; // Right (default)
//                //axStockChartX1.ScaleAlignment = 2; // Left

//                if(fillCombo)
//                { 
//                    fillCombo=false;

//                    axStockChartX1.EnumIndicators();
//                    axStockChartX1.EnumPriceStyles();
//                }


//                // Update the chart
//                // StockChartX1.Update(); // For VS.NET 2005
//                axStockChartX1.CtlUpdate(); // For VS.NET 2003 and under
//            }
//            catch (Exception ex)
//            {
//                // Invoke our policy that is responsible for making sure no secure information
//                // gets out of our layer.
//                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

//                if (rethrow)
//                {
//                    throw;
//                }
//            }
//        }
//        private void VChart_fireRealTicData(object sender, EventArgs e)
//        {
//            const int MAX_DISPLAY_RECORD = 50;

//            TickStruct realTicData = (TickStruct) sender;
//            try
//            {

//                if(realTicData.bidPrice != double.MinValue)
//                    axStockChartX1.AppendValue(realTicData.symbol + ".BidPrice", GetJDate(realTicData.TickTime.ToString()), double.Parse(realTicData.bidPrice.ToString("F2")));
//                else
//                    axStockChartX1.AppendValue(realTicData.symbol + ".BidPrice", GetJDate(realTicData.TickTime.ToString()),(double)STOCKCHARTXLib.DataType.dtNullValue);

//                if(realTicData.askPrice != double.MinValue)
//                    axStockChartX1.AppendValue(realTicData.symbol + ".AskPrice",GetJDate(realTicData.TickTime.ToString()),double.Parse(realTicData.askPrice.ToString("F2"))); // double.Parse(b.askPrice.ToString("F2")));
//                else
//                    axStockChartX1.AppendValue(realTicData.symbol + ".AskPrice",GetJDate(realTicData.TickTime.ToString()),(double)STOCKCHARTXLib.DataType.dtNullValue);

//                if(realTicData.tradePrice != double.MinValue)
//                    axStockChartX1.AppendValue(realTicData.symbol + ".TradePrice",GetJDate(realTicData.TickTime.ToString()),double.Parse(realTicData.tradePrice.ToString("F2"))); // double.Parse(b.askPrice.ToString("F2")));
//                else
//                    axStockChartX1.AppendValue(realTicData.symbol + ".TradePrice",GetJDate(realTicData.TickTime.ToString()),(double)STOCKCHARTXLib.DataType.dtNullValue);

//                axStockChartX1.MaxDisplayRecords = MAX_DISPLAY_RECORD;

//                // Change some display properties:

//                axStockChartX1.UpColor = System.Drawing.Color.Lime;
//                axStockChartX1.DownColor = System.Drawing.Color.Red;

//                //axStockChartX1.ThreeDStyle = true;
//                axStockChartX1.DisplayTitles = true;
//                //axStockChartX1.BarWidth = 1;   //default value
//                axStockChartX1.ScalePrecision = 1;
//                axStockChartX1.Alignment = 1 ;
//                axStockChartX1.IgnoreSeriesLengthErrors = true ;
//                //  axStockChartX1.YScaleMinTick =10;


//                // for custom Indicator
//                //	double MyAlgorithm = CalculateMACDAlgorithm();
//                //	axStockChartX1.SetCustomIndicatorData ("Moving Average Convergence / Divergence (MACD)", MyAlgorithm, false);


//                //Left or right scale alignment
//                //axStockChartX1.ScaleAlignment = 1; // Right (default)
//                //axStockChartX1.ScaleAlignment = 2; // Left

//                if(fillCombo)
//                { 
//                    fillCombo=false;

//                    axStockChartX1.EnumIndicators();
//                    axStockChartX1.EnumPriceStyles();
//                }


//                // Update the chart
//                // StockChartX1.Update(); // For VS.NET 2005
//                axStockChartX1.CtlUpdate(); // For VS.
//            }
//            catch (Exception ex)
//            {
//                // Invoke our policy that is responsible for making sure no secure information
//                // gets out of our layer.
//                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

//                if (rethrow)
//                {
//                    throw;
//                }
//            }
//        }

//        #endregion Received events from SyncServerManager

//        #region Toolbar Events

//        #region Selected Index Changed Events for the underlying combo box in Toolbar

//        private void cboPriceStyles_SelectedIndexChanged(object sender, System.EventArgs e)		
//        {			// NOTE: Make sure you don't set the combo box			// auto sort property to true! Enums are zero based.
//            try
//            {

//                // Set some properties			
//                if (((Infragistics.Win.UltraWinToolbars.ComboBoxTool)ultraToolbarsManager1.Tools["cboPriceStyle"]).SelectedIndex == (int)STOCKCHARTXLib.PriceStyle.psPointAndFigure)
//                {
//                    axStockChartX1.set_PriceStyleParam(1, 0); // Allow StockChartX to figure the box size				
//                    axStockChartX1.set_PriceStyleParam(2, 3); // Reversal siz			
//                }
//                else if (((Infragistics.Win.UltraWinToolbars.ComboBoxTool)ultraToolbarsManager1.Tools["cboPriceStyle"]).SelectedIndex == (int)STOCKCHARTXLib.PriceStyle.psThreeLineBreak)
//                {
//                    axStockChartX1.set_PriceStyleParam(1, 3); // Three line break (could be 1 to 50 line break)
//                }
//                else if (((Infragistics.Win.UltraWinToolbars.ComboBoxTool)ultraToolbarsManager1.Tools["cboPriceStyle"]).SelectedIndex == (int)STOCKCHARTXLib.PriceStyle.psRenko)
//                {
//                    axStockChartX1.set_PriceStyleParam(1, 1); // Box size
//                }
//                else if (((Infragistics.Win.UltraWinToolbars.ComboBoxTool)ultraToolbarsManager1.Tools["cboPriceStyle"]).SelectedIndex == (int)STOCKCHARTXLib.PriceStyle.psKagi)
//                {
//                    axStockChartX1.set_PriceStyleParam(1, 1); // Reversal size
//                    axStockChartX1.set_PriceStyleParam(2, (int)STOCKCHARTXLib.DataType.dtPoints); // Points or percent (eg 1 or 0.01)
//                }

//                axStockChartX1.PriceStyle = (STOCKCHARTXLib.PriceStyle)((Infragistics.Win.UltraWinToolbars.ComboBoxTool)ultraToolbarsManager1.Tools["cboPriceStyle"]).SelectedIndex;

//            }
//            catch (Exception ex)
//            {
//                // Invoke our policy that is responsible for making sure no secure information
//                // gets out of our layer.
//                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

//                if (rethrow)
//                {
//                    throw;
//                }
//            }
//        }

//        #endregion

//        private void ultraToolbarsManager1_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
//        {
//            string SymbolList= ((Infragistics.Win.UltraWinToolbars.TextBoxTool)ultraToolbarsManager1.Tools["txtSymbol"]).Text ;

//            switch(e.Tool.Key)
//            {
//                case "btnZoomIn":    
//                    try
//                    {
//                        axStockChartX1.ZoomOut(5);

//                    }
//                    catch (Exception ex)
//                    {
//                        // Invoke our policy that is responsible for making sure no secure information
//                        // gets out of our layer.
//                        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

//                        if (rethrow)
//                        {
//                            throw;
//                        }
//                    }
//                    break;

//                case "btnZoomOut":    

//                    try
//                    {
//                        axStockChartX1.ZoomIn(5);
//                    }
//                    catch (Exception ex)
//                    {
//                        // Invoke our policy that is responsible for making sure no secure information
//                        // gets out of our layer.
//                        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

//                        if (rethrow)
//                        {
//                            throw;
//                        }
//                    }
//                    break;	

//                case "btnMoveLeft":   
//                    try
//                    {
//                        axStockChartX1.ScrollLeft(5);

//                    }
//                    catch (Exception ex)
//                    {
//                        // Invoke our policy that is responsible for making sure no secure information
//                        // gets out of our layer.
//                        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

//                        if (rethrow)
//                        {
//                            throw;
//                        }
//                    }
//                    break;

//                case "btnMoveRight":    
//                    try
//                    {
//                        axStockChartX1.ScrollRight(5);

//                    }
//                    catch (Exception ex)
//                    {
//                        // Invoke our policy that is responsible for making sure no secure information
//                        // gets out of our layer.
//                        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

//                        if (rethrow)
//                        {
//                            throw;
//                        }
//                    }
//                    break;

//                case "btnAddIndicator" :

//                    int panel = axStockChartX1.AddChartPanel();
//                    string indicator = ((Infragistics.Win.UltraWinToolbars.ComboBoxTool)ultraToolbarsManager1.Tools["cboIndicators"]).Text;

//                    int count = axStockChartX1.GetIndicatorCountByType((STOCKCHARTXLib.Indicator)((Infragistics.Win.UltraWinToolbars.ComboBoxTool)ultraToolbarsManager1.Tools["cboInterval"]).SelectedIndex) + 1;
//                    if(count > 1)
//                    {
//                        indicator += " " + count.ToString();
//                    }

//                    try
//                    {
//                        axStockChartX1.AddIndicatorSeries((STOCKCHARTXLib.Indicator)((Infragistics.Win.UltraWinToolbars.ComboBoxTool)ultraToolbarsManager1.Tools["cboInterval"]).SelectedIndex, indicator, panel, true);
//                        // StockChartX1.Update(); // For VS.NET 2005
//                        axStockChartX1.CtlUpdate(); // For VS.NET 2003 and under

//                    }
//                    catch (Exception ex)
//                    {
//                        // Invoke our policy that is responsible for making sure no secure information
//                        // gets out of our layer.
//                        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

//                        if (rethrow)
//                        {
//                            throw;
//                        }
//                    }
//                    break;

//                case "btnPreference" : //Preference form
//                    try
//                    {
//                        if(LaunchPreferences != null)
//                            LaunchPreferences(sender, e);

////						LiveFeedPreferenceManager.GetInstance().SubModuleName = SUB_MODULE_NAME ;
////						LiveFeedPreference frmLiveFeedPreference= (LiveFeedPreference) LiveFeedPreference.GetInstance();
////						if(frmLiveFeedPreference.Controls.Count > 0)
////							((LiveFeedPreferenceControl)frmLiveFeedPreference.Controls[0]).SelectedTabKey = SUB_MODULE_NAME;
////						frmLiveFeedPreference.Show();
//                    }
//                    catch (Exception ex)
//                    {
//                        // Invoke our policy that is responsible for making sure no secure information
//                        // gets out of our layer.
//                        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

//                        if (rethrow)
//                        {
//                            throw;
//                        }
//                    }
//                    break;

//            }
//        }

//        private void ultraToolbarsManager1_ToolValueChanged(object sender, Infragistics.Win.UltraWinToolbars.ToolEventArgs e)
//        {
//            switch(e.Tool.Key)
//            {
//                case "cboInterval":

//                    try
//                    {
//                        int panel = 0;
//                        _symbol = ((Infragistics.Win.UltraWinToolbars.TextBoxTool)ultraToolbarsManager1.Tools["txtSymbol"]).Text.Trim();
//                        axStockChartX1.RemoveAllSeries();

//                        short  _selectedIndex = Convert.ToInt16(((Infragistics.Win.UltraWinToolbars.ComboBoxTool)ultraToolbarsManager1.Tools["cboInterval"]).SelectedIndex);
//                        ValueListItem _selectedItem = (ValueListItem)((Infragistics.Win.UltraWinToolbars.ComboBoxTool)ultraToolbarsManager1.Tools["cboInterval"]).SelectedItem;

//                        System.DateTime startTimeValue = new DateTime(ultraCboStratDate.DateTime.Year ,ultraCboStratDate.DateTime.Month ,ultraCboStratDate.DateTime.Day ,ultraCboStartTime.DateTime.Hour,ultraCboStartTime.DateTime.Minute,ultraCboStartTime.DateTime.Second);
//                        System.DateTime endTimeValue = new DateTime(ultraCboEndDate.DateTime.Year, ultraCboEndDate.DateTime.Month, ultraCboEndDate.DateTime.Day, ultraCboEndTime.DateTime.Hour, ultraCboEndTime.DateTime.Minute, ultraCboEndTime.DateTime.Second);
//                        if ( _selectedItem.ToString().Trim().Equals("Tick"))
//                        {
//                            #region Add the series for TradePrice,BidPrice,AskPrice and set color

//                            panel = axStockChartX1.AddChartPanel();


//                            axStockChartX1.AddSeries(_symbol + ".BidPrice", STOCKCHARTXLib.SeriesType.stLineChart, panel);
//                            axStockChartX1.AddSeries(_symbol + ".AskPrice", STOCKCHARTXLib.SeriesType.stLineChart, panel);
////							axStockChartX1.AddSeries(_symbol + ".AskSize", STOCKCHARTXLib.SeriesType.stLineChart, panel);
////							axStockChartX1.AddSeries(_symbol + ".BidSize", STOCKCHARTXLib.SeriesType.stLineChart, panel);
//                            axStockChartX1.AddSeries(_symbol + ".TradePrice", STOCKCHARTXLib.SeriesType.stLineChart, panel);
////							axStockChartX1.AddSeries(_symbol + ".TradeVolume", STOCKCHARTXLib.SeriesType.stLineChart, panel);


//                            // Change the color:
//                            //				
//                            axStockChartX1.set_SeriesColor(_symbol +".BidPrice", System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red));
//                            axStockChartX1.set_SeriesColor(_symbol +".AskPrice", System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Green));
////							axStockChartX1.set_SeriesColor(_symbol +".BidSize", System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue));
////							axStockChartX1.set_SeriesColor(_symbol +".AskSize", System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow));


//                            #endregion Add the series for TradePrice,BidPrice,AskPrice and set color

//                            //for Tickdata request
//                            SyncServerManager.GetInstance().RequestTicData(_symbol,startTimeValue, endTimeValue,_selectedIndex ,"",1);
//                        }

//                        else if (_selectedItem.ToString().Trim().Equals("RealTick"))
//                        {
//                            //for RealTimeTicData request
//                            #region Add the series for TradePrice,BidPrice,AskPrice and set color

//                            panel = axStockChartX1.AddChartPanel();
//                            // Now add the open, high, low and close series to that panel:

//                            axStockChartX1.AddSeries(_symbol + ".BidPrice", STOCKCHARTXLib.SeriesType.stLineChart, panel);
//                            axStockChartX1.AddSeries(_symbol + ".AskPrice", STOCKCHARTXLib.SeriesType.stLineChart, panel);
//                            axStockChartX1.AddSeries(_symbol + ".TradePrice", STOCKCHARTXLib.SeriesType.stLineChart, panel);



//                            // Change the color:

//                            axStockChartX1.set_SeriesColor(_symbol +".BidPrice", System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red));
//                            axStockChartX1.set_SeriesColor(_symbol +".AskPrice", System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Green));
//                            axStockChartX1.set_SeriesColor(_symbol +".TradePrice", System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow));

//                            #endregion Add the series for TradePrice,BidPrice,AskPrice and set color
//                            SyncServerManager.GetInstance().RequestRealTicData(_symbol);

//                        }

//                        else if(_selectedItem.ToString().Trim().Equals("5") || _selectedItem.ToString().Trim().Equals("10") || _selectedItem.ToString().Trim().Equals("15") || _selectedItem.ToString().Trim().Equals("30") || _selectedItem.ToString().Trim().Equals("60"))
//                        {
//                            #region Add the series for OHLC data and set color

//                            // First add a panel (chart area) for the OHLC data:
//                            panel = axStockChartX1.AddChartPanel();

//                            // Now add the open, high, low and close series to that panel:
//                            axStockChartX1.AddSeries(_symbol + ".open", STOCKCHARTXLib.SeriesType.stCandleChart,panel);
//                            axStockChartX1.AddSeries(_symbol + ".high", STOCKCHARTXLib.SeriesType.stCandleChart, panel);
//                            axStockChartX1.AddSeries(_symbol + ".low", STOCKCHARTXLib.SeriesType.stCandleChart, panel);
//                            axStockChartX1.AddSeries(_symbol + ".close", STOCKCHARTXLib.SeriesType.stCandleChart, panel);

//                            // Change the color:
//                            axStockChartX1.set_SeriesColor(_symbol + ".close", System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Lime));
//                            axStockChartX1.set_SeriesColor(_symbol + ".open", System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DarkOrange));
//                            axStockChartX1.set_SeriesColor(_symbol + ".high", System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DarkMagenta));
//                            axStockChartX1.set_SeriesColor(_symbol + ".low", System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Purple));

//                            // Add the volume chart panel
//                            panel = axStockChartX1.AddChartPanel();
//                            axStockChartX1.AddSeries(_symbol + ".volume", STOCKCHARTXLib.SeriesType.stVolumeChart, panel);

//                            // Change volume color and weight of the volume panel:
//                            axStockChartX1.set_SeriesColor(_symbol + ".volume", System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue));
//                            axStockChartX1.set_SeriesWeight(_symbol + ".volume", 3);

//                            // Resize the volume panel to make it smaller
//                            axStockChartX1.set_PanelY1(1, (int)(axStockChartX1.Height * 0.5));

//                            #endregion #region Add the series for OHLC data and set color

//                            //for TickBardata request

//                            SyncServerManager.GetInstance().RequestTicBarData(_symbol,startTimeValue, endTimeValue,_selectedIndex ,"",1);
//                        }

//                    }

//                    catch (Exception ex)
//                    {
//                        // Invoke our policy that is responsible for making sure no secure information
//                        // gets out of our layer.
//                        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

//                        if (rethrow)
//                        {
//                            throw;
//                        }
//                    }
//                    break;			
//            }



//        }

//        private void ultraToolbarsManager1_ToolKeyPress(object sender, Infragistics.Win.UltraWinToolbars.ToolKeyPressEventArgs e)
//        {
//            try
//            {  
////				_symbol = ((Infragistics.Win.UltraWinToolbars.TextBoxTool)ultraToolbarsManager1.Tools["txtSymbol"]).Text.Trim();
////
////				if(e.KeyChar == 13)
////				{
////					if(_symbol != "")
////					{
////	
////                        int panel=0;
////						short  _selectedIndex = Convert.ToInt16(((Infragistics.Win.UltraWinToolbars.ComboBoxTool)ultraToolbarsManager1.Tools["cboInterval"]).SelectedIndex);
////						ValueListItem _selectedItem = (ValueListItem)((Infragistics.Win.UltraWinToolbars.ComboBoxTool)ultraToolbarsManager1.Tools["cboInterval"]).SelectedItem;
////					 
////						System.DateTime startTimeValue = new DateTime(ultraCboStratDate.DateTime.Year ,ultraCboStratDate.DateTime.Month ,ultraCboStratDate.DateTime.Day ,ultraCboStartTime.DateTime.Hour,ultraCboStartTime.DateTime.Minute,ultraCboStartTime.DateTime.Second);
////						System.DateTime endTimeValue = new DateTime(ultraCboEndDate.DateTime.Year, ultraCboEndDate.DateTime.Month, ultraCboEndDate.DateTime.Day, ultraCboEndTime.DateTime.Hour, ultraCboEndTime.DateTime.Minute, ultraCboEndTime.DateTime.Second);
////						if ( _selectedItem.ToString().Trim().Equals("Tick"))
////						{
////							#region Add the series for TradePrice,BidPrice,AskPrice and set color
////						  
////							panel = axStockChartX1.AddChartPanel();
////							
////
////							axStockChartX1.AddSeries(_symbol + ".BidPrice", STOCKCHARTXLib.SeriesType.stLineChart, panel);
////							axStockChartX1.AddSeries(_symbol + ".AskPrice", STOCKCHARTXLib.SeriesType.stLineChart, panel);
////							axStockChartX1.AddSeries(_symbol + ".AskSize", STOCKCHARTXLib.SeriesType.stLineChart, panel);
////							axStockChartX1.AddSeries(_symbol + ".BidSize", STOCKCHARTXLib.SeriesType.stLineChart, panel);
////							//							axStockChartX1.AddSeries(_symbol + ".TradePrice", STOCKCHARTXLib.SeriesType.stLineChart, panel);
////							//							axStockChartX1.AddSeries(_symbol + ".TradeVolume", STOCKCHARTXLib.SeriesType.stLineChart, panel);
////
////				
////							// Change the color:
////							//				
////							axStockChartX1.set_SeriesColor(_symbol +".BidPrice", System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red));
////							axStockChartX1.set_SeriesColor(_symbol +".AskPrice", System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Green));
////							axStockChartX1.set_SeriesColor(_symbol +".BidSize", System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue));
////							axStockChartX1.set_SeriesColor(_symbol +".AskSize", System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow));
////
////					  
////							#endregion Add the series for TradePrice,BidPrice,AskPrice and set color
////
////							//for Tickdata request
////							SyncServerManager.GetInstance().RequestTicData(_symbol,startTimeValue, endTimeValue,_selectedIndex ,"",1);
////						}
////
////						else if (_selectedItem.ToString().Trim().Equals("RealTick"))
////						{
////							//for RealTimeTicData request
////							#region Add the series for TradePrice,BidPrice,AskPrice and set color
////						  
////							panel = axStockChartX1.AddChartPanel();
////							// Now add the open, high, low and close series to that panel:
////
////							axStockChartX1.AddSeries(_symbol + ".BidPrice", STOCKCHARTXLib.SeriesType.stLineChart, panel);
////							axStockChartX1.AddSeries(_symbol + ".AskPrice", STOCKCHARTXLib.SeriesType.stLineChart, panel);
////							axStockChartX1.AddSeries(_symbol + ".TradePrice", STOCKCHARTXLib.SeriesType.stLineChart, panel);
////
////                           
////				
////							// Change the color:
////							//				
////							axStockChartX1.set_SeriesColor(_symbol +".BidPrice", System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red));
////							axStockChartX1.set_SeriesColor(_symbol +".AskPrice", System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Green));
////							axStockChartX1.set_SeriesColor(_symbol +".TradePrice", System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow));
////							
////							#endregion Add the series for TradePrice,BidPrice,AskPrice and set color
////							SyncServerManager.GetInstance().RequestRealTicData(_symbol);
////					
////						}
////  
////						else if(_selectedItem.ToString().Trim().Equals("5") || _selectedItem.ToString().Trim().Equals("10") || _selectedItem.ToString().Trim().Equals("15") || _selectedItem.ToString().Trim().Equals("30") || _selectedItem.ToString().Trim().Equals("60"))
////						{
////							#region Add the series for OHLC data and set color
////					        
////							// First add a panel (chart area) for the OHLC data:
////							panel = axStockChartX1.AddChartPanel();
////
////							// Now add the open, high, low and close series to that panel:
////							axStockChartX1.AddSeries(_symbol + ".open", STOCKCHARTXLib.SeriesType.stCandleChart,panel);
////							axStockChartX1.AddSeries(_symbol + ".high", STOCKCHARTXLib.SeriesType.stCandleChart, panel);
////							axStockChartX1.AddSeries(_symbol + ".low", STOCKCHARTXLib.SeriesType.stCandleChart, panel);
////							axStockChartX1.AddSeries(_symbol + ".close", STOCKCHARTXLib.SeriesType.stCandleChart, panel);
////
////							// Change the color:
////							axStockChartX1.set_SeriesColor(_symbol + ".close", System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Lime));
////							axStockChartX1.set_SeriesColor(_symbol + ".open", System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DarkOrange));
////							axStockChartX1.set_SeriesColor(_symbol + "high", System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DarkMagenta));
////							axStockChartX1.set_SeriesColor(_symbol + ".low", System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Purple));
////	
////							// Add the volume chart panel
////							panel = axStockChartX1.AddChartPanel();
////							axStockChartX1.AddSeries(_symbol + ".volume", STOCKCHARTXLib.SeriesType.stVolumeChart, panel);
////
////							// Change volume color and weight of the volume panel:
////							axStockChartX1.set_SeriesColor(_symbol + ".volume", System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue));
////							axStockChartX1.set_SeriesWeight(_symbol + ".volume", 3);
////
////							// Resize the volume panel to make it smaller
////							axStockChartX1.set_PanelY1(1, (int)(axStockChartX1.Height * 0.5));
////					
////							#endregion #region Add the series for OHLC data and set color
////		
////							//for TickBardata request
////				
////							SyncServerManager.GetInstance().RequestTicBarData(_symbol,startTimeValue, endTimeValue,_selectedIndex ,"",1);
////					
////	
////						}
////
////
////
////
////						else
////							MessageBox.Show("Please enter the Symbol in the given textbox.");
////					}
////				}
//            }
//            catch (Exception ex)
//            {
//                // Invoke our policy that is responsible for making sure no secure information
//                // gets out of our layer.
//                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

//                if (rethrow)
//                {
//                    throw;
//                }
//            }

//        }

//        #endregion Toolbar Events

//        #region ICharts Members

//        public Form Reference()
//        {
//            return this;
//        }

//        public event System.EventHandler LaunchPreferences;


//        /// <summary>
//        /// TODO : Complete it.
//        /// </summary>
//        /// <param name="moduleName"></param>
//        public void ApplyPreferences(string moduleName,IPreferenceData prefs)
//        {
//            try
//            {
////				We have to apply this check once PreferenceMain shows the entry for Charts in tree
//                //Return if preferences not updated for this module
////				if(!moduleName.Equals(Global.Common.LIVEFEED_CHART_MODULE)) return;

//                ChartsPreferences _chartsPreferences = ((LiveFeedPreferences)prefs)._chartsPreferences ;
//                ///Now apply this preferences to charts
//            }
//             catch (Exception ex)
//            {
//                // Invoke our policy that is responsible for making sure no secure information
//                // gets out of our layer.
//                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

//                if (rethrow)
//                {
//                    throw;
//                }
//            }

//        }

//        #endregion

//    }

//}