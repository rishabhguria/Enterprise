using System;
using System.Windows.Forms;


namespace Prana.Admin.RoutingLogic.Forms
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class Starting : System.Windows.Forms.Form
    {
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public Starting()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
            this.DesktopLocation = new System.Drawing.Point(-30, -200);
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if (ultraLabel1 != null)
                {
                    ultraLabel1.Dispose();
                }
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Starting));
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.SuspendLayout();
            // 
            // ultraLabel1
            // 
            this.ultraLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            appearance1.TextHAlign = Infragistics.Win.HAlign.Center;
            appearance1.TextVAlign = Infragistics.Win.VAlign.Middle;
            this.ultraLabel1.Appearance = appearance1;
            this.ultraLabel1.Location = new System.Drawing.Point(56, 40);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.TabIndex = 0;
            this.ultraLabel1.Text = "Loading . . .";
            this.ultraLabel1.WrapText = false;
            // 
            // Starting
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
            this.ClientSize = new System.Drawing.Size(224, 109);
            this.Controls.Add(this.ultraLabel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Starting";
            this.Text = "Starting";
            this.ResumeLayout(false);

        }
        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //			Application.Run(new CompanyMaster());
            //			Application.Run(new Forms.CompanyMaster(new System.Windows.Forms.NodeTree() , 20));
            Application.Run(new Forms.CompanyMaster(new System.Windows.Forms.NodeTree("20", "Routing Logic"), 20));
        }
    }
}
