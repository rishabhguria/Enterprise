using Prana.LogManager;
using System;
using System.Drawing;

namespace Prana.LiveFeed.UI
{
    /// <summary>
    /// Summary description for ColorControl.
    /// </summary>
    public class ColorPreferenceControl : System.Windows.Forms.UserControl
    {
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox1;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private Infragistics.Win.Misc.UltraLabel ultraLabel4;
        private Infragistics.Win.UltraWinEditors.UltraColorPicker cmbBackgroundColor;
        private Infragistics.Win.UltraWinEditors.UltraColorPicker cmbAltRowColor;
        private Infragistics.Win.UltraWinEditors.UltraColorPicker cmbRowColor;
        private Infragistics.Win.Misc.UltraLabel ultraLabel3;
        private Infragistics.Win.UltraWinEditors.UltraColorPicker cmbHeaderColor;
        private Infragistics.Win.UltraWinEditors.UltraColorPicker cmbGridLineColor;
        private Infragistics.Win.Misc.UltraLabel ultraLabel5;
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public ColorPreferenceControl()
        {
            // This call is required by the Windows.Forms Form Designer.
            try
            {
                InitializeComponent();

            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
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
                if (ultraGroupBox1 != null)
                {
                    ultraGroupBox1.Dispose();
                }
                if (ultraLabel1 != null)
                {
                    ultraLabel1.Dispose();
                }
                if (ultraLabel2 != null)
                {
                    ultraLabel2.Dispose();
                }
                if (ultraLabel4 != null)
                {
                    ultraLabel4.Dispose();
                }
                if (ultraLabel5 != null)
                {
                    ultraLabel5.Dispose();
                }
                if (cmbBackgroundColor != null)
                {
                    cmbBackgroundColor.Dispose();
                }
                if (cmbAltRowColor != null)
                {
                    cmbAltRowColor.Dispose();
                }
                if (cmbRowColor != null)
                {
                    cmbRowColor.Dispose();
                }
                if (ultraLabel3 != null)
                {
                    ultraLabel3.Dispose();
                }
                if (cmbHeaderColor != null)
                {
                    cmbHeaderColor.Dispose();
                }
                if (cmbGridLineColor != null)
                {
                    cmbGridLineColor.Dispose();
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            this.ultraGroupBox1 = new Infragistics.Win.Misc.UltraGroupBox();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.cmbBackgroundColor = new Infragistics.Win.UltraWinEditors.UltraColorPicker();
            this.cmbAltRowColor = new Infragistics.Win.UltraWinEditors.UltraColorPicker();
            this.cmbRowColor = new Infragistics.Win.UltraWinEditors.UltraColorPicker();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel4 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.cmbHeaderColor = new Infragistics.Win.UltraWinEditors.UltraColorPicker();
            this.ultraLabel5 = new Infragistics.Win.Misc.UltraLabel();
            this.cmbGridLineColor = new Infragistics.Win.UltraWinEditors.UltraColorPicker();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).BeginInit();
            this.ultraGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBackgroundColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAltRowColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbRowColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbHeaderColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbGridLineColor)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraGroupBox1
            // 
            this.ultraGroupBox1.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
            this.ultraGroupBox1.Controls.Add(this.ultraLabel1);
            this.ultraGroupBox1.Controls.Add(this.cmbBackgroundColor);
            this.ultraGroupBox1.Controls.Add(this.cmbAltRowColor);
            this.ultraGroupBox1.Controls.Add(this.cmbRowColor);
            this.ultraGroupBox1.Controls.Add(this.ultraLabel2);
            this.ultraGroupBox1.Controls.Add(this.ultraLabel4);
            this.ultraGroupBox1.Controls.Add(this.ultraLabel3);
            this.ultraGroupBox1.Controls.Add(this.cmbHeaderColor);
            this.ultraGroupBox1.Controls.Add(this.ultraLabel5);
            this.ultraGroupBox1.Controls.Add(this.cmbGridLineColor);
            this.ultraGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraGroupBox1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.ultraGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.ultraGroupBox1.Name = "ultraGroupBox1";
            this.ultraGroupBox1.Size = new System.Drawing.Size(296, 188);
            this.ultraGroupBox1.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.ultraGroupBox1.TabIndex = 2;
            this.ultraGroupBox1.Text = "Grid Color";
            this.ultraGroupBox1.Click += new System.EventHandler(this.ultraGroupBox1_Click);
            // 
            // ultraLabel1
            // 
            this.ultraLabel1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ultraLabel1.Location = new System.Drawing.Point(20, 32);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(62, 16);
            this.ultraLabel1.TabIndex = 4;
            this.ultraLabel1.Text = "Row Color";
            // 
            // cmbBackgroundColor
            // 
            this.cmbBackgroundColor.AlwaysInEditMode = true;
            appearance1.BorderColor = System.Drawing.Color.Black;
            this.cmbBackgroundColor.Appearance = appearance1;
            this.cmbBackgroundColor.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbBackgroundColor.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.cmbBackgroundColor.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbBackgroundColor.Location = new System.Drawing.Point(140, 90);
            this.cmbBackgroundColor.Name = "cmbBackgroundColor";
            this.cmbBackgroundColor.Size = new System.Drawing.Size(144, 20);
            this.cmbBackgroundColor.TabIndex = 3;
            this.cmbBackgroundColor.Text = "Control";
            this.cmbBackgroundColor.ColorChanged += new System.EventHandler(this.ColorChanged);
            // 
            // cmbAltRowColor
            // 
            this.cmbAltRowColor.AlwaysInEditMode = true;
            appearance2.BorderColor = System.Drawing.Color.Black;
            this.cmbAltRowColor.Appearance = appearance2;
            this.cmbAltRowColor.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbAltRowColor.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.cmbAltRowColor.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbAltRowColor.Location = new System.Drawing.Point(138, 58);
            this.cmbAltRowColor.Name = "cmbAltRowColor";
            this.cmbAltRowColor.Size = new System.Drawing.Size(144, 20);
            this.cmbAltRowColor.TabIndex = 1;
            this.cmbAltRowColor.Text = "Control";
            this.cmbAltRowColor.ColorChanged += new System.EventHandler(this.ColorChanged);
            // 
            // cmbRowColor
            // 
            this.cmbRowColor.AlwaysInEditMode = true;
            appearance3.BorderColor = System.Drawing.Color.Black;
            this.cmbRowColor.Appearance = appearance3;
            this.cmbRowColor.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbRowColor.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.cmbRowColor.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbRowColor.Location = new System.Drawing.Point(138, 30);
            this.cmbRowColor.Name = "cmbRowColor";
            this.cmbRowColor.Size = new System.Drawing.Size(146, 20);
            this.cmbRowColor.TabIndex = 0;
            this.cmbRowColor.Text = "Control";
            this.cmbRowColor.ColorChanged += new System.EventHandler(this.ColorChanged);
            // 
            // ultraLabel2
            // 
            this.ultraLabel2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ultraLabel2.Location = new System.Drawing.Point(20, 60);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(106, 16);
            this.ultraLabel2.TabIndex = 4;
            this.ultraLabel2.Text = "Alternate Row Color";
            // 
            // ultraLabel4
            // 
            this.ultraLabel4.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ultraLabel4.Location = new System.Drawing.Point(22, 92);
            this.ultraLabel4.Name = "ultraLabel4";
            this.ultraLabel4.Size = new System.Drawing.Size(98, 16);
            this.ultraLabel4.TabIndex = 4;
            this.ultraLabel4.Text = "BackGround Color";
            // 
            // ultraLabel3
            // 
            this.ultraLabel3.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ultraLabel3.Location = new System.Drawing.Point(20, 154);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(98, 16);
            this.ultraLabel3.TabIndex = 4;
            this.ultraLabel3.Text = "Header Row Color";
            this.ultraLabel3.Visible = false;
            // 
            // cmbHeaderColor
            // 
            this.cmbHeaderColor.AlwaysInEditMode = true;
            appearance4.BorderColor = System.Drawing.Color.Black;
            this.cmbHeaderColor.Appearance = appearance4;
            this.cmbHeaderColor.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbHeaderColor.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.cmbHeaderColor.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbHeaderColor.Location = new System.Drawing.Point(138, 152);
            this.cmbHeaderColor.Name = "cmbHeaderColor";
            this.cmbHeaderColor.Size = new System.Drawing.Size(144, 20);
            this.cmbHeaderColor.TabIndex = 1;
            this.cmbHeaderColor.Text = "Control";
            this.cmbHeaderColor.Visible = false;
            this.cmbHeaderColor.ColorChanged += new System.EventHandler(this.ColorChanged);
            // 
            // ultraLabel5
            // 
            this.ultraLabel5.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ultraLabel5.Location = new System.Drawing.Point(22, 122);
            this.ultraLabel5.Name = "ultraLabel5";
            this.ultraLabel5.Size = new System.Drawing.Size(80, 16);
            this.ultraLabel5.TabIndex = 4;
            this.ultraLabel5.Text = "GridLine Color";
            // 
            // cmbGridLineColor
            // 
            this.cmbGridLineColor.AlwaysInEditMode = true;
            appearance5.BorderColor = System.Drawing.Color.Black;
            this.cmbGridLineColor.Appearance = appearance5;
            this.cmbGridLineColor.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbGridLineColor.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.cmbGridLineColor.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbGridLineColor.Location = new System.Drawing.Point(140, 120);
            this.cmbGridLineColor.Name = "cmbGridLineColor";
            this.cmbGridLineColor.Size = new System.Drawing.Size(144, 20);
            this.cmbGridLineColor.TabIndex = 3;
            this.cmbGridLineColor.Text = "Control";
            this.cmbGridLineColor.ColorChanged += new System.EventHandler(this.ColorChanged);
            // 
            // ColorPreferenceControl
            // 
            this.Controls.Add(this.ultraGroupBox1);
            this.Name = "ColorPreferenceControl";
            this.Size = new System.Drawing.Size(296, 188);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).EndInit();
            this.ultraGroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cmbBackgroundColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAltRowColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbRowColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbHeaderColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbGridLineColor)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private Color _rowColor = Color.Transparent;
        public Color RowColor
        {
            get { return cmbRowColor.Color; }
            set
            {
                _rowColor = value;
                cmbRowColor.Color = _rowColor;
            }
        }

        private Color _rowAltColor = Color.Transparent;
        public Color RowAltColor
        {
            get { return cmbAltRowColor.Color; }
            set
            {
                _rowAltColor = value;
                cmbAltRowColor.Color = _rowAltColor;
            }
        }

        private Color _headerColor = Color.Transparent;
        public Color HeaderColor
        {
            get { return cmbHeaderColor.Color; }
            set
            {
                _headerColor = value;
                cmbHeaderColor.Color = _headerColor;
            }
        }

        //		private Color _fontColor ;
        //		public Color FontColor
        //		{
        //			get{return cmbFontColor.Color ;}
        //			set
        //			{
        //				_fontColor = value;
        //				cmbFontColor.Color = _fontColor;
        //			}
        //		}

        private Color _backgroundColor = Color.Transparent;
        public Color BackgroundColor
        {
            get { return cmbBackgroundColor.Color; }
            set
            {
                _backgroundColor = value;
                cmbBackgroundColor.Color = _backgroundColor;
            }
        }


        private bool _enableGridLineColorCombo = false;
        public bool EnableGridLineColorCombo
        {
            get { return _enableGridLineColorCombo; }
            set
            {
                if (value)
                {
                    _enableGridLineColorCombo = true;
                    cmbGridLineColor.Enabled = true;
                    ultraLabel5.Enabled = true;
                }
                else
                {
                    _enableGridLineColorCombo = false;
                    cmbGridLineColor.Enabled = false;
                    ultraLabel5.Enabled = false;
                }
            }
        }

        private Color _gridLineColor = Color.Transparent;

        private void ColorChanged(object sender, System.EventArgs e)
        {
            try
            {
                Color changedColor = ((Infragistics.Win.UltraWinEditors.UltraColorPicker)sender).Color;
                ((Infragistics.Win.UltraWinEditors.UltraColorPicker)sender).Appearance.BackColor = changedColor;
                ((Infragistics.Win.UltraWinEditors.UltraColorPicker)sender).Appearance.ForeColor = changedColor;
                ((Infragistics.Win.UltraWinEditors.UltraColorPicker)sender).Appearance.BorderColor = changedColor;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void ultraGroupBox1_Click(object sender, System.EventArgs e)
        {

        }

        public Color GridLineColor
        {
            get { return cmbGridLineColor.Color; }
            set
            {
                _gridLineColor = value;
                cmbGridLineColor.Color = _gridLineColor;
            }
        }


    }
}

