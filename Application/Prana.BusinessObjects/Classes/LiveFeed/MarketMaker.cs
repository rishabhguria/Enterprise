using System.ComponentModel;
namespace Prana.BusinessObjects.LiveFeed
{
    public class MarketMaker : INotifyPropertyChanged
    {
        string _symbol = string.Empty;
        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }

        string _mmid = string.Empty;
        public string MMID
        {
            get { return _mmid; }
            set { _mmid = value; }
        }


        double _price = double.MinValue;
        public double Price
        {
            get { return _price; }
            set
            {
                if (value < 0)
                {
                    _price = 0;
                }
                else
                {
                    _price = value;
                }
            }
        }

        private long _size = int.MinValue;
        public long Size
        {
            get { return _size; }
            set
            {
                if (value < 0)
                {
                    _size = 0;
                }
                else
                {
                    _size = value;
                }
            }
        }

        private string _time;
        public string Time
        {
            get { return _time; }
            set
            {
                _time = value;
            }
        }

        private string _bidAsk;
        public string BidAsk
        {
            get { return _bidAsk; }
            set
            {
                _bidAsk = value;
            }
        }



        char _updateFlag = char.MinValue;
        /// <summary>
        /// Add, Delete and modify record
        /// </summary>
        public char UpdateFlag
        {
            get { return _updateFlag; }
            set { _updateFlag = value; }
        }





        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void PropertyHasChanged()
        {
            PropertyChanged(this, null);
        }


        #endregion
    }
}
