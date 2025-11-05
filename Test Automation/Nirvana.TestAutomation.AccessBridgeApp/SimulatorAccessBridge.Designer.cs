using Nirvana.TestAutomation.AccessBridgeApp;
namespace Nirvana.TestAutomation.AccessBridgeApp
{
    partial class SimulatorAccessBridge
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Text = string.Empty;
            this.MinimumSize = new System.Drawing.Size(1, 1);
            this.MaximumSize = new System.Drawing.Size(1, 1);
            this.MaximizeBox = false;
            this.ShowIcon = false;
            this._initialRefreshTimer = new System.Windows.Forms.Timer();
            this._initialRefreshTimer.Enabled = true;
            this._initialRefreshTimer.Tick += _initialRefreshTimer_Tick;
            this.Load += SimulatorHelper_Load;
        }

        #endregion
    }
}