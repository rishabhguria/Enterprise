using Prana.BusinessObjects.AppConstants;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class CompanyAccountCashCurrencyValue : IKeyable, IFilterable, INotifyPropertyChangedCustom
    {
        #region Private Fields


        private int _id = 0;
        private int _AccountID;
        private string _AccountName = string.Empty;

        private DateTime _Date;

        private int _BaseCurrencyID;


        #endregion

        #region GetSetSection


        public virtual int CashCurrencyID
        {
            get { return _id; }
            set { _id = value; }
        }

        [Browsable(false)]
        public virtual int AccountID
        {
            get { return _AccountID; }
            set { _AccountID = value; }
        }
        public virtual string AccountName
        {
            get { return _AccountName; }
            set { _AccountName = value; }
        }

        public virtual DateTime Date
        {
            get { return _Date; }
            set { _Date = value; }
        }

        [Browsable(false)]
        public virtual int BaseCurrencyID
        {
            get { return _BaseCurrencyID; }
            set { _BaseCurrencyID = value; }
        }

        private string _baseCurrencyName;

        public virtual string BaseCurrencyName
        {
            get { return _baseCurrencyName; }
            set { _baseCurrencyName = value; }
        }

        private decimal _CashValueBase;

        public virtual decimal CashValueBase
        {
            get { return _CashValueBase; }
            set { _CashValueBase = value; }
        }

        private int _LocalCurrencyID;
        [Browsable(false)]
        public virtual int LocalCurrencyID
        {
            get { return _LocalCurrencyID; }
            set { _LocalCurrencyID = value; }
        }

        private string _LocalCurrencyName;

        public virtual string LocalCurrencyName
        {
            get { return _LocalCurrencyName; }
            set { _LocalCurrencyName = value; }
        }


        private decimal _CashValueLocal;

        public virtual decimal CashValueLocal
        {
            get { return _CashValueLocal; }
            set { _CashValueLocal = value; }
        }

        private List<TaxLot> _taxLots = new List<TaxLot>();

        public virtual List<TaxLot> TaxLots
        {
            get { return _taxLots; }
            set { _taxLots = value; }

        }

        private BalanceType _balanceType = BalanceType.Cash;

        public virtual BalanceType BalanceType
        {
            get { return _balanceType; }
            set { _balanceType = value; }
        }


        //private string _symbol;

        //public virtual string Symbol
        //{
        //    get { return _symbol; }
        //    set { _symbol = value; }
        //}

        #endregion

        #region IKeyable Members

        public virtual string GetKey()
        {
            return _AccountID.ToString() + ':' + _LocalCurrencyID.ToString();
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
