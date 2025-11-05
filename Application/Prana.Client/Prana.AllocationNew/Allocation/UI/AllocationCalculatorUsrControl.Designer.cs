namespace Prana.AllocationNew
{
    partial class AllocationCalculatorUsrControl
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
            this.targetCtrl = new Prana.AllocationNew.AllocationCtrl();
            
            this.currentCtrl = new Prana.AllocationNew.AllocationCtrl();
           
            this.newCtrl = new Prana.AllocationNew.AllocationCtrl();
            this.inboxControlStyler1 = new Infragistics.Win.AppStyling.Runtime.InboxControlStyler(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).BeginInit();
            this.SuspendLayout();
            // 
            // targetCtrl
            // 
            this.targetCtrl.AutoSize = true;
            this.targetCtrl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.targetCtrl.Location = new System.Drawing.Point(242, 33);
            this.targetCtrl.Name = "targetCtrl";
            this.targetCtrl.OnlyPercentage = false;
            this.targetCtrl.Size = new System.Drawing.Size(51, 52);
            this.targetCtrl.TabIndex = 2;
            // 
            // currentCtrl
            // 
            this.currentCtrl.AutoSize = true;
            this.currentCtrl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.currentCtrl.Location = new System.Drawing.Point(83, 33);
            this.currentCtrl.Name = "currentCtrl";
            this.currentCtrl.OnlyPercentage = false;
            this.currentCtrl.Size = new System.Drawing.Size(51, 28);
            this.currentCtrl.TabIndex = 1;
            // 
            // newCtrl
            // 
            this.newCtrl.AutoSize = true;
            this.newCtrl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.newCtrl.Location = new System.Drawing.Point(24, 40);
            this.newCtrl.Name = "newCtrl";
            this.newCtrl.OnlyPercentage = false;
            this.newCtrl.Size = new System.Drawing.Size(51, 21);
            this.newCtrl.TabIndex = 0;
            // 
            // AllocationCalculatorUsrControl
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ClientArea.Controls.Add(this.targetCtrl);
            this.ClientArea.Controls.Add(this.currentCtrl);
            this.ClientArea.Controls.Add(this.newCtrl);
            this.Name = "AllocationCalculatorUsrControl";
            this.Size = new System.Drawing.Size(366, 114);
            this.inboxControlStyler1.SetStyleSettings(this, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AllocationCtrl newCtrl;
        private AllocationCtrl currentCtrl;
        private AllocationCtrl targetCtrl;
        private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler1;
    }
}
