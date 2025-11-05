using Infragistics.Win.UltraWinEditors;
using Prana.Admin.RoutingLogic.MisclFunctions;
using System;
using System.Collections;
using System.Data;
using System.Windows.Forms;



namespace Prana.Admin.RoutingLogic.Controls
{

    public delegate void DelegateGroupToClient();
    /// <summary>
    /// Summary description for Client.
    /// </summary>
    public class Client : System.Windows.Forms.UserControl
    {
        private System.Windows.Forms.CheckedListBox chklstClient;
        private Infragistics.Win.UltraWinEditors.UltraOptionSet optionsetApplyRL;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkedtCheckAll;
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        private Infragistics.Win.Misc.UltraLabel labelName;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtedtName;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbedtRLName0;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbedtRank0;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbedtRLName1;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbedtRLName2;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbedtRank1;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbedtRank2;
        private Controls.AUEC ucAUEC;
        private Infragistics.Win.Misc.UltraLabel labelRLName0;
        private Infragistics.Win.Misc.UltraLabel labelRLName1;
        private Infragistics.Win.Misc.UltraLabel labelRLName2;
        private Infragistics.Win.Misc.UltraLabel labelRLRank0;
        private Infragistics.Win.Misc.UltraLabel labelRLRank1;
        private Infragistics.Win.Misc.UltraLabel labelRLRank2;
        private Controls.IfThenCondition ucIfThenCondition0;
        private Controls.IfThenCondition ucIfThenCondition1;
        private Controls.IfThenCondition ucIfThenCondition2;

        private string strMemoryID = "RL0";
        private string strTabID = "group";
        private System.Collections.ArrayList alIfThenCondition = new ArrayList();
        private System.Collections.ArrayList alRLName = new ArrayList();
        private System.Collections.ArrayList alRank = new ArrayList();
        private System.Data.DataSet dsData;
        private Infragistics.Win.Misc.UltraGroupBox grpboxRL0;
        private Infragistics.Win.Misc.UltraGroupBox grpboxRL1;
        private Infragistics.Win.Misc.UltraGroupBox grpboxRL2; private BLL.DataRoutingLogicObjects dataRL;
        private int iSelectedClientID = Functions.MinValue;
        private int iGroupIDOld = Functions.MinValue;
        private string strGroupNameOld = "New";
        private System.Windows.Forms.NodeTree nodeMain;



        public Client()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            // TODO: Add any initialization after the InitializeComponent call
            this.alIfThenCondition.Add(this.ucIfThenCondition0);
            this.alIfThenCondition.Add(this.ucIfThenCondition1);
            this.alIfThenCondition.Add(this.ucIfThenCondition2);
            this.alRLName.Add(this.cmbedtRLName0);
            this.alRLName.Add(this.cmbedtRLName1);
            this.alRLName.Add(this.cmbedtRLName2);
            this.alRank.Add(this.cmbedtRank0);
            this.alRank.Add(this.cmbedtRank1);
            this.alRank.Add(this.cmbedtRank2);

            this.cmbedtRank0.Value = 1;
            this.cmbedtRank1.Value = 2;
            this.cmbedtRank2.Value = 3;


            this.FocusEventColor();




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
                if (chklstClient != null)
                {
                    chklstClient.Dispose();
                }
                if (optionsetApplyRL != null)
                {
                    optionsetApplyRL.Dispose();
                }
                if (chkedtCheckAll != null)
                {
                    chkedtCheckAll.Dispose();
                }
                if (labelName != null)
                {
                    labelName.Dispose();
                }
                if (txtedtName != null)
                {
                    txtedtName.Dispose();
                }
                if (cmbedtRLName0 != null)
                {
                    cmbedtRLName0.Dispose();
                }
                if (cmbedtRank0 != null)
                {
                    cmbedtRank0.Dispose();
                }
                if (cmbedtRank1 != null)
                {
                    cmbedtRank1.Dispose();
                }
                if (cmbedtRank2 != null)
                {
                    cmbedtRank2.Dispose();
                }
                if (cmbedtRLName1 != null)
                {
                    cmbedtRLName1.Dispose();
                }
                if (cmbedtRLName2 != null)
                {
                    cmbedtRLName2.Dispose();
                }
                if (ucAUEC != null)
                {
                    ucAUEC.Dispose();
                }
                if (labelRLName0 != null)
                {
                    labelRLName0.Dispose();
                }
                if (labelRLName1 != null)
                {
                    labelRLName1.Dispose();
                }
                if (labelRLName2 != null)
                {
                    labelRLName2.Dispose();
                }
                if (labelRLRank0 != null)
                {
                    labelRLRank0.Dispose();
                }
                if (labelRLRank1 != null)
                {
                    labelRLRank1.Dispose();
                }
                if (labelRLRank2 != null)
                {
                    labelRLRank2.Dispose();
                }
                if (ucIfThenCondition0 != null)
                {
                    ucIfThenCondition0.Dispose();
                }
                if (ucIfThenCondition1 != null)
                {
                    ucIfThenCondition1.Dispose();
                }
                if (ucIfThenCondition2 != null)
                {
                    ucIfThenCondition2.Dispose();
                }
                if (grpboxRL0 != null)
                {
                    grpboxRL0.Dispose();
                }
                if (grpboxRL1 != null)
                {
                    grpboxRL1.Dispose();
                }
                if (grpboxRL2 != null)
                {
                    grpboxRL2.Dispose();
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem1 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem2 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem3 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem4 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance("Off");
            Infragistics.Win.ValueListItem valueListItem5 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance("On");
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            this.ucIfThenCondition0 = new Prana.Admin.RoutingLogic.Controls.IfThenCondition();
            this.ucIfThenCondition1 = new Prana.Admin.RoutingLogic.Controls.IfThenCondition();
            this.ucIfThenCondition2 = new Prana.Admin.RoutingLogic.Controls.IfThenCondition();
            this.cmbedtRLName0 = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.cmbedtRank0 = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.cmbedtRLName1 = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.cmbedtRLName2 = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.cmbedtRank1 = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.cmbedtRank2 = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.ucAUEC = new Prana.Admin.RoutingLogic.Controls.AUEC();
            this.labelName = new Infragistics.Win.Misc.UltraLabel();
            this.txtedtName = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.labelRLName0 = new Infragistics.Win.Misc.UltraLabel();
            this.labelRLName1 = new Infragistics.Win.Misc.UltraLabel();
            this.labelRLName2 = new Infragistics.Win.Misc.UltraLabel();
            this.labelRLRank0 = new Infragistics.Win.Misc.UltraLabel();
            this.labelRLRank1 = new Infragistics.Win.Misc.UltraLabel();
            this.labelRLRank2 = new Infragistics.Win.Misc.UltraLabel();
            this.chklstClient = new System.Windows.Forms.CheckedListBox();
            this.optionsetApplyRL = new Infragistics.Win.UltraWinEditors.UltraOptionSet();
            this.chkedtCheckAll = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.grpboxRL0 = new Infragistics.Win.Misc.UltraGroupBox();
            this.grpboxRL1 = new Infragistics.Win.Misc.UltraGroupBox();
            this.grpboxRL2 = new Infragistics.Win.Misc.UltraGroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.cmbedtRLName0)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbedtRank0)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbedtRLName1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbedtRLName2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbedtRank1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbedtRank2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtedtName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.optionsetApplyRL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpboxRL0)).BeginInit();
            this.grpboxRL0.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpboxRL1)).BeginInit();
            this.grpboxRL1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpboxRL2)).BeginInit();
            this.grpboxRL2.SuspendLayout();
            this.SuspendLayout();
            // 
            // ucIfThenCondition0
            // 
            this.ucIfThenCondition0.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((System.Byte)(0)));
            this.ucIfThenCondition0.Location = new System.Drawing.Point(62, 6);
            this.ucIfThenCondition0.Name = "ucIfThenCondition0";
            this.ucIfThenCondition0.Size = new System.Drawing.Size(636, 114);
            this.ucIfThenCondition0.TabIndex = 0;
            this.ucIfThenCondition0.Tag = "ucIfThenCondition0";
            // 
            // ucIfThenCondition1
            // 
            this.ucIfThenCondition1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((System.Byte)(0)));
            this.ucIfThenCondition1.Location = new System.Drawing.Point(62, 6);
            this.ucIfThenCondition1.Name = "ucIfThenCondition1";
            this.ucIfThenCondition1.Size = new System.Drawing.Size(636, 114);
            this.ucIfThenCondition1.TabIndex = 1;
            this.ucIfThenCondition1.Tag = "ucIfThenCondition1";
            // 
            // ucIfThenCondition2
            // 
            this.ucIfThenCondition2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((System.Byte)(0)));
            this.ucIfThenCondition2.Location = new System.Drawing.Point(62, 6);
            this.ucIfThenCondition2.Name = "ucIfThenCondition2";
            this.ucIfThenCondition2.Size = new System.Drawing.Size(636, 114);
            this.ucIfThenCondition2.TabIndex = 2;
            this.ucIfThenCondition2.Tag = "ucIfThenCondition2";
            // 
            // cmbedtRLName0
            // 
            this.cmbedtRLName0.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cmbedtRLName0.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbedtRLName0.Location = new System.Drawing.Point(10, 8);
            this.cmbedtRLName0.Name = "cmbedtRLName0";
            this.cmbedtRLName0.Size = new System.Drawing.Size(48, 20);
            this.cmbedtRLName0.TabIndex = 4;
            this.cmbedtRLName0.Tag = "cmbedtRLName0";
            this.cmbedtRLName0.SelectionChanged += new System.EventHandler(this.LoadSelectedRL);
            // 
            // cmbedtRank0
            // 
            appearance1.ForeColorDisabled = System.Drawing.Color.Black;
            this.cmbedtRank0.Appearance = appearance1;
            this.cmbedtRank0.DropDownButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Never;
            this.cmbedtRank0.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cmbedtRank0.Enabled = false;
            this.cmbedtRank0.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            appearance2.TextHAlign = Infragistics.Win.HAlign.Center;
            appearance2.TextVAlign = Infragistics.Win.VAlign.Middle;
            this.cmbedtRank0.ItemAppearance = appearance2;
            valueListItem1.DataValue = 1;
            valueListItem1.DisplayText = "1";
            valueListItem1.Tag = "1";
            this.cmbedtRank0.Items.Add(valueListItem1);
            this.cmbedtRank0.Location = new System.Drawing.Point(18, 62);
            this.cmbedtRank0.Name = "cmbedtRank0";
            this.cmbedtRank0.Size = new System.Drawing.Size(18, 20);
            this.cmbedtRank0.TabIndex = 5;
            this.cmbedtRank0.Tag = "cmbedtRank0";
            // 
            // cmbedtRLName1
            // 
            this.cmbedtRLName1.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cmbedtRLName1.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbedtRLName1.Location = new System.Drawing.Point(10, 8);
            this.cmbedtRLName1.Name = "cmbedtRLName1";
            this.cmbedtRLName1.Size = new System.Drawing.Size(46, 20);
            this.cmbedtRLName1.TabIndex = 6;
            this.cmbedtRLName1.Tag = "cmbedtRLName1";
            this.cmbedtRLName1.SelectionChanged += new System.EventHandler(this.LoadSelectedRL);
            // 
            // cmbedtRLName2
            // 
            this.cmbedtRLName2.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cmbedtRLName2.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbedtRLName2.Location = new System.Drawing.Point(8, 8);
            this.cmbedtRLName2.Name = "cmbedtRLName2";
            this.cmbedtRLName2.Size = new System.Drawing.Size(46, 20);
            this.cmbedtRLName2.TabIndex = 7;
            this.cmbedtRLName2.Tag = "cmbedtRLName2";
            this.cmbedtRLName2.SelectionChanged += new System.EventHandler(this.LoadSelectedRL);
            // 
            // cmbedtRank1
            // 
            appearance3.ForeColorDisabled = System.Drawing.Color.Black;
            this.cmbedtRank1.Appearance = appearance3;
            this.cmbedtRank1.DropDownButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Never;
            this.cmbedtRank1.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cmbedtRank1.Enabled = false;
            this.cmbedtRank1.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            appearance4.TextHAlign = Infragistics.Win.HAlign.Center;
            appearance4.TextVAlign = Infragistics.Win.VAlign.Middle;
            this.cmbedtRank1.ItemAppearance = appearance4;
            valueListItem2.DataValue = 2;
            valueListItem2.DisplayText = "2";
            valueListItem2.Tag = "2";
            this.cmbedtRank1.Items.Add(valueListItem2);
            this.cmbedtRank1.Location = new System.Drawing.Point(18, 64);
            this.cmbedtRank1.Name = "cmbedtRank1";
            this.cmbedtRank1.Size = new System.Drawing.Size(18, 20);
            this.cmbedtRank1.TabIndex = 8;
            this.cmbedtRank1.Tag = "cmbedtRank1";
            // 
            // cmbedtRank2
            // 
            appearance5.ForeColorDisabled = System.Drawing.Color.Black;
            this.cmbedtRank2.Appearance = appearance5;
            this.cmbedtRank2.DropDownButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Never;
            this.cmbedtRank2.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cmbedtRank2.Enabled = false;
            this.cmbedtRank2.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            appearance6.TextHAlign = Infragistics.Win.HAlign.Center;
            appearance6.TextVAlign = Infragistics.Win.VAlign.Middle;
            this.cmbedtRank2.ItemAppearance = appearance6;
            valueListItem3.DataValue = 3;
            valueListItem3.DisplayText = "3";
            valueListItem3.Tag = "3";
            this.cmbedtRank2.Items.Add(valueListItem3);
            this.cmbedtRank2.Location = new System.Drawing.Point(20, 62);
            this.cmbedtRank2.Name = "cmbedtRank2";
            this.cmbedtRank2.Size = new System.Drawing.Size(18, 20);
            this.cmbedtRank2.TabIndex = 9;
            this.cmbedtRank2.Tag = "cmbedtRank2";
            // 
            // ucAUEC
            // 
            this.ucAUEC.Location = new System.Drawing.Point(30, 1);
            this.ucAUEC.Name = "ucAUEC";
            this.ucAUEC.Size = new System.Drawing.Size(484, 22);
            this.ucAUEC.TabIndex = 10;
            this.ucAUEC.Tag = "ucAUEC";
            // 
            // labelName
            // 
            this.labelName.Location = new System.Drawing.Point(0, 35);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(68, 14);
            this.labelName.TabIndex = 11;
            this.labelName.Tag = "labelName";
            this.labelName.Text = "Group Name";
            // 
            // txtedtName
            // 
            appearance7.ForeColorDisabled = System.Drawing.Color.Black;
            appearance7.TextHAlign = Infragistics.Win.HAlign.Left;
            appearance7.TextVAlign = Infragistics.Win.VAlign.Middle;
            this.txtedtName.Appearance = appearance7;
            this.txtedtName.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.txtedtName.Location = new System.Drawing.Point(66, 33);
            this.txtedtName.Name = "txtedtName";
            this.txtedtName.Size = new System.Drawing.Size(106, 20);
            this.txtedtName.TabIndex = 12;
            this.txtedtName.Tag = "txtedtName";
            this.txtedtName.MaxLength = 50;
            this.txtedtName.Leave += new System.EventHandler(this.UpdateName);
            // 
            // labelRLName0
            // 
            this.labelRLName0.Location = new System.Drawing.Point(8, 34);
            this.labelRLName0.Name = "labelRLName0";
            this.labelRLName0.Size = new System.Drawing.Size(50, 16);
            this.labelRLName0.TabIndex = 13;
            this.labelRLName0.Text = "RL Name";
            // 
            // labelRLName1
            // 
            this.labelRLName1.Location = new System.Drawing.Point(8, 34);
            this.labelRLName1.Name = "labelRLName1";
            this.labelRLName1.Size = new System.Drawing.Size(50, 16);
            this.labelRLName1.TabIndex = 14;
            this.labelRLName1.Text = "RL Name";
            // 
            // labelRLName2
            // 
            this.labelRLName2.Location = new System.Drawing.Point(8, 32);
            this.labelRLName2.Name = "labelRLName2";
            this.labelRLName2.Size = new System.Drawing.Size(50, 16);
            this.labelRLName2.TabIndex = 15;
            this.labelRLName2.Text = "RL Name";
            // 
            // labelRLRank0
            // 
            this.labelRLRank0.Location = new System.Drawing.Point(14, 86);
            this.labelRLRank0.Name = "labelRLRank0";
            this.labelRLRank0.Size = new System.Drawing.Size(30, 16);
            this.labelRLRank0.TabIndex = 16;
            this.labelRLRank0.Text = "Rank";
            // 
            // labelRLRank1
            // 
            this.labelRLRank1.Location = new System.Drawing.Point(14, 88);
            this.labelRLRank1.Name = "labelRLRank1";
            this.labelRLRank1.Size = new System.Drawing.Size(30, 16);
            this.labelRLRank1.TabIndex = 1;
            this.labelRLRank1.Text = "Rank";
            // 
            // labelRLRank2
            // 
            this.labelRLRank2.Location = new System.Drawing.Point(14, 84);
            this.labelRLRank2.Name = "labelRLRank2";
            this.labelRLRank2.Size = new System.Drawing.Size(30, 16);
            this.labelRLRank2.TabIndex = 0;
            this.labelRLRank2.Text = "Rank";
            // 
            // chklstClient
            // 
            this.chklstClient.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.chklstClient.CheckOnClick = true;
            this.chklstClient.HorizontalScrollbar = true;
            this.chklstClient.Location = new System.Drawing.Point(584, 1);
            this.chklstClient.Name = "chklstClient";
            this.chklstClient.Size = new System.Drawing.Size(114, 50);
            this.chklstClient.Sorted = true;
            this.chklstClient.TabIndex = 17;
            this.chklstClient.Tag = "chklstClient";
            this.chklstClient.ThreeDCheckBoxes = true;
            this.chklstClient.DoubleClick += new System.EventHandler(this.UnselectClient);
            this.chklstClient.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.CheckClientChange);
            // 
            // optionsetApplyRL
            // 
            this.optionsetApplyRL.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.optionsetApplyRL.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            appearance8.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance8.BorderColor = System.Drawing.Color.Transparent;
            appearance8.BorderColor3DBase = System.Drawing.Color.Transparent;
            this.optionsetApplyRL.ItemAppearance = appearance8;
            appearance9.Tag = false;
            valueListItem4.Appearance = appearance9;
            valueListItem4.DataValue = 0;
            valueListItem4.DisplayText = "Off";
            valueListItem4.Tag = 0;
            appearance10.Tag = true;
            valueListItem5.Appearance = appearance10;
            valueListItem5.DataValue = 1;
            valueListItem5.DisplayText = "On";
            valueListItem5.Tag = 1;
            this.optionsetApplyRL.Items.Add(valueListItem4);
            this.optionsetApplyRL.Items.Add(valueListItem5);
            this.optionsetApplyRL.Location = new System.Drawing.Point(176, 35);
            this.optionsetApplyRL.Name = "optionsetApplyRL";
            this.optionsetApplyRL.Size = new System.Drawing.Size(72, 16);
            this.optionsetApplyRL.TabIndex = 19;
            this.optionsetApplyRL.Tag = "optionsetApplyRL";
            this.optionsetApplyRL.ValueChanged += new System.EventHandler(this.OnOffEvent);
            // 
            // chkedtCheckAll
            // 
            this.chkedtCheckAll.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.chkedtCheckAll.Location = new System.Drawing.Point(512, 38);
            this.chkedtCheckAll.Name = "chkedtCheckAll";
            this.chkedtCheckAll.Size = new System.Drawing.Size(70, 12);
            this.chkedtCheckAll.TabIndex = 20;
            this.chkedtCheckAll.Tag = "chkedtCheckAll";
            this.chkedtCheckAll.Text = "Check All";
            this.chkedtCheckAll.CheckedChanged += new System.EventHandler(this.CheckUncheckAll);
            // 
            // grpboxRL0
            // 
            appearance11.BorderColor = System.Drawing.Color.Black;
            appearance11.BorderColor3DBase = System.Drawing.Color.Black;
            this.grpboxRL0.Appearance = appearance11;
            this.grpboxRL0.BorderStyle = Infragistics.Win.Misc.GroupBoxBorderStyle.Rounded;
            this.grpboxRL0.Controls.Add(this.ucIfThenCondition0);
            this.grpboxRL0.Controls.Add(this.cmbedtRank0);
            this.grpboxRL0.Controls.Add(this.cmbedtRLName0);
            this.grpboxRL0.Controls.Add(this.labelRLRank0);
            this.grpboxRL0.Controls.Add(this.labelRLName0);
            this.grpboxRL0.Location = new System.Drawing.Point(1, 60);
            this.grpboxRL0.Name = "grpboxRL0";
            this.grpboxRL0.Size = new System.Drawing.Size(700, 122);
            this.grpboxRL0.SupportThemes = false;
            this.grpboxRL0.TabIndex = 21;
            // 
            // grpboxRL1
            // 
            this.grpboxRL1.Controls.Add(this.ucIfThenCondition1);
            this.grpboxRL1.Controls.Add(this.cmbedtRank1);
            this.grpboxRL1.Controls.Add(this.cmbedtRLName1);
            this.grpboxRL1.Controls.Add(this.labelRLRank1);
            this.grpboxRL1.Controls.Add(this.labelRLName1);
            this.grpboxRL1.Location = new System.Drawing.Point(1, 184);
            this.grpboxRL1.Name = "grpboxRL1";
            this.grpboxRL1.Size = new System.Drawing.Size(700, 122);
            this.grpboxRL1.SupportThemes = false;
            this.grpboxRL1.TabIndex = 22;
            // 
            // grpboxRL2
            // 
            this.grpboxRL2.Controls.Add(this.ucIfThenCondition2);
            this.grpboxRL2.Controls.Add(this.cmbedtRank2);
            this.grpboxRL2.Controls.Add(this.cmbedtRLName2);
            this.grpboxRL2.Controls.Add(this.labelRLRank2);
            this.grpboxRL2.Controls.Add(this.labelRLName2);
            this.grpboxRL2.Location = new System.Drawing.Point(1, 308);
            this.grpboxRL2.Name = "grpboxRL2";
            this.grpboxRL2.Size = new System.Drawing.Size(700, 122);
            this.grpboxRL2.SupportThemes = false;
            this.grpboxRL2.TabIndex = 23;
            // 
            // Client
            // 
            this.Controls.Add(this.grpboxRL1);
            this.Controls.Add(this.grpboxRL0);
            this.Controls.Add(this.chkedtCheckAll);
            this.Controls.Add(this.optionsetApplyRL);
            this.Controls.Add(this.chklstClient);
            this.Controls.Add(this.txtedtName);
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.ucAUEC);
            this.Controls.Add(this.grpboxRL2);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((System.Byte)(0)));
            this.Name = "Client";
            this.Size = new System.Drawing.Size(701, 430);
            this.Tag = "Client";
            ((System.ComponentModel.ISupportInitialize)(this.cmbedtRLName0)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbedtRank0)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbedtRLName1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbedtRLName2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbedtRank1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbedtRank2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtedtName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.optionsetApplyRL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpboxRL0)).EndInit();
            this.grpboxRL0.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpboxRL1)).EndInit();
            this.grpboxRL1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpboxRL2)).EndInit();
            this.grpboxRL2.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        public string TabID
        {
            get
            {
                return strTabID;
            }
        }


        #region fous funct

        private void FocusEventColor()
        {
            this.txtedtName.Leave += new System.EventHandler(Functions.object_LostFocus);
            this.txtedtName.Enter += new System.EventHandler(Functions.object_GotFocus);

            for (int i = 0; i < this.alRLName.Count; i++)
            {
                ((UltraComboEditor)this.alRLName[i]).Leave += new System.EventHandler(Functions.object_LostFocus);
                ((UltraComboEditor)this.alRLName[i]).Enter += new System.EventHandler(Functions.object_GotFocus);

                ((UltraComboEditor)this.alRank[i]).Leave += new System.EventHandler(Functions.object_LostFocus);
                ((UltraComboEditor)this.alRank[i]).Enter += new System.EventHandler(Functions.object_GotFocus);


            }


            this.chkedtCheckAll.Leave += new System.EventHandler(Functions.object_LostFocus);
            this.chkedtCheckAll.Enter += new System.EventHandler(Functions.object_GotFocus);

            //			this.optionsetApplyRL.Leave += new System.EventHandler(Functions.object_LostFocus);
            //			this.optionsetApplyRL.Enter += new System.EventHandler(Functions.object_GotFocus);

            this.chklstClient.Leave += new System.EventHandler(Functions.object_LostFocus);
            this.chklstClient.Enter += new System.EventHandler(Functions.object_GotFocus);

        }

        #endregion

        #region Load Data

        public void LoadData(ref System.Data.DataSet _dsData, ref BLL.DataRoutingLogicObjects _dataRL, ref string _strTabID, ref System.Windows.Forms.NodeTree _nodeMain)//ref System.Data.DataSet _dsData, ref BLL.DataRoutingLogicObjects _dataRL)
        {
            this.dsData = _dsData; this.dataRL = _dataRL; this.strTabID = _strTabID;
            this.nodeMain = _nodeMain;
            //			this.iSelectedClientID = Functions.MinValue;
            this.LoadSetValues();

        }


        //		public void LoadData( ref System.Data.DataSet _dsData, ref BLL.DataRoutingLogicObjects _dataRL, ref string _strTabID, int _iSelectedClientID)//string _strClientNameSelected)//ref System.Data.DataSet _dsData, ref BLL.DataRoutingLogicObjects _dataRL)
        //		{
        //			this.dsData = _dsData;      this.dataRL = _dataRL;      this.strTabID=_strTabID;
        //			this.iSelectedClientID = _iSelectedClientID ;
        //			this.LoadSetValues();		
        //		}

        public int SelectedGroupClientID
        {
            set
            {
                this.iSelectedClientID = value;
            }
        }


        private void LoadSetValues()
        {
            //			this.dataRL.ClientCount(strTabID,3);
            //			this.dataRL.RoutingPathCount(strTabID,3);
            //			for(int i =0;i<3;i++)
            //			{
            //				this.dataRL.ConditionsCount(strTabID,i,3);
            //				this.dataRL.TradingAccountCount(strTabID,i,3);
            //			}


            /// memeoryrl loading
            /// 
            //			if( ! LoadMemoryRLInto( strMemoryID) )
            //			{
            //				MessageBox.Show(" Unable to Load Data ");
            //				return;
            //			}



            //			this.txtedtName.Text = this.dsData.Tables["dtMemoryGroupClient"].Rows[0]["Name"].ToString();

            this.txtedtName.Text = this.dataRL.Name(strTabID);

            //  on/off thing

            //			this.optionsetApplyRL.CheckedIndex = IsNull(this.dsData.Tables["dtMemoryGroupClient"].Rows[0]["ApplyRL"])? 0 : int.Parse(this.dsData.Tables["dtMemoryGroupClient"].Rows[0]["ApplyRL"].ToString());

            if (this.iSelectedClientID < 0)
            {
                if (this.dataRL.ApplyRL(strTabID) > 1)
                {
                    this.optionsetApplyRL.Value = null;

                }
                else
                {
                    this.optionsetApplyRL.CheckedIndex = this.dataRL.ApplyRL(strTabID);
                }
            }
            else
            {
                object _obj = (this.dsData.Tables["dtClientList"].Select("ClientID = " + this.iSelectedClientID.ToString())[0]["ApplyRL"]);
                if (Functions.IsNull(_obj))
                {
                    this.optionsetApplyRL.Value = null;

                }
                else
                {
                    this.optionsetApplyRL.CheckedIndex = (int)_obj;
                }
                //				this.optionsetApplyRL.CheckedIndex=(int);

            }



            this.chklstClient.Items.Clear();

            this.ucAUEC.LoadData(ref dsData, ref dataRL, strMemoryID, strTabID);

            if (this.Tag.ToString().Substring(0, "c".Length).Equals("c"))
            {
                this.strTabID = "client";

                LoadClient();

            }
            else
            {
                this.strTabID = "group";
                LoadGroup();

            }
        }
        #endregion


        //		private bool LoadMemoryRLInto( string _strMemoryID)
        //		{
        ////				if(this.dsData.Tables.Contains("dtMemoryRL"))
        ////					{
        ////						((Forms.CompanyMaster)(this.ParentForm)).SaveToDBMemory();
        ////					}
        //
        ////			System.Data.DataTable dtMemoryRL ;
        ////			bool _bdtMemoryLoaded = BLL.DataCallFunctionsManager.LoadMemoryRL(strMemoryID,strTabID, out dtMemoryRL);
        ////			DataColumn[] _dcPrimaryKey ;
        ////			System.Data.SqlClient.SqlParameter[] _sqlParam ;//= new System.Data.SqlClient.SqlParameter[];
        ////			DataColumn[] _dcPrimaryKey ;//= new DataColumn[];
        ////				System.Data.DataTable dtMemoryRL ;
        ////				int _ipkRLID = (IsNull(this.dsData.Tables["dtMemoryGroupClient"].Rows[0]["RLID0"]))?Functions.MinValue:int.Parse(this.dsData.Tables["dtMemoryGroupClient"].Rows[0]["RLID0"].ToString());
        ////					_sqlParam = new System.Data.SqlClient.SqlParameter[] {new System.Data.SqlClient.SqlParameter("@RLID",_ipkRLID), new System.Data.SqlClient.SqlParameter("@MemoryID",strMemoryID)};
        ////					dtMemoryRL= BLL.DataHandelingManager.DataStoredProcedure("P_GetRL",_sqlParam);
        //
        ////
        ////			if(IsNull(dtMemoryRL))
        ////			{
        ////				return false;
        ////			}
        ////
        //////					dtMemoryRL.TableName="dtMemoryRL";
        ////			
        ////	
        ////			if(this.dsData.Tables.Contains("dtMemoryRL"))
        ////			{
        ////				this.dsData.Tables["dtMemoryRL"].Dispose();
        ////				this.dsData.Tables.Remove("dtMemoryRL");
        ////			}
        ////			this.dsData.Tables.Add(dtMemoryRL) ;
        //			
        //					
        ////			_dcPrimaryKey=  new DataColumn[] {dtMemoryRL.Columns["MemoryID"]};
        ////			this.dsData.Tables["dtMemoryRL"].PrimaryKey = _dcPrimaryKey;
        //		
        //			
        //			return _bdtMemoryLoaded;
        //
        //
        //		}

        #region  load for grp

        private void LoadGroup()
        {

            /// Name
            this.labelName.Text = "Group Name";
            this.txtedtName.Enabled = true;
            this.txtedtName.Text = this.dataRL.Name(strTabID);
            //			this.ucAUEC.Enabled=true;
            //this.txtedtName.Text = this.dsData.Tables["dtMemoryGroupClient"].Rows[0]["Name"].ToString();

            //			this.chkedtCheckAll.Show();
            //
            //			this.chklstClient.Show();





        }
        #endregion



        #region  load for client

        private void LoadClient()
        {

            /// Name
            this.labelName.Text = "Client Name";
            this.txtedtName.Enabled = false;
            this.txtedtName.Text = this.dataRL.Name(strTabID);
            //			this.ucAUEC.Enabled=false;
            //this.txtedtName.Text = this.dsData.Tables["dtMemoryGroupClient"].Rows[0]["Name"].ToString();
            //			this.chklstClient.Items.Clear();
            //			this.chkedtCheckAll.Hide();
            //
            //			this.chklstClient.Hide();
            if (Functions.IsNull(this.optionsetApplyRL.Value))
            {
                this.optionsetApplyRL.Value = 0;
            }



        }
        #endregion


        #region delegations of event  auec- client logic
        public void DelegateLoadData()
        {

            //
            LoadClientCheckList();


            /// loading  rls in combo

            int _iIndex = -1;
            foreach (Infragistics.Win.UltraWinEditors.UltraComboEditor _ceRLName in this.alRLName)
            {
                LoadRLNameCombo(_ceRLName);
                _iIndex = int.Parse(_ceRLName.Tag.ToString().Trim().Remove(0, "cmbedtRLName".Length));
                //				System.EventArgs e = new System.EventArgs();

                if (_iIndex < this.dataRL.RoutingPathCount(strTabID))
                {
                    _ceRLName.Value = this.dataRL.RoutingPathID(strTabID, _iIndex);
                    //					_ceRLName.SelectedItem.DataValue=_ceRLName.Value;

                    if (Functions.IsNull(_ceRLName.Value))//value not in teh rlname list
                    {
                        int _iAUECID = 0;
                        if (this.dataRL.RoutingPathID(strTabID, _iIndex) < 0)
                        {
                            _iAUECID = this.dataRL.AUECID(strTabID);
                        }

                        BLL.DataCallFunctionsManager.LoadRL(strTabID, this.dataRL.RoutingPathID(strTabID, _iIndex), _iIndex);
                        if (this.dataRL.RoutingPathID(strTabID, _iIndex) < 0)
                        {
                            this.dataRL.AUECID(strTabID, _iAUECID);

                        }

                        ((Forms.CompanyMaster)(this.ParentForm)).UserTriggered = false;
                        ((Forms.CompanyMaster)(this.ParentForm)).ValueChanged(strTabID, _iIndex, false);
                        ((Forms.CompanyMaster)(this.ParentForm)).UserTriggered = true;
                        ((Controls.IfThenCondition)(this.alIfThenCondition[_iIndex])).LoadData(ref dsData, ref dataRL, strMemoryID, strTabID, _iIndex);

                    }
                    //					else
                    //					{					
                    //					this.LoadRL(_ceRLName, new System.EventArgs()  );

                }
            }

        }



        private void LoadClientCheckList()
        {
            if (!(Functions.IsNull(this.dataRL) || Functions.IsNull(this.dsData)))
            {
                //				if(true)//this.strTabID.Equals("group"))
                //				{
                //  adding  client list to clientlist chk box

                //			this.chklstClient.SelectionChangeCommitted -= new System.EventHandler(this.SelectedExchange);
                //					this.chklstClient.Items.Clear();

                if (this.dsData.Tables.Contains("dtClientList"))
                {
                    if (this.chklstClient.Items.Count > 0)//&& this.chklstClient.CheckedItems.Count>0)
                    {
                        //							for(int i=0;i<this.dsData.Tables["dtClientList"].Rows.Count-1;i++)
                        //							{
                        //								this.dsData.Tables["dtClientList"].Rows[i]["Checked"] = 0;
                        //							}

                        string _strClientNameChecked = "";
                        string _strClientName = "";
                        bool _bIsChecked = false;

                        foreach (System.Data.DataRow _row in this.dsData.Tables["dtClientList"].Rows)
                        {
                            _strClientName = _row["ClientName"].ToString();


                            _bIsChecked = false;

                            for (int i = 0; i < this.chklstClient.CheckedIndices.Count; i++)
                            {

                                _strClientNameChecked = ((string)(this.chklstClient.Items[this.chklstClient.CheckedIndices[i]]));

                                if (_strClientName.Equals(_strClientNameChecked))
                                {
                                    _bIsChecked = true;
                                    break;
                                }
                                //									foreach(

                            }

                            if (_bIsChecked)
                            {
                                _row["Checked"] = 1;
                            }
                            else
                            {
                                _row["Checked"] = 0;
                                _row["ApplyRL"] = 0;
                            }




                            //								System.Data.DataRow[] _rowArray = (this.dsData.Tables["dtClientList"].Select(" ClientName = '" + _strClientNameChecked + "'")) ;
                            //								for(int j =0;j< _rowArray.Length;j++)
                            //								{
                            //									_rowArray[j]["Checked"]=1;
                            //									//		(this.dsData.Tables["dtClientList"].Select(" ClientName = " + _strClientName))[j]["ApplyRL"]=1;
                            //									//		(this.dsData.Tables["dtClientList"]..Select(" ClientName = " + _strClientName));
                            //								}
                        }

                        //						foreach(             in this.chklstClient.CheckedItems )
                    }

                    this.chklstClient.Items.Clear();
                    this.chkedtCheckAll.CheckedValue = false;

                    foreach (System.Data.DataRow _row in this.dsData.Tables["dtClientList"].Select("AUECID = " + this.dataRL.AUECID(strTabID).ToString()))
                    {
                        if (Functions.IsNull(_row["Checked"]))
                        {
                            _row["Checked"] = 0;
                        }

                        this.chklstClient.Items.Add(_row["ClientName"].ToString(), ((int.Parse(_row["Checked"].ToString())) == 1 ? true : false));
                    }

                    //					this.chklstClient.PerformLayout();

                }


                if (this.chklstClient.CheckedItems.Count == 0 && !this.strTabID.Equals("group"))
                {
                    this.strTabID = "group";
                    this.dataRL.Name(strTabID, this.strGroupNameOld);
                    this.dataRL.ID(strTabID, this.iGroupIDOld);

                    ((Prana.Admin.RoutingLogic.Forms.CompanyMaster)this.ParentForm).SelectGroupInTree = iGroupIDOld;
                    this.LoadGroup();
                }

                //					if(this.chklstClient.Items.Count>0)
                //					{
                //						bool _bCheckAllState = this.chkedtCheckAll.Checked;
                //						this.chklstClient.SelectedIndex = 0;
                //						int _iSelectedIndex = this.chklstClient.Items.IndexOf(this.chklstClient.SelectedItems[0]) ;
                //			
                //						this.chklstClient.SetItemChecked(_iSelectedIndex,   ! this.chklstClient.GetItemChecked(_iSelectedIndex) );
                //						this.chklstClient.SelectedIndex = 0;
                //						this.CheckClientChange(this.chklstClient,null);
                //						this.chklstClient.SetItemChecked(_iSelectedIndex,   ! this.chklstClient.GetItemChecked(_iSelectedIndex) );
                //
                //						this.chkedtCheckAll.CheckedChanged -= new System.EventHandler(this.CheckUncheckAll);
                //						this.chkedtCheckAll.Checked = _bCheckAllState ;
                //						this.chkedtCheckAll.CheckedChanged += new System.EventHandler(this.CheckUncheckAll);
                //
                //
                //					}

                #region commented


                //				System.Collections.ArrayList _alClientName=new ArrayList();
                //				System.Collections.Hashtable _htClientName=new Hashtable();
                //		
                //				bool _bRowIsThere ;
                //
                //				string _strClientName="";
                //
                //				//				System.Data.DataTable dtClientCheckList = new System.Data.DataTable();
                //				//				dtClientCheckList.Columns.Add("Data");
                //				//				dtClientCheckList.Columns.Add("Value");
                //				//				dtClientCheckList.Columns.Add("Check");
                //
                //			
                //				string _strValue; 
                //				int _iValue ;
                //				bool _bCheck;
                //
                //				string _strSelect = "AUECID = " + this.dataRL.AUECID(strTabID);//this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["AUECID"].ToString();
                //				foreach(System.Data.DataRow _drRowSelect in this.dsData.Tables["dtClientList"].Select(_strSelect))
                //				{
                //					_bRowIsThere=false;
                //					_strClientName=_drRowSelect["ClientName"].ToString().Trim();
                //					
                //					_bRowIsThere = _alClientName.Contains(_strClientName);
                //					_bRowIsThere = _htClientName.ContainsKey(_strClientName);
                //											
                //					if(!_bRowIsThere)
                //					{
                //						_alClientName.Add(_strClientName);
                //						
                //						_strValue = _drRowSelect["ClientID"].ToString();
                //						_iValue = (Convert.ToInt32(_strValue));
                //						_bCheck = (Convert.ToInt32((_drRowSelect["ApplyRL"].ToString())) ==1) ? false:true;
                //
                //						_htClientName.Add(_strClientName,_bCheck);
                //
                //						//						object[] row = new object[3]; 
                //						//						row[0] = _strData;
                //						//						row[1] = _iValue;
                //						//						row[2] = _bCheck;
                ////						if(!(this.chklstClient.Items.Contains(_strClientName)))  //"("+_iValue.ToString()+")"+
                ////						{
                ////							this.chklstClient.Items.Add(" "+_strClientName,_bCheck);  //"("+_iValue.ToString()+")"+
                ////							//						dtClientCheckList.Rows.Add(row);
                ////						}
                //						
                //					}
                //				}
                //
                //
                //
                //				for(int i=0;i<this.chklstClient.CheckedItems.Count;i++)
                //				{
                //					
                //					_strValue = this.chklstClient.CheckedItems[i].ToString() ;
                //										
                //					if(_htClientName.ContainsKey(_strValue))
                //					{
                //						_htClientName[_strValue] = true;
                ////						_htClientName.Remove(_strValue);
                ////						_htClientName.Add(_strValue,_bCheck);
                //					}
                //				}
                //
                //				this.chklstClient.Items.Clear();
                //
                //				foreach(  System.Collections.DictionaryEntry _deHTKey in _htClientName)
                //				{
                //					this.chklstClient.Items.Add( _deHTKey.Key.ToString(), (bool)_deHTKey.Value);
                //				}
                //
                //
                //
                ////				string _strValue;
                ////				int i=0,j=this.chklstClient.TopIndex;
                ////				while(i<this.chklstClient.Items.Count)
                ////				{
                ////					
                ////					if(!(this.chklstClient.FindString(" ",j) == j))
                ////					{
                ////						j++;
                ////						continue;
                ////					}
                ////
                //////					_strValue = 
                //////					this.chklstClient.SetSelected(j,false);
                //////					MessageBox.Show(_strValue);
                //////					i++;j++;
                ////
                ////					this.chklstClient.SetSelected(j,true);
                ////					_strValue = this.chklstClient.SelectedValue.ToString();
                ////					if(_alClientName.Contains(_strValue))
                ////					{
                ////						this.chklstClient.SetSelected(j,false);
                ////					}
                ////					else
                ////					{
                ////						this.chklstClient.Items.RemoveAt(j);
                ////					}
                ////					i++;
                ////					j++;
                ////				}
                //
                //
                //
                ////				this.chklstClient.a
                //
                //
                //				//				this.chklstClient.DataSource = dtClientCheckList;
                //				//				this.chklstClient.DisplayMember = "Data";
                //				//				this.chklstClient.ValueMember = "Value";
                //				//				
                //				//				rows
                //				//				for(int i = 0 ; i<dtClientCheckList.Rows.Count;i++)
                //				//                    this.chklstClient.Items.Add(dtClientCheckList.ro
                //
                //				//				checkedLstAUEC.DataSource = dtauec;
                //				//				checkedLstAUEC.DisplayMember 
                //				//this.chklstClient.v
                //				//					this.chklstClient.DataBindings.Add("Value",dtClientCheckList,"Value");
                //						
                #endregion commented

                //				}
            }
        }
        #endregion


        private void LoadRLNameCombo(UltraComboEditor _ceRLName)
        {
            _ceRLName.Items.Clear();
            _ceRLName.Items.Add(Functions.MinValue, " - ");
            foreach (System.Data.DataRow _row in this.dsData.Tables["dtRLList"].Select("AUECID = " + this.dataRL.AUECID(strTabID).ToString()))// this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["AUECID"].ToString()))
            {
                _ceRLName.Items.Add(int.Parse(_row["RLID"].ToString()), _row["RLName"].ToString());
            }

            //			if(_ceRLName.Items.Contains(this.dataRL.



        }



        #region   save data to db
        public string SaveData()

        {
            const string _strKeyHeadingClient = "c:-2";
            //to be corrected

            //			MessageBox.Show(" functon to be corrected ");
            //			object[] parameter = new object[this.dsData.Tables["dtMemoryRL"].Columns.Count];
            //			System.Data.SqlClient.SqlParameter[] _sqlParam = new System.Data.SqlClient.SqlParameter[this.dsData.Tables["dtMemoryRL"].Columns.Count];
            //				
            //			System.Data.DataRow _row = this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID);
            //				
            //			string _strValue="" ;
            //			int _iValue=0;
            //
            //			for(int i=0;i<this.dsData.Tables["dtMemoryRL"].Columns.Count; i++)
            //			{
            //				if(_row[i].GetType().Equals( _strValue.GetType()))
            //				{
            //					_strValue = (IsNull(_row[i]))?"":_row[i].ToString();
            //					_sqlParam[i] = new System.Data.SqlClient.SqlParameter("@"+this.dsData.Tables["dtMemoryRL"].Columns[i].ColumnName.Trim(),_strValue);
            //				}
            //				else if(_row[i].GetType().Equals( _iValue.GetType()))
            //				{
            //
            //					_iValue = (IsNull(_row[i]))?(Functions.MinValue):Convert.ToInt32(_row[i].ToString());
            //					_sqlParam[i] = new System.Data.SqlClient.SqlParameter("@"+this.dsData.Tables["dtMemoryRL"].Columns[i].ColumnName.Trim(),_iValue);
            //				}
            //				else   // null  system.dbnull
            //				{
            //					_sqlParam[i] = new System.Data.SqlClient.SqlParameter("@"+this.dsData.Tables["dtMemoryRL"].Columns[i].ColumnName.Trim(),Functions.MinValue);
            //				}
            //			}
            //
            //			//actula saving call
            //			System.Data.DataTable _dtTemp = BLL.DataHandelingManager.DataStoredProcedure("P_SaveRL",_sqlParam);
            //


            System.Data.DataTable[] _dtTemp = new DataTable[this.dataRL.RoutingPathCount(strTabID)];

            for (int i = 0; i < this.dataRL.RoutingPathCount(strTabID); i++)
            {
                if (this.dataRL.ConditionsCount(strTabID, i) > 0 && this.dataRL.ParameterID(strTabID, i, 0) >= 0)
                {
                    if (this.dataRL.RoutingPathID(strTabID, i) < 0)
                    {
                        _dtTemp[i] = Prana.Admin.RoutingLogic.BLL.DataCallFunctionsManager.SaveRLogic(strMemoryID, strTabID, i);
                        if (Functions.IsNull(_dtTemp[i]))
                        {
                            MessageBox.Show(" Saving Group/client Failed ");
                            return _strKeyHeadingClient;
                        }
                    }
                }

            }

            System.Data.DataTable _dtResult;

            int _iIDRetunedFromSave = Functions.MinValue;
            string _strKey = "";

            const string _cPrefixClient = "c";

            //			const string strTabIDRL="RL";
            //			const string strTabIDGroup="group";
            //			const string strTabIDClient="client";
            //			string _strTabID;

            System.Windows.Forms.NodeTree _nodeClient = (System.Windows.Forms.NodeTree)(this.nodeMain[_strKeyHeadingClient]);



            if (strTabID.Equals("group"))
            {
                //				this.LoadClientCheckList();
                //				this.dataRL.ClientCount(strTabID,0);
                //
                //				System.Data.DataRow[] _rowArray = this.dsData.Tables["dtClientList"].Select("AUECID = " + this.dataRL.AUECID(strTabID));
                //				
                //				
                //				this.dataRL.ClientCount(strTabID,_rowArray.Length);
                //
                //				for(int i =0;i<_rowArray.Length;i++)
                //				{
                //					this.dataRL.ClientID(strTabID,i, Convert.ToInt32(_rowArray[i]["ClientID"].ToString()));
                //					this.dataRL.ClientApplyRL(strTabID,i,Convert.ToInt32(_rowArray[i]["ApplyRL"].ToString()));
                //				}					



                //				this.SaveClientList();

                //				string _strKey="";
                bool _bSeprateClientExist = false;
                string _strClientName = "";
                string _strClientID = "";
                System.Windows.Forms.NodeTree _nodeTemp;
                for (int i = 0; i < this.dataRL.ClientCount(strTabID); i++)
                {
                    _strClientID = this.dataRL.ClientID(strTabID, i).ToString();
                    _strKey = _cPrefixClient + ":" + _strClientID + ":" + this.dataRL.AUECID(strTabID).ToString();
                    _strClientName = this.dsData.Tables["dtClientList"].Select("ClientID = " + _strClientID)[0]["ClientName"].ToString();
                    _nodeTemp = new System.Windows.Forms.NodeTree(_strKey, _strClientName);
                    if (_nodeClient.Contains(_nodeTemp))
                    {
                        _bSeprateClientExist = true;
                        break;
                    }
                }


                if (_bSeprateClientExist)
                {
                    bool _bResult = false;

                    Forms.DialogBox dlgbx = new Forms.DialogBox(" Client, " + _strClientName + " , will be merged in the New Group, " + this.dataRL.Name(strTabID) + " ? ");

                    //			this.Hide();
                    dlgbx.ShowDialog();
                    dlgbx.BringToFront();
                    dlgbx.DesktopLocation = new System.Drawing.Point(-100, 50);

                    _bResult = (dlgbx.DialogResult == DialogResult.Yes) ? true : false;
                    //			this.Show();

                    dlgbx.Close();
                    this.BringToFront();
                    if (!_bResult)
                    {
                        return "g:-1";
                    }


                    _bResult = false;
                    int iDeleteForceFully = 1;
                    _bResult = BLL.DataCallFunctionsManager.Delete(_strKey, iDeleteForceFully);

                    if (!_bResult)
                    {
                        MessageBox.Show(" Failed to Move Client ");
                        return "g:-1";

                    }
                    _nodeClient[_strKey].Remove();
                }

                _strKey = "";
                _dtResult = Prana.Admin.RoutingLogic.BLL.DataCallFunctionsManager.SaveRLGroup(strMemoryID, strTabID);

                this.dataRL.ClientCount(strTabID, 0);

                //				string _strIncompleteMessage="";
                //				if( ! VerificationRLComplete(strTabIDGroup, out _strIncompleteMessage))
                //				{
                //					MessageBox.Show("Group Form Incomplete for : "+ _strIncompleteMessage);
                //					return ;
                //				}



                //modifying tree
                //				int _iIDRetunedFromSave =Functions.MinValue;
                //				string _strKey="";
                //				const string _cPrefixClient = "c";
                const string _cPrefixGroup = "g";
                //				const string _strKeyHeadingClient = "c:-2";
                const string _strKeyHeadingGroup = "g:-3";
                //			const string strTabIDRL="RL";
                //			const string strTabIDGroup="group";
                //			const string strTabIDClient="client";
                //			string _strTabID;

                //				System.Windows.Forms.NodeTree _nodeClient=(System.Windows.Forms.NodeTree)(this.nodeMain.Nodes.GetItem(this.nodeMain.IndexOf(_strKeyHeadingClient)));
                System.Windows.Forms.NodeTree _nodeClientGrp = (System.Windows.Forms.NodeTree)(_nodeClient[_strKeyHeadingGroup]);



                if (Functions.IsNull(_dtResult))
                {
                    MessageBox.Show(" Saving Group/client Failed ");
                    _iIDRetunedFromSave = Functions.MinValue;
                }
                else
                {
                    _iIDRetunedFromSave = int.Parse(_dtResult.Rows[0][0].ToString());
                }
                _strKey = _cPrefixGroup + ":" + _iIDRetunedFromSave.ToString();




                //
                //				if(_iIDRetunedFromSave <0)
                //				{
                //					return;
                //				}
                //tre mod
                if (_iIDRetunedFromSave >= 0)
                {


                    //					string _strKey = _cPrefixGroup +":"+    _iIDRetunedFromSave.ToString();
                    string _strName = this.dataRL.Name(strTabID);//this.dsData.Tables["dtMemoryRL"].Rows.Find(_strMemoryIDRL)["RLName"].ToString();

                    int _iIndex = _nodeClientGrp.IndexOf(_strKey);
                    if (_iIndex >= 0)
                    {
                        //						_nodeClientGrp.Nodes.Remove((System.Windows.Forms.NodeTree)_nodeClientGrp[_iIndex]);
                        //						_nodeClientGrp.Nodes.Insert(_iIndex, _strKey, _strName);	
                        _nodeClientGrp.ChangeAtIndex(_iIndex, _strKey, _strName);

                    }
                    else
                    {
                        if (this.dataRL.ID(strTabID) >= 0)
                        {
                            //remving older one

                            string _strKeyOld = _cPrefixGroup + ":" + this.dataRL.ID(strTabID).ToString();
                            int _iIndexOld = _nodeClientGrp.IndexOf(_strKeyOld);
                            //							if(_iIndexOld >= 0)
                            //							{
                            //								_nodeClientGrp.Nodes.Remove((System.Windows.Forms.NodeTree)_nodeClientGrp.Nodes.GetItem(_iIndexOld));
                            //							}
                            _nodeClientGrp.ChangeAtIndex(_iIndexOld, _strKey, _strName);
                        }
                        else
                        {

                            _nodeClientGrp.Add(_strKey, _strName);
                        }
                        this.dataRL.ID(strTabID, _iIDRetunedFromSave);

                    }

                    //					this.treeMain.AfterSelect -= new System.Windows.Forms.TreeViewEventHandler(this.LoadSelection);
                    //					_nodeClientGrp[_strKey].Selected=true;
                    //					this.treeMain.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.LoadSelection);
                    //					this.LoadGroupClient(_nodeClientGrp[_strKey]);


                }
            }
            else//client
            {

                _dtResult = Prana.Admin.RoutingLogic.BLL.DataCallFunctionsManager.SaveRLClient(strMemoryID, strTabID);

                if (Functions.IsNull(_dtResult))
                {
                    MessageBox.Show(" Saving Group/client Failed ");
                    _iIDRetunedFromSave = Functions.MinValue;
                }
                else
                {
                    _iIDRetunedFromSave = int.Parse(_dtResult.Rows[0][0].ToString());
                }
                _strKey = _cPrefixClient + ":" + _iIDRetunedFromSave.ToString() + ":" + this.dataRL.AUECID(strTabID).ToString();

                if (_iIDRetunedFromSave >= 0)
                {
                    //						return;
                    //					}
                    //						string _strKey = _cPrefixClient+":"+    _iIDRetunedFromSave.ToString()+":"+ this.dataRL.AUECID(strTabID).ToString();
                    string _strName = this.dataRL.Name(strTabID);//this.dsData.Tables["dtMemoryRL"].Rows.Find(_strMemoryIDRL)["RLName"].ToString();

                    int _iIndex = _nodeClient.IndexOf(_strKey);
                    if (_iIndex >= 0)
                    {
                        //						_nodeClient.Nodes.Remove((System.Windows.Forms.NodeTree)_nodeClient.Nodes.GetItem(_iIndex));
                        //						_nodeClient.Nodes.Insert(_iIndex, _strKey, _strName);						
                        _nodeClient.ChangeAtIndex(_iIndex, _strKey, _strName);
                    }
                    else
                    {
                        //						if(this.dataRL.ID(strTabID)>=0)
                        //						{
                        //							//remving older one
                        //						
                        //							string _strKeyOld = _cPrefixClient+":"+this.dataRL.ID(strTabID).ToString()+":"+this.dataRL.AUECID(strTabID).ToString();
                        //							int _iIndexOld = _nodeClient.IndexOf(_strKeyOld);
                        //							//							if(_iIndexOld >= 0)
                        //							//							{
                        //							//								_nodeClient.Nodes.Remove((System.Windows.Forms.NodeTree)_nodeClient.Nodes.GetItem(_iIndexOld));
                        //							//							}
                        //							_nodeClient.ChangeAtIndex(_iIndexOld, _strKey, _strName);
                        //						}
                        //						else
                        //						{
                        _nodeClient.Add(_strKey, _strName);
                        //						}
                        //
                        //						
                        //						this.dataRL.ID(strTabID,_iIDRetunedFromSave);

                    }

                    //					this.treeMain.AfterSelect -= new System.Windows.Forms.TreeViewEventHandler(this.LoadSelection);
                    //					_nodeClient[_strKey].Selected=true;
                    //					this.treeMain.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.LoadSelection);
                }

            }



            //			if(Functions.IsNull(_dtResult))
            //			{
            //				MessageBox.Show(" Saving Group/client Failed ");
            //				return Functions.MinValue;
            //			}
            //
            //
            //			return Convert.ToInt32(_dtResult.Rows[0][0].ToString());






            //			if(Functions.IsNull(_dtTemp[0]))
            //			{
            //				MessageBox.Show(" Saving Group/client Failed ");
            //				return Functions.MinValue;
            //			}
            //			return int.Parse(_dtTemp[0].Rows[0][0].ToString());

            //			System.Data.DataTable _dtTemp = Prana.Admin.RoutingLogic.BLL.DataCallFunctionsManager.SaveGroupClient(strMemoryID, strTabID);
            //
            //			if(IsNull(_dtTemp))
            //			{
            //				MessageBox.Show(" Saving Group/client Failed ");
            //				return Functions.MinValue;
            //			}
            //
            //			return int.Parse(_dtTemp.Rows[0][0].ToString());
            return _strKey;

        }


        public void SaveClientList()
        {
            if (!(Functions.IsNull(this.dataRL) || Functions.IsNull(this.dsData)))
            {

                if (strTabID.Equals("group"))
                {
                    this.LoadClientCheckList();
                    this.dataRL.ClientCount(strTabID, 0);

                    System.Data.DataRow[] _rowArray = this.dsData.Tables["dtClientList"].Select("AUECID = " + this.dataRL.AUECID(strTabID) + " AND Checked =1 ");


                    this.dataRL.ClientCount(strTabID, _rowArray.Length);

                    for (int i = 0; i < _rowArray.Length; i++)
                    {
                        this.dataRL.ClientID(strTabID, i, Convert.ToInt32(_rowArray[i]["ClientID"].ToString()));
                        this.dataRL.ClientApplyRL(strTabID, i, Convert.ToInt32(_rowArray[i]["ApplyRL"].ToString()));
                    }
                }
            }
        }

        #endregion

        //		#region is null
        //		private bool IsNull(Object _obj)
        //		{
        //			if(_obj==null)
        //			{
        //				return true;
        //			}
        //			else if( _obj.Equals(null))
        //			{
        //				return true;
        //			}
        //			else if (_obj.Equals(System.DBNull.Value))
        //			{
        //				return true;
        //			}
        //
        //			return false;
        //		}
        //		#endregion

        private void LoadSelectedRL(object sender, System.EventArgs e)
        {

            int _iIndex = Convert.ToInt32(((UltraComboEditor)sender).Tag.ToString().Trim().Remove(0, "cmbedtRLName".Length));
            if (Functions.IsNull(((UltraComboEditor)this.alRLName[_iIndex]).SelectedItem))
            {
                return;
            }
            int _iValue = (int.Parse(((UltraComboEditor)this.alRLName[_iIndex]).SelectedItem.DataValue.ToString()));

            //			if(_iIndex<this.dataRL.RoutingPathCount(strTabID) && _iValue == this.dataRL.RoutingPathID(strTabID,_iIndex))
            //			{
            //				return ;
            //			}

            if (this.dataRL.RoutingPathCount(strTabID) == 0 && _iValue < 0)
            {
                return;
            }



            if (_iValue < 0)
            {
                if (_iIndex < this.dataRL.RoutingPathCount(strTabID) - 1)
                {

                    this.dataRL.RoutingPathID(strTabID, _iIndex, this.dataRL.RoutingPathID(strTabID, _iIndex + 1));
                    this.dataRL.TradingAccountCount(strTabID, _iIndex, this.dataRL.TradingAccountCount(strTabID, _iIndex + 1));
                    this.dataRL.ConditionsCount(strTabID, _iIndex, this.dataRL.ConditionsCount(strTabID, _iIndex + 1));
                    this.dataRL.TradingAccountIDDefault(strTabID, _iIndex, this.dataRL.TradingAccountIDDefault(strTabID, _iIndex + 1));
                    this.dataRL.RoutingPathName(strTabID, _iIndex, this.dataRL.RoutingPathName(strTabID, _iIndex + 1));

                    for (int i = 0; i < this.dataRL.TradingAccountCount(strTabID, _iIndex); i++)
                    {
                        this.dataRL.TradingAccountID(strTabID, _iIndex, i, this.dataRL.TradingAccountID(strTabID, _iIndex + 1, i));
                        this.dataRL.CounterPartyID(strTabID, _iIndex, i, this.dataRL.CounterPartyID(strTabID, _iIndex + 1, i));
                        this.dataRL.VenueID(strTabID, _iIndex, i, this.dataRL.VenueID(strTabID, _iIndex + 1, i));
                        this.dataRL.CounterPartyVenueID(strTabID, _iIndex, i, this.dataRL.CounterPartyVenueID(strTabID, _iIndex + 1, i));
                    }
                    for (int i = 0; i < this.dataRL.ConditionsCount(strTabID, _iIndex); i++)
                    {
                        this.dataRL.ParameterID(strTabID, _iIndex, i, this.dataRL.ParameterID(strTabID, _iIndex + 1, i));
                        this.dataRL.ParameterValue(strTabID, _iIndex, i, this.dataRL.ParameterValue(strTabID, _iIndex + 1, i));
                        this.dataRL.OperatorID(strTabID, _iIndex, i, this.dataRL.OperatorID(strTabID, _iIndex + 1, i));
                    }

                    ((Controls.IfThenCondition)(this.alIfThenCondition[_iIndex])).LoadData(ref dsData, ref dataRL, strMemoryID, strTabID, _iIndex);

                    ((UltraComboEditor)this.alRLName[_iIndex]).SelectionChanged -= new System.EventHandler(this.LoadSelectedRL);
                    if (!(Functions.IsNull(((UltraComboEditor)this.alRLName[_iIndex + 1]).SelectedItem)))
                    {
                        ((UltraComboEditor)this.alRLName[_iIndex]).Value = ((UltraComboEditor)this.alRLName[_iIndex + 1]).SelectedItem.DataValue;
                    }
                    else
                    {
                        ((UltraComboEditor)this.alRLName[_iIndex]).Value = "";
                    }
                    ((UltraComboEditor)this.alRLName[_iIndex]).SelectionChanged += new System.EventHandler(this.LoadSelectedRL);

                    this.dataRL.RoutingPathID(strTabID, _iIndex + 1, Functions.MinValue);
                    ((UltraComboEditor)(this.alRLName[_iIndex + 1])).Value = Functions.MinValue;
                    //					((UltraComboEditor)(this.alRLName[_iIndex+1])).SelectedItem.DataValue=Functions.MinValue;
                    LoadSelectedRL(((UltraComboEditor)(this.alRLName[_iIndex + 1])), e);



                }
                else
                {
                    if (_iIndex >= this.dataRL.RoutingPathCount(strTabID))
                    {
                        this.dataRL.RoutingPathCount(strTabID, _iIndex + 1);
                    }
                    this.dataRL.RoutingPathID(strTabID, _iIndex, Functions.MinValue);
                    this.dataRL.RoutingPathName(strTabID, _iIndex, "");
                    this.dataRL.TradingAccountCount(strTabID, _iIndex, 0);
                    this.dataRL.ConditionsCount(strTabID, _iIndex, 0);
                    //					this.dataRL.ParameterID(strTabID,_iIndex,0,Functions.MinValue);
                    //					this.dataRL.ParameterValue(strTabID,_iIndex,0,"");
                    this.dataRL.TradingAccountIDDefault(strTabID, _iIndex, Functions.MinValue);
                    this.dataRL.RoutingPathName(strTabID, _iIndex, "");
                    ((UltraComboEditor)this.alRLName[_iIndex]).SelectionChanged -= new System.EventHandler(this.LoadSelectedRL);

                    ((UltraComboEditor)this.alRLName[_iIndex]).Value = "";

                    ((UltraComboEditor)this.alRLName[_iIndex]).SelectionChanged += new System.EventHandler(this.LoadSelectedRL);

                    //					((Controls.IfThenCondition)(this.alIfThenCondition[_iIndex])).
                    ((Controls.IfThenCondition)(this.alIfThenCondition[_iIndex])).LoadData(ref dsData, ref dataRL, strMemoryID, strTabID, _iIndex);
                    if (_iIndex > 0)
                    {
                        this.dataRL.RoutingPathCount(strTabID, _iIndex);
                    }
                    else
                    {
                        this.dataRL.RoutingPathCount(strTabID, 1);
                    }

                }

                //				UserSelectedNone(_iIndex);
                return;
            }

            if (_iIndex >= this.dataRL.RoutingPathCount(strTabID))
            {
                this.dataRL.RoutingPathCount(strTabID, _iIndex + 1);
            }


            this.dataRL.RoutingPathID(strTabID, _iIndex, _iValue);
            BLL.DataCallFunctionsManager.LoadRL(strTabID, _iValue, _iIndex);
            ((Forms.CompanyMaster)(this.ParentForm)).ValueChanged(strTabID, _iIndex, false);
            ((Controls.IfThenCondition)(this.alIfThenCondition[_iIndex])).LoadData(ref dsData, ref dataRL, strMemoryID, strTabID, _iIndex);


            //
            //
            //
            //			int _iRoutingIndex = (Convert.ToInt32(((UltraComboEditor)this.alRank[_iIndex]).SelectedItem.DataValue.ToString()) - 1);
            //			string _strMemoryID = "RL" + _iRoutingIndex.ToString();
            //
            //			/// Auec retention
            //			///
            //			int	_iAUECID = Convert.ToInt32(this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["AUECID"].ToString());
            //			int	_iAssetID= Convert.ToInt32(this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["AssetID"].ToString());
            //			string	_strAssetName= this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["AssetName"].ToString();
            //			int	_iUnderLyingID= Convert.ToInt32(this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["UnderLyingID"].ToString());
            //			string	_strUnderLyingName= this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["UnderLyingName"].ToString();
            //			int	_iAUECExchangeID= Convert.ToInt32(this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["AUECExchangeID"].ToString());
            //			string	_strExchangeName= this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["ExchangeName"].ToString();
            //			
            //			LoadMemoryRLInto(_strMemoryID);
            //
            //			///saving AUEC
            //			///
            //			string _strMemoryIndex;
            //			bool _bRLAlreadyLoaded = false;
            //			for(int i=0;i<3;i++)
            //			{
            //				_strMemoryIndex = "RL" + i.ToString();
            //				_bRLAlreadyLoaded = false;
            //
            //				this.dsData.Tables["dtMemoryRL"].Rows.Find(_strMemoryIndex)["AUECID"] = _iAUECID;
            //				this.dsData.Tables["dtMemoryRL"].Rows.Find(_strMemoryIndex)["AssetID"] = _iAssetID;
            //				this.dsData.Tables["dtMemoryRL"].Rows.Find(_strMemoryIndex)["AssetName"] = _strAssetName;
            //				this.dsData.Tables["dtMemoryRL"].Rows.Find(_strMemoryIndex)["UnderLyingID"] = _iUnderLyingID;
            //				this.dsData.Tables["dtMemoryRL"].Rows.Find(_strMemoryIndex)["UnderLyingName"] = _strUnderLyingName;
            //				this.dsData.Tables["dtMemoryRL"].Rows.Find(_strMemoryIndex)["AUECExchangeID"] = _iAUECExchangeID;
            //				this.dsData.Tables["dtMemoryRL"].Rows.Find(_strMemoryIndex)["ExchangeName"] = _strExchangeName;
            //
            //				_bRLAlreadyLoaded = _iIndex != i  && (int.Parse(((UltraComboEditor)sender).SelectedItem.DataValue.ToString())>=0) && (!IsNull(((UltraComboEditor)this.alRLName[i]).SelectedItem))&& (int.Parse(((UltraComboEditor)this.alRLName[i]).SelectedItem.DataValue.ToString()) == int.Parse(((UltraComboEditor)sender).SelectedItem.DataValue.ToString()) );
            //				if(_bRLAlreadyLoaded)
            //				{					
            //					((UltraComboEditor)sender).Value = Functions.MinValue;
            //				}
            //
            //			}
            //
            //			((Controls.IfThenCondition)(this.alIfThenCondition[_iIndex])).LoadData(ref dsData, ref dataRL, _strMemoryID, strTabID,_iRoutingIndex);
            //			


        }

        private void CheckUncheckAll(object sender, System.EventArgs e)
        {
            //			this.chklstClient.ItemCheck -= new System.Windows.Forms.ItemCheckEventHandler(this.CheckClientChange);
            if (this.chkedtCheckAll.Checked)
            {
                for (int i = 0; i < this.chklstClient.Items.Count; i++)
                {

                    this.chklstClient.SetItemChecked(i, true);
                }
            }
            else
            {
                for (int i = 0; i < this.chklstClient.Items.Count; i++)
                {

                    this.chklstClient.SetItemChecked(i, false);
                }
            }

            if (this.chklstClient.Items.Count > 0)
            {
                bool _bCheckAllState = this.chkedtCheckAll.Checked;
                this.chklstClient.SelectedIndex = 0;// = this.chklstClient.Items[0];
                                                    //this.chklstClient.Selected = this.chklstClient.Items[0];// = this.chklstClient.Items[0];
                int _iSelectedIndex = this.chklstClient.Items.IndexOf(this.chklstClient.SelectedItems[0]);

                this.chklstClient.SetItemChecked(_iSelectedIndex, !this.chklstClient.GetItemChecked(_iSelectedIndex));
                this.chklstClient.SelectedIndex = 0;
                this.CheckClientChange(this.chklstClient, null);
                this.chklstClient.SetItemChecked(_iSelectedIndex, !this.chklstClient.GetItemChecked(_iSelectedIndex));

                this.chkedtCheckAll.CheckedChanged -= new System.EventHandler(this.CheckUncheckAll);
                this.chkedtCheckAll.Checked = _bCheckAllState;
                this.chkedtCheckAll.CheckedChanged += new System.EventHandler(this.CheckUncheckAll);


            }

            //			this.chklstClient.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.CheckClientChange);

        }



        public void GroupToClient(string _strTabID, int _iIndex, bool _bRoutingPath)
        {

            if (_bRoutingPath)
            {
                ((UltraComboEditor)this.alRLName[_iIndex]).SelectionChanged -= new System.EventHandler(this.LoadSelectedRL);
                ((UltraComboEditor)this.alRLName[_iIndex]).Value = Functions.MinValue;
                ((UltraComboEditor)this.alRLName[_iIndex]).SelectionChanged += new System.EventHandler(this.LoadSelectedRL);
                this.dataRL.RoutingPathID(strTabID, _iIndex, Functions.MinValue);
                this.dataRL.RoutingPathName(strTabID, _iIndex, "");
                //}
            }

            if (_strTabID.Equals("group") && this.iSelectedClientID >= 0 && ((Prana.Admin.RoutingLogic.Forms.CompanyMaster)this.ParentForm).UserTriggered)
            {
                //			if(_strTabID.Equals("group") && (this.chklstClient.SelectedItems.Count>0))
                //			{
                //				string _strClientName = this.chklstClient.SelectedItems[0].ToString();
                //				string _strSelect = "AUECID = " + this.dataRL.AUECID(strTabID).ToString()+ " AND ClientName = '" +_strClientName +"'" ;
                //				int _iClientID = Convert.ToInt32((this.dsData.Tables["dtClientList"].Select(_strSelect))[0]["ClientID"].ToString());
                string _strClientName = (this.dsData.Tables["dtClientList"].Select(" ClientID = " + iSelectedClientID.ToString()))[0]["ClientName"].ToString();
                this.strTabID = "client";
                this.dataRL.Name(strTabID, _strClientName);
                this.dataRL.ID(strTabID, iSelectedClientID);

                this.iGroupIDOld = Functions.MinValue;
                this.strGroupNameOld = "New";

                ((Prana.Admin.RoutingLogic.Forms.CompanyMaster)this.ParentForm).SelectClientInTree = true;

                LoadClient();
                //					this.chkedtCheckAll.Checked=true;
                this.chkedtCheckAll.CheckedChanged -= new System.EventHandler(this.CheckUncheckAll);
                this.chkedtCheckAll.Checked = false;
                this.chkedtCheckAll.CheckedChanged += new System.EventHandler(this.CheckUncheckAll);

                for (int i = 0; i < this.chklstClient.Items.Count; i++)
                {
                    this.chklstClient.SetItemChecked(i, false);
                }

                this.chklstClient.ItemCheck -= new System.Windows.Forms.ItemCheckEventHandler(this.CheckClientChange);



                int _iSelectedIndex = this.chklstClient.Items.IndexOf(this.dsData.Tables["dtClientList"].Select("ClientID = " + this.dataRL.ID(strTabID).ToString())[0]["ClientName"].ToString()); ;

                this.chklstClient.SetItemChecked(_iSelectedIndex, true);
                this.chklstClient.SelectedIndex = _iSelectedIndex;
                this.chklstClient.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.CheckClientChange);

                //				MessageBox.Show(  " here " + _iIndex.ToString() + " : GroupToClient " + _strTabID );

                //				MessageBox.Show(  " here " + _iIndex.ToString() + " : GroupToClient " + _strTabID+_bRoutingPath.ToString() + this.chklstClient.SelectedItem.ToString());



            }





            //			MessageBox.Show(  " here " + _iIndex.ToString() + " : GroupToClient " );

        }

        private void OnOffEvent(object sender, System.EventArgs e)
        {
            if (Functions.IsNull(this.dataRL))
            {
                return;
            }

            if (this.iSelectedClientID >= 0 && this.strTabID.Equals("group"))
            {
                if (Functions.IsNull(this.optionsetApplyRL.Value))
                {
                    this.optionsetApplyRL.ValueChanged -= new System.EventHandler(this.OnOffEvent);
                    this.optionsetApplyRL.Value = 0;
                    this.optionsetApplyRL.ValueChanged += new System.EventHandler(this.OnOffEvent);
                }
                foreach (System.Data.DataRow _row in this.dsData.Tables["dtClientList"].Select("ClientID = " + this.iSelectedClientID.ToString()))
                {
                    _row["ApplyRL"] = (int)(this.optionsetApplyRL.Value);
                }
                this.dataRL.ApplyRL(strTabID, 2);
            }
            else
            {
                //				if(this.strTabID.Equals("group"))
                //				{
                int _iValueOptionSet = 2;
                if (Functions.IsNull(this.optionsetApplyRL.Value))
                {
                    _iValueOptionSet = 2;
                }
                else
                {
                    _iValueOptionSet = (int)(this.optionsetApplyRL.Value);

                    foreach (System.Data.DataRow _row in this.dsData.Tables["dtClientList"].Select("Checked = 1"))
                    {
                        _row["ApplyRL"] = _iValueOptionSet;
                    }

                }


                this.dataRL.ApplyRL(strTabID, _iValueOptionSet);

                //				}
                //				else
                //				{
                //
                //
                //
                //
                //				}

            }




            //			this.dataRL.ApplyRL(strTabID,(int)(this.optionsetApplyRL.Value));
            //			if(((int)(this.optionsetApplyRL.Value))==0)
            //			{
            //				this.Enabled=false;
            //				this.optionsetApplyRL.Enabled=true;
            //
            //			}
            //			else
            //			{
            //				this.Enabled=true;
            //			}
        }


        #region event for txt editior- name of RL
        private void UpdateName(object sender, System.EventArgs e)
        {

            if (((Infragistics.Win.UltraWinEditors.UltraTextEditor)sender).ContainsFocus)
            {
                return;
            }
            if (Functions.IsNull(this.dataRL))
            {
                return;
            }
            this.dataRL.Name(strTabID, this.txtedtName.Text);

            //			if(Functions.IsNull(this.dsData.Tables["dtMemoryRL"]))
            //			{
            //				return;
            //			}
            //			this.dsData.Tables["dtMemoryRL"].Rows.Find(strMemoryID)["RLName"] = this.txtedtName.Text ;
            //			DataCascade();		





        }
        #endregion

        private void UnselectClient(object sender, System.EventArgs e)
        {
            //			(this.chklstClient.SelectedItems[0])

            while (this.chklstClient.SelectedItems.Count > 0)
            {
                int i = this.chklstClient.Items.IndexOf(this.chklstClient.SelectedItems[0]);
                this.chklstClient.SetSelected(i, false);
            };



            //			foreach( object _obj in this.chklstClient.SelectedItems)
            //			{
            //				int i = this.chklstClient.Items.IndexOf(_obj);
            //				this.chklstClient.SetSelected(i,false);
            //			}

            //				this.chklstClient.SelectedValue = Functions.MinValue;
        }

        //		private void UnselectCheckAll(object sender, System.Windows.Forms.ItemCheckEventArgs e)
        //		{
        //			
        //		}

        //		private void UnselectCheckAll(object sender, System.EventArgs e)
        //		{
        //			this.chkedtCheckAll.CheckedChanged -= new System.EventHandler(this.CheckUncheckAll);
        //
        //			if( this.chklstClient.Items.Count>0 && (this.chklstClient.CheckedItems.Count == this.chklstClient.Items.Count))
        //			{
        ////				this.chkedtCheckAll.CheckedChanged -= new System.EventHandler(this.CheckUncheckAll);
        //				this.chkedtCheckAll.Checked =  true;
        ////				this.chkedtCheckAll.CheckedChanged += new System.EventHandler(this.CheckUncheckAll);
        //
        //			}
        //			else
        //			{
        ////				this.chkedtCheckAll.CheckedChanged -= new System.EventHandler(this.CheckUncheckAll);
        //				this.chkedtCheckAll.Checked =  false;
        ////				this.chkedtCheckAll.CheckedChanged += new System.EventHandler(this.CheckUncheckAll);
        //
        //			}
        //
        //			this.chkedtCheckAll.CheckedChanged += new System.EventHandler(this.CheckUncheckAll);
        //
        //
        ////			if(this.chklstClient.Items.Count>0)
        ////			{
        ////				if(this.chklstClient.CheckedItems.Count == this.chklstClient.Items.Count)
        ////				{
        ////					this.chkedtCheckAll.CheckedAppearance.BackColor = Color.White;
        ////					this.chkedtCheckAll.Checked =  true;
        ////
        ////				}
        ////				else if(this.chklstClient.CheckedItems.Count==0)
        ////				{
        ////					this.chkedtCheckAll.Checked=false;
        ////					this.chkedtCheckAll.CheckedAppearance.BackColor =Color.White;
        ////
        ////				}
        ////				else 
        ////				{
        ////					this.chkedtCheckAll.CheckedAppearance.BackColor =Color.LightGray;
        ////
        //////					this.chkedtCheckAll.Controls;//.CheckedAppearance.BackColor = Color.LightGray;
        ////
        ////				}
        ////
        ////			}
        ////			else
        ////			{
        ////				this.chkedtCheckAll.Checked=false;
        ////				this.chkedtCheckAll.CheckedAppearance.BackColor=Color.White;
        ////			}
        //		
        //		}

        private void CheckClientChange(object sender, System.Windows.Forms.ItemCheckEventArgs e)
        {
            if (this.chklstClient.SelectedItems.Count > 0)
            {

                //				bool _bCheckedCountToBeOne=false ;
                if ((this.chklstClient.CheckedItems.Count == 0) || (this.chklstClient.CheckedItems.Count == 2 && this.chklstClient.GetItemChecked(this.chklstClient.SelectedIndex)))
                {
                    //						_bCheckedCountToBeOne=true;
                    //					
                    //				}
                    //				else if
                    //				{
                    //
                    //				bool _bCheckedCountToBeOne = 
                    //
                    //				if(this.chklstClient.CheckedItems.Count == 1)
                    //				{
                    if (!this.strTabID.Equals("client"))
                    {
                        string _strClientName = "New";
                        if (this.chklstClient.CheckedItems.Count == 0)
                        {
                            _strClientName = this.chklstClient.SelectedItems[0].ToString();
                        }
                        else
                        {
                            if (this.chklstClient.CheckedItems[0].Equals(this.chklstClient.SelectedItems[0]))
                            {
                                _strClientName = this.chklstClient.CheckedItems[1].ToString();
                            }
                            else
                            {
                                _strClientName = this.chklstClient.CheckedItems[0].ToString();
                            }

                        }
                        this.iSelectedClientID = Convert.ToInt32(this.dsData.Tables["dtClientList"].Select("ClientName = '" + _strClientName + "'")[0]["ClientID"].ToString());
                        this.iGroupIDOld = this.dataRL.ID(strTabID);
                        this.strGroupNameOld = this.dataRL.Name(strTabID);
                        this.strTabID = "client";
                        this.dataRL.Name(strTabID, _strClientName);
                        this.dataRL.ID(strTabID, iSelectedClientID);


                        ((Prana.Admin.RoutingLogic.Forms.CompanyMaster)this.ParentForm).SelectClientInTree = true;

                        this.LoadClient();
                    }

                }
                else  // if(this.chklstClient.CheckedItems.Count >1)
                {
                    if (!this.strTabID.Equals("group"))
                    {
                        this.strTabID = "group";
                        this.dataRL.Name(strTabID, this.strGroupNameOld);
                        this.dataRL.ID(strTabID, this.iGroupIDOld);

                        ((Prana.Admin.RoutingLogic.Forms.CompanyMaster)this.ParentForm).SelectGroupInTree = iGroupIDOld;
                        this.LoadGroup();
                    }
                }
                this.CheckClientTreeUpdate();


            }
            #region commented
            //			else
            //			{
            //				this.chkedtCheckAll.CheckedChanged -= new System.EventHandler(this.CheckUncheckAll);
            //				if( this.chklstClient.Items.Count>0 && (this.chklstClient.CheckedItems.Count == this.chklstClient.Items.Count))
            //				{				
            //					this.chkedtCheckAll.Checked =  true;
            //				}
            //				else
            //				{				
            //					this.chkedtCheckAll.Checked =  false;
            //				}
            //				this.chkedtCheckAll.CheckedChanged += new System.EventHandler(this.CheckUncheckAll);
            //
            //			}
            #endregion
        }

        private void CheckClientTreeUpdate()
        {
            this.chklstClient.ItemCheck -= new System.Windows.Forms.ItemCheckEventHandler(this.CheckClientChange);

            int _iSelectedIndex = this.chklstClient.Items.IndexOf(this.chklstClient.SelectedItems[0]);

            this.chklstClient.SetItemChecked(_iSelectedIndex, !this.chklstClient.GetItemChecked(_iSelectedIndex));

            this.LoadClientCheckList();

            ((Forms.CompanyMaster)(this.ParentForm)).ValueChanged(strTabID, Functions.MinValue, false);

            this.chkedtCheckAll.CheckedChanged -= new System.EventHandler(this.CheckUncheckAll);
            if (this.chklstClient.Items.Count > 0 && (this.chklstClient.CheckedItems.Count == this.chklstClient.Items.Count))
            {
                this.chkedtCheckAll.Checked = true;
            }
            else
            {
                this.chkedtCheckAll.Checked = false;
            }
            this.chkedtCheckAll.CheckedChanged += new System.EventHandler(this.CheckUncheckAll);

            this.chklstClient.SetItemChecked(_iSelectedIndex, !this.chklstClient.GetItemChecked(_iSelectedIndex));
            this.chklstClient.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.CheckClientChange);
        }



        //
        //		#region back color   on/off focus
        //
        //		private void texteditor_GotFocus(object sender, System.EventArgs e)
        //		{
        //			((UltraTextEditor)sender).BackColor = Color.FromArgb(255, 250,205);
        //		}
        //		private void  texteditor_LostFocus(object sender, System.EventArgs e)
        //		{
        //			((UltraTextEditor)sender).BackColor = Color.White;
        //		}
        //		

    }
}
