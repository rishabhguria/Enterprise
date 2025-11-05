using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;

using Prana.Global;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging;
//using Prana.ClientCommon;
//using System.EnterpriseServices;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Instrumentation;
using Prana.CommonDataCache;

namespace Prana.CommissionRules
{
	/// <summary>
	/// Summary description for BlotterFilters.
	/// </summary>
	public class CtrlAllocationFilters : System.Windows.Forms.UserControl
	{
        private const string FORM_NAME = "CtrlAllocationFilter: userControl : ";

		const string C_COMBO_SELECT = "- Select -";
        private Prana.BusinessObjects.CompanyUser _LoginUser;
        private Label label1;
        private Label label6;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label9;
        private TextBox textBox1;
        private UltraCombo cmbCounterParty;
        private UltraCombo cmbVenue;
        private UltraCombo cmbClient;
        private UltraCombo cmbUnderLying;
        private UltraCombo cmbAsset;
        private UltraCombo cmbSide;
        private UltraCombo cmbOrderState;
        private FlowLayoutPanel flowLayoutPanel1;
        private Panel panel1;
        private Panel panel2;
        private Panel panel5;
        private Panel panel3;
        private Panel panel4;
        private Panel panel9;
        private Panel panel8;
        private Panel panel6;
        private Panel panel10;
        private Panel panel11;
        private Label label10;
        private UltraCombo cmbTradingAccount;
        private Label label7;
        private Panel panel12;
        private Button button1;
        private Button btnRefresh;
        private Label label11;
        private UltraCombo ultraCombo1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

        public CtrlAllocationFilters()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call

		}
		
		public void SetUp(Prana.BusinessObjects.CompanyUser user)
		{
			_LoginUser=user;

			BindAsset();		
			BindUnderLying(int.MinValue);
			BindCompanyCounterParty();
			BindVenue(int.MinValue);
			//BindOrderType();
			BindOrderSide();
            BindTradingAccount();
			this.cmbAsset.ValueChanged += new System.EventHandler(this.cmbAsset_ValueChanged);
			this.cmbCounterParty.ValueChanged += new System.EventHandler(this.cmbCounterParty_ValueChanged);
			this.cmbClient.ValueChanged += new System.EventHandler(this.allCombos_ValueChanged);
			this.cmbVenue.ValueChanged += new System.EventHandler(this.allCombos_ValueChanged);
			//this.cmbOrderType.ValueChanged += new System.EventHandler(this.allCombos_ValueChanged);
			this.cmbCounterParty.ValueChanged += new System.EventHandler(this.cmbCounterParty_ValueChanged);
			this.cmbOrderState.ValueChanged += new System.EventHandler(this.allCombos_ValueChanged);
			this.cmbSide.ValueChanged += new System.EventHandler(this.allCombos_ValueChanged);
			this.cmbUnderLying.ValueChanged += new System.EventHandler(this.allCombos_ValueChanged);
            this.cmbTradingAccount.ValueChanged += new EventHandler(this.allCombos_ValueChanged);
			///This call is given here to Bind Venues and UnderLying
			///(If given in the Bind Methods, throws exception since at that time other combos
			///are not yet binded and calling of refresh methods leads to unwanted exceptions)
		}

        void cmbTradingAccount_ValueChanged(object sender, EventArgs e)
        {
            
        }
		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				this.cmbAsset.ValueChanged -= new System.EventHandler(this.cmbAsset_ValueChanged);
				this.cmbCounterParty.ValueChanged -= new System.EventHandler(this.cmbCounterParty_ValueChanged);
			
				this.cmbClient.ValueChanged -= new System.EventHandler(this.allCombos_ValueChanged);
				this.cmbVenue.ValueChanged -= new System.EventHandler(this.allCombos_ValueChanged);
				//this.cmbOrderType.ValueChanged -= new System.EventHandler(this.allCombos_ValueChanged);
				this.cmbCounterParty.ValueChanged -= new System.EventHandler(this.cmbCounterParty_ValueChanged);
				this.cmbOrderState.ValueChanged -= new System.EventHandler(this.allCombos_ValueChanged);
				this.cmbSide.ValueChanged -= new System.EventHandler(this.allCombos_ValueChanged);
				this.cmbUnderLying.ValueChanged -= new System.EventHandler(this.allCombos_ValueChanged);
                this.cmbTradingAccount.ValueChanged -= new System.EventHandler(this.cmbTradingAccount_ValueChanged);
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
            Infragistics.Win.Appearance appearance141 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance142 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance143 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.Appearance appearance144 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance145 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance146 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance147 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance148 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance149 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance150 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance151 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance152 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance153 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance154 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance169 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance170 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance171 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand2 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.Appearance appearance172 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance173 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance174 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance175 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance176 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance177 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance178 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance179 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance180 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance181 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance182 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance183 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance184 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance185 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand3 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.Appearance appearance186 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance187 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance188 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance189 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance190 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance191 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance192 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance193 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance194 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance195 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance196 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance197 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance198 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance199 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand4 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.Appearance appearance200 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance201 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance202 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance203 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance204 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance205 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance206 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance207 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance208 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance209 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance210 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance211 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance212 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance213 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand5 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.Appearance appearance214 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance215 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance216 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance217 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance218 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance219 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance220 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance221 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance222 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance223 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance224 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance225 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance226 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance227 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand6 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.Appearance appearance228 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance229 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance230 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance231 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance232 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance233 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance234 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance235 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance236 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance237 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance238 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance239 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance240 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance241 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand7 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.Appearance appearance242 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance243 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance244 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance245 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance246 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance247 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance248 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance249 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance250 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance251 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance252 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance253 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance254 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance255 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand8 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.Appearance appearance256 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance257 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance258 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance259 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance260 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance261 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance262 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance263 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance264 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance265 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance266 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance267 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance268 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance269 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand9 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.Appearance appearance270 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance271 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance272 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance273 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance274 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance275 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance276 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance277 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance278 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance279 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance280 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CtrlAllocationFilters));
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.cmbCounterParty = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.cmbVenue = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.cmbClient = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.cmbUnderLying = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.cmbAsset = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.cmbSide = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.cmbOrderState = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel9 = new System.Windows.Forms.Panel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.panel11 = new System.Windows.Forms.Panel();
            this.cmbTradingAccount = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.label10 = new System.Windows.Forms.Label();
            this.panel10 = new System.Windows.Forms.Panel();
            this.ultraCombo1 = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.label11 = new System.Windows.Forms.Label();
            this.panel12 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCounterParty)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbVenue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbClient)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbUnderLying)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAsset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSide)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbOrderState)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel9.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel11.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTradingAccount)).BeginInit();
            this.panel10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCombo1)).BeginInit();
            this.panel12.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 18);
            this.label1.TabIndex = 1;
            this.label1.Text = "Symbol";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(0, 1);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 18);
            this.label6.TabIndex = 11;
            this.label6.Text = "Venue";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(0, 1);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 18);
            this.label5.TabIndex = 9;
            this.label5.Text = "C. Party";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(0, 1);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 18);
            this.label4.TabIndex = 7;
            this.label4.Text = "Side";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(0, 1);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 18);
            this.label3.TabIndex = 5;
            this.label3.Text = "Underlying";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(0, 1);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 18);
            this.label2.TabIndex = 3;
            this.label2.Text = "Asset";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(0, 1);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(70, 18);
            this.label9.TabIndex = 17;
            this.label9.Text = "Exchange";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBox1
            // 
            this.textBox1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textBox1.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.textBox1.Location = new System.Drawing.Point(70, 1);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(74, 18);
            this.textBox1.TabIndex = 53;
            // 
            // cmbCounterParty
            // 
            appearance141.BorderColor = System.Drawing.Color.Black;
            this.cmbCounterParty.Appearance = appearance141;
            appearance142.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance142.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.cmbCounterParty.ButtonAppearance = appearance142;
            this.cmbCounterParty.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance143.BackColor = System.Drawing.SystemColors.Window;
            appearance143.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbCounterParty.DisplayLayout.Appearance = appearance143;
            ultraGridBand1.ColHeadersVisible = false;
            this.cmbCounterParty.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.cmbCounterParty.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbCounterParty.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance144.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance144.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance144.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance144.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbCounterParty.DisplayLayout.GroupByBox.Appearance = appearance144;
            appearance145.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbCounterParty.DisplayLayout.GroupByBox.BandLabelAppearance = appearance145;
            this.cmbCounterParty.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance146.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance146.BackColor2 = System.Drawing.SystemColors.Control;
            appearance146.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance146.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbCounterParty.DisplayLayout.GroupByBox.PromptAppearance = appearance146;
            this.cmbCounterParty.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbCounterParty.DisplayLayout.MaxRowScrollRegions = 1;
            appearance147.BackColor = System.Drawing.SystemColors.Window;
            appearance147.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbCounterParty.DisplayLayout.Override.ActiveCellAppearance = appearance147;
            appearance148.BackColor = System.Drawing.SystemColors.Highlight;
            appearance148.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbCounterParty.DisplayLayout.Override.ActiveRowAppearance = appearance148;
            this.cmbCounterParty.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbCounterParty.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance149.BackColor = System.Drawing.SystemColors.Window;
            this.cmbCounterParty.DisplayLayout.Override.CardAreaAppearance = appearance149;
            appearance150.BorderColor = System.Drawing.Color.Silver;
            appearance150.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbCounterParty.DisplayLayout.Override.CellAppearance = appearance150;
            this.cmbCounterParty.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbCounterParty.DisplayLayout.Override.CellPadding = 0;
            appearance151.BackColor = System.Drawing.SystemColors.Control;
            appearance151.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance151.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance151.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance151.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbCounterParty.DisplayLayout.Override.GroupByRowAppearance = appearance151;
            appearance152.TextHAlignAsString = "Left";
            this.cmbCounterParty.DisplayLayout.Override.HeaderAppearance = appearance152;
            this.cmbCounterParty.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbCounterParty.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.Standard;
            appearance153.BackColor = System.Drawing.SystemColors.Window;
            appearance153.BorderColor = System.Drawing.Color.Silver;
            this.cmbCounterParty.DisplayLayout.Override.RowAppearance = appearance153;
            this.cmbCounterParty.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance154.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbCounterParty.DisplayLayout.Override.TemplateAddRowAppearance = appearance154;
            this.cmbCounterParty.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbCounterParty.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbCounterParty.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbCounterParty.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Default;
            this.cmbCounterParty.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbCounterParty.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbCounterParty.Location = new System.Drawing.Point(70, 1);
            this.cmbCounterParty.Name = "cmbCounterParty";
            this.cmbCounterParty.Size = new System.Drawing.Size(74, 18);
            this.cmbCounterParty.TabIndex = 59;
            this.cmbCounterParty.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbCounterParty.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.cmbCounterParty_InitializeLayout);
            // 
            // cmbVenue
            // 
            appearance169.BorderColor = System.Drawing.Color.Black;
            this.cmbVenue.Appearance = appearance169;
            appearance170.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance170.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.cmbVenue.ButtonAppearance = appearance170;
            this.cmbVenue.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance171.BackColor = System.Drawing.SystemColors.Window;
            appearance171.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbVenue.DisplayLayout.Appearance = appearance171;
            ultraGridBand2.ColHeadersVisible = false;
            this.cmbVenue.DisplayLayout.BandsSerializer.Add(ultraGridBand2);
            this.cmbVenue.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbVenue.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance172.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance172.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance172.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance172.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbVenue.DisplayLayout.GroupByBox.Appearance = appearance172;
            appearance173.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbVenue.DisplayLayout.GroupByBox.BandLabelAppearance = appearance173;
            this.cmbVenue.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance174.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance174.BackColor2 = System.Drawing.SystemColors.Control;
            appearance174.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance174.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbVenue.DisplayLayout.GroupByBox.PromptAppearance = appearance174;
            this.cmbVenue.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbVenue.DisplayLayout.MaxRowScrollRegions = 1;
            appearance175.BackColor = System.Drawing.SystemColors.Window;
            appearance175.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbVenue.DisplayLayout.Override.ActiveCellAppearance = appearance175;
            appearance176.BackColor = System.Drawing.SystemColors.Highlight;
            appearance176.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbVenue.DisplayLayout.Override.ActiveRowAppearance = appearance176;
            this.cmbVenue.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbVenue.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance177.BackColor = System.Drawing.SystemColors.Window;
            this.cmbVenue.DisplayLayout.Override.CardAreaAppearance = appearance177;
            appearance178.BorderColor = System.Drawing.Color.Silver;
            appearance178.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbVenue.DisplayLayout.Override.CellAppearance = appearance178;
            this.cmbVenue.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbVenue.DisplayLayout.Override.CellPadding = 0;
            appearance179.BackColor = System.Drawing.SystemColors.Control;
            appearance179.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance179.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance179.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance179.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbVenue.DisplayLayout.Override.GroupByRowAppearance = appearance179;
            appearance180.TextHAlignAsString = "Left";
            this.cmbVenue.DisplayLayout.Override.HeaderAppearance = appearance180;
            this.cmbVenue.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbVenue.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.Standard;
            appearance181.BackColor = System.Drawing.SystemColors.Window;
            appearance181.BorderColor = System.Drawing.Color.Silver;
            this.cmbVenue.DisplayLayout.Override.RowAppearance = appearance181;
            this.cmbVenue.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance182.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbVenue.DisplayLayout.Override.TemplateAddRowAppearance = appearance182;
            this.cmbVenue.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbVenue.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbVenue.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbVenue.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Default;
            this.cmbVenue.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbVenue.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbVenue.Location = new System.Drawing.Point(70, 1);
            this.cmbVenue.Name = "cmbVenue";
            this.cmbVenue.Size = new System.Drawing.Size(74, 18);
            this.cmbVenue.TabIndex = 60;
            this.cmbVenue.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // cmbClient
            // 
            appearance183.BorderColor = System.Drawing.Color.Black;
            this.cmbClient.Appearance = appearance183;
            appearance184.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance184.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.cmbClient.ButtonAppearance = appearance184;
            this.cmbClient.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance185.BackColor = System.Drawing.SystemColors.Window;
            appearance185.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbClient.DisplayLayout.Appearance = appearance185;
            ultraGridBand3.ColHeadersVisible = false;
            this.cmbClient.DisplayLayout.BandsSerializer.Add(ultraGridBand3);
            this.cmbClient.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbClient.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance186.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance186.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance186.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance186.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbClient.DisplayLayout.GroupByBox.Appearance = appearance186;
            appearance187.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbClient.DisplayLayout.GroupByBox.BandLabelAppearance = appearance187;
            this.cmbClient.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance188.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance188.BackColor2 = System.Drawing.SystemColors.Control;
            appearance188.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance188.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbClient.DisplayLayout.GroupByBox.PromptAppearance = appearance188;
            this.cmbClient.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbClient.DisplayLayout.MaxRowScrollRegions = 1;
            appearance189.BackColor = System.Drawing.SystemColors.Window;
            appearance189.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbClient.DisplayLayout.Override.ActiveCellAppearance = appearance189;
            appearance190.BackColor = System.Drawing.SystemColors.Highlight;
            appearance190.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbClient.DisplayLayout.Override.ActiveRowAppearance = appearance190;
            this.cmbClient.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbClient.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance191.BackColor = System.Drawing.SystemColors.Window;
            this.cmbClient.DisplayLayout.Override.CardAreaAppearance = appearance191;
            appearance192.BorderColor = System.Drawing.Color.Silver;
            appearance192.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbClient.DisplayLayout.Override.CellAppearance = appearance192;
            this.cmbClient.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbClient.DisplayLayout.Override.CellPadding = 0;
            appearance193.BackColor = System.Drawing.SystemColors.Control;
            appearance193.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance193.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance193.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance193.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbClient.DisplayLayout.Override.GroupByRowAppearance = appearance193;
            appearance194.TextHAlignAsString = "Left";
            this.cmbClient.DisplayLayout.Override.HeaderAppearance = appearance194;
            this.cmbClient.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbClient.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.Standard;
            appearance195.BackColor = System.Drawing.SystemColors.Window;
            appearance195.BorderColor = System.Drawing.Color.Silver;
            this.cmbClient.DisplayLayout.Override.RowAppearance = appearance195;
            this.cmbClient.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance196.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbClient.DisplayLayout.Override.TemplateAddRowAppearance = appearance196;
            this.cmbClient.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbClient.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbClient.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbClient.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Default;
            this.cmbClient.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbClient.Enabled = false;
            this.cmbClient.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbClient.Location = new System.Drawing.Point(70, 1);
            this.cmbClient.Name = "cmbClient";
            this.cmbClient.Size = new System.Drawing.Size(74, 18);
            this.cmbClient.TabIndex = 61;
            this.cmbClient.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // cmbUnderLying
            // 
            appearance197.BorderColor = System.Drawing.Color.Black;
            this.cmbUnderLying.Appearance = appearance197;
            appearance198.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance198.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.cmbUnderLying.ButtonAppearance = appearance198;
            this.cmbUnderLying.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance199.BackColor = System.Drawing.SystemColors.Window;
            appearance199.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbUnderLying.DisplayLayout.Appearance = appearance199;
            ultraGridBand4.ColHeadersVisible = false;
            this.cmbUnderLying.DisplayLayout.BandsSerializer.Add(ultraGridBand4);
            this.cmbUnderLying.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbUnderLying.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance200.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance200.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance200.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance200.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbUnderLying.DisplayLayout.GroupByBox.Appearance = appearance200;
            appearance201.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbUnderLying.DisplayLayout.GroupByBox.BandLabelAppearance = appearance201;
            this.cmbUnderLying.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance202.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance202.BackColor2 = System.Drawing.SystemColors.Control;
            appearance202.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance202.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbUnderLying.DisplayLayout.GroupByBox.PromptAppearance = appearance202;
            this.cmbUnderLying.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbUnderLying.DisplayLayout.MaxRowScrollRegions = 1;
            appearance203.BackColor = System.Drawing.SystemColors.Window;
            appearance203.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbUnderLying.DisplayLayout.Override.ActiveCellAppearance = appearance203;
            appearance204.BackColor = System.Drawing.SystemColors.Highlight;
            appearance204.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbUnderLying.DisplayLayout.Override.ActiveRowAppearance = appearance204;
            this.cmbUnderLying.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbUnderLying.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance205.BackColor = System.Drawing.SystemColors.Window;
            this.cmbUnderLying.DisplayLayout.Override.CardAreaAppearance = appearance205;
            appearance206.BorderColor = System.Drawing.Color.Silver;
            appearance206.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbUnderLying.DisplayLayout.Override.CellAppearance = appearance206;
            this.cmbUnderLying.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbUnderLying.DisplayLayout.Override.CellPadding = 0;
            appearance207.BackColor = System.Drawing.SystemColors.Control;
            appearance207.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance207.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance207.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance207.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbUnderLying.DisplayLayout.Override.GroupByRowAppearance = appearance207;
            appearance208.TextHAlignAsString = "Left";
            this.cmbUnderLying.DisplayLayout.Override.HeaderAppearance = appearance208;
            this.cmbUnderLying.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbUnderLying.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.Standard;
            appearance209.BackColor = System.Drawing.SystemColors.Window;
            appearance209.BorderColor = System.Drawing.Color.Silver;
            this.cmbUnderLying.DisplayLayout.Override.RowAppearance = appearance209;
            this.cmbUnderLying.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance210.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbUnderLying.DisplayLayout.Override.TemplateAddRowAppearance = appearance210;
            this.cmbUnderLying.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbUnderLying.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbUnderLying.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbUnderLying.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Default;
            this.cmbUnderLying.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbUnderLying.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbUnderLying.Location = new System.Drawing.Point(70, 1);
            this.cmbUnderLying.Name = "cmbUnderLying";
            this.cmbUnderLying.Size = new System.Drawing.Size(74, 18);
            this.cmbUnderLying.TabIndex = 55;
            this.cmbUnderLying.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // cmbAsset
            // 
            appearance211.BorderColor = System.Drawing.Color.Black;
            this.cmbAsset.Appearance = appearance211;
            appearance212.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance212.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.cmbAsset.ButtonAppearance = appearance212;
            this.cmbAsset.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance213.BackColor = System.Drawing.SystemColors.Window;
            appearance213.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbAsset.DisplayLayout.Appearance = appearance213;
            ultraGridBand5.ColHeadersVisible = false;
            this.cmbAsset.DisplayLayout.BandsSerializer.Add(ultraGridBand5);
            this.cmbAsset.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbAsset.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance214.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance214.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance214.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance214.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbAsset.DisplayLayout.GroupByBox.Appearance = appearance214;
            appearance215.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbAsset.DisplayLayout.GroupByBox.BandLabelAppearance = appearance215;
            this.cmbAsset.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance216.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance216.BackColor2 = System.Drawing.SystemColors.Control;
            appearance216.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance216.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbAsset.DisplayLayout.GroupByBox.PromptAppearance = appearance216;
            this.cmbAsset.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbAsset.DisplayLayout.MaxRowScrollRegions = 1;
            appearance217.BackColor = System.Drawing.SystemColors.Window;
            appearance217.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbAsset.DisplayLayout.Override.ActiveCellAppearance = appearance217;
            appearance218.BackColor = System.Drawing.SystemColors.Highlight;
            appearance218.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbAsset.DisplayLayout.Override.ActiveRowAppearance = appearance218;
            this.cmbAsset.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbAsset.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance219.BackColor = System.Drawing.SystemColors.Window;
            this.cmbAsset.DisplayLayout.Override.CardAreaAppearance = appearance219;
            appearance220.BorderColor = System.Drawing.Color.Silver;
            appearance220.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbAsset.DisplayLayout.Override.CellAppearance = appearance220;
            this.cmbAsset.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbAsset.DisplayLayout.Override.CellPadding = 0;
            appearance221.BackColor = System.Drawing.SystemColors.Control;
            appearance221.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance221.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance221.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance221.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbAsset.DisplayLayout.Override.GroupByRowAppearance = appearance221;
            appearance222.TextHAlignAsString = "Left";
            this.cmbAsset.DisplayLayout.Override.HeaderAppearance = appearance222;
            this.cmbAsset.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbAsset.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.Standard;
            appearance223.BackColor = System.Drawing.SystemColors.Window;
            appearance223.BorderColor = System.Drawing.Color.Silver;
            this.cmbAsset.DisplayLayout.Override.RowAppearance = appearance223;
            this.cmbAsset.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance224.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbAsset.DisplayLayout.Override.TemplateAddRowAppearance = appearance224;
            this.cmbAsset.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbAsset.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbAsset.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbAsset.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Default;
            this.cmbAsset.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbAsset.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbAsset.Location = new System.Drawing.Point(70, 1);
            this.cmbAsset.Name = "cmbAsset";
            this.cmbAsset.Size = new System.Drawing.Size(74, 18);
            this.cmbAsset.TabIndex = 54;
            this.cmbAsset.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // cmbSide
            // 
            appearance225.BorderColor = System.Drawing.Color.Black;
            this.cmbSide.Appearance = appearance225;
            appearance226.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance226.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.cmbSide.ButtonAppearance = appearance226;
            this.cmbSide.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance227.BackColor = System.Drawing.SystemColors.Window;
            appearance227.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbSide.DisplayLayout.Appearance = appearance227;
            ultraGridBand6.ColHeadersVisible = false;
            this.cmbSide.DisplayLayout.BandsSerializer.Add(ultraGridBand6);
            this.cmbSide.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbSide.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance228.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance228.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance228.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance228.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbSide.DisplayLayout.GroupByBox.Appearance = appearance228;
            appearance229.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbSide.DisplayLayout.GroupByBox.BandLabelAppearance = appearance229;
            this.cmbSide.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance230.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance230.BackColor2 = System.Drawing.SystemColors.Control;
            appearance230.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance230.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbSide.DisplayLayout.GroupByBox.PromptAppearance = appearance230;
            this.cmbSide.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbSide.DisplayLayout.MaxRowScrollRegions = 1;
            appearance231.BackColor = System.Drawing.SystemColors.Window;
            appearance231.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbSide.DisplayLayout.Override.ActiveCellAppearance = appearance231;
            appearance232.BackColor = System.Drawing.SystemColors.Highlight;
            appearance232.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbSide.DisplayLayout.Override.ActiveRowAppearance = appearance232;
            this.cmbSide.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbSide.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance233.BackColor = System.Drawing.SystemColors.Window;
            this.cmbSide.DisplayLayout.Override.CardAreaAppearance = appearance233;
            appearance234.BorderColor = System.Drawing.Color.Silver;
            appearance234.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbSide.DisplayLayout.Override.CellAppearance = appearance234;
            this.cmbSide.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbSide.DisplayLayout.Override.CellPadding = 0;
            appearance235.BackColor = System.Drawing.SystemColors.Control;
            appearance235.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance235.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance235.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance235.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbSide.DisplayLayout.Override.GroupByRowAppearance = appearance235;
            appearance236.TextHAlignAsString = "Left";
            this.cmbSide.DisplayLayout.Override.HeaderAppearance = appearance236;
            this.cmbSide.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbSide.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.Standard;
            appearance237.BackColor = System.Drawing.SystemColors.Window;
            appearance237.BorderColor = System.Drawing.Color.Silver;
            this.cmbSide.DisplayLayout.Override.RowAppearance = appearance237;
            this.cmbSide.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance238.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbSide.DisplayLayout.Override.TemplateAddRowAppearance = appearance238;
            this.cmbSide.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbSide.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbSide.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbSide.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Default;
            this.cmbSide.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbSide.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbSide.Location = new System.Drawing.Point(70, 1);
            this.cmbSide.Name = "cmbSide";
            this.cmbSide.Size = new System.Drawing.Size(74, 18);
            this.cmbSide.TabIndex = 56;
            this.cmbSide.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // cmbOrderState
            // 
            appearance239.BorderColor = System.Drawing.Color.Black;
            this.cmbOrderState.Appearance = appearance239;
            appearance240.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance240.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.cmbOrderState.ButtonAppearance = appearance240;
            this.cmbOrderState.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance241.BackColor = System.Drawing.SystemColors.Window;
            appearance241.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbOrderState.DisplayLayout.Appearance = appearance241;
            ultraGridBand7.ColHeadersVisible = false;
            this.cmbOrderState.DisplayLayout.BandsSerializer.Add(ultraGridBand7);
            this.cmbOrderState.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbOrderState.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance242.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance242.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance242.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance242.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbOrderState.DisplayLayout.GroupByBox.Appearance = appearance242;
            appearance243.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbOrderState.DisplayLayout.GroupByBox.BandLabelAppearance = appearance243;
            this.cmbOrderState.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance244.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance244.BackColor2 = System.Drawing.SystemColors.Control;
            appearance244.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance244.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbOrderState.DisplayLayout.GroupByBox.PromptAppearance = appearance244;
            this.cmbOrderState.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbOrderState.DisplayLayout.MaxRowScrollRegions = 1;
            appearance245.BackColor = System.Drawing.SystemColors.Window;
            appearance245.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbOrderState.DisplayLayout.Override.ActiveCellAppearance = appearance245;
            appearance246.BackColor = System.Drawing.SystemColors.Highlight;
            appearance246.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbOrderState.DisplayLayout.Override.ActiveRowAppearance = appearance246;
            this.cmbOrderState.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbOrderState.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance247.BackColor = System.Drawing.SystemColors.Window;
            this.cmbOrderState.DisplayLayout.Override.CardAreaAppearance = appearance247;
            appearance248.BorderColor = System.Drawing.Color.Silver;
            appearance248.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbOrderState.DisplayLayout.Override.CellAppearance = appearance248;
            this.cmbOrderState.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbOrderState.DisplayLayout.Override.CellPadding = 0;
            appearance249.BackColor = System.Drawing.SystemColors.Control;
            appearance249.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance249.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance249.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance249.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbOrderState.DisplayLayout.Override.GroupByRowAppearance = appearance249;
            appearance250.TextHAlignAsString = "Left";
            this.cmbOrderState.DisplayLayout.Override.HeaderAppearance = appearance250;
            this.cmbOrderState.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbOrderState.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.Standard;
            appearance251.BackColor = System.Drawing.SystemColors.Window;
            appearance251.BorderColor = System.Drawing.Color.Silver;
            this.cmbOrderState.DisplayLayout.Override.RowAppearance = appearance251;
            this.cmbOrderState.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance252.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbOrderState.DisplayLayout.Override.TemplateAddRowAppearance = appearance252;
            this.cmbOrderState.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbOrderState.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbOrderState.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbOrderState.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Default;
            this.cmbOrderState.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbOrderState.Enabled = false;
            this.cmbOrderState.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbOrderState.Location = new System.Drawing.Point(70, 1);
            this.cmbOrderState.Name = "cmbOrderState";
            this.cmbOrderState.Size = new System.Drawing.Size(74, 18);
            this.cmbOrderState.TabIndex = 57;
            this.cmbOrderState.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.panel1);
            this.flowLayoutPanel1.Controls.Add(this.panel2);
            this.flowLayoutPanel1.Controls.Add(this.panel5);
            this.flowLayoutPanel1.Controls.Add(this.panel3);
            this.flowLayoutPanel1.Controls.Add(this.panel4);
            this.flowLayoutPanel1.Controls.Add(this.panel9);
            this.flowLayoutPanel1.Controls.Add(this.panel8);
            this.flowLayoutPanel1.Controls.Add(this.panel6);
            this.flowLayoutPanel1.Controls.Add(this.panel11);
            this.flowLayoutPanel1.Controls.Add(this.panel10);
            this.flowLayoutPanel1.Controls.Add(this.panel12);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(154, 315);
            this.flowLayoutPanel1.TabIndex = 63;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(144, 22);
            this.panel1.TabIndex = 54;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.cmbAsset);
            this.panel2.Location = new System.Drawing.Point(3, 31);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(144, 22);
            this.panel2.TabIndex = 55;
            // 
            // panel5
            // 
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Controls.Add(this.label3);
            this.panel5.Controls.Add(this.cmbUnderLying);
            this.panel5.Location = new System.Drawing.Point(3, 59);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(144, 22);
            this.panel5.TabIndex = 56;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.cmbSide);
            this.panel3.Location = new System.Drawing.Point(3, 87);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(144, 22);
            this.panel3.TabIndex = 56;
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.cmbOrderState);
            this.panel4.Controls.Add(this.label9);
            this.panel4.Location = new System.Drawing.Point(3, 115);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(144, 22);
            this.panel4.TabIndex = 56;
            this.panel4.Visible = false;
            // 
            // panel9
            // 
            this.panel9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel9.Controls.Add(this.cmbCounterParty);
            this.panel9.Controls.Add(this.label5);
            this.panel9.Location = new System.Drawing.Point(3, 143);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(144, 22);
            this.panel9.TabIndex = 58;
            // 
            // panel8
            // 
            this.panel8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel8.Controls.Add(this.label6);
            this.panel8.Controls.Add(this.cmbVenue);
            this.panel8.Location = new System.Drawing.Point(3, 171);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(144, 22);
            this.panel8.TabIndex = 59;
            // 
            // panel6
            // 
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Controls.Add(this.label7);
            this.panel6.Controls.Add(this.cmbClient);
            this.panel6.Location = new System.Drawing.Point(3, 199);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(144, 22);
            this.panel6.TabIndex = 57;
            this.panel6.Visible = false;
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(0, 1);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(64, 18);
            this.label7.TabIndex = 13;
            this.label7.Text = "Currency";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel11
            // 
            this.panel11.Controls.Add(this.cmbTradingAccount);
            this.panel11.Controls.Add(this.label10);
            this.panel11.Location = new System.Drawing.Point(3, 227);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(144, 22);
            this.panel11.TabIndex = 57;
            this.panel11.Visible = false;
            // 
            // cmbTradingAccount
            // 
            appearance253.BorderColor = System.Drawing.Color.Black;
            this.cmbTradingAccount.Appearance = appearance253;
            appearance254.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance254.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.cmbTradingAccount.ButtonAppearance = appearance254;
            this.cmbTradingAccount.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance255.BackColor = System.Drawing.SystemColors.Window;
            appearance255.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbTradingAccount.DisplayLayout.Appearance = appearance255;
            ultraGridBand8.ColHeadersVisible = false;
            this.cmbTradingAccount.DisplayLayout.BandsSerializer.Add(ultraGridBand8);
            this.cmbTradingAccount.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbTradingAccount.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance256.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance256.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance256.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance256.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbTradingAccount.DisplayLayout.GroupByBox.Appearance = appearance256;
            appearance257.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbTradingAccount.DisplayLayout.GroupByBox.BandLabelAppearance = appearance257;
            this.cmbTradingAccount.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance258.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance258.BackColor2 = System.Drawing.SystemColors.Control;
            appearance258.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance258.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbTradingAccount.DisplayLayout.GroupByBox.PromptAppearance = appearance258;
            this.cmbTradingAccount.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbTradingAccount.DisplayLayout.MaxRowScrollRegions = 1;
            appearance259.BackColor = System.Drawing.SystemColors.Window;
            appearance259.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbTradingAccount.DisplayLayout.Override.ActiveCellAppearance = appearance259;
            appearance260.BackColor = System.Drawing.SystemColors.Highlight;
            appearance260.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbTradingAccount.DisplayLayout.Override.ActiveRowAppearance = appearance260;
            this.cmbTradingAccount.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbTradingAccount.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance261.BackColor = System.Drawing.SystemColors.Window;
            this.cmbTradingAccount.DisplayLayout.Override.CardAreaAppearance = appearance261;
            appearance262.BorderColor = System.Drawing.Color.Silver;
            appearance262.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbTradingAccount.DisplayLayout.Override.CellAppearance = appearance262;
            this.cmbTradingAccount.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbTradingAccount.DisplayLayout.Override.CellPadding = 0;
            appearance263.BackColor = System.Drawing.SystemColors.Control;
            appearance263.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance263.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance263.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance263.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbTradingAccount.DisplayLayout.Override.GroupByRowAppearance = appearance263;
            appearance264.TextHAlignAsString = "Left";
            this.cmbTradingAccount.DisplayLayout.Override.HeaderAppearance = appearance264;
            this.cmbTradingAccount.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbTradingAccount.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.Standard;
            appearance265.BackColor = System.Drawing.SystemColors.Window;
            appearance265.BorderColor = System.Drawing.Color.Silver;
            this.cmbTradingAccount.DisplayLayout.Override.RowAppearance = appearance265;
            this.cmbTradingAccount.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance266.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbTradingAccount.DisplayLayout.Override.TemplateAddRowAppearance = appearance266;
            this.cmbTradingAccount.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbTradingAccount.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbTradingAccount.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbTradingAccount.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Default;
            this.cmbTradingAccount.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbTradingAccount.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbTradingAccount.Location = new System.Drawing.Point(70, 1);
            this.cmbTradingAccount.Name = "cmbTradingAccount";
            this.cmbTradingAccount.Size = new System.Drawing.Size(74, 18);
            this.cmbTradingAccount.TabIndex = 56;
            this.cmbTradingAccount.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbTradingAccount.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.cmbTradingAccount_InitializeLayout);
            // 
            // label10
            // 
            this.label10.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(0, 1);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(70, 18);
            this.label10.TabIndex = 7;
            this.label10.Text = "Trad Acc.";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel10
            // 
            this.panel10.Controls.Add(this.ultraCombo1);
            this.panel10.Controls.Add(this.label11);
            this.panel10.Location = new System.Drawing.Point(3, 255);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(144, 24);
            this.panel10.TabIndex = 58;
            this.panel10.Visible = false;
            // 
            // ultraCombo1
            // 
            appearance267.BorderColor = System.Drawing.Color.Black;
            this.ultraCombo1.Appearance = appearance267;
            appearance268.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance268.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ultraCombo1.ButtonAppearance = appearance268;
            this.ultraCombo1.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance269.BackColor = System.Drawing.SystemColors.Window;
            appearance269.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.ultraCombo1.DisplayLayout.Appearance = appearance269;
            ultraGridBand9.ColHeadersVisible = false;
            this.ultraCombo1.DisplayLayout.BandsSerializer.Add(ultraGridBand9);
            this.ultraCombo1.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ultraCombo1.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance270.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance270.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance270.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance270.BorderColor = System.Drawing.SystemColors.Window;
            this.ultraCombo1.DisplayLayout.GroupByBox.Appearance = appearance270;
            appearance271.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ultraCombo1.DisplayLayout.GroupByBox.BandLabelAppearance = appearance271;
            this.ultraCombo1.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance272.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance272.BackColor2 = System.Drawing.SystemColors.Control;
            appearance272.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance272.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ultraCombo1.DisplayLayout.GroupByBox.PromptAppearance = appearance272;
            this.ultraCombo1.DisplayLayout.MaxColScrollRegions = 1;
            this.ultraCombo1.DisplayLayout.MaxRowScrollRegions = 1;
            appearance273.BackColor = System.Drawing.SystemColors.Window;
            appearance273.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ultraCombo1.DisplayLayout.Override.ActiveCellAppearance = appearance273;
            appearance274.BackColor = System.Drawing.SystemColors.Highlight;
            appearance274.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.ultraCombo1.DisplayLayout.Override.ActiveRowAppearance = appearance274;
            this.ultraCombo1.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.ultraCombo1.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance275.BackColor = System.Drawing.SystemColors.Window;
            this.ultraCombo1.DisplayLayout.Override.CardAreaAppearance = appearance275;
            appearance276.BorderColor = System.Drawing.Color.Silver;
            appearance276.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.ultraCombo1.DisplayLayout.Override.CellAppearance = appearance276;
            this.ultraCombo1.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.ultraCombo1.DisplayLayout.Override.CellPadding = 0;
            appearance277.BackColor = System.Drawing.SystemColors.Control;
            appearance277.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance277.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance277.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance277.BorderColor = System.Drawing.SystemColors.Window;
            this.ultraCombo1.DisplayLayout.Override.GroupByRowAppearance = appearance277;
            appearance278.TextHAlignAsString = "Left";
            this.ultraCombo1.DisplayLayout.Override.HeaderAppearance = appearance278;
            this.ultraCombo1.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.ultraCombo1.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.Standard;
            appearance279.BackColor = System.Drawing.SystemColors.Window;
            appearance279.BorderColor = System.Drawing.Color.Silver;
            this.ultraCombo1.DisplayLayout.Override.RowAppearance = appearance279;
            this.ultraCombo1.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance280.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ultraCombo1.DisplayLayout.Override.TemplateAddRowAppearance = appearance280;
            this.ultraCombo1.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.ultraCombo1.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.ultraCombo1.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.ultraCombo1.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Default;
            this.ultraCombo1.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.ultraCombo1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ultraCombo1.Location = new System.Drawing.Point(69, 1);
            this.ultraCombo1.Name = "ultraCombo1";
            this.ultraCombo1.Size = new System.Drawing.Size(74, 18);
            this.ultraCombo1.TabIndex = 61;
            this.ultraCombo1.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // label11
            // 
            this.label11.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(0, -1);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(64, 18);
            this.label11.TabIndex = 62;
            this.label11.Text = "User";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel12
            // 
            this.panel12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel12.Controls.Add(this.button1);
            this.panel12.Controls.Add(this.btnRefresh);
            this.panel12.Location = new System.Drawing.Point(3, 285);
            this.panel12.Name = "panel12";
            this.panel12.Size = new System.Drawing.Size(144, 24);
            this.panel12.TabIndex = 61;
            // 
            // button1
            // 
            this.button1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button1.BackgroundImage")));
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(76, 1);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(63, 18);
            this.button1.TabIndex = 52;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnRefresh.BackgroundImage")));
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Location = new System.Drawing.Point(0, 1);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(63, 18);
            this.btnRefresh.TabIndex = 32;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // CtrlCommissionFilters
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "CtrlCommissionFilters";
            this.Size = new System.Drawing.Size(153, 315);
            ((System.ComponentModel.ISupportInitialize)(this.cmbCounterParty)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbVenue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbClient)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbUnderLying)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAsset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSide)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbOrderState)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel9.ResumeLayout(false);
            this.panel9.PerformLayout();
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel11.ResumeLayout(false);
            this.panel11.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTradingAccount)).EndInit();
            this.panel10.ResumeLayout(false);
            this.panel10.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCombo1)).EndInit();
            this.panel12.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		
	
		#endregion
		public UltraCombo CmbSide
		{
			get{return cmbSide;}
		}
        //public UltraCombo CmbOrderType
        //{
        //    get{return cmbOrderType;}
        //}
		public UltraCombo CmbCounterParty
		{
			get{return cmbCounterParty;}
		}
		public UltraCombo CmbVenue
		{
			get{return cmbVenue;}
		}
		public UltraCombo CmbAsset
		{
			get{return cmbAsset;}
		}
		public UltraCombo CmbUnderLying
		{
			get{return cmbUnderLying;}
		}
		public TextBox TextBoxSymbol
		{
			get{return textBox1;}
		}
        
        public UltraCombo CmbTradingAccount
        {
            get { return cmbTradingAccount; }
        }
		private void BindAsset()
		{
			try
			{
			
				Assets assets = new Assets();
                assets = ClientsCommonDataManager.GetCompanyUserAssets(_LoginUser.CompanyUserID);
				assets.Insert(0, new Asset(int.MinValue,C_COMBO_SELECT));
			//	this.cmbAsset.ValueChanged -= new System.EventHandler(this.cmbAsset_ValueChanged);
				cmbAsset.DataSource = null;
				cmbAsset.DataSource = assets;			
			//	this.cmbAsset.ValueChanged += new System.EventHandler(this.cmbAsset_ValueChanged);
				cmbAsset.DisplayMember = "Name";
				cmbAsset.ValueMember = "AssetID";			

				this.cmbAsset.Value=int.MinValue;

		
				ColumnsCollection columns = cmbAsset.DisplayLayout.Bands[0].Columns;
				foreach (UltraGridColumn column in columns)
				{
				
					if(column.Key != "Name")
					{
						column.Hidden = true;
					}
					//				else
					//				{
					//					column.Header.Enabled = false;
					//				}
				}
			}
			catch(Exception ex)
			{
				// Invoke our policy that is responsible for making sure no secure information
				// gets out of our layer.
				bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

				if (rethrow)
				{
					throw; 
				}
			}
		}
		public void BindUnderLying(int AssetID)
		{
			try
			{
			
				//	cmbbxAsset.DataSource= AssetManager.
                UnderLyings underLyings = new UnderLyings();
                underLyings = ClientsCommonDataManager.GetUnderLyingsByAssetAndUserID(_LoginUser.CompanyUserID, AssetID);
                underLyings.Insert(0, new UnderLying(int.MinValue, C_COMBO_SELECT));
		
				cmbUnderLying.DataSource = null;
				cmbUnderLying.DataSource = underLyings;
				cmbUnderLying.DisplayMember = "Name";
				cmbUnderLying.ValueMember = "UnderLyingID";
				cmbUnderLying.Value=int.MinValue;
				ColumnsCollection columns = cmbUnderLying.DisplayLayout.Bands[0].Columns;
				foreach (UltraGridColumn column in columns)
				{
 
				 
					if(column.Key != "Name")
					{
						column.Hidden = true;
					}
					//				else
					//				{
					//					column.Header.Enabled = false;
					//				}
				}
			}
			catch(Exception ex)
			{
				// Invoke our policy that is responsible for making sure no secure information
				// gets out of our layer.
				bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

				if (rethrow)
				{
					throw; 
				}
			}

		}
		private void BindCompanyCounterParty()
		{
			try
			{
				CounterPartyCollection counterParties = new CounterPartyCollection ();
				if (cmbAsset.Value != null)
				{
					int assetID=Convert.ToInt32(cmbAsset.Value);
				}
				if (cmbUnderLying.Value != null)
				{
					int underlyingID=Convert.ToInt32(cmbUnderLying.Value);			
				}
                counterParties = ClientsCommonDataManager.GetCompanyCounterParties(_LoginUser.CompanyID);// .GetCounterPartiesByAUIDAndUserID(_LoginUser.CompanyUserID,assetID,underlyingID);
                counterParties.Insert(0, new CounterParty(int.MinValue, C_COMBO_SELECT));
			
			//	this.cmbCounterParty.ValueChanged -= new System.EventHandler(this.cmbCounterParty_ValueChanged);
				cmbCounterParty.DataSource = null;
				cmbCounterParty.DataSource = counterParties;
			//	this.cmbCounterParty.ValueChanged += new System.EventHandler(this.cmbCounterParty_ValueChanged);
				
				cmbCounterParty.DisplayMember = "Name";
				cmbCounterParty.ValueMember = "CounterPartyID";
				this.cmbCounterParty.Value = int.MinValue;

				ColumnsCollection columns = cmbCounterParty.DisplayLayout.Bands[0].Columns;
				foreach (UltraGridColumn column in columns)
				{
					if(column.Key != "Name")
					{
						column.Hidden = true;
					}
					//				else
					//				{
					//					column.Header.Enabled = false;
					//				}
				}
			}
			catch(Exception ex)
			{
				// Invoke our policy that is responsible for making sure no secure information
				// gets out of our layer.
				bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

				if (rethrow)
				{
					throw; 
				}
			}
		}
		private void BindVenue(int counterPartyID)
		{
			try
			{
				VenueCollection venues = new VenueCollection();
				if (cmbAsset.Value != null)
				{
					int assetID=Convert.ToInt32(cmbAsset.Value);
				}
				if (cmbUnderLying.Value != null)
				{
					int underlyingID=Convert.ToInt32(cmbUnderLying.Value);			
				}
				venues =ClientsCommonDataManager.GetVenues(_LoginUser.CompanyUserID, counterPartyID);// enuesByAUIDCounterPartyAndUserID(_LoginUser.CompanyUserID, counterPartyID,assetID,underlyingID);
				venues.Insert(0,new Prana.BusinessObjects.Venue(int.MinValue,C_COMBO_SELECT));
				cmbVenue.DataSource = null;

				cmbVenue.DataSource = venues;
				cmbVenue.DisplayMember = "Name";
				cmbVenue.ValueMember = "VenueID";
				cmbVenue.Value=int.MinValue;

				ColumnsCollection columns = cmbVenue.DisplayLayout.Bands[0].Columns;
				foreach (UltraGridColumn column in columns)
				{
					if(column.Key != "Name")
					{
						column.Hidden = true;
					}
					//				else
					//				{
					//					column.Header.Enabled = false;
					//				}
				}
			}
			catch(Exception ex)
			{
				// Invoke our policy that is responsible for making sure no secure information
				// gets out of our layer.
				bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

				if (rethrow)
				{
					throw; 
				}
			}
		}
        //private void BindOrderType()
        //{
        //    try
        //    {
        //        OrderTypes orderTypes = new OrderTypes();
        //        orderTypes = ClientsCommonDataManager.GetOrderTypes();// GetOrderTypesByAUCVID(assetID,underlyingID,counterPartyID, venueID);
			
        //        orderTypes.Insert( 0,new OrderType(int.MinValue,C_COMBO_SELECT, C_COMBO_SELECT));
        //       cmbOrderType.DataSource = null;
        //       cmbOrderType.DataSource = orderTypes;
        //       cmbOrderType.DisplayMember = "Type";
        //       cmbOrderType.ValueMember = "OrderTypesID";
        //       cmbOrderType.Value=int.MinValue;
			   
        //        ColumnsCollection columns = cmbOrderType.DisplayLayout.Bands[0].Columns;
        //        foreach (UltraGridColumn column in columns)
        //        {
        //            if(column.Key != "Type")
        //            {
        //                column.Hidden = true;
        //            }
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

        //        if (rethrow)
        //        {
        //            throw; 
        //        }
        //    }
        //}
		private void BindTradingAccount()
		{
			try
			{
                TradingAccountCollection accounts = new TradingAccountCollection();
                accounts = CachedDataManager.GetInstance.GetUserTradingAccounts();
                //accounts.Insert(0,new  Prana.Admin.BLL.TradingAccount(int.MinValue,C_COMBO_SELECT));
                cmbTradingAccount.DataSource = null;
                cmbTradingAccount.DataSource = accounts;
                cmbTradingAccount.DisplayMember = "Name";
                //// valuemember changed to tagvalue as we are identifying sides by that value across the application
                //// wherever we are using the filter we can check for cmbside.value to fixconstants.sides...
                cmbTradingAccount.ValueMember = "TradingAccountID";
                cmbTradingAccount.Value = int.MinValue.ToString();


                ColumnsCollection columns = cmbTradingAccount.DisplayLayout.Bands[0].Columns;
                foreach (UltraGridColumn column in columns)
                {
                    if (column.Key != "Name")
                    {
                        column.Hidden = true;
                    }
                }
			}
			catch(Exception ex)
			{
				// Invoke our policy that is responsible for making sure no secure information
				// gets out of our layer.
				bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

				if (rethrow)
				{
					throw; 
				}
			}
		}

        private void BindOrderSide()
        {
            try
            {
                Sides orderSides = new Sides();
                //orderSides = ClientsCommonDataManager.GetOrderSidesByCVAUEC(int.Parse(cmbAsset.Value.ToString()), int.Parse(cmbUnderLying.Value.ToString()), int.Parse(cmbCounterParty.Value.ToString()), int.Parse(cmbVenue.Value.ToString()));
                orderSides = ClientsCommonDataManager.GetSides();// GetOrderTypesByAUCVID(assetID,underlyingID,counterPartyID, venueID);
                orderSides.Insert(0, new Side(int.MinValue, C_COMBO_SELECT, int.MinValue.ToString()));
                cmbSide.DataSource = null;
                cmbSide.DataSource = orderSides;
                cmbSide.DisplayMember = "Name";
                // valuemember changed to tagvalue as we are identifying sides by that value across the application
                // wherever we are using the filter we can check for cmbside.value to fixconstants.sides...
                cmbSide.ValueMember = "TagValue";
                cmbSide.Value = int.MinValue.ToString();


                ColumnsCollection columns = cmbSide.DisplayLayout.Bands[0].Columns;
                foreach (UltraGridColumn column in columns)
                {

                    if (column.Key != "Name")
                    {
                        column.Hidden = true;
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        } 

		private void BindOrderStates()
		{
		}
	

		private void cmbAsset_ValueChanged(object sender, System.EventArgs e)
		{
			try
			{
				int assetID = int.MinValue;
					if 	(cmbAsset.Value != null)
				{
					assetID=  Convert.ToInt32(cmbAsset.Value);
				}
				BindUnderLying(assetID);
				if (RefreshClick != null)
				{
					RefreshClick(this,null);
				}
			}
			catch(Exception ex)
			{
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }

			}
			finally
			{
				
			}
		}

		private void cmbCounterParty_ValueChanged(object sender, System.EventArgs e)
		{
			try
			{
				int counterPartyID = int.MinValue;
				if 	(cmbCounterParty.Value != null)
				{
					counterPartyID = int.Parse(cmbCounterParty.Value.ToString());
				}
				BindVenue(counterPartyID);
			
				if (RefreshClick != null)
				{
					RefreshClick(this,null);
				}
			}
			catch(Exception ex)
			{
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }

			}
			finally
			{
				
			}
		}


	

		private void btnClear_Click(object sender, System.EventArgs e)
		{
			try
			{
				textBox1.Clear();
				cmbAsset.Value = int.MinValue;
				cmbUnderLying.Value = int.MinValue;
				cmbCounterParty.Value = int.MinValue;
				cmbVenue.Value = int.MinValue;
				cmbSide.Value = int.MinValue;
				//		cmbOrderState.Value = int.MinValue;
				//cmbOrderType.Value = int.MinValue;
				//		cmbClient.Value = int.MinValue;
                cmbTradingAccount.Value = int.MinValue;
				if (RefreshClick != null)
				{
					RefreshClick(this,null);
				}
			}
			catch(Exception ex)
			{
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }

			}
			finally
			{
				
			}
		}
		
		public event EventHandler RefreshClick;
		private void btnRefresh_Click(object sender, System.EventArgs e)
		{
			try
			{
				if (RefreshClick != null)
				{
					RefreshClick(this,null);
				}
			}
			catch(Exception ex)
			{
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }

			}
			finally
			{
				
			}
		}
		/// <summary>
		/// This method is called to refresh the grid when values of OrderType, OrderSide, 
		/// Venue, UnderLying, Client and OrderState is changed
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void allCombos_ValueChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (RefreshClick != null)
				{
					RefreshClick(this,null);
				}
			}
			catch(Exception ex)
			{
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }

			}
			finally
			{
				
			}
        }

        private void cmbCounterParty_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
			try
			{
				textBox1.Clear();
				cmbAsset.Value = int.MinValue;
				cmbUnderLying.Value = int.MinValue;
				cmbCounterParty.Value = int.MinValue;
				cmbVenue.Value = int.MinValue;
				cmbSide.Value = int.MinValue;
				//		cmbOrderState.Value = int.MinValue;
				//cmbOrderType.Value = int.MinValue;
				//		cmbClient.Value = int.MinValue;
                cmbTradingAccount.Value = int.MinValue;
				if (RefreshClick != null)
				{
					RefreshClick(this,null);
				}
			}
			catch(Exception ex)
			{
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }

			}
			finally
			{
				
			}
	
        }

        private void cmbTradingAccount_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {

        }

       

        
     

	}
}
