using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Prana.WatchList.Classes
{
    public class OptionChainDataModel : INotifyPropertyChanged
    {
        private string _underlyingSymbol = string.Empty;
        public string UnderlyingSymbol
        {
            get { return _underlyingSymbol; }
            set
            {
                if (_underlyingSymbol != value)
                    _underlyingSymbol = value;
            }
        }

        private DateTime _expirationDate;
        public DateTime ExpirationDate
        {
            get { return _expirationDate; }
            set
            {
                if (_expirationDate != value)
                    _expirationDate = value;
            }
        }

        private bool _call_select = false;
        public bool Call_Select
        {
            get { return _call_select; }
            set
            {
                if (_call_select != value)
                {
                    _call_select = value;
                    PropertyHasChanged();
                }
            }
        }

        private string _call_symbol = string.Empty;
        public string Call_Symbol
        {
            get { return _call_symbol; }
            set
            {
                if (_call_symbol != value)
                    _call_symbol = value;
            }
        }

        private string _call_tick = "NO_TICK";
        public string Call_Tick
        {
            get { return _call_tick; }
            set
            {
                if (_call_tick != value)
                    _call_tick = value;
            }
        }

        private double? _call_last;
        public double? Call_Last
        {
            get { return _call_last; }
            set
            {
                if (_call_last != value)
                {
                    _call_last = value;
                    PropertyHasChanged();
                }
            }
        }

        private double? _call_change;
        public double? Call_Change
        {
            get { return _call_change; }
            set
            {
                if (_call_change != value)
                {
                    _call_change = value;
                    PropertyHasChanged();
                }
            }
        }

        private double? _call_bid;
        public double? Call_Bid
        {
            get { return _call_bid; }
            set
            {
                if (_call_bid != value)
                {
                    _call_bid = value;
                    PropertyHasChanged();
                }
            }
        }

        private double? _call_ask;
        public double? Call_Ask
        {
            get { return _call_ask; }
            set
            {
                if (_call_ask != value)
                {
                    _call_ask = value;
                    PropertyHasChanged();
                }
            }
        }

        private double? _call_volume;
        public double? Call_Volume
        {
            get { return _call_volume; }
            set
            {
                if (_call_volume != value)
                {
                    _call_volume = value;
                    PropertyHasChanged();
                }
            }
        }

        private double? _call_openInt;
        public double? Call_OpenInt
        {
            get { return _call_openInt; }
            set
            {
                if (_call_openInt != value)
                {
                    _call_openInt = value;
                    PropertyHasChanged();
                }
            }
        }

        private double _strikePrice;
        public double StrikePrice
        {
            get { return _strikePrice; }
            set
            {
                if (_strikePrice != value)
                {
                    _strikePrice = value;
                    PropertyHasChanged();
                }
            }
        }

        private bool _put_select = false;
        public bool Put_Select
        {
            get { return _put_select; }
            set
            {
                if (_put_select != value)
                {
                    _put_select = value;
                    PropertyHasChanged();
                }
            }
        }

        private string _put_symbol = string.Empty;
        public string Put_Symbol
        {
            get { return _put_symbol; }
            set
            {
                if (_put_symbol != value)
                    _put_symbol = value;
            }
        }

        private string _put_tick = "NO_TICK";
        public string Put_Tick
        {
            get { return _put_tick; }
            set
            {
                if (_put_tick != value)
                    _put_tick = value;
            }
        }

        private double? _put_last;
        public double? Put_Last
        {
            get { return _put_last; }
            set
            {
                if (_put_last != value)
                {
                    _put_last = value;
                    PropertyHasChanged();
                }
            }
        }

        private double? _put_change;
        public double? Put_Change
        {
            get { return _put_change; }
            set
            {
                if (_put_change != value)
                {
                    _put_change = value;
                    PropertyHasChanged();
                }
            }
        }

        private double? _put_bid;
        public double? Put_Bid
        {
            get { return _put_bid; }
            set
            {
                if (_put_bid != value)
                {
                    _put_bid = value;
                    PropertyHasChanged();
                }
            }
        }

        private double? _put_ask;
        public double? Put_Ask
        {
            get { return _put_ask; }
            set
            {
                if (_put_ask != value)
                {
                    _put_ask = value;
                    PropertyHasChanged();
                }
            }
        }

        private double? _put_volume;
        public double? Put_Volume
        {
            get { return _put_volume; }
            set
            {
                if (_put_volume != value)
                {
                    _put_volume = value;
                    PropertyHasChanged();
                }
            }
        }

        private double? _put_openInt;
        public double? Put_OpenInt
        {
            get { return _put_openInt; }
            set
            {
                if (_put_openInt != value)
                {
                    _put_openInt = value;
                    PropertyHasChanged();
                }
            }
        }

        #region Other Symbologies
        private string _call_bloombergSymbol = string.Empty;
        public string Call_BloombergSymbol
        {
            get { return _call_bloombergSymbol; }
            set
            {
                if (_call_bloombergSymbol != value)
                    _call_bloombergSymbol = value;
            }
        }

        private string _put_bloombergSymbol = string.Empty;
        public string Put_BloombergSymbol
        {
            get { return _put_bloombergSymbol; }
            set
            {
                if (_put_bloombergSymbol != value)
                    _put_bloombergSymbol = value;
            }
        }

        private string _call_factsetSymbol = string.Empty;
        public string Call_FactSetSymbol
        {
            get { return _call_factsetSymbol; }
            set
            {
                if (_call_factsetSymbol != value)
                    _call_factsetSymbol = value;
            }
        }

        private string _put_factsetSymbol = string.Empty;
        public string Put_FactSetSymbol
        {
            get { return _put_factsetSymbol; }
            set
            {
                if (_put_factsetSymbol != value)
                    _put_factsetSymbol = value;
            }
        }

        private string _call_activSymbol = string.Empty;
        public string Call_ActivSymbol
        {
            get { return _call_activSymbol; }
            set
            {
                if (_call_activSymbol != value)
                    _call_activSymbol = value;
            }
        }

        private string _put_activeSymbol = string.Empty;
        public string Put_ActivSymbol
        {
            get { return _put_activeSymbol; }
            set
            {
                if (_put_activeSymbol != value)
                    _put_activeSymbol = value;
            }
        }
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        public void PropertyHasChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(propertyName, null);
            }
        }
        #endregion
    }
}
