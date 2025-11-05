using Prana.BusinessObjects;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Prana.Utilities.UI
{
    public partial class ActivityIndicator : UserControl
    {
        public ActivityIndicator()
        {
            InitializeComponent();
        }

        public void SetProgressText(ProgressInfo info)
        {
            this.toolStripStatusLabel1.ForeColor = Color.Black;
            this.toolStripStatusLabel1.Text = info.ProgressStatus;

            this.toolStripStatusLabel2.ForeColor = Color.Black;

            if (info.IsTaskCompleted)
            {
                this.toolStripStatusLabel2.Text = string.Empty;
                //this.toolStripStatusLabel2.Text = "Operation Run Succesfully...";

            }
            else
            {
                this.toolStripStatusLabel2.Text = info.HeaderText;
            }
        }

        public void Start()
        {
            this.ultraActivityIndicator1.Start();
            this.ultraActivityIndicator1.Visible = true;
        }


        public void Stop()
        {
            this.ultraActivityIndicator1.Stop();
            this.ultraActivityIndicator1.Visible = false;
        }

        public void SetViewStyle(Infragistics.Win.UltraActivityIndicator.ActivityIndicatorViewStyle viewStyle)
        {

            this.ultraActivityIndicator1.BorderStyle = Infragistics.Win.UIElementBorderStyle.WindowsVista;

            this.ultraActivityIndicator1.ViewStyle = viewStyle;

            this.ultraActivityIndicator1.AnimationSpeed = 20;

            // this.ultraActivityIndicator1.UseOsThemes = Infragistics.Win.DefaultableBoolean.True;



            // Set Appearance object
            // this.ultraActivityIndicator1.MarqueeFillAppearance.BackColor = Color.;
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

    }
}
