using Infragistics.Win;
using Infragistics.Win.UltraWinForm;
using System.Drawing;
using System.Windows.Forms;


namespace Prana.Utilities.UI.UIUtilities
{
    public class MainFormTitleHelper : IUIElementDrawFilter
    {
        private string leftText;
        private string _rightText;
        private Font currentFont;
        private Font titleFont;
        private Color _foreColor = Color.FromArgb(255, (byte)155, (byte)187, (byte)89);
        private int _rightTextMargin = 85;
        public string RightText
        {
            set { _rightText = value; }
        }
        public Color ForeColor
        {
            set { _foreColor = value; }
        }

        public MainFormTitleHelper(string companyName, string moduleName, Font titleFont, Font font)
        {
            // TODO: Complete member initialization
            this.leftText = companyName;
            this._rightText = moduleName;
            this.currentFont = font;
            this.titleFont = titleFont;
        }

        public MainFormTitleHelper(string companyName, string moduleName, Font titleFont, Font font, int rightTextMargin)
            : this(companyName, moduleName, titleFont, font)
        {
            this._rightTextMargin = rightTextMargin;
        }

        public bool DrawElement(DrawPhase drawPhase, ref UIElementDrawParams drawParams)
        {
            UltraFormCaptionUIElement caption = drawParams.Element as UltraFormCaptionUIElement;
            if (null == caption)
            {
                return false;
            }

            Rectangle captionRect = caption.Rect;
            Size leftTextSize = TextRenderer.MeasureText(this.leftText, this.titleFont);

            Size rightTextSize = TextRenderer.MeasureText(this._rightText, this.currentFont);
            if (leftTextSize.Width + rightTextSize.Width + 5 > captionRect.Width)
            {
                return false;
            }

            Point rightTextStartPoint = new Point(captionRect.Right - rightTextSize.Width - _rightTextMargin, captionRect.Y);
            drawParams.Graphics.DrawString(this._rightText, this.currentFont, new SolidBrush(_foreColor), rightTextStartPoint);

            return false;
        }

        public DrawPhase GetPhasesToFilter(ref UIElementDrawParams drawParams)
        {
            UltraFormCaptionUIElement caption = drawParams.Element as UltraFormCaptionUIElement;
            if (caption != null)
            {
                return DrawPhase.BeforeDrawChildElements;
            }

            return DrawPhase.None;
        }
    }
    public class FormTitleHelper : IUIElementDrawFilter
    {
        private string leftText;
        private string rightText;
        private Font currentFont;

        public FormTitleHelper(string companyName, string moduleName, Font font)
        {
            // TODO: Complete member initialization
            this.leftText = companyName;
            this.rightText = moduleName;
            this.currentFont = font;
        }


        public bool DrawElement(DrawPhase drawPhase, ref UIElementDrawParams drawParams)
        {
            UltraFormCaptionUIElement caption = drawParams.Element as UltraFormCaptionUIElement;
            if (null == caption)
            {
                return false;
            }

            Rectangle captionRect = caption.Rect;
            Size leftTextSize = TextRenderer.MeasureText(this.leftText, this.currentFont);

            Size rightTextSize = TextRenderer.MeasureText(this.rightText, this.currentFont);
            if (leftTextSize.Width + rightTextSize.Width + 5 > captionRect.Width)
            {
                return false;
            }

            Point rightTextStartPoint = new Point(captionRect.Right - rightTextSize.Width, captionRect.Y);
            drawParams.Graphics.DrawString(this.rightText, this.currentFont, new SolidBrush(Color.FromArgb(255, (byte)155, (byte)187, (byte)89)), rightTextStartPoint);

            return false;
        }

        public DrawPhase GetPhasesToFilter(ref UIElementDrawParams drawParams)
        {
            UltraFormCaptionUIElement caption = drawParams.Element as UltraFormCaptionUIElement;
            if (caption != null)
            {
                return DrawPhase.BeforeDrawChildElements;
            }

            return DrawPhase.None;
        }
    }
}
