using System;
using System.Windows.Forms;

namespace Prana.Utilities.UI.UIUtilities
{
    /// <summary>
    /// The Label that always displays its text
    /// </summary>
    public class AlwaysFitLabel : Label
    {
        private float _maxFontSize = 16F; // Pick a default value...
        public float maxFontSize { get { return _maxFontSize; } set { _maxFontSize = value; } }

        public AlwaysFitLabel()
        {
            // Add a handler for the resize event, which we implement below
            this.Resize += new EventHandler(AlwaysFitLabel_Resize);
        }
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {   // always resize it when text changes...
                base.Text = value;
                AlwaysFitLabel_Resize(this, new EventArgs());
            }
        }
        void AlwaysFitLabel_Resize(object sender, EventArgs e)
        {
            // This function uses a temporary label to put the text with the largest font size,
            // see if it fits, and if not, keep knocking the font size down by .5 until it does
            // fit.  Then set the real label's font size to this newly calculated font size.
            Label lbl = new Label();
            float fntSize = maxFontSize;
            lbl.Text = this.Text;
            lbl.Font = new System.Drawing.Font(this.Font.FontFamily, fntSize);
            while (lbl.PreferredWidth > this.Width && fntSize > 0.6F)
            {
                fntSize -= 0.5F;
                lbl.Font = new System.Drawing.Font(this.Font.FontFamily, fntSize);
            }
            this.Font = new System.Drawing.Font(this.Font.FontFamily, fntSize);
            this.Height = this.PreferredHeight;
        }
    }



}
