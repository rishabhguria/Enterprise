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
	/// Summary description for CounterPartyVenueVenues.
	/// </summary>
	public class CounterPartyVenueVenues : System.Windows.Forms.UserControl
	{
		private const string FORM_NAME = "CounterPartyVenueVenues : ";
		const string C_COMBO_SELECT = "- Select -";
		
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.ComboBox cmbVenueType;
		private System.Windows.Forms.TextBox txtVenueName;
		private System.Windows.Forms.Label label46;
		private System.Windows.Forms.Label label45;
		private System.Windows.Forms.TextBox txtFullNameRoute;
		private System.Windows.Forms.Label label19;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label3;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public CounterPartyVenueVenues()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
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
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.cmbVenueType = new System.Windows.Forms.ComboBox();
			this.txtVenueName = new System.Windows.Forms.TextBox();
			this.label46 = new System.Windows.Forms.Label();
			this.label45 = new System.Windows.Forms.Label();
			this.txtFullNameRoute = new System.Windows.Forms.TextBox();
			this.label19 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.groupBox3.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.cmbVenueType);
			this.groupBox3.Controls.Add(this.txtVenueName);
			this.groupBox3.Controls.Add(this.label46);
			this.groupBox3.Controls.Add(this.label45);
			this.groupBox3.Controls.Add(this.txtFullNameRoute);
			this.groupBox3.Controls.Add(this.label19);
			this.groupBox3.Controls.Add(this.label2);
			this.groupBox3.Controls.Add(this.label1);
			this.groupBox3.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.groupBox3.Location = new System.Drawing.Point(4, 0);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(334, 116);
			this.groupBox3.TabIndex = 1;
			this.groupBox3.TabStop = false;
			// 
			// cmbVenueType
			// 
			this.cmbVenueType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbVenueType.Location = new System.Drawing.Point(150, 38);
			this.cmbVenueType.Name = "cmbVenueType";
			this.cmbVenueType.Size = new System.Drawing.Size(152, 21);
			this.cmbVenueType.TabIndex = 5;
			this.cmbVenueType.GotFocus += new System.EventHandler(this.cmbVenueType_GotFocus);
			this.cmbVenueType.LostFocus += new System.EventHandler(this.cmbVenueType_LostFocus);
			// 
			// txtVenueName
			// 
			this.txtVenueName.Location = new System.Drawing.Point(150, 14);
			this.txtVenueName.MaxLength = 50;
			this.txtVenueName.Name = "txtVenueName";
			this.txtVenueName.Size = new System.Drawing.Size(152, 21);
			this.txtVenueName.TabIndex = 4;
			this.txtVenueName.Text = "";
			this.txtVenueName.LostFocus += new System.EventHandler(this.txtVenueName_LostFocus);
			this.txtVenueName.GotFocus += new System.EventHandler(this.txtVenueName_GotFocus);
			// 
			// label46
			// 
			this.label46.Location = new System.Drawing.Point(8, 43);
			this.label46.Name = "label46";
			this.label46.Size = new System.Drawing.Size(96, 16);
			this.label46.TabIndex = 3;
			this.label46.Text = "Type of Venue";
			// 
			// label45
			// 
			this.label45.Location = new System.Drawing.Point(8, 19);
			this.label45.Name = "label45";
			this.label45.Size = new System.Drawing.Size(104, 16);
			this.label45.TabIndex = 2;
			this.label45.Text = "Name of Venue";
			// 
			// txtFullNameRoute
			// 
			this.txtFullNameRoute.Location = new System.Drawing.Point(150, 63);
			this.txtFullNameRoute.MaxLength = 50;
			this.txtFullNameRoute.Name = "txtFullNameRoute";
			this.txtFullNameRoute.Size = new System.Drawing.Size(152, 21);
			this.txtFullNameRoute.TabIndex = 6;
			this.txtFullNameRoute.Text = "";
			this.txtFullNameRoute.LostFocus += new System.EventHandler(this.txtFullNameRoute_LostFocus);
			this.txtFullNameRoute.GotFocus += new System.EventHandler(this.txtFullNameRoute_GotFocus);
			// 
			// label19
			// 
			this.label19.Location = new System.Drawing.Point(6, 68);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(120, 16);
			this.label19.TabIndex = 0;
			this.label19.Text = "Full Name of Route";
			// 
			// label2
			// 
			this.label2.ForeColor = System.Drawing.Color.Red;
			this.label2.Location = new System.Drawing.Point(138, 16);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(11, 14);
			this.label2.TabIndex = 33;
			this.label2.Text = "*";
			// 
			// label1
			// 
			this.label1.ForeColor = System.Drawing.Color.Red;
			this.label1.Location = new System.Drawing.Point(137, 64);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(12, 14);
			this.label1.TabIndex = 34;
			this.label1.Text = "*";
			// 
			// label3
			// 
			this.label3.ForeColor = System.Drawing.Color.Red;
			this.label3.Location = new System.Drawing.Point(8, 92);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(118, 14);
			this.label3.TabIndex = 35;
			this.label3.Text = "* Required Field";
			// 
			// CounterPartyVenueVenues
			// 
			this.Controls.Add(this.label3);
			this.Controls.Add(this.groupBox3);
			this.Name = "CounterPartyVenueVenues";
			this.Size = new System.Drawing.Size(346, 128);
			this.Load += new System.EventHandler(this.CounterPartyVenueVenues_Load);
			this.groupBox3.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		public Venue VenueProperty
		{
			get{return GetVenueDetails();}
			set{SetVenueDetails(value);}
		}

		private Venue GetVenueDetails()
		{
			Venue venue = new Venue();
			venue.VenueName = txtVenueName.Text.Trim();
			venue.VenueTypeID = int.Parse(cmbVenueType.SelectedValue.ToString());
			venue.Route = txtFullNameRoute.Text.Trim();

			return venue;			
		}

		public void SetVenueDetails(Venue venue)
		{
			if(venue != null)
			{
				txtVenueName.Text = venue.VenueName;
				cmbVenueType.SelectedValue = venue.VenueTypeID;
				txtFullNameRoute.Text = venue.Route;
			}
		}

		private void BindVenueTypes()
		{
			VenueTypes venueTypes = VenueManager.GetVenueTypes();	
			if(venueTypes.Count > 0)
			{
				cmbVenueType.DataSource = venueTypes;
				venueTypes.Insert(0, new VenueType(int.MinValue, C_COMBO_SELECT));
				cmbVenueType.DisplayMember = "Type";
				cmbVenueType.ValueMember = "VenueTypeID";
			}
		}

		private void CounterPartyVenueVenues_Load(object sender, System.EventArgs e)
		{
			try
			{
				BindVenueTypes();
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

		private StatusBar _statusBar = null;
		public StatusBar ParentStatusBar
		{
			set{_statusBar = value;}
		}

		public void Refresh(object sender, System.EventArgs e)
		{
			txtFullNameRoute.Text = "";
			txtVenueName.Text = "";
		}

		private int _venueID = int.MinValue;

		public int VenueID
		{
			set{_venueID = value;}
		}

		public int SaveVenues()
		{			
			int result = int.MinValue;
			
			if(txtVenueName.Text.Trim() == "")
			{
				_statusBar.Text = "Please Enter Venue Name!";
				txtVenueName.Focus();
			}
			else if(int.Parse(cmbVenueType.SelectedValue.ToString()) == int.MinValue)
			{
				_statusBar.Text = "Please select Venue Type!";
				cmbVenueType.Focus();
			}
			else if(txtFullNameRoute.Text.Trim() == "")
			{
				//MessageBox.Show("In Save Venues");
				_statusBar.Text = "Please enter Route Name!";
				txtFullNameRoute.Focus();
			}
			else
			{
				Nirvana.Admin.BLL.Venue venue = new Nirvana.Admin.BLL.Venue();
				venue.VenueID = _venueID;
				venue.VenueName = txtVenueName.Text.Trim();
				venue.VenueTypeID = int.Parse(cmbVenueType.SelectedValue.ToString());
				venue.Route = txtFullNameRoute.Text.Trim();
				int venueID = VenueManager.SaveVenue(venue);
				if(venueID == -1)
				{
					_statusBar.Text = "Venue already exists.";
				}	
				else
				{
					_statusBar.Text = "Stored!!.";
				}
				result = venueID;
			}							
			return result;			
		}

		#region Controls Focus Colors

		private void txtFullNameRoute_GotFocus(object sender, System.EventArgs e)
		{
			txtFullNameRoute.BackColor = Color.LemonChiffon;
		}
		private void txtFullNameRoute_LostFocus(object sender, System.EventArgs e)
		{
			txtFullNameRoute.BackColor = Color.White;
		}
		private void txtVenueName_GotFocus(object sender, System.EventArgs e)
		{
			txtVenueName.BackColor = Color.LemonChiffon;
		}
		private void txtVenueName_LostFocus(object sender, System.EventArgs e)
		{
			txtVenueName.BackColor = Color.White;
		}
		
		private void cmbVenueType_GotFocus(object sender, System.EventArgs e)
		{
			cmbVenueType.BackColor = Color.LemonChiffon;
		}
		private void cmbVenueType_LostFocus(object sender, System.EventArgs e)
		{
			cmbVenueType.BackColor = Color.White;
		}
		#endregion
	}
}
