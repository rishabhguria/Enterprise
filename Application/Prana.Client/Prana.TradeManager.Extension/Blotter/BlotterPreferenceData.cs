using Prana.BusinessObjects;
using System;
using System.Text;
using System.Xml.Serialization;

namespace Prana.TradeManager.Extension
{
    [Serializable]
    public class BlotterPreferenceData : IPreferenceData
    {
        public BlotterPreferenceData()
        {
        }

        private System.Drawing.Color _buyOrder = System.Drawing.Color.FromArgb(255, 128, 0);
        private System.Drawing.Color _coverOrder = System.Drawing.Color.FromArgb(255, 128, 0);
        private System.Drawing.Color _sellOrder = System.Drawing.Color.DeepSkyBlue;
        private System.Drawing.Color _shortOrder = System.Drawing.Color.DeepSkyBlue;

        private RGB _buyOrderRGB = new RGB();
        private RGB _coverOrderRGB = new RGB();
        private RGB _sellOrderRGB = new RGB();
        private RGB _shortOrderRGB = new RGB();

        /// <summary>
        /// The default export path
        /// </summary>
        private string _defaultExportPath;
        /// <summary>
        /// Gets or sets the default export path.
        /// </summary>
        /// <value>
        /// The default export path.
        /// </value>
        public string DefaultExportPath
        {
            get { return _defaultExportPath; }
            set { _defaultExportPath = value; }
        }

        private string _defaultTabName = "Orders";
        public string DefaultTabName
        {
            get { return _defaultTabName; }
            set { _defaultTabName = value; }
        }

        private bool _applyColorPreferencesInTheme = true;
        public bool ApplyColorPreferencesInTheme
        {
            get { return _applyColorPreferencesInTheme; }
            set { _applyColorPreferencesInTheme = value; }
        }

        private bool _rejectionPopup = true;
        public bool RejectionPopup
        {
            get { return _rejectionPopup; }
            set { _rejectionPopup = value; }
        }

        private bool _wrapHeader = false;
        public bool WrapHeader
        {
            get { return _wrapHeader; }
            set { _wrapHeader = value; }
        }

        private void CopyColor(ref System.Drawing.Color color, RGB rgb)
        {
            color = System.Drawing.Color.FromArgb(rgb.R, rgb.G, rgb.B);
        }

        public RGB BuyOrderRGB
        {
            get { return _buyOrderRGB; }
            set
            {
                _buyOrderRGB = value;
                CopyColor(ref _buyOrder, _buyOrderRGB);
            }
        }

        public RGB CoverOrderRGB
        {
            get { return _coverOrderRGB; }
            set
            {
                _coverOrderRGB = value;
                CopyColor(ref _coverOrder, _coverOrderRGB);
            }
        }

        public RGB SellOrderRGB
        {
            get { return _sellOrderRGB; }
            set
            {
                _sellOrderRGB = value;
                CopyColor(ref _sellOrder, _sellOrderRGB);
            }
        }

        public RGB ShortOrderRGB
        {
            get { return _shortOrderRGB; }
            set
            {
                _shortOrderRGB = value;
                CopyColor(ref _shortOrder, _shortOrderRGB);
            }
        }

        [XmlIgnore]
        public System.Drawing.Color BuyOrder
        {
            get { return _buyOrder; }
            set
            {
                _buyOrder = value;
                _buyOrderRGB = new RGB(_buyOrder);
            }
        }

        [XmlIgnore]
        public System.Drawing.Color CoverOrder
        {
            get { return _coverOrder; }
            set
            {
                _coverOrder = value;
                _coverOrderRGB = new RGB(_coverOrder);
            }
        }

        [XmlIgnore]
        public System.Drawing.Color SellOrder
        {
            get { return _sellOrder; }
            set
            {
                _sellOrder = value;
                _sellOrderRGB = new RGB(_sellOrder);
            }
        }

        [XmlIgnore]
        public System.Drawing.Color ShortOrder
        {
            get { return _shortOrder; }
            set
            {
                _shortOrder = value;
                _shortOrderRGB = new RGB(_shortOrder);
            }
        }

        public string Serialize()
        {
            StringBuilder sb = new StringBuilder();
            string str = string.Empty;
            sb.Append(_buyOrderRGB.ToString());
            sb.Append(';');
            sb.Append(_coverOrderRGB.ToString());
            sb.Append(';');
            sb.Append(_sellOrderRGB.ToString());
            sb.Append(';');
            sb.Append(_shortOrderRGB.ToString());
            sb.Append(';');
            sb.Append(_applyColorPreferencesInTheme);
            sb.Append(';');
            sb.Append(_defaultTabName.ToString());
            sb.Append(';');
            sb.Append(_wrapHeader);
            sb.Append(';');
            sb.Append(_defaultExportPath);
            sb.Append(';');
            sb.Append(_rejectionPopup);
            str = sb.ToString();
            return str;
        }

        public void DeSerialize(string message)
        {
            if (message != null)
            {
                string[] str = message.Split(';');

                _buyOrderRGB.Deserialize(str[0]);
                CopyColor(ref _buyOrder, _buyOrderRGB);
                _coverOrderRGB.Deserialize(str[1]);
                CopyColor(ref _coverOrder, _coverOrderRGB);
                _sellOrderRGB.Deserialize(str[2]);
                CopyColor(ref _sellOrder, _sellOrderRGB);
                _shortOrderRGB.Deserialize(str[3]);
                CopyColor(ref _shortOrder, _shortOrderRGB);
                if (str.Length >= 5)
                {
                    if (!bool.TryParse(str[4], out _applyColorPreferencesInTheme))
                    {
                        _applyColorPreferencesInTheme = false;
                    }
                }

                if (str.Length >= 6)
                {
                    _defaultTabName = str[5];

                }
                if (str.Length >= 7)
                {
                    if (!bool.TryParse(str[6], out _wrapHeader))
                    {
                        _wrapHeader = false;
                    }
                }
                if (str.Length >= 8)
                    _defaultExportPath = str[7];
                if (str.Length >= 9)
                {
                    if (!bool.TryParse(str[8], out _rejectionPopup))
                    {
                        _rejectionPopup = false;
                    }
                }
            }
        }
    }

    [Serializable]
    public class RGB
    {
        private int _r = int.MinValue;
        private int _g = int.MinValue;
        private int _b = int.MinValue;

        public RGB(int r, int g, int b)
        {
            _r = r;
            _g = g;
            _b = b;
        }

        public RGB(System.Drawing.Color color)
        {
            _r = color.R;
            _g = color.G;
            _b = color.B;
        }

        public RGB()
        {
        }

        public int R
        {
            get { return _r; }
            set { _r = value; }
        }

        public int G
        {
            get { return _g; }
            set { _g = value; }
        }

        public int B
        {
            get { return _b; }
            set { _b = value; }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            string str = string.Empty;

            sb.Append(_r.ToString());
            sb.Append(',');
            sb.Append(_b.ToString());
            sb.Append(',');
            sb.Append(_g.ToString());

            str = sb.ToString();
            return str;
        }

        public void Deserialize(string message)
        {
            string[] str = message.Split(',');
            _r = int.Parse(str[0]);
            _b = int.Parse(str[1]);
            _g = int.Parse(str[2]);
        }
    }
}