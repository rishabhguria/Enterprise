using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Nirvana.Admin.BLL;
using Nirvana.Admin.Utility;

using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation;
using Microsoft.Practices.EnterpriseLibrary.Logging.Sinks;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tracing;

namespace Nirvana.Admin.Controls
{
	/// <summary>
	/// Summary description for CounterPartyVenueAcceptedOrderTypes.
	/// </summary>
	public class uctCounterPartyVenueAcceptedOrderTypes : System.Windows.Forms.UserControl
	{
		private const string FORM_NAME = "uctCounterPartyVenueAcceptedOrderTypes : ";
		const string C_COMBO_SELECT = "- Select -";

		private System.Windows.Forms.ComboBox cmbAlgos;
		private System.Windows.Forms.ComboBox cmbAdvancedOrders;
		private System.Windows.Forms.ComboBox cmbExecutionInstr;
		private System.Windows.Forms.ComboBox cmbHandlingInstructions;
		private System.Windows.Forms.ComboBox cmbTIF;
		private System.Windows.Forms.ComboBox cmbOrderTypes;
		private System.Windows.Forms.Label label42;
		private System.Windows.Forms.Label label41;
		private System.Windows.Forms.Label label40;
		private System.Windows.Forms.Label label39;
		private System.Windows.Forms.Label label38;
		private System.Windows.Forms.Label label37;
		private System.Windows.Forms.Label label36;
		private System.Windows.Forms.ComboBox cmbSide;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public uctCounterPartyVenueAcceptedOrderTypes()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call
			try
			{
				BindSide();
				BindOrderTypes();
				BindTimeInForce();
				BindHandlingInstructions();
				BindExecutionInstructions();
				BindAdvancedOrders();
			}
			catch(Exception ex)
			{
				string formattedInfo = ex.StackTrace.ToString();
				Logger.Write(formattedInfo, Common.LOG_CATEGORY_EXCEPTION, 1, 1, Severity.Error, 
					FORM_NAME);
				AppMessageExceptionHandler appMessageExceptionHandler = new AppMessageExceptionHandler();
				appMessageExceptionHandler.HandleException(new Exception(Common.ERROR_STATEMENT), Common.POLICY_GLOBAL, System.Guid.NewGuid());
			}
			finally
			{
				#region LogEntry

				LogEntry logEntry = new LogEntry("btnLogin_Click", 
					Common.LOG_CATEGORY_UI, 1, 1, Severity.Information, 
					FORM_NAME + "btnLogin_Click"); 
				Logger.Write(logEntry); 

				#endregion
			}
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
			this.cmbAlgos = new System.Windows.Forms.ComboBox();
			this.cmbAdvancedOrders = new System.Windows.Forms.ComboBox();
			this.cmbExecutionInstr = new System.Windows.Forms.ComboBox();
			this.cmbHandlingInstructions = new System.Windows.Forms.ComboBox();
			this.cmbTIF = new System.Windows.Forms.ComboBox();
			this.cmbOrderTypes = new System.Windows.Forms.ComboBox();
			this.cmbSide = new System.Windows.Forms.ComboBox();
			this.label42 = new System.Windows.Forms.Label();
			this.label41 = new System.Windows.Forms.Label();
			this.label40 = new System.Windows.Forms.Label();
			this.label39 = new System.Windows.Forms.Label();
			this.label38 = new System.Windows.Forms.Label();
			this.label37 = new System.Windows.Forms.Label();
			this.label36 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// cmbAlgos
			// 
			this.cmbAlgos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbAlgos.Enabled = false;
			this.cmbAlgos.Location = new System.Drawing.Point(212, 158);
			this.cmbAlgos.Name = "cmbAlgos";
			this.cmbAlgos.Size = new System.Drawing.Size(136, 21);
			this.cmbAlgos.TabIndex = 27;
			this.cmbAlgos.GotFocus += new System.EventHandler(this.cmbAlgos_GotFocus);
			this.cmbAlgos.LostFocus += new System.EventHandler(this.cmbAlgos_LostFocus);

			// 
			// cmbAdvancedOrders
			// 
			this.cmbAdvancedOrders.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbAdvancedOrders.Location = new System.Drawing.Point(212, 134);
			this.cmbAdvancedOrders.Name = "cmbAdvancedOrders";
			this.cmbAdvancedOrders.Size = new System.Drawing.Size(136, 21);
			this.cmbAdvancedOrders.TabIndex = 26;
			this.cmbAdvancedOrders.GotFocus += new System.EventHandler(this.cmbAdvancedOrders_GotFocus);
			this.cmbAdvancedOrders.LostFocus += new System.EventHandler(this.cmbAdvancedOrders_LostFocus);
			// 
			// cmbExecutionInstr
			// 
			this.cmbExecutionInstr.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbExecutionInstr.Location = new System.Drawing.Point(212, 110);
			this.cmbExecutionInstr.Name = "cmbExecutionInstr";
			this.cmbExecutionInstr.Size = new System.Drawing.Size(136, 21);
			this.cmbExecutionInstr.TabIndex = 25;
			this.cmbExecutionInstr.GotFocus += new System.EventHandler(this.cmbExecutionInstr_GotFocus);
			this.cmbExecutionInstr.LostFocus += new System.EventHandler(this.cmbExecutionInstr_LostFocus);
			// 
			// cmbHandlingInstructions
			// 
			this.cmbHandlingInstructions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbHandlingInstructions.Location = new System.Drawing.Point(212, 86);
			this.cmbHandlingInstructions.Name = "cmbHandlingInstructions";
			this.cmbHandlingInstructions.Size = new System.Drawing.Size(136, 21);
			this.cmbHandlingInstructions.TabIndex = 24;
			this.cmbHandlingInstructions.GotFocus += new System.EventHandler(this.cmbHandlingInstructions_GotFocus);
			this.cmbHandlingInstructions.LostFocus += new System.EventHandler(this.cmbHandlingInstructions_LostFocus);
			// 
			// cmbTIF
			// 
			this.cmbTIF.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbTIF.Location = new System.Drawing.Point(212, 62);
			this.cmbTIF.Name = "cmbTIF";
			this.cmbTIF.Size = new System.Drawing.Size(136, 21);
			this.cmbTIF.TabIndex = 23;
			this.cmbTIF.GotFocus += new System.EventHandler(this.cmbTIF_GotFocus);
			this.cmbTIF.LostFocus += new System.EventHandler(this.cmbTIF_LostFocus);
			// 
			// cmbOrderTypes
			// 
			this.cmbOrderTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbOrderTypes.Location = new System.Drawing.Point(212, 38);
			this.cmbOrderTypes.Name = "cmbOrderTypes";
			this.cmbOrderTypes.Size = new System.Drawing.Size(136, 21);
			this.cmbOrderTypes.TabIndex = 22;
			this.cmbOrderTypes.GotFocus += new System.EventHandler(this.cmbOrderTypes_GotFocus);
			this.cmbOrderTypes.LostFocus += new System.EventHandler(this.cmbOrderTypes_LostFocus);
			// 
			// cmbSide
			// 
			this.cmbSide.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbSide.Location = new System.Drawing.Point(212, 14);
			this.cmbSide.Name = "cmbSide";
			this.cmbSide.Size = new System.Drawing.Size(136, 21);
			this.cmbSide.TabIndex = 21;
			this.cmbSide.SelectedIndexChanged += new System.EventHandler(this.cmbSide_SelectedIndexChanged);
			this.cmbSide.GotFocus += new System.EventHandler(this.cmbSide_GotFocus);
			this.cmbSide.LostFocus += new System.EventHandler(this.cmbSide_LostFocus);
			// 
			// label42
			// 
			this.label42.Location = new System.Drawing.Point(14, 158);
			this.label42.Name = "label42";
			this.label42.Size = new System.Drawing.Size(100, 16);
			this.label42.TabIndex = 20;
			this.label42.Text = "Algos";
			// 
			// label41
			// 
			this.label41.Location = new System.Drawing.Point(14, 134);
			this.label41.Name = "label41";
			this.label41.Size = new System.Drawing.Size(104, 16);
			this.label41.TabIndex = 19;
			this.label41.Text = "Advanced Orders";
			// 
			// label40
			// 
			this.label40.Location = new System.Drawing.Point(14, 110);
			this.label40.Name = "label40";
			this.label40.Size = new System.Drawing.Size(100, 16);
			this.label40.TabIndex = 18;
			this.label40.Text = "Execution Instr";
			// 
			// label39
			// 
			this.label39.Location = new System.Drawing.Point(14, 86);
			this.label39.Name = "label39";
			this.label39.Size = new System.Drawing.Size(136, 16);
			this.label39.TabIndex = 17;
			this.label39.Text = "Handling Instructions";
			// 
			// label38
			// 
			this.label38.Location = new System.Drawing.Point(14, 62);
			this.label38.Name = "label38";
			this.label38.Size = new System.Drawing.Size(100, 16);
			this.label38.TabIndex = 16;
			this.label38.Text = "TIF";
			// 
			// label37
			// 
			this.label37.Location = new System.Drawing.Point(14, 38);
			this.label37.Name = "label37";
			this.label37.Size = new System.Drawing.Size(100, 16);
			this.label37.TabIndex = 15;
			this.label37.Text = "Order Types";
			// 
			// label36
			// 
			this.label36.Location = new System.Drawing.Point(14, 14);
			this.label36.Name = "label36";
			this.label36.Size = new System.Drawing.Size(100, 16);
			this.label36.TabIndex = 14;
			this.label36.Text = "Side";
			// 
			// uctCounterPartyVenueAcceptedOrderTypes
			// 
			this.Controls.Add(this.cmbAlgos);
			this.Controls.Add(this.cmbAdvancedOrders);
			this.Controls.Add(this.cmbExecutionInstr);
			this.Controls.Add(this.cmbHandlingInstructions);
			this.Controls.Add(this.cmbTIF);
			this.Controls.Add(this.cmbOrderTypes);
			this.Controls.Add(this.cmbSide);
			this.Controls.Add(this.label42);
			this.Controls.Add(this.label41);
			this.Controls.Add(this.label40);
			this.Controls.Add(this.label39);
			this.Controls.Add(this.label38);
			this.Controls.Add(this.label37);
			this.Controls.Add(this.label36);
			this.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.Name = "uctCounterPartyVenueAcceptedOrderTypes";
			this.Size = new System.Drawing.Size(360, 192);
			this.Load += new System.EventHandler(this.CounterPartyVenueAcceptedOrderTypes_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private StatusBar _statusBar = null;
		public StatusBar ParentStatusBar
		{
			set{_statusBar = value;}
		}
		
		public CounterPartyVenue CounterPartyProperty
		{
			get 
			{
				CounterPartyVenue counterPartyVenue = new CounterPartyVenue();
				GetCounterPartyVenueAcceptedOrderTypes(counterPartyVenue);
				return counterPartyVenue;
			}
			set {SetCounterPartyVenueAcceptedOrderTypes(value);}
		}

		public void GetCounterPartyVenueAcceptedOrderTypes(CounterPartyVenue counterPartyVenue)
		{			
			counterPartyVenue.SideID = int.Parse(cmbSide.SelectedValue.ToString());
			counterPartyVenue.OrderTypesID = int.Parse(cmbOrderTypes.SelectedValue.ToString());
			counterPartyVenue.TimeInForceID = int.Parse(cmbTIF.SelectedValue.ToString());
			counterPartyVenue.HandlingInstructionsID = int.Parse(cmbHandlingInstructions.SelectedValue.ToString());
			counterPartyVenue.ExecutionInstructionsID = int.Parse(cmbExecutionInstr.SelectedValue.ToString());
			counterPartyVenue.AdvancedOrdersID = int.Parse(cmbAdvancedOrders.SelectedValue.ToString());
		}

		public int GetCounterPartyVenueAcceptedOrderTypesForSave(CounterPartyVenue counterPartyVenue)
		{	
			int result = int.MinValue;

			if(int.Parse(cmbSide.SelectedValue.ToString()) == int.MinValue)
			{
				_statusBar.Text = "Please select Side!";
				cmbSide.Focus();
			}
			else if(int.Parse(cmbOrderTypes.SelectedValue.ToString()) == int.MinValue)
			{
				_statusBar.Text = "Please select OrderTypes!";
				cmbOrderTypes.Focus();
			}
			else if(int.Parse(cmbTIF.SelectedValue.ToString()) == int.MinValue)
			{
				_statusBar.Text = "Please select Time In Force(TIF)!";
				cmbTIF.Focus(); 
			}
			else if(int.Parse(cmbHandlingInstructions.SelectedValue.ToString()) == int.MinValue)
			{
				_statusBar.Text = "Please select Handling Instructions!";
				cmbHandlingInstructions.Focus();
			}
			else if(int.Parse(cmbExecutionInstr.SelectedValue.ToString()) == int.MinValue)
			{
				_statusBar.Text = "Please select Execution Instructions!";
				cmbExecutionInstr.Focus();
			}
			else if(int.Parse(cmbAdvancedOrders.SelectedValue.ToString()) == int.MinValue)
			{
				_statusBar.Text = "Please select Advanced Orders!";
				cmbAdvancedOrders.Focus();
			}
				
			else
			{
				counterPartyVenue.SideID = int.Parse(cmbSide.SelectedValue.ToString());
				counterPartyVenue.OrderTypesID = int.Parse(cmbOrderTypes.SelectedValue.ToString());
				counterPartyVenue.TimeInForceID = int.Parse(cmbTIF.SelectedValue.ToString());
				counterPartyVenue.HandlingInstructionsID = int.Parse(cmbHandlingInstructions.SelectedValue.ToString());
				counterPartyVenue.ExecutionInstructionsID = int.Parse(cmbExecutionInstr.SelectedValue.ToString());
				counterPartyVenue.AdvancedOrdersID = int.Parse(cmbAdvancedOrders.SelectedValue.ToString());
				result = 1;
			}
			return result;
		}

		public void SetCounterPartyVenueAcceptedOrderTypes(CounterPartyVenue counterPartyVenue)
		{
			if(counterPartyVenue != null)
			{
				cmbSide.SelectedValue = counterPartyVenue.SideID;
				cmbOrderTypes.SelectedValue = counterPartyVenue.OrderTypesID;
				cmbTIF.SelectedValue = counterPartyVenue.TimeInForceID;
				cmbHandlingInstructions.SelectedValue = counterPartyVenue.HandlingInstructionsID;
				cmbExecutionInstr.SelectedValue = counterPartyVenue.ExecutionInstructionsID;
				cmbAdvancedOrders.SelectedValue = counterPartyVenue.AdvancedOrdersID;
			}
		}

		private void BindSide()
		{
			Sides sides = OrderManager.GetSides();
			if (sides.Count > 0 )
			{
				sides.Insert(0, new Side(int.MinValue, C_COMBO_SELECT));
				cmbSide.DataSource = sides;				
				cmbSide.DisplayMember = "OrderSide";
				cmbSide.ValueMember = "SideID";
			}
		}

		private void BindOrderTypes()
		{
			OrderTypes orderTypes = OrderManager.GetOrderTypes();
			if (orderTypes.Count > 0 )
			{
				orderTypes.Insert(0, new OrderType(int.MinValue, C_COMBO_SELECT));
				cmbOrderTypes.DataSource = orderTypes;				
				cmbOrderTypes.DisplayMember = "Type";
				cmbOrderTypes.ValueMember = "OrderTypesID";
			}
		}

		private void BindTimeInForce()
		{
			TimeInForces timeInForces = OrderManager.GetTimeInForces();
			if (timeInForces.Count > 0 )
			{
				timeInForces.Insert(0, new TimeInForce(int.MinValue, C_COMBO_SELECT));
				cmbTIF.DataSource = timeInForces;				
				cmbTIF.DisplayMember = "OrderTimeInForce";
				cmbTIF.ValueMember = "TimeInForceID";
			}
		}

		private void BindHandlingInstructions()
		{
			HandlingInstructions handlingInstructions = OrderManager.GetHandlingInstructions();
			if (handlingInstructions.Count > 0 )
			{
				handlingInstructions.Insert(0, new HandlingInstruction(int.MinValue, C_COMBO_SELECT));
				cmbHandlingInstructions.DataSource = handlingInstructions;				
				cmbHandlingInstructions.DisplayMember = "OrderHandlingInstruction";
				cmbHandlingInstructions.ValueMember = "HandlingInstructionID";
			}
		}

		private void BindExecutionInstructions()
		{
			ExecutionInstructions executionInstructions = OrderManager.GetExecutionInstructions();
			if (executionInstructions.Count > 0 )
			{
				executionInstructions.Insert(0, new ExecutionInstruction(int.MinValue, C_COMBO_SELECT));
				cmbExecutionInstr.DataSource = executionInstructions;				
				cmbExecutionInstr.DisplayMember = "ExecutionInstructions";
				cmbExecutionInstr.ValueMember = "ExecutionInstructionsID";
			}
		}

		private void BindAdvancedOrders()
		{
			AdvancedOrders advancedOrders = OrderManager.GetAdvancedOrders();
			if (advancedOrders.Count > 0 )
			{
				advancedOrders.Insert(0, new AdvancedOrder(int.MinValue, C_COMBO_SELECT));
				cmbAdvancedOrders.DataSource = advancedOrders;				
				cmbAdvancedOrders.DisplayMember = "AdvancedOrders";
				cmbAdvancedOrders.ValueMember = "AdvancedOrdersID";
			}
		}

		private void CounterPartyVenueAcceptedOrderTypes_Load(object sender, System.EventArgs e)
		{
			BindSide();
			BindOrderTypes();
			BindTimeInForce();
			BindHandlingInstructions();
			BindExecutionInstructions();
			BindAdvancedOrders();
		}

		private int _counterPartyVenueID = int.MinValue;
		public int CounterPartyVenueID 
		{
			set
			{
				_counterPartyVenueID = value;
			}
		}

//		public int SaveCounterPartyVenues()
//		{
//			int result = int.MinValue;
//			
//			Nirvana.Admin.BLL.CounterPartyVenue counterPartyVenue = new Nirvana.Admin.BLL.CounterPartyVenue();
//			counterPartyVenue.CounterPartyVenueID = 1;
//			counterPartyVenue.SideID = int.Parse(cmbSide.SelectedValue.ToString());
//			counterPartyVenue.OrderTypesID = int.Parse(cmbOrderTypes.SelectedValue.ToString());
//			counterPartyVenue.TimeInForceID = int.Parse(cmbTIF.SelectedValue.ToString());
//			counterPartyVenue.HandlingInstructionsID = int.Parse(cmbHandlingInstructions.SelectedValue.ToString());
//			counterPartyVenue.ExecutionInstructionsID = int.Parse(cmbExecutionInstr.SelectedValue.ToString());
//			counterPartyVenue.AdvancedOrdersID = int.Parse(cmbAdvancedOrders.SelectedValue.ToString());
//			int counterPartyVenueDetailsID = CounterPartyManager.SaveCounterPartyVenue(counterPartyVenue);
//			if(counterPartyVenueDetailsID == -1)
//			{
//				_statusBar.Text = "Counter Party Venue Details already exists.";
//			}	
//			else
//			{
//				_statusBar.Text = "Stored!!.";
//			}
//			result = counterPartyVenueDetailsID;
//								
//			return result;			
//		}
		
		private void cmbSide_SelectedIndexChanged(object sender, System.EventArgs e)
		{
		
		}

# region Controls Focus Colors
		
		private void cmbAdvancedOrders_GotFocus(object sender, System.EventArgs e)
		{
			cmbAdvancedOrders.BackColor = Color.LemonChiffon;
		}
		private void cmbAdvancedOrders_LostFocus(object sender, System.EventArgs e)
		{
			cmbAdvancedOrders.BackColor = Color.White;
		}
		private void cmbAlgos_GotFocus(object sender, System.EventArgs e)
		{
			cmbAlgos.BackColor = Color.LemonChiffon;
		}
		private void cmbAlgos_LostFocus(object sender, System.EventArgs e)
		{
			cmbAlgos.BackColor = Color.White;
		}
		private void cmbExecutionInstr_GotFocus(object sender, System.EventArgs e)
		{
			cmbExecutionInstr.BackColor = Color.LemonChiffon;
		}
		private void cmbExecutionInstr_LostFocus(object sender, System.EventArgs e)
		{
			cmbExecutionInstr.BackColor = Color.White;
		}
		private void cmbHandlingInstructions_GotFocus(object sender, System.EventArgs e)
		{
			cmbHandlingInstructions.BackColor = Color.LemonChiffon;
		}
		private void cmbHandlingInstructions_LostFocus(object sender, System.EventArgs e)
		{
			cmbHandlingInstructions.BackColor = Color.White;
		}
		private void cmbOrderTypes_GotFocus(object sender, System.EventArgs e)
		{
			cmbOrderTypes.BackColor = Color.LemonChiffon;
		}
		private void cmbOrderTypes_LostFocus(object sender, System.EventArgs e)
		{
			cmbOrderTypes.BackColor = Color.White;
        }
		private void cmbSide_GotFocus(object sender, System.EventArgs e)
		{
			cmbSide.BackColor = Color.LemonChiffon;
		}
		private void cmbSide_LostFocus(object sender, System.EventArgs e)
		{
			cmbSide.BackColor = Color.White;
		}
		private void cmbTIF_GotFocus(object sender, System.EventArgs e)
		{
			cmbTIF.BackColor = Color.LemonChiffon;
		}
		private void cmbTIF_LostFocus(object sender, System.EventArgs e)
		{
			cmbTIF.BackColor = Color.White;
		}
		#endregion

	}
}
