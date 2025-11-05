using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation;


using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.Interfaces;
using Nirvana.Global;
using System.Reflection;

namespace Nirvana.PNL1
{
	/// <summary>
	/// Summary description for PNLPrefrencesControl.
	/// </summary>
	public class PNLPrefrencesControl : System.Windows.Forms.UserControl, Nirvana.Interfaces.IPreferences
	{
		private Infragistics.Win.UltraWinTabControl.UltraTabControl ultraTabControl1;
		private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl tabPageColors;
		private System.Windows.Forms.Label label15;
		private Infragistics.Win.Misc.UltraGroupBox grpBoxRow;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private Infragistics.Win.Misc.UltraGroupBox grpBoxText;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label11;
		private Infragistics.Win.Misc.UltraGroupBox grpBoxOther;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label label12;
		private Infragistics.Win.UltraWinEditors.UltraColorPicker clrPckLevel2AltRow;
		private Infragistics.Win.UltraWinEditors.UltraColorPicker clrPckLevel2Row;
		private Infragistics.Win.UltraWinEditors.UltraColorPicker clrPckLevel1AltRow;
		private Infragistics.Win.UltraWinEditors.UltraColorPicker clrPckLevel1Row;
		private Infragistics.Win.UltraWinEditors.UltraColorPicker clrPckSummaryText;
		private Infragistics.Win.UltraWinEditors.UltraColorPicker clrPckNE;
		private Infragistics.Win.UltraWinEditors.UltraColorPicker clrPckSE;
		private Infragistics.Win.UltraWinEditors.UltraColorPicker clrPckLE;
		private Infragistics.Win.UltraWinEditors.UltraColorPicker clrPckNP;
		private Infragistics.Win.UltraWinEditors.UltraColorPicker clrPckSP;
		private Infragistics.Win.UltraWinEditors.UltraColorPicker clrPckLP;
		private Infragistics.Win.UltraWinEditors.UltraColorPicker clrPckDefault;
		private Infragistics.Win.UltraWinEditors.UltraColorPicker clrPckSummaryBG;
		private Infragistics.Win.UltraWinEditors.UltraColorPicker clrPckBackground;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.Label label17;
		private System.Windows.Forms.Label label18;
		private Infragistics.Win.UltraWinEditors.UltraColorPicker clrPckChartBorder;
		private Infragistics.Win.UltraWinEditors.UltraColorPicker clrPckChartText;
		private Infragistics.Win.UltraWinEditors.UltraColorPicker clrPckChartBackground;
		private Infragistics.Win.Misc.UltraGroupBox grpBoxChart;
		private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox1;
		private System.Windows.Forms.Label label19;
		private System.Windows.Forms.Label label20;
		private System.Windows.Forms.NumericUpDown numericUpDownRefreshrate;
		private Infragistics.Win.Misc.UltraLabel ultraLabel1;
		private Infragistics.Win.Misc.UltraLabel ultraLabel2;
		private Infragistics.Win.Misc.UltraLabel ultraLabel3;
		private Infragistics.Win.Misc.UltraLabel ultraLabel4;
		private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbLEFeedColumn;
		private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbSEFeedColumn;
		private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbLPFeedColumn;
		private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbSPFeedColumn;
		private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox2;
		private ArrayList arrlstFeedColumn = new ArrayList();
		private System.Windows.Forms.Label label21;
		private System.Windows.Forms.Label label22;
		private System.Windows.Forms.Label label23;
		private System.Windows.Forms.Label label24;
		private System.Windows.Forms.CheckBox chkLegandVisible;
		private Infragistics.Win.UltraWinEditors.UltraColorPicker clrPckLegandBorder;
		private Infragistics.Win.UltraWinEditors.UltraColorPicker clrPckLegandText;
		private Infragistics.Win.UltraWinEditors.UltraColorPicker clrPckLegandBG;
		private Infragistics.Win.Misc.UltraExpandableGroupBox expdGrpBoxLevel1;
		private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanel1;
		private Infragistics.Win.Misc.UltraExpandableGroupBox expdGrpBoxLevel2;
		private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanel2;
		private System.Windows.Forms.Button btnDownL1;
		private System.Windows.Forms.Button btnUpL1;
		private Infragistics.Win.UltraWinEditors.UltraComboEditor ddlSortColumnL1;
		private System.Windows.Forms.RadioButton rbDescL1;
		private System.Windows.Forms.RadioButton rbAscL1;
		private System.Windows.Forms.Button btnMoveRightL1;
		private System.Windows.Forms.ListBox lbAvailableColumnsL1;
		private System.Windows.Forms.Button btnMoveLeftL1;
		private System.Windows.Forms.Button brnMoveAllRightL1;
		private System.Windows.Forms.Button btnMoveAllLeftL1;
		private System.Windows.Forms.ListBox lbDisplayColumnsL1;
		private System.Windows.Forms.ListBox lbAvailableColumnsL2;
		private System.Windows.Forms.ListBox lbDisplayColumnsL2;
		private System.Windows.Forms.Button btnMoveRightL2;
		private System.Windows.Forms.Button btnMoveLeftL2;
		private System.Windows.Forms.Button brnMoveAllRightL2;
		private System.Windows.Forms.Button btnDownL2;
		private System.Windows.Forms.Button btnUpL2;
		private Infragistics.Win.UltraWinEditors.UltraComboEditor ddlSortColumnL2;
		private System.Windows.Forms.RadioButton rbDescL2;
		private System.Windows.Forms.RadioButton rbAscL2;
		private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox3;
		private System.Windows.Forms.Label label25;
		private System.Windows.Forms.Label label26;
		private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbLevel1Column;
		private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbLevel2Column;
		private System.Windows.Forms.Label label27;
		private System.Windows.Forms.Label label28;
		private System.Windows.Forms.Label label29;
		private System.Windows.Forms.Label label30;
		private System.Windows.Forms.ListBox lbTabs;
		private const string FORM_NAME = "PNL: PreferencesControl";
		
		private ArrayList displayListAssetL1	= new ArrayList();
		private ArrayList displayListAssetL2	= new ArrayList();
		private ArrayList displayListSymbolL1	= new ArrayList();
		private ArrayList displayListSymbolL2	= new ArrayList();
		private ArrayList displayListTradingAccountL1	= new ArrayList();
		private ArrayList displayListTradingAccountL2	= new ArrayList();
		private int level1AssetColumn;
		private int level1SymbolColumn;
		private int level1TradingAccountColumn;
		private int level2AssetColumn;
		private int level2SymbolColumn;
		private int level2TradingAccountColumn;
		private int	sortKeyAssetL1;
		private int	sortKeyAssetL2;
		private int	sortKeySymbolL1;
		private int	sortKeySymbolL2;
		private int	sortKeyTradingAccountL1;
		private int	sortKeyTradingAccountL2;
		private bool ascendingAssetL1;	
		private bool ascendingAssetL2;
		private bool ascendingSymbolL1;	
		private bool ascendingSymbolL2;		
		private bool ascendingTradingAccountL1;	
		private bool ascendingTradingAccountL2;

		private System.Windows.Forms.Label label31;
		private System.Windows.Forms.Label label32;
		private System.Windows.Forms.Button brnMoveAllLeftL2;	
		private int currentTab = -1;

		private PNLPrefrencesData _PNLPrefrences;
		private PNLPreferencesManager _PNLPreferencesManager = null;

		private static PNLPrefrencesControl _pnlPrefrencesControl;
		private Infragistics.Win.UltraWinEditors.UltraColorPicker clrPckHeaderLevel1;
		private Infragistics.Win.UltraWinEditors.UltraColorPicker clrPckHeaderLevel2;
		private System.Windows.Forms.Label label33;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl tabPageOther;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl tabPageColumns;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl tabPageCalculation;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl tabPageChart;
		private Infragistics.Win.Misc.UltraLabel ultraLabel5;
		private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbDefaultFeedColumn;
		private System.Windows.Forms.Label label34;
		
		public PNLPrefrencesData Preferences
		{
			get
			{
				return _PNLPrefrences;
			}
		}

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public PNLPrefrencesControl( )
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			
			_PNLPreferencesManager = PNLPreferencesManager.GetInstance() ;
			_PNLPrefrences = (PNLPrefrencesData)_PNLPreferencesManager.GetPreferences();

			PopulateGroupColumnList();
						
			PopulateTabType();
						
			PopulateFeedColumnDropdown();

			PopulatePreferenceData();

			SetPreferencesData();

			#region init asset tab

			this.lbTabs.SelectedIndex = 0;
			
			this.currentTab = lbTabs.SelectedIndex;

			this.SetGroupByLevelColumn();

			this.UpdateLevel1Panel();

			this.UpdateLevel2Panel();

			#endregion

		}

		
		/// <summary>
		/// Singleton instance for LiveFeedPreference class
		/// </summary>
		/// <returns></returns>
		public static PNLPrefrencesControl GetInstance()
		{
			if(_pnlPrefrencesControl == null)
				_pnlPrefrencesControl = new PNLPrefrencesControl();

			return _pnlPrefrencesControl;
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}


		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(PNLPrefrencesControl));
			Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			Infragistics.Win.UltraWinTabControl.UltraTab ultraTab3 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			Infragistics.Win.UltraWinTabControl.UltraTab ultraTab4 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			Infragistics.Win.UltraWinTabControl.UltraTab ultraTab5 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			this.tabPageColumns = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.ultraGroupBox3 = new Infragistics.Win.Misc.UltraGroupBox();
			this.label34 = new System.Windows.Forms.Label();
			this.label26 = new System.Windows.Forms.Label();
			this.label25 = new System.Windows.Forms.Label();
			this.cmbLevel2Column = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
			this.lbTabs = new System.Windows.Forms.ListBox();
			this.cmbLevel1Column = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
			this.expdGrpBoxLevel1 = new Infragistics.Win.Misc.UltraExpandableGroupBox();
			this.ultraExpandableGroupBoxPanel1 = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
			this.label32 = new System.Windows.Forms.Label();
			this.label28 = new System.Windows.Forms.Label();
			this.label27 = new System.Windows.Forms.Label();
			this.btnDownL1 = new System.Windows.Forms.Button();
			this.btnUpL1 = new System.Windows.Forms.Button();
			this.ddlSortColumnL1 = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
			this.rbDescL1 = new System.Windows.Forms.RadioButton();
			this.rbAscL1 = new System.Windows.Forms.RadioButton();
			this.btnMoveRightL1 = new System.Windows.Forms.Button();
			this.lbAvailableColumnsL1 = new System.Windows.Forms.ListBox();
			this.btnMoveLeftL1 = new System.Windows.Forms.Button();
			this.brnMoveAllRightL1 = new System.Windows.Forms.Button();
			this.btnMoveAllLeftL1 = new System.Windows.Forms.Button();
			this.lbDisplayColumnsL1 = new System.Windows.Forms.ListBox();
			this.expdGrpBoxLevel2 = new Infragistics.Win.Misc.UltraExpandableGroupBox();
			this.ultraExpandableGroupBoxPanel2 = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
			this.label31 = new System.Windows.Forms.Label();
			this.label29 = new System.Windows.Forms.Label();
			this.label30 = new System.Windows.Forms.Label();
			this.rbDescL2 = new System.Windows.Forms.RadioButton();
			this.rbAscL2 = new System.Windows.Forms.RadioButton();
			this.btnDownL2 = new System.Windows.Forms.Button();
			this.btnUpL2 = new System.Windows.Forms.Button();
			this.ddlSortColumnL2 = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
			this.btnMoveRightL2 = new System.Windows.Forms.Button();
			this.lbAvailableColumnsL2 = new System.Windows.Forms.ListBox();
			this.btnMoveLeftL2 = new System.Windows.Forms.Button();
			this.brnMoveAllRightL2 = new System.Windows.Forms.Button();
			this.brnMoveAllLeftL2 = new System.Windows.Forms.Button();
			this.lbDisplayColumnsL2 = new System.Windows.Forms.ListBox();
			this.tabPageColors = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.grpBoxOther = new Infragistics.Win.Misc.UltraGroupBox();
			this.clrPckHeaderLevel2 = new Infragistics.Win.UltraWinEditors.UltraColorPicker();
			this.label33 = new System.Windows.Forms.Label();
			this.clrPckSummaryBG = new Infragistics.Win.UltraWinEditors.UltraColorPicker();
			this.label14 = new System.Windows.Forms.Label();
			this.clrPckHeaderLevel1 = new Infragistics.Win.UltraWinEditors.UltraColorPicker();
			this.clrPckBackground = new Infragistics.Win.UltraWinEditors.UltraColorPicker();
			this.label13 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.grpBoxRow = new Infragistics.Win.Misc.UltraGroupBox();
			this.clrPckLevel2AltRow = new Infragistics.Win.UltraWinEditors.UltraColorPicker();
			this.clrPckLevel2Row = new Infragistics.Win.UltraWinEditors.UltraColorPicker();
			this.clrPckLevel1AltRow = new Infragistics.Win.UltraWinEditors.UltraColorPicker();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.clrPckLevel1Row = new Infragistics.Win.UltraWinEditors.UltraColorPicker();
			this.grpBoxText = new Infragistics.Win.Misc.UltraGroupBox();
			this.clrPckDefault = new Infragistics.Win.UltraWinEditors.UltraColorPicker();
			this.label11 = new System.Windows.Forms.Label();
			this.clrPckNP = new Infragistics.Win.UltraWinEditors.UltraColorPicker();
			this.clrPckSP = new Infragistics.Win.UltraWinEditors.UltraColorPicker();
			this.clrPckLP = new Infragistics.Win.UltraWinEditors.UltraColorPicker();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.clrPckNE = new Infragistics.Win.UltraWinEditors.UltraColorPicker();
			this.clrPckSE = new Infragistics.Win.UltraWinEditors.UltraColorPicker();
			this.clrPckLE = new Infragistics.Win.UltraWinEditors.UltraColorPicker();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.clrPckSummaryText = new Infragistics.Win.UltraWinEditors.UltraColorPicker();
			this.label15 = new System.Windows.Forms.Label();
			this.tabPageCalculation = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.ultraGroupBox2 = new Infragistics.Win.Misc.UltraGroupBox();
			this.ultraLabel5 = new Infragistics.Win.Misc.UltraLabel();
			this.cmbDefaultFeedColumn = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
			this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
			this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
			this.cmbLEFeedColumn = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
			this.ultraLabel4 = new Infragistics.Win.Misc.UltraLabel();
			this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
			this.cmbSEFeedColumn = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
			this.cmbLPFeedColumn = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
			this.cmbSPFeedColumn = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
			this.tabPageChart = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.grpBoxChart = new Infragistics.Win.Misc.UltraGroupBox();
			this.chkLegandVisible = new System.Windows.Forms.CheckBox();
			this.label24 = new System.Windows.Forms.Label();
			this.clrPckLegandBorder = new Infragistics.Win.UltraWinEditors.UltraColorPicker();
			this.label21 = new System.Windows.Forms.Label();
			this.clrPckLegandText = new Infragistics.Win.UltraWinEditors.UltraColorPicker();
			this.clrPckLegandBG = new Infragistics.Win.UltraWinEditors.UltraColorPicker();
			this.label22 = new System.Windows.Forms.Label();
			this.label23 = new System.Windows.Forms.Label();
			this.clrPckChartBorder = new Infragistics.Win.UltraWinEditors.UltraColorPicker();
			this.label16 = new System.Windows.Forms.Label();
			this.clrPckChartText = new Infragistics.Win.UltraWinEditors.UltraColorPicker();
			this.clrPckChartBackground = new Infragistics.Win.UltraWinEditors.UltraColorPicker();
			this.label17 = new System.Windows.Forms.Label();
			this.label18 = new System.Windows.Forms.Label();
			this.tabPageOther = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.ultraGroupBox1 = new Infragistics.Win.Misc.UltraGroupBox();
			this.numericUpDownRefreshrate = new System.Windows.Forms.NumericUpDown();
			this.label20 = new System.Windows.Forms.Label();
			this.label19 = new System.Windows.Forms.Label();
			this.ultraTabControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
			this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
			this.tabPageColumns.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox3)).BeginInit();
			this.ultraGroupBox3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.cmbLevel2Column)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.cmbLevel1Column)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.expdGrpBoxLevel1)).BeginInit();
			this.expdGrpBoxLevel1.SuspendLayout();
			this.ultraExpandableGroupBoxPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ddlSortColumnL1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.expdGrpBoxLevel2)).BeginInit();
			this.expdGrpBoxLevel2.SuspendLayout();
			this.ultraExpandableGroupBoxPanel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ddlSortColumnL2)).BeginInit();
			this.tabPageColors.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.grpBoxOther)).BeginInit();
			this.grpBoxOther.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.clrPckHeaderLevel2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.clrPckSummaryBG)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.clrPckHeaderLevel1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.clrPckBackground)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.grpBoxRow)).BeginInit();
			this.grpBoxRow.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.clrPckLevel2AltRow)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.clrPckLevel2Row)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.clrPckLevel1AltRow)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.clrPckLevel1Row)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.grpBoxText)).BeginInit();
			this.grpBoxText.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.clrPckDefault)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.clrPckNP)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.clrPckSP)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.clrPckLP)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.clrPckNE)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.clrPckSE)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.clrPckLE)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.clrPckSummaryText)).BeginInit();
			this.tabPageCalculation.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox2)).BeginInit();
			this.ultraGroupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.cmbDefaultFeedColumn)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.cmbLEFeedColumn)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.cmbSEFeedColumn)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.cmbLPFeedColumn)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.cmbSPFeedColumn)).BeginInit();
			this.tabPageChart.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.grpBoxChart)).BeginInit();
			this.grpBoxChart.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.clrPckLegandBorder)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.clrPckLegandText)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.clrPckLegandBG)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.clrPckChartBorder)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.clrPckChartText)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.clrPckChartBackground)).BeginInit();
			this.tabPageOther.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).BeginInit();
			this.ultraGroupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownRefreshrate)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ultraTabControl1)).BeginInit();
			this.ultraTabControl1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabPageColumns
			// 
			this.tabPageColumns.Controls.Add(this.ultraGroupBox3);
			this.tabPageColumns.Controls.Add(this.expdGrpBoxLevel1);
			this.tabPageColumns.Controls.Add(this.expdGrpBoxLevel2);
			this.tabPageColumns.Location = new System.Drawing.Point(-10000, -10000);
			this.tabPageColumns.Name = "tabPageColumns";
			this.tabPageColumns.Size = new System.Drawing.Size(542, 315);
			// 
			// ultraGroupBox3
			// 
			this.ultraGroupBox3.Controls.Add(this.label34);
			this.ultraGroupBox3.Controls.Add(this.label26);
			this.ultraGroupBox3.Controls.Add(this.label25);
			this.ultraGroupBox3.Controls.Add(this.cmbLevel2Column);
			this.ultraGroupBox3.Controls.Add(this.lbTabs);
			this.ultraGroupBox3.Controls.Add(this.cmbLevel1Column);
			this.ultraGroupBox3.Location = new System.Drawing.Point(12, 2);
			this.ultraGroupBox3.Name = "ultraGroupBox3";
			this.ultraGroupBox3.Size = new System.Drawing.Size(120, 232);
			this.ultraGroupBox3.SupportThemes = false;
			this.ultraGroupBox3.TabIndex = 25;
			// 
			// label34
			// 
			this.label34.Location = new System.Drawing.Point(16, 8);
			this.label34.Name = "label34";
			this.label34.Size = new System.Drawing.Size(88, 23);
			this.label34.TabIndex = 38;
			this.label34.Text = "Tab";
			// 
			// label26
			// 
			this.label26.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label26.Location = new System.Drawing.Point(8, 184);
			this.label26.Name = "label26";
			this.label26.Size = new System.Drawing.Size(88, 16);
			this.label26.TabIndex = 37;
			this.label26.Text = "Level 2 Display";
			// 
			// label25
			// 
			this.label25.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label25.Location = new System.Drawing.Point(8, 144);
			this.label25.Name = "label25";
			this.label25.Size = new System.Drawing.Size(88, 16);
			this.label25.TabIndex = 36;
			this.label25.Text = "Level 1 Display";
			// 
			// cmbLevel2Column
			// 
			this.cmbLevel2Column.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
			this.cmbLevel2Column.FlatMode = true;
			this.cmbLevel2Column.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.cmbLevel2Column.Location = new System.Drawing.Point(8, 200);
			this.cmbLevel2Column.Name = "cmbLevel2Column";
			this.cmbLevel2Column.Size = new System.Drawing.Size(95, 20);
			this.cmbLevel2Column.TabIndex = 35;
			this.cmbLevel2Column.SelectionChanged += new System.EventHandler(this.cmbLevel2Column_SelectionChanged);
			// 
			// lbTabs
			// 
			this.lbTabs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbTabs.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.lbTabs.Items.AddRange(new object[] {
														"Asset",
														"Symbol",
														"Trading Account"});
			this.lbTabs.Location = new System.Drawing.Point(10, 32);
			this.lbTabs.Name = "lbTabs";
			this.lbTabs.Size = new System.Drawing.Size(94, 106);
			this.lbTabs.TabIndex = 11;
			this.lbTabs.SelectedIndexChanged += new System.EventHandler(this.lbTabs_SelectedIndexChanged);
			// 
			// cmbLevel1Column
			// 
			this.cmbLevel1Column.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
			this.cmbLevel1Column.Enabled = false;
			this.cmbLevel1Column.FlatMode = true;
			this.cmbLevel1Column.Location = new System.Drawing.Point(9, 160);
			this.cmbLevel1Column.Name = "cmbLevel1Column";
			this.cmbLevel1Column.Size = new System.Drawing.Size(95, 20);
			this.cmbLevel1Column.TabIndex = 34;
			this.cmbLevel1Column.SelectionChanged += new System.EventHandler(this.cmbLevel1Column_SelectionChanged);
			// 
			// expdGrpBoxLevel1
			// 
			this.expdGrpBoxLevel1.Controls.Add(this.ultraExpandableGroupBoxPanel1);
			this.expdGrpBoxLevel1.Expanded = false;
			this.expdGrpBoxLevel1.ExpandedSize = new System.Drawing.Size(362, 216);
			this.expdGrpBoxLevel1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
			this.expdGrpBoxLevel1.Location = new System.Drawing.Point(134, 0);
			this.expdGrpBoxLevel1.Name = "expdGrpBoxLevel1";
			this.expdGrpBoxLevel1.Size = new System.Drawing.Size(362, 21);
			this.expdGrpBoxLevel1.SupportThemes = false;
			this.expdGrpBoxLevel1.TabIndex = 23;
			this.expdGrpBoxLevel1.Text = "Level 1";
			// 
			// ultraExpandableGroupBoxPanel1
			// 
			this.ultraExpandableGroupBoxPanel1.Controls.Add(this.label32);
			this.ultraExpandableGroupBoxPanel1.Controls.Add(this.label28);
			this.ultraExpandableGroupBoxPanel1.Controls.Add(this.label27);
			this.ultraExpandableGroupBoxPanel1.Controls.Add(this.btnDownL1);
			this.ultraExpandableGroupBoxPanel1.Controls.Add(this.btnUpL1);
			this.ultraExpandableGroupBoxPanel1.Controls.Add(this.ddlSortColumnL1);
			this.ultraExpandableGroupBoxPanel1.Controls.Add(this.rbDescL1);
			this.ultraExpandableGroupBoxPanel1.Controls.Add(this.rbAscL1);
			this.ultraExpandableGroupBoxPanel1.Controls.Add(this.btnMoveRightL1);
			this.ultraExpandableGroupBoxPanel1.Controls.Add(this.lbAvailableColumnsL1);
			this.ultraExpandableGroupBoxPanel1.Controls.Add(this.btnMoveLeftL1);
			this.ultraExpandableGroupBoxPanel1.Controls.Add(this.brnMoveAllRightL1);
			this.ultraExpandableGroupBoxPanel1.Controls.Add(this.btnMoveAllLeftL1);
			this.ultraExpandableGroupBoxPanel1.Controls.Add(this.lbDisplayColumnsL1);
			this.ultraExpandableGroupBoxPanel1.Location = new System.Drawing.Point(-10000, -10000);
			this.ultraExpandableGroupBoxPanel1.Name = "ultraExpandableGroupBoxPanel1";
			this.ultraExpandableGroupBoxPanel1.Size = new System.Drawing.Size(394, 194);
			this.ultraExpandableGroupBoxPanel1.TabIndex = 0;
			this.ultraExpandableGroupBoxPanel1.Visible = false;
			// 
			// label32
			// 
			this.label32.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label32.Location = new System.Drawing.Point(16, 168);
			this.label32.Name = "label32";
			this.label32.Size = new System.Drawing.Size(72, 16);
			this.label32.TabIndex = 48;
			this.label32.Text = "Sort Column";
			// 
			// label28
			// 
			this.label28.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label28.Location = new System.Drawing.Point(209, 3);
			this.label28.Name = "label28";
			this.label28.Size = new System.Drawing.Size(104, 16);
			this.label28.TabIndex = 35;
			this.label28.Text = "Display Columns";
			// 
			// label27
			// 
			this.label27.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label27.Location = new System.Drawing.Point(11, 4);
			this.label27.Name = "label27";
			this.label27.Size = new System.Drawing.Size(104, 16);
			this.label27.TabIndex = 34;
			this.label27.Text = "Available Columns";
			// 
			// btnDownL1
			// 
			this.btnDownL1.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(192)), ((System.Byte)(192)), ((System.Byte)(255)));
			this.btnDownL1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDownL1.BackgroundImage")));
			this.btnDownL1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnDownL1.ForeColor = System.Drawing.SystemColors.Control;
			this.btnDownL1.Location = new System.Drawing.Point(341, 102);
			this.btnDownL1.Name = "btnDownL1";
			this.btnDownL1.Size = new System.Drawing.Size(32, 24);
			this.btnDownL1.TabIndex = 33;
			this.btnDownL1.Click += new System.EventHandler(this.btnDownL1_Click);
			// 
			// btnUpL1
			// 
			this.btnUpL1.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(192)), ((System.Byte)(192)), ((System.Byte)(255)));
			this.btnUpL1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnUpL1.BackgroundImage")));
			this.btnUpL1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnUpL1.Location = new System.Drawing.Point(341, 54);
			this.btnUpL1.Name = "btnUpL1";
			this.btnUpL1.Size = new System.Drawing.Size(32, 24);
			this.btnUpL1.TabIndex = 32;
			this.btnUpL1.Click += new System.EventHandler(this.btnUpL1_Click);
			// 
			// ddlSortColumnL1
			// 
			this.ddlSortColumnL1.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
			this.ddlSortColumnL1.FlatMode = true;
			this.ddlSortColumnL1.Location = new System.Drawing.Point(88, 168);
			this.ddlSortColumnL1.Name = "ddlSortColumnL1";
			this.ddlSortColumnL1.Size = new System.Drawing.Size(104, 20);
			this.ddlSortColumnL1.TabIndex = 31;
			this.ddlSortColumnL1.SelectionChanged += new System.EventHandler(this.ddlSortColumnL1_SelectionChanged);
			// 
			// rbDescL1
			// 
			this.rbDescL1.Checked = true;
			this.rbDescL1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.rbDescL1.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.rbDescL1.Location = new System.Drawing.Point(272, 168);
			this.rbDescL1.Name = "rbDescL1";
			this.rbDescL1.Size = new System.Drawing.Size(56, 16);
			this.rbDescL1.TabIndex = 30;
			this.rbDescL1.TabStop = true;
			this.rbDescL1.Text = "DESC";
			this.rbDescL1.CheckedChanged += new System.EventHandler(this.rbDescL1_CheckedChanged);
			// 
			// rbAscL1
			// 
			this.rbAscL1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.rbAscL1.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.rbAscL1.Location = new System.Drawing.Point(216, 168);
			this.rbAscL1.Name = "rbAscL1";
			this.rbAscL1.Size = new System.Drawing.Size(48, 16);
			this.rbAscL1.TabIndex = 29;
			this.rbAscL1.Text = "ASC";
			this.rbAscL1.CheckedChanged += new System.EventHandler(this.rbAscL1_CheckedChanged);
			// 
			// btnMoveRightL1
			// 
			this.btnMoveRightL1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnMoveRightL1.BackgroundImage")));
			this.btnMoveRightL1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnMoveRightL1.ForeColor = System.Drawing.SystemColors.Control;
			this.btnMoveRightL1.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
			this.btnMoveRightL1.Location = new System.Drawing.Point(155, 29);
			this.btnMoveRightL1.Name = "btnMoveRightL1";
			this.btnMoveRightL1.Size = new System.Drawing.Size(32, 24);
			this.btnMoveRightL1.TabIndex = 28;
			this.btnMoveRightL1.Click += new System.EventHandler(this.btnMoveRightL1_Click);
			// 
			// lbAvailableColumnsL1
			// 
			this.lbAvailableColumnsL1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbAvailableColumnsL1.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.lbAvailableColumnsL1.Location = new System.Drawing.Point(9, 24);
			this.lbAvailableColumnsL1.Name = "lbAvailableColumnsL1";
			this.lbAvailableColumnsL1.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.lbAvailableColumnsL1.Size = new System.Drawing.Size(120, 132);
			this.lbAvailableColumnsL1.TabIndex = 23;
			// 
			// btnMoveLeftL1
			// 
			this.btnMoveLeftL1.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(192)), ((System.Byte)(192)), ((System.Byte)(255)));
			this.btnMoveLeftL1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnMoveLeftL1.BackgroundImage")));
			this.btnMoveLeftL1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnMoveLeftL1.ForeColor = System.Drawing.SystemColors.Control;
			this.btnMoveLeftL1.Location = new System.Drawing.Point(155, 125);
			this.btnMoveLeftL1.Name = "btnMoveLeftL1";
			this.btnMoveLeftL1.Size = new System.Drawing.Size(32, 24);
			this.btnMoveLeftL1.TabIndex = 25;
			this.btnMoveLeftL1.Click += new System.EventHandler(this.btnMoveLeftL1_Click);
			// 
			// brnMoveAllRightL1
			// 
			this.brnMoveAllRightL1.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(192)), ((System.Byte)(192)), ((System.Byte)(255)));
			this.brnMoveAllRightL1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("brnMoveAllRightL1.BackgroundImage")));
			this.brnMoveAllRightL1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.brnMoveAllRightL1.ForeColor = System.Drawing.SystemColors.Control;
			this.brnMoveAllRightL1.Location = new System.Drawing.Point(155, 61);
			this.brnMoveAllRightL1.Name = "brnMoveAllRightL1";
			this.brnMoveAllRightL1.Size = new System.Drawing.Size(32, 24);
			this.brnMoveAllRightL1.TabIndex = 26;
			this.brnMoveAllRightL1.Click += new System.EventHandler(this.btnMoveAllRightL1_Click);
			// 
			// btnMoveAllLeftL1
			// 
			this.btnMoveAllLeftL1.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(192)), ((System.Byte)(192)), ((System.Byte)(255)));
			this.btnMoveAllLeftL1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnMoveAllLeftL1.BackgroundImage")));
			this.btnMoveAllLeftL1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnMoveAllLeftL1.ForeColor = System.Drawing.SystemColors.Control;
			this.btnMoveAllLeftL1.Location = new System.Drawing.Point(155, 93);
			this.btnMoveAllLeftL1.Name = "btnMoveAllLeftL1";
			this.btnMoveAllLeftL1.Size = new System.Drawing.Size(32, 24);
			this.btnMoveAllLeftL1.TabIndex = 27;
			this.btnMoveAllLeftL1.Click += new System.EventHandler(this.btnMoveAllLeftL1_Click);
			// 
			// lbDisplayColumnsL1
			// 
			this.lbDisplayColumnsL1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbDisplayColumnsL1.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.lbDisplayColumnsL1.Location = new System.Drawing.Point(208, 24);
			this.lbDisplayColumnsL1.Name = "lbDisplayColumnsL1";
			this.lbDisplayColumnsL1.Size = new System.Drawing.Size(120, 132);
			this.lbDisplayColumnsL1.TabIndex = 24;
			// 
			// expdGrpBoxLevel2
			// 
			this.expdGrpBoxLevel2.Controls.Add(this.ultraExpandableGroupBoxPanel2);
			this.expdGrpBoxLevel2.ExpandedSize = new System.Drawing.Size(393, 224);
			this.expdGrpBoxLevel2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
			this.expdGrpBoxLevel2.Location = new System.Drawing.Point(136, 22);
			this.expdGrpBoxLevel2.Name = "expdGrpBoxLevel2";
			this.expdGrpBoxLevel2.Size = new System.Drawing.Size(360, 212);
			this.expdGrpBoxLevel2.SupportThemes = false;
			this.expdGrpBoxLevel2.TabIndex = 24;
			this.expdGrpBoxLevel2.Text = "Level 2";
			// 
			// ultraExpandableGroupBoxPanel2
			// 
			this.ultraExpandableGroupBoxPanel2.Controls.Add(this.label31);
			this.ultraExpandableGroupBoxPanel2.Controls.Add(this.label29);
			this.ultraExpandableGroupBoxPanel2.Controls.Add(this.label30);
			this.ultraExpandableGroupBoxPanel2.Controls.Add(this.rbDescL2);
			this.ultraExpandableGroupBoxPanel2.Controls.Add(this.rbAscL2);
			this.ultraExpandableGroupBoxPanel2.Controls.Add(this.btnDownL2);
			this.ultraExpandableGroupBoxPanel2.Controls.Add(this.btnUpL2);
			this.ultraExpandableGroupBoxPanel2.Controls.Add(this.ddlSortColumnL2);
			this.ultraExpandableGroupBoxPanel2.Controls.Add(this.btnMoveRightL2);
			this.ultraExpandableGroupBoxPanel2.Controls.Add(this.lbAvailableColumnsL2);
			this.ultraExpandableGroupBoxPanel2.Controls.Add(this.btnMoveLeftL2);
			this.ultraExpandableGroupBoxPanel2.Controls.Add(this.brnMoveAllRightL2);
			this.ultraExpandableGroupBoxPanel2.Controls.Add(this.brnMoveAllLeftL2);
			this.ultraExpandableGroupBoxPanel2.Controls.Add(this.lbDisplayColumnsL2);
			this.ultraExpandableGroupBoxPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ultraExpandableGroupBoxPanel2.Location = new System.Drawing.Point(3, 19);
			this.ultraExpandableGroupBoxPanel2.Name = "ultraExpandableGroupBoxPanel2";
			this.ultraExpandableGroupBoxPanel2.Size = new System.Drawing.Size(354, 190);
			this.ultraExpandableGroupBoxPanel2.TabIndex = 0;
			// 
			// label31
			// 
			this.label31.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label31.Location = new System.Drawing.Point(16, 166);
			this.label31.Name = "label31";
			this.label31.Size = new System.Drawing.Size(68, 16);
			this.label31.TabIndex = 47;
			this.label31.Text = "Sort Column";
			// 
			// label29
			// 
			this.label29.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label29.Location = new System.Drawing.Point(184, 3);
			this.label29.Name = "label29";
			this.label29.Size = new System.Drawing.Size(104, 16);
			this.label29.TabIndex = 46;
			this.label29.Text = "Display Columns";
			// 
			// label30
			// 
			this.label30.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label30.Location = new System.Drawing.Point(11, 3);
			this.label30.Name = "label30";
			this.label30.Size = new System.Drawing.Size(104, 16);
			this.label30.TabIndex = 45;
			this.label30.Text = "Available Columns";
			// 
			// rbDescL2
			// 
			this.rbDescL2.Checked = true;
			this.rbDescL2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.rbDescL2.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.rbDescL2.Location = new System.Drawing.Point(268, 164);
			this.rbDescL2.Name = "rbDescL2";
			this.rbDescL2.Size = new System.Drawing.Size(52, 16);
			this.rbDescL2.TabIndex = 44;
			this.rbDescL2.TabStop = true;
			this.rbDescL2.Text = "DESC";
			this.rbDescL2.CheckedChanged += new System.EventHandler(this.rbDescL2_CheckedChanged);
			// 
			// rbAscL2
			// 
			this.rbAscL2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.rbAscL2.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.rbAscL2.Location = new System.Drawing.Point(214, 164);
			this.rbAscL2.Name = "rbAscL2";
			this.rbAscL2.Size = new System.Drawing.Size(47, 16);
			this.rbAscL2.TabIndex = 43;
			this.rbAscL2.Text = "ASC";
			this.rbAscL2.CheckedChanged += new System.EventHandler(this.rbAscL2_CheckedChanged);
			// 
			// btnDownL2
			// 
			this.btnDownL2.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(192)), ((System.Byte)(192)), ((System.Byte)(255)));
			this.btnDownL2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDownL2.BackgroundImage")));
			this.btnDownL2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnDownL2.ForeColor = System.Drawing.SystemColors.Control;
			this.btnDownL2.Location = new System.Drawing.Point(314, 98);
			this.btnDownL2.Name = "btnDownL2";
			this.btnDownL2.Size = new System.Drawing.Size(32, 24);
			this.btnDownL2.TabIndex = 42;
			this.btnDownL2.Click += new System.EventHandler(this.btnDownL2_Click);
			// 
			// btnUpL2
			// 
			this.btnUpL2.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(192)), ((System.Byte)(192)), ((System.Byte)(255)));
			this.btnUpL2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnUpL2.BackgroundImage")));
			this.btnUpL2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnUpL2.ForeColor = System.Drawing.SystemColors.Control;
			this.btnUpL2.Location = new System.Drawing.Point(314, 50);
			this.btnUpL2.Name = "btnUpL2";
			this.btnUpL2.Size = new System.Drawing.Size(32, 24);
			this.btnUpL2.TabIndex = 41;
			this.btnUpL2.Click += new System.EventHandler(this.btnUpL2_Click);
			// 
			// ddlSortColumnL2
			// 
			this.ddlSortColumnL2.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
			this.ddlSortColumnL2.FlatMode = true;
			this.ddlSortColumnL2.Location = new System.Drawing.Point(89, 164);
			this.ddlSortColumnL2.Name = "ddlSortColumnL2";
			this.ddlSortColumnL2.Size = new System.Drawing.Size(103, 20);
			this.ddlSortColumnL2.TabIndex = 40;
			this.ddlSortColumnL2.SelectionChanged += new System.EventHandler(this.ddlSortColumnL2_SelectionChanged);
			// 
			// btnMoveRightL2
			// 
			this.btnMoveRightL2.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(192)), ((System.Byte)(192)), ((System.Byte)(255)));
			this.btnMoveRightL2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnMoveRightL2.BackgroundImage")));
			this.btnMoveRightL2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnMoveRightL2.ForeColor = System.Drawing.SystemColors.Control;
			this.btnMoveRightL2.Location = new System.Drawing.Point(142, 31);
			this.btnMoveRightL2.Name = "btnMoveRightL2";
			this.btnMoveRightL2.Size = new System.Drawing.Size(32, 24);
			this.btnMoveRightL2.TabIndex = 39;
			this.btnMoveRightL2.Click += new System.EventHandler(this.btnMoveRightL2_Click);
			// 
			// lbAvailableColumnsL2
			// 
			this.lbAvailableColumnsL2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbAvailableColumnsL2.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.lbAvailableColumnsL2.Location = new System.Drawing.Point(9, 24);
			this.lbAvailableColumnsL2.Name = "lbAvailableColumnsL2";
			this.lbAvailableColumnsL2.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.lbAvailableColumnsL2.Size = new System.Drawing.Size(120, 132);
			this.lbAvailableColumnsL2.TabIndex = 34;
			// 
			// btnMoveLeftL2
			// 
			this.btnMoveLeftL2.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(192)), ((System.Byte)(192)), ((System.Byte)(255)));
			this.btnMoveLeftL2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnMoveLeftL2.BackgroundImage")));
			this.btnMoveLeftL2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnMoveLeftL2.ForeColor = System.Drawing.SystemColors.Control;
			this.btnMoveLeftL2.Location = new System.Drawing.Point(142, 126);
			this.btnMoveLeftL2.Name = "btnMoveLeftL2";
			this.btnMoveLeftL2.Size = new System.Drawing.Size(32, 24);
			this.btnMoveLeftL2.TabIndex = 36;
			this.btnMoveLeftL2.Click += new System.EventHandler(this.btnMoveLeftL2_Click);
			// 
			// brnMoveAllRightL2
			// 
			this.brnMoveAllRightL2.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(192)), ((System.Byte)(192)), ((System.Byte)(255)));
			this.brnMoveAllRightL2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("brnMoveAllRightL2.BackgroundImage")));
			this.brnMoveAllRightL2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.brnMoveAllRightL2.ForeColor = System.Drawing.SystemColors.Control;
			this.brnMoveAllRightL2.Location = new System.Drawing.Point(142, 62);
			this.brnMoveAllRightL2.Name = "brnMoveAllRightL2";
			this.brnMoveAllRightL2.Size = new System.Drawing.Size(32, 24);
			this.brnMoveAllRightL2.TabIndex = 37;
			this.brnMoveAllRightL2.Click += new System.EventHandler(this.btnMoveAllRightL2_Click);
			// 
			// brnMoveAllLeftL2
			// 
			this.brnMoveAllLeftL2.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(192)), ((System.Byte)(192)), ((System.Byte)(255)));
			this.brnMoveAllLeftL2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("brnMoveAllLeftL2.BackgroundImage")));
			this.brnMoveAllLeftL2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.brnMoveAllLeftL2.ForeColor = System.Drawing.SystemColors.Control;
			this.brnMoveAllLeftL2.Location = new System.Drawing.Point(142, 94);
			this.brnMoveAllLeftL2.Name = "brnMoveAllLeftL2";
			this.brnMoveAllLeftL2.Size = new System.Drawing.Size(32, 24);
			this.brnMoveAllLeftL2.TabIndex = 38;
			this.brnMoveAllLeftL2.Click += new System.EventHandler(this.btnMoveAllLeftL1_Click);
			// 
			// lbDisplayColumnsL2
			// 
			this.lbDisplayColumnsL2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbDisplayColumnsL2.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.lbDisplayColumnsL2.Location = new System.Drawing.Point(184, 24);
			this.lbDisplayColumnsL2.Name = "lbDisplayColumnsL2";
			this.lbDisplayColumnsL2.Size = new System.Drawing.Size(120, 132);
			this.lbDisplayColumnsL2.TabIndex = 35;
			// 
			// tabPageColors
			// 
			this.tabPageColors.Controls.Add(this.grpBoxOther);
			this.tabPageColors.Controls.Add(this.grpBoxRow);
			this.tabPageColors.Controls.Add(this.grpBoxText);
			this.tabPageColors.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.tabPageColors.Location = new System.Drawing.Point(-10000, -10000);
			this.tabPageColors.Name = "tabPageColors";
			this.tabPageColors.Size = new System.Drawing.Size(542, 315);
			// 
			// grpBoxOther
			// 
			this.grpBoxOther.Controls.Add(this.clrPckHeaderLevel2);
			this.grpBoxOther.Controls.Add(this.label33);
			this.grpBoxOther.Controls.Add(this.clrPckSummaryBG);
			this.grpBoxOther.Controls.Add(this.label14);
			this.grpBoxOther.Controls.Add(this.clrPckHeaderLevel1);
			this.grpBoxOther.Controls.Add(this.clrPckBackground);
			this.grpBoxOther.Controls.Add(this.label13);
			this.grpBoxOther.Controls.Add(this.label12);
			this.grpBoxOther.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
			this.grpBoxOther.Location = new System.Drawing.Point(8, 132);
			this.grpBoxOther.Name = "grpBoxOther";
			this.grpBoxOther.Size = new System.Drawing.Size(248, 122);
			this.grpBoxOther.SupportThemes = false;
			this.grpBoxOther.TabIndex = 40;
			this.grpBoxOther.Text = "Other";
			// 
			// clrPckHeaderLevel2
			// 
			this.clrPckHeaderLevel2.AlwaysInEditMode = true;
			appearance1.BorderColor = System.Drawing.Color.Black;
			this.clrPckHeaderLevel2.Appearance = appearance1;
			this.clrPckHeaderLevel2.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
			this.clrPckHeaderLevel2.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
			this.clrPckHeaderLevel2.HideSelection = false;
			this.clrPckHeaderLevel2.Location = new System.Drawing.Point(120, 44);
			this.clrPckHeaderLevel2.Name = "clrPckHeaderLevel2";
			this.clrPckHeaderLevel2.Size = new System.Drawing.Size(115, 20);
			this.clrPckHeaderLevel2.TabIndex = 43;
			this.clrPckHeaderLevel2.Text = "Control";
			this.clrPckHeaderLevel2.ColorChanged += new System.EventHandler(this.clrPck_ColorChanged);
			// 
			// label33
			// 
			this.label33.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label33.Location = new System.Drawing.Point(14, 46);
			this.label33.Name = "label33";
			this.label33.Size = new System.Drawing.Size(80, 16);
			this.label33.TabIndex = 42;
			this.label33.Text = "Header Level 2";
			// 
			// clrPckSummaryBG
			// 
			this.clrPckSummaryBG.AlwaysInEditMode = true;
			appearance2.BorderColor = System.Drawing.Color.Black;
			this.clrPckSummaryBG.Appearance = appearance2;
			this.clrPckSummaryBG.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
			this.clrPckSummaryBG.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
			this.clrPckSummaryBG.HideSelection = false;
			this.clrPckSummaryBG.Location = new System.Drawing.Point(120, 94);
			this.clrPckSummaryBG.Name = "clrPckSummaryBG";
			this.clrPckSummaryBG.Size = new System.Drawing.Size(115, 20);
			this.clrPckSummaryBG.TabIndex = 41;
			this.clrPckSummaryBG.Text = "Control";
			this.clrPckSummaryBG.ColorChanged += new System.EventHandler(this.clrPck_ColorChanged);
			// 
			// label14
			// 
			this.label14.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label14.Location = new System.Drawing.Point(14, 96);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(100, 16);
			this.label14.TabIndex = 40;
			this.label14.Text = "Summary BG Color";
			// 
			// clrPckHeaderLevel1
			// 
			this.clrPckHeaderLevel1.AlwaysInEditMode = true;
			appearance3.BorderColor = System.Drawing.Color.Black;
			this.clrPckHeaderLevel1.Appearance = appearance3;
			this.clrPckHeaderLevel1.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
			this.clrPckHeaderLevel1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
			this.clrPckHeaderLevel1.HideSelection = false;
			this.clrPckHeaderLevel1.Location = new System.Drawing.Point(120, 19);
			this.clrPckHeaderLevel1.Name = "clrPckHeaderLevel1";
			this.clrPckHeaderLevel1.Size = new System.Drawing.Size(115, 20);
			this.clrPckHeaderLevel1.TabIndex = 39;
			this.clrPckHeaderLevel1.Text = "Control";
			this.clrPckHeaderLevel1.ColorChanged += new System.EventHandler(this.clrPck_ColorChanged);
			// 
			// clrPckBackground
			// 
			this.clrPckBackground.AlwaysInEditMode = true;
			appearance4.BorderColor = System.Drawing.Color.Black;
			this.clrPckBackground.Appearance = appearance4;
			this.clrPckBackground.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
			this.clrPckBackground.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
			this.clrPckBackground.HideSelection = false;
			this.clrPckBackground.Location = new System.Drawing.Point(120, 70);
			this.clrPckBackground.Name = "clrPckBackground";
			this.clrPckBackground.Size = new System.Drawing.Size(115, 20);
			this.clrPckBackground.TabIndex = 38;
			this.clrPckBackground.Text = "Control";
			this.clrPckBackground.ColorChanged += new System.EventHandler(this.clrPck_ColorChanged);
			// 
			// label13
			// 
			this.label13.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label13.Location = new System.Drawing.Point(14, 21);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(80, 16);
			this.label13.TabIndex = 37;
			this.label13.Text = "Header Level 1";
			// 
			// label12
			// 
			this.label12.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label12.Location = new System.Drawing.Point(14, 72);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(94, 16);
			this.label12.TabIndex = 36;
			this.label12.Text = "Background Color";
			// 
			// grpBoxRow
			// 
			this.grpBoxRow.Controls.Add(this.clrPckLevel2AltRow);
			this.grpBoxRow.Controls.Add(this.clrPckLevel2Row);
			this.grpBoxRow.Controls.Add(this.clrPckLevel1AltRow);
			this.grpBoxRow.Controls.Add(this.label3);
			this.grpBoxRow.Controls.Add(this.label4);
			this.grpBoxRow.Controls.Add(this.label2);
			this.grpBoxRow.Controls.Add(this.label1);
			this.grpBoxRow.Controls.Add(this.clrPckLevel1Row);
			this.grpBoxRow.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
			this.grpBoxRow.Location = new System.Drawing.Point(8, 8);
			this.grpBoxRow.Name = "grpBoxRow";
			this.grpBoxRow.Size = new System.Drawing.Size(248, 120);
			this.grpBoxRow.SupportThemes = false;
			this.grpBoxRow.TabIndex = 38;
			this.grpBoxRow.Text = "Row";
			// 
			// clrPckLevel2AltRow
			// 
			this.clrPckLevel2AltRow.AlwaysInEditMode = true;
			appearance5.BorderColor = System.Drawing.Color.Black;
			this.clrPckLevel2AltRow.Appearance = appearance5;
			this.clrPckLevel2AltRow.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
			this.clrPckLevel2AltRow.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
			this.clrPckLevel2AltRow.HideSelection = false;
			this.clrPckLevel2AltRow.Location = new System.Drawing.Point(120, 92);
			this.clrPckLevel2AltRow.Name = "clrPckLevel2AltRow";
			this.clrPckLevel2AltRow.Size = new System.Drawing.Size(115, 20);
			this.clrPckLevel2AltRow.TabIndex = 32;
			this.clrPckLevel2AltRow.Text = "Control";
			this.clrPckLevel2AltRow.ColorChanged += new System.EventHandler(this.clrPck_ColorChanged);
			// 
			// clrPckLevel2Row
			// 
			this.clrPckLevel2Row.AlwaysInEditMode = true;
			appearance6.BorderColor = System.Drawing.Color.Black;
			this.clrPckLevel2Row.Appearance = appearance6;
			this.clrPckLevel2Row.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
			this.clrPckLevel2Row.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
			this.clrPckLevel2Row.HideSelection = false;
			this.clrPckLevel2Row.Location = new System.Drawing.Point(120, 68);
			this.clrPckLevel2Row.Name = "clrPckLevel2Row";
			this.clrPckLevel2Row.Size = new System.Drawing.Size(115, 20);
			this.clrPckLevel2Row.TabIndex = 31;
			this.clrPckLevel2Row.Text = "Control";
			this.clrPckLevel2Row.ColorChanged += new System.EventHandler(this.clrPck_ColorChanged);
			// 
			// clrPckLevel1AltRow
			// 
			this.clrPckLevel1AltRow.AlwaysInEditMode = true;
			appearance7.BorderColor = System.Drawing.Color.Black;
			this.clrPckLevel1AltRow.Appearance = appearance7;
			this.clrPckLevel1AltRow.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
			this.clrPckLevel1AltRow.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
			this.clrPckLevel1AltRow.Location = new System.Drawing.Point(120, 44);
			this.clrPckLevel1AltRow.Name = "clrPckLevel1AltRow";
			this.clrPckLevel1AltRow.Size = new System.Drawing.Size(115, 20);
			this.clrPckLevel1AltRow.TabIndex = 30;
			this.clrPckLevel1AltRow.Text = "Control";
			this.clrPckLevel1AltRow.ColorChanged += new System.EventHandler(this.clrPck_ColorChanged);
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label3.Location = new System.Drawing.Point(14, 94);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(66, 16);
			this.label3.TabIndex = 29;
			this.label3.Text = "Level 2 (Alt)";
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label4.Location = new System.Drawing.Point(14, 70);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(44, 16);
			this.label4.TabIndex = 28;
			this.label4.Text = "Level 2 ";
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label2.Location = new System.Drawing.Point(14, 46);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(66, 16);
			this.label2.TabIndex = 27;
			this.label2.Text = "Level 1 (Alt)";
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label1.Location = new System.Drawing.Point(14, 22);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(46, 16);
			this.label1.TabIndex = 26;
			this.label1.Text = "Level 1 ";
			// 
			// clrPckLevel1Row
			// 
			this.clrPckLevel1Row.AllowEmpty = false;
			this.clrPckLevel1Row.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
			this.clrPckLevel1Row.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
			this.clrPckLevel1Row.HideSelection = false;
			this.clrPckLevel1Row.Location = new System.Drawing.Point(120, 20);
			this.clrPckLevel1Row.Name = "clrPckLevel1Row";
			this.clrPckLevel1Row.ShowInkButton = Infragistics.Win.ShowInkButton.Never;
			this.clrPckLevel1Row.Size = new System.Drawing.Size(115, 20);
			this.clrPckLevel1Row.TabIndex = 25;
			this.clrPckLevel1Row.Text = "Control";
			this.clrPckLevel1Row.ColorChanged += new System.EventHandler(this.clrPck_ColorChanged);
			// 
			// grpBoxText
			// 
			this.grpBoxText.Controls.Add(this.clrPckDefault);
			this.grpBoxText.Controls.Add(this.label11);
			this.grpBoxText.Controls.Add(this.clrPckNP);
			this.grpBoxText.Controls.Add(this.clrPckSP);
			this.grpBoxText.Controls.Add(this.clrPckLP);
			this.grpBoxText.Controls.Add(this.label8);
			this.grpBoxText.Controls.Add(this.label9);
			this.grpBoxText.Controls.Add(this.label10);
			this.grpBoxText.Controls.Add(this.clrPckNE);
			this.grpBoxText.Controls.Add(this.clrPckSE);
			this.grpBoxText.Controls.Add(this.clrPckLE);
			this.grpBoxText.Controls.Add(this.label7);
			this.grpBoxText.Controls.Add(this.label6);
			this.grpBoxText.Controls.Add(this.label5);
			this.grpBoxText.Controls.Add(this.clrPckSummaryText);
			this.grpBoxText.Controls.Add(this.label15);
			this.grpBoxText.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
			this.grpBoxText.Location = new System.Drawing.Point(264, 8);
			this.grpBoxText.Name = "grpBoxText";
			this.grpBoxText.Size = new System.Drawing.Size(264, 214);
			this.grpBoxText.SupportThemes = false;
			this.grpBoxText.TabIndex = 39;
			this.grpBoxText.Text = "Text";
			// 
			// clrPckDefault
			// 
			this.clrPckDefault.AlwaysInEditMode = true;
			appearance8.BorderColor = System.Drawing.Color.Black;
			this.clrPckDefault.Appearance = appearance8;
			this.clrPckDefault.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
			this.clrPckDefault.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
			this.clrPckDefault.HideSelection = false;
			this.clrPckDefault.Location = new System.Drawing.Point(136, 162);
			this.clrPckDefault.Name = "clrPckDefault";
			this.clrPckDefault.Size = new System.Drawing.Size(115, 20);
			this.clrPckDefault.TabIndex = 41;
			this.clrPckDefault.Text = "Control";
			this.clrPckDefault.ColorChanged += new System.EventHandler(this.clrPck_ColorChanged);
			// 
			// label11
			// 
			this.label11.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label11.Location = new System.Drawing.Point(16, 164);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(50, 16);
			this.label11.TabIndex = 40;
			this.label11.Text = "Default ";
			// 
			// clrPckNP
			// 
			this.clrPckNP.AlwaysInEditMode = true;
			appearance9.BorderColor = System.Drawing.Color.Black;
			this.clrPckNP.Appearance = appearance9;
			this.clrPckNP.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
			this.clrPckNP.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
			this.clrPckNP.HideSelection = false;
			this.clrPckNP.Location = new System.Drawing.Point(136, 138);
			this.clrPckNP.Name = "clrPckNP";
			this.clrPckNP.Size = new System.Drawing.Size(115, 20);
			this.clrPckNP.TabIndex = 39;
			this.clrPckNP.Text = "Control";
			this.clrPckNP.ColorChanged += new System.EventHandler(this.clrPck_ColorChanged);
			// 
			// clrPckSP
			// 
			this.clrPckSP.AlwaysInEditMode = true;
			appearance10.BorderColor = System.Drawing.Color.Black;
			this.clrPckSP.Appearance = appearance10;
			this.clrPckSP.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
			this.clrPckSP.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
			this.clrPckSP.HideSelection = false;
			this.clrPckSP.Location = new System.Drawing.Point(136, 114);
			this.clrPckSP.Name = "clrPckSP";
			this.clrPckSP.Size = new System.Drawing.Size(115, 20);
			this.clrPckSP.TabIndex = 38;
			this.clrPckSP.Text = "Control";
			this.clrPckSP.ColorChanged += new System.EventHandler(this.clrPck_ColorChanged);
			// 
			// clrPckLP
			// 
			this.clrPckLP.AlwaysInEditMode = true;
			appearance11.BorderColor = System.Drawing.Color.Black;
			this.clrPckLP.Appearance = appearance11;
			this.clrPckLP.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
			this.clrPckLP.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
			this.clrPckLP.HideSelection = false;
			this.clrPckLP.Location = new System.Drawing.Point(136, 90);
			this.clrPckLP.Name = "clrPckLP";
			this.clrPckLP.Size = new System.Drawing.Size(115, 20);
			this.clrPckLP.TabIndex = 37;
			this.clrPckLP.Text = "Control";
			this.clrPckLP.ColorChanged += new System.EventHandler(this.clrPck_ColorChanged);
			// 
			// label8
			// 
			this.label8.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label8.Location = new System.Drawing.Point(16, 140);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(50, 16);
			this.label8.TabIndex = 36;
			this.label8.Text = "Net PNL ";
			// 
			// label9
			// 
			this.label9.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label9.Location = new System.Drawing.Point(16, 116);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(62, 16);
			this.label9.TabIndex = 35;
			this.label9.Text = "Short PNL";
			// 
			// label10
			// 
			this.label10.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label10.Location = new System.Drawing.Point(16, 92);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(56, 16);
			this.label10.TabIndex = 34;
			this.label10.Text = "Long PNL";
			// 
			// clrPckNE
			// 
			this.clrPckNE.AlwaysInEditMode = true;
			appearance12.BorderColor = System.Drawing.Color.Black;
			this.clrPckNE.Appearance = appearance12;
			this.clrPckNE.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
			this.clrPckNE.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
			this.clrPckNE.HideSelection = false;
			this.clrPckNE.Location = new System.Drawing.Point(136, 66);
			this.clrPckNE.Name = "clrPckNE";
			this.clrPckNE.Size = new System.Drawing.Size(115, 20);
			this.clrPckNE.TabIndex = 33;
			this.clrPckNE.Text = "Control";
			this.clrPckNE.ColorChanged += new System.EventHandler(this.clrPck_ColorChanged);
			// 
			// clrPckSE
			// 
			this.clrPckSE.AlwaysInEditMode = true;
			appearance13.BorderColor = System.Drawing.Color.Black;
			this.clrPckSE.Appearance = appearance13;
			this.clrPckSE.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
			this.clrPckSE.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
			this.clrPckSE.HideSelection = false;
			this.clrPckSE.Location = new System.Drawing.Point(136, 42);
			this.clrPckSE.Name = "clrPckSE";
			this.clrPckSE.Size = new System.Drawing.Size(115, 20);
			this.clrPckSE.TabIndex = 32;
			this.clrPckSE.Text = "Control";
			this.clrPckSE.ColorChanged += new System.EventHandler(this.clrPck_ColorChanged);
			// 
			// clrPckLE
			// 
			this.clrPckLE.AlwaysInEditMode = true;
			appearance14.BorderColor = System.Drawing.Color.Black;
			this.clrPckLE.Appearance = appearance14;
			this.clrPckLE.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
			this.clrPckLE.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
			this.clrPckLE.HideSelection = false;
			this.clrPckLE.Location = new System.Drawing.Point(136, 18);
			this.clrPckLE.Name = "clrPckLE";
			this.clrPckLE.Size = new System.Drawing.Size(115, 20);
			this.clrPckLE.TabIndex = 31;
			this.clrPckLE.Text = "Control";
			this.clrPckLE.ColorChanged += new System.EventHandler(this.clrPck_ColorChanged);
			// 
			// label7
			// 
			this.label7.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label7.Location = new System.Drawing.Point(16, 68);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(74, 16);
			this.label7.TabIndex = 30;
			this.label7.Text = "Net Exposure";
			// 
			// label6
			// 
			this.label6.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label6.Location = new System.Drawing.Point(16, 44);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(86, 16);
			this.label6.TabIndex = 29;
			this.label6.Text = "Short Exposure ";
			// 
			// label5
			// 
			this.label5.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label5.Location = new System.Drawing.Point(16, 20);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(82, 16);
			this.label5.TabIndex = 28;
			this.label5.Text = "Long Exposure";
			// 
			// clrPckSummaryText
			// 
			this.clrPckSummaryText.AlwaysInEditMode = true;
			appearance15.BorderColor = System.Drawing.Color.Black;
			this.clrPckSummaryText.Appearance = appearance15;
			this.clrPckSummaryText.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
			this.clrPckSummaryText.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
			this.clrPckSummaryText.HideSelection = false;
			this.clrPckSummaryText.Location = new System.Drawing.Point(136, 186);
			this.clrPckSummaryText.Name = "clrPckSummaryText";
			this.clrPckSummaryText.Size = new System.Drawing.Size(115, 20);
			this.clrPckSummaryText.TabIndex = 37;
			this.clrPckSummaryText.Text = "Control";
			this.clrPckSummaryText.ColorChanged += new System.EventHandler(this.clrPck_ColorChanged);
			// 
			// label15
			// 
			this.label15.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label15.Location = new System.Drawing.Point(16, 188);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(82, 16);
			this.label15.TabIndex = 36;
			this.label15.Text = "Summary Color ";
			// 
			// tabPageCalculation
			// 
			this.tabPageCalculation.Controls.Add(this.ultraGroupBox2);
			this.tabPageCalculation.Location = new System.Drawing.Point(-10000, -10000);
			this.tabPageCalculation.Name = "tabPageCalculation";
			this.tabPageCalculation.Size = new System.Drawing.Size(542, 315);
			// 
			// ultraGroupBox2
			// 
			this.ultraGroupBox2.Controls.Add(this.ultraLabel5);
			this.ultraGroupBox2.Controls.Add(this.cmbDefaultFeedColumn);
			this.ultraGroupBox2.Controls.Add(this.ultraLabel1);
			this.ultraGroupBox2.Controls.Add(this.ultraLabel3);
			this.ultraGroupBox2.Controls.Add(this.cmbLEFeedColumn);
			this.ultraGroupBox2.Controls.Add(this.ultraLabel4);
			this.ultraGroupBox2.Controls.Add(this.ultraLabel2);
			this.ultraGroupBox2.Controls.Add(this.cmbSEFeedColumn);
			this.ultraGroupBox2.Controls.Add(this.cmbLPFeedColumn);
			this.ultraGroupBox2.Controls.Add(this.cmbSPFeedColumn);
			this.ultraGroupBox2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
			this.ultraGroupBox2.Location = new System.Drawing.Point(8, 6);
			this.ultraGroupBox2.Name = "ultraGroupBox2";
			this.ultraGroupBox2.Size = new System.Drawing.Size(462, 156);
			this.ultraGroupBox2.SupportThemes = false;
			this.ultraGroupBox2.TabIndex = 8;
			this.ultraGroupBox2.Text = "Calculation Preferences";
			// 
			// ultraLabel5
			// 
			this.ultraLabel5.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.ultraLabel5.Location = new System.Drawing.Point(12, 128);
			this.ultraLabel5.Name = "ultraLabel5";
			this.ultraLabel5.Size = new System.Drawing.Size(158, 14);
			this.ultraLabel5.TabIndex = 8;
			this.ultraLabel5.Text = "Default Selected Feed Column";
			// 
			// cmbDefaultFeedColumn
			// 
			this.cmbDefaultFeedColumn.AlwaysInEditMode = true;
			appearance16.BorderColor = System.Drawing.Color.Black;
			this.cmbDefaultFeedColumn.Appearance = appearance16;
			this.cmbDefaultFeedColumn.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
			this.cmbDefaultFeedColumn.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
			this.cmbDefaultFeedColumn.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
			this.cmbDefaultFeedColumn.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.cmbDefaultFeedColumn.Location = new System.Drawing.Point(300, 126);
			this.cmbDefaultFeedColumn.Name = "cmbDefaultFeedColumn";
			this.cmbDefaultFeedColumn.Size = new System.Drawing.Size(144, 20);
			this.cmbDefaultFeedColumn.SortStyle = Infragistics.Win.ValueListSortStyle.Ascending;
			this.cmbDefaultFeedColumn.TabIndex = 9;
			this.cmbDefaultFeedColumn.ValueChanged += new System.EventHandler(this.cmbDefaultFeedColumn_ValueChanged);
			// 
			// ultraLabel1
			// 
			this.ultraLabel1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.ultraLabel1.Location = new System.Drawing.Point(12, 31);
			this.ultraLabel1.Name = "ultraLabel1";
			this.ultraLabel1.Size = new System.Drawing.Size(204, 18);
			this.ultraLabel1.TabIndex = 0;
			this.ultraLabel1.Text = "Long Exposure Selected Feed Column";
			// 
			// ultraLabel3
			// 
			this.ultraLabel3.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.ultraLabel3.Location = new System.Drawing.Point(12, 79);
			this.ultraLabel3.Name = "ultraLabel3";
			this.ultraLabel3.Size = new System.Drawing.Size(224, 18);
			this.ultraLabel3.TabIndex = 2;
			this.ultraLabel3.Text = "Long PNL Calculation Selected Feed Column";
			// 
			// cmbLEFeedColumn
			// 
			this.cmbLEFeedColumn.AlwaysInEditMode = true;
			appearance17.BorderColor = System.Drawing.Color.Black;
			this.cmbLEFeedColumn.Appearance = appearance17;
			this.cmbLEFeedColumn.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
			this.cmbLEFeedColumn.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
			this.cmbLEFeedColumn.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
			this.cmbLEFeedColumn.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.cmbLEFeedColumn.HideSelection = false;
			this.cmbLEFeedColumn.Location = new System.Drawing.Point(300, 30);
			this.cmbLEFeedColumn.Name = "cmbLEFeedColumn";
			this.cmbLEFeedColumn.Size = new System.Drawing.Size(144, 20);
			this.cmbLEFeedColumn.SortStyle = Infragistics.Win.ValueListSortStyle.Ascending;
			this.cmbLEFeedColumn.TabIndex = 4;
			// 
			// ultraLabel4
			// 
			this.ultraLabel4.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.ultraLabel4.Location = new System.Drawing.Point(12, 104);
			this.ultraLabel4.Name = "ultraLabel4";
			this.ultraLabel4.Size = new System.Drawing.Size(226, 16);
			this.ultraLabel4.TabIndex = 3;
			this.ultraLabel4.Text = "Short PNL Calculation Selected Feed Column";
			// 
			// ultraLabel2
			// 
			this.ultraLabel2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.ultraLabel2.Location = new System.Drawing.Point(12, 56);
			this.ultraLabel2.Name = "ultraLabel2";
			this.ultraLabel2.Size = new System.Drawing.Size(254, 16);
			this.ultraLabel2.TabIndex = 1;
			this.ultraLabel2.Text = "Short Exposure Calculation Selected Feed Column";
			// 
			// cmbSEFeedColumn
			// 
			this.cmbSEFeedColumn.AlwaysInEditMode = true;
			appearance18.BorderColor = System.Drawing.Color.Black;
			this.cmbSEFeedColumn.Appearance = appearance18;
			this.cmbSEFeedColumn.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
			this.cmbSEFeedColumn.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
			this.cmbSEFeedColumn.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
			this.cmbSEFeedColumn.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.cmbSEFeedColumn.Location = new System.Drawing.Point(300, 54);
			this.cmbSEFeedColumn.Name = "cmbSEFeedColumn";
			this.cmbSEFeedColumn.Size = new System.Drawing.Size(144, 20);
			this.cmbSEFeedColumn.SortStyle = Infragistics.Win.ValueListSortStyle.Ascending;
			this.cmbSEFeedColumn.TabIndex = 5;
			// 
			// cmbLPFeedColumn
			// 
			this.cmbLPFeedColumn.AlwaysInEditMode = true;
			appearance19.BorderColor = System.Drawing.Color.Black;
			this.cmbLPFeedColumn.Appearance = appearance19;
			this.cmbLPFeedColumn.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
			this.cmbLPFeedColumn.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
			this.cmbLPFeedColumn.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
			this.cmbLPFeedColumn.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.cmbLPFeedColumn.Location = new System.Drawing.Point(300, 78);
			this.cmbLPFeedColumn.Name = "cmbLPFeedColumn";
			this.cmbLPFeedColumn.Size = new System.Drawing.Size(144, 20);
			this.cmbLPFeedColumn.SortStyle = Infragistics.Win.ValueListSortStyle.Ascending;
			this.cmbLPFeedColumn.TabIndex = 6;
			// 
			// cmbSPFeedColumn
			// 
			this.cmbSPFeedColumn.AlwaysInEditMode = true;
			appearance20.BorderColor = System.Drawing.Color.Black;
			this.cmbSPFeedColumn.Appearance = appearance20;
			this.cmbSPFeedColumn.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
			this.cmbSPFeedColumn.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
			this.cmbSPFeedColumn.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
			this.cmbSPFeedColumn.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.cmbSPFeedColumn.Location = new System.Drawing.Point(300, 102);
			this.cmbSPFeedColumn.Name = "cmbSPFeedColumn";
			this.cmbSPFeedColumn.Size = new System.Drawing.Size(144, 20);
			this.cmbSPFeedColumn.SortStyle = Infragistics.Win.ValueListSortStyle.Ascending;
			this.cmbSPFeedColumn.TabIndex = 7;
			// 
			// tabPageChart
			// 
			this.tabPageChart.Controls.Add(this.grpBoxChart);
			this.tabPageChart.Location = new System.Drawing.Point(-10000, -10000);
			this.tabPageChart.Name = "tabPageChart";
			this.tabPageChart.Size = new System.Drawing.Size(542, 315);
			this.tabPageChart.Paint += new System.Windows.Forms.PaintEventHandler(this.tabPageChart_Paint);
			// 
			// grpBoxChart
			// 
			this.grpBoxChart.Controls.Add(this.chkLegandVisible);
			this.grpBoxChart.Controls.Add(this.label24);
			this.grpBoxChart.Controls.Add(this.clrPckLegandBorder);
			this.grpBoxChart.Controls.Add(this.label21);
			this.grpBoxChart.Controls.Add(this.clrPckLegandText);
			this.grpBoxChart.Controls.Add(this.clrPckLegandBG);
			this.grpBoxChart.Controls.Add(this.label22);
			this.grpBoxChart.Controls.Add(this.label23);
			this.grpBoxChart.Controls.Add(this.clrPckChartBorder);
			this.grpBoxChart.Controls.Add(this.label16);
			this.grpBoxChart.Controls.Add(this.clrPckChartText);
			this.grpBoxChart.Controls.Add(this.clrPckChartBackground);
			this.grpBoxChart.Controls.Add(this.label17);
			this.grpBoxChart.Controls.Add(this.label18);
			this.grpBoxChart.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
			this.grpBoxChart.Location = new System.Drawing.Point(8, 2);
			this.grpBoxChart.Name = "grpBoxChart";
			this.grpBoxChart.Size = new System.Drawing.Size(342, 204);
			this.grpBoxChart.SupportThemes = false;
			this.grpBoxChart.TabIndex = 0;
			this.grpBoxChart.Text = "Chart Preferences";
			// 
			// chkLegandVisible
			// 
			this.chkLegandVisible.Location = new System.Drawing.Point(194, 170);
			this.chkLegandVisible.Name = "chkLegandVisible";
			this.chkLegandVisible.Size = new System.Drawing.Size(28, 24);
			this.chkLegandVisible.TabIndex = 55;
			// 
			// label24
			// 
			this.label24.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label24.Location = new System.Drawing.Point(24, 174);
			this.label24.Name = "label24";
			this.label24.Size = new System.Drawing.Size(78, 16);
			this.label24.TabIndex = 54;
			this.label24.Text = "Legand Visible";
			// 
			// clrPckLegandBorder
			// 
			this.clrPckLegandBorder.AlwaysInEditMode = true;
			appearance21.BorderColor = System.Drawing.Color.Black;
			this.clrPckLegandBorder.Appearance = appearance21;
			this.clrPckLegandBorder.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
			this.clrPckLegandBorder.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
			this.clrPckLegandBorder.Location = new System.Drawing.Point(192, 146);
			this.clrPckLegandBorder.Name = "clrPckLegandBorder";
			this.clrPckLegandBorder.Size = new System.Drawing.Size(128, 20);
			this.clrPckLegandBorder.TabIndex = 53;
			this.clrPckLegandBorder.Text = "Control";
			this.clrPckLegandBorder.ColorChanged += new System.EventHandler(this.clrPck_ColorChanged);
			// 
			// label21
			// 
			this.label21.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label21.Location = new System.Drawing.Point(24, 148);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(110, 16);
			this.label21.TabIndex = 52;
			this.label21.Text = "Legand Border Color";
			// 
			// clrPckLegandText
			// 
			this.clrPckLegandText.AlwaysInEditMode = true;
			appearance22.BorderColor = System.Drawing.Color.Black;
			this.clrPckLegandText.Appearance = appearance22;
			this.clrPckLegandText.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
			this.clrPckLegandText.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
			this.clrPckLegandText.Location = new System.Drawing.Point(192, 98);
			this.clrPckLegandText.Name = "clrPckLegandText";
			this.clrPckLegandText.ShowOverflowIndicator = true;
			this.clrPckLegandText.Size = new System.Drawing.Size(128, 20);
			this.clrPckLegandText.TabIndex = 51;
			this.clrPckLegandText.Text = "Control";
			this.clrPckLegandText.ColorChanged += new System.EventHandler(this.clrPck_ColorChanged);
			// 
			// clrPckLegandBG
			// 
			this.clrPckLegandBG.AlwaysInEditMode = true;
			appearance23.BorderColor = System.Drawing.Color.Black;
			this.clrPckLegandBG.Appearance = appearance23;
			this.clrPckLegandBG.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
			this.clrPckLegandBG.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
			this.clrPckLegandBG.Location = new System.Drawing.Point(192, 122);
			this.clrPckLegandBG.Name = "clrPckLegandBG";
			this.clrPckLegandBG.Size = new System.Drawing.Size(128, 20);
			this.clrPckLegandBG.TabIndex = 50;
			this.clrPckLegandBG.Text = "Control";
			this.clrPckLegandBG.ColorChanged += new System.EventHandler(this.clrPck_ColorChanged);
			// 
			// label22
			// 
			this.label22.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label22.Location = new System.Drawing.Point(24, 100);
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size(98, 16);
			this.label22.TabIndex = 49;
			this.label22.Text = "Legand Text Color";
			// 
			// label23
			// 
			this.label23.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label23.Location = new System.Drawing.Point(24, 124);
			this.label23.Name = "label23";
			this.label23.Size = new System.Drawing.Size(136, 16);
			this.label23.TabIndex = 48;
			this.label23.Text = "Legand Background Color";
			// 
			// clrPckChartBorder
			// 
			this.clrPckChartBorder.AlwaysInEditMode = true;
			appearance24.BorderColor = System.Drawing.Color.Black;
			this.clrPckChartBorder.Appearance = appearance24;
			this.clrPckChartBorder.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
			this.clrPckChartBorder.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
			this.clrPckChartBorder.Location = new System.Drawing.Point(192, 74);
			this.clrPckChartBorder.Name = "clrPckChartBorder";
			this.clrPckChartBorder.Size = new System.Drawing.Size(128, 20);
			this.clrPckChartBorder.TabIndex = 47;
			this.clrPckChartBorder.Text = "Control";
			this.clrPckChartBorder.ColorChanged += new System.EventHandler(this.clrPck_ColorChanged);
			// 
			// label16
			// 
			this.label16.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label16.Location = new System.Drawing.Point(24, 76);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(74, 16);
			this.label16.TabIndex = 46;
			this.label16.Text = "Border Color";
			// 
			// clrPckChartText
			// 
			this.clrPckChartText.AlwaysInEditMode = true;
			appearance25.BorderColor = System.Drawing.Color.Black;
			this.clrPckChartText.Appearance = appearance25;
			this.clrPckChartText.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
			this.clrPckChartText.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
			this.clrPckChartText.Location = new System.Drawing.Point(192, 26);
			this.clrPckChartText.Name = "clrPckChartText";
			this.clrPckChartText.ShowOverflowIndicator = true;
			this.clrPckChartText.Size = new System.Drawing.Size(128, 20);
			this.clrPckChartText.TabIndex = 45;
			this.clrPckChartText.Text = "Control";
			this.clrPckChartText.ColorChanged += new System.EventHandler(this.clrPck_ColorChanged);
			// 
			// clrPckChartBackground
			// 
			this.clrPckChartBackground.AlwaysInEditMode = true;
			appearance26.BorderColor = System.Drawing.Color.Black;
			this.clrPckChartBackground.Appearance = appearance26;
			this.clrPckChartBackground.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
			this.clrPckChartBackground.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
			this.clrPckChartBackground.Location = new System.Drawing.Point(192, 50);
			this.clrPckChartBackground.Name = "clrPckChartBackground";
			this.clrPckChartBackground.Size = new System.Drawing.Size(128, 20);
			this.clrPckChartBackground.TabIndex = 44;
			this.clrPckChartBackground.Text = "Control";
			this.clrPckChartBackground.ColorChanged += new System.EventHandler(this.clrPck_ColorChanged);
			// 
			// label17
			// 
			this.label17.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label17.Location = new System.Drawing.Point(24, 28);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(58, 16);
			this.label17.TabIndex = 43;
			this.label17.Text = "Text Color";
			// 
			// label18
			// 
			this.label18.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label18.Location = new System.Drawing.Point(24, 52);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(100, 16);
			this.label18.TabIndex = 42;
			this.label18.Text = "Background Color";
			// 
			// tabPageOther
			// 
			this.tabPageOther.Controls.Add(this.ultraGroupBox1);
			this.tabPageOther.Location = new System.Drawing.Point(1, 20);
			this.tabPageOther.Name = "tabPageOther";
			this.tabPageOther.Size = new System.Drawing.Size(542, 315);
			// 
			// ultraGroupBox1
			// 
			this.ultraGroupBox1.Controls.Add(this.numericUpDownRefreshrate);
			this.ultraGroupBox1.Controls.Add(this.label20);
			this.ultraGroupBox1.Controls.Add(this.label19);
			this.ultraGroupBox1.Location = new System.Drawing.Point(10, 16);
			this.ultraGroupBox1.Name = "ultraGroupBox1";
			this.ultraGroupBox1.Size = new System.Drawing.Size(415, 80);
			this.ultraGroupBox1.SupportThemes = false;
			this.ultraGroupBox1.TabIndex = 0;
			// 
			// numericUpDownRefreshrate
			// 
			this.numericUpDownRefreshrate.DecimalPlaces = 1;
			this.numericUpDownRefreshrate.Location = new System.Drawing.Point(160, 29);
			this.numericUpDownRefreshrate.Maximum = new System.Decimal(new int[] {
																					 1000,
																					 0,
																					 0,
																					 0});
			this.numericUpDownRefreshrate.Minimum = new System.Decimal(new int[] {
																					 1,
																					 0,
																					 0,
																					 0});
			this.numericUpDownRefreshrate.Name = "numericUpDownRefreshrate";
			this.numericUpDownRefreshrate.Size = new System.Drawing.Size(64, 21);
			this.numericUpDownRefreshrate.TabIndex = 3;
			this.numericUpDownRefreshrate.Value = new System.Decimal(new int[] {
																				   5,
																				   0,
																				   0,
																				   0});
			// 
			// label20
			// 
			this.label20.Location = new System.Drawing.Point(232, 33);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(40, 13);
			this.label20.TabIndex = 2;
			this.label20.Text = "sec";
			// 
			// label19
			// 
			this.label19.Location = new System.Drawing.Point(24, 32);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(128, 14);
			this.label19.TabIndex = 1;
			this.label19.Text = "Calculation Refresh Rate";
			// 
			// ultraTabControl1
			// 
			appearance27.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(152)), ((System.Byte)(10)));
			appearance27.BackColor2 = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			appearance27.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
			appearance27.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
			this.ultraTabControl1.ActiveTabAppearance = appearance27;
			appearance28.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			appearance28.BackGradientStyle = Infragistics.Win.GradientStyle.None;
			this.ultraTabControl1.Appearance = appearance28;
			this.ultraTabControl1.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.ultraTabControl1.Controls.Add(this.ultraTabSharedControlsPage1);
			this.ultraTabControl1.Controls.Add(this.tabPageOther);
			this.ultraTabControl1.Controls.Add(this.tabPageColumns);
			this.ultraTabControl1.Controls.Add(this.tabPageColors);
			this.ultraTabControl1.Controls.Add(this.tabPageCalculation);
			this.ultraTabControl1.Controls.Add(this.tabPageChart);
			this.ultraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ultraTabControl1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.ultraTabControl1.Location = new System.Drawing.Point(0, 0);
			this.ultraTabControl1.Name = "ultraTabControl1";
			this.ultraTabControl1.SharedControlsPage = this.ultraTabSharedControlsPage1;
			this.ultraTabControl1.Size = new System.Drawing.Size(544, 336);
			this.ultraTabControl1.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.Excel;
			this.ultraTabControl1.TabIndex = 0;
			ultraTab1.Key = "Columns";
			ultraTab1.TabPage = this.tabPageColumns;
			ultraTab1.Text = "Columns";
			ultraTab2.Key = "Colors";
			ultraTab2.TabPage = this.tabPageColors;
			ultraTab2.Text = "Colors";
			ultraTab3.Key = "Calculations";
			ultraTab3.TabPage = this.tabPageCalculation;
			ultraTab3.Text = "Calculations";
			ultraTab4.Key = "Chart";
			ultraTab4.TabPage = this.tabPageChart;
			ultraTab4.Text = "Chart";
			ultraTab5.Key = "General";
			ultraTab5.TabPage = this.tabPageOther;
			ultraTab5.Text = "General";
			this.ultraTabControl1.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
																									   ultraTab1,
																									   ultraTab2,
																									   ultraTab3,
																									   ultraTab4,
																									   ultraTab5});
			this.ultraTabControl1.ViewStyle = Infragistics.Win.UltraWinTabControl.ViewStyle.VisualStudio2005;
			this.ultraTabControl1.SelectedTabChanged += new Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventHandler(this.ultraTabControl1_SelectedTabChanged);
			// 
			// ultraTabSharedControlsPage1
			// 
			this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
			this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
			this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(542, 315);
			// 
			// PNLPrefrencesControl
			// 
			this.Controls.Add(this.ultraTabControl1);
			this.Name = "PNLPrefrencesControl";
			this.Size = new System.Drawing.Size(544, 336);
			this.tabPageColumns.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox3)).EndInit();
			this.ultraGroupBox3.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.cmbLevel2Column)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.cmbLevel1Column)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.expdGrpBoxLevel1)).EndInit();
			this.expdGrpBoxLevel1.ResumeLayout(false);
			this.ultraExpandableGroupBoxPanel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.ddlSortColumnL1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.expdGrpBoxLevel2)).EndInit();
			this.expdGrpBoxLevel2.ResumeLayout(false);
			this.ultraExpandableGroupBoxPanel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.ddlSortColumnL2)).EndInit();
			this.tabPageColors.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.grpBoxOther)).EndInit();
			this.grpBoxOther.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.clrPckHeaderLevel2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.clrPckSummaryBG)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.clrPckHeaderLevel1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.clrPckBackground)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.grpBoxRow)).EndInit();
			this.grpBoxRow.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.clrPckLevel2AltRow)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.clrPckLevel2Row)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.clrPckLevel1AltRow)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.clrPckLevel1Row)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.grpBoxText)).EndInit();
			this.grpBoxText.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.clrPckDefault)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.clrPckNP)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.clrPckSP)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.clrPckLP)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.clrPckNE)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.clrPckSE)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.clrPckLE)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.clrPckSummaryText)).EndInit();
			this.tabPageCalculation.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox2)).EndInit();
			this.ultraGroupBox2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.cmbDefaultFeedColumn)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.cmbLEFeedColumn)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.cmbSEFeedColumn)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.cmbLPFeedColumn)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.cmbSPFeedColumn)).EndInit();
			this.tabPageChart.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.grpBoxChart)).EndInit();
			this.grpBoxChart.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.clrPckLegandBorder)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.clrPckLegandText)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.clrPckLegandBG)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.clrPckChartBorder)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.clrPckChartText)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.clrPckChartBackground)).EndInit();
			this.tabPageOther.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).EndInit();
			this.ultraGroupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownRefreshrate)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ultraTabControl1)).EndInit();
			this.ultraTabControl1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region Populate Dropdowns

		private void PopulatePreferenceData()
		{
			displayListAssetL1	= _PNLPrefrences.DisplayListAssetL1;
			displayListAssetL2	= _PNLPrefrences.DisplayListAssetL2;
			displayListSymbolL1	= _PNLPrefrences.DisplayListSymbolL1;
			displayListSymbolL2	= _PNLPrefrences.DisplayListSymbolL2;
			displayListTradingAccountL1	= _PNLPrefrences.DisplayListTradingAccountL1; 
			displayListTradingAccountL2	= _PNLPrefrences.DisplayListTradingAccountL2;
			
			level1AssetColumn = _PNLPrefrences.Level1AssetColumn; 
			level1SymbolColumn = _PNLPrefrences.Level1SymbolColumn;  
			level1TradingAccountColumn = _PNLPrefrences.Level1TradingAccountColumn; 
				
			level2AssetColumn = _PNLPrefrences.Level2AssetColumn; 
			level2SymbolColumn = _PNLPrefrences.Level2SymbolColumn;  
			level2TradingAccountColumn = _PNLPrefrences.Level2TradingAccountColumn; 
				
			sortKeyAssetL1 = _PNLPrefrences.SortKeyAssetL1;
			sortKeyAssetL2 = _PNLPrefrences.SortKeyAssetL2;
			sortKeySymbolL1 = _PNLPrefrences.SortKeySymbolL1;
			sortKeySymbolL2 = _PNLPrefrences.SortKeySymbolL2;
			sortKeyTradingAccountL1 = _PNLPrefrences.SortKeyTradingAccountL1;
			sortKeyTradingAccountL2 = _PNLPrefrences.SortKeyTradingAccountL2;
			ascendingAssetL1 = _PNLPrefrences.AscendingAssetL1;	
			ascendingAssetL2 = _PNLPrefrences.AscendingAssetL2;
			ascendingSymbolL1 = _PNLPrefrences.AscendingSymbolL1;	
			ascendingSymbolL2 = _PNLPrefrences.AscendingSymbolL2;		
			ascendingTradingAccountL1 = _PNLPrefrences.AscendingTradingAccountL1;	
			ascendingTradingAccountL2 = _PNLPrefrences.AscendingTradingAccountL2;
		}

		private void PopulateTabType()
		{
			PNLTab pnlTab = new PNLTab();
	
			this.lbTabs.SelectedIndexChanged -=new EventHandler(lbTabs_SelectedIndexChanged);

			this.lbTabs.DataSource = Enum.GetValues(pnlTab.GetType());

			this.lbTabs.SelectedIndexChanged +=new EventHandler(lbTabs_SelectedIndexChanged);
		}


		private void PopulateFeedColumnDropdown()
		{
			int count = PNLPrefrencesData.PNLCalcuationColumnNames.Length;

			for(int i=0;i<count;i++)
			{
				Infragistics.Win.ValueListItem item = new Infragistics.Win.ValueListItem();
				item.DisplayText  = PNLPrefrencesData.PNLCalcuationColumnNames[i];
				item.DataValue = i; 
				this.cmbLEFeedColumn.Items.Add(item); 
				this.cmbSEFeedColumn.Items.Add(item); 
				this.cmbLPFeedColumn.Items.Add(item); 
				this.cmbSPFeedColumn.Items.Add(item); 
				this.cmbDefaultFeedColumn.Items.Add(item);
			}
		}


		private void PopulateGroupColumnList()
		{
			PNLGroupColumns pnlGroupColumns = new PNLGroupColumns();
			ArrayList columnList = new ArrayList();
			string [] arrColumnList = (string [])Enum.GetNames(pnlGroupColumns.GetType());
			Array arrColumnValueList = Enum.GetValues(pnlGroupColumns.GetType());
			ArrayList arrlstColumnValueList = new ArrayList(arrColumnValueList);
			columnList.AddRange(arrColumnList);
			
			for(int i=0;i<arrColumnList.Length;i++)
			{
				Infragistics.Win.ValueListItem item = new Infragistics.Win.ValueListItem();
				item.DisplayText = arrColumnList[i];
				item.DataValue = (int)arrlstColumnValueList[i];
				this.cmbLevel1Column.Items.Add(item);
				this.cmbLevel2Column.Items.Add(item);
			}
		
			this.cmbLevel1Column.SelectedIndex = 0;
			this.cmbLevel2Column.SelectedIndex = 0;

		}


		private void PopulateAvailableLevel1ColumnList(PNLTab pnlTabType)
		{
			int level1SelectedColumn = ((int)this.cmbLevel1Column.SelectedItem.DataValue);
			ArrayList availableColumnList = null;

			switch(level1SelectedColumn)
			{
				case (int)PNLColumns.Symbol:
					availableColumnList = _PNLPrefrences.AvailableColumnForSymbol;
					break;

				case (int)PNLColumns.Exchange:
					availableColumnList = _PNLPrefrences.AvailableColumnForExchange;
					break;

				case (int)PNLColumns.Asset:
					availableColumnList = _PNLPrefrences.AvailableColumnForAsset;
					break;

				case (int)PNLColumns.TradingAccount:
					availableColumnList = _PNLPrefrences.AvailableColumnForTradingAccount;
					break;

				case (int)PNLColumns.User:
					availableColumnList = _PNLPrefrences.AvailableColumnForUser;
					break;

				case (int)PNLColumns.Client:
					availableColumnList = _PNLPrefrences.AvailableColumnForClient;
					break;

				case (int)PNLColumns.Side:
					availableColumnList = _PNLPrefrences.AvailableColumnForSide;
					break;
			}
			
			int columnCount = availableColumnList.Count;
			
			this.lbAvailableColumnsL1.DataSource = null;

			this.lbAvailableColumnsL1.Items.Clear();

			for(int i=0;i<columnCount;i++)
			{
				string columnName = PNLPrefrencesData.PNLColumnNames[(int)availableColumnList[i]];
				this.lbAvailableColumnsL1.Items.Add(columnName);
			}

			//remove the columns from display list of l1

			ArrayList displayListL1 = null;

			switch(pnlTabType)
			{
				case PNLTab.Asset:
					displayListL1 = this.displayListAssetL1;
					break;

				case PNLTab.Symbol:
					displayListL1 = this.displayListSymbolL1;
					break;

				case PNLTab.TradingAccount:
					displayListL1 = this.displayListTradingAccountL1;
					break;
			}

			int displayColumnCount = displayListL1.Count;
			ArrayList ColumnList = new ArrayList();
			
			for(int i=0;i<displayColumnCount;i++)
			{
				bool columnFound = false;
				string columnName = PNLPrefrencesData.PNLColumnNames[(int)displayListL1[i]];

				for(int j=0;j<columnCount;j++)
				{
					string columnDisplayName = PNLPrefrencesData.PNLColumnNames[(int)availableColumnList[j]];

					if(columnDisplayName.Equals(columnName))
					{
						columnFound = true;
						break;
					}
				}

				if(columnFound==false)
				{
					ColumnList.Add(columnName);
				}
			}

			//Update the display list listbox
			PopulateDisplayColumnLevel1List(pnlTabType);

			//Remove the columns from the display level 2 box.
			for(int j=0;j<ColumnList.Count;j++)
			{
				if(this.lbDisplayColumnsL1.Items.Contains((string)ColumnList[j]))
					this.lbDisplayColumnsL1.Items.Remove((string)ColumnList[j]);
			}
		}


		private void PopulateAvailableLevel2ColumnList(PNLTab pnlTabType)
		{
			int level2SelectedColumn = ((int)this.cmbLevel2Column.SelectedItem.DataValue);
			ArrayList availableColumnList = null;

			switch(level2SelectedColumn)
			{
				case (int)PNLColumns.Symbol:
					availableColumnList = _PNLPrefrences.AvailableColumnForSymbol;
					break;

				case (int)PNLColumns.Exchange:
					availableColumnList = _PNLPrefrences.AvailableColumnForExchange;
					break;

				case (int)PNLColumns.Asset:
					availableColumnList = _PNLPrefrences.AvailableColumnForAsset;
					break;

				case (int)PNLColumns.TradingAccount:
					availableColumnList = _PNLPrefrences.AvailableColumnForTradingAccount;
					break;

				case (int)PNLColumns.User:
					availableColumnList = _PNLPrefrences.AvailableColumnForUser;
					break;

				case (int)PNLColumns.Client:
					availableColumnList = _PNLPrefrences.AvailableColumnForClient;
					break;

				case (int)PNLColumns.Side:
					availableColumnList = _PNLPrefrences.AvailableColumnForSide;
					break;
			}
			
			int columnCount = availableColumnList.Count;
			
			this.lbAvailableColumnsL2.DataSource = null;

			this.lbAvailableColumnsL2.Items.Clear();

			for(int i=0;i<columnCount;i++)
			{
				string columnName = PNLPrefrencesData.PNLColumnNames[(int)availableColumnList[i]];
				this.lbAvailableColumnsL2.Items.Add(columnName);
			}

			//remove the columns from display list of l2
			ArrayList displayListL2 = null;

			switch(pnlTabType)
			{
				case PNLTab.Asset:
					displayListL2 = this.displayListAssetL2;
					break;

				case PNLTab.Symbol:
					displayListL2 = this.displayListSymbolL2;
					break;

				case PNLTab.TradingAccount:
					displayListL2 = this.displayListTradingAccountL2;
					break;
			}
			
			int displayColumnCount = displayListL2.Count;

			ArrayList ColumnList = new ArrayList();
			
			for(int i=0;i<displayColumnCount;i++)
			{
				bool columnFound = false;
				string columnName = PNLPrefrencesData.PNLColumnNames[(int)displayListL2[i]];

				for(int j=0;j<columnCount;j++)
				{
					string columnDisplayName = PNLPrefrencesData.PNLColumnNames[(int)availableColumnList[j]];

					if(columnDisplayName.Equals(columnName))
					{
						columnFound = true;
						break;
					}
				}

				if(columnFound==false)
				{
					ColumnList.Add(columnName);
					//displayListL2.Remove(columnDisplayName);
				}
			}

			//Update the display list listbox
			PopulateDisplayColumnLevel2List(pnlTabType);

			for(int j=0;j<ColumnList.Count;j++)
			{
				if(this.lbDisplayColumnsL2.Items.Contains((string)ColumnList[j]))
					this.lbDisplayColumnsL2.Items.Remove((string)ColumnList[j]);
			}
		}

		private void PopulateDisplayColumnLevel1List(PNLTab pnlTabType)
		{
			ArrayList displayListL1 = null;
			
			switch(pnlTabType)
			{
				case PNLTab.Asset:
						displayListL1 = this.displayListAssetL1;
				break;

				case PNLTab.Symbol:
						displayListL1 = this.displayListSymbolL1;
				break;

				case PNLTab.TradingAccount:
						displayListL1 = this.displayListTradingAccountL1;
				break;
			}
			
			int columnCountL1 = displayListL1.Count;

			this.lbDisplayColumnsL1.DataSource = null;

			this.lbDisplayColumnsL1.Items.Clear();
			
			//Add the column in the Display list L1 and remove from the available column list
			for(int i=0;i<columnCountL1;i++)
			{
				string columnName = PNLPrefrencesData.PNLColumnNames[(int)displayListL1[i]];
				this.lbDisplayColumnsL1.Items.Add(columnName);

				if(this.lbAvailableColumnsL1.Items.Contains(columnName))
					this.lbAvailableColumnsL1.Items.Remove(columnName);
			}

			this.RefreshLevel1SortCombo();
		}


		private void PopulateDisplayColumnLevel2List(PNLTab pnlTabType)
		{
			ArrayList displayListL2 = null;
			
			switch(pnlTabType)
			{
				case PNLTab.Asset:
					displayListL2 = this.displayListAssetL2;
					break;

				case PNLTab.Symbol:
					displayListL2 = this.displayListSymbolL2;
					break;

				case PNLTab.TradingAccount:
					displayListL2 = this.displayListTradingAccountL2;
					break;
			}
			
			int columnCountL2 = displayListL2.Count;

			this.lbDisplayColumnsL2.DataSource = null;

			this.lbDisplayColumnsL2.Items.Clear();

			//Add the column in the Display list L1 and remove from the available column list
			for(int i=0;i<columnCountL2;i++)
			{
				string columnName = PNLPrefrencesData.PNLColumnNames[(int)displayListL2[i]];
				this.lbDisplayColumnsL2.Items.Add(columnName);

				if(this.lbAvailableColumnsL2.Items.Contains(columnName))
					this.lbAvailableColumnsL2.Items.Remove(columnName);
			}

			this.RefreshLevel2SortCombo();
		}

		#endregion

		#region Set data

		private void SetPreferencesData()
		{
			//General proerties
			this.clrPckBackground.Color  = PNLPrefrencesData.GetColorFromARGB(_PNLPrefrences.BackgroundColor);
			UpdateColorPickerControl(this.clrPckBackground);
			this.clrPckDefault.Color  = PNLPrefrencesData.GetColorFromARGB(_PNLPrefrences.DefaulTextColor);
			UpdateColorPickerControl(this.clrPckDefault);
			this.clrPckHeaderLevel2.Color = PNLPrefrencesData.GetColorFromARGB(_PNLPrefrences.HeaderColorL2);
			UpdateColorPickerControl(this.clrPckHeaderLevel2);
			this.clrPckHeaderLevel1.Color = PNLPrefrencesData.GetColorFromARGB(_PNLPrefrences.HeaderColorL1);
			UpdateColorPickerControl(this.clrPckHeaderLevel1);
			this.clrPckSummaryBG.Color  = PNLPrefrencesData.GetColorFromARGB(_PNLPrefrences.SummaryBackgroundColor);
			UpdateColorPickerControl(this.clrPckSummaryBG);
			this.clrPckSummaryText.Color  = PNLPrefrencesData.GetColorFromARGB(_PNLPrefrences.SummaryTextColor);
			UpdateColorPickerControl(this.clrPckSummaryText);

			//Exposure and PNL related properties
			this.clrPckLP.Color  = PNLPrefrencesData.GetColorFromARGB(_PNLPrefrences.LongPNLTextColor);
			UpdateColorPickerControl(this.clrPckLP);
			this.clrPckSP.Color  = PNLPrefrencesData.GetColorFromARGB(_PNLPrefrences.ShortPNLTextColor);
			UpdateColorPickerControl(this.clrPckSP);
			this.clrPckNP.Color  = PNLPrefrencesData.GetColorFromARGB(_PNLPrefrences.NetPNLTextColor);
			UpdateColorPickerControl(this.clrPckNP);
			this.clrPckLE.Color  = PNLPrefrencesData.GetColorFromARGB(_PNLPrefrences.LongExposureTextColor);
			UpdateColorPickerControl(this.clrPckLE);
			this.clrPckSE.Color  = PNLPrefrencesData.GetColorFromARGB(_PNLPrefrences.ShortExposureTextColor);
			UpdateColorPickerControl(this.clrPckSE);
			this.clrPckNE.Color  = PNLPrefrencesData.GetColorFromARGB(_PNLPrefrences.NetExposureTextColor);
			UpdateColorPickerControl(this.clrPckNE);

			//Grid rows related properties
			this.clrPckLevel1Row.Color  = PNLPrefrencesData.GetColorFromARGB(_PNLPrefrences.RowColorL1);
			UpdateColorPickerControl(this.clrPckLevel1Row);
			this.clrPckLevel1AltRow.Color  = PNLPrefrencesData.GetColorFromARGB(_PNLPrefrences.RowalternateColorL1);
			UpdateColorPickerControl(this.clrPckLevel1AltRow);
			this.clrPckLevel2Row.Color  = PNLPrefrencesData.GetColorFromARGB(_PNLPrefrences.RowColorL2);
			UpdateColorPickerControl(this.clrPckLevel2Row);
			this.clrPckLevel2AltRow.Color  = PNLPrefrencesData.GetColorFromARGB(_PNLPrefrences.RowalternateColorL2);
			UpdateColorPickerControl(this.clrPckLevel2AltRow);
			
			//Chart related properties
			this.clrPckChartBackground.Color = PNLPrefrencesData.GetColorFromARGB(_PNLPrefrences.ChartBackgroundColor);
			UpdateColorPickerControl(this.clrPckChartBackground);
			this.clrPckChartBorder.Color = PNLPrefrencesData.GetColorFromARGB(_PNLPrefrences.ChartBorderColor);
			UpdateColorPickerControl(this.clrPckChartBorder);
			this.clrPckChartText.Color = PNLPrefrencesData.GetColorFromARGB(_PNLPrefrences.ChartTextColor);
			UpdateColorPickerControl(this.clrPckChartText);
			this.clrPckLegandBG.Color = PNLPrefrencesData.GetColorFromARGB(_PNLPrefrences.LegendBackgroundColor);
			UpdateColorPickerControl(this.clrPckLegandBG);
			this.clrPckLegandBorder.Color = PNLPrefrencesData.GetColorFromARGB(_PNLPrefrences.LegendBorderColor);
			UpdateColorPickerControl(this.clrPckLegandBorder);
			this.clrPckLegandText.Color = PNLPrefrencesData.GetColorFromARGB(_PNLPrefrences.LegendTextColor);
			UpdateColorPickerControl(this.clrPckLegandText);
			this.chkLegandVisible.Checked = _PNLPrefrences.ShowLegend;
			
			//Calculation related properties
			this.cmbLEFeedColumn.SelectedIndex =  _PNLPrefrences.LongExposoreCalculcationColumn;  
			this.cmbSEFeedColumn.SelectedIndex =  _PNLPrefrences.ShortExposureCalculationColumn;  
			this.cmbLPFeedColumn.SelectedIndex =  _PNLPrefrences.LongPNLCalculationColumn;   
			this.cmbSPFeedColumn.SelectedIndex =  _PNLPrefrences.ShortPNLCalculcationColumn;  
			this.cmbDefaultFeedColumn.SelectedIndex = _PNLPrefrences.DefaultCalculcationColumn;  
							
			//Refresh rate
			this.numericUpDownRefreshrate.Value = _PNLPrefrences.RefreshRate;
			
			//select default tab type
			this.lbTabs.SelectedIndex = 0;
					
			//Set the default for sorting combos Level1
			for(int i=0;i<this.ddlSortColumnL1.Items.Count;i++)
			{
				if(Convert.ToInt32(this.ddlSortColumnL1.Items[i].DataValue) == _PNLPrefrences.SortKeyAssetL1)
				{
					this.ddlSortColumnL1.SelectedIndex = i;
					break;
				}
			}
			
			if(_PNLPrefrences.AscendingAssetL1)
				this.rbAscL1.Checked = true;
			else
				this.rbDescL1.Checked = true;

			//Set the default for sorting combos Level2
			for(int i=0;i<this.ddlSortColumnL2.Items.Count;i++)
			{
				if(Convert.ToInt32(this.ddlSortColumnL2.Items[i].DataValue) == _PNLPrefrences.SortKeyAssetL2)
				{
					this.ddlSortColumnL2.SelectedIndex = i;
					break;
				}
			}

			if(_PNLPrefrences.AscendingAssetL2)
				this.rbAscL2.Checked = true;
			else
				this.rbDescL2.Checked = true;

		}


		private void StoreColumnAssetData()
		{
			int displayColumnL1Count;
			int displayColumnL2Count;

			this.level1AssetColumn = (int)this.cmbLevel1Column.SelectedItem.DataValue;
			this.level2AssetColumn = (int)this.cmbLevel2Column.SelectedItem.DataValue;

			if(this.ddlSortColumnL1.SelectedItem != null)
				this.sortKeyAssetL1  = (int)this.ddlSortColumnL1.SelectedItem.DataValue;
			this.ascendingAssetL1 = this.rbAscL1.Checked;

			if(this.ddlSortColumnL2.SelectedItem != null)
				this.sortKeyAssetL2  = (int)this.ddlSortColumnL2.SelectedItem.DataValue;
			this.ascendingAssetL2 = this.rbAscL2.Checked;

			displayColumnL1Count = this.lbDisplayColumnsL1.Items.Count;

			this.displayListAssetL1.Clear();

			for(int i=0;i<displayColumnL1Count;i++)
			{
				string columnName = (string)this.lbDisplayColumnsL1.Items[i];
				int columnIndex = PNLPrefrencesData.GetColumnIndex(columnName);
				this.displayListAssetL1.Add((PNLColumns)columnIndex);
			}

			displayColumnL2Count = this.lbDisplayColumnsL2.Items.Count;

			this.displayListAssetL2.Clear();

			for(int i=0;i<displayColumnL2Count;i++)
			{
				string columnName = (string)this.lbDisplayColumnsL2.Items[i];
				int columnIndex = PNLPrefrencesData.GetColumnIndex(columnName);
				this.displayListAssetL2.Add((PNLColumns)columnIndex);
			}
		}

		private void StoreColumnSymbolData()
		{
			int displayColumnL1Count;
			int displayColumnL2Count;

			this.level1SymbolColumn = (int)this.cmbLevel1Column.SelectedItem.DataValue;
			this.level2SymbolColumn = (int)this.cmbLevel2Column.SelectedItem.DataValue;

			if(this.ddlSortColumnL1.SelectedItem != null)
				this.sortKeySymbolL1  = (int)this.ddlSortColumnL1.SelectedItem.DataValue;
			this.ascendingSymbolL1 = this.rbAscL1.Checked;

			if(this.ddlSortColumnL2.SelectedItem != null)
				this.sortKeySymbolL2  = (int)this.ddlSortColumnL2.SelectedItem.DataValue;
			this.ascendingSymbolL2 = this.rbAscL2.Checked;

			displayColumnL1Count = this.lbDisplayColumnsL1.Items.Count;

			this.displayListSymbolL1.Clear();

			for(int i=0;i<displayColumnL1Count;i++)
			{
				string columnName = (string)this.lbDisplayColumnsL1.Items[i];
				int columnIndex = PNLPrefrencesData.GetColumnIndex(columnName);
				this.displayListSymbolL1.Add((PNLColumns)columnIndex);
			}

			displayColumnL2Count = this.lbDisplayColumnsL2.Items.Count;

			this.displayListSymbolL2.Clear();

			for(int i=0;i<displayColumnL2Count;i++)
			{
				string columnName = (string)this.lbDisplayColumnsL2.Items[i];
				int columnIndex = PNLPrefrencesData.GetColumnIndex(columnName);
				this.displayListSymbolL2.Add((PNLColumns)columnIndex);
			}
		}

		private void StoreColumnTradingAccountData()
		{
			int displayColumnL1Count;
			int displayColumnL2Count;

			this.level1TradingAccountColumn = (int)this.cmbLevel1Column.SelectedItem.DataValue;
			this.level2TradingAccountColumn = (int)this.cmbLevel2Column.SelectedItem.DataValue;
					
			if(this.ddlSortColumnL1.SelectedItem != null)
				this.sortKeyTradingAccountL1  = (int)this.ddlSortColumnL1.SelectedItem.DataValue;
			this.ascendingTradingAccountL1 = this.rbAscL1.Checked;
					
			if(this.ddlSortColumnL2.SelectedItem != null)
				this.sortKeyTradingAccountL2  = (int)this.ddlSortColumnL2.SelectedItem.DataValue;
			this.ascendingTradingAccountL2 = this.rbAscL2.Checked;
					
			displayColumnL1Count = this.lbDisplayColumnsL1.Items.Count;
					
			this.displayListTradingAccountL1.Clear();
					
			for(int i=0;i<displayColumnL1Count;i++)
			{
				string columnName = (string)this.lbDisplayColumnsL1.Items[i];
				int columnIndex = PNLPrefrencesData.GetColumnIndex(columnName);
				this.displayListTradingAccountL1.Add((PNLColumns)columnIndex);
			}
					
			displayColumnL2Count = this.lbDisplayColumnsL2.Items.Count;
					
			this.displayListTradingAccountL2.Clear();

			for(int i=0;i<displayColumnL2Count;i++)
			{
				string columnName = (string)this.lbDisplayColumnsL2.Items[i];
				int columnIndex = PNLPrefrencesData.GetColumnIndex(columnName);
				this.displayListTradingAccountL2.Add((PNLColumns)columnIndex);
			}		
		}


		private void SetGroupByLevelColumn()
		{
			int level1Column = int.MinValue;
			int level2Column = int.MinValue;

			switch(this.lbTabs.SelectedIndex)
			{
				case 0:
					level1Column = _PNLPrefrences.Level1AssetColumn;
					level2Column = _PNLPrefrences.Level2AssetColumn;
					break;

				case 1:
					level1Column = _PNLPrefrences.Level1SymbolColumn;
					level2Column = _PNLPrefrences.Level2SymbolColumn;
					break;

				case 2:
					level1Column = _PNLPrefrences.Level1TradingAccountColumn;
					level2Column = _PNLPrefrences.Level2TradingAccountColumn;
					break;
			}

			int level1ColumnCount = this.cmbLevel1Column.Items.Count;

			for(int i=0;i<level1ColumnCount;i++)
			{
				if(Convert.ToInt32(this.cmbLevel1Column.Items[i].DataValue) ==level1Column)
				{
					this.cmbLevel1Column.SelectedIndex = i;
					break;
				}
			}

			int level2ColumnCount = this.cmbLevel2Column.Items.Count;

			for(int i=0;i<level2ColumnCount;i++)
			{
				if(Convert.ToInt32(this.cmbLevel2Column.Items[i].DataValue) == level2Column)
				{
					this.cmbLevel2Column.SelectedIndex = i;
					break;
				}
			}
		}

		private bool ValidatePreferences()
		{
			//Group By Column
			if(this.level1AssetColumn == this.level2AssetColumn
			  || this.level1SymbolColumn == this.level2SymbolColumn
			  || this.level1TradingAccountColumn == this.level2TradingAccountColumn
				)
			{
				MessageBox.Show("Level 1 and Level 2 column cannot be same", "Validation Error Message", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
				return false;
			}

			if(this.displayListAssetL1.Count == 0 || this.displayListAssetL2.Count == 0)
			{

				MessageBox.Show("Display column list for Asset tab can not be empty", "Validation Error Message", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
				return false;
			}

			if(this.displayListSymbolL1.Count == 0 || this.displayListSymbolL2.Count == 0)
			{

				MessageBox.Show("Display column list for Symbol tab can not be empty", "Validation Error Message", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
				return false;
			}
			
			if(this.displayListTradingAccountL1.Count == 0 || this.displayListTradingAccountL2.Count == 0)
			{

				MessageBox.Show("Display column list for Trading Account tab can not be empty", "Validation Error Message", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
				return false;
			}

			if(this.ddlSortColumnL1.SelectedIndex < 0)
			{
				MessageBox.Show("Please select the sort column for Level1", "Validation Error Message", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
				return false;
			}

			if(this.ddlSortColumnL2.SelectedIndex < 0)
			{
				MessageBox.Show("Please select the sort column for Level2", "Validation Error Message", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
				return false;

			}

			
			return true;
		}
		
		private void StorePreferencesData()
		{
			//General proerties
			_PNLPrefrences.BackgroundColor = PNLPrefrencesData.GetARGBFromColor(this.clrPckBackground.Color);
			_PNLPrefrences.DefaulTextColor = PNLPrefrencesData.GetARGBFromColor(this.clrPckDefault.Color);
			_PNLPrefrences.HeaderColorL2 = PNLPrefrencesData.GetARGBFromColor(this.clrPckHeaderLevel2.Color);
			_PNLPrefrences.HeaderColorL1 = PNLPrefrencesData.GetARGBFromColor(this.clrPckHeaderLevel1.Color);
			_PNLPrefrences.SummaryBackgroundColor = PNLPrefrencesData.GetARGBFromColor(this.clrPckSummaryBG.Color);
			_PNLPrefrences.SummaryTextColor = PNLPrefrencesData.GetARGBFromColor(this.clrPckSummaryText.Color);
				
			//Exposure and PNL related properties
			_PNLPrefrences.LongPNLTextColor = PNLPrefrencesData.GetARGBFromColor(this.clrPckLP.Color);
			_PNLPrefrences.ShortPNLTextColor = PNLPrefrencesData.GetARGBFromColor(this.clrPckSP.Color);
			_PNLPrefrences.NetPNLTextColor = PNLPrefrencesData.GetARGBFromColor(this.clrPckNP.Color);
			_PNLPrefrences.LongExposureTextColor = PNLPrefrencesData.GetARGBFromColor(this.clrPckLE.Color);
			_PNLPrefrences.ShortExposureTextColor = PNLPrefrencesData.GetARGBFromColor(this.clrPckSE.Color);
			_PNLPrefrences.NetExposureTextColor = PNLPrefrencesData.GetARGBFromColor(this.clrPckNE.Color);
				
			//Grid rows related properties
			_PNLPrefrences.RowColorL1 = PNLPrefrencesData.GetARGBFromColor(this.clrPckLevel1Row.Color);
			_PNLPrefrences.RowalternateColorL1 = PNLPrefrencesData.GetARGBFromColor(this.clrPckLevel1AltRow.Color);
			_PNLPrefrences.RowColorL2 = PNLPrefrencesData.GetARGBFromColor(this.clrPckLevel2Row.Color);
			_PNLPrefrences.RowalternateColorL2 = PNLPrefrencesData.GetARGBFromColor(this.clrPckLevel2AltRow.Color);
				
			//Chart related properties
			_PNLPrefrences.ChartBackgroundColor = PNLPrefrencesData.GetARGBFromColor(this.clrPckChartBackground.Color);
			_PNLPrefrences.ChartBorderColor = PNLPrefrencesData.GetARGBFromColor(this.clrPckChartBorder.Color);
			_PNLPrefrences.ChartTextColor = PNLPrefrencesData.GetARGBFromColor(this.clrPckChartText.Color);
			_PNLPrefrences.LegendBackgroundColor = PNLPrefrencesData.GetARGBFromColor(this.clrPckLegandBG.Color);
			_PNLPrefrences.LegendBorderColor = PNLPrefrencesData.GetARGBFromColor(this.clrPckLegandBorder.Color);
			_PNLPrefrences.LegendTextColor = PNLPrefrencesData.GetARGBFromColor(this.clrPckLegandText.Color);
			_PNLPrefrences.ShowLegend = this.chkLegandVisible.Checked;

				
			//Calculation related properties
			_PNLPrefrences.LongExposoreCalculcationColumn = this.cmbLEFeedColumn.SelectedIndex;  
			_PNLPrefrences.ShortExposureCalculationColumn = this.cmbSEFeedColumn.SelectedIndex;  
			_PNLPrefrences.LongPNLCalculationColumn = this.cmbLPFeedColumn.SelectedIndex;   
			_PNLPrefrences.ShortPNLCalculcationColumn = this.cmbSPFeedColumn.SelectedIndex;  
			_PNLPrefrences.DefaultCalculcationColumn = this.cmbDefaultFeedColumn.SelectedIndex;  
			
			//Display list related
			_PNLPrefrences.DisplayListAssetL1 = this.displayListAssetL1;
			_PNLPrefrences.DisplayListAssetL2 = this.displayListAssetL2;
			_PNLPrefrences.DisplayListSymbolL1 = this.displayListSymbolL1;
			_PNLPrefrences.DisplayListSymbolL2 = this.displayListSymbolL2;
			_PNLPrefrences.DisplayListTradingAccountL1 = this.displayListTradingAccountL1;
			_PNLPrefrences.DisplayListTradingAccountL2 = this.displayListTradingAccountL2;

			//Sorting column
			_PNLPrefrences.SortKeyAssetL1 = this.sortKeyAssetL1;
			_PNLPrefrences.SortKeyAssetL2 = this.sortKeyAssetL2;
			_PNLPrefrences.SortKeySymbolL1 = this.sortKeySymbolL1;
			_PNLPrefrences.SortKeySymbolL2 = this.sortKeySymbolL2;
			_PNLPrefrences.SortKeyTradingAccountL1 = this.sortKeyTradingAccountL1;
			_PNLPrefrences.SortKeyTradingAccountL2 = this.sortKeyTradingAccountL2;

			//Sorting order
			_PNLPrefrences.AscendingAssetL1 = this.ascendingAssetL1;
			_PNLPrefrences.AscendingAssetL2 = this.ascendingAssetL2;
			_PNLPrefrences.AscendingSymbolL1 = this.ascendingSymbolL1;
			_PNLPrefrences.AscendingSymbolL2 = this.ascendingSymbolL2;
			_PNLPrefrences.AscendingTradingAccountL1 = this.ascendingTradingAccountL1;
			_PNLPrefrences.AscendingTradingAccountL2 = this.ascendingTradingAccountL2;
			
			//Group Column
			_PNLPrefrences.Level1AssetColumn = this.level1AssetColumn;
			_PNLPrefrences.Level2AssetColumn = this.level2AssetColumn;
			_PNLPrefrences.Level1SymbolColumn = this.level1SymbolColumn;
			_PNLPrefrences.Level2SymbolColumn = this.level2SymbolColumn;
			_PNLPrefrences.Level1TradingAccountColumn = this.level1TradingAccountColumn;
			_PNLPrefrences.Level2TradingAccountColumn = this.level2TradingAccountColumn;

			//Refresh rate
			_PNLPrefrences.RefreshRate = (int)this.numericUpDownRefreshrate.Value;
		}

		public void SetDefault()
		{
			//Restore the default settings
			_PNLPrefrences.SetDefaultPreferences();
			
			SetPreferencesData();
		}


		#endregion

		#region Update Column data

		private void UpdateLevel1Panel()
		{
			PNLTab pnlTab = new PNLTab();
			int level1SortColumn = int.MinValue;
			bool level1OrderType = false;

			switch(this.lbTabs.SelectedIndex)
			{
				case 0:
					pnlTab = PNLTab.Asset;

					level1SortColumn = this.sortKeyAssetL1;
			
					level1OrderType = this.ascendingAssetL1;

					break;

				case 1:
					pnlTab = PNLTab.Symbol;

					level1SortColumn = this.sortKeySymbolL1;
					
					level1OrderType = this.ascendingSymbolL1;

					break;

				case 2:
					pnlTab = PNLTab.TradingAccount;
					
					level1SortColumn = this.sortKeyTradingAccountL1;

					level1OrderType = this.ascendingTradingAccountL1;

					break;
			}

			this.PopulateAvailableLevel1ColumnList(pnlTab);
			
			this.RefreshLevel1SortCombo();

			for(int i=0;i<this.ddlSortColumnL1.Items.Count;i++)
			{
				if(Convert.ToInt32(this.ddlSortColumnL1.Items[i].DataValue) == level1SortColumn)
				{
					this.ddlSortColumnL1.SelectedIndex = i;
					break;
				}
			}

			if(level1OrderType)
				this.rbAscL1.Checked = true;
			else
				this.rbDescL1.Checked = true;

		}


		private void UpdateLevel2Panel()
		{
			PNLTab pnlTab = new PNLTab();
			int level2SortColumn = int.MinValue;
			bool level2OrderType = false;

			switch(this.lbTabs.SelectedIndex)
			{
				case 0:
					pnlTab = PNLTab.Asset;

					level2SortColumn = this.sortKeyAssetL2;
			
					level2OrderType = this.ascendingAssetL2;

					break;

				case 1:
					pnlTab = PNLTab.Symbol;

					level2SortColumn = this.sortKeySymbolL2;
					
					level2OrderType = this.ascendingSymbolL2;

					break;

				case 2:
					pnlTab = PNLTab.TradingAccount;
					
					level2SortColumn = this.sortKeyTradingAccountL2;

					level2OrderType = this.ascendingTradingAccountL2;

					break;
			}

			this.PopulateAvailableLevel2ColumnList(pnlTab);
			
			this.RefreshLevel2SortCombo();

			for(int i=0;i<this.ddlSortColumnL2.Items.Count;i++)
			{
				if(Convert.ToInt32(this.ddlSortColumnL2.Items[i].DataValue) == level2SortColumn)
				{
					this.ddlSortColumnL2.SelectedIndex = i;
					break;
				}
			}

			if(level2OrderType)
				this.rbAscL2.Checked = true;
			else
				this.rbDescL2.Checked = true;
		}


		#endregion

		#region Update Sort Dropdowns

		/// <summary>
		/// This will just recheck the available column listbox and rebind the sort combo again.
		/// </summary>
		private void RefreshLevel1SortCombo()
		{
			try
			{
				int selectedIndex = -1;

				if(this.ddlSortColumnL1.SelectedItem != null)
					selectedIndex = (int) this.ddlSortColumnL1.SelectedItem.DataValue;

				this.ddlSortColumnL1.Items.Clear();			
				int index = 0;
				Infragistics.Win.ValueListItem item;

				PNLColumns pnlColumn = new PNLColumns();
				Type t = typeof(PNLColumns); 

				Array columns = Enum.GetValues(pnlColumn.GetType());
				ArrayList arrColumn = new ArrayList(columns);

				foreach( object obj in this.lbDisplayColumnsL1.Items)
				{
					item = new Infragistics.Win.ValueListItem();
					item.DisplayText = obj.ToString();
					item.DataValue   = PNLPrefrencesData.GetColumnIndex(obj.ToString()); // obj.ToString();
					this.ddlSortColumnL1.Items.Add(item);	

					if(Convert.ToInt32(item.DataValue) == selectedIndex)
					{
						this.ddlSortColumnL1.SelectedIndex = index;
					}

					index++;
				}

				if(this.cmbLevel1Column.SelectedItem != null)
				{
					item = new Infragistics.Win.ValueListItem();
					item.DisplayText = this.cmbLevel1Column.SelectedItem.DisplayText;
					item.DataValue   = this.cmbLevel1Column.SelectedItem.DataValue;
					this.ddlSortColumnL1.Items.Add(item);	

					if(Convert.ToInt32(item.DataValue) == selectedIndex)
					{
						this.ddlSortColumnL1.SelectedIndex = index;
					}

				}

			}
			catch(Exception ex)
			{
				string formattedInfo = ex.Message + "\n Stack Trace:" + ex.StackTrace.ToString() + "\n Inner Exception:"  + ex.InnerException;
				Logger.Write(formattedInfo, Common.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, 
					FORM_NAME);
				AppMessageExceptionHandler appMessageExceptionHandler = new AppMessageExceptionHandler(null);
				appMessageExceptionHandler.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), Common.POLICY_LOGANDSHOW, System.Guid.NewGuid());
			}
		}


		/// <summary>
		/// This will just recheck the available column listbox and rebind the sort combo again.
		/// </summary>
		private void RefreshLevel2SortCombo()
		{
			try
			{
				int selectedIndex = -1;

				if(this.ddlSortColumnL2.SelectedItem != null)
					selectedIndex = (int) this.ddlSortColumnL2.SelectedItem.DataValue;

				this.ddlSortColumnL2.Items.Clear();			
				int index = 0;
				Infragistics.Win.ValueListItem item;
				
				foreach( object obj in this.lbDisplayColumnsL2.Items)
				{
					item = new Infragistics.Win.ValueListItem();
					item.DisplayText = obj.ToString();
					item.DataValue   = PNLPrefrencesData.GetColumnIndex(obj.ToString()); // obj.ToString();
					this.ddlSortColumnL2.Items.Add(item);

					if(Convert.ToInt32(item.DataValue) == selectedIndex)
					{
						this.ddlSortColumnL2.SelectedIndex = index;
					}
	
					index++;
				}
				
				if(this.cmbLevel2Column.SelectedItem != null)
				{
					item = new Infragistics.Win.ValueListItem();
					item.DisplayText = this.cmbLevel2Column.SelectedItem.DisplayText;
					item.DataValue   = this.cmbLevel2Column.SelectedItem.DataValue;
					this.ddlSortColumnL2.Items.Add(item);	

					if(Convert.ToInt32(item.DataValue) == selectedIndex)
					{
						this.ddlSortColumnL2.SelectedIndex = index;
					}
				}

			}
			catch(Exception ex)
			{
				string formattedInfo = ex.Message + "\n Stack Trace:" + ex.StackTrace.ToString() + "\n Inner Exception:"  + ex.InnerException;
				Logger.Write(formattedInfo, Common.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, 
					FORM_NAME);
				AppMessageExceptionHandler appMessageExceptionHandler = new AppMessageExceptionHandler(null);
				appMessageExceptionHandler.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), Common.POLICY_LOGANDSHOW, System.Guid.NewGuid());
			}
		}
		#endregion

		#region Save Preferences
		
		public bool Save()
		{
			try
			{
				//Save the previous data
				switch(currentTab)
				{
					case 0:
						StoreColumnAssetData();
						break;

					case 1:
						StoreColumnSymbolData();
						break;

					case 2:
						StoreColumnTradingAccountData();
						break;
				}

				if(ValidatePreferences())
				{				
				
					this.StorePreferencesData();

					//save in db or xml
					this._PNLPreferencesManager.SetPreferences(this._PNLPrefrences);
				}
				else
					return false;

			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message.ToString());
			}

			return true;
		}

        // we have more than one tab in the Livefeed preferences, so need to select a particular Tab from a particular module
        // so declared a property in the IPreference interface
        private string _modulename = string.Empty;
        public string SetModuleActive
        {
            set
            {
                _modulename = value;

            }
        }

		#endregion

		#region Control Event Handlers

		private void btnMoveRightL2_Click(object sender, System.EventArgs e)
		{
			try
			{
				MoveItem(lbAvailableColumnsL2,lbDisplayColumnsL2);
				RefreshLevel2SortCombo();	
			}
			catch(Exception ex)
			{
				#region Catch
				string formattedInfo = ex.Message + "\n Stack Trace:" + ex.StackTrace.ToString() + "\n Inner Exception:"  + ex.InnerException;
				Logger.Write(formattedInfo, Common.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, 
					FORM_NAME);
				AppMessageExceptionHandler appMessageExceptionHandler = new AppMessageExceptionHandler(null);
				appMessageExceptionHandler.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), Common.POLICY_LOGANDSHOW, System.Guid.NewGuid());
				#endregion
			}
			finally
			{
				#region LogEntry

				LogEntry logEntry = new LogEntry("btnMoveRight_Click", 
					Common.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                    FORM_NAME + "btnMoveRight_Click", null); 
				Logger.Write(logEntry); 

				#endregion
			}		
		}
		
		private void btnMoveAllRightL2_Click(object sender, System.EventArgs e)
		{
			MoveAllItemsRight(lbAvailableColumnsL2,lbDisplayColumnsL2);
			RefreshLevel2SortCombo();		
		}

		private void btnMoveAllLeftL2_Click(object sender, System.EventArgs e)
		{
			try
			{
				if (lbDisplayColumnsL2.Items.Count > 1)
				{
					MoveAllItemsLeft(lbDisplayColumnsL2,lbAvailableColumnsL2); 
					RefreshLevel2SortCombo();	
				}
				else
				{
					MessageBox.Show("Atleast one Column should be displayed!!");
				
				}
			}
			catch(Exception ex)
			{
				string formattedInfo = ex.Message + "\n Stack Trace:" + ex.StackTrace.ToString() + "\n Inner Exception:"  + ex.InnerException;
				Logger.Write(formattedInfo, Common.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, 
					FORM_NAME);
				AppMessageExceptionHandler appMessageExceptionHandler = new AppMessageExceptionHandler(null);
				appMessageExceptionHandler.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), Common.POLICY_LOGANDSHOW, System.Guid.NewGuid());
			}
			finally
			{
				#region LogEntry

				LogEntry logEntry = new LogEntry("btnMoveLeft_Click", 
					Common.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                    FORM_NAME + "btnMoveLeft_Click", null);  
				Logger.Write(logEntry); 

				#endregion
			}		
		}

		private void btnMoveLeftL2_Click(object sender, System.EventArgs e)
		{
			try
			{
				MoveItem(lbDisplayColumnsL2,lbAvailableColumnsL2);
				RefreshLevel2SortCombo();	
			}
			catch(Exception ex)
			{
				#region Catch
				string formattedInfo = ex.Message + "\n Stack Trace:" + ex.StackTrace.ToString() + "\n Inner Exception:"  + ex.InnerException;
				Logger.Write(formattedInfo, Common.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, 
					FORM_NAME);
				AppMessageExceptionHandler appMessageExceptionHandler = new AppMessageExceptionHandler(null);
				appMessageExceptionHandler.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), Common.POLICY_LOGANDSHOW, System.Guid.NewGuid());
				#endregion
			}
			finally
			{
				#region LogEntry

				LogEntry logEntry = new LogEntry("btnMoveAllLeft_Click", 
					Common.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                    FORM_NAME + "btnMoveAllLeft_Click", null); 
				Logger.Write(logEntry); 

				#endregion
			}		
		}

		private void btnUpL2_Click(object sender, System.EventArgs e)
		{
			try
			{
				MoveUpDown(lbDisplayColumnsL2,true);
			}
			catch(Exception ex)
			{
				string formattedInfo = ex.Message + "\n Stack Trace:" + ex.StackTrace.ToString() + "\n Inner Exception:"  + ex.InnerException;
				Logger.Write(formattedInfo, Common.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, 
					FORM_NAME);
				AppMessageExceptionHandler appMessageExceptionHandler = new AppMessageExceptionHandler(null);
				appMessageExceptionHandler.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), Common.POLICY_LOGANDSHOW, System.Guid.NewGuid());
			}
			finally
			{
				#region LogEntry

				LogEntry logEntry = new LogEntry("btnUp_Click", 
					Common.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                    FORM_NAME + "btnUp_Click", null); 
				Logger.Write(logEntry); 

				#endregion
			}		
		}

		private void btnDownL2_Click(object sender, System.EventArgs e)
		{
			try
			{
				MoveUpDown(lbDisplayColumnsL2,false);
			}
			catch(Exception ex)
			{
				string formattedInfo = ex.Message + "\n Stack Trace:" + ex.StackTrace.ToString() + "\n Inner Exception:"  + ex.InnerException;
				Logger.Write(formattedInfo, Common.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, 
					FORM_NAME);
				AppMessageExceptionHandler appMessageExceptionHandler = new AppMessageExceptionHandler(null);
				appMessageExceptionHandler.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), Common.POLICY_LOGANDSHOW, System.Guid.NewGuid());
			}
			finally
			{
				#region LogEntry

				LogEntry logEntry = new LogEntry("btnDown_Click", 
					Common.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                    FORM_NAME + "btnDown_Click", null);  
				Logger.Write(logEntry); 

				#endregion
			}		
		}

		private void cmbLevel2Column_SelectionChanged(object sender, System.EventArgs e)
		{
			UpdateLevel2Panel();
		}

		private void ddlSortColumnL2_SelectionChanged(object sender, System.EventArgs e)
		{
			
		}

		private void cmbLevel1Column_SelectionChanged(object sender, System.EventArgs e)
		{
			UpdateLevel1Panel();
		}


		private void ddlSortColumnL1_SelectionChanged(object sender, System.EventArgs e)
		{
		
		}

		private void btnMoveRightL1_Click(object sender, System.EventArgs e)
		{
			try
			{
				MoveItem(lbAvailableColumnsL1,lbDisplayColumnsL1);
				RefreshLevel1SortCombo();	
			}
			catch(Exception ex)
			{
				#region Catch
				string formattedInfo = ex.Message + "\n Stack Trace:" + ex.StackTrace.ToString() + "\n Inner Exception:"  + ex.InnerException;
				Logger.Write(formattedInfo, Common.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, 
					FORM_NAME);
				AppMessageExceptionHandler appMessageExceptionHandler = new AppMessageExceptionHandler(null);
				appMessageExceptionHandler.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), Common.POLICY_LOGANDSHOW, System.Guid.NewGuid());
				#endregion
			}
			finally
			{
				#region LogEntry

				LogEntry logEntry = new LogEntry("btnMoveRight_Click", 
					Common.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
					FORM_NAME + "btnMoveRight_Click",null); 
				Logger.Write(logEntry); 

				#endregion
			}		
		}

		private void btnMoveAllRightL1_Click(object sender, System.EventArgs e)
		{
			MoveAllItemsRight(lbAvailableColumnsL1,lbDisplayColumnsL1);
			RefreshLevel1SortCombo();		
		}

		private void btnMoveAllLeftL1_Click(object sender, System.EventArgs e)
		{
			try
			{
				if (lbDisplayColumnsL1.Items.Count > 1)
				{
					MoveAllItemsLeft(lbDisplayColumnsL1,lbAvailableColumnsL1); 
					RefreshLevel1SortCombo();	
				}
				else
				{
					MessageBox.Show("Atleast one Column should be displayed!!");
				
				}
			}
			catch(Exception ex)
			{
				string formattedInfo = ex.Message + "\n Stack Trace:" + ex.StackTrace.ToString() + "\n Inner Exception:"  + ex.InnerException;
				Logger.Write(formattedInfo, Common.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, 
					FORM_NAME);
				AppMessageExceptionHandler appMessageExceptionHandler = new AppMessageExceptionHandler(null);
				appMessageExceptionHandler.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), Common.POLICY_LOGANDSHOW, System.Guid.NewGuid());
			}
			finally
			{
				#region LogEntry

				LogEntry logEntry = new LogEntry("btnMoveLeft_Click", 
					Common.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
					FORM_NAME + "btnMoveLeft_Click",null); 
				Logger.Write(logEntry); 

				#endregion
			}		
		}

		private void btnMoveLeftL1_Click(object sender, System.EventArgs e)
		{
			try
			{
				MoveItem(lbDisplayColumnsL1,lbAvailableColumnsL1);
				RefreshLevel1SortCombo();	
			}
			catch(Exception ex)
			{
				#region Catch
				string formattedInfo = ex.Message + "\n Stack Trace:" + ex.StackTrace.ToString() + "\n Inner Exception:"  + ex.InnerException;
				Logger.Write(formattedInfo, Common.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, 
					FORM_NAME);
				AppMessageExceptionHandler appMessageExceptionHandler = new AppMessageExceptionHandler(null);
				appMessageExceptionHandler.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), Common.POLICY_LOGANDSHOW, System.Guid.NewGuid());
				#endregion
			}
			finally
			{
				#region LogEntry

				LogEntry logEntry = new LogEntry("btnMoveAllLeft_Click", 
					Common.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
					FORM_NAME + "btnMoveAllLeft_Click",null); 
				Logger.Write(logEntry); 

				#endregion
			}		
		}

		private void btnUpL1_Click(object sender, System.EventArgs e)
		{
			try
			{
				MoveUpDown(lbDisplayColumnsL1,true);
			}
			catch(Exception ex)
			{
				string formattedInfo = ex.Message + "\n Stack Trace:" + ex.StackTrace.ToString() + "\n Inner Exception:"  + ex.InnerException;
				Logger.Write(formattedInfo, Common.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, 
					FORM_NAME);
				AppMessageExceptionHandler appMessageExceptionHandler = new AppMessageExceptionHandler(null);
				appMessageExceptionHandler.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), Common.POLICY_LOGANDSHOW, System.Guid.NewGuid());
			}
			finally
			{
				#region LogEntry

				LogEntry logEntry = new LogEntry("btnUp_Click", 
					Common.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
					FORM_NAME + "btnUp_Click",null); 
				Logger.Write(logEntry); 

				#endregion
			}		
		}

		private void btnDownL1_Click(object sender, System.EventArgs e)
		{
			try
			{
				MoveUpDown(lbDisplayColumnsL1,false);
			}
			catch(Exception ex)
			{
				string formattedInfo = ex.Message + "\n Stack Trace:" + ex.StackTrace.ToString() + "\n Inner Exception:"  + ex.InnerException;
				Logger.Write(formattedInfo, Common.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, 
					FORM_NAME);
				AppMessageExceptionHandler appMessageExceptionHandler = new AppMessageExceptionHandler(null);
				appMessageExceptionHandler.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), Common.POLICY_LOGANDSHOW, System.Guid.NewGuid());
			}
			finally
			{
				#region LogEntry

				LogEntry logEntry = new LogEntry("btnDown_Click", 
					Common.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
					FORM_NAME + "btnDown_Click",null); 
				Logger.Write(logEntry); 

				#endregion
			}		
		}




		private void lbTabs_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			//Save the previous data
			switch(currentTab)
			{
				case 0:
					StoreColumnAssetData();
					break;

				case 1:
					StoreColumnSymbolData();
					break;

				case 2:
					StoreColumnTradingAccountData();
					break;
			}

			this.currentTab = lbTabs.SelectedIndex;

			this.SetGroupByLevelColumn();

			this.UpdateLevel1Panel();

			this.UpdateLevel2Panel();
		}
		
		private void rbAscL2_CheckedChanged(object sender, System.EventArgs e)
		{
			if(rbAscL2.Checked)
				rbDescL2.Checked = false;		
		}

		private void rbDescL2_CheckedChanged(object sender, System.EventArgs e)
		{
			if(rbDescL2.Checked)
				rbAscL2.Checked = false;			
		}

		private void rbAscL1_CheckedChanged(object sender, System.EventArgs e)
		{
			if(rbAscL1.Checked)
				rbDescL1.Checked = false;

		}

		private void rbDescL1_CheckedChanged(object sender, System.EventArgs e)
		{
			if(rbDescL1.Checked)
				rbAscL1.Checked = false;		
		}

		private void lbDisplayColumnsL1_SizeChanged(object sender, System.EventArgs e)
		{
			RefreshLevel1SortCombo();
		}

		private void lbDisplayColumnsL2_SizeChanged(object sender, System.EventArgs e)
		{
			RefreshLevel2SortCombo();
		}

		#endregion
	
		#region item shift in listbox functionality
		/// <summary>
		/// Single item transferred from source to destination listbox
		/// </summary>
		/// <param name="source"></param>
		/// <param name="destination"></param>
		private  void MoveItem(ListBox source, ListBox destination )
		{
			try
			{
				if (source.Items.Count > 0)
				{
					System.Windows.Forms.ListBox.SelectedObjectCollection selected = source.SelectedItems;
				
					string[] list = new string[selected.Count];
					int index = 0;

					// add to list
					foreach ( object obj in selected )
					{
						//destination.Items.Add(obj.ToString());
						list[index++] = obj.ToString();
					}


					source.DataSource = null;

					foreach (string columnName in list )
					{
						source.Items.Remove(columnName);
						destination.Items.Add(columnName);
					}

				}
			}
			catch(Exception ex)
			{
				#region Catch
				string formattedInfo = ex.Message + "\n Stack Trace:" + ex.StackTrace.ToString() + "\n Inner Exception:"  + ex.InnerException;
				Logger.Write(formattedInfo, Common.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, 
					FORM_NAME);
				AppMessageExceptionHandler appMessageExceptionHandler = new AppMessageExceptionHandler(null);
				appMessageExceptionHandler.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), Common.POLICY_LOGANDSHOW, System.Guid.NewGuid());
				#endregion
			}
			
		}


		/// <summary>
		/// All items moved to left list box
		/// </summary>
		/// <param name="source"></param>
		/// <param name="destination"></param>
		private  void MoveAllItemsLeft(ListBox source, ListBox destination)
		{
			try
			{
				MoveAllItems(source, destination);
			}
			catch(Exception ex)
			{
				#region Catch
				string formattedInfo = ex.Message + "\n Stack Trace:" + ex.StackTrace.ToString() + "\n Inner Exception:"  + ex.InnerException;
				Logger.Write(formattedInfo, Common.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, 
					FORM_NAME);
				AppMessageExceptionHandler appMessageExceptionHandler = new AppMessageExceptionHandler(null);
				appMessageExceptionHandler.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), Common.POLICY_LOGANDSHOW, System.Guid.NewGuid());
				#endregion
			}
		}

		private void MoveAllItems(ListBox source, ListBox destination)
		{
			int sourceCount = source.Items.Count;

			if (sourceCount !=1)
			{
				foreach (object obj in source.Items )
				{
					destination.Items.Add(obj.ToString());
				}
					
				source.Items.Clear();

			}
			else
			{
				MessageBox.Show("Atleast one Column should be displayed!!");
			}
		}

		/// <summary>
		/// All items moved to right list box
		/// </summary>
		/// <param name="source"></param>
		/// <param name="destination"></param>
		private  void MoveAllItemsRight(ListBox source, ListBox destination)
		{
			try
			{
					MoveAllItems(source, destination);
			}
			catch(Exception ex)
			{
				#region Catch
				string formattedInfo = ex.Message + "\n Stack Trace:" + ex.StackTrace.ToString() + "\n Inner Exception:"  + ex.InnerException;
				Logger.Write(formattedInfo, Common.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, 
					FORM_NAME);
				AppMessageExceptionHandler appMessageExceptionHandler = new AppMessageExceptionHandler(null);
				appMessageExceptionHandler.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), Common.POLICY_LOGANDSHOW, System.Guid.NewGuid());
				#endregion
			}
		}


		/// <summary>
		/// Functionality to move up or down
		/// </summary>
		/// <param name="listBox"></param>
		/// <param name="moveUp"></param>
		public  void MoveUpDown(ListBox listBox, bool moveUp )
		{
			try
			{
				if ( listBox.SelectedItem == null ) 
				{
					return;
				}

				string selectedItem = listBox.SelectedItem.ToString();
				int selectedIndex = Convert.ToInt32(listBox.SelectedIndex); 
			
				// copy source items in a list
				ArrayList list = new ArrayList();
				foreach(object obj in listBox.Items)
				{
					list.Add(obj.ToString());
				}

				if ( moveUp && selectedIndex!=0)
				{
					list[selectedIndex]   = list[selectedIndex-1].ToString();
					list[selectedIndex-1] = selectedItem;
					selectedIndex--;
				}
				else if ( !moveUp && (selectedIndex != (listBox.Items.Count-1) ) )
				{
					list[selectedIndex]   = list[selectedIndex+1].ToString();
					list[selectedIndex+1] = selectedItem; 
					selectedIndex++;
				}
  
				listBox.DataSource = null;
				listBox.Items.Clear();
				listBox.DataSource = list;
				listBox.SelectedIndex = selectedIndex; 
			}
			catch(Exception ex)
			{
				string formattedInfo = ex.Message + "\n Stack Trace:" + ex.StackTrace.ToString() + "\n Inner Exception:"  + ex.InnerException;
				Logger.Write(formattedInfo, Common.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, 
					FORM_NAME);
				AppMessageExceptionHandler appMessageExceptionHandler = new AppMessageExceptionHandler(null);
				appMessageExceptionHandler.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), Common.POLICY_LOGANDSHOW, System.Guid.NewGuid());
			}
		}


		private void MoveItemSortCombo(ListBox source)
		{

		}

		#endregion

		#region IPreferences Members
		
		public void SavePrefs()
		{
			this.Save();
		}

		public void RestoreDefault()
		{
			this.SetDefault();
		}
		
		public UserControl Reference()
		{
			return this;
		}

		public IPreferenceData GetPrefs()
		{
			this.StorePreferencesData();
			return this._PNLPrefrences;
		}

		#endregion

		private void clrPck_ColorChanged(object sender, System.EventArgs e)
		{
			Infragistics.Win.UltraWinEditors.UltraColorPicker objUltraColorPicker = (Infragistics.Win.UltraWinEditors.UltraColorPicker)sender;
			
			objUltraColorPicker.Appearance.BackColor=objUltraColorPicker.Color;
			objUltraColorPicker.Appearance.BorderColor =objUltraColorPicker.Color;
			objUltraColorPicker.Appearance.ForeColor=objUltraColorPicker.Color;
		}

		private void UpdateColorPickerControl(Infragistics.Win.UltraWinEditors.UltraColorPicker objUltraColorPicker)
		{
			clrPck_ColorChanged(objUltraColorPicker, null);
		}

		private void tabPageChart_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
		
		}

		private void cmbDefaultFeedColumn_ValueChanged(object sender, System.EventArgs e)
		{
		
		}

		private void ultraTabControl1_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
		{
		
		}
	}
}
