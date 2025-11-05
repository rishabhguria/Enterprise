using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
namespace Prana.AllocationNew
{
    class AccountStrategyDefaultsUsrCtrl:UserControl 
    {
        private AccountStrategyMappingUserCtrlNew accountStrategyMappingUserCtrlNew1;
        private Infragistics.Win.Misc.UltraButton btnSave;

        private void InitializeComponent()
        {
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.accountStrategyMappingUserCtrlNew1 = new Prana.AllocationNew.AccountStrategyMappingUserCtrlNew();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnSave.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.btnSave.Location = new System.Drawing.Point(209, 213);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 102;
            this.btnSave.Text = "Save";
            this.btnSave.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.btnSave.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
            this.btnSave.UseAppStyling = false;
            this.btnSave.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // accountStrategyMappingUserCtrlNew1
            // 
            this.accountStrategyMappingUserCtrlNew1.BackColor = System.Drawing.Color.Gray;
            this.accountStrategyMappingUserCtrlNew1.BorderStyle = Infragistics.Win.UIElementBorderStyle.RaisedSoft;
            this.accountStrategyMappingUserCtrlNew1.Location = new System.Drawing.Point(103, 33);
            this.accountStrategyMappingUserCtrlNew1.Name = "accountStrategyMappingUserCtrlNew1";
            this.accountStrategyMappingUserCtrlNew1.Size = new System.Drawing.Size(309, 163);
            this.accountStrategyMappingUserCtrlNew1.TabIndex = 0;
            // 
            // AccountStrategyDefaultsUsrCtrl
            // 
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.accountStrategyMappingUserCtrlNew1);
            this.Name = "AccountStrategyDefaultsUsrCtrl";
            this.Size = new System.Drawing.Size(566, 264);
            this.ResumeLayout(false);

        }

        private void btnSave_Click(object sender, EventArgs e)
        {

        }
    }
}
