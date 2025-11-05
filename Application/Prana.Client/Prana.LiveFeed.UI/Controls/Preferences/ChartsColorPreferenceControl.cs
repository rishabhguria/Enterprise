using Prana.LogManager;
using System;
using System.Drawing;

namespace Prana.LiveFeed.UI
{
    /// <summary>
    /// Summary description for ColorPreferences.
    /// </summary>
    public class ChartsColorPreferenceControl : System.Windows.Forms.UserControl
    {
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox2;
        private Infragistics.Win.Misc.UltraLabel lblGradientValueView;
        private Infragistics.Win.Misc.UltraLabel lblGradentbottom;
        private Infragistics.Win.Misc.UltraLabel lblGradientTop;
        private Infragistics.Win.Misc.UltraLabel lblSingleBarColor;
        private Infragistics.Win.Misc.UltraLabel lblSeriesColor;
        private Infragistics.Win.Misc.UltraLabel lblDownTickColor;
        private Infragistics.Win.Misc.UltraLabel lblUpTickColor;
        private Infragistics.Win.Misc.UltraLabel lblGridColor;
        private Infragistics.Win.Misc.UltraLabel lblBackColor;
        private Infragistics.Win.Misc.UltraLabel lblForeColor;
        private Infragistics.Win.UltraWinEditors.UltraColorPicker ultraCmbBackgroundColor;
        private Infragistics.Win.UltraWinEditors.UltraColorPicker ultraCmbForegroundColor;
        private Infragistics.Win.UltraWinEditors.UltraColorPicker ultraCmbGridColor;
        private Infragistics.Win.UltraWinEditors.UltraColorPicker ultraCmbUpTickColor;
        private Infragistics.Win.UltraWinEditors.UltraColorPicker ultraCmbDownTickColor;
        private Infragistics.Win.UltraWinEditors.UltraColorPicker ultraCmbSeriesColor;
        private Infragistics.Win.UltraWinEditors.UltraColorPicker ultraCmbSingleBarColor;
        private Infragistics.Win.UltraWinEditors.UltraColorPicker ultraCmbGradientTopColor;
        private Infragistics.Win.UltraWinEditors.UltraColorPicker ultraCmbGradientBottomColor;
        private Infragistics.Win.UltraWinEditors.UltraColorPicker ultraCmbGradientValueViewColor;
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public ChartsColorPreferenceControl()
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
                if (lblBackColor != null)
                {
                    lblBackColor.Dispose();
                }
                if (ultraGroupBox2 != null)
                {
                    ultraGroupBox2.Dispose();
                }
                if (lblGradientValueView != null)
                {
                    lblGradientValueView.Dispose();
                }
                if (lblGradentbottom != null)
                {
                    lblGradentbottom.Dispose();
                }
                if (lblGradientTop != null)
                {
                    lblGradientTop.Dispose();
                }
                if (lblSingleBarColor != null)
                {
                    lblSingleBarColor.Dispose();
                }
                if (lblSeriesColor != null)
                {
                    lblSeriesColor.Dispose();
                }
                if (lblDownTickColor != null)
                {
                    lblDownTickColor.Dispose();
                }
                if (lblUpTickColor != null)
                {
                    lblUpTickColor.Dispose();
                }
                if (lblGridColor != null)
                {
                    lblGridColor.Dispose();
                }
                if (lblForeColor != null)
                {
                    lblForeColor.Dispose();
                }
                if (ultraCmbBackgroundColor != null)
                {
                    ultraCmbBackgroundColor.Dispose();
                }
                if (ultraCmbSeriesColor != null)
                {
                    ultraCmbSeriesColor.Dispose();
                }
                if (ultraCmbForegroundColor != null)
                {
                    ultraCmbForegroundColor.Dispose();
                }
                if (ultraCmbGridColor != null)
                {
                    ultraCmbGridColor.Dispose();
                }
                if (ultraCmbUpTickColor != null)
                {
                    ultraCmbUpTickColor.Dispose();
                }
                if (ultraCmbDownTickColor != null)
                {
                    ultraCmbDownTickColor.Dispose();
                }
                if (ultraCmbSingleBarColor != null)
                {
                    ultraCmbSingleBarColor.Dispose();
                }
                if (ultraCmbGradientBottomColor != null)
                {
                    ultraCmbGradientBottomColor.Dispose();
                }
                if (ultraCmbGradientTopColor != null)
                {
                    ultraCmbGradientTopColor.Dispose();
                }
                if (ultraCmbGradientValueViewColor != null)
                {
                    ultraCmbGradientValueViewColor.Dispose();
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
            this.ultraGroupBox2 = new Infragistics.Win.Misc.UltraGroupBox();
            this.ultraCmbGradientValueViewColor = new Infragistics.Win.UltraWinEditors.UltraColorPicker();
            this.ultraCmbGradientBottomColor = new Infragistics.Win.UltraWinEditors.UltraColorPicker();
            this.ultraCmbGradientTopColor = new Infragistics.Win.UltraWinEditors.UltraColorPicker();
            this.ultraCmbSingleBarColor = new Infragistics.Win.UltraWinEditors.UltraColorPicker();
            this.ultraCmbSeriesColor = new Infragistics.Win.UltraWinEditors.UltraColorPicker();
            this.ultraCmbDownTickColor = new Infragistics.Win.UltraWinEditors.UltraColorPicker();
            this.ultraCmbUpTickColor = new Infragistics.Win.UltraWinEditors.UltraColorPicker();
            this.ultraCmbGridColor = new Infragistics.Win.UltraWinEditors.UltraColorPicker();
            this.ultraCmbBackgroundColor = new Infragistics.Win.UltraWinEditors.UltraColorPicker();
            this.ultraCmbForegroundColor = new Infragistics.Win.UltraWinEditors.UltraColorPicker();
            this.lblGradientValueView = new Infragistics.Win.Misc.UltraLabel();
            this.lblGradentbottom = new Infragistics.Win.Misc.UltraLabel();
            this.lblGradientTop = new Infragistics.Win.Misc.UltraLabel();
            this.lblSingleBarColor = new Infragistics.Win.Misc.UltraLabel();
            this.lblSeriesColor = new Infragistics.Win.Misc.UltraLabel();
            this.lblDownTickColor = new Infragistics.Win.Misc.UltraLabel();
            this.lblUpTickColor = new Infragistics.Win.Misc.UltraLabel();
            this.lblGridColor = new Infragistics.Win.Misc.UltraLabel();
            this.lblBackColor = new Infragistics.Win.Misc.UltraLabel();
            this.lblForeColor = new Infragistics.Win.Misc.UltraLabel();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox2)).BeginInit();
            this.ultraGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCmbGradientValueViewColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCmbGradientBottomColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCmbGradientTopColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCmbSingleBarColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCmbSeriesColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCmbDownTickColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCmbUpTickColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCmbGridColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCmbBackgroundColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCmbForegroundColor)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraGroupBox2
            // 
            this.ultraGroupBox2.Controls.Add(this.ultraCmbGradientValueViewColor);
            this.ultraGroupBox2.Controls.Add(this.ultraCmbGradientBottomColor);
            this.ultraGroupBox2.Controls.Add(this.ultraCmbGradientTopColor);
            this.ultraGroupBox2.Controls.Add(this.ultraCmbSingleBarColor);
            this.ultraGroupBox2.Controls.Add(this.ultraCmbSeriesColor);
            this.ultraGroupBox2.Controls.Add(this.ultraCmbDownTickColor);
            this.ultraGroupBox2.Controls.Add(this.ultraCmbUpTickColor);
            this.ultraGroupBox2.Controls.Add(this.ultraCmbGridColor);
            this.ultraGroupBox2.Controls.Add(this.ultraCmbBackgroundColor);
            this.ultraGroupBox2.Controls.Add(this.ultraCmbForegroundColor);
            this.ultraGroupBox2.Controls.Add(this.lblGradientValueView);
            this.ultraGroupBox2.Controls.Add(this.lblGradentbottom);
            this.ultraGroupBox2.Controls.Add(this.lblGradientTop);
            this.ultraGroupBox2.Controls.Add(this.lblSingleBarColor);
            this.ultraGroupBox2.Controls.Add(this.lblSeriesColor);
            this.ultraGroupBox2.Controls.Add(this.lblDownTickColor);
            this.ultraGroupBox2.Controls.Add(this.lblUpTickColor);
            this.ultraGroupBox2.Controls.Add(this.lblGridColor);
            this.ultraGroupBox2.Controls.Add(this.lblBackColor);
            this.ultraGroupBox2.Controls.Add(this.lblForeColor);
            this.ultraGroupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraGroupBox2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.ultraGroupBox2.Location = new System.Drawing.Point(10, 10);
            this.ultraGroupBox2.Name = "ultraGroupBox2";
            this.ultraGroupBox2.Size = new System.Drawing.Size(444, 236);
            this.ultraGroupBox2.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.ultraGroupBox2.TabIndex = 21;
            this.ultraGroupBox2.Text = "Color Settings";
            // 
            // ultraCmbGradientValueViewColor
            // 
            this.ultraCmbGradientValueViewColor.Color = System.Drawing.Color.Empty;
            this.ultraCmbGradientValueViewColor.Location = new System.Drawing.Point(364, 122);
            this.ultraCmbGradientValueViewColor.Name = "ultraCmbGradientValueViewColor";
            this.ultraCmbGradientValueViewColor.Size = new System.Drawing.Size(64, 22);
            this.ultraCmbGradientValueViewColor.TabIndex = 39;
            this.ultraCmbGradientValueViewColor.Visible = false;
            this.ultraCmbGradientValueViewColor.ColorChanged += new System.EventHandler(this.ColorChanged);
            // 
            // ultraCmbGradientBottomColor
            // 
            this.ultraCmbGradientBottomColor.Color = System.Drawing.Color.Empty;
            this.ultraCmbGradientBottomColor.Location = new System.Drawing.Point(364, 92);
            this.ultraCmbGradientBottomColor.Name = "ultraCmbGradientBottomColor";
            this.ultraCmbGradientBottomColor.Size = new System.Drawing.Size(64, 22);
            this.ultraCmbGradientBottomColor.TabIndex = 38;
            this.ultraCmbGradientBottomColor.ColorChanged += new System.EventHandler(this.ColorChanged);
            // 
            // ultraCmbGradientTopColor
            // 
            this.ultraCmbGradientTopColor.Color = System.Drawing.Color.Empty;
            this.ultraCmbGradientTopColor.Location = new System.Drawing.Point(364, 62);
            this.ultraCmbGradientTopColor.Name = "ultraCmbGradientTopColor";
            this.ultraCmbGradientTopColor.Size = new System.Drawing.Size(64, 22);
            this.ultraCmbGradientTopColor.TabIndex = 37;
            this.ultraCmbGradientTopColor.ColorChanged += new System.EventHandler(this.ColorChanged);
            // 
            // ultraCmbSingleBarColor
            // 
            this.ultraCmbSingleBarColor.Color = System.Drawing.Color.Empty;
            this.ultraCmbSingleBarColor.Location = new System.Drawing.Point(364, 32);
            this.ultraCmbSingleBarColor.Name = "ultraCmbSingleBarColor";
            this.ultraCmbSingleBarColor.Size = new System.Drawing.Size(64, 22);
            this.ultraCmbSingleBarColor.TabIndex = 36;
            this.ultraCmbSingleBarColor.ColorChanged += new System.EventHandler(this.ColorChanged);
            // 
            // ultraCmbSeriesColor
            // 
            this.ultraCmbSeriesColor.Color = System.Drawing.Color.Empty;
            this.ultraCmbSeriesColor.Location = new System.Drawing.Point(364, 152);
            this.ultraCmbSeriesColor.Name = "ultraCmbSeriesColor";
            this.ultraCmbSeriesColor.Size = new System.Drawing.Size(64, 22);
            this.ultraCmbSeriesColor.TabIndex = 35;
            this.ultraCmbSeriesColor.Visible = false;
            this.ultraCmbSeriesColor.ColorChanged += new System.EventHandler(this.ColorChanged);
            // 
            // ultraCmbDownTickColor
            // 
            this.ultraCmbDownTickColor.Color = System.Drawing.Color.Empty;
            this.ultraCmbDownTickColor.Location = new System.Drawing.Point(122, 150);
            this.ultraCmbDownTickColor.Name = "ultraCmbDownTickColor";
            this.ultraCmbDownTickColor.Size = new System.Drawing.Size(64, 22);
            this.ultraCmbDownTickColor.TabIndex = 34;
            this.ultraCmbDownTickColor.ColorChanged += new System.EventHandler(this.ColorChanged);
            // 
            // ultraCmbUpTickColor
            // 
            this.ultraCmbUpTickColor.Color = System.Drawing.Color.Empty;
            this.ultraCmbUpTickColor.Location = new System.Drawing.Point(122, 120);
            this.ultraCmbUpTickColor.Name = "ultraCmbUpTickColor";
            this.ultraCmbUpTickColor.Size = new System.Drawing.Size(64, 22);
            this.ultraCmbUpTickColor.TabIndex = 33;
            this.ultraCmbUpTickColor.ColorChanged += new System.EventHandler(this.ColorChanged);
            // 
            // ultraCmbGridColor
            // 
            this.ultraCmbGridColor.Color = System.Drawing.Color.Empty;
            this.ultraCmbGridColor.Location = new System.Drawing.Point(122, 90);
            this.ultraCmbGridColor.Name = "ultraCmbGridColor";
            this.ultraCmbGridColor.Size = new System.Drawing.Size(64, 22);
            this.ultraCmbGridColor.TabIndex = 32;
            this.ultraCmbGridColor.ColorChanged += new System.EventHandler(this.ColorChanged);
            // 
            // ultraCmbBackgroundColor
            // 
            this.ultraCmbBackgroundColor.Color = System.Drawing.Color.Empty;
            this.ultraCmbBackgroundColor.Location = new System.Drawing.Point(122, 60);
            this.ultraCmbBackgroundColor.Name = "ultraCmbBackgroundColor";
            this.ultraCmbBackgroundColor.Size = new System.Drawing.Size(66, 22);
            this.ultraCmbBackgroundColor.TabIndex = 31;
            this.ultraCmbBackgroundColor.ColorChanged += new System.EventHandler(this.ColorChanged);
            // 
            // ultraCmbForegroundColor
            // 
            this.ultraCmbForegroundColor.Color = System.Drawing.Color.Empty;
            this.ultraCmbForegroundColor.Location = new System.Drawing.Point(122, 28);
            this.ultraCmbForegroundColor.Name = "ultraCmbForegroundColor";
            this.ultraCmbForegroundColor.Size = new System.Drawing.Size(66, 22);
            this.ultraCmbForegroundColor.TabIndex = 30;
            this.ultraCmbForegroundColor.ColorChanged += new System.EventHandler(this.ColorChanged);
            // 
            // lblGradientValueView
            // 
            this.lblGradientValueView.Location = new System.Drawing.Point(198, 124);
            this.lblGradientValueView.Name = "lblGradientValueView";
            this.lblGradientValueView.Size = new System.Drawing.Size(156, 16);
            this.lblGradientValueView.TabIndex = 29;
            this.lblGradientValueView.Text = "Gradient Value View Color";
            this.lblGradientValueView.Visible = false;
            // 
            // lblGradentbottom
            // 
            this.lblGradentbottom.Location = new System.Drawing.Point(198, 94);
            this.lblGradentbottom.Name = "lblGradentbottom";
            this.lblGradentbottom.Size = new System.Drawing.Size(156, 16);
            this.lblGradentbottom.TabIndex = 28;
            this.lblGradentbottom.Text = "Gradiet Bottom Color";
            // 
            // lblGradientTop
            // 
            this.lblGradientTop.Location = new System.Drawing.Point(198, 64);
            this.lblGradientTop.Name = "lblGradientTop";
            this.lblGradientTop.Size = new System.Drawing.Size(152, 16);
            this.lblGradientTop.TabIndex = 27;
            this.lblGradientTop.Text = "Gradient Top Color";
            // 
            // lblSingleBarColor
            // 
            this.lblSingleBarColor.Location = new System.Drawing.Point(200, 36);
            this.lblSingleBarColor.Name = "lblSingleBarColor";
            this.lblSingleBarColor.Size = new System.Drawing.Size(154, 16);
            this.lblSingleBarColor.TabIndex = 26;
            this.lblSingleBarColor.Text = "SingleBarColor";
            // 
            // lblSeriesColor
            // 
            this.lblSeriesColor.Location = new System.Drawing.Point(198, 154);
            this.lblSeriesColor.Name = "lblSeriesColor";
            this.lblSeriesColor.Size = new System.Drawing.Size(156, 16);
            this.lblSeriesColor.TabIndex = 25;
            this.lblSeriesColor.Text = "Series Color";
            this.lblSeriesColor.Visible = false;
            // 
            // lblDownTickColor
            // 
            this.lblDownTickColor.Location = new System.Drawing.Point(14, 154);
            this.lblDownTickColor.Name = "lblDownTickColor";
            this.lblDownTickColor.Size = new System.Drawing.Size(98, 16);
            this.lblDownTickColor.TabIndex = 24;
            this.lblDownTickColor.Text = "Down Tick Color";
            // 
            // lblUpTickColor
            // 
            this.lblUpTickColor.Location = new System.Drawing.Point(14, 122);
            this.lblUpTickColor.Name = "lblUpTickColor";
            this.lblUpTickColor.Size = new System.Drawing.Size(98, 16);
            this.lblUpTickColor.TabIndex = 23;
            this.lblUpTickColor.Text = "Up Tick Color";
            // 
            // lblGridColor
            // 
            this.lblGridColor.Location = new System.Drawing.Point(14, 92);
            this.lblGridColor.Name = "lblGridColor";
            this.lblGridColor.Size = new System.Drawing.Size(98, 16);
            this.lblGridColor.TabIndex = 22;
            this.lblGridColor.Text = "Grid Color";
            // 
            // lblBackColor
            // 
            this.lblBackColor.Location = new System.Drawing.Point(14, 64);
            this.lblBackColor.Name = "lblBackColor";
            this.lblBackColor.Size = new System.Drawing.Size(98, 16);
            this.lblBackColor.TabIndex = 21;
            this.lblBackColor.Text = "Back Color";
            // 
            // lblForeColor
            // 
            this.lblForeColor.Location = new System.Drawing.Point(14, 34);
            this.lblForeColor.Name = "lblForeColor";
            this.lblForeColor.Size = new System.Drawing.Size(98, 16);
            this.lblForeColor.TabIndex = 20;
            this.lblForeColor.Text = "Fore Color";
            // 
            // ChartsColorPreferenceControl
            // 
            this.Controls.Add(this.ultraGroupBox2);
            this.DockPadding.All = 10;
            this.Name = "ChartsColorPreferenceControl";
            this.Size = new System.Drawing.Size(464, 256);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox2)).EndInit();
            this.ultraGroupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraCmbGradientValueViewColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCmbGradientBottomColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCmbGradientTopColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCmbSingleBarColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCmbSeriesColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCmbDownTickColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCmbUpTickColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCmbGridColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCmbBackgroundColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCmbForegroundColor)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        #region Public Properties

        private Color _foreColor;
        public Color ChartsForegroundColor
        {
            get { return ultraCmbForegroundColor.Color; }
            set
            {
                _foreColor = value;
                ultraCmbForegroundColor.Color = _foreColor;
            }
        }

        private Color _backColor;
        public Color ChartsBackgroundColor
        {
            get { return ultraCmbBackgroundColor.Color; }
            set
            {
                _backColor = value;
                ultraCmbBackgroundColor.Color = _backColor;
            }
        }



        private Color _gridColor;
        public Color ChartsGridColor
        {
            get
            { return ultraCmbGridColor.Color; }
            set
            {
                _gridColor = value;
                ultraCmbGridColor.Color = _gridColor;
            }
        }

        private Color _upTickColor;
        public Color ChartsUpTickColor
        {
            get
            { return ultraCmbUpTickColor.Color; }
            set
            {
                _upTickColor = value;
                ultraCmbUpTickColor.Color = _upTickColor;
            }
        }

        private Color _downTickColor;
        public Color ChartsDownTickColor
        {
            get
            { return ultraCmbDownTickColor.Color; }
            set
            {
                _downTickColor = value;
                ultraCmbDownTickColor.Color = _downTickColor;
            }
        }

        //		private Color _seriesColor = Color.Transparent;
        //		public Color SeriesColor
        //		{
        //			get 
        //			{ return ultraCmbSeriesColor.Color;}
        //			set
        //			{
        //				_seriesColor = value;
        //				ultraCmbSeriesColor.Color = _seriesColor;
        //			}
        //		}

        private Color _singleBarColor;
        public Color ChartsSingleBarColor
        {
            get
            { return ultraCmbSingleBarColor.Color; }
            set
            {
                _singleBarColor = value;
                ultraCmbSingleBarColor.Color = _singleBarColor;
            }
        }

        private Color _gradientTopColor;
        public Color ChartsGradientTopColor
        {
            get
            { return ultraCmbGradientTopColor.Color; }
            set
            {
                _gradientTopColor = value;
                ultraCmbGradientTopColor.Color = _gradientTopColor;
            }
        }

        private Color _gradientBottomColor;
        public Color ChartsGradientBottomColor
        {
            get
            { return ultraCmbGradientBottomColor.Color; }
            set
            {
                _gradientBottomColor = value;
                ultraCmbGradientBottomColor.Color = _gradientBottomColor;
            }
        }

        //		private Color _gradientValueViewColor = Color.Transparent;
        //		public Color GradientValueViewColor
        //		{
        //			get 
        //			{ return ultraCmbGradientValueViewColor.Color;}
        //			set
        //			{
        //				_gradientValueViewColor = value;
        //				ultraCmbGradientValueViewColor.Color = _gradientValueViewColor;
        //			}
        //		}

        #endregion Public Properties

        /// <summary>
        /// Will set the background, foreground of the control of the same color as selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }



    }
}
