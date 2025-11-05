using System;
using System.Drawing;
using System.Windows.Forms;

namespace Prana.Admin.Controls.Company
{
    /// <summary>
    /// A textbox that supports a watermak hint.
    /// </summary>
    public class WatermarkTextBox : TextBox
    {
        /// <summary>
        /// The text that will be presented as the watermak hint
        /// </summary>
        private string _watermarkText = "Search";
        /// <summary>
        /// Gets or Sets the text that will be presented as the watermak hint
        /// </summary>
        public string WatermarkText
        {
            get { return _watermarkText; }
            set { _watermarkText = value; }
        }

        /// <summary>
        /// Whether watermark effect is enabled or not
        /// </summary>
        private bool _watermarkActive = true;
        /// <summary>
        /// Gets or Sets whether watermark effect is enabled or not
        /// </summary>
        public bool WatermarkActive
        {
            get { return _watermarkActive; }
            set { _watermarkActive = value; }
        }

        /// <summary>
        /// Create a new TextBox that supports watermak hint
        /// </summary>
        public WatermarkTextBox()
        {
            this._watermarkActive = true;
            this.Text = _watermarkText;
            this.ForeColor = Color.Gray;

            this.GotFocus += new EventHandler(RemoveWatermak);
            this.LostFocus += new EventHandler(ApplyWatermark);

            //this.LostFocus += new EventHandler(WatermarkTextBox_LostFocus);

            //GotFocus += (source, e) =>
            //{
            //    RemoveWatermak();
            //};

            //LostFocus += (source, e) =>
            //{
            //    ApplyWatermark();
            //};

        }


        /// <summary>
        /// Remove watermark from the textbox
        /// </summary>
        public void RemoveWatermak(object sender, EventArgs e)
        {
            if (this._watermarkActive)
            {
                this._watermarkActive = false;
                this.Text = "";
                this.ForeColor = Color.Black;
            }
            else
                this.SelectAll();
        }

        /// <summary>
        /// Applywatermak immediately
        /// </summary>
        public void ApplyWatermark(object sender, EventArgs e)
        {
            if (!this._watermarkActive && string.IsNullOrEmpty(this.Text)
                || ForeColor == Color.Gray)
            {
                this._watermarkActive = true;
                this.Text = _watermarkText;
                this.ForeColor = Color.Gray;
            }
        }

        /// <summary>
        /// Apply watermak to the textbox. 
        /// </summary>
        /// <param name="newText">Text to apply</param>
        public void ApplyWatermark(string newText)
        {
            WatermarkText = newText;
            ApplyWatermark(this, null);
        }

    }
}
