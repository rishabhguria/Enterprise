using System;

namespace Prana.BusinessObjects
{
    [Serializable(), System.Runtime.InteropServices.ComVisible(false)]
    public class DayMarkPrice : MarkPriceBase
    {
        [System.ComponentModel.Browsable(false)]
        public string DayMarkPriceDateString
        {
            get { return _dayMarkPriceDate.ToString("MMM dd yyyy HH:mm:ss:fff"); } 
        }

        private DateTime _dayMarkPriceDate = DateTime.Today.Date;
        [System.Xml.Serialization.XmlIgnore()]
        public DateTime DayMarkPriceDate
        {
            get { return _dayMarkPriceDate; }
            set
            {
                _dayMarkPriceDate = value;
                PropertyHasChanged();
            }
        }
    }
}
