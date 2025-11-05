//using System;
//using System.Collections;
//using System.ComponentModel;
//using System.Drawing;
//using System.Data;
//using System.Windows.Forms;
//using Nirvana.Admin.BLL;
//using Nirvana.Admin.Utility;
//
//using Microsoft.Practices.EnterpriseLibrary.Logging;
//using Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation;
//using Microsoft.Practices.EnterpriseLibrary.Logging.Sinks;
//using Microsoft.Practices.EnterpriseLibrary.Logging.Tracing;
//
//namespace Nirvana.Admin.Controls
//{
//	/// <summary>
//	/// Summary description for CounterPartyVenueAcceptedOrderTypes.
//	/// </summary>
//	public class uctCounterPartyVenueAcceptedOrderTypes : System.Windows.Forms.UserControl
//	{
//		private const string FORM_NAME = "uctCounterPartyVenueAcceptedOrderTypes : ";
//		const string C_COMBO_SELECT = "- Select -";
//
//		private System.Windows.Forms.ComboBox cmbAlgos;
//		private System.Windows.Forms.ComboBox cmbAdvancedOrders;
//		private System.Windows.Forms.ComboBox cmbExecutionInstr;
//		private System.Windows.Forms.ComboBox cmbHandlingInstructions;
//		private System.Windows.Forms.ComboBox cmbTIF;
//		private System.Windows.Forms.ComboBox cmbOrderTypes;
//		private System.Windows.Forms.Label label42;
//		private System.Windows.Forms.Label label41;
//		private System.Windows.Forms.Label label40;
//		private System.Windows.Forms.Label label39;
//		private System.Windows.Forms.Label label38;
//		private System.Windows.Forms.Label label37;
//		private System.Windows.Forms.Label label36;
//		private System.Windows.Forms.ComboBox cmbSide;
//		private System.Windows.Forms.GroupBox groupBox1;
//		private System.Windows.Forms.Label label3;
//		private System.Windows.Forms.Label label5;
//		private System.Windows.Forms.Label label1;
//		private System.Windows.Forms.Label label2;
//		private System.Windows.Forms.Label label4;
//		private System.Windows.Forms.Label label6;
//		private System.Windows.Forms.Label label7;
//		private System.Windows.Forms.Label label8;
//		private System.Windows.Forms.ErrorProvider errorProvider1;
//		/// <summary> 
//		/// Required designer variable.
//		/// </summary>
//		private System.ComponentModel.Container components = null;
//
//		public uctCounterPartyVenueAcceptedOrderTypes()
//		{
//			// This call is required by the Windows.Forms Form Designer.
//			InitializeComponent();
//
//			
//			try
//			{
//				BindSide();
//				BindOrderTypes();
//				BindTimeInForce();
//				BindHandlingInstructions();
//				BindExecutionInstructions();
//				BindAdvancedOrders();
//			}
//			catch(Exception ex)
//			{
//				string formattedInfo = ex.StackTrace.ToString();
//				Logger.Write(formattedInfo, Common.LOG_CATEGORY_EXCEPTION, 1, 1, Severity.Error, 
//					FORM_NAME);
//				AppMessageExceptionHandler appMessageExceptionHandler = new AppMessageExceptionHandler();
//				appMessageExceptionHandler.HandleException(new Exception(Common.ERROR_STATEMENT), Common.POLICY_GLOBAL, System.Guid.NewGuid());
//			}
//			finally
//			{
//				#region LogEntry
//
//				LogEntry logEntry = new LogEntry("btnLogin_Click", 
//					Common.LOG_CATEGORY_UI, 1, 1, Severity.Information, 
//					FORM_NAME + "btnLogin_Click"); 
//				Logger.Write(logEntry); 
//
//				#endregion
//			}
//		}
//
//		/// <summary> 
//		/// Clean up any resources being used.
//		/// </summary>
//		protected override void Dispose( bool disposing )
//		{
//			if( disposing )
//			{
//				if(components != null)
//				{
//					components.Dispose();
//				}
//			}
//			base.Dispose( disposing );
//		}
//
//		#region Component Designer generated code
//		/// <summary> 
//		/// Required method for Designer support - do not modify 
//		/// the contents of this method with the code editor.
//		/// </summary>
//		private void InitializeComponent()
//		{
//			this.cmbAlgos = new System.Windows.Forms.ComboBox();
//			this.cmbAdvancedOrders = new System.Windows.Forms.ComboBox();
//			this.cmbExecutionInstr = new System.Windows.Forms.ComboBox();
//			this.cmbHandlingInstructions = new System.Windows.Forms.ComboBox();
//			this.cmbTIF = new System.Windows.Forms.ComboBox();
//			this.cmbOrderTypes = new System.Windows.Forms.ComboBox();
//			this.cmbSide = new System.Windows.Forms.ComboBox();
//			this.label42 = new System.Windows.Forms.Label();
//			this.label41 = new System.Windows.Forms.Label();
//			this.label40 = new System.Windows.Forms.Label();
//			this.label39 = new System.Windows.Forms.Label();
//			this.label38 = new System.Windows.Forms.Label();
//			this.label37 = new System.Windows.Forms.Label();
//			this.label36 = new System.Windows.Forms.Label();
//			this.groupBox1 = new System.Windows.Forms.GroupBox();
//			this.label8 = new System.Windows.Forms.Label();
//			this.label7 = new System.Windows.Forms.Label();
//			this.label6 = new System.Windows.Forms.Label();
//			this.label4 = new System.Windows.Forms.Label();
//			this.label2 = new System.Windows.Forms.Label();
//			this.label1 = new System.Windows.Forms.Label();
//			this.label5 = new System.Windows.Forms.Label();
//			this.label3 = new System.Windows.Forms.Label();
//			this.errorProvider1 = new System.Windows.Forms.ErrorProvider();
//			this.groupBox1.SuspendLayout();
//			this.SuspendLayout();
//			// 
//			// cmbAlgos
//			// 
//			this.cmbAlgos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
//			this.cmbAlgos.Enabled = false;
//			this.cmbAlgos.Font = new System.Drawing.Font("Verdana", 8.25F);
//			this.cmbAlgos.Location = new System.Drawing.Point(210, 166);
//			this.cmbAlgos.Name = "cmbAlgos";
//			this.cmbAlgos.Size = new System.Drawing.Size(136, 21);
//			this.cmbAlgos.TabIndex = 27;
//			this.cmbAlgos.GotFocus += new System.EventHandler(this.cmbAlgos_GotFocus);
//			this.cmbAlgos.LostFocus += new System.EventHandler(this.cmbAlgos_LostFocus);
//			// 
//			// cmbAdvancedOrders
//			// 
//			this.cmbAdvancedOrders.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
//			this.cmbAdvancedOrders.Font = new System.Drawing.Font("Verdana", 8.25F);
//			this.cmbAdvancedOrders.Location = new System.Drawing.Point(210, 142);
//			this.cmbAdvancedOrders.Name = "cmbAdvancedOrders";
//			this.cmbAdvancedOrders.Size = new System.Drawing.Size(136, 21);
//			this.cmbAdvancedOrders.TabIndex = 26;
//			this.cmbAdvancedOrders.GotFocus += new System.EventHandler(this.cmbAdvancedOrders_GotFocus);
//			this.cmbAdvancedOrders.LostFocus += new System.EventHandler(this.cmbAdvancedOrders_LostFocus);
//			// 
//			// cmbExecutionInstr
//			// 
//			this.cmbExecutionInstr.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
//			this.cmbExecutionInstr.Font = new System.Drawing.Font("Verdana", 8.25F);
//			this.cmbExecutionInstr.Location = new System.Drawing.Point(210, 118);
//			this.cmbExecutionInstr.Name = "cmbExecutionInstr";
//			this.cmbExecutionInstr.Size = new System.Drawing.Size(136, 21);
//			this.cmbExecutionInstr.TabIndex = 25;
//			this.cmbExecutionInstr.GotFocus += new System.EventHandler(this.cmbExecutionInstr_GotFocus);
//			this.cmbExecutionInstr.LostFocus += new System.EventHandler(this.cmbExecutionInstr_LostFocus);
//			// 
//			// cmbHandlingInstructions
//			// 
//			this.cmbHandlingInstructions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
//			this.cmbHandlingInstructions.Font = new System.Drawing.Font("Verdana", 8.25F);
//			this.cmbHandlingInstructions.Location = new System.Drawing.Point(210, 94);
//			this.cmbHandlingInstructions.Name = "cmbHandlingInstructions";
//			this.cmbHandlingInstructions.Size = new System.Drawing.Size(136, 21);
//			this.cmbHandlingInstructions.TabIndex = 24;
//			this.cmbHandlingInstructions.GotFocus += new System.EventHandler(this.cmbHandlingInstructions_GotFocus);
//			this.cmbHandlingInstructions.LostFocus += new System.EventHandler(this.cmbHandlingInstructions_LostFocus);
//			// 
//			// cmbTIF
//			// 
//			this.cmbTIF.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
//			this.cmbTIF.Font = new System.Drawing.Font("Verdana", 8.25F);
//			this.cmbTIF.Location = new System.Drawing.Point(210, 70);
//			this.cmbTIF.Name = "cmbTIF";
//			this.cmbTIF.Size = new System.Drawing.Size(136, 21);
//			this.cmbTIF.TabIndex = 23;
//			this.cmbTIF.GotFocus += new System.EventHandler(this.cmbTIF_GotFocus);
//			this.cmbTIF.LostFocus += new System.EventHandler(this.cmbTIF_LostFocus);
//			// 
//			// cmbOrderTypes
//			// 
//			this.cmbOrderTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
//			this.cmbOrderTypes.Font = new System.Drawing.Font("Verdana", 8.25F);
//			this.cmbOrderTypes.Location = new System.Drawing.Point(210, 46);
//			this.cmbOrderTypes.Name = "cmbOrderTypes";
//			this.cmbOrderTypes.Size = new System.Drawing.Size(136, 21);
//			this.cmbOrderTypes.TabIndex = 22;
//			this.cmbOrderTypes.GotFocus += new System.EventHandler(this.cmbOrderTypes_GotFocus);
//			this.cmbOrderTypes.LostFocus += new System.EventHandler(this.cmbOrderTypes_LostFocus);
//			// 
//			// cmbSide
//			// 
//			this.cmbSide.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
//			this.cmbSide.Font = new System.Drawing.Font("Verdana", 8.25F);
//			this.cmbSide.Location = new System.Drawing.Point(210, 24);
//			this.cmbSide.Name = "cmbSide";
//			this.cmbSide.Size = new System.Drawing.Size(136, 21);
//			this.cmbSide.TabIndex = 21;
//			this.cmbSide.GotFocus += new System.EventHandler(this.cmbSide_GotFocus);
//			this.cmbSide.SelectedIndexChanged += new System.EventHandler(this.cmbSide_SelectedIndexChanged);
//			this.cmbSide.LostFocus += new System.EventHandler(this.cmbSide_LostFocus);
//			// 
//			// label42
//			// 
//			this.label42.Font = new System.Drawing.Font("Verdana", 8.25F);
//			this.label42.Location = new System.Drawing.Point(12, 172);
//			this.label42.Name = "label42";
//			this.label42.Size = new System.Drawing.Size(100, 16);
//			this.label42.TabIndex = 20;
//			this.label42.Text = "Algos";
//			// 
//			// label41
//			// 
//			this.label41.Font = new System.Drawing.Font("Verdana", 8.25F);
//			this.label41.Location = new System.Drawing.Point(12, 148);
//			this.label41.Name = "label41";
//			this.label41.Size = new System.Drawing.Size(104, 16);
//			this.label41.TabIndex = 19;
//			this.label41.Text = "Advanced Orders";
//			// 
//			// label40
//			// 
//			this.label40.Font = new System.Drawing.Font("Verdana", 8.25F);
//			this.label40.Location = new System.Drawing.Point(12, 124);
//			this.label40.Name = "label40";
//			this.label40.Size = new System.Drawing.Size(100, 16);
//			this.label40.TabIndex = 18;
//			this.label40.Text = "Execution Instr";
//			// 
//			// label39
//			// 
//			this.label39.Font = new System.Drawing.Font("Verdana", 8.25F);
//			this.label39.Location = new System.Drawing.Point(12, 100);
//			this.label39.Name = "label39";
//			this.label39.Size = new System.Drawing.Size(136, 16);
//			this.label39.TabIndex = 17;
//			this.label39.Text = "Handling Instructions";
//			// 
//			// label38
//			// 
//			this.label38.Font = new System.Drawing.Font("Verdana", 8.25F);
//			this.label38.Location = new System.Drawing.Point(12, 76);
//			this.label38.Name = "label38";
//			this.label38.Size = new System.Drawing.Size(100, 16);
//			this.label38.TabIndex = 16;
//			this.label38.Text = "TIF";
//			// 
//			// label37
//			// 
//			this.label37.Font = new System.Drawing.Font("Verdana", 8.25F);
//			this.label37.Location = new System.Drawing.Point(12, 52);
//			this.label37.Name = "label37";
//			this.label37.Size = new System.Drawing.Size(100, 16);
//			this.label37.TabIndex = 15;
//			this.label37.Text = "Order Types";
//			// 
//			// label36
//			// 
//			this.label36.Font = new System.Drawing.Font("Verdana", 8.25F);
//			this.label36.Location = new System.Drawing.Point(12, 28);
//			this.label36.Name = "label36";
//			this.label36.Size = new System.Drawing.Size(100, 16);
//			this.label36.TabIndex = 14;
//			this.label36.Text = "Side";
//			// 
//			// groupBox1
//			// 
//			this.groupBox1.Controls.Add(this.label8);
//			this.groupBox1.Controls.Add(this.label7);
//			this.groupBox1.Controls.Add(this.label6);
//			this.groupBox1.Controls.Add(this.label4);
//			this.groupBox1.Controls.Add(this.label2);
//			this.groupBox1.Controls.Add(this.label1);
//			this.groupBox1.Controls.Add(this.label5);
//			this.groupBox1.Controls.Add(this.label3);
//			this.groupBox1.Controls.Add(this.cmbOrderTypes);
//			this.groupBox1.Controls.Add(this.cmbAdvancedOrders);
//			this.groupBox1.Controls.Add(this.cmbExecutionInstr);
//			this.groupBox1.Controls.Add(this.label42);
//			this.groupBox1.Controls.Add(this.label41);
//			this.groupBox1.Controls.Add(this.label40);
//			this.groupBox1.Controls.Add(this.label39);
//			this.groupBox1.Controls.Add(this.label37);
//			this.groupBox1.Controls.Add(this.label36);
//			this.groupBox1.Controls.Add(this.cmbHandlingInstructions);
//			this.groupBox1.Controls.Add(this.cmbSide);
//			this.groupBox1.Controls.Add(this.label38);
//			this.groupBox1.Controls.Add(this.cmbAlgos);
//			this.groupBox1.Controls.Add(this.cmbTIF);
//			this.groupBox1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
//			this.groupBox1.Location = new System.Drawing.Point(4, 4);
//			this.groupBox1.Name = "groupBox1";
//			this.groupBox1.Size = new System.Drawing.Size(374, 222);
//			this.groupBox1.TabIndex = 28;
//			this.groupBox1.TabStop = false;
//			this.groupBox1.Text = "Counter Party Venue Accepted Order Types";
//			// 
//			// label8
//			// 
//			this.label8.ForeColor = System.Drawing.Color.Red;
//			this.label8.Location = new System.Drawing.Point(198, 168);
//			this.label8.Name = "label8";
//			this.label8.Size = new System.Drawing.Size(12, 14);
//			this.label8.TabIndex = 43;
//			this.label8.Text = "*";
//			// 
//			// label7
//			// 
//			this.label7.ForeColor = System.Drawing.Color.Red;
//			this.label7.Location = new System.Drawing.Point(198, 144);
//			this.label7.Name = "label7";
//			this.label7.Size = new System.Drawing.Size(12, 14);
//			this.label7.TabIndex = 42;
//			this.label7.Text = "*";
//			// 
//			// label6
//			// 
//			this.label6.ForeColor = System.Drawing.Color.Red;
//			this.label6.Location = new System.Drawing.Point(198, 120);
//			this.label6.Name = "label6";
//			this.label6.Size = new System.Drawing.Size(12, 14);
//			this.label6.TabIndex = 41;
//			this.label6.Text = "*";
//			// 
//			// label4
//			// 
//			this.label4.ForeColor = System.Drawing.Color.Red;
//			this.label4.Location = new System.Drawing.Point(198, 26);
//			this.label4.Name = "label4";
//			this.label4.Size = new System.Drawing.Size(12, 14);
//			this.label4.TabIndex = 40;
//			this.label4.Text = "*";
//			// 
//			// label2
//			// 
//			this.label2.ForeColor = System.Drawing.Color.Red;
//			this.label2.Location = new System.Drawing.Point(198, 48);
//			this.label2.Name = "label2";
//			this.label2.Size = new System.Drawing.Size(12, 14);
//			this.label2.TabIndex = 39;
//			this.label2.Text = "*";
//			// 
//			// label1
//			// 
//			this.label1.ForeColor = System.Drawing.Color.Red;
//			this.label1.Location = new System.Drawing.Point(198, 72);
//			this.label1.Name = "label1";
//			this.label1.Size = new System.Drawing.Size(12, 14);
//			this.label1.TabIndex = 38;
//			this.label1.Text = "*";
//			// 
//			// label5
//			// 
//			this.label5.ForeColor = System.Drawing.Color.Red;
//			this.label5.Location = new System.Drawing.Point(198, 96);
//			this.label5.Name = "label5";
//			this.label5.Size = new System.Drawing.Size(12, 14);
//			this.label5.TabIndex = 37;
//			this.label5.Text = "*";
//			// 
//			// label3
//			// 
//			this.label3.Font = new System.Drawing.Font("Verdana", 8.25F);
//			this.label3.ForeColor = System.Drawing.Color.Red;
//			this.label3.Location = new System.Drawing.Point(12, 206);
//			this.label3.Name = "label3";
//			this.label3.Size = new System.Drawing.Size(110, 14);
//			this.label3.TabIndex = 34;
//			this.label3.Text = "* Required Field";
//			// 
//			// errorProvider1
//			// 
//			this.errorProvider1.ContainerControl = this;
//			// 
//			// uctCounterPartyVenueAcceptedOrderTypes
//			// 
//			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
//			this.Controls.Add(this.groupBox1);
//			this.Font = new System.Drawing.Font("Verdana", 8.25F);
//			this.Name = "uctCounterPartyVenueAcceptedOrderTypes";
//			this.Size = new System.Drawing.Size(384, 230);
//			this.Load += new System.EventHandler(this.CounterPartyVenueAcceptedOrderTypes_Load);
//			this.groupBox1.ResumeLayout(false);
//			this.ResumeLayout(false);
//
//		}
//		#endregion
//
//		private StatusBar _statusBar = null;
//		public StatusBar ParentStatusBar
//		{
//			set{_statusBar = value;}
//		}
//		
//		public CounterPartyVenue CounterPartyProperty
//		{
//			get 
//			{
//				CounterPartyVenue counterPartyVenue = new CounterPartyVenue();
//				GetCounterPartyVenueAcceptedOrderTypes(counterPartyVenue);
//				return counterPartyVenue;
//			}
//			set {SetCounterPartyVenueAcceptedOrderTypes(value);}
//		}
//
//		public void GetCounterPartyVenueAcceptedOrderTypes(CounterPartyVenue counterPartyVenue)
//		{			
//			counterPartyVenue.SideID = int.Parse(cmbSide.SelectedValue.ToString());
//			counterPartyVenue.OrderTypesID = int.Parse(cmbOrderTypes.SelectedValue.ToString());
//			counterPartyVenue.TimeInForceID = int.Parse(cmbTIF.SelectedValue.ToString());
//			counterPartyVenue.HandlingInstructionsID = int.Parse(cmbHandlingInstructions.SelectedValue.ToString());
//			counterPartyVenue.ExecutionInstructionsID = int.Parse(cmbExecutionInstr.SelectedValue.ToString());
//			counterPartyVenue.AdvancedOrdersID = int.Parse(cmbAdvancedOrders.SelectedValue.ToString());
//		}
//
//		public int GetCounterPartyVenueAcceptedOrderTypesForSave(CounterPartyVenue counterPartyVenue)
//		{	
//			int result = int.MinValue;
//
//			errorProvider1.SetError(cmbSide, "");
//			errorProvider1.SetError(cmbOrderTypes, "");
//			errorProvider1.SetError(cmbHandlingInstructions, "");
//			errorProvider1.SetError(cmbAdvancedOrders, "");
//			errorProvider1.SetError(cmbTIF, "");
//			errorProvider1.SetError(cmbExecutionInstr, "");
//			if(int.Parse(cmbSide.SelectedValue.ToString()) == int.MinValue)
//			{
//				errorProvider1.SetError(cmbSide, "Please select Side!");
//				cmbSide.Focus();
//			}
//			else if(int.Parse(cmbOrderTypes.SelectedValue.ToString()) == int.MinValue)
//			{
//				errorProvider1.SetError(cmbOrderTypes, "Please select OrderTypes!");
//				cmbOrderTypes.Focus();
//			}
//			else if(int.Parse(cmbTIF.SelectedValue.ToString()) == int.MinValue)
//			{
//				errorProvider1.SetError(cmbTIF, "Please select Time In Force(TIF)!");
//				cmbTIF.Focus(); 
//			}
//			else if(int.Parse(cmbHandlingInstructions.SelectedValue.ToString()) == int.MinValue)
//			{
//				errorProvider1.SetError(cmbHandlingInstructions, "Please select Handling Instructions!");
//				cmbHandlingInstructions.Focus();
//			}
//			else if(int.Parse(cmbExecutionInstr.SelectedValue.ToString()) == int.MinValue)
//			{
//				errorProvider1.SetError(cmbExecutionInstr, "Please select Execution Instructions!");
//				cmbExecutionInstr.Focus();
//			}
//			else if(int.Parse(cmbAdvancedOrders.SelectedValue.ToString()) == int.MinValue)
//			{
//				errorProvider1.SetError(cmbAdvancedOrders, "Please select Advanced Orders!");
//				cmbAdvancedOrders.Focus();
//			}
//				
//			else
//			{
//				counterPartyVenue.SideID = int.Parse(cmbSide.SelectedValue.ToString());
//				counterPartyVenue.OrderTypesID = int.Parse(cmbOrderTypes.SelectedValue.ToString());
//				counterPartyVenue.TimeInForceID = int.Parse(cmbTIF.SelectedValue.ToString());
//				counterPartyVenue.HandlingInstructionsID = int.Parse(cmbHandlingInstructions.SelectedValue.ToString());
//				counterPartyVenue.ExecutionInstructionsID = int.Parse(cmbExecutionInstr.SelectedValue.ToString());
//				counterPartyVenue.AdvancedOrdersID = int.Parse(cmbAdvancedOrders.SelectedValue.ToString());
//				result = 1;
//			}
//			return result;
//		}
//
//		public void SetCounterPartyVenueAcceptedOrderTypes(CounterPartyVenue counterPartyVenue)
//		{
//			if(counterPartyVenue != null)
//			{
//				cmbSide.SelectedValue = counterPartyVenue.SideID;
//				cmbOrderTypes.SelectedValue = counterPartyVenue.OrderTypesID;
//				cmbTIF.SelectedValue = counterPartyVenue.TimeInForceID;
//				cmbHandlingInstructions.SelectedValue = counterPartyVenue.HandlingInstructionsID;
//				cmbExecutionInstr.SelectedValue = counterPartyVenue.ExecutionInstructionsID;
//				cmbAdvancedOrders.SelectedValue = counterPartyVenue.AdvancedOrdersID;
//			}
//		}
//
//		private void BindSide()
//		{
//			Sides sides = OrderManager.GetSides();
//			if (sides.Count > 0 )
//			{
//				sides.Insert(0, new Side(int.MinValue, C_COMBO_SELECT));
//				cmbSide.DataSource = sides;				
//				cmbSide.DisplayMember = "OrderSide";
//				cmbSide.ValueMember = "SideID";
//			}
//		}
//
//		private void BindOrderTypes()
//		{
//			OrderTypes orderTypes = OrderManager.GetOrderTypes();
//			if (orderTypes.Count > 0 )
//			{
//				orderTypes.Insert(0, new OrderType(int.MinValue, C_COMBO_SELECT));
//				cmbOrderTypes.DataSource = orderTypes;				
//				cmbOrderTypes.DisplayMember = "Type";
//				cmbOrderTypes.ValueMember = "OrderTypesID";
//			}
//		}
//
//		private void BindTimeInForce()
//		{
//			TimeInForces timeInForces = OrderManager.GetTimeInForces();
//			if (timeInForces.Count > 0 )
//			{
//				timeInForces.Insert(0, new TimeInForce(int.MinValue, C_COMBO_SELECT));
//				cmbTIF.DataSource = timeInForces;				
//				cmbTIF.DisplayMember = "OrderTimeInForce";
//				cmbTIF.ValueMember = "TimeInForceID";
//			}
//		}
//
//		private void BindHandlingInstructions()
//		{
//			HandlingInstructions handlingInstructions = OrderManager.GetHandlingInstructions();
//			if (handlingInstructions.Count > 0 )
//			{
//				handlingInstructions.Insert(0, new HandlingInstruction(int.MinValue, C_COMBO_SELECT));
//				cmbHandlingInstructions.DataSource = handlingInstructions;				
//				cmbHandlingInstructions.DisplayMember = "OrderHandlingInstruction";
//				cmbHandlingInstructions.ValueMember = "HandlingInstructionID";
//			}
//		}
//
//		private void BindExecutionInstructions()
//		{
//			ExecutionInstructions executionInstructions = OrderManager.GetExecutionInstructions();
//			if (executionInstructions.Count > 0 )
//			{
//				executionInstructions.Insert(0, new ExecutionInstruction(int.MinValue, C_COMBO_SELECT));
//				cmbExecutionInstr.DataSource = executionInstructions;				
//				cmbExecutionInstr.DisplayMember = "ExecutionInstructions";
//				cmbExecutionInstr.ValueMember = "ExecutionInstructionsID";
//			}
//		}
//
//		private void BindAdvancedOrders()
//		{
//			AdvancedOrders advancedOrders = OrderManager.GetAdvancedOrders();
//			if (advancedOrders.Count > 0 )
//			{
//				advancedOrders.Insert(0, new AdvancedOrder(int.MinValue, C_COMBO_SELECT));
//				cmbAdvancedOrders.DataSource = advancedOrders;				
//				cmbAdvancedOrders.DisplayMember = "AdvancedOrders";
//				cmbAdvancedOrders.ValueMember = "AdvancedOrdersID";
//			}
//		}
//
//		private void CounterPartyVenueAcceptedOrderTypes_Load(object sender, System.EventArgs e)
//		{
//			BindSide();
//			BindOrderTypes();
//			BindTimeInForce();
//			BindHandlingInstructions();
//			BindExecutionInstructions();
//			BindAdvancedOrders();
//		}
//
//		private int _counterPartyVenueID = int.MinValue;
//		public int CounterPartyVenueID 
//		{
//			set
//			{
//				_counterPartyVenueID = value;
//			}
//		}
//
////		public int SaveCounterPartyVenues()
////		{
////			int result = int.MinValue;
////			
////			Nirvana.Admin.BLL.CounterPartyVenue counterPartyVenue = new Nirvana.Admin.BLL.CounterPartyVenue();
////			counterPartyVenue.CounterPartyVenueID = 1;
////			counterPartyVenue.SideID = int.Parse(cmbSide.SelectedValue.ToString());
////			counterPartyVenue.OrderTypesID = int.Parse(cmbOrderTypes.SelectedValue.ToString());
////			counterPartyVenue.TimeInForceID = int.Parse(cmbTIF.SelectedValue.ToString());
////			counterPartyVenue.HandlingInstructionsID = int.Parse(cmbHandlingInstructions.SelectedValue.ToString());
////			counterPartyVenue.ExecutionInstructionsID = int.Parse(cmbExecutionInstr.SelectedValue.ToString());
////			counterPartyVenue.AdvancedOrdersID = int.Parse(cmbAdvancedOrders.SelectedValue.ToString());
////			int counterPartyVenueDetailsID = CounterPartyManager.SaveCounterPartyVenue(counterPartyVenue);
////			if(counterPartyVenueDetailsID == -1)
////			{
////				_statusBar.Text = "Counter Party Venue Details already exists.";
////			}	
////			else
////			{
////				_statusBar.Text = "Stored!!.";
////			}
////			result = counterPartyVenueDetailsID;
////								
////			return result;			
////		}
//		
//		private void cmbSide_SelectedIndexChanged(object sender, System.EventArgs e)
//		{
//		
//		}
//
//# region Controls Focus Colors
//		
//		private void cmbAdvancedOrders_GotFocus(object sender, System.EventArgs e)
//		{
//			cmbAdvancedOrders.BackColor = Color.LemonChiffon;
//		}
//		private void cmbAdvancedOrders_LostFocus(object sender, System.EventArgs e)
//		{
//			cmbAdvancedOrders.BackColor = Color.White;
//		}
//		private void cmbAlgos_GotFocus(object sender, System.EventArgs e)
//		{
//			cmbAlgos.BackColor = Color.LemonChiffon;
//		}
//		private void cmbAlgos_LostFocus(object sender, System.EventArgs e)
//		{
//			cmbAlgos.BackColor = Color.White;
//		}
//		private void cmbExecutionInstr_GotFocus(object sender, System.EventArgs e)
//		{
//			cmbExecutionInstr.BackColor = Color.LemonChiffon;
//		}
//		private void cmbExecutionInstr_LostFocus(object sender, System.EventArgs e)
//		{
//			cmbExecutionInstr.BackColor = Color.White;
//		}
//		private void cmbHandlingInstructions_GotFocus(object sender, System.EventArgs e)
//		{
//			cmbHandlingInstructions.BackColor = Color.LemonChiffon;
//		}
//		private void cmbHandlingInstructions_LostFocus(object sender, System.EventArgs e)
//		{
//			cmbHandlingInstructions.BackColor = Color.White;
//		}
//		private void cmbOrderTypes_GotFocus(object sender, System.EventArgs e)
//		{
//			cmbOrderTypes.BackColor = Color.LemonChiffon;
//		}
//		private void cmbOrderTypes_LostFocus(object sender, System.EventArgs e)
//		{
//			cmbOrderTypes.BackColor = Color.White;
//        }
//		private void cmbSide_GotFocus(object sender, System.EventArgs e)
//		{
//			cmbSide.BackColor = Color.LemonChiffon;
//		}
//		private void cmbSide_LostFocus(object sender, System.EventArgs e)
//		{
//			cmbSide.BackColor = Color.White;
//		}
//		private void cmbTIF_GotFocus(object sender, System.EventArgs e)
//		{
//			cmbTIF.BackColor = Color.LemonChiffon;
//		}
//		private void cmbTIF_LostFocus(object sender, System.EventArgs e)
//		{
//			cmbTIF.BackColor = Color.White;
//		}
//		#endregion
//
//	}
//}
