using System.Windows.Forms;
namespace Prana.ClientCommon
{
    partial class ctrlClosingAlgo
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
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
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.closingPreferencesUsrCtr = new Prana.ClientCommon.ClosingPreferencesUsrCtrl();
            this.inboxControlStyler1 = new Infragistics.Win.AppStyling.Runtime.InboxControlStyler(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraLabel3
            // 
            appearance1.BackColor = System.Drawing.Color.SteelBlue;
            appearance1.BackColor2 = System.Drawing.Color.White;
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance1.ForeColor = System.Drawing.Color.White;
            appearance1.TextHAlignAsString = "Center";
            this.ultraLabel3.Appearance = appearance1;
            this.ultraLabel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraLabel3.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel3.Location = new System.Drawing.Point(0, 0);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(717, 20);
            this.ultraLabel3.TabIndex = 25;
            this.ultraLabel3.Text = "Closing Methodology";
            // 
            // closingPreferencesUsrCtr
            // 
            this.closingPreferencesUsrCtr.BackColor = System.Drawing.Color.White;
            this.closingPreferencesUsrCtr.ForeColor = System.Drawing.Color.Black;
            this.closingPreferencesUsrCtr.Location = new System.Drawing.Point(0, 22);
            this.closingPreferencesUsrCtr.Name = "closingPreferencesUsrCtr";
            this.closingPreferencesUsrCtr.Size = new System.Drawing.Size(715, 475);
            this.closingPreferencesUsrCtr.TabIndex = 0;
            // 
            // chkCopyOpeningTradeAttributes
            // 
            this.closingPreferencesUsrCtr.chkCopyOpeningTradeAttributes.Visible = false;
            // 
            // ultraGroupBox3
            // 
            this.closingPreferencesUsrCtr.ultraGroupBox3.Size = new System.Drawing.Size(244, 420);
            // 
            // ctrlClosingAlgo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ultraLabel3);
            this.Controls.Add(this.closingPreferencesUsrCtr);
            this.Name = "ctrlClosingAlgo";
            this.Size = new System.Drawing.Size(717, 497);
            this.inboxControlStyler1.SetStyleSettings(this, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        internal ClosingPreferencesUsrCtrl closingPreferencesUsrCtr;
        private Infragistics.Win.Misc.UltraLabel ultraLabel3;
        private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler1;
    }
}
