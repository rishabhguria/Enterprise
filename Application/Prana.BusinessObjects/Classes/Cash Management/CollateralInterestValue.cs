using System;
using System.ComponentModel;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class CollateralInterestValue : IKeyable, IFilterable, INotifyPropertyChangedCustom
    {
        #region Private Fields

        private int _ciID = 0;
        private int _AccountID;
        private string _BenchmarkName = string.Empty;
        private decimal _BenchmarkRate = 0;
        private int _Spread = 0;
        private DateTime _Date;

        #endregion

        #region GetSetSection

        public virtual int CollateralInterestId
        {
            get { return _ciID; }
            set { _ciID = value; }
        }

        [Browsable(false)]
        public virtual int AccountID
        {
            get { return _AccountID; }
            set { _AccountID = value; }
        }

        public virtual string BenchmarkName
        {
            get { return _BenchmarkName; }
            set { _BenchmarkName = value; }
        }

        public virtual decimal BenchmarkRate
        {
            get { return _BenchmarkRate; }
            set { _BenchmarkRate = value; }
        }

        public virtual int Spread
        {
            get { return _Spread; }
            set { _Spread = value; }
        }

        public virtual DateTime Date
        {
            get { return _Date; }
            set { _Date = value; }
        }

        #endregion

        #region IKeyable Members

        public virtual string GetKey()
        {
            return _AccountID.ToString() + ':' + _BenchmarkName.ToString();
        }

        public virtual void Update(IKeyable item)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IFilterable Members

        public virtual DateTime GetDate()
        {
            return Date;
        }

        public virtual DateTime GetDateModified()
        {
            return Date;
        }
        public virtual string GetSymbol()
        {
            return string.Empty;
        }
        public virtual int GetAccountID()
        {
            return AccountID;
        }
        #endregion

        #region INotifyPropertyChangedCustom Members

        [field: NonSerialized]
        public virtual event PropertyChangedEventHandler PropertyChanged;

        public virtual void PropertyHasChanged()
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, null);
            }
        }
        #endregion


    }
}
