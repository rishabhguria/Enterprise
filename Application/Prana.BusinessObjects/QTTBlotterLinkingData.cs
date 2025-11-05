using System;
using System.Drawing;

namespace Prana.BusinessObjects
{
    public class QTTBlotterLinkingData : EventArgs
    {
        public int Index { get; set; }
        public string Symbol { get; set; }
        public int BrokerID { get; set; }
        public int? AccountID { get; set; }
        public int? VenueID { get; set; }
        public Color ForeColor { get; set; }
        public Color BackColor { get; set; }

        public QTTBlotterLinkingData(int index, Color foreColor, Color backColor)
        {
            this.ForeColor = foreColor;
            this.BackColor = backColor;
            this.Index = index;
            this.Symbol = string.Empty;
        }
    }
}
