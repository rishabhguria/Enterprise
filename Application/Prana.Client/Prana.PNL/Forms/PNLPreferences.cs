using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Nirvana.Global;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation;


using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace Nirvana.PNL
{
	/// <summary>
	/// Summary description for PNLPreferences.
	/// </summary>
	public class PNLPreferences : System.Windows.Forms.Form
	{
		private Nirvana.PNL.PNLPrefrencesControl pnlPrefrencesControl1;
		private PNLPrefrencesData _pnlPreferences;
		private const string FORM_NAME = "PNL: Preferences";
		private System.Windows.Forms.Button btnRestoreDefault;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Panel pnlButtons;
		private System.Windows.Forms.Panel pnlContainer;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public PNLPreferences()
		{
			try
			{
				//
				// Required for Windows Form Designer support
				//
				InitializeComponent();

				//Add the PNLPreferenceControl
				pnlPrefrencesControl1 = new PNLPrefrencesControl();
				pnlPrefrencesControl1.Dock = DockStyle.Fill; 

				//pnlPrefrencesControl1.SaveClick +=new EventHandler(pnlPrefrencesControl1_SaveClick);
				//pnlPrefrencesControl1.ApplyPreferences +=new EventHandler(pnlPrefrencesControl1_ApplyPreferences);

				this.pnlContainer.Controls.Add(pnlPrefrencesControl1);
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

		public PNLPrefrencesData Preferences
		{
			get
			{
				_pnlPreferences = pnlPrefrencesControl1.Preferences;
				return _pnlPreferences;
			}
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing)
			{
				if(components != null)
				{
					components.Dispose();
				}

				_instPNLPreferences = null;
			}
			base.Dispose( disposing );
		}

		private static PNLPreferences _instPNLPreferences ;

		/// <summary>
		/// Singleton instance for the newsStory form
		/// </summary>
		/// <returns></returns>
		public static PNLPreferences GetInstance()
		{
			if(_instPNLPreferences == null || _instPNLPreferences.IsDisposed)
				_instPNLPreferences = new PNLPreferences();

			return _instPNLPreferences ;
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(PNLPreferences));
			this.pnlButtons = new System.Windows.Forms.Panel();
			this.btnRestoreDefault = new System.Windows.Forms.Button();
			this.btnSave = new System.Windows.Forms.Button();
			this.btnClose = new System.Windows.Forms.Button();
			this.pnlContainer = new System.Windows.Forms.Panel();
			this.pnlButtons.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlButtons
			// 
			this.pnlButtons.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.pnlButtons.Controls.Add(this.btnRestoreDefault);
			this.pnlButtons.Controls.Add(this.btnSave);
			this.pnlButtons.Controls.Add(this.btnClose);
			this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pnlButtons.Location = new System.Drawing.Point(0, 326);
			this.pnlButtons.Name = "pnlButtons";
			this.pnlButtons.Size = new System.Drawing.Size(544, 48);
			this.pnlButtons.TabIndex = 2;
			// 
			// btnRestoreDefault
			// 
			this.btnRestoreDefault.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnRestoreDefault.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnRestoreDefault.BackgroundImage")));
			this.btnRestoreDefault.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnRestoreDefault.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this.btnRestoreDefault.ForeColor = System.Drawing.SystemColors.ControlText;
			this.btnRestoreDefault.Location = new System.Drawing.Point(128, 16);
			this.btnRestoreDefault.Name = "btnRestoreDefault";
			this.btnRestoreDefault.Size = new System.Drawing.Size(100, 23);
			this.btnRestoreDefault.TabIndex = 5;
			this.btnRestoreDefault.Click += new System.EventHandler(this.btnRestoreDefault_Click);
			// 
			// btnSave
			// 
			this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSave.BackgroundImage")));
			this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnSave.Location = new System.Drawing.Point(247, 16);
			this.btnSave.Name = "btnSave";
			this.btnSave.TabIndex = 3;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnClose
			// 
			this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClose.BackgroundImage")));
			this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnClose.Location = new System.Drawing.Point(340, 17);
			this.btnClose.Name = "btnClose";
			this.btnClose.TabIndex = 4;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// pnlContainer
			// 
			this.pnlContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlContainer.Location = new System.Drawing.Point(0, 0);
			this.pnlContainer.Name = "pnlContainer";
			this.pnlContainer.Size = new System.Drawing.Size(544, 326);
			this.pnlContainer.TabIndex = 3;
			// 
			// PNLPreferences
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.ClientSize = new System.Drawing.Size(544, 374);
			this.Controls.Add(this.pnlContainer);
			this.Controls.Add(this.pnlButtons);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "PNLPreferences";
			this.Text = " PNL Preferences";
			this.pnlButtons.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void pnlPrefrencesControl1_ApplyPreferences(object sender, System.EventArgs e)
		{

		}

		private void pnlPrefrencesControl1_SaveClick(object sender, System.EventArgs e)
		{
			if(ApplyPreferences != null)
				ApplyPreferences(null, null);
		}

		private void btnSave_Click(object sender, System.EventArgs e)
		{
				pnlPrefrencesControl1.Save();	
		}

		private void btnRestoreDefault_Click(object sender, System.EventArgs e)
		{
			pnlPrefrencesControl1.SetDefault();
		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		public event System.EventHandler SaveClick;

		public event System.EventHandler ApplyPreferences;


	}
}
