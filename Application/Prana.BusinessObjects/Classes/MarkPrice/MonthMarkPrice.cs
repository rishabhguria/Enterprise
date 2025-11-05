using System;

namespace Prana.BusinessObjects
{
    [Serializable(), System.Runtime.InteropServices.ComVisible(false)]
    public class MonthMarkPrice : MarkPriceBase
    {
        [System.ComponentModel.Browsable(false)]
        public string MonthMarkPriceDateString
        {
            get { return _monthMarkPriceDate.ToString("MMM dd yyyy HH:mm:ss:fff"); }
        }

        private DateTime _monthMarkPriceDate = DateTime.Today.Date;
        [System.Xml.Serialization.XmlIgnore()]
        public DateTime MonthMarkPriceDate
        {
            get { return _monthMarkPriceDate; }
            set
            {
                _monthMarkPriceDate = value;
                PropertyHasChanged();
            }
        }

        private string _month;

        /// <summary>
        /// Gets or sets the month.
        /// </summary>
        /// <value>The month.</value>
        public string Month
        {
            get { return _month; }
            set { _month = value; }
        }

    }
}
