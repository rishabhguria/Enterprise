using System.Collections.Generic;
using System.ComponentModel;
using Nirvana.Middleware;

namespace CSBatchUI
{
    public class UpperCaseComparer : IEqualityComparer<string>
    {
        public bool Equals(string x, string y)
        {
            return x.ToUpper().Equals(y.ToUpper());
        }

        public int GetHashCode(string obj)
        {
            return obj.ToUpper().GetHashCode();
        }
    }

    public class FundUI :
        INotifyPropertyChanged
    {
        public Fund Fund { get; set; }

        public bool Selected { get; set; }

        public List<string> Symbols { get; set; }

        private string _Text;

        public string Text
        {
            get { return this._Text; }
            set
            {
                if (this._Text == value)
                    return;

                this._Text = value;
                this.FirePropertyChanged("Text");
            }
        }

        private int _SelectionStart;

        public int SelectionStart
        {
            get { return this._SelectionStart; }
            set { this._SelectionStart = value; }
        }

        protected void FirePropertyChanged(string propertyName)
        {
            if (this.propertyChangedDelegate != null)
                this.propertyChangedDelegate(this, new PropertyChangedEventArgs(propertyName));
        }

        private PropertyChangedEventHandler propertyChangedDelegate;

        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                if (this.propertyChangedDelegate == null)
                    this.propertyChangedDelegate = value;
                else
                    this.propertyChangedDelegate = (PropertyChangedEventHandler)System.Delegate.Combine(this.propertyChangedDelegate, value);
            }
            remove
            {
                if (this.propertyChangedDelegate != null)
                    this.propertyChangedDelegate = (PropertyChangedEventHandler)System.Delegate.Remove(this.propertyChangedDelegate, value);
            }
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
