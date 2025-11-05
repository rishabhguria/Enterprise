#region Using


#endregion

namespace Prana.Admin.Controls
{
    /// <summary>
    /// Summary description for TimeControl.
    /// </summary>
    public class TimeControl : System.Windows.Forms.UserControl
    {
        #region Local variables

        private string _time = string.Empty;
        private System.Windows.Forms.NumericUpDown hour;
        private System.Windows.Forms.NumericUpDown min;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;

        #endregion

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public TimeControl()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
            // TODO: Add any initialization after the InitializeComponent call
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
                if (hour != null)
                {
                    hour.Dispose();
                }
                if (min != null)
                {
                    min.Dispose();
                }
                if (label1 != null)
                {
                    label1.Dispose();
                }
                if (label2 != null)
                {
                    label2.Dispose();
                }
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
            this.hour = new System.Windows.Forms.NumericUpDown();
            this.min = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.hour)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.min)).BeginInit();
            this.SuspendLayout();
            // 
            // hour
            // 
            this.hour.ForeColor = System.Drawing.SystemColors.ControlText;
            this.hour.Location = new System.Drawing.Point(0, 0);
            this.hour.Maximum = new decimal(new int[] {
            23,
            0,
            0,
            0});
            this.hour.Name = "hour";
            this.hour.Size = new System.Drawing.Size(40, 21);
            this.hour.TabIndex = 0;
            // 
            // min
            // 
            this.min.ForeColor = System.Drawing.SystemColors.ControlText;
            this.min.Location = new System.Drawing.Point(48, 0);
            this.min.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
            this.min.Name = "min";
            this.min.Size = new System.Drawing.Size(40, 21);
            this.min.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(40, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(8, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = ":";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.label2.Location = new System.Drawing.Point(88, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "hh:mm";
            // 
            // TimeControl
            // 
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.min);
            this.Controls.Add(this.hour);
            this.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.Name = "TimeControl";
            this.Size = new System.Drawing.Size(128, 24);
            ((System.ComponentModel.ISupportInitialize)(this.hour)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.min)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        public string Time
        {
            get
            {
                _time = hour.Value.ToString() + ":" + min.Value.ToString();
                return _time;
            }
            set
            {
                _time = value;
                char[] seperator = new char[1];
                seperator[0] = ':';
                string[] result = value.Split(seperator, 2);
                hour.Value = int.Parse(result[0]);
                min.Value = int.Parse(result[1]);
            }
        }
    }
}
