using Prana.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Prana.SM.OTC
{
    public class SecMasterOTCDataModel : BindableBase
    {
        /// <summary>
        /// Id
        /// </summary>
        private int id;
        public int Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }

        private Visibility isEquitySwapVisible = Visibility.Visible;
        public Visibility IsEquitySwapVisible
        {
            get { return isEquitySwapVisible; }
            set
            {
                isEquitySwapVisible = value;
                OnPropertyChanged("IsEquitySwapVisible");
            }
        }

        private Visibility isConvertibleBondVisible = Visibility.Collapsed;
        public Visibility IsConvertibleBondVisible
        {
            get { return isConvertibleBondVisible; }
            set
            {
                isConvertibleBondVisible = value;
                OnPropertyChanged("IsConvertibleBondVisible");
            }
        }


        /// <summary>
        /// ISDACounterParty
        /// </summary>
        private KeyValuePair<int, string> currency;
        public KeyValuePair<int, string> Currency
        {
            get { return currency; }
            set
            {
                currency = value;
                OnPropertyChanged("Currency");
            }
        }

        private Visibility isDeleteButtonVisible = Visibility.Visible;
        public Visibility IsDeleteButtonVisible
        {
            get { return isDeleteButtonVisible; }
            set
            {
                isDeleteButtonVisible = value;
                OnPropertyChanged("IsDeleteButtonVisible");
            }
        }

        private string _symbol;
        public string Symbol
        {
            get { return _symbol; }
            set
            {
                _symbol = value;
                OnPropertyChanged("Symbol");

            }
        }


        private string _uniqueIdentifier;
        public string UniqueIdentifier
        {
            get { return _uniqueIdentifier; }
            set
            {
                _uniqueIdentifier = value;
                OnPropertyChanged("UniqueIdentifier");

            }
        }

        private Visibility isEquityCFDVisible = Visibility.Collapsed;
        public Visibility IsEquityCFDVisible
        {
            get { return isEquityCFDVisible; }
            set
            {
                isEquityCFDVisible = value;
                OnPropertyChanged("IsEquityCFDVisible");
            }
        }



        /// <summary>
        /// Name
        /// </summary>
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }


        /// <summary>
        /// InstrumentType
        /// </summary>
        private InstrumentType instrumentType;
        public InstrumentType InstrumentType
        {
            get { return instrumentType; }
            set
            {
                instrumentType = value;
                OnPropertyChanged("InstrumentType");
            }
        }

        /// <summary>
        /// InstrumentType
        /// </summary>
        private string description;
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged("Description");
            }
        }

        /// <summary>
        /// UnderlyingAssetID
        /// </summary>
        private AssetClass underlyingAssetID;
        public AssetClass UnderlyingAssetID
        {
            get { return underlyingAssetID; }
            set
            {
                underlyingAssetID = value;
                OnPropertyChanged("UnderlyingAssetID");
            }
        }

        /// <summary>
        /// ISDACounterParty
        /// </summary>
        private KeyValuePair<int, string> iSDACounterParty;
        public KeyValuePair<int, string> ISDACounterParty
        {
            get { return iSDACounterParty; }
            set
            {
                iSDACounterParty = value;
                OnPropertyChanged("ISDACounterParty");
            }
        }


        private DateTime effectiveDate;
        public DateTime EffectiveDate
        {
            get { return effectiveDate; }
            set
            {
                effectiveDate = value;
                OnPropertyChanged("EffectiveDate");
            }
        }

        /// <summary>
        /// CreatedBy
        /// </summary>
        private string createdBy;
        public string CreatedBy
        {
            get { return createdBy; }
            set
            {
                createdBy = value;
                OnPropertyChanged("CreatedBy");
            }
        }

        /// <summary>
        /// CreationDate
        /// </summary>
        private DateTime creationDate = DateTime.Now;
        public DateTime CreationDate
        {
            get { return creationDate; }
            set
            {
                creationDate = value;
                OnPropertyChanged("CreationDate");
            }
        }

        /// <summary>
        /// LastModifiedBy
        /// </summary>
        private string lastModifiedBy;
        public string LastModifiedBy
        {
            get { return lastModifiedBy; }
            set
            {
                lastModifiedBy = value;
                OnPropertyChanged("LastModifiedBy");
            }
        }

        /// <summary>
        /// LastModifieDate
        /// </summary>
        private DateTime lastModifieDate = DateTime.Now;
        public DateTime LastModifieDate
        {
            get { return lastModifieDate; }
            set
            {
                lastModifieDate = value;
                OnPropertyChanged("LastModifieDate");
            }
        }

        /// <summary>
        /// ISDAContract
        /// </summary>
        private string iSDAContract;
        public string ISDAContract
        {
            get { return iSDAContract; }
            set
            {
                iSDAContract = value;
                OnPropertyChanged("ISDAContract");
            }
        }

        /// <summary>
        /// CreationDate
        /// </summary>
        private int daysToSettle;
        public int DaysToSettle
        {
            get { return daysToSettle; }
            set
            {
                daysToSettle = value;
                OnPropertyChanged("DaysToSettle");
            }
        }

        private KeyValuePair<int, string> _selectedCounterPart;
        public KeyValuePair<int, string> SelectedCounterPart
        {
            get { return _selectedCounterPart; }
            set
            {
                _selectedCounterPart = value;
                OnPropertyChanged("SelectedCounterPart");
            }
        }

        public string View { get; set; }

        public string Delete { get; set; }

        private DateTime tradeDate;

        public DateTime TradeDate
        {
            get { return tradeDate; }
            set
            {
                tradeDate = value;
                OnPropertyChanged("TradeDate");
            }
        }

        private string _sedol = string.Empty;
        public string Sedol
        {
            get { return _sedol; }
            set
            {
                _sedol = value;
                OnPropertyChanged("Sedol");
            }
        }


        private string _isin = string.Empty;
        public string Isin
        {
            get { return _isin; }
            set
            {
                _isin = value;
                OnPropertyChanged("Isin");
            }
        }

        private string _cusip = string.Empty;
        public string Cusip
        {
            get { return _cusip; }
            set
            {
                _cusip = value;
                OnPropertyChanged("Cusip");
            }
        }


        private string _selectedCurrency = string.Empty;
        public string SelectedCurrency
        {
            get { return _selectedCurrency; }
            set
            {
                _selectedCurrency = value;
                OnPropertyChanged("SelectedCurrency");
            }
        }



    }
}
