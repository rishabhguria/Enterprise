using System;
using System.Drawing;
using System.Text;

namespace Prana.ClientPreferences
{
    [Serializable]
    public class QuickTTPrefs
    {
        private string[] _instanceNames = new string[10];
        public string[] InstanceNames
        {
            get
            {
                return _instanceNames;
            }
        }

        private Color[] _instanceForeColors = new Color[10];
        public Color[] InstanceForeColors
        {
            get
            {
                return _instanceForeColors;
            }
        }

        private Color[] _instanceBackColors = new Color[10];
        public Color[] InstanceBackColors
        {
            get
            {
                return _instanceBackColors;
            }
        }

        private int[] _hotButtonQuantities = new int[3];
        public int[] HotButtonQuantities
        {
            get
            {
                return _hotButtonQuantities;
            }
        }

        public bool UseAccountForLinking { get; set; }

        public bool UseVenueForLinking { get; set; }

        private string GetStringFromColor(Color color)
        {
            return color.R + "~" + color.G + "~" + color.B;
        }

        private Color GetColorFromString(string rgb)
        {
            string[] arr = rgb.Split('~');
            if (arr.Length == 3)
            {
                return Color.FromArgb(Convert.ToInt32(arr[0]), Convert.ToInt32(arr[1]), Convert.ToInt32(arr[2]));
            }
            return Color.Transparent;
        }

        public string Serialize()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 10; i++)
            {
                sb.Append(_instanceNames[i]);
                sb.Append(';');
                sb.Append(GetStringFromColor(_instanceForeColors[i]));
                sb.Append(';');
                sb.Append(GetStringFromColor(_instanceBackColors[i]));
                sb.Append(';');
            }
            sb.Append(_hotButtonQuantities[0].ToString());
            sb.Append(';');
            sb.Append(_hotButtonQuantities[1].ToString());
            sb.Append(';');
            sb.Append(_hotButtonQuantities[2].ToString());
            sb.Append(';');
            sb.Append(UseAccountForLinking.ToString());
            sb.Append(';');
            sb.Append(UseVenueForLinking.ToString());
            return sb.ToString();
        }

        public void DeSerialize(string message)
        {
            if (message != null)
            {
                string[] str = message.Split(';');
                if (str.Length >= 35)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        int j = i * 3;
                        _instanceNames[i] = str[j].Trim();
                        _instanceForeColors[i] = GetColorFromString(str[j + 1].Trim());
                        _instanceBackColors[i] = GetColorFromString(str[j + 2].Trim());
                    }
                    _hotButtonQuantities[0] = Convert.ToInt32(str[30]);
                    _hotButtonQuantities[1] = Convert.ToInt32(str[31]);
                    _hotButtonQuantities[2] = Convert.ToInt32(str[32]);
                    UseAccountForLinking = Convert.ToBoolean(str[33]);
                    UseVenueForLinking = Convert.ToBoolean(str[34]);
                }
            }
        }

        public QuickTTPrefs()
        {
            for (int i = 0; i < 10; i++)
            {
                _instanceNames[i] = "Quick TT(" + (i + 1) + ")";
                _instanceForeColors[i] = Color.FromArgb(155, 187, 89);
                _instanceBackColors[i] = Color.FromArgb(33, 44, 57);
            }
            _hotButtonQuantities[0] = 100;
            _hotButtonQuantities[1] = 1000;
            _hotButtonQuantities[2] = 10000;
        }

    }
}
