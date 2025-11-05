using Prana.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Prana.SM.OTC
{
    public class SecMasterConvertibleBondModel : BindableBase
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
        /// Collateral Margin
        /// </summary>
        private double _equityLeg_ConversionRatio;
        public double EquityLeg_ConversionRatio
        {
            get { return _equityLeg_ConversionRatio; }
            set
            {
                _equityLeg_ConversionRatio = value;
                OnPropertyChanged("EquityLeg_ConversionRatio");
            }
        }

        private Visibility isFieldVisibleBasedOnCoupon = Visibility.Visible;
        public Visibility IsFieldVisibleBasedOnCoupon
        {
            get { return isFieldVisibleBasedOnCoupon; }
            set
            {
                isFieldVisibleBasedOnCoupon = value;
                OnPropertyChanged("IsFieldVisibleBasedOnCoupon");
            }
        }

        /// <summary>
        /// EquityLeg Price
        /// </summary>
        private double _equityLeg_ConversionPrice;
        public double EquityLeg_ConversionPrice
        {
            get { return _equityLeg_ConversionPrice; }
            set
            {
                _equityLeg_ConversionPrice = value;
                OnPropertyChanged("EquityLeg_ConversionPrice");
            }
        }


        /// <summary>
        /// Collateral Margin
        /// </summary>
        private DateTime _equityLeg_ConversionDate;
        public DateTime EquityLeg_ConversionDate
        {
            get { return _equityLeg_ConversionDate; }
            set
            {
                _equityLeg_ConversionDate = value;
                OnPropertyChanged("EquityLeg_ConversionDate");
            }
        }


        private DateTime financeLeg_FirstPaymentDate = DateTimeConstants.MinValue;
        public DateTime FinanceLeg_FirstPaymentDate
        {
            get { return financeLeg_FirstPaymentDate; }
            set
            {
                financeLeg_FirstPaymentDate = value;
                OnPropertyChanged("FinanceLeg_FirstPaymentDate");
            }
        }



        private DateTime financeLeg_FirstResetDate = DateTimeConstants.MinValue;
        public DateTime FinanceLeg_FirstResetDate
        {
            get { return financeLeg_FirstResetDate; }
            set
            {
                financeLeg_FirstResetDate = value;
                OnPropertyChanged("FinanceLeg_FirstResetDate");
            }
        }


        private string financeLeg_ParValue = string.Empty;
        public string FinanceLeg_ParValue
        {
            get { return financeLeg_ParValue; }
            set
            {
                financeLeg_ParValue = value;
                OnPropertyChanged("FinanceLeg_ParValue");
            }
        }


        Visibility isTradeview = Visibility.Collapsed;
        public Visibility IsTradeView
        {
            get { return isTradeview; }
            set
            {
                isTradeview = value;
                OnPropertyChanged("IsTradeView");
            }
        }

        /// <summary>
        /// EquityLeg BulletSwap
        /// </summary>
        private bool financeLeg_ZeroCoupon;
        public bool FinanceLeg_ZeroCoupon
        {
            get { return financeLeg_ZeroCoupon; }
            set
            {
                financeLeg_ZeroCoupon = value;
                if (value)
                {
                    IsFinanceLeg_ZeroCoupon = Visibility.Collapsed;
                }
                else { IsFinanceLeg_ZeroCoupon = Visibility.Visible; }

                if ((IsTradeView == Visibility.Visible) && !value)
                {
                    IsFieldVisibleBasedOnCoupon = Visibility.Visible;
                }
                else
                {
                    IsFieldVisibleBasedOnCoupon = Visibility.Collapsed;
                }

                OnPropertyChanged("FinanceLeg_ZeroCoupon");
            }
        }

        /// <summary>
        /// IsFinanceLeg_ZeroCoupon 
        /// </summary>
        private Visibility isfinanceLeg_ZeroCoupon = Visibility.Visible;
        public Visibility IsFinanceLeg_ZeroCoupon
        {
            get { return isfinanceLeg_ZeroCoupon; }
            set { isfinanceLeg_ZeroCoupon = value; OnPropertyChanged("IsFinanceLeg_ZeroCoupon"); }
        }



        /// <summary>
        /// EquityLeg BulletSwap
        /// </summary>
        private PeriodFrequency financeLeg_IRBenchMark;
        public PeriodFrequency FinanceLeg_IRBenchMark
        {
            get { return financeLeg_IRBenchMark; }
            set { financeLeg_IRBenchMark = value; OnPropertyChanged("FinanceLeg_IRBenchMark"); }
        }


        /// <summary>
        /// fianace ScriptlendingFee
        /// </summary>
        private PeriodFrequency _financeLeg_CouponFreq;
        public PeriodFrequency FinanceLeg_CouponFreq
        {
            get { return _financeLeg_CouponFreq; }
            set { _financeLeg_CouponFreq = value; OnPropertyChanged("FinanceLeg_CouponFreq"); }
        }


        /// <summary>
        ///  Commissionbasis
        /// </summary>
        private CommisionType _commissionbasis;
        public CommisionType Commission_Basis
        {
            get { return _commissionbasis; }
            set { _commissionbasis = value; OnPropertyChanged("Commission_Basis"); }
        }


        /// <summary>
        ///  HardCommRate
        /// </summary>
        private double commission_HardCommRate;
        public double Commission_HardCommRate
        {
            get { return commission_HardCommRate; }
            set { commission_HardCommRate = value; OnPropertyChanged("Commission_HardCommRate"); }
        }


        /// <summary>
        /// fianace ScriptlendingFee
        /// </summary>
        private double _commission_SoftCommRate;
        public double Commission_SoftCommRate
        {
            get { return _commission_SoftCommRate; }
            set { _commission_SoftCommRate = value; OnPropertyChanged("Commission_SoftCommRate"); }
        }


        /// <summary>
        /// Fianace InteresrRatebenchmark
        /// </summary>
        private double _financeLeg_FXRate;
        public double FinanceLeg_FXRate
        {
            get { return _financeLeg_FXRate; }
            set { _financeLeg_FXRate = value; OnPropertyChanged("FinanceLeg_FXRate"); }
        }


        /// <summary>
        /// Fianace Fixedrate
        /// </summary>
        private double _financeLeg_SBPoint;
        public double FinanceLeg_SBPoint
        {
            get { return _financeLeg_SBPoint; }
            set { _financeLeg_SBPoint = value; OnPropertyChanged("FinanceLeg_SBPoint"); }
        }




        /// <summary>
        /// FinanceLeg DayCount
        /// </summary>
        private DayCount financeLeg_DayCount;
        public DayCount FinanceLeg_DayCount
        {
            get { return financeLeg_DayCount; }
            set { financeLeg_DayCount = value; OnPropertyChanged("FinanceLeg_DayCount"); }
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
    }
}
