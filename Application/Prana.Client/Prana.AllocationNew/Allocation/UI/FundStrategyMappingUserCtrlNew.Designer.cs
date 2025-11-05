namespace Prana.AllocationNew
{
    partial class AccountStrategyMappingUserCtrlNew
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
            UnBindEvents();
            if (accountStrategyAllocationControl != null)
            {
            accountStrategyAllocationControl.ValueChanged -= allocationCtrl_ValueChanged;
                accountStrategyAllocationControl.Dispose();
                accountStrategyAllocationControl = null;
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
            this.inboxControlStyler1 = new Infragistics.Win.AppStyling.Runtime.InboxControlStyler(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).BeginInit();
            this.SuspendLayout();
            // 
            // AccountStrategyMappingUserCtrlNew
            // 
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.BorderStyle = Infragistics.Win.UIElementBorderStyle.RaisedSoft;
            this.Name = "AccountStrategyMappingUserCtrlNew";
            this.Size = new System.Drawing.Size(309, 163);
            this.inboxControlStyler1.SetStyleSettings(this, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler1;
    }
}
