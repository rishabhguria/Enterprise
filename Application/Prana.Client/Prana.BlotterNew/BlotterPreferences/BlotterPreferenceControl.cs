using Prana.BusinessObjects;
using Prana.LogManager;
using Prana.TradeManager.Extension;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Prana.Blotter.Prefs
{
    public class BlotterPrefControl : System.Windows.Forms.UserControl, Prana.Interfaces.IPreferences
    {
        #region Private Variables
        private Infragistics.Win.UltraWinTabControl.UltraTabControl ultraTabControl2;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage2;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl3;
        private IContainer components;

        private BlotterColorControl blotterColorControl;
        private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler;
        #endregion

        public void InitControl(BlotterPreferenceData blotterPreferenceData)
        {
            try
            {
                blotterColorControl.SetUIBlotterPreferenceData(blotterPreferenceData);
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

        public BlotterPrefControl()
        {
            try
            {
                InitializeComponent();
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

        public void SetUp(CompanyUser loginUser)
        {
            BlotterPreferenceManager.GetInstance().SetUser(loginUser);
            InitControl((BlotterPreferenceData)BlotterPreferenceManager.GetInstance().GetPreferencesBinary());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if (ultraTabControl2 != null)
                {
                    ultraTabControl2.Dispose();
                }
                if (ultraTabSharedControlsPage2 != null)
                {
                    ultraTabSharedControlsPage2.Dispose();
                }
                if (ultraTabPageControl3 != null)
                {
                    ultraTabPageControl3.Dispose();
                }
                if (blotterColorControl != null)
                {
                    blotterColorControl.Dispose();
                }
                if (inboxControlStyler != null)
                {
                    inboxControlStyler.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab4 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            this.ultraTabPageControl3 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabControl2 = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage2 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.inboxControlStyler = new Infragistics.Win.AppStyling.Runtime.InboxControlStyler(this.components);
            this.blotterColorControl = new Prana.Blotter.Prefs.BlotterColorControl();
            this.ultraTabPageControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTabControl2)).BeginInit();
            this.ultraTabControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraTabPageControl3
            // 
            this.ultraTabPageControl3.Controls.Add(this.blotterColorControl);
            this.ultraTabPageControl3.Location = new System.Drawing.Point(1, 23);
            this.ultraTabPageControl3.Name = "ultraTabPageControl3";
            this.ultraTabPageControl3.Size = new System.Drawing.Size(539, 278);
            // 
            // ultraTabControl2
            // 
            this.ultraTabControl2.Controls.Add(this.ultraTabSharedControlsPage2);
            this.ultraTabControl2.Controls.Add(this.ultraTabPageControl3);
            this.ultraTabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraTabControl2.Location = new System.Drawing.Point(0, 0);
            this.ultraTabControl2.Name = "ultraTabControl2";
            this.ultraTabControl2.NavigationStyle = Infragistics.Win.UltraWinTabControl.NavigationStyle.Activate;
            this.ultraTabControl2.ScaleImages = Infragistics.Win.ScaleImage.Never;
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ultraTabControl2.SelectedTabAppearance = appearance1;
            this.ultraTabControl2.SharedControlsPage = this.ultraTabSharedControlsPage2;
            this.ultraTabControl2.Size = new System.Drawing.Size(543, 304);
            this.ultraTabControl2.TabIndex = 1;
            ultraTab4.Key = "Color";
            ultraTab4.TabPage = this.ultraTabPageControl3;
            ultraTab4.Text = "Color";
            this.ultraTabControl2.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab4});
            // 
            // ultraTabSharedControlsPage2
            // 
            this.ultraTabSharedControlsPage2.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage2.Name = "ultraTabSharedControlsPage2";
            this.ultraTabSharedControlsPage2.Size = new System.Drawing.Size(539, 278);
            // 
            // blotterColorControl
            // 
            this.blotterColorControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(210)))), ((int)(((byte)(212)))));
            this.blotterColorControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.blotterColorControl.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.blotterColorControl.Location = new System.Drawing.Point(0, 0);
            this.blotterColorControl.Name = "blotterColorControl";
            this.blotterColorControl.Size = new System.Drawing.Size(539, 278);
            this.blotterColorControl.TabIndex = 0;
            // 
            // BlotterPrefControl
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.ultraTabControl2);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.Name = "BlotterPrefControl";
            this.Size = new System.Drawing.Size(543, 304);
            this.inboxControlStyler.SetStyleSettings(this, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.ultraTabPageControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraTabControl2)).EndInit();
            this.ultraTabControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        #region IPreferences Members
        public void RestoreDefault()
        {
            try
            {
                blotterColorControl.SetUIBlotterPreferenceData(BlotterPreferenceManager.GetInstance().SetDefaultPreferences());
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

        public UserControl Reference()
        {
            return this;
        }

        public bool Save()
        {
            bool isSaved = false;
            try
            {
                isSaved = BlotterPreferenceManager.GetInstance().SetPreferencesBinary((BlotterPreferenceData)GetPrefs());
                if (!isSaved)
                {
                    MessageBox.Show("Blotter Preferences Could not be saved.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return isSaved;
        }

        public IPreferenceData GetPrefs()
        {
            return blotterColorControl.GetUIBlotterPreferenceData();
        }

        private string _modulename = string.Empty;
        public string SetModuleActive
        {
            set
            {
                _modulename = value;
            }
        }
        #endregion
    }
}